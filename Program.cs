using System;
using System.Windows.Forms;

namespace Custard
{
	internal sealed class Program
	{
		static string ProcessInput(string s)
    	{
       		return s;
    	}
		
		[STAThread]
		private static void Main(string[] args)
		{
			foreach (string s in args)
      		{
        		Arguments.dirtyArguments = ProcessInput(s);
      		}
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            Form startUp = new MainForm();
			Application.Run(startUp);
		}
	}
}
