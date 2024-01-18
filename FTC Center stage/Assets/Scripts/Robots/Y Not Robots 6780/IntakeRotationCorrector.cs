using UnityEngine;

public class IntakeRotationCorrector : MonoBehaviour
{

    [SerializeField] private YNotRobots6780 YNotRobots6780;

    private void LateUpdate()
    {
        if (YNotRobots6780.IntakeRotation < -90)
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
