using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TMP_Text buttonText; // 버튼 하위 텍스트
    public Color hoverColor;
    public Color clickColor;
    public Color defaultColor;

    void Start()
    {
        // 기본 색상을 텍스트에 적용
        buttonText.color = defaultColor;
    }

    // 버튼에 마우스를 올렸을 때 실행
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = hoverColor;
    }

    // 버튼에서 마우스를 뗐을 때 실행
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = defaultColor;
    }

    // 버튼 클릭 시 실행
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(ClickColor());
    }

    public IEnumerator ClickColor()
    {
        buttonText.color = clickColor;
        yield return new WaitForSeconds(0.1f);
        buttonText.color = defaultColor;
    }
}
