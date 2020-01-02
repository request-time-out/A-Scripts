// Decompiled with JetBrains decompiler
// Type: Illusion.Game.Elements.EasyLoader.Motion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Illusion.Game.Elements.EasyLoader
{
  [Serializable]
  public class Motion : BaseMotion
  {
    public bool isCrossFade;
    public float transitionDuration;
    public float normalizedTime;
    public int[] layers;

    public Motion()
    {
    }

    public Motion(string bundle, string asset)
      : base(bundle, asset)
    {
    }

    public Motion(string bundle, string asset, string state)
      : base(bundle, asset, state)
    {
    }

    private static int[] defaultLayers { get; } = new int[1];

    public virtual bool Setting(Animator animator)
    {
      return this.Setting(animator, this.bundle, this.asset, this.state, false);
    }

    public virtual bool Setting(
      Animator animator,
      string bundle,
      string asset,
      string state,
      bool isCheck)
    {
      if (isCheck && !this.Check(bundle, asset, state))
        return false;
      bool flag = false;
      if (!asset.IsNullOrEmpty())
      {
        this.asset = asset;
        if (!bundle.IsNullOrEmpty())
          this.bundle = bundle;
        this.LoadAnimator(animator);
        flag = true;
      }
      if (!state.IsNullOrEmpty())
      {
        this.state = state;
        flag = true;
      }
      return flag;
    }

    public void LoadAnimator(Animator animator, string bundle, string asset)
    {
      new Motion(bundle, asset).LoadAnimator(animator);
    }

    public void LoadAnimator(Animator animator)
    {
      if (((AssetBundleData) this).isEmpty)
        return;
      animator.set_runtimeAnimatorController(this.GetAsset<RuntimeAnimatorController>());
      this.UnloadBundle(false, false);
    }

    public void Play(Animator animator)
    {
      if (!((Component) animator).get_gameObject().get_activeInHierarchy())
        return;
      int[] numArray = !((IList<int>) this.layers).IsNullOrEmpty<int>() ? this.layers : Motion.defaultLayers;
      if (!this.isCrossFade)
      {
        foreach (int num in numArray)
          animator.Play(this.state, num, this.normalizedTime);
      }
      else
      {
        foreach (int num in numArray)
          animator.CrossFade(this.state, this.transitionDuration, num, this.normalizedTime);
      }
    }
  }
}
