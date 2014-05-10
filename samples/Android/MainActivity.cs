using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using DSoft.Messaging;
using MessageBoxAndroid.Events;

namespace MessageBoxAndroid
{
	[Activity (Label = "MessageBox Sample", MainLauncher = true)]
	public class MainActivity : Activity
	{
		#region Fields

		private MessageBusEventHandler mEvHandler;
		private TextView txtOutput;

		#endregion

		/// <summary>
		/// Gets the message handler.
		/// </summary>
		/// <value>The message handler.</value>
		public MessageBusEventHandler MessageHandler {
			get
			{
				if (mEvHandler == null)
				{
					mEvHandler = new MessageBusEventHandler () {
						EventId = SecondActivity.kEventID,
						EventAction = MessageBusEventHandler,
					};
				}

				return mEvHandler;
			}

		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			var regbutton = FindViewById<Button> (Resource.Id.btnRegister);
			var dregbutton = FindViewById<Button> (Resource.Id.btnDeRegister);
			var spButton = FindViewById<Button> (Resource.Id.btnShowPoster);
			txtOutput = FindViewById<TextView> (Resource.Id.txtOutput);

			txtOutput.Text = String.Empty;

			// register the event handlers on click
			regbutton.Click += delegate {

				//register for an event
				MessageBus.Default.Register (MessageHandler);

				//register for CustomMessageBusEvent 
				MessageBus.Default.Register<CustomMessageBusEvent> (CustomMessageEventHandler);

			};

			//show the poster activity
			spButton.Click += delegate {

				StartActivity (typeof(SecondActivity));
			};

			// deregister the event handlers on click
			dregbutton.Click += delegate {
				//deregister for an event handler
				MessageBus.Default.DeRegister (MessageHandler);

				//
				MessageBus.Default.DeRegister<CustomMessageBusEvent> (CustomMessageEventHandler);
			};

		}

		/// <summary>
		/// Messages the bus event handler.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="evnt">Evnt.</param>
		public void MessageBusEventHandler (object sender, MessageBusEvent evnt)
		{
			//extrac the data
			var data2 = evnt.Data [0] as String;

			//post to the output box
			txtOutput.Text += data2 + System.Environment.NewLine;
		}

		/// <summary>
		/// Customs the message event handler.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="evnt">Evnt.</param>
		public void CustomMessageEventHandler (object sender, MessageBusEvent evnt)
		{
			if (evnt is CustomMessageBusEvent)
			{
				//convert to customer event type
				var custEvent = evnt as CustomMessageBusEvent;

				//post to the output box
				txtOutput.Text += String.Format ("Custom Event Timestamp: {0}", custEvent.TimeStamp) + System.Environment.NewLine;
			}


		}
	}
}


