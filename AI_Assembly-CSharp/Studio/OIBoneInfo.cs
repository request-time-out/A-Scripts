// Decompiled with JetBrains decompiler
// Type: Studio.OIBoneInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.IO;

namespace Studio
{
  public class OIBoneInfo : ObjectInfo
  {
    public OIBoneInfo(int _key)
      : base(_key)
    {
      this.group = (OIBoneInfo.BoneGroup) 0;
      this.level = 0;
    }

    public override int kind
    {
      get
      {
        return -1;
      }
    }

    public OIBoneInfo.BoneGroup group { get; set; }

    public int level { get; set; }

    public override void Save(BinaryWriter _writer, Version _version)
    {
      _writer.Write(this.dicKey);
      this.changeAmount.Save(_writer);
    }

    public override void Load(BinaryReader _reader, Version _version, bool _import, bool _tree = true)
    {
      base.Load(_reader, _version, _import, false);
    }

    public override void DeleteKey()
    {
      Studio.Studio.DeleteIndex(this.dicKey);
    }

    public enum BoneGroup
    {
      Body = 1,
      RightLeg = 2,
      LeftLeg = 4,
      RightArm = 8,
      LeftArm = 16, // 0x00000010
      RightHand = 32, // 0x00000020
      LeftHand = 64, // 0x00000040
      Hair = 128, // 0x00000080
      Neck = 256, // 0x00000100
      Breast = 512, // 0x00000200
      Skirt = 1024, // 0x00000400
    }
  }
}
