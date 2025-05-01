using System;

namespace OopDemo
{
    // 抽象类Animal，定义动物的基本行为
    public abstract class Animal
    {
        // 动物名称属性
        public string Name { get; set; }
        public string Color { get; set; }

        // 构造函数，接收名称参数
        protected Animal(string name, string color)
        {
            Name = name;
            Color = color;
        }

        // 抽象方法，用于定义动物发声的行为
        public abstract void MakeSound();

        // 虚方法，用于显示动物的信息
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"{Name}这个动物并不特殊！");
        }
    }
}