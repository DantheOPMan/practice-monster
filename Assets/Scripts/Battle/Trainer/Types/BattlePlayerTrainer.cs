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

        public override IEnumerator SelectMove(Battle battle, System.Action<int> onMoveSelected, BattleUIManager battleUIManager)
        {
            Monster currentMonster = GetCurrentMonster();
            battleUIManager.ShowMoveSelectionUI(currentMonster);
            yield return new WaitUntil(() => battleUIManager.IsMoveSelected());
            int selectedMoveIndex = battleUIManager.GetSelectedMoveIndex();
            onMoveSelected(selectedMoveIndex);
        }

        public override IEnumerator Defend(Battle battle, System.Action<int> onDefenseSelected, BattleUIManager battleUIManager)
        {
            battleUIManager.ShowDefenseSelectionUI();
            yield return new WaitUntil(() => battleUIManager.IsDefenseSelected());
            int selectedDefenseIndex = battleUIManager.GetSelectedDefenseIndex();
            onDefenseSelected(selectedDefenseIndex);
        }

        public override IEnumerator SwitchMonster(Battle battle, System.Action<int> onSwitchSelected, BattleUIManager battleUIManager)
        {
            int currentMonsterIndex = PartyMonsters.IndexOf(GetCurrentMonster());
            battleUIManager.ShowSwitchSelectionUI(PartyMonsters, currentMonsterIndex);
            yield return new WaitUntil(() => battleUIManager.IsSwitchSelected());
            int selectedSwitchIndex = battleUIManager.GetSelectedSwitchIndex();
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
