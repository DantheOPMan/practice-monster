using UnityEngine;

public class PlayerTrainer : Trainer
{
    public override Move ChooseMove(Monster monster)
    {
        // For simplicity, randomly choose a move for now
        // In a real game, you'd get input from the player
        int moveChoice = Random.Range(0, monster.moves.Count);
        return monster.moves[moveChoice];
    }
}
