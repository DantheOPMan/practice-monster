using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    public class BattleManager : MonoBehaviour
    {
        public GameObject battleUIManagerPrefab; // Reference to the BattleUIManager prefab

        private List<Battle> activeBattles = new List<Battle>();
        private Dictionary<ITrainerData, BattleTrainer> activeTrainers = new Dictionary<ITrainerData, BattleTrainer>();

        void Start()
        {
            // Ensure the GameObjects have the TrainerComponent script attached
        }

        private void InitializeTrainerComponent(GameObject trainerObject, ITrainerData data)
        {
            TrainerComponent trainerComponent = trainerObject.GetComponent<TrainerComponent>();
            if (trainerComponent == null)
            {
                trainerComponent = trainerObject.AddComponent<TrainerComponent>();
            }
            trainerComponent.Initialize(data);
        }

        public void StartBattle(BattleTrainer trainer1, BattleTrainer trainer2)
        {
            if (activeTrainers.ContainsKey(trainer1.Data) || activeTrainers.ContainsKey(trainer2.Data))
            {
                Debug.Log("One of the trainers is already in a battle.");
                return;
            }

            // Instantiate the BattleUIManager prefab
            GameObject battleUIManagerInstance = Instantiate(battleUIManagerPrefab);
            BattleUIManager battleUIManager = battleUIManagerInstance.GetComponent<BattleUIManager>();

            // Initialize and start the battle
            Battle battle = gameObject.AddComponent<Battle>();
            battle.Initialize(trainer1, trainer2, battleUIManager);
            activeBattles.Add(battle);
            activeTrainers[trainer1.Data] = trainer1;
            activeTrainers[trainer2.Data] = trainer2;
        }

        public void EndBattle(Battle battle)
        {
            if (battle != null)
            {
                BattleUIManager battleUIManager = battle.GetBattleUIManager();
                if (battleUIManager != null)
                {
                    battleUIManager.EndBattleUI();
                    Destroy(battleUIManager.gameObject);
                }

                activeTrainers.Remove(battle.GetTrainer1().Data);
                activeTrainers.Remove(battle.GetTrainer2().Data);
                activeBattles.Remove(battle);
                Destroy(battle);
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Trainer"))
            {
                TrainerComponent trainerComponent1 = GetComponent<TrainerComponent>();
                TrainerComponent trainerComponent2 = collider.GetComponent<TrainerComponent>();

                if (trainerComponent1 != null && trainerComponent2 != null)
                {
                    // Initialize trainers based on data
                    BattleTrainer trainer1 = CreateTrainerFromData(trainerComponent1.TrainerData);
                    BattleTrainer trainer2 = CreateTrainerFromData(trainerComponent2.TrainerData);
                    StartBattle(trainer1, trainer2);
                }
            }
        }

        private BattleTrainer CreateTrainerFromData(ITrainerData data)
        {
            if (activeTrainers.TryGetValue(data, out var existingTrainer))
            {
                return existingTrainer;
            }

            if (data is PlayerTrainerData)
            {
                return new BattlePlayerTrainer((PlayerTrainerData)data);
            }
            else if (data is WildMonsterData)
            {
                return new WildMonster((WildMonsterData)data); // Assuming you use AITrainerData for wild monsters as well
            }
            else if (data is AITrainerData)
            {
                return new BattleAITrainer((AITrainerData)data);
            }
            else
            {
                throw new System.Exception("Unknown trainer data type");
            }
        }
    }
}
