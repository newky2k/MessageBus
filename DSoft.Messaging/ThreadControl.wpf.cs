using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;

namespace DSoft.MessageBus
{
    public partial class ThreadControl
    {
        static bool PlatformIsMainThread 
        {
            get
            {

                return (Thread.CurrentThread == Application.Current.Dispatcher.Thread);

            }
        }

        static void PlatformBeginInvokeOnMainThread(Action action)
        {
            if (PlatformIsMainThread)
            { 
                action();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(action);
            }
        }

    }
}
