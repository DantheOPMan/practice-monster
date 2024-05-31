using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    public Slider monster1HealthBar;
    public Slider monster2HealthBar;
    public Text battleMessageText;
    public Button moveButton1;
    public Button moveButton2;
    public Button moveButton3;
    public Button moveButton4;

    private Monster playerMonster;
    private Monster aiMonster;
    private Battle battle;

    void Start()
    {
        // Assume battle is assigned or find it in the scene
        battle = FindObjectOfType<Battle>();
        UpdateHealthBars();
        UpdateMoveButtons();
        battleMessageText.text = "Battle Start!";
    }

    public void SetMonsters(Monster player, Monster ai)
    {
        playerMonster = player;
        aiMonster = ai;
        monster1HealthBar.maxValue = playerMonster.maxHp;
        monster2HealthBar.maxValue = aiMonster.maxHp;
        UpdateHealthBars();
    }

    void UpdateHealthBars()
    {
        monster1HealthBar.value = playerMonster.hp;
        monster2HealthBar.value = aiMonster.hp;
    }

    void UpdateMoveButtons()
    {
        moveButton1.GetComponentInChildren<Text>().text = playerMonster.moves[0].name;
        moveButton2.GetComponentInChildren<Text>().text = playerMonster.moves[1].name;
        moveButton3.GetComponentInChildren<Text>().text = playerMonster.moves[2].name;
        moveButton4.GetComponentInChildren<Text>().text = playerMonster.moves[3].name;
    }

    public void OnMoveButton1()
    {
        battle.PlayerChooseMove(playerMonster.moves[0]);
    }

    public void OnMoveButton2()
    {
        battle.PlayerChooseMove(playerMonster.moves[1]);
    }

    public void OnMoveButton3()
    {
        battle.PlayerChooseMove(playerMonster.moves[2]);
    }

    public void OnMoveButton4()
    {
        battle.PlayerChooseMove(playerMonster.moves[3]);
    }

    public void DisplayMessage(string message)
    {
        battleMessageText.text = message;
    }
}
