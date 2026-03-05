using UnityEngine;

public class ApartmentInitializer : MonoBehaviour
{
    public GameObject furnishedObjects;

    void Start()
    {
        furnishedObjects.SetActive(MainMenuController.isFurnished);
    }
}