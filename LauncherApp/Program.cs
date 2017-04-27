// A Sample Application by Michael Kennedy
// http://www.michaelckennedy.net  | @mkennedy
// 
// This application is meant to run a very rudimentary comparison
// of performance between SQL Server and MongoDB in .NET
// See the full blog post here:
//
//    URL-TO-COME
//
using System;
using System.Windows.Forms;

namespace LauncherApp
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new Form1() );
		}
	}
}