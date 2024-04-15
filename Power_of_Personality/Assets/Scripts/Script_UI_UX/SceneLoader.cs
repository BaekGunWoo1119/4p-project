using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using TMPro;
//using UnityEditor.Experimental.GraphView;

public class SceneLoader : MonoBehaviour
{
    private string NowScene;
    private string PrevScene;

    private Button Main_Single;
    private Button Main_Multi;
    private Button Main_Setting;
    private Button Main_Exit;

    private Button MultiRoomChoice_CreateRoom;
    private Button MultiRoomChoice_JoinRoom;
    private Button MultiRoomChoice_Back;

    private Button MultiEnterCode_Select;
    private Button MultiEnterCode_Back;
    private TMP_InputField MultiEnterCode_LobbyCode;

    private Button MultiLobby_Ready;
    private Button MultiLobby_ClassSelect;
    private Button MultiLobby_Back;

    private Button Setting_Back;

    private Button MBTIChoice_INFP;
    private Button MBTIChoice_INFJ;
    private Button MBTIChoice_INTP;
    private Button MBTIChoice_INTJ;
    private Button MBTIChoice_ISFP;
    private Button MBTIChoice_ISFJ;
    private Button MBTIChoice_ISTP;
    private Button MBTIChoice_ISTJ;
    private Button MBTIChoice_ENFP;
    private Button MBTIChoice_ENFJ;
    private Button MBTIChoice_ENTP;
    private Button MBTIChoice_ENTJ;
    private Button MBTIChoice_ESFP;
    private Button MBTIChoice_ESFJ;
    private Button MBTIChoice_ESTP;
    private Button MBTIChoice_ESTJ;
    private Button MBTIChoice_Back;

    private Button ClassChoice_Warrior;
    private Button ClassChoice_Rogue;
    private Button ClassChoice_Archer;
    private Button ClassChoice_Wizard;
    private Button ClassChoice_Back;

    //캐릭터 프리펩
    public GameObject prf_Warrior;
    public GameObject prf_Rogue;
    public GameObject prf_Archer;
    public GameObject prf_Wizard;
    public Sprite img_Warrior;
    public Sprite img_Rogue;
    public Sprite img_Archer;
    public Sprite img_Wizard;

    private Button SpellChoice_Back;
    private Button SpellChoice_Select;
    private SkillSelect SkillSelectScript;
    public int selectedCount;

    private Button BonusStat_Back;
    private Button BonusStat_Start;

