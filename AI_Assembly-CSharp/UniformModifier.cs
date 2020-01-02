// Decompiled with JetBrains decompiler
// Type: UniformModifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI.ProceduralImage;

[ModifierID("Uniform")]
public class UniformModifier : ProceduralImageModifier
{
  [SerializeField]
  private float radius;

  public float Radius
  {
    get
    {
      return this.radius;
    }
    set
    {
      this.radius = value;
    }
  }

  public override Vector4 CalculateRadius(Rect imageRect)
  {
    float radius = this.radius;
    return new Vector4(radius, radius, radius, radius);
  }
}
