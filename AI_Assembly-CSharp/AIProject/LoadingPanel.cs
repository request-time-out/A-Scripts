// Decompiled with JetBrains decompiler
// Type: AIProject.LoadingPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEx;

namespace AIProject
{
  public class LoadingPanel : MonoBehaviour
  {
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private ProgressWave _waveAnim;
    [SerializeField]
    private Image[] _tipsImage;
    private List<CanvasGroup> _tipsImageCGList;
    [SerializeField]
    private float _duration;
    [SerializeField]
    private float _interval;
    private IDisposable _disposable;
    private int _prevID;

    public LoadingPanel()
    {
      base.\u002Ector();
    }

    public CanvasGroup CanvasGroup
    {
      get
      {
        return this._canvasGroup;
      }
    }

    private void Awake()
    {
      foreach (Component component1 in this._tipsImage)
      {
        CanvasGroup component2 = (CanvasGroup) component1.GetComponent<CanvasGroup>();
        if (Object.op_Inequality((Object) component2, (Object) null))
          this._tipsImageCGList.Add(component2);
      }
    }

    public void Play()
    {
      if (Object.op_Inequality((Object) this._waveAnim, (Object) null))
        this._waveAnim.PlayAnim(true);
      this._disposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.SlideShowCoroutine(true)), false));
    }

    public void Stop()
    {
      if (Object.op_Inequality((Object) this._waveAnim, (Object) null))
        this._waveAnim.Stop();
      if (this._disposable == null)
        return;
      this._disposable.Dispose();
    }

    [DebuggerHidden]
    private IEnumerator SlideShowCoroutine(bool isLoop)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new LoadingPanel.\u003CSlideShowCoroutine\u003Ec__Iterator0()
      {
        isLoop = isLoop,
        \u0024this = this
      };
    }

    private void SetCanvasGroupAlpha(int index, float alpha)
    {
      this._tipsImageCGList[index].set_alpha(alpha);
    }

    private int GetUseSPID()
    {
      List<int> toRelease = ListPool<int>.Get();
      using (Dictionary<int, AssetBundleInfo>.Enumerator enumerator = Singleton<Game>.Instance.LoadingSpriteABList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, AssetBundleInfo> current = enumerator.Current;
          toRelease.Add(current.Key);
        }
      }
      int num1 = -1;
      while (toRelease.Count > 0)
      {
        int num2 = toRelease[Random.Range(0, toRelease.Count)];
        if (this._prevID != num2)
        {
          num1 = num2;
          this._prevID = num2;
          break;
        }
        toRelease.Remove(num2);
      }
      ListPool<int>.Release(toRelease);
      return num1;
    }

    public static Sprite LoadSpriteAsset(
      string assetBundleName,
      string assetName,
      string manifestName)
    {
      manifestName = !manifestName.IsNullOrEmpty() ? manifestName : (string) null;
      if (AssetBundleCheck.IsSimulation)
        manifestName = string.Empty;
      if (!AssetBundleCheck.IsFile(assetBundleName, assetName))
      {
        Debug.LogWarning((object) string.Format("読み込みエラー\r\nassetBundleName：{0}\tassetName：{1}", (object) assetBundleName, (object) assetName));
        return (Sprite) null;
      }
      AssetBundleLoadAssetOperation loadAssetOperation = AssetBundleManager.LoadAsset(assetBundleName, assetName, typeof (Sprite), !manifestName.IsNullOrEmpty() ? manifestName : (string) null);
      Sprite asset1 = loadAssetOperation.GetAsset<Sprite>();
      if (Object.op_Equality((Object) asset1, (Object) null))
      {
        Texture2D asset2 = loadAssetOperation.GetAsset<Texture2D>();
        if (Object.op_Equality((Object) asset2, (Object) null))
          return (Sprite) null;
        asset1 = Sprite.Create(asset2, new Rect(0.0f, 0.0f, (float) ((Texture) asset2).get_width(), (float) ((Texture) asset2).get_height()), Vector2.get_zero());
      }
      return asset1;
    }
  }
}
