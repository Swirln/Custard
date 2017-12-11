using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Net;
using System.Threading;

namespace Custard
{
    public partial class ImpersonatorForm : Form
	{
		public ImpersonatorForm()
		{
			InitializeComponent();
        }

        void NumericUpDown1TextChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Text != "")
            {
                Arguments.victimId = numericUpDown1.Text;
            }
        }
        void Button1Click(object sender, EventArgs e)
        {
            this.Hide();
            Form launcher = new MainForm();
            launcher.Show();
        }
	}
}
