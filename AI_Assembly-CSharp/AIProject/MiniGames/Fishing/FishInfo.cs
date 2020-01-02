// Decompiled with JetBrains decompiler
// Type: AIProject.MiniGames.Fishing.FishInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace AIProject.MiniGames.Fishing
{
  public struct FishInfo
  {
    public FishInfo(
      int _categoryID,
      int _itemID,
      string _itemName,
      int _sizeID,
      int _modelID,
      int _heartPoint,
      int _minExPoint,
      int _maxExPoint,
      int _tankPointID,
      float _nicknameHeightOffset)
    {
      this.CategoryID = _categoryID;
      this.ItemID = _itemID;
      this.ItemName = _itemName;
      this.SizeID = _sizeID;
      this.ModelID = _modelID;
      this.HeartPoint = _heartPoint;
      this.MinExPoint = _minExPoint;
      this.MaxExPoint = _maxExPoint;
      this.TankPointID = _tankPointID;
      this.NicknameHeightOffset = _nicknameHeightOffset;
    }

    public int CategoryID { get; private set; }

    public int ItemID { get; private set; }

    public string ItemName { get; private set; }

    public int SizeID { get; private set; }

    public int ModelID { get; private set; }

    public int HeartPoint { get; private set; }

    public int MinExPoint { get; private set; }

    public int MaxExPoint { get; private set; }

    public int TankPointID { get; private set; }

    public float NicknameHeightOffset { get; private set; }

    public bool IsActive
    {
      get
      {
        return 0 < this.HeartPoint && !this.ItemName.IsNullOrEmpty();
      }
    }
  }
}
