// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Effect.FilterImageLoad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace ADV.Commands.Effect
{
  public class FilterImageLoad : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]
        {
          "Bundle",
          "Asset",
          "Manifest"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[3]
        {
          string.Empty,
          string.Empty,
          string.Empty
        };
      }
    }

    public override void Do()
    {
      base.Do();
      int num1 = 0;
      string bundleName = (string) null;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      Action<string> act1 = (Action<string>) (s => bundleName = s);
      if (args1.SafeProc(index1, act1))
      {
        string assetName = (string) null;
        string[] args2 = this.args;
        int index2 = num2;
        int num3 = index2 + 1;
        Action<string> act2 = (Action<string>) (s => assetName = s);
        args2.SafeProc(index2, act2);
        string manifest = (string) null;
        string[] args3 = this.args;
        int index3 = num3;
        int num4 = index3 + 1;
        Action<string> act3 = (Action<string>) (s => manifest = s);
        args3.SafeProc(index3, act3);
        Illusion.Game.Utils.Bundle.LoadSprite(bundleName, assetName, this.scenario.FilterImage, false, (string) null, manifest, (string) null);
      }
      ((Behaviour) this.scenario.FilterImage).set_enabled(true);
    }
  }
}
