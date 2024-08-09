using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameinstance
    {
        get
        {
            if(m_gameinstance == null)
            {
                m_gameinstance = FindObjectOfType<GameManager>();
            }
            return m_gameinstance;
        }
    }

    private static GameManager m_gameinstance;
    private float score = 0;
    public bool isGameOver { get; private set; }  
    
    private void Awake()
    {
        if(m_gameinstance != this)
        {
            Destroy(gameObject);
        }
    }   
    
    public void AddScore(float newScore)
    {
        if(!isGameOver)
        {
            score += newScore;
            UIManager.instance.UpdateScoreText(newScore);
        }
    }      
}
