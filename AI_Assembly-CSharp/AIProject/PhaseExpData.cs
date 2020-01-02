// Decompiled with JetBrains decompiler
// Type: AIProject.PhaseExpData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class PhaseExpData : ScriptableObject
  {
    public List<PhaseExpData.Param> param;

    public PhaseExpData()
    {
      base.\u002Ector();
    }

    [Serializable]
    public class Param
    {
      public List<string> ExpArray = new List<string>();
      public int Personality;
      public string Name;
    }
  }
}
