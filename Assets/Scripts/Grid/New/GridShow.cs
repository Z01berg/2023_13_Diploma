using UnityEngine;

/**
 *  Publiczna clasa GridShow jest wykorzystywana:
 *  - Na starcie dla odczytu wszystkich csv gridów na mapie
 *  - Update przy wciśnięcu przyciska "O" _isVisiblle = !_isVisible
 */

public class GridShow : MonoBehaviour
{
    private bool _isVisible = true;
    private Transform[] _children;

    void Start()
    {
        // Pobierz wszystkie dzieci obiektu Parenta
        _children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            _children[i] = transform.GetChild(i);
        }
    }

    void Update()
    {
        // Sprawdź czy został wciśnięty przycisk "O"
        if (Input.GetKeyDown(KeyCode.O))
        {
            _isVisible = !_isVisible;
            SetVisibility(_isVisible);
        }
    }

    // Metoda ustawiająca widoczność wszystkich dzieci
    void SetVisibility(bool visible)
    {
        foreach (Transform child in _children)
        {
            child.gameObject.SetActive(visible);
        }
    }

   
}