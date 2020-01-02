// Decompiled with JetBrains decompiler
// Type: AIProject.HarvestDataInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIProject
{
  public class HarvestDataInfo
  {
    public HarvestDataInfo(HarvestData.Param data)
    {
      this._data = data;
      ILookup<int, HarvestData.Data> lookup = data.data.ToLookup<HarvestData.Data, int, HarvestData.Data>((Func<HarvestData.Data, int>) (v => v.group), (Func<HarvestData.Data, HarvestData.Data>) (v => v));
      Func<IGrouping<int, HarvestData.Data>, int> keySelector = (Func<IGrouping<int, HarvestData.Data>, int>) (v => v.Key);
      // ISSUE: reference to a compiler-generated field
      if (HarvestDataInfo.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        HarvestDataInfo.\u003C\u003Ef__mg\u0024cache0 = new Func<IGrouping<int, HarvestData.Data>, List<HarvestData.Data>>(Enumerable.ToList<HarvestData.Data>);
      }
      // ISSUE: reference to a compiler-generated field
      Func<IGrouping<int, HarvestData.Data>, List<HarvestData.Data>> fMgCache0 = HarvestDataInfo.\u003C\u003Ef__mg\u0024cache0;
      this.table = lookup.ToDictionary<IGrouping<int, HarvestData.Data>, int, List<HarvestData.Data>>(keySelector, fMgCache0);
    }

    public IReadOnlyCollection<HarvestData.Data> Get()
    {
      List<HarvestData.Data> dataList = new List<HarvestData.Data>();
      foreach (KeyValuePair<int, List<HarvestData.Data>> keyValuePair in this.table)
      {
        Dictionary<HarvestData.Data, int> dictionary = keyValuePair.Value.ToDictionary<HarvestData.Data, HarvestData.Data, int>((Func<HarvestData.Data, HarvestData.Data>) (p => p), (Func<HarvestData.Data, int>) (p => p.percent));
        if (!dictionary.Any<KeyValuePair<HarvestData.Data, int>>())
        {
          Debug.LogError((object) string.Format("収穫抽選失敗:{0}", (object) keyValuePair.Key));
        }
        else
        {
          HarvestData.Data fromDict = Utils.ProbabilityCalclator.DetermineFromDict<HarvestData.Data>(dictionary);
          if (fromDict.nameHash != -1)
            dataList.Add(fromDict);
        }
      }
      return (IReadOnlyCollection<HarvestData.Data>) dataList;
    }

    private HarvestData.Param _data { get; }

    public Dictionary<int, List<HarvestData.Data>> table { get; }

    public int Time
    {
      get
      {
        return this._data.time;
      }
    }

    public int nameHash
    {
      get
      {
        return this._data.nameHash;
      }
    }
  }
}
