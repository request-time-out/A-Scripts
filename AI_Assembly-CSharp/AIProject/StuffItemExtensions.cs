// Decompiled with JetBrains decompiler
// Type: AIProject.StuffItemExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIProject
{
  public static class StuffItemExtensions
  {
    public static StuffItem FindItem(this List<StuffItem> self, StuffItem item)
    {
      return item == null ? (StuffItem) null : self.Find((Predicate<StuffItem>) (p => p.CategoryID == item.CategoryID && p.ID == item.ID));
    }

    public static StuffItem[] FindItems(this IEnumerable<StuffItem> self, StuffItem item)
    {
      return item == null ? new StuffItem[0] : self.Where<StuffItem>((Func<StuffItem, bool>) (node => item.CategoryID == node.CategoryID && item.ID == node.ID)).ToArray<StuffItem>();
    }

    public static bool AddItem(this ICollection<StuffItem> self, StuffItem item)
    {
      return self.AddItem(item, item.Count);
    }

    public static bool AddItem(this ICollection<StuffItem> self, StuffItem item, int count)
    {
      item.LatestDateTime = DateTime.Now;
      int itemSlotMax = Singleton<Resources>.Instance.DefinePack.MapDefines.ItemSlotMax;
      foreach (StuffItem stuffItem in (IEnumerable<StuffItem>) ((IEnumerable<StuffItem>) self.FindItems(item)).OrderByDescending<StuffItem, int>((Func<StuffItem, int>) (x => x.Count)))
      {
        int num1 = itemSlotMax - stuffItem.Count;
        if (num1 != 0)
        {
          int num2 = count - num1;
          if (num2 > 0)
          {
            stuffItem.Count = itemSlotMax;
            count = num2;
          }
          else
          {
            stuffItem.Count += count;
            count = 0;
          }
          stuffItem.LatestDateTime = item.LatestDateTime;
          if (count <= 0)
            break;
        }
      }
      bool flag = false;
      int num;
      for (; count > 0; count = num)
      {
        flag = true;
        num = count - itemSlotMax;
        if (num > 0)
        {
          item.Count = itemSlotMax;
          self.Add(new StuffItem(item));
        }
        else
        {
          item.Count = count;
          self.Add(item);
          count = 0;
          break;
        }
      }
      return flag;
    }

    public static bool AddItem(
      this ICollection<StuffItem> self,
      StuffItem item,
      int count,
      int maxSlot)
    {
      item.LatestDateTime = DateTime.Now;
      int itemSlotMax = Singleton<Resources>.Instance.DefinePack.MapDefines.ItemSlotMax;
      foreach (StuffItem stuffItem in (IEnumerable<StuffItem>) ((IEnumerable<StuffItem>) self.FindItems(item)).OrderByDescending<StuffItem, int>((Func<StuffItem, int>) (x => x.Count)))
      {
        int num1 = itemSlotMax - stuffItem.Count;
        if (num1 != 0)
        {
          int num2 = count - num1;
          if (num2 > 0)
          {
            stuffItem.Count = itemSlotMax;
            count = num2;
          }
          else
          {
            stuffItem.Count += count;
            count = 0;
          }
          stuffItem.LatestDateTime = item.LatestDateTime;
          if (count <= 0)
            break;
        }
      }
      bool flag = false;
      int num;
      for (; count > 0 && self.Count < maxSlot; count = num)
      {
        flag = true;
        num = count - itemSlotMax;
        if (num > 0)
        {
          item.Count = itemSlotMax;
          self.Add(new StuffItem(item));
        }
        else
        {
          item.Count = count;
          self.Add(item);
          count = 0;
          break;
        }
      }
      return flag;
    }

    public static bool RemoveItem(this List<StuffItem> self, StuffItem item)
    {
      int count = item.Count;
      return self.RemoveItem(item, ref count);
    }

    public static bool RemoveItem(this List<StuffItem> self, StuffItem item, ref int count)
    {
      bool flag = false;
      foreach (StuffItem stuffItem in (IEnumerable<StuffItem>) ((IEnumerable<StuffItem>) self.FindItems(item)).OrderBy<StuffItem, int>((Func<StuffItem, int>) (x => x.Count)))
      {
        int num = count - stuffItem.Count;
        if (num >= 0)
        {
          stuffItem.Count = 0;
          count = num;
          flag = true;
        }
        else
        {
          stuffItem.Count -= count;
          count = 0;
          break;
        }
      }
      if (flag)
        self.RemoveAll((Predicate<StuffItem>) (x => x.Count <= 0));
      return flag;
    }

    public static bool CanAddItem(
      this IReadOnlyCollection<StuffItem> self,
      int capacity,
      StuffItem item)
    {
      return self.CanAddItem(capacity, item, item.Count);
    }

    public static bool CanAddItem(
      this IReadOnlyCollection<StuffItem> self,
      int capacity,
      StuffItem item,
      out int possible)
    {
      return self.CanAddItem(capacity, item, item.Count, out possible);
    }

    public static bool CanAddItem(
      this IReadOnlyCollection<StuffItem> self,
      int capacity,
      StuffItem item,
      int count)
    {
      return self.CanAddItem(capacity, item, count, out int _);
    }

    public static bool CanAddItem(
      this IReadOnlyCollection<StuffItem> self,
      int capacity,
      StuffItem item,
      int count,
      out int possible)
    {
      int ItemSlotMax = Singleton<Resources>.Instance.DefinePack.MapDefines.ItemSlotMax;
      int num1 = ((IEnumerable<StuffItem>) self).FindItems(item).Sum<StuffItem>((Func<StuffItem, int>) (x => ItemSlotMax - x.Count));
      int num2 = count - num1;
      if (num2 <= 0)
        num2 = 0;
      int num3 = num2 / ItemSlotMax + (num2 % ItemSlotMax <= 0 ? 0 : 1);
      int num4 = capacity - self.get_Count() - num3;
      possible = num1;
      if (num4 > 0)
        possible += ItemSlotMax * num4;
      if (num4 >= 0 && num2 > 0)
        possible += ItemSlotMax - num2;
      if (num4 >= 0)
        return true;
      return possible > 0 && possible >= count;
    }
  }
}
