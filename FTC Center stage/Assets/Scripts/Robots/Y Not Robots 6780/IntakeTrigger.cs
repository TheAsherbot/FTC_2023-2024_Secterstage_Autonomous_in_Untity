using System;

using UnityEngine;

public class IntakeTrigger : MonoBehaviour
{
    public event Action<GameObject> OnTriggerStart;
    public event Action<GameObject> OnTrigger;
    public event Action<GameObject> OnTriggerStop;




    private void OnTriggerEnter(Collider other)
    {
        OnTriggerStart?.Invoke(other.gameObject);
    }
    private void OnTriggerStay(Collider other)
    {
        OnTrigger?.Invoke(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        OnTriggerStop?.Invoke(other.gameObject);
    }


}
