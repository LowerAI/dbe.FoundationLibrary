using dbe.FoundationLibrary.Core.Common;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dbe.FoundationLibrary.Core.FiniteStateMachine
{
    /// <summary>
    /// 状态机
    /// </summary>
    public class StateMachine
    {
        #region    字段 Start
        /// <summary>
        /// 运行 Update 时间间隔 毫秒
        /// </summary>
        public int RunInterval = 50;
        /// <summary>
        /// 当前状态
        /// </summary>
        private RunningStatus CurrentState = RunningStatus.Stop;
        /// <summary>
        /// 字典存放当前所有对象
        /// </summary>
        private Dictionary<string, IStateObject> DictState = new Dictionary<string, IStateObject>();
        /// <summary>
        /// 当前的线程对象
        /// </summary>
        private Thread thread;
        /// <summary>
        /// 是否已经在运行
        /// </summary>
        //private bool IsRun = false;
        #endregion 字段 End

        #region    构造与析构 Start
        public StateMachine(int runInterval = 50)
        {
            this.RunInterval = runInterval;
        }
        #endregion 构造与析构 End

        #region    公开方法 Start
        /// <summary>
        /// 注册一个状态对象
        /// </summary>
        /// <param name="stateObject"></param>
        /// <param name="istateObject"></param>
        public void Register(string stateObjectId, IStateObject stateObject)
        {
            DictState.Add(stateObjectId, stateObject);
        }

        /// <summary>
        /// 注册一个状态对象
        /// </summary>
        /// <param name="stateObject"></param>
        /// <param name="istateObject"></param>
        public void Register(Dictionary<string, IStateObject> stateObjects)
        {
            if (stateObjects?.Any() == true)
            {
                foreach (var item in stateObjects)
                {
                    DictState.Add(item.Key, item.Value);
                }
            }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start()
        {
            //if (!IsRun)
            if (CurrentState == RunningStatus.Stop)
            {
                //IsRun = true;
                thread = new Thread(new ThreadStart(Run));
                thread.IsBackground = true;
                thread.Start();
                Trace.WriteLine("状态机已启动");
            }
        }

        /// <summary>
        /// 暂停服务
        /// </summary>
        public void Pause()
        {
            if (CurrentState == RunningStatus.Running)
            {
                try
                {
                    SetState(RunningStatus.Pause);
                    thread.Suspend();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"暂停状态机失败：{ex.Message}");
                }
                Thread.Sleep(50);
                Trace.WriteLine("状态机已暂停");
            }
        }

        /// <summary>
        /// 继续服务
        /// </summary>
        public void Continue()
        {
            if (CurrentState == RunningStatus.Pause)
            {
                try
                {
                    SetState(RunningStatus.Running);
                    thread.Resume();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"继续状态机失败：{ex.Message}");
                }
                Thread.Sleep(50);
                Trace.WriteLine("状态机已继续运行");
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            //if (IsRun)
            if (CurrentState == RunningStatus.Running || CurrentState == RunningStatus.Pause)
            {
                //最后一个状态直接退出
                //if (CurrentState != null && DictState.TryGetValue(CurrentState, out var oldObj))
                //if (DictState.TryGetValue(CurrentState, out var oldObj))
                //{
                //    oldObj.ExitState(CurrentState);
                //}
                //IsRun = false;
                try
                {
                    SetState(RunningStatus.Stop);
                    thread.Interrupt();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"停止状态机失败：{ex.Message}");
                }
                Thread.Sleep(50);
                thread = null;
                Trace.WriteLine("状态机已停止");
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public void Updata()
        {
            //if (CurrentState != null && DictState.TryGetValue(CurrentState, out var obj))
            //if (DictState.TryGetValue(CurrentState, out var obj))
            //{
            //    obj.UpdateState(CurrentState);
            //}

            Parallel.ForEach(DictState.AsEnumerable(), kvp =>
            {
                kvp.Value.UpdateState(CurrentState);
            });
        }
        #endregion 公开方法 End

        #region    私有方法 Start
        /// <summary>
        /// 设置当前状态
        /// </summary>
        /// <param name="stateObject"></param>
        private void SetState(RunningStatus stateObject)
        {
            if (CurrentState != stateObject)
            {
                //if (CurrentState != null && DictState.TryGetValue(CurrentState, out var oldObj))
                //if (DictState.TryGetValue(CurrentState, out var oldObj))
                //{
                //    oldObj.ExitState(CurrentState);
                //}

                Parallel.ForEach(DictState.AsEnumerable(), kvp =>
                {
                    kvp.Value.ExitState(CurrentState);
                });
                //foreach (var item in DictState)
                //{
                //    item.Value.ExitState(CurrentState);
                //}
                CurrentState = stateObject;
                //if (CurrentState != null && DictState.TryGetValue(CurrentState, out var newObj))
                //if (DictState.TryGetValue(CurrentState, out var newObj))
                //{
                //    newObj.EnterState(CurrentState);
                //}

                Parallel.ForEach(DictState.AsEnumerable(), kvp =>
                {
                    kvp.Value.EnterState(CurrentState);
                });
                //foreach (var item in DictState)
                //{
                //    item.Value.EnterState(CurrentState);
                //}
            }
        }

        /// <summary>
        /// 线程执行的任务
        /// </summary>
        private void Run()
        {
            try
            {
                SetState(RunningStatus.Running);
                while (CurrentState == RunningStatus.Running)
                {
                    Updata();
                    //SpinWait.SpinUntil(() => !IsRun, RunInterval);
                    SpinWait.SpinUntil(() => CurrentState != RunningStatus.Running, RunInterval);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"停止状态机失败：{ex.Message}");
            }
        }
        #endregion 私有方法 End
    }
}