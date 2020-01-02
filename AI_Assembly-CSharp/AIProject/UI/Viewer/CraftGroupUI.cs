// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Viewer.CraftGroupUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Scene;
using Manager;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEx;

namespace AIProject.UI.Viewer
{
  public class CraftGroupUI : MonoBehaviour
  {
    [SerializeField]
    private CraftUI _craftUI;
    [SerializeField]
    private Image _cursor;
    [SerializeField]
    private Toggle _toggle;
    [SerializeField]
    private int _id;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private string _iconText;
    [SerializeField]
    private RectTransform _parent;
    private CraftViewer _viewer;

    public CraftGroupUI()
    {
      base.\u002Ector();
    }

    public Image cursor
    {
      get
      {
        return this._cursor;
      }
    }

    public Toggle toggle
    {
      get
      {
        return this._toggle;
      }
    }

    public CraftViewer viewer
    {
      get
      {
        if (Object.op_Equality((Object) this._viewer, (Object) null))
        {
          string bundle = Singleton<Resources>.Instance.DefinePack.ABPaths.MapScenePrefab;
          this._viewer = (CraftViewer) ((GameObject) Object.Instantiate<GameObject>((M0) CommonLib.LoadAsset<GameObject>(bundle, "CraftViewer", false, string.Empty), (Transform) this._parent, false)).GetComponent<CraftViewer>();
          if (Object.op_Inequality((Object) this._image, (Object) null))
            this._viewer.SetIcon(this._image.get_sprite());
          this._viewer.SetIcon(this._iconText);
          this._viewer.SetID(this._id);
          this._viewer.itemListUI.SetCraftUI(this._craftUI);
          if (!MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == bundle)))
            MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>(bundle, string.Empty));
        }
        return this._viewer;
      }
    }

    public void SetActive(bool isOn)
    {
      this.viewer.Visible = isOn;
    }
  }
}
