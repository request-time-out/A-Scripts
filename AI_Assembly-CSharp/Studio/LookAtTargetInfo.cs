// Decompiled with JetBrains decompiler
// Type: Studio.LookAtTargetInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.IO;

namespace Studio
{
  public class LookAtTargetInfo : ObjectInfo
  {
    public LookAtTargetInfo(int _key)
      : base(_key)
    {
    }

    public override int kind
    {
      get
      {
        return -1;
      }
    }

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
  }
}
