using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RotateFix : MonoBehaviour
{
    public GameObject humanoidObject; // �޸ӳ��̵� ������Ʈ ����

    void Update()
    {
        // �޸ӳ��̵� ������Ʈ�� ȸ�� ���� ����
        if (humanoidObject != null)
        {
            humanoidObject.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
            humanoidObject.transform.rotation = Quaternion.identity;
        }
    }
}
