// Decompiled with JetBrains decompiler
// Type: HsvColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class HsvColor
{
  private float _h;
  private float _s;
  private float _v;

  public HsvColor(float hue, float saturation, float brightness)
  {
    if ((double) hue < 0.0 || 360.0 < (double) hue)
      throw new ArgumentException("hueは0~360の値です。", nameof (hue));
    if ((double) saturation < 0.0 || 1.0 < (double) saturation)
      throw new ArgumentException("saturationは0以上1以下の値です。", nameof (saturation));
    if ((double) brightness < 0.0 || 1.0 < (double) brightness)
      throw new ArgumentException("brightnessは0以上1以下の値です。", nameof (brightness));
    this._h = hue;
    this._s = saturation;
    this._v = brightness;
  }

  public HsvColor(HsvColor src)
  {
    this._h = src._h;
    this._s = src._s;
    this._v = src._v;
  }

  public float this[int index]
  {
    get
    {
      switch (index)
      {
        case 0:
          return this.H;
        case 1:
          return this.S;
        case 2:
          return this.V;
        default:
          return float.MaxValue;
      }
    }
    set
    {
      switch (index)
      {
        case 0:
          this.H = value;
          break;
        case 1:
          this.S = value;
          break;
        case 2:
          this.V = value;
          break;
      }
    }
  }

  public float H
  {
    get
    {
      return this._h;
    }
    set
    {
      this._h = value;
    }
  }

  public float S
  {
    get
    {
      return this._s;
    }
    set
    {
      this._s = value;
    }
  }

  public float V
  {
    get
    {
      return this._v;
    }
    set
    {
      this._v = value;
    }
  }

  public void Copy(HsvColor src)
  {
    this._h = src._h;
    this._s = src._s;
    this._v = src._v;
  }

  public static HsvColor FromRgb(Color rgb)
  {
    float r = (float) rgb.r;
    float g = (float) rgb.g;
    float b = (float) rgb.b;
    float num1 = Math.Max(r, Math.Max(g, b));
    float num2 = Math.Min(r, Math.Min(g, b));
    float hue = 0.0f;
    if ((double) num1 == (double) num2)
      hue = 0.0f;
    else if ((double) num1 == (double) r)
      hue = (float) ((60.0 * ((double) g - (double) b) / ((double) num1 - (double) num2) + 360.0) % 360.0);
    else if ((double) num1 == (double) g)
      hue = (float) (60.0 * ((double) b - (double) r) / ((double) num1 - (double) num2) + 120.0);
    else if ((double) num1 == (double) b)
      hue = (float) (60.0 * ((double) r - (double) g) / ((double) num1 - (double) num2) + 240.0);
    float saturation = (double) num1 != 0.0 ? (num1 - num2) / num1 : 0.0f;
    float brightness = num1;
    return new HsvColor(hue, saturation, brightness);
  }

  public static Color ToRgb(float h, float s, float v)
  {
    return HsvColor.ToRgb(new HsvColor(h, s, v));
  }

  public static Color ToRgb(HsvColor hsv)
  {
    float v = hsv.V;
    float s = hsv.S;
    float num1;
    float num2;
    float num3;
    if ((double) s == 0.0)
    {
      num1 = v;
      num2 = v;
      num3 = v;
    }
    else
    {
      float num4 = hsv.H / 60f;
      int num5 = (int) Math.Floor((double) num4) % 6;
      float num6 = num4 - (float) Math.Floor((double) num4);
      float num7 = v * (1f - s);
      float num8 = v * (float) (1.0 - (double) s * (double) num6);
      float num9 = v * (float) (1.0 - (double) s * (1.0 - (double) num6));
      switch (num5)
      {
        case 0:
          num1 = v;
          num2 = num9;
          num3 = num7;
          break;
        case 1:
          num1 = num8;
          num2 = v;
          num3 = num7;
          break;
        case 2:
          num1 = num7;
          num2 = v;
          num3 = num9;
          break;
        case 3:
          num1 = num7;
          num2 = num8;
          num3 = v;
          break;
        case 4:
          num1 = num9;
          num2 = num7;
          num3 = v;
          break;
        case 5:
          num1 = v;
          num2 = num7;
          num3 = num8;
          break;
        default:
          throw new ArgumentException("色相の値が不正です。", nameof (hsv));
      }
    }
    return new Color(num1, num2, num3);
  }

  public static Color ToRgba(HsvColor hsv, float alpha)
  {
    Color rgb = HsvColor.ToRgb(hsv);
    rgb.a = (__Null) (double) alpha;
    return rgb;
  }
}
