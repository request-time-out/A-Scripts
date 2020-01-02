// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Recycling.ToggleElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject.UI.Recycling
{
  [Serializable]
  public class ToggleElement
  {
    [SerializeField]
    private Image _cursor;
    [SerializeField]
    private Toggle _toggle;

    public Image Cursor
    {
      get
      {
        return this._cursor;
      }
    }

    public Toggle Toggle
    {
      get
      {
        return this._toggle;
      }
    }

    public int Index { get; set; } = -1;

    public void Start()
    {
      this.Refresh();
      if (Object.op_Equality((Object) this._toggle, (Object) null) || Object.op_Equality((Object) this._cursor, (Object) null))
        return;
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<PointerEventData>(Observable.Where<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._toggle), (Func<M0, bool>) (_ => Object.op_Inequality((Object) this._cursor, (Object) null))), (Action<M0>) (_ => ((Behaviour) this._cursor).set_enabled(true))), (Component) this._toggle);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<PointerEventData>(Observable.Where<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) this._toggle), (Func<M0, bool>) (_ => Object.op_Inequality((Object) this._cursor, (Object) null))), (Action<M0>) (_ => ((Behaviour) this._cursor).set_enabled(false))), (Component) this._toggle);
    }

    public void Refresh()
    {
      if (!Object.op_Inequality((Object) this._cursor, (Object) null))
        return;
      ((Behaviour) this._cursor).set_enabled(false);
    }
  }
}
