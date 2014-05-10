using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DSoft.Messaging;
using MessageBoxAndroid.Events;

namespace MessageBoxAndroid
{
	[Activity (Label = "MessageBox - Sender")]			
	public class SecondActivity : Activity
	{
		public const String kEventID = "123456";

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Second);

			// Create your application here
			var btnSend = FindViewById<Button> (Resource.Id.btnSendMessage);
			var btnCustomPost = FindViewById<Button> (Resource.Id.btnCustomEvent);
			var edtMessage = FindViewById<EditText> (Resource.Id.edtMessage);

			btnSend.Click += (object sender, EventArgs e) => {


				var message = edtMessage.Text;

				//Creare a MessageBusEvent
				var aEvent = new CoreMessageBusEvent (kEventID) {
					Sender = this,
					Data = new object[]{ message },
				};

				//send it
				MessageBus.Default.PostEvent (aEvent);


			};

			btnCustomPost.Click += (object sender, EventArgs e) => {
				MessageBus.Default.PostEvent (new CustomMessageBusEvent ());
			};
		}
	}
}

