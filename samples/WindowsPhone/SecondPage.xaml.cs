using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using DSoft.Messaging;
using MessageBusWP.Events;

namespace MessageBusWP
{
    public partial class SecondPage : PhoneApplicationPage
    {
        public const String kEventID = "123456";

        public SecondPage()
        {
            InitializeComponent();

            btnPostMessage.Click += (object sender, RoutedEventArgs e) =>
            {
                var message = edtMessage.Text;

                //Creare a MessageBusEvent
                var aEvent = new CoreMessageBusEvent(kEventID)
                {
                    Sender = this,
                    Data = new object[] { message },
                };

                //send it
                MessageBus.Default.Post(aEvent);
            };

            btnPostCustom.Click += (object sender, RoutedEventArgs e) =>
            {
                MessageBus.Default.Post(new CustomMessageBusEvent());
            };


        }
    }
}