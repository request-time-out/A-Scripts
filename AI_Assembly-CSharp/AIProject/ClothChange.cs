// Decompiled with JetBrains decompiler
// Type: AIProject.ClothChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class ClothChange : AgentStateAction
  {
    public override void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.ClothChange;
      List<string> closetCoordinateList = Singleton<Game>.Instance.Environment.ClosetCoordinateList;
      string element = closetCoordinateList.GetElement<string>(Random.Range(0, closetCoordinateList.Count));
      agent.AgentData.NowCoordinateFileName = element;
      base.OnStart();
    }

    protected override void OnCompletedStateTask()
    {
      this.Agent.AgentData.IsOtherCoordinate = true;
    }
  }
}
