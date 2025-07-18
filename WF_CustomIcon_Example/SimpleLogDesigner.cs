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
            var imageName = "log_icon.png"; // ������ͼ����ļ���
            var assembly = typeof(SimpleLog).Module.Assembly;
            // �޸���Դ·�����췽ʽ
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
                brush.Freeze(); // �������

                this.Icon = brush;
            }
        }
    }
}