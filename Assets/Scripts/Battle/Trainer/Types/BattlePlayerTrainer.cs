using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    public class BattlePlayerTrainer : BattleTrainer
    {
        public List<Monster> PCMonsters { get; private set; }

        public BattlePlayerTrainer(PlayerTrainerData trainerData) : base(trainerData)
        {
            PCMonsters = trainerData.PCMonsters;
        }

        public override IEnumerator SelectMove(Battle battle, System.Action<int> onMoveSelected)
        {
            Monster currentMonster = GetCurrentMonster();
            BattleUIManager.Instance.ShowMoveSelectionUI(currentMonster, onMoveSelected);
            yield return new WaitUntil(() => BattleUIManager.Instance.IsMoveSelected());
            int selectedMoveIndex = BattleUIManager.Instance.GetSelectedMoveIndex();
            onMoveSelected(selectedMoveIndex);
        }

        public override IEnumerator Defend(Battle battle, System.Action<int> onDefenseSelected)
        {
            BattleUIManager.Instance.ShowDefenseSelectionUI(onDefenseSelected);
            yield return new WaitUntil(() => BattleUIManager.Instance.IsDefenseActionSelected());
            int selectedDefenseIndex = BattleUIManager.Instance.GetSelectedDefenseIndex();
            onDefenseSelected(selectedDefenseIndex);
            Debug.Log(selectedDefenseIndex);
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
