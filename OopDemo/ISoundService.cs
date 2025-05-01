using System;

namespace OopDemo
{
    // 声音服务接口
    public interface ISoundService
    {
        // 播放声音的方法
        void PlaySound(string sound);
    }

    // 基础声音服务实现
    public class BasicSoundService : ISoundService
    {
        public void PlaySound(string sound)
        {
            Console.WriteLine($"基础声音服务播放: {sound}");
        }
    }

    // 高级声音服务实现
    public class AdvancedSoundService : ISoundService
    {
        public void PlaySound(string sound)
        {
            Console.WriteLine($"高级声音服务播放: {sound} (高品质)");
        }
    }
}