    private Button Exit_Game;
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "1 (Main)")
        {
            Main_Single = GameObject.Find("SinglePlay").GetComponent<Button>();
            Main_Multi = GameObject.Find("MultiPlay").GetComponent<Button>();
            Main_Setting = GameObject.Find("Settings").GetComponent<Button>();
            Main_Exit = GameObject.Find("Exit").GetComponent<Button>();

            if (Main_Single != null)
            {
                Main_Single.onClick.AddListener(Main_Single_Pressed);
            }
            if (Main_Multi != null)
            {
                Main_Multi.onClick.AddListener(Main_Multi_Pressed);
            }
            if (Main_Setting != null)
            {
                Main_Setting.onClick.AddListener(Main_Setting_Pressed);
            }
            if (Main_Exit != null)
            {
                Main_Exit.onClick.AddListener(Main_Exit_Pressed);
            }
        }

        if (SceneManager.GetActiveScene().name == "1-1-1 (Multi Room Choice)")
        {
            MultiRoomChoice_CreateRoom = GameObject.Find("CreateRoom").GetComponent<Button>();
            MultiRoomChoice_JoinRoom = GameObject.Find("JoinRoom").GetComponent<Button>();
            MultiRoomChoice_Back = GameObject.Find("Back").GetComponent<Button>();

            if (MultiRoomChoice_CreateRoom != null)
            {
                MultiRoomChoice_CreateRoom.onClick.AddListener(MultiRoomChoice_CreateRoom_Pressed);
            }
            if (MultiRoomChoice_JoinRoom != null)
            {
                MultiRoomChoice_JoinRoom.onClick.AddListener(MultiRoomChoice_JoinRoom_Pressed);
            }
            if (MultiRoomChoice_Back != null)
            {
                MultiRoomChoice_Back.onClick.AddListener(MultiRoomChoice_Back_Pressed);
            }
        }

        if (SceneManager.GetActiveScene().name == "1-1-2 (Multi Enter Code)")
        {
            MultiEnterCode_LobbyCode = GameObject.Find("LobbyCodeInput").GetComponent<TMP_InputField>();
            MultiEnterCode_Select = GameObject.Find("Select").GetComponent<Button>();
            MultiEnterCode_Back = GameObject.Find("Back").GetComponent<Button>();

            if (MultiEnterCode_Select != null)
            {
                MultiEnterCode_Select.onClick.AddListener(MultiEnterCode_Select_Pressed);
            }
            if (MultiEnterCode_Back != null)
            {
                MultiEnterCode_Back.onClick.AddListener(MultiEnterCode_Back_Pressed);
            }
        }

        if (SceneManager.GetActiveScene().name == "1-2 (Multi Lobby)")
        {
            MultiLobby_Ready = GameObject.Find("Ready").GetComponent<Button>();
            MultiLobby_ClassSelect = GameObject.Find("ClassSelect").GetComponent<Button>();
            MultiLobby_Back = GameObject.Find("Back").GetComponent<Button>();

            if(MultiLobby_Ready != null)
            {
                MultiLobby_Ready.onClick.AddListener(MultiLobby_Ready_Pressed);
            }
            if(MultiLobby_ClassSelect != null)
            {
                MultiLobby_ClassSelect.onClick.AddListener(MultiLobby_ClassSelect_Pressed);
            }
            if(MultiLobby_Back != null)
            {
                MultiLobby_Back.onClick.AddListener(MultiLobby_Back_Pressed);
            }
        }

        if (SceneManager.GetActiveScene().name == "1-3 (Setting)")
        {
            Setting_Back = GameObject.Find("Back").GetComponent<Button>();

            if (Setting_Back != null)
            {
                Setting_Back.onClick.AddListener(Setting_Back_Pressed);
            }
        }

        if (SceneManager.GetActiveScene().name == "2-1 (MBTI Choice)")
        {
            MBTIChoice_INFP = GameObject.Find("INFP").GetComponent<Button>();
            MBTIChoice_INFJ = GameObject.Find("INFJ").GetComponent<Button>();
            MBTIChoice_INTP = GameObject.Find("INTP").GetComponent<Button>();
            MBTIChoice_INTJ = GameObject.Find("INTJ").GetComponent<Button>();
            MBTIChoice_ISFP = GameObject.Find("ISFP").GetComponent<Button>();
            MBTIChoice_ISFJ = GameObject.Find("ISFJ").GetComponent<Button>();
            MBTIChoice_ISTP = GameObject.Find("ISTP").GetComponent<Button>();
            MBTIChoice_ISTJ = GameObject.Find("ISTJ").GetComponent<Button>();
            MBTIChoice_ENFP = GameObject.Find("ENFP").GetComponent<Button>();
            MBTIChoice_ENFJ = GameObject.Find("ENFJ").GetComponent<Button>();
            MBTIChoice_ENTP = GameObject.Find("ENTP").GetComponent<Button>();
            MBTIChoice_ENTJ = GameObject.Find("ENTJ").GetComponent<Button>();
            MBTIChoice_ESFP = GameObject.Find("ESFP").GetComponent<Button>();
            MBTIChoice_ESFJ = GameObject.Find("ESFJ").GetComponent<Button>();
            MBTIChoice_ESTP = GameObject.Find("ESTP").GetComponent<Button>();
            MBTIChoice_ESTJ = GameObject.Find("ESTJ").GetComponent<Button>();
            MBTIChoice_Back = GameObject.Find("Back").GetComponent<Button>();

            if (MBTIChoice_ENFP != null && MBTIChoice_ENTP != null && MBTIChoice_ESFP != null && MBTIChoice_ESTP != null)
            {
                MBTIChoice_ENFP.onClick.AddListener(ENFP_Selected);
                MBTIChoice_ENTP.onClick.AddListener(ENTP_Selected);
                MBTIChoice_ESFP.onClick.AddListener(ESFP_Selected);
                MBTIChoice_ESTP.onClick.AddListener(ESTP_Selected);
            }

            if (MBTIChoice_ENFJ != null && MBTIChoice_ENTJ != null && MBTIChoice_ESFJ != null && MBTIChoice_ESTJ != null)
            {
                MBTIChoice_ENFJ.onClick.AddListener(ENFJ_Selected);
                MBTIChoice_ENTJ.onClick.AddListener(ENTJ_Selected);
                MBTIChoice_ESFJ.onClick.AddListener(ESFJ_Selected);
                MBTIChoice_ESTJ.onClick.AddListener(ESTJ_Selected);
            }

            if (MBTIChoice_INFP != null && MBTIChoice_INTP != null && MBTIChoice_ISFP != null && MBTIChoice_ISTP != null)
            {
                MBTIChoice_INFP.onClick.AddListener(INFP_Selected);
                MBTIChoice_INTP.onClick.AddListener(INTP_Selected);
                MBTIChoice_ISFP.onClick.AddListener(ISFP_Selected);
                MBTIChoice_ISTP.onClick.AddListener(ISTP_Selected);
            }

            if (MBTIChoice_INFJ != null && MBTIChoice_INTJ != null && MBTIChoice_ISFJ != null && MBTIChoice_ISTJ != null)
            {
                MBTIChoice_INFJ.onClick.AddListener(INFJ_Selected);
                MBTIChoice_INTJ.onClick.AddListener(INTJ_Selected);
                MBTIChoice_ISFJ.onClick.AddListener(ISFJ_Selected);
                MBTIChoice_ISTJ.onClick.AddListener(ISTJ_Selected);
            }

            if (MBTIChoice_Back != null && PlayerPrefs.GetString("PlayMode") == "Single")
            {
                MBTIChoice_Back.onClick.AddListener(MBTIChoice_Back_Pressed);
            }

            else if (MBTIChoice_Back != null && PlayerPrefs.GetString("PlayMode") == "Multi")
            {
                MBTIChoice_Back.onClick.AddListener(MultiMBTIChoice_Back_Pressed);
            }
        }

        if (SceneManager.GetActiveScene().name == "2-2 (Class Choice)")
        {
            ClassChoice_Warrior = GameObject.Find("Warrior").GetComponent<Button>();
            ClassChoice_Rogue = GameObject.Find("Rogue").GetComponent<Button>();
            ClassChoice_Archer = GameObject.Find("Archer").GetComponent<Button>();
            ClassChoice_Wizard = GameObject.Find("Wizard").GetComponent<Button>();
            ClassChoice_Back = GameObject.Find("Back").GetComponent<Button>();
            if (ClassChoice_Warrior != null)
            {
                ClassChoice_Warrior.onClick.AddListener(ClassChoice_Class_Warrior_Pressed);
            }
            if (ClassChoice_Rogue != null)
            {
                ClassChoice_Rogue.onClick.AddListener(ClassChoice_Class_Rogue_Pressed);
            }
            if (ClassChoice_Archer != null)
            {
                ClassChoice_Archer.onClick.AddListener(ClassChoice_Class_Archer_Pressed);
            }
            if (ClassChoice_Wizard != null)
            {
                ClassChoice_Wizard.onClick.AddListener(ClassChoice_Class_Wizard_Pressed);
            }
            if (ClassChoice_Back != null)
            {
                ClassChoice_Back.onClick.AddListener(ClassChoice_Back_Pressed);
            }
        }

        if (SceneManager.GetActiveScene().name == "2-3 (Spell Choice)")
        {
            SkillSelectScript = FindObjectOfType<SkillSelect>();
            SpellChoice_Back = GameObject.Find("Back").GetComponent<Button>();
            SpellChoice_Select = GameObject.Find("Select").GetComponent<Button>();
            if (SpellChoice_Back != null)
            {
                SpellChoice_Back.onClick.AddListener(SpellChoice_Back_Pressed);
            }
            if(SpellChoice_Select != null)
            {
                SpellChoice_Select.onClick.AddListener(SpellChoice_Select_Pressed);
            }
        }

        if (SceneManager.GetActiveScene().name == "3 (Bonus Stat)")
        {
            BonusStat_Back = GameObject.Find("Back").GetComponent<Button>();
            BonusStat_Start = GameObject.Find("Start").GetComponent<Button>();

            if(BonusStat_Back != null)
            {
                BonusStat_Back.onClick.AddListener(BonusStat_Back_Pressed);
            }
            if(BonusStat_Start != null)
            {
                BonusStat_Start.onClick.AddListener(BonusStat_Start_Pressed);
            }
        }

        if(SceneManager.GetActiveScene().name == "Sewer_Example" ||
        SceneManager.GetActiveScene().name == "Forest_Example" ||
        SceneManager.GetActiveScene().name == "Cave_Example" ||
        SceneManager.GetActiveScene().name == "Castle_Example")
        {
            Exit_Game =  GameObject.Find("Exit").GetComponent<Button>();
            Setting_Back = GameObject.Find("OK").GetComponent<Button>();
            GameObject.Find("Player-MBTI").GetComponent<TMP_Text>().text = PlayerPrefs.GetString("PlayerMBTI");

            Time.timeScale = 1.0f;
            Exit_Game.onClick.AddListener(Setting_Back_Pressed);
            Setting_Back.onClick.AddListener(Setting_Back_Pressed);
            //캐릭터 선택 코드
            if(LobbyManager.PlayerProperties["PlayerClass"] == "Warrior")
            {
                if(prf_Warrior != null)
                {
                    GameObject spwPlayer = Instantiate(prf_Warrior, new Vector3(0,0,0), Quaternion.Euler(0f, 90, 0f));
                    GameObject parPlatfom = GameObject.FindWithTag("StartingPos");
                    spwPlayer.transform.parent = parPlatfom.transform;
                    GameObject.Find("CharImg").GetComponent<Image>().sprite = img_Warrior;
                    GameObject.Find("CharImg2").GetComponent<Image>().sprite = img_Warrior;
                }
            }
            else if(LobbyManager.PlayerProperties["PlayerClass"] == "Rogue")
            {
                if(prf_Rogue != null)
                {
                    GameObject spwPlayer = Instantiate(prf_Rogue, new Vector3(0,0,0), Quaternion.Euler(0f, 90, 0f));
                    GameObject parPlatfom = GameObject.FindWithTag("StartingPos");
                    spwPlayer.transform.parent = parPlatfom.transform;
                    GameObject.Find("CharImg").GetComponent<Image>().sprite = img_Rogue;
                    GameObject.Find("CharImg2").GetComponent<Image>().sprite = img_Rogue;
                }
            }
            else if(LobbyManager.PlayerProperties["PlayerClass"] == "Archer")
            {
                if(prf_Archer != null)
                {   
                    GameObject spwPlayer = Instantiate(prf_Archer, new Vector3(0,0,0), Quaternion.Euler(0f, 90, 0f));
                    GameObject parPlatfom = GameObject.FindWithTag("StartingPos");
                    spwPlayer.transform.parent = parPlatfom.transform;
                    GameObject.Find("CharImg").GetComponent<Image>().sprite = img_Archer;
                    GameObject.Find("CharImg2").GetComponent<Image>().sprite = img_Archer;
                }
            }
            else if(LobbyManager.PlayerProperties["PlayerClass"] == "Wizard")
            {
                if(prf_Wizard != null)
                {
                    GameObject spwPlayer = Instantiate(prf_Wizard, new Vector3(0,0,0), Quaternion.Euler(0f, 90, 0f));
                    GameObject parPlatfom = GameObject.FindWithTag("StartingPos");
                    spwPlayer.transform.parent = parPlatfom.transform;
                    GameObject.Find("CharImg").GetComponent<Image>().sprite = img_Wizard;
                    GameObject.Find("CharImg2").GetComponent<Image>().sprite = img_Wizard;
                }
            }   
        }
        

    }
    void Main_Single_Pressed()
    {
        PlayerPrefs.SetString("PlayMode", "Single");
        SceneManager.LoadScene("2-1 (MBTI Choice)");
    }
    void Main_Multi_Pressed()
    {
        PlayerPrefs.SetString("PlayMode", "Multi");
        LobbyManager.SeverConnect();
    }
    void Main_Setting_Pressed()
    {
        SceneManager.LoadScene("1-3 (Setting)");
    }
    void Main_Exit_Pressed()
    {
        Application.Quit();
    }
    void MultiRoomChoice_CreateRoom_Pressed()
    {
        LobbyManager.RoomCreate();
    }
    void MultiRoomChoice_JoinRoom_Pressed()
    {
        SceneManager.LoadScene("1-1-2 (Multi Enter Code)");
    }
    void MultiRoomChoice_Back_Pressed()
    {
        PlayerPrefs.SetString("PlayMode", "None");
        LobbyManager.DisConnect();
    }
    void MultiEnterCode_Select_Pressed()
    {
        LobbyManager.Connect((string)MultiEnterCode_LobbyCode.text);
        
    }
    void MultiEnterCode_Back_Pressed()
    {
        SceneManager.LoadScene("1-1-1 (Multi Room Choice)");
    }
    void MultiLobby_Ready_Pressed()
    {
        LobbyManager.Ready();
        //SceneManager.LoadScene("Sewer_Example");
    }
    void MultiLobby_ClassSelect_Pressed()
    {
        SceneManager.LoadScene("2-1 (MBTI Choice)");
    }
    void MultiLobby_Back_Pressed()
    {
        LobbyManager.Leave();
    }
    void Setting_Back_Pressed()
    {
        SceneManager.LoadScene("1 (Main)");
    }
    void MBTIChoice_Back_Pressed()
    {
        PlayerPrefs.SetString("PlayMode", "None");
        SceneManager.LoadScene("1 (Main)");
    }
    void MultiMBTIChoice_Back_Pressed()
    {
        SceneManager.LoadScene("1-2 (Multi Lobby)");
    }
    void ClassChoice_Back_Pressed()
    {
        SceneManager.LoadScene("2-1 (MBTI Choice)");
    }
    void ENFP_Selected()
    {
        PlayerPrefs.SetString("RecommendedClass", "Rogue");
        SceneManager.LoadScene("2-2 (Class Choice)");
        PlayerPrefs.SetString("PlayerMBTI", "ENFP");
    }
    void ENTP_Selected()
    {
        PlayerPrefs.SetString("RecommendedClass", "Rogue");
        SceneManager.LoadScene("2-2 (Class Choice)");
        PlayerPrefs.SetString("PlayerMBTI", "ENTP");
    }
    void ESFP_Selected()
    {
        PlayerPrefs.SetString("RecommendedClass", "Rogue");
        SceneManager.LoadScene("2-2 (Class Choice)");
        PlayerPrefs.SetString("PlayerMBTI", "ESFP");
    }
    void ESTP_Selected()
    {
        PlayerPrefs.SetString("RecommendedClass", "Rogue");
        SceneManager.LoadScene("2-2 (Class Choice)");
        PlayerPrefs.SetString("PlayerMBTI", "ESTP");
    }

    void ENFJ_Selected()
    {
        PlayerPrefs.SetString("RecommendedClass", "Warrior");
        SceneManager.LoadScene("2-2 (Class Choice)");
        PlayerPrefs.SetString("PlayerMBTI", "ENFJ");
    }
    void ENTJ_Selected()
    {
        PlayerPrefs.SetString("RecommendedClass", "Warrior");
        SceneManager.LoadScene("2-2 (Class Choice)");
        PlayerPrefs.SetString("PlayerMBTI", "ENTJ");
    }
    void ESFJ_Selected()
    {
        PlayerPrefs.SetString("RecommendedClass", "Warrior");
        SceneManager.LoadScene("2-2 (Class Choice)");
        PlayerPrefs.SetString("PlayerMBTI", "ESFJ");
    }
    void ESTJ_Selected()
    {
        PlayerPrefs.SetString("RecommendedClass", "Warrior");
        SceneManager.LoadScene("2-2 (Class Choice)");
        PlayerPrefs.SetString("PlayerMBTI", "ESTJ");
    }
    void INFP_Selected()
    {
        PlayerPrefs.SetString("RecommendedClass", "Archer");
        SceneManager.LoadScene("2-2 (Class Choice)");
        PlayerPrefs.SetString("PlayerMBTI", "INFP");
    }
    void INTP_Selected()
    {
        PlayerPrefs.SetString("RecommendedClass", "Archer");
        SceneManager.LoadScene("2-2 (Class Choice)");
        PlayerPrefs.SetString("PlayerMBTI", "INTP");
    }
    void ISFP_Selected()
    {
        PlayerPrefs.SetString("RecommendedClass", "Archer");
        SceneManager.LoadScene("2-2 (Class Choice)");
        PlayerPrefs.SetString("PlayerMBTI", "ISFP");
    }
    void ISTP_Selected()
    {
        PlayerPrefs.SetString("RecommendedClass", "Archer");
        SceneManager.LoadScene("2-2 (Class Choice)");
        PlayerPrefs.SetString("PlayerMBTI", "ISTP");
    }
    void INFJ_Selected()
    {
        PlayerPrefs.SetString("RecommendedClass", "Wizard");
        SceneManager.LoadScene("2-2 (Class Choice)");
        PlayerPrefs.SetString("PlayerMBTI", "INFJ");
    }
    void INTJ_Selected()
    {
        PlayerPrefs.SetString("RecommendedClass", "Wizard");
        SceneManager.LoadScene("2-2 (Class Choice)");
        PlayerPrefs.SetString("PlayerMBTI", "INTJ");
    }
    void ISFJ_Selected()
    {
        PlayerPrefs.SetString("RecommendedClass", "Wizard");
        SceneManager.LoadScene("2-2 (Class Choice)");
        PlayerPrefs.SetString("PlayerMBTI", "ISFJ");
    }
    void ISTJ_Selected()
    {
        PlayerPrefs.SetString("RecommendedClass", "Wizard");
        SceneManager.LoadScene("2-2 (Class Choice)");
        PlayerPrefs.SetString("PlayerMBTI", "ISTJ");
    }
    void ClassChoice_Class_Wizard_Pressed()
    {
        LobbyManager.ClassSelectWizard();
        SceneManager.LoadScene("2-3 (Spell Choice)");
    }
    void ClassChoice_Class_Warrior_Pressed()
    {
        LobbyManager.ClassSelectWarrior();
        SceneManager.LoadScene("2-3 (Spell Choice)");
    }
    void ClassChoice_Class_Rogue_Pressed()
    {
        LobbyManager.ClassSelectRogue();
        SceneManager.LoadScene("2-3 (Spell Choice)");
    }
    void ClassChoice_Class_Archer_Pressed()
    {
        LobbyManager.ClassSelectArcher();
        SceneManager.LoadScene("2-3 (Spell Choice)");
    }
    void SpellChoice_Back_Pressed()
    {
        SceneManager.LoadScene("2-2 (Class Choice)");
    }
    void SpellChoice_Select_Pressed()
    {
        if(selectedCount == 2)
        {   
            if(PlayerPrefs.GetString("PlayMode")=="Multi"){
                SceneManager.LoadScene("1-2 (Multi Lobby)");
            }
            else{
            SceneManager.LoadScene("3 (Bonus Stat)");
            }
        }
    }

    void BonusStat_Back_Pressed()
    {
        SceneManager.LoadScene("2-3 (Spell Choice)");
    }
    void BonusStat_Start_Pressed()
    {
        SceneManager.LoadScene("Forest_Example");
    }
}