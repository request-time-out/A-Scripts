// Decompiled with JetBrains decompiler
// Type: AIChara.ChaListData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using MessagePack;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AIChara
{
  [MessagePackObject(true)]
  public class ChaListData
  {
    [IgnoreMember]
    public static readonly string ChaListDataMark = "【ChaListData】";

    public ChaListData()
    {
      this.mark = string.Empty;
      this.categoryNo = 0;
      this.distributionNo = 0;
      this.filePath = string.Empty;
      this.lstKey = new List<string>();
      this.dictList = new Dictionary<int, List<string>>();
    }

    public string mark { get; set; }

    public int categoryNo { get; set; }

    public int distributionNo { get; set; }

    public string filePath { get; set; }

    public List<string> lstKey { get; set; }

    public Dictionary<int, List<string>> dictList { get; set; }

    [IgnoreMember]
    public string fileName
    {
      get
      {
        return Path.GetFileName(this.filePath);
      }
    }

    public Dictionary<string, string> GetInfoAll(int id)
    {
      List<string> stringList = (List<string>) null;
      if (!this.dictList.TryGetValue(id, out stringList))
      {
        Debug.LogWarningFormat("{0}: 指定されたIDは存在しません", new object[1]
        {
          (object) this.fileName
        });
        return (Dictionary<string, string>) null;
      }
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      int count = this.lstKey.Count;
      if (stringList.Count != count)
      {
        Debug.LogWarningFormat("{0}: 情報数とキーの数が一致しない", new object[1]
        {
          (object) this.fileName
        });
        return (Dictionary<string, string>) null;
      }
      for (int index = 0; index < count; ++index)
        dictionary[this.lstKey[index]] = stringList[index];
      return dictionary;
    }

    public string GetInfo(int id, string key)
    {
      List<string> stringList = (List<string>) null;
      if (!this.dictList.TryGetValue(id, out stringList))
      {
        Debug.LogWarningFormat("{0}: 指定されたIDは存在しません", new object[1]
        {
          (object) this.fileName
        });
        return string.Empty;
      }
      int index = this.lstKey.IndexOf(key);
      if (index == -1)
      {
        Debug.LogWarningFormat("{0}: 指定されたキーは存在しません", new object[1]
        {
          (object) this.fileName
        });
        return string.Empty;
      }
      int count = this.lstKey.Count;
      if (stringList.Count == count)
        return stringList[index];
      Debug.LogWarningFormat("{0}: 情報数とキーの数が一致しない", new object[1]
      {
        (object) this.fileName
      });
      return (string) null;
    }
  }
}
