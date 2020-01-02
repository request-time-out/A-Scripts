// Decompiled with JetBrains decompiler
// Type: CommonLib
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CommonLib
{
  public static List<string> GetAssetBundleNameListFromPath(string path, bool subdirCheck = false)
  {
    List<string> stringList1 = new List<string>();
    if (!AssetBundleCheck.IsSimulation)
    {
      string path1 = AssetBundleManager.BaseDownloadingURL + path;
      if (subdirCheck)
      {
        List<string> stringList2 = new List<string>();
        CommonLib.GetAllFiles(path1, "*.unity3d", stringList2);
        stringList1 = stringList2.Select<string, string>((Func<string, string>) (s => s.Replace(AssetBundleManager.BaseDownloadingURL, string.Empty))).ToList<string>();
      }
      else
      {
        if (!Directory.Exists(path1))
          return stringList1;
        stringList1 = ((IEnumerable<string>) Directory.GetFiles(path1, "*.unity3d")).Select<string, string>((Func<string, string>) (s => s.Replace(AssetBundleManager.BaseDownloadingURL, string.Empty))).ToList<string>();
      }
    }
    return stringList1;
  }

  public static void GetAllFiles(string path, string searchPattern, List<string> lst)
  {
    if (!Directory.Exists(path))
      return;
    lst.AddRange((IEnumerable<string>) Directory.GetFiles(path, searchPattern));
    foreach (string directory in Directory.GetDirectories(path))
      CommonLib.GetAllFiles(directory, searchPattern, lst);
  }

  public static void CopySameNameTransform(Transform trfDst, Transform trfSrc)
  {
    FindAssist findAssist1 = new FindAssist();
    findAssist1.Initialize(trfDst);
    Dictionary<string, GameObject> dictObjName1 = findAssist1.dictObjName;
    FindAssist findAssist2 = new FindAssist();
    findAssist2.Initialize(trfSrc);
    Dictionary<string, GameObject> dictObjName2 = findAssist2.dictObjName;
    GameObject gameObject = (GameObject) null;
    using (Dictionary<string, GameObject>.Enumerator enumerator = dictObjName1.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        KeyValuePair<string, GameObject> current = enumerator.Current;
        if (dictObjName2.TryGetValue(current.Key, out gameObject))
        {
          current.Value.get_transform().set_localPosition(gameObject.get_transform().get_localPosition());
          current.Value.get_transform().set_localRotation(gameObject.get_transform().get_localRotation());
          current.Value.get_transform().set_localScale(gameObject.get_transform().get_localScale());
        }
      }
    }
  }

  public static T LoadAsset<T>(
    string assetBundleName,
    string assetName,
    bool clone = false,
    string manifestName = "")
    where T : Object
  {
    if (AssetBundleCheck.IsSimulation)
      manifestName = string.Empty;
    if (!AssetBundleCheck.IsFile(assetBundleName, assetName))
    {
      Debug.LogWarning((object) ("読み込みエラー\r\nassetBundleName：" + assetBundleName + "\tassetName：" + assetName));
      return (T) null;
    }
    AssetBundleLoadAssetOperation loadAssetOperation = AssetBundleManager.LoadAsset(assetBundleName, assetName, typeof (T), !manifestName.IsNullOrEmpty() ? manifestName : (string) null);
    if (loadAssetOperation.IsEmpty())
    {
      Debug.LogError((object) ("読み込みエラー\r\nassetName：" + assetName));
      return (T) null;
    }
    T obj1 = loadAssetOperation.GetAsset<T>();
    if (Object.op_Inequality((Object) null, (Object) (object) obj1) && clone)
    {
      T obj2 = Object.Instantiate<T>(obj1);
      obj2.set_name(obj1.get_name());
      obj1 = obj2;
    }
    return obj1;
  }
}
