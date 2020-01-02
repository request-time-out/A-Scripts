// Decompiled with JetBrains decompiler
// Type: Illusion.Game.Elements.EasyLoader.YureMotion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using System;

namespace Illusion.Game.Elements.EasyLoader
{
  [Serializable]
  public class YureMotion : AssetBundleData
  {
    public YureMotion()
    {
    }

    public YureMotion(string bundle, string asset)
      : base(bundle, asset)
    {
    }

    public YureCtrlEx yureCtrl { get; private set; }

    public void Create(ChaControl chaCtrl, YureCtrlEx yureCtrl = null)
    {
      if (yureCtrl != null)
      {
        this.yureCtrl = yureCtrl;
      }
      else
      {
        this.yureCtrl = new YureCtrlEx();
        this.yureCtrl.Init(chaCtrl);
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
      if (isCheck && !this.Check(bundle, asset))
        return;
      bool flag = false;
      if (this.yureCtrl == null)
        this.Create(chaCtrl, (YureCtrlEx) null);
      if (!asset.IsNullOrEmpty())
      {
        this.asset = asset;
        if (!bundle.IsNullOrEmpty())
          this.bundle = bundle;
        this.yureCtrl.Load(this.bundle, this.asset);
        flag = true;
      }
      if (!state.IsNullOrEmpty())
        flag = true;
      if (!flag)
        return;
      this.yureCtrl.Proc(state);
    }
  }
}
