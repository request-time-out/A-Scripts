// Decompiled with JetBrains decompiler
// Type: AIProject.SaveData.StuffItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ADV;
using MessagePack;
using System;
using System.Collections.Generic;

namespace AIProject.SaveData
{
  [MessagePackObject(false)]
  public class StuffItem : ICommandData
  {
    public StuffItem()
    {
    }

    public StuffItem(int category, int id, int count)
    {
      this.CategoryID = category;
      this.ID = id;
      this.Count = count;
    }

    public StuffItem(StuffItem source)
    {
      this.CategoryID = source.CategoryID;
      this.ID = source.ID;
      this.Count = source.Count;
      this.LatestDateTime = source.LatestDateTime;
    }

    [Key(0)]
    public int CategoryID { get; set; }

    [Key(1)]
    public int ID { get; set; }

    [Key(2)]
    public int Count { get; set; }

    [Key(3)]
    public DateTime LatestDateTime { get; set; }

    public IEnumerable<CommandData> CreateCommandData(string head)
    {
      return (IEnumerable<CommandData>) new CommandData[4]
      {
        new CommandData(CommandData.Command.Int, head + string.Format(".{0}", (object) "CategoryID"), (Func<object>) (() => (object) this.CategoryID), (Action<object>) null),
        new CommandData(CommandData.Command.Int, head + string.Format(".{0}", (object) "ID"), (Func<object>) (() => (object) this.ID), (Action<object>) null),
        new CommandData(CommandData.Command.Int, head + string.Format(".{0}", (object) "Count"), (Func<object>) (() => (object) this.Count), (Action<object>) null),
        new CommandData(CommandData.Command.String, head + string.Format(".{0}", (object) "LatestDateTime"), (Func<object>) (() => (object) this.LatestDateTime.ToString()), (Action<object>) null)
      };
    }

    public static int RemoveStorages(StuffItem item, IReadOnlyCollection<List<StuffItem>> items)
    {
      return StuffItem.RemoveStorages(item, item.Count, items);
    }

    public static int RemoveStorages(
      StuffItem item,
      int count,
      IReadOnlyCollection<List<StuffItem>> items)
    {
      foreach (List<StuffItem> self in (IEnumerable<List<StuffItem>>) items)
      {
        self.RemoveItem(item, ref count);
        if (count <= 0)
          break;
      }
      return count;
    }

    public static StuffItem CreateSystemItem(int id, int category = 0, int count = 1)
    {
      return new StuffItem(category, id, count);
    }

    public bool MatchItem(ItemIDKeyPair key)
    {
      return this.CategoryID == key.categoryID && this.ID == key.itemID;
    }
  }
}
