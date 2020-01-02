// Decompiled with JetBrains decompiler
// Type: SaveAssist.BlockHeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using OutputLogControl;
using System;
using System.IO;
using System.Text;

namespace SaveAssist
{
  public class BlockHeader
  {
    public string tagName = string.Empty;
    private const int tagSize = 128;
    public byte[] tag;
    public int version;
    public long pos;
    public long size;

    public void SetHeader(string _tagName, int _version, long _pos, long _size)
    {
      this.tagName = _tagName;
      this.tag = BlockHeader.ChangeStringToByte(this.tagName);
      this.version = _version;
      this.pos = _pos;
      this.size = _size;
    }

    public bool SaveHeader(BinaryWriter writer)
    {
      if (this.tag == null)
      {
        OutputLog.Error("BlockHeaderのタグ情報がNULLです", true, "Log");
        return false;
      }
      writer.Write(this.tag);
      writer.Write(this.version);
      writer.Write(this.pos);
      writer.Write(this.size);
      return true;
    }

    public bool LoadHeader(BinaryReader reader)
    {
      this.tag = reader.ReadBytes(128);
      this.tagName = BlockHeader.ChangeByteToString(this.tag);
      this.version = reader.ReadInt32();
      this.pos = reader.ReadInt64();
      this.size = reader.ReadInt64();
      return true;
    }

    public static int GetBlockHeaderSize()
    {
      return 148;
    }

    public static byte[] ChangeStringToByte(string _tagName)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(_tagName);
      if (bytes.GetLength(0) > 128)
      {
        OutputLog.Error("ブロック識別コードのバイト数がオーバーしています", true, "Log");
        return (byte[]) null;
      }
      byte[] numArray = new byte[128];
      Buffer.BlockCopy((Array) bytes, 0, (Array) numArray, 0, bytes.GetLength(0));
      return numArray;
    }

    public static string ChangeByteToString(byte[] _tag)
    {
      return Encoding.UTF8.GetString(_tag);
    }
  }
}
