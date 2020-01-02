// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UISelectionUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Rewired.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
  public static class UISelectionUtility
  {
    public static Selectable FindNextSelectable(
      Selectable selectable,
      Transform transform,
      List<Selectable> allSelectables,
      Vector3 direction)
    {
      RectTransform rectTransform1 = transform as RectTransform;
      if (Object.op_Equality((Object) rectTransform1, (Object) null))
        return (Selectable) null;
      ((Vector3) ref direction).Normalize();
      Vector2 dir = Vector2.op_Implicit(direction);
      Vector2 vector2_1 = Vector2.op_Implicit(UITools.GetPointOnRectEdge(rectTransform1, dir));
      bool flag = Vector2.op_Equality(dir, Vector2.op_Multiply(Vector2.get_right(), -1f)) || Vector2.op_Equality(dir, Vector2.get_right());
      float num1 = float.PositiveInfinity;
      float num2 = float.PositiveInfinity;
      Selectable selectable1 = (Selectable) null;
      Selectable selectable2 = (Selectable) null;
      Vector2 vector2_2 = Vector2.op_Addition(vector2_1, Vector2.op_Multiply(dir, 999999f));
      for (int index = 0; index < allSelectables.Count; ++index)
      {
        Selectable allSelectable = allSelectables[index];
        if (!Object.op_Equality((Object) allSelectable, (Object) selectable) && !Object.op_Equality((Object) allSelectable, (Object) null))
        {
          Navigation navigation = allSelectable.get_navigation();
          if (((Navigation) ref navigation).get_mode() != null && (allSelectable.IsInteractable() || (int) ReflectionTools.GetPrivateField<Selectable, bool>((M0) allSelectable, "m_GroupsAllowInteraction") != 0))
          {
            RectTransform transform1 = ((Component) allSelectable).get_transform() as RectTransform;
            if (!Object.op_Equality((Object) transform1, (Object) null))
            {
              Rect rect1 = UITools.InvertY(UITools.TransformRectTo((Transform) transform1, transform, transform1.get_rect()));
              float num3;
              if (MathTools.LineIntersectsRect(vector2_1, vector2_2, rect1, ref num3))
              {
                if (flag)
                  num3 *= 0.25f;
                if ((double) num3 < (double) num2)
                {
                  num2 = num3;
                  selectable2 = allSelectable;
                }
              }
              RectTransform rectTransform2 = transform1;
              Transform transform2 = transform;
              Rect rect2 = transform1.get_rect();
              Vector3 vector3 = Vector2.op_Implicit(((Rect) ref rect2).get_center());
              Vector2 vector2_3 = Vector2.op_Subtraction(Vector2.op_Implicit(UnityTools.TransformPoint((Transform) rectTransform2, transform2, vector3)), vector2_1);
              if ((double) Mathf.Abs(Vector2.Angle(dir, vector2_3)) <= 75.0)
              {
                float sqrMagnitude = ((Vector2) ref vector2_3).get_sqrMagnitude();
                if ((double) sqrMagnitude < (double) num1)
                {
                  num1 = sqrMagnitude;
                  selectable1 = allSelectable;
                }
              }
            }
          }
        }
      }
      if (!Object.op_Inequality((Object) selectable2, (Object) null) || !Object.op_Inequality((Object) selectable1, (Object) null))
        return selectable2 ?? selectable1;
      return (double) num2 > (double) num1 ? selectable1 : selectable2;
    }
  }
}
