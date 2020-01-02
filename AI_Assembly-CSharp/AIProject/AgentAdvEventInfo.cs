// Decompiled with JetBrains decompiler
// Type: AIProject.AgentAdvEventInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIProject
{
  public class AgentAdvEventInfo : ScriptableObject
  {
    public List<AgentAdvEventInfo.Param> param;

    public AgentAdvEventInfo()
    {
      base.\u002Ector();
    }

    [Serializable]
    public class Param
    {
      public string FileName = string.Empty;
      public int[] PlaceIDs = new int[0];
      public int[] Weathers = new int[0];
      public AgentAdvEventInfo.TimeRound TimeRound = new AgentAdvEventInfo.TimeRound();
      public int[] Phases = new int[0];
      public AgentAdvEventInfo.State[] States = new AgentAdvEventInfo.State[0];
      public int ExpansionID = -1;
      public bool LookPlayer = true;
      public AgentAdvEventInfo.EventPosition EventPos = new AgentAdvEventInfo.EventPosition();
      public int EventType;
      public int SortID;

      public bool IsStateEmpty
      {
        get
        {
          return this.States.IsNullOrEmpty<AgentAdvEventInfo.State>();
        }
      }

      public bool IsState(int id, int actionID, int poseID)
      {
        return this.IsStateEmpty || ((IEnumerable<AgentAdvEventInfo.State>) this.States).Any<AgentAdvEventInfo.State>((Func<AgentAdvEventInfo.State, bool>) (item => item.Check(id, actionID, poseID)));
      }
    }

    [Serializable]
    public class TimeRound
    {
      public int start = -1;
      public int end = -1;

      public bool isAll
      {
        get
        {
          return this.start < 0 || this.end < 0;
        }
      }

      public bool Check(int now)
      {
        if (this.isAll)
          return true;
        if (this.start <= this.end)
        {
          if (now >= this.start && now <= this.end)
            return true;
        }
        else if (now >= this.start || now <= this.end)
          return true;
        return false;
      }
    }

    [Serializable]
    public class State
    {
      public int pointID = -1;
      public int actionID = -1;
      public int poseID = -1;

      public bool Check(int pointID, int actionID, int poseID)
      {
        return (this.pointID == -1 || this.pointID == pointID) && (this.actionID == -1 || this.actionID == actionID) && (this.poseID == -1 || this.poseID == poseID);
      }
    }

    [Serializable]
    public class EventPosition
    {
      public int mapID = -1;
      public int ID = -1;

      public bool isOrder
      {
        get
        {
          return this.mapID != -1 && this.ID != -1;
        }
      }
    }
  }
}
