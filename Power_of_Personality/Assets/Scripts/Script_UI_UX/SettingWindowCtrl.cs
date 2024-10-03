using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindowCtrl : MonoBehaviour
{
    public GameObject setWindow; // 세팅 윈도우
    public KeyDownCtrl escKeyDown; //ESC 키 제어
    private Vector3 originalScale;
    private Button button;

    private void Start()
    {
        // 오브젝트의 스케일 저장
        if (setWindow != null)
        {
            originalScale = setWindow.transform.localScale;
            setWindow.transform.localScale = new Vector3(0, 0, 0);
        }

        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(Setting_On);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }

    // 버튼 클릭
    private void Setting_On()
    {
        if (setWindow != null)
        {
            // 오브젝트의 스케일을 초기 스케일로 변경
            setWindow.transform.localScale = originalScale;
            escKeyDown.enabled = false;
        }
    }

    //esc키 다시 기능 추가하기 및 세팅창 비활성화
    public void Setting_Off()
    {
        setWindow.transform.localScale = new Vector3(0, 0, 0);
        escKeyDown.enabled = true;
    }


}
