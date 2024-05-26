using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopChange : MonoBehaviour
{
    public GameObject[] actObj;
    private bool changeleft = false;
    private bool changeright = false;

    void Update()
    {
        /*
        actObj[0].GetComponent<RectTransform>().localPosition = actObj[1].transform.position;
        actObj[2].GetComponent<RectTransform>().localPosition = actObj[3].transform.position;
        
        애니메이션 오류로 딸깍으로 구현해 둠(추후 구현 예정)
        if(changeleft == true)
        {
            Vector3 vel = Vector3.zero;
            actObj[1].transform.position = Vector3.SmoothDamp(actObj[1].transform.position, new Vector3(2000, 0, 0), ref vel, 0.03f);
            actObj[3].transform.position = Vector3.SmoothDamp(actObj[3].transform.position, new Vector3(0, 0, 0), ref vel, 0.03f);
        }
        
        if(changeright == true)
        {
            Vector3 vel = Vector3.zero;
            actObj[3].transform.position = Vector3.SmoothDamp(actObj[3].transform.position, new Vector3(-2000, 0, 0), ref vel, 0.03f);
            actObj[1].transform.position = Vector3.SmoothDamp(actObj[1].transform.position, new Vector3(0, 0, 0), ref vel, 0.03f);
        }

        if(actObj[1].transform.position.x > 500)
        {
            changeleft = false;
        }

        if(actObj[3].transform.position.x < -500)
        {
            changeright = false;
        }
        */

        if(changeleft == true)
        {
            actObj[0].GetComponent<RectTransform>().localPosition = new Vector3(2000, 0, 0);
            actObj[2].GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        }
        if(changeright == true)
        {
            actObj[0].GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            actObj[2].GetComponent<RectTransform>().localPosition = new Vector3(-2000, 0, 0);
        }
        
        if(actObj[0].GetComponent<RectTransform>().localPosition.x == 2000)
        {
            changeleft = false;
        }

        if(actObj[2].GetComponent<RectTransform>().localPosition.x == -2000)
        {
            changeright = false;
        }
    }

    // Update is called once per frame
    public void ArrowClickLeft()
    {
        changeleft = true;
    }

    public void ArrowClickRight()
    {
        changeright = true;
    }
}
