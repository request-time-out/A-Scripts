// Decompiled with JetBrains decompiler
// Type: AIProject.SetStoryPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class SetStoryPoint : AgentAction
  {
    [SerializeField]
    private int _pointID;

    public virtual TaskStatus OnUpdate()
    {
      StoryPoint storyPoint = (StoryPoint) null;
      if (Singleton<Manager.Map>.IsInstance())
      {
        PointManager pointAgent = Singleton<Manager.Map>.Instance.PointAgent;
        if (Object.op_Equality((Object) pointAgent, (Object) null))
          return (TaskStatus) 1;
        Dictionary<int, StoryPoint> storyPointTable = pointAgent.StoryPointTable;
        if (storyPointTable.IsNullOrEmpty<int, StoryPoint>())
          return (TaskStatus) 1;
        storyPointTable.TryGetValue(this._pointID, out storyPoint);
      }
      this.Agent.TargetStoryPoint = storyPoint;
      return Object.op_Equality((Object) storyPoint, (Object) null) ? (TaskStatus) 1 : (TaskStatus) 2;
    }
  }
}
