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


public class Ball : MonoBehaviour
{   
    public eBallState ballState { get; set; }
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
            float hitForce = 100f;
            rigidBody.AddForce(hitDirection * hitForce, ForceMode.Impulse);
            rigidBody.useGravity = true;
        }        
    }


    void DrawTrajectory()
    {
        lineRenderer.enabled = true;
        positions.Add(transform.position);
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

}
