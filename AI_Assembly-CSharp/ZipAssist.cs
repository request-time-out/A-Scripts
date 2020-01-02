// Decompiled with JetBrains decompiler
// Type: ZipAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Ionic.Zip;
using Ionic.Zlib;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class ZipAssist
{
  public ZipAssist()
  {
    this.progress = 0.0f;
    this.cntSaved = 0;
    this.cntTotal = 0;
  }

  public float progress { get; private set; }

  public int cntSaved { get; private set; }

  public int cntTotal { get; private set; }

  public void SaveProgress(object sender, SaveProgressEventArgs e)
  {
  }

  public byte[] SaveUnzipFile(byte[] srcBytes, EventHandler<SaveProgressEventArgs> callBack = null)
  {
    byte[] numArray = (byte[]) null;
    try
    {
      using (MemoryStream memoryStream1 = new MemoryStream(srcBytes))
      {
        ReadOptions readOptions1 = new ReadOptions();
        readOptions1.set_Encoding(Encoding.GetEncoding("shift_jis"));
        ReadOptions readOptions2 = readOptions1;
        using (ZipFile zipFile = ZipFile.Read((Stream) memoryStream1, readOptions2))
        {
          using (MemoryStream memoryStream2 = new MemoryStream())
          {
            zipFile.get_Item(0).Extract((Stream) memoryStream2);
            numArray = memoryStream2.ToArray();
          }
        }
      }
    }
    catch (Exception ex)
    {
      Debug.LogErrorFormat("ZipError: {0}", new object[1]
      {
        (object) ex.Message
      });
    }
    return numArray;
  }

  public byte[] SaveZipBytes(
    byte[] srcBytes,
    string entryName,
    EventHandler<SaveProgressEventArgs> callBack = null)
  {
    byte[] numArray = (byte[]) null;
    try
    {
      using (ZipFile zipFile = new ZipFile(Encoding.GetEncoding("shift_jis")))
      {
        if (callBack != null)
          zipFile.add_SaveProgress(callBack);
        else
          zipFile.add_SaveProgress(new EventHandler<SaveProgressEventArgs>(this.SaveProgress));
        zipFile.set_AlternateEncodingUsage((ZipOption) 2);
        zipFile.set_CompressionLevel((CompressionLevel) 9);
        zipFile.AddEntry(entryName, srcBytes);
        using (MemoryStream memoryStream = new MemoryStream())
        {
          if ((long) srcBytes.Length % 65536L == 0L)
            zipFile.set_ParallelDeflateThreshold(-1L);
          zipFile.Save((Stream) memoryStream);
          numArray = memoryStream.ToArray();
        }
      }
    }
    catch (Exception ex)
    {
      Debug.LogErrorFormat("ZipError: {0}", new object[1]
      {
        (object) ex.Message
      });
    }
    return numArray;
  }
}
