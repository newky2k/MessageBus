using Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.MessageBus
{
    public partial class ThreadControl
    {
        static bool PlatformIsMainThread => NSThread.Current.IsMainThread;

        static void PlatformBeginInvokeOnMainThread(Action action)
        {
            NSRunLoop.Main.BeginInvokeOnMainThread(action.Invoke);
        }

    }
}
