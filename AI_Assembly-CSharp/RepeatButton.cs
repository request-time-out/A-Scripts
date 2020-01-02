// Decompiled with JetBrains decompiler
// Type: RepeatButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class RepeatButton : MonoBehaviour
{
  public float interval;
  private float nextClick;
  private bool isPressed;

  public RepeatButton()
  {
    base.\u002Ector();
  }

  public bool IsPress
  {
    get
    {
      return this.isPressed;
    }
  }

  private void OnPress(bool isPress)
  {
    this.isPressed = isPress;
    this.nextClick = Time.get_realtimeSinceStartup() + this.interval;
  }

  private void Update()
  {
    if (!this.isPressed || (double) Time.get_realtimeSinceStartup() >= (double) this.nextClick)
      return;
    this.nextClick = Time.get_realtimeSinceStartup() + this.interval;
  }
}
