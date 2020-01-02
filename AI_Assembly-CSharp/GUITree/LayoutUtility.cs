// Decompiled with JetBrains decompiler
// Type: GUITree.LayoutUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace GUITree
{
  public static class LayoutUtility
  {
    public static float GetMinSize(ILayoutElement _element, int _axis)
    {
      return _axis == 0 ? LayoutUtility.GetMinWidth(_element) : LayoutUtility.GetMinHeight(_element);
    }

    public static float GetPreferredSize(ILayoutElement _element, int _axis)
    {
      return _axis == 0 ? LayoutUtility.GetPreferredWidth(_element) : LayoutUtility.GetPreferredHeight(_element);
    }

    public static float GetFlexibleSize(ILayoutElement _element, int _axis)
    {
      return _axis == 0 ? LayoutUtility.GetFlexibleWidth(_element) : LayoutUtility.GetFlexibleHeight(_element);
    }

    public static float GetMinWidth(ILayoutElement _element)
    {
      return _element == null ? 0.0f : _element.get_minWidth();
    }

    public static float GetPreferredWidth(ILayoutElement _element)
    {
      return _element == null ? 0.0f : Mathf.Max(Mathf.Max(_element.get_minWidth(), _element.get_preferredWidth()), 0.0f);
    }

    public static float GetFlexibleWidth(ILayoutElement _element)
    {
      return _element == null ? 0.0f : _element.get_flexibleWidth();
    }

    public static float GetMinHeight(ILayoutElement _element)
    {
      return _element == null ? 0.0f : _element.get_minHeight();
    }

    public static float GetPreferredHeight(ILayoutElement _element)
    {
      return _element == null ? 0.0f : Mathf.Max(Mathf.Max(_element.get_minHeight(), _element.get_preferredHeight()), 0.0f);
    }

    public static float GetFlexibleHeight(ILayoutElement _element)
    {
      return _element == null ? 0.0f : _element.get_flexibleHeight();
    }
  }
}
