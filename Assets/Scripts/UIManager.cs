using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static RectTransform s_ui_Menu;
    public static RectTransform s_ui_Option;
    public static RectTransform s_ui_History;
    public static RectTransform s_ui_Stage;
    public static RectTransform s_ui_HatchingSuccess;
    public static RectTransform s_ui_HatchingFailure;

    [Header("UI Canvas")]
    public RectTransform ui_Menu;
    public RectTransform ui_Option;
    public RectTransform ui_History;
    public RectTransform ui_Stage;
    public RectTransform ui_HatchingSuccess;
    public RectTransform ui_HatchingFailure;

    private void Awake()
    {
        s_ui_Menu = ui_Menu;
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

    public void OnButtonClick_OpenOption()
    {
        s_ui_Menu.gameObject.SetActive(false);
        s_ui_Option.gameObject.SetActive(true);
    }

    public void OnButtonClick_CloseOption()
    {
        s_ui_Option.gameObject.SetActive(false);
        s_ui_Menu.gameObject.SetActive(true);
    }

    public void OnButtonClick_OpenHistory()
    {
        s_ui_Menu.gameObject.SetActive(false);
        s_ui_History.gameObject.SetActive(true);
    }

    public void OnButtonClick_CloseHistory()
    {
        s_ui_History.gameObject.SetActive(false);
        s_ui_Menu.gameObject.SetActive(true);
    }

    public void OnButtonClick_OpenStage()
    {
        s_ui_Menu.gameObject.SetActive(false);
        s_ui_Stage.gameObject.SetActive(true);
    }

    public void OnButtonClick_CloseStage()
    {
        s_ui_Stage.gameObject.SetActive(false);
        s_ui_Menu.gameObject.SetActive(true);
    }

    public void OnButtonClick_CloseHatchingSuccess()
    {
        s_ui_HatchingSuccess.gameObject.SetActive(false);
    }

    public void OnButtonClick_CloseHatchingFailure()
    {
        s_ui_HatchingFailure.gameObject.SetActive(false);
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
                break;
            case HatchingStatus.HatchingFailure:
                GameManager.s_currentHatchingStatus = HatchingStatus.NotHatched;
                s_ui_HatchingFailure.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}