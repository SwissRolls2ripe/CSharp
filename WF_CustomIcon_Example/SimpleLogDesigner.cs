using System;
using System.Activities.Presentation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WF_CustomIcon_Example
{
    public class SimpleLogDesigner : ActivityDesigner
    {
        public SimpleLogDesigner()
        {
            var imageName = "log_icon.png"; // 这里是图标的文件名
            var assembly = typeof(SimpleLog).Module.Assembly;
            // 修改资源路径构造方式
            var resourcePath = assembly.GetManifestResourceNames()
                .FirstOrDefault(name => name.EndsWith(imageName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(resourcePath))
            {
                Debug.WriteLine($"Error: Resource ending with '{imageName}' not found.");
                return;
            }

            using (Stream manifestResourceStream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (manifestResourceStream == null)
                {
                    Debug.WriteLine($"Error: Cannot open stream for resource '{resourcePath}'");
                    return;
                }

                var bmpframe = BitmapFrame.Create(manifestResourceStream);

                var brush = new DrawingBrush
                {
                    Drawing = new ImageDrawing
                    {
                        Rect = new System.Windows.Rect(0, 0, 16, 16),
                        ImageSource = bmpframe
                    }
                };
                brush.Freeze(); // 提高性能

                this.Icon = brush;
            }
        }
    }
}