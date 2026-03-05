using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    public UIDocument uiDocument;

    private TextField codeInput;
    private Toggle furnishedToggle;
    private Toggle minimapToggle;

    private Button enterBtn;
    private Button exitBtn;
    private Button helpBtn;

    public XRLoaderController xrLoader;

    public static bool isFurnished;
    public static bool useMinimap;
    public static string apartmentCode;

    private VisualElement errorModal;
    private Label errorText;
    private Button errorCloseBtn;

    void Start()
    {
        var root = uiDocument.rootVisualElement;

        if (xrLoader == null)
        {
            Debug.LogError("XRLoader no asignado");
            return;
        }

        codeInput = root.Q<TextField>("CodeInput");

        furnishedToggle = root.Q<Toggle>("ModeToggle");
        furnishedToggle.value = true;

        minimapToggle = root.Q<Toggle>("MinimapToggle");

        enterBtn = root.Q<Button>("EnterBtn");
        exitBtn = root.Q<Button>("ExitBtn");
        helpBtn = root.Q<Button>("HelpBtn");

        enterBtn.clicked += OnEnterPressed;
        exitBtn.clicked += OnExitPressed;
        helpBtn.clicked += OnHelpPressed;

        errorModal = root.Q<VisualElement>("ErrorModalOverlay");
        errorText = root.Q<Label>("ErrorText");
        errorCloseBtn = root.Q<Button>("ErrorCloseBtn");

        errorCloseBtn.clicked += HideError;

        errorModal.RegisterCallback<ClickEvent>(evt =>
        {
            HideError();
        });
        
        var modalBox = root.Q<VisualElement>("ErrorModal");

        modalBox.RegisterCallback<ClickEvent>(evt =>
        {
            evt.StopPropagation();
        });
    }

    void OnEnterPressed()
    {
        apartmentCode = codeInput.value.Trim().ToUpper();

        if (string.IsNullOrEmpty(apartmentCode))
        {
            ShowError("Debe ingresar un código de apartamento.");
            return;
        }

        isFurnished = furnishedToggle.value;
        useMinimap = minimapToggle.value;

        if (!SceneExists(apartmentCode))
        {
            ShowError($"El apartamento '{apartmentCode}' no existe.");
            return;
        }

        StartCoroutine(StartVRAndLoad());
    }

    IEnumerator StartVRAndLoad()
    {
        yield return StartCoroutine(xrLoader.StartXR());

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(apartmentCode);
    }

    bool SceneExists(string sceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);

            if (name == sceneName)
                return true;
        }

        return false;
    }

    void OnExitPressed()
    {
        Application.Quit();
    }

    void OnHelpPressed()
    {
        Debug.Log("Mostrar ayuda (aún no implementado)");
    }

    void ShowError(string message)
    {
        errorText.text = message;
        errorModal.style.display = DisplayStyle.Flex;
    }

    void HideError()
    {
        errorModal.style.display = DisplayStyle.None;
    }
}