// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Viewer.CraftTagSelectionUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject.UI.Viewer
{
  public class CraftTagSelectionUI : MonoBehaviour
  {
    [SerializeField]
    private CraftGroupUI[] _group;
    private CraftTagSelectionUI.Element[] _elements;

    public CraftTagSelectionUI()
    {
      base.\u002Ector();
    }

    public Toggle this[int index]
    {
      get
      {
        return this.elements[index].toggle;
      }
    }

    public Action<int> Selection { get; set; }

    public CraftGroupUI[] group
    {
      get
      {
        return this._group;
      }
    }

    private CompositeDisposable disposable { get; }

    [DebuggerHidden]
    public IEnumerator<Toggle> GetEnumerator()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<Toggle>) new CraftTagSelectionUI.\u003CGetEnumerator\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private CraftTagSelectionUI.Element[] elements
    {
      get
      {
        return ((object) this).GetCache<CraftTagSelectionUI.Element[]>(ref this._elements, (Func<CraftTagSelectionUI.Element[]>) (() => ((IEnumerable<CraftGroupUI>) this._group).Select<CraftGroupUI, CraftTagSelectionUI.Element>((Func<CraftGroupUI, CraftTagSelectionUI.Element>) (x => new CraftTagSelectionUI.Element(x))).ToArray<CraftTagSelectionUI.Element>()));
      }
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CraftTagSelectionUI.\u003CStart\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void OnDestroy()
    {
      this.disposable.Clear();
      this.Selection = (Action<int>) null;
    }

    private class Element : TagSelection.ICursorTagElement
    {
      public Element(CraftGroupUI ui)
      {
        this.cursor = ui.cursor;
        this.selectable = (Selectable) ui.toggle;
        this.toggle = ui.toggle;
      }

      public Image cursor { get; }

      public Selectable selectable { get; }

      public Toggle toggle { get; }
    }
  }
}
