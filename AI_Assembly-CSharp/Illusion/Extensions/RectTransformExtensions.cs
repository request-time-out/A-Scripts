// Decompiled with JetBrains decompiler
// Type: Illusion.Extensions.RectTransformExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace Illusion.Extensions
{
  public static class RectTransformExtensions
  {
    public static void SetPosition(
      this RectTransform self,
      Transform target3D,
      Vector3 setPos,
      Camera targetCamera = null)
    {
      if (Object.op_Equality((Object) targetCamera, (Object) null))
        targetCamera = Camera.get_main();
      Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(targetCamera, target3D.get_position());
      ((Transform) self).set_position(Vector2.op_Implicit(new Vector2(((Vector2) ref screenPoint).get_Item(0) + ((Vector3) ref setPos).get_Item(0), ((Vector2) ref screenPoint).get_Item(1) + ((Vector3) ref setPos).get_Item(1))));
    }

    public static void AdjustSize(this RectTransform self, Text text)
    {
      self.set_sizeDelta(new Vector2(text.get_preferredWidth(), text.get_preferredHeight()));
    }

    public static void AdjustHeight(this RectTransform self, Text text)
    {
      self.set_sizeDelta(new Vector2((float) self.get_sizeDelta().x, text.get_preferredHeight()));
    }

    public static bool IsHeightOver(this RectTransform self, Text text)
    {
      double preferredHeight = (double) text.get_preferredHeight();
      Rect rect = self.get_rect();
      double height = (double) ((Rect) ref rect).get_height();
      return preferredHeight > height;
    }

    public static void Left(this RectTransform self, float v)
    {
      self.set_offsetMin(new Vector2(v, (float) self.get_offsetMin().y));
    }

    public static void Top(this RectTransform self, float v)
    {
      self.set_offsetMin(new Vector2((float) self.get_offsetMin().x, v));
    }

    public static void Right(this RectTransform self, float v)
    {
      self.set_offsetMax(new Vector2(-v, (float) self.get_offsetMax().y));
    }

    public static void Bottom(this RectTransform self, float v)
    {
      self.set_offsetMax(new Vector2((float) self.get_offsetMax().x, -v));
    }

    public static void Width(this RectTransform self, Vector2 v)
    {
      self.Left((float) v.x);
      self.Right((float) v.y);
    }

    public static void Height(this RectTransform self, Vector2 v)
    {
      self.Top((float) v.x);
      self.Bottom((float) v.y);
    }

    public static float Left(this RectTransform self)
    {
      return (float) self.get_offsetMin().x;
    }

    public static float Top(this RectTransform self)
    {
      return (float) self.get_offsetMin().y;
    }

    public static float Right(this RectTransform self)
    {
      return (float) -self.get_offsetMax().x;
    }

    public static float Bottom(this RectTransform self)
    {
      return (float) -self.get_offsetMax().y;
    }

    public static Vector2 Width(this RectTransform self)
    {
      return new Vector2(self.Left(), self.Right());
    }

    public static Vector2 Height(this RectTransform self)
    {
      return new Vector2(self.Top(), self.Bottom());
    }

    public static bool IsStretchWidth(this RectTransform self)
    {
      return self.get_anchorMin().x == 0.0 && self.get_anchorMax().x == 1.0;
    }

    public static bool IsStretchHeight(this RectTransform self)
    {
      return self.get_anchorMin().y == 0.0 && self.get_anchorMax().y == 1.0;
    }

    public static bool IsStretch(this RectTransform self)
    {
      return self.IsStretchWidth() || self.IsStretchHeight();
    }

    public static bool IsStretchWidthHeight(this RectTransform self)
    {
      return self.IsStretchWidth() && self.IsStretchHeight();
    }
  }
}
