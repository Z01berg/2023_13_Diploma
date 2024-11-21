using UnityEngine;
using TMPro;

/**
 * Public class TimerData encapsulates the data for each timer.
 */
public class TimerData
{
    public int Value { get; set; } // znaczenie timera
    public string Tag { get; set; } // znaczenie TAG
    public GameObject HP { get; set; } // slider przyczepiony do obiektu do którego nalezy timer i hp
    public int EnemyId { get; set; }
    public TMP_Text Text { get; set; }
    public string Id { get; set; } 

    public TimerData(int value, string tag, GameObject hp, int enemyId, TMP_Text text, string id)
    {
        Value = value;
        Tag = tag;
        HP = hp;
        EnemyId = enemyId;
        Text = text;
        Id = id;
    }

    public void UpdateValue(int change)
    {
        Value = Mathf.Clamp(Value + change, 0, 99);
    }
}