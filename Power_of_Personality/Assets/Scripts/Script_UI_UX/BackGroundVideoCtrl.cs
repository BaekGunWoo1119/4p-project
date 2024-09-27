using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackGroundVideoCtrl : MonoBehaviour
{
    private static BackGroundVideoCtrl instance;

    void Awake()
    {
        // 기존 인스턴스가 있으면 중복 방지를 위해 새로 생성된 객체는 삭제
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            // 인스턴스가 없으면 이 객체를 인스턴스로 지정하고 파괴되지 않도록 설정
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "2-1 (MBTI Choice)")
        {
            Destroy(gameObject);
        }
    }
}
