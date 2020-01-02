// Decompiled with JetBrains decompiler
// Type: AIProject.SaveData.MerchantData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ADV;
using AIProject.Definitions;
using Illusion.Extensions;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIProject.SaveData
{
  [MessagePackObject(false)]
  public class MerchantData : ICharacterInfo, IDiffComparer, IParams, ICommandData
  {
    private CharaParams _param;
    private bool isSpecialVendorLoad;

    public MerchantData()
    {
    }

    public MerchantData(MerchantData _data)
    {
      this.Copy(_data);
    }

    [IgnoreMember]
    public CharaParams param
    {
      get
      {
        return this.GetCache<CharaParams>(ref this._param, (Func<CharaParams>) (() => new CharaParams((ICommandData) this, "M")));
      }
    }

    [IgnoreMember]
    Params IParams.param
    {
      get
      {
        return (Params) this.param;
      }
    }

    [Key(0)]
    public System.Version Version { get; set; } = new System.Version();

    [Key(1)]
    public string CharaFileName { get; set; } = string.Empty;

    [Key(2)]
    public int ChunkID { get; set; }

    [Key(3)]
    public int AreaID { get; set; }

    [Key(4)]
    public int ActionTargetID { get; set; } = -1;

    [Key(5)]
    public bool Unlock { get; set; }

    [Key(6)]
    public Merchant.ActionType ModeType { get; set; } = Merchant.ActionType.ToWait;

    [Key(7)]
    public Merchant.ActionType LastNormalModeType { get; set; } = Merchant.ActionType.ToWait;

    [Key(8)]
    public Merchant.StateType StateType { get; set; } = Merchant.StateType.Wait;

    [Key(9)]
    public Vector3 Position { get; set; }

    [Key(10)]
    public Quaternion Rotation { get; set; }

    [Key(11)]
    public List<StuffItem> ItemList { get; set; } = new List<StuffItem>();

    [Key(12)]
    public MerchantActor.MerchantSchedule CurrentSchedule { get; set; }

    [Key(13)]
    public MerchantActor.MerchantSchedule PrevSchedule { get; set; }

    [Key(14)]
    public List<MerchantActor.MerchantSchedule> ScheduleList { get; set; } = new List<MerchantActor.MerchantSchedule>();

    [Key(15)]
    public int PointID { get; set; } = -1;

    [Key(16)]
    public int PointAreaID { get; set; }

    [Key(17)]
    public int PointGroupID { get; set; }

    [Key(18)]
    public List<MerchantData.VendorItem> vendorItemList { get; set; } = new List<MerchantData.VendorItem>();

    [Key(19)]
    public List<MerchantData.VendorItem> vendorSpItemList { get; set; } = new List<MerchantData.VendorItem>();

    [Key(20)]
    public int OpenAreaID { get; set; }

    [Key(21)]
    public Dictionary<int, MerchantData.VendorItem> vendorSpItemTable { get; set; } = new Dictionary<int, MerchantData.VendorItem>();

    [Key(22)]
    public bool isThereafterH { get; set; }

    [Key(23)]
    public float Wetness { get; set; }

    [Key(24)]
    public int MapID { get; set; }

    [Key(25)]
    public Vector3 PointPosition { get; set; } = Vector3.get_zero();

    [Key(26)]
    public bool ElapsedDay { get; set; }

    [Key(27)]
    public Vector3 MainPointPosition { get; set; } = Vector3.get_zero();

    public void UpdateDiff()
    {
      this.ItemList.OrganizeItemList();
    }

    public void Copy(MerchantData _data)
    {
      this.CharaFileName = _data.CharaFileName;
      this.ChunkID = _data.ChunkID;
      this.AreaID = _data.AreaID;
      this.ActionTargetID = _data.ActionTargetID;
      this.Unlock = _data.Unlock;
      this.ModeType = _data.ModeType;
      this.LastNormalModeType = _data.LastNormalModeType;
      this.StateType = _data.StateType;
      this.Position = _data.Position;
      this.Rotation = _data.Rotation;
      this.ItemList = _data.ItemList.Select<StuffItem, StuffItem>((Func<StuffItem, StuffItem>) (x => new StuffItem(x))).ToList<StuffItem>();
      if (_data.CurrentSchedule != null)
        this.CurrentSchedule = new MerchantActor.MerchantSchedule(_data.CurrentSchedule);
      if (_data.PrevSchedule != null)
        this.PrevSchedule = new MerchantActor.MerchantSchedule(_data.PrevSchedule);
      this.ScheduleList = _data.ScheduleList.Select<MerchantActor.MerchantSchedule, MerchantActor.MerchantSchedule>((Func<MerchantActor.MerchantSchedule, MerchantActor.MerchantSchedule>) (x => new MerchantActor.MerchantSchedule(x))).ToList<MerchantActor.MerchantSchedule>();
      this.PointID = _data.PointID;
      this.PointAreaID = _data.PointAreaID;
      this.PointGroupID = _data.PointGroupID;
      this.vendorItemList = _data.vendorItemList.Select<MerchantData.VendorItem, MerchantData.VendorItem>((Func<MerchantData.VendorItem, MerchantData.VendorItem>) (x => new MerchantData.VendorItem(x))).ToList<MerchantData.VendorItem>();
      if (_data.vendorSpItemTable != null)
        this.vendorSpItemTable = _data.vendorSpItemTable.ToDictionary<KeyValuePair<int, MerchantData.VendorItem>, int, MerchantData.VendorItem>((Func<KeyValuePair<int, MerchantData.VendorItem>, int>) (v => v.Key), (Func<KeyValuePair<int, MerchantData.VendorItem>, MerchantData.VendorItem>) (v => new MerchantData.VendorItem(v.Value)));
      else if (_data.vendorSpItemList != null)
      {
        // ISSUE: object of a compiler-generated type is created
        this.vendorSpItemTable = _data.vendorSpItemList.Select<MerchantData.VendorItem, \u003C\u003E__AnonType11<int, MerchantData.VendorItem>>((Func<MerchantData.VendorItem, int, \u003C\u003E__AnonType11<int, MerchantData.VendorItem>>) ((Value, Key) => new \u003C\u003E__AnonType11<int, MerchantData.VendorItem>(Key, Value))).ToDictionary<\u003C\u003E__AnonType11<int, MerchantData.VendorItem>, int, MerchantData.VendorItem>((Func<\u003C\u003E__AnonType11<int, MerchantData.VendorItem>, int>) (v => v.Key), (Func<\u003C\u003E__AnonType11<int, MerchantData.VendorItem>, MerchantData.VendorItem>) (v => new MerchantData.VendorItem(v.Value)));
      }
      this.OpenAreaID = _data.OpenAreaID;
      this.isThereafterH = _data.isThereafterH;
      this.Wetness = _data.Wetness;
      this.MapID = _data.MapID;
      this.PointPosition = _data.PointPosition;
      this.ElapsedDay = _data.ElapsedDay;
      this.MainPointPosition = _data.MainPointPosition;
    }

    public void ComplementDiff()
    {
    }

    public void ResetVendor(
      IReadOnlyDictionary<int, List<VendItemInfo>> vendTable)
    {
      VendItemInfo[] array = vendTable.get_Keys().SelectMany<int, VendItemInfo>((Func<int, IEnumerable<VendItemInfo>>) (i => (IEnumerable<VendItemInfo>) MerchantData.GetSaleChoice((IReadOnlyCollection<VendItemInfo>) vendTable.get_Item(i), 2))).ToArray<VendItemInfo>();
      this.vendorItemList.Clear();
      this.vendorItemList.AddRange(((IEnumerable<VendItemInfo>) array).Select<VendItemInfo, MerchantData.VendorItem>((Func<VendItemInfo, MerchantData.VendorItem>) (p =>
      {
        int num = p.Stocks.Length != 1 ? Enumerable.Range(p.Stocks[0], p.Stocks[1] - p.Stocks[0]).Shuffle<int>().First<int>() : p.Stocks[0];
        return new MerchantData.VendorItem(p.CategoryID, p.ID, num, p.Rate, num);
      })));
      Debug.Log((object) "ショップアイテム更新");
    }

    public void ResetSpecialVendor(
      IReadOnlyDictionary<int, VendItemInfo> specialTable)
    {
      if (this.isSpecialVendorLoad)
        return;
      if (this.vendorSpItemTable == null)
        this.vendorSpItemTable = new Dictionary<int, MerchantData.VendorItem>();
      HashSet<int> intSet = new HashSet<int>((IEnumerable<int>) this.vendorSpItemTable.Keys);
      foreach (KeyValuePair<int, VendItemInfo> keyValuePair in (IEnumerable<KeyValuePair<int, VendItemInfo>>) specialTable)
      {
        VendItemInfo vendItemInfo = keyValuePair.Value;
        intSet.Remove(keyValuePair.Key);
        int num = Mathf.Max(1, vendItemInfo.Stocks[0]);
        MerchantData.VendorItem vendorItem;
        if (this.vendorSpItemTable.TryGetValue(keyValuePair.Key, out vendorItem))
        {
          vendorItem.CategoryID = vendItemInfo.CategoryID;
          vendorItem.ID = vendItemInfo.ID;
          vendorItem.Rate = vendItemInfo.Rate;
          vendorItem.Count += num - Mathf.Max(1, vendorItem.Stock);
          vendorItem.Count = Mathf.Max(0, vendorItem.Count);
          vendorItem.Stock = num;
        }
        else
          this.vendorSpItemTable[keyValuePair.Key] = new MerchantData.VendorItem(vendItemInfo.CategoryID, vendItemInfo.ID, num, vendItemInfo.Rate, num);
      }
      foreach (int key in intSet)
        this.vendorSpItemTable.Remove(key);
      this.vendorSpItemTable = this.vendorSpItemTable.OrderBy<KeyValuePair<int, MerchantData.VendorItem>, int>((Func<KeyValuePair<int, MerchantData.VendorItem>, int>) (v => v.Key)).ToDictionary<KeyValuePair<int, MerchantData.VendorItem>, int, MerchantData.VendorItem>((Func<KeyValuePair<int, MerchantData.VendorItem>, int>) (v => v.Key), (Func<KeyValuePair<int, MerchantData.VendorItem>, MerchantData.VendorItem>) (v => v.Value));
      this.isSpecialVendorLoad = true;
      Debug.Log((object) "ショップスペシャルアイテム更新");
    }

    private static List<VendItemInfo> GetSaleChoice(
      IReadOnlyCollection<VendItemInfo> vendor,
      int num)
    {
      List<VendItemInfo> vendItemInfoList = new List<VendItemInfo>();
      Dictionary<VendItemInfo, int> dictionary = ((IEnumerable<VendItemInfo>) vendor).ToDictionary<VendItemInfo, VendItemInfo, int>((Func<VendItemInfo, VendItemInfo>) (p => p), (Func<VendItemInfo, int>) (p => p.Percent));
      if (!dictionary.Any<KeyValuePair<VendItemInfo, int>>())
        return vendItemInfoList;
      for (int index = 0; index < num; ++index)
      {
        VendItemInfo fromDict = Illusion.Utils.ProbabilityCalclator.DetermineFromDict<VendItemInfo>(dictionary);
        if (dictionary.Remove(fromDict))
          vendItemInfoList.Add(fromDict);
        else
          break;
      }
      return vendItemInfoList;
    }

    public IEnumerable<CommandData> CreateCommandData(string head)
    {
      List<CommandData> list = new List<CommandData>();
      string key1 = head + "ModeType";
      list.Add(new CommandData(CommandData.Command.String, key1, (Func<object>) (() => (object) this.ModeType.ToString()), (System.Action<object>) null));
      string key2 = head + "LastNormalModeType";
      list.Add(new CommandData(CommandData.Command.String, key2, (Func<object>) (() => (object) this.LastNormalModeType.ToString()), (System.Action<object>) null));
      string key3 = head + "StateType";
      list.Add(new CommandData(CommandData.Command.String, key3, (Func<object>) (() => (object) this.StateType.ToString()), (System.Action<object>) null));
      string str = head + "ItemList";
      // ISSUE: object of a compiler-generated type is created
      foreach (\u003C\u003E__AnonType10<StuffItem, int> anonType10 in this.ItemList.Select<StuffItem, \u003C\u003E__AnonType10<StuffItem, int>>((Func<StuffItem, int, \u003C\u003E__AnonType10<StuffItem, int>>) ((value, index) => new \u003C\u003E__AnonType10<StuffItem, int>(value, index))))
        anonType10.value.AddList(list, str + string.Format("[{0}]", (object) anonType10.index));
      return (IEnumerable<CommandData>) list;
    }

    [MessagePackObject(false)]
    public class VendorItem : StuffItem
    {
      public VendorItem(int category, int id, int count, int rate, int stock)
        : this(category, id, count)
      {
        this.Rate = rate;
        this.Stock = stock;
      }

      public VendorItem(int category, int id, int count)
        : base(category, id, count)
      {
      }

      public VendorItem(MerchantData.VendorItem source)
        : base((StuffItem) source)
      {
        this.Rate = source.Rate;
        this.Stock = source.Stock;
      }

      [Key(4)]
      public int Rate { get; set; }

      [Key(5)]
      public int Stock { get; set; } = 1;
    }
  }
}
