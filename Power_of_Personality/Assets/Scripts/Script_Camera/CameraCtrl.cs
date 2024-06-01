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
    public Vector3 currentVelocity = Vector3.zero;
    
    private CameraEffect cameraEffect;

    //카메라 흔들림
    public float shakeAmount = 0f;
    public float shakeDuration = 0.03f;
    public float shakeTimer = 0f;

    //카메라 고정
    public Vector3 forwardDirection;
    public Vector3 UpDirection;
    public float focusTimer = 0f;
    public float xPos;
    public float yPos;
    public float zPos;
    public float xRot;
    public float yRot;
    public float zRot;
    public string timeValue;

    private bool isMove;

    protected virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        cameraEffect = new CameraEffect();
    }

    protected virtual void LateUpdate()
    {
        FollowPlayer();

        ShakeCamera_Update();

        FocusCamera_Update();
    }

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
        offset = forwardDirection * -10f + UpDirection * 4.25f;
        if (target != null && focusTimer <= 0 && shakeTimer <= 0 && !isMove)
        {
            // 플레이어의 움직임을 살짝 딜레이를 주고 따라 감
            transform.position = Vector3.SmoothDamp(transform.position, target.position+offset, ref currentVelocity, 0f);
            transform.rotation = target.transform.root.GetComponentInParent<Transform>().rotation;
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
    //상속시켜서 직업별로 다르게 할 예정
    public virtual void UltimateCamera(float SkillYRot)
    {
    }
    #endregion

    #region 점프 카메라 코드
    //달라지는 직업 있을 시 사용할 예정
    public virtual void JumpCamera()
    {
    }
    #endregion

    public virtual void DamageCamera()
    {
        cameraEffect.BlurCamera(1.15f);
        cameraEffect.ColorDiffuse(0.4f);
        cameraEffect.RoundCamera(0.4f, 255, 0, 0);
    }
}

public class CameraEffect
{
    #region 카메라 이펙트 변수/디버그
    public Volume globalVolume; // Global Volume 참조
    public VolumeProfile volumeProfile; // Volume Profile을 참조

    private DepthOfField depthOfField; //Depth Of Field 효과(블러)
    private ChromaticAberration chromaticAberration; //Chromatic Aberration 효과(색번짐)
    private ColorAdjustments colorAdjustments; //Color Adjustments 효과(색필터)
    private PaniniProjection paniniProjection;

    private Vignette vignette; //Vignette 효과 (카메라 테두리)

    void Start()
    {
        // 연결된 Global Volume이 없으면 종료
        if (globalVolume == null)
        {
            Debug.LogError("Global Volume이 연결되지 않았습니다.");
            return;
        }

        // VolumeProfile 받아오기
        volumeProfile = globalVolume.profile;

        //파라미터 받아오기
        if (!volumeProfile.TryGet(out depthOfField))
        {
            Debug.LogError("Depth Of Field 파라미터를 찾을 수 없습니다.");
            return;
        }

        //파라미터 받아오기
        if (!volumeProfile.TryGet(out chromaticAberration))
        {
            Debug.LogError("Chromatic Aberration 파라미터를 찾을 수 없습니다.");
            return;
        }

        if(!volumeProfile.TryGet(out colorAdjustments))
        {
            Debug.LogError("Color Adjustments 파라미터를 찾을 수 없습니다.");
            return;
        }

        if(!volumeProfile.TryGet(out paniniProjection))
        {
            Debug.LogError("Panini Projection 파라미터를 찾을 수 없습니다.");
            return;
        }

        if(!volumeProfile.TryGet(out vignette))
        {
            Debug.LogError("vignette 파라미터를 찾을 수 없습니다.");
            return;
        }
    }

    #endregion
    public void BlurCamera(float BlurValue)
    {
        // Depth Of Field의 블러 강도 조정
        depthOfField.active = true; // Depth Of Field 효과 활성화
        depthOfField.focusDistance.overrideState = true; // 효과 내 Focus Distance 항목 활성화
        depthOfField.focusDistance.value = BlurValue; // 포커스 거리 설정(0 ~ 100)
    }

    public void ColorDiffuse(float DiffuseValue)
    {
        chromaticAberration.active = true; // Chromatic Aberration 효과 활성화
        chromaticAberration.intensity.overrideState = true; // 효과 내 Intensity 항목 활성화
        chromaticAberration.intensity.value = DiffuseValue; // 색번짐 범위 설정(0 ~ 1)
    }

    public void ColorFilter(float FilterR, float FilterG, float FilterB, float contrastValue)
    {
        colorAdjustments.active = true; // 효과 활성화
        colorAdjustments.contrast.overrideState = true; //효과 내 contrast 항목 활성화
        colorAdjustments.colorFilter.overrideState = true;
        colorAdjustments.contrast.value = contrastValue;
        colorAdjustments.colorFilter.value = new Color(FilterR, FilterG, FilterB, 1f);
    }

    public void WeakZoom(int zoomValue)
    {
        paniniProjection.active = true;
        paniniProjection.distance.overrideState = true;
        paniniProjection.distance.value = zoomValue;
    }

    public void RoundCamera(float roundValue, float colorR, float colorG, float colorB)
    {
        vignette.active = true;
        vignette.color.overrideState = true;
        vignette.color.value = new Color(colorR, colorG, colorB, 1f);
        vignette.intensity.value = roundValue;
    }
}