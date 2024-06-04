using System.Collections;
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

            StartCoroutine(BattleLoop());
        }

        public void PrintStatus()
        {
            BattleUIManager.Instance.Log($"Battle Status:\n{trainer1?.Name}: {trainer1?.GetCurrentMonster().Nickname} HP = {trainer1?.GetCurrentMonster().CurrentHP}, Stamina = {trainer1?.GetCurrentMonster().Stamina}, Action Turn = {trainer1?.ActionTurn}\n" +
                      $"{trainer2?.Name}: {trainer2?.GetCurrentMonster().Nickname} HP = {trainer2?.GetCurrentMonster().CurrentHP}, Stamina = {trainer2?.GetCurrentMonster().Stamina}, Action Turn = {trainer2?.ActionTurn}");
        }

        private IEnumerator BattleLoop()
        {
            while (state != BattleState.EndBattle)
            {
                switch (state)
                {
                    case BattleState.Start:
                        BattleUIManager.Instance.Log("Start turn");
                        yield return StartCoroutine(StartTurn());
                        break;
                    case BattleState.SelectMove:
                        yield return StartCoroutine(SelectMoves());
                        BattleUIManager.Instance.LogReset();
                        break;
                    case BattleState.ExecuteMove:
                        yield return StartCoroutine(ExecuteMovePhase());
                        break;
                    case BattleState.CheckBattleEnd:
                        yield return StartCoroutine(CheckBattleEndPhase());
                        break;
                    case BattleState.EndTurn:
                        state = BattleState.Start;
                        BattleUIManager.Instance.Log("End turn");
                        BattleUIManager.Instance.UpdateBattleUI(trainer1, trainer2);
                        yield return new WaitForSeconds(7f);
                        BattleUIManager.Instance.LogReset();
                        break;
                }
            }
        }

        private IEnumerator StartTurn()
        {
            turnCount++;
            PrintStatus();

            while (trainer1.ActionTurn < 0 && trainer2.ActionTurn < 0)
            {
                IncrementActionTurns();
            }

            state = BattleState.SelectMove;
            yield break;
        }

        private IEnumerator SelectMoves()
        {
            DetermineAttackerAndDefender(out attacker, out defender);
            Monster attackMonster = attacker.GetCurrentMonster();
            Monster defenseMonster = defender.GetCurrentMonster();

            bool attackerMoveSelected = false;
            bool defenderActionSelected = false;
            attackerMoveIndex = -1;
            defenderActionIndex = -1;

            StartCoroutine(attacker.SelectMove(this, (index) =>
            {
                attackerMoveIndex = index;
                attackerMoveSelected = true;
            }));
            StartCoroutine(defender.Defend(this, (index) =>
            {
                defenderActionIndex = index;
                defenderActionSelected = true;
            }));

            yield return new WaitUntil(() => attackerMoveSelected && defenderActionSelected);

            state = BattleState.ExecuteMove;
        }

        private IEnumerator ExecuteMovePhase()
        {
            PrintStatus();

            Monster attackMonster = attacker.GetCurrentMonster();
            Monster defenseMonster = defender.GetCurrentMonster();

            BattleUIManager.Instance.Log($"Turn {turnCount}: {attackMonster.Nickname}'s turn to attack");

            ExecuteMove(attacker, defender, attackerMoveIndex, defenderActionIndex);

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
                if (!SwitchToNextMonster(defenseMonster == trainer1.GetCurrentMonster() ? trainer1 : trainer2))
                {
                    BattleUIManager.Instance.Log($"{(defenseMonster == trainer1.GetCurrentMonster() ? trainer2.Name : trainer1.Name)} wins the battle!");
                    battleManager.EndBattle();
                    state = BattleState.EndBattle;
                    yield break;
                }
            }
            if (attackMonster.CurrentHP <= 0)
            {
                if (!SwitchToNextMonster(attackMonster == trainer1.GetCurrentMonster() ? trainer1 : trainer2))
                {
                    BattleUIManager.Instance.Log($"{(attackMonster == trainer1.GetCurrentMonster() ? trainer2.Name : trainer1.Name)} wins the battle!");
                    battleManager.EndBattle();
                    state = BattleState.EndBattle;
                    yield break;
                }
            }
            state = BattleState.EndTurn;
            yield break;
        }

        private void ExecuteMove(BattleTrainer attacker, BattleTrainer defender, int attackerMoveIndex, int defenderActionIndex)
        {
            Monster attackMonster = attacker.GetCurrentMonster();
            Monster defenseMonster = defender.GetCurrentMonster();
            Move selectedMove = attackMonster.Data.Moves[attackerMoveIndex];

            BattleUIManager.Instance.Log($"{attackMonster.Nickname} uses {selectedMove.Name}!");


            if (attackMonster.Stamina < selectedMove.StaminaCost)
            {
                BattleUIManager.Instance.Log($"{attackMonster.Nickname} does not have enough stamina to use {selectedMove.Name}!");
                return;
            }

            // Defender's defensive action
            string defensiveAction = GetDefensiveAction(defenseMonster, defenderActionIndex);

            attackMonster.Stamina -= selectedMove.StaminaCost;
            attacker.ActionTurn += selectedMove.TurnAdjustment + attackMonster.GetActionTurnReset();

            // Calculate hit probability
            if (!CalculateHit(attackMonster, defenseMonster, selectedMove, defensiveAction))
            {
                BattleUIManager.Instance.Log($"{attackMonster.Nickname} missed with {selectedMove.Name}!");
                return;
            }

            // Calculate damage and apply
            int damage = CalculateDamage(attackMonster, defenseMonster, selectedMove, defensiveAction);
            defenseMonster.CurrentHP -= damage;
            BattleUIManager.Instance.Log($"{attackMonster.Nickname} used {selectedMove.Name} and dealt {damage} damage to {defenseMonster.Nickname}!");

            // Adjust action turn based on move's speed adjustment
        }

        private bool CalculateHit(Monster attacker, Monster defender, Move move, string defensiveAction)
        {
            if (defensiveAction == "Brace")
            {
                return true;
            }
            float dodgeProbability = defensiveAction == "Dodge" ? defender.GetDodgeProbability(attacker.CurrentSpeed) : 0;
            float hitProbability = move.Accuracy * (1 - dodgeProbability) * attacker.GetAccuracyMultiplier() * defender.GetEvasivenessMultiplier();
            return Random.value <= hitProbability;
        }

        private int CalculateDamage(Monster attacker, Monster defender, Move move, string defensiveAction)
        {
            float defensiveMultiplier = 1f;
            if (defensiveAction == "Brace")
            {
                defensiveMultiplier = 1.5f;
            }
            int attackStat = move.MoveType == "physical" ? attacker.CurrentAttack : attacker.CurrentSpecialAttack;
            int defenseStat = move.MoveType == "physical" ? defender.CurrentDefense : defender.CurrentSpecialDefense;
            float modifier = 1;

            // STAB (Same Type Attack Bonus)
            if (attacker.Data.Species.Types.Contains(move.MoveType))
            {
                modifier *= 1.5f;
            }

            // Critical hit
            if (Random.value < move.CritRate)
            {
                modifier *= 1.5f;
                BattleUIManager.Instance.Log("Critical hit!");
            }

            // Type effectiveness - for simplicity, set to 1 (you can implement type effectiveness logic here)
            float typeEffectiveness = 1;
            modifier *= typeEffectiveness;

            float damage = ((((attacker.Data.Level * 2f + 10f) / 250f)) * (move.Power * modifier) * ((float)attackStat / (defensiveMultiplier * (float)defenseStat))) + 1;
            return Mathf.FloorToInt(damage);
        }

        private string GetDefensiveAction(Monster defender, int index)
        {
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
            if(trainer1.GetCurrentMonster().Stamina > 100)
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
    }
}
