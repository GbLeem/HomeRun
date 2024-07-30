using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//실제로 치는 동작
public class Bat : MonoBehaviour
{
    //bat data
    public BatData batData;

    //pci anchor UI
    public RectTransform target;
    private float moveSpeed = 300f;
    public Transform batTransform;
    public Camera UIcamera;

    //for timing UI
    public RectTransform timingUI;
    public RectTransform hitTimingUI;
    public float speed = 5f;

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
        batCollider = GetComponentInChildren<Collider>();
        batCollider.enabled = false;
        batTransform = GetComponentInChildren<Transform>();

        //Vector2 pivot = new Vector2(1.0f, 0.7f);
        //target.pivot = pivot;

        float batScale = batData.batScale;
        Vector3 scale = new Vector3(batScale, batScale, batScale);
        target.localScale = scale;
    }

    private void Update()
    {
        HandleKeyboardInput();

        if(Input.GetKeyDown(KeyCode.Space) && !isSwinging)
        {
            //HitTimingUI();
            StartCoroutine(SwingBat());
            StartCoroutine(SwingUI(1f));
        }
    }

    void SettingTargetUIPosition()
    {
        if(target != null && batTransform)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
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
        batHand.transform.position += batDirection * 1.5f *Time.deltaTime;
        
        //PCI 이동
        target.transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    //private void HitTimingUI()
    //{
    //    Vector2 currentPos = hitTimingUI.anchoredPosition;
    //    float baseHeight = timingUI.rect.height;
    //    float barHeight = hitTimingUI.rect.height;

    //    float targetY = baseHeight;
    //    currentPos.y += speed * Time.deltaTime;
    //    //currentPos.y = Mathf.Clamp(currentPos.y, -baseHeight / 2 + barHeight / 2, targetY);

    //    hitTimingUI.anchoredPosition = currentPos;        
    //}

    //TODO 배팅 하면, UI 움직이게
    IEnumerator SwingUI(float duration)
    {
        Vector2 currentPos = hitTimingUI.anchoredPosition;
        Vector2 tempPos = currentPos;

        //최대 100까지
        //float maxPosition = 100f;

        float elapsed = 0f;
        while(elapsed < duration && currentPos.y < 100f)
        {
            currentPos.y += 0.5f;
            elapsed += Time.deltaTime;
            hitTimingUI.anchoredPosition = currentPos;
            yield return null;
        }

        hitTimingUI.anchoredPosition = tempPos;
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

        //yield return new WaitForSeconds(0.5f);

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