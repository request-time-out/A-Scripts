// Decompiled with JetBrains decompiler
// Type: AIProject.Drink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using Manager;

namespace AIProject
{
  [TaskCategory("")]
  public class Drink : AgentStateAction
  {
    private StuffItem _targetItem;

    public override void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.Drink;
      this._targetItem = agent.SelectDrinkItem();
      base.OnStart();
    }

    protected override void OnCompletedStateTask()
    {
      this.Agent.SetDesire(Desire.GetDesireKey(Desire.Type.Drink), 0.0f);
      AgentActor agent = this.Agent;
      agent.ApplyDrinkParameter(this._targetItem);
      ItemIDKeyPair coconutDrinkId = Singleton<Resources>.Instance.AgentProfile.CoconutDrinkID;
      if (this._targetItem.CategoryID == coconutDrinkId.categoryID && this._targetItem.ID == coconutDrinkId.itemID)
        agent.SetStatus(0, 50f);
      this._targetItem = (StuffItem) null;
    }
  }
}
