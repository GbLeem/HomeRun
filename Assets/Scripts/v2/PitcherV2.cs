using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherV2 : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;

    public Ball ballPrefab;
    //public BallData[] ballData;

    //새로 만든거
    public BallDataV2[] ballDatas;
    private int totalBallDataSize;

    //랜덤 위치
    public Transform strikeZone;
    private Vector2 strikeZoneSize;

    //애니메이션
    private Animator pitcherAnimator;

    //ball state
    private Ball ball;    

    private void Awake()
    {
        pitcherAnimator = GetComponent<Animator>();
        ball = FindObjectOfType<Ball>();        
    }

    private void Start()
    {
        //맨 처음에는 그냥 던짐
        if (UIManager.instance.ballCount == 0)
        {            
            pitcherAnimator.SetBool("canPitch", true);
        }

        //TODO 인덱스는 나중에 랜덤으로 바꿔야함 혹은 난이도마다 다르게 적용
        //strikeZoneSize = ballData[0].StrikeZoneSize;
        totalBallDataSize = ballDatas.Length;
        //strikeZoneSize = ballData[Random.Range(0, totalBallDataSize)].StrikeZoneSize;
    }

    private void Update()
    {        
        if (ball == null)
        {
            ball = FindObjectOfType<Ball>();         
        }

        //여기서 ball state 가져와서 set bool로 적용
        //has exit time은 pitch 하고 돌아올때만 적용
        if (ball != null)
        {
            if (ball.ballState == eBallState.finish)
            {
                pitcherAnimator.SetBool("canPitch", true);
            }
            else
            {
                pitcherAnimator.SetBool("canPitch", false);
            }
        }
        
        if(UIManager.instance.ballCount == 10)
        {
            //애니메이션 멈추기
            //TODO 자연스럽게 만들기
            pitcherAnimator.enabled = false;
        }
    }  

    //현재 이 함수가 animation event를 통해서 실행되고 있음
    void Pitching()
    {
        int index = SelectBallIndex();

        //최대 공 갯수 보다 작으면 공 던지도록 함
        if(UIManager.instance.ballCount < 10)
        {
            Vector3 randomPoint = GetRandomPointInStrikeZone();

            Ball ball = Instantiate(ballPrefab, startPosition.position, startPosition.rotation);
            Rigidbody rigidbody = ball.GetComponent<Rigidbody>();
            ball.ballState = eBallState.none;

            Vector3 dir = (randomPoint - startPosition.position).normalized;

            //rigidbody.AddForce(dir * ballData[0].force, ForceMode.Impulse);
            rigidbody.AddForce(dir * ballDatas[index].force, ForceMode.Impulse);

            //공 텍스쳐 돌아가게 보일려고
            //TODO 직구랑 슬라이더랑 회전 방향 다르게
            //rigidbody.AddTorque(Vector3.right * 10f, ForceMode.VelocityChange);    

            //if (UIManager.instance.ballCount < 5)
            //    StartCoroutine(Slider(rigidbody));

            //if (UIManager.instance.ballCount >= 5)
            //    StartCoroutine(Fork(rigidbody));
            StartCoroutine(BreakingBall(rigidbody, index));
        }
    }

    //IEnumerator Slider(Rigidbody rb)
    //{
    //    float elapsedTime = 0f;
    //    while(elapsedTime < ballData[0].sliderDuration)
    //    {
    //        rb.AddForce(-transform.right * ballData[0].sliderForce * Time.deltaTime, ForceMode.VelocityChange);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //}

    //IEnumerator Fork(Rigidbody rb)
    //{
    //    yield return new WaitForSeconds(0.5f);

    //    float elapsedTime = 0f;
    //    while (elapsedTime < ballData[0].forkDuration)
    //    {
    //        rb.AddForce(-transform.up * ballData[0].forkForce * Time.deltaTime, ForceMode.VelocityChange);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //}

    IEnumerator BreakingBall(Rigidbody rb, int index)
    {
        //1 4seam
        //2 slider
        //3 forkball

        if (ballDatas[index].breakingballDuration == 0)
            yield return null;

        yield return new WaitForSeconds(ballDatas[index].delayTime);
        
        float elapsedTime = 0f;
        while (elapsedTime < ballDatas[index].breakingballDuration)
        {
            rb.AddForce(ballDatas[index].forceDir * ballDatas[index].breakingballForce * Time.deltaTime, ForceMode.VelocityChange);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    int SelectBallIndex()
    {
        totalBallDataSize = ballDatas.Length;
        int idx = Random.Range(0, totalBallDataSize);
        Debug.Log(idx);

        return idx;
    }

    Vector3 GetRandomPointInStrikeZone()
    {
        float randomX = Random.Range(-strikeZoneSize.x / 2, strikeZoneSize.x / 2);
        float randomY = Random.Range(-strikeZoneSize.y / 2, strikeZoneSize.y / 2);

        Vector3 randomPoint = strikeZone.position + new Vector3(randomX, randomY, 0f);
        return randomPoint;
    }
}