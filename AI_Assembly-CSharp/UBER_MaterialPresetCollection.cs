// Decompiled with JetBrains decompiler
// Type: UBER_MaterialPresetCollection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class UBER_MaterialPresetCollection : ScriptableObject
{
  [SerializeField]
  [HideInInspector]
  public string currentPresetName;
  [SerializeField]
  [HideInInspector]
  public UBER_PresetParamSection whatToRestore;
  [SerializeField]
  [HideInInspector]
  public UBER_MaterialPreset[] matPresets;
  [SerializeField]
  [HideInInspector]
  public string[] names;

  public UBER_MaterialPresetCollection()
  {
    base.\u002Ector();
  }
}
