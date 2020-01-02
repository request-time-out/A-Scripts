// Decompiled with JetBrains decompiler
// Type: AIProject.ObjectPool`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AIProject
{
  public class ObjectPool<T> where T : new()
  {
    private readonly Stack<T> _stack = new Stack<T>();
    private readonly UnityAction<T> _actionOnGet;
    private readonly UnityAction<T> _actionOnRelease;

    public ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
    {
      this._actionOnGet = actionOnGet;
      this._actionOnRelease = actionOnRelease;
    }

    public int countAll { get; private set; }

    public int countActive
    {
      get
      {
        return this.countAll - this.countInactive;
      }
    }

    public int countInactive
    {
      get
      {
        return this._stack.Count;
      }
    }

    public T Get()
    {
      T obj;
      if (this._stack.Count == 0)
      {
        obj = new T();
        ++this.countAll;
      }
      else
        obj = this._stack.Pop();
      if (this._actionOnGet != null)
        this._actionOnGet.Invoke(obj);
      return obj;
    }

    public void Release(T element)
    {
      if (this._stack.Count > 0 && object.ReferenceEquals((object) this._stack.Peek(), (object) element))
        Debug.LogError((object) "Internal error. Trying to destroy object that is already released to pool.");
      if (this._actionOnRelease != null)
        this._actionOnRelease.Invoke(element);
      this._stack.Push(element);
    }
  }
}
