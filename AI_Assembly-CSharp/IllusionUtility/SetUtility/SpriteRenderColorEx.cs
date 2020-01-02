// Decompiled with JetBrains decompiler
// Type: IllusionUtility.SetUtility.SpriteRenderColorEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace IllusionUtility.SetUtility
{
  public static class SpriteRenderColorEx
  {
    public static void SetColorR(this SpriteRenderer sr, float r)
    {
      Color color;
      ((Color) ref color).\u002Ector(r, (float) sr.get_color().g, (float) sr.get_color().b, (float) sr.get_color().a);
      sr.set_color(color);
    }

    public static void SetColorG(this SpriteRenderer sr, float g)
    {
      Color color;
      ((Color) ref color).\u002Ector((float) sr.get_color().r, g, (float) sr.get_color().b, (float) sr.get_color().a);
      sr.set_color(color);
    }

    public static void SetColorB(this SpriteRenderer sr, float b)
    {
      Color color;
      ((Color) ref color).\u002Ector((float) sr.get_color().r, (float) sr.get_color().g, b, (float) sr.get_color().a);
      sr.set_color(color);
    }

    public static void SetColorA(this SpriteRenderer sr, float a)
    {
      Color color;
      ((Color) ref color).\u002Ector((float) sr.get_color().r, (float) sr.get_color().g, (float) sr.get_color().b, a);
      sr.set_color(color);
    }
  }
}
