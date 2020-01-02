// Decompiled with JetBrains decompiler
// Type: AssetBundleLoadLevelOperation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetBundleLoadLevelOperation : AssetBundleLoadOperation
{
  protected string m_AssetBundleName;
  protected string m_LevelName;
  protected string m_ManifestAssetBundleName;
  protected bool m_IsAdditive;
  protected string m_DownloadingError;
  protected AsyncOperation m_Request;

  public AssetBundleLoadLevelOperation(
    string assetbundleName,
    string levelName,
    bool isAdditive,
    string manifestAssetBundleName)
  {
    this.m_AssetBundleName = assetbundleName;
    this.m_LevelName = levelName;
    this.m_IsAdditive = isAdditive;
    this.m_ManifestAssetBundleName = manifestAssetBundleName;
  }

  public AsyncOperation Request
  {
    get
    {
      return this.m_Request;
    }
  }

  public override bool Update()
  {
    if (this.m_Request != null)
      return false;
    if (AssetBundleManager.GetLoadedAssetBundle(this.m_AssetBundleName, out this.m_DownloadingError, this.m_ManifestAssetBundleName) == null)
      return true;
    this.m_Request = SceneManager.LoadSceneAsync(this.m_LevelName, !this.m_IsAdditive ? (LoadSceneMode) 0 : (LoadSceneMode) 1);
    return false;
  }

  public override bool IsDone()
  {
    if (this.m_Request == null && this.m_DownloadingError != null)
    {
      Debug.LogError((object) this.m_DownloadingError);
      return true;
    }
    return this.m_Request != null && this.m_Request.get_isDone();
  }
}
