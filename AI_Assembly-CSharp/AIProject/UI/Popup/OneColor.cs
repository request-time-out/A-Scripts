// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Popup.OneColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace AIProject.UI.Popup
{
  [Serializable]
  public struct OneColor
  {
    [SerializeField]
    private Color color;
    [SerializeField]
    [ReadOnly]
    private string colorCode;

    public OneColor(float _r, float _g, float _b, float _a = 255f)
    {
      this.color = new Color(_r / (float) byte.MaxValue, _g / (float) byte.MaxValue, _b / (float) byte.MaxValue, _a / (float) byte.MaxValue);
      this.colorCode = string.Empty;
      this.Init = false;
      this.Apply();
    }

    public OneColor(Color _color)
    {
      this.color = _color;
      this.colorCode = string.Empty;
      this.Init = false;
      this.Apply();
    }

    public bool Init { get; private set; }

    public static implicit operator string(OneColor c)
    {
      if (!c.Init)
        c.Apply();
      return c.colorCode;
    }

    public static implicit operator Color(OneColor c)
    {
      return c.color;
    }

    public void Apply()
    {
      this.Init = true;
      this.colorCode = "#" + ColorUtility.ToHtmlStringRGBA(this.color);
    }

    public override string ToString()
    {
      return this.colorCode;
    }
  }
}
