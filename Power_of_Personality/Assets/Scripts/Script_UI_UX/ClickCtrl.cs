using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BtnSetActive : MonoBehaviour
{
    public GameObject activeObj; // 활성화할 오브젝트
    private Vector3 originalScale;
    private Button button;

    private void Start()
    {
        // 오브젝트의 스케일 저장
        if (activeObj != null)
        {
            originalScale = activeObj.transform.localScale;
            activeObj.transform.localScale = new Vector3(0, 0, 0);
        }

        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(SetActiveObj);
        }
    }

    // 버튼 클릭
    private void SetActiveObj()
    {
        if (activeObj != null)
        {
            // 오브젝트의 스케일을 초기 스케일로 변경
            activeObj.transform.localScale = originalScale;
        }
    }

    // 이 스크립트가 비활성화될 때 호출되는 함수
    private void OnDestroy()
    {
        if (button != null)
        {
            // 연결한 이벤트 리스너를 해제
            button.onClick.RemoveListener(SetActiveObj);
        }
    }
}
