// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.RectTransformExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Battlehub.UIControls
{
  public static class RectTransformExtensions
  {
    public static void Stretch(this RectTransform rt)
    {
      rt.set_anchorMin(new Vector2(0.0f, 0.0f));
      rt.set_anchorMax(new Vector2(1f, 1f));
      rt.set_offsetMin(Vector2.get_zero());
      rt.set_offsetMax(Vector2.get_zero());
    }
  }
}
