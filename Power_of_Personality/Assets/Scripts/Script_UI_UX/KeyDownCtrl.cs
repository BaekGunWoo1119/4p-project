using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDownCtrl : MonoBehaviour
{
    public GameObject activeObj; // 활성화할 오브젝트
    public KeyCode ctrlKey;
    private Vector3 originalScale;

    private void Start()
    {
        // 오브젝트의 스케일 저장
        if (activeObj != null)
        {
            originalScale = activeObj.transform.localScale;
            activeObj.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(ctrlKey))
        {
            SetActiveObj();
        }
    }

    // 특정 키보드 키 클릭
    private void SetActiveObj()
    {
        if (activeObj != null)
        {
            // 오브젝트의 스케일을 초기 스케일로 변경
            activeObj.transform.localScale = originalScale;
        }
    }
}
