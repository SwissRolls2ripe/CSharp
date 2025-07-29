using ObserverPatternDemo.Interfaces;
using ObserverPatternDemo.Models;

namespace ObserverPatternDemo.Observers
{
    /// <summary>
    /// 活动跟踪器
    /// 职责：记录和分析用户行为数据
    /// </summary>
    public class ActivityTracker : IObserver
    {
        private readonly ISubject _subject;

        /// <summary>
        /// 观察者名称
        /// </summary>
        public string Name => "活动跟踪器";

        public ActivityTracker(ISubject subject)
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
            Console.WriteLine($"[{Name}] 开始跟踪用户 '{loginEvent.Username}' 的活动数据...");

            // 记录登录行为
            await Task.Delay(100);
            Console.WriteLine($"[{Name}] 登录行为已记录到数据仓库");

            // 更新用户活跃度
            await Task.Delay(100);
            Console.WriteLine($"[{Name}] 用户活跃度评分已更新");

            // 触发推荐算法
            await Task.Delay(100);
            Console.WriteLine($"[{Name}] 个性化推荐算法已触发");

            // 更新用户画像
            await Task.Delay(100);
            Console.WriteLine($"[{Name}] 用户画像数据已更新");

            Console.WriteLine($"[{Name}] 活动跟踪处理完成");
        }
    }
}
