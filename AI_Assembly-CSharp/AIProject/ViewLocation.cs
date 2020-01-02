// Decompiled with JetBrains decompiler
// Type: AIProject.ViewLocation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class ViewLocation : AgentAction
  {
    public virtual void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.Location;
      agent.CurrentPoint = agent.TargetInSightActionPoint;
      ((Task) this).OnStart();
      agent.DisableActionFlag();
    }

    public virtual TaskStatus OnUpdate()
    {
      this.Complete();
      return (TaskStatus) 2;
    }

    private void Complete()
    {
      AgentActor agent = this.Agent;
      int actionID = (int) Action.NameTable[EventType.Location].Item1;
      agent.UpdateStatus(actionID, 0);
      int desireKey = Desire.GetDesireKey(Desire.Type.Location);
      agent.SetDesire(desireKey, 0.0f);
      ++agent.AgentData.LocationCount;
      if (agent.AgentData.LocationCount >= agent.AgentData.LocationTaskCount)
      {
        switch (agent.CurrentPoint.RegisterID)
        {
          case 269:
            agent.ChaControl.fileGameInfo.favoritePlace = 11;
            break;
          case 270:
            agent.ChaControl.fileGameInfo.favoritePlace = 9;
            break;
          case 271:
            agent.ChaControl.fileGameInfo.favoritePlace = 10;
            break;
          case 272:
            agent.ChaControl.fileGameInfo.favoritePlace = 8;
            break;
          case 273:
            agent.ChaControl.fileGameInfo.favoritePlace = 7;
            break;
          case 274:
            agent.ChaControl.fileGameInfo.favoritePlace = 5;
            break;
          case 275:
            agent.ChaControl.fileGameInfo.favoritePlace = 6;
            break;
          case 276:
            agent.ChaControl.fileGameInfo.favoritePlace = 4;
            break;
          case 277:
            agent.ChaControl.fileGameInfo.favoritePlace = 3;
            break;
          case 278:
            agent.ChaControl.fileGameInfo.favoritePlace = 1;
            break;
          case 279:
            agent.ChaControl.fileGameInfo.favoritePlace = 2;
            break;
        }
        agent.AgentData.LocationTaskCount = Random.Range(1, 3);
      }
      agent.CauseSick();
      agent.ResetActionFlag();
      agent.CurrentPoint.SetActiveMapItemObjs(true);
      agent.CurrentPoint.ReleaseSlot((Actor) agent);
      agent.CurrentPoint = (ActionPoint) null;
      agent.PrevActionPoint = agent.TargetInSightActionPoint;
    }
  }
}
