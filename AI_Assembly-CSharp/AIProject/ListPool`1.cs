// Decompiled with JetBrains decompiler
// Type: AIProject.ListPool`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine.Events;

namespace AIProject
{
  public static class ListPool<T>
  {
    private static readonly ObjectPool<List<T>> _listPool = new ObjectPool<List<T>>((UnityAction<List<T>>) null, new UnityAction<List<T>>((object) null, __methodptr(\u003C_listPool\u003Em__0)));

    public static List<T> Get()
    {
      return ListPool<T>._listPool.Get();
    }

    public static void Release(List<T> toRelease)
    {
      ListPool<T>._listPool.Release(toRelease);
    }
  }
}
