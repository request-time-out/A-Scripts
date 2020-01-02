// Decompiled with JetBrains decompiler
// Type: ConfigScene.BaseSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ConfigScene
{
  public abstract class BaseSetting : MonoBehaviour
  {
    private bool _isPlaySE;

    protected BaseSetting()
    {
      base.\u002Ector();
    }

    public bool isPlaySE
    {
      get
      {
        return this._isPlaySE;
      }
      set
      {
        this._isPlaySE = value;
      }
    }

    protected void EnterSE()
    {
      if (!this._isPlaySE)
        return;
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
    }

    protected void LinkToggle(Toggle toggle, Action<bool> act)
    {
      ObservableExtensions.Subscribe<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) toggle.onValueChanged), (Action<M0>) (isOn =>
      {
        this.EnterSE();
        act(isOn);
      }));
    }

    protected void LinkToggleArray(Toggle[] _tgls, Action<int> _action)
    {
      Toggle[] toggleArray = _tgls;
      // ISSUE: reference to a compiler-generated field
      if (BaseSetting.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        BaseSetting.\u003C\u003Ef__mg\u0024cache0 = new Func<Toggle, IObservable<bool>>(UnityUIComponentExtensions.OnValueChangedAsObservable);
      }
      // ISSUE: reference to a compiler-generated field
      Func<Toggle, IObservable<bool>> fMgCache0 = BaseSetting.\u003C\u003Ef__mg\u0024cache0;
      ObservableExtensions.Subscribe<int>(Observable.Skip<int>((IObservable<M0>) ReactivePropertyExtensions.ToReadOnlyReactiveProperty<int>(Observable.Where<int>((IObservable<M0>) Observable.Select<IList<bool>, int>((IObservable<M0>) Observable.CombineLatest<bool>((IEnumerable<IObservable<M0>>) ((IEnumerable<Toggle>) toggleArray).Select<Toggle, IObservable<bool>>(fMgCache0)), (Func<M0, M1>) (list => list.IndexOf(true))), (Func<M0, bool>) (i => i >= 0))), 1), (Action<M0>) (i =>
      {
        Action<int> action = _action;
        if (action != null)
          action(i);
        this.EnterSE();
      }));
    }

    protected void LinkSlider(Slider slider, Action<float> act)
    {
      ObservableExtensions.Subscribe<float>(UnityEventExtensions.AsObservable<float>((UnityEvent<M0>) slider.get_onValueChanged()), (Action<M0>) (value => act(value)));
      ObservableExtensions.Subscribe<PointerEventData>(Observable.Where<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerDownAsObservable((UIBehaviour) slider), (Func<M0, bool>) (_ => Input.GetMouseButtonDown(0))), (Action<M0>) (_ => this.EnterSE()));
    }

    protected void LinkTmpDropdown(TMP_Dropdown dropdown, Action<float> act)
    {
      ObservableExtensions.Subscribe<int>(UnityEventExtensions.AsObservable<int>((UnityEvent<M0>) dropdown.get_onValueChanged()), (Action<M0>) (value => act((float) value)));
      ObservableExtensions.Subscribe<PointerEventData>(Observable.Where<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerDownAsObservable((UIBehaviour) dropdown), (Func<M0, bool>) (_ => Input.GetMouseButtonDown(0))), (Action<M0>) (_ => this.EnterSE()));
    }

    protected void SetToggleUIArray(Toggle[] _toggles, Action<Toggle, int> _action)
    {
      // ISSUE: object of a compiler-generated type is created
      using (IEnumerator<\u003C\u003E__AnonType9<Toggle, int>> enumerator = ((IEnumerable<Toggle>) _toggles).Select<Toggle, \u003C\u003E__AnonType9<Toggle, int>>((Func<Toggle, int, \u003C\u003E__AnonType9<Toggle, int>>) ((tgl, index) => new \u003C\u003E__AnonType9<Toggle, int>(tgl, index))).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          // ISSUE: variable of a compiler-generated type
          \u003C\u003E__AnonType9<Toggle, int> current = enumerator.Current;
          _action(current.tgl, current.index);
        }
      }
    }

    public abstract void Init();

    protected abstract void ValueToUI();

    public void UIPresenter()
    {
      bool isPlaySe = this._isPlaySE;
      this._isPlaySE = false;
      this.ValueToUI();
      this._isPlaySE = isPlaySe;
    }
  }
}
