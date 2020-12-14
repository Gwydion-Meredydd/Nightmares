using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public ScriptsManager SM;
    public int Points;
    public bool DoublePoints;
    public void PointsIncrease(int Score)
    {
        if (DoublePoints == false)
        {
            Points = Points + Score;
            SM.ScoreScript.Score = SM.ScoreScript.Score + Score;
        }
        else 
        {
            Points = Points + (Score * 2);
            SM.ScoreScript.Score = SM.ScoreScript.Score + (Score * 2);
        }
        SM.GameMenuScript.ScoreText.text = Points.ToString();
    }
    public void RoundEndIncrease(int Score)
    {
        if (DoublePoints == false)
        {
            Points = Points + (Score * 100);
            SM.ScoreScript.Score = SM.ScoreScript.Score + (Score * 100);
        }
        else 
        {
            Points = Points + ((Score * 100) * 2);
            SM.ScoreScript.Score = SM.ScoreScript.Score + ((Score * 100) * 2);
        }
        SM.GameMenuScript.ScoreText.text = Points.ToString();
    }
}
