// Decompiled with JetBrains decompiler
// Type: AIChara.ChaFileCustom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using MessagePack;
using System;
using System.IO;

namespace AIChara
{
  public class ChaFileCustom : ChaFileAssist
  {
    public static readonly string BlockName = "Custom";
    public ChaFileFace face;
    public ChaFileBody body;
    public ChaFileHair hair;

    public ChaFileCustom()
    {
      this.MemberInit();
    }

    public void MemberInit()
    {
      this.face = new ChaFileFace();
      this.body = new ChaFileBody();
      this.hair = new ChaFileHair();
    }

    public byte[] SaveBytes()
    {
      byte[] buffer1 = MessagePackSerializer.Serialize<ChaFileFace>((M0) this.face);
      byte[] buffer2 = MessagePackSerializer.Serialize<ChaFileBody>((M0) this.body);
      byte[] buffer3 = MessagePackSerializer.Serialize<ChaFileHair>((M0) this.hair);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
        {
          binaryWriter.Write(buffer1.Length);
          binaryWriter.Write(buffer1);
          binaryWriter.Write(buffer2.Length);
          binaryWriter.Write(buffer2);
          binaryWriter.Write(buffer3.Length);
          binaryWriter.Write(buffer3);
          return memoryStream.ToArray();
        }
      }
    }

    public bool LoadBytes(byte[] data, Version ver)
    {
      using (MemoryStream memoryStream = new MemoryStream(data))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) memoryStream))
        {
          int count1 = binaryReader.ReadInt32();
          this.face = (ChaFileFace) MessagePackSerializer.Deserialize<ChaFileFace>(binaryReader.ReadBytes(count1));
          int count2 = binaryReader.ReadInt32();
          this.body = (ChaFileBody) MessagePackSerializer.Deserialize<ChaFileBody>(binaryReader.ReadBytes(count2));
          int count3 = binaryReader.ReadInt32();
          this.hair = (ChaFileHair) MessagePackSerializer.Deserialize<ChaFileHair>(binaryReader.ReadBytes(count3));
          this.face.ComplementWithVersion();
          this.body.ComplementWithVersion();
          this.hair.ComplementWithVersion();
          return true;
        }
      }
    }

    public void SaveFace(string path)
    {
      this.SaveFileAssist<ChaFileFace>(path, this.face);
    }

    public void LoadFace(string path)
    {
      this.LoadFileAssist<ChaFileFace>(path, out this.face);
      this.face.ComplementWithVersion();
    }

    public void LoadFace(byte[] bytes)
    {
      this.LoadFileAssist<ChaFileFace>(bytes, out this.face);
      this.face.ComplementWithVersion();
    }

    public void SaveBody(string path)
    {
      this.SaveFileAssist<ChaFileBody>(path, this.body);
    }

    public void LoadBody(string path)
    {
      this.LoadFileAssist<ChaFileBody>(path, out this.body);
      this.body.ComplementWithVersion();
    }

    public void SaveHair(string path)
    {
      this.SaveFileAssist<ChaFileHair>(path, this.hair);
    }

    public void LoadHair(string path)
    {
      this.LoadFileAssist<ChaFileHair>(path, out this.hair);
      this.hair.ComplementWithVersion();
    }

    public int GetBustSizeKind()
    {
      int num1 = 1;
      float num2 = this.body.shapeValueBody[1];
      if (0.330000013113022 >= (double) num2)
        num1 = 0;
      else if (0.660000026226044 <= (double) num2)
        num1 = 2;
      return num1;
    }

    public int GetHeightKind()
    {
      int num1 = 1;
      float num2 = this.body.shapeValueBody[0];
      if (0.330000013113022 >= (double) num2)
        num1 = 0;
      else if (0.660000026226044 <= (double) num2)
        num1 = 2;
      return num1;
    }
  }
}
