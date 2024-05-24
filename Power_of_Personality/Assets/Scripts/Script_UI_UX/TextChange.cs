using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextChange : MonoBehaviour
{
    public string innerText;
    public TMP_Text changeText;
    
    //범용성 있게 둘 다 public으로 지정해둠. 필요한 곳에 넣어서 사용 가능
    protected virtual void Start()
    {
        innerText = changeText.text;
    }

    // 외부에서 텍스트 바꿀 때 이걸로 사용하면 됨
    public virtual void ChangeText(string othertext)
    {
        othertext = changeText.text;
    }
}
