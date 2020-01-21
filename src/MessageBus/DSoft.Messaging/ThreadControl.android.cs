using Android.OS;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.MessageBus
{
    public partial class ThreadControl
    {
        static volatile Handler handler;

        static bool PlatformIsMainThread
        {
            get
            {
                if (Platform.HasApiLevel(BuildVersionCodes.M))
                    return Looper.MainLooper.IsCurrentThread;

                return Looper.MyLooper() == Looper.MainLooper;
            }
        }

        static void PlatformBeginInvokeOnMainThread(Action action)
        {
            if (handler?.Looper != Looper.MainLooper)
                handler = new Handler(Looper.MainLooper);

            handler.Post(action);
        }
    }
}
