// Decompiled with JetBrains decompiler
// Type: BaseLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public class BaseLoader : MonoBehaviour
{
  [SerializeField]
  protected bool isErase;
  public const string LocalPath = "file://";
  public const string NetWorkPath = "http://";

  public BaseLoader()
  {
    base.\u002Ector();
  }

  protected virtual void Awake()
  {
    Debug.Log((object) string.Format("{0}.{1}", (object) nameof (BaseLoader), (object) nameof (Awake)));
    this.Initialize();
    if (!this.isErase)
      return;
    Object.Destroy((Object) ((Component) this).get_gameObject());
  }

  protected void Initialize()
  {
    if (Singleton<AssetBundleManager>.IsInstance())
      return;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.get_dataPath());
    string str = "/../abdata" + (object) '/';
    stringBuilder.Append(str);
    AssetBundleManager.Initialize(stringBuilder.ToString());
  }

  public string GetRelativePath()
  {
    if (Application.get_isEditor())
      return "file://" + Environment.CurrentDirectory.Replace("\\", "/");
    return Application.get_isMobilePlatform() || Application.get_isConsolePlatform() ? Application.get_streamingAssetsPath() : "file://" + Application.get_streamingAssetsPath();
  }

  private static string GetPlatformFolderForAssetBundles(RuntimePlatform platform)
  {
    if (platform == 1)
      return "OSX";
    if (platform == 2)
      return "Windows";
    switch (platform - 8)
    {
      case 0:
        return "iOS";
      case 3:
        return "Android";
      default:
        return (string) null;
    }
  }

  protected T Load<T>(
    string assetBundleName,
    string assetName,
    bool isClone = false,
    string manifestAssetBundleName = null)
    where T : Object
  {
    T obj1 = new AssetBundleManifestData(assetBundleName, assetName, manifestAssetBundleName).GetAsset<T>();
    if (Object.op_Inequality((Object) (object) obj1, (Object) null) && isClone)
    {
      T obj2 = Object.Instantiate<T>(obj1);
      obj2.set_name(obj1.get_name());
      obj1 = obj2;
    }
    return obj1;
  }

  [DebuggerHidden]
  protected IEnumerator Load_Coroutine<T>(
    string assetBundleName,
    string assetName,
    Action<T> act = null,
    bool isClone = false,
    string manifestAssetBundleName = null)
    where T : Object
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new BaseLoader.\u003CLoad_Coroutine\u003Ec__Iterator0<T>()
    {
      assetName = assetName,
      assetBundleName = assetBundleName,
      manifestAssetBundleName = manifestAssetBundleName,
      isClone = isClone,
      act = act,
      \u0024this = this
    };
  }

  [Conditional("BASE_LOADER_LOG")]
  private void Log(string str)
  {
    Debug.Log((object) str);
  }
}
