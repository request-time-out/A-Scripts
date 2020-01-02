// Decompiled with JetBrains decompiler
// Type: AIProject.SaveData.WorldData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Animal;
using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AIProject.SaveData
{
  [MessagePackObject(false)]
  public class WorldData
  {
    [Key(12)]
    public Dictionary<int, bool> TutorialOpenStateTable = new Dictionary<int, bool>();

    [Key(0)]
    private Version Version { get; set; } = new Version();

    [Key(1)]
    public int WorldID { get; set; }

    [Key(2)]
    public bool FreeMode { get; set; }

    [IgnoreMember]
    public DateTime SaveTime { get; set; }

    [Key(4)]
    public string Name { get; set; }

    [Key(5)]
    public Environment Environment { get; set; } = new Environment();

    [Key(6)]
    public Dictionary<int, AgentData> AgentTable { get; set; } = new Dictionary<int, AgentData>();

    [Key(7)]
    public PlayerData PlayerData { get; set; } = new PlayerData();

    [Key(8)]
    public MerchantData MerchantData { get; set; } = new MerchantData();

    [Key(9)]
    public HousingData HousingData { get; set; } = new HousingData();

    [Key(10)]
    public bool Cleared { get; set; }

    [Key(11)]
    public string SaveTimeString { get; set; }

    [Key(13)]
    public Dictionary<AnimalTypes, Dictionary<int, WildAnimalData>> WildAnimalTable { get; set; } = new Dictionary<AnimalTypes, Dictionary<int, WildAnimalData>>();

    [Key(14)]
    public int MapID { get; set; }

    public void Copy(WorldData source)
    {
      this.WorldID = source.WorldID;
      this.FreeMode = source.FreeMode;
      this.SaveTime = source.SaveTime;
      this.Name = source.Name;
      this.Environment.Copy(source.Environment);
      foreach (KeyValuePair<int, AgentData> keyValuePair in source.AgentTable)
      {
        AgentData agentData1;
        if (!this.AgentTable.TryGetValue(keyValuePair.Key, out agentData1))
        {
          AgentData agentData2 = new AgentData();
          this.AgentTable[keyValuePair.Key] = agentData2;
          agentData1 = agentData2;
        }
        agentData1.Copy(keyValuePair.Value);
        agentData1.param.Bind(keyValuePair.Value.param.actor);
      }
      if (this.PlayerData == null)
        this.PlayerData = new PlayerData();
      this.PlayerData.Copy(source.PlayerData);
      this.PlayerData.param.Bind(source.PlayerData.param.actor);
      if (this.MerchantData == null)
        this.MerchantData = new MerchantData();
      this.MerchantData.Copy(source.MerchantData);
      this.MerchantData.param.Bind(source.MerchantData.param.actor);
      this.HousingData = new HousingData(source.HousingData);
      this.HousingData.CopyInstances(source.HousingData);
      this.Cleared = source.Cleared;
      this.SaveTimeString = source.SaveTimeString;
      if (!source.TutorialOpenStateTable.IsNullOrEmpty<int, bool>())
        this.TutorialOpenStateTable = source.TutorialOpenStateTable.ToDictionary<KeyValuePair<int, bool>, int, bool>((Func<KeyValuePair<int, bool>, int>) (x => x.Key), (Func<KeyValuePair<int, bool>, bool>) (x => x.Value));
      else
        this.TutorialOpenStateTable.Clear();
      if (!source.WildAnimalTable.IsNullOrEmpty<AnimalTypes, Dictionary<int, WildAnimalData>>())
        this.WildAnimalTable = source.WildAnimalTable.Where<KeyValuePair<AnimalTypes, Dictionary<int, WildAnimalData>>>((Func<KeyValuePair<AnimalTypes, Dictionary<int, WildAnimalData>>, bool>) (x => !x.Value.IsNullOrEmpty<int, WildAnimalData>())).ToDictionary<KeyValuePair<AnimalTypes, Dictionary<int, WildAnimalData>>, AnimalTypes, Dictionary<int, WildAnimalData>>((Func<KeyValuePair<AnimalTypes, Dictionary<int, WildAnimalData>>, AnimalTypes>) (x => x.Key), (Func<KeyValuePair<AnimalTypes, Dictionary<int, WildAnimalData>>, Dictionary<int, WildAnimalData>>) (x => x.Value.Where<KeyValuePair<int, WildAnimalData>>((Func<KeyValuePair<int, WildAnimalData>, bool>) (y => y.Value != null)).ToDictionary<KeyValuePair<int, WildAnimalData>, int, WildAnimalData>((Func<KeyValuePair<int, WildAnimalData>, int>) (y => y.Key), (Func<KeyValuePair<int, WildAnimalData>, WildAnimalData>) (y => new WildAnimalData(y.Value)))));
      else
        this.WildAnimalTable.Clear();
      this.MapID = source.MapID;
    }

    public void ComplementDiff()
    {
      DateTime result;
      this.SaveTime = !DateTime.TryParse(this.SaveTimeString, out result) ? DateTime.MinValue : result;
      this.PlayerData.ComplementDiff();
      foreach (KeyValuePair<int, AgentData> keyValuePair in this.AgentTable)
        keyValuePair.Value.ComplementDiff();
      this.MerchantData.ComplementDiff();
      if (this.HousingData != null)
        this.HousingData.UpdateDiff();
      else
        this.HousingData = new HousingData();
    }

    public void SaveFile(byte[] buffer)
    {
      using (MemoryStream memoryStream = new MemoryStream(buffer))
        this.SaveFile((Stream) memoryStream);
    }

    public void SaveFile(string path)
    {
      using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
        this.SaveFile((Stream) fileStream);
    }

    public void SaveFile(Stream stream)
    {
      using (BinaryWriter writer = new BinaryWriter(stream))
        this.SaveFile(writer);
    }

    public void SaveFile(BinaryWriter writer)
    {
      byte[] buffer = MessagePackSerializer.Serialize<WorldData>((M0) this);
      writer.Write(buffer);
    }

    public static WorldData LoadFile(string fileName)
    {
      WorldData worldData = new WorldData();
      return worldData.Load(fileName) ? worldData : (WorldData) null;
    }

    public bool Load(string fileName)
    {
      try
      {
        using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
          if (fileStream.Length != 0L)
            return this.Load((Stream) fileStream);
          Debug.LogError((object) "空データ");
          return false;
        }
      }
      catch (Exception ex)
      {
        if (ex is FileNotFoundException)
        {
          Debug.Log((object) ("セーブデータが見つからないので新規作成: " + fileName));
          return false;
        }
        Debug.LogException(ex);
        return false;
      }
    }

    public bool Load(Stream stream)
    {
      using (BinaryReader reader = new BinaryReader(stream))
        return this.Load(reader);
    }

    public bool Load(BinaryReader reader)
    {
      try
      {
        byte[] source1 = reader.ReadBytes((int) reader.BaseStream.Length);
        if (source1.IsNullOrEmpty<byte>())
          return false;
        WorldData source2 = (WorldData) MessagePackSerializer.Deserialize<WorldData>(source1);
        source2.CheckDiff();
        this.Copy(source2);
        return true;
      }
      catch (Exception ex)
      {
        Debug.LogException(ex);
      }
      return false;
    }

    private void CheckDiff()
    {
      this.Environment.UpdateDiff();
      foreach (KeyValuePair<int, AgentData> keyValuePair in this.AgentTable)
        keyValuePair.Value.UpdateDiff();
      if (this.PlayerData != null)
        this.PlayerData.UpdateDiff();
      else
        this.PlayerData = new PlayerData();
      if (this.MerchantData != null)
        this.MerchantData.UpdateDiff();
      else
        this.MerchantData = new MerchantData();
      if (this.HousingData != null)
        this.HousingData.UpdateDiff();
      else
        this.HousingData = new HousingData();
    }
  }
}
