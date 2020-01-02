// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UITools
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
  public static class UITools
  {
    public static GameObject InstantiateGUIObject<T>(
      GameObject prefab,
      Transform parent,
      string name)
      where T : Component
    {
      GameObject gameObject = UITools.InstantiateGUIObject_Pre<T>(prefab, parent, name);
      if (Object.op_Equality((Object) gameObject, (Object) null))
        return (GameObject) null;
      RectTransform component = (RectTransform) gameObject.GetComponent<RectTransform>();
      if (Object.op_Equality((Object) component, (Object) null))
        Debug.LogError((object) (name + " prefab is missing RectTransform component!"));
      else
        ((Transform) component).set_localScale(Vector3.get_one());
      return gameObject;
    }

    public static GameObject InstantiateGUIObject<T>(
      GameObject prefab,
      Transform parent,
      string name,
      Vector2 pivot,
      Vector2 anchorMin,
      Vector2 anchorMax,
      Vector2 anchoredPosition)
      where T : Component
    {
      GameObject gameObject = UITools.InstantiateGUIObject_Pre<T>(prefab, parent, name);
      if (Object.op_Equality((Object) gameObject, (Object) null))
        return (GameObject) null;
      RectTransform component = (RectTransform) gameObject.GetComponent<RectTransform>();
      if (Object.op_Equality((Object) component, (Object) null))
      {
        Debug.LogError((object) (name + " prefab is missing RectTransform component!"));
      }
      else
      {
        ((Transform) component).set_localScale(Vector3.get_one());
        component.set_pivot(pivot);
        component.set_anchorMin(anchorMin);
        component.set_anchorMax(anchorMax);
        component.set_anchoredPosition(anchoredPosition);
      }
      return gameObject;
    }

    private static GameObject InstantiateGUIObject_Pre<T>(
      GameObject prefab,
      Transform parent,
      string name)
      where T : Component
    {
      if (Object.op_Equality((Object) prefab, (Object) null))
      {
        Debug.LogError((object) (name + " prefab is null!"));
        return (GameObject) null;
      }
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) prefab);
      if (!string.IsNullOrEmpty(name))
        ((Object) gameObject).set_name(name);
      T component = gameObject.GetComponent<T>();
      if (Object.op_Equality((Object) (object) component, (Object) null))
      {
        Debug.LogError((object) (name + " prefab is missing the " + component.GetType().ToString() + " component!"));
        return (GameObject) null;
      }
      if (Object.op_Inequality((Object) parent, (Object) null))
        gameObject.get_transform().SetParent(parent, false);
      return gameObject;
    }

    public static Vector3 GetPointOnRectEdge(RectTransform rectTransform, Vector2 dir)
    {
      if (Object.op_Equality((Object) rectTransform, (Object) null))
        return Vector3.get_zero();
      if (Vector2.op_Inequality(dir, Vector2.get_zero()))
        dir = Vector2.op_Division(dir, Mathf.Max(Mathf.Abs((float) dir.x), Mathf.Abs((float) dir.y)));
      Rect rect = rectTransform.get_rect();
      dir = Vector2.op_Addition(((Rect) ref rect).get_center(), Vector2.Scale(((Rect) ref rect).get_size(), Vector2.op_Multiply(dir, 0.5f)));
      return Vector2.op_Implicit(dir);
    }

    public static Rect GetWorldSpaceRect(RectTransform rt)
    {
      if (Object.op_Equality((Object) rt, (Object) null))
        return (Rect) null;
      Rect rect = rt.get_rect();
      Vector2 vector2_1 = Vector2.op_Implicit(((Transform) rt).TransformPoint(Vector2.op_Implicit(new Vector2(((Rect) ref rect).get_xMin(), ((Rect) ref rect).get_yMin()))));
      Vector2 vector2_2 = Vector2.op_Implicit(((Transform) rt).TransformPoint(Vector2.op_Implicit(new Vector2(((Rect) ref rect).get_xMin(), ((Rect) ref rect).get_yMax()))));
      Vector2 vector2_3 = Vector2.op_Implicit(((Transform) rt).TransformPoint(Vector2.op_Implicit(new Vector2(((Rect) ref rect).get_xMax(), ((Rect) ref rect).get_yMin()))));
      return new Rect((float) vector2_1.x, (float) vector2_1.y, (float) (vector2_3.x - vector2_1.x), (float) (vector2_2.y - vector2_1.y));
    }

    public static Rect TransformRectTo(Transform from, Transform to, Rect rect)
    {
      Vector3 vector3_1;
      Vector3 vector3_2;
      Vector3 vector3_3;
      if (Object.op_Inequality((Object) from, (Object) null))
      {
        vector3_1 = from.TransformPoint(Vector2.op_Implicit(new Vector2(((Rect) ref rect).get_xMin(), ((Rect) ref rect).get_yMin())));
        vector3_2 = from.TransformPoint(Vector2.op_Implicit(new Vector2(((Rect) ref rect).get_xMin(), ((Rect) ref rect).get_yMax())));
        vector3_3 = from.TransformPoint(Vector2.op_Implicit(new Vector2(((Rect) ref rect).get_xMax(), ((Rect) ref rect).get_yMin())));
      }
      else
      {
        vector3_1 = Vector2.op_Implicit(new Vector2(((Rect) ref rect).get_xMin(), ((Rect) ref rect).get_yMin()));
        vector3_2 = Vector2.op_Implicit(new Vector2(((Rect) ref rect).get_xMin(), ((Rect) ref rect).get_yMax()));
        vector3_3 = Vector2.op_Implicit(new Vector2(((Rect) ref rect).get_xMax(), ((Rect) ref rect).get_yMin()));
      }
      if (Object.op_Inequality((Object) to, (Object) null))
      {
        vector3_1 = to.InverseTransformPoint(vector3_1);
        vector3_2 = to.InverseTransformPoint(vector3_2);
        vector3_3 = to.InverseTransformPoint(vector3_3);
      }
      return new Rect((float) vector3_1.x, (float) vector3_1.y, (float) (vector3_3.x - vector3_1.x), (float) (vector3_1.y - vector3_2.y));
    }

    public static Rect InvertY(Rect rect)
    {
      return new Rect(((Rect) ref rect).get_xMin(), ((Rect) ref rect).get_yMin(), ((Rect) ref rect).get_width(), -((Rect) ref rect).get_height());
    }

    public static void SetInteractable(Selectable selectable, bool state, bool playTransition)
    {
      if (Object.op_Equality((Object) selectable, (Object) null))
        return;
      if (!playTransition)
      {
        if (selectable.get_transition() == 1)
        {
          ColorBlock colors = selectable.get_colors();
          float fadeDuration = ((ColorBlock) ref colors).get_fadeDuration();
          ((ColorBlock) ref colors).set_fadeDuration(0.0f);
          selectable.set_colors(colors);
          selectable.set_interactable(state);
          ((ColorBlock) ref colors).set_fadeDuration(fadeDuration);
          selectable.set_colors(colors);
        }
        else
          selectable.set_interactable(state);
      }
      else
        selectable.set_interactable(state);
    }
  }
}
