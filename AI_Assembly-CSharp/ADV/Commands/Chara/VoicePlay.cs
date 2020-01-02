// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Chara.VoicePlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using System;
using UnityEngine;

namespace ADV.Commands.Chara
{
  public class VoicePlay : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[12]
        {
          "No",
          "Type",
          "Bundle",
          "Asset",
          "Delay",
          "Fade",
          "isLoop",
          "isAsync",
          "VoiceNo",
          "Pitch",
          "is2D",
          "useADV"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[12]
        {
          int.MaxValue.ToString(),
          VoicePlay.Type.Normal.ToString(),
          string.Empty,
          string.Empty,
          "0",
          "0",
          bool.FalseString,
          bool.TrueString,
          string.Empty,
          string.Empty,
          bool.FalseString,
          bool.TrueString
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
      int num4 = args2[index2].Check(true, Illusion.Utils.Enum<VoicePlay.Type>.Names);
      CharaData chara = this.scenario.commandController.GetChara(no);
      string[] args3 = this.args;
      int index3 = num3;
      int num5 = index3 + 1;
      string str1 = args3[index3];
      string[] args4 = this.args;
      int index4 = num5;
      int num6 = index4 + 1;
      string str2 = args4[index4];
      string[] args5 = this.args;
      int index5 = num6;
      int num7 = index5 + 1;
      float num8 = float.Parse(args5[index5]);
      string[] args6 = this.args;
      int index6 = num7;
      int num9 = index6 + 1;
      float num10 = float.Parse(args6[index6]);
      string[] args7 = this.args;
      int index7 = num9;
      int num11 = index7 + 1;
      bool flag1 = bool.Parse(args7[index7]);
      string[] args8 = this.args;
      int index8 = num11;
      int num12 = index8 + 1;
      bool flag2 = bool.Parse(args8[index8]);
      int voiceNo = 0;
      string[] args9 = this.args;
      int index9 = num12;
      int num13 = index9 + 1;
      Action<string> act1 = (Action<string>) (s => voiceNo = int.Parse(s));
      bool flag3 = args9.SafeProc(index9, act1);
      float pitch = 1f;
      string[] args10 = this.args;
      int index10 = num13;
      int num14 = index10 + 1;
      Action<string> act2 = (Action<string>) (s => pitch = float.Parse(s));
      bool flag4 = args10.SafeProc(index10, act2);
      string[] args11 = this.args;
      int index11 = num14;
      int num15 = index11 + 1;
      bool flag5 = bool.Parse(args11[index11]);
      string[] args12 = this.args;
      int index12 = num15;
      int num16 = index12 + 1;
      bool flag6 = bool.Parse(args12[index12]);
      Illusion.Game.Utils.Voice.Setting s1 = new Illusion.Game.Utils.Voice.Setting()
      {
        no = voiceNo,
        assetBundleName = str1,
        assetName = str2,
        delayTime = num8,
        fadeTime = num10,
        isPlayEndDelete = !flag1,
        isAsync = flag2,
        pitch = pitch,
        is2D = flag5
      };
      ChaControl chaCtrl = (ChaControl) null;
      if (chara != null)
      {
        chaCtrl = chara.chaCtrl;
        if (!flag3)
          s1.no = chara.voiceNo;
        if (!flag4)
          s1.pitch = chara.voicePitch;
        s1.voiceTrans = chara.voiceTrans;
      }
      Transform trfVoice = (Transform) null;
      switch ((VoicePlay.Type) num4)
      {
        case VoicePlay.Type.Normal:
          trfVoice = Illusion.Game.Utils.Voice.OnecePlayChara(s1);
          break;
        case VoicePlay.Type.Onece:
          trfVoice = Illusion.Game.Utils.Voice.OnecePlay(s1);
          break;
        case VoicePlay.Type.Overlap:
          trfVoice = Illusion.Game.Utils.Voice.Play(s1);
          break;
      }
      if (Object.op_Inequality((Object) chaCtrl, (Object) null))
        chaCtrl.SetVoiceTransform(trfVoice);
      if (!Object.op_Inequality((Object) trfVoice, (Object) null))
        return;
      AudioSource component = (AudioSource) ((Component) trfVoice).GetComponent<AudioSource>();
      if (!Object.op_Inequality((Object) component, (Object) null))
        return;
      if (flag6)
        Singleton<Manager.Sound>.Instance.AudioSettingData3DOnly(component, 1);
      if (!flag1)
        return;
      component.set_loop(flag1);
      this.scenario.loopVoiceList.Add(new TextScenario.LoopVoicePack(s1.no, chaCtrl, component));
    }

    private enum Type
    {
      Normal,
      Onece,
      Overlap,
    }
  }
}
