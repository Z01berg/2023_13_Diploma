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

public class ShowCaseTutorial : MonoBehaviour
{
    [HideInInspector] public InstantiatedRoom room;
    
    private bool vpressed = false;
    private bool skipped = false;
    private bool is_triggered = false;


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
        if (Input.GetKeyDown(KeyCode.B))
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
        Debug.Log(room.enemyInRoomList.Count);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}