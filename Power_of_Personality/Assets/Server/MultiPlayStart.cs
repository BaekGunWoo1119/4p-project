using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리

public class MultiPlayStart : MonoBehaviourPunCallbacks
{
    public static MultiPlayStart Instance = null; // 싱글톤 인스턴스

    public Transform SpawnPoint1;
    public Transform SpawnPoint2;
    public Transform SpawnPoint3;
    private GameObject Player_Character;
    public string PlayerClass;

    private PhotonView photonview; //포톤뷰 (멀티)

    void Awake()
    {
        photonview = GetComponent<PhotonView>();
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않게 설정
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 중복 생성 방지
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerClass = PlayerPrefs.GetString("PlayerClass");
        SpawnPlayer();
    }
 
    void Update()
    {
        //Debug.Log(GameObject.FindGameObjectsWithTag("Monster"));
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

    public void SpawnPlayer()
    {
        Debug.Log("SpawnPlayer");
        if (PlayerClass == "Wizard")
        {
            Player_Character = PhotonNetwork.Instantiate("Server/Wizard/Server_Player_wizard", SpawnPoint1.position, SpawnPoint1.rotation, 0);
            Debug.Log("Wizard");
        }
        else if (PlayerClass == "Archer")
        {
            Player_Character = PhotonNetwork.Instantiate("Server/Archer/Server_Player_archer", SpawnPoint1.position, SpawnPoint1.rotation, 0);
            Debug.Log("Archer");
        }
        else if (PlayerClass == "Warrior")
        {
            Player_Character = PhotonNetwork.Instantiate("Server/Warrior/Server_Player_warrior", SpawnPoint1.position, SpawnPoint1.rotation, 0);
            Debug.Log("Warrior");
        }
        else if (PlayerClass == "Rogue")
        {
            Player_Character = PhotonNetwork.Instantiate("Server/Rogue/Server_Player_rogue", SpawnPoint1.position, SpawnPoint1.rotation, 0);
            Debug.Log("Rogue");
        }
        if (Player_Character != null)
        {
            int playerViewID = Player_Character.GetComponent<PhotonView>().ViewID;
            photonview.RPC("PlayerSetParent", RpcTarget.All, playerViewID);
        }
    }

    [PunRPC]
    public void PlayerSetParent(int playerViewID)
    {
        GameObject playerCharacter = PhotonView.Find(playerViewID).gameObject;
        if (playerCharacter != null)
        {
            playerCharacter.transform.SetParent(this.transform);
        }
        else
        {
            Debug.LogError("PlayerSetParent: Could not find GameObject with PhotonViewID " + playerViewID);
        }
    }
}
