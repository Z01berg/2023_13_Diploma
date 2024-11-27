using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Dungeon;
using UnityEditor.PackageManager;
using UnityEngine;

public class CameraProperties : MonoBehaviour
{
    
    //[SerializeField] private float _zoomOutOnTheRoomValue;
    //[SerializeField] private float _zoomInOnTheRoomValue;
    
    public CinemachineVirtualCamera virtualCamera;
    public Transform player;
    public float zoomSpeed = 2f;
    public float minZoom = 1f;
    public float maxZoom = 100f;
    private float _previousZoom;


    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player obj not connected to the camera");
        }
    }

    private void Update()
    {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(zoomInput);
    }

    private void ZoomCamera(float increment)
    {
        float currentZoom = virtualCamera.m_Lens.OrthographicSize;
        float newZoom = Mathf.Clamp(currentZoom - increment * zoomSpeed, minZoom, maxZoom);
        virtualCamera.m_Lens.OrthographicSize = newZoom;
    }

    public void AdjustCameraToTheRoomSize(Vector2 size, Vector3 centre)
    {
        virtualCamera.m_Lens.OrthographicSize = MathF.Max(size.x, size.y);
        virtualCamera.transform.position = new Vector3(centre.x, centre.y, centre.z);
    }
}
