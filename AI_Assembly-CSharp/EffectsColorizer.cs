// Decompiled with JetBrains decompiler
// Type: EffectsColorizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class EffectsColorizer : MonoBehaviour
{
  public Color TintColor;
  public bool UseInstanceWhenNotEditorMode;
  private Color oldColor;

  public EffectsColorizer()
  {
    base.\u002Ector();
  }

  private void Start()
  {
  }

  private void Update()
  {
    if (Color.op_Inequality(this.oldColor, this.TintColor))
      this.ChangeColor(((Component) this).get_gameObject(), this.TintColor);
    this.oldColor = this.TintColor;
  }

  private void ChangeColor(GameObject effect, Color color)
  {
    foreach (Renderer componentsInChild1 in (Renderer[]) effect.GetComponentsInChildren<Renderer>())
    {
      Material material1 = !this.UseInstanceWhenNotEditorMode ? componentsInChild1.get_sharedMaterial() : componentsInChild1.get_material();
      EffectsColorizer.HSBColor hsv1 = this.ColorToHSV(this.TintColor);
      if (!Object.op_Equality((Object) material1, (Object) null))
      {
        if (material1.HasProperty("_TintColor"))
        {
          EffectsColorizer.HSBColor hsv2 = this.ColorToHSV(material1.GetColor("_TintColor"));
          hsv2.h = hsv1.h / 360f;
          color = this.HSVToColor(hsv2);
          material1.SetColor("_TintColor", color);
        }
        if (material1.HasProperty("_CoreColor"))
        {
          EffectsColorizer.HSBColor hsv2 = this.ColorToHSV(material1.GetColor("_CoreColor"));
          hsv2.h = hsv1.h / 360f;
          color = this.HSVToColor(hsv2);
          material1.SetColor("_CoreColor", color);
        }
        foreach (Projector componentsInChild2 in (Projector[]) effect.GetComponentsInChildren<Projector>())
        {
          Material material2 = componentsInChild2.get_material();
          if (!Object.op_Equality((Object) material2, (Object) null) && material2.HasProperty("_TintColor"))
          {
            EffectsColorizer.HSBColor hsv2 = this.ColorToHSV(material2.GetColor("_TintColor"));
            hsv2.h = hsv1.h / 360f;
            color = this.HSVToColor(hsv2);
            componentsInChild2.get_material().SetColor("_TintColor", color);
          }
        }
      }
    }
    Light componentInChildren = (Light) effect.GetComponentInChildren<Light>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    componentInChildren.set_color(color);
  }

  public EffectsColorizer.HSBColor ColorToHSV(Color color)
  {
    EffectsColorizer.HSBColor hsbColor = new EffectsColorizer.HSBColor(0.0f, 0.0f, 0.0f, (float) color.a);
    float r = (float) color.r;
    float g = (float) color.g;
    float b = (float) color.b;
    float num1 = Mathf.Max(r, Mathf.Max(g, b));
    if ((double) num1 <= 0.0)
      return hsbColor;
    float num2 = Mathf.Min(r, Mathf.Min(g, b));
    float num3 = num1 - num2;
    if ((double) num1 > (double) num2)
    {
      hsbColor.h = (double) g != (double) num1 ? ((double) b != (double) num1 ? ((double) b <= (double) g ? (float) (((double) g - (double) b) / (double) num3 * 60.0) : (float) (((double) g - (double) b) / (double) num3 * 60.0 + 360.0)) : (float) (((double) r - (double) g) / (double) num3 * 60.0 + 240.0)) : (float) (((double) b - (double) r) / (double) num3 * 60.0 + 120.0);
      if ((double) hsbColor.h < 0.0)
        hsbColor.h += 360f;
    }
    else
      hsbColor.h = 0.0f;
    hsbColor.h *= 1f / 360f;
    hsbColor.s = (float) ((double) num3 / (double) num1 * 1.0);
    hsbColor.b = num1;
    return hsbColor;
  }

  public Color HSVToColor(EffectsColorizer.HSBColor hsbColor)
  {
    float num1 = hsbColor.b;
    float num2 = hsbColor.b;
    float num3 = hsbColor.b;
    if ((double) hsbColor.s != 0.0)
    {
      float b = hsbColor.b;
      float num4 = hsbColor.b * hsbColor.s;
      float num5 = hsbColor.b - num4;
      float num6 = hsbColor.h * 360f;
      if ((double) num6 < 60.0)
      {
        num1 = b;
        num2 = (float) ((double) num6 * (double) num4 / 60.0) + num5;
        num3 = num5;
      }
      else if ((double) num6 < 120.0)
      {
        num1 = (float) (-((double) num6 - 120.0) * (double) num4 / 60.0) + num5;
        num2 = b;
        num3 = num5;
      }
      else if ((double) num6 < 180.0)
      {
        num1 = num5;
        num2 = b;
        num3 = (float) (((double) num6 - 120.0) * (double) num4 / 60.0) + num5;
      }
      else if ((double) num6 < 240.0)
      {
        num1 = num5;
        num2 = (float) (-((double) num6 - 240.0) * (double) num4 / 60.0) + num5;
        num3 = b;
      }
      else if ((double) num6 < 300.0)
      {
        num1 = (float) (((double) num6 - 240.0) * (double) num4 / 60.0) + num5;
        num2 = num5;
        num3 = b;
      }
      else if ((double) num6 <= 360.0)
      {
        num1 = b;
        num2 = num5;
        num3 = (float) (-((double) num6 - 360.0) * (double) num4 / 60.0) + num5;
      }
      else
      {
        num1 = 0.0f;
        num2 = 0.0f;
        num3 = 0.0f;
      }
    }
    return new Color(Mathf.Clamp01(num1), Mathf.Clamp01(num2), Mathf.Clamp01(num3), hsbColor.a);
  }

  public struct HSBColor
  {
    public float h;
    public float s;
    public float b;
    public float a;

    public HSBColor(float h, float s, float b, float a)
    {
      this.h = h;
      this.s = s;
      this.b = b;
      this.a = a;
    }
  }
}
