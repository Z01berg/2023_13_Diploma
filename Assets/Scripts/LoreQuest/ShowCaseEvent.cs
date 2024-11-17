using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Publiczna klasa ShowCaseEvent dziedzicząca po klasie MonoBehaviour
 *
 * Klasa odpowiedzialna za wyświetlanie informacji o obiektach w grze
 * oraz za wyświetlanie powiadomień w grze
 *
 */

public class ShowCaseEvent : MonoBehaviour
{
    [HideInInspector] public InstantiatedRoom room;
    
    private bool vpressed = false;
    private bool skipped = false;
    private bool is_triggered = false;
    [NonSerialized] public bool _inRange = false;
    
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private GameObject _body;


    private void Start()
    {
        EventSystem.SkipedText.AddListener(ChangeBool);
    }
    
    private void ChangeBool(bool skipper)
    {
        skipped = skipper;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !vpressed && _inRange)
        {
            vpressed = true;
            EventSystem.HideHand?.Invoke(vpressed);
            is_triggered = !is_triggered;
            ToastNotification.Show(
                "Yeah, a simple Key can display a message. And this message doens't have a \"timer\" display render",
                "Clerick");
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0) && is_triggered)
        {
            EventSystem.SkipText.Invoke();
            
            if (skipped)
            {
                ToastNotification.Hide();
                Kill();
            }
            
            skipped = !skipped;
        }
        
    }
    
    private void Kill()
    {
        vpressed = !vpressed;
        EventSystem.HideHand?.Invoke(vpressed);
        room.enemyInRoomList.Remove(_gameObject); 
        Destroy(_gameObject);
        Destroy(_body);
        Debug.Log("killed EVENT BROO");
        Debug.Log(room.enemyInRoomList.Count);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}