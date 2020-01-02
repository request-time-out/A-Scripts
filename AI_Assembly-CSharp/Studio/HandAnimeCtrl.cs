// Decompiled with JetBrains decompiler
// Type: Studio.HandAnimeCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;

namespace Studio
{
  public class HandAnimeCtrl : MonoBehaviour
  {
    [SerializeField]
    private HandAnimeCtrl.HandKind hand;
    [SerializeField]
    private Animator animator;
    private IntReactiveProperty _ptn;
    private string bundlePath;
    private string fileName;

    public HandAnimeCtrl()
    {
      base.\u002Ector();
    }

    public int sex { get; private set; }

    public int ptn
    {
      get
      {
        return ((ReactiveProperty<int>) this._ptn).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._ptn).set_Value(value);
      }
    }

    public int max { get; private set; }

    public bool isInit { get; private set; }

    public void Init(int _sex)
    {
      if (this.isInit)
        return;
      this.max = Singleton<Info>.Instance.dicHandAnime[(int) this.hand].Count;
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this._ptn, (Action<M0>) (_ => this.LoadAnime()));
      this.sex = _sex;
      this.ptn = 0;
      this.isInit = true;
    }

    private void LoadAnime()
    {
      Info.HandAnimeInfo handAnimeInfo = (Info.HandAnimeInfo) null;
      if (!Singleton<Info>.Instance.dicHandAnime[(int) this.hand].TryGetValue(this.ptn, out handAnimeInfo))
      {
        ((Behaviour) this).set_enabled(false);
      }
      else
      {
        if (this.bundlePath != handAnimeInfo.bundlePath || this.fileName != handAnimeInfo.fileName)
        {
          RuntimeAnimatorController animatorController = CommonLib.LoadAsset<RuntimeAnimatorController>(handAnimeInfo.bundlePath, handAnimeInfo.fileName, false, string.Empty);
          if (Object.op_Inequality((Object) animatorController, (Object) null))
          {
            this.bundlePath = handAnimeInfo.bundlePath;
            this.fileName = handAnimeInfo.fileName;
            this.animator.set_runtimeAnimatorController(animatorController);
          }
          AssetBundleManager.UnloadAssetBundle(handAnimeInfo.bundlePath, false, (string) null, false);
        }
        ((Behaviour) this).set_enabled(true);
        this.animator.Play(handAnimeInfo.clip);
      }
    }

    private void OnEnable()
    {
      ((Behaviour) this.animator).set_enabled(true);
    }

    private void OnDisable()
    {
      ((Behaviour) this.animator).set_enabled(false);
    }

    public enum HandKind
    {
      Left,
      Right,
    }
  }
}
