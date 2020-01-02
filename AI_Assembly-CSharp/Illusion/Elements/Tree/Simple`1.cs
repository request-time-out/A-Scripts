// Decompiled with JetBrains decompiler
// Type: Illusion.Elements.Tree.Simple`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Illusion.Elements.Tree
{
  public class Simple<T>
  {
    public Simple(T data, int level = 0)
    {
      this.level = level;
      this.data = data;
      this.children = new List<Simple<T>>();
    }

    public void RootAction(Action<Simple<T>> act)
    {
      for (Simple<T> simple = this; simple != null; simple = simple.parent)
        act(simple);
    }

    public bool RootCheck(Func<Simple<T>, bool> func)
    {
      for (Simple<T> simple = this; simple != null; simple = simple.parent)
      {
        if (func(simple))
          return true;
      }
      return false;
    }

    public int level { get; private set; }

    public Simple<T> parent { get; private set; }

    public List<Simple<T>> children { get; private set; }

    public T data { get; private set; }

    public Simple<T> AddChild(T child)
    {
      Simple<T> simple = new Simple<T>(child, this.level + 1);
      simple.parent = this;
      this.children.Add(simple);
      return simple;
    }

    public bool RemoveChild(T child)
    {
      return this.children.Remove(this.children.FirstOrDefault<Simple<T>>((Func<Simple<T>, bool>) (p => p.data.Equals((object) child))));
    }
  }
}
