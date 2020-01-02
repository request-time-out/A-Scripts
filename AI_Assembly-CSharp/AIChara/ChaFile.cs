// Decompiled with JetBrains decompiler
// Type: AIChara.ChaFile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using MessagePack;
using OutputLogControl;
using System;
using System.IO;
using UnityEngine;

namespace AIChara
{
  public class ChaFile
  {
    public Version loadVersion = new Version(ChaFileDefine.ChaFileVersion.ToString());
    public string userID = string.Empty;
    public string dataID = string.Empty;
    public int loadProductNo;
    public int language;
    public byte[] pngData;
    public ChaFileCustom custom;
    public ChaFileCoordinate coordinate;
    public ChaFileParameter parameter;
    public ChaFileGameInfo gameinfo;
    public ChaFileStatus status;
    private int lastLoadErrorCode;

    public ChaFile()
    {
      this.custom = new ChaFileCustom();
      this.coordinate = new ChaFileCoordinate();
      this.parameter = new ChaFileParameter();
      this.gameinfo = new ChaFileGameInfo();
      this.status = new ChaFileStatus();
      this.lastLoadErrorCode = 0;
    }

    public string charaFileName { get; protected set; }

    public int GetLastErrorCode()
    {
      return this.lastLoadErrorCode;
    }

    protected bool SaveFile(string path, int lang)
    {
      string directoryName = Path.GetDirectoryName(path);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      this.charaFileName = Path.GetFileName(path);
      using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
        return this.SaveFile((Stream) fileStream, true, lang);
    }

    protected bool SaveFile(Stream st, bool savePng, int lang)
    {
      using (BinaryWriter bw = new BinaryWriter(st))
        return this.SaveFile(bw, savePng, lang);
    }

    protected bool SaveFile(BinaryWriter bw, bool savePng, int lang)
    {
      if (savePng && this.pngData != null)
        bw.Write(this.pngData);
      bw.Write(100);
      bw.Write("【AIS_Chara】");
      bw.Write(ChaFileDefine.ChaFileVersion.ToString());
      bw.Write(lang);
      bw.Write(this.userID);
      bw.Write(this.dataID);
      byte[] customBytes = this.GetCustomBytes();
      byte[] coordinateBytes = this.GetCoordinateBytes();
      byte[] parameterBytes = this.GetParameterBytes();
      byte[] gameInfoBytes = this.GetGameInfoBytes();
      byte[] statusBytes = this.GetStatusBytes();
      int length = 5;
      long num1 = 0;
      string[] strArray1 = new string[5]
      {
        ChaFileCustom.BlockName,
        ChaFileCoordinate.BlockName,
        ChaFileParameter.BlockName,
        ChaFileGameInfo.BlockName,
        ChaFileStatus.BlockName
      };
      string[] strArray2 = new string[5]
      {
        ChaFileDefine.ChaFileCustomVersion.ToString(),
        ChaFileDefine.ChaFileCoordinateVersion.ToString(),
        ChaFileDefine.ChaFileParameterVersion.ToString(),
        ChaFileDefine.ChaFileGameInfoVersion.ToString(),
        ChaFileDefine.ChaFileStatusVersion.ToString()
      };
      long[] numArray1 = new long[length];
      numArray1[0] = customBytes != null ? (long) customBytes.Length : 0L;
      numArray1[1] = coordinateBytes != null ? (long) coordinateBytes.Length : 0L;
      numArray1[2] = parameterBytes != null ? (long) parameterBytes.Length : 0L;
      numArray1[3] = gameInfoBytes != null ? (long) gameInfoBytes.Length : 0L;
      numArray1[4] = statusBytes != null ? (long) statusBytes.Length : 0L;
      long[] numArray2 = new long[5]
      {
        num1,
        num1 + numArray1[0],
        num1 + numArray1[0] + numArray1[1],
        num1 + numArray1[0] + numArray1[1] + numArray1[2],
        num1 + numArray1[0] + numArray1[1] + numArray1[2] + numArray1[3]
      };
      BlockHeader blockHeader = new BlockHeader();
      for (int index = 0; index < length; ++index)
      {
        BlockHeader.Info info = new BlockHeader.Info()
        {
          name = strArray1[index],
          version = strArray2[index],
          size = numArray1[index],
          pos = numArray2[index]
        };
        blockHeader.lstInfo.Add(info);
      }
      byte[] buffer = MessagePackSerializer.Serialize<BlockHeader>((M0) blockHeader);
      bw.Write(buffer.Length);
      bw.Write(buffer);
      long num2 = 0;
      foreach (long num3 in numArray1)
        num2 += num3;
      bw.Write(num2);
      bw.Write(customBytes);
      bw.Write(coordinateBytes);
      bw.Write(parameterBytes);
      bw.Write(gameInfoBytes);
      bw.Write(statusBytes);
      return true;
    }

