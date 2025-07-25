using System;
using System.Activities.Tracking;
using System.IO;
using System.Text;

namespace EnhancedWorkflowActivities
{
    internal class StatusTrackingParticipant : TrackingParticipant
    {
        /// <summary>
        /// 当工作流运行时发出跟踪记录时，此方法被调用。
        /// </summary>
        /// <param name="record">工作流运行时发出的跟踪记录。</param>
        /// <param name="timeout">操作的超时时间。</param>
        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            // 构造日志文件的路径，以工作流实例ID命名
            string logFileName = record.InstanceId.ToString() + ".txt";
            DateTime localEventTime = record.EventTime.ToLocalTime(); // 将 UTC 时间转换为本地时间

            // 尝试将记录转换为 CustomTrackingRecord
            if (record is CustomTrackingRecord customRecord)
            {
                // 如果是 CustomTrackingRecord，则记录其名称和数据
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"[Custom Record Received] Name: {customRecord.Name}");
                sb.AppendLine($"  Level: {customRecord.Level}, Timestamp: {localEventTime:yyyy-MM-dd HH:mm:ss.fff}");
                if (customRecord.Data.Count > 0)
                {
                    sb.AppendLine("  Data:");
                    foreach (var dataPair in customRecord.Data)
                    {
                        sb.AppendLine($"    - {dataPair.Key}: {dataPair.Value}");
                    }
                }
                sb.AppendLine(new string('-', 40));

                // 将格式化后的字符串追加到日志文件
                File.AppendAllText(logFileName, sb.ToString());
                return; // 处理完毕，直接返回
            }

            // 尝试将记录转换为 ActivityStateRecord (原始逻辑)
            if (record is ActivityStateRecord activityStateRecord)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"[Activity State Change] Name: {activityStateRecord.Activity.Name}");
                sb.AppendLine($"  State: {activityStateRecord.State}, Timestamp: {localEventTime:yyyy-MM-dd HH:mm:ss.fff}");

                // 特别处理 WriteLine 活动，记录其输出内容
                if (activityStateRecord.State == ActivityStates.Executing &&
                    activityStateRecord.Activity.TypeName == "System.Activities.Statements.WriteLine" &&
                    activityStateRecord.Arguments.ContainsKey("Text"))
                {
                    sb.AppendLine($"  WriteLine Output: \"{activityStateRecord.Arguments["Text"]}\"");
                }
                sb.AppendLine(new string('-', 40));

                // 将格式化后的字符串追加到日志文件
                File.AppendAllText(logFileName, sb.ToString());
            }
        }
    }
}
