// Decompiled with JetBrains decompiler
// Type: FreeModifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI.ProceduralImage;

[ModifierID("Free")]
public class FreeModifier : ProceduralImageModifier
{
  [SerializeField]
  private Vector4 radius;

  public Vector4 Radius
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
    return this.radius;
  }

  protected void OnValidate()
  {
    this.radius.x = (__Null) (double) Mathf.Max(0.0f, (float) this.radius.x);
    this.radius.y = (__Null) (double) Mathf.Max(0.0f, (float) this.radius.y);
    this.radius.z = (__Null) (double) Mathf.Max(0.0f, (float) this.radius.z);
    this.radius.w = (__Null) (double) Mathf.Max(0.0f, (float) this.radius.w);
  }
}
