using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.MessageBus
{
    public partial class ThreadControl
    {
        static void PlatformBeginInvokeOnMainThread(Action action) => throw new Exception("Not supportered on this platform");

        static bool PlatformIsMainThread => throw new Exception("Not supportered on this platform");
    }
}
