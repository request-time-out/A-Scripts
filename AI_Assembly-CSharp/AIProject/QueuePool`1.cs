// Decompiled with JetBrains decompiler
// Type: AIProject.QueuePool`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine.Events;

namespace AIProject
{
  public static class QueuePool<T>
  {
    private static readonly ObjectPool<Queue<T>> _queuePool = new ObjectPool<Queue<T>>((UnityAction<Queue<T>>) null, new UnityAction<Queue<T>>((object) null, __methodptr(\u003C_queuePool\u003Em__0)));

    public static Queue<T> Get()
    {
      return QueuePool<T>._queuePool.Get();
    }

    public static void Release(Queue<T> toRelease)
    {
      QueuePool<T>._queuePool.Release(toRelease);
    }
  }
}
