// Decompiled with JetBrains decompiler
// Type: RoundModifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI.ProceduralImage;

[ModifierID("Round")]
public class RoundModifier : ProceduralImageModifier
{
  public override Vector4 CalculateRadius(Rect imageRect)
  {
    float num = Mathf.Min(((Rect) ref imageRect).get_width(), ((Rect) ref imageRect).get_height()) * 0.5f;
    return new Vector4(num, num, num, num);
  }
}
