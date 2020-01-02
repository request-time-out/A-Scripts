// Decompiled with JetBrains decompiler
// Type: Studio.OICameraInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.IO;

namespace Studio
{
  public class OICameraInfo : ObjectInfo
  {
    public string name = string.Empty;
    public bool active;

    public OICameraInfo(int _key)
      : base(_key)
    {
      this.name = string.Format("カメラ{0}", (object) Singleton<Studio.Studio>.Instance.cameraCount);
      this.active = false;
    }

    public override int kind
    {
      get
      {
        return 5;
      }
    }

    public override void Save(BinaryWriter _writer, Version _version)
    {
      base.Save(_writer, _version);
      _writer.Write(this.name);
      _writer.Write(this.active);
    }

    public override void Load(BinaryReader _reader, Version _version, bool _import, bool _tree = true)
    {
      base.Load(_reader, _version, _import, true);
      this.name = _reader.ReadString();
      this.active = _reader.ReadBoolean();
    }

    public override void DeleteKey()
    {
      Studio.Studio.DeleteIndex(this.dicKey);
    }
  }
}
