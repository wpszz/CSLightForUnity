using UnityEngine;
using System.Collections;

public abstract class CSMap
{
    public MonoBehaviour behaviour
    {
        get;
        private set;
    }

    protected virtual IEnumerator Start()
    {
        Debug.LogWarning("CSMap.Start");

        yield return 0;
    }

    // start 之后 update 之前
    protected virtual void UpdatePrevious()
    {

    }

    protected virtual void Update()
    {

    }

    protected virtual void Clear()
    {
        GameObject.Destroy(behaviour.gameObject);
    }
}
