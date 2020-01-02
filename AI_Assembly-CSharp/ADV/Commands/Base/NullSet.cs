// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.NullSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion;
using Illusion.Extensions;
using UnityEngine;

namespace ADV.Commands.Base
{
  public class NullSet : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "Name", "Type" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[2]
        {
          string.Empty,
          NullSet.Type.Base.ToString()
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
      string name = args1[index1];
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      string self = args2[index2];
      switch (self.Check(true, Utils.Enum<NullSet.Type>.Names))
      {
        case 0:
          this.Set(this.scenario.commandController.BaseRoot, name);
          break;
        case 1:
          Transform cameraRoot = this.scenario.commandController.CameraRoot;
          Vector3 position = cameraRoot.get_position();
          Quaternion rotation = cameraRoot.get_rotation();
          cameraRoot.set_position(Vector3.get_zero());
          cameraRoot.set_rotation(Quaternion.get_identity());
          this.Set(((Component) this.scenario.AdvCamera).get_transform(), name);
          cameraRoot.SetPositionAndRotation(position, rotation);
          break;
        case 2:
          this.Set(this.scenario.commandController.CharaRoot, name);
          break;
        default:
          Debug.LogError((object) ("該当なし:" + self));
          break;
      }
    }

    private void Set(Transform transform, string name)
    {
      Transform transform1;
      if (this.scenario.commandController.NullDic.TryGetValue(name, out transform1))
        transform.SetPositionAndRotation(transform1.get_position(), transform1.get_rotation());
      else
        Debug.LogError((object) (name + " : not find"));
    }

    private enum Type
    {
      Base,
      Camera,
      Chara,
    }
  }
}
