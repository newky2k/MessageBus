using ElmSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.MessageBus
{
    public partial class ThreadControl
    {
        static void PlatformBeginInvokeOnMainThread(Action action)
        {
            if (PlatformIsMainThread)
                action();
            else
                EcoreMainloop.PostAndWakeUp(action);
        }

        static bool PlatformIsMainThread
            => EcoreMainloop.IsMainThread;
    }
}
