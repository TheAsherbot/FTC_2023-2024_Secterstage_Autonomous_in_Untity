using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrussDoor : MonoBehaviour
{
    private void Start()
    {
        HingeJoint joint = GetComponent<HingeJoint>();
        joint.connectedAnchor = transform.position;
    }
}
