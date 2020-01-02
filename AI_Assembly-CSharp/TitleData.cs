// Decompiled with JetBrains decompiler
// Type: TitleData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class TitleData : ScriptableObject
{
  public List<TitleData.Param> param;

  public TitleData()
  {
    base.\u002Ector();
  }

  [Serializable]
  public class Param
  {
    public int id;
    public string comment;
    public string assetPath;
    public string fileName;
    public string manifest;
  }
}
