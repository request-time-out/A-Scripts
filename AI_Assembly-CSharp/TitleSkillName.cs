// Decompiled with JetBrains decompiler
// Type: TitleSkillName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class TitleSkillName : ScriptableObject
{
  public List<TitleSkillName.Param> param;

  public TitleSkillName()
  {
    base.\u002Ector();
  }

  [Serializable]
  public class Param
  {
    public int id;
    public string name0;
    public string name1;
    public string name2;
    public string name3;
    public string name4;
  }
}
