// Decompiled with JetBrains decompiler
// Type: SimpleFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class SimpleFade : MonoBehaviour
{
  public SimpleFade.Fade _Fade;
  public float _Time;
  public Color _Color;
  public Texture2D _Texture;
  protected float timer;
  private SimpleFade.FadeInOut fadeInOut;

  public SimpleFade()
  {
    base.\u002Ector();
  }

  public bool IsFadeNow
  {
    get
    {
      return this._Fade == SimpleFade.Fade.In || !this.IsEnd;
    }
  }

  public bool IsEnd
  {
    get
    {
      return (double) this.timer == (double) this._Time && this.fadeInOut == null;
    }
  }

  public void FadeSet(SimpleFade.Fade fade, float time = -1f, Texture2D tex = null)
  {
    this._Fade = fade;
    if ((double) time != -1.0)
      this._Time = time;
    if (Object.op_Inequality((Object) tex, (Object) null))
      this._Texture = tex;
    this.Init();
  }

  public void FadeInOutSet(SimpleFade.FadeInOut set, Texture2D tex = null)
  {
    if (Object.op_Inequality((Object) tex, (Object) null))
      this._Texture = tex;
    this.FadeInOutStart(set);
    this.Init();
  }

  public void Init()
  {
    this.timer = 0.0f;
    this._Color.a = this._Fade != SimpleFade.Fade.In ? (__Null) 1.0 : (__Null) 0.0;
    if (!Object.op_Equality((Object) this._Texture, (Object) null))
      return;
    this._Texture = Texture2D.get_whiteTexture();
  }

  public void ForceEnd()
  {
    this.timer = this._Time;
    this._Color.a = this._Fade != SimpleFade.Fade.In ? (__Null) 0.0 : (__Null) 1.0;
  }

  private void FadeInOutStart(SimpleFade.FadeInOut set)
  {
    if (set != null)
      this.fadeInOut = set;
    if (this.fadeInOut == null)
      return;
    this._Fade = set == null ? SimpleFade.Fade.Out : SimpleFade.Fade.In;
    this._Time = set == null ? this.fadeInOut.outTime : this.fadeInOut.inTime;
    this._Color = set == null ? this.fadeInOut.outColor : this.fadeInOut.inColor;
    this.fadeInOut = set;
    this.Init();
  }

  protected virtual void Awake()
  {
    this.ForceEnd();
  }

  protected virtual void Update()
  {
    float num1 = this._Fade != SimpleFade.Fade.In ? 1f : 0.0f;
    float num2 = this._Fade != SimpleFade.Fade.In ? 0.0f : 1f;
    this.timer = Mathf.Min(this.timer + Time.get_unscaledDeltaTime(), this._Time);
    if (this.fadeInOut != null && (double) this.timer == (double) this._Time && this.fadeInOut.Update())
      this.FadeInOutStart((SimpleFade.FadeInOut) null);
    else
      this._Color.a = (__Null) (double) Mathf.Lerp(num1, num2, Mathf.InverseLerp(0.0f, this._Time, this.timer));
  }

  protected virtual void OnGUI()
  {
    if (Object.op_Equality((Object) this._Texture, (Object) null))
      return;
    GUI.set_color(this._Color);
    GUI.DrawTexture(new Rect(0.0f, 0.0f, (float) Screen.get_width(), (float) Screen.get_height()), (Texture) this._Texture);
  }

  public enum Fade
  {
    In,
    Out,
  }

  public class FadeInOut
  {
    public float inTime = 1f;
    public float outTime = 1f;
    public Color inColor = Color.get_white();
    public Color outColor = Color.get_white();
    public float waitTime = 1f;
    private float timer;

    public FadeInOut()
    {
    }

    public FadeInOut(SimpleFade fade)
    {
      this.inTime = this.outTime = fade._Time;
      this.inColor = this.outColor = fade._Color;
    }

    public bool Update()
    {
      this.timer = Mathf.Min(this.timer + Time.get_unscaledDeltaTime(), this.waitTime);
      return (double) this.timer == (double) this.waitTime;
    }
  }
}
