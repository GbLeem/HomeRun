using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitcher : MonoBehaviour
{
    //TODO 랜덤하게 일정 시간마다 공 던지는거 추가
    //TODO 타구의 비거리 측정
    //TODO 공의 상태에 대한 처리가 필요할 듯함, 포수가 잡았는지, 타격이 되었는지

    public GameManager gameManager;
    public GameObject ballPrefab;
    public Transform PitchPoint;
    public Transform CatcherPoint;
    //private BallController ballController;

    public float fastballForce = 30f;
    public float sliderForce = 25f;
    public float forkballForce = 20f;

    //UI 용
    public float ballDistance = 0f;
    public int hittingCount = -1;

    enum ePitchType
    {
        FastBall,
        Slider,
        Slurve,
        ForkBall
    };

    private void Awake()
    {

    }
    private void Start()
    {
        //gameManager = GetComponent<GameManager>();        
        //ballController = FindObjectOfType<BallController>();        
        //ballController = GetComponent<BallController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PitchSlider();
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            PitchFastBall();
        }        
        else if(Input.GetKeyDown(KeyCode.X))
        {
            PitchForkBall();
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            PitchSlurve();
        }        
        //ballDistance = ballController.GetDistanceTo();
    }

    private void FixedUpdate()
    {       
    }

    void PitchFastBall()
    {
        //Vector3 target = CatcherPoint.position + CatcherPoint.right * -1.0f;
        Vector3 middlePoint = PitchPoint.position + (CatcherPoint.position - PitchPoint.position) * 0.5f;
        PitchBallToCatcher(PitchPoint.position, middlePoint, CatcherPoint.position, fastballForce, ePitchType.FastBall);        
    }

    void PitchSlider()
    {
        Vector3 target = CatcherPoint.position + CatcherPoint.right * -0.5f;        
        Vector3 middlePoint = PitchPoint.position + (CatcherPoint.position - PitchPoint.position) * 0.7f;
        PitchBallToCatcher(PitchPoint.position, middlePoint, target, sliderForce, ePitchType.Slider);
    }
    void PitchSlurve()
    {
        Vector3 target = CatcherPoint.position + CatcherPoint.right * -0.5f;
        Vector3 middlePoint = PitchPoint.position + (CatcherPoint.position - PitchPoint.position) * 0.8f;
        PitchBallToCatcher(PitchPoint.position, middlePoint, target, sliderForce, ePitchType.Slurve);
    }

    void PitchForkBall()
    {
        Vector3 target = CatcherPoint.position + CatcherPoint.up * -0.4f;        
        Vector3 middlePoint = PitchPoint.position + (CatcherPoint.position - PitchPoint.position) * 0.8f;
        PitchBallToCatcher(PitchPoint.position, middlePoint, target, forkballForce, ePitchType.ForkBall);
    }

    void PitchBallToCatcher(Vector3 start, Vector3 middle, Vector3 end, float force, ePitchType type)
    {
        GameObject ball = Instantiate(ballPrefab, PitchPoint.position, PitchPoint.rotation);
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        Vector3 direction1 = (middle - start).normalized;
        Vector3 direction2 = (end - middle).normalized;
        
        rb.AddForce(direction1 * force, ForceMode.Impulse);

        BallController ballController = ball.GetComponent<BallController>();
        ballController.Initialize(middle, direction2, force);
        if (hittingCount < 10)
            hittingCount += 1;
    }
}
