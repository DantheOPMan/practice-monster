using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    public interface ITrainerData
    {
        string Name { get; }
        List<Monster> InventoryMonsters { get; }
    }

}
