// Decompiled with JetBrains decompiler
// Type: AIProject.ColorBlock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIProject
{
  [Serializable]
  public struct ColorBlock : IEquatable<ColorBlock>
  {
    [SerializeField]
    private Color _normalColor;
    [SerializeField]
    private Color _highlightColor;
    [SerializeField]
    private Color _pressedColor;

    public Color NormalColor
    {
      get
      {
        return this._normalColor;
      }
      set
      {
        this._normalColor = value;
      }
    }

    public Color HighlightColor
    {
      get
      {
        return this._highlightColor;
      }
      set
      {
        this._highlightColor = value;
      }
    }

    public Color PressedColor
    {
      get
      {
        return this._pressedColor;
      }
      set
      {
        this._pressedColor = value;
      }
    }

    public static ColorBlock Default
    {
      get
      {
        return new ColorBlock()
        {
          _normalColor = (Color) null,
          _highlightColor = (Color) null,
          _pressedColor = (Color) null
        };
      }
    }

    public override bool Equals(object obj)
    {
      return obj is ColorBlock other && this.Equals(other);
    }

    public bool Equals(ColorBlock other)
    {
      return Color.op_Equality(this.NormalColor, other.NormalColor) && Color.op_Equality(this.HighlightColor, other.HighlightColor) && Color.op_Equality(this.PressedColor, other.PressedColor);
    }

    public static bool operator ==(ColorBlock a, ColorBlock b)
    {
      return a.Equals(b);
    }

    public static bool operator !=(ColorBlock a, ColorBlock b)
    {
      return !a.Equals(b);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}
