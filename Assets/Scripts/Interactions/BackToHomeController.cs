using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BackToHomeController : MonoBehaviour
{
    public XRLoaderController xrLoader;

    private bool isGazing = false;

    private void OnEnable()
    {
        GazeManager.Instance.OnGazeSelection += HandleGazeComplete;
    }

    private void OnDisable()
    {
        if (GazeManager.Instance != null)
            GazeManager.Instance.OnGazeSelection -= HandleGazeComplete;
    }

    public void StartGaze()
    {
        isGazing = true;
    }

    public void StopGaze()
    {
        isGazing = false;
    }

    private void HandleGazeComplete()
    {
        if (!isGazing) return;

        StartCoroutine(StopXRAndReturnHome());
    }

    private IEnumerator StopXRAndReturnHome()
    {
        yield return StartCoroutine(xrLoader.StopXR());

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("Home");
    }
}