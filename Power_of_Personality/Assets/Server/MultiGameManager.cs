using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리

public class MultiGameManager : MonoBehaviourPunCallbacks
{
    public float WaveTime; // 웨이브 시간
    public float CurrentTime; // 현재 시간
    public int CurrentWave; // 현재 웨이브
    public int MonsterCount; // 현재 몬스터 수
    public int Spawned; // 생성한 몬스터 수
    public int TargetSpawn; // 목표 몬스터 수
    public float RespawnTime = 3.0f; // 몬스터 리스폰 시간
    public float TempTime; // 몬스터 저장용 시간
    public float sec; // 타이머용 시간
    public int min; // 타이머용 시간
    public static Transform[] SpawnPoints; // 현재 스테이지 몬스터 스폰 포인트들
    public Transform[] SpawnPoints1; // 1스테이지 몬스터 스폰 포인트들
    public Transform[] SpawnPoints2; // 2스테이지 몬스터 스폰 포인트들
    public Transform[] SpawnPoints3; // 3스테이지 몬스터 스폰 포인트들
    public Transform[] SpawnPoints4; // 4스테이지 몬스터 스폰 포인트들
    public Transform[] StageSpawnPoints; // 스테이지별 스폰 포인트들
    public Transform ShopTr; // 상점 위치
    public TMP_Text timerText;
    public GameObject Player;
    public static bool IsDie; // 죽었는지 판단
    private WaveDatas JSONWaveList; // JSON에서 받아온 웨이브 데이터
    public bool IsWave; // 현재 웨이브 진행 중인지
    public Collider PlayerCol; // 플레이어 콜라이더
    public GameObject Watching; //관전 UI
    public GameObject WaitPlayer; //상점 대기 UI
    public PhotonView photonview;
    public bool IsAllLoad; //플레이어 다 들어왔나 체크
    //public static Hashtable PlayerProperties = new Hashtable(); //로컬플레이어 프로퍼티

    public InventoryCtrl InvenCtrl;

    #region JSON 관련 스크립트
    // JSON 데이터를 저장할 클래스 정의
    [System.Serializable]
    public class WaveDatas
    {
        public List<Wave> WaveData;
    }

    [System.Serializable]
    public class Wave
    {
        public int Quantity; // 몬스터 수
        public string Monster1; // 등장 몬스터 1
        public string Monster2; // 등장 몬스터 2
    }

