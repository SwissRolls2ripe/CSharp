using ObserverPatternDemo.Interfaces;
using ObserverPatternDemo.Models;

namespace ObserverPatternDemo.Observers
{
    /// <summary>
    /// 会话管理器
    /// 职责：管理用户会话生命周期
    /// </summary>
    public class SessionManager : IObserver
    {
        private readonly ISubject _subject;
        /// <summary>
        /// 观察者名称
        /// </summary>
        public string Name => "会话管理器";

        public SessionManager(ISubject subject)
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
            Console.WriteLine($"[{Name}] 开始管理用户 '{loginEvent.Username}' 的会话...");

            // 创建新会话
            await Task.Delay(100);
            Console.WriteLine($"[{Name}] 新会话已创建，过期时间: {loginEvent.LoginTime.AddHours(8):HH:mm:ss}");

            // 设置会话过期时间
            await Task.Delay(100);
            Console.WriteLine($"[{Name}] 会话过期时间已设置为8小时");

            // 清理过期会话
            await Task.Delay(100);
            Console.WriteLine($"[{Name}] 已清理过期会话");

            // 同步到集群
            await Task.Delay(100);
            Console.WriteLine($"[{Name}] 会话信息已同步到集群节点");

            Console.WriteLine($"[{Name}] 会话管理处理完成");
        }
    }
}
