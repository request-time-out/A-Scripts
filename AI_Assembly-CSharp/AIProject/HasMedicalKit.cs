﻿// Decompiled with JetBrains decompiler
// Type: AIProject.HasMedicalKit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using Manager;

namespace AIProject
{
  [TaskCategory("")]
  public class HasMedicalKit : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      ItemIDKeyPair gauzeId = Singleton<Resources>.Instance.CommonDefine.ItemIDDefine.GauzeID;
      foreach (StuffItem stuffItem in this.Agent.AgentData.ItemList)
      {
        if (stuffItem.CategoryID == gauzeId.categoryID && stuffItem.ID == gauzeId.itemID)
          return (TaskStatus) 2;
      }
      return (TaskStatus) 1;
    }
  }
}