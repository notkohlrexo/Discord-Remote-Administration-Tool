using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using Microsoft.Win32;

namespace Discord_New
{
    public class WindowsBackground
    {
        public enum WallpaperStyle
        {
            Tiled,
            Centered,
            Strectched,
            Fill
        }

        private const ushort SPI_SETDESKWALLPAPER = 20;
        private const ushort SPIF_UPDATEINIFILE = 0x01;
        private const ushort SPIF_SENDWININICHANGE = 0x02;

        private readonly List<string> backgroundUriCollection = new List<string>();

        private readonly int taskServiceDelay = 1000;

        private Thread backgroundTaskService;

        public WindowsBackground() : this(100, "http://sfwallpaper.com/images/smile-wallpaper-12.jpg")
        {
        }

        public WindowsBackground(int delay = 100, params string[] uriPath)
        {
            backgroundUriCollection.AddRange(uriPath);
            taskServiceDelay = delay;
        }

        public WindowsBackground(IEnumerable<string> uriPath, int delay = 100)
        {
            backgroundUriCollection.AddRange(uriPath);
            taskServiceDelay = delay;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uiParam, string pvParam, int fWinIni);

        public bool isRunningService()
        {
            if (backgroundTaskService == null)
                return false;
            return backgroundTaskService.IsAlive;
        }

        public void run()
        {
            var indexOfName = new List<string>();
            for (var i = 0; i < backgroundUriCollection.Count; i++) indexOfName.Add(Guid.NewGuid().ToString());
            while (true)
                foreach (var uri in backgroundUriCollection)
                {
                    var status = SetWallpaper(new Uri(uri), indexOfName[backgroundUriCollection.IndexOf(uri)]);
                    Thread.Sleep(taskServiceDelay);
                }
        }

        public bool startChangeWallpaper()
        {
            if (backgroundTaskService != null) return false;
            try
            {
                backgroundTaskService = new Thread(run);
                backgroundTaskService.Start();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool stopChangeWallpaper(bool needToTerminate = false)
        {
            try
            {
                if (needToTerminate)
                    backgroundTaskService.Abort();
                else
                    backgroundTaskService.Interrupt();
                return true;
            }
            catch (SecurityException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public int SetWallpaper(Uri uri, string name = "temp.png", WallpaperStyle style = WallpaperStyle.Strectched)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), $"{name}");
            if (!File.Exists(tempPath))
            {
                var stream = new WebClient().OpenRead(uri.ToString());
                var image = Image.FromStream(stream);
                image.Save(tempPath, ImageFormat.Png);
            }

            var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            switch (style)
            {
                case WallpaperStyle.Strectched:
                    key.SetValue(@"WallpaperStyle", 2.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                    break;

                case WallpaperStyle.Centered:
                    key.SetValue(@"WallpaperStyle", 1.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                    break;

                case WallpaperStyle.Tiled:
                    key.SetValue(@"WallpaperStyle", 1.ToString());
                    key.SetValue(@"TileWallpaper", 1.ToString());
                    break;

                case WallpaperStyle.Fill:
                    key.SetValue(@"WallpaperStyle", 10.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                    break;

                default:
                    key.SetValue(@"WallpaperStyle", 1.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                    break;
            }

            return SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, tempPath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}