using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TurnPlayer : MonoBehaviour
{
    public GameObject CurMap;
    public GameObject NextMap;
    public GameObject MainCamera;
    public GameObject bfCamera;
    public GameObject CurCamera;
    public GameObject NextCamera;
    public GameObject Player;

    void Start()
    {
        StartCoroutine(FindPlayer());
        if(CurCamera != null && NextCamera != null)
        {
            CurCamera.SetActive(false);
            NextCamera.SetActive(false);
        }
        // PlayerCtrl_Rogue playerCtrl = Player.GetComponent<PlayerCtrl_Rogue>();
        // PlayerYRot = playerCtrl.YRot;

        //Quaternion desiredRotation = Quaternion.Euler(0, YRot, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(NextMapClass.transform.eulerAngles.y);
    }

    void OnTriggerExit(Collider col) 
    {
        //Debug.Log(col);
        if(col.CompareTag("Player"))
        {
            Debug.Log("PlayerENTER");
            //if(IsNext == false){
            if(Player.transform.localEulerAngles.y > 60 && Player.transform.localEulerAngles.y < 120)
            {
                if(CurCamera != null && NextCamera != null && Player.transform.parent.gameObject != NextMap)
                {
                    StartCoroutine(TurnCamera(NextCamera));
                    Player.transform.SetParent(NextMap.transform);
                }

                //Player.transform.SetParent(NextMap.transform);
                //Player.transform.localRotation = Quaternion.Euler(0,90,0);
                //Player.transform.position=NextMap.transform.position;
                //IsNext = true;
                //Debug.Log(Player.transform.localEulerAngles.y);
            }
            else
            {
                if(CurCamera != null && NextCamera != null  && Player.transform.parent.gameObject != CurMap)
                {
                    StartCoroutine(TurnCamera(CurCamera));
                    Player.transform.SetParent(CurMap.transform);
                }

                //Player.transform.SetParent(CurMap.transform);
                //Player.transform.localRotation = Quaternion.Euler(0,-90,0);
                //Player.transform.position=CurMap.transform.position;
                //IsNext = false;
                //Debug.Log(Player.transform.localEulerAngles.y);
            }
        }
    }

    IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(0.2f);
        Player = GameObject.FindWithTag("Player");
    }

    IEnumerator TurnCamera(GameObject Camera)
    {
        if(Camera == CurCamera)
        {
            MainCamera.GetComponent<CameraCtrl>().SetCamera(0);
            NextCamera.SetActive(true);
            //bfCamera.SetActive(false);
            yield return new WaitForSeconds(0.01f);
            //Camera.SetActive(true);
            bfCamera.SetActive(false);
            yield return new WaitForSeconds(0.01f);
            Camera.SetActive(true);
            //NextCamera.SetActive(false);
            yield return new WaitForSeconds(0.6f);
            MainCamera.GetComponent<CameraCtrl>().SetCamera(0);
            NextCamera.SetActive(false);
        }
        else if(Camera == NextCamera)
        {
            MainCamera.GetComponent<CameraCtrl>().SetCamera(0);
            CurCamera.SetActive(true);
            //bfCamera.SetActive(false);
            yield return new WaitForSeconds(0.01f);
            //Camera.SetActive(true);
            bfCamera.SetActive(false);
            yield return new WaitForSeconds(0.01f);
            Camera.SetActive(true);
            //CurCamera.SetActive(false);
            yield return new WaitForSeconds(0.6f);
            MainCamera.GetComponent<CameraCtrl>().SetCamera(0);
            CurCamera.SetActive(false);
        }
    }
}