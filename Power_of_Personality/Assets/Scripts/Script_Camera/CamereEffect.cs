using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CamereEffect : MonoBehaviour
{
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
