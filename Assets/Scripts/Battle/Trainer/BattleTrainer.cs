using System.Collections;
using System.Collections.Generic;

namespace PracticeMonster
{
    public abstract class BattleTrainer
    {
        public string Name { get; protected set; }
        public List<Monster> Monsters { get; protected set; }
        public int CurrentMonsterIndex { get; protected set; }
        public int ActionTurn { get; set; }
        public int selectedMoveIndex { get; set; }
        public int selectedDefenseIndex { get; set; }

        protected BattleTrainer(ITrainerData trainerData)
        {
            Name = trainerData.Name;
            Monsters = trainerData.InventoryMonsters;
            CurrentMonsterIndex = 0;
            ActionTurn = 0;
        }

        public abstract IEnumerator SelectMove(Battle battle, System.Action<int> onMoveSelected);
        public abstract IEnumerator Defend(Battle battle, System.Action<int> onDefenseSelected);

        public abstract Monster GetNextMonster();
        public Monster GetCurrentMonster()
        {
            return Monsters[CurrentMonsterIndex];
        }
        public abstract void HealAll();
    }
}
