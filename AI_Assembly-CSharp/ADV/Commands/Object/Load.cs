// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Object.Load
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace ADV.Commands.Object
{
  public class Load : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[4]
        {
          "Name",
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
        return new string[4]
        {
          string.Empty,
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
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      string self = args1[index1];
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      string assetBundleName = args2[index2];
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      string assetName = args3[index3];
      string[] args4 = this.args;
      int index4 = num4;
      int num5 = index4 + 1;
      string manifestAssetBundleName = args4[index4];
      GameObject asset = AssetBundleManager.LoadAsset(assetBundleName, assetName, typeof (GameObject), manifestAssetBundleName).GetAsset<GameObject>();
      GameObject go = (GameObject) UnityEngine.Object.Instantiate<GameObject>((M0) asset);
      if (!self.IsNullOrEmpty())
        ((UnityEngine.Object) go).set_name(self);
      else
        ((UnityEngine.Object) go).set_name(((UnityEngine.Object) asset).get_name());
      AssetBundleManager.UnloadAssetBundle(assetBundleName, false, manifestAssetBundleName, false);
      this.scenario.commandController.SetObject(go);
    }
  }
}
