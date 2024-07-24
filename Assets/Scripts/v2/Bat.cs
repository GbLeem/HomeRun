using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//실제로 치는 동작
public class Bat : MonoBehaviour
{
    //pci anchor ui
    public RectTransform target;
    private float moveSpeed = 200f;

    //for swing
    public Transform batHand;
    private bool isSwinging = false;
    private Collider batCollider;

    public float swingDuration = 0.2f; // 스윙 시간
    public float swingAngle = 120f;

    private void Awake()
    {
    }
    private void Start()
    {
        //batCollider = batHand.GetComponentInChildren<Collider>();
        
        batCollider = GetComponent<Collider>();
        batCollider.enabled = false;
    }

    private void Update()
    {
        HandleKeyboardInput();
        if(Input.GetKeyDown(KeyCode.Space) && !isSwinging)
        {
            StartCoroutine(SwingBat());
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
    IEnumerator SwingBat()
    {
        isSwinging = true;
        batCollider.enabled = true;        

        Quaternion originalRotation = batHand.transform.rotation;
        Quaternion targetRotation = originalRotation * Quaternion.Euler(0, -swingAngle, 0);

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