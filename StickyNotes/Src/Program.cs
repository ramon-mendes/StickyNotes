using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SciterSharp;
using SciterSharp.Interop;

namespace StickyNotes
{
	class Program
	{
		public static Window WndGlobal;
		public static Hooker HookerInstance = new Hooker();

		[STAThread]
		static void Main(string[] args)
		{
			// Sciter needs this for drag'n'drop support; STAThread is required for OleInitialize succeess
			int oleres = PInvokeWindows.OleInitialize(IntPtr.Zero);
			Debug.Assert(oleres == 0);
			
			// Create the window
			var wnd = WndGlobal = new Window();
			wnd.CreateMainWindow(500, 320, SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_POPUP | SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ALPHA);
			wnd.CenterTopLevelWindow();
			wnd.HideTaskbarIcon();
			wnd.Title = "Sciter-based desktop TemplateDesktopGadgets";
			wnd.Icon = Properties.Resources.IconMain;
			//wnd.ExtendNCArea();

			// Prepares SciterHost and then load the page
			var host = new Host();
			host.Setup(wnd);
			host.AttachEvh(new HostEvh());
			host.SetupPage("index.html");

			HookerInstance.SetMessageHook();

			// Show window and Run message loop
			wnd.Show();
			PInvokeUtils.RunMsgLoop();

			HookerInstance.ClearHook();
		}

		/*public static void RunHooker()
		{
			string hookexe = Environment.Is64BitOperatingSystem ? @"\Hook\64\Hooker.exe" : @"\Hook\32\Hooker.exe";
			hookexe = AppDomain.CurrentDomain.BaseDirectory + hookexe;
			Debug.Assert(System.IO.File.Exists(hookexe));

			var p = Process.Start(new ProcessStartInfo()
			{
				FileName = hookexe,
				WindowStyle = ProcessWindowStyle.Hidden
			});

			Debug.Assert(p.HasExited==false);
		}*/
	}
}