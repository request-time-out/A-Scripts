﻿// Decompiled with JetBrains decompiler
// Type: AIProject.ActByproductData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class ActByproductData : ScriptableObject
  {
    public List<ActByproductData.Param> param;

    public ActByproductData()
    {
      base.\u002Ector();
    }

    [Serializable]
    public class Param
    {
      public List<string> ItemList = new List<string>();
      public string ActName;
      public string PoseName;
      public int ActionID;
      public int PoseID;
    }
  }
}
