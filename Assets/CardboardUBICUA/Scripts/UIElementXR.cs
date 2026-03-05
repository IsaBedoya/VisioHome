using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class UIElementXR : MonoBehaviour
{
    public UnityEvent OnXRPointerEnter;
    public UnityEvent OnXRPointerExit;

    private Camera xRCamera;

    void Start()
    {
        xRCamera = CameraPointerManager.Instance.gameObject.GetComponent<Camera>();
    }

    public void OnPointerClickXR()
    {
        PointerEventData pointerEvent = PlacePointer();
        ExecuteEvents.Execute(gameObject, pointerEvent, ExecuteEvents.pointerClickHandler);
    }

    public void OnPointerEnterXR()
    {
        GazeManager.Instance.SetUpGaze(1.5f);
        OnXRPointerEnter?.Invoke();

        PointerEventData pointerEvent = PlacePointer();
        ExecuteEvents.Execute(gameObject, pointerEvent, ExecuteEvents.pointerDownHandler);
    }

    public void OnPointerExitXR()
    {
        GazeManager.Instance.SetUpGaze(2.5f);
        OnXRPointerExit?.Invoke();

        PointerEventData pointerEvent = PlacePointer();
        ExecuteEvents.Execute(gameObject, pointerEvent, ExecuteEvents.pointerUpHandler);
    }

    private PointerEventData PlacePointer()
    {
        Vector3 screePos = xRCamera.WorldToScreenPoint(CameraPointerManager.Instance.hitPoint);
        PointerEventData pointerEvent = new PointerEventData(EventSystem.current);
        pointerEvent.position = new Vector2(screePos.x, screePos.y);
        return pointerEvent;
    }
}
