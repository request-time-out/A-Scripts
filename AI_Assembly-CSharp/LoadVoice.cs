// Decompiled with JetBrains decompiler
// Type: LoadVoice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class LoadVoice : LoadAudioBase
{
  public bool isPlayEndDelete = true;
  public float pitch = 1f;
  public int no;
  public Transform voiceTrans;
  public Manager.Voice.Type type;
  public bool is2D;

  [DebuggerHidden]
  public override IEnumerator _Init()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new LoadVoice.\u003C_Init\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  [DebuggerHidden]
  protected override IEnumerator Start()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new LoadVoice.\u003CStart\u003Ec__Iterator1()
    {
      \u0024this = this
    };
  }
}
