// Decompiled with JetBrains decompiler
// Type: ADV.Commands.MapScene.PlayActionSE
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using UnityEngine;

namespace ADV.Commands.MapScene
{
  public class PlayActionSE : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[4]
        {
          "ID",
          "FadeTime",
          "Type",
          "Pos"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[4]
        {
          "0",
          "0",
          Sound.Type.GameSE3D.ToString(),
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
      int key = int.Parse(args1[index1]);
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      float fadeTime = float.Parse(args2[index2]);
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      string str = args3[index3];
      string posStr = string.Empty;
      string[] args4 = this.args;
      int index4 = num4;
      int num5 = index4 + 1;
      Action<string> act = (Action<string>) (s => posStr = s);
      args4.SafeProc(index4, act);
      int result;
      if (!int.TryParse(str, out result))
        result = Illusion.Utils.Enum<Sound.Type>.FindIndex(str, true);
      AudioSource audioSource = Singleton<Resources>.Instance.SoundPack.Play(key, (Sound.Type) result, fadeTime);
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
  }
}
