using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eBallState
{
    none,

    hitting,
    foul,

    strike,
    ball,

    done,
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
    public float lifeTime = 3f;

    //for draw trajectory
    private LineRenderer lineRenderer;
    private List<Vector3> positions = new List<Vector3>();

    //ball state check
    public Vector3 homePlateDir = -Vector3.forward;

    //ball distance check
    private Transform ballStartPosition;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);

        ballState = eBallState.none;

        lineRenderer.enabled = false;
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    private void Update()
    {
        if(ballState == eBallState.hitting)
        {
            DrawTrajectory();
        }

        if(ballState == eBallState.done)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //공이 배트랑 충돌 발생시
        if(collision.gameObject.CompareTag("Bat"))
        {
            //충돌 방향계산
            Vector3 hitDirection = (transform.position - collision.transform.position).normalized;

            //충돌 방향이 0보다 작으면 파울
            ballTiming = CalculateTiming(homePlateDir, Vector3.up, hitDirection);

            if(Vector3.Dot(hitDirection, homePlateDir) < 0f)            
            {
                Debug.Log("Foul");
                ballState = eBallState.foul;
            }
            else
            {
                Debug.Log("Hit");
                ballState = eBallState.hitting;
            }

            //공이 충돌하기 시작한 위치 설정
            ballStartPosition = collision.transform;

            //TODO 타이밍에 따른 hit force 적용
            float hitForce = 50f;

            if (ballTiming == eBallTiming.good)
                hitForce = 150f;
            else if (ballTiming == eBallTiming.late)
                hitForce = 50f;
            else if(ballTiming == eBallTiming.fast)
                hitForce = 100f;

            Debug.Log(hitForce);

            rigidBody.AddForce(hitDirection * hitForce, ForceMode.Impulse);
            rigidBody.useGravity = true;
        }        
    }

    private eBallTiming CalculateTiming(Vector3 originDir, Vector3 upDir, Vector3 hitDir)
    {
        eBallTiming timing;
        float upAngle = Vector3.Dot(upDir, hitDir);

        //땅으로 가는 타구
        if(upAngle < 0f)
        {
            timing = eBallTiming.fast;
            return timing;
        }
        
        //아닌 경우
        float angle = Vector3.Angle(originDir, hitDir);
        
        if (angle > 15f && angle < 40f)
            timing = eBallTiming.good;
        else
            timing = eBallTiming.late;

        Debug.Log("Angle : " + angle + "Timing : " + timing);
        
        return timing;
    }

    void DrawTrajectory()
    {
        lineRenderer.enabled = true;
        positions.Add(transform.position);
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

}
