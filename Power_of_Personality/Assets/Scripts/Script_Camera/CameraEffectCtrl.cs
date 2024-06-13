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
    private Bloom bloom; //Bloom 효과(고급 색상 필터)
    private PaniniProjection paniniProjection; //Panini Projection 효과(볼록렌즈)
    private Vignette vignette; //Vignette 효과 (카메라 테두리)

    private bool Reset = false;

    #region 효과 Value 값

    //페이드 시간
    private float fadeTime;

    //블러 효과 값들
    private float blurValue;
    private float blurTime = 0;
    private bool bluring = false;

    //색번짐 효과 값들
    private float diffuseValue;
    private float diffuseTime = 0;
    private bool diffusing = false;

    //고급필터 효과 값들
    private float fillterValue;
    private float fillterTime = 0;
    private bool filltering = false;
    public Texture[] texture;

    //테두리 효과 값들
    private float roundValue;
    private float roundTime = 0;
    private bool rounding = false;

    #endregion

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

        if(!volumeProfile.TryGet(out bloom))
        {
            Debug.LogError("Bloom 파라미터를 찾을 수 없습니다.");
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
        Filter_Set();

        RoundCamera_Set();

    }

    #region 값 범용적 효과들

    public IEnumerator BlurCamera(float Value, float time)
    {
        // Depth Of Field의 블러 강도 조정
        depthOfField.active = true; // Depth Of Field 효과 활성화
        depthOfField.focusDistance.overrideState = true; // 효과 내 Focus Distance 항목 활성화
        blurValue = Value;
        fadeTime = time;
        depthOfField.focusDistance.value = 2;
        yield return new WaitForSeconds(0.03f);
        bluring = true;
    }

    public void BlurCamera_Set()
    {
        if(!bluring)
        {
            blurTime = 0;
        }
        else
        {
            blurTime += Time.deltaTime;
            depthOfField.focusDistance.value = Mathf.Lerp(2, blurValue, Time.deltaTime); 
        }

        if(blurTime > fadeTime)
        {
            bluring = false;
            depthOfField.active = false;
        }
    }

    public IEnumerator ColorDiffuse(float Value, float time)
    {
        chromaticAberration.active = true; // Chromatic Aberration 효과 활성화
        chromaticAberration.intensity.overrideState = true; // 효과 내 Intensity 항목 활성화
        diffuseValue = Value;
        fadeTime = time;
        depthOfField.focusDistance.value = 0;
        yield return new WaitForSeconds(0.03f);
        diffusing = true;
    }

    public void ColorDiffuse_Set()
    {
        if(!diffusing)
        {
            diffuseTime = 0;
        }
        else
        {
            diffuseTime += Time.deltaTime;
            chromaticAberration.intensity.value = Mathf.Lerp(0, diffuseValue, Time.deltaTime); // 포커스 거리 설정(0 ~ 100)          
        }

        if(diffuseTime > 0.3f)
        {
            diffusing = false;
            chromaticAberration.active = false;
        }
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
        yield return new WaitForSeconds(contrastValue);
        colorAdjustments.active = false;
    }

    public IEnumerator RoyalFilter(float Value, float time, float FilterR, float FilterG, float FilterB, float dirtInt, Texture dirtTexture)
    {
        bloom.active = true;
        bloom.threshold.value = 0.9f;
        bloom.scatter.value = 0.7f;
        bloom.tint.value = new Color(FilterR, FilterG, FilterB, 1f);
        bloom.dirtTexture.value = dirtTexture;
        bloom.dirtIntensity.value = dirtInt;
        fillterValue = Value;
        fillterTime = time;
        yield return new WaitForSeconds(0.03f);
        filltering = true;
    }


    public void Filter_Set()
    {
        if(!filltering)
        {
            fillterTime = 0;
        }
        else
        {
            fillterTime += Time.deltaTime;
            bloom.intensity.value = Mathf.Lerp(0, fillterValue, 0.5f); // 포커스 거리 설정(0 ~ 100)          
        }

        if(fillterTime > 0.8f)
        {
            filltering = false;
            bloom.active = false;
        }
    }

    public IEnumerator WeakZoom(int zoomValue)
    {
        paniniProjection.active = true;
        paniniProjection.distance.overrideState = true;
        yield return new WaitForSeconds(0.1f);
        paniniProjection.distance.value = zoomValue;
    }
    
    public IEnumerator RoundCamera(float Value, float colorR, float colorG, float colorB, float time)
    {
        vignette.active = true;
        vignette.intensity.value = 0;
        vignette.color.overrideState = true;
        roundValue = Value;
        fadeTime = time;
        vignette.color.value = new Color(colorR, colorG, colorB, 1f);
        yield return new WaitForSeconds(0.03f);
        rounding = true;
    }

    public void RoundCamera_Set()
    {
        if(!rounding)
        {
            roundTime = 0;
        }
        else
        {
            roundTime += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(0, roundValue, 1f); // 포커스 거리 설정(0 ~ 100)
            Debug.Log("라운딩");
        }

        if(roundTime > fadeTime * 2)
        {
            rounding = false;
            vignette.active = false;
        }
        else if(roundTime > fadeTime) // 페이드 아웃
        {
            roundTime += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(roundValue, 0, 1f);
            Debug.Log("라운딩 페이드");
        }
    }   

    #endregion

    #region 값 설정된 효과들

    public void DamageCamera()
    {
        StartCoroutine(BlurCamera(2f, 0.3f));
        StartCoroutine(ColorDiffuse(10f, 0.3f));
        StartCoroutine(RoundCamera(0.4f, 1, 0, 0, 0.3f));
    }

    public void BigAttackCamera()
    {
        StartCoroutine(ColorDiffuse(80f, 0.7f)); 
    }

    public void DangerousCamera()
    {
        StartCoroutine(RoundCamera(0.3f, 1, 0, 0, 1.0f));
    }

    public void PropEffectCamera(int Num, float time)
    {
        if(PlayerPrefs.GetString("property") == "Ice")
        {
            StartCoroutine(RoyalFilter(4, time, 1.3f, 1.7f, 2.5f, 4, texture[Num]));
        }
        else if(PlayerPrefs.GetString("property") == "Fire")
        {
            StartCoroutine(RoyalFilter(4, time, 2.3f, 0.9f, 0.3f, 4, texture[Num]));
        }
    }

    public void poisonEffectCamera()
    {
        StartCoroutine(RoyalFilter(4, 0.5f, 121, 0, 255, 4, texture[1]));
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
