// Decompiled with JetBrains decompiler
// Type: AIProject.StartCarryFood
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class StartCarryFood : AgentAction
  {
    [SerializeField]
    private bool _selectCanStandFood;

    public virtual TaskStatus OnUpdate()
    {
      AgentData agentData = this.Agent.AgentData;
      Dictionary<int, Dictionary<int, Dictionary<int, FoodParameterPacket>>> foodParameterTable = Singleton<Resources>.Instance.GameInfo.FoodParameterTable;
      List<StuffItem> stuffItemList = ListPool<StuffItem>.Get();
      ItemIDKeyPair[] canStandEatItems = Singleton<Resources>.Instance.AgentProfile.CanStandEatItems;
      foreach (StuffItem stuffItem in agentData.ItemList)
      {
        Dictionary<int, Dictionary<int, FoodParameterPacket>> dictionary;
        if (foodParameterTable.TryGetValue(stuffItem.CategoryID, out dictionary) && dictionary.TryGetValue(stuffItem.ID, out Dictionary<int, FoodParameterPacket> _))
        {
          if (this._selectCanStandFood)
          {
            bool flag = false;
            foreach (ItemIDKeyPair itemIdKeyPair in canStandEatItems)
            {
              if (itemIdKeyPair.categoryID == stuffItem.CategoryID && itemIdKeyPair.itemID == stuffItem.ID)
              {
                flag = true;
                break;
              }
            }
            if (!flag)
              continue;
          }
          stuffItemList.Add(stuffItem);
        }
      }
      StuffItem element = stuffItemList.GetElement<StuffItem>(Random.Range(0, stuffItemList.Count));
      if (element == null)
        return (TaskStatus) 1;
      agentData.CarryingItem = new StuffItem(element.CategoryID, element.ID, 1);
      ListPool<StuffItem>.Release(stuffItemList);
      return agentData.CarryingItem == null ? (TaskStatus) 1 : (TaskStatus) 2;
    }
  }
}
