using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    public class WildMonsterInitializer : MonoBehaviour
    {
        void Start()
        {
            // Create moves for the wild monster
            List<Move> wildBatMoves = new List<Move>
            {
                new Move("Bite", 60, 20, 1.0f, "Dark", MoveCategory.Physical, 0, 0.1f),
                new Move("Wing Attack", 60, 20, 1.0f, "Flying", MoveCategory.Physical, 0, 0.1f),
                new Move("Screech", 0, 10, 1.0f, "Normal", MoveCategory.Status, 0)
            };

            // Create a monster for the wild encounter
            MonsterSpecies species = new MonsterSpecies(
                "WildBat",
                new Dictionary<string, int> { { "hp", 40 }, { "attack", 45 }, { "defense", 35 }, { "special_attack", 30 }, { "special_defense", 40 }, { "speed", 55 } },
                wildBatMoves,
                new List<string> { "Flying" },
                new List<string> { "Inner Focus" },
                new List<string>(),
                new Dictionary<string, int> { { "speed", 1 } }, // EV increases
                64, // Base XP
                ExperienceGroup.MediumFast
            );

            Monster wildMonster = new Monster(species, 5);

            WildMonsterData wildMonsterData = new WildMonsterData(wildMonster);
            TrainerComponent trainerComponent = GetComponent<TrainerComponent>();
            if (trainerComponent == null)
            {
                trainerComponent = gameObject.AddComponent<TrainerComponent>();
            }
            trainerComponent.Initialize(wildMonsterData);
        }
    }
}
