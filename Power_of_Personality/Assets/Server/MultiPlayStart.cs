using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리

public class MultiPlayStart : MonoBehaviourPunCallbacks
{
    public Transform SpawnPoint1;
    public Transform SpawnPoint2;
    public Transform SpawnPoint3;
    private static GameObject Player_Character;
    // private GameObject Monster1;
    // private GameObject Monster2;
    // private GameObject Monster3;
    public static string PlayerClass;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
    }
 
    // Update is called once per frame
    void Update()
    {
        Debug.Log(GameObject.FindGameObjectsWithTag("Monster"));
        if(GameObject.FindGameObjectsWithTag("Monster").Length < 1)
        {
            //SpawnMonster();
        }
    }

    void SpawnMonster()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Server/Monster/Prefab_Cave/Server_Spider", SpawnPoint1.position, SpawnPoint1.rotation, 0);
            PhotonNetwork.Instantiate("Server/Monster/Prefab_Cave/Server_Mimic", SpawnPoint2.position, SpawnPoint2.rotation, 0);
            PhotonNetwork.Instantiate("Server/Monster/Prefab_Cave/Server_Golem", SpawnPoint3.position, SpawnPoint3.rotation, 0);
        }
    }

    public static void SpawnPlayer()
    {
        PlayerClass = PlayerPrefs.GetString("PlayerClass");
        MultiPlayStart instance = FindObjectOfType<MultiPlayStart>(); // Find an instance of the class to access instance members
        if (instance != null)
        {
            if (PlayerClass == "Wizard")
            {
                Player_Character = PhotonNetwork.Instantiate("Server/Wizard/Server_Player_wizard", instance.SpawnPoint1.position, instance.SpawnPoint1.rotation, 0);
            }
            else if (PlayerClass == "Archer")
            {
                Player_Character = PhotonNetwork.Instantiate("Server/Archer/Server_Player_archer", instance.SpawnPoint1.position, instance.SpawnPoint1.rotation, 0);
            }
            else if (PlayerClass == "Warrior")
            {
                Player_Character = PhotonNetwork.Instantiate("Server/Warrior/Server_Player_warrior", instance.SpawnPoint1.position, instance.SpawnPoint1.rotation, 0);
            }
            else if (PlayerClass == "Rogue")
            {
                Player_Character = PhotonNetwork.Instantiate("Server/Rogue/Server_Player_rogue", instance.SpawnPoint1.position, instance.SpawnPoint1.rotation, 0);
            }
            Player_Character.transform.SetParent(instance.transform);
        }
    }
}
