// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.RepeatIntervalButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace UnityEngine.UI
{
  public abstract class RepeatIntervalButton : RepeatButton
  {
    [SerializeField]
    private float interval = 0.5f;
    private float timer;

    protected bool isOn { get; private set; }

    protected override void Process(bool push)
    {
      if (push)
      {
        this.isOn = (double) this.timer == 0.0 || (double) this.timer == (double) this.interval;
        this.timer += Time.get_deltaTime();
        this.timer = Mathf.Min(this.timer, this.interval);
      }
      else
      {
        this.isOn = false;
        this.timer = 0.0f;
      }
      if (this.IsSelect)
        return;
      this.isOn = false;
    }
  }
}
