using System;
using System.Activities;
using System.Activities.Tracking;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EnhancedWorkflowActivities
{
    /// <summary>
    /// EnhancedSequence 是一个自定义的流程控制活动。
    /// 它按顺序执行其子活动集合，并提供了额外的控制功能，
    /// 例如通过书签暂停和恢复执行，以及跳过特定活动。
    /// </summary>
    public sealed class EnhancedSequence : NativeActivity
    {
        private int _currentActivityIndex = 0;

        /// <summary>
        /// 获取或设置要执行的子活动集合。
        /// 这是工作流设计器中可见的活动列表。
        /// </summary>
        [DefaultValue(null)]
        public Collection<Activity> Activities { get; set; }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示是否应跳过所有子活动的执行。
        /// 这是一个输入参数，可以在工作流实例运行时进行设置。
        /// </summary>
        [Category("Control")]
        [DisplayName("Skip All")]
        public InArgument<bool> SkipExecution { get; set; }

        /// <summary>
        /// 获取或设置用于控制工作流暂停和恢复的书签名称。
        /// 如果设置了此名称，工作流将在执行每个活动之前尝试创建书签。
        /// </summary>
        [Category("Control")]
        [DisplayName("Bookmark Name")]
        public InArgument<string> BookmarkName { get; set; }

        public EnhancedSequence()
        {
            Activities = new Collection<Activity>();
            SkipExecution = new InArgument<bool>(false);
        }

        /// <summary>
        /// 在活动执行期间缓存元数据。
        /// 此方法将子活动和内部变量注册到工作流运行时。
        /// </summary>
        /// <param name="metadata">工作流活动的元数据。</param>
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            // 首先调用基类的方法，这是一个良好的实践。
            base.CacheMetadata(metadata);

            // 将内部用于跟踪索引的变量注册到运行时
            metadata.SetChildrenCollection(this.Activities);
        }

        protected override bool CanInduceIdle
        {
            get { return true; }
        }

        /// <summary>
        /// 活动的核心执行逻辑。
        /// 此方法由工作流运行时调用。
        /// </summary>
        /// <param name="context">当前的执行上下文，提供了对运行时功能的访问。</param>
        protected override void Execute(NativeActivityContext context)
        {
            // 检查是否设置了跳过执行的标志
            bool skip = SkipExecution.Get(context);
            if (skip)
            {
                Console.WriteLine("Skipping execution of EnhancedSequence.");
                // 发送自定义跟踪记录
                var customRecord = new CustomTrackingRecord("EnhancedSequenceSkipped", System.Diagnostics.TraceLevel.Warning)
                {
                    Data = { { "Reason", "SkipExecution flag was set to true." } }
                };
                context.Track(customRecord);
                return;
            }

            ScheduleNextActivity(context);
        }

        /// <summary>
        /// 调度执行下一个子活动。
        /// </summary>
        /// <param name="context">当前的执行上下文。</param>
        private void ScheduleNextActivity(NativeActivityContext context)
        {
            if (_currentActivityIndex < Activities.Count)
            {
                Activity nextActivity = Activities[_currentActivityIndex];
                context.ScheduleActivity(nextActivity, new CompletionCallback(OnActivityCompleted));
            }
            else
            {
                // 所有活动都已完成
                Console.WriteLine("\nEnhancedSequence completed.");
            }
        }

        /// <summary>
        /// 当通过书签恢复工作流时调用的回调方法。
        /// </summary>
        /// <param name="context">当前的执行上下文。</param>
        /// <param name="bookmark">被恢复的书签对象。</param>
        /// <param name="value">外部传递给书签的值。</param>
        private void OnBookmarkResumed(NativeActivityContext context, Bookmark bookmark, object value)
        {
            // 发送跟踪记录，表明工作流已恢复
            context.Track(new CustomTrackingRecord("WorkflowResumed")
            {
                Data = { { "BookmarkName", bookmark.Name }, { "ResumedWithValue", value?.ToString() } }
            });

            Console.WriteLine($"Workflow resumed at bookmark '{bookmark.Name}' with value: {value}");

            // 移动到下一个活动的索引
            _currentActivityIndex++;

            // 调度下一个活动
            ScheduleNextActivity(context);
        }


        /// <summary>
        /// 当一个子活动执行完成时调用的回调方法。
        /// </summary>
        /// <param name="context">当前的执行上下文。</param>
        /// <param name="completedInstance">已完成的活动实例。</param>
        private void OnActivityCompleted(NativeActivityContext context, ActivityInstance completedInstance)
        {
            string bookmarkName = BookmarkName.Get(context);

            // 如果设置了书签名称，则在执行活动前创建书签并暂停
            if (!string.IsNullOrEmpty(bookmarkName))
            {
                // 创建一个书签，工作流将在此暂停
                // 书签的名称可以是动态的，例如 "Step_1", "Step_2"
                string dynamicBookmarkName = $"{bookmarkName}_{_currentActivityIndex}";
                context.CreateBookmark(dynamicBookmarkName, new BookmarkCallback(OnBookmarkResumed));
                //走到上面这句话，并没有立马停止，而是走完下面的跟踪，才回到 Host 执行 Idle

                // 发送跟踪记录，表明工作流已暂停
                context.Track(new CustomTrackingRecord("WorkflowPaused")
                {
                    Data = { { "BookmarkName", dynamicBookmarkName } }
                });
            }
            else
            {
                _currentActivityIndex++;
                ScheduleNextActivity(context);
            }
        }
    }
}