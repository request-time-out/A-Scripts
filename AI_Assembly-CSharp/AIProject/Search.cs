// Decompiled with JetBrains decompiler
// Type: AIProject.Search
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System.Collections.Generic;

namespace AIProject
{
  [TaskCategory("")]
  public class Search : AgentStateAction
  {
    public override void OnStart()
    {
      this.Agent.EventKey = EventType.Search;
      base.OnStart();
    }

    protected override void OnCompletedStateTask()
    {
      AgentActor agent = this.Agent;
      int desireKey = Desire.GetDesireKey(Desire.Type.Hunt);
      agent.SetDesire(desireKey, 0.0f);
      Dictionary<int, AIProject.SaveData.Environment.SearchActionInfo> searchActionLockTable = agent.AgentData.SearchActionLockTable;
      AIProject.SaveData.Environment.SearchActionInfo searchActionInfo;
      if (!searchActionLockTable.TryGetValue(agent.CurrentPoint.RegisterID, out searchActionInfo))
        searchActionInfo = new AIProject.SaveData.Environment.SearchActionInfo();
      ++searchActionInfo.Count;
      searchActionLockTable[agent.CurrentPoint.RegisterID] = searchActionInfo;
      ActionPoint currentPoint = this.Agent.CurrentPoint;
      Dictionary<int, ItemTableElement> itemTableInArea = Singleton<Resources>.Instance.GameInfo.GetItemTableInArea(currentPoint.IDList.IsNullOrEmpty<int>() ? currentPoint.ID : currentPoint.IDList.GetElement<int>(0));
      if (itemTableInArea != null)
        ;
      Actor.SearchInfo searchInfo = agent.RandomAddItem(itemTableInArea, false);
      if (!searchInfo.IsSuccess)
        return;
      foreach (Actor.ItemSearchInfo itemSearchInfo in searchInfo.ItemList)
      {
        StuffItem stuffItem = new StuffItem(itemSearchInfo.categoryID, itemSearchInfo.id, itemSearchInfo.count);
        agent.AgentData.ItemList.AddItem(stuffItem);
        StuffItemInfo info = Singleton<Resources>.Instance.GameInfo.GetItem(itemSearchInfo.categoryID, itemSearchInfo.id);
        MapUIContainer.AddItemLog((Actor) agent, info, itemSearchInfo.count, false);
      }
    }
  }
}
