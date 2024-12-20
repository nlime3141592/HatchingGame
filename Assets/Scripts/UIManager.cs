using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static Text s_ui_DebugText;
    public static RectTransform s_ui_Menu;
    public static Text s_ui_HatchingTimer;
    public static Text s_ui_ManagingButton;
    public static Image s_ui_ManagingBar;
    public static Button s_ui_StageButton;
    public static RectTransform s_ui_Option;
    public static RectTransform s_ui_History;
    public static RectTransform s_ui_Stage;
    public static RectTransform s_ui_HatchingSuccess;
    public static RectTransform s_ui_HatchingFailure;

    [Header("UI Canvas")]
    public Text ui_DebugText;
    public RectTransform ui_Menu;
    public Text ui_HatchingTimer;
    public Text ui_ManagingButton;
    public Image ui_ManagingBar;
    public Button ui_StageButton;
    public RectTransform ui_Option;
    public RectTransform ui_History;
    public RectTransform ui_Stage;
    public RectTransform ui_HatchingSuccess;
    public RectTransform ui_HatchingFailure;

    private void Awake()
    {
        s_ui_DebugText = ui_DebugText;
        s_ui_Menu = ui_Menu;
        s_ui_HatchingTimer = ui_HatchingTimer;
        s_ui_ManagingButton = ui_ManagingButton;
        s_ui_ManagingBar = ui_ManagingBar;
        s_ui_StageButton = ui_StageButton;
        s_ui_Option = ui_Option;
        s_ui_History = ui_History;
        s_ui_Stage = ui_Stage;
        s_ui_HatchingSuccess = ui_HatchingSuccess;
        s_ui_HatchingFailure = ui_HatchingFailure;
    }

    private void Start()
    {
        s_ui_Menu.gameObject.SetActive(true);
        s_ui_Option.gameObject.SetActive(false);
        s_ui_History.gameObject.SetActive(false);
        s_ui_Stage.gameObject.SetActive(false);
        s_ui_HatchingSuccess.gameObject.SetActive(false);
        s_ui_HatchingFailure.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        HatchingStatus currentStatus = GameManager.s_currentHatchingStatus;
        
        switch (currentStatus)
        {
            case HatchingStatus.Hatching:
                long end = GameManager.s_currentHatchingDataOrNull.hatchingEndTime;
                long cur = GameManager.s_currentTimestamp;
                long leftTime = end - cur;
                long hours = leftTime / 3600;
                long minutes = leftTime % 3600 / 60;
                long seconds = leftTime % 60;
                string leftTimeString = "";
                leftTimeString += string.Format("{0:d02}", hours);
                leftTimeString += ":";
                leftTimeString += string.Format("{0:d02}", minutes);
                leftTimeString += ":";
                leftTimeString += string.Format("{0:d02}", seconds);
                s_ui_HatchingTimer.gameObject.SetActive(true);
                s_ui_HatchingTimer.text = string.Format("Hatching After\n{0}", leftTimeString);
                // s_ui_ManagingButton.text = "包府";
                s_ui_ManagingButton.transform.parent.gameObject.SetActive(true);
                s_ui_ManagingBar.transform.parent.gameObject.SetActive(true);
                long managingCurrentTime = GameManager.s_currentTimestamp;
                long managingEndTime = GameManager.s_currentHatchingDataOrNull.managingEndTime;
                long managingRequireTime = GameManager.s_currentHatchingDataOrNull.managingRequireTime;
                s_ui_ManagingBar.fillAmount = (float)(managingEndTime - managingCurrentTime) / (float)managingRequireTime;
                s_ui_StageButton.interactable = false;
                break;
            case HatchingStatus.HatchingSuccess:
                s_ui_HatchingTimer.gameObject.SetActive(true);
                s_ui_HatchingTimer.text = string.Format("Hatching Available !");
                // s_ui_ManagingButton.text = "何拳";
                s_ui_ManagingButton.transform.parent.gameObject.SetActive(true);
                s_ui_ManagingBar.transform.parent.gameObject.SetActive(false);
                s_ui_StageButton.interactable = false;
                break;
            case HatchingStatus.HatchingFailure:
                s_ui_HatchingTimer.gameObject.SetActive(true);
                s_ui_HatchingTimer.text = string.Format("Egg Dead...");
                // s_ui_ManagingButton.text = "局档窍扁";
                s_ui_ManagingButton.transform.parent.gameObject.SetActive(true);
                s_ui_ManagingBar.transform.parent.gameObject.SetActive(false);
                s_ui_StageButton.interactable = false;
                break;
            default:
                s_ui_HatchingTimer.gameObject.SetActive(false);
                s_ui_ManagingButton.transform.parent.gameObject.SetActive(false);
                s_ui_ManagingBar.transform.parent.gameObject.SetActive(false);
                s_ui_StageButton.interactable = true;
                break;
        }
    }

    public void OnButtonClick_OpenOption()
    {
        s_ui_Option.gameObject.SetActive(true);
        s_ui_Menu.gameObject.SetActive(false);
    }

    public void OnButtonClick_CloseOption()
    {
        s_ui_Option.gameObject.SetActive(false);
        s_ui_Menu.gameObject.SetActive(true);
    }

    public void OnButtonClick_OpenHistory()
    {
        s_ui_History.gameObject.SetActive(true);
        s_ui_Menu.gameObject.SetActive(false);
    }

    public void OnButtonClick_CloseHistory()
    {
        s_ui_History.gameObject.SetActive(false);
        s_ui_Menu.gameObject.SetActive(true);
    }

    public void OnButtonClick_OpenStage()
    {
        s_ui_Stage.gameObject.SetActive(true);
        s_ui_Menu.gameObject.SetActive(false);
    }

    public void OnButtonClick_CloseStage()
    {
        s_ui_Stage.gameObject.SetActive(false);
        s_ui_Menu.gameObject.SetActive(true);
    }

    public void OnButtonClick_CloseHatchingSuccess()
    {
        s_ui_HatchingSuccess.gameObject.SetActive(false);
        s_ui_Menu.gameObject.SetActive(true);
    }

    public void OnButtonClick_CloseHatchingFailure()
    {
        s_ui_HatchingFailure.gameObject.SetActive(false);
        s_ui_Menu.gameObject.SetActive(true);
    }

    public void OnButtonClick_Managing()
    {
        HatchingStatus currentStatus = GameManager.s_currentHatchingStatus;
        long currentTimestamp = GameManager.s_currentTimestamp;

        switch (currentStatus)
        {
            case HatchingStatus.Hatching:
                GameManager.ManagingEgg();
                break;
            case HatchingStatus.HatchingSuccess:
                GameManager.s_currentHatchingStatus = HatchingStatus.Hatched;
                s_ui_HatchingSuccess.gameObject.SetActive(true);
                s_ui_Menu.gameObject.SetActive(false);
                break;
            case HatchingStatus.HatchingFailure:
                GameManager.s_currentHatchingStatus = HatchingStatus.NotHatched;
                s_ui_HatchingFailure.gameObject.SetActive(true);
                s_ui_Menu.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void OnButtonClick_SelectStage1()
    {
        OnButtonClick_SelectStage(0);
    }

    public void OnButtonClick_SelectStage2()
    {
        OnButtonClick_SelectStage(1);
    }

    public void OnButtonClick_SelectStage3()
    {
        OnButtonClick_SelectStage(2);
    }

    private void OnButtonClick_SelectStage(int index)
    {
        HatchingDataSO dataSO = GameManager.s_hatchingDataSO[index];
        GameManager.s_currentHatchingDataSO = GameObject.Instantiate(dataSO) as HatchingDataSO;
        GameManager.s_currentHatchingDataOrNull = GameManager.s_currentHatchingDataSO.hatchingData;
        // HatchingData data = new HatchingData(dataSO.hatchingData);
        GameManager.s_currentHatchingDataOrNull.hatchingEndTime = GameManager.s_currentTimestamp + GameManager.s_currentHatchingDataOrNull.hatchingRequireTime;
        GameManager.s_currentHatchingDataOrNull.managingEndTime = GameManager.s_currentTimestamp + GameManager.s_currentHatchingDataOrNull.managingRequireTime;
        GameManager.s_currentHatchingDataOrNull.hatchingStatus = HatchingStatus.Hatching;

        // data.hatchingStatus = HatchingStatus.Hatching;
        // GameManager.s_hatchingFile.HatchingDatas.Add(data);

        s_ui_Stage.gameObject.SetActive(false);
        s_ui_Menu.gameObject.SetActive(true);
    }
}