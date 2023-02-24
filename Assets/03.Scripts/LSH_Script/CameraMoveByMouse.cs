using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraMoveByMouse : MonoBehaviour
{
    private Vector3 resetCamera;

    [SerializeField]
    [Range(0.01f, 0.1f)]
    float cameraSpeed = 0.01f;

    // 카메라가 맵의 일정 범위 내에서 움직이도록 제한
    [SerializeField]
    int mapBoundaryMinX;
    [SerializeField]
    int mapBoundaryMinZ;
    [SerializeField]
    int mapBoundaryMaxX;
    [SerializeField]
    int mapBoundaryMaxZ;

    Vector3 originalCamPos;


    private void Start()
    {
        resetCamera = Vector3.zero;
        originalCamPos = this.transform.localPosition;
    }

    private void LateUpdate()
    {
        if (this.gameObject.GetComponent<HexMapCamera>().IsCameraAutoMoving)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            print(resetCamera);
            this.transform.position = Vector3.zero;
        }

        Vector2 mousePosition = Input.mousePosition;
        Vector3 cameraPosition = this.transform.localPosition;

        // 1. 마우스가 게임 화면 밖에 있을 때
        if (mousePosition.x < 0 || mousePosition.x > Screen.width ||
            mousePosition.y < 0 || mousePosition.y > Screen.height)
            return;
        // 2. 마우스가 게임 화면의 가운데 영역에 있을 때
        else if (Math.Abs((Screen.width / 2) - mousePosition.x) <= 400
            && Math.Abs((Screen.height / 2) - mousePosition.y) <= 250)
            return;

        Vector3 newCameraPosition = this.transform.localPosition;

        newCameraPosition.x += (Input.mousePosition.x - Screen.width / 2) * cameraSpeed * Time.deltaTime;
        newCameraPosition.z += (Input.mousePosition.y - Screen.height / 2) * cameraSpeed * Time.deltaTime;

        // 3. 카메라가 게임맵 영역 밖에 있을 때
        if ((cameraPosition.x <= mapBoundaryMinX && cameraPosition.x > newCameraPosition.x)
            || (cameraPosition.z <= mapBoundaryMinZ && cameraPosition.z > newCameraPosition.z)
            || (cameraPosition.x >= mapBoundaryMaxX && cameraPosition.x < newCameraPosition.z)
            || (cameraPosition.z >= mapBoundaryMaxZ && cameraPosition.z < newCameraPosition.z))
            return;

        this.transform.localPosition = newCameraPosition;
    }
}
