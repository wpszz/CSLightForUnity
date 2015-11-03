using UnityEngine;
using System.Collections;

public class CSMapTestCoroutine : CSMap
{
    protected override IEnumerator Start()
    {
        Debug.LogWarning("脚本: Coroutine ------ 1 ------" + Time.time);

        // 调用父类协程
        yield return behaviour.StartCoroutine(base.Start());

        Debug.LogWarning("脚本: Coroutine ------ 2 ------" + Time.time);

        for (int i = 0; i < 5; i++)
        {
            // 只支持在for表达式第1层执行yield表达式
            yield return new WaitForSeconds(1f);

            Debug.LogWarningFormat("脚本: Coroutine ------ for({0}) ------{1}", i, Time.time);
        }

        if (UnityEngine.Random.Range(0, 2) > 0)
        {
            Debug.LogWarningFormat("脚本: Coroutine ------ if() ------{0}", Time.time);

            // 只支持在if表达式末尾执行yield表达式
            yield return new WaitForSeconds(1f);
        }

        // 暂不支持的用法
/*
        for (int i = 0; i < 5; i++)
        {
            if (i == 5)
                yield return new WaitForSeconds(1f);
        }

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
                yield return new WaitForSeconds(1f);
        }

        if (UnityEngine.Random.Range(0, 2) > 0)
        {
            yield return new WaitForSeconds(1f);

            Debug.LogWarningFormat("脚本: Coroutine ------ if() ------{0}", Time.time);
        }
*/
    }
}
