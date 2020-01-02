// Decompiled with JetBrains decompiler
// Type: CrossFadeObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class CrossFadeObject : MonoBehaviour
{
  public RenderTexture tex;
  private float span;
  private float delay;
  private float timer;
  private int depth;
  private float alpha;

  public CrossFadeObject()
  {
    base.\u002Ector();
  }

  private float CalcRate()
  {
    if ((double) this.span == 0.0)
      return 1f;
    return (double) this.timer < (double) this.delay ? 0.0f : Mathf.Clamp01((this.timer - this.delay) / this.span);
  }

  private void Awake()
  {
    this.alpha = 1f;
  }

  private void Update()
  {
    float num = this.CalcRate();
    if ((double) this.timer > (double) this.delay && (double) num >= 1.0)
    {
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }
    else
    {
      this.alpha = 1f - num;
      this.timer += Time.get_deltaTime();
    }
  }

  private void OnDestroy()
  {
    if (!Object.op_Implicit((Object) this.tex))
      return;
    this.tex.Release();
    Object.Destroy((Object) this.tex);
    this.tex = (RenderTexture) null;
  }

  private void OnGUI()
  {
    GUI.set_depth(this.depth);
    GUI.set_color(new Color(1f, 1f, 1f, this.alpha));
    if (Object.op_Equality((Object) this.tex, (Object) null))
      return;
    GUI.DrawTexture(new Rect(0.0f, 0.0f, (float) Screen.get_width(), (float) Screen.get_height()), (Texture) this.tex, (ScaleMode) 2, false);
  }
}
