using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    [Serializable]
    public class PlayerTrainerData : ITrainerData
    {
        public string Name { get; private set; }
        public List<Monster> InventoryMonsters { get; private set; }
        public List<Monster> PCMonsters { get; private set; }

        public PlayerTrainerData(string name, List<Monster> inventoryMonsters, List<Monster> pcMonsters)
        {
            if (inventoryMonsters.Count > 6)
            {
                throw new System.ArgumentException("InventoryMonsters cannot contain more than 6 monsters.");
            }

            Name = name;
            InventoryMonsters = inventoryMonsters;
            PCMonsters = pcMonsters;
        }
        public void AddMonster(Monster newMonster)
        {
            if (InventoryMonsters.Count < 6)
            {
                InventoryMonsters.Add(newMonster);
                Debug.Log($"{newMonster.Nickname} was added to the inventory.");
            }
            else
            {
                PCMonsters.Add(newMonster);
                Debug.Log($"{newMonster.Nickname} was added to the PC.");
            }
        }
    }
}
