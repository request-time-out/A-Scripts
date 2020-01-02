// Decompiled with JetBrains decompiler
// Type: BuildPartsInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BuildPartsInfo : MonoBehaviour
{
  public int nHeight;
  public int nPutFloor;
  public List<GridInfo> putGridInfos;
  public List<int> putSmallGridInfos;
  private int nID;
  private int nPoolID;
  private int nFormKind;
  private int nItemKind;
  private int nCatKind;
  private int nDirection;

  public BuildPartsInfo()
  {
    base.\u002Ector();
  }

  public void Init(
    int id,
    int formkind,
    int itemkind,
    int catkind,
    int Dire,
    int poolId,
    int height = 1)
  {
    this.nID = id;
    this.nPoolID = poolId;
    this.nFormKind = formkind;
    this.nItemKind = itemkind;
    this.nCatKind = catkind;
    this.nDirection = Dire;
    this.nHeight = height;
  }

  public void SetDirection(int Dire)
  {
    this.nDirection = Dire;
  }

  public int GetInfo(int kind)
  {
    switch (kind)
    {
      case 0:
        return this.nID;
      case 1:
        return this.nPoolID;
      case 2:
        return this.nFormKind;
      case 3:
        return this.nItemKind;
      case 4:
        return this.nDirection;
      case 5:
        return this.nCatKind;
      default:
        return -1;
    }
  }
}
