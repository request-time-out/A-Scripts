// Decompiled with JetBrains decompiler
// Type: PlayfulSystems.LoadingScreen.CanvasGroupFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace PlayfulSystems.LoadingScreen
{
  public class CanvasGroupFade : MonoBehaviour
  {
    private CanvasGroup group;

    public CanvasGroupFade()
    {
      base.\u002Ector();
    }

    public void FadeAlpha(float fromAlpha, float toAlpha, float duration)
    {
      if (Object.op_Equality((Object) this.group, (Object) null))
        this.group = (CanvasGroup) ((Component) this).GetComponent<CanvasGroup>();
      if (!Object.op_Inequality((Object) this.group, (Object) null))
        return;
      if ((double) duration > 0.0)
      {
        this.StopAllCoroutines();
        ((Component) this).get_gameObject().SetActive(true);
        this.StartCoroutine(this.DoFade(fromAlpha, toAlpha, duration));
      }
      else
      {
        this.group.set_alpha(toAlpha);
        ((Component) this).get_gameObject().SetActive((double) toAlpha > 0.0);
      }
    }

    [DebuggerHidden]
    private IEnumerator DoFade(float fromAlpha, float toAlpha, float duration)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CanvasGroupFade.\u003CDoFade\u003Ec__Iterator0()
      {
        duration = duration,
        fromAlpha = fromAlpha,
        toAlpha = toAlpha,
        \u0024this = this
      };
    }
  }
}
