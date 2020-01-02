// Decompiled with JetBrains decompiler
// Type: Illusion.Game.Elements.EasyLoader.BaseMotion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace Illusion.Game.Elements.EasyLoader
{
  [Serializable]
  public abstract class BaseMotion : AssetBundleData
  {
    public string state;

    public BaseMotion()
    {
    }

    public BaseMotion(string bundle, string asset)
      : base(bundle, asset)
    {
    }

    public BaseMotion(string bundle, string asset, string state)
      : base(bundle, asset)
    {
      this.state = state;
    }

    public new bool isEmpty
    {
      get
      {
        return base.isEmpty || this.state.IsNullOrEmpty();
      }
    }

    public bool Check(string bundle, string asset, string state)
    {
      return !state.IsNullOrEmpty() && this.state != state || this.Check(bundle, asset);
    }
  }
}
