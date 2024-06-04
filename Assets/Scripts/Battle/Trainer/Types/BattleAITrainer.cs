using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    public class BattleAITrainer : BattleTrainer
    {
        public BattleAITrainer(AITrainerData trainerData) : base(trainerData)
        {
        }

        public override IEnumerator SelectMove(Battle battle, System.Action<int> onMoveSelected)
        {
            yield return new WaitForSeconds(1); // Simulate thinking time
            int randomMoveIndex = Random.Range(0, Monsters[CurrentMonsterIndex].Data.Moves.Count);
            onMoveSelected(randomMoveIndex);
            Debug.Log($"{Name} selected move index {randomMoveIndex}");
        }

        public override IEnumerator Defend(Battle battle, System.Action<int> onDefenseSelected)
        {
            yield return new WaitForSeconds(1); // Simulate thinking time
            int randomDefenseIndex = Random.Range(0, 3); // 0 for Dodge, 1 for Brace, 2 for Standby
            onDefenseSelected(randomDefenseIndex);
            Debug.Log($"{Name} selected defense index {randomDefenseIndex}");
        }

        public override Monster GetNextMonster()
        {
            foreach (Monster monster in Monsters)
            {
                if (monster.CurrentHP > 0)
                {
                    CurrentMonsterIndex = Monsters.IndexOf(monster);
                    return monster;
                }
            }
            return null;
        }
    }

}
