// Decompiled with JetBrains decompiler
// Type: AIProject.UI.RefrigeratorUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AIProject.UI
{
  public class RefrigeratorUI : ItemBoxUI
  {
    private int[] _categorize;
    private StuffItemInfo[] _craftCheck;

    [DebuggerHidden]
    public override IEnumerator SetStorage(
      ItemBoxUI.ItemBoxDataPack pack,
      Action<List<StuffItem>> action)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RefrigeratorUI.\u003CSetStorage\u003Ec__Iterator0()
      {
        pack = pack,
        action = action
      };
    }

    public override void ViewCategorize(
      out int[] categorize,
      out List<StuffItem> viewList,
      List<StuffItem> itemList)
    {
      categorize = this.categorize;
      viewList = this.Convert((IEnumerable<StuffItem>) itemList).ToList<StuffItem>();
    }

    private int[] categorize
    {
      get
      {
        return ((object) this).GetCache<int[]>(ref this._categorize, (Func<int[]>) (() =>
        {
          HashSet<int> source = new HashSet<int>();
          source.Add(0);
          foreach (int itemCategory in Singleton<Resources>.Instance.GameInfo.GetItemCategories())
          {
            Dictionary<int, StuffItemInfo> itemTable = Singleton<Resources>.Instance.GameInfo.GetItemTable(itemCategory);
            if (itemTable != null && this.IsCategorize((IEnumerable<StuffItemInfo>) itemTable.Values))
              source.Add(itemCategory);
          }
          return source.ToArray<int>();
        }));
      }
    }

    private StuffItemInfo[] craftCheck
    {
      get
      {
        return ((object) this).GetCache<StuffItemInfo[]>(ref this._craftCheck, (Func<StuffItemInfo[]>) (() =>
        {
          Resources.GameInfoTables gameInfo = Singleton<Resources>.Instance.GameInfo;
          IReadOnlyDictionary<int, RecipeDataInfo[]> cookTable = gameInfo.recipe.cookTable;
          IEnumerable<int> second = cookTable.get_Values().SelectMany<RecipeDataInfo[], int>((Func<RecipeDataInfo[], IEnumerable<int>>) (x => ((IEnumerable<RecipeDataInfo>) x).SelectMany<RecipeDataInfo, int>((Func<RecipeDataInfo, IEnumerable<int>>) (y => ((IEnumerable<RecipeDataInfo.NeedData>) y.NeedList).Select<RecipeDataInfo.NeedData, int>((Func<RecipeDataInfo.NeedData, int>) (z => z.nameHash))))));
          return cookTable.get_Keys().Concat<int>(second).Distinct<int>().Select<int, StuffItemInfo>((Func<int, StuffItemInfo>) (nameHash => gameInfo.FindItemInfo(nameHash))).ToArray<StuffItemInfo>();
        }));
      }
    }

    private IEnumerable<StuffItem> Convert(IEnumerable<StuffItem> itemList)
    {
      return itemList.Where<StuffItem>((Func<StuffItem, bool>) (item => Singleton<Resources>.Instance.GameInfo.CanEat(item) || ((IEnumerable<StuffItemInfo>) this.craftCheck).Any<StuffItemInfo>((Func<StuffItemInfo, bool>) (p => p.CategoryID == item.CategoryID && p.ID == item.ID))));
    }

    private bool IsCategorize(IEnumerable<StuffItemInfo> itemList)
    {
      return itemList.Where<StuffItemInfo>((Func<StuffItemInfo, bool>) (item => Singleton<Resources>.Instance.GameInfo.CanEat(item) || ((IEnumerable<StuffItemInfo>) this.craftCheck).Any<StuffItemInfo>((Func<StuffItemInfo, bool>) (p => p.CategoryID == item.CategoryID && p.ID == item.ID)))).Any<StuffItemInfo>();
    }
  }
}
