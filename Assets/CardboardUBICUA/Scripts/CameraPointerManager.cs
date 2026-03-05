using UnityEngine;

public class CameraPointerManager : MonoBehaviour
{
    public static CameraPointerManager Instance;
    [SerializeField] private GameObject pointerPrefab;
    [SerializeField] private float maxDistancePointer = 4.5f;
    [Range(0, 1)][SerializeField] private float distancePointerObject = 0.95f;
    [HideInInspector] public Vector3 hitPoint;

    private const float MaxDistance = 10.0f;
    private GameObject _gazedObject = null;
    private readonly string interctableTag = "Interactable";
    private float scaleSize = 0.025f;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        GazeManager.Instance.OnGazeSelection += GazeSelection;
    }
    void GazeSelection()
    {
        _gazedObject?.SendMessage("OnPointerClickXR", null, SendMessageOptions.DontRequireReceiver);
    }
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, MaxDistance))
        {
            hitPoint = hit.point;
            if (_gazedObject != hit.transform.gameObject)
            {
                _gazedObject?.SendMessage("OnPointerExitXR", null, SendMessageOptions.DontRequireReceiver);
                _gazedObject = hit.transform.gameObject;
                _gazedObject.SendMessage("OnPointerEnterXR", null, SendMessageOptions.DontRequireReceiver);

                GazeManager.Instance.StartGazeSelection();
            }
            if (hit.transform.CompareTag(interctableTag))
            {
                PointerOnGaze(hit.point);
            }
            else
            {
                PointerOutGaze();
            }
        }
        else
        {
            _gazedObject?.SendMessage("OnPointerExitXR", null, SendMessageOptions.DontRequireReceiver);
            _gazedObject = null;
        }
        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            _gazedObject?.SendMessage("OnPointerClickXR", null, SendMessageOptions.DontRequireReceiver);
        }
    }
    private void PointerOnGaze(Vector3 hitPoint)
    {
        float scaleFactor = scaleSize * Vector3.Distance(transform.position, hitPoint);
        pointerPrefab.transform.localScale = Vector3.one * scaleFactor;
        pointerPrefab.transform.parent.position = CalculatePointerPosition(transform.position, hitPoint, distancePointerObject);
    }
    private void PointerOutGaze()
    {
        pointerPrefab.transform.localScale = Vector3.one * 0.1f;
        pointerPrefab.transform.parent.transform.localPosition = new Vector3(0, 0, maxDistancePointer);
        pointerPrefab.transform.parent.transform.rotation = transform.rotation;
        GazeManager.Instance.CancelGazeSelection();
    }
    private Vector3 CalculatePointerPosition(Vector3 p0, Vector3 p1, float t)
    {
        float x = p0.x + t * (p1.x - p0.x); ;
        float y = p0.y + t * (p1.y - p0.y);
        float z = p0.z + t * (p1.z - p0.z);

        return new Vector3(x, y, z);
    }
}
