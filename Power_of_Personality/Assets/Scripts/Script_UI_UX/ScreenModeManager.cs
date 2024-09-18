using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenModeManager : MonoBehaviour
{
    public TMP_Dropdown screenModeDropdown; // Dropdown for screen mode

    void Start()
    {
        // Dropdown에 화면 모드 옵션을 추가
        PopulateScreenModeDropdown();

        // Dropdown 값이 변경되면 OnScreenModeChange 함수 호출
        if (screenModeDropdown != null)
            screenModeDropdown.onValueChanged.AddListener(delegate { OnScreenModeChange(screenModeDropdown); });

        // 기본 선택 값을 현재 화면 모드로 설정
        SetDefaultScreenMode();
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
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 1: // 테두리 없는 창모드
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2: // 전체화면
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
        }
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