using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dev_CameraController : MonoBehaviour
{
    public float moveSpeed = 10.0f;   // 카메라 이동 속도
    public float mouseSensitivity = 100.0f;  // 마우스 감도
    private float pitch = 0.0f;   // 상하 회전 (마우스 Y축)
    private float yaw = 0.0f;     // 좌우 회전 (마우스 X축)

    void Start()
    {
        // 마우스 커서를 화면 중앙에 고정하고 잠금
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. 마우스를 따라 카메라 회전
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f); // 상하 각도 제한 (90도 이상 넘어가지 않게)

        transform.localRotation = Quaternion.Euler(pitch, yaw, 3.0f);

        // 2. WASD 키로 카메라 이동
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime; // A, D 키로 좌우 이동
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;   // W, S 키로 전후 이동

        Vector3 move = transform.right * moveX + transform.forward * moveZ;  // 카메라의 방향에 따라 이동
        transform.position += move;

        // ESC 키로 마우스 커서 잠금 해제
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
