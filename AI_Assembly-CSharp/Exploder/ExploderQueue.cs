// Decompiled with JetBrains decompiler
// Type: Exploder.ExploderQueue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Exploder
{
  internal class ExploderQueue
  {
    private readonly Queue<ExploderParams> queue;
    private readonly Core core;

    public ExploderQueue(Core core)
    {
      this.core = core;
      this.queue = new Queue<ExploderParams>();
    }

    public void Enqueue(
      ExploderObject exploderObject,
      ExploderObject.OnExplosion callback,
      bool crack,
      params GameObject[] target)
    {
      this.queue.Enqueue(new ExploderParams(exploderObject)
      {
        Callback = callback,
        Targets = target,
        Crack = crack,
        processing = false
      });
      this.ProcessQueue();
    }

    private void ProcessQueue()
    {
      if (this.queue.Count <= 0)
        return;
      ExploderParams p = this.queue.Peek();
      if (p.processing)
        return;
      p.id = Random.Range(int.MinValue, int.MaxValue);
      p.processing = true;
      this.core.StartExplosionFromQueue(p);
    }

    public void OnExplosionFinished(int id, long ellapsedMS)
    {
      ExploderParams exploderParams = this.queue.Dequeue();
      if (exploderParams.Callback != null)
        exploderParams.Callback((float) ellapsedMS, !exploderParams.Crack ? ExploderObject.ExplosionState.ExplosionFinished : ExploderObject.ExplosionState.ObjectCracked);
      this.ProcessQueue();
    }
  }
}
