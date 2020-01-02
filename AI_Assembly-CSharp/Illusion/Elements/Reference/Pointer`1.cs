// Decompiled with JetBrains decompiler
// Type: Illusion.Elements.Reference.Pointer`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace Illusion.Elements.Reference
{
  public class Pointer<T>
  {
    private Func<T> get;
    private Action<T> set;

    public Pointer(Func<T> get, Action<T> set = null)
    {
      this.get = get;
      this.set = set;
    }

    public T value
    {
      get
      {
        return this.get.Call<T>(default (T));
      }
      set
      {
        this.set.Call<T>(value);
      }
    }
  }
}
