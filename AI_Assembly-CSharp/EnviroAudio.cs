// Decompiled with JetBrains decompiler
// Type: EnviroAudio
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EnviroAudio
{
  [Header("Volume Settings:")]
  [Range(0.0f, 1f)]
  [Tooltip("The volume of ambient sounds played by enviro.")]
  public float ambientSFXVolume = 0.5f;
  [Range(0.0f, 1f)]
  [Tooltip("The volume of weather sounds played by enviro.")]
  public float weatherSFXVolume = 1f;
  [Tooltip("The prefab with AudioSources used by Enviro. Will be instantiated at runtime.")]
  public GameObject SFXHolderPrefab;
  [HideInInspector]
  public EnviroAudioSource currentAmbientSource;
  [HideInInspector]
  public float ambientSFXVolumeMod;
  [HideInInspector]
  public float weatherSFXVolumeMod;
}
