using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DSoft.MessageBus
{
    /// <summary>
    /// ThreadControl.
    /// </summary>
    public partial class ThreadControl
    {
        /// <summary>
        /// Gets a value indicating whether this instance is main thread.
        /// </summary>
        /// <value><c>true</c> if this instance is main thread; otherwise, <c>false</c>.</value>
        public static bool IsMainThread => PlatformIsMainThread;

        /// <summary>
        /// Execute the action on the UI thread
        /// </summary>
        /// <param name="action">Action.</param>
        public static void RunOnMainThread(Action action)
		{
			if (IsMainThread)
			{
				action();
			}
			else
			{
				PlatformBeginInvokeOnMainThread(action);
			}
		}

        /// <summary>
        /// Runs the on main thread asynchronous.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>Task.</returns>
        public static Task RunOnMainThreadAsync(Action action)
		{
			if (IsMainThread)
			{
				action();

				return Task.CompletedTask;
			}

			var tcs = new TaskCompletionSource<bool>();

			RunOnMainThread(() =>
			{
				try
				{
					action();
					tcs.TrySetResult(true);
				}
				catch (Exception ex)
				{
					tcs.TrySetException(ex);
				}
			});

			return tcs.Task;
		}

        /// <summary>
        /// Get main task scheduler as an asynchronous operation.
        /// </summary>
        /// <returns>A Task&lt;TaskScheduler&gt; representing the asynchronous operation.</returns>
        public static async Task<TaskScheduler> GetMainTaskSchedulerAsync()
		{
			TaskScheduler ret = null;
			await RunOnMainThreadAsync(() =>
				ret = TaskScheduler.Current).ConfigureAwait(false);
			return ret;
		}
	}
}
