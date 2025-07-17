# WF45GettingStartedTutorial 项目

## 项目概述

WF45GettingStartedTutorial是一个基于Windows Workflow Foundation (WF4.5)技术实现的猜数字游戏项目。项目展示了WF4.5工作流的动态更新功能，通过三种不同类型的工作流（顺序工作流、状态机工作流和流程图工作流）实现了相同的猜数字游戏逻辑。项目支持工作流实例的持久化存储，并且能够在工作流运行时进行动态更新，展示了WF4.5在业务流程管理方面的强大功能。

## 项目结构

项目按模块划分如下：

### NumberGuessWorkflowActivities
- **FlowchartNumberGuessWorkflow.xaml**：使用流程图方式实现的猜数字游戏工作流。
- **SequentialNumberGuessWorkflow.xaml**：使用顺序工作流方式实现的猜数字游戏工作流。
- **StateMachineNumberGuessWorkflow.xaml**：使用状态机方式实现的猜数字游戏工作流。
- **Prompt.xaml**：用户输入提示活动。
- **ReadInt.cs**：读取整数输入的自定义活动。

### NumberGuessWorkflowHost
- **WorkflowHostForm.cs**：主窗体实现，包含工作流实例管理和用户界面。
- **StatusTrackingParticipant.cs**：工作流跟踪参与者，用于记录工作流执行状态。
- **WorkflowVersionMap.cs**：工作流版本映射管理。

### CreateUpdateMaps
- **Program.cs**：生成工作流更新映射文件的主程序。

### ApplyDynamicUpdate
- **Program.cs**：应用工作流动态更新的主程序。
- **DynamicUpdateInfo.cs**：动态更新信息类。

### DbConn
- **DBConn.cs**：数据库连接管理类。
- **Db.config**：数据库连接配置文件。

## 运行说明

1. 环境准备
   - 安装 .NET Framework 4.8
   - 安装 SQL Server 2012或更高版本
   - 安装 Visual Studio 2019或更高版本

2. 数据库配置
   - 在SQL Server中创建名为`WF45GettingStartedTutorial`的数据库，数据库配置参考：https://learn.microsoft.com/zh-cn/dotnet/framework/windows-workflow-foundation/how-to-create-and-run-a-long-running-workflow
   - 修改`DbConn/Db.config`中的数据库连接字符串

3. 项目运行
   - 使用Visual Studio打开解决方案文件`WF45GettingStartedTutorial.sln`
   - 编译整个解决方案
   - 运行`NumberGuessWorkflowHost`项目启动游戏
   - 可以选择不同类型的工作流（顺序、状态机、流程图）进行游戏

4. 动态更新示例
   - 运行`CreateUpdateMaps`项目生成更新映射文件
   - 运行`ApplyDynamicUpdate`项目将更新应用到已持久化的工作流实例

## 设计思路

1. 模块划分
   - 工作流定义模块：包含游戏逻辑的不同实现方式
   - 工作流宿主模块：负责工作流的执行和管理
   - 动态更新模块：处理工作流的版本更新
   - 数据访问模块：管理数据库连接

2. 核心机制
   - 使用XAML定义工作流，便于可视化设计和修改
   - 采用SQL Server持久化存储工作流实例状态
   - 实现工作流跟踪机制，监控执行过程
   - 支持工作流动态更新，实现版本升级

3. 技术实现策略
   - 使用Windows Forms创建用户界面
   - 通过配置文件管理数据库连接
   - 使用工作流持久化实现游戏状态保存
   - 实现三种不同类型的工作流以展示WF4.5的灵活性

## 贡献

欢迎对项目进行贡献！请确保遵循项目的编码规范，并在提交请求前进行充分的测试。