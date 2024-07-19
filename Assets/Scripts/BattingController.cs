using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattingController : MonoBehaviour
{
    //Make PCI anchor
    public RectTransform target;
    private Canvas canvas;    
    public float moveSpeed = 200f;

    //for swing 
    private bool isSwinging = false;    
    public Transform batHand;
    private Collider batCollider;

    //TODO 적절한 값 찾고 private로 바꾸자
    public float swingDuration = 0.2f; // 스윙 시간
    public float swingAngle = 120f; // 회전 각도

    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        batCollider = batHand.GetComponentInChildren<Collider>();

        batCollider.enabled = false;

        //안보이게 만드는 코드
        MeshRenderer meshRenderer = batHand.GetComponentInChildren<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
    }

    void HandleKeyboardInput()
    {
        Vector3 moveDirection = Vector3.zero;
        Vector3 batDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            batDirection.y += 1;
            moveDirection.y += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            batDirection.y -= 1;
            moveDirection.y -= 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection.x -= 1;
            batDirection.x += 1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection.x += 1;
            batDirection.x -= 1;
        }

        // 타겟 이동
        // 타겟은 안보이게 하기
        batHand.transform.position += batDirection * Time.deltaTime;

        //PCI 이동
        target.transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        HandleKeyboardInput();
        if(Input.GetKeyDown(KeyCode.Space) && !isSwinging)
        {
            StartCoroutine(SwingBat());            
        }        
    }

    IEnumerator SwingBat()
    {
        isSwinging = true;

        //타격하기 전에는 collider 꺼두기
        batCollider.enabled = true;
        //회전 수정 완료        
        Quaternion originalRotation = batHand.transform.rotation; //기존 회전값
        Quaternion targetRotation = originalRotation * Quaternion.Euler(0, -swingAngle, 0); //목표 회전값

        yield return RotateOverTime(originalRotation, targetRotation, swingDuration); //회전
        yield return RotateOverTime(targetRotation, originalRotation, swingDuration); //돌아오기
        isSwinging = false;
        batCollider.enabled = false;
    }

    private IEnumerator RotateOverTime(Quaternion fromRotation, Quaternion toRotation, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            batHand.transform.rotation = Quaternion.Slerp(fromRotation, toRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        batHand.transform.rotation = toRotation;
    }
}

