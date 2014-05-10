// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace MessageBus_iOS
{
	[Register ("MessageBus_iOSViewController")]
	partial class MainViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton btnDeRegister { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnRegister { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnShowChild { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextView txtOutput { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (btnShowChild != null)
			{
				btnShowChild.Dispose ();
				btnShowChild = null;
			}

			if (txtOutput != null)
			{
				txtOutput.Dispose ();
				txtOutput = null;
			}

			if (btnRegister != null)
			{
				btnRegister.Dispose ();
				btnRegister = null;
			}

			if (btnDeRegister != null)
			{
				btnDeRegister.Dispose ();
				btnDeRegister = null;
			}
		}
	}
}
