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
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DesignerIconAttribute : Attribute
    {
        public Type OwnType { get; set; }
        public string ImageName { get; set; }

        public DesignerIconAttribute(Type activityType, string imageName)
        {
            try
            {
                this.OwnType = activityType;
                this.ImageName = imageName;

                var assembly = activityType.Module.Assembly;
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

                    var property = typeof(WorkflowDesignerIcons).GetProperty("IconResourceDictionary",
                        BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Static);
                    if (property == null)
                    {
                        Debug.WriteLine("Error: Cannot find IconResourceDictionary property");
                        return;
                    }

                    var iconResource = property.GetValue(null) as ResourceDictionary;
                    string resourceKey = activityType.Name + "Icon";

                    if (iconResource != null && !iconResource.Contains(resourceKey))
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (!iconResource.Contains(resourceKey))
                            {
                                iconResource.Add(resourceKey, brush);
                            }
                        }));
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"异常详情: {ex}");
                Debug.WriteLine($"异常堆栈: {ex.StackTrace}");
            }
        }
    }
}