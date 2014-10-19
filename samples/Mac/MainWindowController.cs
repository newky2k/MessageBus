using System;

using Foundation;
using AppKit;
using DSoft.Messaging;

namespace MessageBusMac
{
	public partial class MainWindowController : NSWindowController
	{
		#region Fields
		public const String kEventID = "123456";
		private MessageBusEventHandler mEvHandler;

		#endregion
		#region Properties
		public new MainWindow Window
		{
			get { return (MainWindow)base.Window; }
		}

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
						EventId = MainWindowController.kEventID,
						EventAction = MessageBusEventHandler,
					};
				}

				return mEvHandler;
			}

		}
		#endregion

		#region Constructors
		public MainWindowController(IntPtr handle) : base(handle)
		{
		}

		[Export("initWithCoder:")]
		public MainWindowController(NSCoder coder) : base(coder)
		{
		}

		public MainWindowController() : base("MainWindow")
		{
		}

		#endregion

		#region Overrides
		public override void AwakeFromNib()
		{
			base.AwakeFromNib();
		}
		#endregion

		#region Event Handlers

		partial void didPostEvent(AppKit.NSButton sender)
		{
			MessageBus.Default.Post (new CustomMessageBusEvent ());
		}

		partial void didRegister(AppKit.NSButton sender)
		{
			//register for an event
			MessageBus.Default.Register (MessageHandler);

			//register for CustomMessageBusEvent 
			MessageBus.Default.Register<CustomMessageBusEvent> (CustomMessageEventHandler);
		}

		partial void didSendMessage(AppKit.NSButton sender)
		{
			var message = txtMessage.StringValue;

			//Creare a MessageBusEvent
			var aEvent = new CoreMessageBusEvent (kEventID) {
				Sender = this,
				Data = new object[]{ message },
			};

			//send it
			MessageBus.Default.Post (aEvent);
		}

		partial void didUnRegister(AppKit.NSButton sender)
		{
			//register for an event
			MessageBus.Default.DeRegister (MessageHandler);

			//
			MessageBus.Default.DeRegister<CustomMessageBusEvent> (CustomMessageEventHandler);
		}

		#endregion

		#region MessageBus event handlers
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
				var aString = txtOutput.TextStorage.Value;

				aString  += data2 + Environment.NewLine;

				txtOutput.TextStorage.SetString(new NSAttributedString(aString));
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
				BeginInvokeOnMainThread (() => {
				//convert to customer event type
				var custEvent = evnt as CustomMessageBusEvent;

				var aString = txtOutput.TextStorage.Value;

				aString +=  String.Format ("Custom Event Timestamp: {0}", custEvent.TimeStamp) + Environment.NewLine;

				txtOutput.TextStorage.SetString(new NSAttributedString(aString));

				});
			}


		}

		#endregion
	}
}
