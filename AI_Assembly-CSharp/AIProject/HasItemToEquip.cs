// Decompiled with JetBrains decompiler
// Type: AIProject.HasItemToEquip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class HasItemToEquip : AgentConditional
  {
    [SerializeField]
    private int _itemID;

    public virtual TaskStatus OnUpdate()
    {
      foreach (StuffItem stuffItem in this.Agent.AgentData.ItemList)
      {
        StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(stuffItem.CategoryID, stuffItem.ID);
        if (stuffItemInfo != null && stuffItemInfo.EquipableState >= ItemEquipableState.Heroine && this._itemID == stuffItemInfo.ID)
          return (TaskStatus) 2;
      }
      return (TaskStatus) 1;
    }
  }
}
