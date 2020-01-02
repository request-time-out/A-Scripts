// Decompiled with JetBrains decompiler
// Type: Illusion.Game.Elements.EasyLoader.IKMotion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using System;

namespace Illusion.Game.Elements.EasyLoader
{
  [Serializable]
  public class IKMotion : AssetBundleData
  {
    public IKMotion()
    {
    }

    public IKMotion(string bundle, string asset)
      : base(bundle, asset)
    {
    }

    public MotionIK motionIK { get; private set; }

    public bool use { get; set; } = true;

    public void Create(ChaControl chaCtrl, MotionIK motionIK = null, params MotionIK[] partners)
    {
      if (motionIK != null)
      {
        this.motionIK = motionIK;
      }
      else
      {
        this.motionIK = new MotionIK(chaCtrl, false, (MotionIKData) null);
        this.motionIK.SetPartners(partners);
      }
    }

    public void Setting(ChaControl chaCtrl, string state)
    {
      this.Setting(chaCtrl, this.bundle, this.asset, state, false);
    }

    public void Setting(
      ChaControl chaCtrl,
      string bundle,
      string asset,
      string state,
      bool isCheck)
    {
      if (!this.use || isCheck && !this.Check(bundle, asset))
        return;
      if (this.motionIK == null)
        this.Create(chaCtrl, (MotionIK) null, (MotionIK[]) Array.Empty<MotionIK>());
      bool flag = false;
      if (!asset.IsNullOrEmpty())
      {
        this.asset = asset;
        if (!bundle.IsNullOrEmpty())
          this.bundle = bundle;
        this.motionIK.LoadData(bundle, asset, false);
        this.UnloadBundle(false, false);
        flag = true;
      }
      if (!state.IsNullOrEmpty())
        flag = true;
      this.motionIK.enabled = flag && this.motionIK.GetNowState(state) != null;
      if (!this.motionIK.enabled)
        return;
      this.motionIK.Calc(state);
    }
  }
}
