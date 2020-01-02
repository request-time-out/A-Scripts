// Decompiled with JetBrains decompiler
// Type: BuildPartsPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BuildPartsPool : ObjPool
{
  private List<int> ReserveList = new List<int>();
  private int nPartsFormKind;
  private int nItemKind;
  private int nCatKind;
  private int nID;
  private int nHeight;
  private bool bLock;

  public void CreatePool(
    GameObject PoolObj,
    Transform createPlace,
    int PreMaxNum,
    int formkind,
    int itemkind,
    int id,
    int catkind,
    int height)
  {
    this.nPartsFormKind = formkind;
    this.nItemKind = itemkind;
    this.nCatKind = catkind;
    this.nID = id;
    this.nHeight = height;
    this.CreatePool(PoolObj, createPlace, PreMaxNum);
    for (int id1 = 0; id1 < this.pool.Count; ++id1)
    {
      if (Object.op_Equality((Object) this.pool[id1].GetComponent<BuildPartsInfo>(), (Object) null))
        this.pool[id1].AddComponent<BuildPartsInfo>();
      BuildPartsInfo component = (BuildPartsInfo) this.pool[id1].GetComponent<BuildPartsInfo>();
      component.Init(id1, this.nPartsFormKind, this.nItemKind, this.nCatKind, 0, this.nID, this.nHeight);
      component.nPutFloor = -1;
    }
  }

  public GameObject Get(ref int ID)
  {
    for (int ID1 = 0; ID1 < this.pool.Count; ++ID1)
    {
      if (!this.pool[ID1].get_activeSelf() && !this.ReserveListCheck(ID1))
      {
        this.pool[ID1].SetActive(false);
        if (this.ReserveList.Count > ID1)
          this.ReserveList[ID1] = ID1;
        else
          this.ReserveList.Add(ID1);
        ID = ID1;
        return this.pool[ID1];
      }
    }
    GameObject poolObject = this.CreatePoolObject();
    poolObject.SetActive(false);
    this.pool.Add(poolObject);
    this.ReserveList.Add(this.ReserveList.Count);
    if (Object.op_Equality((Object) poolObject.GetComponent<BuildPartsInfo>(), (Object) null))
      poolObject.AddComponent<BuildPartsInfo>();
    BuildPartsInfo component = (BuildPartsInfo) poolObject.GetComponent<BuildPartsInfo>();
    component.Init(this.ReserveList.Count - 1, this.nPartsFormKind, this.nItemKind, this.nCatKind, 0, this.nID, this.nHeight);
    component.nPutFloor = -1;
    ID = this.ReserveList.Count - 1;
    return poolObject;
  }

  public int GetFormKind()
  {
    return this.nPartsFormKind;
  }

  public int GetItemKind()
  {
    return this.nItemKind;
  }

  public int GetCategoryKind()
  {
    return this.nCatKind;
  }

  public bool ReserveListCheck(int ID)
  {
    return ID < this.ReserveList.Count && this.ReserveList[ID] == ID;
  }

  public void ReserveListDel(int ID, int mode = 0)
  {
    if (mode == 0)
      this.ReserveList[ID] = -1;
    else
      this.ReserveList.Clear();
  }

  public void ReserveListChange(int ID, int mode = 0)
  {
    if (ID >= this.ReserveList.Count)
      return;
    if (mode == 0)
      this.ReserveList[ID] = ID;
    else
      this.ReserveList[ID] = -1;
  }

  public void SetLock()
  {
    this.bLock = true;
  }

  public void UnLock()
  {
    this.bLock = false;
  }

  public bool CheckLock()
  {
    return this.bLock;
  }
}
