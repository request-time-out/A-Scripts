// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Viewer.CursorFrame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Linq;
using UnityEngine;

namespace AIProject.UI.Viewer
{
  public static class CursorFrame
  {
    private static RectTransform.Axis[] _axes;

    private static RectTransform.Axis[] axes
    {
      get
      {
        return CursorFrame._axes ?? (CursorFrame._axes = Enum.GetValues(typeof (RectTransform.Axis)).Cast<RectTransform.Axis>().ToArray<RectTransform.Axis>());
      }
    }

    public static void Set(RectTransform cursor, RectTransform target, RectTransform size = null)
    {
      ((Transform) cursor).set_position(((Transform) target).get_position());
      if (Object.op_Equality((Object) size, (Object) null))
        size = target;
      CursorFrame.SetSize(cursor, size);
    }

    public static void SetSize(RectTransform cursor, RectTransform size)
    {
      for (int index = 0; index < CursorFrame.axes.Length; ++index)
      {
        RectTransform rectTransform = cursor;
        int ax = (int) CursorFrame.axes[index];
        Rect rect = size.get_rect();
        Vector2 size1 = ((Rect) ref rect).get_size();
        double num = (double) ((Vector2) ref size1).get_Item(index);
        rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) ax, (float) num);
      }
    }

    public static void Set(
      RectTransform cursor,
      RectTransform target,
      ref Vector3 velocity,
      float? smoothMoveTime = null,
      float? smoothSizeTime = null)
    {
      if (!smoothMoveTime.HasValue)
        ((Transform) cursor).set_position(((Transform) target).get_position());
      else
        ((Transform) cursor).set_position(Smooth.Damp(((Transform) cursor).get_position(), ((Transform) target).get_position(), ref velocity, smoothMoveTime.Value));
      if (!smoothSizeTime.HasValue)
        return;
      Vector3 velocity1 = velocity;
      CursorFrame.SetSize(cursor, target, ref velocity1, smoothSizeTime.Value);
      if (smoothMoveTime.HasValue)
        return;
      velocity = velocity1;
    }

    public static void SetSize(
      RectTransform cursor,
      RectTransform target,
      ref Vector3 velocity,
      float smoothTime)
    {
      for (int index = 0; index < CursorFrame.axes.Length; ++index)
      {
        float num1 = ((Vector3) ref velocity).get_Item(index);
        Rect rect1 = cursor.get_rect();
        Vector2 size1 = ((Rect) ref rect1).get_size();
        double num2 = (double) ((Vector2) ref size1).get_Item(index);
        Rect rect2 = target.get_rect();
        Vector2 size2 = ((Rect) ref rect2).get_size();
        double num3 = (double) ((Vector2) ref size2).get_Item(index);
        ref float local = ref num1;
        double num4 = (double) smoothTime;
        float num5 = Smooth.Damp((float) num2, (float) num3, ref local, (float) num4);
        ((Vector3) ref velocity).set_Item(index, num1);
        cursor.SetSizeWithCurrentAnchors((RectTransform.Axis) (int) CursorFrame.axes[index], num5);
      }
    }
  }
}
