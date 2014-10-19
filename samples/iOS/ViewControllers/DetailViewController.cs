using System;
using System.Drawing;
using DSoft.Messaging;

#if __UNIFIED__
using Foundation;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

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
				var aEvent = new CoreMessageBusEvent (kEventID) {
					Sender = this,
					Data = new object[]{ message },
				};

				//send it
				MessageBus.Default.Post (aEvent);


			};

			btnCustomPost.TouchUpInside += (object sender, EventArgs e) => {
				MessageBus.Default.Post (new CustomMessageBusEvent ());
			};
		}
	}
}

