// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Text
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ADV.Commands.Base
{
  public class Text : CommandBase
  {
    private Text.Next next;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[2]{ "Name", nameof (Text) };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return (string[]) null;
      }
    }

    public override void Convert(string fileName, ref string[] args)
    {
      int index1 = 1;
      string self = args.SafeGet<string>(index1);
      if (self.IsNullOrEmpty())
      {
        Debug.LogWarningFormat("FileName:{0}, Command TEXT text none\nArgs:{1}", new object[2]
        {
          (object) fileName,
          (object) string.Join(",", args)
        });
      }
      else
      {
        string[] strArray = args;
        int index2 = index1;
        int num = index2 + 1;
        string str = string.Join("\n", self.Split(new string[1]
        {
          "@br"
        }, StringSplitOptions.None));
        strArray[index2] = str;
      }
    }

    public override void Do()
    {
      base.Do();
      Text.Data data = new Text.Data(this.args);
      this.scenario.fontColorKey = data.name;
      if (data.name != string.Empty)
        data.name = this.scenario.ReplaceText(data.name);
      if (data.text != string.Empty)
        data.text = this.scenario.ReplaceText(data.text);
      TextScenario.CurrentCharaData currentCharaData = this.scenario.currentCharaData;
      if (currentCharaData.isSkip || Manager.Config.GameData.NextVoiceStop)
        this.scenario.VoicePlay((List<TextScenario.IVoice[]>) null, (Action) null, (Action) null);
      currentCharaData.isSkip = false;
      this.scenario.captionSystem.SetName(data.name);
      this.scenario.captionSystem.SetText(data.text, false);
      string str = (string) null;
      switch (data.colorKey)
      {
        case "[H]":
          str = MapUIContainer.CharaNameColor(this.scenario.currentChara?.data?.actor);
          break;
        case "[P]":
          str = MapUIContainer.CharaNameColor(this.scenario.player?.actor);
          break;
        case "[M]":
          Actor actor = (Actor) this.scenario.currentChara?.data?.merchantActor;
          if (Object.op_Equality((Object) actor, (Object) null))
          {
            List<TextScenario.ParamData> heroineList = this.scenario.heroineList;
            actor = heroineList != null ? heroineList.FirstOrDefault<TextScenario.ParamData>((Func<TextScenario.ParamData, bool>) (p => Object.op_Inequality((Object) p.merchantActor, (Object) null)))?.actor : (Actor) null;
          }
          str = MapUIContainer.CharaNameColor(actor);
          break;
      }
      if (str != null)
        data.name = str;
      this.scenario.TextLogCall(data, (IReadOnlyCollection<TextScenario.IVoice[]>) currentCharaData.voiceList);
      this.next = new Text.Next(this.scenario);
    }

    public override bool Process()
    {
      base.Process();
      return this.next.Process();
    }

    public override void Result(bool processEnd)
    {
      base.Result(processEnd);
      this.next.Result();
    }

    public class Data
    {
      public string name = string.Empty;
      public string text = string.Empty;

      public Data(params string[] args)
      {
        int index1 = 1;
        int index2 = index1;
        this.colorKey = this.name = args.SafeGet<string>(0) ?? string.Empty;
        this.text = args.SafeGet<string>(index2) ?? args.SafeGet<string>(index1) ?? string.Empty;
      }

      public string colorKey { get; private set; }
    }

    private class Next
    {
      private List<List<Action>> playList = new List<List<Action>>();
      private int currentNo;
      private TextScenario scenario;
      private Action onChange;
      private bool voicePlayEnd;

      public Next(TextScenario scenario)
      {
        Text.Next next = this;
        this.scenario = scenario;
        TextScenario.CurrentCharaData currentCharaData = scenario.currentCharaData;
        List<TextScenario.IMotion[]> motionList = currentCharaData.motionList;
        List<TextScenario.IExpression[]> expressionList = currentCharaData.expressionList;
        int cnt = 0;
        Func<bool> func1 = (Func<bool>) (() => !motionList.IsNullOrEmpty<TextScenario.IMotion[]>() && motionList.SafeGet<TextScenario.IMotion[]>(cnt) != null);
        Func<bool> func2 = (Func<bool>) (() => !expressionList.IsNullOrEmpty<TextScenario.IExpression[]>() && expressionList.SafeGet<TextScenario.IExpression[]>(cnt) != null);
        while (func1() || func2())
        {
          TextScenario.IMotion[] motion = func1() ? motionList[cnt] : (TextScenario.IMotion[]) null;
          TextScenario.IExpression[] expression = func2() ? expressionList[cnt] : (TextScenario.IExpression[]) null;
          this.playList.Add(new List<Action>()
          {
            (Action) (() => closure_0.Play(expression)),
            (Action) (() => closure_0.Play(motion))
          });
          ++cnt;
        }
        if (currentCharaData.voiceList.IsNullOrEmpty<TextScenario.IVoice[]>())
          return;
        this.onChange = (Action) (() => next.Play());
        scenario.VoicePlay(currentCharaData.voiceList, this.onChange, (Action) (() => next.voicePlayEnd = true));
      }

      private bool Play()
      {
        return this.playList.SafeProc<List<Action>>(this.currentNo++, (Action<List<Action>>) (p => p.ForEach((Action<Action>) (proc => proc()))));
      }

      public bool Process()
      {
        if (this.scenario.currentCharaData.isSkip || this.playList.Count <= this.currentNo && this.voicePlayEnd)
          return true;
        if (this.onChange == null)
        {
          List<TextScenario.IMotion[]> motionList = this.scenario.currentCharaData.motionList;
          bool flag1;
          if (this.currentNo == 0)
          {
            flag1 = true;
          }
          else
          {
            bool flag2 = false;
            if (!motionList.IsNullOrEmpty<TextScenario.IMotion[]>() && this.currentNo < motionList.Count)
              flag2 = !((IList<TextScenario.IMotion>) motionList[this.currentNo]).IsNullOrEmpty<TextScenario.IMotion>();
            TextScenario.IMotion[] list = !flag2 ? (TextScenario.IMotion[]) null : motionList[this.currentNo - 1];
            flag1 = ((IList<TextScenario.IMotion>) list).IsNullOrEmpty<TextScenario.IMotion>() || this.MotionEndCheck(list);
          }
          if (flag1 && !this.Play())
          {
            bool lastMotionEnd = true;
            bool flag2 = !motionList.IsNullOrEmpty<TextScenario.IMotion[]>();
            if (flag2)
              flag2 = motionList.LastOrDefault<TextScenario.IMotion[]>().SafeProc<TextScenario.IMotion[]>((Action<TextScenario.IMotion[]>) (last => lastMotionEnd = this.MotionEndCheck(last)));
            return !flag2 || lastMotionEnd;
          }
        }
        return false;
      }

      public void Result()
      {
        while (this.currentNo < this.playList.Count)
          this.Play();
      }

      private void Play(TextScenario.IMotion[] motionList)
      {
        if (((IList<TextScenario.IMotion>) motionList).IsNullOrEmpty<TextScenario.IMotion>())
          return;
        this.scenario.CrossFadeStart();
        foreach (TextScenario.IChara motion in motionList)
          motion.Play(this.scenario);
      }

      private void Play(TextScenario.IExpression[] expressionList)
      {
        if (((IList<TextScenario.IExpression>) expressionList).IsNullOrEmpty<TextScenario.IExpression>())
          return;
        foreach (TextScenario.IChara expression in expressionList)
          expression.Play(this.scenario);
      }

      private bool MotionEndCheck(TextScenario.IMotion[] list)
      {
        Func<ChaControl, bool> endCheck = (Func<ChaControl, bool>) (chaCtrl =>
        {
          AnimatorStateInfo animatorStateInfo = chaCtrl.getAnimatorStateInfo(0);
          return (double) ((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime() >= 1.0;
        });
        return ((IEnumerable<TextScenario.IMotion>) list).All<TextScenario.IMotion>((Func<TextScenario.IMotion, bool>) (motion => endCheck(motion.GetChara(this.scenario).chaCtrl)));
      }
    }
  }
}
