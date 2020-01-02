// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Effect.SceneFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using UnityEngine;

namespace ADV.Commands.Effect
{
  public class SceneFade : CommandBase
  {
    private SimpleFade fade;
    private string assetBundleName;
    private float bkFadeTime;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[5]
        {
          "Fade",
          "Time",
          "Color",
          "Bundle",
          "Asset"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[5]
        {
          "in",
          string.Empty,
          string.Empty,
          string.Empty,
          "none"
        };
      }
    }

    public override void Do()
    {
      base.Do();
      int num1 = 0;
      this.fade = (SimpleFade) Singleton<Scene>.Instance.sceneFade;
      SimpleFade fade1 = this.fade;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      int num3 = !args1[index1].Compare("in", true) ? 1 : 0;
      fade1._Fade = (SimpleFade.Fade) num3;
      this.bkFadeTime = this.fade._Time;
      string[] args2 = this.args;
      int index2 = num2;
      int num4 = index2 + 1;
      Action<string> act = (Action<string>) (s => this.fade._Time = float.Parse(s));
      args2.SafeProc(index2, act);
      string[] args3 = this.args;
      int index3 = num4;
      int num5 = index3 + 1;
      string self1 = args3[index3];
      if (!self1.IsNullOrEmpty())
        this.fade._Color = self1.GetColor();
      string[] args4 = this.args;
      int index4 = num5;
      int num6 = index4 + 1;
      string self2 = args4[index4];
      if (!self2.IsNullOrEmpty())
      {
        SimpleFade fade2 = this.fade;
        string assetBundleName = self2;
        string[] args5 = this.args;
        int index5 = num6;
        int num7 = index5 + 1;
        string assetName = args5[index5];
        System.Type type = typeof (Texture2D);
        Texture2D asset = AssetBundleManager.LoadAsset(assetBundleName, assetName, type, (string) null).GetAsset<Texture2D>();
        fade2._Texture = asset;
      }
      this.fade.Init();
    }

    public override bool Process()
    {
      base.Process();
      return this.fade.IsEnd;
    }

    public override void Result(bool processEnd)
    {
      if (this.fade._Fade == SimpleFade.Fade.Out && !this.assetBundleName.IsNullOrEmpty())
        AssetBundleManager.UnloadAssetBundle(this.assetBundleName, true, (string) null, false);
      this.fade._Time = this.bkFadeTime;
      this.fade.ForceEnd();
    }
  }
}
