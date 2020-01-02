// Decompiled with JetBrains decompiler
// Type: BuildPartsMgr
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildPartsMgr : MonoBehaviour
{
  private List<BuildPartsPool>[] Allpool;
  private List<CraftItemInfo> infos;
  public List<Transform> CreatePlace;
  private Dictionary<int, List<GameObject>> BuildPartDic;
  public Dictionary<int, Dictionary<int, Tuple<BuildPartsPool, CraftItemInfo>>> BuildPartPoolDic;
  private const int PreBuildMaxNum = 50;

  public BuildPartsMgr()
  {
    base.\u002Ector();
  }

  public void Init()
  {
    CraftResource instance = Singleton<CraftResource>.Instance;
    foreach (int craftItemCategory in instance.GetCraftItemCategories())
    {
      using (IEnumerator<KeyValuePair<int, CraftItemInfo>> enumerator = instance.GetItemTable(craftItemCategory).GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.infos.Add(enumerator.Current.Value);
      }
    }
    this.Allpool = new List<BuildPartsPool>[10];
    for (int index = 0; index < this.Allpool.Length; ++index)
      this.Allpool[index] = new List<BuildPartsPool>();
    Singleton<CraftCommandListBaseObject>.Instance.CategoryNames = new Dictionary<int, string>();
    List<CraftItemInfo>[] craftItemInfoListArray = new List<CraftItemInfo>[17];
    for (int index = 0; index < craftItemInfoListArray.Length; ++index)
      craftItemInfoListArray[index] = new List<CraftItemInfo>();
    for (int index = 0; index < this.infos.Count; ++index)
      craftItemInfoListArray[this.infos[index].Itemkind - 1].Add(this.infos[index]);
    for (int index1 = 0; index1 < craftItemInfoListArray.Length; ++index1)
    {
      for (int index2 = 0; index2 < craftItemInfoListArray[index1].Count; ++index2)
      {
        int num = index2;
        craftItemInfoListArray[index1][index2].Id = num;
      }
    }
    for (int key = 0; key < 10; ++key)
      this.BuildPartPoolDic.Add(key, new Dictionary<int, Tuple<BuildPartsPool, CraftItemInfo>>());
    for (int key = 1; key < 17; ++key)
      this.BuildPartDic.Add(key, new List<GameObject>());
    for (int index = 0; index < this.infos.Count; ++index)
    {
      this.BuildPartDic[this.infos[index].Itemkind].Add(this.infos[index].obj);
      BuildPartsPool buildPartsPool = new BuildPartsPool();
      buildPartsPool.CreatePool(this.BuildPartDic[this.infos[index].Itemkind][this.infos[index].Id], this.CreatePlace[this.infos[index].Itemkind - 1], 50, this.infos[index].Formkind, this.infos[index].Itemkind, this.Allpool[this.infos[index].Formkind].Count, this.infos[index].Catkind, this.infos[index].Height);
      this.Allpool[this.infos[index].Formkind].Add(buildPartsPool);
      if (!this.BuildPartPoolDic[this.infos[index].Formkind].ContainsKey(this.Allpool[this.infos[index].Formkind].Count - 1))
        this.BuildPartPoolDic[this.infos[index].Formkind].Add(this.Allpool[this.infos[index].Formkind].Count - 1, new Tuple<BuildPartsPool, CraftItemInfo>(buildPartsPool, this.infos[index]));
    }
    for (int index = 0; index < this.infos.Count; ++index)
    {
      if (!Singleton<CraftCommandListBaseObject>.Instance.CategoryNames.ContainsKey(this.infos[index].Catkind))
        Singleton<CraftCommandListBaseObject>.Instance.CategoryNames.Add(this.infos[index].Catkind, this.infos[index].CategoryName);
    }
    this.Allpool[2][0].SetLock();
  }

  public List<BuildPartsPool>[] GetPool()
  {
    return this.Allpool;
  }

  public BuildPartsInfo GetBuildPartsInfo(GameObject buildPart)
  {
    return (BuildPartsInfo) buildPart.GetComponent<BuildPartsInfo>();
  }
}
