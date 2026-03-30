using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BackToHomeController : MonoBehaviour
{
    public XRLoaderController xrLoader;

    private bool isGazing = false;
    private float lastGazeTime;

    private bool isBeingLookedAt = false;

    public void OnLookEnter() => isBeingLookedAt = true;
    public void OnLookExit() => isBeingLookedAt = false;

    private void OnEnable()
    {
        // Suscribirse solo si GazeManager ya existe
        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.OnGazeSelection += HandleGazeComplete;
        }
        else
        {
            // Esperar hasta que se cree
            StartCoroutine(WaitForGazeManager());
        }
    }

    private IEnumerator WaitForGazeManager()
    {
        // Esperar hasta que exista
        while (GazeManager.Instance == null)
            yield return null;

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
        lastGazeTime = Time.time;
    }

    public void StopGaze()
    {
        isGazing = false;
    }

    private IEnumerator GoHome()
    {
        yield return xrLoader.StopXR();
        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene("Home", LoadSceneMode.Single);
    }

    private void HandleGazeComplete()
    {
        if (!isBeingLookedAt)
        {
            return;
        }

        if (SceneLoader.Instance == null)
        {
            return;
        }

        SceneLoader.Instance.LoadHome();
    }

    private IEnumerator StopXRAndReturnHome()
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("Home");
    }
}