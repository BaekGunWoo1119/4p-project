using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoveDestroys : MonoBehaviour
{
    public GameObject m_gameObjectMain; // 주 객체
    public GameObject m_gameObjectTail; // 꼬리 객체
    GameObject m_makedObject; // 생성된 객체
    public Transform m_hitObject; // 충돌 객체
    public float maxLength; // 최대 거리
    public bool isDestroy; // 파괴 여부
    public float ObjectDestroyTime; // 객체 파괴 시간
    public float TailDestroyTime; // 꼬리 파괴 시간
    public float HitObjectDestroyTime; // 충돌 객체 파괴 시간
    public float maxTime = 1; // 최대 시간
    public float MoveSpeed = 10; // 이동 속도
    public bool isCheckHitTag; // 태그 체크 여부
    public string mtag; // 태그
    public bool isShieldActive = false; // 쉴드 활성화 여부
    public bool isHitMake = true; // 충돌 객체 생성 여부

    float time; // 시간
    bool ishit; // 충돌 여부
    float m_scalefactor; // 스케일 팩터

    private void Start()
    {
        m_scalefactor = 1;
        time = Time.time;
    }

    void LateUpdate()
    {
        // 객체를 앞으로 이동
        transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed * m_scalefactor);
        if (!ishit)
        {
            RaycastHit hit;
            // 레이캐스트를 사용하여 충돌 확인

            if (Physics.Raycast(transform.position, transform.forward, out hit, maxLength))
            {                
                HitObj(hit);
            }
        }

        // 파괴 여부 확인 후 객체 파괴
        if (isDestroy)
        {
            if (Time.time > time + ObjectDestroyTime)
            {
                MakeHitObject(transform);
                Destroy(gameObject);
            }
        }
    }

    // 충돌 객체 생성 (RaycastHit 사용)
    void MakeHitObject(RaycastHit hit)
    {
        if (isHitMake == false)
            return;
        m_makedObject = Instantiate(m_hitObject, hit.point, Quaternion.Euler(0f, 0f, 0f)).gameObject;
        m_makedObject.transform.parent = transform.parent;
        m_makedObject.transform.localScale = new Vector3(1, 1, 1);
    }

    // 충돌 객체 생성 (Transform 사용)
    void MakeHitObject(Transform point)
    {
        if (isHitMake == false)
            return;
        m_makedObject = Instantiate(m_hitObject, point.transform.position, point.rotation).gameObject;
        m_makedObject.transform.parent = transform.parent;
        m_makedObject.transform.localScale = new Vector3(1, 1, 1);
    }

    // 충돌 처리 함수
    void HitObj(RaycastHit hit)
    {
        // 태그 체크 여부 확인
        if (isCheckHitTag)
            if (hit.transform.tag != mtag)
                return;
        ishit = true;
        // 꼬리 객체가 있으면 부모에서 분리
        if (m_gameObjectTail)
            m_gameObjectTail.transform.parent = null;
        // 충돌 객체 생성
        MakeHitObject(hit);

        // 쉴드 활성화 시 쉴드 객체에 충돌 위치 전달
        if (isShieldActive)
        {
            ShieldActivate m_sc = hit.transform.GetComponent<ShieldActivate>();
            if (m_sc)
                m_sc.AddHitObject(hit.point);
        }

        // 현재 객체 파괴
        Destroy(this.gameObject);
        // 꼬리 객체 파괴
        Destroy(m_gameObjectTail, TailDestroyTime);
        // 생성된 충돌 객체 파괴
        Destroy(m_makedObject, HitObjectDestroyTime);
    }
}
