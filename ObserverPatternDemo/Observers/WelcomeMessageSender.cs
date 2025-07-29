using ObserverPatternDemo.Interfaces;
using ObserverPatternDemo.Models;

namespace ObserverPatternDemo.Observers
{
    /// <summary>
    /// 欢迎消息发送器
    /// 职责：向用户推送个性化欢迎信息
    /// </summary>
    public class WelcomeMessageSender : IObserver
    {
        private readonly ISubject _subject;

        /// <summary>
        /// 观察者名称
        /// </summary>
        public string Name => "欢迎消息发送器";

        /// <summary>
        /// 构造函数：创建观察者并自动订阅（使用弱引用）
        /// </summary>
        /// <param name="subject">被观察者</param>
        public WelcomeMessageSender(ISubject subject)
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
            Console.WriteLine($"[{Name}] 开始为用户 '{loginEvent.Username}' 准备欢迎消息...");

            // 生成欢迎消息
            await Task.Delay(100);
            var greeting = GetTimeGreeting(loginEvent.LoginTime);
            Console.WriteLine($"[{Name}] 生成欢迎消息: \"{greeting}，{loginEvent.Username}！欢迎回来\"");

            // 推送系统公告
            await Task.Delay(100);
            Console.WriteLine($"[{Name}] 推送系统公告和未读消息");

            // 发送到客户端
            await Task.Delay(100);
            Console.WriteLine($"[{Name}] 消息已发送到客户端");

            Console.WriteLine($"[{Name}] 欢迎消息发送完成");
        }

        /// <summary>
        /// 获取时间问候语
        /// </summary>
        /// <param name="loginTime">登录时间</param>
        /// <returns>问候语</returns>
        private string GetTimeGreeting(DateTime loginTime)
        {
            var hour = loginTime.Hour;
            return hour switch
            {
                >= 5 and < 12 => "早上好",
                >= 12 and < 18 => "下午好",
                >= 18 and < 23 => "晚上好",
                _ => "夜深了"
            };
        }
    }
}
