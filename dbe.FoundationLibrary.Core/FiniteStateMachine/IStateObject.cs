using dbe.FoundationLibrary.Core.Common;

namespace dbe.FoundationLibrary.Core.FiniteStateMachine
{

    /// <summary>
    /// 状态对象
    /// </summary>
    public interface IStateObject
    {
        /// <summary>
        /// 进入xxx状态
        /// </summary>
        void EnterState(RunningStatus state);
        /// <summary>
        /// 离开xxx状态
        /// </summary>
        void ExitState(RunningStatus state);
        /// <summary>
        /// 更新状态
        /// </summary>
        void UpdateState(RunningStatus state);
    }
}