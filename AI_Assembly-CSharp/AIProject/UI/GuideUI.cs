// Decompiled with JetBrains decompiler
// Type: AIProject.UI.GuideUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Scene;
using Manager;
using System;
using System.Collections.Generic;
using UniRx.Toolkit;
using UnityEngine;
using UnityEx;

namespace AIProject.UI
{
  public class GuideUI : MonoBehaviour
  {
    [SerializeField]
    private RectTransform _parent;
    [SerializeField]
    private GameObject _source;
    private List<GuideOption> _options;
    private GuideUI.GuideOptionPool _pool;
    private ValueTuple<string, ActionID>[] _elements;

    public GuideUI()
    {
      base.\u002Ector();
    }

    public ValueTuple<string, ActionID>[] Elements
    {
      get
      {
        return this._elements;
      }
      set
      {
        this._elements = value;
        foreach (GuideOption option in this._options)
          this._pool.Return(option);
        this._options.Clear();
        foreach (ValueTuple<string, ActionID> valueTuple in value)
        {
          GuideOption guideOption = this._pool.Rent();
          ((Component) guideOption).get_transform().SetParent((Transform) this._parent, false);
          guideOption.Icon = (Sprite) null;
          guideOption.CaptionText = "決定";
        }
      }
    }

    private void Start()
    {
      string bundle = Singleton<Resources>.Instance.DefinePack.ABPaths.MapScenePrefab;
      string manifest = "abdata";
      this._source = CommonLib.LoadAsset<GameObject>(bundle, "GuideOption", false, manifest);
      if (!MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == bundle && (string) x.Item2 == manifest)))
        MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>(bundle, manifest));
      this._pool.Source = this._source;
    }

    private void OnDestroy()
    {
      this._pool.Clear(false);
    }

    public class GuideOptionPool : ObjectPool<GuideOption>
    {
      public GuideOptionPool()
      {
        base.\u002Ector();
      }

      public GameObject Source { get; set; }

      protected virtual GuideOption CreateInstance()
      {
        return (GuideOption) ((GameObject) Object.Instantiate<GameObject>((M0) this.Source)).GetComponent<GuideOption>();
      }
    }
  }
}
