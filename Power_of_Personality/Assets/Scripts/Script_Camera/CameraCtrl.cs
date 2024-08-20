
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;

public class CameraCtrl : MonoBehaviour
{
    public static Transform target; //Player
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
    
    public GameObject cameraEffect;

    //카메라 흔들림
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
    public CinemachineVirtualCamera[] virtualCameras;
    private int cameraIdx;
    private CinemachineBrain cinemachineBrain;
    private CinemachineBasicMultiChannelPerlin shakeNoise;
    private int focusingCamera = 0;

    protected virtual void Start()
    {
        StartCoroutine(SetTarget());
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        cameraEffect = GameObject.FindWithTag("CameraEffect");
        if(!this.GetComponent<CinemachineBrain>())
        {
            shakeNoise = this.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    protected virtual void FixedUpdate()
    {
        FollowPlayer();

        ShakeCamera_Update();

        FocusCamera_Update();
    }

    protected virtual IEnumerator SetTarget()
    {
        yield return new WaitForSeconds(0.03f);
        target = GameObject.FindGameObjectWithTag("Player").transform;

        if(!this.GetComponent<CinemachineBrain>())
        {
        }
        else
        {
            for (int i = 1; i < virtualCameras.Length; i++)
            {
                virtualCameras[i].gameObject.SetActive(false);
            }
            virtualCameras[focusingCamera].gameObject.SetActive(true);
        }
    }

    #region 카메라 켜기/끄기

    public virtual void SetCamera(int cameraNumber)
    {
        for (int i = 0; i < virtualCameras.Length; i++)
        {
            virtualCameras[i].gameObject.SetActive(i == cameraNumber);
            //Debug.Log("카메라 변경");
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
        if (target != null && focusTimer <= 0 && !isMove)
        {
            // 플레이어의 움직임을 살짝 딜레이를 주고 따라 감
            transform.parent.position = Vector3.SmoothDamp(transform.parent.position, target.position, ref currentVelocity, 0.1f);
            transform.parent.rotation = Quaternion.Euler(target.transform.root.GetComponentInParent<Transform>().eulerAngles.x, 
                                                        target.transform.root.GetComponentInParent<Transform>().eulerAngles.y + 90, 
                                                        target.transform.root.GetComponentInParent<Transform>().eulerAngles.z);
        }
    }

    protected virtual IEnumerator Setoffset(float wait)
    {
        yield return new WaitForSeconds(wait);
        if(shakeTimer <= 0 && focusTimer <= 0)
        {
            //offset = offsetOrigin;
            isMove = false;
        }
    }

    public virtual void moveStop(float seconds)
    {
        StartCoroutine(Setoffset(seconds));
    }
    
    #region 카메라 흔들기 코드
    //범용적으로 제작, 카메라 추가 필요할 시 적절히 넣어볼 것
    public virtual void ShakeCamera(float Amount, float Duration, string zoom)
    {
        if(shakeNoise != null)
        {
            shakeNoise.m_AmplitudeGain = Amount;
            shakeNoise.m_FrequencyGain = Amount;
        }
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

            // 흔들림 시간 감소
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            // 흔들림이 끝나면 초기 위치로 돌아가기
            shakeTimer = 0f;
            if(shakeNoise != null)
            {
                shakeNoise.m_AmplitudeGain = 0f;
                shakeNoise.m_FrequencyGain = 0f;
            }
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
        focusingCamera = 46;
        SetCamera(focusingCamera);
        float currentFOV = virtualCameras[focusingCamera].m_Lens.FieldOfView;
        virtualCameras[focusingCamera].m_Lens.FieldOfView = 30f;
        yield return new WaitForSeconds(2.0f);
        virtualCameras[focusingCamera].m_Lens.FieldOfView = currentFOV;
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            focusingCamera = 13;
            SetCamera(focusingCamera);
        }
        else
        {
            focusingCamera = 21;
            SetCamera(focusingCamera);
        }
        SubCameraCtrl CCT = virtualCameras[focusingCamera].transform.GetComponent<SubCameraCtrl>();
        CCT.ShakeCamera(6f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.6f);
        CCT.ShakeCamera(10f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.8f);
        CCT.ShakeCamera(2f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.4f);
        CCT.ShakeCamera(4f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.4f);
        CCT.ShakeCamera(2f, 0.3f, "zoom");
        yield return new WaitForSeconds(1.1f);
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            focusingCamera = 24;
            SetCamera(focusingCamera);
        }
        else
        {
            focusingCamera = 30;
            SetCamera(focusingCamera);
        }
        CCT = virtualCameras[focusingCamera].transform.GetComponent<SubCameraCtrl>();
        virtualCameras[focusingCamera].m_Lens.FieldOfView = 60f;
        yield return new WaitForSeconds(0.1f);
        CCT.ShakeCamera(6f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.8f);
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            focusingCamera = 28;
            SetCamera(focusingCamera);
        }
        else
        {
            focusingCamera = 35;
            SetCamera(focusingCamera);
        }
        CCT = virtualCameras[focusingCamera].transform.GetComponent<SubCameraCtrl>();
        virtualCameras[focusingCamera].m_Lens.FieldOfView = 60f;
        yield return new WaitForSeconds(0.1f);
        cameraEffect.GetComponent<CameraEffectCtrl>().BigAttackCamera();
        cameraEffect.GetComponent<CameraEffectCtrl>().PropEffectCamera(2, 0.2f, 1.2f);
        CCT.ShakeCamera(12f, 0.3f, "zoom");
        yield return new WaitForSeconds(1f);
        moveStop(0.1f);
        SetCamera(0);
        virtualCameras[46].m_Lens.FieldOfView = currentFOV; 
        virtualCameras[24].m_Lens.FieldOfView = currentFOV; 
        virtualCameras[30].m_Lens.FieldOfView = currentFOV; 
        virtualCameras[28].m_Lens.FieldOfView = currentFOV; 
        virtualCameras[35].m_Lens.FieldOfView = currentFOV; 
    }

