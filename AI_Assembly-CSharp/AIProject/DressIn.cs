// Decompiled with JetBrains decompiler
// Type: AIProject.DressIn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class DressIn : AgentStateAction
  {
    public override void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.DressIn;
      List<string> dressCoordinateList = Singleton<Game>.Instance.Environment.DressCoordinateList;
      string element = dressCoordinateList.GetElement<string>(Random.Range(0, dressCoordinateList.Count));
      agent.AgentData.BathCoordinateFileName = element;
      base.OnStart();
    }

    protected override void OnCompletedStateTask()
    {
      this.Agent.AgentData.PlayedDressIn = true;
    }
  }
}
