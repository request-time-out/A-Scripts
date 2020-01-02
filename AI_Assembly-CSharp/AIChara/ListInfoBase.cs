// Decompiled with JetBrains decompiler
// Type: AIChara.ListInfoBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion;
using Illusion.Extensions;
using System;
using System.Collections.Generic;

namespace AIChara
{
  [Serializable]
  public class ListInfoBase
  {
    private Dictionary<int, string> _dictInfo = new Dictionary<int, string>();

    public ListInfoBase()
    {
      this.dictInfo = (IReadOnlyDictionary<int, string>) this._dictInfo;
    }

    public int ListIndex
    {
      get
      {
        return this.GetInfoInt(ChaListDefine.KeyType.ListIndex);
      }
    }

    public int Category
    {
      get
      {
        return this.GetInfoInt(ChaListDefine.KeyType.Category);
      }
    }

    public int Distribution
    {
      get
      {
        return this.GetInfoInt(ChaListDefine.KeyType.DistributionNo);
      }
    }

    public int Id
    {
      get
      {
        return this.GetInfoInt(ChaListDefine.KeyType.ID);
      }
    }

    public int Kind
    {
      get
      {
        return this.GetInfoInt(ChaListDefine.KeyType.Kind);
      }
    }

    public string Name
    {
      get
      {
        return this.GetInfo(ChaListDefine.KeyType.Name);
      }
    }

    public IReadOnlyDictionary<int, string> dictInfo { get; }

    public bool Set(
      int entryCnt,
      int _cateNo,
      int _distNo,
      List<string> lstKey,
      List<string> lstData)
    {
      string[] names = Utils.Enum<ChaListDefine.KeyType>.Names;
      this._dictInfo[names.Check<string>("ListIndex")] = entryCnt.ToString();
      this._dictInfo[names.Check<string>("Category")] = _cateNo.ToString();
      this._dictInfo[names.Check<string>("DistributionNo")] = _distNo.ToString();
      for (int index = 0; index < lstKey.Count; ++index)
        this._dictInfo[names.Check<string>(lstKey[index])] = lstData[index];
      return true;
    }

    public void ChangeListIndex(int index)
    {
      string[] names = Utils.Enum<ChaListDefine.KeyType>.Names;
      this._dictInfo[0] = index.ToString();
    }

    public int GetInfoInt(ChaListDefine.KeyType keyType)
    {
      int result;
      return !int.TryParse(this.GetInfo(keyType), out result) ? -1 : result;
    }

    public float GetInfoFloat(ChaListDefine.KeyType keyType)
    {
      float result;
      return !float.TryParse(this.GetInfo(keyType), out result) ? -1f : result;
    }

    public string GetInfo(ChaListDefine.KeyType keyType)
    {
      string str;
      return !this._dictInfo.TryGetValue((int) keyType, out str) ? "0" : str;
    }
  }
}
