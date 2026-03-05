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
    }

    void OnEnterPressed()
    {
        apartmentCode = codeInput.value;
        isFurnished = furnishedToggle.value;
        useMinimap = minimapToggle.value;

        StartCoroutine(StartVRAndLoad());
    }

    IEnumerator StartVRAndLoad()
    {
        yield return StartCoroutine(xrLoader.StartXR());

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("Apartment_A");
    }

    void OnExitPressed()
    {
        Application.Quit();
    }

    void OnHelpPressed()
    {
        Debug.Log("Mostrar ayuda (aún no implementado)");
    }
}