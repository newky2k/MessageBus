using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using DSoft.Messaging;

namespace MessageBus_iOS.ViewControllers
{
	public partial class DetailViewController : UIViewController
	{
		public const String kEventID = "123456";

		public DetailViewController () : base ("DetailViewController", null)
		{
			this.Title = "MessageBus - Sender";
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			btnSend.TouchUpInside += (object sender, EventArgs e) => {


				var message = edtMessage.Text;

				//Creare a MessageBusEvent
				var aEvent = new MessageBusEvent () {
					EventId = kEventID,
					Sender = this,
					Data = new object[]{ message },
				};

				//send it
				DSoft.Messaging.MessageBus.PostEvent (aEvent);


			};
		}
	}
}

