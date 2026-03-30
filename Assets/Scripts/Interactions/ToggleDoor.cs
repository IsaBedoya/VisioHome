using UnityEngine;

public class ToggleDoor : MonoBehaviour
{
    public Transform doorHinge;
    public float openAngle = 90f;
    public float speed = 2f;

    private bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private bool isBeingLookedAt = false;

    void Start()
    {
        if (doorHinge == null)
            doorHinge = transform.parent;

        closedRotation = doorHinge.localRotation;

        Vector3 hingeAxis = doorHinge.up;
        openRotation = closedRotation * Quaternion.AngleAxis(openAngle, hingeAxis);

        GazeManager.Instance.OnGazeSelection += OnGazeComplete;
    }

    void OnDestroy()
    {
        GazeManager.Instance.OnGazeSelection -= OnGazeComplete;
    }

    void Update()
    {
        if (doorHinge == null) return;

        Quaternion targetRotation = isOpen ? openRotation : closedRotation;

        doorHinge.localRotation = Quaternion.Lerp(
            doorHinge.localRotation,
            targetRotation,
            Time.deltaTime * speed
        );
    }

    public void OnLookEnter()
    {
        isBeingLookedAt = true;
    }

    public void OnLookExit()
    {
        isBeingLookedAt = false;
    }

    private void OnGazeComplete()
    {
        if (isBeingLookedAt)
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        isOpen = !isOpen;
    }
}