using Grid.New;
using UnityEngine;

/**
 * Publiczna clasa Position jest wyłącznie dla kosmetycznego ustalenia obiektów w grze
 * 
 * pobiera:
 * - xy Obiektu A
 * - xy Obiektu B
 * 
 * predefinuje:
 * - x
 * - y
 * 
 * po czym ustala Obiekt B do Obiektu A zgodnie z formułą
 * xy Obiekt A + xy predefiniowanymi parametrami.
 */
public class Position : MonoBehaviour
{
    
    [SerializeField] private Transform _bohater; 
    [SerializeField] private Transform _timer;
    [SerializeField] private float _x = 0.5f;
    [SerializeField] private float _y = -0.2f;


    void Update()
    {
        if (_bohater != null && _timer != null)
        {
            Vector2 position = (Vector2)_bohater.position + new Vector2(_x, _y); 
            _timer.position = position;
        }
        else
        {
            Debug.LogError("Brak przypisanych obiektów!");
        }
    }
}