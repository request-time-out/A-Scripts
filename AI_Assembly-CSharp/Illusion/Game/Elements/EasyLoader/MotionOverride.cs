// Decompiled with JetBrains decompiler
// Type: Illusion.Game.Elements.EasyLoader.MotionOverride
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Illusion.Game.Elements.EasyLoader
{
  [Serializable]
  public class MotionOverride : Motion
  {
    public AssetBundleData overrideMotion = new AssetBundleData();

    public MotionOverride()
    {
    }

    public MotionOverride(string bundle, string asset)
      : base(bundle, asset)
    {
    }

    public MotionOverride(string bundle, string asset, string state)
      : base(bundle, asset, state)
    {
    }

    public override bool Setting(Animator animator)
    {
      return this.Setting(animator, this.bundle, this.asset, this.overrideMotion.bundle, this.overrideMotion.asset, this.state, false);
    }

    public bool Setting(
      Animator animator,
      string bundle,
      string asset,
      string overrideBundle,
      string overrideAsset,
      string state,
      bool isCheck)
    {
      bool flag = this.Setting(animator, bundle, asset, state, isCheck);
      if ((flag || !isCheck || this.overrideMotion.Check(overrideBundle, overrideAsset)) && !overrideAsset.IsNullOrEmpty())
      {
        this.overrideMotion.asset = overrideAsset;
        if (!overrideBundle.IsNullOrEmpty())
          this.overrideMotion.bundle = overrideBundle;
        this.UnloadBundle(false, false);
        this.overrideMotion.UnloadBundle(false, false);
        animator.set_runtimeAnimatorController((RuntimeAnimatorController) Illusion.Utils.Animator.SetupAnimatorOverrideController(animator.get_runtimeAnimatorController(), this.overrideMotion.GetAsset<RuntimeAnimatorController>()));
        flag = true;
      }
      return flag;
    }
  }
}
