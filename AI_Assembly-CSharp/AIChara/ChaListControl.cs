// Decompiled with JetBrains decompiler
// Type: AIChara.ChaListControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using MessagePack;
using OutputLogControl;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AIChara
{
  public class ChaListControl
  {
    private Dictionary<int, Dictionary<int, ListInfoBase>> dictListInfo = new Dictionary<int, Dictionary<int, ListInfoBase>>();
    private List<int> lstItemIsInit = new List<int>();
    private List<int> lstItemIsNew = new List<int>();

    public ChaListControl()
    {
      foreach (int index in (ChaListDefine.CategoryNo[]) Enum.GetValues(typeof (ChaListDefine.CategoryNo)))
        this.dictListInfo[index] = new Dictionary<int, ListInfoBase>();
      this.itemIDInfo = (IReadOnlyDictionary<int, byte>) this._itemIDInfo;
    }

    public Dictionary<int, byte> _itemIDInfo { get; set; } = new Dictionary<int, byte>();

    public IReadOnlyDictionary<int, byte> itemIDInfo { get; }

    public Dictionary<int, ListInfoBase> GetCategoryInfo(
      ChaListDefine.CategoryNo type)
    {
      Dictionary<int, ListInfoBase> dictionary = (Dictionary<int, ListInfoBase>) null;
      return !this.dictListInfo.TryGetValue((int) type, out dictionary) ? (Dictionary<int, ListInfoBase>) null : new Dictionary<int, ListInfoBase>((IDictionary<int, ListInfoBase>) dictionary);
    }

    public ListInfoBase GetListInfo(ChaListDefine.CategoryNo type, int id)
    {
      Dictionary<int, ListInfoBase> dictionary = (Dictionary<int, ListInfoBase>) null;
      if (!this.dictListInfo.TryGetValue((int) type, out dictionary))
        return (ListInfoBase) null;
      ListInfoBase listInfoBase = (ListInfoBase) null;
      return !dictionary.TryGetValue(id, out listInfoBase) ? (ListInfoBase) null : listInfoBase;
    }

    public bool LoadListInfoAll()
    {
      ChaListDefine.CategoryNo[] values = (ChaListDefine.CategoryNo[]) Enum.GetValues(typeof (ChaListDefine.CategoryNo));
      Dictionary<int, ListInfoBase> dictData = (Dictionary<int, ListInfoBase>) null;
      List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath("list/characustom/", false);
      for (int index = 0; index < nameListFromPath.Count; ++index)
      {
        AssetBundleLoadAssetOperation loadAssetOperation = AssetBundleManager.LoadAllAsset(nameListFromPath[index], typeof (TextAsset), (string) null);
        if (loadAssetOperation == null)
          Debug.LogWarning((object) ("読み込みエラー\r\nassetBundleName：" + nameListFromPath[index]));
        else if (loadAssetOperation.IsEmpty())
        {
          AssetBundleManager.UnloadAssetBundle(nameListFromPath[index], true, (string) null, false);
        }
        else
        {
          TextAsset[] allAssets = loadAssetOperation.GetAllAssets<TextAsset>();
          if (allAssets == null || allAssets.Length == 0)
          {
            AssetBundleManager.UnloadAssetBundle(nameListFromPath[index], true, (string) null, false);
          }
          else
          {
            foreach (ChaListDefine.CategoryNo categoryNo in values)
            {
              if (!this.dictListInfo.TryGetValue((int) categoryNo, out dictData))
              {
                Debug.LogWarning((object) "リストを読むための準備ができてない");
              }
              else
              {
                foreach (TextAsset ta in allAssets)
                {
                  if (!(YS_Assist.GetRemoveStringRight(((Object) ta).get_name(), "_", false) != categoryNo.ToString() + "_"))
                    this.LoadListInfo(dictData, ta);
                }
              }
            }
            AssetBundleManager.UnloadAssetBundle(nameListFromPath[index], true, (string) null, false);
          }
        }
      }
      this.EntryClothesIsInit();
      this.LoadItemID();
      OutputLog.Log(nameof (LoadListInfoAll), false, "UnloadUnusedAssets");
      Resources.UnloadUnusedAssets();
      GC.Collect();
      return true;
    }

    private bool LoadListInfo(
      Dictionary<int, ListInfoBase> dictData,
      string assetBundleName,
      string assetName)
    {
      TextAsset ta = CommonLib.LoadAsset<TextAsset>(assetBundleName, assetName, false, string.Empty);
      if (Object.op_Equality((Object) null, (Object) ta))
      {
        Debug.LogError((object) "あってはならない");
        return false;
      }
      bool flag = this.LoadListInfo(dictData, ta);
      AssetBundleManager.UnloadAssetBundle(assetBundleName, true, (string) null, false);
      return flag;
    }

    private bool LoadListInfo(Dictionary<int, ListInfoBase> dictData, TextAsset ta)
    {
      if (Object.op_Equality((Object) null, (Object) ta))
        return false;
      ChaListData chaListData = (ChaListData) MessagePackSerializer.Deserialize<ChaListData>(ta.get_bytes());
      if (chaListData == null)
        return false;
      foreach (KeyValuePair<int, List<string>> dict in chaListData.dictList)
      {
        int count = dictData.Count;
        ListInfoBase listInfoBase = new ListInfoBase();
        if (listInfoBase.Set(count, chaListData.categoryNo, chaListData.distributionNo, chaListData.lstKey, dict.Value))
        {
          if (dictData.ContainsKey(listInfoBase.Id))
          {
            Debug.LogWarningFormat("[{0}] カスタムリストのIDが重複しています。", new object[1]
            {
              (object) (listInfoBase.Category * 1000 + listInfoBase.Id)
            });
          }
          else
          {
            dictData[listInfoBase.Id] = listInfoBase;
            int infoInt = listInfoBase.GetInfoInt(ChaListDefine.KeyType.Possess);
            int num = listInfoBase.Category * 1000 + listInfoBase.Id;
            switch (infoInt)
            {
              case 1:
                this.lstItemIsInit.Add(num);
                continue;
              case 2:
                this.lstItemIsNew.Add(num);
                continue;
              default:
                continue;
            }
          }
        }
      }
      return true;
    }

    public static List<ExcelData.Param> LoadExcelData(
      string assetBunndlePath,
      string assetName,
      int cellS,
      int rowS)
    {
      if (!AssetBundleCheck.IsFile(assetBunndlePath, assetName))
      {
        OutputLog.Error(string.Format("ExcelData:{0}がない", (object) assetName), false, "Log");
        return (List<ExcelData.Param>) null;
      }
      AssetBundleLoadAssetOperation loadAssetOperation = AssetBundleManager.LoadAsset(assetBunndlePath, assetName, typeof (ExcelData), (string) null);
      AssetBundleManager.UnloadAssetBundle(assetBunndlePath, true, (string) null, false);
      if (loadAssetOperation.IsEmpty())
      {
        OutputLog.Error(string.Format("ExcelData:{0}がない？", (object) assetName), false, "Log");
        return (List<ExcelData.Param>) null;
      }
      ExcelData asset = loadAssetOperation.GetAsset<ExcelData>();
      int cell = asset.MaxCell - 1;
      int row = asset.list[cell].list.Count - 1;
      return asset.Get(new ExcelData.Specify(cellS, rowS), new ExcelData.Specify(cell, row));
    }

    public void EntryClothesIsInit()
    {
      for (int index = 0; index < this.lstItemIsInit.Count; ++index)
        this.AddItemID(this.lstItemIsInit[index], (byte) 2);
      for (int index = 0; index < this.lstItemIsNew.Count; ++index)
        this.AddItemID(this.lstItemIsNew[index], (byte) 1);
      this.lstItemIsInit.Clear();
      this.lstItemIsInit.TrimExcess();
      this.lstItemIsNew.Clear();
      this.lstItemIsNew.TrimExcess();
    }

    public void SaveItemID()
    {
      string path = UserData.Path + ChaListDefine.CheckItemFile;
      string directoryName = Path.GetDirectoryName(path);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
        {
          binaryWriter.Write(ChaListDefine.CheckItemVersion.ToString());
          binaryWriter.Write(this._itemIDInfo.Count);
          foreach (KeyValuePair<int, byte> keyValuePair in this._itemIDInfo)
          {
            binaryWriter.Write(keyValuePair.Key);
            binaryWriter.Write(keyValuePair.Value);
          }
        }
      }
    }

    public void LoadItemID()
    {
      string path = UserData.Path + ChaListDefine.CheckItemFile;
      if (!File.Exists(path))
        return;
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
        {
          binaryReader.ReadString();
          int num1 = binaryReader.ReadInt32();
          for (int index = 0; index < num1; ++index)
          {
            int key = binaryReader.ReadInt32();
            byte num2 = binaryReader.ReadByte();
            byte num3 = 0;
            if (this._itemIDInfo.TryGetValue(key, out num3))
            {
              if ((int) num3 < (int) num2)
                this._itemIDInfo[key] = num2;
            }
            else
              this._itemIDInfo.Add(key, num2);
          }
        }
      }
    }

    public void AddItemID(string IDStr, byte flags = 1)
    {
      string str = IDStr;
      char[] chArray = new char[1]{ '/' };
      foreach (string s in str.Split(chArray))
      {
        int key = int.Parse(s);
        byte num = 0;
        if (this._itemIDInfo.TryGetValue(key, out num))
        {
          if ((int) num < (int) flags)
            this._itemIDInfo[key] = flags;
        }
        else
          this._itemIDInfo.Add(key, flags);
      }
    }

    public void AddItemID(int pid, byte flags = 1)
    {
      byte num = 0;
      if (this._itemIDInfo.TryGetValue(pid, out num))
      {
        if ((int) num >= (int) flags)
          return;
        this._itemIDInfo[pid] = flags;
      }
      else
        this._itemIDInfo.Add(pid, flags);
    }

    public void AddItemID(int category, int id, byte flags)
    {
      this.AddItemID(category * 1000 + id, flags);
    }

    public byte CheckItemID(int pid)
    {
      byte num = 0;
      return this._itemIDInfo.TryGetValue(pid, out num) ? num : num;
    }

    public byte CheckItemID(int category, int id)
    {
      return this.CheckItemID(category * 1000 + id);
    }
  }
}
