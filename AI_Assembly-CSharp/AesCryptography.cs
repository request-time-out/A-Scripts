// Decompiled with JetBrains decompiler
// Type: AesCryptography
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.IO;
using System.Security.Cryptography;
using System.Text;

public class AesCryptography
{
  private const string AesInitVector = "1234567890abcdefghujklmnopqrstuv";
  private const string AesKey = "piyopiyopiyopiyopiyopiyopiyopiyo";
  private const int BlockSize = 256;
  private const int KeySize = 256;

  public byte[] Encrypt(byte[] binData)
  {
    RijndaelManaged rijndaelManaged = new RijndaelManaged();
    rijndaelManaged.Padding = PaddingMode.Zeros;
    rijndaelManaged.Mode = CipherMode.CBC;
    rijndaelManaged.KeySize = 256;
    rijndaelManaged.BlockSize = 256;
    byte[] bytes1 = Encoding.UTF8.GetBytes("piyopiyopiyopiyopiyopiyopiyopiyo");
    byte[] bytes2 = Encoding.UTF8.GetBytes("1234567890abcdefghujklmnopqrstuv");
    ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(bytes1, bytes2);
    MemoryStream memoryStream = new MemoryStream();
    using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
    {
      byte[] buffer = binData;
      cryptoStream.Write(buffer, 0, buffer.Length);
    }
    memoryStream.Close();
    return memoryStream.ToArray();
  }

  public byte[] Decrypt(byte[] binData)
  {
    RijndaelManaged rijndaelManaged = new RijndaelManaged();
    rijndaelManaged.Padding = PaddingMode.Zeros;
    rijndaelManaged.Mode = CipherMode.CBC;
    rijndaelManaged.KeySize = 256;
    rijndaelManaged.BlockSize = 256;
    byte[] bytes1 = Encoding.UTF8.GetBytes("piyopiyopiyopiyopiyopiyopiyopiyo");
    byte[] bytes2 = Encoding.UTF8.GetBytes("1234567890abcdefghujklmnopqrstuv");
    ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(bytes1, bytes2);
    byte[] buffer1 = binData;
    byte[] buffer2 = new byte[buffer1.Length];
    using (MemoryStream memoryStream = new MemoryStream(buffer1))
    {
      using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read))
        cryptoStream.Read(buffer2, 0, buffer2.Length);
    }
    return buffer2;
  }
}
