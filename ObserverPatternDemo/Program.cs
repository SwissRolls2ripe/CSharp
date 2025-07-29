using ObserverPatternDemo.Core;
using ObserverPatternDemo.Interfaces;
using ObserverPatternDemo.Models;
using ObserverPatternDemo.Observers;

namespace ObserverPatternDemo
{
    /// <summary>
    /// 用户登录状态变更系统 - 观察者模式演示程序
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// 程序入口点
        /// </summary>
        /// <param name="args">命令行参数</param>
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== 用户登录状态变更系统 - 观察者模式演示 ===");
            Console.WriteLine();

            try
            {
                // 创建用户登录管理器（被观察者）
                var loginManager = new UserLoginManager();

                // 注册所有观察者
                RegisterObserversAsync(loginManager);

                Console.WriteLine();
                Console.WriteLine("=== 开始模拟用户登录场景 ===");
                Console.WriteLine();

                // 模拟多个用户登录场景
                await SimulateUserLoginScenariosAsync(loginManager);

                Console.WriteLine();
                Console.WriteLine("=== 演示取消订阅功能 ===");
                Console.WriteLine();

                // 演示取消订阅
                DemonstrateUnsubscribe(loginManager);

                Console.WriteLine();
                Console.WriteLine("=== 演示完成 ===");
                Console.WriteLine();
                Console.WriteLine("按任意键退出...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"程序执行异常: {ex.Message}");
                Console.WriteLine($"堆栈跟踪: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 注册所有观察者到登录管理器
        /// </summary>
        /// <param name="loginManager">登录管理器</param>
        /// <returns>观察者列表</returns>
        private static void RegisterObserversAsync(ISubject loginManager)
        {
            Console.WriteLine("=== 注册观察者 ===");

            // 创建并注册各个观察者
            _ = new SecurityLogger(loginManager);
            _ = new OnlineStatusUpdater(loginManager);
            _ = new WelcomeMessageSender(loginManager);
            _ = new ActivityTracker(loginManager);
            _ = new SessionManager(loginManager);

            Console.WriteLine($"总共注册了 {loginManager.Observers.Count()} 个观察者");
        }

        /// <summary>
        /// 演示取消订阅功能
        /// </summary>
        /// <param name="observers">观察者列表</param>
        private static void DemonstrateUnsubscribe(ISubject loginManager)
        {
            Console.WriteLine("演示手动释放观察者资源...");

            var observers = loginManager.Observers;
            foreach (var observer in observers)
            {
                loginManager.Unsubscribe(observer);
            }

            Console.WriteLine($"当前观察者数量: {loginManager.Observers.Count}");
            Console.WriteLine("观察者资源已释放");
        }

        /// <summary>
        /// 模拟用户登录场景
        /// </summary>
        /// <param name="loginManager">登录管理器</param>
        /// <returns>异步任务</returns>
        private static async Task SimulateUserLoginScenariosAsync(ISubject loginManager)
        {
            // 场景1：正常内网用户登录
            await SimulateLoginScenario(loginManager, new LoginRequest
            {
                Username = "张三",
                Password = "password123",
                IpAddress = "192.168.1.100",
                DeviceInfo = "Windows Desktop Chrome/120.0"
            }, "场景1：正常内网用户登录");

            await Task.Delay(2000);
            Console.WriteLine();

            // 场景2：外网用户登录（可能触发安全告警）
            await SimulateLoginScenario(loginManager, new LoginRequest
            {
                Username = "李四",
                Password = "password456",
                IpAddress = "203.208.60.1",
                DeviceInfo = "iPhone Mobile Safari/17.0"
            }, "场景2：外网用户登录");

            await Task.Delay(2000);
            Console.WriteLine();

            // 场景3：移动端用户登录
            await SimulateLoginScenario(loginManager, new LoginRequest
            {
                Username = "王五",
                Password = "password789",
                IpAddress = "192.168.1.200",
                DeviceInfo = "Android Mobile Chrome/119.0"
            }, "场景3：移动端用户登录");
        }

        /// <summary>
        /// 模拟单个登录场景
        /// </summary>
        /// <param name="loginManager">登录管理器</param>
        /// <param name="request">登录请求</param>
        /// <param name="scenarioName">场景名称</param>
        /// <returns>异步任务</returns>
        private static async Task SimulateLoginScenario(ISubject loginManager, LoginRequest request, string scenarioName)
        {
            Console.WriteLine($"--- {scenarioName} ---");
            Console.WriteLine($"用户: {request.Username}, IP: {request.IpAddress}, 设备: {request.DeviceInfo}");
            Console.WriteLine();

            var startTime = DateTime.Now;

            // 处理登录请求
            var success = await loginManager.ProcessLoginAsync(request);

            var duration = DateTime.Now - startTime;

            Console.WriteLine();
            Console.WriteLine($"登录处理结果: {(success ? "成功" : "失败")}, 总耗时: {duration.TotalMilliseconds:F0}ms");
            Console.WriteLine($"--- {scenarioName} 完成 ---");
        }
    }
}
