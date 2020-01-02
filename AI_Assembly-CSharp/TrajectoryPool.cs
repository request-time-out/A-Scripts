// Decompiled with JetBrains decompiler
// Type: TrajectoryPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class TrajectoryPool
{
  private List<GameObject> PoolobjList;
  private GameObject PoolObj;
  private Transform CreatePlace;

  public void CreatePool(GameObject obj, int nMaxCount, Transform createPlace)
  {
    this.PoolObj = obj;
    this.PoolobjList = new List<GameObject>();
    this.CreatePlace = createPlace;
    for (int index = 0; index < nMaxCount; ++index)
    {
      GameObject poolObject = this.CreatePoolObject();
      poolObject.get_transform().SetParent(this.CreatePlace, false);
      poolObject.SetActive(false);
      this.PoolobjList.Add(poolObject);
    }
  }

  public GameObject GetObject()
  {
    for (int index1 = 0; index1 < this.PoolobjList.Count; ++index1)
    {
      int index2 = index1;
      if (!this.PoolobjList[index2].get_activeSelf())
      {
        this.PoolobjList[index2].SetActive(true);
        return this.PoolobjList[index2];
      }
    }
    GameObject poolObject = this.CreatePoolObject();
    poolObject.get_transform().SetParent(this.CreatePlace, false);
    poolObject.SetActive(true);
    this.PoolobjList.Add(poolObject);
    return poolObject;
  }

  private GameObject CreatePoolObject()
  {
    return (GameObject) Object.Instantiate<GameObject>((M0) this.PoolObj);
  }

  public int getListCount()
  {
    return this.PoolobjList.Count;
  }

  public List<GameObject> getList()
  {
    return this.PoolobjList;
  }
}
