using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DSoft.MessageBus
{
    public partial class ThreadControl
    {
		public static bool IsMainThread => PlatformIsMainThread;

		/// <summary>
		/// Execute the action on the UI thread
		/// </summary>
		/// <param name="Command">Command.</param>
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

		public static async Task<TaskScheduler> GetMainTaskSchedulerAsync()
		{
			TaskScheduler ret = null;
			await RunOnMainThreadAsync(() =>
				ret = TaskScheduler.Current).ConfigureAwait(false);
			return ret;
		}
	}
}
