using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpellHover : MonoBehaviour
{
    public GameObject[] spell;
    public GameObject actObj;
    private string titleText;
    private string innerText;
    private float a;
    private float screenWidth;
    private float screenHeight;

    void Start()
    {
        actObj.transform.localScale = new Vector3(0, 0, 0);
        // 현재 화면의 크기 정보 가져오기
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    void Update() 
    {   
        if(screenWidth != Screen.width)
            screenWidth = Screen.width;
        if(screenHeight != Screen.height)
            screenHeight = Screen.height;

        // 호버 시 따라다니게
        float yRect = Input.mousePosition.y;
        if (yRect > screenHeight * 0.537f)  // 580 / 1080을 비율로 계산
        {
            a = screenHeight * 0.6f;      // 820 / 1080을 비율로 계산
        }
        else if (yRect <= screenHeight * 0.537f)  // 580 / 1080
        {
            a = screenHeight * 0.3f;      // 210 / 1080
        }

        // 마우스 위치에 맞게 actObj의 위치를 설정 (화면 비율 고려)
        actObj.GetComponent<RectTransform>().localPosition = new Vector3(
            (Input.mousePosition.x / screenWidth) * 1920 - 900,  // x 좌표 비율 조정
            (Input.mousePosition.y / screenHeight) * 1080 - a,   // y 좌표 비율 조정
            Input.mousePosition.z
        );
    }

    public void Spell_SwiftnessHover()
    {
        actObj.transform.localScale = new Vector3(1, 1, 1);
        titleText = "유체화";
        innerText = "플레이어의 이동 속도를\n15초간 증가시킵니다.\n(재사용 대기시간 105초)";
        spell[0].GetComponent<TextChange>().ChangeText(titleText, 0);
        spell[0].GetComponent<TextChange>().ChangeText(innerText, 1);
    } 

    public void Spell_StunHover()
    {
        actObj.transform.localScale = new Vector3(1, 1, 1);
        titleText = "기절";
        innerText = "망치를 소환하여\n플레이어 전방의 적을\n10초간 기절시킵니다.\n(재사용 대기시간 100초)";
        spell[0].GetComponent<TextChange>().ChangeText(titleText, 0);
        spell[0].GetComponent<TextChange>().ChangeText(innerText, 1);
    } 

    public void Spell_HealHover()
    {
        actObj.transform.localScale = new Vector3(1, 1, 1);
        titleText = "회복";
        innerText = "플레이어의 체력을\n전부 회복시킵니다.\n(재사용 대기시간 90초)";
        spell[0].GetComponent<TextChange>().ChangeText(titleText, 0);
        spell[0].GetComponent<TextChange>().ChangeText(innerText, 1);
    } 

    public void Spell_RoarOfAngerHover()
    {
        actObj.transform.localScale = new Vector3(1, 1, 1);
        titleText = "표효";
        innerText = "표효하며 공격력이\n 15초간 증가합니다.\n(재사용 대기시간 75초)";
        spell[0].GetComponent<TextChange>().ChangeText(titleText, 0);
        spell[0].GetComponent<TextChange>().ChangeText(innerText, 1);
    } 

    public void Spell_UnstoppableHover()
    {
        actObj.transform.localScale = new Vector3(1, 1, 1);
        titleText = "저지불가";
        innerText = "플레이어의 경직을\n 20초간 제거합니다.\n(재사용 대기시간 80초)";
        spell[0].GetComponent<TextChange>().ChangeText(titleText, 0);
        spell[0].GetComponent<TextChange>().ChangeText(innerText, 1);
    } 

    public void Spell_TimeSlowdownHover()
    {
        actObj.transform.localScale = new Vector3(1, 1, 1);
        titleText = "시간감속";
        innerText = "게임 속 시간이\n 15초간 느려집니다.\n(재사용 대기시간 105초)";
        spell[0].GetComponent<TextChange>().ChangeText(titleText, 0);
        spell[0].GetComponent<TextChange>().ChangeText(innerText, 1);
    } 

    public void Spell_ImmuneHover()
    {
        actObj.transform.localScale = new Vector3(1, 1, 1);
        titleText = "무적";
        innerText = "플레이어가 10초간\n데미지를 받지 않습니다.\n(재사용 대기시간 190초)";
        spell[0].GetComponent<TextChange>().ChangeText(titleText, 0);
        spell[0].GetComponent<TextChange>().ChangeText(innerText, 1);
    } 

    public void Spell_ResurrectHover()
    {
        actObj.transform.localScale = new Vector3(1, 1, 1);
        titleText = "부활";
        innerText = "플레이어가 죽은 후\n부활할 수 있습니다.\n(재사용 대기시간 120초)";
        spell[0].GetComponent<TextChange>().ChangeText(titleText, 0);
        spell[0].GetComponent<TextChange>().ChangeText(innerText, 1);
    } 

    public void Spell_HoverExit()
    {
        actObj.transform.localScale = new Vector3(0, 0, 0);
    }
}
