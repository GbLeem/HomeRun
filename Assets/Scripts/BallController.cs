using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float lifeTime = 10f;
    private bool Strike = false;
    private bool Catching = false;

    //for draw lines
    public LineRenderer lineRenderer;
    private List<Vector3> positions = new List<Vector3>();

    //변화구
    private Rigidbody rb;
    private bool forceApplied = false;
    private Vector3 middlePoint;
    private Vector3 direction2;
    private float additionalForce;

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
            // Check if the ball has passed the middle point
            //if (transform.position.z < middlePoint.z)
            if (Vector3.Distance(transform.position, middlePoint) < 0.2f)
            {
                rb.velocity = Vector3.zero; // Optional: reset velocity
                //rb.AddForce(direction2 * additionalForce, ForceMode.Impulse);
                rb.AddForceAtPosition(direction2 * additionalForce, middlePoint, ForceMode.Impulse);
                forceApplied = true;
            }
            yield return null;
        }
    }

    private void Start()
    {        
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
        DrawTrajectory();
    }

    void DrawTrajectory()
    {        
        positions.Add(transform.position);     
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            if(other.gameObject.CompareTag("Strike"))
            {
                Strike = true;
                Debug.Log("Strike");
                Catching = true;
            }
            if(Catching == false && other.gameObject.CompareTag("Ball"))
            {               
                Strike = false;
                Debug.Log("Ball");
                Catching = false;
            }                        
        }        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bat"))
        {
            Debug.Log("Hit");
            Vector3 hitDirection = (transform.position - collision.transform.position).normalized;

            //타이밍을 계산해서 hitforce변화를 준다.
            float hitForce = 50f;

            rb.AddForce(hitDirection * hitForce, ForceMode.Impulse);
        }
    }
}