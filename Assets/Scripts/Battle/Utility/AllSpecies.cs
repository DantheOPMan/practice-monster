using System.Collections.Generic;

namespace PracticeMonster
{
    public static class AllSpecies
    {
        public static readonly MonsterSpecies ElectricMouse = new MonsterSpecies(
            "ElectricMouse",
            new Dictionary<string, int> { { "hp", 35 }, { "attack", 55 }, { "defense", 40 }, { "special_attack", 50 }, { "special_defense", 50 }, { "speed", 90 } },
            new List<Move> { AllMoves.ThunderShock, AllMoves.QuickAttack, AllMoves.TailWhip },
            new List<string> { "Electric" },
            new List<Ability> { new Blaze() },
            new List<string>(),
            new Dictionary<string, int> { { "speed", 2 } }, // EV increases
            60, // Base XP
            ExperienceGroup.MediumFast
        );

        public static readonly MonsterSpecies RockGolem = new MonsterSpecies(
            "RockGolem",
            new Dictionary<string, int> { { "hp", 40 }, { "attack", 80 }, { "defense", 100 }, { "special_attack", 30 }, { "special_defense", 30 }, { "speed", 20 } },
            new List<Move> { AllMoves.RockThrow, AllMoves.DefenseCurl, AllMoves.Tackle },
            new List<string> { "Rock" },
            new List<Ability> { new Intimidate() },
            new List<string>(),
            new Dictionary<string, int> { { "defense", 1 } }, // EV increases
            75, // Base XP
            ExperienceGroup.Slow
        );

        public static readonly MonsterSpecies FireCat = new MonsterSpecies(
            "FireCat",
            new Dictionary<string, int> { { "hp", 50 }, { "attack", 52 }, { "defense", 43 }, { "special_attack", 60 }, { "special_defense", 50 }, { "speed", 120 } },
            new List<Move> { AllMoves.Ember, AllMoves.Scratch, AllMoves.Growl },
            new List<string> { "Fire" },
            new List<Ability> { new Blaze() },
            new List<string>(),
            new Dictionary<string, int> { { "speed", 1 } }, // EV increases
            62, // Base XP
            ExperienceGroup.MediumSlow
        );

        public static readonly MonsterSpecies WaterOtter = new MonsterSpecies(
            "WaterOtter",
            new Dictionary<string, int> { { "hp", 55 }, { "attack", 65 }, { "defense", 60 }, { "special_attack", 50 }, { "special_defense", 50 }, { "speed", 45 } },
            new List<Move> { AllMoves.WaterGun, AllMoves.Tackle, AllMoves.TailWhip },
            new List<string> { "Water" },
            new List<Ability> { new Blaze() },
            new List<string>(),
            new Dictionary<string, int> { { "attack", 1 } }, // EV increases
            63, // Base XP
            ExperienceGroup.MediumFast
        );

        public static readonly MonsterSpecies NatureBear = new MonsterSpecies(
            "NatureBear",
            new Dictionary<string, int> { { "hp", 60 }, { "attack", 80 }, { "defense", 50 }, { "special_attack", 50 }, { "special_defense", 50 }, { "speed", 40 } },
            new List<Move> { AllMoves.VineWhip, AllMoves.Scratch, AllMoves.Growl },
            new List<string> { "Nature" },
            new List<Ability> { new Overgrow() },
            new List<string>(),
            new Dictionary<string, int> { { "attack", 1 } }, // EV increases
            68, // Base XP
            ExperienceGroup.Slow
        );

        public static readonly MonsterSpecies WildBat = new MonsterSpecies(
            "WildBat",
            new Dictionary<string, int> { { "hp", 40 }, { "attack", 45 }, { "defense", 35 }, { "special_attack", 30 }, { "special_defense", 40 }, { "speed", 55 } },
            new List<Move> { AllMoves.Bite, AllMoves.WingAttack, AllMoves.Screech },
            new List<string> { "Flying" },
            new List<Ability> { new Blaze() },
            new List<string>(),
            new Dictionary<string, int> { { "speed", 1 } }, // EV increases
            64, // Base XP
            ExperienceGroup.MediumFast
        );
    }
}
