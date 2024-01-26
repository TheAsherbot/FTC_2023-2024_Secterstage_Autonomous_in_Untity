using System;
using System.Collections.Generic;

using UnityEngine;

public class AutonomousRoundManager : MonoBehaviour
{

    


    private const int MAX_ROUND_TIME = 30;


    public event Action OnRoundFinished;


    [Header("Prefabs")]
    [SerializeField] private GameObject yellowPixelPrefab;
    [SerializeField] private GameObject purplePixelPrefab;

    [Header("Robot start Positions")]
    [SerializeField] private Transform redBackStartPositionParent;
    [SerializeField] private Transform redFrontStartPositionParent;
    [SerializeField] private Transform blueBackStartPositionParent;
    [SerializeField] private Transform blueFrontStartPositionParent;
    [SerializeField] private Robot robot1;
    [SerializeField] private Robot robot2;
    [SerializeField] private Robot robot3;
    [SerializeField] private Robot robot4;
    [SerializeField] private List<AutonomousStartPositionScriptableObject> robotStartPositionList;

    private double elapsedTime = 0;
    private bool isRoundFinished = false;




    private void Start()
    {
        StartRound();
    }

    public void StartRound()
    {
        Time.timeScale = 1;
        elapsedTime = 0;
        isRoundFinished = false;

        StartRobot(robot1, StartingSpot.RedBack);
        StartRobot(robot2, StartingSpot.RedFront);
        StartRobot(robot3, StartingSpot.BlueBack);
        StartRobot(robot4, StartingSpot.BlueFront);
    }

    private void Update()
    {
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

    private void StartRobot(Robot robot, StartingSpot startingSpot)
    {
        for (int i = 0; i < robotStartPositionList.Count; i++)
        {
            if (robotStartPositionList[i].robot == robot)
            {
                _BaseRobot spawnedRobot = Instantiate(robotStartPositionList[i].robotPrefab).GetComponent<_BaseRobot>();

                spawnedRobot.Trigger_OnPositionDecided(startingSpot);
                switch (startingSpot)
                {
                    case StartingSpot.RedFront:
                        PositionRobotAndPixel(spawnedRobot, robotStartPositionList[i].redFront, transform.parent.parent, redFrontStartPositionParent);
                        break;
                    case StartingSpot.RedBack:
                        PositionRobotAndPixel(spawnedRobot, robotStartPositionList[i].redBack, transform.parent.parent, redBackStartPositionParent);
                        break;
                    case StartingSpot.BlueFront:
                        PositionRobotAndPixel(spawnedRobot, robotStartPositionList[i].blueFront, transform.parent.parent, blueFrontStartPositionParent);
                        break;
                    case StartingSpot.BlueBack:
                        PositionRobotAndPixel(spawnedRobot, robotStartPositionList[i].blueBack, transform.parent.parent, blueBackStartPositionParent);
                        break;
                }
            }
        }

        Debug.LogWarning($"Robot {robot} has not been assigned a Starting position Scriptable Object. The Robot will be rerolled");

        return;

        void PositionRobotAndPixel(_BaseRobot robot, AutonomousStartPositionScriptableObject.StartingSpot startingSpot, Transform parent, Transform spawnOffset)
        {
            robot.transform.parent = spawnOffset;
            robot.transform.localPosition = startingSpot.position;
            robot.transform.localRotation = startingSpot.rotation;

            if (startingSpot.yellowPixel.usePixel)
            {
                Transform yellowPixel = Instantiate(yellowPixelPrefab, spawnOffset).transform;
                yellowPixel.localPosition = startingSpot.yellowPixel.pixelPosition;
                yellowPixel.localRotation = startingSpot.yellowPixel.pixelRotation;
                yellowPixel.parent = parent;
            }
            if (startingSpot.purplePixel.usePixel)
            {
                Transform purplePixel = Instantiate(purplePixelPrefab, spawnOffset).transform;
                purplePixel.localPosition= startingSpot.purplePixel.pixelPosition;
                purplePixel.localRotation = startingSpot.purplePixel.pixelRotation;
                purplePixel.parent = parent;
            }
            robot.transform.parent = parent;
        }
    }




}
