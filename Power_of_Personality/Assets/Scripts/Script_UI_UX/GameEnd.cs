using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameEnd : MonoBehaviour
{
    public float fadeSpeed = 1.0f; // 페이드 인/아웃 속도
    public GameObject gameOverGRP; // 페이드 인/아웃 적용할 UI 요소
    //public GameObject gameClearGRP; // 페이드 인/아웃 적용할 UI 요소
    private Graphic[] targetGRP;
    public Button okbutton; 

    private bool isFading = false; // 페이드 여부
    private float targetAlpha = 0.0f; //알파 값 (0: 투명, 1: 불투명)

    private TMP_Text endText; //정보 적을 텍스트
    private float PlayTime = 0.0f;
    private float TempPlayTime = 0.0f;

    void Start()
    {
        endText = GameObject.Find("Text-GameOver").GetComponent<TMP_Text>(); 
        okbutton.onClick.AddListener(OK_Pressed);
        // 초기에는 UI 요소를 숨김
        ApplyFadeChild(gameOverGRP.transform);
        // 변수 초기화
        targetGRP = new Graphic[gameOverGRP.transform.childCount + 1];
        gameOverGRP.SetActive(false);
        PlayTime = 0.0f;
    }

    void Update()
    {
        TempPlayTime += Time.deltaTime;
        if (isFading)
        {
            for(int i = 0; i < gameOverGRP.transform.childCount; i++)
            {
                float newAlpha = Mathf.MoveTowards(targetGRP[i].color.a, targetAlpha, fadeSpeed * Time.deltaTime);
                SetAlpha(targetGRP[i], newAlpha);
            }
        }
        else{
            PlayTime = TempPlayTime;
        }
    }

    // 페이드 인/아웃
    public void GameOver(bool fadeIn)
    {
        
        // 페이드 중이 아닌 경우에만 페이드를 시작
        if (fadeIn == true)
        {
            gameOverGRP.SetActive(true);
            endText.text = "플레이 시간 : " + (int)PlayTime +" 초"+ "\n최종 MBTI : "+ PlayerPrefs.GetString("PlayerMBTI") +"\n추가 보너스 스텟 : 1";
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

    //게임 오버 된 후 OK버튼 누를 시 메인화면(원래 SceneLoader에 있던 코드인데 지속적인 오류로 분리해 둠)
    private void OK_Pressed()
    {
        SceneManager.LoadScene("1 (Main)");
    }

}
