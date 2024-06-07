using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    public class AITrainerInitializer : MonoBehaviour
    {
        void Start()
        {
            // Create monsters for the AI trainer
            Monster monster1 = new Monster(AllSpecies.ElectricMouse, 5);
            Monster monster2 = new Monster(AllSpecies.RockGolem, 5);

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
