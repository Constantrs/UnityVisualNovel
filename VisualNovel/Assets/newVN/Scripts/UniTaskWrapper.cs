using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Commands
{
    public class UniTaskWapper
    {
        public bool isDone = false;

        private UniTask _task;
        private CancellationTokenSource _cts;
        private PlayerLoopTiming _executeTiming = PlayerLoopTiming.Update;

        public UniTaskWapper(PlayerLoopTiming timing)
        {
            _cts = new CancellationTokenSource();
            _executeTiming = timing;
        }

        private async void StartTask()
        {
            _task = Execute();
            await _task;
        }

        private async UniTask Execute()
        {
            await UniTask.Yield(_executeTiming);
        }
    }
}
