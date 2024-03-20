using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraProperties : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    CinemachineFramingTransposer transposer;
    [SerializeField] float sensitivity = 10f;

    private void Start()
    {
        if (virtualCamera != null)
        {
            transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        else
        {
            Debug.LogError("Virtual camera not assigned!");
        }
    }

    private void Update()
    {
        if (transposer == null)
            return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0)
        {
            float newDistance = transposer.m_CameraDistance - scroll * sensitivity;
            transposer.m_CameraDistance = Mathf.Max(transposer.m_MinimumDistance, Mathf.Min(transposer.m_MaximumDistance, newDistance));
        }
    }
}
