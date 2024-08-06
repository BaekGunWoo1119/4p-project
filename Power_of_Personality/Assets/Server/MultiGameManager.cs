using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리

public class MultiGameManager : MonoBehaviourPunCallbacks
{
    public int WaveTime; // 웨이브 시간
    public float CurrentTime; // 현재 시간
    public int CurrentWave; // 현재 웨이브
    public int MonsterCount; // 현재 몬스터 수
    public int Spawned; // 생성한 몬스터 수
    public int TargetSpawn; // 목표 몬스터 수
    public float RespawnTime = 3.0f; // 몬스터 리스폰 시간
    public float TempTime; // 몬스터 저장용 시간
    public float sec; // 타이머용 시간
    public int min; // 타이머용 시간
    public Transform[] SpawnPoints; // 몬스터 스폰 포인트들
    public Transform[] StageSpawnPoints; // 스테이지별 스폰 포인트들
    public Transform ShopTr; //상점 위치
    public TMP_Text timerText;
    public GameObject Player;
    public bool IsDie; //죽었는지 판단
    private WaveDatas JSONWaveList; // JSON에서 받아온 웨이브 데이터
    public bool IsWave; //현재 웨이브 진행중인지

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
        TempTime = 0f;
        CurrentWave = 1;
        sec = 0f;
        min = 0;
        Spawned = 0; // 초기화 추가

        string jsondata = Resources.Load<TextAsset>("JSON/WaveData").text;
        // JSON 데이터를 WaveDatas 클래스로 Deserialize
        JSONWaveList = JsonUtility.FromJson<WaveDatas>(jsondata);
        WaveUpdate(); // 초기 웨이브 설정
    }

    void Update()
    {
        if (!Player == null){
            Player = GameObject.FindWithTag("Player"); //플레이어 오브젝트 찾기
        }
        #region 인게임 타이머
        sec += Time.deltaTime;
        if (sec >= 60f)
        {
            min += 1;
            sec = 0;
        }

        // 인게임에 UI 넣어야 됨 고민해야 될 내용(수정완료)
        timerText.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);
        #endregion

        CurrentTime += Time.deltaTime;
        TempTime += Time.deltaTime;
        MonsterCount = GameObject.FindGameObjectsWithTag("Monster").Length; // 몬스터 수 카운트

        // 웨이브 시간 종료
        if (CurrentTime > WaveTime)
        {
            // 몬스터가 0명이라면
            if (GameObject.FindGameObjectsWithTag("Monster").Length < 1)
            {
                EndWave();
                WaveUpdate();
            }
        }
        else
        {
            // 목표 몬스터 채울 때까지 생성
            if (Spawned < TargetSpawn && TempTime >= RespawnTime)
            {
                SpawnMonster();
            }
        }
        //웨이브 끝났을 때 상점나왔는지 체크
        if( IsWave == false){
            if(CheckExitShop() == true){
                //startwave();
            }
        }
    }

    // 웨이브 종료 후 상점으로 이동
    void EndWave()
    {
        IsWave = false;
        // 체력 회복
        Status.HP = Status.MaxHP;
        CurrentWave += 1;
        CurrentTime = 0f; // 초기화 추가
        WaveUpdate(); // 다음 웨이브 설정
        // 상점으로 이동 (상점 스크립트 바꿔서 적용해야 할 듯)
        // 상점에서 나가면은 레디가 되도록 스크립트 새로 만들던가
        Player.transform.position =ShopTr.position;
    }

    // 몬스터 스폰
    void SpawnMonster()
    {   
        //호스트에서 몹 소환하도록
        if (PhotonNetwork.IsMasterClient)
        {
            int spawnIndex = Random.Range(0, SpawnPoints.Length);
            Transform spawnPoint = SpawnPoints[spawnIndex];

            var currentWaveData = JSONWaveList.WaveData[CurrentWave - 1];
            string monsterPrefab = (Spawned % 2 == 0) ? currentWaveData.Monster1 : currentWaveData.Monster2;

            PhotonNetwork.Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation, 0);
            Spawned += 1; // 생성한 몬스터 수 증가
        }
        TempTime = 0f; // 리스폰 대기시간 초기화
    }

    //스폰포인트로 플레이어 이동 후 웨이브 시작
    void StartWave(){
        LobbyManager.PlayerProperties["IsExitShop"]=false;
        IsWave = true;
        switch (CurrentWave){
            case 1: 
            Player.transform.position = StageSpawnPoints[0].position;
            break;
            case 2: 
            Player.transform.position = StageSpawnPoints[0].position;
            break;
            case 3: 
            Player.transform.position = StageSpawnPoints[0].position;
            break;
            case 4: 
            Player.transform.position = StageSpawnPoints[1].position;
            break;
            case 5: 
            Player.transform.position = StageSpawnPoints[1].position;
            break;
            case 6: 
            Player.transform.position = StageSpawnPoints[1].position;
            break;
            case 7: 
            Player.transform.position = StageSpawnPoints[2].position;
            break;
            case 8: 
            Player.transform.position = StageSpawnPoints[2].position;
            break;
            case 9: 
            Player.transform.position = StageSpawnPoints[2].position;
            break;
            case 10: 
            Player.transform.position = StageSpawnPoints[3].position;
            break;
            case 11: 
            Player.transform.position = StageSpawnPoints[3].position;
            break;
            case 12: 
            Player.transform.position = StageSpawnPoints[3].position;
            break;
            
        }
    }

    void ExitShop(){
        //플레이어가 상점 나오면 서버로 응답 보냄
        if (PlayerCtrl.isShop == false){
            LobbyManager.PlayerProperties["IsExitShop"]=true;
        }
    }

    public bool CheckExitShop(){
        bool IsAllReady = true;
        //플레이어 모두가 레디라면은
        foreach(Player Player in PhotonNetwork.PlayerList){
            if((bool)Player.CustomProperties["IsExitShop"] == false){
                IsAllReady = false;
            };
        }
        return IsAllReady;
    }
}
