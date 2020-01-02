// Decompiled with JetBrains decompiler
// Type: AIProject.CommandPointMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AIProject
{
  public class CommandPointMarker : UIBehaviour
  {
    [SerializeField]
    private RectTransform _locoAnimationTarget;
    [SerializeField]
    private Vector2 _from;
    [SerializeField]
    private Vector2 _to;
    private BoolReactiveProperty _isActivePanel;

    public CommandPointMarker()
    {
      base.\u002Ector();
    }

    public bool IsActivePanel
    {
      get
      {
        return ((ReactiveProperty<bool>) this._isActivePanel).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._isActivePanel).set_Value(value);
      }
    }

    protected virtual void Start()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._isActivePanel, (Action<M0>) (isOn => ((Component) this._locoAnimationTarget).get_gameObject().SetActive(isOn)));
      Observable.ToYieldInstruction<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.Locomotion()), false));
    }

    [DebuggerHidden]
    private IEnumerator Locomotion()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CommandPointMarker.\u003CLocomotion\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }
  }
}
