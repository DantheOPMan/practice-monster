using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    public class BattlePlayerTrainer : BattleTrainer
    {
        public List<Monster> PCMonsters => ((PlayerTrainerData)Data).PCMonsters;

        public BattlePlayerTrainer(PlayerTrainerData trainerData) : base(trainerData)
        {
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
            yield return new WaitUntil(() => BattleUIManager.Instance.IsDefenseSelected());
            int selectedDefenseIndex = BattleUIManager.Instance.GetSelectedDefenseIndex();
            onDefenseSelected(selectedDefenseIndex);
        }
        public override IEnumerator SwitchMonster(Battle battle, System.Action<int> onSwitchSelected)
        {
            BattleUIManager.Instance.ShowSwitchSelectionUI(PartyMonsters, onSwitchSelected);
            yield return new WaitUntil(() => BattleUIManager.Instance.IsSwitchSelected());
            int selectedSwitchIndex = BattleUIManager.Instance.GetSelectedSwitchIndex();
            onSwitchSelected(selectedSwitchIndex);
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
            foreach (Monster monster in PCMonsters)
            {
                monster.InitializeStats();
            }
        }
    }
}
