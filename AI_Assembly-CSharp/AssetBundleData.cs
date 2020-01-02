// Decompiled with JetBrains decompiler
// Type: AssetBundleData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class AssetBundleData
{
  public string bundle = string.Empty;
  public string asset = string.Empty;
  protected AssetBundleLoadAssetOperation request;

  public AssetBundleData()
  {
  }

  public AssetBundleData(string bundle, string asset)
  {
    this.bundle = bundle;
    this.asset = asset;
  }

  public bool isEmpty
  {
    get
    {
      return this.bundle.IsNullOrEmpty() || this.asset.IsNullOrEmpty();
    }
  }

  public bool Check(string bundle, string asset)
  {
    return !asset.IsNullOrEmpty() && this.asset != asset || !bundle.IsNullOrEmpty() && this.bundle != bundle;
  }

  public virtual LoadedAssetBundle LoadedBundle
  {
    get
    {
      return AssetBundleManager.GetLoadedAssetBundle(this.bundle, out string _, (string) null);
    }
  }

  public bool isFile
  {
    get
    {
      return this.LoadedBundle != null || File.Exists(AssetBundleManager.BaseDownloadingURL + this.bundle);
    }
  }

  public virtual string[] AllAssetNames
  {
    get
    {
      LoadedAssetBundle loadedBundle = this.LoadedBundle;
      AssetBundle assetBundle = loadedBundle == null ? AssetBundle.LoadFromFile(AssetBundleManager.BaseDownloadingURL + this.bundle) : loadedBundle.m_AssetBundle;
      string[] allAssetNames = assetBundle.GetAllAssetNames();
      if (AssetBundleData.\u003C\u003Ef__mg\u0024cache0 == null)
        AssetBundleData.\u003C\u003Ef__mg\u0024cache0 = new Func<string, string>(Path.GetFileNameWithoutExtension);
      Func<string, string> fMgCache0 = AssetBundleData.\u003C\u003Ef__mg\u0024cache0;
      string[] array = ((IEnumerable<string>) allAssetNames).Select<string, string>(fMgCache0).ToArray<string>();
      if (loadedBundle == null)
        assetBundle.Unload(true);
      return array;
    }
  }

  public static List<string> GetAssetBundleNameListFromPath(string path, bool subdirCheck = false)
  {
    List<string> stringList = new List<string>();
    string basePath = AssetBundleManager.BaseDownloadingURL;
    string path1 = basePath + path;
    return !Directory.Exists(path1) ? stringList : (!subdirCheck ? (IEnumerable<string>) Directory.GetFiles(path1, "*.unity3d") : (IEnumerable<string>) Directory.GetFiles(path1, "*.unity3d", SearchOption.AllDirectories)).Select<string, string>((Func<string, string>) (s => s.Replace(basePath, string.Empty))).ToList<string>();
  }

  public void ClearRequest()
  {
    this.request = (AssetBundleLoadAssetOperation) null;
  }

  public virtual AssetBundleLoadAssetOperation LoadBundle<T>() where T : Object
  {
    return !this.isFile ? (AssetBundleLoadAssetOperation) null : this.request ?? (this.request = AssetBundleManager.LoadAsset(this, typeof (T)));
  }

  public virtual AssetBundleLoadAssetOperation LoadBundleAsync<T>() where T : Object
  {
    return !this.isFile ? (AssetBundleLoadAssetOperation) null : this.request ?? (this.request = AssetBundleManager.LoadAssetAsync(this, typeof (T)));
  }

  public virtual AssetBundleLoadAssetOperation LoadAllBundle<T>() where T : Object
  {
    return !this.isFile ? (AssetBundleLoadAssetOperation) null : this.request ?? (this.request = AssetBundleManager.LoadAllAsset(this, typeof (T)));
  }

  public virtual AssetBundleLoadAssetOperation LoadAllBundleAsync<T>() where T : Object
  {
    return !this.isFile ? (AssetBundleLoadAssetOperation) null : this.request ?? (this.request = AssetBundleManager.LoadAllAssetAsync(this, typeof (T)));
  }

  public virtual T GetAsset<T>() where T : Object
  {
    if (this.request == null)
      this.request = this.LoadBundle<T>();
    return this.request == null ? (T) null : this.request.GetAsset<T>();
  }

  public virtual T[] GetAllAssets<T>() where T : Object
  {
    if (this.request == null)
      this.request = this.LoadAllBundle<T>();
    return this.request == null ? (T[]) null : this.request.GetAllAssets<T>();
  }

  [DebuggerHidden]
  public IEnumerator GetAsset<T>(Action<T> act) where T : Object
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AssetBundleData.\u003CGetAsset\u003Ec__Iterator0<T>()
    {
      act = act,
      \u0024this = this
    };
  }

  [DebuggerHidden]
  public IEnumerator GetAllAssets<T>(Action<T[]> act) where T : Object
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AssetBundleData.\u003CGetAllAssets\u003Ec__Iterator1<T>()
    {
      act = act,
      \u0024this = this
    };
  }

  public virtual void UnloadBundle(bool isUnloadForceRefCount = false, bool unloadAllLoadedObjects = false)
  {
    if (this.request != null)
      AssetBundleManager.UnloadAssetBundle(this, isUnloadForceRefCount, unloadAllLoadedObjects);
    this.request = (AssetBundleLoadAssetOperation) null;
  }

  protected static bool isSimulation
  {
    get
    {
      return false;
    }
  }

  [Conditional("BASE_LOADER_LOG")]
  private void LogError(string str)
  {
    Debug.LogError((object) (str + " isn't loaded successfully"));
  }
}
