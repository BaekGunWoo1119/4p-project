using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPlayer : MonoBehaviour
{
    //public GameObject TurnPoint;
    public GameObject CurMap;
    public GameObject NextMap;
    //public GameObject CurMapClass;
    //public GameObject NextMapClass;

    public GameObject Player;
    //private bool IsNext = false;
    //private float PlayerYRot;
    //private PlayerCtrl_Rogue playerCtrl;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        // PlayerCtrl_Rogue playerCtrl = Player.GetComponent<PlayerCtrl_Rogue>();
        // PlayerYRot = playerCtrl.YRot;

        //Quaternion desiredRotation = Quaternion.Euler(0, YRot, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(NextMapClass.transform.eulerAngles.y);
    }

    void OnTriggerExit(Collider col) {
        //Debug.Log(col);
        if(col.CompareTag("Player")){
            Debug.Log("PlayerENTER");
            //if(IsNext == false){
            if(Player.transform.localEulerAngles.y > 60 && Player.transform.localEulerAngles.y < 120){
            Player.transform.SetParent(NextMap.transform);
            //Player.transform.localRotation = Quaternion.Euler(0,90,0);
            //Player.transform.position=NextMap.transform.position;
            //IsNext = true;
            //Debug.Log(Player.transform.localEulerAngles.y);
            }
            else{
            Player.transform.SetParent(CurMap.transform);
            //Player.transform.localRotation = Quaternion.Euler(0,-90,0);
            //Player.transform.position=CurMap.transform.position;
            //IsNext = false;
            //Debug.Log(Player.transform.localEulerAngles.y);
            }
        }
    }
}