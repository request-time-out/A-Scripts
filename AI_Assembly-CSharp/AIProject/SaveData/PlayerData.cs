// Decompiled with JetBrains decompiler
// Type: AIProject.SaveData.PlayerData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ADV;
using MessagePack;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject.SaveData
{
  [MessagePackObject(false)]
  public class PlayerData : ICharacterInfo, IDiffComparer, IParams, ICommandData
  {
    private CharaParams _param;

    public PlayerData()
    {
    }

    public PlayerData(PlayerData source)
    {
      if (source == null)
        return;
      this.Version = source.Version;
      this.Sex = source.Sex;
      for (int index = 0; index < 2; ++index)
        this.CharaFileNames[index] = source.CharaFileNames[index];
      this.Position = source.Position;
      this.Rotation = source.Rotation;
      this.ItemList.Clear();
      foreach (StuffItem source1 in source.ItemList)
        this.ItemList.Add(new StuffItem(source1));
      if (source.LastAcquiredItem != null)
        this.LastAcquiredItem = new StuffItem(source.LastAcquiredItem);
      this.Wetness = source.Wetness;
      this.EquipedGloveItem = new StuffItem(source.EquipedGloveItem);
      this.EquipedShovelItem = new StuffItem(source.EquipedShovelItem);
      this.EquipedPickelItem = new StuffItem(source.EquipedPickelItem);
      this.EquipedNetItem = new StuffItem(source.EquipedNetItem);
      this.EquipedFishingItem = new StuffItem(source.EquipedFishingItem);
      this.EquipedHeadItem = new StuffItem(source.EquipedHeadItem);
      this.EquipedBackItem = new StuffItem(source.EquipedBackItem);
      this.EquipedNeckItem = new StuffItem(source.EquipedNeckItem);
      this.EquipedLampItem = new StuffItem(source.EquipedLampItem);
      this.SpendMoney = source.SpendMoney;
      this.FishingSkill = new Skill(source.FishingSkill);
      this.InventorySlotMax = source.InventorySlotMax;
      this.AreaID = source.AreaID;
      this.ChunkID = source.ChunkID;
      this.CraftPossibleTable.Clear();
      foreach (int num in source.CraftPossibleTable)
        this.CraftPossibleTable.Add(num);
      this.FirstCreatedItemTable.Clear();
      foreach (int num in source.FirstCreatedItemTable)
        this.FirstCreatedItemTable.Add(num);
      this.IsOnbu = source.IsOnbu;
      this.PartnerID = source.PartnerID;
      this.DateEatTrigger = source.DateEatTrigger;
    }

    [IgnoreMember]
    public CharaParams param
    {
      get
      {
        return this.GetCache<CharaParams>(ref this._param, (Func<CharaParams>) (() => new CharaParams((ICommandData) this, "P")));
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
    public byte Sex { get; set; }

    [Key(2)]
    public string[] CharaFileNames { get; set; } = new string[2];

    [IgnoreMember]
    public string CharaFileName
    {
      get
      {
        return this.Sex == (byte) 0 ? this.CharaFileNames[0] : this.CharaFileNames[1];
      }
    }

    [Key(3)]
    public Vector3 Position { get; set; }

    [Key(4)]
    public Quaternion Rotation { get; set; }

    [Key(5)]
    public List<StuffItem> ItemList { get; set; } = new List<StuffItem>();

    [Key(6)]
    public StuffItem LastAcquiredItem { get; set; }

    [Key(7)]
    public float Wetness { get; set; }

    public StuffItem EquipedSearchItem(int id)
    {
      switch (id)
      {
        case 0:
        case 1:
        case 2:
          return this.EquipedGloveItem;
        case 3:
          return this.EquipedShovelItem;
        case 4:
          return this.EquipedPickelItem;
        case 5:
          return this.EquipedNetItem;
        case 6:
          return this.EquipedFishingItem;
        default:
          return (StuffItem) null;
      }
    }

    [Key(8)]
    public StuffItem EquipedGloveItem { get; set; } = new StuffItem()
    {
      ID = -1
    };

    [Key(9)]
    public StuffItem EquipedShovelItem { get; set; } = new StuffItem()
    {
      ID = -1
    };

    [Key(10)]
    public StuffItem EquipedPickelItem { get; set; } = new StuffItem()
    {
      ID = -1
    };

    [Key(11)]
    public StuffItem EquipedNetItem { get; set; } = new StuffItem()
    {
      ID = -1
    };

    [Key(12)]
    public StuffItem EquipedFishingItem { get; set; } = new StuffItem()
    {
      ID = -1
    };

    [Key(13)]
    public StuffItem EquipedHeadItem { get; set; } = new StuffItem()
    {
      ID = -1
    };

    [Key(14)]
    public StuffItem EquipedBackItem { get; set; } = new StuffItem()
    {
      ID = -1
    };

    [Key(15)]
    public StuffItem EquipedNeckItem { get; set; } = new StuffItem()
    {
      ID = -1
    };

    [Key(16)]
    public StuffItem EquipedLampItem { get; set; } = new StuffItem()
    {
      ID = -1
    };

    [Key(17)]
    public int SpendMoney { get; set; }

    [Key(18)]
    public Skill FishingSkill { get; set; } = new Skill();

    [Key(19)]
    public int InventorySlotMax { get; set; } = 1;

    [Key(20)]
    public int AreaID { get; set; }

    [Key(21)]
    public int ChunkID { get; set; }

    [Key(22)]
    public HashSet<int> CraftPossibleTable { get; set; } = new HashSet<int>();

    [Key(23)]
    public HashSet<int> FirstCreatedItemTable { get; set; } = new HashSet<int>();

    [Key(24)]
    public bool IsOnbu { get; set; }

    [Key(25)]
    public int? PartnerID { get; set; }

    [Key(26)]
    public bool DateEatTrigger { get; set; }

    public void Copy(PlayerData source)
    {
      if (source == null)
        return;
      this.Version = source.Version;
      this.Sex = source.Sex;
      for (int index = 0; index < 2; ++index)
        this.CharaFileNames[index] = source.CharaFileNames[index];
      this.Position = source.Position;
      this.Rotation = source.Rotation;
      this.ItemList.Clear();
      foreach (StuffItem source1 in source.ItemList)
        this.ItemList.Add(new StuffItem(source1));
      if (source.LastAcquiredItem != null)
        this.LastAcquiredItem = new StuffItem(source.LastAcquiredItem);
      this.Wetness = source.Wetness;
      this.EquipedGloveItem = new StuffItem(source.EquipedGloveItem);
      this.EquipedShovelItem = new StuffItem(source.EquipedShovelItem);
      this.EquipedPickelItem = new StuffItem(source.EquipedPickelItem);
      this.EquipedNetItem = new StuffItem(source.EquipedNetItem);
      this.EquipedFishingItem = new StuffItem(source.EquipedFishingItem);
      this.EquipedHeadItem = new StuffItem(source.EquipedHeadItem);
      this.EquipedBackItem = new StuffItem(source.EquipedBackItem);
      this.EquipedNeckItem = new StuffItem(source.EquipedNeckItem);
      this.EquipedLampItem = new StuffItem(source.EquipedLampItem);
      this.SpendMoney = source.SpendMoney;
      this.FishingSkill = new Skill(source.FishingSkill);
      this.InventorySlotMax = source.InventorySlotMax;
      this.AreaID = source.AreaID;
      this.ChunkID = source.ChunkID;
      this.CraftPossibleTable.Clear();
      foreach (int num in source.CraftPossibleTable)
        this.CraftPossibleTable.Add(num);
      this.FirstCreatedItemTable.Clear();
      foreach (int num in source.FirstCreatedItemTable)
        this.FirstCreatedItemTable.Add(num);
      this.IsOnbu = source.IsOnbu;
      this.PartnerID = source.PartnerID;
      this.DateEatTrigger = source.DateEatTrigger;
    }

    public void ComplementDiff()
    {
      if (this.Version < new System.Version("0.0.1"))
      {
        this.IsOnbu = false;
        this.PartnerID = new int?();
      }
      this.Version = AIProject.Definitions.Version.PlayerDataVersion;
    }

    public void UpdateDiff()
    {
      this.ItemList.OrganizeItemList();
    }

    public IEnumerable<CommandData> CreateCommandData(string head)
    {
      List<CommandData> commandDataList = new List<CommandData>();
      string key = head + "Sex";
      commandDataList.Add(new CommandData(CommandData.Command.Int, key, (Func<object>) (() => (object) (int) this.Sex), (System.Action<object>) null));
      return (IEnumerable<CommandData>) commandDataList;
    }
  }
}