    void WaveUpdate()
    {
        if (CurrentWave - 1 < JSONWaveList.WaveData.Count)
        {
            var currentWaveData = JSONWaveList.WaveData[CurrentWave - 1];
            TargetSpawn = currentWaveData.Quantity;
            Spawned = 0; // 초기화 추가
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.SerializationRate = 60;
        PhotonNetwork.SendRate = 60;
        WaveTime = 10f;
        CurrentTime = 0f;
        TempTime = 0f;
        CurrentWave = 1;
        sec = 0f;
        min = 0;
        Spawned = 0; // 초기화 추가
        IsDie = false;
        photonview = GetComponent<PhotonView>();
        Status.HP = Status.MaxHP;
        SpawnPoints = SpawnPoints1;
        InvenCtrl = GameObject.Find("InventoryCtrl").GetComponent<InventoryCtrl>();

        var properties = PhotonNetwork.LocalPlayer.CustomProperties;
        properties["IsExitShop"] = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        string jsondata = Resources.Load<TextAsset>("JSON/WaveData_test").text;
        // JSON 데이터를 WaveDatas 클래스로 Deserialize
        JSONWaveList = JsonUtility.FromJson<WaveDatas>(jsondata);
        WaveUpdate(); // 초기 웨이브 설정
        InvenCtrl.ResetInven();
    }

    void Update()
    {
        if(PhotonNetwork.PlayerList.Length==GameObject.FindGameObjectsWithTag("Target").Length&&IsAllLoad==false){
            IsAllLoad=true;
            SetPlayer();
            StartWave();
        }
        if(IsAllLoad==true){
            //Debug.Log("Status.IsShop = "+Status.IsShop);
            SetPlayer();

            #region 인게임 타이머
            sec += Time.deltaTime;
            if (sec >= 60f)
            {
                min += 1;
                sec = 0;
            }

            timerText.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);
            #endregion

            CurrentTime += Time.deltaTime;
            TempTime += Time.deltaTime;
            MonsterCount = GameObject.FindGameObjectsWithTag("Monster").Length; // 몬스터 수 카운트

            //웨이브 중일때
            if(IsWave == true){
                // 웨이브 시간, 목표 스폰량 종료
                if (CurrentTime > WaveTime && Spawned >= TargetSpawn)
                {
                    // 몬스터가 0명이라면
                    if (MonsterCount < 1)
                    {
                        IsWave = false;
                        if (PhotonNetwork.IsMasterClient){
                            if(CurrentWave>12){
                                photonview.RPC("GameClear",RpcTarget.All);
                            }
                            else{
                            photonview.RPC("EndWave",RpcTarget.All);
                            }
                        }
                        WaveUpdate();
                    }
                }
                else
                {
                    // 일정 주기로 몬스터 생성
                    if (TempTime >= RespawnTime)
                    {
                        SpawnMonster();
                    }
                }
            }

            // 웨이브 끝났을 때 상점 나왔는지 체크
            if (IsWave == false)
            {
                // 플레이어가 상점에서 나오면 서버로 응답 보냄
                if (Status.IsShop == false)
                {
                    ExitShop();
                    if (CheckExitShop() == true)
                    {
                        if (PhotonNetwork.IsMasterClient){
                            photonview.RPC("StartWave",RpcTarget.All);
                        }
                    }
                }
                
            }
            //죽으면 다른 플레이어 관전(UI추가해야함)
            if (Status.HP<=0){
                //내가 죽어있을 떄 다른 플레이어도 죽어있다면
                if(GameObject.FindGameObjectWithTag("OtherPlayer") == null){
                    photonview.RPC("GameOver",RpcTarget.All);
                }
                else{
                    Watching.SetActive(true);
                    CameraCtrl.target = GameObject.FindGameObjectWithTag("OtherPlayer").transform;
                }
            }
            else {
                IsDie = false;
                if(GameObject.FindGameObjectWithTag("Player") != null)
                {
                    SetPlayer();
                    Watching.SetActive(false);
                    CameraCtrl.target = GameObject.FindGameObjectWithTag("Player").transform;
                }
            }
        }
    }

    [PunRPC]
    // 웨이브 종료 후 상점으로 이동
    void EndWave()
    {
        CurrentWave += 1;
        Status.HP = Status.MaxHP;
        if (IsDie == true){
            IsDie = false;
            MultiPlayStart.Instance.SpawnPlayer(SpawnPoints[1]);
            SetPlayer();
        }
        IsWave = false;
        // 체력 회복
        CurrentTime = 0f; // 초기화 추가
        WaveUpdate(); // 다음 웨이브 설정
        // 상점으로 이동
        GameObject.Find("EventSystem").GetComponent<Shop_PortalCtrl>().Open_Shop(PlayerCol);
        Player.transform.position = ShopTr.position;
    }