    public static bool GetProductInfo(string path, out ChaFile.ProductInfo info)
    {
      info = new ChaFile.ProductInfo();
      if (!File.Exists(path))
        return false;
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        using (BinaryReader br = new BinaryReader((Stream) fileStream))
        {
          long pngSize = PngFile.GetPngSize(br);
          if (pngSize != 0L)
          {
            br.BaseStream.Seek(pngSize, SeekOrigin.Current);
            if (br.BaseStream.Length - br.BaseStream.Position == 0L)
            {
              OutputLog.Warning("ただのPNGファイルの可能性があります。", true, "CharaLoad");
              return false;
            }
          }
          try
          {
            info.productNo = br.ReadInt32();
            info.tag = br.ReadString();
            if (info.tag != "【AIS_Chara】")
            {
              OutputLog.Error("ファイルの種類が違います", true, "CharaLoad");
              return false;
            }
            info.version = new Version(br.ReadString());
            if (info.version > ChaFileDefine.ChaFileVersion)
            {
              OutputLog.Error("実行ファイルよりも新しいファイルです。", true, "CharaLoad");
              return false;
            }
            info.language = br.ReadInt32();
            info.userID = br.ReadString();
            info.dataID = br.ReadString();
            return true;
          }
          catch (EndOfStreamException ex)
          {
            Debug.LogError((object) ("データが破損している可能性があります：" + ex.GetType().Name));
            return false;
          }
        }
      }
    }

    protected bool LoadFile(string path, int lang, bool noLoadPNG = false, bool noLoadStatus = true)
    {
      if (!File.Exists(path))
      {
        this.lastLoadErrorCode = -6;
        return false;
      }
      this.charaFileName = Path.GetFileName(path);
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
        return this.LoadFile((Stream) fileStream, lang, noLoadPNG, noLoadStatus);
    }

    protected bool LoadFile(Stream st, int lang, bool noLoadPNG = false, bool noLoadStatus = true)
    {
      using (BinaryReader br = new BinaryReader(st))
        return this.LoadFile(br, lang, noLoadPNG, noLoadStatus);
    }

