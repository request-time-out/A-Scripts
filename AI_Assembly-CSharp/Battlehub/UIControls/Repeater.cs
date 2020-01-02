// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.Repeater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace Battlehub.UIControls
{
  public class Repeater
  {
    private float m_firstDelay;
    private float m_delay;
    private float m_nextT;
    private Action m_callback;

    public Repeater(float t, float initDelay, float firstDelay, float delay, Action callback)
    {
      this.m_nextT = t + initDelay;
      this.m_firstDelay = firstDelay;
      this.m_delay = delay;
      this.m_callback = callback;
    }

    public void Repeat(float t)
    {
      if ((double) t < (double) this.m_nextT)
        return;
      this.m_callback();
      if ((double) this.m_firstDelay > 0.0)
      {
        this.m_nextT += this.m_firstDelay;
        this.m_firstDelay = 0.0f;
      }
      else
        this.m_nextT += this.m_delay;
    }
  }
}
