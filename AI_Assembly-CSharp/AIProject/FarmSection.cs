// Decompiled with JetBrains decompiler
// Type: AIProject.FarmSection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Scene;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  public class FarmSection : MonoBehaviour
  {
    private AIProject.SaveData.Environment.PlantInfo _plantInfo;
    private PlantItem _plantItem;

    public FarmSection()
    {
      base.\u002Ector();
    }

    public int HarvestID { get; set; }

    public int SectionID { get; set; }

    private void Start()
    {
      ObservableExtensions.Subscribe<long>(Observable.OnErrorRetry<long>(Observable.Do<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) this), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()), (Action<Exception>) (ex => Debug.LogException(ex)))), (Action<M0>) (_ => {}));
    }

    private void OnUpdate()
    {
      List<AIProject.SaveData.Environment.PlantInfo> source;
      if (!Singleton<Game>.Instance.Environment.FarmlandTable.TryGetValue(this.HarvestID, out source))
        return;
      AIProject.SaveData.Environment.PlantInfo element = source.GetElement<AIProject.SaveData.Environment.PlantInfo>(this.SectionID);
      if (element == null)
      {
        if (this._plantInfo == null)
          return;
        this._plantInfo = (AIProject.SaveData.Environment.PlantInfo) null;
        if (!Object.op_Inequality((Object) this._plantItem, (Object) null))
          return;
        Object.Destroy((Object) ((Component) this._plantItem).get_gameObject());
        this._plantItem = (PlantItem) null;
      }
      else
      {
        if (this._plantInfo != element)
        {
          this._plantInfo = element;
          StuffItemInfo seedInfo = Singleton<Resources>.Instance.GameInfo.FindItemInfo(element.nameHash);
          AssetBundleInfo assetBundleInfo;
          if (Singleton<Resources>.Instance.Map.PlantItemList.TryGetValue(Singleton<Resources>.Instance.Map.PlantIvyFilterList.Find((Predicate<ItemInfo>) (x => x.CategoryID == seedInfo.CategoryID && x.ItemID == seedInfo.ID)).ObjID, out assetBundleInfo))
          {
            GameObject gameObject = CommonLib.LoadAsset<GameObject>((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.asset, false, (string) assetBundleInfo.manifest);
            MapScene.AddAssetBundlePath((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.manifest);
            if (Object.op_Inequality((Object) gameObject, (Object) null))
            {
              if (Object.op_Inequality((Object) this._plantItem, (Object) null))
              {
                Object.Destroy((Object) ((Component) this._plantItem).get_gameObject());
                this._plantItem = (PlantItem) null;
              }
              this._plantItem = (PlantItem) ((GameObject) Object.Instantiate<GameObject>((M0) gameObject, ((Component) this).get_transform(), false)).GetComponent<PlantItem>();
              this._plantItem.ChangeState(0);
            }
          }
        }
        if (!Object.op_Inequality((Object) this._plantItem, (Object) null))
          return;
        float num = element.timer / (float) element.timeLimit;
        if ((double) num < 0.5)
          this._plantItem.ChangeState(0);
        else if ((double) num < 1.0)
          this._plantItem.ChangeState(1);
        else
          this._plantItem.ChangeState(2);
      }
    }
  }
}
