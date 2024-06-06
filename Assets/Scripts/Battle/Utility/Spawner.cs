using System.Collections;
using UnityEngine;

namespace PracticeMonster
{
    public class Spawner : MonoBehaviour
    {
        public GameObject aiTrainerPrefab;
        public GameObject wildMonsterPrefab;
        public float spawnRadius = 15f;
        public float checkInterval = 5f;

        private GameObject currentAITrainer;
        private GameObject currentWildMonster;

        void Start()
        {
            SpawnAITrainer();
            SpawnWildMonster();
            StartCoroutine(CheckAndRespawn());
        }

        void SpawnAITrainer()
        {
            if (currentAITrainer != null)
            {
                Destroy(currentAITrainer);
            }

            Vector3 spawnPosition = GetRandomSpawnPosition();
            currentAITrainer = Instantiate(aiTrainerPrefab, spawnPosition, Quaternion.identity);
        }

        void SpawnWildMonster()
        {
            if (currentWildMonster != null)
            {
                Destroy(currentWildMonster);
            }

            Vector3 spawnPosition = GetRandomSpawnPosition();
            currentWildMonster = Instantiate(wildMonsterPrefab, spawnPosition, Quaternion.identity);
        }

        Vector3 GetRandomSpawnPosition()
        {
            Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
            randomDirection += transform.position;
            randomDirection.y = 1; // Assuming we want to keep it on the same plane
            return randomDirection;
        }

        IEnumerator CheckAndRespawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(checkInterval);

                if (AllMonstersDefeated(currentAITrainer))
                {
                    SpawnAITrainer();
                }

                if (AllMonstersDefeated(currentWildMonster))
                {
                    SpawnWildMonster();
                }
            }
        }

        bool AllMonstersDefeated(GameObject trainerObject)
        {
            if (trainerObject == null) return true;

            TrainerComponent trainerComponent = trainerObject.GetComponent<TrainerComponent>();
            if (trainerComponent == null || trainerComponent.TrainerData == null) return true;

            BattleTrainer battleTrainer = trainerComponent.BattleTrainer;
            if (battleTrainer == null) return true;


            foreach (var monster in battleTrainer.PartyMonsters)
            {
                if (monster.CurrentHP > 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
