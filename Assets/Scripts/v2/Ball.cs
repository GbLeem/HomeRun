using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eBallState
{
    none,       //일반 상태
    flying,     //쳐서 날아가는 상태

    hitting,    //쳣을때
    foul,       //foul
    homerun,    //homerun

    strike,     
    ball,

    done,       //처리가 끝남
    finish,
};

public enum eBallTiming
{
    late,
    good,
    fast,        
};

public class Ball : MonoBehaviour
{   
    public eBallState ballState { get; set; }
    public eBallTiming ballTiming { get; private set; }

    private Rigidbody rigidBody;

    //for draw trajectory
    private LineRenderer lineRenderer;
    private List<Vector3> positions = new List<Vector3>();

    //ball state check
    public Vector3 homePlateDir = -Vector3.forward;

    //ball distance check
    private Transform ballStartPosition;

    //total hitting chance    
    private bool bIsShowUI = false;

    private void Awake()
    { 
        rigidBody = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();

        ballState = eBallState.none;
    }
    private void OnDestroy()
    {
        //ballState = eBallState.done;
    }
    private void Start()
    {
        //공 멀리 뜰때 버그 나는 것 이걸로 수정함 life time 떄문인듯
        //Destroy(gameObject, lifeTime);

        //공이 처음에는 none 상태로 시작함
        ballState = eBallState.none;

        //line renderer 처리
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        
        //total 공 갯수 체크
        UIManager.instance.ballCount += 1;        
    }    

    private void Update()
    {
        //공 다쓰고, done 일 때
        if (UIManager.instance.ballCount == 10)
        {
            if(ballState == eBallState.finish)
                UIManager.instance.GameOver();
        }

        //hitting 이후 공의 상태가 flying 
        if (ballState == eBallState.flying)
        {
            if(bIsShowUI)
            {
                UIManager.instance.UpdateBallImage(UIManager.instance.ballCount, ballState);
                bIsShowUI = false;
            }

            DrawTrajectory();
            //TODO : distance 측정이 끝나는 시간을 정해야함
            CalculateDistance();
        }

        else if(ballState == eBallState.done)
        {
            ballState = eBallState.finish;
            Destroy(gameObject, 2f);            
        }     
    }
    private void OnTriggerEnter(Collider other)
    {
        //맨 처음에 왜 인식이 안되는것? -> collider 너무 작아서
        bIsShowUI = false;

        if (other.gameObject.CompareTag("StrikeZone"))
        {
            //Debug.Log("Strike OK");
            ballState = eBallState.strike;
            UIManager.instance.UpdateBallImage(UIManager.instance.ballCount, eBallState.strike);

            //TODO destroy 하지말고 그냥 ball state로 처리하기
            ballState = eBallState.done;
        }

        if (other.gameObject.CompareTag("HomeRun"))
        {
            ballState = eBallState.homerun;
            UIManager.instance.ShowHomeRunText();

            //홈런친 순간 적용
            UIManager.instance.UpdateBallImage(UIManager.instance.ballCount, eBallState.homerun);

            ballState = eBallState.done;
        }

        if(other.gameObject.CompareTag("Foul"))
        {
            ballState = eBallState.foul;
            UIManager.instance.UpdateBallImage(UIManager.instance.ballCount, eBallState.foul);

            ballState = eBallState.done;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        bIsShowUI = true;

        //공이 땅에 닿았을때 한 turn 이 끝남
        if(collision.gameObject.CompareTag("Ground"))
        {
            ballState = eBallState.done;
        }

        //공이 배트랑 충돌 발생시
        if(collision.gameObject.CompareTag("Bat"))
        {            
            //충돌 방향계산
            Vector3 hitDirection = (transform.position - collision.transform.position).normalized;

            //ball timing 계산해서 force 적용

            //타이밍 계산 함수 수정
            //ballTiming = CalculateTiming(homePlateDir, Vector3.up, hitDirection);
            ballTiming = UIManager.instance.CalculateTimingByUI();

            UIManager.instance.ShowTimingText(ballTiming);

            //충돌 방향이 0보다 작으면 파울
            if (Vector3.Dot(hitDirection, homePlateDir) < 0.0001f)            
            {
                //Debug.Log("Foul");
                ballState = eBallState.foul;
            }
            else
            {
                //Debug.Log("Hit");
                ballState = eBallState.hitting;
            }

            //공이 충돌하기 시작한 위치 설정
            ballStartPosition = collision.transform;

            //타이밍에 따른 hit force 적용
            float hitForce = 50f;

            if (ballTiming == eBallTiming.good)
                hitForce = 50f;
            else if (ballTiming == eBallTiming.late)
                hitForce = 15f;
            else if(ballTiming == eBallTiming.fast)
                hitForce = 25f;

            rigidBody.AddForce(hitDirection * hitForce, ForceMode.Impulse);
            rigidBody.useGravity = true;
        }               
    }   

    //이제는 안쓰는 함수
    //private eBallTiming CalculateTiming(Vector3 originDir, Vector3 upDir, Vector3 hitDir)
    //{
    //    eBallTiming timing;
    //    float upAngle = Vector3.Dot(upDir, hitDir);

    //    //땅으로 가는 타구
    //    if(upAngle < 0f)
    //    {
    //        timing = eBallTiming.fast;
    //        return timing;
    //    }
        
    //    //아닌 경우
    //    float angle = Vector3.Angle(originDir, hitDir);
        
    //    if (angle > 15f && angle < 40f)
    //        timing = eBallTiming.good;
    //    else
    //        timing = eBallTiming.late;
        
    //    return timing;
    //}

    void CalculateDistance()
    {
        float distance;

        //distance 계산
        distance = Vector3.Distance(transform.position, ballStartPosition.position);

        //UI 연동
        UIManager.instance.UpdateDistanceText(distance);        
    }

    void DrawTrajectory()
    {        
        lineRenderer.enabled = true;
        positions.Add(transform.position);
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }    
}
