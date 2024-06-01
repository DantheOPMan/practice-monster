using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject battleUIPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AI"))
        {
            StartBattle();
        }
    }

    void StartBattle()
    {
        Debug.Log("Battle Started!");

        // Activate the battle UI panel
        if (battleUIPanel != null)
        {
            battleUIPanel.SetActive(true);
        }

        // Additional logic to start the battle can go here
    }
}
