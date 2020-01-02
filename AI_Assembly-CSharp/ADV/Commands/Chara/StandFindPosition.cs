// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.StandFindPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ADV.Commands.Chara
{
  public class StandFindPosition : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[4]
        {
          "No",
          "Type",
          "Name",
          "Child"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[4]
        {
          int.MaxValue.ToString(),
          StandFindPosition.Type.World.ToString(),
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
      int num4 = args2[index2].Check(true, Enum.GetNames(typeof (StandFindPosition.Type)));
      string[] args3 = this.args;
      int index3 = num3;
      int num5 = index3 + 1;
      string findName = args3[index3];
      string childName = string.Empty;
      string[] args4 = this.args;
      int index4 = num5;
      int num6 = index4 + 1;
      Action<string> act = (Action<string>) (s => childName = s);
      args4.SafeProc(index4, act);
      Transform transform = this.scenario.commandController.GetChara(no).transform;
      Transform stand = (Transform) null;
      switch (num4)
      {
        case 0:
          GameObject.Find(findName).SafeProc<GameObject>((Action<GameObject>) (p => stand = p.get_transform()));
          break;
        case 1:
          GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag(findName);
          stand = gameObjectWithTag.get_transform();
          if (!childName.IsNullOrEmpty())
          {
            stand = ((IEnumerable<Transform>) gameObjectWithTag.GetComponentsInChildren<Transform>()).FirstOrDefault<Transform>((Func<Transform, bool>) (t => ((Object) t).get_name().Compare(childName, true)));
            break;
          }
          break;
        case 2:
          stand = this.scenario.commandController.NullDic[findName];
          if (!childName.IsNullOrEmpty())
          {
            stand = ((IEnumerable<Transform>) ((Component) stand).GetComponentsInChildren<Transform>()).FirstOrDefault<Transform>((Func<Transform, bool>) (t => ((Object) t).get_name().Compare(childName, true)));
            break;
          }
          break;
        case 3:
          stand = this.scenario.commandController.EventCGRoot.Children().Find((Predicate<Transform>) (p => ((Object) p).get_name().Compare(findName, true)));
          if (!childName.IsNullOrEmpty())
          {
            stand = stand.Children().Find((Predicate<Transform>) (t => ((Object) t).get_name().Compare(childName, true)));
            break;
          }
          break;
      }
      transform.SetPositionAndRotation(stand.get_position(), stand.get_rotation());
    }

    public enum Type
    {
      World,
      Tag,
      Null,
      EventCG,
    }
  }
}
