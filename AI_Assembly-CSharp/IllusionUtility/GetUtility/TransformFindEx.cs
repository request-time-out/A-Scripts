// Decompiled with JetBrains decompiler
// Type: IllusionUtility.GetUtility.TransformFindEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IllusionUtility.GetUtility
{
  public static class TransformFindEx
  {
    public static GameObject FindLoop(this Transform transform, string name)
    {
      if (string.Compare(name, ((Object) ((Component) transform).get_gameObject()).get_name()) == 0)
        return ((Component) transform).get_gameObject();
      for (int index = 0; index < transform.get_childCount(); ++index)
      {
        GameObject loop = transform.GetChild(index).FindLoop(name);
        if (Object.op_Inequality((Object) null, (Object) loop))
          return loop;
      }
      return (GameObject) null;
    }

    public static void FindLoopPrefix(this Transform transform, List<GameObject> list, string name)
    {
      if (string.Compare(name, 0, ((Object) ((Component) transform).get_gameObject()).get_name(), 0, name.Length) == 0)
        list.Add(((Component) transform).get_gameObject());
      IEnumerator enumerator = transform.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          ((Transform) enumerator.Current).FindLoopPrefix(list, name);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }

    public static void FindLoopTag(this Transform transform, List<GameObject> list, string tag)
    {
      if (((Component) transform).get_gameObject().CompareTag(tag))
        list.Add(((Component) transform).get_gameObject());
      IEnumerator enumerator = transform.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          ((Transform) enumerator.Current).FindLoopTag(list, tag);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }

    public static void FindLoopAll(this Transform transform, List<GameObject> list)
    {
      list.Add(((Component) transform).get_gameObject());
      IEnumerator enumerator = transform.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          ((Transform) enumerator.Current).FindLoopAll(list);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }

    public static GameObject FindTop(this Transform transform)
    {
      return Object.op_Equality((Object) null, (Object) transform.get_parent()) ? ((Component) transform).get_gameObject() : transform.get_parent().FindTop();
    }

    public static GameObject[] FindRootObject(this Transform transform)
    {
      return Array.FindAll<GameObject>((GameObject[]) Object.FindObjectsOfType<GameObject>(), (Predicate<GameObject>) (item => Object.op_Equality((Object) item.get_transform().get_parent(), (Object) null)));
    }
  }
}
