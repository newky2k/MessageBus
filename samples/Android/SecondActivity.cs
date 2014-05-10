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

namespace MessageBoxAndroid
{
	[Activity (Label = "MessageBox - Sender")]			
	public class SecondActivity : Activity
	{
		public const String kEventID = "123456";

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
		}
	}
}

