// Decompiled with JetBrains decompiler
// Type: AIProject.DictionaryPool`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine.Events;

namespace AIProject
{
  public static class DictionaryPool<TKey, TValue>
  {
    private static readonly ObjectPool<Dictionary<TKey, TValue>> _dictionaryPool = new ObjectPool<Dictionary<TKey, TValue>>((UnityAction<Dictionary<TKey, TValue>>) null, new UnityAction<Dictionary<TKey, TValue>>((object) null, __methodptr(\u003C_dictionaryPool\u003Em__0)));

    public static Dictionary<TKey, TValue> Get()
    {
      return DictionaryPool<TKey, TValue>._dictionaryPool.Get();
    }

    public static void Release(Dictionary<TKey, TValue> toRelease)
    {
      DictionaryPool<TKey, TValue>._dictionaryPool.Release(toRelease);
    }
  }
}
