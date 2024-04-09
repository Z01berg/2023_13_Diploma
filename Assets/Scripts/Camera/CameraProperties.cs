using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraProperties : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float zoomSpeed = 2f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    void Update()
    {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(zoomInput);
    }

    void ZoomCamera(float increment)
    {
        float currentZoom = virtualCamera.m_Lens.OrthographicSize;
        float newZoom = Mathf.Clamp(currentZoom - increment * zoomSpeed, minZoom, maxZoom);
        virtualCamera.m_Lens.OrthographicSize = newZoom;
    }
}
