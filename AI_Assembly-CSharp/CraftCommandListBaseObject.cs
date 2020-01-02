// Decompiled with JetBrains decompiler
// Type: CraftCommandListBaseObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;

public class CraftCommandListBaseObject : Singleton<CraftCommandListBaseObject>
{
  public List<int> MaxPutHeight = new List<int>();
  public List<GridInfo> BaseGridInfo;
  public List<BuildPartsPool>[] BaseParts;
  public int nMaxFloorCnt;
  public int nTargetFloorCnt;
  public List<bool[]> tmpGridActiveList;
  public List<bool> tmpGridActiveListUpdate;
  public Dictionary<int, string> CategoryNames;
  public int nPutPartsNum;
}
