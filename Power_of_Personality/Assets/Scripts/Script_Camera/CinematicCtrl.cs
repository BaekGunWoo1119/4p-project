using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCtrl : MonoBehaviour
{
    public GameObject player;
    public GameObject finalBoss;
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
            StartCoroutine(Play_Cinematic());
        }
    }

    IEnumerator Play_Cinematic()
    {
        cinematicCamera[0].SetActive(true);
        isCinematic = true;
        finalBoss.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(1.5f);
        isCinematic = false;
        cinematicCamera[0].SetActive(false);
        cinematicCamera[1].SetActive(true);
        yield return new WaitForSeconds(3.5f);
        finalBoss.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = changeAnim.runtimeAnimatorController;
        cinematicCamera[1].SetActive(false);
    }
}
