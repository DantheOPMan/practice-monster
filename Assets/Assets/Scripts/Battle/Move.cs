using UnityEngine;

[System.Serializable]
public class Move
{
    public string name;
    public int power;
    public int staminaCost;
    public float accuracy;
    public string moveType;  // "physical" or "special"
    public int turnAdjustment;
    public float critRate;

    public Move(string name, int power, int staminaCost, float accuracy, string moveType, int turnAdjustment, float critRate = 0.1f)
    {
        this.name = name;
        this.power = power;
        this.staminaCost = staminaCost;
        this.accuracy = accuracy;
        this.moveType = moveType;
        this.turnAdjustment = turnAdjustment;
        this.critRate = critRate;
    }
}
