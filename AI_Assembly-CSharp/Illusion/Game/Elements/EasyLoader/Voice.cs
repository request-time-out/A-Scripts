// Decompiled with JetBrains decompiler
// Type: Illusion.Game.Elements.EasyLoader.Voice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using System;

namespace Illusion.Game.Elements.EasyLoader
{
  [Serializable]
  public class Voice : AssetBundleData
  {
    public int personality;

    public virtual void Setting(ChaControl chaCtrl, int personality, string bundle, string asset)
    {
      bool flag = false;
      if (!asset.IsNullOrEmpty())
      {
        this.asset = asset;
        if (!bundle.IsNullOrEmpty())
          this.bundle = bundle;
        flag = true;
      }
      if (!flag)
        ;
    }

    public virtual void Setting(ChaControl chaCtrl)
    {
      this.Setting(chaCtrl, this.personality, this.bundle, this.asset);
    }
  }
}
