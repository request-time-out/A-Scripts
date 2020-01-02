// Decompiled with JetBrains decompiler
// Type: SpriteInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class SpriteInfo
{
  public static Vector2 GetPivotInSprite(Sprite sprite)
  {
    Vector2 vector2 = (Vector2) null;
    if (Object.op_Inequality((Object) null, (Object) sprite))
    {
      Bounds bounds1 = sprite.get_bounds();
      if (((Bounds) ref bounds1).get_size().x != 0.0)
      {
        ref Vector2 local = ref vector2;
        Bounds bounds2 = sprite.get_bounds();
        // ISSUE: variable of the null type
        __Null x1 = ((Bounds) ref bounds2).get_center().x;
        Bounds bounds3 = sprite.get_bounds();
        // ISSUE: variable of the null type
        __Null x2 = ((Bounds) ref bounds3).get_size().x;
        double num = 0.5 - x1 / x2;
        local.x = (__Null) num;
      }
      Bounds bounds4 = sprite.get_bounds();
      if (((Bounds) ref bounds4).get_size().y != 0.0)
      {
        ref Vector2 local = ref vector2;
        Bounds bounds2 = sprite.get_bounds();
        // ISSUE: variable of the null type
        __Null y1 = ((Bounds) ref bounds2).get_center().y;
        Bounds bounds3 = sprite.get_bounds();
        // ISSUE: variable of the null type
        __Null y2 = ((Bounds) ref bounds3).get_size().y;
        double num = 0.5 - y1 / y2;
        local.y = (__Null) num;
      }
    }
    return vector2;
  }
}
