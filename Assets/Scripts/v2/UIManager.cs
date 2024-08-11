using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance;

    //text ui
    public Text distanceText;
    public Text scoreText;
    public Text homeRunText;
    public Text timingText;
    public Text ballText;
    public Text gameOverScoreText;

    //ball count ui
    public Image[] ballImage;
    public int ballCount = 0;

    //timing check ui
    public RectTransform timingUI;
    public RectTransform batUI;
    public float duration = 1f;

    //homerun text delay time
    private float displayTime = 1.5f;

    //Score Check
    private float score = 0f;

    //GameOver
    public GameObject gameOverUI;

    //pitfall
    public Image trashImage;


    //fire works
    public ParticleSystem particle1;
    public ParticleSystem particle2;
    public ParticleSystem particle3;

    public void PlayParticle()
    {
        particle1.gameObject.SetActive(true);
        particle1.Play();
        particle2.gameObject.SetActive(true);
        particle2.Play();
        particle3.gameObject.SetActive(true);
        particle3.Play();

        StartCoroutine(StopParticle());
    }


    IEnumerator StopParticle()
    {
        yield return new WaitForSeconds(3f);
        particle1.gameObject.SetActive(false);
        particle2.gameObject.SetActive(false);
        particle3.gameObject.SetActive(false);
    }

    public void UpdateDistanceText(float distance)
    {
        distanceText.text = string.Format("{0:F2}", distance) + " m";
    }

    public void UpdateScoreText(float newScore)
    {
        score += newScore;
        scoreText.text = "Score : " + string.Format("{0:F2}", score);
    }
    public void ShowHomeRunText()
    {
        homeRunText.gameObject.SetActive(true);
        homeRunText.text = "HOME RUN";
        StartCoroutine(HideTextAfterDelay());
    }

    public void ShowTimingText(eBallTiming timing)
    {
        timingText.gameObject.SetActive(true);
        timingText.text = timing.ToString();
        StartCoroutine(HideTimingTextAfterDelay());
    }

    public void ShowBallText(string name)
    {
        ballText.gameObject.SetActive(true);
        ballText.text = name;
        StartCoroutine(HideBallTextDelay());
    }

    //TODO Delay 함수 통합하기
    private IEnumerator HideTextAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        homeRunText.gameObject.SetActive(false);
    }

    private IEnumerator HideTimingTextAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        timingText.gameObject.SetActive(false);
    }

    private IEnumerator HideBallTextDelay()
    {
        yield return new WaitForSeconds(2f);
        ballText.gameObject.SetActive(false);
    }


    private IEnumerator delayTimeSomeSecondsAndGameOver(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
        if (gameOverScoreText != null)
            gameOverScoreText.text = "Score : " + string.Format("{0:F2}", score);
    }


    public IEnumerator SwingUI()
    {
        //yield return null;
        Vector2 currentPos = batUI.anchoredPosition;

        float elapsed = 0f;
        while (elapsed < duration && currentPos.y < 150f)
        {
            currentPos.y += 3.0f; //TODO 적절한 스피드의 배트 UI
            elapsed += Time.deltaTime;
            batUI.anchoredPosition = currentPos;
            yield return null;
        }

        //이건 타격이 안되었을때 reset
        batUI.anchoredPosition = new Vector2(0f, -150f);        
    }    

    public IEnumerator ResetSwingUI()
    {
        yield return new WaitForSeconds(2f);
        batUI.anchoredPosition = new Vector2(0, -150f);
    }

    public void UpdateBallImage(int ballIdx, eBallState ballState)
    {
        //foul 일 때 빨간색
        if(ballState == eBallState.foul)
        {
            ballImage[ballIdx - 1].color = new Color(1, 0, 0, 0.5f);
        }

        //쳤으면, 초록
        else if (ballState == eBallState.flying)
        {
            ballImage[ballIdx - 1].color = new Color(0, 1, 0, 0.5f);            
        }

        //홈런 파랑
        else if(ballState == eBallState.homerun)
        {
            ballImage[ballIdx - 1].color = new Color(0, 0, 1, 0.5f);
        }

        //못침 빨강
        else if(ballState == eBallState.strike)
        {
            ballImage[ballIdx - 1].color = new Color(1, 0, 0, 0.5f);
        }
               
    }

    public void ShowTrashImage()
    {
        if(trashImage.gameObject.activeSelf == false)
        {
            trashImage.gameObject.SetActive(true);
            StartCoroutine(deactiveImg());
        }
    }

    IEnumerator deactiveImg()
    {
        yield return new WaitForSeconds(4f);
        trashImage.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        StartCoroutine(delayTimeSomeSecondsAndGameOver(3f));                
    }

    public eBallTiming CalculateTimingByUI()
    {
        eBallTiming timing;

        //float timingUITotalSize = timingUI.rect.height;
        float batUICurrentPos = batUI.anchoredPosition.y; //-100 ~ 100

        if (batUICurrentPos < -40f)
            timing = eBallTiming.late;
        else if (batUICurrentPos > -40f && batUICurrentPos < 40f)
            timing = eBallTiming.good;
        else
            timing = eBallTiming.fast;

        return timing;
    }
}
