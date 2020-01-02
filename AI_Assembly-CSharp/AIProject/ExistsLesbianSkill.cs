// Decompiled with JetBrains decompiler
// Type: AIProject.ExistsLesbianSkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System.Collections.Generic;

namespace AIProject
{
  [TaskCategory("")]
  public class ExistsLesbianSkill : AgentConditional
  {
    public virtual TaskStatus OnUpdate()
    {
      AgentProfile.HSkillIDDefines hskillIdSetting = Singleton<Resources>.Instance.AgentProfile.HSkillIDSetting;
      foreach (KeyValuePair<int, int> keyValuePair in this.Agent.ChaControl.fileGameInfo.hSkill)
      {
        if (keyValuePair.Value != -1 && keyValuePair.Value == hskillIdSetting.homosexualID)
          return (TaskStatus) 2;
      }
      return (TaskStatus) 1;
    }
  }
}
