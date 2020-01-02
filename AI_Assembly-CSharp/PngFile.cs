// Decompiled with JetBrains decompiler
// Type: PngFile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.IO;
using UnityEngine;

public static class PngFile
{
  public static long GetPngSize(BinaryReader br)
  {
    return PngFile.GetPngSize(br.BaseStream);
  }

  public static long GetPngSize(Stream st)
  {
    if (st == null)
      return 0;
    long position = st.Position;
    long num;
    try
    {
      byte[] buffer1 = new byte[8];
      byte[] numArray = new byte[8]
      {
        (byte) 137,
        (byte) 80,
        (byte) 78,
        (byte) 71,
        (byte) 13,
        (byte) 10,
        (byte) 26,
        (byte) 10
      };
      st.Read(buffer1, 0, 8);
      for (int index = 0; index < 8; ++index)
      {
        if ((int) buffer1[index] != (int) numArray[index])
        {
          st.Seek(position, SeekOrigin.Begin);
          return 0;
        }
      }
      bool flag = true;
      while (flag)
      {
        byte[] buffer2 = new byte[4];
        st.Read(buffer2, 0, 4);
        Array.Reverse((Array) buffer2);
        int int32 = BitConverter.ToInt32(buffer2, 0);
        byte[] buffer3 = new byte[4];
        st.Read(buffer3, 0, 4);
        if (BitConverter.ToInt32(buffer3, 0) == 1145980233)
          flag = false;
        if ((long) (int32 + 4) > st.Length - st.Position)
        {
          Debug.LogError((object) "PNGが破損している可能性があります：");
          st.Seek(position, SeekOrigin.Begin);
          return 0;
        }
        st.Seek((long) (int32 + 4), SeekOrigin.Current);
      }
      num = st.Position - position;
      st.Seek(position, SeekOrigin.Begin);
    }
    catch (EndOfStreamException ex)
    {
      Debug.LogError((object) ("PNGが破損している可能性があります：" + ex.GetType().Name));
      st.Seek(position, SeekOrigin.Begin);
      return 0;
    }
    return num;
  }

  public static long SkipPng(Stream st)
  {
    long pngSize = PngFile.GetPngSize(st);
    st.Seek(pngSize, SeekOrigin.Current);
    return pngSize;
  }

  public static long SkipPng(BinaryReader br)
  {
    long pngSize = PngFile.GetPngSize(br);
    br.BaseStream.Seek(pngSize, SeekOrigin.Current);
    return pngSize;
  }

  public static byte[] LoadPngBytes(string path)
  {
    using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
      return PngFile.LoadPngBytes((Stream) fileStream);
  }

  public static byte[] LoadPngBytes(Stream st)
  {
    using (BinaryReader br = new BinaryReader(st))
      return PngFile.LoadPngBytes(br);
  }

  public static byte[] LoadPngBytes(BinaryReader br)
  {
    long pngSize = PngFile.GetPngSize(br);
    return pngSize == 0L ? (byte[]) null : br.ReadBytes((int) pngSize);
  }
}
