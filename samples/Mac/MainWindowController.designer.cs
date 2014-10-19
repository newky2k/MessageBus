// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MessageBusMac
{
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		AppKit.NSTextField txtMessage { get; set; }

		[Outlet]
		AppKit.NSTextView txtOutput { get; set; }

		[Action ("didPostEvent:")]
		partial void didPostEvent (AppKit.NSButton sender);

		[Action ("didRegister:")]
		partial void didRegister (AppKit.NSButton sender);

		[Action ("didSendMessage:")]
		partial void didSendMessage (AppKit.NSButton sender);

		[Action ("didUnRegister:")]
		partial void didUnRegister (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (txtMessage != null) {
				txtMessage.Dispose ();
				txtMessage = null;
			}

			if (txtOutput != null) {
				txtOutput.Dispose ();
				txtOutput = null;
			}
		}
	}
}
