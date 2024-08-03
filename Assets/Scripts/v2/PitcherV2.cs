using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherV2 : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;

    public Ball ballPrefab;
    public BallData[] ballData;

    //랜덤 위치
    public Transform strikeZone;
    public Vector2 strikeZoneSize;

    //애니메이션
    private Animator pitcherAnimator;

    private void Awake()
    {
        pitcherAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        pitcherAnimator.SetTrigger("Pitch");
    }

    //현재 이 함수가 animation event를 통해서 실행되고 있음
    void Pitching()
    {
        Vector3 randomPoint = GetRandomPointInStrikeZone();

        Ball ball = Instantiate(ballPrefab, startPosition.position, startPosition.rotation);
        Rigidbody rigidbody = ball.GetComponent<Rigidbody>();
        ball.ballState = eBallState.none;

        //Vector3 dir = (endPosition.position - startPosition.position).normalized;
        Vector3 dir = (randomPoint - startPosition.position).normalized;

        rigidbody.AddForce(dir * ballData[0].force, ForceMode.Impulse);
        //StartCoroutine(TimeDelay());        
    }

    Vector3 GetPitcherHandPosition()
    {
        Vector3 handPosition = Vector3.zero;

        return handPosition;
    }

    Vector3 GetRandomPointInStrikeZone()
    {
        float randomX = Random.Range(-strikeZoneSize.x / 2, strikeZoneSize.x / 2);
        float randomY = Random.Range(-strikeZoneSize.y / 2, strikeZoneSize.y / 2);

        Vector3 randomPoint = strikeZone.position + new Vector3(randomX, randomY, 0f);
        return randomPoint;
    }
}