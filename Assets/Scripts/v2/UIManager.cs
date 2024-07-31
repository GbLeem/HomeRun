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
    public Image[] ballImage;

    public void UpdateDistanceText(float distance)
    {
        distanceText.text = distance + " m";
    }

    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score : " + newScore;
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
