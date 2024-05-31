using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public Trainer trainer1;  // Can be either PlayerTrainer or AITrainer
    public Trainer trainer2;  // Can be either PlayerTrainer or AITrainer
    private Monster monster1;
    private Monster monster2;
    private int turnCount;

    void Start()
    {
        turnCount = 0;
        monster1 = trainer1.GetNextAvailableMonster();
        monster2 = trainer2.GetNextAvailableMonster();
    }

    void Update()
    {
        if (monster1 != null && monster2 != null && monster1.hp > 0 && monster2.hp > 0)
        {
            NextTurn();
        }
    }

    void PrintStatus()
    {
        Debug.Log("Battle Status:");
        Debug.Log($"{monster1.monsterName} ({monster1.species.speciesName}): HP = {monster1.hp}, Stamina = {monster1.stamina}, Action Turn = {monster1.actionTurn}");
        Debug.Log($"{monster2.monsterName} ({monster2.species.speciesName}): HP = {monster2.hp}, Stamina = {monster2.stamina}, Action Turn = {monster2.actionTurn}");
    }

    void NextTurn()
    {
        turnCount += 1;

        while (monster1.actionTurn < 0 && monster2.actionTurn < 0)
        {
            monster1.UpdateActionTurn();
            monster2.UpdateActionTurn();
        }

        Monster attacker, defender;
        if (monster1.actionTurn == 0 && monster2.actionTurn == 0)
        {
            if (monster1.speed > monster2.speed)
            {
                attacker = monster1;
                defender = monster2;
            }
            else if (monster1.speed < monster2.speed)
            {
                attacker = monster2;
                defender = monster1;
            }
            else
            {
                var pair = Random.Range(0, 2) == 0 ? (monster1, monster2) : (monster2, monster1);
                attacker = pair.Item1;
                defender = pair.Item2;
            }
        }
        else
        {
            attacker = monster1.actionTurn == 0 ? monster1 : monster2;
            defender = monster1.actionTurn == 0 ? monster2 : monster1;
        }

        PrintStatus();

        Debug.Log($"\nTurn {turnCount}: {attacker.monsterName} ({attacker.species.speciesName})'s turn to attack");

        Move move;
        if (attacker == monster1)
        {
            move = trainer1.ChooseMove(attacker);
        }
        else
        {
            move = trainer2.ChooseMove(attacker);
        }

        if (move != null)
        {
            attacker.AttackMove(defender, move);
        }

        if (attacker.hp <= 0 || defender.hp <= 0)
        {
            string winner = attacker.hp > 0 ? attacker.monsterName : defender.monsterName;
            Debug.Log($"\n{winner} wins the battle!");
            enabled = false; // Stop the battle
        }
    }
}
