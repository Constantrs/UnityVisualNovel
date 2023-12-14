using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using Dialogue;
using System;
using Unity.VisualScripting.Antlr3.Runtime;

public class UniTaskSample
{
    public async UniTask Start()
    {
        var rusultA = Example();
        var rusultB = Example();
        var rusultC = Example();

        await UniTask.WhenAll(rusultA, rusultB, rusultC);
    }

    public async UniTask StartWait()
    {
        await UniTask.WhenAll(Wait(1000), Wait(3000), Wait(5000));
    }

    public async UniTask Example()
    {
        // 1000ミリ秒
        await UniTask.Delay(1000);
    }

    public async UniTask Wait(int time)
    {
        await UniTask.Delay(time);
        Debug.Log( $"WaitTime : {time}");
    }
}

public class UniTaskDelayProcess
{
    public enum ProcessState
    {
        Ready,
        Executing,
        Completed,
    }

    private ProcessState _state = ProcessState.Ready;
    private CancellationTokenSource cts;

    public UniTaskDelayProcess()
    {
        cts = new CancellationTokenSource();
    }

    public async UniTask Processing()
    {
        Debug.Log("Start Process");
        _state = ProcessState.Executing;
        await UniTask.Delay(500, delayTiming : PlayerLoopTiming.Update, cancellationToken : cts.Token);
        _state = ProcessState.Completed;
    }

    public async UniTask Processing2()
    {
        Debug.Log("Start Process");
        _state = ProcessState.Executing;
        await UniTask.DelayFrame(60, delayTiming: PlayerLoopTiming.Update, cancellationToken: cts.Token);
        _state = ProcessState.Completed;
    }

    public void StopProcess()
    {
        _state = ProcessState.Completed;
        cts.Cancel();
    }

    public bool IsExecuting()
    {
        return _state == ProcessState.Executing;
    }

    public bool IsCompleted()
    {
        return _state == ProcessState.Completed;
    }
}

public class UniTaskYieldProcess
{
    public enum ProcessState
    {
        Ready,
        Executing,
        Completed,
    }

    private float timer = 0.0f;
    private float waittime = 0.0f;

    private ProcessState _state = ProcessState.Ready;
    private CancellationTokenSource _cts;
    private PlayerLoopTiming _timing;

    public UniTaskYieldProcess(float waittime, PlayerLoopTiming timing)
    {
        this.waittime = waittime;
        _cts = new CancellationTokenSource();
        _timing = timing;
    }

    public async UniTask Process()
    {
        try
        {
            await Run(_timing, _cts.Token);
            _state = ProcessState.Completed;
        }
        catch (OperationCanceledException e)
        {
            Debug.Log(" UniTaskYieldProcess has been cancel!");
        }
    }

    public async UniTask Run(PlayerLoopTiming timing, CancellationToken taken)
    {
        Debug.Log("Start Process");
       _state = ProcessState.Executing;
       while (timer < waittime)
       {
            var manager = UniTaskTest.instance;
            if (manager == null)
            {
                return;
            }

            timer += manager.GetTimeScale();
            await UniTask.Yield(timing: _timing, cancellationToken: _cts.Token);
        }

    }

    public void StopProcess()
    {
        _state = ProcessState.Completed;
        _cts.Cancel();
        _cts.Dispose();
    }

    public bool IsExecuting()
    {
        return _state == ProcessState.Executing;
    }

    public bool IsCompleted()
    {
        return _state == ProcessState.Completed;
    }
}

public class UniTaskTest : MonoBehaviour
{
    public static UniTaskTest instance { get; private set; }

    public enum FrameMode
    {
        Unlimited = 0,
        FPS_30,
        FPS_60,
        FPS_120,
    }

    public UniTaskSample sample;
    public UniTaskYieldProcess process;

    public FrameMode frameMode;
    public float frame = 0.0f;
    public bool pause = false;
    public bool exit = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {

        }

        switch (frameMode) 
        {
            case FrameMode.Unlimited:
                QualitySettings.vSyncCount = 1;
                Application.targetFrameRate = -1;
                break;
            case FrameMode.FPS_30:
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 30;
                break;
            case FrameMode.FPS_60:
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 60;
                break;
            case FrameMode.FPS_120:
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 120;
                break;
        }

    }

    private void OnDestroy()
    {
        if(instance == this) 
        {
            instance = null;
        }
    }


    private void Start()
    {
        UpdateProcess().Forget();
        UpdateMainloop().Forget();
    }

    private async UniTask UpdateProcess()
    {
        while (!exit)
        {
            if (frame == 0.0f)
            {
                //process = new UniTaskProcess();
                //process.Processing2().Forget();
                process = new UniTaskYieldProcess(120.0f, PlayerLoopTiming.Update);
                process.Process().Forget();
                Debug.Log("Start Update");
            }

            await UniTask.Yield(PlayerLoopTiming.EarlyUpdate);
        }
    }

    private async UniTask UpdateMainloop()
    {
        while (!exit)
        {
            if (process != null && !process.IsCompleted())
            {
                frame += GetTimeScale();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                process.StopProcess();
                process = null;
            }

            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
        }
    }

    public float GetTimeScale()
    {
        if (pause) 
        {
            return 0.0f;
        }
        else
        {
            if(frameMode == FrameMode.FPS_30)
            {
                return 2.0f;
            }
            else if(frameMode == FrameMode.FPS_60)
            {
                return 1.0f;
            }
            else if(frameMode == FrameMode.FPS_120)
            {
                return 0.5f;
            }
            else
            {
                return Time.deltaTime;
            }
        }
    }
}
