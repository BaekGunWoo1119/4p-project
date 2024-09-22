using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenModeManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown; // 해상도 변경 Dropdown
    public TMP_Dropdown screenModeDropdown; // 화면 모드 변경 Dropdown

    private List<Resolution> uniqueResolutions; // 중복을 제거한 해상도 목록
    private FullScreenMode currentScreenMode; // 현재 화면 모드

    void Start()
    {
        // 현재 모니터의 해상도를 가져와 중복을 제거
        uniqueResolutions = GetUniqueResolutions();
        PopulateResolutionDropdown();

        // 현재 스크린 모드 설정
        currentScreenMode = Screen.fullScreenMode;
        PopulateScreenModeDropdown();

        // 해상도 Dropdown의 값이 변경될 때 ChangeResolution 함수를 호출
        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(resolutionDropdown); });

        // 화면 모드 Dropdown의 값이 변경될 때 ChangeScreenMode 함수를 호출
        if (screenModeDropdown != null)
            screenModeDropdown.onValueChanged.AddListener(delegate { OnScreenModeChange(screenModeDropdown); });

        // 현재 해상도와 화면 모드를 기본 선택 값으로 설정
        SetDefaultResolution();
        SetDefaultScreenMode();
    }

    // 해상도 중복을 제거한 리스트를 반환하는 함수
    List<Resolution> GetUniqueResolutions()
    {
        Resolution[] allResolutions = Screen.resolutions;
        HashSet<string> resolutionSet = new HashSet<string>(); // 중복 방지를 위한 해시셋
        List<Resolution> uniqueResolutions = new List<Resolution>();

        foreach (Resolution res in allResolutions)
        {
            string option = res.width + " x " + res.height;

            if (resolutionSet.Add(option)) // 중복이 아니면 추가
            {
                uniqueResolutions.Add(res);
            }
        }

        return uniqueResolutions;
    }

    // Dropdown에 해상도 옵션을 추가
    void PopulateResolutionDropdown()
    {
        if (resolutionDropdown != null)
            resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        foreach (Resolution res in uniqueResolutions)
        {
            string option = res.width + " x " + res.height;
            options.Add(option);
        }

        // Dropdown에 옵션을 추가
        if (resolutionDropdown != null)
            resolutionDropdown.AddOptions(options);
    }

    // 선택된 옵션에 따른 해상도 변경
    public void OnResolutionChange(TMP_Dropdown dropdown)
    {
        int selectedIndex = dropdown.value;
        Resolution selectedResolution = uniqueResolutions[selectedIndex];

        // 선택된 해상도로 변경하며 현재의 스크린 모드 유지
        ChangeResolution(selectedResolution.width, selectedResolution.height, currentScreenMode);
    }

    // 해상도를 변경하는 함수 (현재 스크린 모드를 유지)
    public void ChangeResolution(int width, int height, FullScreenMode screenMode)
    {
        Screen.SetResolution(width, height, screenMode);
        Debug.Log("Resolution changed to: " + width + "x" + height + " with screen mode: " + screenMode);
    }

    // 현재 해상도를 기본 선택 값으로 설정
    void SetDefaultResolution()
    {
        Resolution currentResolution = Screen.currentResolution;

        for (int i = 0; i < uniqueResolutions.Count; i++)
        {
            if (uniqueResolutions[i].width == currentResolution.width &&
                uniqueResolutions[i].height == currentResolution.height)
            {
                if (resolutionDropdown != null)
                    resolutionDropdown.value = i;
                break;
            }
        }
    }

    // Dropdown에 화면 모드 옵션 추가
    void PopulateScreenModeDropdown()
    {
        if (screenModeDropdown != null)
            screenModeDropdown.ClearOptions();

        // Dropdown에 표시될 화면 모드 리스트를 준비
        List<string> options = new List<string>
        {
            "Window",
            "FullScreenWindow",
            "FullScreen"
        };

        // Dropdown에 옵션 추가
        if (screenModeDropdown != null)
            screenModeDropdown.AddOptions(options);
    }

    // 선택된 옵션에 따른 화면 모드 변경
    public void OnScreenModeChange(TMP_Dropdown dropdown)
    {
        int selectedIndex = dropdown.value;

        switch (selectedIndex)
        {
            case 0: // 창모드
                currentScreenMode = FullScreenMode.Windowed;
                break;
            case 1: // 테두리 없는 창모드
                currentScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2: // 전체화면
                currentScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
        }

        // Dropdown에서 선택된 해상도를 가져옴
        int selectedResolutionIndex = resolutionDropdown.value;
        Resolution selectedResolution = uniqueResolutions[selectedResolutionIndex];

        // 현재 해상도를 유지한 채 스크린 모드를 변경
        ChangeResolution(selectedResolution.width, selectedResolution.height, currentScreenMode);
    }

    // 현재 화면 모드를 기본 선택 값으로 설정
    void SetDefaultScreenMode()
    {
        FullScreenMode currentMode = Screen.fullScreenMode;

        switch (currentMode)
        {
            case FullScreenMode.Windowed:
                if (screenModeDropdown != null)
                    screenModeDropdown.value = 0; // 창모드
                break;
            case FullScreenMode.FullScreenWindow:
                if (screenModeDropdown != null)
                    screenModeDropdown.value = 1; // 테두리 없는 창모드
                break;
            case FullScreenMode.ExclusiveFullScreen:
                if (screenModeDropdown != null)
                    screenModeDropdown.value = 2; // 전체화면
                break;
        }
    }
}