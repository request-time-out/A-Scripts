// Decompiled with JetBrains decompiler
// Type: CraftResource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

public class CraftResource : Singleton<CraftResource>
{
  public bool bEnd;
  private string _AssetBandlePath;
  private ReadOnlyDictionary<int, ReadOnlyDictionary<int, CraftItemInfo>> _itemTables;

  protected override void Awake()
  {
    this.bEnd = false;
    this.Init();
  }

  public void Init()
  {
    this._AssetBandlePath = "craft/list/";
    this.StartCoroutine("LoadCraftItemList");
  }

  public int[] GetCraftItemCategories()
  {
    return ((IEnumerable<int>) this._itemTables.get_Keys()).ToArray<int>();
  }

  public ReadOnlyDictionary<int, CraftItemInfo> GetItemTable(int id)
  {
    return this._itemTables.get_Item(id);
  }

  public CraftItemInfo GetItem(int category, int id)
  {
    ReadOnlyDictionary<int, ReadOnlyDictionary<int, CraftItemInfo>> itemTables = this._itemTables;
    if (itemTables == null)
      return (CraftItemInfo) null;
    CraftItemInfo craftItemInfo;
    return !itemTables.get_Item(category).TryGetValue(id, ref craftItemInfo) ? (CraftItemInfo) null : craftItemInfo;
  }

  [DebuggerHidden]
  private IEnumerator LoadCraftItemList()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new CraftResource.\u003CLoadCraftItemList\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }
}
