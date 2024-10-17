using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    public float fadeSpeed = 1.0f; // 페이드 인/아웃 속도
    public GameObject gameClearGRP; // 페이드 인/아웃 적용할 UI 요소
    private Graphic[] targetGRP;
    public Button okbutton; 
    public GameObject creditbutton;
    public GameObject threeStars; 

    private bool isFading = false; // 페이드 여부
    private float targetAlpha = 0.0f; //알파 값 (0: 투명, 1: 불투명)

    public TMP_Text clearText; //클리어 멘트 적을 텍스트
    public TMP_Text endText; //정보 적을 텍스트
    public static float PlayTime = 0.0f;
    private float TempPlayTime = 0.0f;
    private int defaultBonus;
    private int afterBonus;
    private int BonusGap;

    void Start()
    {
        okbutton.onClick.AddListener(OK_Pressed);
        creditbutton.SetActive(false);
        creditbutton.GetComponent<Button>().onClick.AddListener(Credit_Pressed);
        // 초기에는 UI 요소를 숨김
        ApplyFadeChild(gameClearGRP.transform);
        // 변수 초기화
        targetGRP = new Graphic[gameClearGRP.transform.childCount + 1];
        PlayTime = 0.0f;
        defaultBonus = PlayerPrefs.GetInt("BonusStat", 0);
    }

    void Update()
    {
        TempPlayTime += Time.deltaTime;
        if (isFading)
        {
            for(int i = 0; i < gameClearGRP.transform.childCount; i++)
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
    public void Game_Clear(bool fadeIn)
    {
        
        // 페이드 중이 아닌 경우에만 페이드를 시작
        if (fadeIn == true)
        {   
            SetBonusStat();
            gameClearGRP.SetActive(true);
            creditbutton.SetActive(true);
            clearText.text = "축하합니다!";
            endText.text = "플레이 시간 : " + (int)PlayTime +" 초"+ "\n최종 MBTI : "+ PlayerPrefs.GetString("PlayerMBTI") +"\n추가 보너스 스텟 : "+ BonusGap;
            targetGRP[0] = gameClearGRP.GetComponent<Graphic>();
            targetGRP[gameClearGRP.transform.childCount - 1] = threeStars.GetComponent<Graphic>();
            for(int i = 1; i <= gameClearGRP.transform.childCount - 2; i++)
            {
                targetGRP[i] = gameClearGRP.transform.GetChild(i - 1).GetComponent<Graphic>();
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

    //게임 오버 된 후 OK버튼 누를 시 메인화면(원래 SceneLoader에 있던 코드인데 지속적인 오류로 분리해 둠)
    private void Credit_Pressed()
    {
        SceneManager.LoadScene("Credit");
    }

    private void SetBonusStat()
    {   
        Scene scene = SceneManager.GetActiveScene(); //함수 안에 선언하여 사용한다.
        switch (scene.name){
            case "Forest_Example":
                afterBonus = 0;
                break;
            case "Cave_Example":
                afterBonus = 4;
                break;
            case "Sewer_Example":
                afterBonus = 8;
                break;
            case "Castle_Example":
                afterBonus = 12;
                break;
        }
        BonusGap = afterBonus - defaultBonus;
        if(BonusGap > 0){
            PlayerPrefs.SetInt("BonusStat", afterBonus);
        }
        else{
            BonusGap = 0;
        }
    }
}
