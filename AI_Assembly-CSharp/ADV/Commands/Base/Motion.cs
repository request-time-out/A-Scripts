// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Motion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ADV.Commands.Base
{
  public class Motion : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[11]
        {
          "No",
          "State",
          "Bundle",
          "Asset",
          "IKBundle",
          "IKAsset",
          "ShakeBundle",
          "ShakeAsset",
          "OverrideBundle",
          "OverrideAsset",
          "LayerNo"
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

    public static List<Motion.Data> Convert(
      ref string[] args,
      TextScenario scenario,
      int argsLen)
    {
      List<Motion.Data> dataList = new List<Motion.Data>();
      if (args.Length > 1)
      {
        int cnt = 0;
        while (!args.IsNullOrEmpty(cnt))
        {
          string check = (string) null;
          args.SafeProc(cnt + 1, (Action<string>) (s => check = s));
          string[] strArray;
          if (check != null && scenario.commandController.motionDic.TryGetValue(check, out strArray))
          {
            string[] array = Enumerable.Repeat<string>(string.Empty, argsLen - 1).ToArray<string>();
            int num = Mathf.Min(array.Length, strArray.Length);
            for (int index = 0; index < num; ++index)
            {
              if (strArray[index] != null)
                array[index] = strArray[index];
            }
            args = ((IEnumerable<string>) args).Take<string>(cnt + 1).Concat<string>((IEnumerable<string>) array).Concat<string>(((IEnumerable<string>) args).Skip<string>(cnt + 2)).ToArray<string>();
          }
          dataList.Add(new Motion.Data(args, ref cnt));
        }
      }
      return dataList;
    }

    public override void Do()
    {
      base.Do();
      this.scenario.currentCharaData.CreateMotionList();
      this.scenario.currentCharaData.motionList.Add((TextScenario.IMotion[]) Motion.Convert(ref this.args, this.scenario, this.ArgsLabel.Length).ToArray());
    }

    public class Data : TextScenario.IMotion, TextScenario.IChara
    {
      public Data(string[] args, ref int cnt)
      {
        try
        {
          string[] strArray1 = args;
          int num1;
          cnt = (num1 = cnt) + 1;
          int index1 = num1;
          this.no = int.Parse(strArray1[index1]);
          string[] strArray2 = args;
          int num2;
          cnt = (num2 = cnt) + 1;
          int index2 = num2;
          this.stateName = strArray2[index2];
          string[] array1 = args;
          int num3;
          cnt = (num3 = cnt) + 1;
          int index3 = num3;
          this.assetBundleName = array1.SafeGet<string>(index3);
          string[] array2 = args;
          int num4;
          cnt = (num4 = cnt) + 1;
          int index4 = num4;
          this.assetName = array2.SafeGet<string>(index4);
          string[] array3 = args;
          int num5;
          cnt = (num5 = cnt) + 1;
          int index5 = num5;
          this.ikAssetBundleName = array3.SafeGet<string>(index5);
          string[] array4 = args;
          int num6;
          cnt = (num6 = cnt) + 1;
          int index6 = num6;
          this.ikAssetName = array4.SafeGet<string>(index6);
          string[] array5 = args;
          int num7;
          cnt = (num7 = cnt) + 1;
          int index7 = num7;
          this.shakeAssetBundleName = array5.SafeGet<string>(index7);
          string[] array6 = args;
          int num8;
          cnt = (num8 = cnt) + 1;
          int index8 = num8;
          this.shakeAssetName = array6.SafeGet<string>(index8);
          string[] array7 = args;
          int num9;
          cnt = (num9 = cnt) + 1;
          int index9 = num9;
          this.overrideAssetBundleName = array7.SafeGet<string>(index9);
          string[] array8 = args;
          int num10;
          cnt = (num10 = cnt) + 1;
          int index10 = num10;
          this.overrideAssetName = array8.SafeGet<string>(index10);
          string[] args1 = args;
          int num11;
          cnt = (num11 = cnt) + 1;
          int index11 = num11;
          Action<string> act = (Action<string>) (s =>
          {
            string[] strArray = s.Split(',');
            // ISSUE: reference to a compiler-generated field
            if (Motion.Data.\u003C\u003Ef__mg\u0024cache0 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Motion.Data.\u003C\u003Ef__mg\u0024cache0 = new Func<string, int>(int.Parse);
            }
            // ISSUE: reference to a compiler-generated field
            Func<string, int> fMgCache0 = Motion.Data.\u003C\u003Ef__mg\u0024cache0;
            this.layerNo = ((IEnumerable<string>) strArray).Select<string, int>(fMgCache0).ToArray<int>();
          });
          args1.SafeProc(index11, act);
          int result1;
          int result2;
          if (!(false | int.TryParse(this.stateName, out result1) | int.TryParse(this.assetBundleName, out result2)))
            return;
          this.pair = new PoseKeyPair?(new PoseKeyPair()
          {
            postureID = result1,
            poseID = result2
          });
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Motion:" + string.Join(",", args)));
        }
      }

      public Motion.Data[] Get()
      {
        string[] strArray = this.stateName.Split(',');
        if (strArray.Length == 1)
          return new Motion.Data[1]{ this };
        List<string[]> strArrayList = new List<string[]>();
        strArrayList.Add(strArray);
        strArrayList.Add(this.assetBundleName.Split(','));
        strArrayList.Add(this.assetName.Split(','));
        strArrayList.Add(this.ikAssetBundleName.Split(','));
        strArrayList.Add(this.ikAssetName.Split(','));
        strArrayList.Add(this.shakeAssetBundleName.Split(','));
        strArrayList.Add(this.shakeAssetName.Split(','));
        strArrayList.Add(this.overrideAssetBundleName.Split(','));
        strArrayList.Add(this.overrideAssetName.Split(','));
        List<string[]> row = new List<string[]>();
        for (int index = 0; index < strArray.Length; ++index)
        {
          List<string> stringList = new List<string>();
          stringList.Add(this.no.ToString());
          foreach (string[] array in strArrayList)
            stringList.Add(array.SafeGet<string>(index) ?? string.Empty);
          stringList.Add(string.Join(",", ((IEnumerable<int>) this.layerNo).Select<int, string>((Func<int, string>) (no => no.ToString())).ToArray<string>()));
          row.Add(stringList.ToArray());
        }
        return Enumerable.Range(0, strArray.Length).Select<int, Motion.Data>((Func<int, Motion.Data>) (i => new Motion.Data(row[i], ref i))).ToArray<Motion.Data>();
      }

      public int no { get; private set; }

      public string stateName { get; private set; }

      public string assetBundleName { get; private set; }

      public string assetName { get; private set; }

      public string ikAssetBundleName { get; private set; }

      public string ikAssetName { get; private set; }

      public string shakeAssetBundleName { get; private set; }

      public string shakeAssetName { get; private set; }

      public string overrideAssetBundleName { get; private set; }

      public string overrideAssetName { get; private set; }

      public int[] layerNo { get; private set; }

      public PoseKeyPair? pair { get; private set; }

      public void Play(TextScenario scenario)
      {
        this.GetChara(scenario).MotionPlay(this, false);
      }

      public CharaData GetChara(TextScenario scenario)
      {
        return scenario.commandController.GetChara(this.no);
      }
    }
  }
}