    protected bool LoadFile(BinaryReader br, int lang, bool noLoadPNG = false, bool noLoadStatus = true)
    {
      long pngSize = PngFile.GetPngSize(br);
      if (pngSize != 0L)
      {
        if (noLoadPNG)
          br.BaseStream.Seek(pngSize, SeekOrigin.Current);
        else
          this.pngData = br.ReadBytes((int) pngSize);
        if (br.BaseStream.Length - br.BaseStream.Position == 0L)
        {
          OutputLog.Warning("ただのPNGファイルの可能性があります。", true, "CharaLoad");
          this.lastLoadErrorCode = -5;
          return false;
        }
      }
      try
      {
        this.loadProductNo = br.ReadInt32();
        if (this.loadProductNo > 100)
        {
          OutputLog.Error("実行ファイルよりも新しい製品番号です。", true, "CharaLoad");
          this.lastLoadErrorCode = -3;
          return false;
        }
        if (br.ReadString() != "【AIS_Chara】")
        {
          OutputLog.Error("ファイルの種類が違います", true, "CharaLoad");
          this.lastLoadErrorCode = -1;
          return false;
        }
        this.loadVersion = new Version(br.ReadString());
        if (this.loadVersion > ChaFileDefine.ChaFileVersion)
        {
          OutputLog.Error("実行ファイルよりも新しいコーディネートファイルです。", true, "CharaLoad");
          this.lastLoadErrorCode = -2;
          return false;
        }
        this.language = br.ReadInt32();
        this.userID = br.ReadString();
        this.dataID = br.ReadString();
        int count = br.ReadInt32();
        BlockHeader blockHeader = (BlockHeader) MessagePackSerializer.Deserialize<BlockHeader>(br.ReadBytes(count));
        long num = br.ReadInt64();
        long position = br.BaseStream.Position;
        BlockHeader.Info info1 = blockHeader.SearchInfo(ChaFileCustom.BlockName);
        if (info1 != null)
        {
          Version ver = new Version(info1.version);
          if (ver > ChaFileDefine.ChaFileCustomVersion)
          {
            OutputLog.Error("実行ファイルよりも新しいカスタム情報です。", true, "CharaLoad");
            this.lastLoadErrorCode = -2;
          }
          else
          {
            br.BaseStream.Seek(position + info1.pos, SeekOrigin.Begin);
            this.SetCustomBytes(br.ReadBytes((int) info1.size), ver);
          }
        }
        BlockHeader.Info info2 = blockHeader.SearchInfo(ChaFileCoordinate.BlockName);
        if (info2 != null)
        {
          Version ver = new Version(info2.version);
          if (ver > ChaFileDefine.ChaFileCoordinateVersion)
          {
            OutputLog.Error("実行ファイルよりも新しいコーディネート情報です。", true, "CharaLoad");
            this.lastLoadErrorCode = -2;
          }
          else
          {
            br.BaseStream.Seek(position + info2.pos, SeekOrigin.Begin);
            this.SetCoordinateBytes(br.ReadBytes((int) info2.size), ver);
          }
        }
        BlockHeader.Info info3 = blockHeader.SearchInfo(ChaFileParameter.BlockName);
        if (info3 != null)
        {
          if (new Version(info3.version) > ChaFileDefine.ChaFileParameterVersion)
          {
            OutputLog.Error("実行ファイルよりも新しいパラメータ情報です。", true, "CharaLoad");
            this.lastLoadErrorCode = -2;
          }
          else
          {
            br.BaseStream.Seek(position + info3.pos, SeekOrigin.Begin);
            this.SetParameterBytes(br.ReadBytes((int) info3.size));
          }
        }
        BlockHeader.Info info4 = blockHeader.SearchInfo(ChaFileGameInfo.BlockName);
        if (info4 != null)
        {
          if (new Version(info4.version) > ChaFileDefine.ChaFileGameInfoVersion)
          {
            OutputLog.Error("実行ファイルよりも新しいゲーム情報です。", true, "CharaLoad");
            this.lastLoadErrorCode = -2;
          }
          else
          {
            br.BaseStream.Seek(position + info4.pos, SeekOrigin.Begin);
            this.SetGameInfoBytes(br.ReadBytes((int) info4.size));
          }
        }
        if (!noLoadStatus)
        {
          BlockHeader.Info info5 = blockHeader.SearchInfo(ChaFileStatus.BlockName);
          if (info5 != null)
          {
            if (new Version(info5.version) > ChaFileDefine.ChaFileStatusVersion)
            {
              OutputLog.Error("実行ファイルよりも新しいステータス情報です。", true, "CharaLoad");
              this.lastLoadErrorCode = -2;
            }
            else
            {
              br.BaseStream.Seek(position + info5.pos, SeekOrigin.Begin);
              this.SetStatusBytes(br.ReadBytes((int) info5.size));
            }
          }
        }
        br.BaseStream.Seek(position + num, SeekOrigin.Begin);
      }
      catch (EndOfStreamException ex)
      {
        Debug.LogError((object) ("データが破損している可能性があります：" + ex.GetType().Name));
        this.lastLoadErrorCode = -999;
        return false;
      }
      if (lang != this.language)
        this.parameter.fullname = string.Empty;
      this.lastLoadErrorCode = 0;
      return true;
    }

    public byte[] GetCustomBytes()
    {
      return ChaFile.GetCustomBytes(this.custom);
    }

    public static byte[] GetCustomBytes(ChaFileCustom _custom)
    {
      return _custom.SaveBytes();
    }

    public byte[] GetCoordinateBytes()
    {
      return ChaFile.GetCoordinateBytes(this.coordinate);
    }

