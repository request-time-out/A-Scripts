// Decompiled with JetBrains decompiler
// Type: ADV.Commands.EventCG.Setting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ADV.EventCG;
using AIChara;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace ADV.Commands.EventCG
{
  public class Setting : CommandBase
  {
    private Data data;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "Bundle", "Asset", "No" };
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
      Common.Release(this.scenario);
      int num1 = 0;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      string bundle = args1[index1];
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      string asset = args2[index2];
      int? no = new int?();
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      Action<string> act = (Action<string>) (s => no = new int?(int.Parse(s)));
      args3.SafeProc(index3, act);
      Action action = (Action) (() =>
      {
        GameObject asset1 = AssetBundleManager.LoadAsset(bundle, asset, typeof (GameObject), (string) null).GetAsset<GameObject>();
        this.data = (Data) ((GameObject) Object.Instantiate<GameObject>((M0) asset1, this.scenario.commandController.EventCGRoot, false)).GetComponent<Data>();
        if (no.HasValue)
        {
          Transform transform1 = ((Component) this.data).get_transform();
          Transform transform2 = this.scenario.commandController.GetChara(no.Value).backup.transform;
          Transform transform3 = transform1;
          transform3.set_position(Vector3.op_Addition(transform3.get_position(), transform2.get_position()));
          Transform transform4 = transform1;
          transform4.set_rotation(Quaternion.op_Multiply(transform4.get_rotation(), transform2.get_rotation()));
        }
        ((Object) this.data).set_name(((Object) asset1).get_name());
        AssetBundleManager.UnloadAssetBundle(bundle, false, (string) null, false);
      });
      if (!bundle.IsNullOrEmpty())
      {
        action();
      }
      else
      {
        foreach (string assetBundleName in CommonLib.GetAssetBundleNameListFromPath("adv/scenario/eventcg/", true))
        {
          if (asset.Check(true, AssetBundleCheck.GetAllAssetName(assetBundleName, false, (string) null, false)) != -1)
          {
            bundle = assetBundleName;
            action();
            break;
          }
        }
      }
      this.scenario.commandController.useCorrectCamera = false;
      if (Object.op_Inequality((Object) this.scenario.virtualCamera, (Object) null))
      {
        this.data.camRoot = ((Component) this.scenario.virtualCamera).get_transform();
        if (Object.op_Inequality((Object) this.data.cameraData, (Object) null))
        {
          CharaData charaData = this.scenario.commandController.Characters.Values.FirstOrDefault<CharaData>((Func<CharaData, bool>) (p => p.data.isHeroine));
          if (charaData != null)
          {
            ChaControl chaCtrl = charaData.chaCtrl;
            if (!((Collection<ChaControl>) this.data.cameraData.chaCtrlList).Contains(chaCtrl))
              ((Collection<ChaControl>) this.data.cameraData.chaCtrlList).Add(chaCtrl);
          }
        }
      }
      else
        this.data.camRoot = ((Component) this.scenario.AdvCamera).get_transform();
      CommandController commandController = this.scenario.commandController;
      this.data.SetChaRoot(commandController.CharaRoot, commandController.Characters);
      this.data.Next(0, commandController.Characters);
    }
  }
}
