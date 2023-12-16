using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Card",menuName ="Cards")]
public class CardsSO : ScriptableObject
{
    public Sprite BG;
    public Sprite banner;

    public string Title;
    public string Description;
    public string Cost;
    public string Attack;
    public string Move;


}
