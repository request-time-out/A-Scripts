// Decompiled with JetBrains decompiler
// Type: AIChara.ChaFileCoordinate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using MessagePack;
using OutputLogControl;
using System;
using System.IO;
using UnityEngine;

namespace AIChara
{
  public class ChaFileCoordinate : ChaFileAssist
  {
    public static readonly string BlockName = "Coordinate";
    public Version loadVersion = new Version(ChaFileDefine.ChaFileCoordinateVersion.ToString());
    public string coordinateName = string.Empty;
    public int loadProductNo;
    public int language;
    public ChaFileClothes clothes;
    public ChaFileAccessory accessory;
    public byte[] pngData;
    private int lastLoadErrorCode;

    public ChaFileCoordinate()
    {
      this.MemberInit();
    }

    public string coordinateFileName { get; private set; }

    public int GetLastErrorCode()
    {
      return this.lastLoadErrorCode;
    }

    public void MemberInit()
    {
      this.clothes = new ChaFileClothes();
      this.accessory = new ChaFileAccessory();
      this.coordinateFileName = string.Empty;
      this.coordinateName = string.Empty;
      this.pngData = (byte[]) null;
      this.lastLoadErrorCode = 0;
    }

    public byte[] SaveBytes()
    {
      byte[] buffer1 = MessagePackSerializer.Serialize<ChaFileClothes>((M0) this.clothes);
      byte[] buffer2 = MessagePackSerializer.Serialize<ChaFileAccessory>((M0) this.accessory);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
        {
          binaryWriter.Write(buffer1.Length);
          binaryWriter.Write(buffer1);
          binaryWriter.Write(buffer2.Length);
          binaryWriter.Write(buffer2);
          return memoryStream.ToArray();
        }
      }
    }

    public bool LoadBytes(byte[] data, Version ver)
    {
      using (MemoryStream memoryStream = new MemoryStream(data))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) memoryStream))
        {
          try
          {
            int count1 = binaryReader.ReadInt32();
            this.clothes = (ChaFileClothes) MessagePackSerializer.Deserialize<ChaFileClothes>(binaryReader.ReadBytes(count1));
            int count2 = binaryReader.ReadInt32();
            this.accessory = (ChaFileAccessory) MessagePackSerializer.Deserialize<ChaFileAccessory>(binaryReader.ReadBytes(count2));
          }
          catch (EndOfStreamException ex)
          {
            Debug.LogError((object) ("データが破損している可能性があります：" + ex.GetType().Name));
            return false;
          }
          this.clothes.ComplementWithVersion();
          this.accessory.ComplementWithVersion();
          return true;
        }
      }
    }

    public void SaveFile(string path, int lang)
    {
      string directoryName = Path.GetDirectoryName(path);
      if (!System.IO.Directory.Exists(directoryName))
        System.IO.Directory.CreateDirectory(directoryName);
      this.coordinateFileName = Path.GetFileName(path);
      using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
        {
          if (this.pngData != null)
            binaryWriter.Write(this.pngData);
          binaryWriter.Write(100);
          binaryWriter.Write("【AIS_Clothes】");
          binaryWriter.Write(ChaFileDefine.ChaFileClothesVersion.ToString());
          binaryWriter.Write(lang);
          binaryWriter.Write(this.coordinateName);
          byte[] buffer = this.SaveBytes();
          binaryWriter.Write(buffer.Length);
          binaryWriter.Write(buffer);
        }
      }
    }

    public static int GetProductNo(string path)
    {
      if (!File.Exists(path))
        return -1;
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        using (BinaryReader br = new BinaryReader((Stream) fileStream))
        {
          try
          {
            PngFile.SkipPng(br);
            if (br.BaseStream.Length - br.BaseStream.Position != 0L)
              return br.ReadInt32();
            OutputLog.Error("ただのPNGファイルの可能性があります。", true, "CharaLoad");
            return -1;
          }
          catch (EndOfStreamException ex)
          {
            Debug.LogError((object) ("データが破損している可能性があります：" + ex.GetType().Name));
            return -1;
          }
        }
      }
    }

    public bool LoadFile(TextAsset ta)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        memoryStream.Write(ta.get_bytes(), 0, ta.get_bytes().Length);
        memoryStream.Seek(0L, SeekOrigin.Begin);
        return this.LoadFile((Stream) memoryStream, (int) Singleton<GameSystem>.Instance.language);
      }
    }

    public bool LoadFile(string path)
    {
      if (!File.Exists(path))
      {
        this.lastLoadErrorCode = -6;
        return false;
      }
      this.coordinateFileName = Path.GetFileName(path);
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
        return this.LoadFile((Stream) fileStream, (int) Singleton<GameSystem>.Instance.language);
    }

    public bool LoadFile(Stream st, int lang)
    {
      using (BinaryReader br = new BinaryReader(st))
      {
        try
        {
          PngFile.SkipPng(br);
          if (br.BaseStream.Length - br.BaseStream.Position == 0L)
          {
            OutputLog.Error("ただのPNGファイルの可能性があります。", true, "CharaLoad");
            this.lastLoadErrorCode = -5;
            return false;
          }
          this.loadProductNo = br.ReadInt32();
          if (this.loadProductNo > 100)
          {
            OutputLog.Error("実行ファイルよりも新しい製品番号です。", true, "CharaLoad");
            this.lastLoadErrorCode = -3;
            return false;
          }
          if (br.ReadString() != "【AIS_Clothes】")
          {
            OutputLog.Error("ファイルの種類が違います", true, "CharaLoad");
            this.lastLoadErrorCode = -1;
            return false;
          }
          this.loadVersion = new Version(br.ReadString());
          if (this.loadVersion > ChaFileDefine.ChaFileClothesVersion)
          {
            OutputLog.Error("実行ファイルよりも新しいコーディネートファイルです。", true, "CharaLoad");
            this.lastLoadErrorCode = -2;
            return false;
          }
          this.language = br.ReadInt32();
          this.coordinateName = br.ReadString();
          int count = br.ReadInt32();
          if (this.LoadBytes(br.ReadBytes(count), this.loadVersion))
          {
            if (lang != this.language)
              this.coordinateName = string.Empty;
            this.lastLoadErrorCode = 0;
            return true;
          }
          this.lastLoadErrorCode = -999;
          return false;
        }
        catch (EndOfStreamException ex)
        {
          Debug.LogError((object) ("データが破損している可能性があります：" + ex.GetType().Name));
          this.lastLoadErrorCode = -999;
          return false;
        }
      }
    }

    protected void SaveClothes(string path)
    {
      this.SaveFileAssist<ChaFileClothes>(path, this.clothes);
    }

    protected void LoadClothes(string path)
    {
      this.LoadFileAssist<ChaFileClothes>(path, out this.clothes);
      this.clothes.ComplementWithVersion();
    }

    protected void SaveAccessory(string path)
    {
      this.SaveFileAssist<ChaFileAccessory>(path, this.accessory);
    }

    protected void LoadAccessory(string path)
    {
      this.LoadFileAssist<ChaFileAccessory>(path, out this.accessory);
      this.accessory.ComplementWithVersion();
    }
  }
}
