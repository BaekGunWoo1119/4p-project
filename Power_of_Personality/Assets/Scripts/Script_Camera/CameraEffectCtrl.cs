using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraEffectCtrl : MonoBehaviour
{
    public Volume globalVolume; // Global Volume 참조
    public VolumeProfile volumeProfile; // Volume Profile을 참조

    private DepthOfField depthOfField; //Depth Of Field 효과(블러)
    private ChromaticAberration chromaticAberration; //Chromatic Aberration 효과(색번짐)
    private ColorAdjustments colorAdjustments; //Color Adjustments 효과(색필터)
    private PaniniProjection paniniProjection; //Panini Projection 효과(볼록렌즈)
    private Vignette vignette; //Vignette 효과 (카메라 테두리)

    private bool Reset = false;

    void Start()
    {
        globalVolume = this.GetComponent<Volume>();
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

    void Update()
    {
        if(Reset == true)
        {
            ResetCameraEffect();
        }

        BlurCamera_Set();
        ColorDiffuse_Set();

    }

    #region 값 범용적 효과들

    public IEnumerator BlurCamera(float BlurValue)
    {
        // Depth Of Field의 블러 강도 조정
        depthOfField.active = true; // Depth Of Field 효과 활성화
        depthOfField.focusDistance.overrideState = true; // 효과 내 Focus Distance 항목 활성화
        float blurTime = 0;
        yield return new WaitForSeconds(0.1f);
        while(blurTime == 1.5f)
        {
            blurTime += Time.deltaTime;
            depthOfField.focusDistance.value = Mathf.Lerp(3, BlurValue, Time.deltaTime); // 포커스 거리 설정(0 ~ 100)
        }
    }

    public void BlurCamera_Set()
    {

    }

    public IEnumerator ColorDiffuse(float DiffuseValue)
    {
        chromaticAberration.active = true; // Chromatic Aberration 효과 활성화
        chromaticAberration.intensity.overrideState = true; // 효과 내 Intensity 항목 활성화
        float diffuseTime = 0;
        yield return new WaitForSeconds(0.1f);
        while(diffuseTime == 1.5f)
        {
            diffuseTime  += Time.deltaTime;
            chromaticAberration.intensity.value  = Mathf.Lerp(0, DiffuseValue, Time.deltaTime); // 포커스 거리 설정(0 ~ 100)
        }
    }

    public void ColorDiffuse_Set()
    {
        
    }

    public IEnumerator ColorFilter(float FilterR, float FilterG, float FilterB, float contrastValue)
    {
        colorAdjustments.active = true; // 효과 활성화
        colorAdjustments.contrast.overrideState = true; //효과 내 contrast 항목 활성화
        colorAdjustments.colorFilter.overrideState = true;
        /*
        startValue = 0;
        endValue = contrastValue;
        timeSpeed = 1;
        */
        yield return new WaitForSeconds(0.1f);
        colorAdjustments.contrast.value = Mathf.Lerp(0, contrastValue, Time.deltaTime);
        colorAdjustments.colorFilter.value = new Color(FilterR, FilterG, FilterB, 1f);
    }

    public IEnumerator WeakZoom(int zoomValue)
    {
        paniniProjection.active = true;
        paniniProjection.distance.overrideState = true;
        yield return new WaitForSeconds(0.1f);
        paniniProjection.distance.value = zoomValue;
    }
    
    public IEnumerator RoundCamera(float roundValue, float colorR, float colorG, float colorB)
    {
        vignette.active = true;
        vignette.color.overrideState = true;
        vignette.color.value = new Color(colorR, colorG, colorB, 1f);
        yield return new WaitForSeconds(0.1f);
        vignette.intensity.value = Mathf.Lerp(0, roundValue, 1f); // 포커스 거리 설정(0 ~ 100)
        yield return new WaitForSeconds(1.1f);
        vignette.intensity.value = Mathf.Lerp(roundValue, 0, 1f); // 포커스 거리 설정(0 ~ 100)
        yield return new WaitForSeconds(0.1f);
        //vignette.active = false;
    }   

    #endregion

    #region 값 설정된 효과들

    public void DamageCamera()
    {
        StartCoroutine(BlurCamera(2f));
        StartCoroutine(ColorDiffuse(0.4f));
        StartCoroutine(RoundCamera(0.4f, 1, 0, 0));
    }

    #endregion

    #region 효과 초기화

    public void ResetCameraEffect()
    {
        //서서히 바뀌는거 추가할 예정
        /*
        depthOfField.active = false; 
        chromaticAberration.active = false; 
        colorAdjustments.active = false; 
        paniniProjection.active = false;
        vignette.active = false;
        */
    }

    #endregion
}
