using UnityEngine;

public class GridShow : MonoBehaviour
{
    private bool isVisible = false;
    private Transform[] children;

    void Start()
    {
        // Pobierz wszystkie dzieci obiektu Parenta
        children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }
    }

    void Update()
    {
        // Sprawdź czy został wciśnięty przycisk "O"
        if (Input.GetKeyDown(KeyCode.O))
        {
            isVisible = !isVisible;
            SetVisibility(isVisible);
        }
    }

    // Metoda ustawiająca widoczność wszystkich dzieci
    void SetVisibility(bool visible)
    {
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(visible);
        }
    }

   
}