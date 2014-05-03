using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
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

		public MainViewController () : base ("MessageBus_iOSViewController", null)
		{
			this.Title = "MessageBus Sample";

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			txtOutput.Text = String.Empty;

			//show the child page
			this.btnShowChild.TouchUpInside += (object sender, EventArgs e) => {

				this.NavigationController.PushViewController (new DetailViewController (), true);
			};

			// register/re-register event handler
			this.btnRegister.TouchUpInside += (object sender, EventArgs e) => {
				//register for an event
				MessageBus.Default.Register (MessageHandler);
			};

			// deregitset the event handler
			this.btnDeRegister.TouchUpInside += (object sender, EventArgs e) => {
				//register for an event
				MessageBus.Default.DeRegister (MessageHandler);
			};

			//register for an event
			MessageBus.Default.Register (MessageHandler);

			//register for CustomMessageBusEvent 
			MessageBus.Default.Register<CustomMessageBusEvent> (CustomMessageEventHandler);
		}

		/// <summary>
		/// Messages the bus event handler.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="evnt">Evnt.</param>
		public void MessageBusEventHandler(object sender, MessageBusEvent evnt)
		{
			var data2 = evnt.Data [0] as String;

			txtOutput.Text += data2 + Environment.NewLine;
		}

		/// <summary>
		/// Customs the message event handler.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="evnt">Evnt.</param>
		public void CustomMessageEventHandler(object sender, MessageBusEvent evnt)
		{
			var custEvent = evnt as CustomMessageBusEvent;

			Console.WriteLine (String.Format ("Time: {0}", custEvent.TimeStamp));

		}
	}
}

