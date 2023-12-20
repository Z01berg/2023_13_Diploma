using UnityEngine;

public class Position : MonoBehaviour
{
    [SerializeField] private Transform bohater; 
    [SerializeField] private Transform timer;
    [SerializeField] private float x = 0.5f;
    [SerializeField] private float y = -0.2f;

    void Update()
    {
        if (bohater != null && timer != null)
        {
            Vector2 position = (Vector2)bohater.position + new Vector2(x, y); 
            timer.position = position;
        }
        else
        {
            Debug.LogError("Brak przypisanych obiektów!");
        }
    }
}