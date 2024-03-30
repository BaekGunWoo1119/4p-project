using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    public float fadeSpeed = 1.0f; // 페이드 인/아웃 속도
    public GameObject gameOverGRP; // 페이드 인/아웃 적용할 UI 요소
    //public GameObject gameClearGRP; // 페이드 인/아웃 적용할 UI 요소
    private Graphic[] targetGRP;

    private bool isFading = false; // 페이드 여부
    private float targetAlpha = 0.0f; //알파 값 (0: 투명, 1: 불투명)

    void Start()
    {
        // 초기에는 UI 요소를 숨김
        ApplyFadeChild(gameOverGRP.transform);
        // 변수 초기화
        targetGRP = new Graphic[gameOverGRP.transform.childCount + 1];
    }

    void Update()
    {
        if (isFading)
        {
            for(int i = 0; i < gameOverGRP.transform.childCount; i++)
            {
                float newAlpha = Mathf.MoveTowards(targetGRP[i].color.a, targetAlpha, fadeSpeed * Time.deltaTime);
                SetAlpha(targetGRP[i], newAlpha);
            }
        }
    }

    // 페이드 인/아웃
    public void GameOver(bool fadeIn)
    {
        // 페이드 중이 아닌 경우에만 페이드를 시작
        if (fadeIn == true)
        {
            targetGRP[0] = gameOverGRP.GetComponent<Graphic>();
            for(int i = 1; i <= gameOverGRP.transform.childCount; i++)
            {
                targetGRP[i] = gameOverGRP.transform.GetChild(i - 1).GetComponent<Graphic>();
            }
            targetAlpha = fadeIn ? 1.0f : 0.0f;
            isFading = true;
        }
    }

    private void ApplyFadeChild(Transform parent)
    {
        // 현재 객체의 그래픽 컴포넌트
        Graphic graphic = parent.GetComponent<Graphic>();

        // 그래픽 컴포넌트가 존재하면 페이드 인/아웃 알파값 설정
        if (graphic != null)
        {
            SetAlpha(graphic, 0.0f);
        }

        // 모든 자식 객체에 대해 재귀함수 호출
        foreach (Transform child in parent)
        {
            ApplyFadeChild(child);
        }
    }

    // UI 요소의 알파 값
    private void SetAlpha(Graphic graphic, float alpha)
    {
        Color color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }
}
