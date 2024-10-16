using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SecretShopSelect : MonoBehaviour
{
    public GameObject eventSystem;
    public static bool hidden_pick;
    // Start is called before the first frame update

    void Start()
    {
        hidden_pick = false;
        //eventSystem의 SecretShop 받아옴
        eventSystem = GameObject.Find("EventSystem");
        Button btn = GameObject.Find("Pick_Hidden").GetComponent<Button>();
        btn.onClick.AddListener(PickatRandom);
    }

    public void PickatRandom()
    {
        eventSystem.GetComponent<SecretShop>().StartAtRandom();
        hidden_pick = true;
        StartCoroutine(PickItem());
    }

    public IEnumerator PickItem()
    {
        float count = 1;
        yield return new WaitForSeconds(1.0f);
        while(count <= 10)
        {
            eventSystem.GetComponent<SecretShop>().SlowRotate(0.02f);
            count = count + 1;
            Debug.Log(count);
            yield return new WaitForSeconds(0.6f);
            if(count >= 10)
            {
                Debug.Log("멈춰");
                eventSystem.GetComponent<SecretShop>().StopAtRandom();
                yield break;
            }
        }
    }
}
