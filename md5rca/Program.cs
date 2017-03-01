using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Cryptography;

namespace md5rca
{
    class Program
    {
        static string getMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.            
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        static string getSHA1Hash(string input)
        {
            // Create a new instance of the SHA1CryptoServiceProvider object.            
            SHA1 sha1Hasher = SHA1.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = sha1Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }        

        static public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                //Create a new instance of RSACryptoServiceProvider.
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                //Import the RSA Key information. This only needs
                //toinclude the public key information.
                RSA.ImportParameters(RSAKeyInfo);

                //Encrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                return RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }

        }

        static public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                //Create a new instance of RSACryptoServiceProvider.
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                //Import the RSA Key information. This needs
                //to include the private key information.
                RSA.ImportParameters(RSAKeyInfo);

                //Decrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                return RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }

        }

        static void Main(string[] args)
        {
            string source = "Hello\n World!";

            string hash = getMd5Hash(source);

            Console.WriteLine("The MD5 hash of " + source + " is: " + hash + ".");
            Console.WriteLine();

            hash = getSHA1Hash(source);

            Console.WriteLine("The SH1 hash of " + source + " is: " + hash + ".");
            Console.WriteLine();


            //Create a UnicodeEncoder to convert between byte array and string.
			UnicodeEncoding ByteConverter = new UnicodeEncoding();

			//Create byte arrays to hold original, encrypted, and decrypted data.
            byte[] dataToEncrypt = ByteConverter.GetBytes(hash/*source*/);
			byte[] encryptedData;
			byte[] decryptedData;
			
			//Create a new instance of RSACryptoServiceProvider to generate
			//public and private key data.
			RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2048);

            //how to get the private key
            RSAParameters privKey = RSA.ExportParameters(true);

            //and the public key ...
            RSAParameters pubKey = RSA.ExportParameters(false);

            //converting the public key into a string representation
            string privKeyString;
            {
                //we need some buffer
                var sw = new System.IO.StringWriter();
                //we need a serializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //serialize the key into the stream
                xs.Serialize(sw, privKey);
                //get the string from the stream
                privKeyString = sw.ToString();
            }
            Console.WriteLine("private key: {0}", privKeyString);
            Console.WriteLine();

            //converting the public key into a string representation
            string pubKeyString;
            {
                //we need some buffer
                var sw = new System.IO.StringWriter();
                //we need a serializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //serialize the key into the stream
                xs.Serialize(sw, pubKey);
                //get the string from the stream
                pubKeyString = sw.ToString();
            }
            Console.WriteLine("public key: {0}", pubKeyString);
            Console.WriteLine();

            //converting it back
            {
                //get a stream from the string
                var sr = new System.IO.StringReader(pubKeyString);
                //we need a deserializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //get the object back from the stream
                pubKey = (RSAParameters)xs.Deserialize(sr);
            }            

			//Pass the data to ENCRYPT, the public key information 
			//(using RSACryptoServiceProvider.ExportParameters(false),
			//and a boolean flag specifying no OAEP padding.
			//encryptedData = RSAEncrypt(dataToEncrypt,RSA.ExportParameters(false), false);
            encryptedData = RSAEncrypt(dataToEncrypt, pubKey, false);

            //Display the Encrypted data to the console.
            Console.Write("Encrypted data byte value: ");
            /*foreach (byte b in encryptedData)
            {
                Console.Write(b + " ");
            }*/
            encryptedData.ToList().ForEach(n => Console.Write(n + " "));
            Console.WriteLine();

            //we might want a string representation of our cypher text... base64 will do
            string cypherText = Convert.ToBase64String(encryptedData);

            Console.WriteLine("Encrypted data in base64: {0}", cypherText);
            Console.WriteLine();

            //in base64url from base64 + become -, / become _ and = are suppressed
            Console.WriteLine("Encrypted data in base64url: {0}", cypherText.Replace('+', '-').Replace('/', '_').Replace("=", ""));
            Console.WriteLine();

			//Pass the data to DECRYPT, the private key information 
			//(using RSACryptoServiceProvider.ExportParameters(true),
			//and a boolean flag specifying no OAEP padding.
			//decryptedData = RSADecrypt(encryptedData,RSA.ExportParameters(true), false);
            decryptedData = RSADecrypt(encryptedData, privKey, false);

			//Display the decrypted plaintext to the console. 
			Console.WriteLine("Decrypted plaintext: {0}", ByteConverter.GetString(decryptedData));
        }
    }
}
