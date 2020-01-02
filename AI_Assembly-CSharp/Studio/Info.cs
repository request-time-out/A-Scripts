// Decompiled with JetBrains decompiler
// Type: Studio.Info
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Studio
{
  public class Info : Singleton<Info>
  {
    public Dictionary<int, Info.BoneInfo> dicBoneInfo = new Dictionary<int, Info.BoneInfo>();
    public Dictionary<int, Info.GroupInfo> dicItemGroupCategory = new Dictionary<int, Info.GroupInfo>();
    public Dictionary<int, Dictionary<int, Dictionary<int, Info.ItemLoadInfo>>> dicItemLoadInfo = new Dictionary<int, Dictionary<int, Dictionary<int, Info.ItemLoadInfo>>>();
    public Dictionary<int, Dictionary<int, Dictionary<int, ItemColorData.ColorData>>> dicItemColorData = new Dictionary<int, Dictionary<int, Dictionary<int, ItemColorData.ColorData>>>();
    public Dictionary<int, Info.AccessoryGroupInfo> dicAccessoryGroup = new Dictionary<int, Info.AccessoryGroupInfo>();
    public Dictionary<int, Info.LightLoadInfo> dicLightLoadInfo = new Dictionary<int, Info.LightLoadInfo>();
    public Dictionary<int, Info.GroupInfo> dicAGroupCategory = new Dictionary<int, Info.GroupInfo>();
    public Dictionary<int, Dictionary<int, Dictionary<int, Info.AnimeLoadInfo>>> dicAnimeLoadInfo = new Dictionary<int, Dictionary<int, Dictionary<int, Info.AnimeLoadInfo>>>();
    public Dictionary<int, Info.HandAnimeInfo>[] dicHandAnime = new Dictionary<int, Info.HandAnimeInfo>[2]
    {
      new Dictionary<int, Info.HandAnimeInfo>(),
      new Dictionary<int, Info.HandAnimeInfo>()
    };
    public Dictionary<int, Info.GroupInfo> dicVoiceGroupCategory = new Dictionary<int, Info.GroupInfo>();
    public Dictionary<int, Dictionary<int, Dictionary<int, Info.LoadCommonInfo>>> dicVoiceLoadInfo = new Dictionary<int, Dictionary<int, Dictionary<int, Info.LoadCommonInfo>>>();
    public Dictionary<int, Info.LoadCommonInfo> dicBGMLoadInfo = new Dictionary<int, Info.LoadCommonInfo>();
    public Dictionary<int, Info.LoadCommonInfo> dicENVLoadInfo = new Dictionary<int, Info.LoadCommonInfo>();
    public Dictionary<int, Info.MapLoadInfo> dicMapLoadInfo = new Dictionary<int, Info.MapLoadInfo>();
    public Dictionary<int, Info.LoadCommonInfo> dicColorGradingLoadInfo = new Dictionary<int, Info.LoadCommonInfo>();
    public Dictionary<int, Info.LoadCommonInfo> dicReflectionProbeLoadInfo = new Dictionary<int, Info.LoadCommonInfo>();
    private ExcelData m_AccessoryPointGroup;
    private Info.FileCheck fileCheck;
    private Info.WaitTime waitTime;

    public int AccessoryPointNum { get; private set; }

    public int[] AccessoryPointsIndex
    {
      get
      {
        HashSet<int> source = new HashSet<int>();
        foreach (KeyValuePair<int, Info.AccessoryGroupInfo> keyValuePair in this.dicAccessoryGroup)
          source.UnionWith(Enumerable.Range(keyValuePair.Value.Targets[0], keyValuePair.Value.Targets[1] - keyValuePair.Value.Targets[0] + 1));
        return source.ToArray<int>();
      }
    }

    public ExcelData accessoryPointGroup
    {
      get
      {
        return this.m_AccessoryPointGroup;
      }
    }

    public bool isLoadList { get; private set; }

    [DebuggerHidden]
    public IEnumerator LoadExcelDataCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Info.\u003CLoadExcelDataCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public Info.LoadCommonInfo GetVoiceInfo(int _group, int _category, int _no)
    {
      Dictionary<int, Dictionary<int, Info.LoadCommonInfo>> dictionary1 = (Dictionary<int, Dictionary<int, Info.LoadCommonInfo>>) null;
      if (!this.dicVoiceLoadInfo.TryGetValue(_group, out dictionary1))
        return (Info.LoadCommonInfo) null;
      Dictionary<int, Info.LoadCommonInfo> dictionary2 = (Dictionary<int, Info.LoadCommonInfo>) null;
      if (!dictionary1.TryGetValue(_category, out dictionary2))
        return (Info.LoadCommonInfo) null;
      Info.LoadCommonInfo loadCommonInfo = (Info.LoadCommonInfo) null;
      return dictionary2.TryGetValue(_no, out loadCommonInfo) ? loadCommonInfo : (Info.LoadCommonInfo) null;
    }

    public ItemColorData.ColorData SafeGetItemColorData(
      int _group,
      int _category,
      int _id)
    {
      Dictionary<int, Dictionary<int, ItemColorData.ColorData>> dictionary1 = (Dictionary<int, Dictionary<int, ItemColorData.ColorData>>) null;
      if (!this.dicItemColorData.TryGetValue(_group, out dictionary1))
        return (ItemColorData.ColorData) null;
      Dictionary<int, ItemColorData.ColorData> dictionary2 = (Dictionary<int, ItemColorData.ColorData>) null;
      if (!dictionary1.TryGetValue(_category, out dictionary2))
        return (ItemColorData.ColorData) null;
      ItemColorData.ColorData colorData = (ItemColorData.ColorData) null;
      return dictionary2.TryGetValue(_id, out colorData) ? colorData : (ItemColorData.ColorData) null;
    }

    private ExcelData LoadExcelData(string _bundlePath, string _fileName)
    {
      string manifestName = string.Empty;
      if (AssetBundleCheck.IsSimulation)
      {
        if (!AssetBundleCheck.FindFile(_bundlePath, _fileName, false))
          return (ExcelData) null;
      }
      else
      {
        bool flag = false;
        foreach (KeyValuePair<string, AssetBundleManager.BundlePack> keyValuePair in AssetBundleManager.ManifestBundlePack.Where<KeyValuePair<string, AssetBundleManager.BundlePack>>((Func<KeyValuePair<string, AssetBundleManager.BundlePack>, bool>) (v => Regex.Match(v.Key, "studio(\\d*)").Success)))
        {
          flag |= ((IEnumerable<string>) keyValuePair.Value.AssetBundleManifest.GetAllAssetBundles()).ToList<string>().FindIndex((Predicate<string>) (s => s == _bundlePath)) != -1;
          if (flag)
          {
            manifestName = keyValuePair.Key;
            break;
          }
        }
        if (!flag)
          return (ExcelData) null;
      }
      ExcelData excelData = CommonLib.LoadAsset<ExcelData>(_bundlePath, _fileName, false, manifestName);
      return Object.op_Equality((Object) null, (Object) excelData) ? (ExcelData) null : excelData;
    }

    private string[] FindAllAssetName(string _bundlePath, string _regex)
    {
      string[] strArray = (string[]) null;
      if (AssetBundleCheck.IsSimulation)
      {
        strArray = AssetBundleCheck.FindAllAssetName(_bundlePath, _regex, false, RegexOptions.IgnoreCase);
      }
      else
      {
        foreach (KeyValuePair<string, AssetBundleManager.BundlePack> keyValuePair in AssetBundleManager.ManifestBundlePack.Where<KeyValuePair<string, AssetBundleManager.BundlePack>>((Func<KeyValuePair<string, AssetBundleManager.BundlePack>, bool>) (v => Regex.Match(v.Key, "studio(\\d*)").Success)))
        {
          if (((IEnumerable<string>) keyValuePair.Value.AssetBundleManifest.GetAllAssetBundles()).ToList<string>().FindIndex((Predicate<string>) (s => s == _bundlePath)) != -1)
          {
            LoadedAssetBundle loadedAssetBundle = (LoadedAssetBundle) null;
            if (!keyValuePair.Value.LoadedAssetBundles.TryGetValue(_bundlePath, out loadedAssetBundle))
            {
              loadedAssetBundle = AssetBundleManager.LoadAssetBundle(_bundlePath, false, keyValuePair.Key);
              if (loadedAssetBundle == null)
                break;
            }
            string[] allAssetNames = loadedAssetBundle.m_AssetBundle.GetAllAssetNames();
            // ISSUE: reference to a compiler-generated field
            if (Info.\u003C\u003Ef__mg\u0024cache0 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Info.\u003C\u003Ef__mg\u0024cache0 = new Func<string, string>(Path.GetFileNameWithoutExtension);
            }
            // ISSUE: reference to a compiler-generated field
            Func<string, string> fMgCache0 = Info.\u003C\u003Ef__mg\u0024cache0;
            strArray = ((IEnumerable<string>) allAssetNames).Select<string, string>(fMgCache0).Where<string>((Func<string, bool>) (s => Regex.Match(s, _regex, RegexOptions.IgnoreCase).Success)).ToArray<string>();
            loadedAssetBundle = (LoadedAssetBundle) null;
            break;
          }
        }
      }
      return strArray;
    }

    private void LoadAccessoryGroupInfo(ExcelData _ed)
    {
      if (Object.op_Equality((Object) _ed, (Object) null))
        return;
      foreach (List<string> list in _ed.list.Skip<ExcelData.Param>(1).Select<ExcelData.Param, List<string>>((Func<ExcelData.Param, List<string>>) (v => v.list)))
      {
        int result = -1;
        if (int.TryParse(list.SafeGet<string>(0), out result))
        {
          string str = list.SafeGet<string>(1);
          if (!str.IsNullOrEmpty())
            this.dicAccessoryGroup[result] = new Info.AccessoryGroupInfo(str, list.SafeGet<string>(2));
        }
      }
      this.m_AccessoryPointGroup = _ed;
      int num1 = 0;
      foreach (KeyValuePair<int, Info.AccessoryGroupInfo> keyValuePair in this.dicAccessoryGroup)
      {
        int num2 = keyValuePair.Value.Targets.SafeGet<int>(0);
        int num3 = keyValuePair.Value.Targets.SafeGet<int>(1);
        for (int index = num2; index <= num3; ++index)
          ++num1;
      }
      this.AccessoryPointNum = num1;
    }

    private void LoadBoneInfo(ExcelData _ed, Dictionary<int, Info.BoneInfo> _dic)
    {
      if (Object.op_Equality((Object) _ed, (Object) null))
        return;
      foreach (List<string> _lst in _ed.list.Skip<ExcelData.Param>(1).Select<ExcelData.Param, List<string>>((Func<ExcelData.Param, List<string>>) (v => v.list)))
      {
        int num1 = 0;
        int result = -1;
        List<string> stringList1 = _lst;
        int index1 = num1;
        int num2 = index1 + 1;
        if (int.TryParse(stringList1[index1], out result))
        {
          List<string> stringList2 = _lst;
          int index2 = num2;
          int num3 = index2 + 1;
          string str = stringList2[index2];
          if (!str.IsNullOrEmpty())
            _dic[result] = new Info.BoneInfo(result, str, _lst);
        }
      }
    }

    [DebuggerHidden]
    private IEnumerator LoadItemLoadInfoCoroutine(string _bundlePath, string _regex)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Info.\u003CLoadItemLoadInfoCoroutine\u003Ec__Iterator1()
      {
        _bundlePath = _bundlePath,
        _regex = _regex,
        \u0024this = this
      };
    }

    private void SortDictionary(
      string[] files,
      string _regex,
      SortedDictionary<int, SortedDictionary<int, string>> _sortDic)
    {
      string lower = _regex.Split('_')[0].ToLower();
      for (int index = 0; index < files.Length; ++index)
      {
        if (files[index].Split('_')[0].ToLower().CompareTo(lower) == 0)
        {
          Match match = Regex.Match(files[index], _regex, RegexOptions.IgnoreCase);
          int key1 = int.Parse(match.Groups[1].Value);
          int key2 = int.Parse(match.Groups[2].Value);
          if (!_sortDic.ContainsKey(key1))
            _sortDic.Add(key1, new SortedDictionary<int, string>());
          _sortDic[key1].Add(key2, files[index]);
        }
      }
    }

    private void LoadItemLoadInfo(ExcelData _ed)
    {
      if (Object.op_Equality((Object) _ed, (Object) null))
        return;
      foreach (List<string> stringList in _ed.list.Skip<ExcelData.Param>(1).Select<ExcelData.Param, List<string>>((Func<ExcelData.Param, List<string>>) (v => v.list)))
      {
        int result = -1;
        if (!int.TryParse(stringList.SafeGet<string>(0), out result))
          break;
        int key1 = int.Parse(stringList[1]);
        int key2 = int.Parse(stringList[2]);
        if (!this.dicItemLoadInfo.ContainsKey(key1))
          this.dicItemLoadInfo[key1] = new Dictionary<int, Dictionary<int, Info.ItemLoadInfo>>();
        if (!this.dicItemLoadInfo[key1].ContainsKey(key2))
          this.dicItemLoadInfo[key1][key2] = new Dictionary<int, Info.ItemLoadInfo>();
        this.dicItemLoadInfo[key1][key2][result] = new Info.ItemLoadInfo(stringList);
      }
    }

    private void LoadItemBoneInfo(string _bundlePath, string _regex)
    {
      string[] allAssetName = this.FindAllAssetName(_bundlePath, _regex);
      if (((IList<string>) allAssetName).IsNullOrEmpty<string>())
        return;
      SortedDictionary<int, SortedDictionary<int, string>> _sortDic = new SortedDictionary<int, SortedDictionary<int, string>>();
      this.SortDictionary(allAssetName, _regex, _sortDic);
      foreach (KeyValuePair<int, SortedDictionary<int, string>> keyValuePair1 in _sortDic)
      {
        foreach (KeyValuePair<int, string> keyValuePair2 in keyValuePair1.Value)
          this.LoadItemBoneInfo(this.LoadExcelData(_bundlePath, keyValuePair2.Value), keyValuePair1.Key, keyValuePair2.Key);
      }
    }

    private void LoadItemColorData(string _bundlePath, string _file)
    {
      string manifestName = string.Empty;
      if (AssetBundleCheck.IsSimulation)
      {
        if (!AssetBundleCheck.FindFile(_bundlePath, _file, false))
          return;
      }
      else
      {
        bool flag = false;
        foreach (KeyValuePair<string, AssetBundleManager.BundlePack> keyValuePair in AssetBundleManager.ManifestBundlePack.Where<KeyValuePair<string, AssetBundleManager.BundlePack>>((Func<KeyValuePair<string, AssetBundleManager.BundlePack>, bool>) (v => Regex.Match(v.Key, "studio(\\d*)").Success)))
        {
          flag |= ((IEnumerable<string>) keyValuePair.Value.AssetBundleManifest.GetAllAssetBundles()).ToList<string>().FindIndex((Predicate<string>) (s => s == _bundlePath)) != -1;
          if (flag)
          {
            manifestName = keyValuePair.Key;
            break;
          }
        }
        if (!flag)
          return;
      }
      ItemColorData itemColorData = CommonLib.LoadAsset<ItemColorData>(_bundlePath, _file, false, manifestName);
      if (Object.op_Equality((Object) itemColorData, (Object) null))
        return;
      foreach (KeyValuePair<int, Dictionary<int, Dictionary<int, ItemColorData.ColorData>>> colorData in itemColorData.ColorDatas)
      {
        Dictionary<int, Dictionary<int, ItemColorData.ColorData>> dictionary1 = (Dictionary<int, Dictionary<int, ItemColorData.ColorData>>) null;
        if (!this.dicItemColorData.TryGetValue(colorData.Key, out dictionary1))
        {
          dictionary1 = new Dictionary<int, Dictionary<int, ItemColorData.ColorData>>();
          this.dicItemColorData.Add(colorData.Key, dictionary1);
        }
        foreach (KeyValuePair<int, Dictionary<int, ItemColorData.ColorData>> keyValuePair1 in colorData.Value)
        {
          Dictionary<int, ItemColorData.ColorData> dictionary2 = (Dictionary<int, ItemColorData.ColorData>) null;
          if (!dictionary1.TryGetValue(keyValuePair1.Key, out dictionary2))
          {
            dictionary2 = new Dictionary<int, ItemColorData.ColorData>();
            dictionary1.Add(keyValuePair1.Key, dictionary2);
          }
          foreach (KeyValuePair<int, ItemColorData.ColorData> keyValuePair2 in keyValuePair1.Value)
            dictionary2[keyValuePair2.Key] = new ItemColorData.ColorData(keyValuePair2.Value);
        }
      }
    }

    private void LoadItemBoneInfo(ExcelData _ed, int _group, int _category)
    {
      if (Object.op_Equality((Object) _ed, (Object) null))
        return;
      Dictionary<int, Dictionary<int, Info.ItemLoadInfo>> dictionary1 = (Dictionary<int, Dictionary<int, Info.ItemLoadInfo>>) null;
      if (!this.dicItemLoadInfo.TryGetValue(_group, out dictionary1))
        return;
      Dictionary<int, Info.ItemLoadInfo> dictionary2 = (Dictionary<int, Info.ItemLoadInfo>) null;
      if (!dictionary1.TryGetValue(_category, out dictionary2))
        return;
      foreach (List<string> stringList in _ed.list.Skip<ExcelData.Param>(1).Select<ExcelData.Param, List<string>>((Func<ExcelData.Param, List<string>>) (v => v.list)))
      {
        Info.ItemLoadInfo itemLoadInfo = (Info.ItemLoadInfo) null;
        int result = -1;
        if (int.TryParse(stringList.SafeGet<string>(0), out result) && dictionary2.TryGetValue(result, out itemLoadInfo))
          itemLoadInfo.bones = stringList.Skip<string>(1).Where<string>((Func<string, bool>) (s => !s.IsNullOrEmpty())).ToList<string>();
      }
    }

    private void LoadLightLoadInfo(ExcelData _ed)
    {
      if (Object.op_Equality((Object) _ed, (Object) null))
        return;
      foreach (List<string> stringList1 in _ed.list.Skip<ExcelData.Param>(1).Select<ExcelData.Param, List<string>>((Func<ExcelData.Param, List<string>>) (v => v.list)))
      {
        int num1 = 0;
        Info.LightLoadInfo lightLoadInfo1 = new Info.LightLoadInfo();
        Info.LightLoadInfo lightLoadInfo2 = lightLoadInfo1;
        List<string> stringList2 = stringList1;
        int index1 = num1;
        int num2 = index1 + 1;
        int num3 = int.Parse(stringList2[index1]);
        lightLoadInfo2.no = num3;
        Info.LightLoadInfo lightLoadInfo3 = lightLoadInfo1;
        List<string> stringList3 = stringList1;
        int index2 = num2;
        int num4 = index2 + 1;
        string str1 = stringList3[index2];
        lightLoadInfo3.name = str1;
        Info.LightLoadInfo lightLoadInfo4 = lightLoadInfo1;
        List<string> stringList4 = stringList1;
        int index3 = num4;
        int num5 = index3 + 1;
        string str2 = stringList4[index3];
        lightLoadInfo4.manifest = str2;
        Info.LightLoadInfo lightLoadInfo5 = lightLoadInfo1;
        List<string> stringList5 = stringList1;
        int index4 = num5;
        int num6 = index4 + 1;
        string str3 = stringList5[index4];
        lightLoadInfo5.bundlePath = str3;
        Info.LightLoadInfo lightLoadInfo6 = lightLoadInfo1;
        List<string> stringList6 = stringList1;
        int index5 = num6;
        int num7 = index5 + 1;
        string str4 = stringList6[index5];
        lightLoadInfo6.fileName = str4;
        Info.LightLoadInfo lightLoadInfo7 = lightLoadInfo1;
        List<string> stringList7 = stringList1;
        int index6 = num7;
        int num8 = index6 + 1;
        int num9 = int.Parse(stringList7[index6]);
        lightLoadInfo7.target = (Info.LightLoadInfo.Target) num9;
        this.dicLightLoadInfo[lightLoadInfo1.no] = lightLoadInfo1;
      }
    }

    private void LoadAnimeGroupInfo(ExcelData _ed, Dictionary<int, Info.GroupInfo> _dic)
    {
      if (Object.op_Equality((Object) _ed, (Object) null))
        return;
      foreach (List<string> stringList1 in _ed.list.Skip<ExcelData.Param>(1).Select<ExcelData.Param, List<string>>((Func<ExcelData.Param, List<string>>) (v => v.list)))
      {
        int num1 = 0;
        int result1 = 0;
        List<string> list1 = stringList1;
        int index1 = num1;
        int num2 = index1 + 1;
        if (int.TryParse(list1.SafeGet<string>(index1), out result1))
        {
          int result2 = -1;
          List<string> list2 = stringList1;
          int index2 = num2;
          int num3 = index2 + 1;
          if (int.TryParse(list2.SafeGet<string>(index2), out result2))
          {
            List<string> stringList2 = stringList1;
            int index3 = num3;
            int num4 = index3 + 1;
            string str = stringList2[index3];
            Info.GroupInfo groupInfo1 = (Info.GroupInfo) null;
            if (_dic.TryGetValue(result2, out groupInfo1))
            {
              groupInfo1.sort = result1;
              groupInfo1.name = str;
            }
            else
            {
              Info.GroupInfo groupInfo2 = new Info.GroupInfo()
              {
                sort = result1,
                name = str
              };
              groupInfo2.name = str;
              _dic.Add(result2, groupInfo2);
            }
          }
        }
      }
    }

    private void LoadAnimeCategoryInfo(
      string _bundlePath,
      string _regex,
      Dictionary<int, Info.GroupInfo> _dic)
    {
      string[] allAssetName = this.FindAllAssetName(_bundlePath, _regex);
      if (((IList<string>) allAssetName).IsNullOrEmpty<string>())
        return;
      string lower = _regex.Split('_')[0].ToLower();
      SortedDictionary<int, SortedDictionary<int, string>> sortedDictionary = new SortedDictionary<int, SortedDictionary<int, string>>();
      for (int index = 0; index < allAssetName.Length; ++index)
      {
        if (allAssetName[index].Split('_')[0].ToLower().CompareTo(lower) == 0)
        {
          Match match = Regex.Match(allAssetName[index], _regex, RegexOptions.IgnoreCase);
          int key1 = int.Parse(match.Groups[1].Value);
          int key2 = int.Parse(match.Groups[2].Value);
          if (!sortedDictionary.ContainsKey(key2))
            sortedDictionary.Add(key2, new SortedDictionary<int, string>());
          sortedDictionary[key2].Add(key1, allAssetName[index]);
        }
      }
      foreach (KeyValuePair<int, SortedDictionary<int, string>> keyValuePair1 in sortedDictionary)
      {
        if (_dic.ContainsKey(keyValuePair1.Key))
        {
          foreach (KeyValuePair<int, string> keyValuePair2 in keyValuePair1.Value)
            this.LoadAnimeCategoryInfo(this.LoadExcelData(_bundlePath, keyValuePair2.Value), _dic[keyValuePair1.Key]);
        }
      }
    }

    private void LoadAnimeCategoryInfo(ExcelData _ed, Info.GroupInfo _info)
    {
      if (Object.op_Equality((Object) _ed, (Object) null))
        return;
      foreach (List<string> stringList in _ed.list.Skip<ExcelData.Param>(1).Select<ExcelData.Param, List<string>>((Func<ExcelData.Param, List<string>>) (v => v.list)))
      {
        int num1 = 0;
        int result1 = 0;
        List<string> list1 = stringList;
        int index1 = num1;
        int num2 = index1 + 1;
        if (int.TryParse(list1.SafeGet<string>(index1), out result1))
        {
          int result2 = -1;
          List<string> list2 = stringList;
          int index2 = num2;
          int num3 = index2 + 1;
          if (int.TryParse(list2.SafeGet<string>(index2), out result2))
          {
            Dictionary<int, Info.CategoryInfo> dicCategory = _info.dicCategory;
            int index3 = result2;
            Info.CategoryInfo categoryInfo1 = new Info.CategoryInfo();
            categoryInfo1.sort = result1;
            Info.CategoryInfo categoryInfo2 = categoryInfo1;
            List<string> list3 = stringList;
            int index4 = num3;
            int num4 = index4 + 1;
            string str = list3.SafeGet<string>(index4);
            categoryInfo2.name = str;
            Info.CategoryInfo categoryInfo3 = categoryInfo1;
            dicCategory[index3] = categoryInfo3;
          }
        }
      }
    }

    [DebuggerHidden]
    private IEnumerator LoadAnimeLoadInfoCoroutine(
      string _bundlePath,
      string _regex,
      Dictionary<int, Dictionary<int, Dictionary<int, Info.AnimeLoadInfo>>> _dic,
      Info.LoadAnimeInfoCoroutineFunc _func)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Info.\u003CLoadAnimeLoadInfoCoroutine\u003Ec__Iterator2()
      {
        _bundlePath = _bundlePath,
        _regex = _regex,
        _func = _func,
        _dic = _dic,
        \u0024this = this
      };
    }

    private void LoadAnimeLoadInfo(
      ExcelData _ed,
      Dictionary<int, Dictionary<int, Dictionary<int, Info.AnimeLoadInfo>>> _dic)
    {
      if (Object.op_Equality((Object) _ed, (Object) null))
        return;
      foreach (List<string> list1 in _ed.list.Skip<ExcelData.Param>(2).Select<ExcelData.Param, List<string>>((Func<ExcelData.Param, List<string>>) (v => v.list)))
      {
        if (this.fileCheck.Check(list1.SafeGet<string>(5)))
        {
          int num1 = 0;
          int result1 = 0;
          List<string> list2 = list1;
          int index1 = num1;
          int num2 = index1 + 1;
          if (int.TryParse(list2.SafeGet<string>(index1), out result1))
          {
            int result2 = -1;
            List<string> list3 = list1;
            int index2 = num2;
            int num3 = index2 + 1;
            if (int.TryParse(list3.SafeGet<string>(index2), out result2))
            {
              List<string> list4 = list1;
              int index3 = num3;
              int num4 = index3 + 1;
              int key1 = int.Parse(list4.SafeGet<string>(index3));
              List<string> list5 = list1;
              int index4 = num4;
              int num5 = index4 + 1;
              int key2 = int.Parse(list5.SafeGet<string>(index4));
              if (!_dic.ContainsKey(key1))
                _dic.Add(key1, new Dictionary<int, Dictionary<int, Info.AnimeLoadInfo>>());
              if (!_dic[key1].ContainsKey(key2))
                _dic[key1].Add(key2, new Dictionary<int, Info.AnimeLoadInfo>());
              Dictionary<int, Info.AnimeLoadInfo> dictionary = _dic[key1][key2];
              int index5 = result2;
              Info.AnimeLoadInfo animeLoadInfo1 = new Info.AnimeLoadInfo();
              animeLoadInfo1.sort = result1;
              Info.AnimeLoadInfo animeLoadInfo2 = animeLoadInfo1;
              List<string> list6 = list1;
              int index6 = num5;
              int num6 = index6 + 1;
              string str1 = list6.SafeGet<string>(index6);
              animeLoadInfo2.name = str1;
              Info.AnimeLoadInfo animeLoadInfo3 = animeLoadInfo1;
              List<string> list7 = list1;
              int index7 = num6;
              int num7 = index7 + 1;
              string str2 = list7.SafeGet<string>(index7);
              animeLoadInfo3.bundlePath = str2;
              Info.AnimeLoadInfo animeLoadInfo4 = animeLoadInfo1;
              List<string> list8 = list1;
              int index8 = num7;
              int num8 = index8 + 1;
              string str3 = list8.SafeGet<string>(index8);
              animeLoadInfo4.fileName = str3;
              Info.AnimeLoadInfo animeLoadInfo5 = animeLoadInfo1;
              List<string> list9 = list1;
              int index9 = num8;
              int num9 = index9 + 1;
              string str4 = list9.SafeGet<string>(index9);
              animeLoadInfo5.clip = str4;
              Info.AnimeLoadInfo animeLoadInfo6 = animeLoadInfo1;
              List<string> _list = list1;
              int _start = num9;
              int num10 = _start + 1;
              List<Info.OptionItemInfo> optionItemInfoList = Info.AnimeLoadInfo.LoadOption(_list, _start, true);
              animeLoadInfo6.option = optionItemInfoList;
              Info.AnimeLoadInfo animeLoadInfo7 = animeLoadInfo1;
              dictionary[index5] = animeLoadInfo7;
            }
          }
        }
      }
    }

    private void LoadHAnimeLoadInfo(
      ExcelData _ed,
      Dictionary<int, Dictionary<int, Dictionary<int, Info.AnimeLoadInfo>>> _dic)
    {
      if (Object.op_Equality((Object) _ed, (Object) null))
        return;
      foreach (List<string> stringList in _ed.list.Skip<ExcelData.Param>(2).Select<ExcelData.Param, List<string>>((Func<ExcelData.Param, List<string>>) (v => v.list)))
      {
        if (this.fileCheck.Check(stringList.SafeGet<string>(5)))
        {
          int result1 = 0;
          if (int.TryParse(stringList.SafeGet<string>(0), out result1))
          {
            int result2 = 0;
            if (int.TryParse(stringList.SafeGet<string>(1), out result2))
            {
              int key1 = int.Parse(stringList.SafeGet<string>(2));
              int key2 = int.Parse(stringList.SafeGet<string>(3));
              if (!_dic.ContainsKey(key1))
                _dic.Add(key1, new Dictionary<int, Dictionary<int, Info.AnimeLoadInfo>>());
              if (!_dic[key1].ContainsKey(key2))
                _dic[key1].Add(key2, new Dictionary<int, Info.AnimeLoadInfo>());
              _dic[key1][key2][result2] = (Info.AnimeLoadInfo) new Info.HAnimeLoadInfo(4, stringList);
            }
          }
        }
      }
    }

    [DebuggerHidden]
    private IEnumerator LoadVoiceLoadInfoCoroutine(string _bundlePath, string _regex)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Info.\u003CLoadVoiceLoadInfoCoroutine\u003Ec__Iterator3()
      {
        _bundlePath = _bundlePath,
        _regex = _regex,
        \u0024this = this
      };
    }

    private void LoadVoiceLoadInfo(ExcelData _ed)
    {
      foreach (List<string> _list in _ed.list.Skip<ExcelData.Param>(2).Select<ExcelData.Param, List<string>>((Func<ExcelData.Param, List<string>>) (v => v.list)))
        this.LoadVoiceLoadInfo(_list);
    }

    private Info.LoadCommonInfo LoadVoiceLoadInfo(List<string> _list)
    {
      int num1 = 0;
      Info.LoadCommonInfo loadCommonInfo1 = new Info.LoadCommonInfo();
      int result1 = -1;
      List<string> stringList1 = _list;
      int index1 = num1;
      int num2 = index1 + 1;
      if (!int.TryParse(stringList1[index1], out result1))
        return (Info.LoadCommonInfo) null;
      int result2 = -1;
      List<string> stringList2 = _list;
      int index2 = num2;
      int num3 = index2 + 1;
      if (!int.TryParse(stringList2[index2], out result2))
        return (Info.LoadCommonInfo) null;
      int result3 = -1;
      List<string> stringList3 = _list;
      int index3 = num3;
      int num4 = index3 + 1;
      if (!int.TryParse(stringList3[index3], out result3))
        return (Info.LoadCommonInfo) null;
      Info.LoadCommonInfo loadCommonInfo2 = loadCommonInfo1;
      List<string> stringList4 = _list;
      int index4 = num4;
      int num5 = index4 + 1;
      string str1 = stringList4[index4];
      loadCommonInfo2.name = str1;
      Info.LoadCommonInfo loadCommonInfo3 = loadCommonInfo1;
      List<string> stringList5 = _list;
      int index5 = num5;
      int num6 = index5 + 1;
      string str2 = stringList5[index5];
      loadCommonInfo3.bundlePath = str2;
      Info.LoadCommonInfo loadCommonInfo4 = loadCommonInfo1;
      List<string> stringList6 = _list;
      int index6 = num6;
      int num7 = index6 + 1;
      string str3 = stringList6[index6];
      loadCommonInfo4.fileName = str3;
      if (!this.dicVoiceLoadInfo.ContainsKey(result2))
        this.dicVoiceLoadInfo.Add(result2, new Dictionary<int, Dictionary<int, Info.LoadCommonInfo>>());
      if (!this.dicVoiceLoadInfo[result2].ContainsKey(result3))
        this.dicVoiceLoadInfo[result2].Add(result3, new Dictionary<int, Info.LoadCommonInfo>());
      this.dicVoiceLoadInfo[result2][result3][result1] = loadCommonInfo1;
      return loadCommonInfo1;
    }

    private void LoadSoundLoadInfo(ExcelData _ed, Dictionary<int, Info.LoadCommonInfo> _dic)
    {
      if (Object.op_Equality((Object) _ed, (Object) null))
        return;
      foreach (List<string> stringList1 in _ed.list.Skip<ExcelData.Param>(1).Select<ExcelData.Param, List<string>>((Func<ExcelData.Param, List<string>>) (v => v.list)))
      {
        int num1 = 0;
        int result = -1;
        List<string> stringList2 = stringList1;
        int index1 = num1;
        int num2 = index1 + 1;
        if (int.TryParse(stringList2[index1], out result))
        {
          Dictionary<int, Info.LoadCommonInfo> dictionary = _dic;
          int index2 = result;
          Info.LoadCommonInfo loadCommonInfo1 = new Info.LoadCommonInfo();
          Info.LoadCommonInfo loadCommonInfo2 = loadCommonInfo1;
          List<string> stringList3 = stringList1;
          int index3 = num2;
          int num3 = index3 + 1;
          string str1 = stringList3[index3];
          loadCommonInfo2.name = str1;
          Info.LoadCommonInfo loadCommonInfo3 = loadCommonInfo1;
          List<string> stringList4 = stringList1;
          int index4 = num3;
          int num4 = index4 + 1;
          string str2 = stringList4[index4];
          loadCommonInfo3.bundlePath = str2;
          Info.LoadCommonInfo loadCommonInfo4 = loadCommonInfo1;
          List<string> stringList5 = stringList1;
          int index5 = num4;
          int num5 = index5 + 1;
          string str3 = stringList5[index5];
          loadCommonInfo4.fileName = str3;
          Info.LoadCommonInfo loadCommonInfo5 = loadCommonInfo1;
          dictionary[index2] = loadCommonInfo5;
        }
      }
    }

    private void LoadMapLoadInfo(ExcelData _ed, Dictionary<int, Info.MapLoadInfo> _dic)
    {
      if (Object.op_Equality((Object) _ed, (Object) null))
        return;
      foreach (List<string> _list in _ed.list.Skip<ExcelData.Param>(2).Select<ExcelData.Param, List<string>>((Func<ExcelData.Param, List<string>>) (v => v.list)))
      {
        int num1 = 0;
        int result = -1;
        List<string> stringList = _list;
        int index = num1;
        int num2 = index + 1;
        if (int.TryParse(stringList[index], out result))
          _dic[result] = new Info.MapLoadInfo(_list);
      }
    }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
      this.isLoadList = false;
    }

    public class FileInfo
    {
      public string manifest = string.Empty;
      public string bundlePath = string.Empty;
      public string fileName = string.Empty;

      public bool Check
      {
        get
        {
          return !this.bundlePath.IsNullOrEmpty() & !this.fileName.IsNullOrEmpty();
        }
      }

      public void Clear()
      {
        this.manifest = string.Empty;
        this.bundlePath = string.Empty;
        this.fileName = string.Empty;
      }
    }

    public class LoadCommonInfo : Info.FileInfo
    {
      public string name = string.Empty;
    }

    public class CategoryInfo
    {
      public string name = string.Empty;
      public int sort;
    }

    public class GroupInfo
    {
      public string name = string.Empty;
      public Dictionary<int, Info.CategoryInfo> dicCategory = new Dictionary<int, Info.CategoryInfo>();
      public int sort;
    }

    public class BoneInfo
    {
      public string bone = string.Empty;
      public string name = string.Empty;
      public int group = -1;
      public int sync = -1;
      public int no;
      public int level;

      public BoneInfo(int _no, string _bone, List<string> _lst)
      {
        this.no = _no;
        this.bone = _bone;
        int num1 = 2;
        List<string> list1 = _lst;
        int index1 = num1;
        int num2 = index1 + 1;
        this.name = list1.SafeGet<string>(index1);
        List<string> list2 = _lst;
        int index2 = num2;
        int num3 = index2 + 1;
        if (!int.TryParse(list2.SafeGet<string>(index2), out this.group))
          this.group = 0;
        List<string> list3 = _lst;
        int index3 = num3;
        int num4 = index3 + 1;
        if (!int.TryParse(list3.SafeGet<string>(index3), out this.level))
          this.level = 0;
        List<string> list4 = _lst;
        int index4 = num4;
        int num5 = index4 + 1;
        if (int.TryParse(list4.SafeGet<string>(index4), out this.sync))
          return;
        this.sync = -1;
      }
    }

    public class ItemLoadInfo : Info.LoadCommonInfo
    {
      public List<string> bones;

      public ItemLoadInfo(List<string> _lst)
      {
        this.name = _lst[3];
        this.manifest = _lst[4];
        this.bundlePath = _lst[5];
        this.fileName = _lst[6];
      }
    }

    public class AccessoryGroupInfo
    {
      public AccessoryGroupInfo(string _name, string _targets)
      {
        this.Name = _name;
        string[] strArray = _targets.Split('-');
        // ISSUE: reference to a compiler-generated field
        if (Info.AccessoryGroupInfo.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Info.AccessoryGroupInfo.\u003C\u003Ef__mg\u0024cache0 = new Func<string, int>(int.Parse);
        }
        // ISSUE: reference to a compiler-generated field
        Func<string, int> fMgCache0 = Info.AccessoryGroupInfo.\u003C\u003Ef__mg\u0024cache0;
        this.Targets = ((IEnumerable<string>) strArray).Select<string, int>(fMgCache0).ToArray<int>();
      }

      public string Name { get; private set; } = string.Empty;

      public int[] Targets { get; private set; }
    }

    public class LightLoadInfo : Info.LoadCommonInfo
    {
      public int no;
      public Info.LightLoadInfo.Target target;

      public enum Target
      {
        All,
        Chara,
        Map,
      }
    }

    public class ParentageInfo
    {
      public string parent = string.Empty;
      public string child = string.Empty;
    }

    public class OptionItemInfo : Info.FileInfo
    {
      public bool isAnimeSync = true;
      public Info.FileInfo anmInfo;
      public Info.FileInfo anmOveride;
      public Info.ParentageInfo[] parentageInfo;
      public bool counterScale;
    }

    public class AnimeLoadInfo : Info.LoadCommonInfo
    {
      public string clip = string.Empty;
      public int sort;
      public List<Info.OptionItemInfo> option;

      public static List<Info.OptionItemInfo> LoadOption(
        List<string> _list,
        int _start,
        bool _animeSync)
      {
        List<Info.OptionItemInfo> optionItemInfoList = new List<Info.OptionItemInfo>();
        int num1 = _start;
        while (true)
        {
          Info.OptionItemInfo info = new Info.OptionItemInfo();
          List<string> args1 = _list;
          int index1 = num1;
          int num2 = index1 + 1;
          Action<string> act1 = (Action<string>) (_s => info.bundlePath = _s);
          if (args1.SafeProc(index1, act1))
          {
            List<string> args2 = _list;
            int index2 = num2;
            int num3 = index2 + 1;
            Action<string> act2 = (Action<string>) (_s => info.fileName = _s);
            if (args2.SafeProc(index2, act2))
            {
              List<string> args3 = _list;
              int index3 = num3;
              int num4 = index3 + 1;
              Action<string> act3 = (Action<string>) (_s => info.manifest = _s);
              if (args3.SafeProc(index3, act3))
              {
                info.anmInfo = new Info.FileInfo();
                Info.FileInfo anmInfo1 = info.anmInfo;
                List<string> list1 = _list;
                int index4 = num4;
                int num5 = index4 + 1;
                string str1 = list1.SafeGet<string>(index4);
                anmInfo1.bundlePath = str1;
                Info.FileInfo anmInfo2 = info.anmInfo;
                List<string> list2 = _list;
                int index5 = num5;
                int num6 = index5 + 1;
                string str2 = list2.SafeGet<string>(index5);
                anmInfo2.fileName = str2;
                info.anmOveride = new Info.FileInfo();
                Info.FileInfo anmOveride1 = info.anmOveride;
                List<string> list3 = _list;
                int index6 = num6;
                int num7 = index6 + 1;
                string str3 = list3.SafeGet<string>(index6);
                anmOveride1.bundlePath = str3;
                Info.FileInfo anmOveride2 = info.anmOveride;
                List<string> list4 = _list;
                int index7 = num7;
                int num8 = index7 + 1;
                string str4 = list4.SafeGet<string>(index7);
                anmOveride2.fileName = str4;
                Info.OptionItemInfo optionItemInfo = info;
                List<string> list5 = _list;
                int index8 = num8;
                int num9 = index8 + 1;
                Info.ParentageInfo[] parentageInfoArray = Info.AnimeLoadInfo.AnalysisParentageInfo(list5.SafeGet<string>(index8));
                optionItemInfo.parentageInfo = parentageInfoArray;
                List<string> list6 = _list;
                int index9 = num9;
                num1 = index9 + 1;
                bool.TryParse(list6.SafeGet<string>(index9), out info.counterScale);
                if (_animeSync && !bool.TryParse(_list.SafeGet<string>(num1++), out info.isAnimeSync))
                  info.isAnimeSync = true;
                optionItemInfoList.Add(info);
              }
              else
                break;
            }
            else
              break;
          }
          else
            break;
        }
        return optionItemInfoList;
      }

      private static Info.ParentageInfo[] AnalysisParentageInfo(string _str)
      {
        if (_str.IsNullOrEmpty())
          return (Info.ParentageInfo[]) null;
        string[] strArray1 = _str.Split(',');
        List<Info.ParentageInfo> parentageInfoList = new List<Info.ParentageInfo>();
        for (int index = 0; index < strArray1.Length; ++index)
        {
          string[] strArray2 = strArray1[index].Split('/');
          Info.ParentageInfo parentageInfo = new Info.ParentageInfo();
          parentageInfo.parent = strArray2[0];
          if (strArray2.Length > 1)
            parentageInfo.child = strArray2[1];
          parentageInfoList.Add(parentageInfo);
        }
        return parentageInfoList.ToArray();
      }
    }

    public class HAnimeLoadInfo : Info.AnimeLoadInfo
    {
      public int breastLayer = -1;
      public Info.FileInfo overrideFile;
      public Info.FileInfo yureFile;
      public int motionID;
      public int num;
      public bool isMotion;
      public bool[] pv;

      public HAnimeLoadInfo(int _startIdx, List<string> _list)
      {
        int num1 = _startIdx;
        int.TryParse(_list.SafeGet<string>(0), out this.sort);
        List<string> list1 = _list;
        int index1 = num1;
        int num2 = index1 + 1;
        this.name = list1.SafeGet<string>(index1);
        List<string> list2 = _list;
        int index2 = num2;
        int num3 = index2 + 1;
        this.bundlePath = list2.SafeGet<string>(index2);
        List<string> list3 = _list;
        int index3 = num3;
        int num4 = index3 + 1;
        this.fileName = list3.SafeGet<string>(index3);
        Info.FileInfo fileInfo1 = new Info.FileInfo();
        Info.FileInfo fileInfo2 = fileInfo1;
        List<string> list4 = _list;
        int index4 = num4;
        int num5 = index4 + 1;
        string str1 = list4.SafeGet<string>(index4);
        fileInfo2.bundlePath = str1;
        Info.FileInfo fileInfo3 = fileInfo1;
        List<string> list5 = _list;
        int index5 = num5;
        int num6 = index5 + 1;
        string str2 = list5.SafeGet<string>(index5);
        fileInfo3.fileName = str2;
        this.overrideFile = fileInfo1;
        List<string> list6 = _list;
        int index6 = num6;
        int num7 = index6 + 1;
        this.clip = list6.SafeGet<string>(index6);
        List<string> list7 = _list;
        int index7 = num7;
        int num8 = index7 + 1;
        int.TryParse(list7.SafeGet<string>(index7), out this.breastLayer);
        Info.FileInfo fileInfo4 = new Info.FileInfo();
        Info.FileInfo fileInfo5 = fileInfo4;
        List<string> list8 = _list;
        int index8 = num8;
        int num9 = index8 + 1;
        string str3 = list8.SafeGet<string>(index8);
        fileInfo5.bundlePath = str3;
        Info.FileInfo fileInfo6 = fileInfo4;
        List<string> list9 = _list;
        int index9 = num9;
        int num10 = index9 + 1;
        string str4 = list9.SafeGet<string>(index9);
        fileInfo6.fileName = str4;
        this.yureFile = fileInfo4;
        List<string> list10 = _list;
        int index10 = num10;
        int num11 = index10 + 1;
        int.TryParse(list10.SafeGet<string>(index10), out this.motionID);
        List<string> list11 = _list;
        int index11 = num11;
        int num12 = index11 + 1;
        int.TryParse(list11.SafeGet<string>(index11), out this.num);
        List<string> list12 = _list;
        int index12 = num12;
        int num13 = index12 + 1;
        this.isMotion = bool.Parse(list12.SafeGet<string>(index12));
        this.pv = Enumerable.Repeat<bool>(true, 8).ToArray<bool>();
        for (int index13 = 0; index13 < 8; ++index13)
        {
          bool result = true;
          if (bool.TryParse(_list.SafeGet<string>(num13++), out result))
            this.pv[index13] = result;
        }
        List<string> _list1 = _list;
        int _start = num13;
        int num14 = _start + 1;
        this.option = Info.AnimeLoadInfo.LoadOption(_list1, _start, false);
      }

      public bool isBreastLayer
      {
        get
        {
          return this.breastLayer != -1;
        }
      }
    }

    public class HandAnimeInfo : Info.LoadCommonInfo
    {
      public string clip = string.Empty;
    }

    public class MapLoadInfo : Info.LoadCommonInfo
    {
      public Info.FileInfo vanish;

      public MapLoadInfo(List<string> _list)
      {
        this.name = _list[1];
        this.bundlePath = _list[2];
        this.fileName = _list[3];
        this.manifest = _list.SafeGet<string>(4);
        this.vanish = new Info.FileInfo();
        this.vanish.bundlePath = _list.SafeGet<string>(5);
        this.vanish.fileName = _list.SafeGet<string>(6);
      }
    }

    private class FileCheck
    {
      private Dictionary<string, bool> dicConfirmed;

      public FileCheck()
      {
        this.dicConfirmed = new Dictionary<string, bool>();
      }

      public bool Check(string _path)
      {
        if (_path.IsNullOrEmpty())
          return false;
        bool flag1 = false;
        if (this.dicConfirmed.TryGetValue(_path, out flag1))
          return flag1;
        bool flag2 = !AssetBundleCheck.IsSimulation && File.Exists(AssetBundleManager.BaseDownloadingURL + _path);
        this.dicConfirmed.Add(_path, flag2);
        return flag2;
      }
    }

    public class WaitTime
    {
      private const float intervalTime = 0.03f;
      private float nextFrameTime;

      public WaitTime()
      {
        this.Next();
      }

      public bool isOver
      {
        get
        {
          return (double) Time.get_realtimeSinceStartup() >= (double) this.nextFrameTime;
        }
      }

      public void Next()
      {
        this.nextFrameTime = Time.get_realtimeSinceStartup() + 0.03f;
      }
    }

    private class FileListInfo
    {
      public Dictionary<string, string[]> dicFile;

      public FileListInfo(List<string> _list)
      {
        this.dicFile = new Dictionary<string, string[]>();
        foreach (string str in _list)
          this.dicFile.Add(str, AssetBundleCheck.GetAllFileName(str, false));
      }

      public bool Check(string _path, string _file)
      {
        string[] strArray = (string[]) null;
        if (!AssetBundleCheck.IsSimulation)
          _file = _file.ToLower();
        return this.dicFile.TryGetValue(_path, out strArray) && ((IEnumerable<string>) strArray).Contains<string>(_file);
      }
    }

    private delegate void LoadAnimeInfoCoroutineFunc(
      ExcelData _ed,
      Dictionary<int, Dictionary<int, Dictionary<int, Info.AnimeLoadInfo>>> _dic);
  }
}
