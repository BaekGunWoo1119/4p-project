using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown; // 해상도 변경 Dropdown
    private Resolution[] availableResolutions;

    void Start()
    {
        // 현재 모니터의 해상도를 가져옴
        availableResolutions = Screen.resolutions;
        PopulateResolutionDropdown();
        
        // Dropdown의 값이 변경될 때 ChangeResolution 함수를 호출
        if(resolutionDropdown != null)
            resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(resolutionDropdown); });
        
        // 현재 해상도를 기본 선택 값으로 설정
        SetDefaultResolution();
    }

    // Dropdown에 해상도 옵션을 추가
    void PopulateResolutionDropdown()
    {
        if(resolutionDropdown != null)
            resolutionDropdown.ClearOptions();

        // Dropdown에 표시될 해상도 리스트를 준비
        List<string> options = new List<string>();
        foreach (Resolution res in availableResolutions)
        {
            string option = res.width + " x " + res.height;
            options.Add(option);
        }

        // Dropdown에 옵션을 추가
        if(resolutionDropdown != null)
            resolutionDropdown.AddOptions(options);
    }

    // 선택된 옵션에 따른 해상도 변경
    public void OnResolutionChange(TMP_Dropdown dropdown)
    {
        int selectedIndex = dropdown.value;
        Resolution selectedResolution = availableResolutions[selectedIndex];
        
        // 선택된 해상도로 변경
        ChangeResolution(selectedResolution.width, selectedResolution.height);
    }

    // 해상도를 변경하는 함수
    public void ChangeResolution(int width, int height)
    {
        Screen.SetResolution(width, height, true);
        Debug.Log("Resolution changed to: " + width + "x" + height);
    }

    // 현재 해상도를 기본 선택 값으로 설정
    void SetDefaultResolution()
    {
        Resolution currentResolution = Screen.currentResolution;

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            if (availableResolutions[i].width == currentResolution.width &&
                availableResolutions[i].height == currentResolution.height)
            {
                if(resolutionDropdown != null)
                    resolutionDropdown.value = i;
                break;
            }
        }
    }
}
