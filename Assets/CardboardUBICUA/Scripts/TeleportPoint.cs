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

        Vector3 currentPlayerPosition = player.transform.position;
        Vector3 currentPlayerRotation = player.transform.eulerAngles;

        player.transform.position = new Vector3(
            transform.position.x,
            currentPlayerPosition.y,
            transform.position.z
        );

        player.transform.rotation = Quaternion.Euler(currentPlayerRotation);
    }
}
