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
    public IEnumerator coroutine;


    //for swing
    public Transform batHand;
    private bool isSwinging = false;
    private Collider batCollider;
    public float swingDuration = 0.2f; // 스윙 시간
    public float swingAngle = 120f;

    //animation
    public Animator anim;

    //Ball state
    private Ball ball;

    private void Awake()
    {
        ball = FindObjectOfType<Ball>();
    }

    private void Start()
    {
        batCollider = GetComponentInChildren<Collider>();
        batCollider.enabled = false;
        batTransform = GetComponentInChildren<Transform>();

        Vector2 pivot = new Vector2(0.5f, 1.5f);
        target.pivot = pivot;

        float batScale = batData.batScale;
        Vector3 scale = new Vector3(batScale, batScale, batScale);
        target.localScale = scale;

        //안보이게 만드는 코드
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }

    }        

    private void Update()
    {
        ball = FindObjectOfType<Ball>();

        HandleKeyboardInput();

        if(Input.GetKeyDown(KeyCode.Space) && !isSwinging)
        {
            //HitTimingUI();
            StartCoroutine(SwingBat());
            //StartCoroutine(SwingUI(1f));
            coroutine = UIManager.instance.SwingUI();
            StartCoroutine(coroutine);

            //NEW
            //UIManager.instance.SwingBatUI(batTransform.eulerAngles.y/100f);

            //swing animation
            anim.SetBool("IsSwing", true);
            anim.SetFloat("swingSpeed", 1.5f);
            StartCoroutine(ResetSwing());
        }

        if (ball != null)
        {
            //쳤을때 체크 하기 -> ui 조정
            if(ball.ballState == eBallState.hitting || ball.ballState == eBallState.foul)
            {
                //Debug.Log("INTO");
                StopCoroutine(coroutine);
                StartCoroutine(UIManager.instance.ResetSwingUI());
                ball.ballState = eBallState.flying;
            }
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

    IEnumerator ResetSwing()
    {        
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.SetBool("IsSwing", false);
    }    
}