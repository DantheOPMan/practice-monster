using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        public GameObject moveSelectionPanel;
        public GameObject defenseSelectionPanel;

        public GameObject battleUIPrefab; // Reference to the BattleUI prefab
        private GameObject battleUIInstance;

        private bool moveSelected = false;
        private bool defenseActionSelected = false;
        private int selectedMoveIndex;
        private int selectedDefenseIndex;

        // References to HP sliders and name text
        public Slider trainer1HpSlider;
        public TextMeshProUGUI trainer1NameText;
        public Slider trainer2HpSlider;
        public TextMeshProUGUI trainer2NameText;
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

            moveButton1 = moveSelectionPanel.transform.Find("MoveButton1").GetComponent<Button>();
            moveButton2 = moveSelectionPanel.transform.Find("MoveButton2").GetComponent<Button>();
            moveButton3 = moveSelectionPanel.transform.Find("MoveButton3").GetComponent<Button>();
            moveButton4 = moveSelectionPanel.transform.Find("MoveButton4").GetComponent<Button>();

            dodgeButton = defenseSelectionPanel.transform.Find("DodgeButton").GetComponent<Button>();
            braceButton = defenseSelectionPanel.transform.Find("BraceButton").GetComponent<Button>();
            standbyButton = defenseSelectionPanel.transform.Find("StandbyButton").GetComponent<Button>();

            // Find and assign HP sliders and name texts
            trainer1HpSlider = battleUIInstance.transform.Find("Trainer1HpSlider").GetComponent<Slider>();
            trainer1NameText = battleUIInstance.transform.Find("Trainer1NameText").GetComponent<TextMeshProUGUI>();
            trainer2HpSlider = battleUIInstance.transform.Find("Trainer2HpSlider").GetComponent<Slider>();
            trainer2NameText = battleUIInstance.transform.Find("Trainer2NameText").GetComponent<TextMeshProUGUI>();

            moveSelectionPanel.SetActive(false);
            defenseSelectionPanel.SetActive(false);

            // Set initial HP and names
            UpdateHpSlider(trainer1HpSlider, trainer1.GetCurrentMonster().CurrentHP, trainer1.GetCurrentMonster().Data.MaxHP);
            trainer1NameText.text = trainer1.GetCurrentMonster().Nickname;

            UpdateHpSlider(trainer2HpSlider, trainer2.GetCurrentMonster().CurrentHP, trainer2.GetCurrentMonster().Data.MaxHP);
            trainer2NameText.text = trainer2.GetCurrentMonster().Nickname;

            logText = battleUIInstance.transform.Find("LogText").GetComponent<TextMeshProUGUI>();
        }

        private void SetMoveButton(Button button, Monster monster, int moveIndex, System.Action<int> callback)
        {
            if (moveIndex < monster.Data.Moves.Count)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = monster.Data.Moves[moveIndex].Name;
                button.gameObject.SetActive(true);
                button.onClick.AddListener(() => { callback(moveIndex); moveSelected = true; selectedMoveIndex = moveIndex; moveSelectionPanel.SetActive(false); });
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
            dodgeButton.onClick.AddListener(() => { callback(0); selectedDefenseIndex = 0; defenseActionSelected = true; defenseSelectionPanel.SetActive(false); });
            braceButton.onClick.AddListener(() => { callback(1); selectedDefenseIndex = 1;  defenseActionSelected = true; defenseSelectionPanel.SetActive(false); });
            standbyButton.onClick.AddListener(() => { callback(2); selectedDefenseIndex = 2; defenseActionSelected = true; defenseSelectionPanel.SetActive(false); });
        }


        public void UpdateHpSlider(Slider hpSlider, int currentHP, int maxHP)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }

        public void UpdateMonsterName(TextMeshProUGUI nameText, string name)
        {
            nameText.text = name;
        }

        public void UpdateBattleUI(BattleTrainer trainer1, BattleTrainer trainer2)
        {
            UpdateHpSlider(trainer1HpSlider, trainer1.GetCurrentMonster().CurrentHP, trainer1.GetCurrentMonster().Data.MaxHP);
            trainer1NameText.text = trainer1.GetCurrentMonster().Nickname;

            UpdateHpSlider(trainer2HpSlider, trainer2.GetCurrentMonster().CurrentHP, trainer2.GetCurrentMonster().Data.MaxHP);
            trainer2NameText.text = trainer2.GetCurrentMonster().Nickname;
        }
        public void Log(string message)
        {
            logText.text += message + "\n";
        }
        public void LogReset()
        {
            logText.text ="";
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
            defenseActionSelected = true;
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

        public bool IsDefenseActionSelected()
        {
            return defenseActionSelected;
        }

        public int GetSelectedDefenseIndex()
        {
            defenseActionSelected = false;
            return selectedDefenseIndex;
        }
    }
}
