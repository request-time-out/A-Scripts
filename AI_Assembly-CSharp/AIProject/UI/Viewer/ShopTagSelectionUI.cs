// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Viewer.ShopTagSelectionUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject.UI.Viewer
{
  public class ShopTagSelectionUI : MonoBehaviour
  {
    [SerializeField]
    private ShopTagSelectionUI.Element[] _elements;

    public ShopTagSelectionUI()
    {
      base.\u002Ector();
    }

    public int Index
    {
      get
      {
        return Array.FindIndex<ShopTagSelectionUI.Element>(this._elements, (Predicate<ShopTagSelectionUI.Element>) (x => x.toggle.get_isOn()));
      }
    }

    public Toggle this[int index]
    {
      get
      {
        return this._elements[index].toggle;
      }
    }

    public Action<int> Selection { get; set; }

    private CompositeDisposable disposable { get; }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ShopTagSelectionUI.\u003CStart\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void OnDestroy()
    {
      this.disposable.Clear();
      this.Selection = (Action<int>) null;
    }

    [Serializable]
    private class Element : TagSelection.ICursorTagElement
    {
      [SerializeField]
      private Image _cursor;
      [SerializeField]
      private Toggle _toggle;

      public Image cursor
      {
        get
        {
          return this._cursor;
        }
      }

      public Selectable selectable
      {
        get
        {
          return (Selectable) this._toggle;
        }
      }

      public Toggle toggle
      {
        get
        {
          return this._toggle;
        }
      }
    }
  }
}
