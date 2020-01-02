// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.ItemFind
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ADV.Commands.Chara
{
  public class ItemFind : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[3]{ "No", "ItemNo", "Name" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[3]
        {
          int.MaxValue.ToString(),
          "0",
          "Find"
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
      int index3 = int.Parse(args2[index2]);
      string[] args3 = this.args;
      int index4 = num3;
      int num4 = index4 + 1;
      string findName = args3[index4];
      CharaData chara = this.scenario.commandController.GetChara(no);
      Transform transform = ((IEnumerable<Transform>) ((Component) chara.chaCtrl).GetComponentsInChildren<Transform>(true)).FirstOrDefault<Transform>((Func<Transform, bool>) (p => ((Object) p).get_name() == findName));
      if (!Object.op_Inequality((Object) transform, (Object) null))
        return;
      chara.itemDic[index3] = new CharaData.CharaItem(((Component) transform).get_gameObject());
    }
  }
}
