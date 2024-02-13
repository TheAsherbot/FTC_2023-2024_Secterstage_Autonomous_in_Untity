using System;
using System.Collections.Generic;

using UnityEngine;

public class AutonomousRoundManager : MonoBehaviour
{


    private const int MAX_ROUND_TIME = 30;


    public event Action OnRoundFinished;


    [SerializeField] private Transform objectParent;


    [Header("Prefabs")]
    [SerializeField] private GameObject whitePixelPrefab;
    [SerializeField] private GameObject yellowPixelPrefab;
    [SerializeField] private GameObject purplePixelPrefab;


    [Header("Robot start Positions")]
    [SerializeField] private Transform redBackStartPositionOffset;
    [SerializeField] private Transform redFrontStartPositionOffset;
    [SerializeField] private Transform blueBackStartPositionOffset;
    [SerializeField] private Transform blueFrontStartPositionOffset;
    [SerializeField] private Robot robot1;
    [SerializeField] private Robot robot2;
    [SerializeField] private Robot robot3;
    [SerializeField] private Robot robot4;
    [SerializeField] private List<AutonomousStartPositionScriptableObject> robotStartPositionList;

    private double elapsedTime = 0;
    private bool isRoundFinished = false;

    private SpikeMarkSpot spikeMarkSpot;

    private bool startNewRound;



    private void Start()
    {
        StartRound();
    }

    private void StartRound()
    {
        foreach (Transform child in objectParent.transform)
        {
            Destroy(child.gameObject);
        }


        Time.timeScale = 1;
        elapsedTime = 0;
        isRoundFinished = false;

        spikeMarkSpot = (SpikeMarkSpot)UnityEngine.Random.Range(0, 3);

        StartRobot(robot1, StartingSpot.RedBack, spikeMarkSpot, true);
        StartRobot(robot2, StartingSpot.RedFront, spikeMarkSpot, true);
        StartRobot(robot3, StartingSpot.BlueBack, spikeMarkSpot, false);
        StartRobot(robot4, StartingSpot.BlueFront, spikeMarkSpot, false);
    }

    private void Update()
    {
        if (startNewRound)
        {
            startNewRound = false;
            StartRound();
            return;
        }

        if (!isRoundFinished)
        {
            elapsedTime += Time.deltaTime;
        }

        if (elapsedTime >= MAX_ROUND_TIME)
        {
            Debug.Log("FINISHED!!!");
            OnRoundFinished?.Invoke();
            elapsedTime = 0;
            isRoundFinished = true;
            Time.timeScale = 0;
        }
    }





    public double GetElapsedTime()
    {
        return elapsedTime;
    }

