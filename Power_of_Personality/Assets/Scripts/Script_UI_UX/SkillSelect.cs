using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelect : MonoBehaviour
{
    public SceneLoader sceneLoader;
    private ToggleGroup toggleGroup; // Ư�� Toggle Group�� Inspector���� ����
    private Toggle lastSelectedToggle;
    public GameObject DangerText_Exceed2;
    public GameObject DangerText_less2;
    public int selectedCount;
    private Toggle Spell1; //처음으로 선택한 보조스킬
    private Toggle Spell2; //두번째로 선택한 보조스킬
    public Image spell1_image;
    public Image spell2_image;

    void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        toggleGroup = GameObject.Find("Checkbox").GetComponent<ToggleGroup>();
        DangerText_Exceed2 = GameObject.Find("DangerText_Exceed2");
        DangerText_less2 = GameObject.Find("DangerText_less2");
        DangerText_less2.transform.localScale = new Vector3(1, 1, 1);
        if (toggleGroup != null)
        {
            // ��� �׷��� �� ��ۿ� ���� �̺�Ʈ ������ �߰�
            Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
            foreach (Toggle toggle in toggles)
            {
                toggle.onValueChanged.AddListener(delegate { OnToggleValueChanged(toggle); });
            }
        }
    }

    void Update(){
        if(Spell1 != null){
            //Spell1.graphic = spell1_image;
            Spell1.gameObject.transform.Find("Background").transform.Find("Checkmark").GetComponent<Image>().sprite = spell1_image.sprite;
        }
        if(Spell2 != null){
            //Spell2.graphic = spell2_image;
            Spell2.gameObject.transform.Find("Background").transform.Find("Checkmark").GetComponent<Image>().sprite = spell2_image.sprite;
        }
    }

    // ��� ���� ����� �� ȣ��Ǵ� �Լ�
    private void OnToggleValueChanged(Toggle changedToggle)
    {
        if (changedToggle.isOn)
        {
            selectedCount++;
            if(selectedCount == 1){
                PlayerPrefs.SetString("Spell_1",changedToggle.gameObject.name);
                Debug.Log(PlayerPrefs.GetString("Spell_1"));
                Spell1 = changedToggle;
            }
            else if(selectedCount == 2){
                PlayerPrefs.SetString("Spell_2",changedToggle.gameObject.name);
                Debug.Log(PlayerPrefs.GetString("Spell_2"));
                Spell2 = changedToggle;
            }

            if (selectedCount > 2)
            {
                changedToggle.isOn = false;
                DangerText_Exceed2.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (selectedCount < 2)
            {
                DangerText_less2.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                sceneLoader.selectedCount = selectedCount;
                DangerText_Exceed2.transform.localScale = new Vector3(0, 0, 0);
                DangerText_less2.transform.localScale = new Vector3(0, 0, 0);
                lastSelectedToggle = changedToggle;
            }
        }
        else
        {
            // ����� üũ ������ ��
            selectedCount--;
            if (changedToggle == Spell1 && Spell2 == null){
                Spell1 = null;
            }
            else if (changedToggle == Spell1){
                Spell1 = Spell2;
                PlayerPrefs.SetString("Spell_1",Spell1.gameObject.name);
                Spell2 = null;
            }
            else{
                Spell2 = null;
            }

            if (selectedCount < 2)
            {
                DangerText_less2.transform.localScale = new Vector3(1, 1, 1);
            }

            if (changedToggle == lastSelectedToggle)
            {
                lastSelectedToggle = null;
            }
        }
    }
}