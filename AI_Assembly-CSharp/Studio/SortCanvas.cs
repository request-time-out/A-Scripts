// Decompiled with JetBrains decompiler
// Type: Studio.SortCanvas
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Studio
{
  public class SortCanvas : Singleton<SortCanvas>
  {
    [SerializeField]
    private Canvas[] canvas;

    public static Canvas select
    {
      set
      {
        if (!Singleton<SortCanvas>.IsInstance())
          return;
        Singleton<SortCanvas>.Instance.OnSelect(value);
      }
    }

    public void OnSelect(Canvas _canvas)
    {
      if (Object.op_Equality((Object) _canvas, (Object) null))
        return;
      SortedList<int, Canvas> sortedList = new SortedList<int, Canvas>();
      _canvas.set_sortingOrder(10);
      for (int index = 0; index < this.canvas.Length; ++index)
        sortedList.Add(this.canvas[index].get_sortingOrder(), this.canvas[index]);
      // ISSUE: object of a compiler-generated type is created
      using (IEnumerator<\u003C\u003E__AnonType35<Canvas, int>> enumerator = ((IEnumerable<KeyValuePair<int, Canvas>>) sortedList).Select<KeyValuePair<int, Canvas>, \u003C\u003E__AnonType35<Canvas, int>>((Func<KeyValuePair<int, Canvas>, int, \u003C\u003E__AnonType35<Canvas, int>>) ((l, i) => new \u003C\u003E__AnonType35<Canvas, int>(l.Value, i))).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          // ISSUE: variable of a compiler-generated type
          \u003C\u003E__AnonType35<Canvas, int> current = enumerator.Current;
          current.Value.set_sortingOrder(current.i);
        }
      }
    }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
    }
  }
}
