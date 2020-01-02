// Decompiled with JetBrains decompiler
// Type: AIProject.CheckPantry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class CheckPantry : AgentAction
  {
    [SerializeField]
    private CheckPantry.CheckType _checkType;

    public virtual void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.CurrentPoint = agent.TargetInSightActionPoint;
      ((Task) this).OnStart();
      agent.DisableActionFlag();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.Complete();
      return (TaskStatus) 2;
    }

    private void Complete()
    {
      AgentActor agent = this.Agent;
      agent.ResetActionFlag();
      AgentData agentData = agent.AgentData;
      List<StuffItem> itemListInPantry = Singleton<Game>.Instance.WorldData.Environment.ItemListInPantry;
      List<StuffItem> stuffItemList1 = ListPool<StuffItem>.Get();
      Dictionary<int, Dictionary<int, Dictionary<int, FoodParameterPacket>>> dictionary1 = this._checkType != CheckPantry.CheckType.Eat ? Singleton<Resources>.Instance.GameInfo.DrinkParameterTable : Singleton<Resources>.Instance.GameInfo.FoodParameterTable;
      foreach (StuffItem stuffItem in itemListInPantry)
      {
        Dictionary<int, Dictionary<int, FoodParameterPacket>> dictionary2;
        if (dictionary1.TryGetValue(stuffItem.CategoryID, out dictionary2) && dictionary2.TryGetValue(stuffItem.ID, out Dictionary<int, FoodParameterPacket> _))
          stuffItemList1.Add(stuffItem);
      }
      StuffItem stuffItem1 = (StuffItem) null;
      if (this._checkType == CheckPantry.CheckType.Eat)
      {
        stuffItem1 = stuffItemList1.GetElement<StuffItem>(Random.Range(0, stuffItemList1.Count));
      }
      else
      {
        AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
        float num = agentData.StatsTable[0];
        if ((double) num <= (double) agentProfile.ColdTempBorder)
        {
          List<StuffItem> stuffItemList2 = ListPool<StuffItem>.Get();
          foreach (StuffItem stuffItem2 in stuffItemList1)
          {
            StuffItem stuffItem = stuffItem2;
            if (agentProfile.LowerTempDrinkItems.Exists<ItemIDKeyPair>((Predicate<ItemIDKeyPair>) (pair => pair.categoryID == stuffItem.CategoryID && pair.itemID == stuffItem.ID)))
              stuffItemList2.Add(stuffItem);
          }
          if (!stuffItemList2.IsNullOrEmpty<StuffItem>())
            stuffItem1 = stuffItemList2.GetElement<StuffItem>(Random.Range(0, stuffItemList2.Count));
          ListPool<StuffItem>.Release(stuffItemList2);
        }
        else if ((double) num >= (double) agentProfile.HotTempBorder)
        {
          List<StuffItem> stuffItemList2 = ListPool<StuffItem>.Get();
          foreach (StuffItem stuffItem2 in stuffItemList1)
          {
            StuffItem stuffItem = stuffItem2;
            if (agentProfile.RaiseTempDrinkItems.Exists<ItemIDKeyPair>((Predicate<ItemIDKeyPair>) (pair => pair.categoryID == stuffItem.CategoryID && pair.itemID == stuffItem.ID)))
              stuffItemList2.Add(stuffItem);
          }
          if (!stuffItemList2.IsNullOrEmpty<StuffItem>())
            stuffItem1 = stuffItemList2.GetElement<StuffItem>(Random.Range(0, stuffItemList2.Count));
          ListPool<StuffItem>.Release(stuffItemList2);
        }
        if (stuffItem1 == null)
          stuffItem1 = stuffItemList1.GetElement<StuffItem>(Random.Range(0, stuffItemList1.Count));
      }
      if (stuffItem1 != null)
      {
        StuffItem stuffItem2 = new StuffItem(stuffItem1.CategoryID, stuffItem1.ID, 1);
        agentData.ItemList.Add(stuffItem2);
        itemListInPantry.RemoveItem(stuffItem2);
      }
      ListPool<StuffItem>.Release(stuffItemList1);
      if (Object.op_Inequality((Object) agent.CurrentPoint, (Object) null))
      {
        agent.CurrentPoint.SetActiveMapItemObjs(true);
        agent.CurrentPoint.ReleaseSlot((Actor) agent);
        agent.CurrentPoint = (ActionPoint) null;
      }
      agent.EventKey = (EventType) 0;
      agent.PrevActionPoint = agent.TargetInSightActionPoint;
      agent.TargetInSightActionPoint = (ActionPoint) null;
    }

    private enum CheckType
    {
      Eat,
      Drink,
    }
  }
}
