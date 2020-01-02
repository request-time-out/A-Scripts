// Decompiled with JetBrains decompiler
// Type: GUITree.ObjectPool`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GUITree
{
  internal class ObjectPool<T> where T : new()
  {
    private readonly Stack<T> m_Stack = new Stack<T>();
    private readonly UnityAction<T> m_ActionOnGet;
    private readonly UnityAction<T> m_ActionOnRelease;

    public ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
    {
      this.m_ActionOnGet = actionOnGet;
      this.m_ActionOnRelease = actionOnRelease;
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
        return this.m_Stack.Count;
      }
    }

    public T Get()
    {
      T obj;
      if (this.m_Stack.Count == 0)
      {
        obj = new T();
        ++this.countAll;
      }
      else
        obj = this.m_Stack.Pop();
      if (this.m_ActionOnGet != null)
        this.m_ActionOnGet.Invoke(obj);
      return obj;
    }

    public void Release(T element)
    {
      if (this.m_Stack.Count > 0 && object.ReferenceEquals((object) this.m_Stack.Peek(), (object) element))
        Debug.LogError((object) "Internal error. Trying to destroy object that is already released to pool.");
      if (this.m_ActionOnRelease != null)
        this.m_ActionOnRelease.Invoke(element);
      this.m_Stack.Push(element);
    }
  }
}
