using UnityEngine;

public class ApartmentInitializer : MonoBehaviour
{
    [Header("Basic Delivery Objects")]
    [SerializeField] private GameObject[] basicObjects;

    private void Start()
    {
        bool isBasic = MainMenuController.isBasicDelivery;

        foreach (GameObject obj in basicObjects)
        {
            if (obj != null)
                obj.SetActive(isBasic);
        }
    }
}