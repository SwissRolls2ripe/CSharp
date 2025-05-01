using System;

namespace OopDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("C# OOP体现");
            Console.WriteLine("---------------");

            // 创建声音服务实例
            ISoundService basicService = new BasicSoundService();
            ISoundService advancedService = new AdvancedSoundService();

            // 创建动物实例并注入名称和不同的声音服务
            Dog dog = new Dog("旺财", "black", basicService);
            Cat cat = new Cat("咪咪", "white", advancedService);

            // 使用Animal类型的引用调用方法，展示多态性
            Console.WriteLine("\n使用Animal引用调用方法：");
            Animal animal1 = dog;
            animal1.Color = "brown"; // 修改颜色属性
            Console.WriteLine($"\n修改{animal1.Name}颜色属性为棕色：{animal1.Color}");
            Animal animal2 = cat;

            Console.WriteLine($"{animal1.Name}发出声音：");
            animal1.MakeSound(); // 调用Dog的MakeSound方法
            animal1.DisplayInfo(); // 调用Dog的DisplayInfo方法
            Console.WriteLine($"{animal2.Name}发出声音：");
            animal2.MakeSound(); // 调用Cat的MakeSound方法
            animal2.DisplayInfo(); // 调用Cat的DisplayInfo方法

            // 使用IMovable接口引用调用方法
            Console.WriteLine("\n使用IMovable引用调用方法：");
            IMovable movable1 = dog;
            IMovable movable2 = cat;

            movable1.Move(); // 调用Dog的Move方法
            movable2.Move(); // 调用Cat的Move方法

            // 调用Dog特有的重载方法
            Console.WriteLine("\n调用Dog的重载方法：");
            dog.MakeSound(3); // 调用重载的MakeSound方法，声音重复3次

            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
