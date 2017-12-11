using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Net;
using System.Threading;

namespace Custard
{
    public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
            string cleanArgs = Arguments.dirtyArguments.Replace("rbxrevivaluri://", "").Replace("rbxrevivaluri", "").Replace("rbxrevivaluri", "").Replace(":", "").Replace("/", "").Replace("?", "");
            string[] splitArgs = cleanArgs.Split(';');
            Arguments.userId = splitArgs[0];
            Arguments.gameId = splitArgs[1];
            Arguments.authKey = splitArgs[2];
            // joinscript h4x
            var joinscriptWeb = new WebClient();
            joinscriptWeb.Headers.Add("user-agent", "RBXRevival/1.3");
            // get the client hash too
            var clientHashServant = new WebClient();
            clientHashServant.Headers.Add("user-agent", "45982984358698324%^&%$#@19345310~!@#$%^&^%$#@"); // the launcher has a weird user-agent
            richTextBox1.AppendText("Downloading Joinscript..." + Environment.NewLine);
            // downloading
            //chash ( do it first because js uses the chash!!!)
            try
            {
                Arguments.clientHash = clientHashServant.DownloadString(address: "http://rbxrevival.xyz/game/getCurrHash.php");
            }
            catch (Exception)
            {
                richTextBox1.AppendText("Could not download Client Hash!" + Environment.NewLine);
                return;
            }
            try
            {
                Arguments.joinScript = joinscriptWeb.DownloadString(address: "http://rbxrevival.xyz/game/join.php?gameid=" + Arguments.gameId + "&id=" + Arguments.userId + "&key=" + Arguments.authKey + "&lhash=" + Arguments.launcherHash + "&chash=" + Arguments.clientHash);
            }
            catch (Exception)
            {
                richTextBox1.AppendText("Could not download Joinscript!");
                return;
            }
            richTextBox1.AppendText("Done downloading Joinscript!" + Environment.NewLine); // switch to event later?
            string path = Assembly.GetExecutingAssembly().Location;
            string joinScriptPath = Path.Combine(path, @"..\joinscript.lua");
            if (!File.Exists(joinScriptPath))
            {
                File.Create(joinScriptPath).Dispose();
            }
            File.WriteAllText(joinScriptPath, Arguments.joinScript);
            // start
            startGame();
        }

        void Button1Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText("Closing all instances of RBXRevival..." + Environment.NewLine);
            Process[] clients = Process.GetProcessesByName("RBXRevival-Client");
            Process[] launchers = Process.GetProcessesByName("RBXRevival-Launcher");
            if (clients != null)
            {
                foreach (var process in clients)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch(Exception)
                    {
                        richTextBox1.AppendText("Could not kill the RBXRevival Processes!" + Environment.NewLine);
                    }
                }
            }
            else
            {
                richTextBox1.AppendText("No instances of RBXRevival found!" + Environment.NewLine);
            }
            if (launchers != null)
            {
                foreach (var process in clients)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch(Exception)
                    {
                        richTextBox1.AppendText("Could not kill the Custard Launchers!" + Environment.NewLine);
                    }
                }
            }
            else
            {
                richTextBox1.AppendText("No instances of Custard Launcher found!" + Environment.NewLine);
            }
            this.Close();
        }

        void Button2Click(object sender, EventArgs e)
        {
			using (var saveDialog = new SaveFileDialog())
        	{
            	saveDialog.Filter = "Lua source (*.lua)|*.lua";
            	saveDialog.FilterIndex = 2;
            	saveDialog.FileName = "joinscript.lua";
            	saveDialog.Title = "Save Joinscript";

            	if (saveDialog.ShowDialog() == DialogResult.OK)
            	{
            		File.WriteAllText(saveDialog.FileName, Arguments.joinScript);
                    richTextBox1.AppendText("Successfully saved Joinscript!" + Environment.NewLine);
            	}     
			}			
        }

        void killMe(object sender, EventArgs e)
        {
            this.Close();
        }

        void startGame()
		{
            richTextBox1.AppendText("Starting RBXRevival Game..." + Environment.NewLine);
            richTextBox1.AppendText("userId: " + Arguments.userId + Environment.NewLine);
            //richTextBox1.AppendText("victimId: " + Arguments.victimId + Environment.NewLine);
            richTextBox1.AppendText("gameId: " + Arguments.gameId + Environment.NewLine);
            richTextBox1.AppendText("authKey: " + Arguments.authKey + Environment.NewLine);
            richTextBox1.AppendText("clientHash (aka chash): " + Arguments.clientHash + Environment.NewLine);
            richTextBox1.AppendText("launcherHash (aka lhash): " + Arguments.launcherHash + Environment.NewLine);
            string client = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\RBXRevival-Client.exe";
            string args = "-script \"dofile(\'rbxasset://..//joinscript.lua')\"";
            try
            {
                Process clientProcess = new Process();
                clientProcess.StartInfo.FileName = client;
                clientProcess.StartInfo.Arguments = args;
                clientProcess.EnableRaisingEvents = true;
                clientProcess.Exited += new EventHandler(killMe);
                clientProcess.Start();
            }
            catch (Exception)
            {
                richTextBox1.AppendText("Could not launch client!" + Environment.NewLine);
                return;
            }
        }
	}
}
