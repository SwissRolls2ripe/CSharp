using Microsoft.VisualBasic.Activities;
using System;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.Presentation.Metadata;
using System.Activities.Statements;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows;

namespace WF_CustomIcon_Example
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private WorkflowDesigner designer;
        private Sequence sequence;

        public MainWindow()
        {
            InitializeComponent();
            //RegisterMetadata(); // 必须在前
            designer = new WorkflowDesigner();
            DesignerHost.Content = designer.View;
            LoadWorkflow();
        }

        private void LoadWorkflow()
        {
            sequence = new Sequence
            {
                Variables = 
                {
                    new Variable<string> { Name = "logMessage", Default = "123" },
                    new Variable<string> { Name = "logMessageOut" }
                },
                DisplayName = "My Test Workflow"
            };
            sequence.Activities.Add(new SimpleLog
            {
                Message = new InArgument<string>(sequence.Variables.Where(v => v.Name == "logMessage").FirstOrDefault()),
                MessageOut = new OutArgument<string>(sequence.Variables.Where(v => v.Name == "logMessageOut").FirstOrDefault())
            });
            sequence.Activities.Add(new WriteLine
            {
                Text = new InArgument<string>(sequence.Variables.Where(v => v.Name == "logMessageOut").FirstOrDefault())
            });
            sequence.Activities.Add(new WriteLine
            {
                Text = "Workflow finished."
            });

            designer.Load(sequence);  // 这里是唯一的 Load 调用
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            LogTextBox.Clear();
            // 运行当前的 sequence
            WorkflowApplication wfa = new WorkflowApplication(sequence);
            StringWriter logWriter = new StringWriter();
            wfa.Extensions.Add(logWriter);
            wfa.Completed = delegate (WorkflowApplicationCompletedEventArgs z)
            {
                var writers = z.GetInstanceExtensions<StringWriter>();
                foreach (var writer in writers)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                        new Action<string>(UpdateStatus),
                        System.Windows.Threading.DispatcherPriority.Background,
                        writer.ToString()
                    );
                }
            };
            
            wfa.Run();
        }

        public void UpdateStatus(string msg)
        {
            // We may be on a different thread so we need to
            // make this call using BeginInvoke.
            if (!msg.EndsWith("\r\n"))
            {
                msg += "\r\n";
            }
            LogTextBox.AppendText(msg);
        }

        private void RegisterMetadata()
        {
            var builder = new AttributeTableBuilder();
            builder.AddCustomAttributes(
                typeof(SimpleLog),
                new System.ComponentModel.DesignerAttribute(typeof(WF_CustomIcon_Example.SimpleLogDesigner))
            );
            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
