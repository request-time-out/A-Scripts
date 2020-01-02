// Decompiled with JetBrains decompiler
// Type: LoadSound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System.Collections;
using System.Diagnostics;

public class LoadSound : LoadAudioBase
{
  public Sound.Type type = Sound.Type.GameSE2D;
  public bool isAssetEqualPlay = true;

  [DebuggerHidden]
  public override IEnumerator _Init()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new LoadSound.\u003C_Init\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  [DebuggerHidden]
  protected override IEnumerator Start()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new LoadSound.\u003CStart\u003Ec__Iterator1()
    {
      \u0024this = this
    };
  }
}
