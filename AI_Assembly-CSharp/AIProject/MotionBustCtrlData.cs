// Decompiled with JetBrains decompiler
// Type: AIProject.MotionBustCtrlData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class MotionBustCtrlData : ScriptableObject
  {
    public List<MotionBustCtrlData.Param> param;

    public MotionBustCtrlData()
    {
      base.\u002Ector();
    }

    [Serializable]
    public class Param
    {
      public List<string> Parameters = new List<string>();
      public string Summary;
      public string State;
    }
  }
}
