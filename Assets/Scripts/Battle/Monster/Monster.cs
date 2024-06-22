using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

namespace PracticeMonster
{
    public class Monster
    {
        public MonsterData Data { get; private set; }
        public Ability Ability { get; private set; }
        public string HeldItem { get; private set; }
        public string Nickname { get; private set; }

        public int CurrentHP { get; set; }
        public int CurrentAttack { get; set; }
        public int CurrentDefense { get; set; }
        public int CurrentSpecialAttack { get; set; }
        public int CurrentSpecialDefense { get; set; }
        public int CurrentSpeed { get; set; }

        public int Stamina { get; set; }
        public int AccuracyStage { get; set; }
        public int EvasivenessStage { get; set; }
        public int AttackStage { get; set; }
        public int DefenseStage { get; set; }
        public int SpecialAttackStage { get; set; }
        public int SpecialDefenseStage { get; set; }
        public int SpeedStage { get; set; }
        public StatusEffect ActiveStatusEffect { get; private set; }

        public Monster(MonsterSpecies species, int level)
        {
            Data = new MonsterData(species, level);

            Nickname = species.Name;

            Ability = species.Abilities[Random.Range(0, species.Abilities.Count)];

            if (Random.value < 0.25f && species.HeldItems.Count > 0)  // 25% chance to hold an item
            {
                HeldItem = species.HeldItems[Random.Range(0, species.HeldItems.Count)];
            }
            else
            {
                HeldItem = null;
            }

            ActiveStatusEffect = null;

            InitializeStats();
        }

        #region Initialization Methods
        public void InitializeStats()
        {
            CurrentHP = Data.MaxHP;
            CurrentAttack = Data.Attack;
            CurrentDefense = Data.Defense;
            CurrentSpecialAttack = Data.SpecialAttack;
            CurrentSpecialDefense = Data.SpecialDefense;
            CurrentSpeed = Data.Speed;
            InitializeBattleState();
        }

        public void InitializeBattleState()
        {
            Stamina = 100;
            AccuracyStage = 0;
            EvasivenessStage = 0;
            AttackStage = 0;
            DefenseStage = 0;
            SpecialAttackStage = 0;
            SpecialDefenseStage = 0;
            SpeedStage = 0;
            UpdateCurrentStats();
        }
        #endregion

        #region Reusable Methods
        public float ActivateAbility(Battle battle, AbilityTrigger trigger, Move move, Monster target = null, BattleUIManager battleUIManager=null)
        {
            if (Ability != null && Ability.Triggers.Contains(trigger))
            {
                return Ability.Activate(battle, this, move, target, battleUIManager);
            }
            return 1.0f;
        }

        public void ChangeMove(int moveIndex, string newMoveName)
        {
            Move newMove = Data.Species.Moves.Find(move => move.Name == newMoveName);
            if (newMove != null && moveIndex >= 0 && moveIndex < Data.Moves.Count)
            {
                if (!Data.Moves.Exists(move => move.Name == newMoveName))
                {
                    Data.Moves[moveIndex] = newMove;
                    Debug.Log($"{Nickname} has learned {newMoveName}!");
                }
                else
                {
                    Debug.Log($"{Nickname} already knows {newMoveName}.");
                }
            }
            else
            {
                Debug.Log($"{newMoveName} is not a valid move for {Nickname} or invalid move index.");
            }
        }
        #endregion

        #region Battle Methods
        public int GetActionTurnReset()
        {
            if (CurrentSpeed <= 15)
            {
                return -12;
            }
            else if (CurrentSpeed <= 31)
            {
                return -11;
            }
            else if (CurrentSpeed <= 55)
            {
                return -10;
            }
            else if (CurrentSpeed <= 88)
            {
                return -9;
            }
            else if (CurrentSpeed <= 129)
            {
                return -8;
            }
            else if (CurrentSpeed <= 181)
            {
                return -7;
            }
            else if (CurrentSpeed <= 242)
            {
                return -6;
            }
            else if (CurrentSpeed <= 316)
            {
                return -5;
            }
            else if (CurrentSpeed <= 401)
            {
                return -4;
            }
            else
            {
                return -3;
            }
        }

