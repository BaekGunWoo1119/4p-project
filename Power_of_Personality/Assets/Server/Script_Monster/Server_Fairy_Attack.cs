using UnityEngine;

public class Server_Fairy_Attack : MonoBehaviour
{
    private Transform PlayerTr;
    private Rigidbody rb;
    public float ATK = 5.0f;
    private string fairyParent = "Monster(Script)"; // 찾고자 하는 부모 객체의 이름
    public Server_MonsterCtrl monsterCtrl;
    private string curproperty;
    void Awake()

    {
        curproperty = PlayerPrefs.GetString("property");
        PlayerTr = GameObject.FindWithTag("Player").transform;
        Vector3 targetPosition = PlayerTr.position + Vector3.up;
        rb = GetComponent<Rigidbody>();
        Vector3 Dir = new Vector3(targetPosition.x - transform.position.x, 0, targetPosition.z - transform.position.z).normalized;
        if(Status.Spell_TimeSlowdown_ON){
            rb.AddForce(Dir.normalized * 10 *0.3f, ForceMode.Impulse);
        }
        else{
            rb.AddForce(Dir.normalized * 10, ForceMode.Impulse);
        }
        Transform parent = this.transform;
        monsterCtrl = null;

        while (parent != null)
        {
            if (parent.name ==  fairyParent)
            {
                monsterCtrl = parent.GetComponent<Server_MonsterCtrl>();
                break;
            }
            parent = parent.parent;
        }

        this.transform.SetParent(null);
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (monsterCtrl != null && Status.set2_3_Activated)
            {
                float reflectDamage = Status.TotalArmor;
                monsterCtrl.ReflectDamage(reflectDamage,curproperty);
            }
            this.transform.localPosition = new Vector3(0, -600f, 0);
            Destroy(gameObject,5.0f);
        }
    }

    public void Update()
    {
        transform.localRotation = Quaternion.identity; // 초기 회전으로 설정
    }
    
}