    // 몬스터 스폰
    void SpawnMonster()
    {
        // 호스트에서 몹 소환하도록
        if (PhotonNetwork.IsMasterClient)
        {

            switch (CurrentWave)
            {
                case 1:
                case 2:
                case 3:
                    SpawnPoints = SpawnPoints1;
                    break;
                case 4:
                case 5:
                case 6:
                    SpawnPoints = SpawnPoints2;
                    break;
                case 7:
                case 8:
                case 9:
                    SpawnPoints = SpawnPoints3;
                    break;
                case 10:
                case 11:
                case 12:
                    SpawnPoints = SpawnPoints4;
                    break;
            }
            int spawnIndex = Random.Range(0, SpawnPoints.Length);
            Transform spawnPoint = SpawnPoints[spawnIndex];
            var currentWaveData = JSONWaveList.WaveData[CurrentWave - 1];
            string monsterPrefab = (Spawned % 2 == 0) ? currentWaveData.Monster1 : currentWaveData.Monster2;
            PhotonNetwork.Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation, 0);
            Spawned += 1; // 생성한 몬스터 수 증가
        }
        TempTime = 0f; // 리스폰 대기시간 초기화
    }

    [PunRPC]
    // 스폰포인트로 플레이어 이동 후 웨이브 시작
    void StartWave()
    {
        if(IsWave ==false){
            int playerViewID = Player.GetComponent<PhotonView>().ViewID;
            int SpawnPointViewID;
            min = 0;
            sec = 0f;
            WaitPlayer.SetActive(false);
            var properties = PhotonNetwork.LocalPlayer.CustomProperties;
            properties["IsExitShop"] = false;
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

            IsWave = true;
            switch (CurrentWave)
            {
                case 1:
                case 2:
                case 3:
                    StageSpawnPoints[0].tag="CurrentSpawnPotint";
                    SpawnPointViewID = StageSpawnPoints[0].gameObject.GetComponent<PhotonView>().ViewID;
                    MultiPlayStart.Instance.PlayerSetParent(playerViewID,SpawnPointViewID);
                    Player.transform.position = StageSpawnPoints[0].position;
                    break;
                case 4:
                case 5:
                case 6:
                    StageSpawnPoints[0].tag="Untagged";
                    StageSpawnPoints[1].tag="CurrentSpawnPotint";
                    SpawnPointViewID = StageSpawnPoints[1].gameObject.GetComponent<PhotonView>().ViewID;
                    MultiPlayStart.Instance.PlayerSetParent(playerViewID,SpawnPointViewID);
                    Player.transform.position = StageSpawnPoints[1].position;
                    break;
                case 7:
                case 8:
                case 9:
                    StageSpawnPoints[1].tag="Untagged";
                    StageSpawnPoints[2].tag="CurrentSpawnPotint";
                    SpawnPointViewID = StageSpawnPoints[2].gameObject.GetComponent<PhotonView>().ViewID;
                    MultiPlayStart.Instance.PlayerSetParent(playerViewID,SpawnPointViewID);
                    Player.transform.position = StageSpawnPoints[2].position;
                    break;
                case 10:
                case 11:
                case 12:
                    StageSpawnPoints[2].tag="Untagged";
                    StageSpawnPoints[3].tag="CurrentSpawnPotint";
                    SpawnPointViewID = StageSpawnPoints[3].gameObject.GetComponent<PhotonView>().ViewID;
                    MultiPlayStart.Instance.PlayerSetParent(playerViewID,SpawnPointViewID);
                    Player.transform.position = StageSpawnPoints[3].position;
                    break;
            }
        }
    }

    public void ExitShop()
    {
        
            var properties = PhotonNetwork.LocalPlayer.CustomProperties;
            properties["IsExitShop"] = true;
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
            WaitPlayer.SetActive(true); 
            //Debug.Log("CheckExitShop()="+CheckExitShop());
    
    }

    public void SetPlayer(){
        if (Player == null)
            {
                Player = GameObject.FindWithTag("Player"); // 플레이어 오브젝트 찾기
                if (Player != null)
                {
                    PlayerCol = Player.GetComponent<Collider>();
                    if (PlayerCol == null)
                    {
                        //Debug.LogWarning("Player 오브젝트에 Collider가 없습니다.");
                    }
                }
                else
                {
                    //Debug.LogWarning("Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
                }
            }
    }
    public bool CheckExitShop()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("IsExitShop", out var isExitShop) && isExitShop is bool exitShopFlag)
            {
                if (!exitShopFlag)
                {
                    return false; // 한 명이라도 나가지 않았으면 바로 false 반환
                }
            }
            else
            {
                return false; // IsExitShop이 없거나 잘못된 경우도 false 반환
            }
        }
        return true; // 모든 플레이어가 나갔으면 true 반환
    }

    [PunRPC]
    public void GameOver(){
        //게임오버 넣어야됨
        PhotonNetwork.LoadLevel("1-2 (Multi Lobby)");
        IsDie=false;
        Status.isDie();
    }

    [PunRPC]
    public void GameClear(){
        //GameClear.SetActive(true);
        //delay
        Status.isDie();
        PhotonNetwork.LoadLevel("1-2 (Multi Lobby)");
        IsDie=false;
    }
}
