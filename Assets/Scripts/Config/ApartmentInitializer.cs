using UnityEngine;

public class ApartmentInitializer : MonoBehaviour
{
    [SerializeField] private GameObject basicDeliveryObjects;

    private void Start()
    {
        if (basicDeliveryObjects == null)
        {
            Debug.LogWarning("Basic delivery objects are not assigned.");
            return;
        }

        basicDeliveryObjects.SetActive(MainMenuController.isBasicDelivery);
    }
}