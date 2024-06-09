
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraCtrl : MonoBehaviour
{
    public Transform target; //Player
    public Vector3 offset;

    //오프셋에 추가할 값
    public float offsetPosX;
    public float offsetPosY;
    public float offsetPosZ;

    //오프셋 초기 값
    public float offsetRotX;
    public float offsetRotY;
    public float offsetRotZ;
    public Vector3 currentVelocity = Vector3.zero;
    
    //public CameraEffect cameraEffect;

    //카메라 흔들림
    public float shakeAmount = 0f;
    public float shakeDuration = 0.03f;
    public float shakeTimer = 0f;

    //카메라 고정
    public Vector3 forwardDirection;
    public Vector3 UpDirection;
    public Vector3 BothDirection;
    public float focusTimer = 0f;
    public float xPos;
    public float yPos;
    public float zPos;
    public float xRot;
    public float yRot;
    public float zRot;
    public string timeValue;

    private bool isMove;

    //카메라 여러 대
    public Camera[] camera;

    protected virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void FixedUpdate()
    {
        FollowPlayer();

        ShakeCamera_Update();

        FocusCamera_Update();
    }

    #region 카메라 켜기/끄기

    protected virtual void SetCamera(int cameraNumber)
    {
        if(camera[0] = this.gameObject.GetComponent<Camera>())
        {
            camera[cameraNumber].targetDisplay = 1;
        }
    }
    protected virtual void UnSetCamera(int cameraNumber)
    {
        if(camera[0] = this.gameObject.GetComponent<Camera>())
        {
            camera[cameraNumber].targetDisplay = 2;
        }
    }

    #endregion

    protected virtual void FollowPlayer()
    {
        if(target == null){
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (forwardDirection == null || forwardDirection != target.transform.root.transform.forward)
        {
            forwardDirection = target.transform.root.transform.forward;
        }
        if (UpDirection == null || UpDirection != target.transform.root.transform.up)
        {
            UpDirection = target.transform.root.transform.up;
        }
        if(BothDirection == null || BothDirection != target.transform.root.transform.right)
        {
            BothDirection = target.transform.root.transform.right;
        }
        offset = forwardDirection * offsetPosZ + UpDirection * offsetPosY + BothDirection * offsetPosX;
        if (target != null && focusTimer <= 0 && shakeTimer <= 0 && !isMove)
        {
            // 플레이어의 움직임을 살짝 딜레이를 주고 따라 감
            transform.position = Vector3.SmoothDamp(transform.position, target.position+offset, ref currentVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(target.transform.root.GetComponentInParent<Transform>().eulerAngles.x + offsetRotX, 
            target.transform.root.GetComponentInParent<Transform>().eulerAngles.y + offsetRotY, 
            target.transform.root.GetComponentInParent<Transform>().eulerAngles.z + offsetRotZ);
        }
    }

    protected virtual IEnumerator Setoffset(float wait)
    {
        yield return new WaitForSeconds(wait);
        //if(offset != offsetOrigin && shakeTimer <= 0 && focusTimer <= 0)
        //{
        //    offset = offsetOrigin;
        //    isMove = false;
        //}
    }

    public virtual void moveStop(float seconds)
    {
        StartCoroutine(Setoffset(seconds));
    }
    
    #region 카메라 흔들기 코드
    //범용적으로 제작, 카메라 추가 필요할 시 적절히 넣어볼 것
    public virtual void ShakeCamera(float Amount, float Duration, string zoom)
    {
        shakeAmount = Amount;
        shakeDuration = Duration;
        shakeTimer = shakeDuration;
        if(zoom == "zoom")
        {
            offset = new Vector3(0, 2, -4);
        }
    }

    public virtual void ShakeCamera_Update()
    {
        if (shakeTimer > 0)
        {
            isMove = true;
            // 카메라를 흔들이기
            transform.localPosition += UnityEngine.Random.insideUnitSphere * shakeAmount;

            // 흔들림 시간 감소
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            // 흔들림이 끝나면 초기 위치로 돌아가기
            shakeTimer = 0f;
            transform.localPosition = transform.localPosition;
        }
    }
    #endregion

    #region 카메라 포커스 코드
    //범용적으로 제작, 카메라 추가 필요할 시 적절히 넣어 볼 것
    public virtual void FocusCamera(float xP, float yP, float zP, float R, float Duration, string value)
    {   
        xPos = xP;
        yPos = yP;
        zPos = zP;
        yRot = R;
        xRot = 5;
        zRot = 0;

        timeValue = value;
        focusTimer = Duration;
    }

    public virtual void FocusCamera_Update()
    {
        if (focusTimer > 0)
        {
            isMove = true;
            if(timeValue == "stop")
            {
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(xPos,yPos,zPos), ref currentVelocity, 0.1f);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(xRot,yRot,zRot), Time.deltaTime * 3.5f);
            }

            else if(timeValue == "round")
            {
                xRot = 10;
                zRot = 0;
                if(yRot == 60)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(target.position.x - 2,yPos,zPos - 5), ref currentVelocity, 0.1f);
                }
                else
                {
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(target.position.x - 1,yPos,zPos - 5), ref currentVelocity, 0.1f);
                }
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(xRot,yRot,zRot), Time.deltaTime * 4.0f);
            }
            else if(timeValue == "forward")
            {
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(xPos,yPos,zPos), ref currentVelocity, 0.1f);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(10,yRot,0), Time.deltaTime * 4.0f);
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(xPos,yPos,zPos), ref currentVelocity, 0.1f);
            }
            
            focusTimer -= Time.deltaTime;
        }
        else
        {
            focusTimer = 0f;
            timeValue = "play";
            xPos = 0;
            yPos = 0;
            zPos = 0;
            isMove = false;
        }
    }
    #endregion

    #region 궁극기 카메라 코드
    //상속시켜서 직업별로 다르게 할 예정 <-- 변경
    //06.08 수정 : 직업별로 각 다른 카메라 코드를 짜 둘 예정
    public virtual void UltimateCamera_Warrior(float SkillYRot)
    {
        StartCoroutine(Warrior_E_Camera(SkillYRot));
    }

    IEnumerator Warrior_E_Camera(float SkillYRot)
    {
        yield return new WaitForSeconds(2.0f);
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            FocusCamera(target.transform.position.x - 5, target.transform.position.y + 2.5f, target.transform.position.z, 60, 5.3f, "round");
        }
        else
        {
            FocusCamera(target.transform.position.x - 2.5f, target.transform.position.y + 2.5f, target.transform.position.z, -30, 5.3f, "round");
        }
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.6f);
        ShakeCamera(0.5f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.8f);
        ShakeCamera(0.1f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.4f);
        ShakeCamera(0.2f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.4f);
        ShakeCamera(0.1f, 0.1f, "zoom");
        yield return new WaitForSeconds(1.2f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(1.0f);
        ShakeCamera(0.6f, 0.1f, "zoom");
        yield return new WaitForSeconds(1f);
        moveStop(0.1f);
    }

    public virtual void UltimateCamera_Rogue(float SkillYRot)
    {
        StartCoroutine(Rogue_E_Camera(SkillYRot));
    }

    IEnumerator Rogue_E_Camera(float SkillYRot)
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SetCamera(9);
        }
        else
        {
            SetCamera(3);
        }
        UnSetCamera(0);
        yield return new WaitForSeconds(1.1f);
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SetCamera(1);
        }
        else
        {
            SetCamera(11);
        }
        UnSetCamera(9);
        ShakeCamera(0.4f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.4f);
        ShakeCamera(0.6f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.7f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.4f);
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            FocusCamera(target.transform.position.x - 8, target.transform.position.y + 2f, target.transform.position.z + 3.0f, 60, 2.3f, "round");
        }
        else
        {
            FocusCamera(target.transform.position.x - 5.5f, target.transform.position.y + 2.2f, target.transform.position.z + 3.0f, -30, 2.3f, "round");
        }
        yield return new WaitForSeconds(0.5f);
        ShakeCamera(0.8f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.8f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.8f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.8f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.8f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        moveStop(0.1f);
        /*
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            FocusCamera(target.transform.position.x + 3.0f, target.transform.position.y + 1.5f, target.transform.position.z, -90, 1.1f, "forward");
        }
        else
        {
            FocusCamera(target.transform.position.x - 3.0f, target.transform.position.y + 1.5f, target.transform.position.z, 90, 1.1f, "forward");
        }
        yield return new WaitForSeconds(1.1f);
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            FocusCamera(target.transform.position.x, target.transform.position.y + 2.5f, target.transform.position.z - 5.0f, 0, 2.0f, "forward");
        }
        else
        {
            FocusCamera(target.transform.position.x, target.transform.position.y + 2.5f, target.transform.position.z - 5.0f, 180, 2.0f, "forward");
        }
        ShakeCamera(0.4f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.4f);
        ShakeCamera(0.6f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.7f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.3f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.4f);
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            FocusCamera(target.transform.position.x - 8, target.transform.position.y + 2f, target.transform.position.z + 3.0f, 60, 2.3f, "round");
        }
        else
        {
            FocusCamera(target.transform.position.x - 5.5f, target.transform.position.y + 2.2f, target.transform.position.z + 3.0f, -30, 2.3f, "round");
        }
        yield return new WaitForSeconds(0.5f);
        ShakeCamera(0.8f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.8f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.8f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.8f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        ShakeCamera(0.8f, 0.1f, "zoom");
        yield return new WaitForSeconds(0.05f);
        moveStop(0.1f);
        */
    }

    public virtual void UltimateCamera_Archer(float SkillYRot)
    {
        StartCoroutine(Archer_E_Camera(SkillYRot));
    }

    IEnumerator Archer_E_Camera(float SkillYRot)
    {
        yield return null;
    }

    public virtual void UltimateCamera_Wizard(float SkillYRot)
    {
        StartCoroutine(Wizard_E_Camera(SkillYRot));
    }

    IEnumerator Wizard_E_Camera(float SkillYRot)
    {
        yield return null;
    }

    #endregion

    #region 점프 카메라 코드
    //달라지는 직업 있을 시 사용할 예정
    public virtual void JumpCamera_Warrior()
    {
        FocusCamera(target.transform.position.x, target.transform.position.y + 2, target.transform.position.z - 9, 0, 0.2f, "null");
    }
    #endregion
}