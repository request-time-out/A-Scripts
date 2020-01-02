// Decompiled with JetBrains decompiler
// Type: MetaballCellObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class MetaballCellObject : MonoBehaviour
{
  protected MetaballCell _cell;

  public MetaballCellObject()
  {
    base.\u002Ector();
  }

  public MetaballCell Cell
  {
    get
    {
      return this._cell;
    }
  }

  public virtual void Setup(MetaballCell cell)
  {
    this._cell = cell;
  }
}
