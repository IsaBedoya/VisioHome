using UnityEngine;
using UnityEngine.XR.Management;
using System.Collections;

public class XRLoaderController : MonoBehaviour
{
    public IEnumerator StartXR()
    {
        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
        }

        XRGeneralSettings.Instance.Manager.StartSubsystems();
    }

    public void StopXR()
    {
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
    }
}