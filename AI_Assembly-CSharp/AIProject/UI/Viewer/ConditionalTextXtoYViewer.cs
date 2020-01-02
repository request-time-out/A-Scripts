// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Viewer.ConditionalTextXtoYViewer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject.UI.Viewer
{
  [Serializable]
  public class ConditionalTextXtoYViewer
  {
    [SerializeField]
    private GameObject _layout;
    [SerializeField]
    private Text _xText;
    [SerializeField]
    private Text _yText;

    public bool isOver
    {
      get
      {
        return ((ReactiveProperty<int>) this._x).get_Value() >= ((ReactiveProperty<int>) this._y).get_Value();
      }
    }

    public bool isUnder
    {
      get
      {
        return ((ReactiveProperty<int>) this._x).get_Value() <= ((ReactiveProperty<int>) this._y).get_Value();
      }
    }

    public bool isGreater
    {
      get
      {
        return ((ReactiveProperty<int>) this._x).get_Value() > ((ReactiveProperty<int>) this._y).get_Value();
      }
    }

    public bool isLesser
    {
      get
      {
        return ((ReactiveProperty<int>) this._x).get_Value() < ((ReactiveProperty<int>) this._y).get_Value();
      }
    }

    public IObservable<int> X
    {
      get
      {
        return (IObservable<int>) this._x;
      }
    }

    public IObservable<int> Y
    {
      get
      {
        return (IObservable<int>) this._y;
      }
    }

    public int x
    {
      get
      {
        return ((ReactiveProperty<int>) this._x).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._x).set_Value(value);
      }
    }

    public int y
    {
      get
      {
        return ((ReactiveProperty<int>) this._y).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._y).set_Value(value);
      }
    }

    public bool visible
    {
      get
      {
        return this._layout.get_activeSelf();
      }
      set
      {
        this._layout.SetActive(value);
      }
    }

    public void VisibleUpdate()
    {
      if (!Object.op_Inequality((Object) this._layout, (Object) null) || !this.visible)
        return;
      this.visible = false;
      this.visible = true;
    }

    public void Refresh()
    {
      ((ReactiveProperty<int>) this._x).SetValueAndForceNotify(((ReactiveProperty<int>) this._x).get_Value());
    }

    public GameObject layout
    {
      get
      {
        return this._layout;
      }
    }

    public Text xText
    {
      get
      {
        return this._xText;
      }
    }

    public Text yText
    {
      get
      {
        return this._yText;
      }
    }

    public bool initialized { get; private set; }

    private IntReactiveProperty _x { get; } = new IntReactiveProperty();

    private IntReactiveProperty _y { get; } = new IntReactiveProperty();

    public void Initialize()
    {
      if (this.initialized)
        return;
      if (Object.op_Inequality((Object) this._yText, (Object) null))
      {
        DisposableExtensions.AddTo<IDisposable>((M0) UnityUIComponentExtensions.SubscribeToText<int>((IObservable<M0>) this._y, this._yText), (Component) this._yText);
        DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<int>((IObservable<M0>) this._y, (Action<M0>) (_ => this.Refresh())), (Component) this._yText);
      }
      if (Object.op_Inequality((Object) this._xText, (Object) null))
        DisposableExtensions.AddTo<IDisposable>((M0) UnityUIComponentExtensions.SubscribeToText<int>((IObservable<M0>) this._x, this._xText), (Component) this._xText);
      this.VisibleUpdate();
      this.initialized = true;
    }
  }
}
