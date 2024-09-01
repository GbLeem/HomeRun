using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherV2 : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;

    public Ball ballPrefab;

    //스크럽터블 데이터 이용
    public BallDataV2[] ballDatas;
    private int totalBallDataSize;

    //랜덤 위치
    public Transform strikeZoneCenter;
    //private Vector2 strikeZoneSize;

    private Vector2 maxStrikeZone;
    private Vector2 minStrikeZone;

    //애니메이션
    private Animator pitcherAnimator;

    //ball state
    private Ball ball;

    //sound
    private AudioSource pitcherAudio;
    public AudioClip pitchAudio;
    public AudioSource backgroundMusic;

    //ball name
    private string ballName;

    private void Awake()
    {
        pitcherAnimator = GetComponent<Animator>();
        pitcherAudio = GetComponent<AudioSource>();
        ball = FindObjectOfType<Ball>();        
    }

    private void Start()
    {
        //맨 처음에는 그냥 던짐
        if (UIManager.instance.ballCount == 0)
        {            
            pitcherAnimator.SetBool("canPitch", true);
        }

        //random ball type
        totalBallDataSize = ballDatas.Length;

        //background music
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.loop = true;  // 무한 루프 설정
            backgroundMusic.Play();       // 음악 재생
        }        
    }

    private void Update()
    {        
        if (ball == null)
        {
            ball = FindObjectOfType<Ball>();
        }

        //여기서 ball state 가져와서 set bool로 적용
        //has exit time은 pitch 하고 돌아올때만 적용
        if (ball != null)
        {
            if (ball.ballState == eBallState.finish)
            {
                pitcherAnimator.SetBool("canPitch", true);
            }
            else
            {
                pitcherAnimator.SetBool("canPitch", false);
            }
        }
        
        if(UIManager.instance.ballCount == 10)
        {
            //애니메이션 멈추기            
            pitcherAnimator.enabled = false;
        }

        if(ballName == "trash")
        {
            if(ball != null)
            {                
                ball.ColorChange(); 
                if(ball.ballState == eBallState.flying ||ball.ballState == eBallState.hitting)
                {
                    UIManager.instance.ShowTrashImage();
                }
            }
        }
        
    }  

    //animation event를 통해서 실행되는 함수
    void Pitching()
    {
        //sound
        pitcherAudio.PlayOneShot(pitchAudio, 0.5f);

        int index = SelectBallIndex();
        
        //ball data를 통해 random 한 투구 영역 정하기
        maxStrikeZone = ballDatas[index].maxStrikeZone;
        minStrikeZone = ballDatas[index].minStrikeZone;

        //최대 공 갯수 보다 작으면 공 던지도록 함
        if(UIManager.instance.ballCount < 10)
        {
            Vector3 randomPoint = GetRandomPointInStrikeZone();

            Ball ball = Instantiate(ballPrefab, startPosition.position, startPosition.rotation);
            Rigidbody rigidbody = ball.GetComponent<Rigidbody>();
            ball.ballState = eBallState.none;

            Vector3 dir = (randomPoint - startPosition.position).normalized;

            rigidbody.AddForce(dir * ballDatas[index].force, ForceMode.Impulse);

            //
            if (ballDatas[index].ballname == "trash")
            {
                ballName = "trash";
                ball.ColorChange();
            }
            else
            {
                ballName = "none";
            }

            //커브
            if (ballDatas[index].curveForce.y != 0)
                rigidbody.AddForce(ballDatas[index].curveForce, ForceMode.Impulse);
                            
            StartCoroutine(BreakingBall(rigidbody, index));
        }

        UIManager.instance.ShowBallText(ballDatas[index].ballname);
    }
    
    //변화구의 경우 추가로 Addforce 해주는 코루틴 
    IEnumerator BreakingBall(Rigidbody rb, int index)
    {
        //1 4seam
        //2 slider
        //3 forkball

        if (ballDatas[index].breakingballDuration == 0)
            yield return null;

        yield return new WaitForSeconds(ballDatas[index].delayTime);
        
        float elapsedTime = 0f;
        while (elapsedTime < ballDatas[index].breakingballDuration)
        {
            rb.AddForce(ballDatas[index].forceDir * ballDatas[index].breakingballForce * Time.deltaTime, ForceMode.VelocityChange);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }    

    //ball data에서 랜덤한 구종 정하기
    int SelectBallIndex()
    {
        totalBallDataSize = ballDatas.Length;
        int idx = Random.Range(0, totalBallDataSize);        
        return idx;
    }

    //ball data로부터 가져온 위치를 통해 투구가 될 위치 설정
    Vector3 GetRandomPointInStrikeZone()
    {        
        float randomX = Random.Range(minStrikeZone.x, maxStrikeZone.x);
        float randomY = Random.Range(minStrikeZone.y, maxStrikeZone.y);

        Vector3 randomPoint = strikeZoneCenter.position + new Vector3(randomX, randomY, 0f);
        return randomPoint;
    }    
}