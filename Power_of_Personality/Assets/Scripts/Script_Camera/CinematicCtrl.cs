using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCtrl : MonoBehaviour
{
    public GameObject player;
    public GameObject finalBoss;
    public GameObject appearedEffect;
    public Transform rendingPoint;
    public Transform descentPoint;
    public GameObject[] cinematicCamera;
    public Animator changeAnim;

    private bool isCinematic = false;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        finalBoss.transform.position = descentPoint.position;
        appearedEffect.SetActive(false);


        for(int i = 0; i < cinematicCamera.Length; i++)
        {
            cinematicCamera[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if(isCinematic == true)
        {
            finalBoss.transform.position = Vector3.SmoothDamp(finalBoss.transform.position, rendingPoint.position, ref velocity, 0.7f);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "Player" && transform.localPosition.x - player.transform.localPosition.x < 0)
        {
            Debug.Log("시네마틱 재생");
            // 현재 위치와 로테이션을 저장
            //Vector3 savedPosition = player.transform.position;
            Vector3 savedPosition = new Vector3(18f, 23.5f, -24f);

            // 부모 변경
            player.transform.SetParent(transform.parent);

            // 위치와 회전을 다시 적용
            //player.transform.localPosition = savedPosition;
            //player.transform.rotation = savedRotation;
            StartCoroutine(Play_Cinematic());
        }
    }

    IEnumerator Play_Cinematic()
    {
        cinematicCamera[0].SetActive(true);
        isCinematic = true;
        finalBoss.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(1.2f);
        appearedEffect.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        isCinematic = false;
        cinematicCamera[0].SetActive(false);
        cinematicCamera[1].SetActive(true);
        yield return new WaitForSeconds(3.5f);
        finalBoss.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = changeAnim.runtimeAnimatorController;
        finalBoss.transform.SetParent(transform.parent.transform);
        cinematicCamera[1].SetActive(false);
        finalBoss.transform.GetChild(0).GetComponent<DemonKingCtrl>().enabled = true;
    }
}
