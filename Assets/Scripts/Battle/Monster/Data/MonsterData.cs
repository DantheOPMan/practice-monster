using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    [System.Serializable]
    public class MonsterData
    {
        public MonsterSpecies Species { get; private set; }
        public int Level { get; private set; }
        public int CurrentExperience { get; protected set; }
        public Dictionary<string, int> IVs { get; private set; }
        public Dictionary<string, int> EVs { get; private set; }
        public List<Move> Moves { get; private set; }
        public int MaxHP { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int SpecialAttack { get; private set; }
        public int SpecialDefense { get; private set; }
        public int Speed { get; private set; }

        public MonsterData(MonsterSpecies species, int level)
        {
            Species = species;
            Level = level;
            CurrentExperience = Species.ExperienceForNextLevel(level);
            IVs = GenerateRandomIVs();
            EVs = InitializeEVs();
            Moves = InitializeMoves();
            CalculateInitialStats();
        }

        private Dictionary<string, int> GenerateRandomIVs()
        {
            return new Dictionary<string, int>
            {
                {"hp", Random.Range(1, 21)},
                {"attack", Random.Range(1, 21)},
                {"defense", Random.Range(1, 21)},
                {"special_attack", Random.Range(1, 21)},
                {"special_defense", Random.Range(1, 21)},
                {"speed", Random.Range(1, 21)}
            };
        }

        private Dictionary<string, int> InitializeEVs()
        {
            return new Dictionary<string, int>
            {
                {"hp", 0},
                {"attack", 0},
                {"defense", 0},
                {"special_attack", 0},
                {"special_defense", 0},
                {"speed", 0}
            };
        }

        private List<Move> InitializeMoves()
        {
            List<Move> moves = new List<Move>();
            int start = Mathf.Max(0, Species.Moves.Count - 4);
            for (int i = start; i < Species.Moves.Count; i++)
            {
                moves.Add(Species.Moves[i]);
            }
            return moves;
        }

        private void CalculateInitialStats()
        {
            MaxHP = CalculateStat("hp");
            Attack = CalculateStat("attack");
            Defense = CalculateStat("defense");
            SpecialAttack = CalculateStat("special_attack");
            SpecialDefense = CalculateStat("special_defense");
            Speed = CalculateStat("speed");
        }

        private int CalculateStat(string statType)
        {
            int baseValue = Species.BaseStats[statType];
            int iv = IVs[statType];
            int ev = EVs[statType];

            if (statType == "hp")
            {
                return Mathf.FloorToInt(((2f * baseValue + iv + (ev / 4)) * Level / 100) + Level + 10);
            }
            else
            {
                return Mathf.FloorToInt(((2f * baseValue + iv + (ev / 4)) * Level / 100) + 5);
            }
        }
        public void GainExperience(int xp)
        {
            CurrentExperience += xp;
        }
        public void LevelUp()
        {
            Level++;
            CalculateInitialStats();
        }
    }
}
