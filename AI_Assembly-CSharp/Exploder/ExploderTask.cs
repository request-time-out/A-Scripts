// Decompiled with JetBrains decompiler
// Type: Exploder.ExploderTask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Diagnostics;

namespace Exploder
{
  internal abstract class ExploderTask
  {
    protected Core core;

    protected ExploderTask(Core Core)
    {
      this.core = Core;
      this.Watch = Stopwatch.StartNew();
    }

    public abstract TaskType Type { get; }

    public Stopwatch Watch { get; private set; }

    public virtual void OnDestroy()
    {
    }

    public virtual void Init()
    {
      this.Watch.Reset();
      this.Watch.Start();
    }

    public abstract bool Run(float frameBudget);
  }
}
