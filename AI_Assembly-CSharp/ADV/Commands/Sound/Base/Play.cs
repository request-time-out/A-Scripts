// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Sound.Base.Play
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace ADV.Commands.Sound.Base
{
  public abstract class Play : CommandBase
  {
    private Manager.Sound.Type type;
    private float delayTime;
    private bool isWait;
    private bool isStop;
    private Transform transform;
    private float timer;
    private Vector3? move;
    private float? stopTime;

    public Play(Manager.Sound.Type type)
    {
      this.type = type;
    }

    public override string[] ArgsLabel
    {
      get
      {
        return new string[18]
        {
          "Bundle",
          "Asset",
          "Delay",
          "Fade",
          "isName",
          "isAsync",
          "SettingNo",
          "isWait",
          "isStop",
          "isLoop",
          "Pitch",
          "PanStereo",
          "SpatialBlend",
          "Time",
          "Volume",
          "Pos",
          "Move",
          "Stop"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[18]
        {
          string.Empty,
          string.Empty,
          "0",
          "0",
          bool.TrueString,
          bool.TrueString,
          "-1",
          bool.FalseString,
          bool.FalseString,
          string.Empty,
          string.Empty,
          string.Empty,
          string.Empty,
          string.Empty,
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
      Illusion.Game.Utils.Sound.Setting s1 = new Illusion.Game.Utils.Sound.Setting(this.type);
      int num1 = 0;
      Illusion.Game.Utils.Sound.Setting setting1 = s1;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      string str1 = args1[index1];
      setting1.assetBundleName = str1;
      Illusion.Game.Utils.Sound.Setting setting2 = s1;
      string[] args2 = this.args;
      int index2 = num2;
      int num3 = index2 + 1;
      string str2 = args2[index2];
      setting2.assetName = str2;
      Illusion.Game.Utils.Sound.Setting setting3 = s1;
      string[] args3 = this.args;
      int index3 = num3;
      int num4 = index3 + 1;
      double num5;
      float num6 = (float) (num5 = (double) float.Parse(args3[index3]));
      setting3.delayTime = (float) num5;
      this.delayTime = num6;
      Illusion.Game.Utils.Sound.Setting setting4 = s1;
      string[] args4 = this.args;
      int index4 = num4;
      int num7 = index4 + 1;
      double num8 = (double) float.Parse(args4[index4]);
      setting4.fadeTime = (float) num8;
      Illusion.Game.Utils.Sound.Setting setting5 = s1;
      string[] args5 = this.args;
      int index5 = num7;
      int num9 = index5 + 1;
      int num10 = bool.Parse(args5[index5]) ? 1 : 0;
      setting5.isAssetEqualPlay = num10 != 0;
      Illusion.Game.Utils.Sound.Setting setting6 = s1;
      string[] args6 = this.args;
      int index6 = num9;
      int num11 = index6 + 1;
      int num12 = bool.Parse(args6[index6]) ? 1 : 0;
      setting6.isAsync = num12 != 0;
      Illusion.Game.Utils.Sound.Setting setting7 = s1;
      string[] args7 = this.args;
      int index7 = num11;
      int num13 = index7 + 1;
      int num14 = int.Parse(args7[index7]);
      setting7.settingNo = num14;
      string[] args8 = this.args;
      int index8 = num13;
      int num15 = index8 + 1;
      this.isWait = bool.Parse(args8[index8]);
      string[] args9 = this.args;
      int index9 = num15;
      int num16 = index9 + 1;
      this.isStop = bool.Parse(args9[index9]);
      this.transform = Illusion.Game.Utils.Sound.Play(s1);
      AudioSource audioSource = (AudioSource) ((Component) this.transform).GetComponent<AudioSource>();
      string[] args10 = this.args;
      int index10 = num16;
      int num17 = index10 + 1;
      Action<string> act1 = (Action<string>) (s => audioSource.set_loop(bool.Parse(s)));
      args10.SafeProc(index10, act1);
      string[] args11 = this.args;
      int index11 = num17;
      int num18 = index11 + 1;
      Action<string> act2 = (Action<string>) (s => audioSource.set_pitch(float.Parse(s)));
      args11.SafeProc(index11, act2);
      string[] args12 = this.args;
      int index12 = num18;
      int num19 = index12 + 1;
      Action<string> act3 = (Action<string>) (s => audioSource.set_panStereo(float.Parse(s)));
      args12.SafeProc(index12, act3);
      string[] args13 = this.args;
      int index13 = num19;
      int num20 = index13 + 1;
      Action<string> act4 = (Action<string>) (s => audioSource.set_spatialBlend(float.Parse(s)));
      args13.SafeProc(index13, act4);
      string[] args14 = this.args;
      int index14 = num20;
      int num21 = index14 + 1;
      Action<string> act5 = (Action<string>) (s => audioSource.set_time(float.Parse(s)));
      args14.SafeProc(index14, act5);
      string[] args15 = this.args;
      int index15 = num21;
      int num22 = index15 + 1;
      Action<string> act6 = (Action<string>) (s => audioSource.set_volume(float.Parse(s)));
      args15.SafeProc(index15, act6);
      string[] args16 = this.args;
      int index16 = num22;
      int num23 = index16 + 1;
      Action<string> act7 = (Action<string>) (s =>
      {
        Vector3 v;
        if (!this.scenario.commandController.V3Dic.TryGetValue(s, out v))
        {
          int cnt = 0;
          CommandBase.CountAddV3(s.Split(','), ref cnt, ref v);
        }
        this.transform.set_position(v);
      });
      args16.SafeProc(index16, act7);
      string[] args17 = this.args;
      int index17 = num23;
      int num24 = index17 + 1;
      Action<string> act8 = (Action<string>) (s =>
      {
        Vector3 v;
        if (!this.scenario.commandController.V3Dic.TryGetValue(s, out v))
        {
          int cnt = 0;
          CommandBase.CountAddV3(s.Split(','), ref cnt, ref v);
        }
        this.move = new Vector3?(v);
      });
      args17.SafeProc(index17, act8);
      string[] args18 = this.args;
      int index18 = num24;
      int num25 = index18 + 1;
      Action<string> act9 = (Action<string>) (s => this.stopTime = new float?(float.Parse(s)));
      args18.SafeProc(index18, act9);
    }

    public override bool Process()
    {
      base.Process();
      if (!this.isWait)
        return true;
      if ((double) this.timer >= (double) this.delayTime)
      {
        if (!Singleton<Manager.Sound>.Instance.IsPlay(this.type, this.transform))
          return true;
      }
      else
      {
        this.timer += Time.get_deltaTime();
        Debug.Log((object) ("timer:" + (object) this.timer));
      }
      if (this.move.HasValue)
        this.transform.Translate(Vector3.op_Multiply(this.move.Value, Time.get_deltaTime()));
      return this.stopTime.HasValue && (double) this.timer >= (double) this.stopTime.Value;
    }

    public override void Result(bool processEnd)
    {
      base.Result(processEnd);
      if (!this.isStop)
        return;
      Singleton<Manager.Sound>.Instance.Stop(this.transform);
    }
  }
}
