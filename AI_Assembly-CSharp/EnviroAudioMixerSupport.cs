// Decompiled with JetBrains decompiler
// Type: EnviroAudioMixerSupport
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Audio;

[AddComponentMenu("Enviro/Utility/Audio Mixer Support")]
public class EnviroAudioMixerSupport : MonoBehaviour
{
  [Header("Mixer")]
  public AudioMixer audioMixer;
  [Header("Group Names")]
  public string ambientMixerGroup;
  public string weatherMixerGroup;
  public string thunderMixerGroup;

  public EnviroAudioMixerSupport()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (!Object.op_Inequality((Object) this.audioMixer, (Object) null) || !Object.op_Inequality((Object) EnviroSky.instance, (Object) null))
      return;
    this.StartCoroutine(this.Setup());
  }

  [DebuggerHidden]
  private IEnumerator Setup()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new EnviroAudioMixerSupport.\u003CSetup\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }
}
