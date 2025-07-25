using System;
using System.Activities;
using System.Activities.Statements;
using System.Threading;

namespace EnhancedWorkflowActivities
{
    class Program
    {
        static string BookmarkName = "UserApproval";
        static void Main(string[] args)
        {
            // 创建一个 AutoResetEvent 用于同步主线程和工作流
            AutoResetEvent syncEvent = new AutoResetEvent(false);
            AutoResetEvent idleEvent = new AutoResetEvent(false);

            // 创建工作流应用程序实例
            var wfApp = new WorkflowApplication(CreateWorkflow());

            wfApp.Extensions.Add(new StatusTrackingParticipant());

            // 设置工作流生命周期回调
            wfApp.Completed = (workflowApplicationCompletedEventArgs) =>
            {
                Console.WriteLine("\nWorkflow Completed.");
                syncEvent.Set();
            };

            wfApp.Aborted = (workflowApplicationAbortedEventArgs) =>
            {
                Console.WriteLine(workflowApplicationAbortedEventArgs.Reason.Message);
                syncEvent.Set();
            };

            wfApp.Idle = (workflowApplicationIdleEventArgs) =>
            {
                Console.WriteLine("The workflow is idle and waiting for user approval (Y/N).");
                idleEvent.Set();
                // 当工作流因等待书签而空闲时，此事件被触发
                // 在实际应用中，这里可以通知外部系统或用户进行操作
            };

            // 运行工作流
            wfApp.Run();

            int index = 0;
            string[] approvals = { "Approved by System", "Processing data Approval", "Final Approval" };

            WaitHandle[] handles = new WaitHandle[] { syncEvent, idleEvent };
            while (WaitHandle.WaitAny(handles) != 0)
            {
                // Gather the user input and resume the bookmark.
                bool validEntry = false;
                while (!validEntry)
                {
                    string UserApproval = Console.ReadLine().ToUpper();
                    if (UserApproval != "N" && UserApproval != "Y")
                    {
                        Console.WriteLine("Please enter Y or N (Ignore case).");
                    }
                    else
                    {
                        validEntry = true;
                        if (UserApproval == "Y")
                        {
                            var newBookmarkName = $"{BookmarkName}_{index}";
                            Console.WriteLine($"Resuming bookmark '{newBookmarkName}'...");
                            wfApp.ResumeBookmark(newBookmarkName, approvals[index]);
                        }
                        else
                        {
                            wfApp.Abort("\nWorkflow aborted by user.");
                        }
                        index++;
                    }
                }
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// 创建一个包含 EnhancedSequence 活动的工作流。
        /// </summary>
        /// <returns>一个 Activity 对象。</returns>
        static Activity CreateWorkflow()
        {
            return new EnhancedSequence
            {
                // 设置书签名称，这将使工作流在每一步都暂停
                BookmarkName = BookmarkName,
                Activities =
                {
                    new WriteLine { DisplayName = "Step 1 WriteLine", Text = "\nStep 1: Requesting user approval." },
                    new WriteLine { DisplayName = "Step 2 WriteLine", Text = "\nStep 2: Processing data after approval." },
                    new WriteLine { DisplayName = "Step 3 WriteLine", Text = "\nStep 3: Finalizing the process." }
                },
                SkipExecution = false // 设置为 false 以执行活动
            };
        }
    }
}