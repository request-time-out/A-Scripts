// Decompiled with JetBrains decompiler
// Type: UploaderSystem.NetCacheControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UploaderSystem
{
  public class NetCacheControl : MonoBehaviour
  {
    private const int cacheFileMax = 50;
    public bool enableCache;
    private Dictionary<string, List<NetCacheControl.CacheHeader>>[] dictCacheHeaderInfo;

    public NetCacheControl()
    {
      base.\u002Ector();
    }

    private NetworkInfo netInfo
    {
      get
      {
        return Singleton<NetworkInfo>.Instance;
      }
    }

    private Dictionary<int, string> GetCacheFileList(DataType type)
    {
      string str = UserData.Path + new string[2]
      {
        "cache/chara/",
        "cache/housing/"
      }[(int) type];
      Dictionary<int, string> source = new Dictionary<int, string>();
      string empty = string.Empty;
      for (int index = 0; index < 50; ++index)
      {
        string path = str + index.ToString("00") + ".dat";
        if (File.Exists(path))
          source[index] = path;
      }
      if (source.Count >= 50)
      {
        // ISSUE: object of a compiler-generated type is created
        List<\u003C\u003E__AnonType30<int, System.IO.FileInfo>> list = source.Select<KeyValuePair<int, string>, \u003C\u003E__AnonType30<int, System.IO.FileInfo>>((Func<KeyValuePair<int, string>, \u003C\u003E__AnonType30<int, System.IO.FileInfo>>) (x => new \u003C\u003E__AnonType30<int, System.IO.FileInfo>(x.Key, new System.IO.FileInfo(x.Value)))).OrderBy<\u003C\u003E__AnonType30<int, System.IO.FileInfo>, DateTime>((Func<\u003C\u003E__AnonType30<int, System.IO.FileInfo>, DateTime>) (x => x.v.LastAccessTime)).ToList<\u003C\u003E__AnonType30<int, System.IO.FileInfo>>();
        source.Remove(list[0].k);
        File.Delete(list[0].v.FullName);
      }
      return source;
    }

    public void UpdateCacheHeaderInfo(DataType type)
    {
      if (this.dictCacheHeaderInfo[(int) type] == null)
        this.dictCacheHeaderInfo[(int) type] = new Dictionary<string, List<NetCacheControl.CacheHeader>>();
      else
        this.dictCacheHeaderInfo[(int) type].Clear();
      foreach (KeyValuePair<int, string> cacheFile in this.GetCacheFileList(type))
      {
        using (FileStream fileStream = new FileStream(cacheFile.Value, FileMode.Open, FileAccess.Read))
        {
          using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
          {
            binaryReader.ReadString();
            binaryReader.ReadInt32();
            int num = binaryReader.ReadInt32();
            List<NetCacheControl.CacheHeader> cacheHeaderList = new List<NetCacheControl.CacheHeader>();
            for (int index = 0; index < num; ++index)
              cacheHeaderList.Add(new NetCacheControl.CacheHeader()
              {
                idx = binaryReader.ReadInt32(),
                update_idx = binaryReader.ReadInt32(),
                pos = binaryReader.ReadInt64(),
                size = binaryReader.ReadInt32()
              });
            this.dictCacheHeaderInfo[(int) type][cacheFile.Value] = cacheHeaderList;
          }
        }
      }
    }

    public string GetCacheHeader(DataType type, int idx, out NetCacheControl.CacheHeader ch)
    {
      ch = (NetCacheControl.CacheHeader) null;
      foreach (KeyValuePair<string, List<NetCacheControl.CacheHeader>> keyValuePair in this.dictCacheHeaderInfo[(int) type])
      {
        foreach (NetCacheControl.CacheHeader cacheHeader in keyValuePair.Value)
        {
          if (cacheHeader.idx == idx)
          {
            ch = new NetCacheControl.CacheHeader();
            ch.idx = cacheHeader.idx;
            ch.update_idx = cacheHeader.update_idx;
            ch.pos = cacheHeader.pos;
            ch.size = cacheHeader.size;
            return keyValuePair.Key;
          }
        }
      }
      return string.Empty;
    }

    public void DeleteCache(DataType type)
    {
      foreach (KeyValuePair<int, string> cacheFile in this.GetCacheFileList(type))
      {
        if (File.Exists(cacheFile.Value))
          File.Delete(cacheFile.Value);
      }
      this.UpdateCacheHeaderInfo(type);
    }

    public bool CreateCache(DataType type, Dictionary<int, Tuple<int, byte[]>> dictGet)
    {
      if (dictGet.Count == 0)
        return false;
      string str1 = UserData.Path + new string[2]
      {
        "cache/chara/",
        "cache/housing/"
      }[(int) type];
      Dictionary<int, Tuple<int, byte[]>> dictPNG = (Dictionary<int, Tuple<int, byte[]>>) null;
      string str2 = string.Empty;
      for (int index = 0; index < 50; ++index)
      {
        str2 = str1 + index.ToString("00") + ".dat";
        if (File.Exists(str2))
        {
          List<NetCacheControl.CacheHeader> cacheHeaderList = (List<NetCacheControl.CacheHeader>) null;
          if (!this.dictCacheHeaderInfo[(int) type].TryGetValue(str2, out cacheHeaderList) || cacheHeaderList.Count < 1000)
          {
            dictPNG = this.LoadCacheFile(str2);
            break;
          }
        }
        else
        {
          dictPNG = new Dictionary<int, Tuple<int, byte[]>>();
          break;
        }
      }
      foreach (KeyValuePair<int, Tuple<int, byte[]>> keyValuePair in dictGet)
      {
        if (keyValuePair.Value.Item2 != null)
          dictPNG[keyValuePair.Key] = new Tuple<int, byte[]>(keyValuePair.Value.Item1, keyValuePair.Value.Item2);
      }
      this.SaveCacheFile(str2, dictPNG);
      return true;
    }

    public void SaveCacheFile(string path, Dictionary<int, Tuple<int, byte[]>> dictPNG)
    {
      string directoryName = Path.GetDirectoryName(path);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      int[] array = dictPNG.Keys.ToArray<int>();
      Dictionary<int, long> dictionary = new Dictionary<int, long>();
      byte[] buffer = (byte[]) null;
      long num = (long) (Encoding.UTF8.GetByteCount("【CacheFile】") + 4 + 4 + 20 * array.Length + 1);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
        {
          for (int index1 = 0; index1 < array.Length; ++index1)
          {
            int index2 = array[index1];
            dictionary[index2] = num;
            num += (long) dictPNG[index2].Item2.Length;
            binaryWriter.Write(dictPNG[index2].Item2);
          }
          buffer = memoryStream.ToArray();
        }
      }
      using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
        {
          binaryWriter.Write("【CacheFile】");
          binaryWriter.Write(100);
          binaryWriter.Write(array.Length);
          for (int index1 = 0; index1 < array.Length; ++index1)
          {
            int index2 = array[index1];
            binaryWriter.Write(index2);
            binaryWriter.Write(dictPNG[index2].Item1);
            binaryWriter.Write(dictionary[index2]);
            binaryWriter.Write(dictPNG[index2].Item2.Length);
          }
          binaryWriter.Write(buffer);
        }
      }
    }

    public Dictionary<int, Tuple<int, byte[]>> LoadCacheFile(string path)
    {
      Dictionary<int, Tuple<int, byte[]>> dictionary = new Dictionary<int, Tuple<int, byte[]>>();
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
        {
          binaryReader.ReadString();
          binaryReader.ReadInt32();
          List<NetCacheControl.CacheHeader> cacheHeaderList = new List<NetCacheControl.CacheHeader>();
          int num = binaryReader.ReadInt32();
          for (int index = 0; index < num; ++index)
            cacheHeaderList.Add(new NetCacheControl.CacheHeader()
            {
              idx = binaryReader.ReadInt32(),
              update_idx = binaryReader.ReadInt32(),
              pos = binaryReader.ReadInt64(),
              size = binaryReader.ReadInt32()
            });
          int count = cacheHeaderList.Count;
          for (int index = 0; index < count; ++index)
          {
            fileStream.Seek(cacheHeaderList[index].pos, SeekOrigin.Begin);
            byte[] numArray = binaryReader.ReadBytes(cacheHeaderList[index].size);
            dictionary[cacheHeaderList[index].idx] = new Tuple<int, byte[]>(cacheHeaderList[index].update_idx, numArray);
          }
        }
      }
      return dictionary;
    }

    public byte[] LoadCache(string path, long pos, int size)
    {
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        fileStream.Seek(pos, SeekOrigin.Begin);
        using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
          return binaryReader.ReadBytes(size);
      }
    }

    public void GetCache(DataType type, Dictionary<int, Tuple<int, byte[]>> dictPNG)
    {
      foreach (int idx in dictPNG.Keys.ToArray<int>())
      {
        NetCacheControl.CacheHeader ch = (NetCacheControl.CacheHeader) null;
        string cacheHeader = this.GetCacheHeader(type, idx, out ch);
        if (ch != null)
          dictPNG[ch.idx] = new Tuple<int, byte[]>(ch.update_idx, this.LoadCache(cacheHeader, ch.pos, ch.size));
      }
    }

    private void Start()
    {
      this.UpdateCacheHeaderInfo(DataType.Chara);
      this.UpdateCacheHeaderInfo(DataType.Housing);
    }

    private void Update()
    {
    }

    public class CacheHeader
    {
      public int idx;
      public int update_idx;
      public long pos;
      public int size;
    }
  }
}
