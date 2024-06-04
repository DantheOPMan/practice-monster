using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    public class Move
    {
        public string Name { get; private set; }
        public int Power { get; private set; }
        public int StaminaCost { get; private set; }
        public float Accuracy { get; private set; }
        public string MoveType { get; private set; }
        public int TurnAdjustment { get; private set; }
        public float CritRate { get; private set; }

        public Move(string name, int power, int staminaCost, float accuracy, string moveType, int turnAdjustment, float critRate = 0.1f)
        {
            Name = name;
            Power = power;
            StaminaCost = staminaCost;
            Accuracy = accuracy;
            MoveType = moveType;
            TurnAdjustment = turnAdjustment;
            CritRate = critRate;
        }
    }
}
