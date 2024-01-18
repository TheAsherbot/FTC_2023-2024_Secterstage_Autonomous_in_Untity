using UnityEngine;

public class BlueBackstage : Backdrop
{



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains(PIXEL_TAG))
        {
            scoreManager.OnBlueTeamScoreOnBackstage?.Invoke();
        }
        else if (other.tag == RED_ROBOT_TAG)
        {
            scoreManager.OnRedMinorPenalty?.Invoke();
        }
        else if (other.tag == BLUE_ROBOT_TAG)
        {
            scoreManager.OnBlueRobotEnterBackstage?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains(PIXEL_TAG))
        {
            scoreManager?.OnBlueTeamScoreOnBackstage?.Invoke();
        }
        else if (other.tag == BLUE_ROBOT_TAG)
        {
            scoreManager.OnBlueRobotExitBackstage?.Invoke();
        }
    }



}
