using UnityEngine;

public class RedBackdrop : Backdrop
{



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains(PIXEL_TAG))
        {
            scoreManager.OnRedTeamScoreOnBackdrop?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains(PIXEL_TAG))
        {
            scoreManager.OnRedTeamDescoreOnBackdrop?.Invoke();
        }
    }



}
