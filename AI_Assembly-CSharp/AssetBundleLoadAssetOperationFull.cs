// Decompiled with JetBrains decompiler
// Type: AssetBundleLoadAssetOperationFull
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class AssetBundleLoadAssetOperationFull : AssetBundleLoadAssetOperation
{
  protected string m_AssetBundleName;
  protected string m_AssetName;
  protected string m_ManifestAssetBundleName;
  protected System.Type m_Type;
  protected string m_DownloadingError;
  protected AssetBundleRequest m_Request;

  public AssetBundleLoadAssetOperationFull(
    string bundleName,
    string assetName,
    System.Type type,
    string manifestAssetBundleName)
  {
    this.m_AssetBundleName = bundleName;
    this.m_AssetName = assetName;
    this.m_Type = type;
    this.m_ManifestAssetBundleName = manifestAssetBundleName;
  }

  public override bool IsEmpty()
  {
    return this.m_Request == null || !((AsyncOperation) this.m_Request).get_isDone() || Object.op_Equality(this.m_Request.get_asset(), (Object) null);
  }

  public override T GetAsset<T>()
  {
    return this.m_Request != null && ((AsyncOperation) this.m_Request).get_isDone() ? this.m_Request.get_asset() as T : (T) null;
  }

  public override T[] GetAllAssets<T>()
  {
    if (this.m_Request == null || !((AsyncOperation) this.m_Request).get_isDone())
      return (T[]) null;
    T[] objArray = new T[this.m_Request.get_allAssets().Length];
    for (int index = 0; index < objArray.Length; ++index)
      objArray[index] = this.m_Request.get_allAssets()[index] as T;
    return objArray;
  }

  public override bool Update()
  {
    if (this.m_Request != null)
      return false;
    LoadedAssetBundle loadedAssetBundle = AssetBundleManager.GetLoadedAssetBundle(this.m_AssetBundleName, out this.m_DownloadingError, this.m_ManifestAssetBundleName);
    if (loadedAssetBundle == null)
      return true;
    if (Object.op_Implicit((Object) loadedAssetBundle.m_AssetBundle))
      this.m_Request = !this.m_AssetName.IsNullOrEmpty() ? loadedAssetBundle.m_AssetBundle.LoadAssetAsync(this.m_AssetName, this.m_Type) : loadedAssetBundle.m_AssetBundle.LoadAllAssetsAsync(this.m_Type);
    return false;
  }

  public override bool IsDone()
  {
    if (this.m_Request == null && this.m_DownloadingError != null)
    {
      Debug.LogError((object) this.m_DownloadingError);
      return true;
    }
    return this.m_Request != null && ((AsyncOperation) this.m_Request).get_isDone();
  }
}
