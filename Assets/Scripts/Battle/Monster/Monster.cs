using System.Collections.Generic;
using UnityEngine;

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
            InitializeBattleState();
        }

        #region Initialization Methods
        private void InitializeStats()
        {
            CurrentHP = Data.MaxHP;
            CurrentAttack = Data.Attack;
            CurrentDefense = Data.Defense;
            CurrentSpecialAttack = Data.SpecialAttack;
            CurrentSpecialDefense = Data.SpecialDefense;
            CurrentSpeed = Data.Speed;
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
            return Mathf.Clamp(dodgeChance / 100, 0, 1);  // Ensure probability is between 0 and 1
        }

        public float GetAccuracyMultiplier()
        {
            if (AccuracyStage > 0)
                return 1 + (AccuracyStage * 0.5f);
            else
                return 1 - (Mathf.Abs(AccuracyStage) * (100 / 7) / 100f);
        }

        public float GetEvasivenessMultiplier()
        {
            if (EvasivenessStage > 0)
                return 1 + (EvasivenessStage * (100 / 7) / 100f);
            else
                return 1 - (Mathf.Abs(EvasivenessStage) * (100 / 7) / 100f);
        }
        #endregion
    }
}
