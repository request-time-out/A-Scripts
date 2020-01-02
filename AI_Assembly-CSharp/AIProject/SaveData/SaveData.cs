// Decompiled with JetBrains decompiler
// Type: AIProject.SaveData.SaveData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AIProject.SaveData
{
  [MessagePackObject(false)]
  public class SaveData
  {
    [Key(0)]
    public WorldData AutoData { get; set; }

    [Key(1)]
    public Dictionary<int, WorldData> WorldList { get; set; } = new Dictionary<int, WorldData>();

    public void SaveFile(byte[] buffer)
    {
      using (MemoryStream memoryStream = new MemoryStream(buffer))
        this.SaveFile((Stream) memoryStream);
    }

    public void SaveFile(string path)
    {
      using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
        this.SaveFile((Stream) fileStream);
    }

    public void SaveFile(Stream stream)
    {
      using (BinaryWriter writer = new BinaryWriter(stream))
        this.SaveFile(writer);
    }

    public void SaveFile(BinaryWriter writer)
    {
      byte[] buffer = MessagePackSerializer.Serialize<AIProject.SaveData.SaveData>((M0) this);
      writer.Write(buffer);
    }

    public static AIProject.SaveData.SaveData LoadFile(string fileName)
    {
      AIProject.SaveData.SaveData saveData = new AIProject.SaveData.SaveData();
      if (!saveData.Load(fileName))
        return (AIProject.SaveData.SaveData) null;
      saveData.ComplementDiff();
      return saveData;
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
          Debug.Log((object) string.Format("セーブデータが見つからないので新規作成: {0}", (object) fileName));
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
        byte[] source = reader.ReadBytes((int) reader.BaseStream.Length);
        if (source.IsNullOrEmpty<byte>())
          return false;
        this.Copy((AIProject.SaveData.SaveData) MessagePackSerializer.Deserialize<AIProject.SaveData.SaveData>(source));
        return true;
      }
      catch (Exception ex)
      {
        Debug.LogException(ex);
      }
      return false;
    }

    public void Copy(AIProject.SaveData.SaveData source)
    {
      if (source.AutoData != null)
      {
        this.AutoData = new WorldData();
        this.AutoData.Copy(source.AutoData);
      }
      foreach (KeyValuePair<int, WorldData> world in source.WorldList)
      {
        WorldData worldData = new WorldData();
        worldData.Copy(world.Value);
        this.WorldList[world.Key] = worldData;
      }
    }

    public void ComplementDiff()
    {
      this.AutoData?.ComplementDiff();
      foreach (KeyValuePair<int, WorldData> world in this.WorldList)
      {
        if (world.Value != null)
          world.Value.ComplementDiff();
      }
    }
  }
}
