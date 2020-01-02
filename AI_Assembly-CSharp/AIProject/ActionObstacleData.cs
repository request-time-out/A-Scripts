// Decompiled with JetBrains decompiler
// Type: AIProject.ActionObstacleData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class ActionObstacleData : ScriptableObject
  {
    public List<ActionObstacleData.Param> param;

    public ActionObstacleData()
    {
      base.\u002Ector();
    }

    [Serializable]
    public class Param
    {
      public string ActionName { get; set; }

      public string Summary { get; set; }

      public int ActionID { get; set; }

      public int PoseID { get; set; }
    }
  }
}
