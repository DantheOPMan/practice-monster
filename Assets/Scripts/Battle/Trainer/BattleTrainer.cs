using System.Collections;
using System.Collections.Generic;

namespace PracticeMonster
{
    public abstract class BattleTrainer
    {
        public ITrainerData Data { get; protected set; }
        public string Name => Data.Name;
        public List<Monster> PartyMonsters => Data.InventoryMonsters;
        public int CurrentMonsterIndex { get; set; }
        public int ActionTurn { get; set; }
        public int selectedMoveIndex { get; set; }
        public int selectedDefenseIndex { get; set; }

        protected BattleTrainer(ITrainerData trainerData)
        {
            Data = trainerData;
            CurrentMonsterIndex = 0;
            ActionTurn = 0;
        }

        public abstract IEnumerator SelectMove(Battle battle, System.Action<int> onMoveSelected);
        public abstract IEnumerator Defend(Battle battle, System.Action<int> onDefenseSelected);
        public abstract IEnumerator SwitchMonster(Battle battle, System.Action<int> onSwitchSelected);

        public abstract Monster GetNextMonster();
        public Monster GetCurrentMonster()
        {
            return PartyMonsters[CurrentMonsterIndex];
        }
        public abstract void HealAll();
    }
}
