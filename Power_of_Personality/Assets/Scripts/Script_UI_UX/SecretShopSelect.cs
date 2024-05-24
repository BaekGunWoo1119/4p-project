using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SecretShopSelect : MonoBehaviour
{
    public GameObject eventSystem;
    public SecretShop secretShop;
    // Start is called before the first frame update
    void Start()
    {
        //eventSystem의 SecretShop 받아옴
        eventSystem = GameObject.Find("EventSystem");
        secretShop = eventSystem.GetComponent<SecretShop>();
        Button btn = GameObject.Find("Pick").GetComponent<Button>();
        btn.onClick.AddListener(PickatRandom);
    }

    public void PickatRandom()
    {
        Debug.Log("픽");
        /*
        for(int i = 1; i <= 10; i++)
        {
            secretShop.SlowRotate(0.3f);
            Debug.Log("느려져라");
        }

        secretShop.StopAtRandom();
        */
        StartCoroutine(PickItem());
    }

    public IEnumerator PickItem()
    {
        float count = 1;
        while(count <= 10)
        {
            eventSystem.GetComponent<SecretShop>().SlowRotate(0.02f);
            count = count + 1;
            Debug.Log(count);
            yield return new WaitForSeconds(1.0f);
            if(count >= 10)
            {
                Debug.Log("멈춰");
                eventSystem.GetComponent<SecretShop>().StopAtRandom();
                yield break;
            }
        }
    }
}
