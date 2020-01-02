// Decompiled with JetBrains decompiler
// Type: EnviroComponents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EnviroComponents
{
  [Tooltip("The Enviro sun object.")]
  public GameObject Sun;
  [Tooltip("The Enviro moon object.")]
  public GameObject Moon;
  [Tooltip("The directional light for direct sun and moon lighting.")]
  public Transform DirectLight;
  [Tooltip("The Enviro global reflection probe for dynamic reflections.")]
  public ReflectionProbe GlobalReflectionProbe;
  [Tooltip("Your WindZone that reflect our weather wind settings.")]
  public WindZone windZone;
  [Tooltip("The Enviro Lighting Flash Component.")]
  public EnviroLightning LightningGenerator;
  [Tooltip("Link to the object that hold all additional satellites as childs.")]
  public Transform satellites;
  [Tooltip("Just a transform for stars rotation calculations. ")]
  public Transform starsRotation;
  [Tooltip("Plane to cast cloud shadows.")]
  public GameObject cloudsShadowPlane;
}
