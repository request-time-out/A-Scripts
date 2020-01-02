// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.SpiralPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject.Animal
{
  public class SpiralPoint
  {
    public Vector2Int Vector = Vector2Int.get_left();
    public Vector2Int Count = Vector2Int.get_zero();
    public Vector2Int Current = Vector2Int.get_zero();
    public int Counter;
    public int Limit;
    public bool End;

    public SpiralPoint(int limit)
    {
      this.Clear();
      this.Limit = limit;
    }

    public void Clear()
    {
      this.Vector = Vector2Int.get_left();
      this.Count = this.Current = Vector2Int.get_zero();
      this.Counter = this.Limit = 0;
      this.End = false;
    }

    public Vector2Int Next()
    {
      if (((Vector2Int) ref this.Count).get_x() == ((Vector2Int) ref this.Count).get_y() && ((Vector2Int) ref this.Count).get_x() == this.Counter)
      {
        this.RotaRight();
        this.Count = Vector2Int.get_zero();
        ++this.Counter;
        if (this.Limit == this.Counter)
          this.End = true;
      }
      int y = ((Vector2Int) ref this.Count).get_y();
      SpiralPoint spiralPoint = this;
      spiralPoint.Current = Vector2Int.op_Addition(spiralPoint.Current, this.Vector);
      ref Vector2Int local1 = ref this.Count;
      ((Vector2Int) ref local1).set_x(((Vector2Int) ref local1).get_x() + Mathf.Abs(((Vector2Int) ref this.Vector).get_x()));
      ref Vector2Int local2 = ref this.Count;
      ((Vector2Int) ref local2).set_y(((Vector2Int) ref local2).get_y() + Mathf.Abs(((Vector2Int) ref this.Vector).get_y()));
      if (this.Counter != y && this.Counter == ((Vector2Int) ref this.Count).get_y())
        this.RotaRight();
      return this.Current;
    }

    private void RotaRight()
    {
      if (((Vector2Int) ref this.Vector).get_x() != 0)
      {
        ((Vector2Int) ref this.Vector).set_y(-((Vector2Int) ref this.Vector).get_x());
        ((Vector2Int) ref this.Vector).set_x(0);
      }
      else
      {
        ((Vector2Int) ref this.Vector).set_x(((Vector2Int) ref this.Vector).get_y());
        ((Vector2Int) ref this.Vector).set_y(0);
      }
    }
  }
}
