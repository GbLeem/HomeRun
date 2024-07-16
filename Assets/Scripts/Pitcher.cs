using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitcher : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform PitchPoint;
    public Transform CatcherPoint;
    private BallController ballController;

    public float fastballForce = 30f;
    public float sliderForce = 25f;
    public float forkballForce = 20f;
    enum ePitchType
    {
        FastBall,
        Slider,
        Slurve,
        ForkBall
    };

    private void Start()
    {
        ballController = GetComponent<BallController>();
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
        Vector3 target = CatcherPoint.position + CatcherPoint.right * -0.4f;        
        Vector3 middlePoint = PitchPoint.position + (CatcherPoint.position - PitchPoint.position) * 0.6f;
        PitchBallToCatcher(PitchPoint.position, middlePoint, target, sliderForce, ePitchType.Slider);
    }
    void PitchSlurve()
    {
        Vector3 target = CatcherPoint.position + CatcherPoint.right * -0.3f;
        Vector3 middlePoint = PitchPoint.position + (CatcherPoint.position - PitchPoint.position) * 0.7f;
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
 
    }
}
