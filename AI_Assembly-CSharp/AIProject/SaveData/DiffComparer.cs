// Decompiled with JetBrains decompiler
// Type: AIProject.SaveData.DiffComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIProject.SaveData
{
  public static class DiffComparer
  {
    public static void OrganizeItemList(this List<StuffItem> itemList)
    {
      // ISSUE: object of a compiler-generated type is created
      \u003C\u003E__AnonType10<StuffItem, int>[] array = itemList.Select<StuffItem, \u003C\u003E__AnonType10<StuffItem, int>>((Func<StuffItem, int, \u003C\u003E__AnonType10<StuffItem, int>>) ((value, index) => new \u003C\u003E__AnonType10<StuffItem, int>(value, index))).ToArray<\u003C\u003E__AnonType10<StuffItem, int>>();
      List<Tuple<int, StuffItem>> source = ListPool<Tuple<int, StuffItem>>.Get();
      foreach (\u003C\u003E__AnonType10<StuffItem, int> anonType10 in array)
      {
        // ISSUE: variable of a compiler-generated type
        \u003C\u003E__AnonType10<StuffItem, int> item = anonType10;
        Tuple<int, StuffItem> tuple = source.Find((Predicate<Tuple<int, StuffItem>>) (x => x.Item2.CategoryID == item.value.CategoryID && x.Item2.ID == item.value.ID));
        if (tuple == null)
        {
          source.Add(new Tuple<int, StuffItem>(item.index, new StuffItem(item.value)));
        }
        else
        {
          tuple.Item2.Count += item.value.Count;
          if (tuple.Item2.Count > Singleton<Resources>.Instance.DefinePack.MapDefines.ItemStackUpperLimit)
          {
            int count = tuple.Item2.Count - 99;
            tuple.Item2.Count -= count;
            StuffItem stuffItem = new StuffItem(tuple.Item2.CategoryID, tuple.Item2.ID, count);
            int num = ((IEnumerable<\u003C\u003E__AnonType10<StuffItem, int>>) array).Last<\u003C\u003E__AnonType10<StuffItem, int>>().index + 1;
            source.Add(new Tuple<int, StuffItem>(num, stuffItem));
          }
        }
      }
      List<Tuple<int, StuffItem>> list = source.OrderBy<Tuple<int, StuffItem>, int>((Func<Tuple<int, StuffItem>, int>) (x => x.Item2.ID)).ToList<Tuple<int, StuffItem>>();
      List<StuffItem> toRelease = ListPool<StuffItem>.Get();
      foreach (Tuple<int, StuffItem> tuple in list)
        toRelease.Add(tuple.Item2);
      itemList.Clear();
      itemList.AddRange((IEnumerable<StuffItem>) toRelease.ToArray());
      ListPool<StuffItem>.Release(toRelease);
      ListPool<Tuple<int, StuffItem>>.Release(list);
    }
  }
}
