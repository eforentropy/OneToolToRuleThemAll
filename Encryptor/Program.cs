using System;
using System.IO;
using System.Text;

namespace Encryptor
{
    class Program
    {
        // XOR key (RuleThemAll'da aynı key kullanılacak)
        private static byte[] xorKey = new byte[] { 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48 };

        static void Main(string[] args)
        {
            Console.WriteLine("=== .NET Binary Encryptor ===");
            Console.WriteLine("Encrypts files with XOR and creates .bin files");
            Console.WriteLine();

            if (args.Length == 0)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  Encryptor.exe <file_path>");
                Console.WriteLine("  Encryptor.exe --param <string>");
                Console.WriteLine();
                Console.WriteLine("Examples:");
                Console.WriteLine("  Encryptor.exe target.exe");
                Console.WriteLine("  Encryptor.exe --param /nowrap /ptt");
                return;
            }

            // --param komutu kontrolü
            if (args[0] == "--param")
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("[ERROR] String required for --param command!");
                    Console.WriteLine("Example: Encryptor.exe --param /nowrap /ptt");
                    return;
                }

                // --param'den sonraki tüm argümanları birleştir
                string parameter = string.Join(" ", args, 1, args.Length - 1);
                Console.WriteLine(string.Format("[INFO] Encrypting parameter: {0}", parameter));
                
                // String'i byte array'e çevir
                byte[] stringBytes = Encoding.UTF8.GetBytes(parameter);
                
                // XOR ile encrypt et
                byte[] encryptedBytes = EncryptBytes(stringBytes);
                
                // Base64'e çevir
                string base64Result = Convert.ToBase64String(encryptedBytes);
                
                Console.WriteLine("[SUCCESS] Encrypted parameter:");
                Console.WriteLine(base64Result);
                return;
            }

            // Normal dosya encrypt işlemi
            string filePath = args[0];
            
            if (!File.Exists(filePath))
            {
                Console.WriteLine(string.Format("[ERROR] File not found: {0}", filePath));
                return;
            }

            try
            {
                Console.WriteLine(string.Format("[INFO] Encrypting file: {0}", filePath));
                
                // Dosyayı oku
                byte[] fileBytes = File.ReadAllBytes(filePath);
                Console.WriteLine(string.Format("[INFO] File size: {0} bytes", fileBytes.Length));
                
                // XOR ile encrypt et
                byte[] encryptedBytes = EncryptBytes(fileBytes);
                Console.WriteLine("[INFO] File encrypted successfully");
                
                // .bin dosyası oluştur
                string outputFile = Path.GetFileNameWithoutExtension(filePath) + ".bin";
                File.WriteAllBytes(outputFile, encryptedBytes);
                
                Console.WriteLine(string.Format("[SUCCESS] Encrypted file created: {0}", outputFile));
                Console.WriteLine();
                Console.WriteLine("You can add this .bin file as a resource in the RuleThemAll project.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("[ERROR] Error occurred: {0}", ex.Message));
            }
        }

        private static byte[] EncryptBytes(byte[] data)
        {
            byte[] encrypted = new byte[data.Length];
            
            for (int i = 0; i < data.Length; i++)
            {
                encrypted[i] = (byte)(data[i] ^ xorKey[i % xorKey.Length]);
            }
            
            return encrypted;
        }
    }
} 