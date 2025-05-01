using System;

namespace OopDemo
{
    // Cat类，继承自Animal抽象类，实现IMovable接口
    public class Cat : Animal, IMovable
    {
        private readonly ISoundService _soundService;

        // 通过构造函数注入ISoundService和名称
        public Cat(string name, string color, ISoundService soundService) : base(name, color)
        {
            _soundService = soundService;
        }

        // 实现抽象方法MakeSound
        public override void MakeSound()
        {
            _soundService.PlaySound($"{Name}说：喵喵!");
        }

        // 实现IMovable接口的Move方法
        public void Move()
        {
            Console.WriteLine("猫在优雅地走动...");
        }

        // 重写DisplayInfo方法，提供特定的实现
        public override void DisplayInfo()
        {
            Console.WriteLine($"{Name}这个动物很奇特！");
        }
    }
}