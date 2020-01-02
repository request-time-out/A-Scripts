// Decompiled with JetBrains decompiler
// Type: LoadAudioBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public abstract class LoadAudioBase : AssetLoader
{
  public int settingNo = -1;
  protected bool isReleaseClip = true;
  public float delayTime;
  public float fadeTime;

  public AudioClip clip { get; private set; }

  public AudioSource audioSource { get; private set; }

  public void Init(AudioSource audioSource)
  {
    this.audioSource = audioSource;
    ((Component) this).get_transform().SetParent(((Component) audioSource).get_transform(), false);
    this.Init();
  }

  [DebuggerHidden]
  public override IEnumerator _Init()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new LoadAudioBase.\u003C_Init\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  [DebuggerHidden]
  protected IEnumerator Play(GameObject fadeOut)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new LoadAudioBase.\u003CPlay\u003Ec__Iterator1()
    {
      fadeOut = fadeOut,
      \u0024this = this
    };
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (!this.isReleaseClip || !Singleton<Sound>.IsInstance())
      return;
    Singleton<Sound>.Instance.Remove(this.clip);
  }

  [Conditional("BASE_LOADER_LOG")]
  private void LogError(string str)
  {
    Debug.LogError((object) str);
  }
}
