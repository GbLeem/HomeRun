using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattingController : MonoBehaviour
{
    //Make PCI anchor
    public RectTransform target;
    private Canvas canvas;
    public float moveSpeed = 150f;

    //for swing 
    private bool isSwinging = false;
    public Transform cylinder;
    public Transform batHand;
    public float swingDuration = 0.5f; // 스윙 시간
    public float swingAngle = 120f; // 회전 각도

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    void HandleKeyboardInput()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection.y += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection.y -= 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection.x += 1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection.x -= 1;
        }

        // 타겟 이동
        cylinder.transform.position += moveDirection * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        HandleKeyboardInput();
        if(Input.GetKey(KeyCode.Space) && !isSwinging)
        {
            StartCoroutine(SwingBat());            
        }        
    }

    IEnumerator SwingBat()
    {
        isSwinging = true;

        Quaternion originalRotation = transform.rotation; //기존 회전값
        Quaternion targetRotation = originalRotation * Quaternion.Euler(0, swingAngle, 0); //목표 회전값

        yield return RotateOverTime(originalRotation, targetRotation, swingDuration); //회전
        yield return RotateOverTime(targetRotation, originalRotation, swingDuration); //돌아오기
        isSwinging = false;
    }
    private IEnumerator RotateOverTime(Quaternion fromRotation, Quaternion toRotation, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(fromRotation, toRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = toRotation;
    }
    //IEnumerator SwingBat()
    //{
    //    Quaternion initialRotation = batHand.localRotation;
    //    Quaternion targetRotation = initialRotation * Quaternion.Euler(0, swingAngle, 0);

    //    float elapsedTime = 0f;

    //    while (elapsedTime < swingDuration)
    //    {
    //        batHand.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / swingDuration);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    // 최종 회전 상태 설정
    //    batHand.localRotation = targetRotation;

    //    // 원래 상태로 복귀 (선택적)
    //    yield return new WaitForSeconds(0.5f); // 잠시 대기
    //    elapsedTime = 0f;
    //    while (elapsedTime < swingDuration)
    //    {
    //        batHand.localRotation = Quaternion.Slerp(targetRotation, initialRotation, elapsedTime / swingDuration);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    // 복귀 완료
    //    batHand.localRotation = initialRotation;
    //}
}

