using UnityEngine;
using UnityEngine.Events;
public class TeleportPoint : MonoBehaviour
{
    public UnityEvent OnTeleportEnter;
    public UnityEvent OnTeleport;
    public UnityEvent OnTeleportExit;

    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnPointerEnterXR()
    {
        OnTeleportEnter?.Invoke();
    }
    public void OnPointerClickXR()
    {
        ExecuteTeleportation();

        OnTeleport?.Invoke();
        TeleportManager.Instance.DisableTeleportPoint(gameObject);
    }
    public void OnPointerExitXR()
    {
        OnTeleportExit?.Invoke();
    }

    private void ExecuteTeleportation()
    {
        GameObject player = TeleportManager.Instance.Player;
        player.transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        Camera camera = player.GetComponentInChildren<Camera>();
        float rotY = transform.eulerAngles.y - camera.transform.localEulerAngles.y;
        player.transform.rotation = Quaternion.Euler(0, rotY, 0);
    }
}
