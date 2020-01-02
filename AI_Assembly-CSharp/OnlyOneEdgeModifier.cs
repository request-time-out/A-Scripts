// Decompiled with JetBrains decompiler
// Type: OnlyOneEdgeModifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI.ProceduralImage;

[ModifierID("Only One Edge")]
public class OnlyOneEdgeModifier : ProceduralImageModifier
{
  [SerializeField]
  private float radius;
  [SerializeField]
  private OnlyOneEdgeModifier.ProceduralImageEdge side;

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

  public OnlyOneEdgeModifier.ProceduralImageEdge Side
  {
    get
    {
      return this.side;
    }
    set
    {
      this.side = value;
    }
  }

  public override Vector4 CalculateRadius(Rect imageRect)
  {
    switch (this.side)
    {
      case OnlyOneEdgeModifier.ProceduralImageEdge.Top:
        return new Vector4(this.radius, this.radius, 0.0f, 0.0f);
      case OnlyOneEdgeModifier.ProceduralImageEdge.Bottom:
        return new Vector4(0.0f, 0.0f, this.radius, this.radius);
      case OnlyOneEdgeModifier.ProceduralImageEdge.Left:
        return new Vector4(this.radius, 0.0f, 0.0f, this.radius);
      case OnlyOneEdgeModifier.ProceduralImageEdge.Right:
        return new Vector4(0.0f, this.radius, this.radius, 0.0f);
      default:
        return new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
    }
  }

  public enum ProceduralImageEdge
  {
    Top,
    Bottom,
    Left,
    Right,
  }
}
