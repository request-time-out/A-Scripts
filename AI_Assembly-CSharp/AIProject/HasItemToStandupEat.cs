// Decompiled with JetBrains decompiler
// Type: AIProject.HasItemToStandupEat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;

namespace AIProject
{
  [TaskCategory("")]
  public class HasItemToStandupEat : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
      foreach (StuffItem stuffItem in this.Agent.AgentData.ItemList)
      {
        StuffItem item = stuffItem;
        if (agentProfile.CanStandEatItems.Exists<ItemIDKeyPair>((Predicate<ItemIDKeyPair>) (pair => pair.categoryID == item.CategoryID && pair.itemID == item.ID)))
          return (TaskStatus) 2;
      }
      return (TaskStatus) 1;
    }
  }
}
