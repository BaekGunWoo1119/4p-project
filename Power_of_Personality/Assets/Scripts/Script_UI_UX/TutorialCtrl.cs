using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCtrl : MonoBehaviour
{
    public RectTransform window;
    private float moveDuration = 0.3f;
    public float moveValue;

    private Vector2 windowPos;
    private Vector2 targetPos;
    private float elapsedTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        windowPos = window.anchoredPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            targetPos = new Vector2(windowPos.x, windowPos.y + moveValue);
            elapsedTime = 0.0f; 
        }
    }

    void Update()
    {
        // OnTriggerEnter에서 이동 명령을 받은 경우
        if (elapsedTime < moveDuration)
        {
            // 보간된 위치 계산
            float t = elapsedTime / moveDuration; // 보간 값
            Vector2 newPos = Vector2.Lerp(windowPos, targetPos, t);
            // UI 요소의 위치 업데이트
            window.anchoredPosition = newPos;

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;
        }
    }

}
