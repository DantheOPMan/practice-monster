using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace PracticeMonster
{
    public class Battle : MonoBehaviour
    {
        private BattleTrainer trainer1;
        private BattleTrainer trainer2;
        private int turnCount;
        private BattleManager battleManager;
        private BattleState state;

        private BattleTrainer attacker;
        private BattleTrainer defender;
        private int attackerMoveIndex;
        private int defenderActionIndex;
        private int selectedActionIndex;

        private bool actionSelected;
        private bool defenseSelected;

        public void Initialize(BattleTrainer trainer1, BattleTrainer trainer2)
        {
            this.trainer1 = trainer1;
            this.trainer2 = trainer2;
            turnCount = 0;

            // Initialize action turns
            trainer1.ActionTurn = 0;
            trainer2.ActionTurn = 0;

            battleManager = FindObjectOfType<BattleManager>();
            state = BattleState.Start;

            BattleUIManager.Instance.InitializeUI(trainer1, trainer2);
            InitializeTrainerMonsters(trainer1);
            InitializeTrainerMonsters(trainer2);
            StartCoroutine(BattleLoop());
        }

        private void InitializeTrainerMonsters(BattleTrainer trainer)
        {
            foreach (var monster in trainer.PartyMonsters)
            {
                monster.InitializeBattleState();
            }
        }

        public void PrintStatus()
        {
            BattleUIManager.Instance.Log($"Battle Status:\n{trainer1?.Name}: {trainer1?.GetCurrentMonster().Nickname} Action Turn = {trainer1?.ActionTurn}\n" +
                      $"{trainer2?.Name}: {trainer2?.GetCurrentMonster().Nickname}  Action Turn = {trainer2?.ActionTurn}");
        }

        private IEnumerator BattleLoop()
        {
            trainer1.GetCurrentMonster().ActivateAbility(this, AbilityTrigger.OnSwitchIn, null, trainer2.GetCurrentMonster());
            trainer2.GetCurrentMonster().ActivateAbility(this, AbilityTrigger.OnSwitchIn, null, trainer1.GetCurrentMonster());

            while (state != BattleState.EndBattle)
            {
                switch (state)
                {
                    case BattleState.Start:
                        PrepareForTurn();
                        yield return StartCoroutine(StartTurn());
                        break;
                    case BattleState.SelectAction:
                        BattleUIManager.Instance.UpdateBattleUI(trainer1, trainer2);
                        StartCoroutine(SelectAction());
                        StartCoroutine(SelectDefense());
                        yield return new WaitUntil(() => actionSelected);
                        break;
                    case BattleState.SelectMove:
                        yield return StartCoroutine(SelectMoves());
                        break;
                    case BattleState.SelectSwitch:
                        yield return new WaitUntil(() => defenseSelected);
                        yield return StartCoroutine(SelectSwitch());
                        break;
                    case BattleState.SelectItem:
                        yield return new WaitUntil(() => defenseSelected);
                        yield return StartCoroutine(SelectItem());
                        break;
                    case BattleState.SelectCatch:
                        yield return StartCoroutine(SelectCatch());
                        break;
                    case BattleState.ExecuteMove:
                        yield return new WaitUntil(() => defenseSelected); 
                        yield return StartCoroutine(ExecuteMovePhase());
                        break;
                    case BattleState.CheckBattleEnd:
                        yield return StartCoroutine(CheckBattleEndPhase());
                        break;
                    case BattleState.EndTurn:
                        state = BattleState.Start;
                        yield return StartCoroutine(EndTurn());
                        break;
                }
            }
        }
        private void PrepareForTurn()
        {
            actionSelected = false;
            defenseSelected = false;
            attackerMoveIndex = -1;
            defenderActionIndex = -1;
            BattleUIManager.Instance.Log("Start turn");
        }
        private IEnumerator StartTurn()
        {
            turnCount++;

            while (trainer1.ActionTurn < 0 && trainer2.ActionTurn < 0)
            {
                IncrementActionTurns();
            }

            DetermineAttackerAndDefender(out attacker, out defender);
            state = BattleState.SelectAction;

            List<string> turnQueue = CalculateTurnQueue();
            BattleUIManager.Instance.UpdateTurnQueue(turnQueue);
            yield break;
        }

        private IEnumerator SelectAction()
        {
            if (attacker is BattlePlayerTrainer)
            {
                BattleUIManager.Instance.ShowActionSelectionUI((index) =>
                {
                    selectedActionIndex = index;
                    actionSelected = true;
                });

                yield return new WaitUntil(() => actionSelected);

                switch (selectedActionIndex)
                {
                    case 0:
                        state = BattleState.SelectMove;
                        break;
                    case 1:
                        state = BattleState.SelectSwitch;
                        break;
                    case 2:
                        state = BattleState.SelectItem;
                        break;
                    case 3:
                        state = BattleState.SelectCatch;
                        break;
                }
            }
            else
            {
                actionSelected = true;
                state = BattleState.SelectMove;
            }
        }

        private IEnumerator SelectDefense()
        {

            StartCoroutine(defender.Defend(this, (index) =>
            {
                defenderActionIndex = index;
                defenseSelected = true;
            }));
            yield return new WaitUntil(() => defenseSelected);
        }

        private IEnumerator SelectMoves()
        {
            Monster attackMonster = attacker.GetCurrentMonster();
            Monster defenseMonster = defender.GetCurrentMonster();

            bool attackerMoveSelected = false;
            attackerMoveIndex = -1;

            StartCoroutine(attacker.SelectMove(this, (index) =>
            {
                attackerMoveIndex = index;
                attackerMoveSelected = true;
            }));
            
            yield return new WaitUntil(() => attackerMoveSelected);
            state = BattleState.ExecuteMove;
        }

        private IEnumerator SelectSwitch()
        {
            bool switchSelected = false;
            StartCoroutine(attacker.SwitchMonster(this, (index) =>
            {
                attacker.ActionTurn += attacker.GetCurrentMonster().GetActionTurnReset();
                attacker.CurrentMonsterIndex = index;
                switchSelected = true;
            }));
             
            yield return new WaitUntil(() => switchSelected);

            attacker.GetCurrentMonster().ActivateAbility(this, AbilityTrigger.OnSwitchIn, null, defender.GetCurrentMonster());
            defender.GetCurrentMonster().ActivateAbility(this, AbilityTrigger.OnSwitchIn, null, attacker.GetCurrentMonster());

            state = BattleState.EndTurn;
        }

        private IEnumerator SelectItem()
        {
            Monster attackMonster = attacker.GetCurrentMonster();
            attacker.ActionTurn += attackMonster.GetActionTurnReset();
            attackMonster.CurrentHP = Mathf.Min(attackMonster.CurrentHP + 10, attackMonster.Data.MaxHP);
            BattleUIManager.Instance.Log($"{attackMonster.Nickname} healed 10 HP!");
            state = BattleState.EndTurn;
            yield break;
        }

        private IEnumerator SelectCatch()
        {
            if (defender is WildMonster)
            {
                Monster defenseMonster = defender.GetCurrentMonster();
                attacker.Data.InventoryMonsters.Add(defenseMonster);
                BattleUIManager.Instance.Log($"{defenseMonster.Nickname} was caught!");
                state = BattleState.EndBattle;
                yield return new WaitForSeconds(5);
                battleManager.EndBattle();
            }
            else
            {
                attacker.ActionTurn += attacker.GetCurrentMonster().GetActionTurnReset();
                BattleUIManager.Instance.Log("Cannot catch another trainer's monster!");
                state = BattleState.EndTurn;
            }
            yield break;
        }

        private IEnumerator ExecuteMovePhase()
        {
            BattleUIManager.Instance.LogReset();
            PrintStatus();

            Monster attackMonster = attacker.GetCurrentMonster();
            Monster defenseMonster = defender.GetCurrentMonster();

            BattleUIManager.Instance.Log($"Turn {turnCount}: {attackMonster.Nickname}'s turn to attack");

            ExecuteMove(attacker, defender);

            state = BattleState.CheckBattleEnd;
            yield break;
        }

        private IEnumerator CheckBattleEndPhase()
        {
            Monster attackMonster = attacker.GetCurrentMonster();
            Monster defenseMonster = defender.GetCurrentMonster();

            // Check if either monster is defeated
            if (defenseMonster.CurrentHP <= 0)
            {
                AwardExperience(attackMonster, defenseMonster, true); // Assuming both monsters in battle
                attackMonster.GainEVs(defenseMonster);

                if (!SwitchToNextMonster(defenseMonster == trainer1.GetCurrentMonster() ? trainer1 : trainer2))
                {
                    BattleUIManager.Instance.UpdateBattleUI(trainer1, trainer2);
                    BattleUIManager.Instance.Log($"{(defenseMonster == trainer1.GetCurrentMonster() ? trainer2.Name : trainer1.Name)} wins the battle!");
                    state = BattleState.EndBattle;
                    yield return new WaitForSeconds(5);
                    battleManager.EndBattle();
                    yield break;
                }
            }
            if (attackMonster.CurrentHP <= 0)
            {
                AwardExperience(defenseMonster, attackMonster, true); // Assuming both monsters in battle
                attackMonster.GainEVs(defenseMonster);

                if (!SwitchToNextMonster(attackMonster == trainer1.GetCurrentMonster() ? trainer1 : trainer2))
                {
                    BattleUIManager.Instance.UpdateBattleUI(trainer1, trainer2);

                    BattleUIManager.Instance.Log($"{(attackMonster == trainer1.GetCurrentMonster() ? trainer2.Name : trainer1.Name)} wins the battle!");
                    state = BattleState.EndBattle;
                    yield return new WaitForSeconds(5);
                    battleManager.EndBattle();
                    yield break;
                }
            }
            state = BattleState.EndTurn;
            yield break;
        }

        private IEnumerator EndTurn()
        {
            state = BattleState.Start;
            BattleUIManager.Instance.Log("End turn");
            BattleUIManager.Instance.UpdateBattleUI(trainer1, trainer2);
            yield break;
        }


        private void ExecuteMove(BattleTrainer attacker, BattleTrainer defender)
        {
            Monster attackMonster = attacker.GetCurrentMonster();
            Monster defenseMonster = defender.GetCurrentMonster();
            Move selectedMove = attackMonster.Data.Moves[attackerMoveIndex];

            string defensiveAction = GetDefensiveAction(defenseMonster, defenderActionIndex);

            selectedMove.Execute(attacker, defender, defensiveAction);
        }

        private string GetDefensiveAction(Monster defender, int index)
        {
            if (defender.CheckStatusEffectForAction())
            {
                BattleUIManager.Instance.Log($"{defender.Nickname} stands by.");
                return "Standby";
            }
            if (index == 0 && defender.Stamina >= 30)
            {
                BattleUIManager.Instance.Log($"{defender.Nickname} attempts to dodge!");
                defender.Stamina -= 30;
                return "Dodge";
            }
            else if (index == 1 && defender.Stamina >= 25)
            {
                BattleUIManager.Instance.Log($"{defender.Nickname} braces for impact!");
                defender.Stamina -= 25;
                return "Brace";
            }
            else if (index != 2)
            {
                if (index == 0)
                {
                    BattleUIManager.Instance.Log($"{defender.Nickname} does not have enough energy to dodge.");
                }
                else if (index == 1)
                {
                    BattleUIManager.Instance.Log($"{defender.Nickname} does not have enough energy to brace.");
                }
            }
            BattleUIManager.Instance.Log($"{defender.Nickname} stands by.");
            return "Standby";
        }

        private bool SwitchToNextMonster(BattleTrainer trainer)
        {
            Monster nextMonster = trainer.GetNextMonster();
            if (nextMonster == null)
            {
                BattleUIManager.Instance.Log($"{trainer.Name} has no more monsters left!");
                return false;
            }

            BattleUIManager.Instance.Log("Switching to next Monster: " + nextMonster.Nickname);
            return true;
        }

        private void IncrementActionTurns()
        {
            trainer1.ActionTurn++;
            trainer2.ActionTurn++;
            trainer1.GetCurrentMonster().Stamina += 2;
            trainer2.GetCurrentMonster().Stamina += 2;
            if (trainer1.GetCurrentMonster().Stamina > 100)
            {
                trainer1.GetCurrentMonster().Stamina = 100;
            }
            if (trainer2.GetCurrentMonster().Stamina > 100)
            {
                trainer2.GetCurrentMonster().Stamina = 100;
            }
        }

        private void DetermineAttackerAndDefender(out BattleTrainer attacker, out BattleTrainer defender)
        {
            if (trainer1.ActionTurn == 0 && trainer2.ActionTurn == 0)
            {
                if (trainer1.GetCurrentMonster().CurrentSpeed > trainer2.GetCurrentMonster().CurrentSpeed)
                {
                    attacker = trainer1;
                    defender = trainer2;
                }
                else if (trainer1.GetCurrentMonster().CurrentSpeed < trainer2.GetCurrentMonster().CurrentSpeed)
                {
                    attacker = trainer2;
                    defender = trainer1;
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        attacker = trainer1;
                        defender = trainer2;
                    }
                    else
                    {
                        attacker = trainer2;
                        defender = trainer1;
                    }
                }
            }
            else if (trainer1.ActionTurn == 0)
            {
                attacker = trainer1;
                defender = trainer2;
            }
            else
            {
                attacker = trainer2;
                defender = trainer1;
            }
        }

        private void AwardExperience(Monster winner, Monster loser, bool inBattle)
        {
            int baseExperience = loser.Data.Species.BaseExperience;
            int levelOfFainted = loser.Data.Level;
            int levelOfVictor = winner.Data.Level;

            float xpGained = Mathf.FloorToInt(
                (((baseExperience * levelOfFainted / 7) * (Mathf.Pow((2 * levelOfFainted + 10) / (float)(levelOfFainted + levelOfVictor + 10), 2.5f)) + 1) / (inBattle ? 1 : 2)));

            winner.GainExperience((int)xpGained);
            BattleUIManager.Instance.Log($"{winner.Nickname} gained {xpGained} XP!");
        }

        private List<string> CalculateTurnQueue()
        {
            List<string> turnQueue = new List<string>();

            // Copy the current action turns
            int trainer1ActionTurn = trainer1.ActionTurn;
            int trainer2ActionTurn = trainer2.ActionTurn;

            // Copy the current monsters
            Monster trainer1Monster = trainer1.GetCurrentMonster();
            Monster trainer2Monster = trainer2.GetCurrentMonster();
            if (attacker == trainer1)
            {
                turnQueue.Add(trainer1Monster.Nickname);
                trainer1ActionTurn += trainer1Monster.GetActionTurnReset();
            }
            else
            {
                turnQueue.Add(trainer2Monster.Nickname);
                trainer2ActionTurn += trainer2Monster.GetActionTurnReset();
            }
            // Simulate the next 5 turns
            for (int i = 0; i < 4; i++)
            {
                if (trainer1ActionTurn >= trainer2ActionTurn)
                {
                    turnQueue.Add(trainer1Monster.Nickname);
                    trainer1ActionTurn += trainer1Monster.GetActionTurnReset();
                }
                else
                {
                    turnQueue.Add(trainer2Monster.Nickname);
                    trainer2ActionTurn += trainer2Monster.GetActionTurnReset();
                }
            }

            return turnQueue;
        }

    }
}
