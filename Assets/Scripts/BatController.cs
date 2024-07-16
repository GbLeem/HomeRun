using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatController : MonoBehaviour
{
    public float swingSpeed = 100f;
    public float swingDuration = 0.5f;
    public float swingAngle = 60f;
    private bool isSwinging = false;

    public Transform BatSocket;
    private Transform startTransform;
    
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

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection.y += 1;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection.y -= 1;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection.x -= 1;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection.x += 1;
        }

        // 타겟 이동
        target.anchoredPosition += (Vector2)moveDirection * moveSpeed * Time.deltaTime;
    }
    void HandleMouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out mousePos);
            target.anchoredPosition = mousePos;
        }
    }
    public static float GetAngle(Vector3 from, Vector3 to)
    {
        Vector3 v = to - from;
        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    IEnumerator SwingBat()
    {
        isSwinging = true;
        float rotAngle = 0f;

        while(rotAngle < swingAngle)
        {
            //transform.Rotate(Vector3.up, swingSpeed * Time.deltaTime);
            BatSocket.Rotate(BatSocket.up, swingSpeed * Time.deltaTime);
            rotAngle = GetAngle(startTransform.position, transform.position);
            //elapsedTime += Time.deltaTime;
            yield return null;
        }
        isSwinging = false;
        transform.position = startTransform.position;
    }
}
