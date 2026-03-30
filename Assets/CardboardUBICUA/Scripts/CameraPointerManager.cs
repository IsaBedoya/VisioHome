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

    private readonly string interactableTag = "Interactable";
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

            GameObject hitObject = hit.transform.gameObject;

            // SOLO interactuar si es Interactable
            if (hit.transform.CompareTag(interactableTag))
            {
                if (_gazedObject != hitObject)
                {
                    // Salir del anterior
                    if (_gazedObject != null)
                    {
                        _gazedObject.SendMessage("OnPointerExitXR", null, SendMessageOptions.DontRequireReceiver);
                    }

                    // Nuevo objeto
                    _gazedObject = hitObject;
                    _gazedObject.SendMessage("OnPointerEnterXR", null, SendMessageOptions.DontRequireReceiver);

                    GazeManager.Instance.StartGazeSelection();
                }

                PointerOnGaze(hit.point);
            }
            else
            {
                ClearGaze();
            }
        }
        else
        {
            ClearGaze();
        }

        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            _gazedObject?.SendMessage("OnPointerClickXR", null, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void ClearGaze()
    {
        if (_gazedObject != null)
        {
            _gazedObject.SendMessage("OnPointerExitXR", null, SendMessageOptions.DontRequireReceiver);
            _gazedObject = null;
        }

        PointerOutGaze();
        GazeManager.Instance.CancelGazeSelection();
    }

    private void PointerOnGaze(Vector3 hitPoint)
    {
        float scaleFactor = scaleSize * Vector3.Distance(transform.position, hitPoint);

        pointerPrefab.transform.localScale = Vector3.one * scaleFactor;

        pointerPrefab.transform.parent.position =
            CalculatePointerPosition(transform.position, hitPoint, distancePointerObject);

        // 🔥 CLAVE: el pointer SIEMPRE mira como la cámara
        pointerPrefab.transform.parent.rotation = transform.rotation;
    }

    private void PointerOutGaze()
    {
        pointerPrefab.transform.localScale = Vector3.one * 0.1f;

        pointerPrefab.transform.parent.localPosition = new Vector3(0, 0, maxDistancePointer);
        pointerPrefab.transform.parent.rotation = transform.rotation;
    }

    private Vector3 CalculatePointerPosition(Vector3 p0, Vector3 p1, float t)
    {
        return new Vector3(
            p0.x + t * (p1.x - p0.x),
            p0.y + t * (p1.y - p0.y),
            p0.z + t * (p1.z - p0.z)
        );
    }
}