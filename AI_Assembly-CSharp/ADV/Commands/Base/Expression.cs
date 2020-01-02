// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Expression
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ADV.Commands.Base
{
  public class Expression : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[12]
        {
          "No",
          "眉",
          "目",
          "口",
          "眉開き",
          "目開き",
          "口開き",
          "視線",
          "頬赤",
          "ハイライト",
          "涙",
          "瞬き"
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

    public static List<Expression.Data> Convert(
      ref string[] args,
      TextScenario scenario)
    {
      List<Expression.Data> dataList = new List<Expression.Data>();
      if (args.Length > 1)
      {
        int cnt = 0;
        while (!args.IsNullOrEmpty(cnt))
        {
          string check = (string) null;
          args.SafeProc(cnt + 1, (Action<string>) (s => check = s));
          if (check != null)
          {
            int no = int.Parse(args[cnt]);
            Game.Expression expression1 = Game.GetExpression(scenario.commandController.expDic, check);
            if (expression1 != null)
            {
              dataList.Add(new Expression.Data(no, expression1));
              cnt += 2;
              continue;
            }
            CharaData chara = scenario.commandController.GetChara(no);
            ChaControl chaCtrl = chara.chaCtrl;
            int personality = 0;
            if (chara.data != null)
            {
              if (chara.data.agentData != null)
              {
                VoiceInfo.Param obj;
                if (Singleton<Manager.Voice>.Instance.voiceInfoDic.TryGetValue(chara.data.chaCtrl.fileParam.personality, out obj))
                  personality = obj.No;
              }
              else if (chara.data.merchantData != null)
              {
                VoiceInfo.Param obj;
                if (Singleton<Manager.Voice>.Instance.voiceInfoDic.TryGetValue(-90, out obj))
                  personality = obj.No;
              }
              else
              {
                VoiceInfo.Param obj;
                if (chara.data.playerData != null && Singleton<Manager.Voice>.Instance.voiceInfoDic.TryGetValue(-99, out obj))
                  personality = obj.No;
              }
            }
            if (Singleton<Game>.IsInstance())
            {
              Game.Expression expression2 = Singleton<Game>.Instance.GetExpression(personality, check);
              if (expression2 != null)
              {
                dataList.Add(new Expression.Data(no, expression2));
                cnt += 2;
                continue;
              }
            }
          }
          Expression.Data data = new Expression.Data(args, ref cnt);
          data.IsChangeSkip = true;
          dataList.Add(data);
        }
      }
      return dataList;
    }

    public override void Do()
    {
      base.Do();
      this.scenario.currentCharaData.CreateExpressionList();
      this.scenario.currentCharaData.expressionList.Add((TextScenario.IExpression[]) Expression.Convert(ref this.args, this.scenario).ToArray());
    }

    public class Data : Game.Expression, TextScenario.IExpression, TextScenario.IChara
    {
      public Data(string[] args, ref int cnt)
        : base(args, ref cnt)
      {
      }

      public Data(string[] args)
        : base(args)
      {
      }

      public Data(int no, Game.Expression src)
      {
        this.no = no;
        src.Copy((Game.Expression) this);
      }

      public int no { get; private set; }

      public override void Initialize(string[] args, ref int cnt, bool isThrow = false)
      {
        try
        {
          string[] strArray = args;
          int num;
          cnt = (num = cnt) + 1;
          int index = num;
          this.no = int.Parse(strArray[index]);
          base.Initialize(args, ref cnt, true);
        }
        catch (Exception ex)
        {
          if (isThrow)
            throw new Exception(string.Join(",", args));
          Debug.LogError((object) ("ADVExpression:" + string.Join(",", args)));
        }
      }

      public void Play(TextScenario scenario)
      {
        this.Change(this.GetChara(scenario).chaCtrl);
      }

      public CharaData GetChara(TextScenario scenario)
      {
        return scenario.commandController.GetChara(this.no);
      }
    }
  }
}
