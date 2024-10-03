using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Server_ChangeEffect : MonoBehaviourPun
{
    public GameObject fireCircle;
    public GameObject iceCircle;
    public GameObject fireSpin;
    public GameObject iceSpin;
    private PhotonView photonview;
    private string CurProperty;
    

    // Start is called before the first frame update
    void Start()
    {
        photonview = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(photonview.IsMine)
        {
            CurProperty = PlayerPrefs.GetString("property");
            photonview.RPC("SetEffect",RpcTarget.All,CurProperty);
        }
    }

    IEnumerator Effect(string EffectType)
    {
        if(EffectType == "Ice")
        {
            iceCircle.SetActive(true);
            fireCircle.SetActive(false);
            iceSpin.SetActive(true);
            fireSpin.SetActive(false);
        }
        else if(EffectType == "Fire")
        {
            iceCircle.SetActive(false);
            fireCircle.SetActive(true);
            iceSpin.SetActive(false);
            fireSpin.SetActive(true);
        }

        yield break;
    }

    [PunRPC]
    void SetEffect(string property){
        if(property == "Ice"
        && iceCircle != null && iceSpin != null)
        {
            StartCoroutine(Effect("Ice"));
        }
        else if(property == "Fire"
        && fireCircle != null && fireSpin != null)
        {
            StartCoroutine(Effect("Fire"));
        }
    }
}
