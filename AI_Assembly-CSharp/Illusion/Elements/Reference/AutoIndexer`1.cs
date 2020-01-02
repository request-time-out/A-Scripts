// Decompiled with JetBrains decompiler
// Type: Illusion.Elements.Reference.AutoIndexer`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Illusion.Elements.Reference
{
  public class AutoIndexer<T>
  {
    protected Dictionary<string, T> dic = new Dictionary<string, T>();
    protected T initializeValue;
    protected Func<T> initializeValueFunc;

    public AutoIndexer()
    {
      this.initializeValue = default (T);
    }

    public AutoIndexer(T initializeValue)
    {
      this.initializeValue = initializeValue;
    }

    public AutoIndexer(Func<T> initializeValueFunc)
    {
      this.initializeValueFunc = initializeValueFunc;
    }

    public T this[int index]
    {
      get
      {
        return this[index.ToString()];
      }
      set
      {
        this[index.ToString()] = value;
      }
    }

    public virtual T this[string key]
    {
      get
      {
        T obj1;
        if (!this.dic.TryGetValue(key, out obj1))
        {
          if (this.initializeValueFunc == null)
          {
            T initializeValue = this.initializeValue;
            this.dic[key] = initializeValue;
            obj1 = initializeValue;
          }
          else
          {
            T obj2 = this.initializeValueFunc();
            this.dic[key] = obj2;
            obj1 = obj2;
          }
        }
        return obj1;
      }
      set
      {
        this.dic[key] = value;
      }
    }

    public void Clear()
    {
      this.dic.Clear();
    }

    public Dictionary<string, T> Source
    {
      get
      {
        return this.dic;
      }
    }

    public Dictionary<string, T> ToStringDictionary()
    {
      return this.dic.ToDictionary<KeyValuePair<string, T>, string, T>((Func<KeyValuePair<string, T>, string>) (v => v.Key), (Func<KeyValuePair<string, T>, T>) (v => v.Value));
    }

    public Dictionary<int, T> ToIntDictionary()
    {
      return this.dic.ToDictionary<KeyValuePair<string, T>, int, T>((Func<KeyValuePair<string, T>, int>) (v => int.Parse(v.Key)), (Func<KeyValuePair<string, T>, T>) (v => v.Value));
    }
  }
}
