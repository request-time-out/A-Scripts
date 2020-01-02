// Decompiled with JetBrains decompiler
// Type: Housing.OIFolder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using MessagePack;
using System.Collections.Generic;
using UnityEngine;

namespace Housing
{
  [MessagePackObject(false)]
  public class OIFolder : IObjectInfo
  {
    public OIFolder()
    {
    }

    public OIFolder(OIFolder _src)
    {
      this.Pos = _src.Pos;
      this.Rot = _src.Rot;
      this.Name = _src.Name;
      this.Expand = _src.Expand;
      foreach (IObjectInfo objectInfo in _src.Child)
      {
        switch (objectInfo.Kind)
        {
          case 0:
            this.Child.Add((IObjectInfo) new OIItem(objectInfo as OIItem));
            continue;
          case 1:
            this.Child.Add((IObjectInfo) new OIFolder(objectInfo as OIFolder));
            continue;
          default:
            continue;
        }
      }
    }

    public OIFolder(OIFolder _src, bool _idCopy)
    {
      this.Pos = _src.Pos;
      this.Rot = _src.Rot;
      this.Name = _src.Name;
      this.Expand = _src.Expand;
      foreach (IObjectInfo objectInfo in _src.Child)
      {
        switch (objectInfo.Kind)
        {
          case 0:
            this.Child.Add((IObjectInfo) new OIItem(objectInfo as OIItem, _idCopy));
            continue;
          case 1:
            this.Child.Add((IObjectInfo) new OIFolder(objectInfo as OIFolder, _idCopy));
            continue;
          default:
            continue;
        }
      }
    }

    [IgnoreMember]
    public int Kind
    {
      get
      {
        return 1;
      }
    }

    [Key(0)]
    public Vector3 Pos { get; set; } = Vector3.get_zero();

    [Key(1)]
    public Vector3 Rot { get; set; } = Vector3.get_zero();

    [Key(2)]
    public string Name { get; set; } = "フォルダー";

    [Key(3)]
    public List<IObjectInfo> Child { get; set; } = new List<IObjectInfo>();

    [Key(4)]
    public bool Expand { get; set; } = true;
  }
}
