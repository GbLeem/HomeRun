using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    //공의 상태
    public enum eBallState
    {
        none,
        hitting,
        strike,
        ball,
        foul,
        done
    };
    public eBallState getBallState
    {
        get
        {
            return ballState;
        }
        set
        {
            ballState = value;
        }
    }
    public float lifeTime = 10f;    
    private bool Catching = false;
    public eBallState ballState;

    //for draw lines
    public LineRenderer lineRenderer;
    private List<Vector3> positions = new List<Vector3>();

    //변화구
    private Rigidbody rb;
    private bool forceApplied = false;
    private Vector3 middlePoint;
    private Vector3 direction2;
    private float additionalForce;

    //비거리 측정
    public float distance = 0;
    private Vector3 homePlate = -Vector3.forward;
    private Transform hitTransform;

    public void Initialize(Vector3 middle, Vector3 dir2, float force)
    {
        rb = GetComponent<Rigidbody>();
        middlePoint = middle;
        direction2 = dir2;
        additionalForce = force;
        StartCoroutine(CheckAndApplyForce());
    }

    IEnumerator CheckAndApplyForce()
    {
        while (!forceApplied)
        {            
            if (Vector3.Distance(transform.position, middlePoint) < 0.2f)
            {
                rb.velocity = Vector3.zero; // Optional: reset velocity
                rb.AddForceAtPosition(direction2 * additionalForce, middlePoint, ForceMode.Impulse);
                forceApplied = true;
            }
            yield return null;
        }
    }

    private void Start()
    {        
        ballState = eBallState.none;
        //if(ballState == eBallState.done)
        Destroy(gameObject, lifeTime);

        lineRenderer = GetComponent<LineRenderer>();
        if(lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    private void Update()
    {
        //타격된 이후 그리기 
        //& TODO 거리 측정
        if (ballState == eBallState.hitting)
        {
            DrawTrajectory();            
        }
        distance = GetDistance(transform);
    }

    void DrawTrajectory()
    {        
        positions.Add(transform.position);     
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());        
    }

    public BallController.eBallState GetEBallState()
    {
        return ballState;
    }

    public float GetDistanceTo()
    {
        return distance;
    }

    private float GetDistance(Transform ball)
    {
        if (ballState == eBallState.hitting)
        {
            Vector3 distanceVector = homePlate.normalized;
            Vector3 ballVector = ball.position - hitTransform.position;
            return Vector3.Dot(ballVector, distanceVector);
        }
        else
            return 0f;
    }

    private void OnDestroy()
    {
        //distance = GetDistance(transform);
        Debug.Log(distance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            if(other.gameObject.CompareTag("Strike"))
            {
                ballState = eBallState.strike;
                Debug.Log("Strike");
                Catching = true;
            }
            if(Catching == false && other.gameObject.CompareTag("Ball"))
            {
                ballState = eBallState.ball;
                Debug.Log("Ball");
                Catching = false;
            }                        
        }        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bat"))
        {
            //Debug.Log("Hit");
           
            Vector3 hitDirection = (transform.position - collision.transform.position).normalized;
            if (Vector3.Dot(hitDirection, homePlate) < 0f)
            {
                ballState = eBallState.foul;
            }
            else
                ballState = eBallState.hitting;

            hitTransform = collision.transform;

            //TODO 타이밍을 계산해서 hitforce변화를 준다.
            float hitForce = 50f;

            rb.AddForce(hitDirection * hitForce, ForceMode.Impulse);

            //TODO 타격 후에는 중력 적용하기
            rb.useGravity = true;
        }
    }
}