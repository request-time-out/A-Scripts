// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Sound.BGM.Play
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using UnityEngine;

namespace ADV.Commands.Sound.BGM
{
  public class Play : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[9]
        {
          "Bundle",
          "Asset",
          "Delay",
          "Fade",
          "isAsync",
          "Pitch",
          "PanStereo",
          "Time",
          "Volume"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[9]
        {
          "sound/data/bgm/00.unity3d",
          string.Empty,
          "0",
          string.Empty,
          bool.TrueString,
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
      Illusion.Game.Utils.Sound.SettingBGM setting = new Illusion.Game.Utils.Sound.SettingBGM(args1[index1]);
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      Action<string> act1 = (Action<string>) (s => setting.assetName = s);
      args2.SafeProc(index2, act1);
      Illusion.Game.Utils.Sound.SettingBGM settingBgm1 = setting;
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      double num5 = (double) float.Parse(args3[index3]);
      settingBgm1.delayTime = (float) num5;
      string[] args4 = this.args;
      int index4 = num4;
      int num6 = index4 + 1;
      Action<string> act2 = (Action<string>) (s => setting.fadeTime = float.Parse(s));
      args4.SafeProc(index4, act2);
      Illusion.Game.Utils.Sound.SettingBGM settingBgm2 = setting;
      string[] args5 = this.args;
      int index5 = num6;
      int num7 = index5 + 1;
      int num8 = bool.Parse(args5[index5]) ? 1 : 0;
      settingBgm2.isAsync = num8 != 0;
      if (Singleton<SoundPlayer>.IsInstance())
        Singleton<SoundPlayer>.Instance.MuteHousingAreaBGM(setting.fadeTime, false);
      AudioSource audioSource = (AudioSource) ((Component) Illusion.Game.Utils.Sound.Play((Illusion.Game.Utils.Sound.Setting) setting)).GetComponent<AudioSource>();
      string[] args6 = this.args;
      int index6 = num7;
      int num9 = index6 + 1;
      Action<string> act3 = (Action<string>) (s => audioSource.set_pitch(float.Parse(s)));
      args6.SafeProc(index6, act3);
      string[] args7 = this.args;
      int index7 = num9;
      int num10 = index7 + 1;
      Action<string> act4 = (Action<string>) (s => audioSource.set_panStereo(float.Parse(s)));
      args7.SafeProc(index7, act4);
      string[] args8 = this.args;
      int index8 = num10;
      int num11 = index8 + 1;
      Action<string> act5 = (Action<string>) (s => audioSource.set_time(float.Parse(s)));
      args8.SafeProc(index8, act5);
      string[] args9 = this.args;
      int index9 = num11;
      int num12 = index9 + 1;
      Action<string> act6 = (Action<string>) (s => audioSource.set_volume(float.Parse(s)));
      args9.SafeProc(index9, act6);
    }
  }
}
