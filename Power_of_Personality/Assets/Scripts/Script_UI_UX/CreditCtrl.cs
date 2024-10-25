using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditCtrl : MonoBehaviour
{
    public GameObject credit;
    public GameObject backBtn;
    private Vector3 endPos;
    private Vector3 velocity;
    public float upSpeed = 66.0f;
    // Start is called before the first frame update
    void Start()
    {
        endPos = new Vector3(Screen.width / 2, Screen.height * 7,0);
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(credit != null)
        {
            credit.transform.position = Vector3.SmoothDamp(credit.transform.position, endPos, ref velocity, upSpeed);

            if(credit.transform.position.y > Screen.height * 6.5)
            {
                Credit_Close();
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "1 (Main)")
            Credit_Close();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(upSpeed > 2.0f)
            {
                upSpeed -= 2.0f;
            }
            else
            {
                upSpeed = 1.5f;
            }
        }
    }

    public void Credit_Open()
    {
        SceneManager.LoadScene("Credit");
    }

    public void Credit_Close()
    {
        SceneManager.LoadScene("1 (Main)");
    }
}
