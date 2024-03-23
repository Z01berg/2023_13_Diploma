using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class keeps the object it was assigned to alive while switching scenes.
 * For it to work properly the game object needs to have a unique tag, otherwise
 * it will delete all but one of the objects of the same tag.
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
