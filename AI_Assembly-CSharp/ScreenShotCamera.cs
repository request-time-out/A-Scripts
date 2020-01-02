// Decompiled with JetBrains decompiler
// Type: ScreenShotCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class ScreenShotCamera : MonoBehaviour
{
  public ScreenShotCamera()
  {
    base.\u002Ector();
  }

  public RenderTexture renderTexture { get; private set; }

  [DebuggerHidden]
  private IEnumerator Start()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new ScreenShotCamera.\u003CStart\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  private void OnRenderImage(RenderTexture src, RenderTexture dst)
  {
    Graphics.Blit((Texture) src, this.renderTexture);
    Graphics.Blit((Texture) src, dst);
  }

  private void OnDestroy()
  {
    if (!Object.op_Implicit((Object) this.renderTexture))
      return;
    this.renderTexture.Release();
    this.renderTexture = (RenderTexture) null;
  }
}
