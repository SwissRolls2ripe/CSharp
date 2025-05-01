using System;

namespace OopDemo
{
    // Dog类，继承自Animal抽象类，实现IMovable接口
    public class Dog : Animal, IMovable
    {
        private readonly ISoundService _soundService;

        // 通过构造函数注入ISoundService和名称
        public Dog(string name, string color, ISoundService soundService) : base(name, color)
        {
            _soundService = soundService;
        }

        // 实现抽象方法MakeSound
        public override void MakeSound()
        {
            _soundService.PlaySound($"{Name}说：汪汪!");
        }

        // MakeSound方法的重载版本，允许指定声音重复的次数
        public void MakeSound(int times)
        {
            for (int i = 0; i < times; i++)
            {
                _soundService.PlaySound($"{Name}说：汪汪!");
            }
        }

        // 实现IMovable接口的Move方法
        public void Move()
        {
            Console.WriteLine("狗在奔跑...");
        }
    }
}