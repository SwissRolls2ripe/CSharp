using ObserverPatternDemo.Interfaces;
using ObserverPatternDemo.Models;

namespace ObserverPatternDemo.Observers
{
    /// <summary>
    /// 安全日志记录器
    /// 职责：记录用户登录的安全审计信息
    /// </summary>
    public class SecurityLogger : IObserver
    {
        private readonly ISubject _subject;

        /// <summary>
        /// 观察者名称
        /// </summary>
        public string Name => "安全日志记录器";

        /// <summary>
        /// 构造函数：创建观察者并自动订阅
        /// </summary>
        /// <param name="subject">被观察者</param>
        public SecurityLogger(ISubject subject)
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
            Console.WriteLine($"[{Name}] 开始处理用户 '{loginEvent.Username}' 的安全审计...");

            // 记录登录信息
            await Task.Delay(100);
            Console.WriteLine($"[{Name}] 记录登录信息 - 时间: {loginEvent.LoginTime:HH:mm:ss}, IP: {loginEvent.IpAddress}");

            // 检测异常登录行为
            if (!loginEvent.IpAddress.StartsWith("192.168."))
            {
                Console.WriteLine($"[{Name}] ⚠️ 检测到外网登录，已发送安全告警");
            }

            // 写入安全审计数据库
            await Task.Delay(100);
            Console.WriteLine($"[{Name}] 安全审计记录已保存");

            Console.WriteLine($"[{Name}] 安全审计处理完成");
        }
    }
}
