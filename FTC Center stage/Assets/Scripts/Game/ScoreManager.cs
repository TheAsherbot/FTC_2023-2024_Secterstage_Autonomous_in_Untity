using System;

using UnityEngine;

public class ScoreManager : MonoBehaviour
{


    public event Action<Alliances> OnAllianceWin;


    public Action OnRedTeamScoreOnBackstage;
    public Action OnRedTeamDescoreOnBackstage;
    public Action OnRedTeamScoreOnBackdrop;
    public Action OnRedTeamDescoreOnBackdrop;
    public Action OnRedMinorPenalty;
    public Action OnRedRobotEnterBackstage;
    public Action OnRedRobotExitBackstage;


    public Action OnBlueTeamScoreOnBackstage;
    public Action OnBlueTeamDescoreOnBackstage;
    public Action OnBlueTeamScoreOnBackdrop;
    public Action OnBlueTeamDescoreOnBackdrop;
    public Action OnBlueMinorPenalty;
    public Action OnBlueRobotEnterBackstage;
    public Action OnBlueRobotExitBackstage;



    [SerializeField] private AutonomousRoundManager autonomousRoundManager;


    private bool is1RedRobotInBackstage;
    private bool is2RedRobotsInBackstage;
    private bool is1BlueRobotInBackstage;
    private bool is2BlueRobotsInBackstage;

    private int redScore;
    private int blueScore;


    private void Start()
    {
        autonomousRoundManager.OnRoundFinished += () =>
        {
            if (is2RedRobotsInBackstage)
            {
                ScoreRed(10); // 5 for each robot
            }
            else if (is1RedRobotInBackstage)
            {
                ScoreRed(5); // Only 1 robot in
            }
            if (is2BlueRobotsInBackstage)
            {
                ScoreBlue(10); // 5 for each robot
            }
            else if (is1BlueRobotInBackstage)
            {
                ScoreBlue(5); // Only 1 robot in
            }

            Debug.Log("Blue score: " + blueScore);
            Debug.Log("Red score: " + redScore);

            OnAllianceWin?.Invoke(blueScore > redScore ? Alliances.Blue : Alliances.Red);
        };

        OnRedTeamScoreOnBackstage += () =>
        {
            ScoreRed(3);
        };
        OnRedTeamDescoreOnBackstage += () =>
        {
            ScoreRed(-3);
        };
        OnRedTeamScoreOnBackdrop += () =>
        {
            ScoreRed(5);
        };
        OnRedTeamDescoreOnBackdrop += () =>
        {
            ScoreRed(-5);
        };
        OnRedMinorPenalty += () =>
        {
            ScoreBlue(10);
        };
        OnRedRobotEnterBackstage += () =>
        {
            if (is1RedRobotInBackstage)
            {
                is2RedRobotsInBackstage = true;
            }
            else
            {
                is1RedRobotInBackstage = true;
            }
        };
        OnRedRobotExitBackstage += () =>
        {
            if (is2RedRobotsInBackstage)
            {
                is2RedRobotsInBackstage = false;
            }
            else
            {
                is1RedRobotInBackstage = false;
            }
        };


        OnBlueTeamScoreOnBackstage += () =>
        {
            ScoreBlue(3);
        };
        OnBlueTeamDescoreOnBackstage += () =>
        {
            ScoreBlue(-3);
        };
        OnBlueTeamScoreOnBackdrop += () =>
        {
            ScoreBlue(5);
        };
        OnBlueTeamDescoreOnBackdrop += () =>
        {
            ScoreBlue(-5);
        };
        OnBlueMinorPenalty += () =>
        {
            ScoreRed(10);
        };
        OnBlueRobotEnterBackstage += () =>
        {
            if (is1BlueRobotInBackstage)
            {
                is2BlueRobotsInBackstage = true;
            }
            else
            {
                is1BlueRobotInBackstage = true;
            }
        };
        OnBlueRobotExitBackstage += () =>
        {
            if (is2BlueRobotsInBackstage)
            {
                is2BlueRobotsInBackstage = false;
            }
            else
            {
                is1BlueRobotInBackstage = false;
            }
        };
    }



    private void ScoreRed(int amount)
    {
        redScore += amount;
    }
    private void ScoreBlue(int amount)
    {
        blueScore += amount;
    }



}
