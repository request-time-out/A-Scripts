// Decompiled with JetBrains decompiler
// Type: ME_ColorHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public static class ME_ColorHelper
{
  private static string[] colorProperties = new string[10]
  {
    "_TintColor",
    "_Color",
    "_EmissionColor",
    "_BorderColor",
    "_ReflectColor",
    "_RimColor",
    "_MainColor",
    "_CoreColor",
    "_FresnelColor",
    "_CutoutColor"
  };
  private const float TOLERANCE = 0.0001f;

  public static ME_ColorHelper.HSBColor ColorToHSV(Color color)
  {
    ME_ColorHelper.HSBColor hsbColor = new ME_ColorHelper.HSBColor(0.0f, 0.0f, 0.0f, (float) color.a);
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
      hsbColor.H = (double) Math.Abs(g - num1) >= 9.99999974737875E-05 ? ((double) Math.Abs(b - num1) >= 9.99999974737875E-05 ? ((double) b <= (double) g ? (float) (((double) g - (double) b) / (double) num3 * 60.0) : (float) (((double) g - (double) b) / (double) num3 * 60.0 + 360.0)) : (float) (((double) r - (double) g) / (double) num3 * 60.0 + 240.0)) : (float) (((double) b - (double) r) / (double) num3 * 60.0 + 120.0);
      if ((double) hsbColor.H < 0.0)
        hsbColor.H += 360f;
    }
    else
      hsbColor.H = 0.0f;
    hsbColor.H *= 1f / 360f;
    hsbColor.S = (float) ((double) num3 / (double) num1 * 1.0);
    hsbColor.B = num1;
    return hsbColor;
  }

  public static Color HSVToColor(ME_ColorHelper.HSBColor hsbColor)
  {
    float num1 = hsbColor.B;
    float num2 = hsbColor.B;
    float num3 = hsbColor.B;
    if ((double) Math.Abs(hsbColor.S) > 9.99999974737875E-05)
    {
      float b = hsbColor.B;
      float num4 = hsbColor.B * hsbColor.S;
      float num5 = hsbColor.B - num4;
      float num6 = hsbColor.H * 360f;
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
    return new Color(Mathf.Clamp01(num1), Mathf.Clamp01(num2), Mathf.Clamp01(num3), hsbColor.A);
  }

  public static Color ConvertRGBColorByHUE(Color rgbColor, float hue)
  {
    float num = ME_ColorHelper.ColorToHSV(rgbColor).B;
    if ((double) num < 9.99999974737875E-05)
      num = 0.0001f;
    ME_ColorHelper.HSBColor hsv = ME_ColorHelper.ColorToHSV(Color.op_Division(rgbColor, num));
    hsv.H = hue;
    Color color = Color.op_Multiply(ME_ColorHelper.HSVToColor(hsv), num);
    color.a = rgbColor.a;
    return color;
  }

  public static void ChangeObjectColorByHUE(GameObject go, float hue)
  {
    foreach (Renderer componentsInChild in (Renderer[]) go.GetComponentsInChildren<Renderer>(true))
    {
      Material[] materialArray = Application.get_isPlaying() ? componentsInChild.get_materials() : componentsInChild.get_sharedMaterials();
      if (materialArray.Length != 0)
      {
        foreach (string colorProperty in ME_ColorHelper.colorProperties)
        {
          foreach (Material mat in materialArray)
          {
            if (Object.op_Inequality((Object) mat, (Object) null) && mat.HasProperty(colorProperty))
              ME_ColorHelper.setMatHUEColor(mat, colorProperty, hue);
          }
        }
      }
    }
    foreach (ParticleSystemRenderer componentsInChild in (ParticleSystemRenderer[]) go.GetComponentsInChildren<ParticleSystemRenderer>(true))
    {
      Material trailMaterial = componentsInChild.get_trailMaterial();
      if (!Object.op_Equality((Object) trailMaterial, (Object) null))
      {
        Material material = new Material(trailMaterial);
        ((Object) material).set_name(((Object) trailMaterial).get_name() + " (Instance)");
        Material mat = material;
        componentsInChild.set_trailMaterial(mat);
        foreach (string colorProperty in ME_ColorHelper.colorProperties)
        {
          if (Object.op_Inequality((Object) mat, (Object) null) && mat.HasProperty(colorProperty))
            ME_ColorHelper.setMatHUEColor(mat, colorProperty, hue);
        }
      }
    }
    foreach (SkinnedMeshRenderer componentsInChild in (SkinnedMeshRenderer[]) go.GetComponentsInChildren<SkinnedMeshRenderer>(true))
    {
      Material[] materialArray = Application.get_isPlaying() ? ((Renderer) componentsInChild).get_materials() : ((Renderer) componentsInChild).get_sharedMaterials();
      if (materialArray.Length != 0)
      {
        foreach (string colorProperty in ME_ColorHelper.colorProperties)
        {
          foreach (Material mat in materialArray)
          {
            if (Object.op_Inequality((Object) mat, (Object) null) && mat.HasProperty(colorProperty))
              ME_ColorHelper.setMatHUEColor(mat, colorProperty, hue);
          }
        }
      }
    }
    foreach (Projector componentsInChild in (Projector[]) go.GetComponentsInChildren<Projector>(true))
    {
      if (!((Object) componentsInChild.get_material()).get_name().EndsWith("(Instance)"))
      {
        Projector projector = componentsInChild;
        Material material1 = new Material(componentsInChild.get_material());
        ((Object) material1).set_name(((Object) componentsInChild.get_material()).get_name() + " (Instance)");
        Material material2 = material1;
        projector.set_material(material2);
      }
      Material material = componentsInChild.get_material();
      if (!Object.op_Equality((Object) material, (Object) null))
      {
        foreach (string colorProperty in ME_ColorHelper.colorProperties)
        {
          if (Object.op_Inequality((Object) material, (Object) null) && material.HasProperty(colorProperty))
            componentsInChild.set_material(ME_ColorHelper.setMatHUEColor(material, colorProperty, hue));
        }
      }
    }
    foreach (Light componentsInChild in (Light[]) go.GetComponentsInChildren<Light>(true))
    {
      ME_ColorHelper.HSBColor hsv = ME_ColorHelper.ColorToHSV(componentsInChild.get_color());
      hsv.H = hue;
      componentsInChild.set_color(ME_ColorHelper.HSVToColor(hsv));
    }
    foreach (ParticleSystem componentsInChild in (ParticleSystem[]) go.GetComponentsInChildren<ParticleSystem>(true))
    {
      ParticleSystem.MainModule main1 = componentsInChild.get_main();
      ParticleSystem.MainModule main2 = componentsInChild.get_main();
      ParticleSystem.MinMaxGradient startColor = ((ParticleSystem.MainModule) ref main2).get_startColor();
      ME_ColorHelper.HSBColor hsv = ME_ColorHelper.ColorToHSV(((ParticleSystem.MinMaxGradient) ref startColor).get_color());
      hsv.H = hue;
      ((ParticleSystem.MainModule) ref main1).set_startColor(ParticleSystem.MinMaxGradient.op_Implicit(ME_ColorHelper.HSVToColor(hsv)));
      ParticleSystem.ColorOverLifetimeModule colorOverLifetime = componentsInChild.get_colorOverLifetime();
      ParticleSystem.MinMaxGradient color1 = ((ParticleSystem.ColorOverLifetimeModule) ref colorOverLifetime).get_color();
      ParticleSystem.MinMaxGradient color2 = ((ParticleSystem.ColorOverLifetimeModule) ref colorOverLifetime).get_color();
      Gradient gradient = ((ParticleSystem.MinMaxGradient) ref color2).get_gradient();
      ParticleSystem.MinMaxGradient color3 = ((ParticleSystem.ColorOverLifetimeModule) ref colorOverLifetime).get_color();
      GradientColorKey[] colorKeys = ((ParticleSystem.MinMaxGradient) ref color3).get_gradient().get_colorKeys();
      hsv = ME_ColorHelper.ColorToHSV((Color) colorKeys[0].color);
      float num = Math.Abs(ME_ColorHelper.ColorToHSV((Color) colorKeys[1].color).H - hsv.H);
      hsv.H = hue;
      colorKeys[0].color = (__Null) ME_ColorHelper.HSVToColor(hsv);
      for (int index = 1; index < colorKeys.Length; ++index)
      {
        hsv = ME_ColorHelper.ColorToHSV((Color) colorKeys[index].color);
        hsv.H = Mathf.Repeat(hsv.H + num, 1f);
        colorKeys[index].color = (__Null) ME_ColorHelper.HSVToColor(hsv);
      }
      gradient.set_colorKeys(colorKeys);
      ((ParticleSystem.MinMaxGradient) ref color1).set_gradient(gradient);
      ((ParticleSystem.ColorOverLifetimeModule) ref colorOverLifetime).set_color(color1);
    }
  }

  private static Material setMatHUEColor(Material mat, string name, float hueColor)
  {
    Color color = ME_ColorHelper.ConvertRGBColorByHUE(mat.GetColor(name), hueColor);
    mat.SetColor(name, color);
    return mat;
  }

  private static Material setMatAlphaColor(Material mat, string name, float alpha)
  {
    Color color = mat.GetColor(name);
    color.a = (__Null) (double) alpha;
    mat.SetColor(name, color);
    return mat;
  }

  public struct HSBColor
  {
    public float H;
    public float S;
    public float B;
    public float A;

    public HSBColor(float h, float s, float b, float a)
    {
      this.H = h;
      this.S = s;
      this.B = b;
      this.A = a;
    }
  }
}
