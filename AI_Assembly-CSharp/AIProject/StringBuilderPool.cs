// Decompiled with JetBrains decompiler
// Type: AIProject.StringBuilderPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Text;
using UnityEngine.Events;

namespace AIProject
{
  public class StringBuilderPool
  {
    private static readonly ObjectPool<StringBuilder> _pool = new ObjectPool<StringBuilder>((UnityAction<StringBuilder>) null, new UnityAction<StringBuilder>((object) null, __methodptr(\u003C_pool\u003Em__0)));

    public static StringBuilder Get()
    {
      return StringBuilderPool._pool.Get();
    }

    public static void Release(StringBuilder toRelease)
    {
      StringBuilderPool._pool.Release(toRelease);
    }
  }
}
