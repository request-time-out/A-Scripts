// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.GroundInsect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject.Animal
{
  [RequireComponent(typeof (NavMeshController))]
  public abstract class GroundInsect : AnimalBase
  {
    [SerializeField]
    [Tooltip("ナビメッシュエージェント管理クラス")]
    private NavMeshController _navMeshCon;
    protected AnimalRootMotion rootMotion;
    protected CommandLabel.CommandInfo[] getLabels;

    public NavMeshController NavMeshCon
    {
      get
      {
        return this._navMeshCon;
      }
    }

    public override bool IsNeutralCommand
    {
      get
      {
        return true;
      }
    }

    public override CommandLabel.CommandInfo[] Labels
    {
      get
      {
        return this.getLabels;
      }
    }

    protected override void InitializeCommandLabels()
    {
      if (!((IReadOnlyList<CommandLabel.CommandInfo>) this.getLabels).IsNullOrEmpty<CommandLabel.CommandInfo>())
        return;
      CommonDefine commonDefine = !Singleton<Resources>.IsInstance() ? (CommonDefine) null : Singleton<Resources>.Instance.CommonDefine;
      CommonDefine.CommonIconGroup commonIconGroup = !Object.op_Inequality((Object) commonDefine, (Object) null) ? (CommonDefine.CommonIconGroup) null : commonDefine.Icon;
      this.getLabels = new CommandLabel.CommandInfo[1]
      {
        new CommandLabel.CommandInfo()
        {
          Text = "捕まえる",
          Transform = ((Component) this).get_transform(),
          IsHold = false,
          Icon = (Sprite) null,
          TargetSpriteInfo = commonIconGroup?.CharaSpriteInfo,
          Event = (Action) (() =>
          {
            StuffItemInfo itemInfo = this.ItemInfo;
            if (itemInfo != null)
            {
              StuffItem stuffItem = new StuffItem(itemInfo.CategoryID, itemInfo.ID, 1);
              Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList.AddItem(stuffItem);
              MapUIContainer.AddSystemItemLog(itemInfo, stuffItem.Count, true);
            }
            else
              MapUIContainer.AddNotify(MapUIContainer.ItemGetEmptyText);
            this.Destroy();
          })
        }
      };
    }

    protected override void Awake()
    {
      if (Object.op_Equality((Object) this._navMeshCon, (Object) null))
        this._navMeshCon = (NavMeshController) ((Component) this).GetComponent<NavMeshController>();
      if (Object.op_Equality((Object) this._navMeshCon, (Object) null))
        this._navMeshCon = (NavMeshController) ((Component) this).GetComponentInChildren<NavMeshController>(true);
      if (Object.op_Equality((Object) this._navMeshCon, (Object) null))
        this.Destroy();
      else
        base.Awake();
    }
  }
}
