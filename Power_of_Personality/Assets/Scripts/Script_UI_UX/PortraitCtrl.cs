using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitCtrl : MonoBehaviour
{
    private InventoryCtrl InvenCtrl;
    private Image targetImage; // 색을 반짝이게 만들 Image 컴포넌트
    public Color flashColor = Color.red; // 반짝일 때 사용할 색
    public Color normalColor = Color.white; // 기본 색
    public float flashDuration = 0.5f; // 반짝이는 간격 (초)

    private bool isFlashing = false;

    void Start()
    {
        InvenCtrl = GameObject.Find("InventoryCtrl").GetComponent<InventoryCtrl>();
        targetImage = this.GetComponent<Image>();
    }

    void Update()
    {
        // float 값이 0보다 크면 반짝이게 하고, 그렇지 않으면 멈춤
        if (InvenCtrl.StatPoint > 0 && !isFlashing)
        {
            StartCoroutine(FlashImage());
        }
        else if (InvenCtrl.StatPoint <= 0 && isFlashing)
        {
            StopCoroutine(FlashImage());
            isFlashing = false;
            targetImage.color = normalColor; // 기본 색으로 복구
        }
    }

    // 이미지 색을 반짝이게 하는 코루틴
    IEnumerator FlashImage()
    {
        isFlashing = true;

        while (InvenCtrl.StatPoint > 0) // float 값이 0보다 크면 반복
        {
            // 반짝이기 - flashColor로 변경
            targetImage.color = flashColor;
            yield return new WaitForSeconds(flashDuration);

            // 원래 색으로 복구
            targetImage.color = normalColor;
            yield return new WaitForSeconds(flashDuration);
        }

        isFlashing = false;
    }
}
