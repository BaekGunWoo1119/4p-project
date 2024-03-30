using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bright_Tut_Btn : MonoBehaviour
{
    private Image image;
    public Color[] colors; // 변경할 색 배열
    private int currentIndex = 0; // 현재 색 인덱스

    void Start()
    {
        image = this.GetComponent<Image>();
        // 시작할 때 색 변경 코루틴 시작
        StartCoroutine(ChangeColorCoroutine());
    }

    IEnumerator ChangeColorCoroutine()
    {
        while (true)
        {
            // 현재 인덱스에 해당하는 색으로 이미지의 색 변경
            image.color = colors[currentIndex];
            
            // 다음 색 인덱스로 이동
            currentIndex = (currentIndex + 1) % colors.Length;

            // 3초 대기
            yield return new WaitForSeconds(2f);
        }
    }
}
