using System;

using UnityEngine;

[CreateAssetMenu(fileName = "Autonomous Start Position ScriptableObject", menuName = "Autonomous Start Position", order = 0)]
public class AutonomousStartPositionScriptableObject : ScriptableObject
{
    [Serializable]
    public struct StartingSpot
    {
        [Serializable]
        public struct Pixel
        {
            public bool usePixel;
            public Vector3 pixelPosition;
            public Quaternion pixelRotation;
        }

        [Serializable]
        public struct TeamProp
        {
            public bool useProp;
            public GameObject teamProp;
            public Vector3 propOffset;
            public Quaternion propRotation;
        }


        public Vector3 position;
        public Quaternion rotation;

        public Pixel yellowPixel;
        public Pixel purplePixel;
        public TeamProp redTeamProp;
        public TeamProp blueTeamProp;
    }


    public Robot robot;
    public _BaseRobot robotPrefab;

    public StartingSpot redFront;
    public StartingSpot redBack;
    public StartingSpot blueFront;
    public StartingSpot blueBack;




}
