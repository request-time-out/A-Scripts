// Decompiled with JetBrains decompiler
// Type: AssetLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class AssetLoader : BaseLoader
{
  public bool isAsync = true;
  public bool isBundleUnload = true;
  [SerializeField]
  protected bool isLoad = true;
  public string assetBundleName;
  public string assetName;
  public bool isUnloadForceRefCount;
  public bool unloadAllLoadedObjects;
  public string manifestFileName;
  [SerializeField]
  protected bool isClone;

  public Object loadObject { get; protected set; }

  public bool isLoadEnd { get; private set; }

  public bool initialized { get; protected set; }

  public void Init()
  {
    this.StartCoroutine(this._Init());
  }

  [DebuggerHidden]
  public virtual IEnumerator _Init()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AssetLoader.\u003C_Init\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  [DebuggerHidden]
  protected virtual IEnumerator Start()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AssetLoader.\u003CStart\u003Ec__Iterator1()
    {
      \u0024this = this
    };
  }

  protected virtual void OnDestroy()
  {
    if (!this.isLoadEnd || !this.isBundleUnload || !Singleton<AssetBundleManager>.IsInstance())
      return;
    AssetBundleManager.UnloadAssetBundle(this.assetBundleName, this.isUnloadForceRefCount, this.manifestFileName, this.unloadAllLoadedObjects);
  }
}
