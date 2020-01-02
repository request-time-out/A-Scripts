// Decompiled with JetBrains decompiler
// Type: ADV.Root
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ADV
{
  public class Root : MonoBehaviour
  {
    private Dictionary<string, Transform> _characterStandNulls;
    private int charaStartIndex;
    [SerializeField]
    private Transform _nullRoot;
    [SerializeField]
    private Transform _baseRoot;
    [SerializeField]
    private Transform _cameraRoot;
    [SerializeField]
    private Transform _charaRoot;
    [SerializeField]
    private Transform _eventCGRoot;
    [SerializeField]
    private Transform _objectRoot;

    public Root()
    {
      base.\u002Ector();
    }

    public static Root Load(Transform parent)
    {
      string assetBundleName = "adv/root.unity3d";
      AssetBundleLoadAssetOperation loadAssetOperation = AssetBundleManager.LoadAsset(assetBundleName, "ADV_Root", typeof (GameObject), (string) null);
      AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
      return (Root) ((GameObject) Object.Instantiate<GameObject>((M0) loadAssetOperation.GetAsset<GameObject>(), parent, false)).GetComponent<Root>();
    }

    public void SetBackup()
    {
      this.cameraRootBK?.Set(this._cameraRoot);
      this.charaRootBK?.Set(this._charaRoot);
    }

    public Dictionary<string, Transform> characterStandNulls
    {
      get
      {
        return ((object) this).GetCache<Dictionary<string, Transform>>(ref this._characterStandNulls, (Func<Dictionary<string, Transform>>) (() => Enumerable.Range(0, this.charaStartIndex).Select<int, Transform>((Func<int, Transform>) (i => this._charaRoot.GetChild(i))).ToDictionary<Transform, string, Transform>((Func<Transform, string>) (v => ((Object) v).get_name()), (Func<Transform, Transform>) (v => v), (IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase)));
      }
    }

    private BackupPosRot cameraRootBK { get; set; }

    private BackupPosRot charaRootBK { get; set; }

    public Transform nullRoot
    {
      get
      {
        return this._nullRoot;
      }
    }

    public Transform baseRoot
    {
      get
      {
        return this._baseRoot;
      }
    }

    public Transform cameraRoot
    {
      get
      {
        return this._cameraRoot;
      }
    }

    public Transform charaRoot
    {
      get
      {
        return this._charaRoot;
      }
    }

    public Transform eventCGRoot
    {
      get
      {
        return this._eventCGRoot;
      }
    }

    public Transform objectRoot
    {
      get
      {
        return this._objectRoot;
      }
    }

    private void Awake()
    {
      if (Object.op_Inequality((Object) this._cameraRoot, (Object) null))
        this.cameraRootBK = new BackupPosRot(this._cameraRoot);
      if (!Object.op_Inequality((Object) this._charaRoot, (Object) null))
        return;
      this.charaStartIndex = this._charaRoot.get_childCount();
      this.charaRootBK = new BackupPosRot(this._charaRoot);
    }
  }
}