        public float GetDodgeProbability(int attackerSpeed)
        {
            float dodgeChance = (CurrentSpeed - attackerSpeed) / 2 + 50;
            return (dodgeChance / 100);
        }

        public float GetAccuracyMultiplier()
        {
            if (AccuracyStage > 0)
                return ((3f + AccuracyStage) / 3f);
            else
                return (3f / (Mathf.Abs(AccuracyStage) + 3f));
        }

        public float GetEvasivenessMultiplier()
        {
            if (EvasivenessStage > 0)
                return ((3f + EvasivenessStage) / 3f);
            else
                return (3f / (Mathf.Abs(EvasivenessStage) + 3f));
        }

        public void ApplyStageChanges(Move move, bool isAttacker, BattleUIManager battleUIManager)
        {
            Dictionary<string, int> stageChanges = isAttacker ? move.AttackerStageChanges : move.DefenderStageChanges;

            foreach (var stageChange in stageChanges)
            {
                switch (stageChange.Key)
                {
                    case "Attack":
                        CurrentAttack += stageChange.Value;
                        LogStageChange("Attack", stageChange.Value, battleUIManager);
                        break;
                    case "Defense":
                        CurrentDefense += stageChange.Value;
                        LogStageChange("Defense", stageChange.Value, battleUIManager);
                        break;
                    case "SpecialAttack":
                        CurrentSpecialAttack += stageChange.Value;
                        LogStageChange("Special Attack", stageChange.Value, battleUIManager);
                        break;
                    case "SpecialDefense":
                        CurrentSpecialDefense += stageChange.Value;
                        LogStageChange("Special Defense", stageChange.Value, battleUIManager);
                        break;
                    case "Speed":
                        CurrentSpeed += stageChange.Value;
                        LogStageChange("Speed", stageChange.Value, battleUIManager);
                        break;
                    case "Accuracy":
                        AccuracyStage += stageChange.Value;
                        LogStageChange("Accuracy", stageChange.Value, battleUIManager);
                        break;
                    case "Evasiveness":
                        EvasivenessStage += stageChange.Value;
                        LogStageChange("Evasiveness", stageChange.Value, battleUIManager);
                        break;
                }
            }
        }

        private void LogStageChange(string statName, int change, BattleUIManager battleUIManager)
        {
            if (change == 1)
            {
                battleUIManager.Log($"{Nickname}'s {statName} increased!");
            }
            else if (change == 2)
            {
                battleUIManager.Log($"{Nickname}'s {statName} significantly increased!");
            }
            else if (change >= 3)
            {
                battleUIManager.Log($"{Nickname}'s {statName} dramatically increased!");
            }
            else if (change == -1)
            {
                battleUIManager.Log($"{Nickname}'s {statName} decreased!");
            }
            else if (change == -2)
            {
                battleUIManager.Log($"{Nickname}'s {statName} significantly decreased!");
            }
            else if (change <= -3)
            {
                battleUIManager.Log($"{Nickname}'s {statName} dramatically decreased!");
            }
        }

        public void UpdateCurrentStats()
        {
            CurrentAttack = UpdateCurrentStat(AttackStage, Data.Attack);
            CurrentDefense = UpdateCurrentStat(DefenseStage, Data.Defense);
            CurrentSpecialAttack = UpdateCurrentStat(SpecialAttackStage, Data.SpecialAttack);
            CurrentSpecialDefense = UpdateCurrentStat(SpecialDefenseStage, Data.SpecialDefense);
            CurrentSpeed = UpdateCurrentStat(SpeedStage, Data.Speed);
            ApplyStatusEffectStatChange();
        }

        private int UpdateCurrentStat(int stage, int baseStat)
        {
            float stageFloat = (float)Mathf.Abs(stage);
            float baseStatFloat = (float)baseStat;

            if (stage > 0)
                return Mathf.FloorToInt(((2f + stageFloat) / 2f) * baseStatFloat);
            else if (stage < 0)
                return Mathf.FloorToInt((2f / (2f + stageFloat)) * baseStatFloat);
            else
                return baseStat;
        }

