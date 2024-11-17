using UnityEngine;
using TMPro;

public class TimerData
{
    public int Value;
    public string Tag;
    public GameObject HP;
    public int EnemyId;

    public TMP_Text Text;
    public string Id;

    public TimerData(int value, string tag, GameObject hp, int enemyId, TMP_Text text = null, string id = null)
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