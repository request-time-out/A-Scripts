// Decompiled with JetBrains decompiler
// Type: ADV.Commands.MapScene.PlayFootStepSE
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using Manager;
using UnityEngine;

namespace ADV.Commands.MapScene
{
  public class PlayFootStepSE : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[6]
        {
          "sex",
          "bareFoot",
          "seType",
          "weather",
          "areaType",
          "Pos"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[6]
        {
          "0",
          bool.FalseString,
          AIProject.Definitions.Map.FootStepSE.Sand.ToString(),
          Weather.Clear.ToString(),
          SoundPack.PlayAreaType.Normal.ToString(),
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
      int num3 = int.Parse(args1[index1]);
      string[] args2 = this.args;
      int index2 = num2;
      int num4 = index2 + 1;
      bool bareFoot = bool.Parse(args2[index2]);
      string[] args3 = this.args;
      int index3 = num4;
      int num5 = index3 + 1;
      AIProject.Definitions.Map.FootStepSE seType = (AIProject.Definitions.Map.FootStepSE) PlayFootStepSE.Get<AIProject.Definitions.Map.FootStepSE>(args3[index3]);
      string[] args4 = this.args;
      int index4 = num5;
      int num6 = index4 + 1;
      Weather weather = (Weather) PlayFootStepSE.Get<Weather>(args4[index4]);
      string[] args5 = this.args;
      int index5 = num6;
      int num7 = index5 + 1;
      SoundPack.PlayAreaType areaType = (SoundPack.PlayAreaType) PlayFootStepSE.Get<SoundPack.PlayAreaType>(args5[index5]);
      string posStr = string.Empty;
      string[] args6 = this.args;
      int index6 = num7;
      int num8 = index6 + 1;
      System.Action<string> act = (System.Action<string>) (s => posStr = s);
      args6.SafeProc(index6, act);
      AudioSource audioSource = Singleton<Resources>.Instance.SoundPack.PlayFootStep((byte) num3, bareFoot, seType, weather, areaType);
      if (!Object.op_Inequality((Object) audioSource, (Object) null))
        return;
      Vector3 vector3;
      if (this.scenario.commandController.V3Dic.TryGetValue(posStr, out vector3))
      {
        ((Component) audioSource).get_transform().set_position(vector3);
      }
      else
      {
        if (!Object.op_Inequality((Object) this.scenario.AdvCamera, (Object) null))
          return;
        ((Component) audioSource).get_transform().set_position(((Component) this.scenario.AdvCamera).get_transform().get_position());
      }
    }

    private static int Get<T>(string name) where T : struct
    {
      int result;
      if (!int.TryParse(name, out result))
        result = Illusion.Utils.Enum<T>.FindIndex(name, true);
      return result;
    }
  }
}
