using ObserverPatternDemo.Interfaces;
using ObserverPatternDemo.Models;

namespace ObserverPatternDemo.Observers
{
    /// <summary>
    /// 在线状态更新器
    /// 职责：更新用户在系统中的在线状态显示
    /// </summary>
    public class OnlineStatusUpdater : IObserver
    {
        private readonly ISubject _subject;

        /// <summary>
        /// 观察者名称
        /// </summary>
        public string Name => "在线状态更新器";

        /// <summary>
        /// 构造函数：创建观察者并自动订阅
        /// </summary>
        /// <param name="subject">被观察者</param>
        public OnlineStatusUpdater(ISubject subject)
        {
            _subject = subject ?? throw new ArgumentNullException(nameof(subject));
            _subject.Subscribe(this);
        }

        /// <summary>
        /// 异步处理用户登录事件
        /// </summary>
        /// <param name="loginEvent">用户登录事件</param>
        /// <returns>异步任务</returns>
        public async Task UpdateAsync(UserLoginEvent loginEvent)
        {
            Console.WriteLine($"[{Name}] 开始更新用户 '{loginEvent.Username}' 的在线状态...");

            // 更新好友列表状态
            await Task.Delay(100);
            Console.WriteLine($"[{Name}] 更新好友列表状态: {loginEvent.PreviousStatus} → {loginEvent.CurrentStatus}");

            // 同步状态到缓存
            await Task.Delay(100);
            Console.WriteLine($"[{Name}] 状态已同步到Redis缓存");

            // 通知好友
            await Task.Delay(100);
            Console.WriteLine($"[{Name}] 已通知好友用户状态变更");

            Console.WriteLine($"[{Name}] 在线状态更新完成");
        }
    }
}
