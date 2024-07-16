using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatController : MonoBehaviour
{
    public float swingSpeed = 100f;
    public float swingDuration = 0.5f;
    public float swingAngle = 60f;
    public bool isSwinging = false;

    public Transform BatSocket;
    public Transform BatTarget;
    private Transform startTransform;
    public Transform strikeZoneCenter;

    public RectTransform target;
    private Canvas canvas;
    public float moveSpeed = 100f;    

    private void Start()
    {
        startTransform = BatSocket;
        canvas = FindObjectOfType<Canvas>();
    }

    void Update()
    {
        HandleKeyboardInput();
        if (Input.GetKeyDown(KeyCode.Space)&& !isSwinging)
        {
            StartCoroutine(SwingBat());
        }
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
            moveDirection.x -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection.x += 1;
        }

        // 타겟 이동
        target.anchoredPosition += (Vector2)moveDirection * moveSpeed * Time.deltaTime;


    }
    
    public static float GetAngle(Vector3 from, Vector3 to)
    {
        Vector3 v = to - from;
        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    IEnumerator SwingBat()
    {        
        isSwinging = true;
        float elapsedTime = 0f;
        float targetRotation = 60f; // 목표 회전 각도
        float startRotation = transform.rotation.eulerAngles.y; // 초기 y축 회전 값
        float endRotation = startRotation + targetRotation; // 목표 y축 회전 값
        float currentRotation = startRotation;

        while (elapsedTime < swingDuration)
        {
            currentRotation = Mathf.Lerp(startRotation, endRotation, elapsedTime / swingDuration);
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, currentRotation, transform.rotation.eulerAngles.z));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종적으로 정확한 목표 각도로 설정
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, endRotation, transform.rotation.eulerAngles.z));
        isSwinging = false;
    }
}
