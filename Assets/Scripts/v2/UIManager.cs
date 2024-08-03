using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public Text distanceText;
    public Text scoreText;
    public Text homeRunText;
    public Image[] ballImage;

    public RectTransform timingUI;
    public RectTransform batUI;
    public float duration = 1f;

    private float displayTime = 1.5f;

    public void UpdateDistanceText(float distance)
    {
        distanceText.text = distance + " m";
    }

    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score : " + newScore;
    }
    public void ShowHomeRunText()
    {
        homeRunText.gameObject.SetActive(true);
        homeRunText.text = "HOME RUN";
        StartCoroutine(HideTextAfterDelay());
    }

    private IEnumerator HideTextAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        homeRunText.gameObject.SetActive(false);
    }
    public IEnumerator SwingUI()
    {
        //yield return null;
        Vector2 currentPos = batUI.anchoredPosition;

        float elapsed = 0f;
        while (elapsed < duration && currentPos.y < 100f)
        {
            currentPos.y += 1.5f; //이 값을 임의로 바꾸는 중, 나중에 변수로 바꿔주기
            elapsed += Time.deltaTime;
            batUI.anchoredPosition = currentPos;
            yield return null;
        }

        //이건 타격이 안되었을때 reset
        batUI.anchoredPosition = new Vector2(0f, -100f);        
    }    

    public IEnumerator ResetSwingUI()
    {
        yield return new WaitForSeconds(2f);
        batUI.anchoredPosition = new Vector2(0, -100f);
    }

    public void UpdateBallImage(int maxBallCount, int ballUsedCount, BallController.eBallState ballState)
    {
        //쳤으면, 
        if (ballState == BallController.eBallState.hitting)
        {
            ballImage[ballUsedCount].color = new Color(0, 1, 0, 0.5f);
        }
        //못치거나 foul일 때  
        else
        {
            ballImage[ballUsedCount].color = new Color(1, 0, 0, 0.5f);
        }
    }
}
