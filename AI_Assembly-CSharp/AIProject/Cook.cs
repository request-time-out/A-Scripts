// Decompiled with JetBrains decompiler
// Type: AIProject.Cook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using AIProject.UI;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System.Collections.Generic;

namespace AIProject
{
  [TaskCategory("")]
  public class Cook : AgentStateAction
  {
    private bool _continueCookSeq;
    private CraftUI.CreateItem _createItem;

    public override void OnStart()
    {
      this.Agent.EventKey = EventType.Cook;
      this._continueCookSeq = this.CalcTargetRecipe();
      if (!this._continueCookSeq)
        return;
      base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
      if (this._continueCookSeq)
        return base.OnUpdate();
      this.Agent.SetDesire(Desire.GetDesireKey(Desire.Type.Cook), 0.0f);
      return (TaskStatus) 2;
    }

    protected override void OnCompletedStateTask()
    {
      AgentActor agent = this.Agent;
      int desireKey = Desire.GetDesireKey(Desire.Type.Cook);
      agent.SetDesire(desireKey, 0.0f);
      if (this._createItem == null)
        return;
      StuffItemInfo info = Singleton<Resources>.Instance.GameInfo.GetItem(this._createItem.item.CategoryID, this._createItem.item.ID);
      MapUIContainer.AddItemLog((Actor) agent, info, this._createItem.info.CreateSum, false);
      this._createItem = (CraftUI.CreateItem) null;
    }

    private bool CalcTargetRecipe()
    {
      AgentActor agent = this.Agent;
      List<StuffItem> itemList = agent.AgentData.ItemList;
      this._createItem = CraftUI.CreateCheck(Singleton<Resources>.Instance.GameInfo.recipe.cookTable, (IReadOnlyCollection<IReadOnlyCollection<StuffItem>>) new List<StuffItem>[2]
      {
        itemList,
        Singleton<Game>.Instance.Environment.ItemListInPantry
      });
      int num = agent.ChaControl.fileGameInfo.flavorState[0];
      bool chef = agent.ChaControl.fileGameInfo.normalSkill.ContainsValue(0);
      return CraftUI.CreateCooking(this._createItem, itemList, (float) num, chef);
    }
  }
}
