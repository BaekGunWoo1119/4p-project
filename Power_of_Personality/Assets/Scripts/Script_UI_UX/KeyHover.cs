using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeToggleColor : MonoBehaviour
{
    public Button button; // 색상을 변경할 버튼
    public Color newColor; // 변경할 색상
    public KeyCode targetKey; // 원하는 키
    private Color originalColor; // 원래 색상

    private bool isKeyPressed = false;

    void Start()
    {
        ColorBlock colors = button.colors;
        originalColor = colors.normalColor;
    }

    void Update()
    {
        if (Input.GetKeyDown(targetKey) && button != null)
        {
            isKeyPressed = true;
        }
        else if (Input.GetKeyUp(targetKey))
        {
            isKeyPressed = false;
        }

        if (isKeyPressed)
        {
            if (button != null)
            {
                newColor.a = 1f;
                ColorBlock colors = button.colors;
                colors.normalColor = newColor;
                button.colors = colors;
            }
        }
        else
        {
            if (button != null)
            {
                newColor.a = 1f;
                ColorBlock colors = button.colors;
                colors.normalColor = originalColor;
                button.colors = colors;
            }
        }
    }
}
