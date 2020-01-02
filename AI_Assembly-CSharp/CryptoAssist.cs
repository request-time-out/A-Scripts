// Decompiled with JetBrains decompiler
// Type: CryptoAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.IO;
using System.Text;

public class CryptoAssist
{
  private byte[] desKey;
  private byte[] desIV;

  public CryptoAssist()
  {
    this.desKey = Encoding.UTF8.GetBytes("1234567890abcdefghujklmn");
    this.desIV = Encoding.UTF8.GetBytes("12345678");
    if (!this.Load(UserData.Path + "crypt/data.dat"))
      ;
  }

  public byte[] Key
  {
    get
    {
      return this.desKey;
    }
  }

  public byte[] IV
  {
    get
    {
      return this.desIV;
    }
  }

  public bool Load(string fileName)
  {
    bool flag = false;
    if (File.Exists(fileName))
    {
      using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        byte[] numArray = new byte[0];
        byte[] buffer = new byte[fileStream.Length];
        fileStream.Read(buffer, 0, buffer.Length);
        int index1 = 0;
        int num1;
        for (num1 = 24; index1 < num1; ++index1)
          this.desKey[index1] += buffer[index1];
        int num2 = num1 + 8;
        int index2 = 0;
        while (index1 < num2)
        {
          this.desIV[index2] += buffer[index1];
          ++index1;
          ++index2;
        }
        flag = true;
      }
    }
    return flag;
  }
}
