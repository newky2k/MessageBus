// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace MessageBus_iOS.ViewControllers
{
	[Register ("DetailViewController")]
	partial class DetailViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton btnCustomPost { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnSend { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField edtMessage { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnSend != null) {
				btnSend.Dispose ();
				btnSend = null;
			}

			if (btnCustomPost != null) {
				btnCustomPost.Dispose ();
				btnCustomPost = null;
			}

			if (edtMessage != null) {
				edtMessage.Dispose ();
				edtMessage = null;
			}
		}
	}
}
