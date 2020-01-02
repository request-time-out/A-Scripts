// Decompiled with JetBrains decompiler
// Type: AIProject.ActionTalkData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class ActionTalkData : ScriptableObject
  {
    public List<ActionTalkData.Param> param;

    public ActionTalkData()
    {
      base.\u002Ector();
    }

    [Serializable]
    public class Param
    {
      public string ActionName;
      public string Summary;
      public int ActionID;
      public int PoseID;
      public float ObstacleRadius;
      public bool useNeckLook;
      public bool CanTalk;
      public int TalkAttitudeID;
      public bool CanHCommand;
      public bool IsBadMood;
      public bool IsSpecial;
      public int HPositionID;
      public int HPositionSubID;
    }
  }
}
