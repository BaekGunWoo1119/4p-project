using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RotateFix : MonoBehaviour
{
    public GameObject humanoidObject; // 휴머노이드 오브젝트 설정

    void Update()
    {
        // 휴머노이드 오브젝트의 회전 값을 무시
        if (humanoidObject != null)
        {
            humanoidObject.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
            humanoidObject.transform.rotation = Quaternion.identity;
        }
    }
}
