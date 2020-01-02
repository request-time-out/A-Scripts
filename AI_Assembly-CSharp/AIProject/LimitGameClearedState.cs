// Decompiled with JetBrains decompiler
// Type: AIProject.LimitGameClearedState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Manager;
using System;
using UniRx;
using UnityEngine;

namespace AIProject
{
  public class LimitGameClearedState : MonoBehaviour
  {
    [SerializeField]
    private bool _cleared;
    private bool _prevCleared;

    public LimitGameClearedState()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.Refresh(true);
      ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) this), (Action<M0>) (_ => this.Refresh(false)));
    }

    private void Refresh(bool first = false)
    {
      bool flag = false;
      if (Singleton<Game>.IsInstance())
      {
        WorldData worldData = Singleton<Game>.Instance.WorldData;
        if (worldData != null)
          flag = worldData.Cleared;
      }
      if (!flag)
      {
        if (((Component) this).get_gameObject().get_activeSelf())
          ((Component) this).get_gameObject().SetActive(false);
      }
      else if ((flag && !this._prevCleared || flag && first) && !((Component) this).get_gameObject().get_activeSelf())
        ((Component) this).get_gameObject().SetActive(true);
      this._prevCleared = flag;
    }
  }
}
