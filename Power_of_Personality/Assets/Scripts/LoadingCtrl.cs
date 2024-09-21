using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingCtrl : MonoBehaviour
{
    public GameObject loadingUI; // 비활성화할 객체
    public Transform loadingObj; // 체크할 부모 객체

    void Start()
    {
        // 부모 객체가 null이 아닐 경우 체크
        if (loadingObj != null)
        {
            // 모든 하위 객체가 로드되었는지 체크
            if (AreAllChildrenActive(loadingObj))
            {
                // 하위 객체가 모두 활성화된 경우, 특정 객체 비활성화
                if (loadingUI != null)
                {
                    loadingUI.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogWarning("Parent object is not assigned.");
        }
    }

    // 모든 하위 객체가 활성화되었는지 확인하는 함수
    private bool AreAllChildrenActive(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (!child.gameObject.activeInHierarchy) // 하위 객체가 비활성화되어 있다면
            {
                return false;
            }
        }
        return true; // 모든 하위 객체가 활성화되어 있다면
    }
}
