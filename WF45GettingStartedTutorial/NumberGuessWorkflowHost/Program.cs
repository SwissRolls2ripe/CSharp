using NumberGuessWorkflowActivities;
using System;
using System.Activities;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace NumberGuessWorkflowHost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Windows Forms 程序示例
            Application.EnableVisualStyles();
            Application.Run(new WorkflowHostForm());


            // 控制台程序示例
            //var inputs = new Dictionary<string, object>() { { "MaxNumber", 100 } };
            //var wf = new StateMachineNumberGuessWorkflow(); //FlowchartNumberGuessWorkflow、SequentialNumberGuessWorkflow StateMachineNumberGuessWorkflow

            //AutoResetEvent syncEvent = new AutoResetEvent(false);
            //AutoResetEvent idleEvent = new AutoResetEvent(false);

            //WorkflowApplication wfApp =
            //    new WorkflowApplication(wf, inputs)
            //    {
            //        Completed = delegate (WorkflowApplicationCompletedEventArgs e)
            //        {
            //            int Turns = Convert.ToInt32(e.Outputs["Turns"]);
            //            Console.WriteLine("Congratulations, you guessed the number in {0} turns.", Turns);
            //            Console.ReadLine();
            //            syncEvent.Set();
            //        },

            //        Aborted = delegate (WorkflowApplicationAbortedEventArgs e)
            //        {
            //            Console.WriteLine(e.Reason);
            //            syncEvent.Set();
            //        },

            //        OnUnhandledException = delegate (WorkflowApplicationUnhandledExceptionEventArgs e)
            //        {
            //            Console.WriteLine(e.UnhandledException.ToString());
            //            return UnhandledExceptionAction.Terminate;
            //        },

            //        Idle = delegate (WorkflowApplicationIdleEventArgs e)
            //        {
            //            idleEvent.Set();
            //        }
            //    };

            //wfApp.Run();

            //// Loop until the workflow completes.
            //WaitHandle[] handles = new WaitHandle[] { syncEvent, idleEvent };
            //while (WaitHandle.WaitAny(handles) != 0)
            //{
            //    // Gather the user input and resume the bookmark.
            //    bool validEntry = false;
            //    while (!validEntry)
            //    {
            //        if (!Int32.TryParse(Console.ReadLine(), out int Guess))
            //        {
            //            Console.WriteLine("Please enter an integer.");
            //        }
            //        else
            //        {
            //            validEntry = true;
            //            wfApp.ResumeBookmark("EnterGuess", Guess);
            //        }
            //    }
            //}
        }
    }
}
