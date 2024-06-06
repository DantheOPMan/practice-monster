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

        public Dictionary<string, int> AttackerStageChanges { get; private set; }
        public Dictionary<string, int> DefenderStageChanges { get; private set; }


        public Move(string name, int power, int staminaCost, float accuracy, string moveType, MoveCategory category, int turnAdjustment, float critRate = 0.1f, Dictionary<string, int> attackerStageChanges = null, Dictionary<string, int> defenderStageChanges = null)
        {
            Name = name;
            Power = power;
            StaminaCost = staminaCost;
            Accuracy = accuracy;
            Type = moveType;
            Category = category;
            TurnAdjustment = turnAdjustment;
            CritRate = critRate;
            AttackerStageChanges = attackerStageChanges ?? new Dictionary<string, int>();
            DefenderStageChanges = defenderStageChanges ?? new Dictionary<string, int>();
        }
    }
}
