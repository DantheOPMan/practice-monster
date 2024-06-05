using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    [Serializable]
    public enum ExperienceGroup
    {
        Fast,
        MediumFast,
        MediumSlow,
        Slow
    }
    [Serializable]
    public class MonsterSpecies
    {
        public string Name { get; private set; }
        public Dictionary<string, int> BaseStats { get; private set; }
        public List<Move> Moves { get; private set; }
        public List<string> Types { get; private set; }
        public List<string> Abilities { get; private set; }
        public List<string> HeldItems { get; private set; }
        public Dictionary<string, int> EVs { get; private set; }
        public int BaseExperience { get; private set; }
        public ExperienceGroup ExperienceType { get; private set; }

        public MonsterSpecies(string name, Dictionary<string, int> baseStats, List<Move> moves, List<string> types, List<string> abilities, List<string> heldItems, Dictionary<string, int> evs, int baseExperience, ExperienceGroup experienceType)
        {
            Name = name;
            BaseStats = baseStats;
            Moves = moves;
            Types = types;
            Abilities = abilities;
            HeldItems = heldItems;
            EVs = evs;
            BaseExperience = baseExperience;
            ExperienceType = experienceType;
        }
        public int ExperienceForNextLevel(int currentLevel)
        {
            int level = currentLevel + 1;
            switch (ExperienceType)
            {
                case ExperienceGroup.Fast:
                    return Mathf.FloorToInt(4 * Mathf.Pow(level, 3) / 5);
                case ExperienceGroup.MediumFast:
                    return Mathf.FloorToInt(Mathf.Pow(level, 3));
                case ExperienceGroup.MediumSlow:
                    return Mathf.FloorToInt((6 / 5) * Mathf.Pow(level, 3) - 15 * Mathf.Pow(level, 2) + 100 * level - 140);
                case ExperienceGroup.Slow:
                    return Mathf.FloorToInt(5 * Mathf.Pow(level, 3) / 4);
                default:
                    return 0;
            }
        }

    }
}
