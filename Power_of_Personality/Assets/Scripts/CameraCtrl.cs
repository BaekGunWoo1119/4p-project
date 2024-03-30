using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    private Transform target; //Player
    public Vector3 offsetOrigin; // 카메라 위치
    private Vector3 offset;
    private Vector3 currentVelocity = Vector3.zero;

    //카메라 흔들림
    private float shakeAmount = 0f;
    private float shakeDuration = 0.03f;
    private float shakeTimer = 0f;

    //카메라 고정
    private float focusTimer = 0f;
    private float xPos;
    private float yPos;
    private float zPos;
    private float yRot;
    private string timeValue;

    private bool isMove;

    void Start()
    {
        offset = offsetOrigin;
    }
    
    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target != null && focusTimer <= 0 && shakeTimer <= 0 && !isMove)
        {
            // 플레이어의 움직임을 살짝 딜레이를 주고 따라 감
            transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref currentVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0,0,0);
        }

        if (shakeTimer > 0)
        {
            isMove = true;
            // 카메라를 흔들이기
            transform.localPosition += Random.insideUnitSphere * shakeAmount;

            // 흔들림 시간 감소
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            // 흔들림이 끝나면 초기 위치로 돌아가기
            shakeTimer = 0f;
            transform.localPosition = transform.localPosition;
        }

        if (focusTimer > 0)
        {
            isMove = true;
            if(timeValue == "stop")
            {
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(xPos,yPos,zPos), ref currentVelocity, 0.1f);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,yRot,0), Time.deltaTime * 3.5f);
                StartCoroutine(TimeStop(0.35f));
            }

            else if(timeValue == "round")
            {
                Time.timeScale = 1;
                if(yRot == 60)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(target.position.x - 2,yPos,zPos - 5), ref currentVelocity, 0.1f);
                }
                else
                {
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(target.position.x - 1,yPos,zPos - 5), ref currentVelocity, 0.1f);
                }
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(10,yRot,0), Time.deltaTime * 4.0f);
            }
            else if(timeValue == "forward")
            {
                Time.timeScale = 1;
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(xPos,yPos,zPos), ref currentVelocity, 0.1f);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(10,yRot,0), Time.deltaTime * 4.0f);
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(xPos,yPos,zPos), ref currentVelocity, 0.1f);
            }
            
            if(Time.timeScale == 0)
            {
                focusTimer -= 0.01f;
            }
            else
            {
                focusTimer -= Time.deltaTime;
            }
        }
        else
        {
            focusTimer = 0f;
            Time.timeScale = 1;
            timeValue = "play";
            xPos = 0;
            yPos = 0;
            zPos = 0;
            isMove = false;
        }
    }

    public void ShakeCamera(float Amount, float Duration, string zoom)
    {
        shakeAmount = Amount;
        shakeDuration = Duration;
        shakeTimer = shakeDuration;
        if(zoom == "zoom")
        {
            offset = new Vector3(0, 2, -4);
        }
    }

    public void FocusCamera(float xP, float yP, float zP, float R, float Duration, string value)
    {   
        xPos = xP;
        yPos = yP;
        zPos = zP;
        yRot = R;

        timeValue = value;
        focusTimer = Duration;
    }

    IEnumerator TimeStop(float wait)
    {
        yield return new WaitForSeconds(wait);
        Time.timeScale = 0;
    }

    IEnumerator Setoffset(float wait)
    {
        yield return new WaitForSeconds(wait);
        if(offset != offsetOrigin && shakeTimer <= 0 && focusTimer <= 0)
        {
            offset = offsetOrigin;
            isMove = false;
        }
    }

    public void moveStop(float seconds)
    {
        StartCoroutine(Setoffset(seconds));
    }
}
