namespace ObserverPatternDemo.Models
{
    /// <summary>
    /// 用户状态枚举
    /// </summary>
    public enum UserStatus
    {
        Offline,
        Online,
        Away,
        Busy
    }

    /// <summary>
    /// 用户登录事件数据模型
    /// </summary>
    public class UserLoginEvent
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; } = string.Empty;

        /// <summary>
        /// 设备信息
        /// </summary>
        public string DeviceInfo { get; set; } = string.Empty;

        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; } = string.Empty;

        /// <summary>
        /// 之前的状态
        /// </summary>
        public UserStatus PreviousStatus { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public UserStatus CurrentStatus { get; set; }
    }

    /// <summary>
    /// 登录请求模型
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; } = string.Empty;

        /// <summary>
        /// 设备信息
        /// </summary>
        public string DeviceInfo { get; set; } = string.Empty;
    }
}
