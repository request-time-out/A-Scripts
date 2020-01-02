// Decompiled with JetBrains decompiler
// Type: AIProject.ItemNodeShopUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;

namespace AIProject
{
  public class ItemNodeShopUI : ItemNodeUI
  {
    [SerializeField]
    [Header("Soldout")]
    private GameObject _soldout;

    public GameObject soldout
    {
      get
      {
        return this._soldout;
      }
    }

    protected override void Start()
    {
      base.Start();
      if (!Object.op_Inequality((Object) this._soldout, (Object) null))
        return;
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) Observable.Select<int, bool>((IObservable<M0>) this._stackCount, (Func<M0, M1>) (count => count <= 0)), (Action<M0>) (active =>
      {
        this._canvasGroup.set_blocksRaycasts(!active);
        this._soldout.SetActive(active);
      }));
    }
  }
}
