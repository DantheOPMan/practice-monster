using UnityEngine;

namespace PracticeMonster
{
    public class BattleManager : MonoBehaviour
    {
        public GameObject battleUIManagerPrefab; // Reference to the BattleUIManager prefab
        private GameObject battleUIManagerInstance;

        private BattleTrainer playerTrainer;
        private BattleTrainer opponentTrainer;
        private Battle battle;

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
            // Instantiate the BattleUIManager prefab
            battleUIManagerInstance = Instantiate(battleUIManagerPrefab);
            BattleUIManager.Instance.InitializeUI();

            // Initialize and start the battle
            battle = new Battle(trainer1, trainer2);
            StartCoroutine(battle.NextTurn());
        }

        public void EndBattle()
        {
            // Clean up after the battle
            if (battleUIManagerInstance != null)
            {
                BattleUIManager.Instance.EndBattleUI();
                Destroy(battleUIManagerInstance);
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            Debug.Log("Trigger activated");
            if (collider.CompareTag("Trainer"))
            {
                Debug.Log("isTrainer = true");
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
            if (data is PlayerTrainerData)
            {
                return new BattlePlayerTrainer((PlayerTrainerData)data);
            }
            else if (data is WildMonsterData)
            {
                return new BattleAITrainer((AITrainerData)data); // Assuming you use AITrainerData for wild monsters as well
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
