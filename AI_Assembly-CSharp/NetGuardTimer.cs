// Decompiled with JetBrains decompiler
// Type: NetGuardTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Timers;

public class NetGuardTimer
{
  public const int TimeoutInMilliseconds = 3000;
  private Timer _timer;

  public event SysPost.StdMulticastDelegation Timeout;

  public void Activate()
  {
    this._timer = new Timer(3000.0);
    this._timer.Elapsed += new ElapsedEventHandler(this.OnTimeout);
    this._timer.Start();
  }

  public void Deactivate()
  {
    if (this._timer == null)
      return;
    this._timer.Stop();
    this._timer = (Timer) null;
  }

  private void OnTimeout(object sender, ElapsedEventArgs e)
  {
    SysPost.InvokeMulticast((object) this, (MulticastDelegate) this.Timeout);
  }
}
