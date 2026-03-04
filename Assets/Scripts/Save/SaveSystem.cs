using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;

//Mark as not written while ABS, Part of 2nd grade of 2nd semester and highly "inpired" by the teachers given script
public static class SaveSystem
{
    private static readonly byte[] key = Encoding.UTF8.GetBytes("1234567890abcdef"); // 16 Bytes für AES-128
    private static readonly byte[] iv = Encoding.UTF8.GetBytes("abcdef1234567890");  // 16 Bytes IV

    //create the unreadable byte array
    public static byte[] Encrypt(string plainText)
    {
        //at this point copy paste
        if (string.IsNullOrEmpty(plainText))
            throw new ArgumentNullException(nameof(plainText));

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                using (StreamWriter writer = new StreamWriter(cryptoStream))
                {
                    writer.Write(plainText);
                }

                return memoryStream.ToArray();
            }
        }
    }
    //make the same steps to make the jason unreadable backwards to make it readable
    public static string Decrypt(byte[] cipherData)
    {
        //but still just copy paste
        if (cipherData == null || cipherData.Length == 0)
            throw new ArgumentNullException(nameof(cipherData));

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            using (MemoryStream memoryStream = new MemoryStream(cipherData))
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
            using (StreamReader reader = new StreamReader(cryptoStream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}