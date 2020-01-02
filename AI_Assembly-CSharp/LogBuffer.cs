// Decompiled with JetBrains decompiler
// Type: LogBuffer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Text;

public class LogBuffer
{
  public int BufSize = 16384;
  public const int KB = 1024;
  public const int InternalBufSize = 16384;
  public byte[] Buf;
  public int BufWrittenBytes;

  public LogBuffer(int userDefinedSize = 0)
  {
    if (userDefinedSize != 0)
      this.BufSize = userDefinedSize;
    this.Buf = new byte[this.BufSize];
  }

  public bool Receive(string content)
  {
    byte[] bytes = Encoding.Default.GetBytes(content);
    if (this.BufWrittenBytes + bytes.Length > this.BufSize)
      return false;
    Buffer.BlockCopy((Array) bytes, 0, (Array) this.Buf, this.BufWrittenBytes, bytes.Length);
    this.BufWrittenBytes += bytes.Length;
    return true;
  }

  public void Clear()
  {
    this.BufWrittenBytes = 0;
  }
}
