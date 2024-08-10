using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Camera mainCamera;        // 포수 뒤의 메인 카메라
    public Camera ballFollowCamera;  // 공을 따라가는 카메라
    private Ball ball;

    public GameObject pciUI;

    private void Start()
    {
        // 초기 설정: 메인 카메라만 활성화
        mainCamera.enabled = true;
        ballFollowCamera.enabled = false;

        ball = FindObjectOfType<Ball>();
    }

    void Update()
    {
        if (ball == null)
            ball = FindObjectOfType<Ball>();

        if (ball != null)
        {
            if (ball.ballTiming == eBallTiming.good)
            {
                //Debug.Log(ball.GetHitDirection());

                if (CalculateCamSwitch())
                {
                    SwitchToBallCamera();

                }
            }            
        }

        if(ball != null)
        {
            if(ball.ballState == eBallState.finish)
            {
                // 공이 파괴되면 메인 카메라로 전환
                SwitchToMainCamera();
            }
        }
    }

    void SwitchToBallCamera()
    {
        pciUI.SetActive(false);

        mainCamera.enabled = false;
        ballFollowCamera.enabled = true;        
        ballFollowCamera.transform.LookAt(ball.transform);
    }

    void SwitchToMainCamera()
    {
        pciUI.SetActive(true);

        mainCamera.enabled = true;
        ballFollowCamera.enabled = false;
    }    

    bool CalculateCamSwitch()
    {
        if (ball.GetHitDirection().y > 0f && ball.GetHitDirection().y < 0.8f)
        {
            if (ball.GetHitDirection().x > -0.7f && ball.GetHitDirection().x < 0.7f)
            {
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }
}
