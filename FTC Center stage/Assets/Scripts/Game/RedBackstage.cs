using UnityEngine;

public class RedBackstage : Backdrop
{



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains(PIXEL_TAG))
        {
            scoreManager.OnRedTeamScoreOnBackstage?.Invoke();
        }
        else if (other.tag == BLUE_ROBOT_TAG)
        {
            scoreManager.OnBlueMinorPenalty?.Invoke();
        }
        else if (other.tag == RED_ROBOT_TAG)
        {
            scoreManager.OnRedRobotEnterBackstage?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains(PIXEL_TAG))
        {
            scoreManager.OnRedTeamDescoreOnBackstage?.Invoke();
        }
        else if (other.tag == RED_ROBOT_TAG)
        {
            scoreManager.OnRedRobotExitBackstage?.Invoke();
        }
    }



}
