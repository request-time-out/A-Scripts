// Decompiled with JetBrains decompiler
// Type: MinAttribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class MinAttribute : PropertyAttribute
{
  private float min;

  public MinAttribute(float _min = 0.0f)
  {
    this.\u002Ector();
    this.min = _min;
  }

  public float GetValue(float _value)
  {
    return Mathf.Max(this.min, _value);
  }

  public int GetValue(int _value)
  {
    return (int) Mathf.Max(this.min, (float) _value);
  }
}
