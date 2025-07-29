using ObserverPatternDemo.Models;

namespace ObserverPatternDemo.Interfaces
{
    /// <summary>
    /// 被观察者接口，定义观察者管理和通知机制
    /// </summary>
    public interface ISubject
    {
        /// <summary>
        /// 订阅观察者
        /// </summary>
        /// <param name="observer">观察者实例</param>
        void Subscribe(IObserver observer);

        /// <summary>
        /// 取消订阅观察者
        /// </summary>
        /// <param name="observer">观察者实例</param>
        void Unsubscribe(IObserver observer);

        /// <summary>
        /// 异步通知所有观察者
        /// </summary>
        /// <param name="loginEvent">用户登录事件</param>
        /// <returns>异步任务</returns>
        Task NotifyObserversAsync(UserLoginEvent loginEvent);

        List<IObserver> Observers { get; }

        Task<bool> ProcessLoginAsync(LoginRequest request);
    }
}
