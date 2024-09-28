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
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !vpressed)
        {
            vpressed = true;
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
        room.enemyInRoomList.Remove(this.gameObject); 
        Destroy(_gameObject);
        Destroy(_body);
        Debug.Log("killed EVENT BROO");
    }
}