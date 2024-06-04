using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    [Serializable]
    public class MonsterSpecies
    {
        public string Name { get; private set; }
        public Dictionary<string, int> BaseStats { get; private set; }
        public List<Move> Moves { get; private set; }
        public List<string> Types { get; private set; }
        public List<string> Abilities { get; private set; }
        public List<string> HeldItems { get; private set; }

        public MonsterSpecies(string name, Dictionary<string, int> baseStats, List<Move> moves, List<string> types, List<string> abilities, List<string> heldItems)
        {
            Name = name;
            BaseStats = baseStats;
            Moves = moves;
            Types = types;
            Abilities = abilities;
            HeldItems = heldItems;
        }
    }
}
