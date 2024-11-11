using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private GameObject _overlayContainer;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    

    private void Awake()
    {
        
    }

    public void SetMinimap()
    {
        minX = 0;
        maxX = 0;
        minY = 0;
        maxY = 0;

        foreach (Transform child in _overlayContainer.transform)
        {
            if (child.position.x > maxX) maxX = child.position.x;
            if (child.position.y > maxY) maxY = child.position.y;

            if (child.position.x < minX) minX = child.position.x;
            if (child.position.y < minY) minY = child.position.y;
        }
        //Debug.Log($"Max X: {maxX} | Min X: {minX} | Max Y: {maxY} | Min Y: {minY}");

        //_camera.gameObject.transform.set
    }
}
