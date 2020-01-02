// Decompiled with JetBrains decompiler
// Type: RenderCrossFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class RenderCrossFade : BaseRenderCrossFade
{
  private bool isSubAlpha;

  public bool IsEnd
  {
    get
    {
      return this.state == RenderCrossFade.State.None;
    }
  }

  public RenderCrossFade.State state { get; set; }

  public bool isMyUpdateCap { get; set; }

  public void SubAlphaStart()
  {
    this.isSubAlpha = true;
  }

  public void Set()
  {
    this.state = RenderCrossFade.State.Ready;
    this.isSubAlpha = false;
  }

  public override void End()
  {
    base.End();
    this.state = RenderCrossFade.State.None;
  }

  protected override void Awake()
  {
    base.Awake();
    this.isInitRenderSetting = false;
    this.state = RenderCrossFade.State.None;
    this.isMyUpdateCap = true;
    this.isAlphaAdd = false;
    this.alpha = 0.0f;
    this.timer = 0.0f;
  }

  protected override void Update()
  {
    if (!this.isMyUpdateCap)
      return;
    this.UpdateCalc();
  }

  public void UpdateDrawer()
  {
    if (this.isMyUpdateCap)
      return;
    this.UpdateCalc();
  }

  private void UpdateCalc()
  {
    if (this.state == RenderCrossFade.State.Ready)
    {
      this.Capture();
      this.state = RenderCrossFade.State.Draw;
    }
    if (this.state != RenderCrossFade.State.Draw || !this.isSubAlpha)
      return;
    this.timer += Time.get_deltaTime();
    this.AlphaCalc();
    if ((double) this.timer <= (double) this.maxTime)
      return;
    this.state = RenderCrossFade.State.None;
  }

  protected override void OnGUI()
  {
    if (this.state != RenderCrossFade.State.Draw)
      return;
    GUI.set_depth(10);
    GUI.set_color(new Color(1f, 1f, 1f, this.alpha));
    GUI.DrawTexture(new Rect(0.0f, 0.0f, (float) Screen.get_width(), (float) Screen.get_height()), (Texture) this.texture);
    this.isDrawGUI = true;
  }

  public enum State
  {
    None,
    Ready,
    Draw,
  }
}
