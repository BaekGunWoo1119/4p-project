using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리

public class MultiPlayStart : MonoBehaviourPunCallbacks
{
    private string PlayerClass;
    // Start is called before the first frame update
    void Start()
    {
        PlayerClass = PlayerPrefs.GetString("PlayerClass");
        Transform SpawnPoint = GameObject.Find("SpawnPoint").GetComponent<Transform>();
        if(PlayerClass == "Wizard"){
            PhotonNetwork.Instantiate("Server/Wizard/Server_Player_wizard",SpawnPoint.position,SpawnPoint.rotation,0);
        }
        if(PlayerClass == "Archer"){
            PhotonNetwork.Instantiate("Server/Archer/Server_Player_archer",SpawnPoint.position,SpawnPoint.rotation,0);
        }
        if(PlayerClass == "Warrior"){
            PhotonNetwork.Instantiate("Server/Warrior/Server_Player_warrior",SpawnPoint.position,SpawnPoint.rotation,0);
        }
        if(PlayerClass == "Rogue"){
            PhotonNetwork.Instantiate("Server/Rogue/Server_Player_rogue",SpawnPoint.position,SpawnPoint.rotation,0);
        }

        if (PhotonNetwork.IsMasterClient){
            PhotonNetwork.Instantiate("Server/Monster/Cave/Server_Spider",SpawnPoint.position,SpawnPoint.rotation,0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
