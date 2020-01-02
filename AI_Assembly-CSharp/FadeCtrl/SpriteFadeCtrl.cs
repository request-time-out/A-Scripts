// Decompiled with JetBrains decompiler
// Type: FadeCtrl.SpriteFadeCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace FadeCtrl
{
  public class SpriteFadeCtrl : MonoBehaviour
  {
    [SerializeField]
    private RawImage imageFade;
    [SerializeField]
    private float timeFadeBase;
    [SerializeField]
    private AnimationCurve fadeAnimation;
    private SpriteFadeCtrl.FadeKind kindFade;
    private SpriteFadeCtrl.FadeKindProc kindFadeProc;
    private float timeFade;
    [SerializeField]
    private float timeFadeTime;

    public SpriteFadeCtrl()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      if (!Object.op_Implicit((Object) this.imageFade))
        return;
      ((Behaviour) this.imageFade).set_enabled(false);
    }

    public void SetColor(Color _color)
    {
      if (!Object.op_Implicit((Object) this.imageFade))
        return;
      ((Graphic) this.imageFade).set_color(_color);
    }

    public void FadeStart(SpriteFadeCtrl.FadeKind _kind, float _timeFade = -1f)
    {
      this.timeFadeTime = 0.0f;
      ((Behaviour) this.imageFade).set_enabled(true);
      this.timeFade = (double) _timeFade >= 0.0 ? (_kind == SpriteFadeCtrl.FadeKind.OutIn ? _timeFade * 2f : _timeFade) : (_kind == SpriteFadeCtrl.FadeKind.OutIn ? this.timeFadeBase * 2f : this.timeFadeBase);
      this.kindFade = _kind;
      switch (this.kindFade)
      {
        case SpriteFadeCtrl.FadeKind.Out:
          this.kindFadeProc = SpriteFadeCtrl.FadeKindProc.Out;
          break;
        case SpriteFadeCtrl.FadeKind.In:
          this.kindFadeProc = SpriteFadeCtrl.FadeKindProc.In;
          break;
        case SpriteFadeCtrl.FadeKind.OutIn:
          this.kindFadeProc = SpriteFadeCtrl.FadeKindProc.OutIn;
          break;
      }
    }

    public SpriteFadeCtrl.FadeKindProc GetFadeKindProc()
    {
      return this.kindFadeProc;
    }

    public bool IsFade()
    {
      return this.kindFadeProc == SpriteFadeCtrl.FadeKindProc.In || this.kindFadeProc == SpriteFadeCtrl.FadeKindProc.Out || this.kindFadeProc == SpriteFadeCtrl.FadeKindProc.OutIn;
    }

    [DebuggerHidden]
    public IEnumerator FadeProc()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new SpriteFadeCtrl.\u003CFadeProc\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public enum FadeKind
    {
      Out,
      In,
      OutIn,
    }

    public enum FadeKindProc
    {
      None,
      Out,
      OutEnd,
      In,
      InEnd,
      OutIn,
      OutInEnd,
    }
  }
}
