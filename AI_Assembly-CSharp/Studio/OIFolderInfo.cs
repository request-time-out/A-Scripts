// Decompiled with JetBrains decompiler
// Type: Studio.OIFolderInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace Studio
{
  public class OIFolderInfo : ObjectInfo
  {
    public string name = string.Empty;

    public OIFolderInfo(int _key)
      : base(_key)
    {
      this.name = "フォルダー";
      this.child = new List<ObjectInfo>();
    }

    public override int kind
    {
      get
      {
        return 3;
      }
    }

    public List<ObjectInfo> child { get; private set; }

    public override void Save(BinaryWriter _writer, Version _version)
    {
      base.Save(_writer, _version);
      _writer.Write(this.name);
      int count = this.child.Count;
      _writer.Write(count);
      for (int index = 0; index < count; ++index)
        this.child[index].Save(_writer, _version);
    }

    public override void Load(BinaryReader _reader, Version _version, bool _import, bool _tree = true)
    {
      base.Load(_reader, _version, _import, true);
      this.name = _reader.ReadString();
      ObjectInfoAssist.LoadChild(_reader, _version, this.child, _import);
    }

    public override void DeleteKey()
    {
      Studio.Studio.DeleteIndex(this.dicKey);
      int count = this.child.Count;
      for (int index = 0; index < count; ++index)
        this.child[index].DeleteKey();
    }
  }
}
