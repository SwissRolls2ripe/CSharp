using ObserverPatternDemo.Interfaces;
using ObserverPatternDemo.Models;
using System.Collections.Concurrent;

namespace ObserverPatternDemo.Core
{
    /// <summary>
    /// 用户登录管理器（被观察者）
    /// 职责：管理用户登录状态，维护观察者列表，状态变更时异步通知所有观察者
    /// </summary>
    public class UserLoginManager : ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly Dictionary<string, UserStatus> _userStatuses;
        private readonly object _lockObject = new object();

        public List<IObserver> Observers
        {
            get
            {
                lock (_lockObject)
                {
                    return new List<IObserver>(_observers);
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserLoginManager()
        {
            _observers = new List<IObserver>();
            _userStatuses = new Dictionary<string, UserStatus>();
        }

        /// <summary>
        /// 订阅观察者
        /// </summary>
        /// <param name="observer">观察者实例</param>
        public void Subscribe(IObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            lock (_lockObject)
            {
                if (!_observers.Contains(observer))
                {
                    _observers.Add(observer);
                    Console.WriteLine($"[UserLoginManager] 观察者 '{observer.Name}' 已订阅");
                }
            }
        }

        /// <summary>
        /// 取消订阅观察者
        /// </summary>
        /// <param name="observer">观察者实例</param>
        public void Unsubscribe(IObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            lock (_lockObject)
            {
                if (_observers.Remove(observer))
                {
                    Console.WriteLine($"[UserLoginManager] 观察者 '{observer.Name}' 已取消订阅");
                }
            }
        }

        /// <summary>
        /// 异步通知所有观察者
        /// </summary>
        /// <param name="loginEvent">用户登录事件</param>
        /// <returns>异步任务</returns>
        public async Task NotifyObserversAsync(UserLoginEvent loginEvent)
        {
            if (loginEvent == null)
                throw new ArgumentNullException(nameof(loginEvent));

            List<IObserver> observersCopy;
            lock (_lockObject)
            {
                observersCopy = new List<IObserver>(_observers);
            }

            Console.WriteLine($"[UserLoginManager] 开始通知 {observersCopy.Count} 个观察者...");

            var tasks = new List<Task>();

            foreach (var observer in observersCopy)
            {
                // 为每个观察者创建独立的任务，实现异常隔离
                tasks.Add(NotifyObserverSafelyAsync(observer, loginEvent));
            }

            // 并行执行所有观察者的处理逻辑
            await Task.WhenAll(tasks);
            
            Console.WriteLine($"[UserLoginManager] 所有观察者通知完成");
        }

        /// <summary>
        /// 安全地通知单个观察者（异常隔离）
        /// </summary>
        /// <param name="observer">观察者</param>
        /// <param name="loginEvent">登录事件</param>
        /// <returns>异步任务</returns>
        private async Task NotifyObserverSafelyAsync(IObserver observer, UserLoginEvent loginEvent)
        {
            try
            {
                var startTime = DateTime.Now;
                await observer.UpdateAsync(loginEvent);
                var duration = DateTime.Now - startTime;
                Console.WriteLine($"[UserLoginManager] 观察者 '{observer.Name}' 处理完成，耗时: {duration.TotalMilliseconds}ms");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserLoginManager] 观察者 '{observer.Name}' 处理异常: {ex.Message}");
                // 记录错误日志，但不影响其他观察者的执行
            }
        }

        /// <summary>
        /// 处理用户登录请求
        /// </summary>
        /// <param name="request">登录请求</param>
        /// <returns>异步任务</returns>
        public async Task<bool> ProcessLoginAsync(LoginRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            Console.WriteLine($"[UserLoginManager] 开始处理用户 '{request.Username}' 的登录请求...");

            // 模拟登录验证逻辑
            await Task.Delay(100); // 模拟数据库查询延迟

            // 获取用户之前的状态
            UserStatus previousStatus;
            lock (_lockObject)
            {
                _userStatuses.TryGetValue(request.Username, out previousStatus);
                _userStatuses[request.Username] = UserStatus.Online;
            }

            // 创建登录事件
            var loginEvent = new UserLoginEvent
            {
                UserId = Guid.NewGuid().ToString(),
                Username = request.Username,
                LoginTime = DateTime.Now,
                IpAddress = request.IpAddress,
                DeviceInfo = request.DeviceInfo,
                SessionId = Guid.NewGuid().ToString(),
                PreviousStatus = previousStatus,
                CurrentStatus = UserStatus.Online
            };

            Console.WriteLine($"[UserLoginManager] 用户 '{request.Username}' 登录成功，状态从 '{previousStatus}' 变更为 '{UserStatus.Online}'");

            // 异步通知所有观察者
            await NotifyObserversAsync(loginEvent);

            return true;
        }
    }
}
