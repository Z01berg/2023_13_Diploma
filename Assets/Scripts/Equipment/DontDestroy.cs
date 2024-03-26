using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Publiczna klasa zapobiegajaca zniszczeniu objektu do kturego zostala ona przypisana
 * przy przechodzeniu miedzy scenami.
 * Objekt ten musi mieæ unikatowy w scenie tag, w innym przypadku inne objekty posiadajaca ten sam
 * tag zostana zniszczone przy wejsciu do sceny.
 */

public class DontDestroy : MonoBehaviour
{
    public bool active = true;
    private void Awake()
    {
        if (!active) return;

        GameObject[] objs = GameObject.FindGameObjectsWithTag(this.gameObject.tag);

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
