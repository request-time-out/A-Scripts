// Decompiled with JetBrains decompiler
// Type: ADV.ADVUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.UI.Viewer;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ADV
{
  public class ADVUI : MonoBehaviour
  {
    [SerializeField]
    private CanvasGroup _cg;
    [SerializeField]
    private Toggle _skip;
    [SerializeField]
    private Toggle _auto;
    [SerializeField]
    private Button _log;
    [SerializeField]
    private Button _close;

    public ADVUI()
    {
      base.\u002Ector();
    }

    public bool useSE
    {
      set
      {
        this.playSE.use = value;
      }
    }

    public void PlaySE(SoundPack.SystemSE se)
    {
      this.playSE.Play(se);
    }

    public void Visible(bool visible)
    {
      this.isVisible = visible;
    }

    public Toggle skip
    {
      get
      {
        return this._skip;
      }
    }

    public Toggle auto
    {
      get
      {
        return this._auto;
      }
    }

    public Button log
    {
      get
      {
        return this._log;
      }
    }

    public Button close
    {
      get
      {
        return this._close;
      }
    }

    private PlaySE playSE { get; }

    private bool isVisible { get; set; }

    private BoolReactiveProperty visible { get; }

    private void Start()
    {
      if (!Object.op_Inequality((Object) this._cg, (Object) null))
        return;
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<bool>(Observable.TakeUntilDestroy<bool>((IObservable<M0>) this.visible, (Component) this._cg), (Action<M0>) (isOn =>
      {
        this._cg.set_alpha(!isOn ? 0.0f : 1f);
        this._cg.set_blocksRaycasts(isOn);
      })), (Component) this);
    }

    private void Update()
    {
      ((ReactiveProperty<bool>) this.visible).set_Value(this.isVisible && Manager.Config.GameData.TextWindowOption);
    }
  }
}
