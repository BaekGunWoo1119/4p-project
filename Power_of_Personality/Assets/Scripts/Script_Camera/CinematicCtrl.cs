using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CinematicCtrl : MonoBehaviour
{
    public GameObject player;
    public GameObject finalBoss;
    public GameObject appearedEffect;
    public GameObject heavenEffect;
    public Transform rendingPoint;
    public Transform descentPoint;
    public GameObject[] cinematicCamera;
    public Animator changeAnim;
    public GameObject BossWeapon;

    private float screenHeight;
    public GameObject screenTop;
    public GameObject screenBot;
    private Vector3 screenPos1;
    private Vector3 screenPos2;
    private RectTransform screenTopRect;
    private RectTransform screenBotRect;

    private bool isCinematic = false;
    private Vector3 velocity = Vector3.zero;

    public AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        finalBoss.transform.position = descentPoint.position;
        heavenEffect.SetActive(false);
        appearedEffect.SetActive(false);
        BossWeapon.SetActive(false);

        screenTopRect = screenTop.GetComponent<RectTransform>();
        screenBotRect = screenBot.GetComponent<RectTransform>();

        screenPos1 = new Vector2(screenTopRect.anchoredPosition.x, Screen.height * 0.5f);
        screenPos2 = new Vector2(screenBotRect.anchoredPosition.x, (-Screen.height * 0.5f));
        for(int i = 0; i < cinematicCamera.Length; i++)
        {
            cinematicCamera[i].SetActive(false);
        }

        screenHeight = Screen.height;
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
            screenTopRect.anchoredPosition = Vector2.Lerp(screenTopRect.anchoredPosition, screenPos1, Time.deltaTime);
            screenBotRect.anchoredPosition = Vector2.Lerp(screenBotRect.anchoredPosition, screenPos2, Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "Player" && transform.localPosition.x - player.transform.localPosition.x < 0)
        {
            Debug.Log("시네마틱 재생");
            // 부모 변경
            player.transform.SetParent(transform.parent.GetChild(1));
            StartCoroutine(Play_Cinematic());
            
        }
    }

    IEnumerator Play_Cinematic()
    {
        cinematicCamera[0].SetActive(true);
        isCinematic = true;
        finalBoss.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        heavenEffect.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        cinematicCamera[0].SetActive(false);
        cinematicCamera[1].SetActive(true);
        yield return new WaitForSeconds(0.7f);
        appearedEffect.SetActive(true);
        audio.Play();
        yield return new WaitForSeconds(0.3f);
        isCinematic = false;
        cinematicCamera[1].SetActive(false);
        cinematicCamera[2].SetActive(true);
        yield return new WaitForSeconds(3.5f);
        finalBoss.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = changeAnim.runtimeAnimatorController;
        finalBoss.transform.SetParent(transform.parent.GetChild(1));
        cinematicCamera[2].SetActive(false);
        finalBoss.transform.GetChild(0).GetComponent<DemonKingCtrl>().enabled = true;
        BossWeapon.SetActive(true);
        screenTop.SetActive(false);
        screenBot.SetActive(false);
        audio.Stop();
    }
}
