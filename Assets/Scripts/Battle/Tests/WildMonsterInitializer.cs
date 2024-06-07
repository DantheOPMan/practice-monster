using UnityEngine;

namespace PracticeMonster
{
    public class WildMonsterInitializer : MonoBehaviour
    {
        void Start()
        {
            // Create a monster for the wild encounter
            Monster wildMonster = new Monster(AllSpecies.WildBat, 5);

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
