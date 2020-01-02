// Decompiled with JetBrains decompiler
// Type: AIProject.GlobalSaveData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using MessagePack;
using System;
using System.IO;
using UnityEngine;

namespace AIProject
{
  [MessagePackObject(false)]
  public class GlobalSaveData
  {
    [Key(0)]
    public bool Cleared { get; set; }

    public void Copy(GlobalSaveData source)
    {
      this.Cleared = source.Cleared;
    }

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
      byte[] buffer = MessagePackSerializer.Serialize<GlobalSaveData>((M0) this);
      writer.Write(buffer);
    }

    public static GlobalSaveData LoadFile(string fileName)
    {
      GlobalSaveData globalSaveData = new GlobalSaveData();
      return globalSaveData.Load(fileName) ? globalSaveData : (GlobalSaveData) null;
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
          Debug.Log((object) string.Format("グローバルセーブデータが見つからないので新規作成: {0}", (object) fileName));
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
        this.Copy((GlobalSaveData) MessagePackSerializer.Deserialize<GlobalSaveData>(source));
        return true;
      }
      catch (Exception ex)
      {
        Debug.LogException(ex);
      }
      return false;
    }
  }
}
