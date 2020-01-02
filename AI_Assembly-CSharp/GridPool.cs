// Decompiled with JetBrains decompiler
// Type: GridPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class GridPool : ObjPool
{
  public GameObject Get()
  {
    for (int index = 0; index < this.pool.Count; ++index)
    {
      if (!this.pool[index].get_activeSelf())
      {
        this.pool[index].SetActive(false);
        return this.pool[index];
      }
    }
    GameObject poolObject = this.CreatePoolObject();
    poolObject.SetActive(false);
    this.pool.Add(poolObject);
    return poolObject;
  }
}
