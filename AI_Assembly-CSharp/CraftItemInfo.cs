// Decompiled with JetBrains decompiler
// Type: CraftItemInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class CraftItemInfo
{
  public int Id;
  public int Formkind;
  public int Itemkind;
  public int Catkind;
  public int Horizontal;
  public int Vertical;
  public int Height;
  public float MoveVal;
  public Tuple<int, int>[] recipe;
  public int PutFlag;
  public int[] JudgeCondition;
  public int Cost;
  public int Element;
  public string Name;
  public string CategoryName;
  public GameObject obj;
}
