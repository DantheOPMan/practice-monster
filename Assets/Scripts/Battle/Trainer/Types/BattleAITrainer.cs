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

        public override IEnumerator SelectMove(Battle battle, System.Action<int> onMoveSelected, BattleUIManager battleUIManager = null)
        {
            yield return new WaitForSeconds(1); // Simulate thinking time
            int randomMoveIndex = Random.Range(0, PartyMonsters[CurrentMonsterIndex].Data.Moves.Count);
            onMoveSelected(randomMoveIndex);
        }

        public override IEnumerator Defend(Battle battle, System.Action<int> onDefenseSelected, BattleUIManager battleUIManager = null)
        {
            yield return new WaitForSeconds(1); // Simulate thinking time
            int randomDefenseIndex = Random.Range(0, 3); // 0 for Dodge, 1 for Brace, 2 for Standby
            onDefenseSelected(randomDefenseIndex);
        }
        public override IEnumerator SwitchMonster(Battle battle, System.Action<int> onSwitchSelected, BattleUIManager battleUIManager = null)
        {
            throw new System.NotImplementedException();
        }

        public override Monster GetNextMonster()
        {
            foreach (Monster monster in PartyMonsters)
            {
                if (monster.CurrentHP > 0)
                {
                    CurrentMonsterIndex = PartyMonsters.IndexOf(monster);
                    return monster;
                }
            }
            return null;
        }

        public override void HealAll()
        {
            foreach (Monster monster in PartyMonsters)
            {
                monster.InitializeStats();
            }
        }

        
    }
}
