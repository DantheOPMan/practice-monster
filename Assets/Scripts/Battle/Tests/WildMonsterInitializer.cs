using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    public class WildMonsterInitializer : MonoBehaviour
    {
        void Start()
        {
            // Create a monster for the wild encounter
            MonsterSpecies species = new MonsterSpecies("WildBat", new Dictionary<string, int> { { "hp", 40 }, { "attack", 45 }, { "defense", 35 }, { "special_attack", 30 }, { "special_defense", 40 }, { "speed", 55 } }, new List<Move>(), new List<string> { "Flying" }, new List<string> { "Inner Focus" }, new List<string>());

            Monster wildMonster = new Monster(species, 5);

            WildMonsterData wildMonsterData = new WildMonsterData(wildMonster);
            TrainerComponent trainerComponent = GetComponent<TrainerComponent>();
            if (trainerComponent == null)
            {
                trainerComponent = gameObject.AddComponent<TrainerComponent>();
            }
            trainerComponent.Initialize(wildMonsterData);
            Debug.Log(wildMonsterData);
        }
    }
}
