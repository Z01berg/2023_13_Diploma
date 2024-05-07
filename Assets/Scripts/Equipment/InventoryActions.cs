using UnityEngine;

/**
 * Klasa steruje zachowaniem okna ekwipunku.
 * Po nacisnieciu "E" na klawiaturze, funkcja Update() odpowiednio aktywuje
 * lub dezaktywuje okno.
 * 
 * _active - utrzymuje aktualne informacji o widocznosci okna ekwipunku.
 * _inv - przechowuje objekt UIInventory
 * 
 * Publiczna funkcja LockInventory() pozwala na zablokowanie badz odblokowanie funkcji 
 * zmiany widocznosci okna ekwipunku.
 * 
 * _locked - przypisana mu wartosc mowi o tym czy zmiana wyswietlania ekwipunku zostala zablokowana
 */

public class InventoryActions : MonoBehaviour
{
    private bool _active;
    [SerializeField] private GameObject _inv;

    private bool locked = false;

    private void Start()
    {
        _active = _inv.activeSelf;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !locked)
        {
            if(_active)
            {
                EventSystem.DisableHand?.Invoke();
                _inv.gameObject.SetActive(false);
                _active = false;
                _inv.GetComponent<UIInventory>().SaveState();
            }
            else
            {
                EventSystem.DisableHand?.Invoke();
                _inv.gameObject.SetActive(true);
                _active=true;
            }
        }
    }

    public void LockInventory()
    {
        locked = !locked;
        _active = false;
        _inv.gameObject.SetActive(false);
    }
}
