using UnityEngine;

namespace PracticeMonster
{
    public class BattleManager : MonoBehaviour
    {
        public GameObject battleUIManagerPrefab; // Reference to the BattleUIManager prefab
        private GameObject battleUIManagerInstance;

        private BattlePlayerTrainer playerTrainer;
        private BattleTrainer opponentTrainer;
        private Battle battle;

        void Start()
        {
            // Initialize playerTrainer and opponentTrainer as needed
        }

        public void StartBattle()
        {
            // Instantiate the BattleUIManager prefab
            battleUIManagerInstance = Instantiate(battleUIManagerPrefab);
            BattleUIManager.Instance.InitializeUI();

            // Initialize and start the battle
            battle = new Battle(playerTrainer, opponentTrainer);
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
    }
}
