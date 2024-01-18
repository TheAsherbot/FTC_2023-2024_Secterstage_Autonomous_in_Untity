using System.Collections;

using UnityEngine;

public class Test : MonoBehaviour
{


    private void Start()
    {
        // Test1();
    }

    private void Test1()
    {
        Debug.Log("TEST 1");
        StartCoroutine(Test2());
        Debug.Log("TEST 6");
    }

    private IEnumerator Test2()
    {
        Debug.Log("TEST 2");
        yield return Test3();
        Debug.Log("TEST 5");
    }

    private IEnumerator Test3()
    {
        Debug.Log("TEST 3");
        yield return new WaitForSeconds(1);
        Debug.Log("TEST 4");
    }

}
