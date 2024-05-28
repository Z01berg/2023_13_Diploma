using UnityEngine;

public class OnOffInfo : MonoBehaviour
{
    [SerializeField] private GameObject Text;
    void Start()
    {
        EventSystem.ShowCheatEngine.AddListener(SwitchObject);
    }

    private void SwitchObject()
    {
        Text.SetActive(!Text.activeSelf);
    }
}
  