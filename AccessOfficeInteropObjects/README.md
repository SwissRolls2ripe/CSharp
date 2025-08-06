# AccessOfficeInteropObjects 项目

## 项目概述
该项目是一个 C# 控制台应用程序，演示了如何使用 Office Interop 与 Microsoft Office 应用程序（特别是 Excel 和 Word）进行交互。程序首先创建一组银行账户数据，然后将这些数据显示在一个新的 Excel 电子表格中，并自动调整列宽和格式。最后，它会创建一个 Word 文档，并在其中插入一个链接到该 Excel 电子表格的图标。这个项目旨在展示 C# 在自动化 Office 任务方面的能力。

## 项目结构
```
AccessOfficeInteropObjects/
├── Properties/
│   └── AssemblyInfo.cs          # 程序集信息配置文件
├── App.config                     # 应用程序配置文件
└── Program.cs                     # 主程序文件
```

## 运行说明
1.  环境准备
    *   IDE: Visual Studio 2019 或更高版本
    *   框架: .NET Framework 4.8
    *   依赖: Microsoft Office (Excel 和 Word)
2.  运行项目
    *   使用 Visual Studio 打开 `AccessOfficeInteropObjects.sln` 文件。
    *   按 F5 或点击“启动”按钮运行项目。
    *   程序会自动打开 Excel 和 Word 应用程序来显示结果。

## 设计思路
1.  模块划分
    *   `Program` 类: 包含程序的主入口点和核心逻辑。
    *   `Account` 类: 定义了银行账户的数据结构。
    *   `DisplayInExcel` 方法: 负责与 Excel 交互，创建工作簿并填充数据。
    *   `CreateIconInWordDoc` 方法: 负责与 Word 交互，创建文档并插入链接图标。
2.  核心机制
    *   利用 `Microsoft.Office.Interop.Excel` 和 `Microsoft.Office.Interop.Word` 库来实例化和控制 Office 应用程序。
    *   通过编程方式操作 Excel 的 `Worksheet` 和 Word 的 `Document` 对象，实现数据展示和文档创建。
3.  技术实现
    *   使用 C# 的强类型特性来定义数据模型 (`Account` 类)。
    *   调用 Office Interop API 来实现自动化任务，例如创建工作簿、设置单元格值、格式化表格以及在 Word 中粘贴特殊对象。

## 贡献
欢迎对项目进行贡献！请确保遵循项目的编码规范，并在提交请求前进行充分的测试。
