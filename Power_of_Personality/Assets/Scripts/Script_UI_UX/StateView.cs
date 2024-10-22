using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateView : MonoBehaviour
{
    public string[] state; 
    // Start is called before the first frame update
    void Start()
    {
        state[0] = Status.TotalAD.ToString();
        state[1] = Status.TotalArmor.ToString();
        state[2] = Status.TotalADC.ToString();
        state[3] = Status.TotalAP.ToString();
        state[4] = Status.TotalFire.ToString();
        state[5] = Status.TotalIce.ToString();
        state[6] = Status.TotalSpeed.ToString();
        state[7] = Status.TotalCooltime.ToString();

        for(int i = 0; i <= 7; i++)
        {
            state[i] = GameObject.Find("EventSystem").GetComponent<TextChange>().innerText[i];
            Debug.Log(state[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