    public static byte[] GetCoordinateBytes(ChaFileCoordinate _coordinate)
    {
      return _coordinate.SaveBytes();
    }

    public byte[] GetParameterBytes()
    {
      return ChaFile.GetParameterBytes(this.parameter);
    }

    public static byte[] GetParameterBytes(ChaFileParameter _parameter)
    {
      return MessagePackSerializer.Serialize<ChaFileParameter>((M0) _parameter);
    }

    public byte[] GetGameInfoBytes()
    {
      return ChaFile.GetGameInfoBytes(this.gameinfo);
    }

    public static byte[] GetGameInfoBytes(ChaFileGameInfo _gameinfo)
    {
      return MessagePackSerializer.Serialize<ChaFileGameInfo>((M0) _gameinfo);
    }

    public byte[] GetStatusBytes()
    {
      return ChaFile.GetStatusBytes(this.status);
    }

    public static byte[] GetStatusBytes(ChaFileStatus _status)
    {
      return MessagePackSerializer.Serialize<ChaFileStatus>((M0) _status);
    }

    public void SetCustomBytes(byte[] data, Version ver)
    {
      this.custom.LoadBytes(data, ver);
    }

    public void SetCoordinateBytes(byte[] data, Version ver)
    {
      this.coordinate.LoadBytes(data, ver);
    }

    public void SetParameterBytes(byte[] data)
    {
      ChaFileParameter src = (ChaFileParameter) MessagePackSerializer.Deserialize<ChaFileParameter>(data);
      src.ComplementWithVersion();
      this.parameter.Copy(src);
    }

    public void SetGameInfoBytes(byte[] data)
    {
      ChaFileGameInfo src = (ChaFileGameInfo) MessagePackSerializer.Deserialize<ChaFileGameInfo>(data);
      src.ComplementWithVersion();
      this.gameinfo.Copy(src);
    }

    public void SetStatusBytes(byte[] data)
    {
      ChaFileStatus src = (ChaFileStatus) MessagePackSerializer.Deserialize<ChaFileStatus>(data);
      src.ComplementWithVersion();
      this.status.Copy(src);
    }

    public static void CopyChaFile(
      ChaFile dst,
      ChaFile src,
      bool _custom = true,
      bool _coordinate = true,
      bool _parameter = true,
      bool _gameinfo = true,
      bool _status = true)
    {
      dst.CopyAll(src, _custom, _coordinate, _parameter, _gameinfo, _status);
    }

    public void CopyAll(
      ChaFile _chafile,
      bool _custom = true,
      bool _coordinate = true,
      bool _parameter = true,
      bool _gameinfo = true,
      bool _status = true)
    {
      if (_custom)
        this.CopyCustom(_chafile.custom);
      if (_coordinate)
        this.CopyCoordinate(_chafile.coordinate);
      if (_parameter)
        this.CopyParameter(_chafile.parameter);
      if (_gameinfo)
        this.CopyGameInfo(_chafile.gameinfo);
      if (!_status)
        return;
      this.CopyStatus(_chafile.status);
    }

    public void CopyCustom(ChaFileCustom _custom)
    {
      this.SetCustomBytes(ChaFile.GetCustomBytes(_custom), ChaFileDefine.ChaFileCustomVersion);
    }

    public void CopyCoordinate(ChaFileCoordinate _coordinate)
    {
      this.SetCoordinateBytes(ChaFile.GetCoordinateBytes(_coordinate), ChaFileDefine.ChaFileCoordinateVersion);
    }

    public void CopyParameter(ChaFileParameter _parameter)
    {
      this.SetParameterBytes(ChaFile.GetParameterBytes(_parameter));
    }

    public void CopyGameInfo(ChaFileGameInfo _gameinfo)
    {
      this.SetGameInfoBytes(ChaFile.GetGameInfoBytes(_gameinfo));
    }

    public void CopyStatus(ChaFileStatus _status)
    {
      this.SetStatusBytes(ChaFile.GetStatusBytes(_status));
    }

    public class ProductInfo
    {
      public int productNo = -1;
      public string tag = string.Empty;
      public Version version = new Version(0, 0, 0);
      public string userID = string.Empty;
      public string dataID = string.Empty;
      public int language;
    }
  }
}