    private void StartRobot(Robot robot, StartingSpot startingSpot, SpikeMarkSpot spikeMarkSpot, bool isRedTeam)
    {
        for (int i = 0; i < robotStartPositionList.Count; i++)
        {
            if (robotStartPositionList[i].robot == robot)
            {
                _BaseRobot spawnedRobot = Instantiate(robotStartPositionList[i].robotPrefab);

                spawnedRobot.gameObject.tag = isRedTeam ? "Red Robot" : "Blue Robot";
                spawnedRobot.Trigger_OnPositionDecided(startingSpot);
                spawnedRobot.Trigger_OnSpikeMarkDecided(spikeMarkSpot);

                switch (startingSpot)
                {
                    case StartingSpot.RedFront:
                        PositionRobotAndPixel(spawnedRobot, robotStartPositionList[i].redFront, redFrontStartPositionOffset);
                        PositionTeamProp(robotStartPositionList[i].redFront, isRedTeam, redFrontStartPositionOffset, spikeMarkSpot);
                        break;
                    case StartingSpot.RedBack:
                        PositionRobotAndPixel(spawnedRobot, robotStartPositionList[i].redBack, redBackStartPositionOffset);
                        PositionTeamProp(robotStartPositionList[i].redBack, isRedTeam, redBackStartPositionOffset, spikeMarkSpot);
                        break;
                    case StartingSpot.BlueFront:
                        PositionRobotAndPixel(spawnedRobot, robotStartPositionList[i].blueFront, blueFrontStartPositionOffset);
                        PositionTeamProp(robotStartPositionList[i].blueFront, isRedTeam, blueFrontStartPositionOffset, spikeMarkSpot);
                        break;
                    case StartingSpot.BlueBack:
                        PositionRobotAndPixel(spawnedRobot, robotStartPositionList[i].blueBack, blueBackStartPositionOffset);
                        PositionTeamProp(robotStartPositionList[i].blueBack, isRedTeam, blueBackStartPositionOffset, spikeMarkSpot);
                        break;
                }
            }
        }

        Debug.LogWarning($"Robot {robot} has not been assigned a Starting position Scriptable Object. The Robot will be rerolled");

        return;

        void PositionRobotAndPixel(_BaseRobot robot, AutonomousStartPositionScriptableObject.StartingSpot startingSpot, Transform spawnOffset)
        {
            robot.transform.parent = spawnOffset;
            robot.transform.localPosition = startingSpot.position;
            robot.transform.localRotation = startingSpot.rotation;

            if (startingSpot.yellowPixel.usePixel)
            {
                Transform yellowPixel = Instantiate(yellowPixelPrefab, spawnOffset).transform;
                yellowPixel.localPosition = startingSpot.yellowPixel.pixelPosition;
                yellowPixel.localRotation = startingSpot.yellowPixel.pixelRotation;
                yellowPixel.parent = objectParent;
            }
            if (startingSpot.purplePixel.usePixel)
            {
                Transform purplePixel = Instantiate(purplePixelPrefab, spawnOffset).transform;
                purplePixel.localPosition= startingSpot.purplePixel.pixelPosition;
                purplePixel.localRotation = startingSpot.purplePixel.pixelRotation;
                purplePixel.parent = objectParent;
            }
            robot.transform.parent = objectParent;
        }
        void PositionTeamProp(AutonomousStartPositionScriptableObject.StartingSpot startingSpot, bool isOnRedTeam, Transform positionOffset, SpikeMarkSpot spikeMarkSpot)
        {
            if (isOnRedTeam)
            {
                if (startingSpot.redTeamProp.useProp)
                {
                    GameObject teamProp = Instantiate(startingSpot.redTeamProp.teamProp);
                    PositionSpikeMarkIndicator(teamProp.transform);
                    teamProp.transform.parent = objectParent;
                }
                else
                {
                    GameObject whitePixel = SpawnWhitePixel();
                    PositionSpikeMarkIndicator(whitePixel.transform);
                    whitePixel.transform.parent = objectParent;
                }
            }
            else
            {
                if (startingSpot.blueTeamProp.useProp)
                {
                    GameObject teamProp = Instantiate(startingSpot.blueTeamProp.teamProp);
                    PositionSpikeMarkIndicator(teamProp.transform);
                    teamProp.transform.parent = objectParent;
                }
                else
                {
                    GameObject whitePixel = SpawnWhitePixel();
                    PositionSpikeMarkIndicator(whitePixel.transform);
                    whitePixel.transform.parent = objectParent;
                }
            }

            GameObject SpawnWhitePixel()
            {
                return Instantiate(whitePixelPrefab);
            }
            void PositionSpikeMarkIndicator(Transform indicator)
            {
                switch (spikeMarkSpot)
                {
                    case SpikeMarkSpot.Left:
                        indicator.position = positionOffset.GetChild(0).GetChild(0).position;
                        break;
                    case SpikeMarkSpot.Middle:
                        indicator.position = positionOffset.GetChild(0).GetChild(1).position;
                        break;
                    case SpikeMarkSpot.Right:
                        indicator.position = positionOffset.GetChild(0).GetChild(2).position;
                        break;
                }
            }
        }
    }


    public void StartNewRound()
    {
        startNewRound = true;
    }

}
