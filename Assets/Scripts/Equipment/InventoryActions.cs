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
    private static bool _active;
    [SerializeField] private GameObject _inv;
    [SerializeField] private GameObject _minimap;
    [SerializeField] private GameObject _menuView;
    private Animator pauseMenuAnimator;

    private bool locked = false;

    private void Awake()
    {
        EventSystem.OpenCloseInventory.AddListener(ChangeInventoryState);
    }

    private void Start()
    {
        pauseMenuAnimator = _menuView.GetComponent<Animator>();
        _active = _inv.activeSelf;
    }

    private void ChangeInventoryState()
    {
        if (_active)
        {
            _inv.gameObject.SetActive(false);
            _active = false;
            _inv.GetComponent<UIInventory>().SaveState();
            EventSystem.HideHand?.Invoke(false);
            _minimap.gameObject.SetActive(true);
            pauseMenuAnimator.SetTrigger("Quit");
        }
        else
        {
            _inv.gameObject.SetActive(true);
            _active = true;
            EventSystem.HideHand?.Invoke(true);
            _minimap.gameObject.SetActive(false);
            pauseMenuAnimator.SetTrigger("Inventory");
        }
    }
    
    public void LockInventory()
    {
        locked = !locked;
        _active = false;
        _inv.gameObject.SetActive(false);
    }
    
    public static bool getActive()
    {
        return _active;
    }
}
