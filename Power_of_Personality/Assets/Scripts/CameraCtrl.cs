using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    private Transform target; //Player
    private Vector3 offset;
    private Vector3 currentVelocity = Vector3.zero;

    //카메라 흔들림
    private float shakeAmount = 0f;
    private float shakeDuration = 0.03f;
    private float shakeTimer = 0f;

    //카메라 고정
    private Vector3 forwardDirection;
    private Vector3 UpDirection;
    private float focusTimer = 0f;
    private float xPos;
    private float yPos;
    private float zPos;
    private float xRot;
    private float yRot;
    private float zRot;
    private string timeValue;

    private bool isMove;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        if (forwardDirection == null || forwardDirection != target.transform.root.transform.forward)
        {
            forwardDirection = target.transform.root.transform.forward;
        }
        if (UpDirection == null || UpDirection != target.transform.root.transform.up)
        {
            UpDirection = target.transform.root.transform.up;
        }
        offset = forwardDirection * -10f + UpDirection * 4.25f;
        if (target != null && focusTimer <= 0 && shakeTimer <= 0 && !isMove)
        {
            // 플레이어의 움직임을 살짝 딜레이를 주고 따라 감
            transform.position = Vector3.SmoothDamp(transform.position, target.position+offset, ref currentVelocity, 0f);
            transform.rotation = target.transform.root.GetComponentInParent<Transform>().rotation;
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
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(xRot,yRot,zRot), Time.deltaTime * 3.5f);
            }

            else if(timeValue == "round")
            {
                xRot = 10;
                zRot = 0;
                if(yRot == 60)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(target.position.x - 2,yPos,zPos - 5), ref currentVelocity, 0.1f);
                }
                else
                {
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(target.position.x - 1,yPos,zPos - 5), ref currentVelocity, 0.1f);
                }
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(xRot,yRot,zRot), Time.deltaTime * 4.0f);
            }
            else if(timeValue == "forward")
            {
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(xPos,yPos,zPos), ref currentVelocity, 0.1f);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(10,yRot,0), Time.deltaTime * 4.0f);
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(xPos,yPos,zPos), ref currentVelocity, 0.1f);
            }
            
            focusTimer -= Time.deltaTime;
        }
        else
        {
            focusTimer = 0f;
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
        xRot = 5;
        zRot = 0;

        timeValue = value;
        focusTimer = Duration;
    }

    IEnumerator Setoffset(float wait)
    {
        yield return new WaitForSeconds(wait);
        //if(offset != offsetOrigin && shakeTimer <= 0 && focusTimer <= 0)
        //{
        //    offset = offsetOrigin;
        //    isMove = false;
        //}
    }

    public void moveStop(float seconds)
    {
        StartCoroutine(Setoffset(seconds));
    }
}
