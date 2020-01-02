// Decompiled with JetBrains decompiler
// Type: AIProject.PlayGame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class PlayGame : AgentStateAction
  {
    public override void OnStart()
    {
      this.Agent.EventKey = EventType.Play;
      base.OnStart();
    }

    protected override void OnCompletedStateTask()
    {
      AgentActor agent = this.Agent;
      int desireKey = Desire.GetDesireKey(Desire.Type.Game);
      agent.SetDesire(desireKey, 0.0f);
      PoseKeyPair wateringId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.WateringID;
      if (agent.ActionID != wateringId.postureID || agent.PoseID != wateringId.poseID)
        return;
      float? nullable = new float?();
      FarmPoint farmPoint1 = (FarmPoint) null;
      foreach (FarmPoint farmPoint2 in Singleton<Manager.Map>.Instance.PointAgent.FarmPoints)
      {
        float num = Vector3.Distance(agent.Position, farmPoint2.Position);
        if (nullable.HasValue)
        {
          if ((double) num < (double) nullable.Value)
          {
            nullable = new float?(num);
            farmPoint1 = farmPoint2;
          }
        }
        else
        {
          nullable = new float?(num);
          farmPoint1 = farmPoint2;
        }
      }
      foreach (KeyValuePair<int, FarmPoint> keyValuePair in (IEnumerable<KeyValuePair<int, FarmPoint>>) Singleton<Manager.Map>.Instance.PointAgent.RuntimeFarmPointTable)
      {
        if (keyValuePair.Value.Kind == FarmPoint.FarmKind.Plant)
        {
          float num = Vector3.Distance(agent.Position, keyValuePair.Value.Position);
          if (nullable.HasValue)
          {
            if ((double) num < (double) nullable.Value)
            {
              nullable = new float?(num);
              farmPoint1 = keyValuePair.Value;
            }
          }
          else
          {
            nullable = new float?(num);
            farmPoint1 = keyValuePair.Value;
          }
        }
      }
      List<AIProject.SaveData.Environment.PlantInfo> plantInfoList;
      if (!Object.op_Inequality((Object) farmPoint1, (Object) null) || !Singleton<Game>.Instance.Environment.FarmlandTable.TryGetValue(farmPoint1.RegisterID, out plantInfoList))
        return;
      foreach (AIProject.SaveData.Environment.PlantInfo plantInfo in plantInfoList)
        plantInfo?.AddTimer(60f);
      if (!agent.ChaControl.fileGameInfo.normalSkill.ContainsValue(12))
        return;
      foreach (AIProject.SaveData.Environment.PlantInfo plantInfo in plantInfoList)
        plantInfo?.AddTimer(20f);
    }
  }
}
