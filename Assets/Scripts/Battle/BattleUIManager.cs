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

        public void InitializeUI()
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

            moveSelectionPanel.SetActive(false);
            defenseSelectionPanel.SetActive(false);
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

        private void SetMoveButton(Button button, Monster monster, int moveIndex, System.Action<int> callback)
        {
            if (moveIndex < monster.Data.Moves.Count)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = monster.Data.Moves[moveIndex].Name;
                button.gameObject.SetActive(true);
                button.onClick.AddListener(() => { callback(moveIndex); moveSelected = true; moveSelectionPanel.SetActive(false); });
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        }

        public void ShowDefenseSelectionUI(System.Action<int> callback)
        {
            defenseSelectionPanel.SetActive(true);
            dodgeButton.onClick.AddListener(() => { callback(0); defenseActionSelected = true; defenseSelectionPanel.SetActive(false); });
            braceButton.onClick.AddListener(() => { callback(1); defenseActionSelected = true; defenseSelectionPanel.SetActive(false); });
            standbyButton.onClick.AddListener(() => { callback(2); defenseActionSelected = true; defenseSelectionPanel.SetActive(false); });
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

        public void EndBattleUI()
        {
            if (battleUIInstance != null)
            {
                Destroy(battleUIInstance);
            }
        }
    }
}
