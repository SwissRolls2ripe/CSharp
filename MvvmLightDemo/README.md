# MvvmLightDemo 项目

## 项目概述
MvvmLightDemo 是一个基于 WPF 和 MVVM 架构的学生信息管理演示项目。项目实现了学生信息的添加、删除、展示等基本功能，旨在演示 MVVM 模式在实际桌面应用开发中的应用。项目结构清晰，便于理解 MVVM 的核心思想和数据绑定机制，适合学习和参考。

## 项目结构
- **MainWindow.xaml**：主窗口界面定义文件。
- **MainWindow.xaml.cs**：主窗口逻辑代码，负责初始化和数据上下文绑定。
- **App.xaml**：应用程序入口及全局资源定义。
- **App.xaml.cs**：应用程序启动逻辑。
- **ViewModels/StudentVM.cs**：学生信息的视图模型，包含数据集合和命令实现。
- **ViewModels/RelayCommand.cs**：命令实现类，用于绑定界面操作与逻辑处理。
- **Models/Student.cs**：学生数据模型，包含学生属性及通知机制。
- **Views/DisplayView.xaml**：学生信息展示视图界面定义。
- **Views/DisplayView.xaml.cs**：学生信息展示视图逻辑代码。
- **AssemblyInfo.cs**：程序集和主题资源配置信息。

## 运行说明
1. 安装 .NET 8 SDK 和 Visual Studio 2022，并确保已勾选 WPF 相关开发组件。
2. 克隆或下载本项目源码到本地目录。
4. 使用 Visual Studio 2022 打开 MvvmLightDemo.sln 解决方案文件。
5. 生成并启动项目，即可运行学生信息管理演示程序。

## 设计思路
项目采用 MVVM 架构进行模块划分，数据模型、视图模型和视图分离，提升了代码的可维护性和扩展性。通过 ObservableCollection 实现数据的动态绑定，利用 RelayCommand 实现界面与逻辑的解耦。各模块职责明确，便于后续功能扩展和单元测试。

## 贡献
欢迎对项目进行贡献！请确保遵循项目的编码规范，并在提交请求前进行充分的测试。