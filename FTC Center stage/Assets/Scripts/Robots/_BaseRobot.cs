using System;

using UnityEngine;

public class _BaseRobot : MonoBehaviour
{

    public event Action<StartingSpot> OnPositionDecided;

    public event Action<SpikeMarkSpot> OnSpikeMarkDecided;

    private RenderTexture renderTexture;





    public void Trigger_OnSpikeMarkDecided(SpikeMarkSpot spikeMarkSpot)
    {
        OnSpikeMarkDecided?.Invoke(spikeMarkSpot);
    }

    public void Trigger_OnPositionDecided(StartingSpot startingSpot)
    {
        OnPositionDecided?.Invoke(startingSpot);
    }

}
