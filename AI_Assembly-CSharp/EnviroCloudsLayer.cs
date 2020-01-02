// Decompiled with JetBrains decompiler
// Type: EnviroCloudsLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EnviroCloudsLayer
{
  [HideInInspector]
  public float DirectLightIntensity = 10f;
  [HideInInspector]
  [Tooltip("Base color of clouds.")]
  public Color FirstColor = Color.get_white();
  [HideInInspector]
  [Tooltip("Clouds detail normal power modificator.")]
  public float DetailPower = 2f;
  [HideInInspector]
  public GameObject myObj;
  [HideInInspector]
  public Material myMaterial;
  [HideInInspector]
  public Material myShadowMaterial;
  [HideInInspector]
  [Tooltip("Coverage rate of clouds generated.")]
  public float Coverage;
  [HideInInspector]
  [Tooltip("Density of clouds generated.")]
  public float Density;
  [HideInInspector]
  [Tooltip("Clouds alpha modificator.")]
  public float Alpha;
}
