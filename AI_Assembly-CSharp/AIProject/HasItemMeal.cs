// Decompiled with JetBrains decompiler
// Type: AIProject.HasItemMeal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;

namespace AIProject
{
  [TaskCategory("")]
  public class HasItemMeal : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
      StuffItem carryingItem = agent.AgentData.CarryingItem;
      if (carryingItem == null)
        return (TaskStatus) 1;
      return !agentProfile.CanStandEatItems.Exists<ItemIDKeyPair>((Predicate<ItemIDKeyPair>) (pair => pair.categoryID == carryingItem.CategoryID && pair.itemID == carryingItem.ID)) ? (TaskStatus) 2 : (TaskStatus) 1;
    }
  }
}
