using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    public TMP_InputField codeInput;
    public UnityEngine.UI.Toggle furnishedToggle;
    public XRLoaderController xrLoader;

    public static bool isFurnished;
    public static string apartmentCode;

    public void OnEnterPressed()
    {
        apartmentCode = codeInput.text;
        isFurnished = furnishedToggle.isOn;

        StartCoroutine(StartVRAndLoad());
    }

    IEnumerator StartVRAndLoad()
    {
        yield return StartCoroutine(xrLoader.StartXR());

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("Apartment_A");
    }

    public void OnExitPressed()
    {
        Application.Quit();
    }
}