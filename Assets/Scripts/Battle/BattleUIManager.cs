using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace PracticeMonster
{
    public class BattleUIManager : MonoBehaviour
    {
        public static BattleUIManager Instance;

        public Button moveButton1;
        public Button moveButton2;
        public Button moveButton3;
        public Button moveButton4;

        public Button dodgeButton;
        public Button braceButton;
        public Button standbyButton;

        public Button attackButton;
        public Button switchButton;
        public Button healButton;
        public Button catchButton;

        public GameObject moveSelectionPanel;
        public GameObject defenseSelectionPanel;
        public GameObject actionSelectionPanel;
        public GameObject switchSelectionPanel;

        public GameObject battleUIPrefab; // Reference to the BattleUI prefab
        private GameObject battleUIInstance;

        private bool moveSelected = false;
        private bool defenseSelected = false;
        private bool actionSelected = false;
        private bool switchSelected = false;
        private int selectedMoveIndex;
        private int selectedDefenseIndex;
        private int selectedActionIndex;
        private int selectedSwitchIndex;

        // References to HP, Stamina, and XP sliders and name text
        public Slider trainer1HpSlider;
        public TextMeshProUGUI trainer1HpText;
        public TextMeshProUGUI trainer1StaminaText;
        public TextMeshProUGUI trainer1LevelText;
        public TextMeshProUGUI trainer1NameText;
        public Slider trainer1StaminaSlider;
        public Slider trainer1XpSlider; // XP slider for trainer 1
        public TextMeshProUGUI trainer1XpText; // XP text for trainer 1

        public Slider trainer2HpSlider;
        public TextMeshProUGUI trainer2HpText;
        public TextMeshProUGUI trainer2StaminaText;
        public TextMeshProUGUI trainer2LevelText;
        public TextMeshProUGUI trainer2NameText;
        public Slider trainer2StaminaSlider;
        public Slider trainer2XpSlider; // XP slider for trainer 2
        public TextMeshProUGUI trainer2XpText; // XP text for trainer 2

        public TextMeshProUGUI logText; // Reference to the log text component

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            // Buttons will be assigned in InitializeUI
        }

        public void InitializeUI(BattleTrainer trainer1, BattleTrainer trainer2)
        {
            // Instantiate the BattleUI prefab
            battleUIInstance = Instantiate(battleUIPrefab);

            // Assign the instantiated UI elements to the script variables
            moveSelectionPanel = battleUIInstance.transform.Find("MoveSelectionPanel").gameObject;
            defenseSelectionPanel = battleUIInstance.transform.Find("DefenseSelectionPanel").gameObject;
            actionSelectionPanel = battleUIInstance.transform.Find("ActionSelectionPanel").gameObject;
            switchSelectionPanel = battleUIInstance.transform.Find("SwitchSelectionPanel").gameObject;

            moveButton1 = moveSelectionPanel.transform.Find("MoveButton1").GetComponent<Button>();
            moveButton2 = moveSelectionPanel.transform.Find("MoveButton2").GetComponent<Button>();
            moveButton3 = moveSelectionPanel.transform.Find("MoveButton3").GetComponent<Button>();
            moveButton4 = moveSelectionPanel.transform.Find("MoveButton4").GetComponent<Button>();

            dodgeButton = defenseSelectionPanel.transform.Find("DodgeButton").GetComponent<Button>();
            braceButton = defenseSelectionPanel.transform.Find("BraceButton").GetComponent<Button>();
            standbyButton = defenseSelectionPanel.transform.Find("StandbyButton").GetComponent<Button>();

            attackButton = actionSelectionPanel.transform.Find("AttackButton").GetComponent<Button>();
            switchButton = actionSelectionPanel.transform.Find("SwitchButton").GetComponent<Button>();
            healButton = actionSelectionPanel.transform.Find("HealButton").GetComponent<Button>();
            catchButton = actionSelectionPanel.transform.Find("CatchButton").GetComponent<Button>();

            // Find and assign HP, Stamina, and XP sliders and texts
            Transform dataTransform1 = battleUIInstance.transform.Find("Data");
            trainer1HpSlider = dataTransform1.Find("Trainer1HpSlider").GetComponent<Slider>();
            trainer1HpText = dataTransform1.Find("Trainer1HpText").GetComponent<TextMeshProUGUI>();
            trainer1StaminaSlider = dataTransform1.Find("Trainer1StaminaSlider").GetComponent<Slider>();
            trainer1StaminaText = dataTransform1.Find("Trainer1StaminaText").GetComponent<TextMeshProUGUI>();
            trainer1XpSlider = dataTransform1.Find("Trainer1XpSlider").GetComponent<Slider>(); // XP slider
            trainer1XpText = dataTransform1.Find("Trainer1XpText").GetComponent<TextMeshProUGUI>(); // XP text
            trainer1LevelText = dataTransform1.Find("Trainer1LevelText").GetComponent<TextMeshProUGUI>();
            trainer1NameText = dataTransform1.Find("Trainer1NameText").GetComponent<TextMeshProUGUI>();

            Transform dataTransform2 = battleUIInstance.transform.Find("Data");
            trainer2HpSlider = dataTransform2.Find("Trainer2HpSlider").GetComponent<Slider>();
            trainer2HpText = dataTransform2.Find("Trainer2HpText").GetComponent<TextMeshProUGUI>();
            trainer2StaminaSlider = dataTransform2.Find("Trainer2StaminaSlider").GetComponent<Slider>();
            trainer2StaminaText = dataTransform2.Find("Trainer2StaminaText").GetComponent<TextMeshProUGUI>();
            trainer2XpSlider = dataTransform2.Find("Trainer2XpSlider").GetComponent<Slider>(); // XP slider
            trainer2XpText = dataTransform2.Find("Trainer2XpText").GetComponent<TextMeshProUGUI>(); // XP text
            trainer2LevelText = dataTransform2.Find("Trainer2LevelText").GetComponent<TextMeshProUGUI>();
            trainer2NameText = dataTransform2.Find("Trainer2NameText").GetComponent<TextMeshProUGUI>();


            moveSelectionPanel.SetActive(false);
            defenseSelectionPanel.SetActive(false);
            actionSelectionPanel.SetActive(false);
            switchSelectionPanel.SetActive(false);

            logText = battleUIInstance.transform.Find("LogText").GetComponent<TextMeshProUGUI>();

            UpdateBattleUI(trainer1, trainer2);
        }

        public void ShowActionSelectionUI(System.Action<int> callback)
        {
            actionSelectionPanel.SetActive(true);

            // Remove all previous listeners to prevent multiple invocations
            attackButton.onClick.RemoveAllListeners();
            switchButton.onClick.RemoveAllListeners();
            healButton.onClick.RemoveAllListeners();
            catchButton.onClick.RemoveAllListeners();

            // Add new listeners
            attackButton.onClick.AddListener(() => { callback(0); actionSelected = true; actionSelectionPanel.SetActive(false); });
            switchButton.onClick.AddListener(() => { callback(1); actionSelected = true; actionSelectionPanel.SetActive(false); });
            healButton.onClick.AddListener(() => { callback(2); actionSelected = true; actionSelectionPanel.SetActive(false); });
            catchButton.onClick.AddListener(() => { callback(3); actionSelected = true; actionSelectionPanel.SetActive(false); });
        }

        private void SetMoveButton(Button button, Monster monster, int moveIndex, System.Action<int> callback)
        {
            if (moveIndex < monster.Data.Moves.Count)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = monster.Data.Moves[moveIndex].Name;
                button.gameObject.SetActive(true);
                button.onClick.AddListener(() => { callback(moveIndex); OnMoveButtonClicked(moveIndex); moveSelectionPanel.SetActive(false); });
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        }
        public void ShowMoveSelectionUI(Monster currentMonster, System.Action<int> callback)
        {
            moveSelectionPanel.SetActive(true);

            // Set button names and visibility based on monster's moves
            SetMoveButton(moveButton1, currentMonster, 0, callback);
            SetMoveButton(moveButton2, currentMonster, 1, callback);
            SetMoveButton(moveButton3, currentMonster, 2, callback);
            SetMoveButton(moveButton4, currentMonster, 3, callback);
        }

        public void ShowDefenseSelectionUI(System.Action<int> callback)
        {
            defenseSelectionPanel.SetActive(true);

            // Remove all previous listeners to prevent multiple invocations
            dodgeButton.onClick.RemoveAllListeners();
            braceButton.onClick.RemoveAllListeners();
            standbyButton.onClick.RemoveAllListeners();

            // Add new listeners
            dodgeButton.onClick.AddListener(() => { /*callback(0)*/; OnDefenseButtonClicked(0); defenseSelectionPanel.SetActive(false); });
            braceButton.onClick.AddListener(() => { /*callback(1)*/; OnDefenseButtonClicked(1); defenseSelectionPanel.SetActive(false); });
            standbyButton.onClick.AddListener(() => { /*callback(2)*/; OnDefenseButtonClicked(2); defenseSelectionPanel.SetActive(false); });
        }

        public void ShowSwitchSelectionUI(List<Monster> monsters, System.Action<int> callback)
        {
            switchSelectionPanel.SetActive(true);

            // Assuming you have 6 buttons on the canvas with names SwitchButton0 to SwitchButton5
            for (int i = 0; i < 6; i++)
            {
                Button switchButton = switchSelectionPanel.transform.Find("SwitchButton" + (i+1)).GetComponent<Button>();
                TextMeshProUGUI buttonText = switchButton.GetComponentInChildren<TextMeshProUGUI>();

                if (i < monsters.Count)
                {
                    buttonText.text = monsters[i].Nickname;

                    if (monsters[i].CurrentHP > 0)
                    {
                        switchButton.interactable = true;
                        switchButton.onClick.RemoveAllListeners(); // Clear previous listeners
                        int index = i; // Capture the current index
                        switchButton.onClick.AddListener(() => { /*callback(index)*/; selectedSwitchIndex = index; switchSelected = true; switchSelectionPanel.SetActive(false); });
                    }
                    else
                    {
                        switchButton.interactable = false;
                    }

                    switchButton.gameObject.SetActive(true);
                }
                else
                {
                    switchButton.gameObject.SetActive(false);
                }

                // Ensure the current monster button is visible and disabled
                if (i == selectedMoveIndex)
                {
                    switchButton.interactable = false;
                    switchButton.gameObject.SetActive(true);
                }
            }
        }


        public void UpdateHpSlider(Slider hpSlider, TextMeshProUGUI hpText, int currentHP, int maxHP)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
            hpText.text = $"{currentHP} / {maxHP}";
        }

        public void UpdateStaminaSlider(Slider staminaSlider, TextMeshProUGUI staminaText, int currentStamina)
        {
            staminaSlider.maxValue = 100;
            staminaSlider.value = currentStamina;
            staminaText.text = $"{currentStamina} / 100";
        }

        public void UpdateXpSlider(Slider xpSlider, TextMeshProUGUI xpText, int currentXP, int nextLevelXP)
        {
            xpSlider.maxValue = nextLevelXP;
            xpSlider.value = currentXP;
            xpText.text = $"{currentXP} / {nextLevelXP}";
        }

        public void UpdateMonsterName(TextMeshProUGUI nameText, string name)
        {
            nameText.text = name;
        }

        public void UpdateBattleUI(BattleTrainer trainer1, BattleTrainer trainer2)
        {
            UpdateHpSlider(trainer1HpSlider, trainer1HpText, trainer1.GetCurrentMonster().CurrentHP, trainer1.GetCurrentMonster().Data.MaxHP);
            UpdateStaminaSlider(trainer1StaminaSlider, trainer1StaminaText, trainer1.GetCurrentMonster().Stamina);
            UpdateXpSlider(trainer1XpSlider, trainer1XpText, trainer1.GetCurrentMonster().Data.CurrentExperience - trainer1.GetCurrentMonster().Data.Species.ExperienceForNextLevel(trainer1.GetCurrentMonster().Data.Level - 1), trainer1.GetCurrentMonster().Data.Species.ExperienceForNextLevel(trainer1.GetCurrentMonster().Data.Level) - (trainer1.GetCurrentMonster().Data.Species.ExperienceForNextLevel(trainer1.GetCurrentMonster().Data.Level - 1)));
            trainer1LevelText.text = "Lvl " + trainer1.GetCurrentMonster().Data.Level.ToString();
            trainer1NameText.text = trainer1.GetCurrentMonster().Nickname;

            UpdateHpSlider(trainer2HpSlider, trainer2HpText, trainer2.GetCurrentMonster().CurrentHP, trainer2.GetCurrentMonster().Data.MaxHP);
            UpdateStaminaSlider(trainer2StaminaSlider, trainer2StaminaText, trainer2.GetCurrentMonster().Stamina);
            UpdateXpSlider(trainer2XpSlider, trainer2XpText, trainer2.GetCurrentMonster().Data.CurrentExperience - trainer2.GetCurrentMonster().Data.Species.ExperienceForNextLevel(trainer2.GetCurrentMonster().Data.Level - 1), trainer2.GetCurrentMonster().Data.Species.ExperienceForNextLevel(trainer2.GetCurrentMonster().Data.Level) - (trainer2.GetCurrentMonster().Data.Species.ExperienceForNextLevel(trainer2.GetCurrentMonster().Data.Level - 1)));
            trainer2LevelText.text = "Lvl " + trainer2.GetCurrentMonster().Data.Level.ToString();
            trainer2NameText.text = trainer2.GetCurrentMonster().Nickname;
        }

        public void Log(string message)
        {
            logText.text += message + "\n";
        }

        public void LogReset()
        {
            logText.text = "";
        }

        public void EndBattleUI()
        {
            if (battleUIInstance != null)
            {
                Destroy(battleUIInstance);
            }
        }

        private void OnMoveButtonClicked(int index)
        {
            selectedMoveIndex = index;
            moveSelected = true;
        }

        private void OnDefenseButtonClicked(int index)
        {
            selectedDefenseIndex = index;
            defenseSelected = true;
        }

        public bool IsMoveSelected()
        {
            return moveSelected;
        }

        public int GetSelectedMoveIndex()
        {
            moveSelected = false;
            return selectedMoveIndex;
        }

        public bool IsDefenseSelected()
        {
            return defenseSelected;
        }

        public int GetSelectedDefenseIndex()
        {
            defenseSelected = false;
            return selectedDefenseIndex;
        }
        public bool IsSwitchSelected()
        {
            return switchSelected;
        }

        public int GetSelectedSwitchIndex()
        {
            switchSelected = false;
            return selectedSwitchIndex;
        }

        public bool IsActionSelected()
        {
            return actionSelected;
        }
    }
}
