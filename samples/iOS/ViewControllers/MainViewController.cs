using System;
using System.Drawing;

#if __UNIFIED__
using UIKit;
using Foundation;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

using DSoft.Messaging;
using MessageBus_iOS.ViewControllers;

namespace MessageBus_iOS
{
	public partial class MainViewController : UIViewController
	{
		#region Fields

		private MessageBusEventHandler mEvHandler;

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
						EventId = DetailViewController.kEventID,
						EventAction = MessageBusEventHandler,
					};
				}

				return mEvHandler;
			}

		}

		#region Constructors

		public MainViewController () : base ("MessageBus_iOSViewController", null)
		{
			this.Title = "MessageBus Sample";

		}

		#endregion

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			txtOutput.Text = String.Empty;

			// register/re-register event handlers
			this.btnRegister.TouchUpInside += (object sender, EventArgs e) => {
				//register for an event
				MessageBus.Default.Register (MessageHandler);

				//register for CustomMessageBusEvent 
				MessageBus.Default.Register<CustomMessageBusEvent> (CustomMessageEventHandler);

			};

			//show the child page
			this.btnShowChild.TouchUpInside += (object sender, EventArgs e) => {

				this.NavigationController.PushViewController (new DetailViewController (), true);
			};


			// deregister the event handlers
			this.btnDeRegister.TouchUpInside += (object sender, EventArgs e) => {
				//register for an event
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

			//execute on the UI thread
			BeginInvokeOnMainThread (() => {
				//post to the output box
				txtOutput.Text += data2 + Environment.NewLine;
			});

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

				//execute on the UI thread
				BeginInvokeOnMainThread (() => {
					//post to the output box
					txtOutput.Text += String.Format ("Custom Event Timestamp: {0}", custEvent.TimeStamp) + Environment.NewLine;
				});

			}


		}
	}
}

