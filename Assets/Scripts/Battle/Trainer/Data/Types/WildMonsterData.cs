using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    public class WildMonsterData : ITrainerData
    {
        public string Name { get; private set; }
        public List<Monster> InventoryMonsters { get; private set; }

        public WildMonsterData(Monster monster)
        {
            Name = monster.Data.Species.Name;
            InventoryMonsters = new List<Monster> { monster };
        }
    }

}
