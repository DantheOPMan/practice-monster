using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

namespace PracticeMonster
{
    public class Monster
    {
        public MonsterData Data { get; private set; }
        public string Ability { get; private set; }
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
        public void ApplyStageChanges(Move move, bool isAttacker)
        {
            if (isAttacker)
            {
                LogStageChange("Attack", move.AttackerAttackStageChange);
                AttackStage = Mathf.Clamp(AttackStage + move.AttackerAttackStageChange, -6, 6);

                LogStageChange("Defense", move.AttackerDefenseStageChange);
                DefenseStage = Mathf.Clamp(DefenseStage + move.AttackerDefenseStageChange, -6, 6);

                LogStageChange("Special Attack", move.AttackerSpecialAttackStageChange);
                SpecialAttackStage = Mathf.Clamp(SpecialAttackStage + move.AttackerSpecialAttackStageChange, -6, 6);

                LogStageChange("Special Defense", move.AttackerSpecialDefenseStageChange);
                SpecialDefenseStage = Mathf.Clamp(SpecialDefenseStage + move.AttackerSpecialDefenseStageChange, -6, 6);

                LogStageChange("Speed", move.AttackerSpeedStageChange);
                SpeedStage = Mathf.Clamp(SpeedStage + move.AttackerSpeedStageChange, -6, 6);

                LogStageChange("Accuracy", move.AttackerAccuracyStageChange);
                AccuracyStage = Mathf.Clamp(AccuracyStage + move.AttackerAccuracyStageChange, -6, 6);

                LogStageChange("Evasiveness", move.AttackerEvasivenessStageChange);
                EvasivenessStage = Mathf.Clamp(EvasivenessStage + move.AttackerEvasivenessStageChange, -6, 6);
            }
            else
            {
                LogStageChange("Attack", move.DefenderAttackStageChange);
                AttackStage = Mathf.Clamp(AttackStage + move.DefenderAttackStageChange, -6, 6);

                LogStageChange("Defense", move.DefenderDefenseStageChange);
                DefenseStage = Mathf.Clamp(DefenseStage + move.DefenderDefenseStageChange, -6, 6);

                LogStageChange("Special Attack", move.DefenderSpecialAttackStageChange);
                SpecialAttackStage = Mathf.Clamp(SpecialAttackStage + move.DefenderSpecialAttackStageChange, -6, 6);

                LogStageChange("Special Defense", move.DefenderSpecialDefenseStageChange);
                SpecialDefenseStage = Mathf.Clamp(SpecialDefenseStage + move.DefenderSpecialDefenseStageChange, -6, 6);

                LogStageChange("Speed", move.DefenderSpeedStageChange);
                SpeedStage = Mathf.Clamp(SpeedStage + move.DefenderSpeedStageChange, -6, 6);

                LogStageChange("Accuracy", move.DefenderAccuracyStageChange);
                AccuracyStage = Mathf.Clamp(AccuracyStage + move.DefenderAccuracyStageChange, -6, 6);

                LogStageChange("Evasiveness", move.DefenderEvasivenessStageChange);
                EvasivenessStage = Mathf.Clamp(EvasivenessStage + move.DefenderEvasivenessStageChange, -6, 6);
            }

            UpdateCurrentStats();
        }

        private void LogStageChange(string statName, int change)
        {
            if (change == 1)
            {
                BattleUIManager.Instance.Log($"{Nickname}'s {statName} increased!");
            }
            else if (change == 2)
            {
                BattleUIManager.Instance.Log($"{Nickname}'s {statName} significantly increased!");
            }
            else if (change >= 3)
            {
                BattleUIManager.Instance.Log($"{Nickname}'s {statName} dramatically increased!");
            }
            else if (change == -1)
            {
                BattleUIManager.Instance.Log($"{Nickname}'s {statName} decreased!");
            }
            else if (change == -2)
            {
                BattleUIManager.Instance.Log($"{Nickname}'s {statName} significantly decreased!");
            }
            else if (change <= -3)
            {
                BattleUIManager.Instance.Log($"{Nickname}'s {statName} dramatically decreased!");
            }
        }

        private void UpdateCurrentStats()
        {
            CurrentAttack = UpdateCurrentStat(AttackStage, Data.Attack);
            CurrentDefense = UpdateCurrentStat(DefenseStage, Data.Defense);
            CurrentSpecialAttack = UpdateCurrentStat(SpecialAttackStage, Data.SpecialAttack);
            CurrentSpecialDefense = UpdateCurrentStat(SpecialDefenseStage, Data.SpecialDefense);
            CurrentSpeed = UpdateCurrentStat(SpeedStage, Data.Speed);
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
        public void GainEVs(Monster loser)
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
                BattleUIManager.Instance.Log($"{Nickname} gained {ev.Value} {ev.Key} EV(s)!");
            }
        }
        public void GainExperience(int xp)
        {
            Data.GainExperience(xp);
            while (Data.CurrentExperience >= Data.Species.ExperienceForNextLevel(Data.Level))
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            Data.LevelUp();
            BattleUIManager.Instance.Log($"{Nickname} leveled up to level {Data.Level}!");

            // Learn new moves if any are available at the new level
            LearnNewMoves();
        }
        private void LearnNewMoves()
        {

        }
        #endregion
    }
}
