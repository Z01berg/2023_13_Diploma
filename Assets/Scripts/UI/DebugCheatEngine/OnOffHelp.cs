using UnityEngine;

public class OnOffHelp : MonoBehaviour
{
    [SerializeField] private GameObject Text;
    void Start()
    {
        EventSystem.ShowHelpSheet.AddListener(SwitchObject);
    }

    private void SwitchObject()
    {
        Text.SetActive(!Text.activeSelf);
    }
}
