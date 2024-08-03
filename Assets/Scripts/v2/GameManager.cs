using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }

    private static GameManager m_instance;
    private int score = 0;
    public bool isGameOver { get; private set; }  
    
    private void Awake()
    {
        if(m_instance != this)
        {
            Destroy(gameObject);
        }
    }   
    
    public void AddScore(int newScore)
    {
        if(!isGameOver)
        {
            score += newScore;
            UIManager.instance.UpdateScoreText(newScore);
        }
    }      
}
