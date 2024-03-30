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

    // ��� ���� ����� �� ȣ��Ǵ� �Լ�
    private void OnToggleValueChanged(Toggle changedToggle)
    {
        if (changedToggle.isOn)
        {
            selectedCount++;

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