using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPlayer : MonoBehaviour
{
    public GameObject TurnPoint;
    public GameObject CurMap;
    public GameObject NextMap;
    public GameObject CurMapClass;
    public GameObject NextMapClass;

    public GameObject Player;
    public bool IsNext = false;
    private float PlayerYRot;
    //private PlayerCtrl_Rogue playerCtrl;
    // Start is called before the first frame update
    void Start()
    {
        // PlayerCtrl_Rogue playerCtrl = Player.GetComponent<PlayerCtrl_Rogue>();
        // PlayerYRot = playerCtrl.YRot;

        //Quaternion desiredRotation = Quaternion.Euler(0, YRot, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(NextMapClass.transform.eulerAngles.y);
    }

    void OnTriggerEnter(Collider col) {
        Debug.Log(col);
        if(col.CompareTag("Player")){
            Debug.Log("PlayerENTER");
            if(IsNext == false){
            Player.transform.rotation = Quaternion.Euler(0, NextMapClass.transform.eulerAngles.y+90,0);
            Player.transform.position=NextMap.transform.position;
            IsNext = true;
            }
            else{
            Player.transform.rotation = Quaternion.Euler(0, CurMapClass.transform.eulerAngles.y-90,0);
            Player.transform.position=CurMap.transform.position;
            IsNext = false;
            }
        }
    }
}
    