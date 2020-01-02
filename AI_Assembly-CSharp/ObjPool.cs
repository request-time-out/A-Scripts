// Decompiled with JetBrains decompiler
// Type: ObjPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public abstract class ObjPool
{
  protected List<GameObject> pool = new List<GameObject>();
  private GameObject poolObj;
  private Transform CreatePlace;

  public void CreatePool(GameObject PoolObj, Transform createPlace, int PreMaxNum)
  {
    this.poolObj = PoolObj;
    this.CreatePlace = createPlace;
    for (int index = 0; index < PreMaxNum; ++index)
      this.pool.Add(this.CreatePoolObject());
  }

  protected GameObject CreatePoolObject()
  {
    GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.poolObj, this.CreatePlace);
    gameObject.SetActive(false);
    return gameObject;
  }

  public List<GameObject> GetList()
  {
    return this.pool;
  }
}
