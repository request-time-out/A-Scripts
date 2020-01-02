// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.CoordinateChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using UnityEngine;

namespace ADV.Commands.Chara
{
  public class CoordinateChange : CommandBase
  {
    private int no;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "No", "Bundle", "Asset" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[3]
        {
          "0",
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
      this.no = int.Parse(args1[index1]);
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      string assetBundleName = args2[index2];
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      string assetName = args3[index3];
      TextAsset ta = CommonLib.LoadAsset<TextAsset>(assetBundleName, assetName, false, string.Empty);
      if (Object.op_Inequality((Object) ta, (Object) null))
      {
        ChaControl chaCtrl = this.scenario.commandController.GetChara(this.no).chaCtrl;
        chaCtrl.nowCoordinate.LoadFile(ta);
        chaCtrl.Reload(false, true, true, true, true);
      }
      AssetBundleManager.UnloadAssetBundle(assetBundleName, true, (string) null, false);
    }
  }
}
