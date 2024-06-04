using UnityEngine;
using UnityEngine.UI;

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
            moveButton1.onClick.AddListener(() => OnMoveButtonClicked(0));
            moveButton2.onClick.AddListener(() => OnMoveButtonClicked(1));
            moveButton3.onClick.AddListener(() => OnMoveButtonClicked(2));
            moveButton4.onClick.AddListener(() => OnMoveButtonClicked(3));

            dodgeButton.onClick.AddListener(() => OnDefenseButtonClicked(0));
            braceButton.onClick.AddListener(() => OnDefenseButtonClicked(1));
            standbyButton.onClick.AddListener(() => OnDefenseButtonClicked(2));
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
        }

        public void ShowMoveSelectionUI(System.Action<int> callback)
        {
            moveSelectionPanel.SetActive(true);
            moveButton1.onClick.AddListener(() => { callback(0); moveSelected = true; HideMoveSelectionUI(); });
            moveButton2.onClick.AddListener(() => { callback(1); moveSelected = true; HideMoveSelectionUI(); });
            moveButton3.onClick.AddListener(() => { callback(2); moveSelected = true; HideMoveSelectionUI(); });
            moveButton4.onClick.AddListener(() => { callback(3); moveSelected = true; HideMoveSelectionUI(); });
        }

        public void ShowDefenseSelectionUI(System.Action<int> callback)
        {
            defenseSelectionPanel.SetActive(true);
            dodgeButton.onClick.AddListener(() => { callback(0); defenseActionSelected = true; HideDefenseSelectionUI(); });
            braceButton.onClick.AddListener(() => { callback(1); defenseActionSelected = true; HideDefenseSelectionUI(); });
            standbyButton.onClick.AddListener(() => { callback(2); defenseActionSelected = true; HideDefenseSelectionUI(); });
        }

        private void HideMoveSelectionUI()
        {
            moveSelectionPanel.SetActive(false);
        }

        private void HideDefenseSelectionUI()
        {
            defenseSelectionPanel.SetActive(false);
        }

        private void OnMoveButtonClicked(int index)
        {
            selectedMoveIndex = index;
            moveSelected = true;
            HideMoveSelectionUI();
        }

        private void OnDefenseButtonClicked(int index)
        {
            selectedDefenseIndex = index;
            defenseActionSelected = true;
            HideDefenseSelectionUI();
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
