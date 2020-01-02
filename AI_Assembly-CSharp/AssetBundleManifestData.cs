// Decompiled with JetBrains decompiler
// Type: AssetBundleManifestData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class AssetBundleManifestData : AssetBundleData
{
  [SerializeField]
  private string _manifest = string.Empty;

  public AssetBundleManifestData()
  {
  }

  public AssetBundleManifestData(string bundle, string asset)
    : base(bundle, asset)
  {
  }

  public AssetBundleManifestData(string bundle, string asset, string manifest)
    : base(bundle, asset)
  {
    this._manifest = manifest;
  }

  public string manifest
  {
    get
    {
      return this._manifest;
    }
    set
    {
      this._manifest = value;
    }
  }

  public new bool isEmpty
  {
    get
    {
      return base.isEmpty || this.manifest.IsNullOrEmpty();
    }
  }

  public bool Check(string bundle, string asset, string manifest)
  {
    return !manifest.IsNullOrEmpty() && this._manifest != manifest || this.Check(bundle, asset);
  }

  public override LoadedAssetBundle LoadedBundle
  {
    get
    {
      return AssetBundleManager.GetLoadedAssetBundle(this.bundle, out string _, this._manifest);
    }
  }

  public override AssetBundleLoadAssetOperation LoadBundle<T>()
  {
    return !this.isFile ? (AssetBundleLoadAssetOperation) null : this.request ?? (this.request = AssetBundleManager.LoadAsset(this, typeof (T)));
  }

  public override AssetBundleLoadAssetOperation LoadBundleAsync<T>()
  {
    return !this.isFile ? (AssetBundleLoadAssetOperation) null : this.request ?? (this.request = AssetBundleManager.LoadAssetAsync(this, typeof (T)));
  }

  public override AssetBundleLoadAssetOperation LoadAllBundle<T>()
  {
    return !this.isFile ? (AssetBundleLoadAssetOperation) null : this.request ?? (this.request = AssetBundleManager.LoadAllAsset(this, typeof (T)));
  }

  public override AssetBundleLoadAssetOperation LoadAllBundleAsync<T>()
  {
    return !this.isFile ? (AssetBundleLoadAssetOperation) null : this.request ?? (this.request = AssetBundleManager.LoadAllAssetAsync(this, typeof (T)));
  }

  public override void UnloadBundle(bool isUnloadForceRefCount = false, bool unloadAllLoadedObjects = false)
  {
    if (this.request != null)
      AssetBundleManager.UnloadAssetBundle(this, isUnloadForceRefCount, unloadAllLoadedObjects);
    this.request = (AssetBundleLoadAssetOperation) null;
  }
}
