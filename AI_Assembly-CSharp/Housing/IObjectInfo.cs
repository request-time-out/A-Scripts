// Decompiled with JetBrains decompiler
// Type: Housing.IObjectInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using MessagePack;
using UnityEngine;

namespace Housing
{
  [Union(0, typeof (OIItem))]
  [Union(1, typeof (OIFolder))]
  public interface IObjectInfo
  {
    int Kind { get; }

    Vector3 Pos { get; set; }

    Vector3 Rot { get; set; }
  }
}
