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

		public MessageBusEventHandler MessageHandler {
			get
			{
				if (mEvHandler == null)
				{
					mEvHandler = new MessageBusEventHandler () {
						EventId = DetailViewController.kEventID,
						EventAction = (sender, data) => {
							var data2 = data [0] as String;

							txtOutput.Text += data2 + Environment.NewLine;
						},
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
				DSoft.Messaging.MessageBus.Register (MessageHandler);
			};

			// deregitset the event handler
			this.btnDeRegister.TouchUpInside += (object sender, EventArgs e) => {
				//register for an event
				DSoft.Messaging.MessageBus.DeRegister (MessageHandler);
			};

			//register for an event
			DSoft.Messaging.MessageBus.Register (MessageHandler);
		}
	}
}

