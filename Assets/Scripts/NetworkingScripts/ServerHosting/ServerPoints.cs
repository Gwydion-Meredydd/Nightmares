using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPoints : MonoBehaviour
{
    public static ServerPoints _serverPoints;
    public ServerPoints RefServerPoints;
    public int[] Points;
    public bool DoublePoints;
    private void Start()
    {
        RefServerPoints = this;
        _serverPoints = RefServerPoints;
    }
    public void PointsIncrease(int PlayerID,int Score)
    {
        if (DoublePoints == false)
        {
            Points[PlayerID] = Points[PlayerID] + Score;
        }
        else
        {
            Points[PlayerID] = Points[PlayerID] + (Score * 2);
        }
      
    }
    public void RoundEndIncrease(int Score)
    {
        for (int i = 0; i < Points.Length; i++)
        {
            if (DoublePoints == false)
            {

                Points[i] = Points[i] + (Score * 100);
            }
            else
            {
                Points[i] = Points[i] + ((Score * 100) * 2);
            }
        }
    }
}
