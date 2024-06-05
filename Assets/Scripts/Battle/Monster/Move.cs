using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    public enum MoveCategory
    {
        Physical,
        Special,
        Status
    }
    public class Move
    {
        public string Name { get; private set; }
        public int Power { get; private set; }
        public int StaminaCost { get; private set; }
        public float Accuracy { get; private set; }
        public string Type { get; private set; }
        public MoveCategory Category { get; private set; }
        public int TurnAdjustment { get; private set; }
        public float CritRate { get; private set; }

        public int AttackerAttackStageChange { get; private set; }
        public int AttackerDefenseStageChange { get; private set; }
        public int AttackerSpecialAttackStageChange { get; private set; }
        public int AttackerSpecialDefenseStageChange { get; private set; }
        public int AttackerSpeedStageChange { get; private set; }
        public int AttackerAccuracyStageChange { get; private set; }
        public int AttackerEvasivenessStageChange { get; private set; }

        public int DefenderAttackStageChange { get; private set; }
        public int DefenderDefenseStageChange { get; private set; }
        public int DefenderSpecialAttackStageChange { get; private set; }
        public int DefenderSpecialDefenseStageChange { get; private set; }
        public int DefenderSpeedStageChange { get; private set; }
        public int DefenderAccuracyStageChange { get; private set; }
        public int DefenderEvasivenessStageChange { get; private set; }

        public Move(string name, int power, int staminaCost, float accuracy, string moveType, MoveCategory category, int turnAdjustment, float critRate = 0.1f,
                    int attackerAttackStageChange = 0, int attackerDefenseStageChange = 0, int attackerSpecialAttackStageChange = 0, int attackerSpecialDefenseStageChange = 0,
                    int attackerSpeedStageChange = 0, int attackerAccuracyStageChange = 0, int attackerEvasivenessStageChange = 0,
                    int defenderAttackStageChange = 0, int defenderDefenseStageChange = 0, int defenderSpecialAttackStageChange = 0, int defenderSpecialDefenseStageChange = 0,
                    int defenderSpeedStageChange = 0, int defenderAccuracyStageChange = 0, int defenderEvasivenessStageChange = 0)
        {
            Name = name;
            Power = power;
            StaminaCost = staminaCost;
            Accuracy = accuracy;
            Type = moveType;
            Category = category;
            TurnAdjustment = turnAdjustment;
            CritRate = critRate;
            AttackerAttackStageChange = attackerAttackStageChange;
            AttackerDefenseStageChange = attackerDefenseStageChange;
            AttackerSpecialAttackStageChange = attackerSpecialAttackStageChange;
            AttackerSpecialDefenseStageChange = attackerSpecialDefenseStageChange;
            AttackerSpeedStageChange = attackerSpeedStageChange;
            AttackerAccuracyStageChange = attackerAccuracyStageChange;
            AttackerEvasivenessStageChange = attackerEvasivenessStageChange;
            DefenderAttackStageChange = defenderAttackStageChange;
            DefenderDefenseStageChange = defenderDefenseStageChange;
            DefenderSpecialAttackStageChange = defenderSpecialAttackStageChange;
            DefenderSpecialDefenseStageChange = defenderSpecialDefenseStageChange;
            DefenderSpeedStageChange = defenderSpeedStageChange;
            DefenderAccuracyStageChange = defenderAccuracyStageChange;
            DefenderEvasivenessStageChange = defenderEvasivenessStageChange;
        }
    }

}
