// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.ItemCreate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ADV.Commands.Chara
{
  public class ItemCreate : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[9]
        {
          "No",
          "ItemNo",
          "Bundle",
          "Asset",
          "Root",
          "isWorldPositionStays",
          "Manifest",
          "Pos",
          "Ang"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[9]
        {
          int.MaxValue.ToString(),
          "0",
          string.Empty,
          string.Empty,
          string.Empty,
          bool.FalseString,
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
      int no = int.Parse(args1[index1]);
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      int key = int.Parse(args2[index2]);
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      string bundle = args3[index3];
      string[] args4 = this.args;
      int index4 = num4;
      int num5 = index4 + 1;
      string asset = args4[index4];
      string[] args5 = this.args;
      int index5 = num5;
      int num6 = index5 + 1;
      string root = args5[index5];
      string[] args6 = this.args;
      int index6 = num6;
      int num7 = index6 + 1;
      bool worldPositionStays = bool.Parse(args6[index6]);
      string[] args7 = this.args;
      int index7 = num7;
      int num8 = index7 + 1;
      string manifest = args7.SafeGet<string>(index7);
      string[] args8 = this.args;
      int index8 = num8;
      int num9 = index8 + 1;
      string str1 = args8.SafeGet<string>(index8);
      string[] args9 = this.args;
      int index9 = num9;
      int num10 = index9 + 1;
      string str2 = args9.SafeGet<string>(index9);
      CharaData chara = this.scenario.commandController.GetChara(no);
      Transform root1 = (Transform) null;
      if (!root.IsNullOrEmpty() && Object.op_Inequality((Object) chara.chaCtrl, (Object) null))
        root1 = ((IEnumerable<Transform>) ((Component) ((Component) chara.chaCtrl).get_transform()).GetComponentsInChildren<Transform>(true)).FirstOrDefault<Transform>((Func<Transform, bool>) (p => ((Object) p).get_name() == root));
      if (Object.op_Equality((Object) root1, (Object) null))
        root1 = ((Component) this.scenario.advScene).get_transform();
      CharaData.CharaItem charaItem;
      if (chara.itemDic.TryGetValue(key, out charaItem))
        charaItem.Delete();
      charaItem = new CharaData.CharaItem();
      charaItem.LoadObject(bundle, asset, root1, worldPositionStays, manifest);
      Vector3 pos;
      if (!this.scenario.commandController.GetV3Dic(str1, out pos))
      {
        int cnt = 0;
        CommandBase.CountAddV3(str1.Split(','), ref cnt, ref pos);
      }
      Transform transform1 = charaItem.item.get_transform();
      transform1.set_localPosition(Vector3.op_Addition(transform1.get_localPosition(), pos));
      if (!this.scenario.commandController.GetV3Dic(str2, out pos))
      {
        int cnt = 0;
        CommandBase.CountAddV3(str2.Split(','), ref cnt, ref pos);
      }
      Transform transform2 = charaItem.item.get_transform();
      transform2.set_localEulerAngles(Vector3.op_Addition(transform2.get_localEulerAngles(), pos));
      chara.itemDic[key] = charaItem;
    }
  }
}
