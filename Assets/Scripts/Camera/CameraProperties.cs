using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Dungeon;
using UnityEditor.PackageManager;
using UnityEngine;

public class CameraProperties : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Transform player;
    private float _zoomSpeed = 10f;
    private float _zoomInertia = 2f;
    private float _minZoom = 5f;
    private float _maxZoom = 10f;
    private float _defaultZoom = 7.5f;
    private float _previousZoom;
    private float _targetZoom;
    private float _cameraPanSpeed = 15f;
    private float _cameraPanBorderWidth = 5f;
    private float _cameraPanLimit = 10;
    private Vector3 _velocity = Vector3.zero;
    private float _cameraMoveSmoothness = 0.3f;
    [HideInInspector] public bool isFollowEnabled = true;
    [HideInInspector] public bool isCameraPanEnabled = false;
    
    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player obj not connected to the camera script");
        }

        if (virtualCamera == null)
        {
            Debug.LogError("Cinemachine virtual camera not connected to the camera script");
        }

        _targetZoom = _defaultZoom;
        virtualCamera.m_Lens.OrthographicSize = _defaultZoom;
    }

    private void Update()
    {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(zoomInput);
        if (Input.GetKeyDown(KeyCode.P))
        {
            isCameraPanEnabled = !isCameraPanEnabled;
            if (isCameraPanEnabled)
            {
                virtualCamera.Follow = null;
            } else if (isFollowEnabled)
            {
                virtualCamera.Follow = player.transform;
            }
        }

        if (isFollowEnabled && !isCameraPanEnabled && !CombatMode.isPlayerInCombat)
        {
            virtualCamera.Follow = player.transform;
        }

        if (!isFollowEnabled || isCameraPanEnabled)
        {
            virtualCamera.Follow = null;
            PanCamera();
        }
    }

    private void ZoomCamera(float increment)
    {
        float currentZoom = virtualCamera.m_Lens.OrthographicSize;
        _targetZoom -= increment * _zoomInertia;
        _targetZoom = Mathf.Clamp(_targetZoom, _minZoom, _maxZoom);
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(currentZoom, _targetZoom, Time.deltaTime * _zoomSpeed);
    }

    private void PanCamera()
    {
        Vector3 cameraPosition = virtualCamera.transform.position;
        Vector3 playerPosition = player.transform.position;

        if (Input.mousePosition.x >= Screen.width - _cameraPanBorderWidth)
        {
            cameraPosition.x += _cameraPanSpeed * Time.deltaTime;
        }

        if (Input.mousePosition.x <= _cameraPanBorderWidth)
        {
            cameraPosition.x -= _cameraPanSpeed * Time.deltaTime;
        }

        if (Input.mousePosition.y >= Screen.height - _cameraPanBorderWidth)
        {
            cameraPosition.y += _cameraPanSpeed * Time.deltaTime;
        }

        if (Input.mousePosition.y <= _cameraPanBorderWidth)
        {
            cameraPosition.y -= _cameraPanSpeed * Time.deltaTime;
        }

        cameraPosition.x = Mathf.Clamp(
            cameraPosition.x, 
            playerPosition.x - _cameraPanLimit, 
            playerPosition.x + _cameraPanLimit
            );
        cameraPosition.y = Mathf.Clamp(
            cameraPosition.y, 
            playerPosition.y - _cameraPanLimit, 
            playerPosition.y + _cameraPanLimit
            );

        virtualCamera.transform.position = cameraPosition;
    }

    public void AdjustCameraToTheRoomSize(Vector2 size, Vector3 centre)
    {
        float targetZoom = MathF.Max(size.x, size.y);
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
            virtualCamera.m_Lens.OrthographicSize,
            targetZoom,
            Time.deltaTime * _zoomSpeed);
        virtualCamera.transform.position = new Vector3(centre.x, centre.y, centre.z);
        
        Vector3 currentPosition = virtualCamera.transform.position;
        Vector3 targetPosition = new Vector3(centre.x, centre.y, currentPosition.z);
        virtualCamera.transform.position = Vector3.Lerp(
            currentPosition,
            targetPosition,
            Time.deltaTime * _cameraMoveSmoothness
        );
    }
}