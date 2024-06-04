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
                new Move("Bite", 60, 10, 1.0f, "Dark", 0),
                new Move("Wing Attack", 60, 10, 1.0f, "Flying", 0),
                new Move("Screech", 0, 5, 1.0f, "Normal", 0)
            };

            // Create a monster for the wild encounter
            MonsterSpecies species = new MonsterSpecies("WildBat", new Dictionary<string, int> { { "hp", 40 }, { "attack", 45 }, { "defense", 35 }, { "special_attack", 30 }, { "special_defense", 40 }, { "speed", 55 } }, wildBatMoves, new List<string> { "Flying" }, new List<string> { "Inner Focus" }, new List<string>());

            Monster wildMonster = new Monster(species, 15);

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
