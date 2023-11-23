using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineWrapper
{
    public bool isDone = false;

    private MonoBehaviour owner;
    private Coroutine coroutine;
    public CoroutineWrapper(MonoBehaviour owner, Coroutine coroutine)
    {
        this.owner = owner;
        this.coroutine = coroutine;
    }
    public void Stop()
    {
        owner.StopCoroutine(coroutine);
        isDone = true;
    }
}
