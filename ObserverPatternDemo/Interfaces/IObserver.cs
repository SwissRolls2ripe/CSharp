using ObserverPatternDemo.Models;

namespace ObserverPatternDemo.Interfaces
{
    /// <summary>
    /// 观察者接口，定义状态变更时的响应行为
    /// </summary>
    public interface IObserver
    {
        /// <summary>
        /// 异步更新方法，当被观察者状态发生变化时调用
        /// </summary>
        /// <param name="loginEvent">用户登录事件</param>
        /// <returns>异步任务</returns>
        Task UpdateAsync(UserLoginEvent loginEvent);

        /// <summary>
        /// 观察者名称
        /// </summary>
        string Name { get; }
    }
}
