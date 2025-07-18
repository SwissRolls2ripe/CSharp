using System.Activities;
using System.ComponentModel;
using System;
using System.Windows;
using System.Diagnostics;
using System.Drawing;

namespace WF_CustomIcon_Example
{
    // 应用我们自定义的 DesignerIconAttribute 特性！
    // [DesignerIcon(typeof(SimpleLog), "log_icon.png")]
    [System.ComponentModel.Designer(typeof(WF_CustomIcon_Example.SimpleLogDesigner))]
    public sealed class SimpleLog : CodeActivity
    {

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Message { get; set; }

        [Category("Output")]
        [RequiredArgument]
        public OutArgument<string> MessageOut { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            string msg = context.GetValue(this.Message);
            MessageOut.Set(context, $"SimpleLog: {msg}");
        }
    }
}