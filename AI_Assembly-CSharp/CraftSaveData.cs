// Decompiled with JetBrains decompiler
// Type: CraftSaveData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

internal struct CraftSaveData
{
  public int MaxFloorNum;
  public int nPutPartsNum;
  public List<Vector3> GridPos;
  public List<bool> GridUseState;
  public List<int> nFloorPartsHeight;
  public List<List<List<int>>> SmallGridState;
  public List<List<List<int[]>>> SmallGridOnParts;
  public List<List<List<int[]>>> SmallGridOnStackWall;
  public List<List<List<int>>> SmallGridCanRoofState;
  public List<List<List<bool>>> SmallGridInRoomState;
  public List<int> BuildPartsGridKind;
  public List<int> BuildPartsKind;
  public List<int> BuildPartsFloor;
  public List<Vector3> BuildPartsPos;
  public List<Quaternion> BuildPartsRot;
  public List<List<int>> BuildPartsPutGridInfos;
  public List<List<int>> BuildPartsPutSmallGridInfos;
  public List<int> BuildPartsPutGridInfosNum;
  public List<bool[]> tmpGridActiveList;
  public List<bool> tmpGridActiveListUpdate;
  public List<int> MaxPutHeight;
}
