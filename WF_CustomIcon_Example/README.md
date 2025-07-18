# WF自定义图标示例

## 项目概述
WF_CustomIcon_Example是一个基于Windows Workflow Foundation (WF)的自定义活动图标示例程序。项目展示了如何在WF工作流设计器中为自定义活动添加自定义图标，通过实现ActivityDesigner和使用资源文件，使工作流设计更加直观和易于理解。

## 项目结构
- **MainWindow.xaml/MainWindow.xaml.cs**：主窗口界面及其后台代码，包含工作流设计器的初始化、工作流的加载和执行逻辑。
- **SimpleLog.cs**：自定义的工作流活动类，实现了一个简单的日志记录功能。
- **SimpleLogDesigner.cs**：自定义活动的设计器类，负责加载和显示自定义图标。
- **App.xaml/App.xaml.cs**：应用程序入口文件。
- **Resources/log_icon.png**：自定义活动使用的图标资源文件。

## 运行说明
1. 确保您的开发环境满足以下要求：
   - Visual Studio 2019或更高版本
   - .NET Framework 4.7.2或更高版本
   - Windows Workflow Foundation开发组件

2. 打开项目：
   - 使用Visual Studio打开解决方案文件(WF_CustomIcon_Example.sln)
   - 确保所有NuGet包都已正确还原

3. 运行项目：
   - 按F5或点击"启动"按钮运行项目
   - 主窗口将显示包含自定义图标的工作流设计器界面
   - 点击"运行工作流"按钮可以执行工作流并在日志框中查看结果

## 设计思路
1. 模块划分：
   - 界面层：使用WPF实现用户界面，包含工作流设计器和日志显示区域
   - 活动层：实现自定义的SimpleLog活动，用于日志记录功能
   - 设计器层：通过SimpleLogDesigner类实现自定义图标的加载和显示

2. 核心机制：
   - 通过继承CodeActivity类实现自定义活动
   - 使用ActivityDesigner类实现自定义设计器
   - 利用资源文件机制加载自定义图标
   - 使用WorkflowApplication类执行工作流

3. 技术实现：
   - 采用WPF和Windows Workflow Foundation框架
   - 使用XAML定义用户界面
   - 实现异步日志更新机制
   - 通过资源流加载和处理图标文件

## 贡献
欢迎对项目进行贡献！请确保遵循项目的编码规范，并在提交请求前进行充分的测试。