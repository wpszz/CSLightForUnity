using UnityEngine;
using System.Collections;

public class CSMapTestHelloWorld : CSMap
{
    protected override IEnumerator Start()
    {
        Debug.LogWarning("脚本: HelloWorld ------ 1 ------" + Time.time);
        yield return behaviour.StartCoroutine(base.Start());

        Debug.LogWarning("脚本: HelloWorld ------ 2 ------" + Time.time);

        yield return new WaitForSeconds(1f);

        Debug.LogWarning("脚本: HelloWorld ------ 3 ------" + Time.time);
    }
}
