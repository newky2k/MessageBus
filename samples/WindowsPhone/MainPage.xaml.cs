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
    public partial class MainPage : PhoneApplicationPage
    {
        Action<object, MessageBusEvent> mThing;

        #region Fields

        private MessageBusEventHandler mEvHandler;

        #endregion

        /// <summary>
        /// Gets the message handler.
        /// </summary>
        /// <value>The message handler.</value>
        public MessageBusEventHandler MessageHandler
        {
            get
            {
                if (mEvHandler == null)
                {
                    mEvHandler = new MessageBusEventHandler()
                    {
                        EventId = SecondPage.kEventID,
                        //EventAction = MessageBusEventHandler, <-- Assign her on WP7 will not work
                    };

                    mEvHandler.EventAction = MessageBusEventHandler; //Assgin after the object has been created
                }

                return mEvHandler;
            }

        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            txtOutput.Text = String.Empty;

            btnRegister.Click += (object sender, RoutedEventArgs e) =>
                {
                    //register for an event
                    MessageBus.Default.Register(MessageHandler);

                    //register for CustomMessageBusEvent 
                    MessageBus.Default.Register<CustomMessageBusEvent>(CustomMessageEventHandler);
                };

            btnShow.Click += (object sender, RoutedEventArgs e) =>
            {
                NavigationService.Navigate(new Uri("/SecondPage.xaml",UriKind.Relative));
            };

            btnDeregister.Click += (object sender, RoutedEventArgs e) =>
            {
                //register for an event
                MessageBus.Default.DeRegister(MessageHandler);

                //
                MessageBus.Default.DeRegister<CustomMessageBusEvent>(CustomMessageEventHandler);
            };
        }

        /// <summary>
        /// Messages the bus event handler.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="evnt">Evnt.</param>
        public void MessageBusEventHandler(object sender, MessageBusEvent evnt)
        {
            //extrac the data
            var data2 = evnt.Data[0] as String;

            //post to the output box
            txtOutput.Text += data2 + Environment.NewLine;
        }

        /// <summary>
        /// Customs the message event handler.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="evnt">Evnt.</param>
        public void CustomMessageEventHandler(object sender, MessageBusEvent evnt)
        {
            if (evnt is CustomMessageBusEvent)
            {
                //convert to customer event type
                var custEvent = evnt as CustomMessageBusEvent;

                //post to the output box
                txtOutput.Text += String.Format("Custom Event Timestamp: {0}", custEvent.TimeStamp) + Environment.NewLine;
            }


        }
    }
}