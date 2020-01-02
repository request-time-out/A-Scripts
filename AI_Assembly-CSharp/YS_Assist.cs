// Decompiled with JetBrains decompiler
// Type: YS_Assist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class YS_Assist
{
  private static readonly string passwordChars36 = "0123456789abcdefghijklmnopqrstuvwxyz";
  private static readonly string passwordChars62 = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
  private static readonly char[] tbl36 = new char[36]
  {
    '0',
    '1',
    '2',
    '3',
    '4',
    '5',
    '6',
    '7',
    '8',
    '9',
    'a',
    'b',
    'c',
    'd',
    'e',
    'f',
    'g',
    'h',
    'i',
    'j',
    'k',
    'l',
    'm',
    'n',
    'o',
    'p',
    'q',
    'r',
    's',
    't',
    'u',
    'v',
    'w',
    'x',
    'y',
    'z'
  };
  private static readonly char[] tbl62 = new char[62]
  {
    '0',
    '1',
    '2',
    '3',
    '4',
    '5',
    '6',
    '7',
    '8',
    '9',
    'a',
    'b',
    'c',
    'd',
    'e',
    'f',
    'g',
    'h',
    'i',
    'j',
    'k',
    'l',
    'm',
    'n',
    'o',
    'p',
    'q',
    'r',
    's',
    't',
    'u',
    'v',
    'w',
    'x',
    'y',
    'z',
    'A',
    'B',
    'C',
    'D',
    'E',
    'F',
    'G',
    'H',
    'I',
    'J',
    'K',
    'L',
    'M',
    'N',
    'O',
    'P',
    'Q',
    'R',
    'S',
    'T',
    'U',
    'V',
    'W',
    'X',
    'Y',
    'Z'
  };
  private static readonly byte[] tblRevCode = new byte[128]
  {
    (byte) 50,
    (byte) 112,
    (byte) 114,
    (byte) 160,
    (byte) 140,
    (byte) 152,
    (byte) 202,
    (byte) 10,
    (byte) 235,
    (byte) 9,
    (byte) 198,
    (byte) 113,
    (byte) 78,
    (byte) 208,
    (byte) 182,
    (byte) 154,
    (byte) 247,
    (byte) 249,
    (byte) 64,
    (byte) 243,
    (byte) 232,
    (byte) 102,
    (byte) 184,
    (byte) 130,
    (byte) 196,
    (byte) 33,
    (byte) 149,
    (byte) 171,
    (byte) 62,
    (byte) 235,
    (byte) 124,
    (byte) 183,
    (byte) 193,
    (byte) 189,
    (byte) 168,
    (byte) 165,
    (byte) 243,
    (byte) 117,
    (byte) 48,
    (byte) 23,
    (byte) 16,
    (byte) 114,
    (byte) 192,
    (byte) 105,
    (byte) 122,
    (byte) 253,
    (byte) 206,
    (byte) 143,
    (byte) 240,
    (byte) 183,
    (byte) 150,
    (byte) 127,
    (byte) 115,
    (byte) 117,
    (byte) 19,
    (byte) 135,
    (byte) 140,
    (byte) 187,
    (byte) 73,
    (byte) 133,
    (byte) 254,
    (byte) 231,
    (byte) 48,
    (byte) 92,
    (byte) 205,
    (byte) 127,
    (byte) 122,
    (byte) 237,
    (byte) 250,
    (byte) 212,
    (byte) 27,
    (byte) 92,
    (byte) 153,
    (byte) 237,
    (byte) 54,
    (byte) 161,
    (byte) 135,
    (byte) 216,
    (byte) 104,
    (byte) 10,
    (byte) 60,
    (byte) 128,
    (byte) 97,
    (byte) 33,
    (byte) 47,
    (byte) 124,
    (byte) 18,
    (byte) 218,
    (byte) 168,
    (byte) 133,
    (byte) 217,
    (byte) 249,
    (byte) 188,
    (byte) 179,
    (byte) 198,
    (byte) 104,
    (byte) 68,
    (byte) 229,
    (byte) 179,
    (byte) 61,
    (byte) 10,
    (byte) 22,
    (byte) 10,
    (byte) 183,
    (byte) 8,
    (byte) 189,
    (byte) 74,
    (byte) 86,
    (byte) 107,
    (byte) 47,
    (byte) 230,
    (byte) 233,
    (byte) 158,
    (byte) 241,
    (byte) 27,
    (byte) 85,
    (byte) 198,
    (byte) 164,
    (byte) 151,
    (byte) 135,
    (byte) 148,
    (byte) 4,
    (byte) 24,
    (byte) 172,
    (byte) 122,
    (byte) 214,
    (byte) 18,
    (byte) 140
  };

  public static T DeepCopyWithSerializationSurrogate<T>(T target) where T : ISerializationSurrogate
  {
    using (MemoryStream memoryStream = new MemoryStream())
    {
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      SurrogateSelector surrogateSelector = new SurrogateSelector();
      StreamingContext context = new StreamingContext(StreamingContextStates.All);
      surrogateSelector.AddSurrogate(typeof (T), context, (ISerializationSurrogate) target);
      binaryFormatter.SurrogateSelector = (ISurrogateSelector) surrogateSelector;
      try
      {
        binaryFormatter.Serialize((Stream) memoryStream, (object) target);
        memoryStream.Position = 0L;
        return (T) binaryFormatter.Deserialize((Stream) memoryStream);
      }
      finally
      {
        memoryStream.Close();
      }
    }
  }

  public static bool CheckFlagsList(List<bool> lstFlags)
  {
    int count = lstFlags.Count;
    for (int index = 0; index < count; ++index)
    {
      if (!lstFlags[index])
        return false;
    }
    return true;
  }

  public static bool SetActiveControl(GameObject obj, List<bool> lstFlags)
  {
    if (Object.op_Equality((Object) null, (Object) obj))
      return false;
    int count = lstFlags.Count;
    bool active = true;
    for (int index = 0; index < count; ++index)
    {
      if (!lstFlags[index])
      {
        active = false;
        break;
      }
    }
    return obj.SetActiveIfDifferent(active);
  }

  public static bool SetActiveControl(GameObject obj, bool flag)
  {
    return !Object.op_Equality((Object) null, (Object) obj) && obj.SetActiveIfDifferent(flag);
  }

  public static void GetListString(string text, out string[,] data)
  {
    string[] strArray1 = text.Split('\n');
    int length1 = strArray1.Length;
    if (length1 != 0 && strArray1[length1 - 1].Trim() == string.Empty)
      --length1;
    string[] strArray2 = strArray1[0].Split('\t');
    int length2 = strArray2.Length;
    if (length2 != 0 && strArray2[length2 - 1].Trim() == string.Empty)
      --length2;
    data = new string[length1, length2];
    for (int index1 = 0; index1 < length1; ++index1)
    {
      string[] strArray3 = strArray1[index1].Split('\t');
      for (int index2 = 0; index2 < strArray3.Length && index2 < length2; ++index2)
        data[index1, index2] = strArray3[index2];
      data[index1, strArray3.Length - 1] = data[index1, strArray3.Length - 1].Replace("\r", string.Empty).Replace("\n", string.Empty);
    }
  }

  public static void BitRevBytes(byte[] data, int startPos)
  {
    int index1 = startPos % YS_Assist.tblRevCode.Length;
    for (int index2 = 0; index2 < data.Length; ++index2)
    {
      data[index2] ^= YS_Assist.tblRevCode[index1];
      index1 = (index1 + 1) % YS_Assist.tblRevCode.Length;
    }
  }

  public static string Convert62StringFromInt32(int num)
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    if (num < 0)
    {
      num *= -1;
      stringBuilder1.Append("0");
    }
    for (; num >= 62; num /= 62)
      stringBuilder1.Append(YS_Assist.tbl62[num % 62]);
    stringBuilder1.Append(YS_Assist.tbl62[num]);
    StringBuilder stringBuilder2 = new StringBuilder();
    for (int index = stringBuilder1.Length - 1; index >= 0; --index)
      stringBuilder2.Append(stringBuilder1[index]);
    return stringBuilder2.ToString();
  }

  public static string Convert36StringFromInt32(int num)
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    if (num < 0)
    {
      num *= -1;
      stringBuilder1.Append("0");
    }
    for (; num >= 36; num /= 36)
      stringBuilder1.Append(YS_Assist.tbl36[num % 36]);
    stringBuilder1.Append(YS_Assist.tbl36[num]);
    StringBuilder stringBuilder2 = new StringBuilder();
    for (int index = stringBuilder1.Length - 1; index >= 0; --index)
      stringBuilder2.Append(stringBuilder1[index]);
    return stringBuilder2.ToString();
  }

  public static string GenerateRandomNumber(int length)
  {
    StringBuilder stringBuilder = new StringBuilder(length);
    byte[] data = new byte[length];
    new RNGCryptoServiceProvider().GetBytes(data);
    for (int index = 0; index < length; ++index)
    {
      int num = (int) data[index] % 10;
      stringBuilder.Append(num);
    }
    return stringBuilder.ToString();
  }

  public static string GeneratePassword36(int length)
  {
    StringBuilder stringBuilder = new StringBuilder(length);
    byte[] data = new byte[length];
    new RNGCryptoServiceProvider().GetBytes(data);
    for (int index1 = 0; index1 < length; ++index1)
    {
      int index2 = (int) data[index1] % YS_Assist.passwordChars36.Length;
      char ch = YS_Assist.passwordChars36[index2];
      stringBuilder.Append(ch);
    }
    return stringBuilder.ToString();
  }

  public static string GeneratePassword62(int length)
  {
    StringBuilder stringBuilder = new StringBuilder(length);
    byte[] data = new byte[length];
    new RNGCryptoServiceProvider().GetBytes(data);
    for (int index1 = 0; index1 < length; ++index1)
    {
      int index2 = (int) data[index1] % YS_Assist.passwordChars62.Length;
      char ch = YS_Assist.passwordChars62[index2];
      stringBuilder.Append(ch);
    }
    return stringBuilder.ToString();
  }

  public static byte[] CreateSha256(string planeStr, string key)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(planeStr);
    return new HMACSHA256(Encoding.UTF8.GetBytes(key)).ComputeHash(bytes);
  }

  public static byte[] CreateSha256(byte[] data, string key)
  {
    return new HMACSHA256(Encoding.UTF8.GetBytes(key)).ComputeHash(data);
  }

  public static string ConvertStrX2FromBytes(byte[] data)
  {
    StringBuilder stringBuilder = new StringBuilder(256);
    foreach (byte num in data)
      stringBuilder.Append(num.ToString("x2"));
    return stringBuilder.ToString();
  }

  public static string GetMacAddress()
  {
    string empty = string.Empty;
    NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
    if (networkInterfaces != null)
    {
      foreach (NetworkInterface networkInterface in networkInterfaces)
      {
        PhysicalAddress physicalAddress = networkInterface.GetPhysicalAddress();
        byte[] numArray = (byte[]) null;
        if (physicalAddress != null)
          numArray = physicalAddress.GetAddressBytes();
        if (numArray != null && numArray.Length == 6)
        {
          empty += physicalAddress.ToString();
          Debug.Log((object) physicalAddress.ToString());
          break;
        }
      }
    }
    return empty;
  }

  public static string CreateIrregularString(string str, bool bitRand = false)
  {
    if (str.IsNullOrEmpty())
      return str;
    byte[] bytes = Encoding.UTF8.GetBytes(str);
    int startPos = 7;
    if (bitRand)
      startPos = Random.Range(0, YS_Assist.tblRevCode.Length - 1);
    YS_Assist.BitRevBytes(bytes, startPos);
    int num1 = bytes.Length / 4;
    int num2 = bytes.Length % 4;
    if (num2 != 0)
    {
      int length1 = 4 - num2;
      byte[] numArray = new byte[length1];
      int length2 = bytes.Length;
      Array.Resize<byte>(ref bytes, length2 + length1);
      Array.Copy((Array) numArray, 0, (Array) bytes, length2, length1);
      ++num1;
    }
    StringBuilder stringBuilder = new StringBuilder(str.Length);
    for (int index = 0; index < num1; ++index)
      stringBuilder.Append(YS_Assist.Convert62StringFromInt32(BitConverter.ToInt32(bytes, index * 4)));
    return stringBuilder.ToString();
  }

  public static string CreateIrregularStringFromMacAddress()
  {
    StringBuilder stringBuilder = new StringBuilder(16);
    stringBuilder.Append(YS_Assist.GetMacAddress());
    return string.Empty == stringBuilder.ToString() ? string.Empty : YS_Assist.CreateIrregularString(stringBuilder.ToString(), false);
  }

  public static string CreateUUID()
  {
    return Guid.NewGuid().ToString();
  }

  public static string GetStringRight(string str, int len)
  {
    if (str == null)
      return string.Empty;
    return str.Length <= len ? str : str.Substring(str.Length - len, len);
  }

  public static string GetRemoveStringRight(string str, int len)
  {
    return str == null || str.Length <= len ? string.Empty : str.Substring(0, str.Length - len);
  }

  public static string GetRemoveStringLeft(string str, string find, bool removeFind = true)
  {
    if (str == null)
      return string.Empty;
    int num = str.IndexOf(find);
    if (0 >= num)
      return str;
    int startIndex = num + (!removeFind ? 0 : find.Length);
    return str.Substring(startIndex);
  }

  public static string GetRemoveStringRight(string str, string find, bool removeFind = false)
  {
    if (str == null)
      return string.Empty;
    int num = str.LastIndexOf(find);
    if (0 >= num)
      return str;
    int length = num + (!removeFind ? find.Length : 0);
    return str.Substring(0, length);
  }

  public static byte[] EncryptAES(byte[] srcData, string pw = "illusion", string salt = "unityunity")
  {
    RijndaelManaged rijndaelManaged = new RijndaelManaged();
    rijndaelManaged.KeySize = 128;
    rijndaelManaged.BlockSize = 128;
    byte[] bytes = Encoding.UTF8.GetBytes(salt);
    Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(pw, bytes);
    rfc2898DeriveBytes.IterationCount = 1000;
    rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
    rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
    ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor();
    byte[] numArray = encryptor.TransformFinalBlock(srcData, 0, srcData.Length);
    encryptor.Dispose();
    return numArray;
  }

  public static byte[] DecryptAES(byte[] srcData, string pw = "illusion", string salt = "unityunity")
  {
    RijndaelManaged rijndaelManaged = new RijndaelManaged();
    rijndaelManaged.KeySize = 128;
    rijndaelManaged.BlockSize = 128;
    byte[] bytes = Encoding.UTF8.GetBytes(salt);
    Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(pw, bytes);
    rfc2898DeriveBytes.IterationCount = 1000;
    rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
    rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
    ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor();
    byte[] numArray = decryptor.TransformFinalBlock(srcData, 0, srcData.Length);
    decryptor.Dispose();
    return numArray;
  }

  public static string GetRegistryInfoFrom(string keyName, string valueName, string baseKey = "HKEY_CURRENT_USER")
  {
    string empty = string.Empty;
    string str;
    try
    {
      RegistryKey registryKey;
      if (baseKey == "HKEY_CURRENT_USER")
      {
        registryKey = Registry.CurrentUser.OpenSubKey(keyName);
      }
      else
      {
        if (!(baseKey == "HKEY_LOCAL_MACHINE"))
          return (string) null;
        registryKey = Registry.LocalMachine.OpenSubKey(keyName);
      }
      if (registryKey == null)
        return (string) null;
      str = (string) registryKey.GetValue(valueName);
      registryKey.Close();
    }
    catch (Exception ex)
    {
      throw;
    }
    return str;
  }

  public static bool IsRegistryKey(string keyName, string baseKey = "HKEY_CURRENT_USER")
  {
    try
    {
      RegistryKey registryKey;
      if (baseKey == "HKEY_CURRENT_USER")
      {
        registryKey = Registry.CurrentUser.OpenSubKey(keyName);
      }
      else
      {
        if (!(baseKey == "HKEY_LOCAL_MACHINE"))
          return false;
        registryKey = Registry.LocalMachine.OpenSubKey(keyName);
      }
      if (registryKey == null)
        return false;
      registryKey.Close();
    }
    catch (Exception ex)
    {
      throw;
    }
    return true;
  }

  public static bool CompareFile(string file1, string file2)
  {
    if (file1 == file2)
      return true;
    FileStream fileStream1;
    try
    {
      fileStream1 = new FileStream(file1, FileMode.Open);
    }
    catch (FileNotFoundException ex)
    {
      Debug.LogError((object) (file1 + " がない"));
      return false;
    }
    FileStream fileStream2;
    try
    {
      fileStream2 = new FileStream(file2, FileMode.Open);
    }
    catch (FileNotFoundException ex)
    {
      fileStream1.Close();
      Debug.LogError((object) (file2 + " がない"));
      return false;
    }
    if (fileStream1.Length != fileStream2.Length)
    {
      fileStream1.Close();
      fileStream2.Close();
      return false;
    }
    int num1;
    int num2;
    do
    {
      num1 = fileStream1.ReadByte();
      num2 = fileStream2.ReadByte();
    }
    while (num1 == num2 && num1 != -1);
    fileStream1.Close();
    fileStream2.Close();
    return num1 - num2 == 0;
  }
}
