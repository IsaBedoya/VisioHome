using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        if (Instance == null)
        {
            GameObject go = new GameObject("SceneLoader");
            Instance = go.AddComponent<SceneLoader>();
            DontDestroyOnLoad(go);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadHome()
    {
        StartCoroutine(LoadHomeRoutine());
    }

    private IEnumerator LoadHomeRoutine()
    {
        // 1. Detener XR completamente
        if (UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.isInitializationComplete)
        {
            UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.StopSubsystems();
            UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        }

        // 2. Esperar un frame real
        yield return null;
        yield return new WaitForSeconds(0.2f);

        // 3. Cargar escena
        SceneManager.LoadScene("Home", LoadSceneMode.Single);
    }
}