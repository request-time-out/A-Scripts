// Decompiled with JetBrains decompiler
// Type: AIProject.VoiceSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using UnityEngine;

namespace AIProject
{
  public class VoiceSettings
  {
    public string AssetBundleName { get; set; }

    public string AssetName { get; set; }

    public Voice.Type Type { get; set; }

    public int No { get; set; }

    public float Pitch { get; set; }

    public Transform VoiceTransform { get; set; }

    public float Delaytime { get; set; }

    public float FadeTime { get; set; }

    public bool IsAsync { get; set; }

    public int SettingNo { get; set; } = -1;

    public bool IsPlayEndDelete { get; set; } = true;

    public bool IsBundleUnload { get; set; }

    public bool Is2D { get; set; }
  }
}
