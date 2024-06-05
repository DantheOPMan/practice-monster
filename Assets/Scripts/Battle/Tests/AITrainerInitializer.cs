using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    public class AITrainerInitializer : MonoBehaviour
    {
        void Start()
        {
            // Create moves for the AI trainer's monsters
            List<Move> electricMouseMoves = new List<Move>
            {
                new Move("Thunder Shock", 40, 20, 1.0f, "Electric", MoveCategory.Special, 0, 0.1f),
                new Move("Quick Attack", 40, 10, 1.0f, "Normal", MoveCategory.Physical, -1, 0.1f),
                new Move("Tail Whip", 0, 10, 1.0f, "Normal", MoveCategory.Status, 0)
            };

            List<Move> rockGolemMoves = new List<Move>
            {
                new Move("Rock Throw", 50, 20, 0.9f, "Rock", MoveCategory.Physical, 0, 0.1f),
                new Move("Defense Curl", 0, 10, 1.0f, "Normal", MoveCategory.Status, 0),
                new Move("Tackle", 40, 10, 1.0f, "Normal", MoveCategory.Physical, 0, 0.1f)
            };

            // Create monsters for the AI trainer
            MonsterSpecies species1 = new MonsterSpecies(
                "ElectricMouse",
                new Dictionary<string, int> { { "hp", 35 }, { "attack", 55 }, { "defense", 40 }, { "special_attack", 50 }, { "special_defense", 50 }, { "speed", 90 } },
                electricMouseMoves,
                new List<string> { "Electric" },
                new List<string> { "Static" },
                new List<string>(),
                new Dictionary<string, int> { { "speed", 2 } }, // EV increases
                60, // Base XP
                ExperienceGroup.MediumFast
            );

            MonsterSpecies species2 = new MonsterSpecies(
                "RockGolem",
                new Dictionary<string, int> { { "hp", 40 }, { "attack", 80 }, { "defense", 100 }, { "special_attack", 30 }, { "special_defense", 30 }, { "speed", 20 } },
                rockGolemMoves,
                new List<string> { "Rock" },
                new List<string> { "Sturdy" },
                new List<string>(),
                new Dictionary<string, int> { { "defense", 1 } }, // EV increases
                75, // Base XP
                ExperienceGroup.Slow
            );

            Monster monster1 = new Monster(species1, 5);
            Monster monster2 = new Monster(species2, 5);

            List<Monster> aiMonsters = new List<Monster> { monster1, monster2 };

            AITrainerData aiTrainerData = new AITrainerData("AITrainer", aiMonsters);
            TrainerComponent trainerComponent = GetComponent<TrainerComponent>();
            if (trainerComponent == null)
            {
                trainerComponent = gameObject.AddComponent<TrainerComponent>();
            }
            trainerComponent.Initialize(aiTrainerData);
        }
    }
}
