using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    public class PlayerTrainerInitializer : MonoBehaviour
    {
        void Start()
        {
            // Create monsters for the player
            Monster monster1 = new Monster(AllSpecies.FireCat, 5);
            Monster monster2 = new Monster(AllSpecies.WaterOtter, 5);
            Monster monster3 = new Monster(AllSpecies.NatureBear, 5);

            List<Monster> playerMonsters = new List<Monster> { monster1, monster2, monster3 };
            List<Monster> pcMonsters = new List<Monster>(); // Empty PC monsters list

            PlayerTrainerData playerTrainerData = new PlayerTrainerData("PlayerName", playerMonsters, pcMonsters);
            TrainerComponent trainerComponent = GetComponent<TrainerComponent>();
            if (trainerComponent == null)
            {
                trainerComponent = gameObject.AddComponent<TrainerComponent>();
            }
            trainerComponent.Initialize(playerTrainerData);
        }
    }
}
