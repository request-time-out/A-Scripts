// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Voice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Illusion.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ADV.Commands.Base
{
  public class Voice : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[5]
        {
          "No",
          "Bundle",
          "Asset",
          "Personality",
          "Pitch"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[1]{ int.MaxValue.ToString() };
      }
    }

    public override void Do()
    {
      base.Do();
      TextScenario.CurrentCharaData currentCharaData = this.scenario.currentCharaData;
      currentCharaData.CreateVoiceList();
      List<Voice.Data> dataList = new List<Voice.Data>();
      if (this.args.Length > 1)
      {
        int cnt = 0;
        while (!this.args.IsNullOrEmpty(cnt))
        {
          Voice.Data data = new Voice.Data(this.args, ref cnt);
          CharaData chara = this.scenario.commandController.GetChara(data.no);
          if (chara != null)
          {
            data.transform = chara.voiceTrans;
            data.chaCtrl = chara.chaCtrl;
            if (!data.usePersonality)
              data.personality = chara.voiceNo;
            if (!data.usePitch)
              data.pitch = chara.voicePitch;
          }
          data.is2D = this.scenario.info.audio.is2D;
          data.isNotMoveMouth = this.scenario.info.audio.isNotMoveMouth;
          if (this.scenario.info.audio.eco.use)
            data.eco = this.scenario.info.audio.eco.DeepCopy<ADV.Info.Audio.Eco>();
          dataList.Add(data);
        }
      }
      currentCharaData.voiceList.Add((TextScenario.IVoice[]) dataList.ToArray());
      foreach (Voice.Data data in dataList)
      {
        if (data.bundle.IsNullOrEmpty())
        {
          string str;
          if (this.scenario.currentCharaData.bundleVoices.TryGetValue(data.personality, out str))
            data.bundle = str;
        }
        else
          this.scenario.currentCharaData.bundleVoices[data.personality] = data.bundle;
      }
    }

    public class Data : TextScenario.IVoice
    {
      public Data(string[] args, ref int cnt)
      {
        this.pitch = 1f;
        try
        {
          string[] strArray = args;
          int num1;
          cnt = (num1 = cnt) + 1;
          int index1 = num1;
          this.no = int.Parse(strArray[index1]);
          string[] array1 = args;
          int num2;
          cnt = (num2 = cnt) + 1;
          int index2 = num2;
          this.bundle = array1.SafeGet<string>(index2);
          string[] array2 = args;
          int num3;
          cnt = (num3 = cnt) + 1;
          int index3 = num3;
          this.asset = array2.SafeGet<string>(index3);
          string[] args1 = args;
          int num4;
          cnt = (num4 = cnt) + 1;
          int index4 = num4;
          Action<string> act1 = (Action<string>) (s => this.personality = int.Parse(s));
          this.usePersonality = args1.SafeProc(index4, act1);
          string[] args2 = args;
          int num5;
          cnt = (num5 = cnt) + 1;
          int index5 = num5;
          Action<string> act2 = (Action<string>) (s => this.pitch = float.Parse(s));
          this.usePitch = args2.SafeProc(index5, act2);
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Voice:" + string.Join(",", args)));
        }
      }

      public void Convert2D()
      {
        this.transform = (Transform) null;
      }

      public int no { get; private set; }

      public string bundle { get; set; }

      public string asset { get; private set; }

      public int personality { get; set; }

      public float pitch { get; set; }

      public ChaControl chaCtrl { get; set; }

      public Transform transform { get; set; }

      public ADV.Info.Audio.Eco eco { get; set; }

      public bool is2D { get; set; }

      public bool isNotMoveMouth { get; set; }

      public bool usePersonality { get; private set; }

      public bool usePitch { get; private set; }

      public AudioSource audio { get; private set; }

      public AudioSource Play()
      {
        Transform transform = Illusion.Game.Utils.Voice.Play(new Illusion.Game.Utils.Voice.Setting()
        {
          no = this.personality,
          assetBundleName = this.bundle,
          assetName = this.asset,
          voiceTrans = this.transform,
          isAsync = false,
          pitch = this.pitch,
          is2D = this.is2D
        });
        if (this.eco != null)
        {
          AudioEchoFilter audioEchoFilter = (AudioEchoFilter) ((Component) transform)?.get_gameObject().AddComponent<AudioEchoFilter>();
          audioEchoFilter.set_delay(this.eco.delay);
          audioEchoFilter.set_decayRatio(this.eco.decayRatio);
          audioEchoFilter.set_wetMix(this.eco.wetMix);
          audioEchoFilter.set_dryMix(this.eco.dryMix);
        }
        if (Object.op_Inequality((Object) this.chaCtrl, (Object) null) && Object.op_Inequality((Object) this.transform, (Object) null))
          this.chaCtrl.SetVoiceTransform(!this.isNotMoveMouth ? transform : (Transform) null);
        this.audio = (AudioSource) ((Component) transform)?.GetComponent<AudioSource>();
        if (Object.op_Inequality((Object) this.audio, (Object) null) && Object.op_Inequality((Object) this.transform, (Object) null))
          Singleton<Manager.Sound>.Instance.AudioSettingData3DOnly(this.audio, 1);
        return this.audio;
      }

      public bool Wait()
      {
        return Singleton<Manager.Voice>.Instance.IsVoiceCheck(this.personality, this.transform, false);
      }
    }
  }
}
