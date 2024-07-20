using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Pitcher pitcher;
    //public BallController ballController;    
    public Text distanceText;
    public List<Image> ballImageList;
    private int ballIdx;

    private void Awake()
    {
        //ballController = FindObjectOfType<BallController>();
        //pitcher = GetComponent<Pitcher>();
    }
    private void Start()
    {
        pitcher = FindObjectOfType<Pitcher>();
    }
    private void Update()
    {
        //distanceText.text = string.Format("{0}", pitcher.ballDistance);
        ballIdx = pitcher.hittingCount;

        if(ballIdx >= 0)
        {
            for(int i = 0; i < ballIdx; ++i)
            {
                ballImageList[i].color = Color.black;    
            }
        }
        //for (int i = 0; i < ballIdx; ++i)
        //    ballImageList[ballIdx].color = Color.black;
    }
}
