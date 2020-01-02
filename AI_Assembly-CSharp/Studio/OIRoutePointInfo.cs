// Decompiled with JetBrains decompiler
// Type: Studio.OIRoutePointInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.IO;

namespace Studio
{
  public class OIRoutePointInfo : ObjectInfo
  {
    public float speed = 2f;
    public StudioTween.EaseType easeType = StudioTween.EaseType.linear;
    public OIRoutePointInfo.Connection connection;
    public OIRoutePointAidInfo aidInfo;
    public bool link;

    public OIRoutePointInfo(int _key)
      : base(_key)
    {
      this.number = 0;
      this.speed = 2f;
      this.easeType = StudioTween.EaseType.linear;
    }

    public override int kind
    {
      get
      {
        return 6;
      }
    }

    public string name { get; private set; }

    public int number
    {
      set
      {
        this.name = value != 0 ? string.Format("ポイント{0}", (object) value) : "スタート";
      }
    }

    public override int[] kinds
    {
      get
      {
        return new int[2]{ 6, 4 };
      }
    }

    public override void Save(BinaryWriter _writer, Version _version)
    {
      _writer.Write(this.dicKey);
      this.changeAmount.Save(_writer);
      _writer.Write(this.speed);
      _writer.Write((int) this.easeType);
      _writer.Write((int) this.connection);
      this.aidInfo.Save(_writer, _version);
      _writer.Write(this.link);
    }

    public override void Load(BinaryReader _reader, Version _version, bool _import, bool _tree = true)
    {
      base.Load(_reader, _version, _import, false);
      this.speed = _reader.ReadSingle();
      this.easeType = (StudioTween.EaseType) _reader.ReadInt32();
      this.connection = (OIRoutePointInfo.Connection) _reader.ReadInt32();
      if (this.aidInfo == null)
        this.aidInfo = new OIRoutePointAidInfo(_import ? Studio.Studio.GetNewIndex() : -1);
      this.aidInfo.Load(_reader, _version, _import, true);
      this.link = _reader.ReadBoolean();
    }

    public override void DeleteKey()
    {
      Studio.Studio.DeleteIndex(this.dicKey);
    }

    public enum Connection
    {
      Line,
      Curve,
    }
  }
}
