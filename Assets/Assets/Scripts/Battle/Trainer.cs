using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    public List<Monster> team;

    protected virtual void Start()
    {
        if (team == null)
        {
            team = new List<Monster>(6);
        }
    }

    public Monster GetNextAvailableMonster()
    {
        foreach (var monster in team)
        {
            if (monster.hp > 0)
            {
                return monster;
            }
        }
        return null;
    }

    public virtual Move ChooseMove(Monster monster)
    {
        // To be overridden by derived classes
        return null;
    }
}
