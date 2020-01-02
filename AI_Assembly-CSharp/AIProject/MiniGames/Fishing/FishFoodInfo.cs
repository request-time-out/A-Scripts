// Decompiled with JetBrains decompiler
// Type: AIProject.MiniGames.Fishing.FishFoodInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using UnityEngine;

namespace AIProject.MiniGames.Fishing
{
  public class FishFoodInfo
  {
    private StuffItem RemoveItem;
    private int _count;

    public FishFoodInfo()
    {
    }

    public FishFoodInfo(
      int _itemID,
      string _foodName,
      int _rarelity1HitRange,
      int _rarelity2HitRange,
      int _rarelity3HitRange)
    {
      this.Initialize(_itemID, _foodName, _rarelity1HitRange, _rarelity2HitRange, _rarelity3HitRange);
    }

    public FishFoodInfo(
      StuffItem _stuffItem,
      Sprite _icon,
      FishFoodInfo _fishFoodInfo,
      bool _isInfinity)
    {
      this.Initialize(_stuffItem, _icon, _fishFoodInfo, _isInfinity);
    }

    public int CategoryID { get; private set; } = -1;

    public int ItemID { get; private set; } = -1;

    public string FoodName { get; private set; } = string.Empty;

    public int[] RarelityHitRange { get; private set; } = new int[3];

    public StuffItem StuffItem { get; private set; }

    public Sprite Icon { get; private set; }

    public bool IsInfinity { get; private set; }

    public int Count
    {
      get
      {
        return this._count;
      }
    }

    public void UseFood()
    {
      if (this.StuffItem == null || this.RemoveItem == null || (this._count <= 0 || !Singleton<Manager.Map>.IsInstance()))
        return;
      Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList.RemoveItem(this.RemoveItem);
      --this._count;
    }

    public void Clear()
    {
      this.CategoryID = -1;
      this.ItemID = -1;
      this.FoodName = string.Empty;
      this.RarelityHitRange[0] = 0;
      this.RarelityHitRange[1] = 0;
      this.RarelityHitRange[2] = 0;
      this.StuffItem = (StuffItem) null;
      this.Icon = (Sprite) null;
      this.RemoveItem = (StuffItem) null;
    }

    public void Initialize(
      int _itemID,
      string _foodName,
      int _rarelity1HitRange,
      int _rarelity2HitRange,
      int _rarelity3HitRange)
    {
      this.CategoryID = -1;
      this.ItemID = _itemID;
      this.FoodName = _foodName ?? string.Empty;
      this.RarelityHitRange[0] = _rarelity1HitRange;
      this.RarelityHitRange[1] = _rarelity2HitRange;
      this.RarelityHitRange[2] = _rarelity3HitRange;
      this.StuffItem = (StuffItem) null;
      this.Icon = (Sprite) null;
    }

    public void Initialize(FishFoodInfo _info)
    {
      this.Initialize(_info.ItemID, _info.FoodName, _info.RarelityHitRange[0], _info.RarelityHitRange[1], _info.RarelityHitRange[2]);
    }

    public void Initialize(
      StuffItem _stuffItem,
      Sprite _icon,
      FishFoodInfo _fishFoodInfo,
      bool _isInfinity)
    {
      this.Initialize(_fishFoodInfo);
      this.CategoryID = _stuffItem == null ? -1 : _stuffItem.CategoryID;
      this._count = _stuffItem == null ? 0 : _stuffItem.Count;
      this.StuffItem = _stuffItem;
      this.Icon = _icon;
      bool flag = _isInfinity;
      this.IsInfinity = flag;
      this.RemoveItem = !flag ? new StuffItem(_stuffItem) : (StuffItem) null;
      if (this.RemoveItem == null)
        return;
      this.RemoveItem.Count = 1;
    }

    public void AddCount(int _addCount)
    {
      this._count += _addCount;
    }
  }
}
