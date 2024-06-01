using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public string monsterName;
    public int level;
    public MonsterSpecies species;
    public Dictionary<string, int> ivs;
    public Dictionary<string, int> evs;
    public List<Move> moves;
    public int hp, maxHp, attack, defense, specialAttack, specialDefense, speed;
    public int stamina;
    public int actionTurn;
    public int accuracyStage, evasivenessStage;
    public string defendingAction;

    void Start()
    {
        if (species != null)
        {
            InitializeStats();
        }
    }

    public void InitializeStats()
    {
        // Initialize IVs randomly
        ivs = GenerateRandomIVs();
        evs = new Dictionary<string, int>
        {
            { "hp", 0 },
            { "attack", 0 },
            { "defense", 0 },
            { "specialAttack", 0 },
            { "specialDefense", 0 },
            { "speed", 0 }
        };

        // Initialize moves from species
        moves = new List<Move>(species.moves);

        // Calculate initial stats
        hp = CalculateStat("hp");
        maxHp = hp;
        attack = CalculateStat("attack");
        defense = CalculateStat("defense");
        specialAttack = CalculateStat("specialAttack");
        specialDefense = CalculateStat("specialDefense");
        speed = CalculateStat("speed");
        stamina = 100;
        actionTurn = 0;
        accuracyStage = 0;
        evasivenessStage = 0;
    }

    Dictionary<string, int> GenerateRandomIVs()
    {
        return new Dictionary<string, int>
        {
            { "hp", Random.Range(0, 16) },
            { "attack", Random.Range(0, 16) },
            { "defense", Random.Range(0, 16) },
            { "specialAttack", Random.Range(0, 16) },
            { "specialDefense", Random.Range(0, 16) },
            { "speed", Random.Range(0, 16) }
        };
    }

    public void IncreaseEV(string stat, int amount)
    {
        if (evs[stat] + amount > 450)
        {
            evs[stat] = 450;
        }
        else
        {
            evs[stat] += amount;
        }

        int totalEVs = 0;
        foreach (var ev in evs.Values)
        {
            totalEVs += ev;
        }

        if (totalEVs > 1000)
        {
            evs[stat] -= (totalEVs - 1000);
        }
    }

    int CalculateStat(string statType)
    {
        int baseStat = 0;
        switch (statType)
        {
            case "hp": baseStat = species.baseHp; break;
            case "attack": baseStat = species.baseAttack; break;
            case "defense": baseStat = species.baseDefense; break;
            case "specialAttack": baseStat = species.baseSpecialAttack; break;
            case "specialDefense": baseStat = species.baseSpecialDefense; break;
            case "speed": baseStat = species.baseSpeed; break;
        }

        if (statType == "hp")
        {
            return Mathf.FloorToInt(level + 20 + (level / 100f * (2 * baseStat + ivs[statType] + (evs[statType] / 10f))));
        }
        else
        {
            return Mathf.FloorToInt(20 + (level / 100f * (2 * baseStat + ivs[statType] + (evs[statType] / 10f))));
        }
    }

    public void DisplayStats()
    {
        Debug.Log($"Stats for {monsterName} ({species.speciesName}):");
        Debug.Log($"HP: {hp}");
        Debug.Log($"Attack: {attack}");
        Debug.Log($"Defense: {defense}");
        Debug.Log($"Special Attack: {specialAttack}");
        Debug.Log($"Special Defense: {specialDefense}");
        Debug.Log($"Speed: {speed}");
        Debug.Log($"Stamina: {stamina}");
        Debug.Log($"Action Turn: {actionTurn}\n");
    }

    public bool AttackMove(Monster target, Move move)
    {
        if (stamina < move.staminaCost)
        {
            Debug.Log($"{name} does not have enough stamina to use {move.name}!");
            return false;
        }

        Debug.Log($"{target.name}'s turn to defend");
        target.defendingAction = "nothing"; // For now, we'll default to "nothing". This should be set by the player.

        if (target.defendingAction != "brace")
        {
            float dodgeProbability = 0;
            if (target.defendingAction == "dodge")
            {
                dodgeProbability = target.GetDodgeProbability(speed);
            }
            float hitProbability = move.accuracy * (1 - dodgeProbability) * GetAccuracyMultiplier() * target.GetEvasivenessMultiplier();
            if (Random.value > hitProbability)
            {
                Debug.Log($"{name} missed with {move.name}!");
                stamina -= move.staminaCost;
                actionTurn = GetActionTurnReset() + move.turnAdjustment;
                return false;
            }
        }

        int attackStat = move.moveType == "physical" ? attack : specialAttack;
        int defenseStat = move.moveType == "physical" ? target.defense : target.specialDefense;

        int damage = CalculateDamage(target, move, attackStat, defenseStat);

        target.hp -= damage;
        stamina -= move.staminaCost;

        Debug.Log($"{name} used {move.name} and dealt {damage} damage to {target.name}!");

        actionTurn = GetActionTurnReset() + move.turnAdjustment;

        return true;
    }

    int CalculateDamage(Monster target, Move move, int attackStat, int defenseStat)
    {
        int levelCalc = Mathf.Max(0, level - target.level);
        float baseDamage = (((100 + levelCalc) / 100f) * move.power * (attackStat / (float)defenseStat)) + 1;

        float modifier = 1;
        if (species.types.Contains(move.moveType))
        {
            modifier *= 1.5f;
        }

        float typeEffectiveness = 1;  // Placeholder for type effectiveness calculation
        modifier *= typeEffectiveness;

        if (Random.value < move.critRate)
        {
            modifier *= 1.5f;
            Debug.Log("Critical hit!");
        }

        return Mathf.FloorToInt(baseDamage * modifier);
    }

    float GetAccuracyMultiplier()
    {
        return accuracyStage > 0 ? 1 + (accuracyStage * 0.5f) : 1 - (Mathf.Abs(accuracyStage) * (100 / 7f) / 100f);
    }

    float GetEvasivenessMultiplier()
    {
        return evasivenessStage > 0 ? 1 + (evasivenessStage * (100 / 7f) / 100f) : 1 - (Mathf.Abs(evasivenessStage) * (100 / 7f) / 100f);
    }

    float GetDodgeProbability(int attackerSpeed)
    {
        float dodgeChance = (speed - attackerSpeed) / 2f + 50;
        return Mathf.Clamp(dodgeChance / 100f, 0, 1);
    }

    public void Dodge()
    {
        Debug.Log($"{name} is attempting to dodge the attack!");
        stamina -= 30;
    }

    public void Brace()
    {
        Debug.Log($"{name} is bracing for the next attack!");
        stamina -= 20;
    }

    public void UpdateActionTurn()
    {
        actionTurn += 1;
        stamina += 2;
        if (stamina > 100)
        {
            stamina = 100;
        }
    }

    int GetActionTurnReset()
    {
        if (speed <= 15)
        {
            return -12;
        }
        else if (speed <= 31)
        {
            return -11;
        }
        else if (speed <= 55)
        {
            return -10;
        }
        else if (speed <= 88)
        {
            return -9;
        }
        else if (speed <= 129)
        {
            return -8;
        }
        else if (speed <= 181)
        {
            return -7;
        }
        else if (speed <= 242)
        {
            return -6;
        }
        else if (speed <= 316)
        {
            return -5;
        }
        else if (speed <= 401)
        {
            return -4;
        }
        else
        {
            return -3;
        }
    }
}
