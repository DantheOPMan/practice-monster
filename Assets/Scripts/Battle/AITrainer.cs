using UnityEngine;

public class AITrainer : Trainer
{
    // Inherit the Start method from Trainer

    public override Move ChooseMove(Monster monster)
    {
        // For simplicity, randomly choose a move
        // You can implement more complex AI logic here
        int moveChoice = Random.Range(0, monster.moves.Count);
        return monster.moves[moveChoice];
    }
}
