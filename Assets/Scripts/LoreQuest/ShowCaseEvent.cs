using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowCaseEvent : MonoBehaviour
{
    [HideInInspector] public InstantiatedRoom room;
    
    private bool vpressed = false;
    private bool skipped = false;
    private bool is_triggered = false;
    
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
        if (Input.GetKeyDown(KeyCode.B) && !vpressed)
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
}