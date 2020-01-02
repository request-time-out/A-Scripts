// Decompiled with JetBrains decompiler
// Type: AssetBundleManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetBundleManager : Singleton<AssetBundleManager>
{
  private static HashSet<string> m_AllLoadedAssetBundleNames = new HashSet<string>();
  private static AssetBundleManager.BundlePack MainBundle = (AssetBundleManager.BundlePack) null;
  private static Dictionary<string, AssetBundleManager.BundlePack> m_ManifestBundlePack = new Dictionary<string, AssetBundleManager.BundlePack>();
  private static string m_BaseDownloadingURL = string.Empty;
  private static bool isInitialized = false;
  private List<string> keysToRemove = new List<string>();
  public const string MAIN_MANIFEST_NAME = "abdata";
  public const string Extension = ".unity3d";

  public static string BaseDownloadingURL
  {
    get
    {
      return AssetBundleManager.m_BaseDownloadingURL;
    }
  }

  public static string[] Variants
  {
    get
    {
      return AssetBundleManager.MainBundle.Variants;
    }
    set
    {
      AssetBundleManager.MainBundle.Variants = value;
    }
  }

  public static AssetBundleManager.BundlePack ManifestAdd(
    string manifestAssetBundleName)
  {
    if (AssetBundleManager.m_ManifestBundlePack.ContainsKey(manifestAssetBundleName))
      return (AssetBundleManager.BundlePack) null;
    AssetBundleManager.BundlePack bundlePack = new AssetBundleManager.BundlePack();
    AssetBundleManager.m_ManifestBundlePack.Add(manifestAssetBundleName, bundlePack);
    LoadedAssetBundle loadedAssetBundle = AssetBundleManager.LoadAssetBundle(manifestAssetBundleName, false, manifestAssetBundleName);
    if (loadedAssetBundle == null)
    {
      AssetBundleManager.m_ManifestBundlePack.Remove(manifestAssetBundleName);
      return (AssetBundleManager.BundlePack) null;
    }
    AssetBundleLoadAssetOperationSimulation operationSimulation = new AssetBundleLoadAssetOperationSimulation(loadedAssetBundle.m_AssetBundle.LoadAsset("AssetBundleManifest", typeof (AssetBundleManifest)));
    if (operationSimulation.IsEmpty())
    {
      AssetBundleManager.m_ManifestBundlePack.Remove(manifestAssetBundleName);
      return (AssetBundleManager.BundlePack) null;
    }
    bundlePack.AssetBundleManifest = operationSimulation.GetAsset<AssetBundleManifest>();
    return bundlePack;
  }

  public static HashSet<string> AllLoadedAssetBundleNames
  {
    get
    {
      return AssetBundleManager.m_AllLoadedAssetBundleNames;
    }
  }

  public static Dictionary<string, AssetBundleManager.BundlePack> ManifestBundlePack
  {
    get
    {
      return AssetBundleManager.m_ManifestBundlePack;
    }
  }

  public static float Progress
  {
    get
    {
      int count = AssetBundleManager.MainBundle.LoadedAssetBundles.Count;
      float num = (float) count;
      foreach (AssetBundleCreate assetBundleCreate in AssetBundleManager.MainBundle.CreateAssetBundles.Values)
      {
        ++count;
        num += ((AsyncOperation) assetBundleCreate.m_CreateRequest).get_progress();
      }
      return count == 0 ? 1f : num / (float) count;
    }
  }

  public static LoadedAssetBundle GetLoadedAssetBundle(
    string assetBundleName,
    out string error,
    string manifestAssetBundleName = null)
  {
    assetBundleName = assetBundleName ?? string.Empty;
    if (manifestAssetBundleName.IsNullOrEmpty())
      manifestAssetBundleName = "abdata";
    AssetBundleManager.BundlePack bundlePack = AssetBundleManager.m_ManifestBundlePack[manifestAssetBundleName];
    if (bundlePack.DownloadingErrors.TryGetValue(assetBundleName, out error))
      return (LoadedAssetBundle) null;
    LoadedAssetBundle loadedAssetBundle1 = (LoadedAssetBundle) null;
    bundlePack.LoadedAssetBundles.TryGetValue(assetBundleName, out loadedAssetBundle1);
    if (loadedAssetBundle1 == null)
      return (LoadedAssetBundle) null;
    LoadedAssetBundleDependencies bundleDependencies = bundlePack.Dependencies.Find((Predicate<LoadedAssetBundleDependencies>) (p => p.m_Key == assetBundleName));
    if (bundleDependencies == null)
      return loadedAssetBundle1;
    foreach (string bundleName in bundleDependencies.m_BundleNames)
    {
      if (bundlePack.DownloadingErrors.TryGetValue(assetBundleName, out error))
        return loadedAssetBundle1;
      LoadedAssetBundle loadedAssetBundle2;
      bundlePack.LoadedAssetBundles.TryGetValue(bundleName, out loadedAssetBundle2);
      if (loadedAssetBundle2 == null)
        return (LoadedAssetBundle) null;
    }
    return loadedAssetBundle1;
  }

  public static void Initialize(string basePath)
  {
    if (AssetBundleManager.isInitialized)
      return;
    AssetBundleManager.m_BaseDownloadingURL = basePath;
    GameObject go = new GameObject(nameof (AssetBundleManager), new System.Type[1]
    {
      typeof (AssetBundleManager)
    });
    Object.DontDestroyOnLoad((Object) go);
    if (AssetBundleManager.MainBundle == null)
      AssetBundleManager.MainBundle = AssetBundleManager.ManifestAdd("abdata");
    if (Directory.Exists(basePath))
    {
      IEnumerable<string> source = ((IEnumerable<string>) Directory.GetFiles(basePath, "*.*", SearchOption.TopDirectoryOnly)).Where<string>((Func<string, bool>) (s => Path.GetExtension(s).IsNullOrEmpty()));
      // ISSUE: reference to a compiler-generated field
      if (AssetBundleManager.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AssetBundleManager.\u003C\u003Ef__mg\u0024cache0 = new Func<string, string>(Path.GetFileNameWithoutExtension);
      }
      // ISSUE: reference to a compiler-generated field
      Func<string, string> fMgCache0 = AssetBundleManager.\u003C\u003Ef__mg\u0024cache0;
      foreach (string manifestAssetBundleName in source.Select<string, string>(fMgCache0).Where<string>((Func<string, bool>) (s => s != "abdata")))
        AssetBundleManager.ManifestAdd(manifestAssetBundleName);
    }
    AssetBundleManager.isInitialized = true;
    InitAddComponent.AddComponents(go);
  }

  public static LoadedAssetBundle LoadAssetBundle(
    string assetBundleName,
    bool isAsync,
    string manifestAssetBundleName = null)
  {
    bool flag = assetBundleName == manifestAssetBundleName;
    if (!flag)
      assetBundleName = AssetBundleManager.RemapVariantName(assetBundleName, manifestAssetBundleName);
    if (!AssetBundleManager.LoadAssetBundleInternal(assetBundleName, isAsync, manifestAssetBundleName) && !flag)
      AssetBundleManager.LoadDependencies(assetBundleName, isAsync, manifestAssetBundleName);
    return AssetBundleManager.GetLoadedAssetBundle(assetBundleName, out string _, manifestAssetBundleName);
  }

  protected static string RemapVariantName(string assetBundleName, string manifestAssetBundleName = null)
  {
    if (manifestAssetBundleName.IsNullOrEmpty())
      manifestAssetBundleName = "abdata";
    AssetBundleManager.BundlePack bundlePack = AssetBundleManager.m_ManifestBundlePack[manifestAssetBundleName];
    string[] bundlesWithVariant = bundlePack.AssetBundleManifest.GetAllAssetBundlesWithVariant();
    if (Array.IndexOf<string>(bundlesWithVariant, assetBundleName) < 0)
      return assetBundleName;
    string[] strArray1 = assetBundleName.Split('.');
    int num1 = int.MaxValue;
    int index1 = -1;
    for (int index2 = 0; index2 < bundlesWithVariant.Length; ++index2)
    {
      string[] strArray2 = bundlesWithVariant[index2].Split('.');
      if (!(strArray2[0] != strArray1[0]))
      {
        int num2 = Array.IndexOf<string>(bundlePack.Variants, strArray2[1]);
        if (num2 != -1 && num2 < num1)
        {
          num1 = num2;
          index1 = index2;
        }
      }
    }
    return index1 != -1 ? bundlesWithVariant[index1] : assetBundleName;
  }

  public static bool LoadAssetBundleInternal(
    string assetBundleName,
    bool isAsync,
    string manifestAssetBundleName = null)
  {
    if (manifestAssetBundleName.IsNullOrEmpty())
      manifestAssetBundleName = "abdata";
    AssetBundleManager.BundlePack bundlePack = AssetBundleManager.m_ManifestBundlePack[manifestAssetBundleName];
    LoadedAssetBundle loadedAssetBundle = (LoadedAssetBundle) null;
    bundlePack.LoadedAssetBundles.TryGetValue(assetBundleName, out loadedAssetBundle);
    if (loadedAssetBundle != null)
    {
      ++loadedAssetBundle.m_ReferencedCount;
      return true;
    }
    AssetBundleCreate assetBundleCreate = (AssetBundleCreate) null;
    bundlePack.CreateAssetBundles.TryGetValue(assetBundleName, out assetBundleCreate);
    if (assetBundleCreate != null)
    {
      ++assetBundleCreate.m_ReferencedCount;
      return true;
    }
    if (!AssetBundleManager.m_AllLoadedAssetBundleNames.Add(assetBundleName))
    {
      Debug.LogErrorFormat("Loaded AssetBundle : {0}\nManifestName : {1}, isAsync : {2}", new object[3]
      {
        (object) assetBundleName,
        (object) manifestAssetBundleName,
        (object) isAsync
      });
      return true;
    }
    string str = AssetBundleManager.BaseDownloadingURL + assetBundleName;
    if (!isAsync)
      bundlePack.LoadedAssetBundles.Add(assetBundleName, new LoadedAssetBundle(AssetBundle.LoadFromFile(str)));
    else
      bundlePack.CreateAssetBundles.Add(assetBundleName, new AssetBundleCreate(AssetBundle.LoadFromFileAsync(str)));
    return false;
  }

  protected static void LoadDependencies(
    string assetBundleName,
    bool isAsync,
    string manifestAssetBundleName = null)
  {
    if (manifestAssetBundleName.IsNullOrEmpty())
      manifestAssetBundleName = "abdata";
    AssetBundleManager.BundlePack bundlePack = AssetBundleManager.m_ManifestBundlePack[manifestAssetBundleName];
    if (object.ReferenceEquals((object) bundlePack.AssetBundleManifest, (object) null))
    {
      Debug.LogError((object) "Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
    }
    else
    {
      string[] allDependencies = bundlePack.AssetBundleManifest.GetAllDependencies(assetBundleName);
      if (allDependencies.Length == 0)
        return;
      for (int index = 0; index < allDependencies.Length; ++index)
        allDependencies[index] = AssetBundleManager.RemapVariantName(allDependencies[index], manifestAssetBundleName);
      LoadedAssetBundleDependencies bundleDependencies = bundlePack.Dependencies.Find((Predicate<LoadedAssetBundleDependencies>) (p => p.m_Key == assetBundleName));
      if (bundleDependencies != null)
        ++bundleDependencies.m_ReferencedCount;
      else
        bundlePack.Dependencies.Add(new LoadedAssetBundleDependencies(assetBundleName, allDependencies));
      for (int index = 0; index < allDependencies.Length; ++index)
        AssetBundleManager.LoadAssetBundleInternal(allDependencies[index], isAsync, manifestAssetBundleName);
    }
  }

  public static void UnloadAssetBundle(
    AssetBundleData data,
    bool isUnloadForceRefCount,
    bool unloadAllLoadedObjects = false)
  {
    AssetBundleManager.UnloadAssetBundle(data.bundle, isUnloadForceRefCount, (string) null, unloadAllLoadedObjects);
  }

  public static void UnloadAssetBundle(
    AssetBundleManifestData data,
    bool isUnloadForceRefCount,
    bool unloadAllLoadedObjects = false)
  {
    AssetBundleManager.UnloadAssetBundle(data.bundle, isUnloadForceRefCount, data.manifest, unloadAllLoadedObjects);
  }

  public static void UnloadAssetBundle(
    string assetBundleName,
    bool isUnloadForceRefCount,
    string manifestAssetBundleName = null,
    bool unloadAllLoadedObjects = false)
  {
    if (manifestAssetBundleName.IsNullOrEmpty())
      manifestAssetBundleName = "abdata";
    do
      ;
    while (AssetBundleManager.UnloadBundleAndDependencies(assetBundleName, manifestAssetBundleName, unloadAllLoadedObjects) && isUnloadForceRefCount);
  }

  private static bool UnloadBundleAndDependencies(
    string assetBundleName,
    string manifestAssetBundleName,
    bool unloadAllLoadedObjects)
  {
    AssetBundleManager.BundlePack targetPack;
    if (true)
      targetPack = AssetBundleManager.m_ManifestBundlePack[manifestAssetBundleName];
    else if (!AssetBundleManager.m_ManifestBundlePack.TryGetValue(manifestAssetBundleName, out targetPack))
      targetPack = AssetBundleManager.m_ManifestBundlePack["abdata"];
    bool flag = AssetBundleManager.UnloadBundle(assetBundleName, targetPack, unloadAllLoadedObjects);
    if (flag)
    {
      LoadedAssetBundleDependencies bundleDependencies = targetPack.Dependencies.Find((Predicate<LoadedAssetBundleDependencies>) (p => p.m_Key == assetBundleName));
      if (bundleDependencies != null && --bundleDependencies.m_ReferencedCount == 0)
      {
        foreach (string bundleName in bundleDependencies.m_BundleNames)
          AssetBundleManager.UnloadBundle(bundleName, targetPack, unloadAllLoadedObjects);
        targetPack.Dependencies.Remove(bundleDependencies);
      }
    }
    return flag;
  }

  private static bool UnloadBundle(
    string assetBundleName,
    AssetBundleManager.BundlePack targetPack,
    bool unloadAllLoadedObjects)
  {
    assetBundleName = assetBundleName ?? string.Empty;
    if (targetPack.DownloadingErrors.TryGetValue(assetBundleName, out string _))
      return false;
    LoadedAssetBundle loadedAssetBundle = (LoadedAssetBundle) null;
    if (!targetPack.LoadedAssetBundles.TryGetValue(assetBundleName, out loadedAssetBundle))
      return false;
    if (--loadedAssetBundle.m_ReferencedCount == 0U)
    {
      if (Object.op_Implicit((Object) loadedAssetBundle.m_AssetBundle))
        loadedAssetBundle.m_AssetBundle.Unload(unloadAllLoadedObjects);
      targetPack.LoadedAssetBundles.Remove(assetBundleName);
      AssetBundleManager.m_AllLoadedAssetBundleNames.Remove(assetBundleName);
    }
    return true;
  }

  private void Update()
  {
    foreach (KeyValuePair<string, AssetBundleManager.BundlePack> keyValuePair in AssetBundleManager.m_ManifestBundlePack)
    {
      AssetBundleManager.BundlePack bundlePack = keyValuePair.Value;
      foreach (KeyValuePair<string, AssetBundleCreate> createAssetBundle in bundlePack.CreateAssetBundles)
      {
        AssetBundleCreateRequest createRequest = createAssetBundle.Value.m_CreateRequest;
        if (((AsyncOperation) createRequest).get_isDone())
        {
          bundlePack.LoadedAssetBundles.Add(createAssetBundle.Key, new LoadedAssetBundle(createRequest.get_assetBundle())
          {
            m_ReferencedCount = createAssetBundle.Value.m_ReferencedCount
          });
          this.keysToRemove.Add(createAssetBundle.Key);
        }
      }
      foreach (string key in this.keysToRemove)
        bundlePack.CreateAssetBundles.Remove(key);
      int index = 0;
      while (index < bundlePack.InProgressOperations.Count)
      {
        if (!bundlePack.InProgressOperations[index].Update())
          bundlePack.InProgressOperations.RemoveAt(index);
        else
          ++index;
      }
      this.keysToRemove.Clear();
    }
  }

  public static AssetBundleLoadAssetOperation LoadAsset(
    AssetBundleData data,
    System.Type type)
  {
    return AssetBundleManager.LoadAsset(data.bundle, data.asset, type, (string) null);
  }

  public static AssetBundleLoadAssetOperation LoadAsset(
    AssetBundleManifestData data,
    System.Type type)
  {
    return AssetBundleManager.LoadAsset(data.bundle, data.asset, type, data.manifest);
  }

  public static AssetBundleLoadAssetOperation LoadAsset(
    string assetBundleName,
    string assetName,
    System.Type type,
    string manifestAssetBundleName = null)
  {
    if (manifestAssetBundleName.IsNullOrEmpty())
      manifestAssetBundleName = "abdata";
    AssetBundleManager.BundlePack bundlePack = AssetBundleManager.m_ManifestBundlePack[manifestAssetBundleName];
    AssetBundleLoadAssetOperation loadAssetOperation = (AssetBundleLoadAssetOperation) null;
    if (loadAssetOperation == null)
    {
      AssetBundleManager.LoadAssetBundle(assetBundleName, false, manifestAssetBundleName);
      loadAssetOperation = (AssetBundleLoadAssetOperation) new AssetBundleLoadAssetOperationSimulation(bundlePack.LoadedAssetBundles[assetBundleName].m_AssetBundle.LoadAsset(assetName, type));
    }
    return loadAssetOperation;
  }

  public static AssetBundleLoadAssetOperation LoadAssetAsync(
    AssetBundleData data,
    System.Type type)
  {
    return AssetBundleManager.LoadAssetAsync(data.bundle, data.asset, type, (string) null);
  }

  public static AssetBundleLoadAssetOperation LoadAssetAsync(
    AssetBundleManifestData data,
    System.Type type)
  {
    return AssetBundleManager.LoadAssetAsync(data.bundle, data.asset, type, data.manifest);
  }

  public static AssetBundleLoadAssetOperation LoadAssetAsync(
    string assetBundleName,
    string assetName,
    System.Type type,
    string manifestAssetBundleName = null)
  {
    if (manifestAssetBundleName.IsNullOrEmpty())
      manifestAssetBundleName = "abdata";
    AssetBundleManager.BundlePack bundlePack = AssetBundleManager.m_ManifestBundlePack[manifestAssetBundleName];
    AssetBundleLoadAssetOperation loadAssetOperation = (AssetBundleLoadAssetOperation) null;
    if (loadAssetOperation == null)
    {
      AssetBundleManager.LoadAssetBundle(assetBundleName, true, manifestAssetBundleName);
      loadAssetOperation = (AssetBundleLoadAssetOperation) new AssetBundleLoadAssetOperationFull(assetBundleName, assetName, type, manifestAssetBundleName);
      bundlePack.InProgressOperations.Add((AssetBundleLoadOperation) loadAssetOperation);
    }
    return loadAssetOperation;
  }

  public static AssetBundleLoadAssetOperation LoadAllAsset(
    AssetBundleData data,
    System.Type type)
  {
    return AssetBundleManager.LoadAllAsset(data.bundle, type, (string) null);
  }

  public static AssetBundleLoadAssetOperation LoadAllAsset(
    AssetBundleManifestData data,
    System.Type type)
  {
    return AssetBundleManager.LoadAllAsset(data.bundle, type, data.manifest);
  }

  public static AssetBundleLoadAssetOperation LoadAllAsset(
    string assetBundleName,
    System.Type type,
    string manifestAssetBundleName = null)
  {
    if (manifestAssetBundleName.IsNullOrEmpty())
      manifestAssetBundleName = "abdata";
    AssetBundleManager.BundlePack bundlePack = AssetBundleManager.m_ManifestBundlePack[manifestAssetBundleName];
    AssetBundleLoadAssetOperation loadAssetOperation = (AssetBundleLoadAssetOperation) null;
    if (loadAssetOperation == null)
    {
      AssetBundleManager.LoadAssetBundle(assetBundleName, false, manifestAssetBundleName);
      loadAssetOperation = (AssetBundleLoadAssetOperation) new AssetBundleLoadAssetOperationSimulation(bundlePack.LoadedAssetBundles[assetBundleName].m_AssetBundle.LoadAllAssets(type));
    }
    return loadAssetOperation;
  }

  public static AssetBundleLoadAssetOperation LoadAllAssetAsync(
    AssetBundleData data,
    System.Type type)
  {
    return AssetBundleManager.LoadAllAssetAsync(data.bundle, type, (string) null);
  }

  public static AssetBundleLoadAssetOperation LoadAllAssetAsync(
    AssetBundleManifestData data,
    System.Type type)
  {
    return AssetBundleManager.LoadAllAssetAsync(data.bundle, type, data.manifest);
  }

  public static AssetBundleLoadAssetOperation LoadAllAssetAsync(
    string assetBundleName,
    System.Type type,
    string manifestAssetBundleName = null)
  {
    if (manifestAssetBundleName.IsNullOrEmpty())
      manifestAssetBundleName = "abdata";
    AssetBundleManager.BundlePack bundlePack = AssetBundleManager.m_ManifestBundlePack[manifestAssetBundleName];
    AssetBundleLoadAssetOperation loadAssetOperation = (AssetBundleLoadAssetOperation) null;
    if (loadAssetOperation == null)
    {
      AssetBundleManager.LoadAssetBundle(assetBundleName, true, manifestAssetBundleName);
      loadAssetOperation = (AssetBundleLoadAssetOperation) new AssetBundleLoadAssetOperationFull(assetBundleName, (string) null, type, manifestAssetBundleName);
      bundlePack.InProgressOperations.Add((AssetBundleLoadOperation) loadAssetOperation);
    }
    return loadAssetOperation;
  }

  public static AssetBundleLoadOperation LoadLevel(
    AssetBundleData data,
    bool isAdditive)
  {
    return AssetBundleManager.LoadLevel(data.bundle, data.asset, isAdditive, (string) null);
  }

  public static AssetBundleLoadOperation LoadLevel(
    AssetBundleManifestData data,
    bool isAdditive)
  {
    return AssetBundleManager.LoadLevel(data.bundle, data.asset, isAdditive, data.manifest);
  }

  public static AssetBundleLoadOperation LoadLevel(
    string assetBundleName,
    string levelName,
    bool isAdditive,
    string manifestAssetBundleName = null)
  {
    if (manifestAssetBundleName.IsNullOrEmpty())
      manifestAssetBundleName = "abdata";
    AssetBundleLoadOperation bundleLoadOperation = (AssetBundleLoadOperation) null;
    if (bundleLoadOperation == null)
    {
      AssetBundleManager.LoadAssetBundle(assetBundleName, false, manifestAssetBundleName);
      SceneManager.LoadScene(levelName, !isAdditive ? (LoadSceneMode) 0 : (LoadSceneMode) 1);
      bundleLoadOperation = (AssetBundleLoadOperation) new AssetBundleLoadLevelSimulationOperation();
    }
    return bundleLoadOperation;
  }

  public static AssetBundleLoadOperation LoadLevelAsync(
    AssetBundleData data,
    bool isAdditive)
  {
    return AssetBundleManager.LoadLevelAsync(data.bundle, data.asset, isAdditive, (string) null);
  }

  public static AssetBundleLoadOperation LoadLevelAsync(
    AssetBundleManifestData data,
    bool isAdditive)
  {
    return AssetBundleManager.LoadLevelAsync(data.bundle, data.asset, isAdditive, data.manifest);
  }

  public static AssetBundleLoadOperation LoadLevelAsync(
    string assetBundleName,
    string levelName,
    bool isAdditive,
    string manifestAssetBundleName = null)
  {
    if (manifestAssetBundleName.IsNullOrEmpty())
      manifestAssetBundleName = "abdata";
    AssetBundleManager.BundlePack bundlePack = AssetBundleManager.m_ManifestBundlePack[manifestAssetBundleName];
    AssetBundleLoadOperation bundleLoadOperation = (AssetBundleLoadOperation) null;
    if (bundleLoadOperation == null)
    {
      AssetBundleManager.LoadAssetBundle(assetBundleName, true, manifestAssetBundleName);
      bundleLoadOperation = (AssetBundleLoadOperation) new AssetBundleLoadLevelOperation(assetBundleName, levelName, isAdditive, manifestAssetBundleName);
      bundlePack.InProgressOperations.Add(bundleLoadOperation);
    }
    return bundleLoadOperation;
  }

  public class BundlePack
  {
    private string[] m_Variants = (string[]) Array.Empty<string>();
    private Dictionary<string, LoadedAssetBundle> m_LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();
    private Dictionary<string, AssetBundleCreate> m_CreateAssetBundles = new Dictionary<string, AssetBundleCreate>();
    private Dictionary<string, string> m_DownloadingErrors = new Dictionary<string, string>();
    private List<AssetBundleLoadOperation> m_InProgressOperations = new List<AssetBundleLoadOperation>();
    private List<LoadedAssetBundleDependencies> m_Dependencies = new List<LoadedAssetBundleDependencies>();
    private AssetBundleManifest m_AssetBundleManifest;

    public string[] Variants
    {
      get
      {
        return this.m_Variants;
      }
      set
      {
        this.m_Variants = value;
      }
    }

    public AssetBundleManifest AssetBundleManifest
    {
      get
      {
        return this.m_AssetBundleManifest;
      }
      set
      {
        this.m_AssetBundleManifest = value;
      }
    }

    public Dictionary<string, LoadedAssetBundle> LoadedAssetBundles
    {
      get
      {
        return this.m_LoadedAssetBundles;
      }
      set
      {
        this.m_LoadedAssetBundles = value;
      }
    }

    public Dictionary<string, AssetBundleCreate> CreateAssetBundles
    {
      get
      {
        return this.m_CreateAssetBundles;
      }
      set
      {
        this.m_CreateAssetBundles = value;
      }
    }

    public Dictionary<string, string> DownloadingErrors
    {
      get
      {
        return this.m_DownloadingErrors;
      }
      set
      {
        this.m_DownloadingErrors = value;
      }
    }

    public List<AssetBundleLoadOperation> InProgressOperations
    {
      get
      {
        return this.m_InProgressOperations;
      }
      set
      {
        this.m_InProgressOperations = value;
      }
    }

    public List<LoadedAssetBundleDependencies> Dependencies
    {
      get
      {
        return this.m_Dependencies;
      }
      set
      {
        this.m_Dependencies = value;
      }
    }
  }
}
