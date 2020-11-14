using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public int Points;
    public void PointsIncrease(int Score) 
    {
        Points = Points + Score;
    }
    public void RoundEndIncrease(int Score) 
    {
        Points = Points + (Score * 100);
    }
}
