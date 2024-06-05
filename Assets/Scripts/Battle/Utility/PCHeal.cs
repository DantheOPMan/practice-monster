using PracticeMonster;
using UnityEngine;

public class PCHeal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TrainerComponent playerTrainer = other.GetComponent<TrainerComponent>();
            if (playerTrainer != null)
            {
                playerTrainer.HealAll();
            }
        }
    }
}
