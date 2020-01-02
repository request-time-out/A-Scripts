// Decompiled with JetBrains decompiler
// Type: AIProject.MapItemNull
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Scene;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  public class MapItemNull : ActionPointComponentBase
  {
    private List<GameObject> _itemObjs = new List<GameObject>();
    [SerializeField]
    private Transform _pivot;
    private float _elapsedTime;
    private bool _active;

    protected override void OnStart()
    {
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
    }

    public void SetActiveObjs(bool active, List<int> items = null)
    {
      if (active)
      {
        if (!this._itemObjs.IsNullOrEmpty<GameObject>())
        {
          using (List<GameObject>.Enumerator enumerator = this._itemObjs.GetEnumerator())
          {
            while (enumerator.MoveNext())
              Object.Destroy((Object) enumerator.Current);
          }
          this._itemObjs.Clear();
        }
        foreach (int key in items)
        {
          ActionItemInfo info;
          if (Singleton<Resources>.Instance.Map.EventItemList.TryGetValue(key, out info))
          {
            GameObject gameObject1 = CommonLib.LoadAsset<GameObject>((string) info.assetbundleInfo.assetbundle, (string) info.assetbundleInfo.asset, false, (string) info.assetbundleInfo.manifest);
            if (!MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == (string) info.assetbundleInfo.assetbundle && (string) x.Item2 == (string) info.assetbundleInfo.manifest)))
              MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>((string) info.assetbundleInfo.assetbundle, (string) info.assetbundleInfo.asset));
            if (Object.op_Inequality((Object) gameObject1, (Object) null))
            {
              GameObject gameObject2 = (GameObject) Object.Instantiate<GameObject>((M0) gameObject1, this._pivot != null ? ((Component) this._pivot).get_transform() : (Transform) null, false);
              gameObject2.get_transform().set_localPosition(Vector3.get_zero());
              gameObject2.get_transform().set_localRotation(Quaternion.get_identity());
              gameObject2.get_transform().set_localScale(Vector3.get_one());
              this._itemObjs.Add(gameObject2);
            }
            else
            {
              AssetBundleInfo assetbundleInfo = info.assetbundleInfo;
              Debug.LogError((object) string.Format("イベントアイテム読み込み失敗: バンドルパス[{0}] プレハブ[{1}] マニフェスト[{2}]", (object) assetbundleInfo.assetbundle, (object) assetbundleInfo.asset, (object) assetbundleInfo.manifest));
            }
          }
        }
      }
      else
      {
        if (this._itemObjs.IsNullOrEmpty<GameObject>())
          return;
        using (List<GameObject>.Enumerator enumerator = this._itemObjs.GetEnumerator())
        {
          while (enumerator.MoveNext())
            Object.Destroy((Object) enumerator.Current);
        }
        this._itemObjs.Clear();
      }
      this._active = !this._itemObjs.IsNullOrEmpty<GameObject>() && active;
    }

    private void OnUpdate()
    {
      if (this._active)
      {
        this._elapsedTime += Time.get_unscaledDeltaTime();
        if ((double) this._elapsedTime <= (double) Singleton<Manager.Map>.Instance.EnvironmentProfile.RuntimeMapItemLifeTime)
          return;
        this.SetActiveObjs(false, (List<int>) null);
      }
      else
        this._elapsedTime = 0.0f;
    }
  }
}
