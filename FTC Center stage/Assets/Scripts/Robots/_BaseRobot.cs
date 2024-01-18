using System;

using UnityEngine;

public class _BaseRobot : MonoBehaviour
{

    public event Action<StartingSpot> OnPositionDecided;






    public void Trigger_OnPositionDecided(StartingSpot startingSpot)
    {
        OnPositionDecided?.Invoke(startingSpot);
    }

}
