using UnityEngine;

public class BlueBackdrop : Backdrop
{



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains(PIXEL_TAG))
        {
            scoreManager.OnBlueTeamScoreOnBackdrop?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains(PIXEL_TAG))
        {
            scoreManager.OnBlueTeamDescoreOnBackdrop?.Invoke();
        }
    }



}
