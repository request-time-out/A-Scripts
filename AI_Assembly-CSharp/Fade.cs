// Decompiled with JetBrains decompiler
// Type: Fade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using System;
using UnityEngine;

public class Fade : MonoBehaviour
{
  [SerializeField]
  private CanvasRenderer fadeRenderer;
  [SerializeField]
  private CanvasRenderer loadingRenderer;
  public float fadeWaitTime;
  public Fade.FadeIn fadeIn;
  public Fade.FadeOut fadeOut;
  private float fadeTimer;
  private float nowWaitTimer;
  private bool usingLoadingTex;

  public Fade()
  {
    base.\u002Ector();
  }

  public Fade.Type nowType { get; private set; }

  public bool isFadeNow { get; private set; }

  public bool FadeSet(Fade.Type type, bool _usingLoadingTex = true)
  {
    if (!Object.op_Implicit((Object) this.fadeRenderer))
    {
      Debug.Log((object) "fadeRenderer None");
      this.isFadeNow = false;
      return false;
    }
    if (!Object.op_Implicit((Object) this.loadingRenderer))
    {
      Debug.Log((object) "loadingRenderer None");
      this.isFadeNow = false;
      return false;
    }
    this.isFadeNow = true;
    this.usingLoadingTex = _usingLoadingTex;
    this.nowType = type;
    this.nowWaitTimer = 0.0f;
    this.fadeTimer = 0.0f;
    switch (type)
    {
      case Fade.Type.InOut:
      case Fade.Type.In:
        this.fadeRenderer.SetAlpha(this.fadeIn.start);
        this.loadingRenderer.SetAlpha(!this.usingLoadingTex ? 0.0f : this.fadeIn.start);
        break;
      case Fade.Type.Out:
        this.fadeRenderer.SetAlpha(this.fadeOut.start);
        this.loadingRenderer.SetAlpha(!this.usingLoadingTex ? 0.0f : this.fadeOut.start);
        break;
    }
    return true;
  }

  public void FadeEnd()
  {
    this.nowType = Fade.Type.Out;
    this.isFadeNow = false;
    if (Object.op_Implicit((Object) this.fadeRenderer))
      this.fadeRenderer.SetAlpha(0.0f);
    if (!Object.op_Implicit((Object) this.loadingRenderer))
      return;
    this.loadingRenderer.SetAlpha(0.0f);
  }

  public bool IsFadeIn()
  {
    return this.nowType == Fade.Type.In || this.nowType == Fade.Type.InOut;
  }

  public bool IsAlphaMax()
  {
    return (double) this.fadeRenderer.GetAlpha() >= 1.0;
  }

  public bool IsAlphaMin()
  {
    return (double) this.fadeRenderer.GetAlpha() <= 0.0;
  }

  public void SetColor(Color _color)
  {
    this.fadeRenderer.SetColor(_color);
  }

  private void Awake()
  {
    if (Object.op_Equality((Object) this.fadeRenderer, (Object) null))
    {
      GameObject loop = ((Component) this).get_transform().FindLoop(nameof (Fade));
      if (Object.op_Implicit((Object) loop))
        this.fadeRenderer = (CanvasRenderer) loop.GetComponent<CanvasRenderer>();
    }
    if (Object.op_Equality((Object) this.loadingRenderer, (Object) null))
    {
      GameObject loop = ((Component) this).get_transform().FindLoop("NowLoading");
      if (Object.op_Implicit((Object) loop))
        this.loadingRenderer = (CanvasRenderer) loop.GetComponent<CanvasRenderer>();
    }
    this.fadeRenderer.SetAlpha(0.0f);
    this.loadingRenderer.SetAlpha(0.0f);
    this.isFadeNow = false;
    this.nowType = Fade.Type.Out;
    this.nowWaitTimer = 0.0f;
  }

  private void Start()
  {
  }

  private void Update()
  {
    if (!this.isFadeNow)
      return;
    float num1 = Mathf.Clamp01(Time.get_unscaledDeltaTime());
    if (this.nowType == Fade.Type.In || this.nowType == Fade.Type.InOut)
    {
      this.fadeTimer += num1;
      float num2 = Mathf.Clamp01(Mathf.Lerp(this.fadeIn.start, this.fadeIn.end, Mathf.InverseLerp(0.0f, this.fadeIn.time, this.fadeTimer)));
      this.fadeRenderer.SetAlpha(num2);
      this.loadingRenderer.SetAlpha(!this.usingLoadingTex ? 0.0f : num2);
      if ((double) num2 != (double) this.fadeIn.end || this.nowType != Fade.Type.InOut)
        return;
      this.nowWaitTimer = Mathf.Min(this.nowWaitTimer + num1, this.fadeWaitTime);
      if ((double) this.nowWaitTimer < (double) this.fadeWaitTime)
        return;
      this.nowType = Fade.Type.Out;
      this.fadeTimer = 0.0f;
    }
    else
    {
      if (this.nowType != Fade.Type.Out)
        return;
      this.fadeTimer += num1;
      float num2 = Mathf.Clamp01(Mathf.Lerp(this.fadeOut.start, this.fadeOut.end, Mathf.InverseLerp(0.0f, this.fadeOut.time, this.fadeTimer)));
      this.fadeRenderer.SetAlpha(num2);
      this.loadingRenderer.SetAlpha(!this.usingLoadingTex ? 0.0f : num2);
      if ((double) num2 != (double) this.fadeOut.end)
        return;
      this.FadeEnd();
    }
  }

  public enum Type
  {
    InOut,
    In,
    Out,
  }

  [Serializable]
  public class FadeIn
  {
    public float end = 1f;
    public float time = 2f;
    public float start;
  }

  [Serializable]
  public class FadeOut
  {
    public float start = 1f;
    public float time = 2f;
    public float end;
  }
}
