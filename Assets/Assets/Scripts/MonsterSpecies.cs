using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSpecies", menuName = "Monster/Species", order = 1)]
public class MonsterSpecies : ScriptableObject
{
    public string speciesName;
    public int baseHp;
    public int baseAttack;
    public int baseDefense;
    public int baseSpecialAttack;
    public int baseSpecialDefense;
    public int baseSpeed;
    public List<Move> moves;
    public List<string> types;
}