        public void GainEVs(Monster loser, BattleUIManager battleUIManager)
        {
            int totalEVs = 0;
            foreach (var ev in Data.EVs)
            {
                totalEVs += ev.Value;
            }

            foreach (var ev in loser.Data.Species.EVs)
            {
                if (totalEVs >= 1000)
                {
                    break;
                }

                int availableEVSpace = 1000 - totalEVs;
                int evGain = Mathf.Min(ev.Value, availableEVSpace);
                Data.EVs[ev.Key] += evGain;

                if (Data.EVs[ev.Key] > 450)
                {
                    int excessEVs = Data.EVs[ev.Key] - 450;
                    Data.EVs[ev.Key] = 450;
                    evGain -= excessEVs;
                }

                totalEVs += evGain;
                battleUIManager.Log($"{Nickname} gained {ev.Value} {ev.Key} EV(s)!");
            }
        }

        public void GainExperience(int xp, BattleUIManager battleUIManager)
        {
            Data.GainExperience(xp);
            while (Data.CurrentExperience >= Data.Species.ExperienceForNextLevel(Data.Level))
            {
                LevelUp(battleUIManager);
            }
        }

        private void LevelUp(BattleUIManager battleUIManager)
        {
            Data.LevelUp();
            battleUIManager.Log($"{Nickname} leveled up to level {Data.Level}!");

            // Learn new moves if any are available at the new level
            LearnNewMoves(battleUIManager);
        }

        private void LearnNewMoves(BattleUIManager battleUIManager)
        {
            // Add logic to learn new moves if any are available at the new level
        }

        public void ApplyStatusEffect(StatusEffect effect, BattleUIManager battleUIManager)
        {
            // Check if the monster already has the status effect
            if (ActiveStatusEffect != null)
            {
                return;
            }

            ActiveStatusEffect = effect;
            battleUIManager.Log($"{Nickname} is now affected by {effect.Type}!");
            ApplyStatusEffectStatChange();
        }

        public void ApplyStatusEffectStatChange()
        {
            switch (ActiveStatusEffect?.Type)
            {
                case StatusEffectType.Burn:
                    CurrentAttack = Mathf.FloorToInt(CurrentAttack * 0.5f);
                    break;
                case StatusEffectType.Paralyze:
                    CurrentSpeed = Mathf.FloorToInt(CurrentSpeed * 0.5f);
                    break;
                case StatusEffectType.Frostbite:
                    CurrentSpecialAttack = Mathf.FloorToInt(CurrentSpecialAttack * 0.5f);
                    break;
            }
        }

        public void ApplyStatusEffectDamage(BattleUIManager battleUIManager)
        {
            switch (ActiveStatusEffect?.Type)
            {
                case StatusEffectType.Burn:
                case StatusEffectType.Frostbite:
                case StatusEffectType.Poison:
                case StatusEffectType.BadlyPoisoned:
                    int damage = Mathf.FloorToInt(Data.MaxHP / 16f);
                    if (ActiveStatusEffect.Type == StatusEffectType.BadlyPoisoned)
                    {
                        damage = Mathf.FloorToInt(damage * ActiveStatusEffect.Duration);
                        ActiveStatusEffect.IncreaseDuration();
                    }
                    else if (ActiveStatusEffect.Type == StatusEffectType.Poison)
                    {
                        damage *= 2;
                    }
                    CurrentHP = Mathf.Max(CurrentHP - damage, 0);
                    battleUIManager.Log($"{Nickname} took {damage} damage from {ActiveStatusEffect.Type}!");
                    break;
            }
        }

        public bool CheckStatusEffectForAction(BattleUIManager battleUIManager)
        {
            switch (ActiveStatusEffect?.Type)
            {
                case StatusEffectType.Paralyze:
                    if (Random.value < 0.25f)
                    {
                        battleUIManager.Log($"{Nickname} is paralyzed and can't move!");
                        return true;
                    }
                    break;
                case StatusEffectType.Drowsy:
                    if (Random.value < 0.2f)
                    {
                        battleUIManager.Log($"{Nickname} awakened!");
                        ActiveStatusEffect = null;
                    }
                    else if (Random.value > 0.5f)
                    {
                        battleUIManager.Log($"{Nickname} is sleepy and won't move!");
                        return true;
                    }
                    break;
            }
            return false;
        }

        #endregion
    }
}
