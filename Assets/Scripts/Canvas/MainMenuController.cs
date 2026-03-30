using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    public UIDocument uiDocument;
    public XRLoaderController xrLoader;

    private TextField codeInput;

    private Toggle blackWorkToggle;
    private Toggle basicDeliveryToggle;

    private Button enterBtn;
    private Button exitBtn;
    private Button helpBtn;

    public static bool isBlackWork;
    public static bool isBasicDelivery;
    public static string apartmentCode;

    private VisualElement errorModal;
    private Label errorText;
    private Button errorCloseBtn;

    private bool isUpdatingToggles;

    private Button helpVideoBtn;
    private Label helpTitle;
    private Label helpDescription;
    private Label creditsText;

    private void Start()
    {
        var root = uiDocument.rootVisualElement;

        if (xrLoader == null)
        {
            Debug.LogError("XRLoaderController is not assigned.");
            return;
        }

        codeInput = root.Q<TextField>("CodeInput");

        blackWorkToggle = root.Q<Toggle>("BlackWorkToggle");
        basicDeliveryToggle = root.Q<Toggle>("BasicDeliveryToggle");

        enterBtn = root.Q<Button>("EnterBtn");
        exitBtn = root.Q<Button>("ExitBtn");
        helpBtn = root.Q<Button>("HelpBtn");

        errorModal = root.Q<VisualElement>("ErrorModalOverlay");
        errorText = root.Q<Label>("ErrorText");
        errorCloseBtn = root.Q<Button>("ErrorCloseBtn");

        helpVideoBtn = root.Q<Button>("HelpVideoBtn");
        helpTitle = root.Q<Label>("HelpTitle");
        helpDescription = root.Q<Label>("HelpDescription");
        creditsText = root.Q<Label>("CreditsText");

        helpVideoBtn.clicked += OpenTutorialVideo;

        if (codeInput == null ||
            blackWorkToggle == null ||
            basicDeliveryToggle == null ||
            enterBtn == null ||
            exitBtn == null ||
            helpBtn == null ||
            errorModal == null ||
            errorText == null ||
            errorCloseBtn == null)
        {
            Debug.LogError("One or more UI elements were not found. Check the UXML names.");
            return;
        }

        SetDeliveryMode(true);

        blackWorkToggle.RegisterValueChangedCallback(OnBlackWorkToggleChanged);
        basicDeliveryToggle.RegisterValueChangedCallback(OnBasicDeliveryToggleChanged);

        enterBtn.clicked += OnEnterPressed;
        exitBtn.clicked += OnExitPressed;
        helpBtn.clicked += OnHelpPressed;
        errorCloseBtn.clicked += HideError;

        errorModal.RegisterCallback<ClickEvent>(_ =>
        {
            HideError();
        });

        var modalBox = root.Q<VisualElement>("ErrorModal");
        if (modalBox != null)
        {
            modalBox.RegisterCallback<ClickEvent>(evt =>
            {
                evt.StopPropagation();
            });
        }
    }

    private void OnBlackWorkToggleChanged(ChangeEvent<bool> evt)
    {
        if (isUpdatingToggles)
        {
            return;
        }

        if (evt.newValue)
        {
            SetDeliveryMode(true);
        }
        else if (!basicDeliveryToggle.value)
        {
            SetDeliveryMode(true);
        }
    }

    private void OnBasicDeliveryToggleChanged(ChangeEvent<bool> evt)
    {
        if (isUpdatingToggles)
        {
            return;
        }

        if (evt.newValue)
        {
            SetDeliveryMode(false);
        }
        else if (!blackWorkToggle.value)
        {
            SetDeliveryMode(false);
        }
    }

    private void SetDeliveryMode(bool blackWorkSelected)
    {
        isUpdatingToggles = true;

        blackWorkToggle.SetValueWithoutNotify(blackWorkSelected);
        basicDeliveryToggle.SetValueWithoutNotify(!blackWorkSelected);

        isBlackWork = blackWorkSelected;
        isBasicDelivery = !blackWorkSelected;

        isUpdatingToggles = false;
    }

    private void OnEnterPressed()
    {
        apartmentCode = codeInput.value.Trim().ToUpper();

        if (string.IsNullOrEmpty(apartmentCode))
        {
            ShowError("Debe ingresar un código de apartamento.");
            return;
        }

        isBlackWork = blackWorkToggle.value;
        isBasicDelivery = basicDeliveryToggle.value;

        if (!SceneExists(apartmentCode))
        {
            ShowError($"El apartamento '{apartmentCode}' no existe.");
            return;
        }

        StartCoroutine(StartXRAndLoadScene());
    }

    private IEnumerator StartXRAndLoadScene()
    {
        yield return StartCoroutine(xrLoader.StartXR());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(apartmentCode);
    }

    private bool SceneExists(string sceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);

            if (name == sceneName)
            {
                return true;
            }
        }

        return false;
    }

    private void OnExitPressed()
    {
        Application.Quit();
    }

    private void OnHelpPressed()
    {
        errorModal.style.display = DisplayStyle.Flex;

        errorText.text = "";

        helpTitle.style.display = DisplayStyle.Flex;
        helpDescription.style.display = DisplayStyle.Flex;
        helpVideoBtn.style.display = DisplayStyle.Flex;
        creditsText.style.display = DisplayStyle.Flex;
    }

    private void ShowError(string message)
    {
        errorText.text = message;
        errorModal.style.display = DisplayStyle.Flex;
        helpTitle.style.display = DisplayStyle.None;
        helpDescription.style.display = DisplayStyle.None;
        helpVideoBtn.style.display = DisplayStyle.None;
        creditsText.style.display = DisplayStyle.None;
    }

    private void HideError()
    {
        errorModal.style.display = DisplayStyle.None;
    }

    private void OpenTutorialVideo()
    {
        Application.OpenURL("https://youtu.be/B01e0sbF6Sk");
    }
}