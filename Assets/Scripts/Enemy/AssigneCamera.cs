using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssigneCamera : MonoBehaviour
{
    [SerializeField] private Canvas canvas;  
    private Camera childCamera;
    
    void Start()
    {
        GameObject targetObject = GameObject.Find("Main Camera");

        if (targetObject != null)
        {
            childCamera = targetObject.GetComponent<Camera>();
            
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = childCamera;
        }
        else
        {
            Debug.LogError("TargetObjectName not found");
        }
    }

    
}
