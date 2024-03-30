using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class BtnUnsetActive : MonoBehaviour
{
    public GameObject activeObj; // 활성화할 오브젝트
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(UnsetActiveObj);
        }
    }

    // 버튼 클릭
    private void UnsetActiveObj()
    {
        if (activeObj != null)
        {
            Debug.Log("버튼클릭");
            // 오브젝트의 스케일을 0으로 변경
            activeObj.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    // 이 스크립트가 비활성화될 때 호출되는 함수
    private void OnDestroy()
    {
        if (button != null)
        {
            // 연결한 이벤트 리스너를 해제
            button.onClick.RemoveListener(UnsetActiveObj);
        }
    }
}