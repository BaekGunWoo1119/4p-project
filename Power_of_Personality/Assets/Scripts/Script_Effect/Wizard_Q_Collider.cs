using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Q_Collider : MonoBehaviour
{
    public GameObject QSkill_Collider;
    public float QSkill_zGrowthRate;
    public float maxScale;

    private Vector3 startPosition;
    private Vector3 initialScale;
    private bool isStop = false;

    void Start()
    {
        float ColliderDistanceAdd = GetComponentInParent<ATKEffect_Control>().EffectDistanceAdd;
        startPosition = transform.position;
        initialScale = transform.localScale; // 초기 스케일 저장

        if (Status.set6_3_Activated)
        {
            maxScale = maxScale * ColliderDistanceAdd;
        }

        maxScale += 2f;
    }

    void Update()
    {
        Vector3 currentScale = transform.localScale;
        Vector3 parentScale = transform.parent ? transform.parent.lossyScale : Vector3.one;

        // z축 방향으로만 스케일을 증가시키기
        if (currentScale.z * parentScale.z < maxScale)
        {
            float scaleIncrement = QSkill_zGrowthRate * Time.deltaTime;

            // 부모의 영향을 고려하여 로컬 스케일 조정
            currentScale.z += scaleIncrement / parentScale.z;

            // 변경된 스케일을 오브젝트에 적용
            transform.localScale = currentScale;

            // 위치 보정 (z축으로만 커지게 하기 위해 오브젝트의 위치를 이동)
            if (!isStop && Vector3.Distance(startPosition, transform.position) < maxScale/2)
            {
                transform.localPosition += new Vector3(scaleIncrement / 2, 0, 0);
            } else
            {
                isStop = true;
            }
            
        }
        else
        {
            // maxScale에 도달하면 스케일을 maxScale로 고정
            currentScale.z = maxScale / parentScale.z;
            transform.localScale = currentScale;
        }
    }
}