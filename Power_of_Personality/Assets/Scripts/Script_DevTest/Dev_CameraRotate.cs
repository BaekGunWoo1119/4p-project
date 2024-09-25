using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dev_CameraRotate : MonoBehaviour
{
    public float rotationDuration = 10.0f;  // 10초 동안 회전
    public float minYRotation = 0.0f;       // Y축 최소 회전 값
    public float maxYRotation = 40.0f;      // Y축 최대 회전 값

    private float rotationTime = 0.0f;
    private bool isReversing = false;       // 회전 방향을 제어

    void Update()
    {
        rotationTime += Time.deltaTime;

        if (rotationTime >= rotationDuration)
        {
            isReversing = !isReversing;  // 방향 반전
            rotationTime = 0.0f;         // 타이머 초기화
        }

        float t = rotationTime / rotationDuration; 
        float targetYRotation = isReversing ? Mathf.Lerp(maxYRotation, minYRotation, t) : Mathf.Lerp(minYRotation, maxYRotation, t);

        transform.rotation = Quaternion.Euler(0, targetYRotation, 0);
    }
}
