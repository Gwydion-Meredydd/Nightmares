using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public int Points;
    public bool DoublePoints;
    public void PointsIncrease(int Score)
    {
        if (DoublePoints == false)
        {
            Points = Points + Score;
        }
        else 
        {
            Points = Points + (Score * 2);
        }
    }
    public void RoundEndIncrease(int Score)
    {
        if (DoublePoints == false)
        {
            Points = Points + (Score * 100);
        }
        else 
        {
            Points = Points + ((Score * 100) * 2);
        }
    }
}
