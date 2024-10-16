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
    // Start is called before the first frame update
    void Start()
    {
        endPos = new Vector3(Screen.width / 2, Screen.height * 7,0);
        velocity = Vector3.zero;

        if(backBtn != null)
            StartCoroutine(BackBtnEnable());
    }

    // Update is called once per frame
    void Update()
    {
        if(credit != null)
            credit.transform.position = Vector3.SmoothDamp(credit.transform.position, endPos, ref velocity, 66.0f);

        if(Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "1-3 (Setting)")
            Credit_Close();
    }

    public void Credit_Open()
    {
        SceneManager.LoadScene("Credit");
    }

    IEnumerator BackBtnEnable()
    {
        backBtn.SetActive(false);
        yield return new WaitForSeconds(80f);
        backBtn.SetActive(true);
    }

    public void Credit_Close()
    {
        SceneManager.LoadScene("1-3 (Setting)");
    }
}
