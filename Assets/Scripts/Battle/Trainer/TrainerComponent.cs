using UnityEngine;

namespace PracticeMonster
{
    public class TrainerComponent : MonoBehaviour
    {
        public ITrainerData TrainerData { get; private set; }
        public BattleTrainer BattleTrainer { get; private set; }

        public void Initialize(ITrainerData data)
        {
            TrainerData = data;
            // Initialize BattleTrainer based on the type of TrainerData
            if (data is PlayerTrainerData playerData)
            {
                BattleTrainer = new BattlePlayerTrainer(playerData);
            }
            else if (data is AITrainerData aiData)
            {
                BattleTrainer = new BattleAITrainer(aiData);
            }
            else if (data is WildMonsterData wildMonsterData)
            {
                BattleTrainer = new WildMonster(wildMonsterData);
            }
        }

        public void HealAll()
        {
            BattleTrainer?.HealAll();
        }
    }
}