    public virtual void UltimateCamera_Rogue(float SkillYRot)
    {
        StartCoroutine(Rogue_E_Camera(SkillYRot));
    }

    IEnumerator Rogue_E_Camera(float SkillYRot)
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            focusingCamera = 41;
            SetCamera(focusingCamera);
        }
        else
        {
            focusingCamera = 37;
            SetCamera(focusingCamera);
        }
        float currentFOV = virtualCameras[focusingCamera].m_Lens.FieldOfView;
        virtualCameras[focusingCamera].m_Lens.FieldOfView = 20f;
        yield return new WaitForSeconds(1.7f);
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            focusingCamera = 1;
            SetCamera(focusingCamera);
        }
        else
        {
            focusingCamera = 11;
            SetCamera(focusingCamera);
        }
        SubCameraCtrl CCT = virtualCameras[focusingCamera].transform.GetComponent<SubCameraCtrl>();
        CCT.ShakeCamera(5f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.2f);
        CCT.ShakeCamera(7f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.7f);
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            focusingCamera = 20;
            SetCamera(focusingCamera);
        }
        else
        {
            focusingCamera = 14;
            SetCamera(focusingCamera);
        }
        CCT.ShakeCamera(3f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.05f);
        CCT.ShakeCamera(3f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.05f);
        CCT.ShakeCamera(3f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.05f);
        CCT.ShakeCamera(3f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.05f);
        CCT.ShakeCamera(3f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.05f);
        CCT.ShakeCamera(3f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.05f);
        CCT.ShakeCamera(3f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.7f);
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            focusingCamera = 49;
            SetCamera(focusingCamera);
        }
        else
        {
            focusingCamera = 48;
            SetCamera(focusingCamera);
        }
        virtualCameras[focusingCamera].m_Lens.FieldOfView = 30f;
        yield return new WaitForSeconds(0.1f);
        CCT = virtualCameras[focusingCamera].transform.GetComponent<SubCameraCtrl>();
        cameraEffect.GetComponent<CameraEffectCtrl>().BigAttackCamera();
        CCT.ShakeCamera(10f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.05f);
        CCT.ShakeCamera(10f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.05f);
        CCT.ShakeCamera(10f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.05f);
        CCT.ShakeCamera(10f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.05f);
        cameraEffect.GetComponent<CameraEffectCtrl>().BigAttackCamera();
        CCT.ShakeCamera(10f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.05f);
        moveStop(0.1f);
        yield return new WaitForSeconds(0.4f);
        SetCamera(0);
        virtualCameras[41].m_Lens.FieldOfView = currentFOV; 
        virtualCameras[37].m_Lens.FieldOfView = currentFOV; 
        virtualCameras[48].m_Lens.FieldOfView = currentFOV; 
        virtualCameras[49].m_Lens.FieldOfView = currentFOV; 
    }

    public virtual void UltimateCamera_Archer(float SkillYRot)
    {
        StartCoroutine(Archer_E_Camera(SkillYRot));
    }

    IEnumerator Archer_E_Camera(float SkillYRot)
    {
        focusingCamera = 46;
        SetCamera(focusingCamera);
        float currentFOV = virtualCameras[focusingCamera].m_Lens.FieldOfView;
        virtualCameras[focusingCamera].m_Lens.FieldOfView = 30f;
        yield return new WaitForSeconds(1.7f);
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            focusingCamera = 43;
            SetCamera(focusingCamera);
            yield return new WaitForSeconds(0.1f);
            focusingCamera = 41;
            SetCamera(focusingCamera);
        }
        else
        {
            focusingCamera = 47;
            SetCamera(focusingCamera);
            yield return new WaitForSeconds(0.1f);
            focusingCamera = 37;
            SetCamera(focusingCamera);
            
        }
        virtualCameras[focusingCamera].m_Lens.FieldOfView = 60f;
        yield return new WaitForSeconds(1.4f);
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            focusingCamera = 46;
            SetCamera(focusingCamera);
        }
        else
        {
            focusingCamera = 44;
            SetCamera(focusingCamera);
        }
        virtualCameras[focusingCamera].m_Lens.FieldOfView = 120f;
        yield return new WaitForSeconds(0.1f);
        SubCameraCtrl CCT = virtualCameras[focusingCamera].transform.GetComponent<SubCameraCtrl>();
        cameraEffect.GetComponent<CameraEffectCtrl>().BigAttackCamera();
        cameraEffect.GetComponent<CameraEffectCtrl>().PropEffectCamera(3, 0.5f, 5f);
        CCT.ShakeCamera(30f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.03f);
        CCT.ShakeCamera(30f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.03f);
        CCT.ShakeCamera(30f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.03f);
        CCT.ShakeCamera(30f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.03f);
        CCT.ShakeCamera(30f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.03f);
        CCT.ShakeCamera(30f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.03f);
        cameraEffect.GetComponent<CameraEffectCtrl>().BigAttackCamera();
        CCT.ShakeCamera(30f, 0.3f, "zoom");
        yield return new WaitForSeconds(1.0f);
        SetCamera(0);
        virtualCameras[46].m_Lens.FieldOfView = currentFOV; 
        virtualCameras[14].m_Lens.FieldOfView = currentFOV; 
        virtualCameras[20].m_Lens.FieldOfView = currentFOV; 
    }

    public virtual void UltimateCamera_Wizard(float SkillYRot)
    {
        StartCoroutine(Wizard_E_Camera(SkillYRot));
    }

    IEnumerator Wizard_E_Camera(float SkillYRot)
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            focusingCamera = 41;
            SetCamera(focusingCamera);
        }
        else
        {
            focusingCamera = 37;
            SetCamera(focusingCamera);
        }
        float currentFOV = virtualCameras[focusingCamera].m_Lens.FieldOfView;
        virtualCameras[focusingCamera].m_Lens.FieldOfView = 20f;
        yield return new WaitForSeconds(2.1f);
        SubCameraCtrl CCT = virtualCameras[focusingCamera].transform.GetComponent<SubCameraCtrl>();
        CCT.ShakeCamera(3f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.6f);
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            focusingCamera = 24;
            SetCamera(focusingCamera);
        }
        else
        {
            focusingCamera = 35;
            SetCamera(focusingCamera);
        }
        virtualCameras[focusingCamera].m_Lens.FieldOfView = 90f;
        /*
        yield return new WaitForSeconds(1.3f);
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            focusingCamera = 48;
            SetCamera(focusingCamera);
        }
        else
        {
            focusingCamera = 49;
            SetCamera(focusingCamera);
        }
        yield return new WaitForSeconds(0.6f);
        */
        yield return new WaitForSeconds(1.8f);
        CCT = virtualCameras[focusingCamera].transform.GetComponent<SubCameraCtrl>();
        CCT.ShakeCamera(15f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.1f);
        CCT.ShakeCamera(15f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.1f);
        CCT.ShakeCamera(15f, 0.3f, "zoom");
        yield return new WaitForSeconds(0.1f);
        CCT.ShakeCamera(15f, 0.3f, "zoom");
        yield return new WaitForSeconds(1.5f);
        CCT.ShakeCamera(45f, 0.3f, "zoom");
        cameraEffect.GetComponent<CameraEffectCtrl>().PropEffectCamera(0, 1, 1.2f);
        yield return new WaitForSeconds(1.0f);
        SetCamera(0);
        virtualCameras[41].m_Lens.FieldOfView = currentFOV; 
        virtualCameras[37].m_Lens.FieldOfView = currentFOV; 
        virtualCameras[24].m_Lens.FieldOfView = currentFOV; 
        virtualCameras[35].m_Lens.FieldOfView = currentFOV; 
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