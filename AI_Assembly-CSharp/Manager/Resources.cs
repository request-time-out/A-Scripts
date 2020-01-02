// Decompiled with JetBrains decompiler
// Type: Manager.Resources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using AIProject.Animal;
using AIProject.Animal.Resources;
using AIProject.Definitions;
using AIProject.MiniGames.Fishing;
using AIProject.SaveData;
using AIProject.Scene;
using AIProject.UI.Popup;
using Illusion.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEx;

namespace Manager
{
  public class Resources : Singleton<Manager.Resources>
  {
    private static readonly string[] _separators = new string[2]
    {
      "/",
      "／"
    };
    private static readonly char[] _separationKeywords = new char[3]
    {
      ',',
      '<',
      '>'
    };
    private static Regex _regex = new Regex("<((?:[\\w/.]*[,]*)+)>");
    private static readonly Dictionary<string, int> DicMapIKFrameID = new Dictionary<string, int>()
    {
      {
        "f_t_arm_L",
        0
      },
      {
        "f_t_elbo_L",
        1
      },
      {
        "f_t_arm_R",
        2
      },
      {
        "f_t_elbo_R",
        3
      },
      {
        "f_t_leg_L",
        4
      },
      {
        "f_t_knee_L",
        5
      },
      {
        "f_t_leg_R",
        6
      },
      {
        "f_t_knee_R",
        7
      }
    };
    public readonly Dictionary<string, MotionIKData> MapIKData = new Dictionary<string, MotionIKData>();
    private List<ValueTuple<string, string>> _loadedAssetBundles = new List<ValueTuple<string, string>>();
    private readonly string _mainManifestName = "abdata";
    private Dictionary<int, Rarelity> _recognizableShapeFilterTable = new Dictionary<int, Rarelity>();
    private Dictionary<int, Dictionary<int, Dictionary<int, float>>> _desireAddRateMultiTable = new Dictionary<int, Dictionary<int, Dictionary<int, float>>>();
    private Dictionary<int, ValueTuple<int, int>> _desireBorderTable = new Dictionary<int, ValueTuple<int, int>>();
    [SerializeField]
    private DefinePack _definePack;
    [SerializeField]
    private SoundPack _soundPack;
    [SerializeField]
    private LocomotionProfile _locomotionProfile;
    [SerializeField]
    private PlayerProfile _playerProfile;
    [SerializeField]
    private AgentProfile _agentProfile;
    [SerializeField]
    private StatusProfile _statusProfile;
    [SerializeField]
    private CommonDefine _commonDefine;
    [SerializeField]
    private MerchantProfile _merchantProfile;
    [SerializeField]
    private FishingDefinePack _fishingDefinePack;
    [SerializeField]
    private AnimalDefinePack _animalDefinePack;
    private IConnectableObservable<Unit> _loadMapResourceStream;

    private static void LoadActionAnimationInfo(
      ExcelData actionExcel,
      Dictionary<int, Dictionary<int, PlayState>> dic,
      bool awaitable)
    {
      if (Object.op_Equality((Object) actionExcel, (Object) null))
        return;
      for (int index1 = 1; index1 < actionExcel.MaxCell; ++index1)
      {
        ExcelData.Param obj = actionExcel.list[index1];
        if (!obj.list.IsNullOrEmpty<string>())
        {
          int num1 = 2;
          List<string> list1 = obj.list;
          int index2 = num1;
          int num2 = index2 + 1;
          string element1 = list1.GetElement<string>(index2);
          List<string> list2 = obj.list;
          int index3 = num2;
          int num3 = index3 + 1;
          string element2 = list2.GetElement<string>(index3);
          if (!element1.IsNullOrEmpty() && !element2.IsNullOrEmpty())
          {
            int result1;
            int result2;
            if (!int.TryParse(element1, out result1) || !int.TryParse(element2, out result2))
            {
              Debug.LogWarning((object) string.Format("セルの読み込み失敗: {0} {1}行目 Type = {2} Pose = {3}", (object) ((Object) actionExcel).get_name(), (object) index1, (object) element1, (object) element2));
            }
            else
            {
              int key = result1;
              List<string> list3 = obj.list;
              int index4 = num3;
              int num4 = index4 + 1;
              int result3;
              if (!int.TryParse(list3.GetElement<string>(index4), out result3))
                result3 = 0;
              List<string> list4 = obj.list;
              int index5 = num4;
              int num5 = index5 + 1;
              string element3 = list4.GetElement<string>(index5);
              List<string> list5 = obj.list;
              int index6 = num5;
              int num6 = index6 + 1;
              string element4 = list5.GetElement<string>(index6);
              List<string> list6 = obj.list;
              int index7 = num6;
              int num7 = index7 + 1;
              string[] strArray1 = list6.GetElement<string>(index7).Split(Manager.Resources._separators, StringSplitOptions.RemoveEmptyEntries);
              List<string> list7 = obj.list;
              int index8 = num7;
              int num8 = index8 + 1;
              bool result4;
              bool flag1 = bool.TryParse(list7.GetElement<string>(index8), out result4) && result4;
              List<string> list8 = obj.list;
              int index9 = num8;
              int num9 = index9 + 1;
              int result5;
              int num10 = !int.TryParse(list8.GetElement<string>(index9), out result5) ? 0 : result5;
              List<string> list9 = obj.list;
              int index10 = num9;
              int num11 = index10 + 1;
              int result6;
              int num12 = !int.TryParse(list9.GetElement<string>(index10), out result6) ? 0 : result6;
              List<string> list10 = obj.list;
              int index11 = num11;
              int num13 = index11 + 1;
              bool result7;
              bool flag2 = bool.TryParse(list10.GetElement<string>(index11), out result7) && result7;
              List<string> list11 = obj.list;
              int index12 = num13;
              int num14 = index12 + 1;
              float result8;
              float num15 = !float.TryParse(list11.GetElement<string>(index12), out result8) ? 0.0f : result8;
              List<string> list12 = obj.list;
              int index13 = num14;
              int num16 = index13 + 1;
              string[] strArray2 = list12.GetElement<string>(index13)?.Split(Manager.Resources._separators, StringSplitOptions.RemoveEmptyEntries);
              List<string> list13 = obj.list;
              int index14 = num16;
              int num17 = index14 + 1;
              float result9;
              float num18 = !float.TryParse(list13.GetElement<string>(index14), out result9) ? 1f : result9;
              List<string> list14 = obj.list;
              int index15 = num17;
              int num19 = index15 + 1;
              bool result10;
              bool flag3 = bool.TryParse(list14.GetElement<string>(index15), out result10) && result10;
              List<string> list15 = obj.list;
              int index16 = num19;
              int num20 = index16 + 1;
              float result11;
              float num21 = !float.TryParse(list15.GetElement<string>(index16), out result11) ? 0.0f : result11;
              List<string> list16 = obj.list;
              int index17 = num20;
              int num22 = index17 + 1;
              int result12;
              int randCount = !int.TryParse(list16.GetElement<string>(index17), out result12) ? 0 : result12;
              List<string> list17 = obj.list;
              int index18 = num22;
              int num23 = index18 + 1;
              int result13;
              int layer_ = !int.TryParse(list17.GetElement<string>(index18), out result13) ? 0 : result13;
              List<string> list18 = obj.list;
              int index19 = num23;
              int num24 = index19 + 1;
              bool result14;
              bool flag4 = bool.TryParse(list18.GetElement<string>(index19), out result14) && result14;
              List<string> list19 = obj.list;
              int index20 = num24;
              int num25 = index20 + 1;
              float result15;
              float num26 = !float.TryParse(list19.GetElement<string>(index20), out result15) ? 0.0f : result15;
              Dictionary<int, PlayState> dictionary;
              if (!dic.TryGetValue(key, out dictionary))
                dic[key] = dictionary = new Dictionary<int, PlayState>();
              List<string> list20 = obj.list;
              int index21 = num25;
              int num27 = index21 + 1;
              string element5 = list20.GetElement<string>(index21);
              List<ValueTuple<string, bool, int, bool>> toRelease = ListPool<ValueTuple<string, bool, int, bool>>.Get();
              MatchCollection matchCollection = Manager.Resources._regex.Matches(element5 ?? string.Empty);
              if (matchCollection.Count > 0)
              {
                for (int index22 = 0; index22 < matchCollection.Count; ++index22)
                {
                  Match match = matchCollection[index22];
                  for (int index23 = 0; index23 < match.Groups[1].Captures.Count; ++index23)
                  {
                    string[] strArray3 = match.Groups[1].Captures[index23].Value.Split(Manager.Resources._separationKeywords, StringSplitOptions.None);
                    int num28 = 0;
                    string[] source1 = strArray3;
                    int index24 = num28;
                    int num29 = index24 + 1;
                    string element6 = source1.GetElement<string>(index24);
                    string[] source2 = strArray3;
                    int index25 = num29;
                    int num30 = index25 + 1;
                    bool result16;
                    if (!bool.TryParse(source2.GetElement<string>(index25), out result16))
                      result16 = false;
                    string[] source3 = strArray3;
                    int index26 = num30;
                    int num31 = index26 + 1;
                    int result17;
                    if (!int.TryParse(source3.GetElement<string>(index26), out result17))
                      result17 = -1;
                    string[] source4 = strArray3;
                    int index27 = num31;
                    int num32 = index27 + 1;
                    bool result18;
                    if (!bool.TryParse(source4.GetElement<string>(index27), out result18))
                      result18 = false;
                    toRelease.Add(new ValueTuple<string, bool, int, bool>(element6, result16, result17, result18));
                  }
                }
              }
              List<PlayState.PlayStateInfo> playStateInfoList = ListPool<PlayState.PlayStateInfo>.Get();
              while (num27 < obj.list.Count)
              {
                string element6 = obj.list.GetElement<string>(num27++);
                if (!element6.IsNullOrEmpty())
                {
                  List<string> list21 = obj.list;
                  int index22 = num27;
                  int num28 = index22 + 1;
                  string element7 = list21.GetElement<string>(index22);
                  List<string> list22 = obj.list;
                  int index23 = num28;
                  int num29 = index23 + 1;
                  string[] strArray3 = list22.GetElement<string>(index23)?.Split(Manager.Resources._separators, StringSplitOptions.RemoveEmptyEntries);
                  List<string> list23 = obj.list;
                  int index24 = num29;
                  int num30 = index24 + 1;
                  bool result16;
                  bool flag5 = bool.TryParse(list23.GetElement<string>(index24), out result16) && result16;
                  List<string> list24 = obj.list;
                  int index25 = num30;
                  int num31 = index25 + 1;
                  int result17;
                  int num32 = !int.TryParse(list24.GetElement<string>(index25), out result17) ? 0 : result17;
                  List<string> list25 = obj.list;
                  int index26 = num31;
                  int num33 = index26 + 1;
                  int result18;
                  int num34 = !int.TryParse(list25.GetElement<string>(index26), out result18) ? 0 : result18;
                  List<string> list26 = obj.list;
                  int index27 = num33;
                  int num35 = index27 + 1;
                  bool result19;
                  bool flag6 = bool.TryParse(list26.GetElement<string>(index27), out result19) && result19;
                  List<string> list27 = obj.list;
                  int index28 = num35;
                  int num36 = index28 + 1;
                  float result20;
                  float num37 = !float.TryParse(list27.GetElement<string>(index28), out result20) ? 0.0f : result20;
                  List<string> list28 = obj.list;
                  int index29 = num36;
                  int num38 = index29 + 1;
                  string[] strArray4 = list28.GetElement<string>(index29)?.Split(Manager.Resources._separators, StringSplitOptions.RemoveEmptyEntries);
                  List<string> list29 = obj.list;
                  int index30 = num38;
                  int num39 = index30 + 1;
                  float result21;
                  float num40 = !float.TryParse(list29.GetElement<string>(index30), out result21) ? 1f : result21;
                  List<string> list30 = obj.list;
                  int index31 = num39;
                  int num41 = index31 + 1;
                  bool flag7 = bool.TryParse(list30.GetElement<string>(index31), out bool _);
                  List<string> list31 = obj.list;
                  int index32 = num41;
                  num27 = index32 + 1;
                  float result22;
                  float num42 = !float.TryParse(list31.GetElement<string>(index32), out result22) ? 0.0f : result22;
                  PlayState.PlayStateInfo playStateInfo = new PlayState.PlayStateInfo()
                  {
                    AssetBundleInfo = new AssetBundleInfo(string.Empty, element6, element7, string.Empty),
                    FadeOutTime = num40
                  };
                  playStateInfo.InStateInfo = new PlayState.AnimStateInfo();
                  if (!((IList<string>) strArray3).IsNullOrEmpty<string>())
                  {
                    PlayState.Info[] infoArray1 = new PlayState.Info[strArray3.Length];
                    playStateInfo.InStateInfo.StateInfos = infoArray1;
                    PlayState.Info[] infoArray2 = infoArray1;
                    for (int index33 = 0; index33 < infoArray2.Length; ++index33)
                      infoArray2[index33] = new PlayState.Info(strArray3[index33], layer_);
                  }
                  playStateInfo.OutStateInfo = new PlayState.AnimStateInfo();
                  if (!((IList<string>) strArray4).IsNullOrEmpty<string>())
                  {
                    PlayState.Info[] infoArray1 = new PlayState.Info[strArray4.Length];
                    playStateInfo.OutStateInfo.StateInfos = infoArray1;
                    PlayState.Info[] infoArray2 = infoArray1;
                    for (int index33 = 0; index33 < infoArray2.Length; ++index33)
                      infoArray2[index33] = new PlayState.Info(strArray4[index33], layer_);
                  }
                  playStateInfo.FadeOutTime = num40;
                  playStateInfo.InStateInfo.EnableFade = flag6;
                  playStateInfo.InStateInfo.FadeSecond = num37;
                  playStateInfo.OutStateInfo.EnableFade = flag7;
                  playStateInfo.OutStateInfo.FadeSecond = num42;
                  playStateInfo.IsLoop = flag5;
                  playStateInfo.LoopMin = num32;
                  playStateInfo.LoopMax = num34;
                  playStateInfoList.Add(playStateInfo);
                }
              }
              PlayState playState1 = new PlayState()
              {
                Layer = layer_,
                DirectionType = result3,
                EndEnableBlend = flag4,
                EndBlendRate = num26
              };
              dictionary[result2] = playState1;
              PlayState playState2 = playState1;
              playState2.MainStateInfo.AssetBundleInfo = new AssetBundleInfo(string.Empty, element3, element4, string.Empty);
              playState2.MainStateInfo.InStateInfo = new PlayState.AnimStateInfo();
              if (!((IList<string>) strArray1).IsNullOrEmpty<string>())
              {
                PlayState.Info[] infoArray1 = new PlayState.Info[strArray1.Length];
                playState2.MainStateInfo.InStateInfo.StateInfos = infoArray1;
                PlayState.Info[] infoArray2 = infoArray1;
                for (int index22 = 0; index22 < infoArray2.Length; ++index22)
                  infoArray2[index22] = new PlayState.Info(strArray1[index22], layer_);
              }
              playState2.MainStateInfo.OutStateInfo = new PlayState.AnimStateInfo();
              if (!((IList<string>) strArray2).IsNullOrEmpty<string>())
              {
                PlayState.Info[] infoArray1 = new PlayState.Info[strArray2.Length];
                playState2.MainStateInfo.OutStateInfo.StateInfos = infoArray1;
                PlayState.Info[] infoArray2 = infoArray1;
                for (int index22 = 0; index22 < infoArray2.Length; ++index22)
                  infoArray2[index22] = new PlayState.Info(strArray2[index22], layer_);
              }
              playState2.MainStateInfo.FadeOutTime = num18;
              playState2.MainStateInfo.InStateInfo.EnableFade = flag2;
              playState2.MainStateInfo.InStateInfo.FadeSecond = num15;
              playState2.MainStateInfo.OutStateInfo.EnableFade = flag3;
              playState2.MainStateInfo.OutStateInfo.FadeSecond = num21;
              playState2.MainStateInfo.IsLoop = flag1;
              playState2.MainStateInfo.LoopMin = num10;
              playState2.MainStateInfo.LoopMax = num12;
              foreach (PlayState.PlayStateInfo playStateInfo in playStateInfoList)
                playState2.SubStateInfos.Add(playStateInfo);
              using (List<ValueTuple<string, bool, int, bool>>.Enumerator enumerator = toRelease.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  ValueTuple<string, bool, int, bool> current = enumerator.Current;
                  playState2.AddItemInfo(new PlayState.ItemInfo()
                  {
                    parentName = (string) current.Item1,
                    fromEquipedItem = (bool) current.Item2,
                    itemID = (int) current.Item3,
                    isSync = (bool) current.Item4
                  });
                }
              }
              playState2.ActionInfo = new ActionInfo(!playStateInfoList.IsNullOrEmpty<PlayState.PlayStateInfo>(), randCount);
              ListPool<ValueTuple<string, bool, int, bool>>.Release(toRelease);
              ListPool<PlayState.PlayStateInfo>.Release(playStateInfoList);
            }
          }
        }
      }
    }

    public void LoadMapIK(DefinePack definePack)
    {
      List<ValueTuple<string, string>> AnimatorList = new List<ValueTuple<string, string>>();
      List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.MapIKList, false);
      nameListFromPath.Sort();
      for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(nameListFromPath[index1], System.IO.Path.GetFileNameWithoutExtension(nameListFromPath[index1]), string.Empty);
        if (!Object.op_Equality((Object) excelData, (Object) null))
        {
          int num1 = 1;
          while (num1 < excelData.MaxCell)
          {
            ExcelData.Param obj = excelData.list[num1++];
            int num2 = 0;
            List<ValueTuple<string, string>> valueTupleList = AnimatorList;
            List<string> list1 = obj.list;
            int index2 = num2;
            int num3 = index2 + 1;
            string str1 = list1[index2];
            List<string> list2 = obj.list;
            int index3 = num3;
            int num4 = index3 + 1;
            string str2 = list2[index3];
            ValueTuple<string, string> valueTuple = new ValueTuple<string, string>(str1, str2);
            valueTupleList.Add(valueTuple);
          }
        }
      }
      this.LoadMapIK(AnimatorList);
    }

    public void LoadMapIK(List<ValueTuple<string, string>> AnimatorList)
    {
      int length = ChaFileDefine.cf_bodyshapename.Length;
      float result1 = 0.0f;
      string strA = "f_t_";
      using (List<ValueTuple<string, string>>.Enumerator enumerator = AnimatorList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ValueTuple<string, string> current = enumerator.Current;
          if (GlobalMethod.AssetFileExist((string) current.Item1, (string) current.Item2, string.Empty))
          {
            ExcelData excelData = AssetUtility.LoadAsset<ExcelData>((string) current.Item1, (string) current.Item2, string.Empty);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              this.MapIKData.Add((string) current.Item2, new MotionIKData());
              List<MotionIKData.State> stateList = new List<MotionIKData.State>();
              int index1 = 0;
              int num1 = 1;
              bool flag1 = false;
              int index2 = -1;
              while (num1 < excelData.MaxCell)
              {
                ExcelData.Param obj1 = excelData.list[num1++];
                int index3 = 0;
                if (obj1.list == null || obj1.list.Count <= 0)
                {
                  if (index1 == 8)
                    index1 = 0;
                }
                else if (obj1.list[index3] == string.Empty)
                {
                  if (this.CheckMapIKBlankLine(obj1) && index1 == 8)
                    index1 = 0;
                }
                else
                {
                  bool flag2 = 0 == string.Compare(strA, 0, obj1.list[index3], 0, strA.Length);
                  if (num1 == 1 || index1 == 0 && !flag2)
                  {
                    index2 = stateList.Count;
                    stateList.Add(new MotionIKData.State());
                    flag1 = false;
                    stateList[index2].name = obj1.list[index3];
                    stateList[index2].frames = new MotionIKData.Frame[8];
                  }
                  else
                  {
                    int num2 = 1;
                    int num3;
                    if (!flag1)
                    {
                      if (index1 % 2 == 0)
                      {
                        MotionIKData.Param2 obj2 = stateList[index2].parts[index1 / 2].param2;
                        List<string> list = obj1.list;
                        int index4 = num2;
                        num3 = index4 + 1;
                        string str = list[index4];
                        obj2.target = str;
                      }
                      else
                      {
                        MotionIKData.Param3 obj2 = stateList[index2].parts[index1 / 2].param3;
                        List<string> list = obj1.list;
                        int index4 = num2;
                        num3 = index4 + 1;
                        string str = list[index4];
                        obj2.chein = str;
                      }
                      stateList[index2].frames[index1].editNo = 0;
                      stateList[index2].frames[index1].shapes = new MotionIKData.Shape[length];
                      for (int index4 = 0; index4 < length; ++index4)
                      {
                        int num4 = index4;
                        stateList[index2].frames[index1].shapes[index4].shapeNo = num4;
                      }
                    }
                    else
                      num3 = num2 + 1;
                    int result2 = 0;
                    List<string> list1 = obj1.list;
                    int index5 = num3;
                    int num5 = index5 + 1;
                    MotionIKData.BlendWeightInfo blendWeightInfo;
                    int num6;
                    if (!int.TryParse(list1[index5], out result2))
                    {
                      if (!flag1)
                      {
                        if (index1 % 2 == 0)
                        {
                          if (!float.TryParse(obj1.list[15], out result1))
                            result1 = 1f;
                          stateList[index2].parts[index1 / 2].param2.weightPos = result1;
                          if (!float.TryParse(obj1.list[16], out result1))
                            result1 = 1f;
                          stateList[index2].parts[index1 / 2].param2.weightAng = result1;
                        }
                        else
                        {
                          if (!float.TryParse(obj1.list[15], out result1))
                            result1 = 1f;
                          stateList[index2].parts[index1 / 2].param3.weight = result1;
                        }
                        int result3 = 0;
                        float result4 = -1f;
                        float result5 = -1f;
                        int num4 = 17;
                        if (index1 % 2 == 0)
                        {
                          List<string> list2 = obj1.list;
                          int index4 = num4;
                          int num7 = index4 + 1;
                          if (!int.TryParse(list2[index4], out result3))
                            result3 = 0;
                          blendWeightInfo.pattern = result3;
                          List<string> list3 = obj1.list;
                          int index6 = num7;
                          int num8 = index6 + 1;
                          if (!float.TryParse(list3[index6], out result4))
                            result4 = 0.0f;
                          float result6 = Mathf.Clamp01(result4);
                          blendWeightInfo.StartKey = result6;
                          List<string> list4 = obj1.list;
                          int index7 = num8;
                          int num9 = index7 + 1;
                          if (!float.TryParse(list4[index7], out result5))
                            result5 = 1f;
                          float result7 = Mathf.Clamp01(result5);
                          blendWeightInfo.EndKey = result7;
                          blendWeightInfo.shape = new MotionIKData.Shape();
                          stateList[index2].parts[index1 / 2].param2.blendInfos[0].Add(blendWeightInfo);
                          List<string> list5 = obj1.list;
                          int index8 = num9;
                          int num10 = index8 + 1;
                          if (!int.TryParse(list5[index8], out result3))
                            result3 = 0;
                          blendWeightInfo.pattern = result3;
                          List<string> list6 = obj1.list;
                          int index9 = num10;
                          int num11 = index9 + 1;
                          if (!float.TryParse(list6[index9], out result6))
                            result6 = 0.0f;
                          float num12 = Mathf.Clamp01(result6);
                          blendWeightInfo.StartKey = num12;
                          List<string> list7 = obj1.list;
                          int index10 = num11;
                          num6 = index10 + 1;
                          if (!float.TryParse(list7[index10], out result7))
                            result7 = 1f;
                          float num13 = Mathf.Clamp01(result7);
                          blendWeightInfo.EndKey = num13;
                          stateList[index2].parts[index1 / 2].param2.blendInfos[1].Add(blendWeightInfo);
                        }
                        else
                        {
                          List<string> list2 = obj1.list;
                          int index4 = num4;
                          int num7 = index4 + 1;
                          if (!int.TryParse(list2[index4], out result3))
                            result3 = 0;
                          blendWeightInfo.pattern = result3;
                          List<string> list3 = obj1.list;
                          int index6 = num7;
                          int num8 = index6 + 1;
                          if (!float.TryParse(list3[index6], out result4))
                            result4 = 0.0f;
                          float num9 = Mathf.Clamp01(result4);
                          blendWeightInfo.StartKey = num9;
                          List<string> list4 = obj1.list;
                          int index7 = num8;
                          int num10 = index7 + 1;
                          if (!float.TryParse(list4[index7], out result5))
                            result5 = 1f;
                          float num11 = Mathf.Clamp01(result5);
                          blendWeightInfo.EndKey = num11;
                          blendWeightInfo.shape = new MotionIKData.Shape();
                          stateList[index2].parts[index1 / 2].param3.blendInfos.Add(blendWeightInfo);
                          num6 = num10 + 3;
                        }
                        stateList[index2].frames[index1].frameNo = index1++;
                        flag1 = index1 == 8;
                      }
                    }
                    else
                    {
                      float[,,] numArray = new float[2, 2, 3];
                      List<string> list2 = obj1.list;
                      int index4 = num5;
                      int num4 = index4 + 1;
                      if (!float.TryParse(list2[index4], out numArray.Address(0, 0, 0)))
                        numArray[0, 0, 0] = 0.0f;
                      List<string> list3 = obj1.list;
                      int index6 = num4;
                      int num7 = index6 + 1;
                      if (!float.TryParse(list3[index6], out numArray.Address(0, 0, 1)))
                        numArray[0, 0, 1] = 0.0f;
                      List<string> list4 = obj1.list;
                      int index7 = num7;
                      int num8 = index7 + 1;
                      if (!float.TryParse(list4[index7], out numArray.Address(0, 0, 2)))
                        numArray[0, 0, 2] = 0.0f;
                      List<string> list5 = obj1.list;
                      int index8 = num8;
                      int num9 = index8 + 1;
                      if (!float.TryParse(list5[index8], out numArray.Address(0, 1, 0)))
                        numArray[0, 1, 0] = 0.0f;
                      List<string> list6 = obj1.list;
                      int index9 = num9;
                      int num10 = index9 + 1;
                      if (!float.TryParse(list6[index9], out numArray.Address(0, 1, 1)))
                        numArray[0, 1, 1] = 0.0f;
                      List<string> list7 = obj1.list;
                      int index10 = num10;
                      int num11 = index10 + 1;
                      if (!float.TryParse(list7[index10], out numArray.Address(0, 1, 2)))
                        numArray[0, 1, 2] = 0.0f;
                      List<string> list8 = obj1.list;
                      int index11 = num11;
                      int num12 = index11 + 1;
                      if (!float.TryParse(list8[index11], out numArray.Address(1, 0, 0)))
                        numArray[1, 0, 0] = 0.0f;
                      List<string> list9 = obj1.list;
                      int index12 = num12;
                      int num13 = index12 + 1;
                      if (!float.TryParse(list9[index12], out numArray.Address(1, 0, 1)))
                        numArray[1, 0, 1] = 0.0f;
                      List<string> list10 = obj1.list;
                      int index13 = num13;
                      int num14 = index13 + 1;
                      if (!float.TryParse(list10[index13], out numArray.Address(1, 0, 2)))
                        numArray[1, 0, 2] = 0.0f;
                      List<string> list11 = obj1.list;
                      int index14 = num14;
                      int num15 = index14 + 1;
                      if (!float.TryParse(list11[index14], out numArray.Address(1, 1, 0)))
                        numArray[1, 1, 0] = 0.0f;
                      List<string> list12 = obj1.list;
                      int index15 = num15;
                      int num16 = index15 + 1;
                      if (!float.TryParse(list12[index15], out numArray.Address(1, 1, 1)))
                        numArray[1, 1, 1] = 0.0f;
                      List<string> list13 = obj1.list;
                      int index16 = num16;
                      int num17 = index16 + 1;
                      if (!float.TryParse(list13[index16], out numArray.Address(1, 1, 2)))
                        numArray[1, 1, 2] = 0.0f;
                      int num18;
                      if (!flag1)
                      {
                        if (index1 % 2 == 0)
                        {
                          List<string> list14 = obj1.list;
                          int index17 = num17;
                          int num19 = index17 + 1;
                          if (!float.TryParse(list14[index17], out result1))
                            result1 = 1f;
                          stateList[index2].parts[index1 / 2].param2.weightPos = result1;
                          List<string> list15 = obj1.list;
                          int index18 = num19;
                          num18 = index18 + 1;
                          if (!float.TryParse(list15[index18], out result1))
                            result1 = 1f;
                          stateList[index2].parts[index1 / 2].param2.weightAng = result1;
                        }
                        else
                        {
                          List<string> list14 = obj1.list;
                          int index17 = num17;
                          int num19 = index17 + 1;
                          if (!float.TryParse(list14[index17], out result1))
                            result1 = 1f;
                          stateList[index2].parts[index1 / 2].param3.weight = result1;
                          num18 = num19 + 1;
                        }
                      }
                      else
                        num18 = num17 + 2;
                      int result3 = 0;
                      float result4 = -1f;
                      float result5 = -1f;
                      int num20 = flag1 ? Manager.Resources.DicMapIKFrameID[obj1.list[0]] : index1;
                      if (num20 % 2 == 0)
                      {
                        List<string> list14 = obj1.list;
                        int index17 = num18;
                        int num19 = index17 + 1;
                        if (!int.TryParse(list14[index17], out result3))
                          result3 = 0;
                        blendWeightInfo.pattern = result3;
                        List<string> list15 = obj1.list;
                        int index18 = num19;
                        int num21 = index18 + 1;
                        if (!float.TryParse(list15[index18], out result4))
                          result4 = 0.0f;
                        float result6 = Mathf.Clamp01(result4);
                        blendWeightInfo.StartKey = result6;
                        List<string> list16 = obj1.list;
                        int index19 = num21;
                        int num22 = index19 + 1;
                        if (!float.TryParse(list16[index19], out result5))
                          result5 = 1f;
                        float result7 = Mathf.Clamp01(result5);
                        blendWeightInfo.EndKey = result7;
                        blendWeightInfo.shape = new MotionIKData.Shape();
                        blendWeightInfo.shape.shapeNo = result2;
                        blendWeightInfo.shape.small.pos.x = (__Null) (double) numArray[0, 0, 0];
                        blendWeightInfo.shape.small.pos.y = (__Null) (double) numArray[0, 0, 1];
                        blendWeightInfo.shape.small.pos.z = (__Null) (double) numArray[0, 0, 2];
                        blendWeightInfo.shape.large.pos.x = (__Null) (double) numArray[1, 0, 0];
                        blendWeightInfo.shape.large.pos.y = (__Null) (double) numArray[1, 0, 1];
                        blendWeightInfo.shape.large.pos.z = (__Null) (double) numArray[1, 0, 2];
                        stateList[index2].parts[num20 / 2].param2.blendInfos[0].Add(blendWeightInfo);
                        List<string> list17 = obj1.list;
                        int index20 = num22;
                        int num23 = index20 + 1;
                        if (!int.TryParse(list17[index20], out result3))
                          result3 = 0;
                        blendWeightInfo.pattern = result3;
                        List<string> list18 = obj1.list;
                        int index21 = num23;
                        int num24 = index21 + 1;
                        if (!float.TryParse(list18[index21], out result6))
                          result6 = 0.0f;
                        float num25 = Mathf.Clamp01(result6);
                        blendWeightInfo.StartKey = num25;
                        List<string> list19 = obj1.list;
                        int index22 = num24;
                        num6 = index22 + 1;
                        if (!float.TryParse(list19[index22], out result7))
                          result7 = 1f;
                        float num26 = Mathf.Clamp01(result7);
                        blendWeightInfo.EndKey = num26;
                        blendWeightInfo.shape = new MotionIKData.Shape();
                        blendWeightInfo.shape.shapeNo = result2;
                        blendWeightInfo.shape.small.ang.x = (__Null) (double) numArray[0, 1, 0];
                        blendWeightInfo.shape.small.ang.y = (__Null) (double) numArray[0, 1, 1];
                        blendWeightInfo.shape.small.ang.z = (__Null) (double) numArray[0, 1, 2];
                        blendWeightInfo.shape.large.ang.x = (__Null) (double) numArray[1, 1, 0];
                        blendWeightInfo.shape.large.ang.y = (__Null) (double) numArray[1, 1, 1];
                        blendWeightInfo.shape.large.ang.z = (__Null) (double) numArray[1, 1, 2];
                        stateList[index2].parts[num20 / 2].param2.blendInfos[1].Add(blendWeightInfo);
                      }
                      else
                      {
                        List<string> list14 = obj1.list;
                        int index17 = num18;
                        int num19 = index17 + 1;
                        if (!int.TryParse(list14[index17], out result3))
                          result3 = 0;
                        blendWeightInfo.pattern = result3;
                        List<string> list15 = obj1.list;
                        int index18 = num19;
                        int num21 = index18 + 1;
                        if (!float.TryParse(list15[index18], out result4))
                          result4 = 0.0f;
                        float num22 = Mathf.Clamp01(result4);
                        blendWeightInfo.StartKey = num22;
                        List<string> list16 = obj1.list;
                        int index19 = num21;
                        int num23 = index19 + 1;
                        if (!float.TryParse(list16[index19], out result5))
                          result5 = 1f;
                        float num24 = Mathf.Clamp01(result5);
                        blendWeightInfo.EndKey = num24;
                        blendWeightInfo.shape = new MotionIKData.Shape();
                        blendWeightInfo.shape.shapeNo = result2;
                        blendWeightInfo.shape.small.pos.x = (__Null) (double) numArray[0, 0, 0];
                        blendWeightInfo.shape.small.pos.y = (__Null) (double) numArray[0, 0, 1];
                        blendWeightInfo.shape.small.pos.z = (__Null) (double) numArray[0, 0, 2];
                        blendWeightInfo.shape.small.ang.x = (__Null) (double) numArray[0, 1, 0];
                        blendWeightInfo.shape.small.ang.y = (__Null) (double) numArray[0, 1, 1];
                        blendWeightInfo.shape.small.ang.z = (__Null) (double) numArray[0, 1, 2];
                        blendWeightInfo.shape.large.pos.x = (__Null) (double) numArray[1, 0, 0];
                        blendWeightInfo.shape.large.pos.y = (__Null) (double) numArray[1, 0, 1];
                        blendWeightInfo.shape.large.pos.z = (__Null) (double) numArray[1, 0, 2];
                        blendWeightInfo.shape.large.ang.x = (__Null) (double) numArray[1, 1, 0];
                        blendWeightInfo.shape.large.ang.y = (__Null) (double) numArray[1, 1, 1];
                        blendWeightInfo.shape.large.ang.z = (__Null) (double) numArray[1, 1, 2];
                        stateList[index2].parts[num20 / 2].param3.blendInfos.Add(blendWeightInfo);
                        num6 = num23 + 3;
                      }
                      if (!flag1)
                      {
                        stateList[index2].frames[index1].frameNo = index1++;
                        flag1 = index1 == 8;
                      }
                    }
                  }
                }
              }
              this.MapIKData[(string) current.Item2].states = new MotionIKData.State[stateList.Count];
              for (int index3 = 0; index3 < stateList.Count; ++index3)
                this.MapIKData[(string) current.Item2].states[index3] = stateList[index3];
            }
          }
        }
      }
    }

    private bool CheckMapIKBlankLine(ExcelData.Param param)
    {
      for (int index = 1; index < param.list.Count; ++index)
      {
        if (param.list[index] != string.Empty)
          return false;
      }
      return true;
    }

    public static Dictionary<string, int> StatusTagTable { get; } = new Dictionary<string, int>()
    {
      ["体温"] = 0,
      ["機嫌"] = 1,
      ["満腹"] = 2,
      ["体調"] = 3,
      ["生命"] = 4,
      ["やる気"] = 5,
      ["H"] = 6,
      ["善悪"] = 7,
      ["女子力"] = 10,
      ["信頼"] = 11,
      ["人間性"] = 12,
      ["本能"] = 13,
      ["変態"] = 14,
      ["警戒"] = 15,
      ["闇"] = 16,
      ["社交"] = 17,
      ["トイレ"] = 100,
      ["風呂"] = 101,
      ["睡眠"] = 102,
      ["食事"] = 103,
      ["休憩"] = 104,
      ["ギフト"] = 105,
      ["おねだり"] = 106,
      ["寂しい"] = 107,
      ["H欲"] = 108,
      ["採取"] = 110,
      ["遊び"] = 111,
      ["料理"] = 112,
      ["動物"] = 113,
      ["ロケ"] = 114,
      ["飲み物"] = 115
    };

    public DefinePack DefinePack
    {
      get
      {
        return this._definePack;
      }
    }

    public SoundPack SoundPack
    {
      get
      {
        return this._soundPack;
      }
    }

    public LocomotionProfile LocomotionProfile
    {
      get
      {
        return this._locomotionProfile;
      }
    }

    public PlayerProfile PlayerProfile
    {
      get
      {
        return this._playerProfile;
      }
    }

    public AgentProfile AgentProfile
    {
      get
      {
        return this._agentProfile;
      }
    }

    public StatusProfile StatusProfile
    {
      get
      {
        return this._statusProfile;
      }
    }

    public CommonDefine CommonDefine
    {
      get
      {
        return this._commonDefine;
      }
    }

    public MerchantProfile MerchantProfile
    {
      get
      {
        return this._merchantProfile;
      }
    }

    public FishingDefinePack FishingDefinePack
    {
      get
      {
        return this._fishingDefinePack;
      }
    }

    public AnimalDefinePack AnimalDefinePack
    {
      get
      {
        return this._animalDefinePack;
      }
    }

    public ChaFileCoordinate BathDefaultCoord { get; private set; }

    public bool LoadAssetBundle { get; set; }

    public void BeginLoadAssetBundle()
    {
      this._loadedAssetBundles.Clear();
    }

    public void AddLoadAssetBundle(string assetBundleName, string manifestName)
    {
      if (manifestName.IsNullOrEmpty())
        manifestName = this._mainManifestName;
      if (this._loadedAssetBundles.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == assetBundleName && (string) x.Item2 == manifestName)))
        return;
      this._loadedAssetBundles.Add(new ValueTuple<string, string>(assetBundleName, manifestName));
    }

    public void EndLoadAssetBundle(bool forceRemove = false)
    {
      using (List<ValueTuple<string, string>>.Enumerator enumerator = this._loadedAssetBundles.GetEnumerator())
      {
        while (enumerator.MoveNext())
          AssetBundleManager.UnloadAssetBundle((string) enumerator.Current.Item1, true, (string) null, forceRemove);
      }
      UnityEngine.Resources.UnloadUnusedAssets();
      GC.Collect();
      this._loadedAssetBundles.Clear();
    }

    public Manager.Resources.AnimationTables Animation { get; private set; } = new Manager.Resources.AnimationTables();

    public Manager.Resources.ActionTable Action { get; private set; } = new Manager.Resources.ActionTable();

    public Manager.Resources.BehaviorTreeTables BehaviorTree { get; private set; } = new Manager.Resources.BehaviorTreeTables();

    public Manager.Resources.HSceneTables HSceneTable { get; private set; } = new Manager.Resources.HSceneTables();

    public Manager.Resources.ItemIconTables itemIconTables { get; private set; } = new Manager.Resources.ItemIconTables();

    public Dictionary<int, int> DefaultRequiredExpTablePrimal { get; private set; } = new Dictionary<int, int>();

    public Manager.Resources.FishingTable Fishing { get; private set; } = new Manager.Resources.FishingTable();

    public Manager.Resources.AnimalTables AnimalTable { get; private set; } = new Manager.Resources.AnimalTables();

    public Manager.Resources.PopupInfoTable PopupInfo { get; private set; } = new Manager.Resources.PopupInfoTable();

    public Manager.Resources.SoundTable Sound { get; private set; } = new Manager.Resources.SoundTable();

    public bool IsRecognizable(int lv, Rarelity rarelity)
    {
      return this._recognizableShapeFilterTable[lv].Contains(rarelity);
    }

    public Dictionary<int, Dictionary<int, List<Vector3>>> WaypointDataList { get; private set; } = new Dictionary<int, Dictionary<int, List<Vector3>>>();

    public Dictionary<int, float> GetDesireAddRateTable(int personalID, AIProject.TimeZone zone)
    {
      int key = AIProject.Definitions.Environment.TimeZoneIDTable.get_Item(zone);
      Dictionary<int, Dictionary<int, float>> dictionary1;
      if (!this._desireAddRateMultiTable.TryGetValue(personalID, out dictionary1))
        return (Dictionary<int, float>) null;
      Dictionary<int, float> dictionary2;
      if (dictionary1.TryGetValue(key, out dictionary2))
        return dictionary2;
      Debug.Log((object) string.Format("<color=yellow>存在しないテーブルキー</color>: <color=red>{0}</color>", (object) zone));
      return (Dictionary<int, float>) null;
    }

    public ValueTuple<int, int> GetDesireBorder(int key)
    {
      ValueTuple<int, int> valueTuple;
      return !this._desireBorderTable.TryGetValue(key, out valueTuple) ? (ValueTuple<int, int>) null : valueTuple;
    }

    public Dictionary<int, string> FeatureNameTable { get; private set; }

    public Dictionary<int, ValueTuple<int, int>> FeatureParameterTable { get; private set; }

    public Manager.Resources.GameInfoTables GameInfo { get; private set; } = new Manager.Resources.GameInfoTables();

    public Manager.Resources.MapTables Map { get; private set; } = new Manager.Resources.MapTables();

    public IObservable<Unit> LoadMapResourceStream
    {
      get
      {
        return (IObservable<Unit>) this._loadMapResourceStream;
      }
    }

    protected override void Awake()
    {
      if (this.CheckInstance())
        ;
    }

    public void PreSetup()
    {
    }

    public void SetupMap()
    {
      this._loadMapResourceStream = (IConnectableObservable<Unit>) Observable.PublishLast<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.LoadMapResources()), false));
      this._loadMapResourceStream.Connect();
    }

    [DebuggerHidden]
    public IEnumerator LoadMapResources()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Manager.Resources.\u003CLoadMapResources\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void ReleaseMapResources()
    {
      this.Animation.Release();
      this.Action.Release();
      this.BehaviorTree.Release();
      this.Fishing.Release();
      this.AnimalTable.Release();
      this.PopupInfo.Release();
      this.Map.Release();
      this.Sound.Release();
      this.itemIconTables.Release();
      this.GameInfo.Release();
      this.WaypointDataList.Clear();
      this._desireAddRateMultiTable.Clear();
      this._desireBorderTable.Clear();
      this.HSceneTable.Release();
      GC.Collect();
      Debug.Log((object) "Complete: Manager.Resources.Release Process");
    }

    private void LoadWaypointData()
    {
      List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(this._definePack.ABDirectories.WaypointList, false);
      nameListFromPath.Sort();
      foreach (string assetBundleName in nameListFromPath)
      {
        foreach (NavMeshWayPointData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (NavMeshWayPointData), (string) null).GetAllAssets<NavMeshWayPointData>())
        {
          Dictionary<int, List<Vector3>> dictionary1;
          if (!this.WaypointDataList.TryGetValue(allAsset.MapID, out dictionary1))
          {
            Dictionary<int, List<Vector3>> dictionary2 = new Dictionary<int, List<Vector3>>();
            this.WaypointDataList[allAsset.MapID] = dictionary2;
            dictionary1 = dictionary2;
          }
          List<Vector3> vector3List1;
          if (!dictionary1.TryGetValue(allAsset.AreaID, out vector3List1))
          {
            List<Vector3> vector3List2 = new List<Vector3>();
            dictionary1[allAsset.AreaID] = vector3List2;
            vector3List1 = vector3List2;
          }
          vector3List1.AddRange((IEnumerable<Vector3>) allAsset.Points);
        }
      }
    }

    [DebuggerHidden]
    private IEnumerator LoadExperience()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Manager.Resources.\u003CLoadExperience\u003Ec__Iterator1 experienceCIterator1 = new Manager.Resources.\u003CLoadExperience\u003Ec__Iterator1();
      return (IEnumerator) experienceCIterator1;
    }

    private void LoadDesireInfo()
    {
      List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(this._definePack.ABDirectories.AgentDesire, false);
      nameListFromPath.Sort();
      foreach (string assetBundleName in nameListFromPath)
      {
        foreach (DesireRateData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (DesireRateData), (string) null).GetAllAssets<DesireRateData>())
        {
          foreach (DesireRateData.Param obj in allAsset.param)
          {
            Dictionary<int, Dictionary<int, float>> dictionary1;
            if (!this._desireAddRateMultiTable.TryGetValue(obj.PersonalID, out dictionary1))
            {
              Dictionary<int, Dictionary<int, float>> dictionary2 = new Dictionary<int, Dictionary<int, float>>();
              this._desireAddRateMultiTable[obj.PersonalID] = dictionary2;
              dictionary1 = dictionary2;
            }
            Dictionary<int, float> dictionary3;
            if (!dictionary1.TryGetValue(0, out dictionary3))
            {
              Dictionary<int, float> dictionary2 = new Dictionary<int, float>();
              dictionary1[0] = dictionary2;
              dictionary3 = dictionary2;
            }
            Dictionary<int, float> dictionary4;
            if (!dictionary1.TryGetValue(1, out dictionary4))
            {
              Dictionary<int, float> dictionary2 = new Dictionary<int, float>();
              dictionary1[1] = dictionary2;
              dictionary4 = dictionary2;
            }
            Dictionary<int, float> dictionary5;
            if (!dictionary1.TryGetValue(2, out dictionary5))
            {
              Dictionary<int, float> dictionary2 = new Dictionary<int, float>();
              dictionary1[2] = dictionary2;
              dictionary5 = dictionary2;
            }
            dictionary3[obj.ID] = obj.Morning;
            dictionary4[obj.ID] = obj.Day;
            dictionary5[obj.ID] = obj.Night;
          }
        }
        foreach (DesireBorderData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (DesireBorderData), (string) null).GetAllAssets<DesireBorderData>())
        {
          foreach (DesireBorderData.Param obj in allAsset.param)
            this._desireBorderTable[obj.ID] = new ValueTuple<int, int>(obj.Border, obj.Limit);
        }
        AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
      }
    }

    public class ActionTable
    {
      private static readonly string[] _separators = new string[2]
      {
        "/",
        "／"
      };
      private static readonly int[] _bustIndexes = new int[7]
      {
        2,
        3,
        4,
        5,
        6,
        7,
        8
      };
      private static readonly char[] _splitChars = new char[1]
      {
        ','
      };

      public Dictionary<int, Dictionary<int, int>> PhaseExp { get; private set; } = new Dictionary<int, Dictionary<int, int>>();

      public Dictionary<int, Dictionary<int, Threshold>> PersonalityMotivation { get; private set; } = new Dictionary<int, Dictionary<int, Threshold>>();

      public Dictionary<int, Dictionary<int, int>> LifestyleTable { get; private set; } = new Dictionary<int, Dictionary<int, int>>();

      public Dictionary<int, Dictionary<int, ObtainItemInfo>> FlavorPickSkillTable { get; private set; } = new Dictionary<int, Dictionary<int, ObtainItemInfo>>();

      public Dictionary<int, Dictionary<int, ObtainItemInfo>> FlavorPickHSkillTable { get; private set; } = new Dictionary<int, Dictionary<int, ObtainItemInfo>>();

      public Dictionary<int, Dictionary<int, ActAnimFlagData>> AgentActionFlagTable { get; private set; } = new Dictionary<int, Dictionary<int, ActAnimFlagData>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, ParameterPacket>>> ActionStatusResultTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, ParameterPacket>>>();

      public Dictionary<int, Dictionary<int, ParameterPacket>> SituationStatusResultTable { get; private set; } = new Dictionary<int, Dictionary<int, ParameterPacket>>();

      public Dictionary<int, Dictionary<int, string>> ActionExpressionTable { get; private set; } = new Dictionary<int, Dictionary<int, string>>();

      public Dictionary<int, Dictionary<int, List<ExpressionKeyframe>>> ActionExpressionKeyframeTable { get; private set; } = new Dictionary<int, Dictionary<int, List<ExpressionKeyframe>>>();

      public Dictionary<string, Dictionary<int, YureCtrl.Info>> ActionYureTable { get; private set; } = new Dictionary<string, Dictionary<int, YureCtrl.Info>>();

      public Dictionary<int, int> AgentLocomotionBreathTable { get; private set; } = new Dictionary<int, int>();

      public Dictionary<int, ABInfoData.Param> ComCameraList { get; private set; } = new Dictionary<int, ABInfoData.Param>();

      public Dictionary<int, Dictionary<int, ByproductInfo>> ByproductList { get; private set; } = new Dictionary<int, Dictionary<int, ByproductInfo>>();

      public void Load(DefinePack definePack)
      {
        this.LoadPhaseExp(definePack);
        this.LoadPersonalityMotivation(definePack);
        this.LoadLifestyleTable(definePack);
        this.LoadFlavorPickSkillTable(definePack);
        this.LoadFlavorPickHSkillTable(definePack);
        this.LoadActionTalkFlags(definePack);
        this.LoadLocmotionBreathTable(definePack);
        this.LoadActionExpressionTable(definePack);
        this.LoadActionExpressionKeyFrameTable(definePack);
        this.LoadActionBustCtrlTable(definePack);
        this.LoadActionResultTable(definePack);
        this.LoadSituationResultTable(definePack);
        this.LoadComCemra(definePack);
        this.LoadByproductList(definePack);
      }

      public void Release()
      {
        this.PhaseExp.Clear();
        this.PersonalityMotivation.Clear();
        this.LifestyleTable.Clear();
        this.FlavorPickSkillTable.Clear();
        this.FlavorPickHSkillTable.Clear();
        this.AgentActionFlagTable.Clear();
        this.ActionStatusResultTable.Clear();
        this.SituationStatusResultTable.Clear();
        this.ActionExpressionTable.Clear();
        this.ActionExpressionKeyframeTable.Clear();
        this.ActionYureTable.Clear();
        this.AgentLocomotionBreathTable.Clear();
        this.ComCameraList.Clear();
        this.ByproductList.Clear();
      }

      private void LoadPhaseExp(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.AgentPhase, false);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (PhaseExpData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (PhaseExpData), (string) null).GetAllAssets<PhaseExpData>())
          {
            foreach (PhaseExpData.Param obj in allAsset.param)
            {
              Dictionary<int, int> dictionary1;
              if (!this.PhaseExp.TryGetValue(obj.Personality, out dictionary1))
              {
                Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
                this.PhaseExp[obj.Personality] = dictionary2;
                dictionary1 = dictionary2;
              }
              for (int index = 0; index < obj.ExpArray.Count; ++index)
              {
                string exp = obj.ExpArray[index];
                if (!exp.IsNullOrEmpty())
                {
                  int result;
                  int num = !int.TryParse(exp, out result) ? 0 : result;
                  dictionary1[index] = num;
                }
              }
            }
          }
        }
      }

      private void LoadPersonalityMotivation(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.AgentPersonalityMotivation, false);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (ExcelData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (ExcelData), (string) null).GetAllAssets<ExcelData>())
          {
            int result1;
            if (int.TryParse(((Object) allAsset).get_name(), out result1))
            {
              Dictionary<int, Threshold> dictionary1;
              if (!this.PersonalityMotivation.TryGetValue(result1, out dictionary1))
              {
                Dictionary<int, Threshold> dictionary2 = new Dictionary<int, Threshold>();
                this.PersonalityMotivation[result1] = dictionary2;
                dictionary1 = dictionary2;
              }
              foreach (ExcelData.Param obj in allAsset.list)
              {
                int num1 = 0;
                List<string> list1 = obj.list;
                int index1 = num1;
                int num2 = index1 + 1;
                int result2;
                if (int.TryParse(list1.GetElement<string>(index1), out result2))
                {
                  List<string> list2 = obj.list;
                  int index2 = num2;
                  int num3 = index2 + 1;
                  float result3;
                  if (float.TryParse(list2.GetElement<string>(index2), out result3))
                  {
                    List<string> list3 = obj.list;
                    int index3 = num3;
                    int num4 = index3 + 1;
                    float result4;
                    if (float.TryParse(list3.GetElement<string>(index3), out result4))
                      dictionary1[result2] = new Threshold(result3, result4);
                  }
                }
              }
            }
          }
        }
      }

      private void LoadLifestyleTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.LifestyleTable, false);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (ExcelData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (ExcelData), (string) null).GetAllAssets<ExcelData>())
          {
            foreach (ExcelData.Param obj in allAsset.list)
            {
              int num1 = 0;
              List<string> list = obj.list;
              int index = num1;
              int num2 = index + 1;
              int result1;
              if (int.TryParse(list.GetElement<string>(index), out result1))
              {
                Dictionary<int, int> dictionary1;
                if (!this.LifestyleTable.TryGetValue(result1, out dictionary1))
                {
                  Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
                  this.LifestyleTable[result1] = dictionary2;
                  dictionary1 = dictionary2;
                }
                int num3 = num2 + 1;
                int num4 = 0;
                while (num3 < obj.list.Count)
                {
                  int result2;
                  if (!int.TryParse(obj.list.GetElement<string>(num3++), out result2))
                    ++num4;
                  else
                    dictionary1[num4++] = result2;
                }
              }
            }
          }
        }
      }

      private void LoadFlavorPickSkillTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.FlavorPickSkillTable, false);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (SkillObtainData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (SkillObtainData), (string) null).GetAllAssets<SkillObtainData>())
          {
            int result;
            if (int.TryParse(((Object) allAsset).get_name(), out result))
            {
              Dictionary<int, ObtainItemInfo> dictionary1;
              if (!this.FlavorPickSkillTable.TryGetValue(result, out dictionary1))
              {
                Dictionary<int, ObtainItemInfo> dictionary2 = new Dictionary<int, ObtainItemInfo>();
                this.FlavorPickSkillTable[result] = dictionary2;
                dictionary1 = dictionary2;
              }
              foreach (SkillObtainData.Param obj in allAsset.param)
                dictionary1[obj.ID] = new ObtainItemInfo()
                {
                  Name = obj.Name,
                  Rate = obj.Rate,
                  Info = new AIProject.ItemInfo()
                  {
                    CategoryID = obj.Category,
                    ItemID = obj.ItemID
                  }
                };
            }
          }
        }
      }

      private void LoadFlavorPickHSkillTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.FlavorPickHSkillTable, false);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (SkillObtainData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (SkillObtainData), (string) null).GetAllAssets<SkillObtainData>())
          {
            int result;
            if (int.TryParse(((Object) allAsset).get_name(), out result))
            {
              Dictionary<int, ObtainItemInfo> dictionary1;
              if (!this.FlavorPickHSkillTable.TryGetValue(result, out dictionary1))
              {
                Dictionary<int, ObtainItemInfo> dictionary2 = new Dictionary<int, ObtainItemInfo>();
                this.FlavorPickHSkillTable[result] = dictionary2;
                dictionary1 = dictionary2;
              }
              foreach (SkillObtainData.Param obj in allAsset.param)
                dictionary1[obj.ID] = new ObtainItemInfo()
                {
                  Name = obj.Name,
                  Rate = obj.Rate,
                  Info = new AIProject.ItemInfo()
                  {
                    CategoryID = obj.Category,
                    ItemID = obj.ItemID
                  }
                };
            }
          }
        }
      }

      public void LoadActionTalkFlags(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.AgentCommunicationFlags, false);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (ActionTalkData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (ActionTalkData), (string) null).GetAllAssets<ActionTalkData>())
          {
            foreach (ActionTalkData.Param obj in allAsset.param)
            {
              Dictionary<int, ActAnimFlagData> dictionary1;
              if (!this.AgentActionFlagTable.TryGetValue(obj.ActionID, out dictionary1))
              {
                Dictionary<int, ActAnimFlagData> dictionary2 = new Dictionary<int, ActAnimFlagData>();
                this.AgentActionFlagTable[obj.ActionID] = dictionary2;
                dictionary1 = dictionary2;
              }
              dictionary1[obj.PoseID] = new ActAnimFlagData()
              {
                obstacleRadius = obj.ObstacleRadius,
                useNeckLook = obj.useNeckLook,
                canTalk = obj.CanTalk,
                attitudeID = obj.TalkAttitudeID,
                canHCommand = obj.CanHCommand,
                isBadMood = obj.IsBadMood,
                isSpecial = obj.IsSpecial,
                hPositionID1 = obj.HPositionID,
                hPositionID2 = obj.HPositionSubID
              };
            }
            Singleton<Manager.Resources>.Instance.AddLoadAssetBundle(assetBundleName, string.Empty);
          }
        }
      }

      public void LoadActionResultTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.AgentActionResult, false);
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = CommonLib.LoadAsset<ExcelData>(str, withoutExtension, false, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            Singleton<Manager.Resources>.Instance.AddLoadAssetBundle(str, string.Empty);
            int num1 = 0;
            while (num1 < excelData.MaxCell)
            {
              ExcelData.Param obj = excelData.list[num1++];
              int num2 = 2;
              List<string> list1 = obj.list;
              int index2 = num2;
              int num3 = index2 + 1;
              int result1;
              if (int.TryParse(list1.GetElement<string>(index2), out result1))
              {
                List<string> list2 = obj.list;
                int index3 = num3;
                int num4 = index3 + 1;
                int result2;
                if (int.TryParse(list2.GetElement<string>(index3), out result2))
                {
                  List<string> list3 = obj.list;
                  int index4 = num4;
                  int num5 = index4 + 1;
                  int result3;
                  if (int.TryParse(list3.GetElement<string>(index4), out result3))
                  {
                    List<string> list4 = obj.list;
                    int index5 = num5;
                    int num6 = index5 + 1;
                    int result4;
                    int num7 = !int.TryParse(list4.GetElement<string>(index5), out result4) ? 0 : result4;
                    ParameterPacket parameterPacket = new ParameterPacket()
                    {
                      Probability = (float) num7
                    };
                    while (num6 < obj.list.Count)
                    {
                      List<string> list5 = obj.list;
                      int index6 = num6;
                      int num8 = index6 + 1;
                      string element1 = list5.GetElement<string>(index6);
                      List<string> list6 = obj.list;
                      int index7 = num8;
                      int num9 = index7 + 1;
                      string element2 = list6.GetElement<string>(index7);
                      List<string> list7 = obj.list;
                      int index8 = num9;
                      int num10 = index8 + 1;
                      string element3 = list7.GetElement<string>(index8);
                      List<string> list8 = obj.list;
                      int index9 = num10;
                      num6 = index9 + 1;
                      string element4 = list8.GetElement<string>(index9);
                      if (!element1.IsNullOrEmpty())
                      {
                        int index10;
                        if (!Manager.Resources.StatusTagTable.TryGetValue(element1, out index10))
                        {
                          Debug.LogWarning((object) string.Format("タグ読み取りエラー: 値={0}", (object) element1));
                        }
                        else
                        {
                          int s = !int.TryParse(element2, out result4) ? 0 : result4;
                          int m = !int.TryParse(element3, out result4) ? 0 : result4;
                          int l = !int.TryParse(element4, out result4) ? 0 : result4;
                          parameterPacket.Parameters[index10] = new TriThreshold(s, m, l);
                        }
                      }
                    }
                    Dictionary<int, Dictionary<int, ParameterPacket>> dictionary1;
                    if (!this.ActionStatusResultTable.TryGetValue(result1, out dictionary1))
                    {
                      Dictionary<int, Dictionary<int, ParameterPacket>> dictionary2 = new Dictionary<int, Dictionary<int, ParameterPacket>>();
                      this.ActionStatusResultTable[result1] = dictionary2;
                      dictionary1 = dictionary2;
                    }
                    Dictionary<int, ParameterPacket> dictionary3;
                    if (!dictionary1.TryGetValue(result2, out dictionary3))
                    {
                      Dictionary<int, ParameterPacket> dictionary2 = new Dictionary<int, ParameterPacket>();
                      dictionary1[result2] = dictionary2;
                      dictionary3 = dictionary2;
                    }
                    dictionary3[result3] = parameterPacket;
                  }
                }
              }
            }
          }
        }
      }

      private void LoadLocmotionBreathTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.AgentLocomotionBreath, false);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (LocomotionBreathData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (LocomotionBreathData), (string) null).GetAllAssets<LocomotionBreathData>())
          {
            foreach (LocomotionBreathData.Param obj in allAsset.param)
            {
              if (!obj.State.IsNullOrEmpty())
                this.AgentLocomotionBreathTable[Animator.StringToHash(obj.State)] = obj.VoiceID;
            }
          }
        }
      }

      public void LoadActionExpressionTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.ActionExpList, false);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (MotionExpressionData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (MotionExpressionData), (string) null).GetAllAssets<MotionExpressionData>())
          {
            int key = int.Parse(((Object) allAsset).get_name().Replace("c", string.Empty));
            Dictionary<int, string> dictionary1;
            if (!this.ActionExpressionTable.TryGetValue(key, out dictionary1))
            {
              Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
              this.ActionExpressionTable[key] = dictionary2;
              dictionary1 = dictionary2;
            }
            foreach (MotionExpressionData.Param obj in allAsset.param)
            {
              if (!obj.State.IsNullOrEmpty())
              {
                int hash = Animator.StringToHash(obj.State);
                dictionary1[hash] = obj.ExpressionName;
              }
            }
          }
          Singleton<Manager.Resources>.Instance.AddLoadAssetBundle(assetBundleName, string.Empty);
        }
      }

      public void LoadActionExpressionKeyFrameTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.ActionExpKeyFrameList, false);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (ExcelData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (ExcelData), (string) null).GetAllAssets<ExcelData>())
          {
            int key = int.Parse(((Object) allAsset).get_name().Replace("c", string.Empty));
            Dictionary<int, List<ExpressionKeyframe>> dictionary1;
            if (!this.ActionExpressionKeyframeTable.TryGetValue(key, out dictionary1))
            {
              Dictionary<int, List<ExpressionKeyframe>> dictionary2 = new Dictionary<int, List<ExpressionKeyframe>>();
              this.ActionExpressionKeyframeTable[key] = dictionary2;
              dictionary1 = dictionary2;
            }
            foreach (ExcelData.Param obj in allAsset.list)
            {
              int num1 = 1;
              List<string> list1 = obj.list;
              int index1 = num1;
              int num2 = index1 + 1;
              string element1 = list1.GetElement<string>(index1);
              if (!element1.IsNullOrEmpty())
              {
                int hash = Animator.StringToHash(element1);
                List<ExpressionKeyframe> expressionKeyframeList1;
                if (!dictionary1.TryGetValue(hash, out expressionKeyframeList1))
                {
                  List<ExpressionKeyframe> expressionKeyframeList2 = new List<ExpressionKeyframe>();
                  dictionary1[hash] = expressionKeyframeList2;
                  expressionKeyframeList1 = expressionKeyframeList2;
                }
                while (num2 < obj.list.Count)
                {
                  List<string> list2 = obj.list;
                  int index2 = num2;
                  int num3 = index2 + 1;
                  string element2 = list2.GetElement<string>(index2);
                  List<string> list3 = obj.list;
                  int index3 = num3;
                  num2 = index3 + 1;
                  string element3 = list3.GetElement<string>(index3);
                  float result;
                  if (float.TryParse(element2, out result))
                    expressionKeyframeList1.Add(new ExpressionKeyframe()
                    {
                      normalizedTime = result,
                      expressionName = element3
                    });
                }
              }
            }
          }
        }
      }

      public void LoadActionBustCtrlTable(DefinePack definePack)
      {
        if (definePack.ABDirectories.ActionBustCtrlList.IsNullOrEmpty())
          return;
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.ActionBustCtrlList, false);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (MotionBustCtrlData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (MotionBustCtrlData), (string) null).GetAllAssets<MotionBustCtrlData>())
          {
            Dictionary<int, YureCtrl.Info> dictionary1;
            if (!this.ActionYureTable.TryGetValue(((Object) allAsset).get_name(), out dictionary1))
            {
              Dictionary<int, YureCtrl.Info> dictionary2 = new Dictionary<int, YureCtrl.Info>();
              this.ActionYureTable[((Object) allAsset).get_name()] = dictionary2;
              dictionary1 = dictionary2;
            }
            foreach (MotionBustCtrlData.Param obj in allAsset.param)
            {
              if (!obj.State.IsNullOrEmpty())
              {
                int hash = Animator.StringToHash(obj.State);
                YureCtrl.Info info1 = new YureCtrl.Info();
                dictionary1[hash] = info1;
                YureCtrl.Info info2 = info1;
                int num1 = 0;
                bool[] aIsActive1 = info2.aIsActive;
                List<string> parameters1 = obj.Parameters;
                int index1 = num1;
                int num2 = index1 + 1;
                int num3 = parameters1.GetElement<string>(index1) == "1" ? 1 : 0;
                aIsActive1[0] = num3 != 0;
                info2.aBreastShape[0].MemberInit();
                for (int index2 = 0; index2 < Manager.Resources.ActionTable._bustIndexes.Length; ++index2)
                  info2.aBreastShape[0].breast[index2] = obj.Parameters.GetElement<string>(num2++) == "1";
                ref YureCtrl.BreastShapeInfo local1 = ref info2.aBreastShape[0];
                List<string> parameters2 = obj.Parameters;
                int index3 = num2;
                int num4 = index3 + 1;
                int num5 = parameters2.GetElement<string>(index3) == "1" ? 1 : 0;
                local1.nip = num5 != 0;
                bool[] aIsActive2 = info2.aIsActive;
                List<string> parameters3 = obj.Parameters;
                int index4 = num4;
                int num6 = index4 + 1;
                int num7 = parameters3.GetElement<string>(index4) == "1" ? 1 : 0;
                aIsActive2[1] = num7 != 0;
                info2.aBreastShape[1].MemberInit();
                for (int index2 = 0; index2 < Manager.Resources.ActionTable._bustIndexes.Length; ++index2)
                  info2.aBreastShape[1].breast[index2] = obj.Parameters.GetElement<string>(num6++) == "1";
                ref YureCtrl.BreastShapeInfo local2 = ref info2.aBreastShape[1];
                List<string> parameters4 = obj.Parameters;
                int index5 = num6;
                int num8 = index5 + 1;
                int num9 = parameters4.GetElement<string>(index5) == "1" ? 1 : 0;
                local2.nip = num9 != 0;
                bool[] aIsActive3 = info2.aIsActive;
                List<string> parameters5 = obj.Parameters;
                int index6 = num8;
                int num10 = index6 + 1;
                int num11 = parameters5.GetElement<string>(index6) == "1" ? 1 : 0;
                aIsActive3[2] = num11 != 0;
                bool[] aIsActive4 = info2.aIsActive;
                List<string> parameters6 = obj.Parameters;
                int index7 = num10;
                int num12 = index7 + 1;
                int num13 = parameters6.GetElement<string>(index7) == "1" ? 1 : 0;
                aIsActive4[3] = num13 != 0;
              }
            }
          }
          Singleton<Manager.Resources>.Instance.AddLoadAssetBundle(assetBundleName, string.Empty);
        }
      }

      public void LoadSituationResultTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.AgentSituationResult, false);
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            Singleton<Manager.Resources>.Instance.AddLoadAssetBundle(str, string.Empty);
            int num1 = 0;
            while (num1 < excelData.MaxCell)
            {
              ExcelData.Param obj = excelData.list[num1++];
              int num2 = 1;
              List<string> list1 = obj.list;
              int index2 = num2;
              int num3 = index2 + 1;
              int result1;
              if (int.TryParse(list1.GetElement<string>(index2), out result1))
              {
                List<string> list2 = obj.list;
                int index3 = num3;
                int num4 = index3 + 1;
                int result2;
                if (int.TryParse(list2.GetElement<string>(index3), out result2))
                {
                  List<string> list3 = obj.list;
                  int index4 = num4;
                  int num5 = index4 + 1;
                  int result3;
                  int num6 = !int.TryParse(list3.GetElement<string>(index4), out result3) ? 0 : result3;
                  ParameterPacket parameterPacket = new ParameterPacket()
                  {
                    Probability = (float) num6
                  };
                  while (num5 < obj.list.Count)
                  {
                    List<string> list4 = obj.list;
                    int index5 = num5;
                    int num7 = index5 + 1;
                    string element1 = list4.GetElement<string>(index5);
                    List<string> list5 = obj.list;
                    int index6 = num7;
                    int num8 = index6 + 1;
                    string element2 = list5.GetElement<string>(index6);
                    List<string> list6 = obj.list;
                    int index7 = num8;
                    int num9 = index7 + 1;
                    string element3 = list6.GetElement<string>(index7);
                    List<string> list7 = obj.list;
                    int index8 = num9;
                    num5 = index8 + 1;
                    string element4 = list7.GetElement<string>(index8);
                    if (!element1.IsNullOrEmpty())
                    {
                      int index9;
                      if (!Manager.Resources.StatusTagTable.TryGetValue(element1, out index9))
                      {
                        Debug.LogWarning((object) string.Format("タグ読み取りエラー：値={0}", (object) element1));
                      }
                      else
                      {
                        int s = !int.TryParse(element2, out result3) ? 0 : result3;
                        int m = !int.TryParse(element3, out result3) ? 0 : result3;
                        int l = !int.TryParse(element4, out result3) ? 0 : result3;
                        parameterPacket.Parameters[index9] = new TriThreshold(s, m, l);
                      }
                    }
                  }
                  Dictionary<int, ParameterPacket> dictionary1;
                  if (!this.SituationStatusResultTable.TryGetValue(result1, out dictionary1))
                  {
                    Dictionary<int, ParameterPacket> dictionary2 = new Dictionary<int, ParameterPacket>();
                    this.SituationStatusResultTable[result1] = dictionary2;
                    dictionary1 = dictionary2;
                  }
                  dictionary1[result2] = parameterPacket;
                }
              }
            }
          }
        }
      }

      private void LoadComCemra(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.ComCamera, false);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (ABInfoData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (ABInfoData), (string) null).GetAllAssets<ABInfoData>())
          {
            foreach (ABInfoData.Param obj in allAsset.param)
              this.ComCameraList[obj.ID] = obj;
          }
        }
      }

      private void LoadByproductList(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.ActionByproductList, false);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (ActByproductData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (ActByproductData), (string) null).GetAllAssets<ActByproductData>())
          {
            foreach (ActByproductData.Param obj in allAsset.param)
            {
              Dictionary<int, ByproductInfo> dictionary1;
              if (!this.ByproductList.TryGetValue(obj.ActionID, out dictionary1))
              {
                Dictionary<int, ByproductInfo> dictionary2 = new Dictionary<int, ByproductInfo>();
                this.ByproductList[obj.ActionID] = dictionary2;
                dictionary1 = dictionary2;
              }
              if (!obj.ItemList.IsNullOrEmpty<string>())
              {
                List<List<int>> intListList = new List<List<int>>();
                foreach (string self in obj.ItemList)
                {
                  if (!self.IsNullOrEmpty())
                  {
                    string[] strArray = self.Split(Manager.Resources.ActionTable._splitChars, StringSplitOptions.RemoveEmptyEntries);
                    List<int> intList = new List<int>();
                    foreach (string s in strArray)
                    {
                      int result;
                      if (int.TryParse(s, out result))
                        intList.Add(result);
                    }
                    intListList.Add(intList);
                  }
                }
                dictionary1[obj.PoseID] = new ByproductInfo()
                {
                  ItemList = intListList
                };
              }
            }
          }
        }
      }
    }

    public class AnimalTables
    {
      public readonly Dictionary<int, ActionTypes> AnimalActionIDTable = new Dictionary<int, ActionTypes>()
      {
        {
          0,
          ActionTypes.None
        },
        {
          1,
          ActionTypes.Rest
        }
      };
      public readonly Dictionary<int, DesireType> AnimalDesireIDTable = new Dictionary<int, DesireType>()
      {
        {
          -1,
          DesireType.None
        },
        {
          0,
          DesireType.Sleepiness
        },
        {
          1,
          DesireType.Loneliness
        },
        {
          2,
          DesireType.Action
        }
      };
      public readonly Dictionary<int, ChangeType> ChangeTypeIDTable = new Dictionary<int, ChangeType>()
      {
        {
          0,
          ChangeType.Add
        },
        {
          1,
          ChangeType.Sub
        },
        {
          2,
          ChangeType.Cng
        }
      };
      private string[] separators = new string[4]
      {
        "/",
        "／",
        ",",
        "、"
      };
      private const int AssetBundleNameIndex = 1;
      private const int AssetNameIndex = 2;
      private const int ManifestNameIndex = 3;

      public void Load(AnimalDefinePack _animalDefinePack)
      {
        this.LoadInfo(_animalDefinePack);
        this.LoadAction(_animalDefinePack);
        this.LoadLook(_animalDefinePack);
        this.LoadState(_animalDefinePack);
        this.LoadDesire(_animalDefinePack);
        this.LoadWithActor(_animalDefinePack);
        this.LoadPlayerInfo(_animalDefinePack);
        this.LoadAnimalPoint(_animalDefinePack);
      }

      public void Release()
      {
        this.PetItemInfoTable.Clear();
        this.PetHomeUIInfoTable.Clear();
        this.ExpressionTable.Clear();
        this.TextureTable.Clear();
        this.CommonAnimeTable.Clear();
        this.WithAgentAnimeTable.Clear();
        this.AnimatorTable.Clear();
        this.TempAnimatorTable.Clear();
        this.ActionInfoTable.Clear();
        this.ModelInfoTable.Clear();
        this.DesirePriorityTable.Clear();
        this.DesireSpanTable.Clear();
        this.DesireBorderTable.Clear();
        this.DesireRateTable.Clear();
        this.DesireTargetStateTable.Clear();
        this.DesireResultTable.Clear();
        this.StateConditionTable.Clear();
        this.StateTargetActionTable.Clear();
        this.LookStateTable.Clear();
        this.PlayerCatchAnimalAnimationTable.Clear();
        this.PlayerCatchAnimalPoseTable.Clear();
        this.AnimalPointAssetTable.Clear();
        this.AnimalBaseObjInfoTable.Clear();
      }

      public Dictionary<int, List<ValueTuple<ItemIDKeyPair, int>>> PetItemInfoTable { get; set; } = new Dictionary<int, List<ValueTuple<ItemIDKeyPair, int>>>();

      public Dictionary<int, ValueTuple<int, List<string>>> PetHomeUIInfoTable { get; private set; } = new Dictionary<int, ValueTuple<int, List<string>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<string, List<ValueTuple<string, int, int>>>>>> ExpressionTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<string, List<ValueTuple<string, int, int>>>>>>();

      public Dictionary<int, Dictionary<int, ValueTuple<Texture2D, Color[]>>> TextureTable { get; private set; } = new Dictionary<int, Dictionary<int, ValueTuple<Texture2D, Color[]>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, AnimalPlayState>>> CommonAnimeTable { get; set; } = new Dictionary<int, Dictionary<int, Dictionary<int, AnimalPlayState>>>();

      public Dictionary<int, Dictionary<int, AnimalPlayState>> WithAgentAnimeTable { get; set; } = new Dictionary<int, Dictionary<int, AnimalPlayState>>();

      public Dictionary<int, Dictionary<int, RuntimeAnimatorController>> AnimatorTable { get; set; } = new Dictionary<int, Dictionary<int, RuntimeAnimatorController>>();

      protected Dictionary<string, Dictionary<string, RuntimeAnimatorController>> TempAnimatorTable { get; set; } = new Dictionary<string, Dictionary<string, RuntimeAnimatorController>>();

      public Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<AnimalState, AnimalActionInfo>>> ActionInfoTable { get; set; } = new Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<AnimalState, AnimalActionInfo>>>();

      public Dictionary<int, Dictionary<int, AnimalModelInfo>> ModelInfoTable { get; set; } = new Dictionary<int, Dictionary<int, AnimalModelInfo>>();

      public Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<int, DesireType>>> DesirePriorityTable { get; set; } = new Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<int, DesireType>>>();

      public Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<DesireType, int>>> DesireSpanTable { get; set; } = new Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<DesireType, int>>>();

      public Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<DesireType, Tuple<float, float>>>> DesireBorderTable { get; set; } = new Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<DesireType, Tuple<float, float>>>>();

      public Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<AnimalState, Dictionary<DesireType, float>>>> DesireRateTable { get; set; } = new Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<AnimalState, Dictionary<DesireType, float>>>>();

      public Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<DesireType, List<AnimalState>>>> DesireTargetStateTable { get; set; } = new Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<DesireType, List<AnimalState>>>>();

      public Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<DesireType, Dictionary<bool, Dictionary<DesireType, ChangeParamState>>>>> DesireResultTable { get; set; } = new Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<DesireType, Dictionary<bool, Dictionary<DesireType, ChangeParamState>>>>>();

      public Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<AnimalState, StateCondition>>> StateConditionTable { get; set; } = new Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<AnimalState, StateCondition>>>();

      public Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<AnimalState, List<ActionTypes>>>> StateTargetActionTable { get; set; } = new Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<AnimalState, List<ActionTypes>>>>();

      public Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<AnimalState, LookState>>> LookStateTable { get; set; } = new Dictionary<AnimalTypes, Dictionary<BreedingTypes, Dictionary<AnimalState, LookState>>>();

      public Dictionary<int, Dictionary<int, PlayState>> PlayerCatchAnimalAnimationTable { get; set; } = new Dictionary<int, Dictionary<int, PlayState>>();

      public Dictionary<int, Dictionary<int, PoseKeyPair>> PlayerCatchAnimalPoseTable { get; set; } = new Dictionary<int, Dictionary<int, PoseKeyPair>>();

      public Dictionary<int, Dictionary<int, AssetBundleInfo>> AnimalPointAssetTable { get; set; } = new Dictionary<int, Dictionary<int, AssetBundleInfo>>();

      public Dictionary<int, Dictionary<int, AssetBundleInfo>> AnimalBaseObjInfoTable { get; private set; } = new Dictionary<int, Dictionary<int, AssetBundleInfo>>();

      private string AssetStr(AssetBundleInfo _info)
      {
        return string.Format("AssetBundleName[{0}] AssetName[{1}] ManifestName[{2}]", (object) _info.assetbundle, (object) _info.asset, (object) _info.manifest);
      }

      private string AssetStrWithName(AssetBundleInfo _info)
      {
        return string.Format("Name[{0}] AssetBundleName[{1}] AssetName[{2}] ManifestName[{3}]", (object) _info.name, (object) _info.assetbundle, (object) _info.asset, (object) _info.manifest);
      }

      private RuntimeAnimatorController LoadAnimator(
        string _assetBundleName,
        string _assetName,
        string _manifestName = "")
      {
        RuntimeAnimatorController animatorController = (RuntimeAnimatorController) null;
        if (!this.TempAnimatorTable.ContainsKey(_assetBundleName))
          this.TempAnimatorTable[_assetBundleName] = new Dictionary<string, RuntimeAnimatorController>();
        if (!this.TempAnimatorTable[_assetBundleName].TryGetValue(_assetName, out animatorController))
        {
          animatorController = AssetUtility.LoadAsset<RuntimeAnimatorController>(_assetBundleName, _assetName, _manifestName);
          this.TempAnimatorTable[_assetBundleName][_assetName] = animatorController;
        }
        return animatorController;
      }

      private RuntimeAnimatorController LoadAnimator(AssetBundleInfo _info)
      {
        return this.LoadAnimator((string) _info.assetbundle, (string) _info.asset, (string) _info.manifest);
      }

      private string GetDirectory(string _directory, string _animalName)
      {
        return _directory + _animalName + "/";
      }

      private string GetDirectory(string _directory, string _animalName, string _breedingName)
      {
        return string.Format("{0}{1}/{2}/", (object) _directory, (object) _animalName, (object) _breedingName);
      }

      private AssetBundleInfo GetAssetInfo(
        List<string> _address,
        ref int _idx,
        bool _addSummary)
      {
        string str1;
        if (_addSummary)
        {
          List<string> source = _address;
          int num;
          _idx = (num = _idx) + 1;
          int index = num;
          str1 = source.GetElement<string>(index) ?? string.Empty;
        }
        else
          str1 = string.Empty;
        string str2 = str1;
        List<string> source1 = _address;
        int num1;
        _idx = (num1 = _idx) + 1;
        int index1 = num1;
        string str3 = source1.GetElement<string>(index1) ?? string.Empty;
        List<string> source2 = _address;
        int num2;
        _idx = (num2 = _idx) + 1;
        int index2 = num2;
        string str4 = source2.GetElement<string>(index2) ?? string.Empty;
        List<string> source3 = _address;
        int num3;
        _idx = (num3 = _idx) + 1;
        int index3 = num3;
        string str5 = source3.GetElement<string>(index3) ?? string.Empty;
        return new AssetBundleInfo(str2, str3, str4, str5);
      }

      private void LoadInfo(AnimalDefinePack _animalDefinePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(_animalDefinePack.AssetBundleNames.AnimalInfoDirectory, false);
        if (((IReadOnlyList<string>) nameListFromPath).IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          if (AssetBundleCheck.IsFile(str, withoutExtension))
          {
            ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
              {
                List<string> list = excelData.list[index2].list;
                if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
                {
                  int num = 0;
                  List<string> source = list;
                  int index3 = num;
                  int _idx = index3 + 1;
                  int result;
                  if (int.TryParse(source.GetElement<string>(index3) ?? string.Empty, out result))
                  {
                    AssetBundleInfo assetInfo = this.GetAssetInfo(list, ref _idx, true);
                    switch (result)
                    {
                      case 0:
                        this.LoadPetItemInfoList(assetInfo);
                        continue;
                      case 1:
                        this.LoadPetHomeUIInfoList(assetInfo);
                        continue;
                      case 2:
                        this.LoadAnimatorInfo(assetInfo);
                        continue;
                      case 3:
                        this.LoadAnimStateInfo(assetInfo);
                        continue;
                      case 4:
                        this.LoadModelInfo(assetInfo);
                        continue;
                      case 5:
                        this.LoadExpressionList(assetInfo);
                        continue;
                      case 6:
                        this.LoadAnimalTextureList(assetInfo);
                        continue;
                      case 7:
                        this.LoadAnimalBaseObjList(assetInfo);
                        continue;
                      default:
                        continue;
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadPetItemInfoList(AssetBundleInfo _excelAdress)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_excelAdress);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int _idx = 0;
            this.LoadPetItemInfo(this.GetAssetInfo(list, ref _idx, true));
          }
        }
      }

      private void LoadPetItemInfo(AssetBundleInfo _excelAddress)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_excelAddress);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int _categoryID;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out _categoryID))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                int _itemID;
                if (int.TryParse(source3.GetElement<string>(index4) ?? string.Empty, out _itemID))
                {
                  List<string> source4 = list;
                  int index5 = num4;
                  int num5 = index5 + 1;
                  int result2;
                  if (int.TryParse(source4.GetElement<string>(index5) ?? string.Empty, out result2))
                  {
                    List<ValueTuple<ItemIDKeyPair, int>> valueTupleList1;
                    if (!this.PetItemInfoTable.TryGetValue(result1, out valueTupleList1) || valueTupleList1 == null)
                    {
                      List<ValueTuple<ItemIDKeyPair, int>> valueTupleList2 = new List<ValueTuple<ItemIDKeyPair, int>>();
                      this.PetItemInfoTable[result1] = valueTupleList2;
                      valueTupleList1 = valueTupleList2;
                    }
                    if (0 <= result2)
                    {
                      ValueTuple<ItemIDKeyPair, int> valueTuple;
                      ((ValueTuple<ItemIDKeyPair, int>) ref valueTuple).\u002Ector(new ItemIDKeyPair()
                      {
                        categoryID = _categoryID,
                        itemID = _itemID
                      }, result2);
                      int index6;
                      // ISSUE: cast to a reference type
                      // ISSUE: explicit reference operation
                      // ISSUE: cast to a reference type
                      // ISSUE: explicit reference operation
                      if (0 <= (index6 = valueTupleList1.FindIndex((Predicate<ValueTuple<ItemIDKeyPair, int>>) (x => (^(ItemIDKeyPair&) ref x.Item1).categoryID == _categoryID && (^(ItemIDKeyPair&) ref x.Item1).itemID == _itemID))))
                        valueTupleList1[index6] = valueTuple;
                      else
                        valueTupleList1.Add(valueTuple);
                    }
                    else
                    {
                      // ISSUE: cast to a reference type
                      // ISSUE: explicit reference operation
                      // ISSUE: cast to a reference type
                      // ISSUE: explicit reference operation
                      valueTupleList1.RemoveAll((Predicate<ValueTuple<ItemIDKeyPair, int>>) (x => (^(ItemIDKeyPair&) ref x.Item1).categoryID == _categoryID && (^(ItemIDKeyPair&) ref x.Item1).itemID == _itemID));
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadPetHomeUIInfoList(AssetBundleInfo _excelAddress)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_excelAddress);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num1 = 1;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                ValueTuple<int, List<string>> valueTuple;
                if (!this.PetHomeUIInfoTable.TryGetValue(result1, out valueTuple))
                  ((ValueTuple<int, List<string>>) ref valueTuple).\u002Ector(result2, new List<string>());
                else
                  ((List<string>) valueTuple.Item2).Clear();
                List<string> stringList = (List<string>) valueTuple.Item2;
                while (num3 < list.Count)
                  stringList.Add(list.GetElement<string>(num3++));
                valueTuple.Item2 = (__Null) stringList;
                this.PetHomeUIInfoTable[result1] = valueTuple;
              }
            }
          }
        }
      }

      private void LoadAnimatorInfo(AssetBundleInfo _excelAddress)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_excelAddress);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int _idx = 1;
            int result1;
            int result2;
            if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result1) && int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result2))
            {
              RuntimeAnimatorController animatorController = this.LoadAnimator(this.GetAssetInfo(list, ref _idx, false));
              if (!Object.op_Equality((Object) animatorController, (Object) null))
              {
                Dictionary<int, RuntimeAnimatorController> dictionary1;
                if (!this.AnimatorTable.TryGetValue(result1, out dictionary1) || dictionary1 == null)
                {
                  Dictionary<int, RuntimeAnimatorController> dictionary2 = new Dictionary<int, RuntimeAnimatorController>();
                  this.AnimatorTable[result1] = dictionary2;
                  dictionary1 = dictionary2;
                }
                dictionary1[result2] = animatorController;
              }
            }
          }
        }
      }

      private void LoadAnimStateInfo(AssetBundleInfo _excelAddress)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_excelAddress);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int _idx = 0;
            this.LoadAnimStateInfoElement(this.GetAssetInfo(list, ref _idx, true));
          }
        }
      }

      private void LoadAnimStateInfoElement(AssetBundleInfo _excelAddress)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_excelAddress);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num1 = 2;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                int result3;
                if (int.TryParse(source3.GetElement<string>(index4) ?? string.Empty, out result3))
                {
                  List<string> source4 = list;
                  int index5 = num4;
                  int num5 = index5 + 1;
                  string self1 = source4.GetElement<string>(index5) ?? string.Empty;
                  List<string> source5 = list;
                  int index6 = num5;
                  int num6 = index6 + 1;
                  string self2 = source5.GetElement<string>(index6) ?? string.Empty;
                  if (!self1.IsNullOrEmpty() && !self2.IsNullOrEmpty())
                  {
                    List<string> source6 = list;
                    int index7 = num6;
                    int num7 = index7 + 1;
                    string[] _inStateNames = (source6.GetElement<string>(index7) ?? string.Empty).Split(this.separators, StringSplitOptions.RemoveEmptyEntries);
                    List<string> source7 = list;
                    int index8 = num7;
                    int num8 = index8 + 1;
                    bool result4;
                    if (!bool.TryParse(source7.GetElement<string>(index8) ?? string.Empty, out result4))
                      result4 = false;
                    List<string> source8 = list;
                    int index9 = num8;
                    int num9 = index9 + 1;
                    float result5;
                    if (!float.TryParse(source8.GetElement<string>(index9) ?? string.Empty, out result5))
                      result5 = 0.0f;
                    List<string> source9 = list;
                    int index10 = num9;
                    int num10 = index10 + 1;
                    bool result6;
                    if (!bool.TryParse(source9.GetElement<string>(index10) ?? string.Empty, out result6))
                      result6 = false;
                    List<string> source10 = list;
                    int index11 = num10;
                    int num11 = index11 + 1;
                    int result7;
                    if (!int.TryParse(source10.GetElement<string>(index11) ?? string.Empty, out result7))
                      result7 = 0;
                    List<string> source11 = list;
                    int index12 = num11;
                    int num12 = index12 + 1;
                    int result8;
                    if (!int.TryParse(source11.GetElement<string>(index12) ?? string.Empty, out result8))
                      result8 = 0;
                    List<string> source12 = list;
                    int index13 = num12;
                    int num13 = index13 + 1;
                    string[] _outStateNames = (source12.GetElement<string>(index13) ?? string.Empty).Split(this.separators, StringSplitOptions.RemoveEmptyEntries);
                    List<string> source13 = list;
                    int index14 = num13;
                    int num14 = index14 + 1;
                    bool result9;
                    if (!bool.TryParse(source13.GetElement<string>(index14) ?? string.Empty, out result9))
                      result9 = false;
                    List<string> source14 = list;
                    int index15 = num14;
                    int num15 = index15 + 1;
                    float result10;
                    if (!float.TryParse(source14.GetElement<string>(index15) ?? string.Empty, out result10))
                      result10 = 0.0f;
                    List<string> source15 = list;
                    int index16 = num15;
                    int num16 = index16 + 1;
                    int result11;
                    if (!int.TryParse(source15.GetElement<string>(index16) ?? string.Empty, out result11))
                      result11 = 0;
                    AnimalPlayState _playState = new AnimalPlayState(result11, result3, _inStateNames, _outStateNames);
                    AnimalPlayState.PlayStateInfo mainStateInfo = _playState.MainStateInfo;
                    mainStateInfo.AssetBundleInfo = new AssetBundleInfo(string.Empty, self1, self2, string.Empty);
                    mainStateInfo.InFadeEnable = result4;
                    mainStateInfo.InFadeSecond = result5;
                    mainStateInfo.OutFadeEnable = result9;
                    mainStateInfo.OutFadeSecond = result10;
                    mainStateInfo.IsLoop = result6;
                    mainStateInfo.LoopMin = result7;
                    mainStateInfo.LoopMax = result8;
                    this.LoadAnimalAnimator(_playState);
                    Dictionary<int, Dictionary<int, AnimalPlayState>> dictionary1;
                    if (!this.CommonAnimeTable.TryGetValue(result1, out dictionary1))
                      this.CommonAnimeTable[result1] = dictionary1 = new Dictionary<int, Dictionary<int, AnimalPlayState>>();
                    Dictionary<int, AnimalPlayState> dictionary2;
                    if (!dictionary1.TryGetValue(result2, out dictionary2))
                      dictionary1[result2] = dictionary2 = new Dictionary<int, AnimalPlayState>();
                    dictionary2[result3] = _playState;
                  }
                }
              }
            }
          }
        }
      }

      private void LoadModelInfo(AssetBundleInfo _excelAddress)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_excelAddress);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int _idx = 1;
            int result1;
            int result2;
            if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result1) && int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result2))
            {
              AssetBundleInfo assetInfo = this.GetAssetInfo(list, ref _idx, false);
              bool result3;
              if (!bool.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result3))
                result3 = false;
              string str1 = string.Empty;
              int result4 = -1;
              if (result3)
              {
                str1 = list.GetElement<string>(_idx++) ?? string.Empty;
                result4 = !int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result4) ? -1 : result4;
                result3 = !str1.IsNullOrEmpty() && 0 <= result4;
              }
              else
                _idx += 2;
              bool result5;
              if (!bool.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result5))
                result5 = false;
              string str2 = string.Empty;
              int result6 = -1;
              if (result5)
              {
                str2 = list.GetElement<string>(_idx++) ?? string.Empty;
                result6 = !int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result6) ? -1 : result6;
                result5 = !str2.IsNullOrEmpty() && 0 <= result6;
              }
              else
                _idx += 2;
              AnimalShapeInfo _eyesShapeInfo = new AnimalShapeInfo(result3, str1, result4);
              AnimalShapeInfo _mouthShapeInfo = new AnimalShapeInfo(result5, str2, result6);
              AnimalModelInfo animalModelInfo = new AnimalModelInfo(assetInfo, _eyesShapeInfo, _mouthShapeInfo);
              Dictionary<int, AnimalModelInfo> dictionary1;
              if (!this.ModelInfoTable.TryGetValue(result1, out dictionary1))
              {
                Dictionary<int, AnimalModelInfo> dictionary2 = new Dictionary<int, AnimalModelInfo>();
                this.ModelInfoTable[result1] = dictionary2;
                dictionary1 = dictionary2;
              }
              dictionary1[result2] = animalModelInfo;
            }
          }
        }
      }

      private void LoadExpressionList(AssetBundleInfo _excelAddress)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_excelAddress);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int _idx = 0;
            this.LoadExpressionTable(this.GetAssetInfo(list, ref _idx, true));
          }
        }
      }

      private void LoadExpressionTable(AssetBundleInfo _excelAddress)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_excelAddress);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num1 = 2;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                int result3;
                if (int.TryParse(source3.GetElement<string>(index4) ?? string.Empty, out result3))
                {
                  List<string> source4 = list;
                  int index5 = num4;
                  int num5 = index5 + 1;
                  string element1 = source4.GetElement<string>(index5);
                  if (!element1.IsNullOrEmpty())
                  {
                    Dictionary<int, Dictionary<int, Dictionary<string, List<ValueTuple<string, int, int>>>>> dictionary1;
                    if (!this.ExpressionTable.TryGetValue(result1, out dictionary1) || dictionary1 == null)
                      this.ExpressionTable[result1] = dictionary1 = new Dictionary<int, Dictionary<int, Dictionary<string, List<ValueTuple<string, int, int>>>>>();
                    Dictionary<int, Dictionary<string, List<ValueTuple<string, int, int>>>> dictionary2;
                    if (!dictionary1.TryGetValue(result2, out dictionary2) || dictionary2 == null)
                      dictionary1[result2] = dictionary2 = new Dictionary<int, Dictionary<string, List<ValueTuple<string, int, int>>>>();
                    Dictionary<string, List<ValueTuple<string, int, int>>> dictionary3;
                    if (!dictionary2.TryGetValue(result3, out dictionary3) || dictionary3 == null)
                      dictionary2[result3] = dictionary3 = new Dictionary<string, List<ValueTuple<string, int, int>>>();
                    List<ValueTuple<string, int, int>> valueTupleList;
                    if (!dictionary3.TryGetValue(element1, out valueTupleList) || valueTupleList == null)
                      dictionary3[element1] = valueTupleList = new List<ValueTuple<string, int, int>>();
                    else
                      valueTupleList.Clear();
                    while (num5 < list.Count)
                    {
                      List<string> source5 = list;
                      int index6 = num5;
                      int num6 = index6 + 1;
                      string element2 = source5.GetElement<string>(index6);
                      List<string> source6 = list;
                      int index7 = num6;
                      int num7 = index7 + 1;
                      string s1 = source6.GetElement<string>(index7) ?? string.Empty;
                      List<string> source7 = list;
                      int index8 = num7;
                      num5 = index8 + 1;
                      string s2 = source7.GetElement<string>(index8) ?? string.Empty;
                      int result4;
                      int result5;
                      if (!element2.IsNullOrEmpty() && int.TryParse(s1, out result4) && int.TryParse(s2, out result5))
                        valueTupleList.Add(new ValueTuple<string, int, int>(element2, result4, result5));
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadAnimalTextureList(AssetBundleInfo _excelAddress)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_excelAddress);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                string assetBundleName = source3.GetElement<string>(index4) ?? string.Empty;
                List<string> source4 = list;
                int index5 = num4;
                int num5 = index5 + 1;
                string assetName = source4.GetElement<string>(index5) ?? string.Empty;
                List<string> source5 = list;
                int index6 = num5;
                int num6 = index6 + 1;
                string str = source5.GetElement<string>(index6) ?? string.Empty;
                List<Color> toRelease = ListPool<Color>.Get();
                while (num6 < list.Count)
                {
                  List<string> source6 = list;
                  int index7 = num6;
                  int num7 = index7 + 1;
                  float result3;
                  if (!float.TryParse(source6.GetElement<string>(index7) ?? string.Empty, out result3))
                    result3 = 0.0f;
                  List<string> source7 = list;
                  int index8 = num7;
                  int num8 = index8 + 1;
                  float result4;
                  if (!float.TryParse(source7.GetElement<string>(index8) ?? string.Empty, out result4))
                    result4 = 0.0f;
                  List<string> source8 = list;
                  int index9 = num8;
                  num6 = index9 + 1;
                  float result5;
                  if (!float.TryParse(source8.GetElement<string>(index9) ?? string.Empty, out result5))
                    result5 = 0.0f;
                  toRelease.Add(new Color(result3 / (float) byte.MaxValue, result4 / (float) byte.MaxValue, result5 / (float) byte.MaxValue, 1f));
                }
                for (int index7 = 0; index7 < 4; ++index7)
                {
                  List<string> source6 = list;
                  int index8 = num6;
                  int num7 = index8 + 1;
                  float result3;
                  if (!float.TryParse(source6.GetElement<string>(index8) ?? string.Empty, out result3))
                    result3 = 0.0f;
                  List<string> source7 = list;
                  int index9 = num7;
                  int num8 = index9 + 1;
                  float result4;
                  if (!float.TryParse(source7.GetElement<string>(index9) ?? string.Empty, out result4))
                    result4 = 0.0f;
                  List<string> source8 = list;
                  int index10 = num8;
                  num6 = index10 + 1;
                  float result5;
                  if (!float.TryParse(source8.GetElement<string>(index10) ?? string.Empty, out result5))
                    result5 = 0.0f;
                  toRelease.Add(new Color(result3 / (float) byte.MaxValue, result4 / (float) byte.MaxValue, result5 / (float) byte.MaxValue, 1f));
                }
                Texture2D texture2D = CommonLib.LoadAsset<Texture2D>(assetBundleName, assetName, false, !str.IsNullOrEmpty() ? str : (string) null);
                if (Object.op_Inequality((Object) texture2D, (Object) null))
                  Singleton<Manager.Resources>.Instance.AddLoadAssetBundle(assetBundleName, str);
                Color[] colorArray = new Color[toRelease.Count];
                for (int index7 = 0; index7 < colorArray.Length; ++index7)
                  colorArray[index7] = toRelease[index7];
                ListPool<Color>.Release(toRelease);
                ValueTuple<Texture2D, Color[]> valueTuple;
                ((ValueTuple<Texture2D, Color[]>) ref valueTuple).\u002Ector(texture2D, colorArray);
                Dictionary<int, ValueTuple<Texture2D, Color[]>> dictionary1;
                if (!this.TextureTable.TryGetValue(result1, out dictionary1) || dictionary1 == null)
                {
                  Dictionary<int, ValueTuple<Texture2D, Color[]>> dictionary2 = new Dictionary<int, ValueTuple<Texture2D, Color[]>>();
                  this.TextureTable[result1] = dictionary2;
                  dictionary1 = dictionary2;
                }
                dictionary1[result2] = valueTuple;
              }
            }
          }
        }
      }

      private void LoadAnimalBaseObjList(AssetBundleInfo _excelAddress)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_excelAddress);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num1 = 1;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                string element1 = source3.GetElement<string>(index4);
                List<string> source4 = list;
                int index5 = num4;
                int num5 = index5 + 1;
                string element2 = source4.GetElement<string>(index5);
                List<string> source5 = list;
                int index6 = num5;
                int num6 = index6 + 1;
                string element3 = source5.GetElement<string>(index6);
                if (!element1.IsNullOrEmpty() && !element2.IsNullOrEmpty())
                {
                  Dictionary<int, AssetBundleInfo> dictionary1;
                  if (!this.AnimalBaseObjInfoTable.TryGetValue(result1, out dictionary1) || dictionary1 == null)
                  {
                    Dictionary<int, AssetBundleInfo> dictionary2 = new Dictionary<int, AssetBundleInfo>();
                    this.AnimalBaseObjInfoTable[result1] = dictionary2;
                    dictionary1 = dictionary2;
                  }
                  dictionary1[result2] = new AssetBundleInfo(string.Empty, element1, element2, element3);
                }
              }
            }
          }
        }
      }

      private void LoadAction(AnimalDefinePack _animalDefinePack)
      {
        string listBundleDirectory = _animalDefinePack.AssetBundleNames.ActionInfoListBundleDirectory;
        IReadOnlyList<ValueTuple<string, AnimalTypes>> animalNameList = AIProject.Animal.AnimalData.AnimalNameList;
        for (int index1 = 0; index1 < ((IReadOnlyCollection<ValueTuple<string, AnimalTypes>>) animalNameList).get_Count(); ++index1)
        {
          string _animalName = (string) animalNameList.get_Item(index1).Item1;
          AnimalTypes _animalType = (AnimalTypes) animalNameList.get_Item(index1).Item2;
          IReadOnlyList<ValueTuple<string, BreedingTypes>> breedingNameList = AIProject.Animal.AnimalData.BreedingNameList;
          for (int index2 = 0; index2 < ((IReadOnlyCollection<ValueTuple<string, BreedingTypes>>) breedingNameList).get_Count(); ++index2)
          {
            string _breedingName = (string) breedingNameList.get_Item(index2).Item1;
            BreedingTypes _breedingType = (BreedingTypes) breedingNameList.get_Item(index2).Item2;
            List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(this.GetDirectory(listBundleDirectory, _animalName, _breedingName), false);
            if (!((IReadOnlyList<string>) nameListFromPath).IsNullOrEmpty<string>())
            {
              nameListFromPath.Sort();
              for (int index3 = 0; index3 < nameListFromPath.Count; ++index3)
              {
                string str = nameListFromPath[index3];
                string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
                if (AssetBundleCheck.IsFile(str, withoutExtension))
                {
                  ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
                  if (!Object.op_Equality((Object) excelData, (Object) null))
                  {
                    for (int index4 = 1; index4 < excelData.MaxCell; ++index4)
                    {
                      List<string> list = excelData.list[index4].list;
                      if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
                      {
                        int num1 = 0;
                        List<string> source1 = list;
                        int index5 = num1;
                        int num2 = index5 + 1;
                        int result;
                        if (int.TryParse(source1.GetElement<string>(index5) ?? string.Empty, out result))
                        {
                          int num3 = num2 + 1;
                          AssetBundleInfo _sheetABInfo;
                          ref AssetBundleInfo local = ref _sheetABInfo;
                          string empty = string.Empty;
                          List<string> source2 = list;
                          int index6 = num3;
                          int num4 = index6 + 1;
                          string element1 = source2.GetElement<string>(index6);
                          List<string> source3 = list;
                          int index7 = num4;
                          int num5 = index7 + 1;
                          string element2 = source3.GetElement<string>(index7);
                          List<string> source4 = list;
                          int index8 = num5;
                          int num6 = index8 + 1;
                          string element3 = source4.GetElement<string>(index8);
                          ((AssetBundleInfo) ref local).\u002Ector(empty, element1, element2, element3);
                          if (!((string) _sheetABInfo.assetbundle).IsNullOrEmpty() && !((string) _sheetABInfo.asset).IsNullOrEmpty() && result == 0)
                            this.LoadActionEndInfo(_animalType, _breedingType, _sheetABInfo);
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadActionEndInfo(
        AnimalTypes _animalType,
        BreedingTypes _breedingType,
        AssetBundleInfo _sheetABInfo)
      {
        if (((string) _sheetABInfo.assetbundle).IsNullOrEmpty() || ((string) _sheetABInfo.asset).IsNullOrEmpty())
          return;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetABInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          int num1 = 0;
          int result1 = 0;
          int result2 = 0;
          int result3 = 0;
          AnimalState key = AnimalState.None;
          bool result4 = false;
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            string s1 = source1.GetElement<string>(index2) ?? string.Empty;
            List<string> source2 = list;
            int index3 = num2;
            int num3 = index3 + 1;
            string str1 = source2.GetElement<string>(index3) ?? string.Empty;
            int num4 = num3 + 1;
            List<string> source3 = list;
            int index4 = num4;
            int num5 = index4 + 1;
            string str2 = source3.GetElement<string>(index4) ?? string.Empty;
            List<string> source4 = list;
            int index5 = num5;
            int num6 = index5 + 1;
            string s2 = source4.GetElement<string>(index5) ?? string.Empty;
            List<string> source5 = list;
            int index6 = num6;
            int num7 = index6 + 1;
            string s3 = source5.GetElement<string>(index6) ?? string.Empty;
            if (int.TryParse(s1, out result1) && AIProject.Animal.AnimalData.AnimalStateIDTable.TryGetValue(result1, ref key) && (bool.TryParse(str2, out result4) && int.TryParse(s2, out result2)) && int.TryParse(s3, out result3))
            {
              if (!this.ActionInfoTable.ContainsKey(_animalType))
                this.ActionInfoTable[_animalType] = new Dictionary<BreedingTypes, Dictionary<AnimalState, AnimalActionInfo>>();
              if (!this.ActionInfoTable[_animalType].ContainsKey(_breedingType))
                this.ActionInfoTable[_animalType][_breedingType] = new Dictionary<AnimalState, AnimalActionInfo>();
              AnimalActionInfo animalActionInfo;
              if (!this.ActionInfoTable[_animalType][_breedingType].TryGetValue(key, out animalActionInfo))
                animalActionInfo = new AnimalActionInfo();
              animalActionInfo.timeInfo = new AnimalActionInfo.TimeInfo(result4, result2, result3);
              this.ActionInfoTable[_animalType][_breedingType][key] = animalActionInfo;
            }
          }
        }
      }

      private void LoadAnimalAnimator(AnimalPlayState _playState)
      {
        if (_playState == null)
          return;
        AssetBundleInfo assetBundleInfo1 = _playState.MainStateInfo.AssetBundleInfo;
        assetBundleInfo1.manifest = (__Null) "abdata";
        _playState.MainStateInfo.Controller = this.LoadAnimator(assetBundleInfo1);
        if (((IReadOnlyList<AnimalPlayState.PlayStateInfo>) _playState.SubStateInfos).IsNullOrEmpty<AnimalPlayState.PlayStateInfo>())
          return;
        foreach (AnimalPlayState.PlayStateInfo subStateInfo in _playState.SubStateInfos)
        {
          AssetBundleInfo assetBundleInfo2 = subStateInfo.AssetBundleInfo;
          assetBundleInfo2.manifest = (__Null) "abdata";
          subStateInfo.Controller = this.LoadAnimator(assetBundleInfo2);
        }
      }

      private void LoadLook(AnimalDefinePack _animalDefinePack)
      {
        string listBundleDirectory = _animalDefinePack.AssetBundleNames.LookInfoListBundleDirectory;
        IReadOnlyList<ValueTuple<string, AnimalTypes>> animalNameList = AIProject.Animal.AnimalData.AnimalNameList;
        for (int index1 = 0; index1 < ((IReadOnlyCollection<ValueTuple<string, AnimalTypes>>) animalNameList).get_Count(); ++index1)
        {
          string _animalName = (string) animalNameList.get_Item(index1).Item1;
          AnimalTypes key = (AnimalTypes) animalNameList.get_Item(index1).Item2;
          List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(this.GetDirectory(listBundleDirectory, _animalName), false);
          if (!((IReadOnlyList<string>) nameListFromPath).IsNullOrEmpty<string>())
          {
            nameListFromPath.Sort();
            for (int index2 = 0; index2 < nameListFromPath.Count; ++index2)
            {
              string str1 = nameListFromPath[index2];
              string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str1);
              if (AssetBundleCheck.IsFile(str1, withoutExtension))
              {
                ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str1, withoutExtension, string.Empty);
                if (!Object.op_Equality((Object) excelData, (Object) null))
                {
                  for (int index3 = 1; index3 < excelData.MaxCell; ++index3)
                  {
                    List<string> list = excelData.list[index3].list;
                    if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
                    {
                      int num1 = 0;
                      int result1 = 0;
                      AnimalState index4 = AnimalState.None;
                      List<string> source1 = list;
                      int index5 = num1;
                      int num2 = index5 + 1;
                      string s1 = source1.GetElement<string>(index5) ?? string.Empty;
                      List<string> source2 = list;
                      int index6 = num2;
                      int num3 = index6 + 1;
                      string str2 = source2.GetElement<string>(index6) ?? string.Empty;
                      List<string> source3 = list;
                      int index7 = num3;
                      int num4 = index7 + 1;
                      string s2 = source3.GetElement<string>(index7) ?? string.Empty;
                      List<string> source4 = list;
                      int index8 = num4;
                      int num5 = index8 + 1;
                      string str3 = source4.GetElement<string>(index8) ?? string.Empty;
                      List<string> source5 = list;
                      int index9 = num5;
                      int num6 = index9 + 1;
                      string s3 = source5.GetElement<string>(index9) ?? string.Empty;
                      List<string> source6 = list;
                      int index10 = num6;
                      int num7 = index10 + 1;
                      string str4 = source6.GetElement<string>(index10) ?? string.Empty;
                      if (int.TryParse(s1, out result1) && AIProject.Animal.AnimalData.AnimalStateIDTable.TryGetValue(result1, ref index4))
                      {
                        int result2 = 0;
                        if (int.TryParse(s2, out result2))
                        {
                          bool result3 = false;
                          result3 = bool.TryParse(str3, out result3) && result3;
                          if (!this.LookStateTable.ContainsKey(key))
                            this.LookStateTable[key] = new Dictionary<BreedingTypes, Dictionary<AnimalState, LookState>>();
                          if (!this.LookStateTable[key].ContainsKey(BreedingTypes.Wild))
                            this.LookStateTable[key][BreedingTypes.Wild] = new Dictionary<AnimalState, LookState>();
                          this.LookStateTable[key][BreedingTypes.Wild][index4] = new LookState(result2, result3);
                        }
                        int result4 = 0;
                        if (int.TryParse(s3, out result4))
                        {
                          bool result3 = false;
                          result3 = bool.TryParse(str4, out result3) && result3;
                          if (!this.LookStateTable.ContainsKey(key))
                            this.LookStateTable[key] = new Dictionary<BreedingTypes, Dictionary<AnimalState, LookState>>();
                          if (!this.LookStateTable[key].ContainsKey(BreedingTypes.Pet))
                            this.LookStateTable[key][BreedingTypes.Pet] = new Dictionary<AnimalState, LookState>();
                          this.LookStateTable[key][BreedingTypes.Pet][index4] = new LookState(result4, result3);
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadState(AnimalDefinePack _animalDefinePack)
      {
        string listBundleDirectory = _animalDefinePack.AssetBundleNames.StateInfoListBundleDirectory;
        IReadOnlyList<ValueTuple<string, AnimalTypes>> animalNameList = AIProject.Animal.AnimalData.AnimalNameList;
        for (int index1 = 0; index1 < ((IReadOnlyCollection<ValueTuple<string, AnimalTypes>>) animalNameList).get_Count(); ++index1)
        {
          string _animalName = (string) animalNameList.get_Item(index1).Item1;
          AnimalTypes _animalType = (AnimalTypes) animalNameList.get_Item(index1).Item2;
          IReadOnlyList<ValueTuple<string, BreedingTypes>> breedingNameList = AIProject.Animal.AnimalData.BreedingNameList;
          for (int index2 = 0; index2 < ((IReadOnlyCollection<ValueTuple<string, BreedingTypes>>) breedingNameList).get_Count(); ++index2)
          {
            string _breedingName = (string) breedingNameList.get_Item(index2).Item1;
            BreedingTypes _breedingType = (BreedingTypes) breedingNameList.get_Item(index2).Item2;
            List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(this.GetDirectory(listBundleDirectory, _animalName, _breedingName), false);
            nameListFromPath.Sort();
            if (!((IReadOnlyList<string>) nameListFromPath).IsNullOrEmpty<string>())
            {
              for (int index3 = 0; index3 < nameListFromPath.Count; ++index3)
              {
                string str = nameListFromPath[index3];
                string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
                if (AssetBundleCheck.IsFile(str, withoutExtension))
                {
                  ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
                  if (!Object.op_Equality((Object) excelData, (Object) null))
                  {
                    for (int index4 = 1; index4 < excelData.MaxCell; ++index4)
                    {
                      List<string> list = excelData.list[index4].list;
                      if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
                      {
                        int num1 = 0;
                        List<string> source1 = list;
                        int index5 = num1;
                        int num2 = index5 + 1;
                        int result;
                        if (int.TryParse(source1.GetElement<string>(index5) ?? string.Empty, out result))
                        {
                          int num3 = num2 + 1;
                          AssetBundleInfo _sheetABInfo;
                          ref AssetBundleInfo local = ref _sheetABInfo;
                          string empty = string.Empty;
                          List<string> source2 = list;
                          int index6 = num3;
                          int num4 = index6 + 1;
                          string element1 = source2.GetElement<string>(index6);
                          List<string> source3 = list;
                          int index7 = num4;
                          int index8 = index7 + 1;
                          string element2 = source3.GetElement<string>(index7);
                          string element3 = list.GetElement<string>(index8);
                          ((AssetBundleInfo) ref local).\u002Ector(empty, element1, element2, element3);
                          if (!((string) _sheetABInfo.assetbundle).IsNullOrEmpty() && !((string) _sheetABInfo.asset).IsNullOrEmpty())
                          {
                            switch (result)
                            {
                              case 0:
                                this.LoadChangeState(_animalType, _breedingType, _sheetABInfo);
                                continue;
                              case 1:
                                this.LoadStateTargetActionType(_animalType, _breedingType, _sheetABInfo);
                                continue;
                              default:
                                continue;
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadChangeState(
        AnimalTypes _animalType,
        BreedingTypes _breedingType,
        AssetBundleInfo _sheetABInfo)
      {
        if (((string) _sheetABInfo.assetbundle).IsNullOrEmpty() || ((string) _sheetABInfo.asset).IsNullOrEmpty())
          return;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetABInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num1 = 0;
            int result = 0;
            AnimalState _animalState = AnimalState.None;
            ConditionType _conditionType = ConditionType.None;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            string s = source1.GetElement<string>(index2) ?? string.Empty;
            List<string> source2 = list;
            int index3 = num2;
            int num3 = index3 + 1;
            string str = source2.GetElement<string>(index3) ?? string.Empty;
            List<string> source3 = list;
            int index4 = num3;
            int _dx = index4 + 1;
            string _name = source3.GetElement<string>(index4) ?? string.Empty;
            if (int.TryParse(s, out result) && AIProject.Animal.AnimalData.AnimalStateIDTable.TryGetValue(result, ref _animalState) && this.TryStringToEnum<ConditionType>(_name, out _conditionType))
              this.SetConditionState(_animalType, _breedingType, list, _dx, _animalState, _conditionType);
          }
        }
      }

      private void SetConditionState(
        AnimalTypes _animalType,
        BreedingTypes _breedingType,
        List<string> _row,
        int _dx,
        AnimalState _animalState,
        ConditionType _conditionType)
      {
        StateCondition stateCondition = new StateCondition(_conditionType, _animalState);
        switch (_conditionType)
        {
          case ConditionType.Forced:
            AnimalState _state1;
            if (!this.TryStringToEnum<AnimalState>(_row.GetElement<string>(_dx++) ?? string.Empty, out _state1))
              return;
            stateCondition.AddNextState(_state1, 0.0f);
            break;
          case ConditionType.Proportion:
            while (_dx < _row.Count)
            {
              AnimalState _state2 = AnimalState.None;
              float result = 0.0f;
              string _name = _row.GetElement<string>(_dx++) ?? string.Empty;
              string s = _row.GetElement<string>(_dx++) ?? string.Empty;
              if (this.TryStringToEnum<AnimalState>(_name, out _state2) && float.TryParse(s, out result))
                stateCondition.AddNextState(_state2, result);
              else
                break;
            }
            break;
          case ConditionType.Random:
          case ConditionType.NonOverlap:
            while (_dx < _row.Count)
            {
              AnimalState _state2 = AnimalState.None;
              if (this.TryStringToEnum<AnimalState>(_row.GetElement<string>(_dx++) ?? string.Empty, out _state2))
                stateCondition.AddNextState(_state2, 0.0f);
            }
            break;
          default:
            return;
        }
        if (stateCondition.Count <= 0)
          return;
        if (!this.StateConditionTable.ContainsKey(_animalType))
          this.StateConditionTable[_animalType] = new Dictionary<BreedingTypes, Dictionary<AnimalState, StateCondition>>();
        if (!this.StateConditionTable[_animalType].ContainsKey(_breedingType))
          this.StateConditionTable[_animalType][_breedingType] = new Dictionary<AnimalState, StateCondition>();
        this.StateConditionTable[_animalType][_breedingType][_animalState] = stateCondition;
      }

      private void LoadStateTargetActionType(
        AnimalTypes _animalType,
        BreedingTypes _breedingType,
        AssetBundleInfo _sheetABInfo)
      {
        if (((string) _sheetABInfo.assetbundle).IsNullOrEmpty() || ((string) _sheetABInfo.asset).IsNullOrEmpty())
          return;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetABInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num1 = 0;
            int result1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            string s1 = source1.GetElement<string>(index2) ?? string.Empty;
            int num3 = num2 + 1;
            List<string> source2 = list;
            int index3 = num3;
            int num4 = index3 + 1;
            string[] strArray = (source2.GetElement<string>(index3) ?? string.Empty).Split(this.separators, StringSplitOptions.RemoveEmptyEntries);
            AnimalState index4;
            if (int.TryParse(s1, out result1) && AIProject.Animal.AnimalData.AnimalStateIDTable.TryGetValue(result1, ref index4))
            {
              List<ActionTypes> actionTypesList = new List<ActionTypes>();
              if (!((IReadOnlyList<string>) strArray).IsNullOrEmpty<string>())
              {
                foreach (string s2 in strArray)
                {
                  int result2;
                  ActionTypes actionTypes;
                  if (int.TryParse(s2, out result2) && this.AnimalActionIDTable.TryGetValue(result2, out actionTypes))
                    actionTypesList.Add(actionTypes);
                }
              }
              if (!this.StateTargetActionTable.ContainsKey(_animalType))
                this.StateTargetActionTable[_animalType] = new Dictionary<BreedingTypes, Dictionary<AnimalState, List<ActionTypes>>>();
              if (!this.StateTargetActionTable[_animalType].ContainsKey(_breedingType))
                this.StateTargetActionTable[_animalType][_breedingType] = new Dictionary<AnimalState, List<ActionTypes>>();
              this.StateTargetActionTable[_animalType][_breedingType][index4] = actionTypesList;
            }
          }
        }
      }

      private void LoadDesire(AnimalDefinePack _animalDefinePack)
      {
        string listBundleDirectory = _animalDefinePack.AssetBundleNames.DesireInfoListBundleDirectory;
        IReadOnlyList<ValueTuple<string, AnimalTypes>> animalNameList = AIProject.Animal.AnimalData.AnimalNameList;
        for (int index1 = 0; index1 < ((IReadOnlyCollection<ValueTuple<string, AnimalTypes>>) animalNameList).get_Count(); ++index1)
        {
          string _animalName = (string) animalNameList.get_Item(index1).Item1;
          AnimalTypes _animalType = (AnimalTypes) animalNameList.get_Item(index1).Item2;
          IReadOnlyList<ValueTuple<string, BreedingTypes>> breedingNameList = AIProject.Animal.AnimalData.BreedingNameList;
          for (int index2 = 0; index2 < ((IReadOnlyCollection<ValueTuple<string, BreedingTypes>>) breedingNameList).get_Count(); ++index2)
          {
            string _breedingName = (string) breedingNameList.get_Item(index2).Item1;
            BreedingTypes _breedingType = (BreedingTypes) breedingNameList.get_Item(index2).Item2;
            List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(this.GetDirectory(listBundleDirectory, _animalName, _breedingName), false);
            if (!((IReadOnlyList<string>) nameListFromPath).IsNullOrEmpty<string>())
            {
              nameListFromPath.Sort();
              for (int index3 = 0; index3 < nameListFromPath.Count; ++index3)
              {
                string str = nameListFromPath[index3];
                string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
                if (AssetBundleCheck.IsFile(str, withoutExtension))
                {
                  ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
                  if (!Object.op_Equality((Object) excelData, (Object) null))
                  {
                    for (int index4 = 1; index4 < excelData.MaxCell; ++index4)
                    {
                      List<string> list = excelData.list[index4].list;
                      if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
                      {
                        int num1 = 0;
                        List<string> source1 = list;
                        int index5 = num1;
                        int num2 = index5 + 1;
                        int result;
                        if (int.TryParse(source1.GetElement<string>(index5) ?? string.Empty, out result))
                        {
                          int num3 = num2 + 1;
                          AssetBundleInfo _sheetInfo;
                          ref AssetBundleInfo local = ref _sheetInfo;
                          string empty = string.Empty;
                          List<string> source2 = list;
                          int index6 = num3;
                          int num4 = index6 + 1;
                          string element1 = source2.GetElement<string>(index6);
                          List<string> source3 = list;
                          int index7 = num4;
                          int num5 = index7 + 1;
                          string element2 = source3.GetElement<string>(index7);
                          List<string> source4 = list;
                          int index8 = num5;
                          int num6 = index8 + 1;
                          string element3 = source4.GetElement<string>(index8);
                          ((AssetBundleInfo) ref local).\u002Ector(empty, element1, element2, element3);
                          if (!((string) _sheetInfo.assetbundle).IsNullOrEmpty() && !((string) _sheetInfo.asset).IsNullOrEmpty())
                          {
                            switch (result)
                            {
                              case 0:
                                this.LoadDesirePriority(_animalType, _breedingType, _sheetInfo);
                                continue;
                              case 1:
                                this.LoadDesireSpan(_animalType, _breedingType, _sheetInfo);
                                continue;
                              case 2:
                                this.LoadDesireBorder(_animalType, _breedingType, _sheetInfo);
                                continue;
                              case 3:
                                this.LoadDesireRate(_animalType, _breedingType, _sheetInfo);
                                continue;
                              case 4:
                                this.LoadDesireTargetState(_animalType, _breedingType, _sheetInfo);
                                continue;
                              case 5:
                                this.LoadDesireResult(_animalType, _breedingType, _sheetInfo);
                                continue;
                              default:
                                continue;
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadDesirePriority(
        AnimalTypes _animalType,
        BreedingTypes _breedingType,
        AssetBundleInfo _sheetInfo)
      {
        if (((string) _sheetInfo.assetbundle).IsNullOrEmpty() || ((string) _sheetInfo.asset).IsNullOrEmpty())
          return;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num1 = 0;
            int result1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            string s1 = source1.GetElement<string>(index2) ?? string.Empty;
            List<string> source2 = list;
            int index3 = num2;
            int num3 = index3 + 1;
            string s2 = source2.GetElement<string>(index3) ?? string.Empty;
            int result2;
            DesireType desireType;
            if (int.TryParse(s1, out result1) && int.TryParse(s2, out result2) && this.AnimalDesireIDTable.TryGetValue(result2, out desireType))
            {
              if (!this.DesirePriorityTable.ContainsKey(_animalType))
                this.DesirePriorityTable[_animalType] = new Dictionary<BreedingTypes, Dictionary<int, DesireType>>();
              if (!this.DesirePriorityTable[_animalType].ContainsKey(_breedingType))
                this.DesirePriorityTable[_animalType][_breedingType] = new Dictionary<int, DesireType>();
              this.DesirePriorityTable[_animalType][_breedingType][result1] = desireType;
            }
          }
        }
      }

      private void LoadDesireSpan(
        AnimalTypes _animalType,
        BreedingTypes _breedingType,
        AssetBundleInfo _sheetInfo)
      {
        if (((string) _sheetInfo.assetbundle).IsNullOrEmpty() || ((string) _sheetInfo.asset).IsNullOrEmpty())
          return;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num1 = 0;
            int result1 = 0;
            int result2 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            string s1 = source1.GetElement<string>(index2) ?? string.Empty;
            int num3 = num2 + 1;
            List<string> source2 = list;
            int index3 = num3;
            int num4 = index3 + 1;
            string s2 = source2.GetElement<string>(index3) ?? string.Empty;
            DesireType index4;
            if (int.TryParse(s1, out result1) && int.TryParse(s2, out result2) && this.AnimalDesireIDTable.TryGetValue(result1, out index4))
            {
              if (!this.DesireSpanTable.ContainsKey(_animalType))
                this.DesireSpanTable[_animalType] = new Dictionary<BreedingTypes, Dictionary<DesireType, int>>();
              if (!this.DesireSpanTable[_animalType].ContainsKey(_breedingType))
                this.DesireSpanTable[_animalType][_breedingType] = new Dictionary<DesireType, int>();
              this.DesireSpanTable[_animalType][_breedingType][index4] = result2;
            }
          }
        }
      }

      private void LoadDesireBorder(
        AnimalTypes _animalType,
        BreedingTypes _breedingType,
        AssetBundleInfo _sheetInfo)
      {
        if (((string) _sheetInfo.assetbundle).IsNullOrEmpty() || ((string) _sheetInfo.asset).IsNullOrEmpty())
          return;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num1 = 0;
            float result1 = 0.0f;
            float result2 = 0.0f;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            string s1 = source1.GetElement<string>(index2) ?? string.Empty;
            int num3 = num2 + 1;
            List<string> source2 = list;
            int index3 = num3;
            int num4 = index3 + 1;
            string s2 = source2.GetElement<string>(index3) ?? string.Empty;
            List<string> source3 = list;
            int index4 = num4;
            int num5 = index4 + 1;
            string s3 = source3.GetElement<string>(index4) ?? string.Empty;
            int result3;
            DesireType index5;
            if (int.TryParse(s1, out result3) && float.TryParse(s2, out result1) && (float.TryParse(s3, out result2) && this.AnimalDesireIDTable.TryGetValue(result3, out index5)))
            {
              if (!this.DesireBorderTable.ContainsKey(_animalType))
                this.DesireBorderTable[_animalType] = new Dictionary<BreedingTypes, Dictionary<DesireType, Tuple<float, float>>>();
              if (!this.DesireBorderTable[_animalType].ContainsKey(_breedingType))
                this.DesireBorderTable[_animalType][_breedingType] = new Dictionary<DesireType, Tuple<float, float>>();
              this.DesireBorderTable[_animalType][_breedingType][index5] = new Tuple<float, float>(result1, result2);
            }
          }
        }
      }

      private void LoadDesireRate(
        AnimalTypes _animalType,
        BreedingTypes _breedingType,
        AssetBundleInfo _sheetInfo)
      {
        if (((string) _sheetInfo.assetbundle).IsNullOrEmpty() || ((string) _sheetInfo.asset).IsNullOrEmpty())
          return;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num1 = 0;
            int result1 = 0;
            List<string> source = list;
            int index2 = num1;
            int num2 = index2 + 1;
            AnimalState key1;
            if (int.TryParse(source.GetElement<string>(index2) ?? string.Empty, out result1) && AIProject.Animal.AnimalData.AnimalStateIDTable.TryGetValue(result1, ref key1))
            {
              int index3 = num2 + 1;
              int key2 = 0;
              while (index3 < list.Count)
              {
                DesireType index4;
                if (this.AnimalDesireIDTable.TryGetValue(key2, out index4))
                {
                  float result2 = 0.0f;
                  if (float.TryParse(list.GetElement<string>(index3) ?? string.Empty, out result2))
                  {
                    if (!this.DesireRateTable.ContainsKey(_animalType))
                      this.DesireRateTable[_animalType] = new Dictionary<BreedingTypes, Dictionary<AnimalState, Dictionary<DesireType, float>>>();
                    if (!this.DesireRateTable[_animalType].ContainsKey(_breedingType))
                      this.DesireRateTable[_animalType][_breedingType] = new Dictionary<AnimalState, Dictionary<DesireType, float>>();
                    if (!this.DesireRateTable[_animalType][_breedingType].ContainsKey(key1))
                      this.DesireRateTable[_animalType][_breedingType][key1] = new Dictionary<DesireType, float>();
                    this.DesireRateTable[_animalType][_breedingType][key1][index4] = result2;
                  }
                }
                ++index3;
                ++key2;
              }
            }
          }
        }
      }

      private void LoadDesireTargetState(
        AnimalTypes _animalType,
        BreedingTypes _breedingType,
        AssetBundleInfo _sheetInfo)
      {
        if (((string) _sheetInfo.assetbundle).IsNullOrEmpty() || ((string) _sheetInfo.asset).IsNullOrEmpty())
          return;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num1 = 0;
            int result1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            string s = source1.GetElement<string>(index2) ?? string.Empty;
            int num3 = num2 + 1;
            List<string> source2 = list;
            int index3 = num3;
            int num4 = index3 + 1;
            string[] strArray = (source2.GetElement<string>(index3) ?? string.Empty).Split(this.separators, StringSplitOptions.RemoveEmptyEntries);
            DesireType index4;
            if (int.TryParse(s, out result1) && this.AnimalDesireIDTable.TryGetValue(result1, out index4))
            {
              List<AnimalState> animalStateList = new List<AnimalState>();
              if (!((IReadOnlyList<string>) strArray).IsNullOrEmpty<string>())
              {
                for (int index5 = 0; index5 < strArray.Length; ++index5)
                {
                  int result2;
                  AnimalState animalState;
                  if (int.TryParse(strArray[index5] ?? string.Empty, out result2) && AIProject.Animal.AnimalData.AnimalStateIDTable.TryGetValue(result2, ref animalState))
                    animalStateList.Add(animalState);
                }
              }
              if (!this.DesireTargetStateTable.ContainsKey(_animalType))
                this.DesireTargetStateTable[_animalType] = new Dictionary<BreedingTypes, Dictionary<DesireType, List<AnimalState>>>();
              if (!this.DesireTargetStateTable[_animalType].ContainsKey(_breedingType))
                this.DesireTargetStateTable[_animalType][_breedingType] = new Dictionary<DesireType, List<AnimalState>>();
              this.DesireTargetStateTable[_animalType][_breedingType][index4] = animalStateList;
            }
          }
        }
      }

      private void LoadDesireResult(
        AnimalTypes _animalType,
        BreedingTypes _breedingType,
        AssetBundleInfo _sheetInfo)
      {
        if (((string) _sheetInfo.assetbundle).IsNullOrEmpty() || ((string) _sheetInfo.asset).IsNullOrEmpty())
          return;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        DesireType key = DesireType.None;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num1 = 0;
            int result1 = 0;
            int result2 = 0;
            int result3 = 0;
            float result4 = 0.0f;
            float result5 = 0.0f;
            bool result6 = true;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            string s1 = source1.GetElement<string>(index2) ?? string.Empty;
            int num3 = num2 + 1;
            DesireType desireType;
            if (int.TryParse(s1, out result1) && this.AnimalDesireIDTable.TryGetValue(result1, out desireType))
              key = desireType;
            List<string> source2 = list;
            int index3 = num3;
            int num4 = index3 + 1;
            string str = source2.GetElement<string>(index3) ?? string.Empty;
            List<string> source3 = list;
            int index4 = num4;
            int num5 = index4 + 1;
            string s2 = source3.GetElement<string>(index4) ?? string.Empty;
            List<string> source4 = list;
            int index5 = num5;
            int num6 = index5 + 1;
            string s3 = source4.GetElement<string>(index5) ?? string.Empty;
            List<string> source5 = list;
            int index6 = num6;
            int num7 = index6 + 1;
            string s4 = source5.GetElement<string>(index6) ?? string.Empty;
            List<string> source6 = list;
            int index7 = num7;
            int num8 = index7 + 1;
            string s5 = source6.GetElement<string>(index7) ?? string.Empty;
            DesireType index8;
            ChangeType _changeType;
            if (int.TryParse(s2, out result2) && int.TryParse(s3, out result3) && (this.AnimalDesireIDTable.TryGetValue(result2, out index8) && this.ChangeTypeIDTable.TryGetValue(result3, out _changeType)) && (bool.TryParse(str, out result6) && float.TryParse(s4, out result4)))
            {
              float _maxRange = !float.TryParse(s5, out result5) ? result4 : result5;
              if (!this.DesireResultTable.ContainsKey(_animalType))
                this.DesireResultTable[_animalType] = new Dictionary<BreedingTypes, Dictionary<DesireType, Dictionary<bool, Dictionary<DesireType, ChangeParamState>>>>();
              if (!this.DesireResultTable[_animalType].ContainsKey(_breedingType))
                this.DesireResultTable[_animalType][_breedingType] = new Dictionary<DesireType, Dictionary<bool, Dictionary<DesireType, ChangeParamState>>>();
              if (!this.DesireResultTable[_animalType][_breedingType].ContainsKey(key))
                this.DesireResultTable[_animalType][_breedingType][key] = new Dictionary<bool, Dictionary<DesireType, ChangeParamState>>();
              if (!this.DesireResultTable[_animalType][_breedingType][key].ContainsKey(result6))
                this.DesireResultTable[_animalType][_breedingType][key][result6] = new Dictionary<DesireType, ChangeParamState>();
              this.DesireResultTable[_animalType][_breedingType][key][result6][index8] = new ChangeParamState(_changeType, result4, _maxRange);
            }
          }
        }
      }

      private void LoadWithActor(AnimalDefinePack _animalDefinePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(_animalDefinePack.AssetBundleNames.WithActorAnimeInfoListBundleDirectory, false);
        if (((IReadOnlyList<string>) nameListFromPath).IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          if (AssetBundleCheck.IsFile(str, withoutExtension))
          {
            ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
              {
                List<string> list = excelData.list[index2].list;
                if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
                {
                  int num1 = 0;
                  List<string> source1 = list;
                  int index3 = num1;
                  int num2 = index3 + 1;
                  int result;
                  if (int.TryParse(source1.GetElement<string>(index3) ?? string.Empty, out result))
                  {
                    int num3 = num2 + 1;
                    AssetBundleInfo _sheetInfo;
                    ref AssetBundleInfo local = ref _sheetInfo;
                    string empty = string.Empty;
                    List<string> source2 = list;
                    int index4 = num3;
                    int num4 = index4 + 1;
                    string element1 = source2.GetElement<string>(index4);
                    List<string> source3 = list;
                    int index5 = num4;
                    int num5 = index5 + 1;
                    string element2 = source3.GetElement<string>(index5);
                    List<string> source4 = list;
                    int index6 = num5;
                    int num6 = index6 + 1;
                    string element3 = source4.GetElement<string>(index6);
                    ((AssetBundleInfo) ref local).\u002Ector(empty, element1, element2, element3);
                    if (!((string) _sheetInfo.assetbundle).IsNullOrEmpty() && !((string) _sheetInfo.asset).IsNullOrEmpty() && result == 0)
                      this.LoadWithAgentAnimationList(_sheetInfo, _animalDefinePack);
                  }
                }
              }
            }
          }
        }
      }

      private void LoadWithAgentAnimationList(
        AssetBundleInfo _sheetInfo,
        AnimalDefinePack _animalDefinePack)
      {
        if (((string) _sheetInfo.assetbundle).IsNullOrEmpty() || ((string) _sheetInfo.asset).IsNullOrEmpty())
          return;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num1 = 1;
            ref AssetBundleInfo local = ref _sheetInfo;
            string empty = string.Empty;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            string element1 = source1.GetElement<string>(index2);
            List<string> source2 = list;
            int index3 = num2;
            int num3 = index3 + 1;
            string element2 = source2.GetElement<string>(index3);
            List<string> source3 = list;
            int index4 = num3;
            int num4 = index4 + 1;
            string element3 = source3.GetElement<string>(index4);
            ((AssetBundleInfo) ref local).\u002Ector(empty, element1, element2, element3);
            if (!((string) _sheetInfo.assetbundle).IsNullOrEmpty() && !((string) _sheetInfo.asset).IsNullOrEmpty())
              this.LoadWithAgentAnimationState(_sheetInfo, _animalDefinePack);
          }
        }
      }

      private void LoadWithAgentAnimationState(
        AssetBundleInfo _sheetInfo,
        AnimalDefinePack _animalDefinePack)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null) || excelData.MaxCell == 0)
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num1 = 2;
            int result1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1) && result1 >= 0)
            {
              int result2 = 0;
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                string str1 = source3.GetElement<string>(index4) ?? string.Empty;
                List<string> source4 = list;
                int index5 = num4;
                int num5 = index5 + 1;
                string str2 = source4.GetElement<string>(index5) ?? string.Empty;
                List<string> source5 = list;
                int index6 = num5;
                int num6 = index6 + 1;
                string[] _inStateNames = (source5.GetElement<string>(index6) ?? string.Empty).Split(this.separators, StringSplitOptions.RemoveEmptyEntries);
                List<string> source6 = list;
                int index7 = num6;
                int num7 = index7 + 1;
                bool result3;
                if (!bool.TryParse(source6.GetElement<string>(index7) ?? string.Empty, out result3))
                  result3 = false;
                List<string> source7 = list;
                int index8 = num7;
                int num8 = index8 + 1;
                float result4;
                if (!float.TryParse(source7.GetElement<string>(index8) ?? string.Empty, out result4))
                  result4 = 0.0f;
                List<string> source8 = list;
                int index9 = num8;
                int num9 = index9 + 1;
                string[] _outStateNames = (source8.GetElement<string>(index9) ?? string.Empty).Split(this.separators, StringSplitOptions.RemoveEmptyEntries);
                List<string> source9 = list;
                int index10 = num9;
                int num10 = index10 + 1;
                bool result5;
                if (!bool.TryParse(source9.GetElement<string>(index10) ?? string.Empty, out result5))
                  result5 = false;
                List<string> source10 = list;
                int index11 = num10;
                int num11 = index11 + 1;
                float result6;
                if (!float.TryParse(source10.GetElement<string>(index11) ?? string.Empty, out result6))
                  result6 = 0.0f;
                List<string> source11 = list;
                int index12 = num11;
                int num12 = index12 + 1;
                int result7;
                if (!int.TryParse(source11.GetElement<string>(index12) ?? string.Empty, out result7))
                  result7 = 0;
                List<string> source12 = list;
                int index13 = num12;
                int num13 = index13 + 1;
                int result8;
                if (!int.TryParse(source12.GetElement<string>(index13) ?? string.Empty, out result8))
                  result8 = 0;
                List<string> source13 = list;
                int index14 = num13;
                int num14 = index14 + 1;
                int result9;
                if (!int.TryParse(source13.GetElement<string>(index14) ?? string.Empty, out result9))
                  result9 = 0;
                float actionPointDistance = _animalDefinePack.WithActorInfo.ActionPointDistance;
                float result10 = 0.0f;
                List<string> source14 = list;
                int index15 = num14;
                int num15 = index15 + 1;
                if (!float.TryParse(source14.GetElement<string>(index15) ?? string.Empty, out result10))
                  result10 = actionPointDistance;
                float result11 = 0.0f;
                List<string> source15 = list;
                int index16 = num15;
                int num16 = index16 + 1;
                if (!float.TryParse(source15.GetElement<string>(index16) ?? string.Empty, out result11))
                  result11 = actionPointDistance;
                float result12 = 0.0f;
                List<string> source16 = list;
                int index17 = num16;
                int num17 = index17 + 1;
                if (!float.TryParse(source16.GetElement<string>(index17) ?? string.Empty, out result12))
                  result12 = actionPointDistance;
                AnimalPlayState animalPlayState = new AnimalPlayState(result9, result2, _inStateNames, _outStateNames)
                {
                  FloatList = new float[3]
                  {
                    result10,
                    result11,
                    result12
                  }
                };
                AnimalPlayState.PlayStateInfo mainStateInfo = animalPlayState.MainStateInfo;
                mainStateInfo.AssetBundleInfo = new AssetBundleInfo(string.Empty, str1, str2, string.Empty);
                mainStateInfo.InFadeEnable = result3;
                mainStateInfo.InFadeSecond = result4;
                mainStateInfo.OutFadeEnable = result5;
                mainStateInfo.OutFadeSecond = result6;
                mainStateInfo.IsLoop = 0 < result7 || 0 < result8;
                mainStateInfo.LoopMin = result7;
                mainStateInfo.LoopMax = result8;
                mainStateInfo.Controller = this.LoadAnimator(mainStateInfo.AssetBundleInfo);
                Dictionary<int, AnimalPlayState> dictionary1;
                if (!this.WithAgentAnimeTable.TryGetValue(result1, out dictionary1) || dictionary1 == null)
                {
                  Dictionary<int, AnimalPlayState> dictionary2 = new Dictionary<int, AnimalPlayState>();
                  this.WithAgentAnimeTable[result1] = dictionary2;
                  dictionary1 = dictionary2;
                }
                dictionary1[result2] = animalPlayState;
              }
            }
          }
        }
      }

      private void LoadPlayerInfo(AnimalDefinePack _definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(_definePack.AssetBundleNames.PlayerInfoListBundleDirectory, false);
        if (((IReadOnlyList<string>) nameListFromPath).IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          if (AssetBundleCheck.IsFile(str, withoutExtension))
          {
            ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
              {
                List<string> list = excelData.list[index2].list;
                if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
                {
                  int num1 = 0;
                  List<string> source1 = list;
                  int index3 = num1;
                  int num2 = index3 + 1;
                  int result;
                  if (int.TryParse(source1.GetElement<string>(index3) ?? string.Empty, out result))
                  {
                    int num3 = num2 + 1;
                    AssetBundleInfo _sheetInfo;
                    ref AssetBundleInfo local = ref _sheetInfo;
                    string empty = string.Empty;
                    List<string> source2 = list;
                    int index4 = num3;
                    int num4 = index4 + 1;
                    string element1 = source2.GetElement<string>(index4);
                    List<string> source3 = list;
                    int index5 = num4;
                    int num5 = index5 + 1;
                    string element2 = source3.GetElement<string>(index5);
                    List<string> source4 = list;
                    int index6 = num5;
                    int num6 = index6 + 1;
                    string element3 = source4.GetElement<string>(index6);
                    ((AssetBundleInfo) ref local).\u002Ector(empty, element1, element2, element3);
                    if (!((string) _sheetInfo.assetbundle).IsNullOrEmpty() && !((string) _sheetInfo.asset).IsNullOrEmpty() && result == 0)
                      this.LoadPlayerCatchAnimalPoseTable(_sheetInfo);
                  }
                }
              }
            }
          }
        }
      }

      private void LoadPlayerCatchAnimalPoseTable(AssetBundleInfo _sheetInfo)
      {
        if (((string) _sheetInfo.assetbundle).IsNullOrEmpty() || ((string) _sheetInfo.asset).IsNullOrEmpty())
          return;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num1 = 1;
            AssetBundleInfo _sheetInfo1;
            ref AssetBundleInfo local = ref _sheetInfo1;
            string empty = string.Empty;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            string element1 = source1.GetElement<string>(index2);
            List<string> source2 = list;
            int index3 = num2;
            int num3 = index3 + 1;
            string element2 = source2.GetElement<string>(index3);
            List<string> source3 = list;
            int index4 = num3;
            int num4 = index4 + 1;
            string element3 = source3.GetElement<string>(index4);
            ((AssetBundleInfo) ref local).\u002Ector(empty, element1, element2, element3);
            if (!((string) _sheetInfo1.assetbundle).IsNullOrEmpty() && !((string) _sheetInfo1.asset).IsNullOrEmpty())
              this.LoadPlayerCatchAnimalPoseList(_sheetInfo1);
          }
        }
      }

      private void LoadPlayerCatchAnimalPoseList(AssetBundleInfo _sheetInfo)
      {
        if (((string) _sheetInfo.assetbundle).IsNullOrEmpty() || ((string) _sheetInfo.asset).IsNullOrEmpty())
          return;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num1 = 2;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                int result3;
                if (int.TryParse(source3.GetElement<string>(index4) ?? string.Empty, out result3))
                {
                  List<string> source4 = list;
                  int index5 = num4;
                  int num5 = index5 + 1;
                  int result4;
                  if (int.TryParse(source4.GetElement<string>(index5) ?? string.Empty, out result4))
                  {
                    Dictionary<int, PoseKeyPair> dictionary;
                    if (!this.PlayerCatchAnimalPoseTable.TryGetValue(result1, out dictionary))
                      this.PlayerCatchAnimalPoseTable[result1] = dictionary = new Dictionary<int, PoseKeyPair>();
                    dictionary[result2] = new PoseKeyPair()
                    {
                      postureID = result3,
                      poseID = result4
                    };
                  }
                }
              }
            }
          }
        }
      }

      private void LoadAnimalPoint(AnimalDefinePack _define)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(_define.AssetBundleNames.AnimalPointPrefabBundleDirectory, false);
        if (((IReadOnlyList<string>) nameListFromPath).IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          if (AssetBundleCheck.IsFile(str, withoutExtension))
          {
            ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              int num1 = 1;
              List<ExcelData.Param> list = excelData.list;
              int index2 = num1;
              int num2 = index2 + 1;
              this.LoadAnimalPointPrefabInfo(list.GetElement<ExcelData.Param>(index2)?.list, withoutExtension);
            }
          }
        }
      }

      private void LoadAnimalPointPrefabInfo(List<string> _pathRow, string _ver)
      {
        if (((IReadOnlyList<string>) _pathRow).IsNullOrEmpty<string>())
          return;
        int num1 = 1;
        AssetBundleInfo info;
        ref AssetBundleInfo local = ref info;
        string empty = string.Empty;
        List<string> source1 = _pathRow;
        int index1 = num1;
        int num2 = index1 + 1;
        string str1 = source1.GetElement<string>(index1) ?? string.Empty;
        List<string> source2 = _pathRow;
        int index2 = num2;
        int num3 = index2 + 1;
        string str2 = source2.GetElement<string>(index2) ?? string.Empty;
        List<string> source3 = _pathRow;
        int index3 = num3;
        int num4 = index3 + 1;
        string str3 = source3.GetElement<string>(index3) ?? string.Empty;
        ((AssetBundleInfo) ref local).\u002Ector(empty, str1, str2, str3);
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(info);
        if (Object.op_Equality((Object) excelData, (Object) null) || excelData.MaxCell <= 1)
          return;
        for (int index4 = 1; index4 < excelData.MaxCell; ++index4)
        {
          List<string> list = excelData.list[index4].list;
          if (!((IReadOnlyList<string>) list).IsNullOrEmpty<string>())
          {
            int num5 = 0;
            List<string> source4 = list;
            int index5 = num5;
            int num6 = index5 + 1;
            string str4 = source4.GetElement<string>(index5) ?? string.Empty;
            int result1 = 0;
            List<string> source5 = list;
            int index6 = num6;
            int num7 = index6 + 1;
            if (int.TryParse(source5.GetElement<string>(index6) ?? string.Empty, out result1))
            {
              int result2 = 0;
              List<string> source6 = list;
              int index7 = num7;
              int num8 = index7 + 1;
              if (int.TryParse(source6.GetElement<string>(index7) ?? string.Empty, out result2))
              {
                List<string> source7 = list;
                int index8 = num8;
                int num9 = index8 + 1;
                string str5 = source7.GetElement<string>(index8) ?? string.Empty;
                List<string> source8 = list;
                int index9 = num9;
                int num10 = index9 + 1;
                string str6 = source8.GetElement<string>(index9) ?? string.Empty;
                List<string> source9 = list;
                int index10 = num10;
                int num11 = index10 + 1;
                string str7 = source9.GetElement<string>(index10) ?? string.Empty;
                if (!this.AnimalPointAssetTable.ContainsKey(result1))
                  this.AnimalPointAssetTable[result1] = new Dictionary<int, AssetBundleInfo>();
                this.AnimalPointAssetTable[result1][result2] = new AssetBundleInfo(str4, str5, str6, str7);
              }
            }
          }
        }
      }

      public Tuple<string, string, string> GetName(
        List<string> _row,
        int _bundleIndex,
        int _assetIndex,
        int _manifestIndex)
      {
        return new Tuple<string, string, string>(_row.GetElement<string>(_bundleIndex) ?? string.Empty, _row.GetElement<string>(_assetIndex) ?? string.Empty, _row.GetElement<string>(_manifestIndex) ?? string.Empty);
      }

      public T LoadAsset<T>(
        List<string> _row,
        int _bundleIndex,
        int _assetIndex,
        int _manifestIndex)
        where T : Object
      {
        Tuple<string, string, string> name = this.GetName(_row, _bundleIndex, _assetIndex, _manifestIndex);
        return AssetUtility.LoadAsset<T>(name.Item1, name.Item2, name.Item3);
      }

      private bool TryStringToEnum<T>(string _name, out T _value) where T : struct
      {
        return Enum.TryParse<T>(_name, true, out _value);
      }
    }

    public class AnimationTables
    {
      private static readonly string[] _separators = new string[2]
      {
        "/",
        "／"
      };
      private static readonly char[] _comma = new char[1]
      {
        ','
      };
      private static readonly string[] _seEventSepa = new string[1]
      {
        ">"
      };
      private static readonly string[] _seEventRemoveStr = new string[2]
      {
        "<",
        ">"
      };
      private Dictionary<int, AssetBundleInfo> _playerAnimatorAssetTable = new Dictionary<int, AssetBundleInfo>();
      private Dictionary<int, AssetBundleInfo> _charaAnimatorAssetTable = new Dictionary<int, AssetBundleInfo>();
      private Dictionary<int, AssetBundleInfo> _merchantAnimatorAssetTable = new Dictionary<int, AssetBundleInfo>();
      private Dictionary<int, AssetBundleInfo> _itemAnimatorAssetTable = new Dictionary<int, AssetBundleInfo>();
      private Regex _regex = new Regex("<((?:[\\w/.-]*[,]*)+)>");

      public RuntimeAnimatorController GetPlayerAnimator(
        int id,
        ref AssetBundleInfo outInfo)
      {
        AssetBundleInfo info;
        if (!this._playerAnimatorAssetTable.TryGetValue(id, out info))
          return (RuntimeAnimatorController) null;
        outInfo = info;
        return AssetUtility.LoadAsset<RuntimeAnimatorController>(info);
      }

      public RuntimeAnimatorController GetCharaAnimator(
        int id,
        ref AssetBundleInfo outInfo)
      {
        AssetBundleInfo info;
        if (!this._charaAnimatorAssetTable.TryGetValue(id, out info))
          return (RuntimeAnimatorController) null;
        outInfo = info;
        return AssetUtility.LoadAsset<RuntimeAnimatorController>(info);
      }

      public RuntimeAnimatorController GetMerchantAnimator(
        int id,
        ref AssetBundleInfo outInfo)
      {
        AssetBundleInfo info;
        if (!this._merchantAnimatorAssetTable.TryGetValue(id, out info))
          return (RuntimeAnimatorController) null;
        outInfo = info;
        return AssetUtility.LoadAsset<RuntimeAnimatorController>(info);
      }

      public RuntimeAnimatorController GetItemAnimator(int id)
      {
        AssetBundleInfo info;
        return !this._itemAnimatorAssetTable.TryGetValue(id, out info) ? (RuntimeAnimatorController) null : AssetUtility.LoadAsset<RuntimeAnimatorController>(info);
      }

      public Dictionary<int, Dictionary<int, List<int>>> PersonalActionListTable { get; private set; } = new Dictionary<int, Dictionary<int, List<int>>>();

      public Dictionary<int, Dictionary<int, PlayState>> AgentActionAnimTable { get; private set; } = new Dictionary<int, Dictionary<int, PlayState>>();

      public Dictionary<int, List<AnimeMoveInfo>> AgentMoveInfoTable { get; private set; } = new Dictionary<int, List<AnimeMoveInfo>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>> AgentItemEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>> AgentAnimalEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>>> AgentActSEEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeOnceVoiceEventInfo>>>> AgentActOnceVoiceEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeOnceVoiceEventInfo>>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<int>>>> AgentActLoopVoiceEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<int>>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>>> AgentActParticleEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>> AgentChangeClothEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>>();

      public Dictionary<string, List<PlayState.ItemInfo>> SurpriseItemList { get; private set; } = new Dictionary<string, List<PlayState.ItemInfo>>();

      public Dictionary<int, PlayState> AgentLocomotionStateTable { get; private set; } = new Dictionary<int, PlayState>();

      public Dictionary<int, PoseKeyPair> TalkSpeakerStateTable { get; private set; } = new Dictionary<int, PoseKeyPair>();

      public Dictionary<int, PoseKeyPair> TalkListenerStateTable { get; private set; } = new Dictionary<int, PoseKeyPair>();

      public Dictionary<int, int> TalkSpeakerRelationTable { get; private set; } = new Dictionary<int, int>();

      public Dictionary<int, int> TalkListenerRelationTable { get; private set; } = new Dictionary<int, int>();

      public Dictionary<int, Manager.Resources.TriValues> ItemScaleTable { get; private set; } = new Dictionary<int, Manager.Resources.TriValues>();

      public Dictionary<int, Dictionary<int, PlayState>> WithAnimalStateTable { get; private set; } = new Dictionary<int, Dictionary<int, PlayState>>();

      public Dictionary<int, FootStepInfo[]> AgentFootStepEventKeyTable { get; private set; } = new Dictionary<int, FootStepInfo[]>();

      public Dictionary<int, Dictionary<int, ValueTuple<PoseKeyPair, bool>>> AgentGravurePoseTable { get; private set; } = new Dictionary<int, Dictionary<int, ValueTuple<PoseKeyPair, bool>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, PlayState>>> PlayerActionAnimTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, PlayState>>>();

      public Dictionary<int, Dictionary<int, List<AnimeMoveInfo>>> PlayerMoveInfoTable { get; private set; } = new Dictionary<int, Dictionary<int, List<AnimeMoveInfo>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>>> PlayerItemEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>>> PlayerActExEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>>>> PlayerActSEEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>>>> PlayerActParticleEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>>>>();

      public Dictionary<int, Dictionary<int, PlayState>> PlayerLocomotionStateTable { get; private set; } = new Dictionary<int, Dictionary<int, PlayState>>();

      public Dictionary<int, Dictionary<int, FootStepInfo[]>> PlayerFootStepEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, FootStepInfo[]>>();

      public Dictionary<int, Dictionary<int, PlayState>> MerchantOnlyActionAnimStateTable { get; private set; } = new Dictionary<int, Dictionary<int, PlayState>>();

      public Dictionary<int, Dictionary<int, PlayState>> MerchantCommonActionAnimStateTable { get; private set; } = new Dictionary<int, Dictionary<int, PlayState>>();

      public Dictionary<int, PlayState> MerchantLocomotionStateTable { get; private set; } = new Dictionary<int, PlayState>();

      public Dictionary<int, List<AnimeMoveInfo>> MerchantMoveInfoTable { get; private set; } = new Dictionary<int, List<AnimeMoveInfo>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>> MerchantOnlyItemEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>> MerchantCommonItemEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>>> MerchantOnlySEEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>>> MerchantCommonSEEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeOnceVoiceEventInfo>>>> MerchantOnlyOnceVoiceEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeOnceVoiceEventInfo>>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeOnceVoiceEventInfo>>>> MerchantCommonOnceVoiceEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeOnceVoiceEventInfo>>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<int>>>> MerchantOnlyLoopVoiceEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<int>>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<int>>>> MerchantCommonLoopVoiceEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<int>>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>>> MerchantOnlyParticleEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>>> MerchantCommonParticleEventKeyTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>>>();

      public Dictionary<int, PoseKeyPair> MerchantListenerStateTable { get; private set; } = new Dictionary<int, PoseKeyPair>();

      public Dictionary<int, int> MerchantListenerRelationTable { get; private set; } = new Dictionary<int, int>();

      public Dictionary<int, FootStepInfo[]> MerchantFootStepEventKeyTable { get; private set; } = new Dictionary<int, FootStepInfo[]>();

      public void SyncLoad(DefinePack definePack)
      {
        if (Object.op_Equality((Object) definePack, (Object) null))
          return;
        this.Load(definePack, false);
      }

      public void Load(DefinePack definePack, bool awaitable = true)
      {
        this.LoadAnimatorAssetBundles(definePack, awaitable);
        List<string> nameListFromPath1 = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.AgentAnimeInfo, false);
        nameListFromPath1.Sort();
        for (int id = 0; id < nameListFromPath1.Count; ++id)
        {
          string str = nameListFromPath1[id];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            int num = 1;
            while (num < excelData.MaxCell)
            {
              ExcelData.Param obj = excelData.list[num++];
              int result;
              if (int.TryParse(obj.list.GetElement<string>(0), out result))
              {
                switch (result)
                {
                  case 0:
                    this.LoadAgentActionPersonalIDs(obj.list, id, awaitable);
                    continue;
                  case 1:
                    this.LoadAgentActionStates(obj.list, id, awaitable);
                    continue;
                  case 2:
                    this.LoadAgentMoveInfoList(obj.list, id, awaitable);
                    continue;
                  case 3:
                    this.LoadAgentEventKeyList(obj.list, id, awaitable);
                    continue;
                  case 4:
                    this.LoadAgentLocomotionStateList(obj.list, id, awaitable);
                    continue;
                  case 7:
                    this.LoadTalkSpeakerStateList(obj.list, id, awaitable);
                    continue;
                  case 8:
                    this.LoadTalkListenerStateList(obj.list, id, awaitable);
                    continue;
                  case 9:
                    this.LoadWithAnimalStateList(obj.list, id, awaitable);
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
        }
        List<string> nameListFromPath2 = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.PlayerMaleAnimeInfo, false);
        nameListFromPath2.Sort();
        for (int id = 0; id < nameListFromPath2.Count; ++id)
        {
          string str = nameListFromPath2[id];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            int num = 1;
            while (num < excelData.MaxCell)
            {
              ExcelData.Param obj = excelData.list[num++];
              int result;
              if (int.TryParse(obj.list[0], out result))
              {
                switch (result)
                {
                  case 0:
                    this.LoadPlayerActionStates(obj.list, id, 0, awaitable);
                    continue;
                  case 1:
                    this.LoadPlayerMoveInfoList(obj.list, id, 0, awaitable);
                    continue;
                  case 2:
                    this.LoadPlayerEventKeyList(obj.list, id, 0, awaitable);
                    continue;
                  case 3:
                    this.LoadPlayerLocomotionStateList(obj.list, id, 0, awaitable);
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
        }
        List<string> nameListFromPath3 = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.PlayerFemaleAnimeInfo, false);
        nameListFromPath3.Sort();
        for (int id = 0; id < nameListFromPath3.Count; ++id)
        {
          string str = nameListFromPath3[id];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            int num = 1;
            while (num < excelData.MaxCell)
            {
              ExcelData.Param obj = excelData.list[num++];
              int result;
              if (int.TryParse(obj.list.GetElement<string>(0), out result))
              {
                switch (result)
                {
                  case 0:
                    this.LoadPlayerActionStates(obj.list, id, 1, awaitable);
                    continue;
                  case 1:
                    this.LoadPlayerMoveInfoList(obj.list, id, 1, awaitable);
                    continue;
                  case 2:
                    this.LoadPlayerEventKeyList(obj.list, id, 1, awaitable);
                    continue;
                  case 3:
                    this.LoadPlayerLocomotionStateList(obj.list, id, 1, awaitable);
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
        }
        List<string> nameListFromPath4 = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.MerchantAnimeInfo, false);
        nameListFromPath4.Sort();
        for (int index = 0; index < nameListFromPath4.Count; ++index)
        {
          string str = nameListFromPath4[index];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            int num = 1;
            while (num < excelData.MaxCell)
            {
              ExcelData.Param obj = excelData.list[num++];
              int result;
              if (int.TryParse(obj.list.GetElement<string>(0) ?? string.Empty, out result))
              {
                List<string> list = obj.list;
                switch (result)
                {
                  case 0:
                    this.LoadMerchantOnlyActionAnimState(list, withoutExtension, awaitable);
                    continue;
                  case 1:
                    this.LoadMerchantCommonActionState(list, withoutExtension, awaitable);
                    continue;
                  case 2:
                    this.LoadMerchantLocomotionStateList(list, withoutExtension, awaitable);
                    continue;
                  case 3:
                    this.LoadMerchantMoveInfoList(list, withoutExtension, awaitable);
                    continue;
                  case 4:
                    this.LoadMerchantEventKeyList(list, withoutExtension, awaitable);
                    continue;
                  case 5:
                    this.LoadMerchantListenerStateList(list, withoutExtension, awaitable);
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
        }
        this.LoadItemScaleTable(definePack);
        this.LoadAgentGravurePoseTable(definePack, awaitable);
        this.LoadAgentEventKeyTable(definePack, awaitable);
        this.LoadChangeClothEventKeyTable(definePack, awaitable);
        this.LoadPlayerEventKeyTable(definePack, awaitable);
        this.LoadSurpriseItemList(definePack);
        Singleton<Manager.Resources>.Instance.LoadMapIK(definePack);
      }

      public void Release()
      {
        this._playerAnimatorAssetTable.Clear();
        this._charaAnimatorAssetTable.Clear();
        this._merchantAnimatorAssetTable.Clear();
        this._itemAnimatorAssetTable.Clear();
        this.PersonalActionListTable.Clear();
        this.AgentActionAnimTable.Clear();
        this.AgentMoveInfoTable.Clear();
        this.AgentItemEventKeyTable.Clear();
        this.AgentAnimalEventKeyTable.Clear();
        this.AgentActSEEventKeyTable.Clear();
        this.AgentActOnceVoiceEventKeyTable.Clear();
        this.AgentActLoopVoiceEventKeyTable.Clear();
        this.AgentActParticleEventKeyTable.Clear();
        this.AgentChangeClothEventKeyTable.Clear();
        this.SurpriseItemList.Clear();
        this.AgentLocomotionStateTable.Clear();
        this.TalkSpeakerStateTable.Clear();
        this.TalkListenerStateTable.Clear();
        this.TalkSpeakerRelationTable.Clear();
        this.TalkListenerRelationTable.Clear();
        this.ItemScaleTable.Clear();
        this.WithAnimalStateTable.Clear();
        this.AgentFootStepEventKeyTable.Clear();
        this.AgentGravurePoseTable.Clear();
        this.PlayerActionAnimTable.Clear();
        this.PlayerMoveInfoTable.Clear();
        this.PlayerItemEventKeyTable.Clear();
        this.PlayerActExEventKeyTable.Clear();
        this.PlayerActSEEventKeyTable.Clear();
        this.PlayerActParticleEventKeyTable.Clear();
        this.PlayerLocomotionStateTable.Clear();
        this.PlayerFootStepEventKeyTable.Clear();
        this.MerchantOnlyActionAnimStateTable.Clear();
        this.MerchantCommonActionAnimStateTable.Clear();
        this.MerchantLocomotionStateTable.Clear();
        this.MerchantMoveInfoTable.Clear();
        this.MerchantOnlyItemEventKeyTable.Clear();
        this.MerchantCommonItemEventKeyTable.Clear();
        this.MerchantOnlySEEventKeyTable.Clear();
        this.MerchantCommonSEEventKeyTable.Clear();
        this.MerchantOnlyOnceVoiceEventKeyTable.Clear();
        this.MerchantCommonOnceVoiceEventKeyTable.Clear();
        this.MerchantOnlyLoopVoiceEventKeyTable.Clear();
        this.MerchantCommonLoopVoiceEventKeyTable.Clear();
        this.MerchantOnlyParticleEventKeyTable.Clear();
        this.MerchantCommonParticleEventKeyTable.Clear();
        this.MerchantListenerStateTable.Clear();
        this.MerchantListenerRelationTable.Clear();
        this.MerchantFootStepEventKeyTable.Clear();
        Singleton<Manager.Resources>.Instance.MapIKData.Clear();
      }

      private void LoadAnimatorAssetBundles(DefinePack definePack, bool awaitable)
      {
        if (Debug.get_isDebugBuild())
          Debug.Log((object) "アニメーターリスト");
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.ActorAnimatorList, false);
        nameListFromPath.Sort();
        for (int index = 0; index < nameListFromPath.Count; ++index)
        {
          string str = nameListFromPath[index];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            int num = 1;
            while (num < excelData.MaxCell)
            {
              ExcelData.Param obj = excelData.list[num++];
              int result;
              if (int.TryParse(obj.list.GetElement<string>(0), out result))
              {
                switch (result)
                {
                  case 0:
                    if (Debug.get_isDebugBuild())
                      Debug.Log((object) "キャラのアニメーターリスト");
                    this.LoadAnimator(obj.list, this._charaAnimatorAssetTable);
                    continue;
                  case 1:
                    if (Debug.get_isDebugBuild())
                      Debug.Log((object) "プレイヤーのアニメーターリスト");
                    this.LoadAnimator(obj.list, this._playerAnimatorAssetTable);
                    continue;
                  case 2:
                    if (Debug.get_isDebugBuild())
                      Debug.Log((object) "商人のアニメーターリスト");
                    this.LoadAnimator(obj.list, this._merchantAnimatorAssetTable);
                    continue;
                  case 3:
                    if (Debug.get_isDebugBuild())
                      Debug.Log((object) "マップアイテムのアニメーターリスト");
                    this.LoadAnimator(obj.list, this._itemAnimatorAssetTable);
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
        }
      }

      private void LoadAnimator(List<string> row, Dictionary<int, AssetBundleInfo> dictionary)
      {
        int num1 = 2;
        List<string> source1 = row;
        int index1 = num1;
        int num2 = index1 + 1;
        string element1 = source1.GetElement<string>(index1);
        List<string> source2 = row;
        int index2 = num2;
        int num3 = index2 + 1;
        string element2 = source2.GetElement<string>(index2);
        int num4 = num3 + 1;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(element1, element2, string.Empty);
        if (!Object.op_Inequality((Object) excelData, (Object) null))
          return;
        int num5 = 1;
        while (num5 < excelData.MaxCell)
        {
          ExcelData.Param obj = excelData.list[num5++];
          int num6 = 0;
          List<string> list1 = obj.list;
          int index3 = num6;
          int num7 = index3 + 1;
          int result;
          if (int.TryParse(list1.GetElement<string>(index3), out result))
          {
            List<string> list2 = obj.list;
            int index4 = num7;
            int num8 = index4 + 1;
            string element3 = list2.GetElement<string>(index4);
            List<string> list3 = obj.list;
            int index5 = num8;
            int num9 = index5 + 1;
            string element4 = list3.GetElement<string>(index5);
            List<string> list4 = obj.list;
            int index6 = num9;
            int num10 = index6 + 1;
            string element5 = list4.GetElement<string>(index6);
            List<string> list5 = obj.list;
            int index7 = num10;
            int num11 = index7 + 1;
            string element6 = list5.GetElement<string>(index7);
            Dictionary<int, AssetBundleInfo> dictionary1 = dictionary;
            int index8 = result;
            AssetBundleInfo assetBundleInfo1 = (AssetBundleInfo) null;
            assetBundleInfo1.name = (__Null) element3;
            assetBundleInfo1.assetbundle = (__Null) element4;
            assetBundleInfo1.asset = (__Null) element5;
            assetBundleInfo1.manifest = (__Null) element6;
            AssetBundleInfo assetBundleInfo2 = assetBundleInfo1;
            dictionary1[index8] = assetBundleInfo2;
          }
        }
      }

      private void LoadPlayerActionStates(List<string> row, int id, int sex, bool awaitable)
      {
        int num1 = 2;
        List<string> stringList1 = row;
        int index1 = num1;
        int num2 = index1 + 1;
        string assetbundleName1 = stringList1[index1];
        List<string> stringList2 = row;
        int index2 = num2;
        int num3 = index2 + 1;
        string assetName1 = stringList2[index2];
        List<string> stringList3 = row;
        int index3 = num3;
        int num4 = index3 + 1;
        string manifestName1 = stringList3[index3];
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(assetbundleName1, assetName1, manifestName1);
        if (!Object.op_Inequality((Object) excelData, (Object) null))
          return;
        Dictionary<int, Dictionary<int, PlayState>> dic;
        if (!this.PlayerActionAnimTable.TryGetValue(sex, out dic))
        {
          Dictionary<int, Dictionary<int, PlayState>> dictionary = new Dictionary<int, Dictionary<int, PlayState>>();
          this.PlayerActionAnimTable[sex] = dictionary;
          dic = dictionary;
        }
        for (int index4 = 1; index4 < excelData.MaxCell; ++index4)
        {
          ExcelData.Param obj = excelData.list[index4];
          if (!obj.list.IsNullOrEmpty<string>())
          {
            int num5 = 0;
            List<string> list1 = obj.list;
            int index5 = num5;
            int num6 = index5 + 1;
            if (!list1.GetElement<string>(index5).IsNullOrEmpty())
            {
              List<string> list2 = obj.list;
              int index6 = num6;
              int num7 = index6 + 1;
              string assetbundleName2 = list2[index6];
              List<string> list3 = obj.list;
              int index7 = num7;
              int num8 = index7 + 1;
              string assetName2 = list3[index7];
              List<string> list4 = obj.list;
              int index8 = num8;
              int num9 = index8 + 1;
              string manifestName2 = list4[index8];
              Manager.Resources.LoadActionAnimationInfo(AssetUtility.LoadAsset<ExcelData>(assetbundleName2, assetName2, manifestName2), dic, awaitable);
            }
          }
        }
      }

      private void LoadPlayerMoveInfoList(List<string> row, int id, int sex, bool awaitable)
      {
        int num1 = 2;
        List<string> source1 = row;
        int index1 = num1;
        int num2 = index1 + 1;
        string element1 = source1.GetElement<string>(index1);
        List<string> source2 = row;
        int index2 = num2;
        int num3 = index2 + 1;
        string element2 = source2.GetElement<string>(index2);
        List<string> source3 = row;
        int index3 = num3;
        int num4 = index3 + 1;
        source3.GetElement<string>(index3);
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(element1, element2, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        Dictionary<int, List<AnimeMoveInfo>> dictionary1;
        if (!this.PlayerMoveInfoTable.TryGetValue(sex, out dictionary1))
        {
          Dictionary<int, List<AnimeMoveInfo>> dictionary2 = new Dictionary<int, List<AnimeMoveInfo>>();
          this.PlayerMoveInfoTable[sex] = dictionary2;
          dictionary1 = dictionary2;
        }
        for (int index4 = 1; index4 < excelData.MaxCell; ++index4)
        {
          ExcelData.Param obj = excelData.list[index4];
          int num5 = 0;
          List<string> list1 = obj.list;
          int index5 = num5;
          int num6 = index5 + 1;
          int hash = Animator.StringToHash(list1.GetElement<string>(index5));
          List<AnimeMoveInfo> animeMoveInfoList1;
          if (!dictionary1.TryGetValue(hash, out animeMoveInfoList1))
          {
            List<AnimeMoveInfo> animeMoveInfoList2 = new List<AnimeMoveInfo>();
            dictionary1[hash] = animeMoveInfoList2;
            animeMoveInfoList1 = animeMoveInfoList2;
          }
          while (num6 < obj.list.Count)
          {
            List<string> list2 = obj.list;
            int index6 = num6;
            int num7 = index6 + 1;
            string element3 = list2.GetElement<string>(index6);
            List<string> list3 = obj.list;
            int index7 = num7;
            int num8 = index7 + 1;
            string element4 = list3.GetElement<string>(index7);
            List<string> list4 = obj.list;
            int index8 = num8;
            num6 = index8 + 1;
            string element5 = list4.GetElement<string>(index8);
            float result1;
            float result2;
            if (!element5.IsNullOrEmpty() && float.TryParse(element3, out result1) && float.TryParse(element4, out result2))
            {
              AnimeMoveInfo animeMoveInfo = new AnimeMoveInfo()
              {
                start = result1,
                end = result2,
                movePoint = element5
              };
              animeMoveInfoList1.Add(animeMoveInfo);
            }
          }
        }
      }

      private void LoadPlayerEventKeyList(List<string> row, int id, int sex, bool awaitable)
      {
        int num1 = 2;
        List<string> source1 = row;
        int index1 = num1;
        int num2 = index1 + 1;
        string element1 = source1.GetElement<string>(index1);
        List<string> source2 = row;
        int index2 = num2;
        int num3 = index2 + 1;
        string element2 = source2.GetElement<string>(index2);
        List<string> source3 = row;
        int index3 = num3;
        int num4 = index3 + 1;
        source3.GetElement<string>(index3);
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(element1, element2, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index4 = 1; index4 < excelData.MaxCell; ++index4)
        {
          ExcelData.Param obj = excelData.list[index4];
          int num5 = 1;
          List<string> list1 = obj.list;
          int index5 = num5;
          int num6 = index5 + 1;
          string element3 = list1.GetElement<string>(index5);
          List<string> list2 = obj.list;
          int index6 = num6;
          int num7 = index6 + 1;
          string element4 = list2.GetElement<string>(index6);
          if (!element3.IsNullOrEmpty() && !element4.IsNullOrEmpty())
          {
            if (element4.Contains("item"))
            {
              Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>> eventKeyTable;
              if (!this.PlayerItemEventKeyTable.TryGetValue(sex, out eventKeyTable))
              {
                Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>> dictionary = new Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>>();
                this.PlayerItemEventKeyTable[sex] = dictionary;
                eventKeyTable = dictionary;
              }
              this.LoadEventKeyTable(obj.list, eventKeyTable, awaitable);
            }
            else if (element4.Contains("actex"))
            {
              Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>> eventKeyTable;
              if (!this.PlayerActExEventKeyTable.TryGetValue(sex, out eventKeyTable))
              {
                Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>> dictionary = new Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>>();
                this.PlayerActExEventKeyTable[sex] = dictionary;
                eventKeyTable = dictionary;
              }
              this.LoadEventKeyTable(obj.list, eventKeyTable, awaitable);
            }
          }
        }
      }

      private void LoadPlayerLocomotionStateList(
        List<string> row,
        int id,
        int sex,
        bool awaitable)
      {
        int num1 = 2;
        List<string> stringList1 = row;
        int index1 = num1;
        int num2 = index1 + 1;
        string assetbundleName = stringList1[index1];
        List<string> stringList2 = row;
        int index2 = num2;
        int num3 = index2 + 1;
        string assetName = stringList2[index2];
        List<string> stringList3 = row;
        int index3 = num3;
        int num4 = index3 + 1;
        string manifestName = stringList3[index3];
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(assetbundleName, assetName, manifestName);
        if (!Object.op_Inequality((Object) excelData, (Object) null))
          return;
        Dictionary<int, PlayState> dictionary = new Dictionary<int, PlayState>();
        this.PlayerLocomotionStateTable[sex] = dictionary;
        Dictionary<int, PlayState> dic = dictionary;
        this.LoadLocomotionStateList(excelData, dic, awaitable);
      }

      private void LoadAgentActionPersonalIDs(List<string> row, int id, bool awaitable)
      {
        int num1 = 2;
        List<string> stringList1 = row;
        int index1 = num1;
        int num2 = index1 + 1;
        string assetbundleName1 = stringList1[index1];
        List<string> stringList2 = row;
        int index2 = num2;
        int num3 = index2 + 1;
        string assetName1 = stringList2[index2];
        ExcelData excelData1 = AssetUtility.LoadAsset<ExcelData>(assetbundleName1, assetName1, string.Empty);
        if (!Object.op_Inequality((Object) excelData1, (Object) null))
          return;
        foreach (ExcelData.Param obj1 in excelData1.list)
        {
          if (!obj1.list.IsNullOrEmpty<string>())
          {
            int num4 = 1;
            List<string> list1 = obj1.list;
            int index3 = num4;
            int num5 = index3 + 1;
            string assetbundleName2 = list1[index3];
            List<string> list2 = obj1.list;
            int index4 = num5;
            int num6 = index4 + 1;
            string assetName2 = list2[index4];
            ExcelData excelData2 = AssetUtility.LoadAsset<ExcelData>(assetbundleName2, assetName2, string.Empty);
            if (!Object.op_Equality((Object) excelData2, (Object) null))
            {
              foreach (ExcelData.Param obj2 in excelData2.list)
              {
                int num7 = 1;
                List<string> list3 = obj2.list;
                int index5 = num7;
                int num8 = index5 + 1;
                int result1;
                if (int.TryParse(list3.GetElement<string>(index5), out result1))
                {
                  Dictionary<int, List<int>> dictionary1;
                  if (!this.PersonalActionListTable.TryGetValue(result1, out dictionary1))
                  {
                    Dictionary<int, List<int>> dictionary2 = new Dictionary<int, List<int>>();
                    this.PersonalActionListTable[result1] = dictionary2;
                    dictionary1 = dictionary2;
                  }
                  List<string> list4 = obj2.list;
                  int index6 = num8;
                  int index7 = index6 + 1;
                  int result2;
                  if (int.TryParse(list4.GetElement<string>(index6), out result2))
                  {
                    List<int> intList1;
                    if (!dictionary1.TryGetValue(result2, out intList1))
                    {
                      List<int> intList2 = new List<int>();
                      dictionary1[result2] = intList2;
                      intList1 = intList2;
                    }
                    string element = obj2.list.GetElement<string>(index7);
                    if (!element.IsNullOrEmpty())
                    {
                      foreach (string s in element.Split(Manager.Resources.AnimationTables._comma, StringSplitOptions.RemoveEmptyEntries))
                      {
                        int result3;
                        if (int.TryParse(s, out result3) && !intList1.Contains(result3))
                          intList1.Add(result3);
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadAgentActionStates(List<string> row, int id, bool awaitable)
      {
        int num1 = 2;
        List<string> stringList1 = row;
        int index1 = num1;
        int num2 = index1 + 1;
        string assetbundleName1 = stringList1[index1];
        List<string> stringList2 = row;
        int index2 = num2;
        int num3 = index2 + 1;
        string assetName1 = stringList2[index2];
        List<string> stringList3 = row;
        int index3 = num3;
        int num4 = index3 + 1;
        string manifestName1 = stringList3[index3];
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(assetbundleName1, assetName1, manifestName1);
        if (!Object.op_Inequality((Object) excelData, (Object) null))
          return;
        Dictionary<int, Dictionary<int, PlayState>> agentActionAnimTable = this.AgentActionAnimTable;
        for (int index4 = 1; index4 < excelData.MaxCell; ++index4)
        {
          ExcelData.Param obj = excelData.list[index4];
          if (!obj.list.IsNullOrEmpty<string>())
          {
            int num5 = 0;
            List<string> list1 = obj.list;
            int index5 = num5;
            int num6 = index5 + 1;
            if (!list1.GetElement<string>(index5).IsNullOrEmpty())
            {
              List<string> list2 = obj.list;
              int index6 = num6;
              int num7 = index6 + 1;
              string assetbundleName2 = list2[index6];
              List<string> list3 = obj.list;
              int index7 = num7;
              int num8 = index7 + 1;
              string assetName2 = list3[index7];
              List<string> list4 = obj.list;
              int index8 = num8;
              int num9 = index8 + 1;
              string manifestName2 = list4[index8];
              Manager.Resources.LoadActionAnimationInfo(AssetUtility.LoadAsset<ExcelData>(assetbundleName2, assetName2, manifestName2), agentActionAnimTable, awaitable);
            }
          }
        }
      }

      private void LoadAgentMoveInfoList(List<string> row, int id, bool awaitablw)
      {
        int num1 = 2;
        List<string> source1 = row;
        int index1 = num1;
        int num2 = index1 + 1;
        string element1 = source1.GetElement<string>(index1);
        List<string> source2 = row;
        int index2 = num2;
        int num3 = index2 + 1;
        string element2 = source2.GetElement<string>(index2);
        List<string> source3 = row;
        int index3 = num3;
        int num4 = index3 + 1;
        source3.GetElement<string>(index3);
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(element1, element2, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index4 = 1; index4 < excelData.MaxCell; ++index4)
        {
          ExcelData.Param obj = excelData.list[index4];
          int num5 = 0;
          List<string> list1 = obj.list;
          int index5 = num5;
          int num6 = index5 + 1;
          int hash = Animator.StringToHash(list1.GetElement<string>(index5));
          List<AnimeMoveInfo> animeMoveInfoList1;
          if (!this.AgentMoveInfoTable.TryGetValue(hash, out animeMoveInfoList1))
          {
            List<AnimeMoveInfo> animeMoveInfoList2 = new List<AnimeMoveInfo>();
            this.AgentMoveInfoTable[hash] = animeMoveInfoList2;
            animeMoveInfoList1 = animeMoveInfoList2;
          }
          while (num6 < obj.list.Count)
          {
            List<string> list2 = obj.list;
            int index6 = num6;
            int num7 = index6 + 1;
            string element3 = list2.GetElement<string>(index6);
            List<string> list3 = obj.list;
            int index7 = num7;
            int num8 = index7 + 1;
            string element4 = list3.GetElement<string>(index7);
            List<string> list4 = obj.list;
            int index8 = num8;
            num6 = index8 + 1;
            string element5 = list4.GetElement<string>(index8);
            float result1;
            float result2;
            if (!element5.IsNullOrEmpty() && float.TryParse(element3, out result1) && float.TryParse(element4, out result2))
            {
              AnimeMoveInfo animeMoveInfo = new AnimeMoveInfo()
              {
                start = result1,
                end = result2,
                movePoint = element5
              };
              animeMoveInfoList1.Add(animeMoveInfo);
            }
          }
        }
      }

      private void LoadAgentEventKeyList(List<string> row, int id, bool awaitable)
      {
        int num1 = 2;
        List<string> source1 = row;
        int index1 = num1;
        int num2 = index1 + 1;
        string element1 = source1.GetElement<string>(index1);
        List<string> source2 = row;
        int index2 = num2;
        int num3 = index2 + 1;
        string element2 = source2.GetElement<string>(index2);
        List<string> source3 = row;
        int index3 = num3;
        int num4 = index3 + 1;
        source3.GetElement<string>(index3);
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(element1, element2, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index4 = 1; index4 < excelData.MaxCell; ++index4)
        {
          ExcelData.Param obj = excelData.list[index4];
          int num5 = 1;
          List<string> list1 = obj.list;
          int index5 = num5;
          int num6 = index5 + 1;
          string element3 = list1.GetElement<string>(index5);
          List<string> list2 = obj.list;
          int index6 = num6;
          int num7 = index6 + 1;
          string element4 = list2.GetElement<string>(index6);
          if (!element3.IsNullOrEmpty() && !element4.IsNullOrEmpty() && element4.Contains("item"))
            this.LoadEventKeyTable(obj.list, this.AgentItemEventKeyTable, awaitable);
        }
      }

      private void LoadAgentLocomotionStateList(List<string> row, int id, bool awaitable)
      {
        int num1 = 2;
        List<string> stringList1 = row;
        int index1 = num1;
        int num2 = index1 + 1;
        string assetbundleName = stringList1[index1];
        List<string> stringList2 = row;
        int index2 = num2;
        int num3 = index2 + 1;
        string assetName = stringList2[index2];
        List<string> stringList3 = row;
        int index3 = num3;
        int num4 = index3 + 1;
        string manifestName = stringList3[index3];
        this.LoadLocomotionStateList(AssetUtility.LoadAsset<ExcelData>(assetbundleName, assetName, manifestName), this.AgentLocomotionStateTable, awaitable);
      }

      private void LoadTalkSpeakerStateList(List<string> row, int id, bool awaitable)
      {
        int num1 = 2;
        List<string> stringList1 = row;
        int index1 = num1;
        int num2 = index1 + 1;
        string assetbundleName = stringList1[index1];
        List<string> stringList2 = row;
        int index2 = num2;
        int num3 = index2 + 1;
        string assetName = stringList2[index2];
        List<string> stringList3 = row;
        int index3 = num3;
        int num4 = index3 + 1;
        string manifestName = stringList3[index3];
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(assetbundleName, assetName, manifestName);
        if (!Object.op_Inequality((Object) excelData, (Object) null))
          return;
        int num5 = 1;
        while (num5 < excelData.MaxCell)
        {
          ExcelData.Param obj = excelData.list[num5++];
          int num6 = 0;
          List<string> list1 = obj.list;
          int index4 = num6;
          int num7 = index4 + 1;
          int result1;
          if (int.TryParse(list1.GetElement<string>(index4), out result1))
          {
            int num8 = num7 + 1;
            List<string> list2 = obj.list;
            int index5 = num8;
            int num9 = index5 + 1;
            int result2;
            if (int.TryParse(list2.GetElement<string>(index5), out result2))
            {
              List<string> list3 = obj.list;
              int index6 = num9;
              int num10 = index6 + 1;
              int result3;
              if (int.TryParse(list3.GetElement<string>(index6), out result3))
                this.TalkSpeakerStateTable[result1] = new PoseKeyPair()
                {
                  postureID = result2,
                  poseID = result3
                };
            }
          }
        }
      }

      private void LoadTalkListenerStateList(List<string> row, int id, bool awaitable)
      {
        int num1 = 2;
        List<string> stringList1 = row;
        int index1 = num1;
        int num2 = index1 + 1;
        string assetbundleName = stringList1[index1];
        List<string> stringList2 = row;
        int index2 = num2;
        int num3 = index2 + 1;
        string assetName = stringList2[index2];
        List<string> stringList3 = row;
        int index3 = num3;
        int num4 = index3 + 1;
        string manifestName = stringList3[index3];
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(assetbundleName, assetName, manifestName);
        if (!Object.op_Inequality((Object) excelData, (Object) null))
          return;
        int num5 = 1;
        while (num5 < excelData.MaxCell)
        {
          ExcelData.Param obj = excelData.list[num5++];
          int num6 = 0;
          List<string> list1 = obj.list;
          int index4 = num6;
          int num7 = index4 + 1;
          int result1;
          if (int.TryParse(list1.GetElement<string>(index4), out result1))
          {
            int num8 = num7 + 1;
            List<string> list2 = obj.list;
            int index5 = num8;
            int num9 = index5 + 1;
            int result2;
            if (int.TryParse(list2.GetElement<string>(index5), out result2))
            {
              List<string> list3 = obj.list;
              int index6 = num9;
              int num10 = index6 + 1;
              int result3;
              if (int.TryParse(list3.GetElement<string>(index6), out result3))
              {
                this.TalkListenerStateTable[result1] = new PoseKeyPair()
                {
                  postureID = result2,
                  poseID = result3
                };
                List<string> list4 = obj.list;
                int index7 = num10;
                int num11 = index7 + 1;
                int result4;
                int num12 = !int.TryParse(list4.GetElement<string>(index7), out result4) ? 0 : result4;
                this.TalkListenerRelationTable[result1] = num12;
              }
            }
          }
        }
      }

      private void LoadAgentGravurePoseTable(DefinePack definePack, bool awaitable)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.GravurePoseInfo, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index = 0; index < nameListFromPath.Count; ++index)
        {
          string str = nameListFromPath[index];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          if (!str.IsNullOrEmpty() && !withoutExtension.IsNullOrEmpty())
          {
            AssetBundleInfo listAsset;
            ((AssetBundleInfo) ref listAsset).\u002Ector(string.Empty, str, withoutExtension, string.Empty);
            this.LoadGravurePoseList(listAsset, false);
          }
        }
      }

      private void LoadGravurePoseList(AssetBundleInfo listAsset, bool awaitable)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(listAsset);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              int num3 = num2 + 1;
              List<string> source2 = list;
              int index3 = num3;
              int num4 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num4;
                int num5 = index4 + 1;
                int result3;
                if (int.TryParse(source3.GetElement<string>(index4) ?? string.Empty, out result3))
                {
                  List<string> source4 = list;
                  int index5 = num5;
                  int num6 = index5 + 1;
                  string[] strArray = (source4.GetElement<string>(index5) ?? string.Empty).Split(Manager.Resources._separationKeywords, StringSplitOptions.RemoveEmptyEntries);
                  List<string> source5 = list;
                  int index6 = num6;
                  int num7 = index6 + 1;
                  bool result4;
                  if (!bool.TryParse(source5.GetElement<string>(index6) ?? string.Empty, out result4))
                    result4 = true;
                  List<int> intList = ListPool<int>.Get();
                  if (!((IList<string>) strArray).IsNullOrEmpty<string>())
                  {
                    foreach (string str in strArray)
                    {
                      int result5;
                      if (!str.IsNullOrEmpty() && int.TryParse(str, out result5))
                        intList.Add(result5);
                    }
                  }
                  if (!intList.IsNullOrEmpty<int>())
                  {
                    PoseKeyPair poseKeyPair = new PoseKeyPair()
                    {
                      postureID = result2,
                      poseID = result3
                    };
                    foreach (int key in intList)
                    {
                      Dictionary<int, ValueTuple<PoseKeyPair, bool>> dictionary;
                      if (!this.AgentGravurePoseTable.TryGetValue(key, out dictionary))
                        this.AgentGravurePoseTable[key] = dictionary = new Dictionary<int, ValueTuple<PoseKeyPair, bool>>();
                      dictionary[result1] = new ValueTuple<PoseKeyPair, bool>(poseKeyPair, result4);
                    }
                  }
                  ListPool<int>.Release(intList);
                }
              }
            }
          }
        }
      }

      private void LoadWithAnimalStateList(List<string> row, int id, bool awaitable)
      {
        int num1 = 2;
        List<string> source1 = row;
        int index1 = num1;
        int num2 = index1 + 1;
        string assetbundleName = source1.GetElement<string>(index1) ?? string.Empty;
        List<string> source2 = row;
        int index2 = num2;
        int num3 = index2 + 1;
        string assetName = source2.GetElement<string>(index2) ?? string.Empty;
        List<string> source3 = row;
        int index3 = num3;
        int num4 = index3 + 1;
        string manifestName = source3.GetElement<string>(index3) ?? string.Empty;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(assetbundleName, assetName, manifestName);
        if (Object.op_Equality((Object) excelData, (Object) null) || excelData.MaxCell == 0)
          return;
        int num5 = 1;
        List<ExcelData.Param> list1 = excelData.list;
        int index4 = num5;
        int num6 = index4 + 1;
        this.LoadWithAnimalAnimationList(list1.GetElement<ExcelData.Param>(index4)?.list, awaitable);
        List<ExcelData.Param> list2 = excelData.list;
        int index5 = num6;
        int num7 = index5 + 1;
        this.LoadEventKeyTable(list2.GetElement<ExcelData.Param>(index5)?.list, this.AgentAnimalEventKeyTable, awaitable);
      }

      private void LoadWithAnimalAnimationList(List<string> row, bool awaitable)
      {
        if (row.IsNullOrEmpty<string>())
          return;
        int num1 = 1;
        List<string> source1 = row;
        int index1 = num1;
        int num2 = index1 + 1;
        string assetbundleName1 = source1.GetElement<string>(index1) ?? string.Empty;
        List<string> source2 = row;
        int index2 = num2;
        int num3 = index2 + 1;
        string assetName1 = source2.GetElement<string>(index2) ?? string.Empty;
        List<string> source3 = row;
        int index3 = num3;
        int num4 = index3 + 1;
        string manifestName1 = source3.GetElement<string>(index3) ?? string.Empty;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(assetbundleName1, assetName1, manifestName1);
        if (Object.op_Equality((Object) excelData, (Object) null) || excelData.MaxCell == 0)
          return;
        for (int index4 = 1; index4 < excelData.MaxCell; ++index4)
        {
          List<string> list = excelData.list.GetElement<ExcelData.Param>(index4)?.list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num5 = 1;
            List<string> source4 = list;
            int index5 = num5;
            int num6 = index5 + 1;
            string assetbundleName2 = source4.GetElement<string>(index5) ?? string.Empty;
            List<string> source5 = list;
            int index6 = num6;
            int num7 = index6 + 1;
            string assetName2 = source5.GetElement<string>(index6) ?? string.Empty;
            List<string> source6 = list;
            int index7 = num7;
            num4 = index7 + 1;
            string manifestName2 = source6.GetElement<string>(index7) ?? string.Empty;
            this.LoadWithAnimalAnimationState(AssetUtility.LoadAsset<ExcelData>(assetbundleName2, assetName2, manifestName2), awaitable);
          }
        }
      }

      private void LoadWithAnimalAnimationState(ExcelData withAnimalExcel, bool awaitable)
      {
        if (Object.op_Equality((Object) withAnimalExcel, (Object) null) || withAnimalExcel.MaxCell == 0)
          return;
        int num1 = 1;
        while (num1 < withAnimalExcel.MaxCell)
        {
          List<string> list = withAnimalExcel.list.GetElement<ExcelData.Param>(num1++)?.list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num2 = 2;
            int result1 = 0;
            List<string> source1 = list;
            int index1 = num2;
            int num3 = index1 + 1;
            if (int.TryParse(source1.GetElement<string>(index1) ?? string.Empty, out result1))
            {
              int result2 = 0;
              List<string> source2 = list;
              int index2 = num3;
              int num4 = index2 + 1;
              if (int.TryParse(source2.GetElement<string>(index2) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index3 = num4;
                int num5 = index3 + 1;
                string str1 = source3.GetElement<string>(index3) ?? string.Empty;
                List<string> source4 = list;
                int index4 = num5;
                int num6 = index4 + 1;
                string str2 = source4.GetElement<string>(index4) ?? string.Empty;
                List<string> source5 = list;
                int index5 = num6;
                int num7 = index5 + 1;
                string[] inStateNames = (source5.GetElement<string>(index5) ?? string.Empty).Split(Manager.Resources.AnimationTables._separators, StringSplitOptions.RemoveEmptyEntries);
                List<string> source6 = list;
                int index6 = num7;
                int num8 = index6 + 1;
                bool result3;
                if (!bool.TryParse(source6.GetElement<string>(index6) ?? string.Empty, out result3))
                  result3 = false;
                List<string> source7 = list;
                int index7 = num8;
                int num9 = index7 + 1;
                float result4;
                if (!float.TryParse(source7.GetElement<string>(index7) ?? string.Empty, out result4))
                  result4 = 0.0f;
                List<string> source8 = list;
                int index8 = num9;
                int num10 = index8 + 1;
                string[] outStateNames = (source8.GetElement<string>(index8) ?? string.Empty).Split(Manager.Resources.AnimationTables._separators, StringSplitOptions.RemoveEmptyEntries);
                List<string> source9 = list;
                int index9 = num10;
                int num11 = index9 + 1;
                bool result5;
                if (!bool.TryParse(source9.GetElement<string>(index9) ?? string.Empty, out result5))
                  result5 = false;
                List<string> source10 = list;
                int index10 = num11;
                int num12 = index10 + 1;
                float result6;
                if (!float.TryParse(source10.GetElement<string>(index10) ?? string.Empty, out result6))
                  result6 = 0.0f;
                List<string> source11 = list;
                int index11 = num12;
                int num13 = index11 + 1;
                int result7;
                if (!int.TryParse(source11.GetElement<string>(index11) ?? string.Empty, out result7))
                  result7 = 0;
                List<string> source12 = list;
                int index12 = num13;
                int num14 = index12 + 1;
                int result8;
                if (!int.TryParse(source12.GetElement<string>(index12) ?? string.Empty, out result8))
                  result8 = 0;
                List<string> source13 = list;
                int index13 = num14;
                int num15 = index13 + 1;
                int result9;
                if (!int.TryParse(source13.GetElement<string>(index13) ?? string.Empty, out result9))
                  result9 = 0;
                List<string> source14 = list;
                int index14 = num15;
                int num16 = index14 + 1;
                string input = source14.GetElement<string>(index14) ?? string.Empty;
                List<ValueTuple<string, bool, int, bool>> toRelease = ListPool<ValueTuple<string, bool, int, bool>>.Get();
                MatchCollection matchCollection = this._regex.Matches(input);
                if (0 < matchCollection.Count)
                {
                  for (int index15 = 0; index15 < matchCollection.Count; ++index15)
                  {
                    Match match = matchCollection[index15];
                    for (int index16 = 0; index16 < match.Groups[1].Captures.Count; ++index16)
                    {
                      string[] strArray = match.Groups[1].Captures[index16].Value.Split(Manager.Resources._separationKeywords, StringSplitOptions.RemoveEmptyEntries);
                      int num17 = 0;
                      string[] source15 = strArray;
                      int index17 = num17;
                      int num18 = index17 + 1;
                      string str3 = source15.GetElement<string>(index17) ?? string.Empty;
                      string[] source16 = strArray;
                      int index18 = num18;
                      int num19 = index18 + 1;
                      bool result10;
                      if (!bool.TryParse(source16.GetElement<string>(index18) ?? string.Empty, out result10))
                        result10 = false;
                      string[] source17 = strArray;
                      int index19 = num19;
                      int num20 = index19 + 1;
                      int result11;
                      if (!int.TryParse(source17.GetElement<string>(index19) ?? string.Empty, out result11))
                        result11 = -1;
                      string[] source18 = strArray;
                      int index20 = num20;
                      int num21 = index20 + 1;
                      bool result12;
                      if (!bool.TryParse(source18.GetElement<string>(index20) ?? string.Empty, out result12))
                        result12 = false;
                      toRelease.Add(new ValueTuple<string, bool, int, bool>(str3, result10, result11, result12));
                    }
                  }
                }
                PlayState playState = new PlayState(result9, inStateNames, outStateNames);
                playState.MainStateInfo.AssetBundleInfo = new AssetBundleInfo(string.Empty, str1, str2, string.Empty);
                PlayState.PlayStateInfo mainStateInfo = playState.MainStateInfo;
                mainStateInfo.InStateInfo.EnableFade = result3;
                mainStateInfo.InStateInfo.FadeSecond = result4;
                mainStateInfo.OutStateInfo.EnableFade = result5;
                mainStateInfo.OutStateInfo.FadeSecond = result6;
                mainStateInfo.IsLoop = 0 < result7 || 0 < result8;
                mainStateInfo.LoopMin = result7;
                mainStateInfo.LoopMax = result8;
                playState.ActionInfo = new ActionInfo(false, 0);
                using (List<ValueTuple<string, bool, int, bool>>.Enumerator enumerator = toRelease.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    ValueTuple<string, bool, int, bool> current = enumerator.Current;
                    playState.AddItemInfo(new PlayState.ItemInfo()
                    {
                      parentName = (string) current.Item1,
                      fromEquipedItem = (bool) current.Item2,
                      itemID = (int) current.Item3,
                      isSync = (bool) current.Item4
                    });
                  }
                }
                ListPool<ValueTuple<string, bool, int, bool>>.Release(toRelease);
                if (!this.WithAnimalStateTable.ContainsKey(result1))
                  this.WithAnimalStateTable[result1] = new Dictionary<int, PlayState>();
                this.WithAnimalStateTable[result1][result2] = playState;
              }
            }
          }
        }
      }

      private void LoadEventKeyTable(
        List<string> row,
        Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>> eventKeyTable,
        bool awaitable)
      {
        int num1 = 1;
        List<string> source1 = row;
        int index1 = num1;
        int num2 = index1 + 1;
        string element1 = source1.GetElement<string>(index1);
        List<string> source2 = row;
        int index2 = num2;
        int num3 = index2 + 1;
        string element2 = source2.GetElement<string>(index2);
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(element1, element2, string.Empty);
        for (int index3 = 2; index3 < excelData.MaxCell; ++index3)
        {
          ExcelData.Param obj = excelData.list[index3];
          int num4 = 2;
          List<string> list1 = obj.list;
          int index4 = num4;
          int num5 = index4 + 1;
          string element3 = list1.GetElement<string>(index4);
          List<string> list2 = obj.list;
          int index5 = num5;
          int num6 = index5 + 1;
          string element4 = list2.GetElement<string>(index5);
          int result1;
          int result2;
          if (int.TryParse(element3, out result1) && int.TryParse(element4, out result2))
          {
            List<string> list3 = obj.list;
            int index6 = num6;
            int num7 = index6 + 1;
            int hash = Animator.StringToHash(list3.GetElement<string>(index6));
            Dictionary<int, Dictionary<int, List<AnimeEventInfo>>> dictionary1;
            if (!eventKeyTable.TryGetValue(result1, out dictionary1))
            {
              Dictionary<int, Dictionary<int, List<AnimeEventInfo>>> dictionary2 = new Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>();
              eventKeyTable[result1] = dictionary2;
              dictionary1 = dictionary2;
            }
            Dictionary<int, List<AnimeEventInfo>> dictionary3;
            if (!dictionary1.TryGetValue(result2, out dictionary3))
            {
              Dictionary<int, List<AnimeEventInfo>> dictionary2 = new Dictionary<int, List<AnimeEventInfo>>();
              dictionary1[result2] = dictionary2;
              dictionary3 = dictionary2;
            }
            List<AnimeEventInfo> animeEventInfoList1;
            if (!dictionary3.TryGetValue(hash, out animeEventInfoList1))
            {
              List<AnimeEventInfo> animeEventInfoList2 = new List<AnimeEventInfo>();
              dictionary3[hash] = animeEventInfoList2;
              animeEventInfoList1 = animeEventInfoList2;
            }
            List<string> list4 = obj.list;
            int index7 = num7;
            int num8 = index7 + 1;
            string element5 = list4.GetElement<string>(index7);
            if (element5.IsNullOrEmpty())
            {
              Debug.LogWarning((object) "エラー：イベントが空");
            }
            else
            {
              MatchCollection matchCollection = this._regex.Matches(element5);
              if (matchCollection.Count > 0)
              {
                for (int index8 = 0; index8 < matchCollection.Count; ++index8)
                {
                  Match match = matchCollection[index8];
                  for (int index9 = 0; index9 < match.Groups[1].Captures.Count; ++index9)
                  {
                    string[] strArray = match.Groups[1].Captures[index9].Value.Split(Manager.Resources.AnimationTables._separators, StringSplitOptions.RemoveEmptyEntries);
                    int num9 = 0;
                    string[] source3 = strArray;
                    int index10 = num9;
                    int num10 = index10 + 1;
                    float result3;
                    if (!float.TryParse(source3.GetElement<string>(index10), out result3))
                      result3 = float.MaxValue;
                    string[] source4 = strArray;
                    int index11 = num10;
                    int num11 = index11 + 1;
                    int result4;
                    if (!int.TryParse(source4.GetElement<string>(index11), out result4))
                      result4 = -1;
                    animeEventInfoList1.Add(new AnimeEventInfo()
                    {
                      normalizedTime = result3,
                      eventID = result4
                    });
                  }
                }
              }
            }
          }
        }
      }

      private void LoadSEEventKeyTable(
        string sheetBundleName,
        string sheetAssetName,
        Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>>> table,
        bool awaitable)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(sheetBundleName, sheetAssetName, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 2; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            if (list.GetElement<string>(0) == "end")
              break;
            int num1 = 2;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                string element1 = source3.GetElement<string>(index4);
                if (!element1.IsNullOrEmpty())
                {
                  int hash = Animator.StringToHash(element1);
                  Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>> dictionary1;
                  if (!table.TryGetValue(result1, out dictionary1))
                    table[result1] = dictionary1 = new Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>>();
                  Dictionary<int, List<AnimeSEEventInfo>> dictionary2;
                  if (!dictionary1.TryGetValue(result2, out dictionary2))
                    dictionary1[result2] = dictionary2 = new Dictionary<int, List<AnimeSEEventInfo>>();
                  List<AnimeSEEventInfo> self1;
                  if (!dictionary2.TryGetValue(hash, out self1) || self1 == null)
                    dictionary2[hash] = self1 = new List<AnimeSEEventInfo>();
                  else if (!self1.IsNullOrEmpty<AnimeSEEventInfo>())
                    self1.Clear();
                  List<string> source4 = list;
                  int index5 = num4;
                  int num5 = index5 + 1;
                  string element2 = source4.GetElement<string>(index5);
                  if (!element2.IsNullOrEmpty())
                  {
                    string[] strArray1 = element2.Split(Manager.Resources.AnimationTables._seEventSepa, StringSplitOptions.RemoveEmptyEntries);
                    if (!((IList<string>) strArray1).IsNullOrEmpty<string>())
                    {
                      for (int index6 = 0; index6 < strArray1.Length; ++index6)
                      {
                        string str1 = strArray1[index6];
                        if (!str1.IsNullOrEmpty())
                        {
                          string self2 = str1.Replace(string.Empty, Manager.Resources.AnimationTables._seEventRemoveStr);
                          if (!self2.IsNullOrEmpty())
                          {
                            string[] strArray2 = self2.Split(Manager.Resources.AnimationTables._separators, StringSplitOptions.None);
                            int num6 = 0;
                            string[] source5 = strArray2;
                            int index7 = num6;
                            int num7 = index7 + 1;
                            float result3;
                            if (float.TryParse(source5.GetElement<string>(index7) ?? string.Empty, out result3))
                            {
                              if ((double) result3 < 0.0 || 1.0 >= (double) result3)
                                ;
                              string[] source6 = strArray2;
                              int index8 = num7;
                              int num8 = index8 + 1;
                              int result4;
                              if (int.TryParse(source6.GetElement<string>(index8) ?? string.Empty, out result4))
                              {
                                string[] source7 = strArray2;
                                int index9 = num8;
                                int num9 = index9 + 1;
                                int result5;
                                if (!int.TryParse(source7.GetElement<string>(index9) ?? string.Empty, out result5))
                                  result5 = -1;
                                string[] source8 = strArray2;
                                int index10 = num9;
                                int num10 = index10 + 1;
                                string str2 = source8.GetElement<string>(index10) ?? string.Empty;
                                self1.Add(new AnimeSEEventInfo()
                                {
                                  NormalizedTime = result3,
                                  ClipID = result4,
                                  EventID = result5,
                                  Root = str2
                                });
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadFootStepEventKeyTable(
        string sheetAssetBundle,
        string sheetAsset,
        Dictionary<int, FootStepInfo[]> eventKeyTable,
        bool awaitable)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(sheetAssetBundle, sheetAsset, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 1;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            string str1 = source1.GetElement<string>(index2) ?? string.Empty;
            List<FootStepInfo> footStepInfoList = ListPool<FootStepInfo>.Get();
            while (num2 < list.Count)
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              float result1;
              if (float.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result1))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                float result2;
                if (float.TryParse(source3.GetElement<string>(index4) ?? string.Empty, out result2))
                {
                  List<string> source4 = list;
                  int index5 = num4;
                  num2 = index5 + 1;
                  string[] strArray = (source4.GetElement<string>(index5) ?? string.Empty).Split(Manager.Resources.AnimationTables._separators, StringSplitOptions.RemoveEmptyEntries);
                  if (!((IList<string>) strArray).IsNullOrEmpty<string>())
                  {
                    List<float> floatList = ListPool<float>.Get();
                    foreach (string str2 in strArray)
                    {
                      float result3;
                      if (float.TryParse(str2 ?? string.Empty, out result3))
                        floatList.Add(result3);
                    }
                    if (!floatList.IsNullOrEmpty<float>())
                      footStepInfoList.Add(new FootStepInfo(result1, result2, floatList));
                    ListPool<float>.Release(floatList);
                  }
                  else
                    break;
                }
                else
                  break;
              }
              else
                break;
            }
            if (!footStepInfoList.IsNullOrEmpty<FootStepInfo>())
            {
              int hash = Animator.StringToHash(str1);
              FootStepInfo[] footStepInfoArray = new FootStepInfo[footStepInfoList.Count];
              for (int index3 = 0; index3 < footStepInfoList.Count; ++index3)
                footStepInfoArray[index3] = footStepInfoList[index3];
              eventKeyTable[hash] = footStepInfoArray;
            }
            ListPool<FootStepInfo>.Release(footStepInfoList);
          }
        }
      }

      private void LoadParticleEventKeyTable(
        string sheetAssetBundleName,
        string sheetAssetName,
        Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>>> eventKeyTable,
        bool awaitable)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(sheetAssetBundleName, sheetAssetName, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 2; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 2;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                string element1 = source3.GetElement<string>(index4);
                if (!element1.IsNullOrEmpty())
                {
                  int hash = Animator.StringToHash(element1);
                  Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>> dictionary1;
                  if (!eventKeyTable.TryGetValue(result1, out dictionary1))
                    eventKeyTable[result1] = dictionary1 = new Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>>();
                  Dictionary<int, List<AnimeParticleEventInfo>> dictionary2;
                  if (!dictionary1.TryGetValue(result2, out dictionary2))
                    dictionary1[result2] = dictionary2 = new Dictionary<int, List<AnimeParticleEventInfo>>();
                  List<AnimeParticleEventInfo> self;
                  if (!dictionary2.TryGetValue(hash, out self) || self == null)
                    dictionary2[hash] = self = new List<AnimeParticleEventInfo>();
                  else if (!self.IsNullOrEmpty<AnimeParticleEventInfo>())
                    self.Clear();
                  List<string> source4 = list;
                  int index5 = num4;
                  int num5 = index5 + 1;
                  string element2 = source4.GetElement<string>(index5);
                  if (!element2.IsNullOrEmpty())
                  {
                    MatchCollection matchCollection = this._regex.Matches(element2);
                    if (0 < matchCollection.Count)
                    {
                      for (int index6 = 0; index6 < matchCollection.Count; ++index6)
                      {
                        Match match = matchCollection[index6];
                        for (int index7 = 0; index7 < match.Groups[1].Captures.Count; ++index7)
                        {
                          string[] strArray = match.Groups[1].Captures[index7].Value.Split(Manager.Resources.AnimationTables._separators, StringSplitOptions.RemoveEmptyEntries);
                          int num6 = 0;
                          string[] source5 = strArray;
                          int index8 = num6;
                          int num7 = index8 + 1;
                          float result3;
                          if (float.TryParse(source5.GetElement<string>(index8) ?? string.Empty, out result3))
                          {
                            string[] source6 = strArray;
                            int index9 = num7;
                            int num8 = index9 + 1;
                            int result4;
                            if (int.TryParse(source6.GetElement<string>(index9) ?? string.Empty, out result4))
                            {
                              string[] source7 = strArray;
                              int index10 = num8;
                              int num9 = index10 + 1;
                              int result5;
                              if (int.TryParse(source7.GetElement<string>(index10) ?? string.Empty, out result5))
                              {
                                string[] source8 = strArray;
                                int index11 = num9;
                                int num10 = index11 + 1;
                                string str = source8.GetElement<string>(index11) ?? string.Empty;
                                self.Add(new AnimeParticleEventInfo()
                                {
                                  NormalizedTime = result3,
                                  ParticleID = result4,
                                  EventID = result5,
                                  Root = str
                                });
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadLoopVoiceEventKeyTable(
        string assetBundleName,
        string assetName,
        Dictionary<int, Dictionary<int, Dictionary<int, List<int>>>> table,
        bool awaitable)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(assetBundleName, assetName, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 2;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                string element = source3.GetElement<string>(index4);
                if (!element.IsNullOrEmpty())
                {
                  List<string> source4 = list;
                  int index5 = num4;
                  int num5 = index5 + 1;
                  string[] strArray = (source4.GetElement<string>(index5) ?? string.Empty).Split(Manager.Resources.AnimationTables._separators, StringSplitOptions.RemoveEmptyEntries);
                  List<int> intList = ListPool<int>.Get();
                  if (!((IList<string>) strArray).IsNullOrEmpty<string>())
                  {
                    foreach (string str in strArray)
                    {
                      int result3;
                      if (!str.IsNullOrEmpty() && int.TryParse(str, out result3))
                        intList.Add(result3);
                    }
                  }
                  if (intList.IsNullOrEmpty<int>())
                  {
                    ListPool<int>.Release(intList);
                  }
                  else
                  {
                    int hash = Animator.StringToHash(element);
                    Dictionary<int, Dictionary<int, List<int>>> dictionary1;
                    if (!table.TryGetValue(result1, out dictionary1))
                      table[result1] = dictionary1 = new Dictionary<int, Dictionary<int, List<int>>>();
                    Dictionary<int, List<int>> dictionary2;
                    if (!dictionary1.TryGetValue(result2, out dictionary2))
                      dictionary1[result2] = dictionary2 = new Dictionary<int, List<int>>();
                    List<int> self;
                    if (!dictionary2.TryGetValue(hash, out self) || self == null)
                      dictionary2[hash] = self = new List<int>();
                    else if (!self.IsNullOrEmpty<int>())
                      self.Clear();
                    self.AddRange((IEnumerable<int>) intList);
                    ListPool<int>.Release(intList);
                  }
                }
              }
            }
          }
        }
      }

      private void LoadEventKeyTable(
        string sheetAssetBundleName,
        string sheetAssetName,
        Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>> eventKeyTable,
        bool awaitable)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(sheetAssetBundleName, sheetAssetName, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 2; index1 < excelData.MaxCell; ++index1)
        {
          ExcelData.Param obj = excelData.list[index1];
          int num1 = 2;
          List<string> list1 = obj.list;
          int index2 = num1;
          int num2 = index2 + 1;
          string element1 = list1.GetElement<string>(index2);
          List<string> list2 = obj.list;
          int index3 = num2;
          int num3 = index3 + 1;
          string element2 = list2.GetElement<string>(index3);
          int result1;
          int result2;
          if (int.TryParse(element1, out result1) && int.TryParse(element2, out result2))
          {
            List<string> list3 = obj.list;
            int index4 = num3;
            int num4 = index4 + 1;
            int hash = Animator.StringToHash(list3.GetElement<string>(index4));
            Dictionary<int, Dictionary<int, List<AnimeEventInfo>>> dictionary1;
            if (!eventKeyTable.TryGetValue(result1, out dictionary1))
            {
              Dictionary<int, Dictionary<int, List<AnimeEventInfo>>> dictionary2 = new Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>();
              eventKeyTable[result1] = dictionary2;
              dictionary1 = dictionary2;
            }
            Dictionary<int, List<AnimeEventInfo>> dictionary3;
            if (!dictionary1.TryGetValue(result2, out dictionary3))
            {
              Dictionary<int, List<AnimeEventInfo>> dictionary2 = new Dictionary<int, List<AnimeEventInfo>>();
              dictionary1[result2] = dictionary2;
              dictionary3 = dictionary2;
            }
            List<AnimeEventInfo> self;
            if (!dictionary3.TryGetValue(hash, out self) || self == null)
            {
              List<AnimeEventInfo> animeEventInfoList = new List<AnimeEventInfo>();
              dictionary3[hash] = animeEventInfoList;
              self = animeEventInfoList;
            }
            else if (!self.IsNullOrEmpty<AnimeEventInfo>())
              self.Clear();
            List<string> list4 = obj.list;
            int index5 = num4;
            int num5 = index5 + 1;
            string element3 = list4.GetElement<string>(index5);
            if (element3.IsNullOrEmpty())
            {
              Debug.LogWarning((object) "エラー：イベントが空");
            }
            else
            {
              MatchCollection matchCollection = this._regex.Matches(element3);
              if (matchCollection.Count > 0)
              {
                for (int index6 = 0; index6 < matchCollection.Count; ++index6)
                {
                  Match match = matchCollection[index6];
                  for (int index7 = 0; index7 < match.Groups[1].Captures.Count; ++index7)
                  {
                    string[] strArray = match.Groups[1].Captures[index7].Value.Split(Manager.Resources.AnimationTables._separators, StringSplitOptions.RemoveEmptyEntries);
                    int num6 = 0;
                    string[] source1 = strArray;
                    int index8 = num6;
                    int num7 = index8 + 1;
                    float result3;
                    if (!float.TryParse(source1.GetElement<string>(index8), out result3))
                      result3 = float.MaxValue;
                    string[] source2 = strArray;
                    int index9 = num7;
                    int num8 = index9 + 1;
                    int result4;
                    if (!int.TryParse(source2.GetElement<string>(index9), out result4))
                      result4 = -1;
                    self.Add(new AnimeEventInfo()
                    {
                      normalizedTime = result3,
                      eventID = result4
                    });
                  }
                }
              }
            }
          }
        }
      }

      public void LoadOnceVoiceEventKeyTable(
        string sheetAssetBundleName,
        string sheetAssetName,
        Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeOnceVoiceEventInfo>>>> eventKeyTable,
        bool awaitable)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(sheetAssetBundleName, sheetAssetName, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 2; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 2;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                string self1 = source3.GetElement<string>(index4) ?? string.Empty;
                if (!self1.IsNullOrEmpty())
                {
                  List<string> source4 = list;
                  int index5 = num4;
                  int num5 = index5 + 1;
                  string str = source4.GetElement<string>(index5) ?? string.Empty;
                  if (!str.IsNullOrEmpty())
                  {
                    int hash = Animator.StringToHash(self1);
                    Dictionary<int, Dictionary<int, List<AnimeOnceVoiceEventInfo>>> dictionary1;
                    if (!eventKeyTable.TryGetValue(result1, out dictionary1))
                      eventKeyTable[result1] = dictionary1 = new Dictionary<int, Dictionary<int, List<AnimeOnceVoiceEventInfo>>>();
                    Dictionary<int, List<AnimeOnceVoiceEventInfo>> dictionary2;
                    if (!dictionary1.TryGetValue(result2, out dictionary2))
                      dictionary1[result2] = dictionary2 = new Dictionary<int, List<AnimeOnceVoiceEventInfo>>();
                    List<AnimeOnceVoiceEventInfo> self2;
                    if (!dictionary2.TryGetValue(hash, out self2) || self2 == null)
                      dictionary2[hash] = self2 = new List<AnimeOnceVoiceEventInfo>();
                    else if (!self2.IsNullOrEmpty<AnimeOnceVoiceEventInfo>())
                      self2.Clear();
                    MatchCollection matchCollection = this._regex.Matches(str);
                    if (0 < matchCollection.Count)
                    {
                      for (int index6 = 0; index6 < matchCollection.Count; ++index6)
                      {
                        Match match = matchCollection[index6];
                        for (int index7 = 0; index7 < match.Groups[1].Captures.Count; ++index7)
                        {
                          string[] source5 = match.Groups[1].Captures[index7].Value.Split(Manager.Resources.AnimationTables._separators, StringSplitOptions.RemoveEmptyEntries);
                          int num6 = 0;
                          string[] source6 = source5;
                          int index8 = num6;
                          int num7 = index8 + 1;
                          float result3;
                          if (!float.TryParse(source6.GetElement<string>(index8) ?? string.Empty, out result3))
                            result3 = float.MaxValue;
                          List<int> toRelease = ListPool<int>.Get();
                          while (num7 < source5.Length)
                          {
                            int result4;
                            if (int.TryParse(source5.GetElement<string>(num7++) ?? string.Empty, out result4))
                              toRelease.Add(result4);
                          }
                          AnimeOnceVoiceEventInfo onceVoiceEventInfo = new AnimeOnceVoiceEventInfo()
                          {
                            NormalizedTime = result3,
                            EventIDs = toRelease.ToArray()
                          };
                          ListPool<int>.Release(toRelease);
                          self2.Add(onceVoiceEventInfo);
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadLoopVoiceEventKeyTable(
        string sheetAssetBundleName,
        string sheetAssetName,
        Dictionary<int, Dictionary<int, Dictionary<int, int>>> eventKeyTable)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(sheetAssetBundleName, sheetAssetName, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 2;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                string element = source3.GetElement<string>(index4);
                if (!element.IsNullOrEmpty())
                {
                  List<string> source4 = list;
                  int index5 = num4;
                  int num5 = index5 + 1;
                  int result3;
                  if (int.TryParse(source4.GetElement<string>(index5) ?? string.Empty, out result3))
                  {
                    int hash = Animator.StringToHash(element);
                    Dictionary<int, Dictionary<int, int>> dictionary1;
                    if (!eventKeyTable.TryGetValue(result1, out dictionary1))
                      eventKeyTable[result1] = dictionary1 = new Dictionary<int, Dictionary<int, int>>();
                    Dictionary<int, int> dictionary2;
                    if (!dictionary1.TryGetValue(result2, out dictionary2))
                      dictionary1[result2] = dictionary2 = new Dictionary<int, int>();
                    dictionary2[hash] = result3;
                  }
                }
              }
            }
          }
        }
      }

      private void LoadAgentEventKeyTable(DefinePack definePack, bool awaitable)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.AgentAnimeInfo, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index = 0; index < nameListFromPath.Count; ++index)
        {
          string str = nameListFromPath[index];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          if (!str.IsNullOrEmpty() && !withoutExtension.IsNullOrEmpty())
          {
            string sheetAsset = string.Format("footstepevent_{0}", (object) withoutExtension);
            this.LoadFootStepEventKeyTable(str, sheetAsset, this.AgentFootStepEventKeyTable, awaitable);
            string sheetAssetName1 = string.Format("event_se_{0}", (object) withoutExtension);
            this.LoadSEEventKeyTable(str, sheetAssetName1, this.AgentActSEEventKeyTable, awaitable);
            string sheetAssetName2 = string.Format("event_particle_{0}", (object) withoutExtension);
            this.LoadParticleEventKeyTable(str, sheetAssetName2, this.AgentActParticleEventKeyTable, awaitable);
            string sheetAssetName3 = string.Format("event_voice_{0}", (object) withoutExtension);
            this.LoadOnceVoiceEventKeyTable(str, sheetAssetName3, this.AgentActOnceVoiceEventKeyTable, awaitable);
            string assetName = string.Format("event_voice_loop_{0}", (object) withoutExtension);
            this.LoadLoopVoiceEventKeyTable(str, assetName, this.AgentActLoopVoiceEventKeyTable, awaitable);
          }
        }
      }

      private void LoadPlayerEventKeyTable(DefinePack definePack, bool awaitable)
      {
        this.LoadPlayerEventKeyTable(definePack.ABDirectories.PlayerMaleAnimeInfo, 0, awaitable);
        this.LoadPlayerEventKeyTable(definePack.ABDirectories.PlayerFemaleAnimeInfo, 1, awaitable);
      }

      private void LoadPlayerEventKeyTable(string directory, int sex, bool awaitable)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(directory, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index = 0; index < nameListFromPath.Count; ++index)
        {
          string str = nameListFromPath[index];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          if (!str.IsNullOrEmpty() && !withoutExtension.IsNullOrEmpty())
          {
            string sheetAsset = string.Format("footstepevent_{0}", (object) withoutExtension);
            Dictionary<int, FootStepInfo[]> eventKeyTable1;
            if (!this.PlayerFootStepEventKeyTable.TryGetValue(sex, out eventKeyTable1))
              this.PlayerFootStepEventKeyTable[sex] = eventKeyTable1 = new Dictionary<int, FootStepInfo[]>();
            this.LoadFootStepEventKeyTable(str, sheetAsset, eventKeyTable1, awaitable);
            string sheetAssetName1 = string.Format("event_se_{0}", (object) withoutExtension);
            Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>>> table;
            if (!this.PlayerActSEEventKeyTable.TryGetValue(sex, out table))
              this.PlayerActSEEventKeyTable[sex] = table = new Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>>>();
            this.LoadSEEventKeyTable(str, sheetAssetName1, table, awaitable);
            string sheetAssetName2 = string.Format("event_particle_{0}", (object) withoutExtension);
            Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>>> eventKeyTable2;
            if (!this.PlayerActParticleEventKeyTable.TryGetValue(sex, out eventKeyTable2))
              this.PlayerActParticleEventKeyTable[sex] = eventKeyTable2 = new Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>>>();
            this.LoadParticleEventKeyTable(str, sheetAssetName2, eventKeyTable2, awaitable);
          }
        }
      }

      private void LoadChangeClothEventKeyTable(DefinePack definePack, bool awaitable)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.AgentAnimeInfo, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        foreach (string str in nameListFromPath)
        {
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          if (!str.IsNullOrEmpty() && !withoutExtension.IsNullOrEmpty())
          {
            string sheetAssetName = string.Format("cloth_{0}", (object) withoutExtension);
            this.LoadEventKeyTable(str, sheetAssetName, this.AgentChangeClothEventKeyTable, false);
          }
        }
      }

      private void LoadSurpriseItemList(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.SurpriseItemInfo, false);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (SurpriseItemData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (SurpriseItemData), (string) null).GetAllAssets<SurpriseItemData>())
          {
            foreach (SurpriseItemData.Param obj in allAsset.param)
            {
              List<PlayState.ItemInfo> itemInfoList1;
              if (!this.SurpriseItemList.TryGetValue(obj.Animator, out itemInfoList1))
              {
                List<PlayState.ItemInfo> itemInfoList2 = new List<PlayState.ItemInfo>();
                this.SurpriseItemList[obj.Animator] = itemInfoList2;
                itemInfoList1 = itemInfoList2;
              }
              if (!obj.ItemList.IsNullOrEmpty<string>())
              {
                foreach (string self in obj.ItemList)
                {
                  if (!self.IsNullOrEmpty())
                  {
                    string[] strArray = self.Split(Manager.Resources.AnimationTables._comma, StringSplitOptions.None);
                    int num1 = 0;
                    string[] source1 = strArray;
                    int index1 = num1;
                    int num2 = index1 + 1;
                    int result1;
                    int num3 = !int.TryParse(source1.GetElement<string>(index1), out result1) ? -1 : result1;
                    string[] source2 = strArray;
                    int index2 = num2;
                    int num4 = index2 + 1;
                    string element = source2.GetElement<string>(index2);
                    string[] source3 = strArray;
                    int index3 = num4;
                    int num5 = index3 + 1;
                    bool result2;
                    bool flag = bool.TryParse(source3.GetElement<string>(index3), out result2) && result2;
                    itemInfoList1.Add(new PlayState.ItemInfo()
                    {
                      itemID = num3,
                      parentName = element,
                      isSync = flag
                    });
                  }
                }
              }
            }
          }
          Singleton<Manager.Resources>.Instance.AddLoadAssetBundle(assetBundleName, string.Empty);
        }
      }

      private void LoadMerchantActionState(
        List<string> address,
        string id,
        Dictionary<int, Dictionary<int, PlayState>> dic,
        bool awaitable)
      {
        int num1 = 2;
        List<string> source1 = address;
        int index1 = num1;
        int num2 = index1 + 1;
        string assetbundleName1 = source1.GetElement<string>(index1) ?? string.Empty;
        List<string> source2 = address;
        int index2 = num2;
        int num3 = index2 + 1;
        string assetName1 = source2.GetElement<string>(index2) ?? string.Empty;
        List<string> source3 = address;
        int index3 = num3;
        int num4 = index3 + 1;
        string manifestName1 = source3.GetElement<string>(index3) ?? string.Empty;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(assetbundleName1, assetName1, manifestName1);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index4 = 1; index4 < excelData.MaxCell; ++index4)
        {
          List<string> list = excelData.list[index4].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num5 = 0;
            List<string> source4 = list;
            int index5 = num5;
            int num6 = index5 + 1;
            if (!source4.GetElement<string>(index5).IsNullOrEmpty())
            {
              List<string> source5 = list;
              int index6 = num6;
              int num7 = index6 + 1;
              string assetbundleName2 = source5.GetElement<string>(index6) ?? string.Empty;
              List<string> source6 = list;
              int index7 = num7;
              int num8 = index7 + 1;
              string assetName2 = source6.GetElement<string>(index7) ?? string.Empty;
              List<string> source7 = list;
              int index8 = num8;
              int num9 = index8 + 1;
              string manifestName2 = source7.GetElement<string>(index8) ?? string.Empty;
              Manager.Resources.LoadActionAnimationInfo(AssetUtility.LoadAsset<ExcelData>(assetbundleName2, assetName2, manifestName2), dic, awaitable);
            }
          }
        }
      }

      private void LoadMerchantOnlyActionAnimState(List<string> address, string id, bool awaitable)
      {
        this.LoadMerchantActionState(address, id, this.MerchantOnlyActionAnimStateTable, awaitable);
      }

      private void LoadMerchantCommonActionState(List<string> address, string id, bool awaitable)
      {
        this.LoadMerchantActionState(address, id, this.MerchantCommonActionAnimStateTable, awaitable);
      }

      private void LoadMerchantLocomotionStateList(List<string> address, string id, bool awaitable)
      {
        int num1 = 2;
        List<string> source1 = address;
        int index1 = num1;
        int num2 = index1 + 1;
        string assetbundleName = source1.GetElement<string>(index1) ?? string.Empty;
        List<string> source2 = address;
        int index2 = num2;
        int num3 = index2 + 1;
        string assetName = source2.GetElement<string>(index2) ?? string.Empty;
        List<string> source3 = address;
        int index3 = num3;
        int num4 = index3 + 1;
        string manifestName = source3.GetElement<string>(index3) ?? string.Empty;
        this.LoadLocomotionStateList(AssetUtility.LoadAsset<ExcelData>(assetbundleName, assetName, manifestName), this.MerchantLocomotionStateTable, awaitable);
      }

      private void LoadMerchantMoveInfoList(List<string> address, string id, bool awaitable)
      {
        int num1 = 2;
        List<string> source1 = address;
        int index1 = num1;
        int num2 = index1 + 1;
        string str1 = source1.GetElement<string>(index1) ?? string.Empty;
        List<string> source2 = address;
        int index2 = num2;
        int num3 = index2 + 1;
        string str2 = source2.GetElement<string>(index2) ?? string.Empty;
        if (!AssetBundleCheck.IsFile(str1, str2))
          return;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str1, str2, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index3 = 1; index3 < excelData.MaxCell; ++index3)
        {
          List<string> list = excelData.list[index3].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num4 = 0;
            List<string> source3 = list;
            int index4 = num4;
            int num5 = index4 + 1;
            int hash = Animator.StringToHash(source3.GetElement<string>(index4));
            List<AnimeMoveInfo> animeMoveInfoList1;
            if (!this.MerchantMoveInfoTable.TryGetValue(hash, out animeMoveInfoList1))
            {
              List<AnimeMoveInfo> animeMoveInfoList2 = new List<AnimeMoveInfo>();
              this.MerchantMoveInfoTable[hash] = animeMoveInfoList2;
              animeMoveInfoList1 = animeMoveInfoList2;
            }
            while (num5 < list.Count)
            {
              List<string> source4 = list;
              int index5 = num5;
              int num6 = index5 + 1;
              string element1 = source4.GetElement<string>(index5);
              List<string> source5 = list;
              int index6 = num6;
              int num7 = index6 + 1;
              string element2 = source5.GetElement<string>(index6);
              List<string> source6 = list;
              int index7 = num7;
              num5 = index7 + 1;
              string element3 = source6.GetElement<string>(index7);
              float result1;
              float result2;
              if (!element3.IsNullOrEmpty() && float.TryParse(element1, out result1) && float.TryParse(element2, out result2))
              {
                AnimeMoveInfo animeMoveInfo = new AnimeMoveInfo()
                {
                  start = result1,
                  end = result2,
                  movePoint = element3
                };
                animeMoveInfoList1.Add(animeMoveInfo);
              }
            }
          }
        }
      }

      private void LoadMerchantEventKeyList(List<string> address, string id, bool awaitable)
      {
        int num1 = 2;
        List<string> source1 = address;
        int index1 = num1;
        int num2 = index1 + 1;
        string element1 = source1.GetElement<string>(index1);
        List<string> source2 = address;
        int index2 = num2;
        int num3 = index2 + 1;
        string element2 = source2.GetElement<string>(index2);
        if (!AssetBundleCheck.IsFile(element1, element2))
          return;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(element1, element2, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index3 = 1; index3 < excelData.MaxCell; ++index3)
        {
          List<string> list = excelData.list[index3].list;
          int result;
          if (!list.IsNullOrEmpty<string>() && int.TryParse(list.GetElement<string>(0) ?? string.Empty, out result))
          {
            int num4 = 2;
            List<string> source3 = list;
            int index4 = num4;
            int num5 = index4 + 1;
            string element3 = source3.GetElement<string>(index4);
            List<string> source4 = list;
            int index5 = num5;
            int num6 = index5 + 1;
            string element4 = source4.GetElement<string>(index5);
            if (!element3.IsNullOrEmpty() && !element4.IsNullOrEmpty())
            {
              switch (result)
              {
                case 0:
                  this.LoadEventKeyTable(element3, element4, this.MerchantOnlyItemEventKeyTable, awaitable);
                  continue;
                case 1:
                  this.LoadEventKeyTable(element3, element4, this.MerchantCommonItemEventKeyTable, awaitable);
                  continue;
                case 2:
                  this.LoadSEEventKeyTable(element3, element4, this.MerchantOnlySEEventKeyTable, awaitable);
                  continue;
                case 3:
                  this.LoadSEEventKeyTable(element3, element4, this.MerchantCommonSEEventKeyTable, awaitable);
                  continue;
                case 4:
                  this.LoadParticleEventKeyTable(element3, element4, this.MerchantOnlyParticleEventKeyTable, awaitable);
                  continue;
                case 5:
                  this.LoadParticleEventKeyTable(element3, element4, this.MerchantCommonParticleEventKeyTable, awaitable);
                  continue;
                case 6:
                  this.LoadOnceVoiceEventKeyTable(element3, element4, this.MerchantOnlyOnceVoiceEventKeyTable, awaitable);
                  continue;
                case 7:
                  this.LoadOnceVoiceEventKeyTable(element3, element4, this.MerchantCommonOnceVoiceEventKeyTable, awaitable);
                  continue;
                case 8:
                  this.LoadLoopVoiceEventKeyTable(element3, element4, this.MerchantOnlyLoopVoiceEventKeyTable, awaitable);
                  continue;
                case 9:
                  this.LoadLoopVoiceEventKeyTable(element3, element4, this.MerchantCommonLoopVoiceEventKeyTable, awaitable);
                  continue;
                case 10:
                  this.LoadFootStepEventKeyTable(element3, element4, this.MerchantFootStepEventKeyTable, awaitable);
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }

      private void LoadMerchantListenerStateList(List<string> address, string id, bool awaitable)
      {
        int num1 = 2;
        List<string> source1 = address;
        int index1 = num1;
        int num2 = index1 + 1;
        string assetbundleName = source1.GetElement<string>(index1) ?? string.Empty;
        List<string> source2 = address;
        int index2 = num2;
        int num3 = index2 + 1;
        string assetName = source2.GetElement<string>(index2) ?? string.Empty;
        List<string> source3 = address;
        int index3 = num3;
        int num4 = index3 + 1;
        string manifestName = source3.GetElement<string>(index3) ?? string.Empty;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(assetbundleName, assetName, manifestName);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index4 = 1; index4 < excelData.MaxCell; ++index4)
        {
          List<string> list = excelData.list[index4].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num5 = 0;
            List<string> source4 = list;
            int index5 = num5;
            int num6 = index5 + 1;
            int result1;
            if (int.TryParse(source4.GetElement<string>(index5) ?? string.Empty, out result1))
            {
              int num7 = num6 + 1;
              List<string> source5 = list;
              int index6 = num7;
              int num8 = index6 + 1;
              int result2;
              if (int.TryParse(source5.GetElement<string>(index6) ?? string.Empty, out result2))
              {
                List<string> source6 = list;
                int index7 = num8;
                int num9 = index7 + 1;
                int result3;
                if (int.TryParse(source6.GetElement<string>(index7) ?? string.Empty, out result3))
                {
                  List<string> source7 = list;
                  int index8 = num9;
                  int num10 = index8 + 1;
                  int result4;
                  int num11 = !int.TryParse(source7.GetElement<string>(index8) ?? string.Empty, out result4) ? 0 : result4;
                  this.MerchantListenerRelationTable[result1] = num11;
                  PoseKeyPair poseKeyPair = new PoseKeyPair()
                  {
                    postureID = result2,
                    poseID = result3
                  };
                  this.MerchantListenerStateTable[result1] = poseKeyPair;
                }
              }
            }
          }
        }
      }

      private void LoadItemScaleTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.AgentAnimeInfo, false);
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string assetName = string.Format("itemscale_{0}", (object) System.IO.Path.GetFileNameWithoutExtension(str));
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, assetName, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            int num1 = 1;
            while (num1 < excelData.MaxCell)
            {
              ExcelData.Param obj = excelData.list[num1++];
              int num2 = 1;
              List<string> list1 = obj.list;
              int index2 = num2;
              int num3 = index2 + 1;
              int result1;
              if (int.TryParse(list1.GetElement<string>(index2), out result1))
              {
                List<string> list2 = obj.list;
                int index3 = num3;
                int num4 = index3 + 1;
                int result2;
                if (int.TryParse(list2.GetElement<string>(index3), out result2))
                {
                  List<string> list3 = obj.list;
                  int index4 = num4;
                  int num5 = index4 + 1;
                  string element1 = list3.GetElement<string>(index4);
                  List<string> list4 = obj.list;
                  int index5 = num5;
                  int num6 = index5 + 1;
                  string element2 = list4.GetElement<string>(index5);
                  List<string> list5 = obj.list;
                  int index6 = num6;
                  int num7 = index6 + 1;
                  string element3 = list5.GetElement<string>(index6);
                  float result3;
                  float result4;
                  float result5;
                  if (float.TryParse(element1, out result3) && float.TryParse(element2, out result4) && float.TryParse(element3, out result5))
                  {
                    Manager.Resources.TriValues triValues = new Manager.Resources.TriValues()
                    {
                      ScaleType = result2,
                      SThreshold = result3,
                      MThreshold = result4,
                      LThreshold = result5
                    };
                    this.ItemScaleTable[result1] = triValues;
                  }
                }
              }
            }
          }
        }
      }

      private void LoadLocomotionStateList(
        ExcelData excelData,
        Dictionary<int, PlayState> dic,
        bool awaitable)
      {
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        int num1 = 1;
        while (num1 < excelData.MaxCell)
        {
          ExcelData.Param obj = excelData.list[num1++];
          if (!obj.list.IsNullOrEmpty<string>())
          {
            int num2 = 0;
            List<string> list1 = obj.list;
            int index1 = num2;
            int num3 = index1 + 1;
            string element1 = list1.GetElement<string>(index1);
            int result1;
            if (!int.TryParse(element1, out result1))
            {
              Debug.LogWarning((object) string.Format("セルの読み込み失敗: {0} {1}行目 Type = {2}", (object) ((Object) excelData).get_name(), (object) num1, (object) element1));
            }
            else
            {
              int num4 = num3 + 1;
              List<string> list2 = obj.list;
              int index2 = num4;
              int num5 = index2 + 1;
              string element2 = list2.GetElement<string>(index2);
              List<string> list3 = obj.list;
              int index3 = num5;
              int num6 = index3 + 1;
              string element3 = list3.GetElement<string>(index3);
              List<string> list4 = obj.list;
              int index4 = num6;
              int num7 = index4 + 1;
              string[] strArray1 = list4.GetElement<string>(index4).Split(Manager.Resources.AnimationTables._separators, StringSplitOptions.RemoveEmptyEntries);
              List<string> list5 = obj.list;
              int index5 = num7;
              int num8 = index5 + 1;
              bool result2;
              bool flag1 = bool.TryParse(list5.GetElement<string>(index5), out result2) && result2;
              List<string> list6 = obj.list;
              int index6 = num8;
              int num9 = index6 + 1;
              float result3;
              float num10 = !float.TryParse(list6.GetElement<string>(index6), out result3) ? 0.0f : result3;
              List<string> list7 = obj.list;
              int index7 = num9;
              int num11 = index7 + 1;
              string[] strArray2 = list7.GetElement<string>(index7).Split(Manager.Resources.AnimationTables._separators, StringSplitOptions.RemoveEmptyEntries);
              List<string> list8 = obj.list;
              int index8 = num11;
              int num12 = index8 + 1;
              bool result4;
              bool flag2 = bool.TryParse(list8.GetElement<string>(index8), out result4) && result4;
              List<string> list9 = obj.list;
              int index9 = num12;
              int num13 = index9 + 1;
              float result5;
              float num14 = !float.TryParse(list9.GetElement<string>(index9), out result5) ? 0.0f : result5;
              List<string> list10 = obj.list;
              int index10 = num13;
              int num15 = index10 + 1;
              int result6;
              int layer_1 = !int.TryParse(list10.GetElement<string>(index10), out result6) ? 0 : result6;
              List<string> list11 = obj.list;
              int index11 = num15;
              int num16 = index11 + 1;
              string element4 = list11.GetElement<string>(index11);
              List<string> list12 = obj.list;
              int index12 = num16;
              int num17 = index12 + 1;
              int layer_2 = !int.TryParse(list12.GetElement<string>(index12), out result6) ? 0 : result6;
              PlayState playState1 = new PlayState()
              {
                Layer = layer_1
              };
              dic[result1] = playState1;
              PlayState playState2 = playState1;
              playState2.MainStateInfo.AssetBundleInfo = new AssetBundleInfo(string.Empty, element2, element3, string.Empty);
              playState2.MainStateInfo.InStateInfo = new PlayState.AnimStateInfo();
              if (!((IList<string>) strArray1).IsNullOrEmpty<string>())
              {
                PlayState.Info[] infoArray1 = new PlayState.Info[strArray1.Length];
                playState2.MainStateInfo.InStateInfo.StateInfos = infoArray1;
                PlayState.Info[] infoArray2 = infoArray1;
                for (int index13 = 0; index13 < infoArray2.Length; ++index13)
                  infoArray2[index13] = new PlayState.Info(strArray1[index13], layer_1);
              }
              playState2.MainStateInfo.OutStateInfo = new PlayState.AnimStateInfo();
              if (!((IList<string>) strArray2).IsNullOrEmpty<string>())
              {
                PlayState.Info[] infoArray1 = new PlayState.Info[strArray2.Length];
                playState2.MainStateInfo.OutStateInfo.StateInfos = infoArray1;
                PlayState.Info[] infoArray2 = infoArray1;
                for (int index13 = 0; index13 < infoArray2.Length; ++index13)
                  infoArray2[index13] = new PlayState.Info(strArray2[index13], layer_1);
              }
              playState2.MainStateInfo.InStateInfo.EnableFade = flag1;
              playState2.MainStateInfo.InStateInfo.FadeSecond = num10;
              playState2.MainStateInfo.OutStateInfo.EnableFade = flag2;
              playState2.MainStateInfo.OutStateInfo.FadeSecond = num14;
              playState2.MaskStateInfo = new PlayState.Info(element4, layer_2);
            }
          }
        }
      }

      private enum AnimatorLoadMap
      {
        Chara,
        Player,
        Merchant,
        Item,
      }

      private enum AgentLoadingMap
      {
        Personal,
        Action,
        Move,
        EventKey,
        Locomotion,
        Common,
        Greet,
        TalkSpeaker,
        TalkListener,
        Animal,
      }

      private enum PlayerLoadingMap
      {
        Action,
        Move,
        EventKey,
        Locomotion,
        Common,
      }

      public enum CommonLoadMap
      {
        Stand,
        Chair,
        Sit,
        Sleep,
      }
    }

    public class TriValues
    {
      public int ScaleType { get; set; }

      public float SThreshold { get; set; }

      public float MThreshold { get; set; }

      public float LThreshold { get; set; }
    }

    public class BehaviorTreeTables
    {
      private Dictionary<Desire.ActionType, AgentBehaviorTree> _behaviorTree = new Dictionary<Desire.ActionType, AgentBehaviorTree>();
      private Dictionary<Merchant.ActionType, MerchantBehaviorTree> _merchantBehaviorTree = new Dictionary<Merchant.ActionType, MerchantBehaviorTree>();
      private Dictionary<AIProject.Definitions.Tutorial.ActionType, AgentBehaviorTree> _tutrialBehaviorTree = new Dictionary<AIProject.Definitions.Tutorial.ActionType, AgentBehaviorTree>();

      public AgentBehaviorTree GetBehavior(Desire.ActionType actionType)
      {
        AgentBehaviorTree agentBehaviorTree;
        return !this._behaviorTree.TryGetValue(actionType, out agentBehaviorTree) ? (AgentBehaviorTree) null : agentBehaviorTree;
      }

      public MerchantBehaviorTree GetMerchantBehavior(
        Merchant.ActionType actionType)
      {
        MerchantBehaviorTree merchantBehaviorTree;
        return !this._merchantBehaviorTree.TryGetValue(actionType, out merchantBehaviorTree) ? (MerchantBehaviorTree) null : merchantBehaviorTree;
      }

      public AgentBehaviorTree GetTutorialBehavior(AIProject.Definitions.Tutorial.ActionType actionType)
      {
        AgentBehaviorTree agentBehaviorTree;
        this._tutrialBehaviorTree.TryGetValue(actionType, out agentBehaviorTree);
        return agentBehaviorTree;
      }

      public void Load(DefinePack definePack)
      {
        this.LoadAgentBehaviorTree(definePack);
        this.LoadMerchantBehaviorTree(definePack);
        this.LoadTutorialBehaviorTree(definePack);
      }

      public void Release()
      {
        this._behaviorTree.Clear();
        this._merchantBehaviorTree.Clear();
        this._tutrialBehaviorTree.Clear();
      }

      private void LoadAgentBehaviorTree(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.BehaviorTree, false);
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
            {
              ExcelData.Param element = excelData.list.GetElement<ExcelData.Param>(index2);
              if (element != null && !element.list.IsNullOrEmpty<string>())
              {
                int num1 = 0;
                List<string> list1 = element.list;
                int index3 = num1;
                int num2 = index3 + 1;
                Desire.ActionType result;
                if (Enum.TryParse<Desire.ActionType>(list1[index3], out result))
                {
                  int num3 = num2 + 1;
                  List<string> list2 = element.list;
                  int index4 = num3;
                  int num4 = index4 + 1;
                  string assetbundleName = list2[index4];
                  List<string> list3 = element.list;
                  int index5 = num4;
                  int num5 = index5 + 1;
                  string assetName = list3[index5];
                  GameObject gameObject = AssetUtility.LoadAsset<GameObject>(assetbundleName, assetName, string.Empty);
                  if (Object.op_Equality((Object) gameObject, (Object) null))
                    Debug.Log((object) string.Format("見つからない [ モード: {0} アセットバンドル: {1} アセット: {2} ]", (object) result, (object) assetbundleName, (object) assetName));
                  else
                    this._behaviorTree[result] = (AgentBehaviorTree) gameObject.GetComponent<AgentBehaviorTree>();
                }
              }
            }
          }
        }
      }

      private void LoadMerchantBehaviorTree(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.MerchantBehaviorTree, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
            {
              List<string> list = excelData.list[index2].list;
              Merchant.ActionType result;
              if (!list.IsNullOrEmpty<string>() && Enum.TryParse<Merchant.ActionType>(list.GetElement<string>(0) ?? string.Empty, out result))
              {
                int num1 = 2;
                List<string> source1 = list;
                int index3 = num1;
                int num2 = index3 + 1;
                string assetbundleName = source1.GetElement<string>(index3) ?? string.Empty;
                List<string> source2 = list;
                int index4 = num2;
                int num3 = index4 + 1;
                string assetName = source2.GetElement<string>(index4) ?? string.Empty;
                List<string> source3 = list;
                int index5 = num3;
                int num4 = index5 + 1;
                string manifestName = source3.GetElement<string>(index5) ?? string.Empty;
                MerchantBehaviorTree component = (MerchantBehaviorTree) AssetUtility.LoadAsset<GameObject>(assetbundleName, assetName, manifestName).GetComponent<MerchantBehaviorTree>();
                if (!Object.op_Equality((Object) component, (Object) null))
                  this._merchantBehaviorTree[result] = component;
              }
            }
          }
        }
      }

      private void LoadTutorialBehaviorTree(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.TutorialBehaviorTree, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
            {
              List<string> list = excelData.list[index2].list;
              if (!list.IsNullOrEmpty<string>())
              {
                int num1 = 0;
                List<string> source1 = list;
                int index3 = num1;
                int num2 = index3 + 1;
                AIProject.Definitions.Tutorial.ActionType result;
                if (Enum.TryParse<AIProject.Definitions.Tutorial.ActionType>(source1.GetElement<string>(index3) ?? string.Empty, out result))
                {
                  int num3 = num2 + 1;
                  List<string> source2 = list;
                  int index4 = num3;
                  int num4 = index4 + 1;
                  string assetbundleName = source2.GetElement<string>(index4) ?? string.Empty;
                  List<string> source3 = list;
                  int index5 = num4;
                  int num5 = index5 + 1;
                  string assetName = source3.GetElement<string>(index5) ?? string.Empty;
                  AgentBehaviorTree component = (AgentBehaviorTree) AssetUtility.LoadAsset<GameObject>(assetbundleName, assetName, string.Empty)?.GetComponent<AgentBehaviorTree>();
                  if (!Object.op_Equality((Object) component, (Object) null))
                    this._tutrialBehaviorTree[result] = component;
                }
              }
            }
          }
        }
      }
    }

    public class FishingTable
    {
      private string[] separators = new string[3]
      {
        "/",
        "／",
        ","
      };
      private int mSizeFishCount;

      public Dictionary<int, RuntimeAnimatorController> PlayerAnimatorTable { get; private set; } = new Dictionary<int, RuntimeAnimatorController>();

      public Dictionary<int, PlayState> PlayerAnimStateTable { get; private set; } = new Dictionary<int, PlayState>();

      public Dictionary<string, List<AIProject.Player.Fishing.Schedule>> AnimEventScheduler { get; private set; } = new Dictionary<string, List<AIProject.Player.Fishing.Schedule>>();

      public Dictionary<int, FishingRodInfo> RodInfos { get; private set; } = new Dictionary<int, FishingRodInfo>();

      public Dictionary<int, Tuple<AssetBundleInfo, RuntimeAnimatorController, string>> FishBodyTable { get; private set; } = new Dictionary<int, Tuple<AssetBundleInfo, RuntimeAnimatorController, string>>();

      public Dictionary<int, AssetBundleInfo> EffectTable { get; private set; } = new Dictionary<int, AssetBundleInfo>();

      public Dictionary<int, FishFoodInfo> FishFoodInfoTable { get; private set; } = new Dictionary<int, FishFoodInfo>();

      public Dictionary<int, Dictionary<int, float>> FishHitBaseRangeTable { get; private set; } = new Dictionary<int, Dictionary<int, float>>();

      public Dictionary<int, Manager.Resources.FishBaitHitInfo> FishBaitHitInfoTable { get; private set; } = new Dictionary<int, Manager.Resources.FishBaitHitInfo>();

      public Dictionary<int, Dictionary<int, FishInfo>> FishInfoTable { get; private set; } = new Dictionary<int, Dictionary<int, FishInfo>>();

      public Dictionary<int, Dictionary<int, FishInfo>> FishInfoSizeGroupTable { get; private set; } = new Dictionary<int, Dictionary<int, FishInfo>>();

      public Dictionary<int, Tuple<GameObject, RuntimeAnimatorController>> FishModelTable { get; private set; } = new Dictionary<int, Tuple<GameObject, RuntimeAnimatorController>>();

      public Dictionary<int, Dictionary<int, int>> FishSizeTable { get; private set; } = new Dictionary<int, Dictionary<int, int>>();

      public float ResultFishReferenceExtent { get; private set; } = 1f;

      public ValueTuple<int, int>[] GetItemIDInSizeTalbe(int _sizeID)
      {
        List<ValueTuple<int, int>> toRelease = ListPool<ValueTuple<int, int>>.Get();
        foreach (KeyValuePair<int, Dictionary<int, int>> keyValuePair1 in this.FishSizeTable)
        {
          foreach (KeyValuePair<int, int> keyValuePair2 in keyValuePair1.Value)
          {
            if (keyValuePair2.Value == _sizeID)
              toRelease.Add(new ValueTuple<int, int>(keyValuePair1.Key, keyValuePair2.Key));
          }
        }
        ValueTuple<int, int>[] valueTupleArray = new ValueTuple<int, int>[toRelease.Count];
        for (int index = 0; index < toRelease.Count; ++index)
          valueTupleArray[index] = toRelease[index];
        ListPool<ValueTuple<int, int>>.Release(toRelease);
        return valueTupleArray;
      }

      public bool TryGetFishSize(int _categoryID, int _itemID, out int _sizeID)
      {
        Dictionary<int, int> dictionary;
        int num;
        if (this.FishSizeTable.TryGetValue(_categoryID, out dictionary) && dictionary.TryGetValue(_itemID, out num))
        {
          _sizeID = num;
          return true;
        }
        _sizeID = 0;
        return false;
      }

      private string LogAssetBundleInfo(AssetBundleInfo _info)
      {
        return string.Format("AssetBundleName[{0}] AssetName[{1}] ManifestName[{2}]", (object) _info.assetbundle, (object) _info.asset, (object) _info.manifest);
      }

      private string LogAssetBundleInfo(AssetBundleInfo _info, string _ver)
      {
        return string.Format("AssetBundleName[{0}] AssetName[{1}] ManifestName[{2}] Ver[{3}]", (object) _info.assetbundle, (object) _info.asset, (object) _info.manifest, (object) _ver);
      }

      private string LogAssetBundleInfo(AssetBundleInfo _info, string _ver, int _row, int _clm)
      {
        return string.Format("AssetBundleName[{0}] AssetName[{1}] ManifestName[{2}] Ver[{3}] Row[{4}] Clm[{5}]", (object) _info.assetbundle, (object) _info.asset, (object) _info.manifest, (object) _ver, (object) _row, (object) _clm);
      }

      private AssetBundleInfo GetAssetInfo(
        List<string> _address,
        ref int _idx,
        bool _addSummary)
      {
        string str1;
        if (_addSummary)
        {
          List<string> source = _address;
          int num;
          _idx = (num = _idx) + 1;
          int index = num;
          str1 = source.GetElement<string>(index) ?? string.Empty;
        }
        else
          str1 = string.Empty;
        string str2 = str1;
        List<string> source1 = _address;
        int num1;
        _idx = (num1 = _idx) + 1;
        int index1 = num1;
        string str3 = source1.GetElement<string>(index1) ?? string.Empty;
        List<string> source2 = _address;
        int num2;
        _idx = (num2 = _idx) + 1;
        int index2 = num2;
        string str4 = source2.GetElement<string>(index2) ?? string.Empty;
        List<string> source3 = _address;
        int num3;
        _idx = (num3 = _idx) + 1;
        int index3 = num3;
        string str5 = source3.GetElement<string>(index3) ?? string.Empty;
        return new AssetBundleInfo(str2, str3, str4, str5);
      }

      private void LoadExcelData(
        List<string> _address,
        ref int _idx,
        out AssetBundleInfo _info,
        out ExcelData _data,
        bool _addSummary)
      {
        string str1;
        if (_addSummary)
        {
          List<string> source = _address;
          int num;
          _idx = (num = _idx) + 1;
          int index = num;
          str1 = source.GetElement<string>(index) ?? string.Empty;
        }
        else
          str1 = string.Empty;
        string str2 = str1;
        ref AssetBundleInfo local = ref _info;
        string str3 = str2;
        List<string> source1 = _address;
        int num1;
        _idx = (num1 = _idx) + 1;
        int index1 = num1;
        string str4 = source1.GetElement<string>(index1) ?? string.Empty;
        List<string> source2 = _address;
        int num2;
        _idx = (num2 = _idx) + 1;
        int index2 = num2;
        string str5 = source2.GetElement<string>(index2) ?? string.Empty;
        List<string> source3 = _address;
        int num3;
        _idx = (num3 = _idx) + 1;
        int index3 = num3;
        string str6 = source3.GetElement<string>(index3) ?? string.Empty;
        ((AssetBundleInfo) ref local).\u002Ector(str3, str4, str5, str6);
        _data = AssetUtility.LoadAsset<ExcelData>(_info);
      }

      public void Load(FishingDefinePack _fishingDefinePack)
      {
        this.mSizeFishCount = 0;
        this.ResultFishReferenceExtent = 1f;
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(_fishingDefinePack.AssetBundleNames.FishingInfoListBundleDirectory, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          if (AssetBundleCheck.IsFile(str, withoutExtension))
          {
            ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
              {
                List<string> list = excelData.list[index2].list;
                if (!list.IsNullOrEmpty<string>())
                {
                  int _idx = 0;
                  int result;
                  if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result))
                  {
                    AssetBundleInfo assetInfo = this.GetAssetInfo(list, ref _idx, true);
                    switch (result)
                    {
                      case 0:
                        this.LoadFishingData(assetInfo, withoutExtension);
                        continue;
                      case 1:
                        this.LoadFishingAnimInfo(assetInfo, withoutExtension);
                        continue;
                      case 2:
                        this.LoadFishingInfoList(assetInfo, withoutExtension);
                        continue;
                      default:
                        continue;
                    }
                  }
                }
              }
            }
          }
        }
        if (0 < this.mSizeFishCount)
        {
          Dictionary<int, FishInfo> source;
          if (!this.FishInfoSizeGroupTable.TryGetValue(1, out source) || source.IsNullOrEmpty<int, FishInfo>())
            return;
          List<int> toRelease = ListPool<int>.Get();
          foreach (KeyValuePair<int, FishInfo> keyValuePair in source)
          {
            if (!toRelease.Contains(keyValuePair.Value.ModelID))
              toRelease.Add(keyValuePair.Value.ModelID);
          }
          float num1 = 1f;
          int num2 = 0;
          foreach (int key in toRelease)
          {
            Tuple<GameObject, RuntimeAnimatorController> tuple;
            if (this.FishModelTable.TryGetValue(key, out tuple) && Object.op_Inequality((Object) tuple?.Item1, (Object) null))
            {
              Renderer componentInChildren = (Renderer) tuple.Item1.GetComponentInChildren<Renderer>(true);
              if (Object.op_Inequality((Object) componentInChildren, (Object) null))
              {
                double num3 = (double) num1;
                Bounds bounds1 = componentInChildren.get_bounds();
                // ISSUE: variable of the null type
                __Null y = ((Bounds) ref bounds1).get_extents().y;
                Bounds bounds2 = componentInChildren.get_bounds();
                // ISSUE: variable of the null type
                __Null z = ((Bounds) ref bounds2).get_extents().z;
                double num4 = (double) Mathf.Max((float) y, (float) z);
                num1 = (float) (num3 + num4);
                ++num2;
              }
            }
          }
          if (0 < num2)
            this.ResultFishReferenceExtent = num1 / (float) toRelease.Count;
          ListPool<int>.Release(toRelease);
        }
        else
        {
          if (this.FishModelTable.IsNullOrEmpty<int, Tuple<GameObject, RuntimeAnimatorController>>())
            return;
          float num1 = 1f;
          int num2 = 0;
          using (Dictionary<int, Tuple<GameObject, RuntimeAnimatorController>>.Enumerator enumerator = this.FishModelTable.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GameObject gameObject = enumerator.Current.Value?.Item1;
              Renderer renderer = !Object.op_Inequality((Object) gameObject, (Object) null) ? (Renderer) null : (Renderer) gameObject.GetComponentInChildren<Renderer>(true);
              if (Object.op_Inequality((Object) renderer, (Object) null))
              {
                double num3 = (double) num1;
                Bounds bounds1 = renderer.get_bounds();
                // ISSUE: variable of the null type
                __Null y = ((Bounds) ref bounds1).get_extents().y;
                Bounds bounds2 = renderer.get_bounds();
                // ISSUE: variable of the null type
                __Null z = ((Bounds) ref bounds2).get_extents().z;
                double num4 = (double) Mathf.Max((float) y, (float) z);
                num1 = (float) (num3 + num4);
                ++num2;
              }
            }
          }
          if (0 >= num2)
            return;
          this.ResultFishReferenceExtent = num1 / (float) num2;
        }
      }

      public void Release()
      {
        this.PlayerAnimatorTable.Clear();
        this.PlayerAnimStateTable.Clear();
        this.AnimEventScheduler.Clear();
        this.RodInfos.Clear();
        this.FishBodyTable.Clear();
        this.EffectTable.Clear();
        this.FishFoodInfoTable.Clear();
        this.FishHitBaseRangeTable.Clear();
        this.FishBaitHitInfoTable.Clear();
        this.FishInfoTable.Clear();
        this.FishInfoSizeGroupTable.Clear();
        this.FishModelTable.Clear();
        this.FishSizeTable.Clear();
      }

      private void LoadFishingAnimInfo(AssetBundleInfo _info, string _ver)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_info);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int _idx = 0;
            int result;
            if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result))
            {
              AssetBundleInfo assetInfo = this.GetAssetInfo(list, ref _idx, true);
              switch (result)
              {
                case 0:
                  this.LoadPlayerFishingAnimator(assetInfo, _ver);
                  continue;
                case 1:
                  this.LoadFishingAnimState(assetInfo, _ver);
                  continue;
                case 2:
                  this.LoadFishingAnimEvent(assetInfo, _ver);
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }

      private void LoadPlayerFishingAnimator(AssetBundleInfo _info, string _ver)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_info);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int _idx = 0;
            int result;
            if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result))
            {
              RuntimeAnimatorController animatorController = AssetUtility.LoadAsset<RuntimeAnimatorController>(this.GetAssetInfo(list, ref _idx, true));
              if (!Object.op_Equality((Object) animatorController, (Object) null))
                this.PlayerAnimatorTable[result] = animatorController;
            }
          }
        }
      }

      private void LoadFishingAnimState(AssetBundleInfo _info, string _ver)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_info);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        int index1 = 0;
        int index2 = 3;
        int index3 = 4;
        for (int index4 = 1; index4 < excelData.MaxCell; ++index4)
        {
          List<string> list = excelData.list[index4].list;
          if (!list.IsNullOrEmpty<string>())
          {
            string s1 = list.GetElement<string>(index1) ?? string.Empty;
            string[] strArray = (list.GetElement<string>(index2) ?? string.Empty).Split(this.separators, StringSplitOptions.RemoveEmptyEntries);
            string s2 = list.GetElement<string>(index3) ?? string.Empty;
            int result1 = 0;
            int result2 = 0;
            if (!strArray.IsNullOrEmpty<string>() && int.TryParse(s1, out result1) && int.TryParse(s2, out result2))
            {
              PlayState playState = new PlayState(result2, strArray, (string[]) null);
              this.PlayerAnimStateTable[result1] = playState;
            }
          }
        }
      }

      private void LoadFishingAnimEvent(AssetBundleInfo _info, string _ver)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_info);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        int index1 = 0;
        int index2 = 1;
        int index3 = 2;
        int index4 = 3;
        for (int index5 = 1; index5 < excelData.MaxCell; ++index5)
        {
          List<string> list = excelData.list[index5].list;
          if (!list.IsNullOrEmpty<string>())
          {
            string element1 = list.GetElement<string>(index1);
            string element2 = list.GetElement<string>(index2);
            string element3 = list.GetElement<string>(index3);
            string element4 = list.GetElement<string>(index4);
            int result1 = 0;
            float result2 = 0.0f;
            if (!element1.IsNullOrEmpty() && int.TryParse(element2, out result1) && float.TryParse(element3, out result2))
            {
              AIProject.Player.Fishing.Schedule schedule = new AIProject.Player.Fishing.Schedule(element1, result1, result2, element4);
              List<AIProject.Player.Fishing.Schedule> scheduleList = (List<AIProject.Player.Fishing.Schedule>) null;
              if (!this.AnimEventScheduler.TryGetValue(element1, out scheduleList))
                this.AnimEventScheduler[element1] = scheduleList = new List<AIProject.Player.Fishing.Schedule>();
              scheduleList.Add(schedule);
              this.AnimEventScheduler[element1] = scheduleList;
            }
          }
        }
      }

      private void LoadFishingData(AssetBundleInfo _info, string _ver)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_info);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int _idx = 0;
            int result;
            if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result))
            {
              AssetBundleInfo assetInfo = this.GetAssetInfo(list, ref _idx, true);
              switch (result)
              {
                case 0:
                  this.LoadFishingParticle(assetInfo, _ver);
                  continue;
                case 1:
                  this.LoadFishShadow(assetInfo, _ver);
                  continue;
                case 2:
                  this.LoadFishingRod(assetInfo, _ver);
                  continue;
                case 3:
                  this.LoadFishModel(assetInfo, _ver);
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }

      private void LoadFishingParticle(AssetBundleInfo _info, string _ver)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_info);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int _idx = 0;
            int result;
            if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result))
            {
              AssetBundleInfo assetInfo = this.GetAssetInfo(list, ref _idx, true);
              if (!((string) assetInfo.assetbundle).IsNullOrEmpty() && !((string) assetInfo.asset).IsNullOrEmpty() && bool.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out bool _))
                this.EffectTable[result] = assetInfo;
            }
          }
        }
      }

      private void LoadFishShadow(AssetBundleInfo _info, string _ver)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_info);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int _idx = 0;
            int result;
            if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result))
            {
              AssetBundleInfo assetInfo1 = this.GetAssetInfo(list, ref _idx, true);
              if (!((string) assetInfo1.assetbundle).IsNullOrEmpty() && !((string) assetInfo1.asset).IsNullOrEmpty())
              {
                AssetBundleInfo assetInfo2 = this.GetAssetInfo(list, ref _idx, false);
                if (!((string) assetInfo2.assetbundle).IsNullOrEmpty() && !((string) assetInfo2.asset).IsNullOrEmpty())
                {
                  RuntimeAnimatorController animatorController = AssetUtility.LoadAsset<RuntimeAnimatorController>(assetInfo2);
                  if (!Object.op_Equality((Object) animatorController, (Object) null))
                  {
                    string str = list.GetElement<string>(_idx++) ?? string.Empty;
                    this.FishBodyTable[result] = new Tuple<AssetBundleInfo, RuntimeAnimatorController, string>(assetInfo1, animatorController, str);
                  }
                }
              }
            }
          }
        }
      }

      private void LoadFishingRod(AssetBundleInfo _info, string _ver)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_info);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int _idx = 0;
            int result;
            if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result))
            {
              AssetBundleInfo assetInfo = this.GetAssetInfo(list, ref _idx, false);
              GameObject _rod = CommonLib.LoadAsset<GameObject>((string) assetInfo.assetbundle, (string) assetInfo.asset, false, (string) assetInfo.manifest);
              if (!Object.op_Equality((Object) _rod, (Object) null))
              {
                Singleton<Manager.Resources>.Instance.AddLoadAssetBundle((string) assetInfo.assetbundle, (string) assetInfo.manifest);
                RuntimeAnimatorController _controller = AssetUtility.LoadAsset<RuntimeAnimatorController>(this.GetAssetInfo(list, ref _idx, false));
                if (!Object.op_Equality((Object) _controller, (Object) null))
                {
                  string str = list.GetElement<string>(_idx++) ?? string.Empty;
                  if (!str.IsNullOrEmpty())
                    this.RodInfos[result] = new FishingRodInfo(_rod, _controller, str);
                }
              }
            }
          }
        }
      }

      private void LoadFishModel(AssetBundleInfo _info, string _ver)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_info);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int _idx = 0;
            int result;
            if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result))
            {
              AssetBundleInfo assetInfo1 = this.GetAssetInfo(list, ref _idx, false);
              GameObject gameObject = CommonLib.LoadAsset<GameObject>((string) assetInfo1.assetbundle, (string) assetInfo1.asset, false, (string) assetInfo1.manifest);
              if (!Object.op_Equality((Object) gameObject, (Object) null))
              {
                Singleton<Manager.Resources>.Instance.AddLoadAssetBundle((string) assetInfo1.assetbundle, (string) assetInfo1.manifest);
                AssetBundleInfo assetInfo2 = this.GetAssetInfo(list, ref _idx, false);
                RuntimeAnimatorController animatorController = (RuntimeAnimatorController) null;
                if (!((string) assetInfo2.assetbundle).IsNullOrEmpty() && !((string) assetInfo2.asset).IsNullOrEmpty())
                  animatorController = AssetUtility.LoadAsset<RuntimeAnimatorController>(assetInfo2);
                this.FishModelTable[result] = new Tuple<GameObject, RuntimeAnimatorController>(gameObject, animatorController);
              }
            }
          }
        }
      }

      private void LoadFishingInfoList(AssetBundleInfo _info, string _ver)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_info);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int _idx = 0;
            int result;
            if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result))
            {
              AssetBundleInfo assetInfo = this.GetAssetInfo(list, ref _idx, true);
              switch (result)
              {
                case 0:
                  this.LoadFishFoodInfo(assetInfo, _ver);
                  continue;
                case 1:
                  this.LoadFishInfo(assetInfo, _ver);
                  continue;
                case 2:
                  this.LoadFishHitBaseRange(assetInfo, _ver);
                  continue;
                case 3:
                  this.LoadFishBaitHitInfoList(assetInfo);
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }

      private void LoadFishFoodInfo(AssetBundleInfo _info, string _ver)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_info);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              string str = source2.GetElement<string>(index3) ?? string.Empty;
              if (!str.IsNullOrEmpty())
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                string s1 = source3.GetElement<string>(index4) ?? string.Empty;
                List<string> source4 = list;
                int index5 = num4;
                int num5 = index5 + 1;
                string s2 = source4.GetElement<string>(index5) ?? string.Empty;
                List<string> source5 = list;
                int index6 = num5;
                int num6 = index6 + 1;
                string s3 = source5.GetElement<string>(index6) ?? string.Empty;
                int result2 = !int.TryParse(s1, out result2) ? 0 : result2;
                int result3 = !int.TryParse(s2, out result3) ? 0 : result3;
                int result4 = !int.TryParse(s3, out result4) ? 0 : result4;
                this.FishFoodInfoTable[result1] = new FishFoodInfo(result1, str, result2, result3, result4);
              }
            }
          }
        }
      }

      private void LoadFishInfo(AssetBundleInfo _info, string _ver)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_info);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                string _itemName = source3.GetElement<string>(index4) ?? string.Empty;
                List<string> source4 = list;
                int index5 = num4;
                int num5 = index5 + 1;
                int result3;
                if (int.TryParse(source4.GetElement<string>(index5) ?? string.Empty, out result3))
                {
                  List<string> source5 = list;
                  int index6 = num5;
                  int num6 = index6 + 1;
                  int result4;
                  if (int.TryParse(source5.GetElement<string>(index6) ?? string.Empty, out result4))
                  {
                    List<string> source6 = list;
                    int index7 = num6;
                    int num7 = index7 + 1;
                    int result5;
                    if (int.TryParse(source6.GetElement<string>(index7) ?? string.Empty, out result5))
                    {
                      List<string> source7 = list;
                      int index8 = num7;
                      int num8 = index8 + 1;
                      string s1 = source7.GetElement<string>(index8) ?? string.Empty;
                      List<string> source8 = list;
                      int index9 = num8;
                      int num9 = index9 + 1;
                      string s2 = source8.GetElement<string>(index9) ?? string.Empty;
                      int result6;
                      bool flag1 = int.TryParse(s1, out result6);
                      int result7;
                      bool flag2 = int.TryParse(s2, out result7);
                      if (flag1 || flag2)
                      {
                        if (!flag1)
                          result6 = result7;
                        else if (!flag2)
                        {
                          result7 = result6;
                        }
                        else
                        {
                          int num10 = Mathf.Min(result6, result7);
                          int num11 = Mathf.Max(result6, result7);
                          result6 = num10;
                          result7 = num11;
                        }
                        List<string> source9 = list;
                        int index10 = num9;
                        int num12 = index10 + 1;
                        int result8;
                        if (!int.TryParse(source9.GetElement<string>(index10) ?? string.Empty, out result8))
                          result8 = 0;
                        List<string> source10 = list;
                        int index11 = num12;
                        int num13 = index11 + 1;
                        float result9;
                        if (!float.TryParse(source10.GetElement<string>(index11) ?? string.Empty, out result9))
                          result9 = 0.0f;
                        FishInfo fishInfo = new FishInfo(result1, result2, _itemName, result3, result4, result5, result6, result7, result8, result9);
                        Dictionary<int, FishInfo> dictionary1;
                        if (!this.FishInfoSizeGroupTable.TryGetValue(result3, out dictionary1) || dictionary1 == null)
                        {
                          Dictionary<int, FishInfo> dictionary2 = new Dictionary<int, FishInfo>();
                          this.FishInfoSizeGroupTable[result3] = dictionary2;
                          dictionary1 = dictionary2;
                        }
                        dictionary1[result2] = fishInfo;
                        Dictionary<int, FishInfo> dictionary3;
                        if (!this.FishInfoTable.TryGetValue(result1, out dictionary3) || dictionary3 == null)
                          this.FishInfoTable[result1] = dictionary3 = new Dictionary<int, FishInfo>();
                        dictionary3[result2] = fishInfo;
                        if (result3 == 1)
                          ++this.mSizeFishCount;
                        Dictionary<int, int> dictionary4;
                        if (!this.FishSizeTable.TryGetValue(result1, out dictionary4))
                        {
                          Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
                          this.FishSizeTable[result1] = dictionary2;
                          dictionary4 = dictionary2;
                        }
                        dictionary4[result2] = result3;
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadFishHitBaseRange(AssetBundleInfo _info, string _ver)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_info);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                int result3;
                if (int.TryParse(source3.GetElement<string>(index4) ?? string.Empty, out result3))
                {
                  Dictionary<int, float> dictionary1;
                  if (!this.FishHitBaseRangeTable.TryGetValue(result1, out dictionary1))
                  {
                    Dictionary<int, float> dictionary2 = new Dictionary<int, float>();
                    this.FishHitBaseRangeTable[result1] = dictionary2;
                    dictionary1 = dictionary2;
                  }
                  dictionary1[result2] = (float) result3;
                }
              }
            }
          }
        }
      }

      private void LoadFishBaitHitInfoList(AssetBundleInfo _sheetInfo)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result))
            {
              int num3 = num2 + 1;
              List<string> source2 = list;
              int index3 = num3;
              int num4 = index3 + 1;
              string element1 = source2.GetElement<string>(index3);
              List<string> source3 = list;
              int index4 = num4;
              int num5 = index4 + 1;
              string element2 = source3.GetElement<string>(index4);
              List<string> source4 = list;
              int index5 = num5;
              int num6 = index5 + 1;
              string element3 = source4.GetElement<string>(index5);
              if (!element1.IsNullOrEmpty() && !element2.IsNullOrEmpty())
              {
                AssetBundleInfo _sheetInfo1;
                ((AssetBundleInfo) ref _sheetInfo1).\u002Ector(string.Empty, element1, element2, element3);
                this.LoadBaitHitInfo(_sheetInfo1, result);
              }
            }
          }
        }
      }

      private void LoadBaitHitInfo(AssetBundleInfo _sheetInfo, int _baitID)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        Manager.Resources.FishBaitHitInfo fishBaitHitInfo;
        if (!this.FishBaitHitInfoTable.TryGetValue(_baitID, out fishBaitHitInfo) || fishBaitHitInfo == null)
          this.FishBaitHitInfoTable[_baitID] = fishBaitHitInfo = new Manager.Resources.FishBaitHitInfo();
        else
          fishBaitHitInfo.HitList.Clear();
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<ValueTuple<int, int, int>> valueTupleList = ListPool<ValueTuple<int, int, int>>.Get();
              while (num2 < list.Count)
              {
                List<string> source2 = list;
                int index3 = num2;
                int num3 = index3 + 1;
                string s1 = source2.GetElement<string>(index3) ?? string.Empty;
                int num4 = num3 + 1;
                List<string> source3 = list;
                int index4 = num4;
                int num5 = index4 + 1;
                string s2 = source3.GetElement<string>(index4) ?? string.Empty;
                List<string> source4 = list;
                int index5 = num5;
                num2 = index5 + 1;
                string s3 = source4.GetElement<string>(index5) ?? string.Empty;
                int result2;
                int result3;
                int result4;
                if (int.TryParse(s1, out result2) && int.TryParse(s2, out result3) && int.TryParse(s3, out result4))
                  valueTupleList.Add(new ValueTuple<int, int, int>(result2, result3, result4));
              }
              if (!valueTupleList.IsNullOrEmpty<ValueTuple<int, int, int>>())
                fishBaitHitInfo.HitList.Add(new Manager.Resources.FishBaitHitInfo.HitInfo()
                {
                  Range = result1,
                  FishList = new List<ValueTuple<int, int, int>>((IEnumerable<ValueTuple<int, int, int>>) valueTupleList)
                });
              ListPool<ValueTuple<int, int, int>>.Release(valueTupleList);
            }
          }
        }
      }

      public bool TryGetBaitHitInfo(int baitID, out ValueTuple<int, int, int> hitInfo)
      {
        Manager.Resources.FishBaitHitInfo fishBaitHitInfo;
        if (this.FishBaitHitInfoTable.TryGetValue(baitID, out fishBaitHitInfo) && fishBaitHitInfo != null && !fishBaitHitInfo.HitList.IsNullOrEmpty<Manager.Resources.FishBaitHitInfo.HitInfo>())
          return fishBaitHitInfo.GetFishInfo(out hitInfo);
        ((ValueTuple<int, int, int>) ref hitInfo).\u002Ector(0, -1, -1);
        return false;
      }
    }

    public class FishBaitHitInfo
    {
      public List<Manager.Resources.FishBaitHitInfo.HitInfo> HitList { get; private set; } = new List<Manager.Resources.FishBaitHitInfo.HitInfo>();

      public bool GetFishInfo(out ValueTuple<int, int, int> _info)
      {
        if (this.HitList.IsNullOrEmpty<Manager.Resources.FishBaitHitInfo.HitInfo>())
        {
          ((ValueTuple<int, int, int>) ref _info).\u002Ector(0, -1, -1);
          return false;
        }
        int num1 = 0;
        foreach (Manager.Resources.FishBaitHitInfo.HitInfo hit in this.HitList)
          num1 += hit.Range;
        int num2 = Random.Range(0, num1);
        foreach (Manager.Resources.FishBaitHitInfo.HitInfo hit in this.HitList)
        {
          if (num2 < hit.Range)
          {
            if (!hit.FishList.IsNullOrEmpty<ValueTuple<int, int, int>>())
            {
              int num3 = 0;
              using (List<ValueTuple<int, int, int>>.Enumerator enumerator = hit.FishList.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  ValueTuple<int, int, int> current = enumerator.Current;
                  num3 += (int) current.Item1;
                }
              }
              int num4 = Random.Range(0, num3);
              using (List<ValueTuple<int, int, int>>.Enumerator enumerator = hit.FishList.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  ValueTuple<int, int, int> current = enumerator.Current;
                  if (num4 < current.Item1)
                  {
                    _info = current;
                    return true;
                  }
                  num4 -= (int) current.Item1;
                }
              }
            }
            else
            {
              ((ValueTuple<int, int, int>) ref _info).\u002Ector(0, -1, -1);
              return false;
            }
          }
          else
            num2 -= hit.Range;
        }
        ((ValueTuple<int, int, int>) ref _info).\u002Ector(0, -1, -1);
        return false;
      }

      public struct HitInfo
      {
        public int Range { get; set; }

        public List<ValueTuple<int, int, int>> FishList { get; set; }
      }
    }

    public class GameInfoTables
    {
      private Dictionary<int, Dictionary<int, ItemTableElement>> _itemPickTables = new Dictionary<int, Dictionary<int, ItemTableElement>>();
      private Dictionary<int, Dictionary<int, ItemTableElement>> _frogItemTable = new Dictionary<int, Dictionary<int, ItemTableElement>>();

      public bool initialized { get; private set; }

      private Dictionary<int, Dictionary<int, StuffItemInfo>> _itemTables { get; } = new Dictionary<int, Dictionary<int, StuffItemInfo>>();

      private IReadOnlyDictionary<int, StuffItemInfo> _itemNameHashTables { get; set; }

      private Dictionary<int, Dictionary<int, StuffItemInfo>> _systemItemTables { get; } = new Dictionary<int, Dictionary<int, StuffItemInfo>>();

      public int[] GetItemCategories()
      {
        return this._itemTables.Keys.ToArray<int>();
      }

      public Dictionary<int, StuffItemInfo> GetItemTable(int category)
      {
        Dictionary<int, StuffItemInfo> dictionary;
        return !this._itemTables.TryGetValue(category, out dictionary) ? (Dictionary<int, StuffItemInfo>) null : dictionary;
      }

      public StuffItemInfo FindItemInfo(int nameHash)
      {
        StuffItemInfo stuffItemInfo;
        return !this._itemNameHashTables.TryGetValue(nameHash, ref stuffItemInfo) ? (StuffItemInfo) null : stuffItemInfo;
      }

      public StuffItemInfo GetItem(int category, int id)
      {
        Dictionary<int, StuffItemInfo> itemTable = this.GetItemTable(category);
        if (itemTable == null)
          return (StuffItemInfo) null;
        StuffItemInfo stuffItemInfo;
        return !itemTable.TryGetValue(id, out stuffItemInfo) ? (StuffItemInfo) null : stuffItemInfo;
      }

      public StuffItemInfo GetItem_System(int category, int id)
      {
        Dictionary<int, StuffItemInfo> dictionary;
        if (!this._systemItemTables.TryGetValue(category, out dictionary))
          return (StuffItemInfo) null;
        StuffItemInfo stuffItemInfo;
        return !dictionary.TryGetValue(id, out stuffItemInfo) ? (StuffItemInfo) null : stuffItemInfo;
      }

      public Dictionary<int, Dictionary<int, Dictionary<int, FoodParameterPacket>>> FoodParameterTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, FoodParameterPacket>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, FoodParameterPacket>>> DrinkParameterTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, FoodParameterPacket>>>();

      public bool CanEat(StuffItem item)
      {
        Dictionary<int, Dictionary<int, FoodParameterPacket>> dictionary;
        if (this.FoodParameterTable.TryGetValue(item.CategoryID, out dictionary) && dictionary.ContainsKey(item.ID))
          return true;
        return this.DrinkParameterTable.TryGetValue(item.CategoryID, out dictionary) && dictionary.ContainsKey(item.ID);
      }

      public bool CanEat(StuffItemInfo item)
      {
        Dictionary<int, Dictionary<int, FoodParameterPacket>> dictionary;
        if (this.FoodParameterTable.TryGetValue(item.CategoryID, out dictionary) && dictionary.ContainsKey(item.ID))
          return true;
        return this.DrinkParameterTable.TryGetValue(item.CategoryID, out dictionary) && dictionary.ContainsKey(item.ID);
      }

      public Dictionary<int, Dictionary<int, EquipEventItemInfo>> SearchEquipEventItemTable { get; private set; } = new Dictionary<int, Dictionary<int, EquipEventItemInfo>>();

      public Dictionary<int, Dictionary<int, EquipEventItemInfo>> CommonEquipEventItemTable { get; private set; } = new Dictionary<int, Dictionary<int, EquipEventItemInfo>>();

      public Dictionary<int, Dictionary<int, AccessoryItemInfo>> AccessoryItem { get; private set; } = new Dictionary<int, Dictionary<int, AccessoryItemInfo>>();

      public Dictionary<int, Dictionary<int, RecyclingItemInfo>> RecyclingCreateableItemTable { get; private set; } = new Dictionary<int, Dictionary<int, RecyclingItemInfo>>();

      public List<RecyclingItemInfo> RecyclingCreateableItemList { get; private set; } = new List<RecyclingItemInfo>();

      public int[] GetAreaKeys()
      {
        return this._itemPickTables.Keys.ToArray<int>();
      }

      public Dictionary<int, ItemTableElement> GetItemTableInArea(
        int pointID)
      {
        if (this._itemPickTables == null)
          return (Dictionary<int, ItemTableElement>) null;
        Dictionary<int, ItemTableElement> dictionary;
        return !this._itemPickTables.TryGetValue(pointID, out dictionary) ? (Dictionary<int, ItemTableElement>) null : dictionary;
      }

      public Dictionary<int, ItemTableElement> GetItemTableInFrogPoint(
        int pointID)
      {
        if (this._frogItemTable.IsNullOrEmpty<int, Dictionary<int, ItemTableElement>>())
          return (Dictionary<int, ItemTableElement>) null;
        Dictionary<int, ItemTableElement> dictionary;
        return this._frogItemTable.TryGetValue(pointID, out dictionary) ? dictionary : (Dictionary<int, ItemTableElement>) null;
      }

      public Dictionary<int, Dictionary<int, List<VendItemInfo>>> VendItemInfoTable { get; } = new Dictionary<int, Dictionary<int, List<VendItemInfo>>>();

      public IReadOnlyDictionary<int, VendItemInfo> VendItemInfoSpecialTable { get; private set; }

      public Manager.Resources.GameInfoTables.RecipeInfo recipe { get; } = new Manager.Resources.GameInfoTables.RecipeInfo();

      public AIProject.SaveData.Environment.PlantInfo GetPlantInfo(int nameHash)
      {
        HarvestDataInfo harvestDataInfo;
        if (!this.HarvestDataInfoTable.TryGetValue(nameHash, ref harvestDataInfo) || harvestDataInfo == null)
          return (AIProject.SaveData.Environment.PlantInfo) null;
        StuffItem[] array = ((IEnumerable<HarvestData.Data>) (harvestDataInfo?.Get() ?? (IReadOnlyCollection<HarvestData.Data>) new HarvestData.Data[0])).Select<HarvestData.Data, StuffItem>((Func<HarvestData.Data, StuffItem>) (x =>
        {
          StuffItemInfo itemInfo = this.FindItemInfo(x.nameHash);
          int? categoryId = itemInfo?.CategoryID;
          int category = !categoryId.HasValue ? -1 : categoryId.Value;
          int? id1 = itemInfo?.ID;
          int id2 = !id1.HasValue ? -1 : id1.Value;
          int stock = x.stock;
          return new StuffItem(category, id2, stock);
        })).Where<StuffItem>((Func<StuffItem, bool>) (x => x.CategoryID != -1)).ToArray<StuffItem>();
        return new AIProject.SaveData.Environment.PlantInfo(harvestDataInfo.nameHash, harvestDataInfo.Time, array);
      }

      private IReadOnlyDictionary<int, HarvestDataInfo> HarvestDataInfoTable { get; set; }

      public Manager.Resources.GameInfoTables.AdvPresentItemInfo GetAdvPresentInfo(
        ChaControl chaControl)
      {
        ChaFileControl chaFile = chaControl.chaFile;
        Dictionary<int, int> flavorState = chaFile.gameinfo.flavorState;
        int personality = chaFile.parameter.personality;
        Dictionary<int, AgentAdvPresentInfo.Param> dictionary;
        if (!this.AgentAdvPresentInfoTable.TryGetValue(personality, ref dictionary))
        {
          Debug.LogError((object) string.Format("{0} not found : personality[{1}]", (object) nameof (GetAdvPresentInfo), (object) personality));
          if (!this.AgentAdvPresentInfoTable.TryGetValue(0, ref dictionary))
            return (Manager.Resources.GameInfoTables.AdvPresentItemInfo) null;
        }
        KeyValuePair<int, int> keyValuePair = flavorState.OrderByDescending<KeyValuePair<int, int>, int>((Func<KeyValuePair<int, int>, int>) (v => v.Value)).FirstOrDefault<KeyValuePair<int, int>>();
        AgentAdvPresentInfo.Param obj;
        if (!dictionary.TryGetValue(keyValuePair.Key, out obj))
        {
          Debug.LogError((object) string.Format("Parameter not found : personality[{0}]", (object) personality));
          return (Manager.Resources.GameInfoTables.AdvPresentItemInfo) null;
        }
        StuffItemInfo itemInfo = this.FindItemInfo(Illusion.Utils.ProbabilityCalclator.DetermineFromDict<AgentAdvPresentInfo.ItemData>(obj.ItemData.ToDictionary<AgentAdvPresentInfo.ItemData, AgentAdvPresentInfo.ItemData, int>((Func<AgentAdvPresentInfo.ItemData, AgentAdvPresentInfo.ItemData>) (v => v), (Func<AgentAdvPresentInfo.ItemData, int>) (v => 100))).nameHash);
        return itemInfo == null ? (Manager.Resources.GameInfoTables.AdvPresentItemInfo) null : new Manager.Resources.GameInfoTables.AdvPresentItemInfo(obj.ItemID, itemInfo);
      }

      private IReadOnlyDictionary<int, Dictionary<int, AgentAdvPresentInfo.Param>> AgentAdvPresentInfoTable { get; set; }

      public Manager.Resources.GameInfoTables.AdvPresentItemInfo GetAdvPresentBirthdayInfo(
        ChaControl chaControl)
      {
        int personality = chaControl.chaFile.parameter.personality;
        Dictionary<int, AgentAdvPresentBirthdayInfo.Param> source;
        if (!this.AgentAdvPresentBirthdayInfoTable.TryGetValue(personality, ref source))
        {
          Debug.LogError((object) string.Format("{0} not found : personality[{1}]", (object) nameof (GetAdvPresentBirthdayInfo), (object) personality));
          if (!this.AgentAdvPresentBirthdayInfoTable.TryGetValue(0, ref source))
            return (Manager.Resources.GameInfoTables.AdvPresentItemInfo) null;
        }
        if (!source.Any<KeyValuePair<int, AgentAdvPresentBirthdayInfo.Param>>())
        {
          Debug.LogError((object) string.Format("{0} [table empty] : personality[{1}]", (object) nameof (GetAdvPresentBirthdayInfo), (object) personality));
          return (Manager.Resources.GameInfoTables.AdvPresentItemInfo) null;
        }
        AgentAdvPresentBirthdayInfo.Param obj = source.Values.Shuffle<AgentAdvPresentBirthdayInfo.Param>().First<AgentAdvPresentBirthdayInfo.Param>();
        StuffItemInfo itemInfo = this.FindItemInfo(Illusion.Utils.ProbabilityCalclator.DetermineFromDict<AgentAdvPresentBirthdayInfo.ItemData>(obj.ItemData.ToDictionary<AgentAdvPresentBirthdayInfo.ItemData, AgentAdvPresentBirthdayInfo.ItemData, int>((Func<AgentAdvPresentBirthdayInfo.ItemData, AgentAdvPresentBirthdayInfo.ItemData>) (v => v), (Func<AgentAdvPresentBirthdayInfo.ItemData, int>) (v => 100))).nameHash);
        return itemInfo == null ? (Manager.Resources.GameInfoTables.AdvPresentItemInfo) null : new Manager.Resources.GameInfoTables.AdvPresentItemInfo(obj.ItemID, itemInfo);
      }

      private IReadOnlyDictionary<int, Dictionary<int, AgentAdvPresentBirthdayInfo.Param>> AgentAdvPresentBirthdayInfoTable { get; set; }

      public Tuple<StuffItemInfo, int> GetAdvScroungeInfo(ChaControl chaControl)
      {
        int personality = chaControl.chaFile.parameter.personality;
        Dictionary<int, AgentAdvScroungeInfo.Param> source;
        if (!this.AgentAdvScroungeInfoTable.TryGetValue(personality, ref source))
        {
          Debug.LogError((object) string.Format("{0} not found : personality[{1}]", (object) nameof (GetAdvScroungeInfo), (object) personality));
          if (!this.AgentAdvScroungeInfoTable.TryGetValue(0, ref source))
            return (Tuple<StuffItemInfo, int>) null;
        }
        if (!source.Any<KeyValuePair<int, AgentAdvScroungeInfo.Param>>())
        {
          Debug.LogError((object) string.Format("{0} [table empty] : personality[{1}]", (object) nameof (GetAdvScroungeInfo), (object) personality));
          return (Tuple<StuffItemInfo, int>) null;
        }
        AgentAdvScroungeInfo.ItemData fromDict = Illusion.Utils.ProbabilityCalclator.DetermineFromDict<AgentAdvScroungeInfo.ItemData>(source.Values.Shuffle<AgentAdvScroungeInfo.Param>().First<AgentAdvScroungeInfo.Param>().ItemData.ToDictionary<AgentAdvScroungeInfo.ItemData, AgentAdvScroungeInfo.ItemData, int>((Func<AgentAdvScroungeInfo.ItemData, AgentAdvScroungeInfo.ItemData>) (v => v), (Func<AgentAdvScroungeInfo.ItemData, int>) (v => 100)));
        return Tuple.Create<StuffItemInfo, int>(this.FindItemInfo(fromDict.nameHash), fromDict.sum);
      }

      private IReadOnlyDictionary<int, Dictionary<int, AgentAdvScroungeInfo.Param>> AgentAdvScroungeInfoTable { get; set; }

      public IReadOnlyDictionary<string, AgentAdvEventInfo.Param> GetAgentAdvEvents(
        AgentActor agent)
      {
        int personality = agent.ChaControl.fileParam.personality;
        Dictionary<string, AgentAdvEventInfo.Param> dictionary;
        if (this.AgentAdvEventInfoTable.TryGetValue(personality, ref dictionary))
          return (IReadOnlyDictionary<string, AgentAdvEventInfo.Param>) dictionary;
        Debug.LogError((object) string.Format("{0}:{1}[{2}]", (object) nameof (GetAgentAdvEvents), (object) "charaID", (object) personality));
        return (IReadOnlyDictionary<string, AgentAdvEventInfo.Param>) new Dictionary<string, AgentAdvEventInfo.Param>();
      }

      private IReadOnlyDictionary<int, Dictionary<string, AgentAdvEventInfo.Param>> AgentAdvEventInfoTable { get; set; }

      public IReadOnlyDictionary<int, LifeStyleData.Param> AgentLifeStyleInfoTable { get; private set; }

      public void Load(DefinePack definePack)
      {
        this.LoadItemList();
        this.LoadItemList_System();
        this.LoadFoodParameterTable(definePack);
        this.LoadDrinkParameterTable(definePack);
        this.LoadItemSearchTable(definePack);
        this.LoadFrogItemTable(definePack);
        this.recipe.Load();
        this.LoadSearchEquipItemList(definePack);
        this.LoadCommonEquipItemList(definePack);
        this.LoadAccessoryItem(definePack);
        this.LoadVendItemList();
        this.LoadSpecialVendItemList();
        this.LoadHarvest();
        this.LoadAgentAdvPresentInfoTable();
        this.LoadAgentAdvPresentBirthdayInfoTable();
        this.LoadAgentAdvScroungeInfoTable();
        this.LoadAgentAdvEventInfoTable();
        this.LoadAgentLifeStyleInfoTable();
        this.LoadRecyclingInfo(definePack);
        this.initialized = true;
      }

      public void Release()
      {
        this._itemTables.Clear();
        this._systemItemTables.Clear();
        this.FoodParameterTable.Clear();
        this.DrinkParameterTable.Clear();
        this._itemPickTables.Clear();
        this._frogItemTable.Clear();
        this.SearchEquipEventItemTable.Clear();
        this.CommonEquipEventItemTable.Clear();
        this.AccessoryItem.Clear();
        this.VendItemInfoTable.Clear();
        this.VendItemInfoSpecialTable = (IReadOnlyDictionary<int, VendItemInfo>) null;
        this.recipe.Release();
        this.HarvestDataInfoTable = (IReadOnlyDictionary<int, HarvestDataInfo>) null;
        this.AgentAdvPresentInfoTable = (IReadOnlyDictionary<int, Dictionary<int, AgentAdvPresentInfo.Param>>) null;
        this.AgentAdvPresentBirthdayInfoTable = (IReadOnlyDictionary<int, Dictionary<int, AgentAdvPresentBirthdayInfo.Param>>) null;
        this.AgentAdvScroungeInfoTable = (IReadOnlyDictionary<int, Dictionary<int, AgentAdvScroungeInfo.Param>>) null;
        this.AgentAdvEventInfoTable = (IReadOnlyDictionary<int, Dictionary<string, AgentAdvEventInfo.Param>>) null;
        this.RecyclingCreateableItemTable.Clear();
        this.RecyclingCreateableItemList.Clear();
        this.initialized = false;
      }

      private void LoadItemList()
      {
        Dictionary<int, Tuple<string, Sprite>> categoryIcon = Singleton<Manager.Resources>.Instance.itemIconTables.CategoryIcon;
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath("list/actor/gameitem/info/item/itemlist/", true);
        nameListFromPath.Sort();
        Dictionary<int, Dictionary<int, ItemData.Param>> dictionary1 = new Dictionary<int, Dictionary<int, ItemData.Param>>();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (ItemData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (ItemData), (string) null).GetAllAssets<ItemData>())
          {
            int result;
            if (!int.TryParse(((Object) allAsset).get_name(), out result))
              Debug.LogError((object) string.Format("ItemList Category Name:{0}", (object) ((Object) allAsset).get_name()));
            else if (!categoryIcon.ContainsKey(result))
            {
              Debug.LogError((object) string.Format("ItemList not Register Category:{0}", (object) result));
            }
            else
            {
              Dictionary<int, ItemData.Param> dictionary2;
              if (!dictionary1.TryGetValue(result, out dictionary2))
                dictionary1[result] = dictionary2 = new Dictionary<int, ItemData.Param>();
              foreach (ItemData.Param obj in allAsset.param)
                dictionary2[obj.ID] = obj;
            }
          }
          AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
        }
        foreach (KeyValuePair<int, Dictionary<int, ItemData.Param>> keyValuePair1 in dictionary1)
        {
          int key = keyValuePair1.Key;
          Dictionary<int, StuffItemInfo> dictionary2 = new Dictionary<int, StuffItemInfo>();
          foreach (KeyValuePair<int, ItemData.Param> keyValuePair2 in keyValuePair1.Value)
          {
            StuffItemInfo stuffItemInfo = new StuffItemInfo(key, keyValuePair2.Value, false)
            {
              ReactionType = -1,
              IsAvailableHeroine = false,
              EnnuiAddition = new ThresholdInt(0, 1),
              TasteAdditionNormal = new ThresholdInt(0, 1),
              TasteAdditionEnnui = new ThresholdInt(0, 1),
              EquipableState = ItemEquipableState.Impossible
            };
            dictionary2[keyValuePair2.Key] = stuffItemInfo;
          }
          this._itemTables[key] = dictionary2;
        }
        this._itemNameHashTables = (IReadOnlyDictionary<int, StuffItemInfo>) this._itemTables.Values.SelectMany<Dictionary<int, StuffItemInfo>, StuffItemInfo>((Func<Dictionary<int, StuffItemInfo>, IEnumerable<StuffItemInfo>>) (x => x.Select<KeyValuePair<int, StuffItemInfo>, StuffItemInfo>((Func<KeyValuePair<int, StuffItemInfo>, StuffItemInfo>) (y => y.Value)))).ToDictionary<StuffItemInfo, int, StuffItemInfo>((Func<StuffItemInfo, int>) (v => v.nameHash), (Func<StuffItemInfo, StuffItemInfo>) (v => v));
      }

      private void LoadItemList_System()
      {
        Dictionary<int, Tuple<string, Sprite>> categoryIcon = Singleton<Manager.Resources>.Instance.itemIconTables.CategoryIcon;
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath("list/actor/gameitem/info/item/itemlist_system/", true);
        nameListFromPath.Sort();
        Dictionary<int, Dictionary<int, ItemData_System.Param>> dictionary1 = new Dictionary<int, Dictionary<int, ItemData_System.Param>>();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (ItemData_System allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (ItemData_System), (string) null).GetAllAssets<ItemData_System>())
          {
            int result;
            if (!int.TryParse(((Object) allAsset).get_name(), out result))
            {
              Debug.LogError((object) string.Format("ItemList Category Name:{0}", (object) ((Object) allAsset).get_name()));
            }
            else
            {
              Dictionary<int, ItemData_System.Param> dictionary2;
              if (!dictionary1.TryGetValue(result, out dictionary2))
                dictionary1[result] = dictionary2 = new Dictionary<int, ItemData_System.Param>();
              foreach (ItemData_System.Param obj in allAsset.param)
                dictionary2[obj.ID] = obj;
            }
          }
          AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
        }
        foreach (KeyValuePair<int, Dictionary<int, ItemData_System.Param>> keyValuePair1 in dictionary1)
        {
          int key = keyValuePair1.Key;
          Dictionary<int, StuffItemInfo> dictionary2 = new Dictionary<int, StuffItemInfo>();
          foreach (KeyValuePair<int, ItemData_System.Param> keyValuePair2 in keyValuePair1.Value)
            dictionary2[keyValuePair2.Key] = ItemData_System.Convert(key, keyValuePair2.Value);
          this._systemItemTables[key] = dictionary2;
        }
      }

      private void LoadFoodParameterTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.FoodInfo, false);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (ExcelData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (ExcelData), (string) null).GetAllAssets<ExcelData>())
          {
            foreach (ExcelData.Param obj in allAsset.list)
            {
              int num1 = 1;
              List<string> list1 = obj.list;
              int index1 = num1;
              int num2 = index1 + 1;
              int result1;
              if (int.TryParse(list1.GetElement<string>(index1), out result1))
              {
                Dictionary<int, Dictionary<int, FoodParameterPacket>> dictionary1;
                if (!this.FoodParameterTable.TryGetValue(result1, out dictionary1))
                {
                  Dictionary<int, Dictionary<int, FoodParameterPacket>> dictionary2 = new Dictionary<int, Dictionary<int, FoodParameterPacket>>();
                  this.FoodParameterTable[result1] = dictionary2;
                  dictionary1 = dictionary2;
                }
                List<string> list2 = obj.list;
                int index2 = num2;
                int num3 = index2 + 1;
                int result2;
                if (int.TryParse(list2.GetElement<string>(index2), out result2))
                {
                  Dictionary<int, FoodParameterPacket> dictionary2;
                  if (!dictionary1.TryGetValue(result2, out dictionary2))
                  {
                    Dictionary<int, FoodParameterPacket> dictionary3 = new Dictionary<int, FoodParameterPacket>();
                    dictionary1[result2] = dictionary3;
                    dictionary2 = dictionary3;
                  }
                  List<string> list3 = obj.list;
                  int index3 = num3;
                  int num4 = index3 + 1;
                  int result3;
                  if (int.TryParse(list3.GetElement<string>(index3), out result3))
                  {
                    List<string> list4 = obj.list;
                    int index4 = num4;
                    int num5 = index4 + 1;
                    int result4;
                    int num6 = !int.TryParse(list4.GetElement<string>(index4), out result4) ? 0 : result4;
                    List<string> list5 = obj.list;
                    int index5 = num5;
                    int num7 = index5 + 1;
                    float result5;
                    float minValue1 = !float.TryParse(list5.GetElement<string>(index5), out result5) ? 0.0f : result5;
                    List<string> list6 = obj.list;
                    int index6 = num7;
                    int num8 = index6 + 1;
                    float maxValue1 = !float.TryParse(list6.GetElement<string>(index6), out result5) ? 0.0f : result5;
                    List<string> list7 = obj.list;
                    int index7 = num8;
                    int num9 = index7 + 1;
                    float minValue2 = !float.TryParse(list7.GetElement<string>(index7), out result5) ? 0.0f : result5;
                    List<string> list8 = obj.list;
                    int index8 = num9;
                    int num10 = index8 + 1;
                    float maxValue2 = !float.TryParse(list8.GetElement<string>(index8), out result5) ? 0.0f : result5;
                    List<string> list9 = obj.list;
                    int index9 = num10;
                    int num11 = index9 + 1;
                    int num12 = !int.TryParse(list9.GetElement<string>(index9), out result4) ? 0 : result4;
                    FoodParameterPacket foodParameterPacket1 = new FoodParameterPacket();
                    foodParameterPacket1.Probability = (float) num6;
                    foodParameterPacket1.SatiationAscentThreshold = new Threshold(minValue1, maxValue1);
                    foodParameterPacket1.SatiationDescentThreshold = new Threshold(minValue2, maxValue2);
                    foodParameterPacket1.StomachacheRate = (float) num12;
                    FoodParameterPacket foodParameterPacket2 = foodParameterPacket1;
                    while (num11 < obj.list.Count)
                    {
                      List<string> list10 = obj.list;
                      int index10 = num11;
                      int num13 = index10 + 1;
                      string element1 = list10.GetElement<string>(index10);
                      List<string> list11 = obj.list;
                      int index11 = num13;
                      int num14 = index11 + 1;
                      string element2 = list11.GetElement<string>(index11);
                      List<string> list12 = obj.list;
                      int index12 = num14;
                      int num15 = index12 + 1;
                      string element3 = list12.GetElement<string>(index12);
                      List<string> list13 = obj.list;
                      int index13 = num15;
                      num11 = index13 + 1;
                      string element4 = list13.GetElement<string>(index13);
                      if (!element1.IsNullOrEmpty())
                      {
                        int index14;
                        if (!Manager.Resources.StatusTagTable.TryGetValue(element1, out index14))
                        {
                          Debug.LogWarning((object) string.Format("タグ読み取りエラー: 値={0}", (object) element1));
                        }
                        else
                        {
                          int s = !int.TryParse(element2, out result4) ? 0 : result4;
                          int m = !int.TryParse(element3, out result4) ? 0 : result4;
                          int l = !int.TryParse(element4, out result4) ? 0 : result4;
                          foodParameterPacket2.Parameters[index14] = new TriThreshold(s, m, l);
                        }
                      }
                    }
                    dictionary2[result3] = foodParameterPacket2;
                  }
                }
              }
            }
          }
        }
      }

      private void LoadDrinkParameterTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.DrinkInfo, false);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (ExcelData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (ExcelData), (string) null).GetAllAssets<ExcelData>())
          {
            foreach (ExcelData.Param obj in allAsset.list)
            {
              int num1 = 1;
              List<string> list1 = obj.list;
              int index1 = num1;
              int num2 = index1 + 1;
              int result1;
              if (int.TryParse(list1.GetElement<string>(index1), out result1))
              {
                Dictionary<int, Dictionary<int, FoodParameterPacket>> dictionary1;
                if (!this.DrinkParameterTable.TryGetValue(result1, out dictionary1))
                {
                  Dictionary<int, Dictionary<int, FoodParameterPacket>> dictionary2 = new Dictionary<int, Dictionary<int, FoodParameterPacket>>();
                  this.DrinkParameterTable[result1] = dictionary2;
                  dictionary1 = dictionary2;
                }
                List<string> list2 = obj.list;
                int index2 = num2;
                int num3 = index2 + 1;
                int result2;
                if (int.TryParse(list2.GetElement<string>(index2), out result2))
                {
                  Dictionary<int, FoodParameterPacket> dictionary2;
                  if (!dictionary1.TryGetValue(result2, out dictionary2))
                  {
                    Dictionary<int, FoodParameterPacket> dictionary3 = new Dictionary<int, FoodParameterPacket>();
                    dictionary1[result2] = dictionary3;
                    dictionary2 = dictionary3;
                  }
                  List<string> list3 = obj.list;
                  int index3 = num3;
                  int num4 = index3 + 1;
                  int result3;
                  if (int.TryParse(list3.GetElement<string>(index3), out result3))
                  {
                    List<string> list4 = obj.list;
                    int index4 = num4;
                    int num5 = index4 + 1;
                    int result4;
                    int num6 = !int.TryParse(list4.GetElement<string>(index4), out result4) ? 0 : result4;
                    List<string> list5 = obj.list;
                    int index5 = num5;
                    int num7 = index5 + 1;
                    int num8 = !int.TryParse(list5.GetElement<string>(index5), out result4) ? 0 : result4;
                    FoodParameterPacket foodParameterPacket1 = new FoodParameterPacket();
                    foodParameterPacket1.Probability = (float) num6;
                    foodParameterPacket1.StomachacheRate = (float) num8;
                    FoodParameterPacket foodParameterPacket2 = foodParameterPacket1;
                    while (num7 < obj.list.Count)
                    {
                      List<string> list6 = obj.list;
                      int index6 = num7;
                      int num9 = index6 + 1;
                      string element1 = list6.GetElement<string>(index6);
                      List<string> list7 = obj.list;
                      int index7 = num9;
                      int num10 = index7 + 1;
                      string element2 = list7.GetElement<string>(index7);
                      List<string> list8 = obj.list;
                      int index8 = num10;
                      int num11 = index8 + 1;
                      string element3 = list8.GetElement<string>(index8);
                      List<string> list9 = obj.list;
                      int index9 = num11;
                      num7 = index9 + 1;
                      string element4 = list9.GetElement<string>(index9);
                      if (!element1.IsNullOrEmpty())
                      {
                        int index10;
                        if (!Manager.Resources.StatusTagTable.TryGetValue(element1, out index10))
                        {
                          Debug.LogWarning((object) string.Format("タグ読み取りエラー: 値={0}", (object) element1));
                        }
                        else
                        {
                          int s = !int.TryParse(element2, out result4) ? 0 : result4;
                          int m = !int.TryParse(element3, out result4) ? 0 : result4;
                          int l = !int.TryParse(element4, out result4) ? 0 : result4;
                          foodParameterPacket2.Parameters[index10] = new TriThreshold(s, m, l);
                        }
                      }
                    }
                    dictionary2[result3] = foodParameterPacket2;
                  }
                }
              }
            }
          }
        }
      }

      private void LoadItemSearchTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.GatheringTable, false);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (GatherItemData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (GatherItemData), (string) null).GetAllAssets<GatherItemData>())
          {
            int result;
            if (int.TryParse(((Object) allAsset).get_name(), out result))
            {
              Dictionary<int, ItemTableElement> dictionary1;
              if (!this._itemPickTables.TryGetValue(result, out dictionary1))
              {
                Dictionary<int, ItemTableElement> dictionary2 = new Dictionary<int, ItemTableElement>();
                this._itemPickTables[result] = dictionary2;
                dictionary1 = dictionary2;
              }
              foreach (GatherItemData.Param obj in allAsset.param)
              {
                ItemTableElement itemTableElement1 = new ItemTableElement()
                {
                  Rate = obj.Rate
                };
                dictionary1[obj.ID] = itemTableElement1;
                ItemTableElement itemTableElement2 = itemTableElement1;
                if (obj.CategoryID_0 > 0)
                {
                  string str = string.Empty;
                  Dictionary<int, StuffItemInfo> dictionary2;
                  StuffItemInfo stuffItemInfo;
                  if (this._itemTables.TryGetValue(obj.CategoryID_0, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_0, out stuffItemInfo))
                    str = stuffItemInfo.Name;
                  ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                  {
                    name = str,
                    prob = obj.Probability_0 / 100f,
                    categoryID = obj.CategoryID_0,
                    itemID = obj.ItemID_0,
                    minCount = obj.Min_0,
                    maxCount = obj.Max_0
                  };
                  itemTableElement2.Elements.Add(gatherElement);
                }
                if (obj.CategoryID_1 > 0)
                {
                  string str = string.Empty;
                  Dictionary<int, StuffItemInfo> dictionary2;
                  StuffItemInfo stuffItemInfo;
                  if (this._itemTables.TryGetValue(obj.CategoryID_1, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_1, out stuffItemInfo))
                    str = stuffItemInfo.Name;
                  ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                  {
                    name = str,
                    prob = obj.Probability_1 / 100f,
                    categoryID = obj.CategoryID_1,
                    itemID = obj.ItemID_1,
                    minCount = obj.Min_1,
                    maxCount = obj.Max_1
                  };
                  itemTableElement2.Elements.Add(gatherElement);
                }
                if (obj.CategoryID_2 > 0)
                {
                  string str = string.Empty;
                  Dictionary<int, StuffItemInfo> dictionary2;
                  StuffItemInfo stuffItemInfo;
                  if (this._itemTables.TryGetValue(obj.CategoryID_2, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_2, out stuffItemInfo))
                    str = stuffItemInfo.Name;
                  ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                  {
                    name = str,
                    prob = obj.Probability_2 / 100f,
                    categoryID = obj.CategoryID_2,
                    itemID = obj.ItemID_2,
                    minCount = obj.Min_2,
                    maxCount = obj.Max_2
                  };
                  itemTableElement2.Elements.Add(gatherElement);
                }
                if (obj.CategoryID_3 > 0)
                {
                  string str = string.Empty;
                  Dictionary<int, StuffItemInfo> dictionary2;
                  StuffItemInfo stuffItemInfo;
                  if (this._itemTables.TryGetValue(obj.CategoryID_3, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_3, out stuffItemInfo))
                    str = stuffItemInfo.Name;
                  ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                  {
                    name = str,
                    prob = obj.Probability_3 / 100f,
                    categoryID = obj.CategoryID_3,
                    itemID = obj.ItemID_3,
                    minCount = obj.Min_3,
                    maxCount = obj.Max_3
                  };
                  itemTableElement2.Elements.Add(gatherElement);
                }
                if (obj.CategoryID_4 > 0)
                {
                  string str = string.Empty;
                  Dictionary<int, StuffItemInfo> dictionary2;
                  StuffItemInfo stuffItemInfo;
                  if (this._itemTables.TryGetValue(obj.CategoryID_4, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_4, out stuffItemInfo))
                    str = stuffItemInfo.Name;
                  ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                  {
                    name = str,
                    prob = obj.Probability_4 / 100f,
                    categoryID = obj.CategoryID_4,
                    itemID = obj.ItemID_4,
                    minCount = obj.Min_4,
                    maxCount = obj.Max_4
                  };
                  itemTableElement2.Elements.Add(gatherElement);
                }
                if (obj.CategoryID_5 > 0)
                {
                  string str = string.Empty;
                  Dictionary<int, StuffItemInfo> dictionary2;
                  StuffItemInfo stuffItemInfo;
                  if (this._itemTables.TryGetValue(obj.CategoryID_5, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_5, out stuffItemInfo))
                    str = stuffItemInfo.Name;
                  ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                  {
                    name = str,
                    prob = obj.Probability_5 / 100f,
                    categoryID = obj.CategoryID_5,
                    itemID = obj.ItemID_5,
                    minCount = obj.Min_5,
                    maxCount = obj.Max_5
                  };
                  itemTableElement2.Elements.Add(gatherElement);
                }
                if (obj.CategoryID_6 > 0)
                {
                  string str = string.Empty;
                  Dictionary<int, StuffItemInfo> dictionary2;
                  StuffItemInfo stuffItemInfo;
                  if (this._itemTables.TryGetValue(obj.CategoryID_6, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_6, out stuffItemInfo))
                    str = stuffItemInfo.Name;
                  ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                  {
                    name = str,
                    prob = obj.Probability_6 / 100f,
                    categoryID = obj.CategoryID_6,
                    itemID = obj.ItemID_6,
                    minCount = obj.Min_6,
                    maxCount = obj.Max_6
                  };
                  itemTableElement2.Elements.Add(gatherElement);
                }
                if (obj.CategoryID_7 > 0)
                {
                  string str = string.Empty;
                  Dictionary<int, StuffItemInfo> dictionary2;
                  StuffItemInfo stuffItemInfo;
                  if (this._itemTables.TryGetValue(obj.CategoryID_7, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_7, out stuffItemInfo))
                    str = stuffItemInfo.Name;
                  ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                  {
                    name = str,
                    prob = obj.Probability_7 / 100f,
                    categoryID = obj.CategoryID_7,
                    itemID = obj.ItemID_7,
                    minCount = obj.Min_7,
                    maxCount = obj.Max_7
                  };
                  itemTableElement2.Elements.Add(gatherElement);
                }
                if (obj.CategoryID_8 > 0)
                {
                  string str = string.Empty;
                  Dictionary<int, StuffItemInfo> dictionary2;
                  StuffItemInfo stuffItemInfo;
                  if (this._itemTables.TryGetValue(obj.CategoryID_8, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_8, out stuffItemInfo))
                    str = stuffItemInfo.Name;
                  ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                  {
                    name = str,
                    prob = obj.Probability_8 / 100f,
                    categoryID = obj.CategoryID_8,
                    itemID = obj.ItemID_8,
                    minCount = obj.Min_8,
                    maxCount = obj.Max_8
                  };
                  itemTableElement2.Elements.Add(gatherElement);
                }
                if (obj.CategoryID_9 > 0)
                {
                  string str = string.Empty;
                  Dictionary<int, StuffItemInfo> dictionary2;
                  StuffItemInfo stuffItemInfo;
                  if (this._itemTables.TryGetValue(obj.CategoryID_9, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_9, out stuffItemInfo))
                    str = stuffItemInfo.Name;
                  ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                  {
                    name = str,
                    prob = obj.Probability_9 / 100f,
                    categoryID = obj.CategoryID_9,
                    itemID = obj.ItemID_9,
                    minCount = obj.Min_9,
                    maxCount = obj.Max_9
                  };
                  itemTableElement2.Elements.Add(gatherElement);
                }
              }
            }
          }
          AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
        }
      }

      private void LoadFrogItemTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.FrogItemTable, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        foreach (string str1 in nameListFromPath)
        {
          if (!str1.IsNullOrEmpty())
          {
            foreach (GatherItemData allAsset in AssetBundleManager.LoadAllAsset(str1, typeof (GatherItemData), (string) null).GetAllAssets<GatherItemData>())
            {
              int result;
              if (!Object.op_Equality((Object) allAsset, (Object) null) && int.TryParse(((Object) allAsset).get_name() ?? string.Empty, out result))
              {
                Dictionary<int, ItemTableElement> dictionary1;
                if (!this._frogItemTable.TryGetValue(result, out dictionary1))
                  this._frogItemTable[result] = dictionary1 = new Dictionary<int, ItemTableElement>();
                foreach (GatherItemData.Param obj in allAsset.param)
                {
                  ItemTableElement itemTableElement1 = new ItemTableElement()
                  {
                    Rate = obj.Rate
                  };
                  dictionary1[obj.ID] = itemTableElement1;
                  ItemTableElement itemTableElement2 = itemTableElement1;
                  if (0 < obj.CategoryID_0)
                  {
                    string str2 = string.Empty;
                    Dictionary<int, StuffItemInfo> dictionary2;
                    StuffItemInfo stuffItemInfo;
                    if (this._itemTables.TryGetValue(obj.CategoryID_0, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_0, out stuffItemInfo))
                      str2 = stuffItemInfo.Name;
                    ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                    {
                      name = str2,
                      prob = obj.Probability_0 / 100f,
                      categoryID = obj.CategoryID_0,
                      itemID = obj.ItemID_0,
                      minCount = obj.Min_0,
                      maxCount = obj.Max_0
                    };
                    itemTableElement2.Elements.Add(gatherElement);
                  }
                  if (0 < obj.CategoryID_1)
                  {
                    string str2 = string.Empty;
                    Dictionary<int, StuffItemInfo> dictionary2;
                    StuffItemInfo stuffItemInfo;
                    if (this._itemTables.TryGetValue(obj.CategoryID_1, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_1, out stuffItemInfo))
                      str2 = stuffItemInfo.Name;
                    ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                    {
                      name = str2,
                      prob = obj.Probability_1 / 100f,
                      categoryID = obj.CategoryID_1,
                      itemID = obj.ItemID_1,
                      minCount = obj.Min_1,
                      maxCount = obj.Max_1
                    };
                    itemTableElement2.Elements.Add(gatherElement);
                  }
                  if (0 < obj.CategoryID_2)
                  {
                    string str2 = string.Empty;
                    Dictionary<int, StuffItemInfo> dictionary2;
                    StuffItemInfo stuffItemInfo;
                    if (this._itemTables.TryGetValue(obj.CategoryID_2, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_2, out stuffItemInfo))
                      str2 = stuffItemInfo.Name;
                    ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                    {
                      name = str2,
                      prob = obj.Probability_2 / 100f,
                      categoryID = obj.CategoryID_2,
                      itemID = obj.ItemID_2,
                      minCount = obj.Min_2,
                      maxCount = obj.Max_2
                    };
                    itemTableElement2.Elements.Add(gatherElement);
                  }
                  if (0 < obj.CategoryID_3)
                  {
                    string str2 = string.Empty;
                    Dictionary<int, StuffItemInfo> dictionary2;
                    StuffItemInfo stuffItemInfo;
                    if (this._itemTables.TryGetValue(obj.CategoryID_3, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_3, out stuffItemInfo))
                      str2 = stuffItemInfo.Name;
                    ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                    {
                      name = str2,
                      prob = obj.Probability_3 / 100f,
                      categoryID = obj.CategoryID_3,
                      itemID = obj.ItemID_3,
                      minCount = obj.Min_3,
                      maxCount = obj.Max_3
                    };
                    itemTableElement2.Elements.Add(gatherElement);
                  }
                  if (0 < obj.CategoryID_4)
                  {
                    string str2 = string.Empty;
                    Dictionary<int, StuffItemInfo> dictionary2;
                    StuffItemInfo stuffItemInfo;
                    if (this._itemTables.TryGetValue(obj.CategoryID_4, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_4, out stuffItemInfo))
                      str2 = stuffItemInfo.Name;
                    ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                    {
                      name = str2,
                      prob = obj.Probability_4 / 100f,
                      categoryID = obj.CategoryID_4,
                      itemID = obj.ItemID_4,
                      minCount = obj.Min_4,
                      maxCount = obj.Max_4
                    };
                    itemTableElement2.Elements.Add(gatherElement);
                  }
                  if (0 < obj.CategoryID_5)
                  {
                    string str2 = string.Empty;
                    Dictionary<int, StuffItemInfo> dictionary2;
                    StuffItemInfo stuffItemInfo;
                    if (this._itemTables.TryGetValue(obj.CategoryID_5, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_5, out stuffItemInfo))
                      str2 = stuffItemInfo.Name;
                    ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                    {
                      name = str2,
                      prob = obj.Probability_5 / 100f,
                      categoryID = obj.CategoryID_5,
                      itemID = obj.ItemID_5,
                      minCount = obj.Min_5,
                      maxCount = obj.Max_5
                    };
                    itemTableElement2.Elements.Add(gatherElement);
                  }
                  if (0 < obj.CategoryID_6)
                  {
                    string str2 = string.Empty;
                    Dictionary<int, StuffItemInfo> dictionary2;
                    StuffItemInfo stuffItemInfo;
                    if (this._itemTables.TryGetValue(obj.CategoryID_6, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_6, out stuffItemInfo))
                      str2 = stuffItemInfo.Name;
                    ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                    {
                      name = str2,
                      prob = obj.Probability_6 / 100f,
                      categoryID = obj.CategoryID_6,
                      itemID = obj.ItemID_6,
                      minCount = obj.Min_6,
                      maxCount = obj.Max_6
                    };
                    itemTableElement2.Elements.Add(gatherElement);
                  }
                  if (0 < obj.CategoryID_7)
                  {
                    string str2 = string.Empty;
                    Dictionary<int, StuffItemInfo> dictionary2;
                    StuffItemInfo stuffItemInfo;
                    if (this._itemTables.TryGetValue(obj.CategoryID_7, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_7, out stuffItemInfo))
                      str2 = stuffItemInfo.Name;
                    ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                    {
                      name = str2,
                      prob = obj.Probability_7 / 100f,
                      categoryID = obj.CategoryID_7,
                      itemID = obj.ItemID_7,
                      minCount = obj.Min_7,
                      maxCount = obj.Max_7
                    };
                    itemTableElement2.Elements.Add(gatherElement);
                  }
                  if (0 < obj.CategoryID_8)
                  {
                    string str2 = string.Empty;
                    Dictionary<int, StuffItemInfo> dictionary2;
                    StuffItemInfo stuffItemInfo;
                    if (this._itemTables.TryGetValue(obj.CategoryID_8, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_8, out stuffItemInfo))
                      str2 = stuffItemInfo.Name;
                    ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                    {
                      name = str2,
                      prob = obj.Probability_8 / 100f,
                      categoryID = obj.CategoryID_8,
                      itemID = obj.ItemID_8,
                      minCount = obj.Min_8,
                      maxCount = obj.Max_8
                    };
                    itemTableElement2.Elements.Add(gatherElement);
                  }
                  if (0 < obj.CategoryID_9)
                  {
                    string str2 = string.Empty;
                    Dictionary<int, StuffItemInfo> dictionary2;
                    StuffItemInfo stuffItemInfo;
                    if (this._itemTables.TryGetValue(obj.CategoryID_9, out dictionary2) && dictionary2.TryGetValue(obj.ItemID_9, out stuffItemInfo))
                      str2 = stuffItemInfo.Name;
                    ItemTableElement.GatherElement gatherElement = new ItemTableElement.GatherElement()
                    {
                      name = str2,
                      prob = obj.Probability_9 / 100f,
                      categoryID = obj.CategoryID_9,
                      itemID = obj.ItemID_9,
                      minCount = obj.Min_9,
                      maxCount = obj.Max_9
                    };
                    itemTableElement2.Elements.Add(gatherElement);
                  }
                }
              }
            }
            AssetBundleManager.UnloadAssetBundle(str1, false, (string) null, false);
          }
        }
      }

      private void TryGet(ref bool success, List<string> row, ref int idx, out float value)
      {
        if (success)
        {
          ref bool local = ref success;
          int num1 = success ? 1 : 0;
          List<string> source = row;
          int num2;
          idx = (num2 = idx) + 1;
          int index = num2;
          int num3 = float.TryParse(source.GetElement<string>(index) ?? string.Empty, out value) ? 1 : 0;
          int num4 = num1 & num3;
          local = num4 != 0;
        }
        else
        {
          ++idx;
          value = 0.0f;
        }
      }

      private void TryGet(ref bool success, List<string> row, ref int idx, out int value)
      {
        if (success)
        {
          ref bool local = ref success;
          int num1 = success ? 1 : 0;
          List<string> source = row;
          int num2;
          idx = (num2 = idx) + 1;
          int index = num2;
          int num3 = int.TryParse(source.GetElement<string>(index) ?? string.Empty, out value) ? 1 : 0;
          int num4 = num1 & num3;
          local = num4 != 0;
        }
        else
        {
          ++idx;
          value = 0;
        }
      }

      private void LoadSearchEquipItemList(DefinePack definePack)
      {
        Dictionary<int, ActionItemInfo> eventItemList = Singleton<Manager.Resources>.Instance.Map.EventItemList;
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.SearchEquipItemObjList, false);
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            int num1 = 1;
            while (num1 < excelData.MaxCell)
            {
              ExcelData.Param obj = excelData.list[num1++];
              int num2 = 1;
              List<string> list1 = obj.list;
              int index2 = num2;
              int num3 = index2 + 1;
              int result1;
              if (int.TryParse(list1.GetElement<string>(index2), out result1))
              {
                List<string> list2 = obj.list;
                int index3 = num3;
                int num4 = index3 + 1;
                int result2;
                if (int.TryParse(list2.GetElement<string>(index3), out result2))
                {
                  List<string> list3 = obj.list;
                  int index4 = num4;
                  int num5 = index4 + 1;
                  int result3;
                  if (int.TryParse(list3.GetElement<string>(index4), out result3))
                  {
                    Dictionary<int, EquipEventItemInfo> dictionary1;
                    if (!this.SearchEquipEventItemTable.TryGetValue(result1, out dictionary1))
                    {
                      Dictionary<int, EquipEventItemInfo> dictionary2 = new Dictionary<int, EquipEventItemInfo>();
                      this.SearchEquipEventItemTable[result1] = dictionary2;
                      dictionary1 = dictionary2;
                    }
                    ActionItemInfo actionItemInfo;
                    if (eventItemList.TryGetValue(result3, out actionItemInfo))
                      dictionary1[result2] = new EquipEventItemInfo()
                      {
                        EventItemID = result3,
                        ActionItemInfo = actionItemInfo
                      };
                  }
                }
              }
            }
          }
        }
      }

      private void LoadCommonEquipItemList(DefinePack definePack)
      {
        Dictionary<int, ActionItemInfo> eventItemList = Singleton<Manager.Resources>.Instance.Map.EventItemList;
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.CommonEquipItemObjList, false);
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            int num1 = 1;
            while (num1 < excelData.MaxCell)
            {
              ExcelData.Param obj = excelData.list[num1++];
              int num2 = 1;
              List<string> list1 = obj.list;
              int index2 = num2;
              int num3 = index2 + 1;
              int result1;
              if (int.TryParse(list1.GetElement<string>(index2), out result1))
              {
                List<string> list2 = obj.list;
                int index3 = num3;
                int num4 = index3 + 1;
                int result2;
                if (int.TryParse(list2.GetElement<string>(index3), out result2))
                {
                  List<string> list3 = obj.list;
                  int index4 = num4;
                  int num5 = index4 + 1;
                  int result3;
                  if (int.TryParse(list3.GetElement<string>(index4), out result3))
                  {
                    Dictionary<int, EquipEventItemInfo> dictionary1;
                    if (!this.CommonEquipEventItemTable.TryGetValue(result1, out dictionary1))
                    {
                      Dictionary<int, EquipEventItemInfo> dictionary2 = new Dictionary<int, EquipEventItemInfo>();
                      this.CommonEquipEventItemTable[result1] = dictionary2;
                      dictionary1 = dictionary2;
                    }
                    ActionItemInfo actionItemInfo;
                    if (eventItemList.TryGetValue(result3, out actionItemInfo))
                      dictionary1[result2] = new EquipEventItemInfo()
                      {
                        EventItemID = result2,
                        ActionItemInfo = actionItemInfo
                      };
                  }
                }
              }
            }
          }
        }
      }

      private void LoadAccessoryItem(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.AccessoryItem, false);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (GameItemAccessoryData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (GameItemAccessoryData), (string) null).GetAllAssets<GameItemAccessoryData>())
          {
            int result;
            if (!int.TryParse(((Object) allAsset).get_name(), out result))
            {
              Debug.LogError((object) string.Format("Failed Load: AccessoryItem Category {0}", (object) ((Object) allAsset).get_name()));
            }
            else
            {
              Dictionary<int, AccessoryItemInfo> dictionary1;
              if (!this.AccessoryItem.TryGetValue(result, out dictionary1))
              {
                Dictionary<int, AccessoryItemInfo> dictionary2 = new Dictionary<int, AccessoryItemInfo>();
                this.AccessoryItem[result] = dictionary2;
                dictionary1 = dictionary2;
              }
              foreach (GameItemAccessoryData.Param obj in allAsset.param)
              {
                if (!obj.AssetBundle.IsNullOrEmpty() && !obj.Asset.IsNullOrEmpty() && !obj.ParentName.IsNullOrEmpty())
                  dictionary1[obj.ID] = new AccessoryItemInfo()
                  {
                    Name = obj.Name,
                    AssetBundle = obj.AssetBundle,
                    Asset = obj.Asset,
                    Manifest = obj.Manifest,
                    ParentName = obj.ParentName,
                    Weight = obj.Weight
                  };
              }
            }
          }
        }
      }

      private void LoadVendItemList()
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath("list/actor/merchant/vend/normal/", true);
        nameListFromPath.Sort();
        Dictionary<int, List<VendData.Param>> dictionary1 = new Dictionary<int, List<VendData.Param>>();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (VendData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (VendData), (string) null).GetAllAssets<VendData>())
          {
            int result;
            if (!int.TryParse(((Object) allAsset).get_name(), out result))
              Debug.LogError((object) string.Format("VendData Stage Name:{0}", (object) ((Object) allAsset).get_name()));
            else
              dictionary1[result] = allAsset.param;
          }
          AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
        }
        foreach (KeyValuePair<int, List<VendData.Param>> keyValuePair in dictionary1)
        {
          Dictionary<int, List<VendItemInfo>> dictionary2 = new Dictionary<int, List<VendItemInfo>>();
          foreach (IGrouping<int, VendData.Param> source in keyValuePair.Value.GroupBy<VendData.Param, int>((Func<VendData.Param, int>) (p => p.Group)))
            dictionary2[source.Key] = source.Select<VendData.Param, VendItemInfo>((Func<VendData.Param, VendItemInfo>) (data =>
            {
              StuffItemInfo itemInfo = this.FindItemInfo(data.nameHash);
              if (itemInfo != null)
                return new VendItemInfo(itemInfo, data);
              Debug.LogError((object) string.Format("VendData Item Name:{0}", (object) data.nameHash));
              return (VendItemInfo) null;
            })).Where<VendItemInfo>((Func<VendItemInfo, bool>) (p => p != null)).ToList<VendItemInfo>();
          this.VendItemInfoTable[keyValuePair.Key] = dictionary2;
        }
      }

      private void LoadSpecialVendItemList()
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath("list/actor/merchant/vend/special/", true);
        nameListFromPath.Sort();
        Dictionary<int, VendItemInfo> dictionary = new Dictionary<int, VendItemInfo>();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (VendSpecialData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (VendSpecialData), (string) null).GetAllAssets<VendSpecialData>())
          {
            foreach (VendSpecialData.Param obj in allAsset.param)
            {
              StuffItemInfo itemInfo = this.FindItemInfo(obj.nameHash);
              if (itemInfo == null)
                Debug.LogError((object) string.Format("VendSpecialData Item Name:{0}", (object) obj.nameHash));
              else
                dictionary[obj.ID] = new VendItemInfo(itemInfo, obj);
            }
          }
          AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
        }
        this.VendItemInfoSpecialTable = (IReadOnlyDictionary<int, VendItemInfo>) dictionary;
      }

      private void LoadHarvest()
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath("list/actor/gameitem/harvest/", true);
        nameListFromPath.Sort();
        List<HarvestData.Param> source = new List<HarvestData.Param>();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (HarvestData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (HarvestData), (string) null).GetAllAssets<HarvestData>())
            source.AddRange((IEnumerable<HarvestData.Param>) allAsset.param);
          AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
        }
        this.HarvestDataInfoTable = (IReadOnlyDictionary<int, HarvestDataInfo>) source.Select<HarvestData.Param, HarvestDataInfo>((Func<HarvestData.Param, HarvestDataInfo>) (item => new HarvestDataInfo(item))).Where<HarvestDataInfo>((Func<HarvestDataInfo, bool>) (p => p.table.Any<KeyValuePair<int, List<HarvestData.Data>>>() && this.FindItemInfo(p.nameHash) != null)).ToDictionary<HarvestDataInfo, int, HarvestDataInfo>((Func<HarvestDataInfo, int>) (v => v.nameHash), (Func<HarvestDataInfo, HarvestDataInfo>) (v => v));
      }

      private void LoadAgentAdvPresentInfoTable()
      {
        Dictionary<int, Dictionary<int, AgentAdvPresentInfo.Param>> dictionary1 = new Dictionary<int, Dictionary<int, AgentAdvPresentInfo.Param>>();
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath("list/actor/agent/present/", true);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (AgentAdvPresentInfo allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (AgentAdvPresentInfo), (string) null).GetAllAssets<AgentAdvPresentInfo>())
          {
            int result;
            if (!int.TryParse(((Object) allAsset).get_name(), out result))
            {
              Debug.LogError((object) string.Format("{0}:{1}", (object) nameof (LoadAgentAdvPresentInfoTable), (object) ((Object) allAsset).get_name()));
            }
            else
            {
              Dictionary<int, AgentAdvPresentInfo.Param> dictionary2;
              if (!dictionary1.TryGetValue(result, out dictionary2))
                dictionary1[result] = dictionary2 = new Dictionary<int, AgentAdvPresentInfo.Param>();
              foreach (AgentAdvPresentInfo.Param obj in allAsset.param)
                dictionary2[obj.ID] = obj;
            }
          }
          AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
        }
        foreach (Dictionary<int, AgentAdvPresentInfo.Param> dictionary2 in dictionary1.Values)
        {
          foreach (int index in dictionary2.Keys.ToArray<int>())
          {
            AgentAdvPresentInfo.Param obj = new AgentAdvPresentInfo.Param(dictionary2[index]);
            foreach (AgentAdvPresentInfo.ItemData itemData in obj.ItemData.Where<AgentAdvPresentInfo.ItemData>((Func<AgentAdvPresentInfo.ItemData, bool>) (x => this.FindItemInfo(x.nameHash) == null)))
              obj.ItemData.Remove(itemData);
            dictionary2[index] = obj;
          }
        }
        this.AgentAdvPresentInfoTable = (IReadOnlyDictionary<int, Dictionary<int, AgentAdvPresentInfo.Param>>) dictionary1;
      }

      private void LoadAgentAdvPresentBirthdayInfoTable()
      {
        Dictionary<int, Dictionary<int, AgentAdvPresentBirthdayInfo.Param>> dictionary1 = new Dictionary<int, Dictionary<int, AgentAdvPresentBirthdayInfo.Param>>();
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath("list/actor/agent/present_birthday/", true);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (AgentAdvPresentBirthdayInfo allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (AgentAdvPresentBirthdayInfo), (string) null).GetAllAssets<AgentAdvPresentBirthdayInfo>())
          {
            int result;
            if (!int.TryParse(((Object) allAsset).get_name(), out result))
            {
              Debug.LogError((object) string.Format("{0}:{1}", (object) nameof (LoadAgentAdvPresentBirthdayInfoTable), (object) ((Object) allAsset).get_name()));
            }
            else
            {
              Dictionary<int, AgentAdvPresentBirthdayInfo.Param> dictionary2;
              if (!dictionary1.TryGetValue(result, out dictionary2))
                dictionary1[result] = dictionary2 = new Dictionary<int, AgentAdvPresentBirthdayInfo.Param>();
              foreach (AgentAdvPresentBirthdayInfo.Param obj in allAsset.param)
                dictionary2[obj.ID] = obj;
            }
          }
          AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
        }
        foreach (Dictionary<int, AgentAdvPresentBirthdayInfo.Param> dictionary2 in dictionary1.Values)
        {
          foreach (int index in dictionary2.Keys.ToArray<int>())
          {
            AgentAdvPresentBirthdayInfo.Param obj = new AgentAdvPresentBirthdayInfo.Param(dictionary2[index]);
            foreach (AgentAdvPresentBirthdayInfo.ItemData itemData in obj.ItemData.Where<AgentAdvPresentBirthdayInfo.ItemData>((Func<AgentAdvPresentBirthdayInfo.ItemData, bool>) (x => this.FindItemInfo(x.nameHash) == null)))
              obj.ItemData.Remove(itemData);
            dictionary2[index] = obj;
          }
        }
        this.AgentAdvPresentBirthdayInfoTable = (IReadOnlyDictionary<int, Dictionary<int, AgentAdvPresentBirthdayInfo.Param>>) dictionary1;
      }

      private void LoadAgentAdvScroungeInfoTable()
      {
        Dictionary<int, Dictionary<int, AgentAdvScroungeInfo.Param>> dictionary1 = new Dictionary<int, Dictionary<int, AgentAdvScroungeInfo.Param>>();
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath("list/actor/agent/scrounge/", true);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (AgentAdvScroungeInfo allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (AgentAdvScroungeInfo), (string) null).GetAllAssets<AgentAdvScroungeInfo>())
          {
            int result;
            if (!int.TryParse(((Object) allAsset).get_name(), out result))
            {
              Debug.LogError((object) string.Format("{0}:{1}", (object) nameof (LoadAgentAdvScroungeInfoTable), (object) ((Object) allAsset).get_name()));
            }
            else
            {
              Dictionary<int, AgentAdvScroungeInfo.Param> dictionary2;
              if (!dictionary1.TryGetValue(result, out dictionary2))
                dictionary1[result] = dictionary2 = new Dictionary<int, AgentAdvScroungeInfo.Param>();
              foreach (AgentAdvScroungeInfo.Param obj in allAsset.param)
                dictionary2[obj.ID] = obj;
            }
          }
          AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
        }
        foreach (Dictionary<int, AgentAdvScroungeInfo.Param> dictionary2 in dictionary1.Values)
        {
          foreach (int index in dictionary2.Keys.ToArray<int>())
          {
            AgentAdvScroungeInfo.Param obj = new AgentAdvScroungeInfo.Param(dictionary2[index]);
            foreach (AgentAdvScroungeInfo.ItemData itemData in obj.ItemData.Where<AgentAdvScroungeInfo.ItemData>((Func<AgentAdvScroungeInfo.ItemData, bool>) (x => this.FindItemInfo(x.nameHash) == null)))
              obj.ItemData.Remove(itemData);
            dictionary2[index] = obj;
          }
        }
        this.AgentAdvScroungeInfoTable = (IReadOnlyDictionary<int, Dictionary<int, AgentAdvScroungeInfo.Param>>) dictionary1;
      }

      private void LoadAgentAdvEventInfoTable()
      {
        Dictionary<int, Dictionary<string, AgentAdvEventInfo.Param>> dictionary1 = new Dictionary<int, Dictionary<string, AgentAdvEventInfo.Param>>();
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath("list/actor/agent/event/", true);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (AgentAdvEventInfo allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (AgentAdvEventInfo), (string) null).GetAllAssets<AgentAdvEventInfo>())
          {
            int result;
            if (!int.TryParse(((Object) allAsset).get_name(), out result))
            {
              Debug.LogError((object) string.Format("{0}:{1}", (object) nameof (LoadAgentAdvEventInfoTable), (object) ((Object) allAsset).get_name()));
            }
            else
            {
              Dictionary<string, AgentAdvEventInfo.Param> dictionary2;
              if (!dictionary1.TryGetValue(result, out dictionary2))
                dictionary1[result] = dictionary2 = new Dictionary<string, AgentAdvEventInfo.Param>();
              foreach (AgentAdvEventInfo.Param obj in allAsset.param)
                dictionary2[obj.FileName] = obj;
            }
          }
          AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
        }
        this.AgentAdvEventInfoTable = (IReadOnlyDictionary<int, Dictionary<string, AgentAdvEventInfo.Param>>) dictionary1;
      }

      private void LoadAgentLifeStyleInfoTable()
      {
        Dictionary<int, LifeStyleData.Param> dictionary = new Dictionary<int, LifeStyleData.Param>();
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath("list/actor/agent/lifestyle/", true);
        nameListFromPath.Sort();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (LifeStyleData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (LifeStyleData), (string) null).GetAllAssets<LifeStyleData>())
          {
            foreach (LifeStyleData.Param obj in allAsset.param)
              dictionary[obj.ID] = obj;
          }
          AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
        }
        this.AgentLifeStyleInfoTable = (IReadOnlyDictionary<int, LifeStyleData.Param>) dictionary;
      }

      private void LoadRecyclingInfo(DefinePack definePack)
      {
        if (Object.op_Equality((Object) definePack, (Object) null))
          return;
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.RecyclingInfoList, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string element1 = nameListFromPath.GetElement<string>(index1);
          if (!element1.IsNullOrEmpty())
          {
            string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(element1);
            if (!withoutExtension.IsNullOrEmpty() && AssetBundleCheck.IsFile(element1, withoutExtension))
            {
              ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(element1, withoutExtension, string.Empty);
              if (!Object.op_Equality((Object) excelData, (Object) null) && excelData.MaxCell > 0)
              {
                for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
                {
                  List<string> list = excelData.list[index2].list;
                  if (!list.IsNullOrEmpty<string>())
                  {
                    int num1 = 0;
                    List<string> source1 = list;
                    int index3 = num1;
                    int num2 = index3 + 1;
                    int result;
                    if (int.TryParse(source1.GetElement<string>(index3) ?? string.Empty, out result))
                    {
                      AssetBundleInfo assetBundleInfo = (AssetBundleInfo) null;
                      ref AssetBundleInfo local1 = ref assetBundleInfo;
                      List<string> source2 = list;
                      int index4 = num2;
                      int num3 = index4 + 1;
                      string element2 = source2.GetElement<string>(index4);
                      local1.name = (__Null) element2;
                      ref AssetBundleInfo local2 = ref assetBundleInfo;
                      List<string> source3 = list;
                      int index5 = num3;
                      int num4 = index5 + 1;
                      string element3 = source3.GetElement<string>(index5);
                      local2.assetbundle = (__Null) element3;
                      ref AssetBundleInfo local3 = ref assetBundleInfo;
                      List<string> source4 = list;
                      int index6 = num4;
                      int num5 = index6 + 1;
                      string element4 = source4.GetElement<string>(index6);
                      local3.asset = (__Null) element4;
                      ref AssetBundleInfo local4 = ref assetBundleInfo;
                      List<string> source5 = list;
                      int index7 = num5;
                      int num6 = index7 + 1;
                      string element5 = source5.GetElement<string>(index7);
                      local4.manifest = (__Null) element5;
                      AssetBundleInfo listABInfo = assetBundleInfo;
                      if (!((string) listABInfo.asset).IsNullOrEmpty() && !((string) listABInfo.assetbundle).IsNullOrEmpty() && result == 0)
                        this.LoadRecyclingCreateableItemInfoList(listABInfo);
                    }
                  }
                }
              }
            }
          }
        }
        if (this.RecyclingCreateableItemTable.IsNullOrEmpty<int, Dictionary<int, RecyclingItemInfo>>())
          return;
        List<ValueTuple<int, int>> valueTupleList = ListPool<ValueTuple<int, int>>.Get();
        List<int> toRelease1 = ListPool<int>.Get();
        toRelease1.AddRange((IEnumerable<int>) this.RecyclingCreateableItemTable.Keys);
        for (int index1 = 0; index1 < toRelease1.Count; ++index1)
        {
          int key1 = toRelease1[index1];
          if (this.RecyclingCreateableItemTable.ContainsKey(key1))
          {
            Dictionary<int, RecyclingItemInfo> source = this.RecyclingCreateableItemTable[key1];
            if (!source.IsNullOrEmpty<int, RecyclingItemInfo>())
            {
              List<int> toRelease2 = ListPool<int>.Get();
              toRelease2.AddRange((IEnumerable<int>) source.Keys);
              for (int index2 = 0; index2 < toRelease2.Count; ++index2)
              {
                int key2 = toRelease2[index2];
                if (source.ContainsKey(key2))
                {
                  RecyclingItemInfo info = source[key2];
                  info.ItemInfo = this.GetItem(info.CategoryID, info.ItemID);
                  if (info.ItemInfo == null)
                  {
                    if (!valueTupleList.Exists((Predicate<ValueTuple<int, int>>) (x => x.Item1 == info.CategoryID && x.Item2 == info.ItemID)))
                      valueTupleList.Add(new ValueTuple<int, int>(info.CategoryID, info.ItemID));
                  }
                  else if (info.Adult && !Game.isAdd01 && !valueTupleList.Exists((Predicate<ValueTuple<int, int>>) (x => x.Item1 == info.CategoryID && x.Item2 == info.ItemID)))
                    valueTupleList.Add(new ValueTuple<int, int>(info.CategoryID, info.ItemID));
                  source[key2] = info;
                }
              }
              ListPool<int>.Release(toRelease2);
            }
          }
        }
        ListPool<int>.Release(toRelease1);
        if (!valueTupleList.IsNullOrEmpty<ValueTuple<int, int>>())
        {
          using (List<ValueTuple<int, int>>.Enumerator enumerator = valueTupleList.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              ValueTuple<int, int> current = enumerator.Current;
              Dictionary<int, RecyclingItemInfo> source;
              if (this.RecyclingCreateableItemTable.TryGetValue((int) current.Item1, out source))
              {
                if (source.IsNullOrEmpty<int, RecyclingItemInfo>())
                  this.RecyclingCreateableItemTable.Remove((int) current.Item1);
                else if (source.ContainsKey((int) current.Item2))
                {
                  source.Remove((int) current.Item2);
                  if (source.IsNullOrEmpty<int, RecyclingItemInfo>())
                    this.RecyclingCreateableItemTable.Remove((int) current.Item1);
                }
              }
            }
          }
        }
        ListPool<ValueTuple<int, int>>.Release(valueTupleList);
        if (this.RecyclingCreateableItemTable.IsNullOrEmpty<int, Dictionary<int, RecyclingItemInfo>>())
          return;
        foreach (KeyValuePair<int, Dictionary<int, RecyclingItemInfo>> keyValuePair1 in this.RecyclingCreateableItemTable)
        {
          if (!keyValuePair1.Value.IsNullOrEmpty<int, RecyclingItemInfo>())
          {
            foreach (KeyValuePair<int, RecyclingItemInfo> keyValuePair2 in keyValuePair1.Value)
              this.RecyclingCreateableItemList.Add(keyValuePair2.Value);
          }
        }
        this.RecyclingCreateableItemList.Sort();
      }

      private void LoadRecyclingCreateableItemInfoList(AssetBundleInfo listABInfo)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(listABInfo);
        if (Object.op_Equality((Object) excelData, (Object) null) || excelData.MaxCell <= 0)
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                int result3;
                if (int.TryParse(source3.GetElement<string>(index4) ?? string.Empty, out result3))
                {
                  List<string> source4 = list;
                  int index5 = num4;
                  int num5 = index5 + 1;
                  bool result4;
                  if (bool.TryParse(source4.GetElement<string>(index5) ?? string.Empty, out result4))
                  {
                    List<string> source5 = list;
                    int index6 = num5;
                    int num6 = index6 + 1;
                    bool result5;
                    Dictionary<int, RecyclingItemInfo> source6;
                    if (bool.TryParse(source5.GetElement<string>(index6) ?? string.Empty, out result5) && !result5 && (this.RecyclingCreateableItemTable.TryGetValue(result1, out source6) && !source6.IsNullOrEmpty<int, RecyclingItemInfo>()) && source6.ContainsKey(result2))
                    {
                      source6.Remove(result2);
                      if (source6.Count <= 0)
                        this.RecyclingCreateableItemTable.Remove(result1);
                    }
                    Dictionary<int, RecyclingItemInfo> dictionary1;
                    if (!this.RecyclingCreateableItemTable.TryGetValue(result1, out dictionary1) || dictionary1 == null)
                    {
                      Dictionary<int, RecyclingItemInfo> dictionary2 = new Dictionary<int, RecyclingItemInfo>();
                      this.RecyclingCreateableItemTable[result1] = dictionary2;
                      dictionary1 = dictionary2;
                    }
                    RecyclingItemInfo recyclingItemInfo1;
                    if (!dictionary1.TryGetValue(result2, out recyclingItemInfo1))
                    {
                      RecyclingItemInfo recyclingItemInfo2 = new RecyclingItemInfo();
                      dictionary1[result2] = recyclingItemInfo2;
                      recyclingItemInfo1 = recyclingItemInfo2;
                    }
                    recyclingItemInfo1.CategoryID = result1;
                    recyclingItemInfo1.ItemID = result2;
                    recyclingItemInfo1.IconID = result3;
                    recyclingItemInfo1.Adult = result4;
                    if (recyclingItemInfo1.ItemNameList == null)
                      recyclingItemInfo1.ItemNameList = new List<string>();
                    recyclingItemInfo1.ItemNameList.Clear();
                    while (num6 < list.Count)
                      recyclingItemInfo1.ItemNameList.Add(list.GetElement<string>(num6++) ?? string.Empty);
                    dictionary1[result2] = recyclingItemInfo1;
                  }
                }
              }
            }
          }
        }
      }

      public class RecipeInfo
      {
        private const string LOAD_PATH = "list/actor/gameitem/recipe/";

        public bool initialized { get; private set; }

        public IReadOnlyDictionary<int, RecipeDataInfo[]> this[
          int i]
        {
          get
          {
            switch (i)
            {
              case 0:
                return this.materialTable;
              case 1:
                return this.equipmentTable;
              case 2:
                return this.cookTable;
              case 3:
                return this.petTable;
              case 4:
                return this.medicineTable;
              default:
                Debug.LogError((object) ("RecipeDataInfoTable Not Range:" + (object) i));
                return this.materialTable;
            }
          }
        }

        public IReadOnlyDictionary<int, RecipeDataInfo[]> materialTable { get; private set; }

        public IReadOnlyDictionary<int, RecipeDataInfo[]> equipmentTable { get; private set; }

        public IReadOnlyDictionary<int, RecipeDataInfo[]> cookTable { get; private set; }

        public IReadOnlyDictionary<int, RecipeDataInfo[]> petTable { get; private set; }

        public IReadOnlyDictionary<int, RecipeDataInfo[]> medicineTable { get; private set; }

        public void Load()
        {
          this.Load("material", (System.Action<Dictionary<int, RecipeDataInfo[]>>) (data => this.materialTable = (IReadOnlyDictionary<int, RecipeDataInfo[]>) data));
          this.Load("equipment", (System.Action<Dictionary<int, RecipeDataInfo[]>>) (data => this.equipmentTable = (IReadOnlyDictionary<int, RecipeDataInfo[]>) data));
          this.Load("cook", (System.Action<Dictionary<int, RecipeDataInfo[]>>) (data => this.cookTable = (IReadOnlyDictionary<int, RecipeDataInfo[]>) data));
          this.Load("pet", (System.Action<Dictionary<int, RecipeDataInfo[]>>) (data => this.petTable = (IReadOnlyDictionary<int, RecipeDataInfo[]>) data));
          this.Load("medicine", (System.Action<Dictionary<int, RecipeDataInfo[]>>) (data => this.medicineTable = (IReadOnlyDictionary<int, RecipeDataInfo[]>) data));
          this.initialized = true;
        }

        private void Load(string name, System.Action<Dictionary<int, RecipeDataInfo[]>> action)
        {
          List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath("list/actor/gameitem/recipe/" + string.Format("{0}/", (object) name), true);
          nameListFromPath.Sort();
          List<RecipeData.Param> source = new List<RecipeData.Param>();
          foreach (string assetBundleName in nameListFromPath)
          {
            foreach (RecipeData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (RecipeData), (string) null).GetAllAssets<RecipeData>())
              source.AddRange((IEnumerable<RecipeData.Param>) allAsset.param);
            AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
          }
          ILookup<int, RecipeDataInfo> lookup = source.Select<RecipeData.Param, RecipeDataInfo>((Func<RecipeData.Param, RecipeDataInfo>) (item => new RecipeDataInfo(item))).Where<RecipeDataInfo>((Func<RecipeDataInfo, bool>) (p => ((IEnumerable<RecipeDataInfo.NeedData>) p.NeedList).Any<RecipeDataInfo.NeedData>())).ToLookup<RecipeDataInfo, int, RecipeDataInfo>((Func<RecipeDataInfo, int>) (v => v.nameHash), (Func<RecipeDataInfo, RecipeDataInfo>) (v => v));
          Func<IGrouping<int, RecipeDataInfo>, int> keySelector = (Func<IGrouping<int, RecipeDataInfo>, int>) (v => v.Key);
          // ISSUE: reference to a compiler-generated field
          if (Manager.Resources.GameInfoTables.RecipeInfo.\u003C\u003Ef__mg\u0024cache0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Manager.Resources.GameInfoTables.RecipeInfo.\u003C\u003Ef__mg\u0024cache0 = new Func<IGrouping<int, RecipeDataInfo>, RecipeDataInfo[]>(Enumerable.ToArray<RecipeDataInfo>);
          }
          // ISSUE: reference to a compiler-generated field
          Func<IGrouping<int, RecipeDataInfo>, RecipeDataInfo[]> fMgCache0 = Manager.Resources.GameInfoTables.RecipeInfo.\u003C\u003Ef__mg\u0024cache0;
          Dictionary<int, RecipeDataInfo[]> dictionary = lookup.ToDictionary<IGrouping<int, RecipeDataInfo>, int, RecipeDataInfo[]>(keySelector, fMgCache0);
          action(dictionary);
        }

        public void Release()
        {
          this.materialTable = (IReadOnlyDictionary<int, RecipeDataInfo[]>) null;
          this.equipmentTable = (IReadOnlyDictionary<int, RecipeDataInfo[]>) null;
          this.cookTable = (IReadOnlyDictionary<int, RecipeDataInfo[]>) null;
          this.petTable = (IReadOnlyDictionary<int, RecipeDataInfo[]>) null;
          this.medicineTable = (IReadOnlyDictionary<int, RecipeDataInfo[]>) null;
          this.initialized = false;
        }
      }

      public class AdvPresentItemInfo
      {
        public AdvPresentItemInfo(int eventItemID, StuffItemInfo itemInfo)
        {
          this.eventItemID = eventItemID;
          this.itemInfo = itemInfo;
        }

        public int eventItemID { get; }

        public StuffItemInfo itemInfo { get; }
      }
    }

    public class HSceneTables
    {
      private List<string> row = new List<string>();
      private List<string> pathList = new List<string>();
      public List<HScene.AnimationListInfo>[] lstAnimInfo = new List<HScene.AnimationListInfo>[6];
      public List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>> lstStartAnimInfo = new List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>>();
      public List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>> lstStartAnimInfoM = new List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>>();
      public List<StartWaitAnim> startWaitAnims = new List<StartWaitAnim>();
      public List<HScene.EndMotion> lstEndAnimInfo = new List<HScene.EndMotion>();
      public Dictionary<int, List<string>> lstHitObject = new Dictionary<int, List<string>>();
      public Dictionary<string, List<string>> HitObjAtariName = new Dictionary<string, List<string>>();
      public Dictionary<string, List<HitObjectCtrl.CollisionInfo>> DicLstHitObjInfo = new Dictionary<string, List<HitObjectCtrl.CollisionInfo>>();
      public Dictionary<int, Dictionary<int, Dictionary<string, GameObject>>> DicHitObject = new Dictionary<int, Dictionary<int, Dictionary<string, GameObject>>>();
      public List<Dictionary<int, List<HItemCtrl.ListItem>>>[] lstHItemObjInfo = new List<Dictionary<int, List<HItemCtrl.ListItem>>>[6];
      public List<ValueTuple<string, RuntimeAnimatorController>> lstHItemBase = new List<ValueTuple<string, RuntimeAnimatorController>>();
      private List<string> HitemPathList = new List<string>();
      public Dictionary<int, HPointList> hPointLists = new Dictionary<int, HPointList>();
      public Dictionary<int, HPointList.LoadInfo> hPointListInfos = new Dictionary<int, HPointList.LoadInfo>();
      public Dictionary<int, HPoint.HpointData> loadHPointDatas = new Dictionary<int, HPoint.HpointData>();
      public Dictionary<int, List<ValueTuple<string, string>>> AutoHpointData = new Dictionary<int, List<ValueTuple<string, string>>>();
      public Dictionary<int, float> autoLeavePersonalityRate = new Dictionary<int, float>();
      public Dictionary<int, float> autoLeaveAttributeRate = new Dictionary<int, float>();
      public List<HParticleCtrl.ParticleInfo> lstHParticleCtrl = new List<HParticleCtrl.ParticleInfo>();
      public RuntimeAnimatorController[,] HBaseRuntimeAnimatorControllers = new RuntimeAnimatorController[2, 3];
      private Dictionary<int, Dictionary<int, Manager.Resources.HSceneTables.HmeshInfo>> HmeshDictionary = new Dictionary<int, Dictionary<int, Manager.Resources.HSceneTables.HmeshInfo>>();
      private Manager.Resources.HSceneTables.HmeshInfo hMeshInfo = new Manager.Resources.HSceneTables.HmeshInfo();
      public Dictionary<int, GameObject> HMeshObjDic = new Dictionary<int, GameObject>();
      public Dictionary<int, Dictionary<int, List<YureCtrl.Info>>> DicDicYure = new Dictionary<int, Dictionary<int, List<YureCtrl.Info>>>();
      public Dictionary<int, Dictionary<int, List<YureCtrlMale.Info>>> DicDicYureMale = new Dictionary<int, Dictionary<int, List<YureCtrlMale.Info>>>();
      public Dictionary<int, List<FeelHit.FeelInfo>> DicLstHitInfo = new Dictionary<int, List<FeelHit.FeelInfo>>();
      public Dictionary<string, List<H_Lookat_dan.MotionLookAtList>> DicLstLookAtDan = new Dictionary<string, List<H_Lookat_dan.MotionLookAtList>>();
      public Dictionary<string, List<CollisionCtrl.CollisionInfo>> DicLstCollisionInfo = new Dictionary<string, List<CollisionCtrl.CollisionInfo>>();
      public Dictionary<string, Dictionary<string, HLayerCtrl.HLayerInfo>> LayerInfos = new Dictionary<string, Dictionary<string, HLayerCtrl.HLayerInfo>>();
      public Dictionary<int, Dictionary<string, GameObject>> HPointPrefabs = new Dictionary<int, Dictionary<string, GameObject>>();
      private StringBuilder sbAssetName = new StringBuilder();
      private StringBuilder sbAbName = new StringBuilder();
      private string hscenePrefabPath = "h/scene/";
      private readonly string[] assetNames = new string[6]
      {
        "aibu",
        "houshi",
        "sonyu",
        "tokushu",
        "les",
        "3P_F2M1"
      };
      private readonly string[,] strAssetAnimatorBase = new string[2, 3]
      {
        {
          "animator/h/male/01/aibu.unity3d",
          "animator/h/male/01/houshi.unity3d",
          "animator/h/male/01/sonyu.unity3d"
        },
        {
          "animator/h/female/01/aibu.unity3d",
          "animator/h/female/01/houshi.unity3d",
          "animator/h/female/01/sonyu.unity3d"
        }
      };
      private readonly string[,] racBaseNames = new string[2, 3]
      {
        {
          "aia_m_base",
          "aih_m_base",
          "ais_m_base"
        },
        {
          "aia_f_base",
          "aih_f_base",
          "ais_f_base"
        }
      };
      public HashSet<Manager.Resources.HSceneTables.HAssetBundle>[] hashUseAssetBundle = new HashSet<Manager.Resources.HSceneTables.HAssetBundle>[2]
      {
        new HashSet<Manager.Resources.HSceneTables.HAssetBundle>(),
        new HashSet<Manager.Resources.HSceneTables.HAssetBundle>()
      };
      private ExcelData excelData;
      private GameObject commonSpace;
      private RuntimeAnimatorController tmpHItemRuntimeAnimator;
      public Dictionary<int, AutoHPointData> autoHPointDatas;
      public GameObject HPointObj;
      public List<string> HAutoPathList;
      public HAutoCtrl.HAutoInfo HAutoInfo;
      public HAutoCtrl.AutoLeaveItToYou HAutoLeaveItToYou;
      public string[,] aHsceneBGM;
      public HParticleCtrl hParticle;
      public bool endHLoad;
      public GameObject HSceneSet;
      public GameObject HSceneUISet;
      public GameObject AutoHPointPrefabs;

      public Dictionary<int, ParameterPacket> HBaseParamTable { get; private set; } = new Dictionary<int, ParameterPacket>();

      public Dictionary<int, ParameterPacket> HactionParamTable { get; private set; } = new Dictionary<int, ParameterPacket>();

      public Dictionary<int, Dictionary<int, float>> HSkileParamTable { get; private set; } = new Dictionary<int, Dictionary<int, float>>();

      public static Dictionary<string, int> HTagTable { get; } = new Dictionary<string, int>()
      {
        ["体温"] = 0,
        ["機嫌"] = 1,
        ["満腹"] = 2,
        ["体調"] = 3,
        ["生命"] = 4,
        ["やる気"] = 5,
        ["H"] = 6,
        ["善悪"] = 7,
        ["女子力"] = 10,
        ["信頼"] = 11,
        ["人間性"] = 12,
        ["本能"] = 13,
        ["変態"] = 14,
        ["警戒"] = 15,
        ["闇"] = 16,
        ["社交"] = 17,
        ["トイレ"] = 100,
        ["風呂"] = 101,
        ["睡眠"] = 102,
        ["食事"] = 103,
        ["休憩"] = 104,
        ["ギフト"] = 105,
        ["おねだり"] = 106,
        ["寂しい"] = 107,
        ["H欲"] = 108,
        ["採取"] = 110,
        ["遊び"] = 111,
        ["料理"] = 112,
        ["動物"] = 113,
        ["ロケ"] = 114,
        ["飲み物"] = 115,
        ["ゲージ"] = -1
      };

      [DebuggerHidden]
      public IEnumerator LoadH()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadH\u003Ec__Iterator0()
        {
          \u0024this = this
        };
      }

      public void Release()
      {
        for (int index = 0; index < this.lstAnimInfo.Length; ++index)
          this.lstAnimInfo[index] = (List<HScene.AnimationListInfo>) null;
        this.lstStartAnimInfo.Clear();
        this.lstStartAnimInfoM.Clear();
        this.startWaitAnims.Clear();
        this.lstEndAnimInfo.Clear();
        this.lstHitObject.Clear();
        this.HitObjAtariName.Clear();
        this.DicLstHitObjInfo.Clear();
        using (Dictionary<int, Dictionary<int, Dictionary<string, GameObject>>>.Enumerator enumerator1 = this.DicHitObject.GetEnumerator())
        {
          while (enumerator1.MoveNext())
          {
            using (Dictionary<int, Dictionary<string, GameObject>>.Enumerator enumerator2 = enumerator1.Current.Value.GetEnumerator())
            {
              while (enumerator2.MoveNext())
              {
                using (Dictionary<string, GameObject>.Enumerator enumerator3 = enumerator2.Current.Value.GetEnumerator())
                {
                  while (enumerator3.MoveNext())
                    Object.Destroy((Object) enumerator3.Current.Value);
                }
              }
            }
          }
        }
        this.DicHitObject.Clear();
        for (int index = 0; index < this.lstHItemObjInfo.Length; ++index)
          this.lstHItemObjInfo[index] = (List<Dictionary<int, List<HItemCtrl.ListItem>>>) null;
        this.lstHItemBase.Clear();
        this.HitemPathList.Clear();
        if (Object.op_Inequality((Object) this.HSceneSet, (Object) null))
        {
          HScene componentInChildren = (HScene) this.HSceneSet.GetComponentInChildren<HScene>(true);
          if (Object.op_Inequality((Object) componentInChildren, (Object) null))
            componentInChildren.HParticleSetNull();
        }
        this.hPointLists.Clear();
        using (Dictionary<int, Dictionary<string, GameObject>>.ValueCollection.Enumerator enumerator1 = this.HPointPrefabs.Values.GetEnumerator())
        {
          while (enumerator1.MoveNext())
          {
            Dictionary<string, GameObject> current = enumerator1.Current;
            using (Dictionary<string, GameObject>.ValueCollection.Enumerator enumerator2 = current.Values.GetEnumerator())
            {
              while (enumerator2.MoveNext())
                Object.Destroy((Object) enumerator2.Current);
            }
            current.Clear();
          }
        }
        this.HPointPrefabs.Clear();
        Object.Destroy((Object) this.AutoHPointPrefabs);
        this.AutoHPointPrefabs = (GameObject) null;
        this.autoHPointDatas = (Dictionary<int, AutoHPointData>) null;
        this.loadHPointDatas.Clear();
        this.AutoHpointData.Clear();
        if (Object.op_Inequality((Object) this.HPointObj, (Object) null))
          Object.Destroy((Object) this.HPointObj);
        this.HPointObj = (GameObject) null;
        this.HAutoPathList.Clear();
        this.HAutoInfo = (HAutoCtrl.HAutoInfo) null;
        this.HAutoLeaveItToYou = (HAutoCtrl.AutoLeaveItToYou) null;
        this.autoLeavePersonalityRate.Clear();
        this.autoLeaveAttributeRate.Clear();
        this.aHsceneBGM = (string[,]) null;
        this.lstHParticleCtrl.Clear();
        this.hParticle.ReleaseObject();
        this.hParticle = (HParticleCtrl) null;
        for (int index1 = 0; index1 < this.HBaseRuntimeAnimatorControllers.GetLength(0); ++index1)
        {
          for (int index2 = 0; index2 < this.HBaseRuntimeAnimatorControllers.GetLength(1); ++index2)
            this.HBaseRuntimeAnimatorControllers[index1, index2] = (RuntimeAnimatorController) null;
        }
        this.HmeshDictionary.Clear();
        this.HMeshObjDic.Clear();
        this.DicDicYure.Clear();
        this.DicDicYureMale.Clear();
        this.DicLstHitInfo.Clear();
        this.DicLstLookAtDan.Clear();
        this.DicLstCollisionInfo.Clear();
        this.LayerInfos.Clear();
        this.HBaseParamTable.Clear();
        this.HactionParamTable.Clear();
        this.HSkileParamTable.Clear();
        this.endHLoad = false;
      }

      [DebuggerHidden]
      public IEnumerator LoadHObj()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadHObj\u003Ec__Iterator1()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      private IEnumerator LoadAnimationFileName()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadAnimationFileName\u003Ec__Iterator2()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      private IEnumerator LoadStartAnimationList(bool merchant = false)
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadStartAnimationList\u003Ec__Iterator3()
        {
          merchant = merchant,
          \u0024this = this
        };
      }

      [DebuggerHidden]
      private IEnumerator LoadEndAnimationList()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadEndAnimationList\u003Ec__Iterator4()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      private IEnumerator LoadHItemObjInfo()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadHItemObjInfo\u003Ec__Iterator5()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      private IEnumerator LoadHItemBaseAnim()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadHItemBaseAnim\u003Ec__Iterator6()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      private IEnumerator LoadHPointList()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadHPointList\u003Ec__Iterator7()
        {
          \u0024this = this
        };
      }

      public void LoadHpointPrefabs(int mapID)
      {
        HPointList.LoadInfo loadInfo;
        if (this.hPointListInfos.Count == 0 || !this.hPointListInfos.TryGetValue(mapID, out loadInfo))
          return;
        if (!this.hPointLists.ContainsKey(mapID))
        {
          if (this.HPointPrefabs == null)
            this.HPointPrefabs = new Dictionary<int, Dictionary<string, GameObject>>();
          if (!this.HPointPrefabs.ContainsKey(mapID))
            this.HPointPrefabs.Add(mapID, new Dictionary<string, GameObject>());
          if (!this.HPointPrefabs[mapID].ContainsKey(loadInfo.Path))
            this.HPointPrefabs[mapID].Add(loadInfo.Name, (GameObject) null);
          this.HPointPrefabs[mapID][loadInfo.Name] = CommonLib.LoadAsset<GameObject>(loadInfo.Path, loadInfo.Name, true, loadInfo.Manifest);
          if (Object.op_Equality((Object) this.HPointPrefabs[mapID][loadInfo.Name], (Object) null))
            return;
          GameObject gameObject = this.HPointPrefabs[mapID][loadInfo.Name];
          Transform transform = !Object.op_Equality((Object) this.commonSpace, (Object) null) ? this.commonSpace.get_transform() : GameObject.Find("CommonSpace").get_transform();
          gameObject.get_transform().SetParent(transform, false);
          gameObject.get_transform().set_localPosition(Vector3.get_zero());
          gameObject.get_transform().set_localRotation(Quaternion.get_identity());
          this.hPointLists.Add(mapID, (HPointList) null);
          HPointList component = (HPointList) gameObject.GetComponent<HPointList>();
          if (Object.op_Inequality((Object) component, (Object) null))
          {
            component.Init();
            if (Object.op_Equality((Object) this.hPointLists[mapID], (Object) null))
            {
              this.hPointLists[mapID] = component;
            }
            else
            {
              if (this.hPointLists[mapID].lst == null)
                this.hPointLists[mapID].lst = new Dictionary<int, List<HPoint>>();
              foreach (KeyValuePair<int, List<HPoint>> keyValuePair in component.lst)
              {
                if (!this.hPointLists[mapID].lst.ContainsKey(keyValuePair.Key))
                  this.hPointLists[mapID].lst.Add(keyValuePair.Key, keyValuePair.Value);
                else
                  this.hPointLists[mapID].lst[keyValuePair.Key] = keyValuePair.Value;
              }
            }
          }
        }
        AssetBundleManager.UnloadAssetBundle(loadInfo.Path, false, loadInfo.Manifest, false);
      }

      [DebuggerHidden]
      private IEnumerator LoadHPointInfo()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadHPointInfo\u003Ec__Iterator8()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      public IEnumerator LoadAutoHPointPath()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadAutoHPointPath\u003Ec__Iterator9()
        {
          \u0024this = this
        };
      }

      public void LoadAutoHPoint(int mapID)
      {
        this.pathList.Clear();
        this.pathList = CommonLib.GetAssetBundleNameListFromPath("list/h/hpoint/prefab/", false);
        this.autoHPointDatas = new Dictionary<int, AutoHPointData>();
        GameObject gameObject1 = (GameObject) null;
        for (int index = 0; index < this.pathList.Count; ++index)
        {
          this.sbAbName.Clear();
          this.sbAbName.Append(this.pathList[index]);
          if (GlobalMethod.AssetFileExist(this.sbAbName.ToString(), "Hpointobj", string.Empty))
          {
            if (Object.op_Inequality((Object) this.HPointObj, (Object) null))
            {
              Object.Destroy((Object) this.HPointObj);
              this.HPointObj = (GameObject) null;
            }
            gameObject1 = CommonLib.LoadAsset<GameObject>(this.sbAbName.ToString(), "Hpointobj", false, string.Empty);
            this.hashUseAssetBundle[1].Add(new Manager.Resources.HSceneTables.HAssetBundle(this.sbAbName.ToString(), string.Empty));
          }
        }
        Transform transform = !Object.op_Equality((Object) this.commonSpace, (Object) null) ? this.commonSpace.get_transform() : GameObject.Find("CommonSpace").get_transform();
        if (Object.op_Equality((Object) this.HPointObj, (Object) null) && Object.op_Inequality((Object) gameObject1, (Object) null))
        {
          this.HPointObj = (GameObject) Object.Instantiate<GameObject>((M0) gameObject1, transform);
          ((HPoint) this.HPointObj.GetComponent<HPoint>()).Init();
        }
        if (Object.op_Equality((Object) gameObject1, (Object) null) || !this.AutoHpointData.ContainsKey(mapID))
          return;
        if (Object.op_Equality((Object) this.AutoHPointPrefabs, (Object) null))
          this.AutoHPointPrefabs = new GameObject("AutoHpoints");
        GameObject autoHpointPrefabs = this.AutoHPointPrefabs;
        autoHpointPrefabs.get_transform().SetParent(transform, false);
        autoHpointPrefabs.get_transform().set_localPosition(Vector3.get_zero());
        autoHpointPrefabs.get_transform().set_localRotation(Quaternion.get_identity());
        for (int index = 0; index < this.AutoHpointData[mapID].Count; ++index)
        {
          if (GlobalMethod.AssetFileExist((string) this.AutoHpointData[mapID][index].Item1, (string) this.AutoHpointData[mapID][index].Item2, string.Empty))
          {
            AutoHPointData autoHpointData = CommonLib.LoadAsset<AutoHPointData>((string) this.AutoHpointData[mapID][index].Item1, (string) this.AutoHpointData[mapID][index].Item2, false, string.Empty);
            this.hashUseAssetBundle[0].Add(new Manager.Resources.HSceneTables.HAssetBundle((string) this.AutoHpointData[mapID][index].Item1, string.Empty));
            if (!Object.op_Equality((Object) autoHpointData, (Object) null))
            {
              if (!this.autoHPointDatas.ContainsKey(mapID))
                this.autoHPointDatas.Add(mapID, autoHpointData);
              else
                this.autoHPointDatas[mapID] = autoHpointData;
              if (autoHpointData.Points != null)
              {
                using (Dictionary<string, List<ValueTuple<int, Vector3>>>.Enumerator enumerator1 = autoHpointData.Points.GetEnumerator())
                {
                  while (enumerator1.MoveNext())
                  {
                    KeyValuePair<string, List<ValueTuple<int, Vector3>>> current1 = enumerator1.Current;
                    int num = this.AutoHpointID(current1.Key);
                    if (num >= 0)
                    {
                      using (List<ValueTuple<int, Vector3>>.Enumerator enumerator2 = current1.Value.GetEnumerator())
                      {
                        while (enumerator2.MoveNext())
                        {
                          ValueTuple<int, Vector3> current2 = enumerator2.Current;
                          GameObject gameObject2 = (GameObject) Object.Instantiate<GameObject>((M0) gameObject1);
                          gameObject2.get_transform().SetParent(autoHpointPrefabs.get_transform(), false);
                          gameObject2.get_transform().set_position((Vector3) current2.Item2);
                          gameObject2.get_transform().set_rotation(Quaternion.get_identity());
                          ((HPoint) gameObject2.GetComponent<HPoint>()).id = num;
                          if (!this.hPointLists.ContainsKey(mapID))
                          {
                            this.hPointLists.Add(mapID, (HPointList) new GameObject("HPointLists").AddComponent<HPointList>());
                            this.hPointLists[mapID].lst = new Dictionary<int, List<HPoint>>();
                          }
                          if (!this.hPointLists[mapID].lst.ContainsKey((int) current2.Item1))
                            this.hPointLists[mapID].lst.Add((int) current2.Item1, new List<HPoint>());
                          this.hPointLists[mapID].lst[(int) current2.Item1].Add((HPoint) gameObject2.GetComponent<HPoint>());
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void HPointInitData(int mapID)
      {
        foreach (KeyValuePair<int, List<HPoint>> keyValuePair in this.hPointLists[mapID].lst)
        {
          foreach (HPoint hpoint in keyValuePair.Value)
            hpoint.Init();
        }
      }

      [DebuggerHidden]
      private IEnumerator LoadHsceneParticle()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadHsceneParticle\u003Ec__IteratorA()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      private IEnumerator LoadHsceneBaseRAC()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadHsceneBaseRAC\u003Ec__IteratorB()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      private IEnumerator LoadHAutoInfo()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadHAutoInfo\u003Ec__IteratorC()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      private IEnumerator LoadAutoLeaveItToYou()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadAutoLeaveItToYou\u003Ec__IteratorD()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      private IEnumerator LoadAutoLeaveItToYouPersonality()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadAutoLeaveItToYouPersonality\u003Ec__IteratorE()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      private IEnumerator LoadAutoLeaveItToYouAttribute()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadAutoLeaveItToYouAttribute\u003Ec__IteratorF()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      private IEnumerator LoadHmesh()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadHmesh\u003Ec__Iterator10()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      private IEnumerator LoadStartWaitAnim()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadStartWaitAnim\u003Ec__Iterator11()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      public IEnumerator LoadHYure()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadHYure\u003Ec__Iterator12()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      public IEnumerator LoadHYureMale()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadHYureMale\u003Ec__Iterator13()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      private IEnumerator LoadHScene()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadHScene\u003Ec__Iterator14()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      private IEnumerator LoadFeelHit()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadFeelHit\u003Ec__Iterator15()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      private IEnumerator LoadDankonList()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.HSceneTables.\u003CLoadDankonList\u003Ec__Iterator16()
        {
          \u0024this = this
        };
      }

      public void LoadHParamTable(int mode)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(Singleton<HSceneManager>.Instance.strAssetParam, false);
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str1 = nameListFromPath[index1];
          string str2 = string.Empty;
          switch (mode)
          {
            case 0:
              str2 = "base_" + System.IO.Path.GetFileNameWithoutExtension(str1);
              break;
            case 1:
              str2 = "hresult_" + System.IO.Path.GetFileNameWithoutExtension(str1);
              break;
          }
          if (GlobalMethod.AssetFileExist(str1, str2, string.Empty))
          {
            ExcelData excelData = CommonLib.LoadAsset<ExcelData>(str1, str2, false, string.Empty);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              Singleton<Manager.Resources>.Instance.AddLoadAssetBundle(str1, string.Empty);
              int num1 = 1;
              while (num1 < excelData.MaxCell)
              {
                ExcelData.Param obj = excelData.list[num1++];
                int num2 = 1;
                List<string> list1 = obj.list;
                int index2 = num2;
                int num3 = index2 + 1;
                int result1;
                if (int.TryParse(list1.GetElement<string>(index2), out result1))
                {
                  ParameterPacket parameterPacket = new ParameterPacket();
                  while (num3 < obj.list.Count)
                  {
                    List<string> list2 = obj.list;
                    int index3 = num3;
                    int num4 = index3 + 1;
                    string element1 = list2.GetElement<string>(index3);
                    List<string> list3 = obj.list;
                    int index4 = num4;
                    int num5 = index4 + 1;
                    string element2 = list3.GetElement<string>(index4);
                    List<string> list4 = obj.list;
                    int index5 = num5;
                    int num6 = index5 + 1;
                    string element3 = list4.GetElement<string>(index5);
                    List<string> list5 = obj.list;
                    int index6 = num6;
                    num3 = index6 + 1;
                    string element4 = list5.GetElement<string>(index6);
                    if (!element1.IsNullOrEmpty())
                    {
                      int index7;
                      if (!Manager.Resources.HSceneTables.HTagTable.TryGetValue(element1, out index7))
                      {
                        Debug.LogWarning((object) string.Format("タグ読み取りエラー: 値={0}", (object) element1));
                      }
                      else
                      {
                        int result2;
                        int s = !int.TryParse(element2, out result2) ? 0 : result2;
                        int m = !int.TryParse(element3, out result2) ? 0 : result2;
                        int l = !int.TryParse(element4, out result2) ? 0 : result2;
                        parameterPacket.Parameters[index7] = new TriThreshold(s, m, l);
                      }
                    }
                  }
                  switch (mode)
                  {
                    case 0:
                      this.HBaseParamTable[result1] = parameterPacket;
                      continue;
                    case 1:
                      this.HactionParamTable[result1] = parameterPacket;
                      continue;
                    default:
                      continue;
                  }
                }
              }
            }
          }
        }
      }

      public void LoadHSkilParm()
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(Singleton<HSceneManager>.Instance.strAssetParam, false);
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str1 = nameListFromPath[index1];
          string str2 = "hskil_" + System.IO.Path.GetFileNameWithoutExtension(str1);
          if (GlobalMethod.AssetFileExist(str1, str2, string.Empty))
          {
            ExcelData excelData = CommonLib.LoadAsset<ExcelData>(str1, str2, false, string.Empty);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              Singleton<Manager.Resources>.Instance.AddLoadAssetBundle(str1, string.Empty);
              int num1 = 1;
              while (num1 < excelData.MaxCell)
              {
                ExcelData.Param obj = excelData.list[num1++];
                int num2 = 0;
                List<string> list1 = obj.list;
                int index2 = num2;
                int num3 = index2 + 1;
                int result1;
                if (int.TryParse(list1.GetElement<string>(index2), out result1))
                {
                  int num4 = num3 + 1;
                  Dictionary<int, float> dictionary = new Dictionary<int, float>();
                  while (num4 < obj.list.Count)
                  {
                    List<string> list2 = obj.list;
                    int index3 = num4;
                    int num5 = index3 + 1;
                    string element1 = list2.GetElement<string>(index3);
                    List<string> list3 = obj.list;
                    int index4 = num5;
                    num4 = index4 + 1;
                    string element2 = list3.GetElement<string>(index4);
                    if (!element1.IsNullOrEmpty())
                    {
                      int key;
                      if (!Manager.Resources.HSceneTables.HTagTable.TryGetValue(element1, out key))
                      {
                        Debug.LogWarning((object) string.Format("タグ読み取りエラー: 値={0}", (object) element1));
                      }
                      else
                      {
                        float result2;
                        if (!float.TryParse(element2, out result2))
                          result2 = 0.0f;
                        if (!dictionary.ContainsKey(key))
                          dictionary.Add(key, 0.0f);
                        dictionary[key] = result2;
                      }
                    }
                  }
                  this.HSkileParamTable[result1] = dictionary;
                }
              }
            }
          }
        }
      }

      private void LoadHSceneSet()
      {
        if (Object.op_Inequality((Object) this.HSceneSet, (Object) null))
          return;
        this.sbAbName.Clear();
        StringBuilder stringBuilder = new StringBuilder();
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(this.hscenePrefabPath, false);
        for (int index = 0; index < nameListFromPath.Count; ++index)
        {
          stringBuilder.Clear();
          stringBuilder.AppendFormat("add{0:00}", (object) System.IO.Path.GetFileNameWithoutExtension(nameListFromPath[index]));
          if (GlobalMethod.AssetFileExist(nameListFromPath[index], "HSceneSet", stringBuilder.ToString()))
          {
            this.sbAbName.Clear();
            this.sbAbName.Append(nameListFromPath[index]);
          }
        }
        stringBuilder.Clear();
        stringBuilder.AppendFormat("add{0:00}", (object) System.IO.Path.GetFileNameWithoutExtension(this.sbAbName.ToString()));
        this.HSceneSet = CommonLib.LoadAsset<GameObject>(this.sbAbName.ToString(), "HSceneSet", true, stringBuilder.ToString());
        this.hashUseAssetBundle[1].Add(new Manager.Resources.HSceneTables.HAssetBundle(this.sbAbName.ToString(), stringBuilder.ToString()));
        this.HSceneSet.get_transform().SetParent(!Object.op_Equality((Object) this.commonSpace, (Object) null) ? this.commonSpace.get_transform() : GameObject.Find("CommonSpace").get_transform(), false);
        this.HSceneSet.get_transform().set_localPosition(Vector3.get_zero());
        this.HSceneSet.get_transform().set_localRotation(Quaternion.get_identity());
        ((HSceneFlagCtrl) this.HSceneSet.GetComponentInChildren<HSceneFlagCtrl>()).MapHVoiceInit();
      }

      private void LoadHSceneUISet()
      {
        if (Object.op_Inequality((Object) this.HSceneUISet, (Object) null))
          return;
        this.sbAbName.Clear();
        StringBuilder stringBuilder = new StringBuilder();
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(this.hscenePrefabPath, false);
        for (int index = 0; index < nameListFromPath.Count; ++index)
        {
          stringBuilder.Clear();
          stringBuilder.AppendFormat("add{0:00}", (object) System.IO.Path.GetFileNameWithoutExtension(nameListFromPath[index]));
          if (GlobalMethod.AssetFileExist(nameListFromPath[index], "HSceneUISet", stringBuilder.ToString()))
          {
            this.sbAbName.Clear();
            this.sbAbName.Append(nameListFromPath[index]);
          }
        }
        stringBuilder.Clear();
        stringBuilder.AppendFormat("add{0:00}", (object) System.IO.Path.GetFileNameWithoutExtension(this.sbAbName.ToString()));
        this.HSceneUISet = CommonLib.LoadAsset<GameObject>(this.sbAbName.ToString(), "HSceneUISet", true, stringBuilder.ToString());
        this.hashUseAssetBundle[1].Add(new Manager.Resources.HSceneTables.HAssetBundle(this.sbAbName.ToString(), stringBuilder.ToString()));
        this.HSceneUISet.get_transform().SetParent(!Object.op_Equality((Object) this.commonSpace, (Object) null) ? this.commonSpace.get_transform() : GameObject.Find("CommonSpace").get_transform(), false);
        this.HSceneUISet.get_transform().set_localPosition(Vector3.get_zero());
        this.HSceneUISet.get_transform().set_localRotation(Quaternion.get_identity());
      }

      private bool GetIntMember(string[,] _str, int _y, ref int _line, ref int _member)
      {
        if (_str.GetLength(1) <= _line)
          return false;
        string[,] strArray = _str;
        int index1 = _y;
        int num;
        _line = (num = _line) + 1;
        int index2 = num;
        string str = strArray[index1, index2];
        if (!str.IsNullOrEmpty())
          _member = int.Parse(str);
        return true;
      }

      public void SetHmesh(Transform parent)
      {
        this.HMeshObjDic.Clear();
        foreach (KeyValuePair<int, Dictionary<int, Manager.Resources.HSceneTables.HmeshInfo>> hmesh in this.HmeshDictionary)
        {
          int mapId = Singleton<Manager.Map>.Instance.MapID;
          Manager.Resources.HSceneTables.HmeshInfo hmeshInfo;
          if (hmesh.Value.TryGetValue(mapId, out hmeshInfo))
          {
            GameObject mesh = CommonLib.LoadAsset<GameObject>(hmeshInfo.abName, hmeshInfo.assetName, true, hmeshInfo.manifest);
            this.hashUseAssetBundle[0].Add(new Manager.Resources.HSceneTables.HAssetBundle(hmeshInfo.abName, hmeshInfo.manifest));
            mesh.get_transform().SetParent(parent, false);
            mesh.get_transform().set_localPosition(Vector3.get_zero());
            mesh.get_transform().set_localRotation(Quaternion.get_identity());
            this.HMeshObjDic.Add(mapId, mesh);
            ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.NextFrame((FrameCountType) 0), (System.Action<M0>) (_ =>
            {
              HMeshData component = (HMeshData) mesh.GetComponent<HMeshData>();
              if (Object.op_Equality((Object) component, (Object) null))
                return;
              component.SetColliderAreaMap();
            }));
          }
        }
      }

      private void LoadHitObject()
      {
        this.pathList = CommonLib.GetAssetBundleNameListFromPath(Singleton<HSceneManager>.Instance.strAssetHitObjListFolder, false);
        this.pathList.Sort();
        this.sbAbName.Clear();
        this.sbAssetName.Clear();
        this.sbAssetName.Append("base");
        this.excelData = (ExcelData) null;
        List<string> stringList1 = new List<string>();
        for (int index1 = 0; index1 < this.pathList.Count; ++index1)
        {
          this.sbAbName.Clear();
          this.sbAbName.Append(this.pathList[index1]);
          if (GlobalMethod.AssetFileExist(this.sbAbName.ToString(), this.sbAssetName.ToString(), string.Empty))
          {
            this.excelData = CommonLib.LoadAsset<ExcelData>(this.sbAbName.ToString(), this.sbAssetName.ToString(), false, string.Empty);
            this.hashUseAssetBundle[0].Add(new Manager.Resources.HSceneTables.HAssetBundle(this.sbAbName.ToString(), string.Empty));
            if (!Object.op_Equality((Object) this.excelData, (Object) null))
            {
              int num1 = 1;
              while (num1 < this.excelData.MaxCell)
              {
                this.row = this.excelData.list[num1++].list;
                int num2 = 0;
                List<string> stringList2 = new List<string>();
                int result = -1;
                List<string> row = this.row;
                int index2 = num2;
                int num3 = index2 + 1;
                if (int.TryParse(row.GetElement<string>(index2), out result))
                {
                  while (num3 < this.row.Count)
                    stringList2.Add(this.row.GetElement<string>(num3++));
                  this.lstHitObject.Add(result, stringList2);
                }
              }
            }
          }
        }
      }

      private void CollisionLoadExcel()
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(Singleton<HSceneManager>.Instance.strAssetCollisionListFolder, false);
        nameListFromPath.Sort();
        CollisionCtrl.CollisionInfo info = new CollisionCtrl.CollisionInfo();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          this.sbAbName.Clear();
          this.sbAbName.Append(nameListFromPath[index1]);
          for (int index2 = 0; index2 < this.lstAnimInfo.Length; ++index2)
          {
            for (int index3 = 0; index3 < this.lstAnimInfo[index2].Count; ++index3)
            {
              this.CollisionLoadExcel(this.lstAnimInfo[index2][index3], 0, ref info);
              this.CollisionLoadExcel(this.lstAnimInfo[index2][index3], 1, ref info);
              this.CollisionLoadExcel(this.lstAnimInfo[index2][index3], 2, ref info);
            }
          }
        }
      }

      private void CollisionLoadExcel(
        HScene.AnimationListInfo ainfo,
        int kind,
        ref CollisionCtrl.CollisionInfo info)
      {
        switch (kind)
        {
          case 0:
            if (ainfo.fileMale.IsNullOrEmpty())
              return;
            this.sbAssetName.Clear();
            this.sbAssetName.Append(ainfo.fileMale);
            break;
          case 1:
            if (ainfo.fileFemale.IsNullOrEmpty())
              return;
            this.sbAssetName.Clear();
            this.sbAssetName.Append(ainfo.fileFemale);
            break;
          case 2:
            if (ainfo.fileFemale2.IsNullOrEmpty())
              return;
            this.sbAssetName.Clear();
            this.sbAssetName.Append(ainfo.fileFemale2);
            break;
        }
        if (!GlobalMethod.AssetFileExist(this.sbAbName.ToString(), this.sbAssetName.ToString(), string.Empty))
          return;
        this.excelData = CommonLib.LoadAsset<ExcelData>(this.sbAbName.ToString(), this.sbAssetName.ToString(), false, string.Empty);
        this.hashUseAssetBundle[0].Add(new Manager.Resources.HSceneTables.HAssetBundle(this.sbAbName.ToString(), string.Empty));
        if (Object.op_Equality((Object) this.excelData, (Object) null))
          return;
        if (!this.DicLstCollisionInfo.ContainsKey(this.sbAssetName.ToString()))
          this.DicLstCollisionInfo.Add(this.sbAssetName.ToString(), new List<CollisionCtrl.CollisionInfo>());
        int num1 = 1;
        while (num1 < this.excelData.MaxCell)
        {
          this.row = this.excelData.list[num1++].list;
          int num2 = 0;
          ref CollisionCtrl.CollisionInfo local = ref info;
          List<string> row = this.row;
          int index = num2;
          int num3 = index + 1;
          string element = row.GetElement<string>(index);
          local.nameAnimation = element;
          info.lstIsActive = new List<bool>();
          while (num3 < this.row.Count)
            info.lstIsActive.Add(this.row.GetElement<string>(num3++) == "1");
          if (this.DicLstCollisionInfo[this.sbAssetName.ToString()] == null)
            this.DicLstCollisionInfo[this.sbAssetName.ToString()] = new List<CollisionCtrl.CollisionInfo>();
          this.DicLstCollisionInfo[this.sbAssetName.ToString()].Add(info);
        }
      }

      private void HitObjLoadExcel()
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(Singleton<HSceneManager>.Instance.strAssetHitObjListFolder, false);
        nameListFromPath.Sort();
        HitObjectCtrl.CollisionInfo info = new HitObjectCtrl.CollisionInfo();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          this.sbAbName.Clear();
          this.sbAbName.Append(nameListFromPath[index1]);
          for (int index2 = 0; index2 < this.lstAnimInfo.Length; ++index2)
          {
            for (int index3 = 0; index3 < this.lstAnimInfo[index2].Count; ++index3)
            {
              this.HitObjLoadExcel(this.lstAnimInfo[index2][index3], 0, ref info);
              this.HitObjLoadExcel(this.lstAnimInfo[index2][index3], 1, ref info);
              this.HitObjLoadExcel(this.lstAnimInfo[index2][index3], 2, ref info);
            }
          }
        }
      }

      private void HitObjLoadExcel(
        HScene.AnimationListInfo ai,
        int kind,
        ref HitObjectCtrl.CollisionInfo info)
      {
        switch (kind)
        {
          case 0:
            if (ai.fileMale.IsNullOrEmpty())
              return;
            this.sbAssetName.Clear();
            this.sbAssetName.Append(ai.fileMale);
            break;
          case 1:
            if (ai.fileFemale.IsNullOrEmpty())
              return;
            this.sbAssetName.Clear();
            this.sbAssetName.Append(ai.fileFemale);
            break;
          case 2:
            if (ai.fileFemale2.IsNullOrEmpty())
              return;
            this.sbAssetName.Clear();
            this.sbAssetName.Append(ai.fileFemale2);
            break;
        }
        this.excelData = (ExcelData) null;
        if (!GlobalMethod.AssetFileExist(this.sbAbName.ToString(), this.sbAssetName.ToString(), string.Empty))
          return;
        this.excelData = CommonLib.LoadAsset<ExcelData>(this.sbAbName.ToString(), this.sbAssetName.ToString(), false, string.Empty);
        this.hashUseAssetBundle[0].Add(new Manager.Resources.HSceneTables.HAssetBundle(this.sbAbName.ToString(), string.Empty));
        if (Object.op_Equality((Object) this.excelData, (Object) null))
          return;
        int num1 = 0;
        int num2 = 0;
        num2 = 1;
        if (!this.HitObjAtariName.ContainsKey(this.sbAssetName.ToString()))
          this.HitObjAtariName.Add(this.sbAssetName.ToString(), new List<string>());
        List<ExcelData.Param> list = this.excelData.list;
        int index1 = num1;
        int num3 = index1 + 1;
        this.row = list[index1].list;
        for (int index2 = 1; index2 < this.row.Count; ++index2)
          this.HitObjAtariName[this.sbAssetName.ToString()].Add(this.row.GetElement<string>(index2));
        if (!this.DicLstHitObjInfo.ContainsKey(this.sbAssetName.ToString()))
          this.DicLstHitObjInfo.Add(this.sbAssetName.ToString(), new List<HitObjectCtrl.CollisionInfo>());
        while (num3 < this.excelData.MaxCell)
        {
          int num4 = 0;
          this.row = this.excelData.list[num3++].list;
          ref HitObjectCtrl.CollisionInfo local = ref info;
          List<string> row = this.row;
          int index2 = num4;
          int num5 = index2 + 1;
          string element = row.GetElement<string>(index2);
          local.nameAnimation = element;
          info.lstIsActive = new List<bool>();
          for (int index3 = 1; index3 < this.row.Count; ++index3)
            info.lstIsActive.Add(this.row.GetElement<string>(num5++) == "1");
          this.DicLstHitObjInfo[this.sbAssetName.ToString()].Add(info);
        }
      }

      private void HLayerLoadExcel()
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(Singleton<HSceneManager>.Instance.strAssetLayerCtrlListFolder, false);
        nameListFromPath.Sort();
        this.excelData = (ExcelData) null;
        HLayerCtrl.HLayerInfo info = new HLayerCtrl.HLayerInfo();
        StringBuilder stateName = new StringBuilder();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          this.sbAbName.Clear();
          this.sbAbName.Append(nameListFromPath[index1]);
          for (int index2 = 0; index2 < this.lstAnimInfo.Length; ++index2)
          {
            for (int index3 = 0; index3 < this.lstAnimInfo[index2].Count; ++index3)
            {
              this.HLayerLoadExcel(this.lstAnimInfo[index2][index3], 0, stateName, ref info);
              this.HLayerLoadExcel(this.lstAnimInfo[index2][index3], 1, stateName, ref info);
              this.HLayerLoadExcel(this.lstAnimInfo[index2][index3], 2, stateName, ref info);
            }
          }
        }
      }

      private void HLayerLoadExcel(
        HScene.AnimationListInfo ai,
        int kind,
        StringBuilder stateName,
        ref HLayerCtrl.HLayerInfo info)
      {
        switch (kind)
        {
          case 0:
            if (ai.fileMale.IsNullOrEmpty())
              return;
            this.sbAssetName.Clear();
            this.sbAssetName.Append(ai.fileMale);
            break;
          case 1:
            if (ai.fileFemale.IsNullOrEmpty())
              return;
            this.sbAssetName.Clear();
            this.sbAssetName.Append(ai.fileFemale);
            break;
          case 2:
            if (ai.fileFemale2.IsNullOrEmpty())
              return;
            this.sbAssetName.Clear();
            this.sbAssetName.Append(ai.fileFemale2);
            break;
        }
        if (!GlobalMethod.AssetFileExist(this.sbAbName.ToString(), this.sbAssetName.ToString(), string.Empty))
          return;
        this.excelData = CommonLib.LoadAsset<ExcelData>(this.sbAbName.ToString(), this.sbAssetName.ToString(), false, string.Empty);
        this.hashUseAssetBundle[0].Add(new Manager.Resources.HSceneTables.HAssetBundle(this.sbAbName.ToString(), string.Empty));
        if (Object.op_Equality((Object) this.excelData, (Object) null))
          return;
        if (!this.LayerInfos.ContainsKey(this.sbAssetName.ToString()))
          this.LayerInfos.Add(this.sbAssetName.ToString(), new Dictionary<string, HLayerCtrl.HLayerInfo>());
        int num1 = 1;
        while (num1 < this.excelData.MaxCell)
        {
          this.row = this.excelData.list[num1++].list;
          int num2 = 0;
          stateName.Clear();
          StringBuilder stringBuilder = stateName;
          List<string> row1 = this.row;
          int index1 = num2;
          int num3 = index1 + 1;
          string str = row1[index1];
          stringBuilder.Append(str);
          if (!(stateName.ToString() == string.Empty))
          {
            if (!this.LayerInfos[this.sbAssetName.ToString()].ContainsKey(stateName.ToString()))
              this.LayerInfos[this.sbAssetName.ToString()].Add(stateName.ToString(), new HLayerCtrl.HLayerInfo());
            int result1 = 0;
            float result2 = 0.0f;
            List<string> row2 = this.row;
            int index2 = num3;
            int num4 = index2 + 1;
            if (int.TryParse(row2[index2], out result1))
            {
              List<string> row3 = this.row;
              int index3 = num4;
              int num5 = index3 + 1;
              float.TryParse(row3[index3], out result2);
            }
            else
              result1 = 0;
            info.LayerID = result1;
            info.weight = result2;
            this.LayerInfos[this.sbAssetName.ToString()][stateName.ToString()] = info;
          }
        }
      }

      private int AutoHpointID(string tag)
      {
        int num = -1;
        foreach (KeyValuePair<int, HPoint.HpointData> loadHpointData in this.loadHPointDatas)
        {
          if (HSceneManager.HmeshTag[tag] != -1)
          {
            ValueTuple<int, int> valueTuple;
            if (loadHpointData.Value.place.Count == 1 && loadHpointData.Value.place.TryGetValue(0, out valueTuple) && valueTuple.Item1 == HSceneManager.HmeshTag[tag])
            {
              num = loadHpointData.Key;
              break;
            }
          }
          else if (loadHpointData.Value.place.Count == 2)
          {
            bool flag = true;
            using (Dictionary<int, ValueTuple<int, int>>.Enumerator enumerator = loadHpointData.Value.place.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                KeyValuePair<int, ValueTuple<int, int>> current = enumerator.Current;
                if (current.Value.Item1 != null && current.Value.Item1 != 1)
                {
                  flag = false;
                  break;
                }
              }
            }
            if (flag)
            {
              num = loadHpointData.Key;
              break;
            }
          }
        }
        return num;
      }

      private void HparticleInit()
      {
        HScene componentInChildren = (HScene) this.HSceneSet.GetComponentInChildren<HScene>(true);
        this.hParticle = new HParticleCtrl();
        this.hParticle.particlePlace = componentInChildren.hParticlePlace;
        this.hParticle.Init();
      }

      private void HitObjListInit()
      {
        HScene componentInChildren = (HScene) this.HSceneSet.GetComponentInChildren<HScene>(true);
        for (int key = 0; key < 2; ++key)
        {
          componentInChildren.ctrlHitObjectFemales[key] = new HitObjectCtrl();
          componentInChildren.ctrlHitObjectFemales[key].Place = componentInChildren.hitobjPlace;
          if (!this.DicHitObject.ContainsKey(1))
            this.DicHitObject.Add(1, new Dictionary<int, Dictionary<string, GameObject>>());
          if (!this.DicHitObject[1].ContainsKey(key))
            this.DicHitObject[1].Add(key, new Dictionary<string, GameObject>());
          int count = this.lstHitObject[1].Count;
          for (int index = 0; index < count; index += 3)
          {
            this.sbAbName.Clear();
            this.sbAbName.Append(this.lstHitObject[1][index + 1]);
            if (GlobalMethod.AssetFileExist(this.sbAbName.ToString(), this.lstHitObject[1][index + 2], string.Empty))
            {
              GameObject gameObject = CommonLib.LoadAsset<GameObject>(this.sbAbName.ToString(), this.lstHitObject[1][index + 2], true, string.Empty);
              this.hashUseAssetBundle[1].Add(new Manager.Resources.HSceneTables.HAssetBundle(this.sbAbName.ToString(), string.Empty));
              if (!Object.op_Equality((Object) gameObject, (Object) null))
              {
                gameObject.SetActive(false);
                gameObject.get_transform().SetParent(componentInChildren.hitobjPlace, false);
                if (!this.DicHitObject[1][key].ContainsKey(this.lstHitObject[1][index + 2]))
                  this.DicHitObject[1][key].Add(this.lstHitObject[1][index + 2], gameObject);
                else
                  this.DicHitObject[1][key][this.lstHitObject[1][index + 2]] = gameObject;
              }
            }
          }
        }
        for (int key = 0; key < 2; ++key)
        {
          componentInChildren.ctrlHitObjectMales[key] = new HitObjectCtrl();
          componentInChildren.ctrlHitObjectMales[key].Place = componentInChildren.hitobjPlace;
          if (!this.DicHitObject.ContainsKey(0))
            this.DicHitObject.Add(0, new Dictionary<int, Dictionary<string, GameObject>>());
          if (!this.DicHitObject[0].ContainsKey(key))
            this.DicHitObject[0].Add(key, new Dictionary<string, GameObject>());
          int count = this.lstHitObject[0].Count;
          for (int index = 0; index < count; index += 3)
          {
            this.sbAbName.Clear();
            this.sbAbName.Append(this.lstHitObject[0][index + 1]);
            if (GlobalMethod.AssetFileExist(this.sbAbName.ToString(), this.lstHitObject[0][index + 2], string.Empty))
            {
              GameObject gameObject = CommonLib.LoadAsset<GameObject>(this.sbAbName.ToString(), this.lstHitObject[0][index + 2], true, string.Empty);
              this.hashUseAssetBundle[1].Add(new Manager.Resources.HSceneTables.HAssetBundle(this.sbAbName.ToString(), string.Empty));
              if (!Object.op_Equality((Object) gameObject, (Object) null))
              {
                gameObject.SetActive(false);
                gameObject.get_transform().SetParent(componentInChildren.hitobjPlace, false);
                if (!this.DicHitObject[0][key].ContainsKey(this.lstHitObject[0][index + 2]))
                  this.DicHitObject[0][key].Add(this.lstHitObject[0][index + 2], gameObject);
                else
                  this.DicHitObject[0][key][this.lstHitObject[0][index + 2]] = gameObject;
              }
            }
          }
        }
      }

      public void ChangeMapHpoint(int mapID)
      {
        if (this.HPointPrefabs != null && this.HPointPrefabs.Count > 0)
        {
          using (Dictionary<int, Dictionary<string, GameObject>>.Enumerator enumerator1 = this.HPointPrefabs.GetEnumerator())
          {
            while (enumerator1.MoveNext())
            {
              KeyValuePair<int, Dictionary<string, GameObject>> current = enumerator1.Current;
              if (current.Key != mapID)
              {
                using (Dictionary<string, GameObject>.ValueCollection.Enumerator enumerator2 = current.Value.Values.GetEnumerator())
                {
                  while (enumerator2.MoveNext())
                    Object.Destroy((Object) enumerator2.Current);
                }
                current.Value.Clear();
              }
            }
          }
        }
        if (Object.op_Inequality((Object) this.AutoHPointPrefabs, (Object) null))
        {
          foreach (HPoint componentsInChild in (HPoint[]) this.AutoHPointPrefabs.GetComponentsInChildren<HPoint>(true))
          {
            if (!Object.op_Equality((Object) componentsInChild, (Object) null) && !Object.op_Equality((Object) ((Component) componentsInChild).get_gameObject(), (Object) null))
              Object.Destroy((Object) ((Component) componentsInChild).get_gameObject());
          }
        }
        if (this.hPointLists != null && this.hPointLists.Count > 0)
        {
          List<int> intList = new List<int>();
          foreach (KeyValuePair<int, HPointList> hPointList in this.hPointLists)
          {
            if (hPointList.Key != mapID)
            {
              foreach (KeyValuePair<int, List<HPoint>> keyValuePair in hPointList.Value.lst)
              {
                foreach (HPoint hpoint in keyValuePair.Value)
                {
                  if (!Object.op_Equality((Object) hpoint, (Object) null) && !Object.op_Equality((Object) ((Component) hpoint).get_gameObject(), (Object) null))
                    Object.Destroy((Object) ((Component) hpoint).get_gameObject());
                }
                keyValuePair.Value.Clear();
              }
              intList.Add(hPointList.Key);
            }
          }
          for (int index = 0; index < intList.Count; ++index)
            this.hPointLists.Remove(intList[index]);
        }
        this.LoadHpointPrefabs(mapID);
        this.LoadAutoHPoint(mapID);
        this.HPointInitData(mapID);
      }

      public class HAssetBundle
      {
        public string path;
        public string manifest;

        public HAssetBundle(string _path, string _manifest = "")
        {
          this.path = _path;
          if (_manifest != string.Empty)
            this.manifest = _manifest;
          else
            this.manifest = "abdata";
        }
      }

      private struct HmeshInfo
      {
        public int mapID;
        public string abName;
        public string assetName;
        public string manifest;
      }
    }

    public class ItemIconTables
    {
      public Dictionary<int, Tuple<string, Sprite>> CategoryIcon = new Dictionary<int, Tuple<string, Sprite>>();
      public Dictionary<int, Tuple<string, Sprite>> ItemIcon = new Dictionary<int, Tuple<string, Sprite>>();
      public Dictionary<int, Tuple<string, Sprite>> MenuIcon = new Dictionary<int, Tuple<string, Sprite>>();
      public Dictionary<int, Tuple<string, Sprite>> SystemIcon = new Dictionary<int, Tuple<string, Sprite>>();
      public Dictionary<int, int> MiniMapIcon = new Dictionary<int, int>();
      public Dictionary<int, string> MiniMapIconName = new Dictionary<int, string>();
      public Dictionary<int, string> BaseName = new Dictionary<int, string>();
      private readonly Dictionary<Manager.Resources.ItemIconTables.IconCategory, string> BundleNameListPath = new Dictionary<Manager.Resources.ItemIconTables.IconCategory, string>()
      {
        {
          Manager.Resources.ItemIconTables.IconCategory.System,
          "system"
        },
        {
          Manager.Resources.ItemIconTables.IconCategory.Menu,
          "menu"
        },
        {
          Manager.Resources.ItemIconTables.IconCategory.Category,
          "category"
        },
        {
          Manager.Resources.ItemIconTables.IconCategory.Item,
          "item"
        }
      };

      private Dictionary<int, Tuple<string, Sprite>> this[
        Manager.Resources.ItemIconTables.IconCategory index]
      {
        get
        {
          switch (index)
          {
            case Manager.Resources.ItemIconTables.IconCategory.System:
              return this.SystemIcon;
            case Manager.Resources.ItemIconTables.IconCategory.Menu:
              return this.MenuIcon;
            case Manager.Resources.ItemIconTables.IconCategory.Category:
              return this.CategoryIcon;
            case Manager.Resources.ItemIconTables.IconCategory.Item:
              return this.ItemIcon;
            default:
              return (Dictionary<int, Tuple<string, Sprite>>) null;
          }
        }
      }

      public Dictionary<int, Sprite> InputIconTable { get; private set; } = new Dictionary<int, Sprite>();

      public Dictionary<int, Sprite> ActionIconTable { get; private set; } = new Dictionary<int, Sprite>();

      public Dictionary<int, Sprite> ActorIconTable { get; private set; } = new Dictionary<int, Sprite>();

      public Dictionary<int, Dictionary<int, int>> EquipmentIconTable { get; private set; } = new Dictionary<int, Dictionary<int, int>>();

      public Dictionary<int, Dictionary<int, Sprite>> StatusIconTable { get; private set; } = new Dictionary<int, Dictionary<int, Sprite>>();

      public Dictionary<int, Sprite> SickIconTable { get; private set; } = new Dictionary<int, Sprite>();

      public Dictionary<int, Sprite> WeatherIconTable { get; private set; } = new Dictionary<int, Sprite>();

      [DebuggerHidden]
      public IEnumerator LoadIcon(Manager.Resources.ItemIconTables.IconCategory iconCategory)
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.ItemIconTables.\u003CLoadIcon\u003Ec__Iterator0()
        {
          iconCategory = iconCategory,
          \u0024this = this
        };
      }

      public static void SetIcon(
        Manager.Resources.ItemIconTables.IconCategory category,
        int iconID,
        Image icon,
        bool imageName = true)
      {
        Dictionary<int, Tuple<string, Sprite>> itemIconTable = Singleton<Manager.Resources>.Instance.itemIconTables[category];
        if (!itemIconTable.ContainsKey(iconID))
          return;
        icon.set_sprite(itemIconTable[iconID].Item2);
        if (!imageName)
          return;
        UnityEngine.UI.Text componentInChildren = (UnityEngine.UI.Text) ((Component) icon).GetComponentInChildren<UnityEngine.UI.Text>();
        if (Object.op_Equality((Object) componentInChildren, (Object) null))
          return;
        componentInChildren.set_text(itemIconTable[iconID].Item1);
        ((Behaviour) componentInChildren).set_enabled(Object.op_Equality((Object) icon.get_sprite(), (Object) null));
        ((Behaviour) icon).set_enabled(!((Behaviour) componentInChildren).get_enabled());
      }

      public void Load(DefinePack definePack)
      {
        this.LoadInputIcon(definePack);
        this.LoadActionIcon(definePack);
        this.LoadActorIcon(definePack);
        this.LoadWeatherIcon(definePack);
        this.LoadEquipItemIcon(definePack);
        this.LoadStatusIcon(definePack);
        this.LoadSickIcon(definePack);
      }

      public void Release()
      {
        this.CategoryIcon.Clear();
        this.ItemIcon.Clear();
        this.MenuIcon.Clear();
        this.SystemIcon.Clear();
        this.MiniMapIcon.Clear();
        this.MiniMapIconName.Clear();
        this.BaseName.Clear();
        this.InputIconTable.Clear();
        this.ActionIconTable.Clear();
        this.ActorIconTable.Clear();
        this.EquipmentIconTable.Clear();
        this.StatusIconTable.Clear();
        this.SickIconTable.Clear();
        this.WeatherIconTable.Clear();
      }

      private void LoadInputIcon(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.InputIconList, false);
        nameListFromPath.Sort();
        this.LoadIcon(nameListFromPath, this.InputIconTable);
      }

      private void LoadActionIcon(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.ActionIconList, false);
        nameListFromPath.Sort();
        this.LoadIcon(nameListFromPath, this.ActionIconTable);
      }

      private void LoadActorIcon(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.ActorIconList, false);
        nameListFromPath.Sort();
        this.LoadIcon(nameListFromPath, this.ActorIconTable);
      }

      private void LoadWeatherIcon(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.WeatherIconList, false);
        nameListFromPath.Sort();
        this.LoadIcon(nameListFromPath, this.WeatherIconTable);
      }

      [DebuggerHidden]
      public IEnumerator LoadMinimapActionIconList()
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.ItemIconTables.\u003CLoadMinimapActionIconList\u003Ec__Iterator1()
        {
          \u0024this = this
        };
      }

      [DebuggerHidden]
      public IEnumerator LoadMinimapActionIconNameList(DefinePack definePack)
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.ItemIconTables.\u003CLoadMinimapActionIconNameList\u003Ec__Iterator2()
        {
          definePack = definePack,
          \u0024this = this
        };
      }

      private void LoadIcon(List<string> pathList, Dictionary<int, Sprite> table)
      {
        foreach (string path in pathList)
        {
          foreach (GameIconData allAsset in AssetBundleManager.LoadAllAsset(path, typeof (GameIconData), (string) null).GetAllAssets<GameIconData>())
          {
            foreach (GameIconData.Param obj in allAsset.param)
            {
              Sprite sprite = Manager.Resources.ItemIconTables.LoadSpriteAsset(obj.Bundle, obj.Asset, obj.Manifest);
              if (!Object.op_Equality((Object) sprite, (Object) null))
              {
                Singleton<Manager.Resources>.Instance.AddLoadAssetBundle(obj.Bundle, obj.Manifest);
                table[obj.ID] = sprite;
              }
            }
          }
          Singleton<Manager.Resources>.Instance.AddLoadAssetBundle(path, string.Empty);
        }
      }

      [DebuggerHidden]
      private IEnumerator LoadIconAsync(
        List<string> pathList,
        Dictionary<int, Sprite> table)
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.ItemIconTables.\u003CLoadIconAsync\u003Ec__Iterator3()
        {
          pathList = pathList,
          table = table
        };
      }

      private void LoadEquipItemIcon(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.EquipItemIconList, false);
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
            {
              ExcelData.Param obj = excelData.list[index2];
              int num1 = 1;
              List<string> list1 = obj.list;
              int index3 = num1;
              int num2 = index3 + 1;
              int result1;
              if (int.TryParse(list1.GetElement<string>(index3), out result1))
              {
                List<string> list2 = obj.list;
                int index4 = num2;
                int num3 = index4 + 1;
                int result2;
                if (int.TryParse(list2.GetElement<string>(index4), out result2))
                {
                  List<string> list3 = obj.list;
                  int index5 = num3;
                  int num4 = index5 + 1;
                  int result3;
                  if (int.TryParse(list3.GetElement<string>(index5), out result3))
                  {
                    Dictionary<int, int> dictionary1;
                    if (!this.EquipmentIconTable.TryGetValue(result1, out dictionary1))
                    {
                      Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
                      this.EquipmentIconTable[result1] = dictionary2;
                      dictionary1 = dictionary2;
                    }
                    dictionary1[result2] = result3;
                  }
                }
              }
            }
          }
        }
      }

      private void LoadStatusIcon(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.StatusIconList, false);
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
            {
              ExcelData.Param obj = excelData.list[index2];
              int num1 = 2;
              List<string> list1 = obj.list;
              int index3 = num1;
              int num2 = index3 + 1;
              int result1;
              if (int.TryParse(list1.GetElement<string>(index3), out result1))
              {
                List<string> list2 = obj.list;
                int index4 = num2;
                int num3 = index4 + 1;
                int result2;
                if (int.TryParse(list2.GetElement<string>(index4), out result2))
                {
                  List<string> list3 = obj.list;
                  int index5 = num3;
                  int num4 = index5 + 1;
                  string element1 = list3.GetElement<string>(index5);
                  List<string> list4 = obj.list;
                  int index6 = num4;
                  int num5 = index6 + 1;
                  string element2 = list4.GetElement<string>(index6);
                  List<string> list5 = obj.list;
                  int index7 = num5;
                  int num6 = index7 + 1;
                  string element3 = list5.GetElement<string>(index7);
                  Dictionary<int, Sprite> dictionary1;
                  if (!this.StatusIconTable.TryGetValue(result1, out dictionary1))
                  {
                    Dictionary<int, Sprite> dictionary2 = new Dictionary<int, Sprite>();
                    this.StatusIconTable[result1] = dictionary2;
                    dictionary1 = dictionary2;
                  }
                  Sprite sprite = Manager.Resources.ItemIconTables.LoadSpriteAsset(element1, element2, element3);
                  dictionary1[result2] = sprite;
                  Singleton<Manager.Resources>.Instance.AddLoadAssetBundle(element1, element3);
                }
              }
            }
          }
        }
      }

      private void LoadSickIcon(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.SickIconList, false);
        nameListFromPath.Sort();
        this.LoadIcon(nameListFromPath, this.SickIconTable);
      }

      public static Sprite LoadSpriteAsset(
        string assetBundleName,
        string assetName,
        string manifestName)
      {
        manifestName = !manifestName.IsNullOrEmpty() ? manifestName : (string) null;
        if (AssetBundleCheck.IsSimulation)
          manifestName = string.Empty;
        if (!AssetBundleCheck.IsFile(assetBundleName, assetName))
        {
          Debug.LogWarning((object) string.Format("読み込みエラー\r\nassetBundleName：{0}\tassetName：{1}", (object) assetBundleName, (object) assetName));
          return (Sprite) null;
        }
        AssetBundleLoadAssetOperation loadAssetOperation = AssetBundleManager.LoadAsset(assetBundleName, assetName, typeof (Sprite), !manifestName.IsNullOrEmpty() ? manifestName : (string) null);
        Sprite asset1 = loadAssetOperation.GetAsset<Sprite>();
        if (Object.op_Equality((Object) asset1, (Object) null))
        {
          Texture2D asset2 = loadAssetOperation.GetAsset<Texture2D>();
          if (Object.op_Equality((Object) asset2, (Object) null))
            return (Sprite) null;
          asset1 = Sprite.Create(asset2, new Rect(0.0f, 0.0f, (float) ((Texture) asset2).get_width(), (float) ((Texture) asset2).get_height()), Vector2.get_zero());
        }
        return asset1;
      }

      [DebuggerHidden]
      public IEnumerator LoadMinimapActionIconList(
        List<string> pathList,
        Dictionary<int, int> table)
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.ItemIconTables.\u003CLoadMinimapActionIconList\u003Ec__Iterator4()
        {
          pathList = pathList,
          table = table
        };
      }

      [DebuggerHidden]
      public IEnumerator LoadMinimapActionIconNameList(
        List<string> pathList,
        Dictionary<int, string> table,
        int mode)
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new Manager.Resources.ItemIconTables.\u003CLoadMinimapActionIconNameList\u003Ec__Iterator5()
        {
          pathList = pathList,
          mode = mode,
          table = table
        };
      }

      public enum IconCategory
      {
        System,
        Menu,
        Category,
        Item,
      }
    }

    public class MapTables
    {
      private static readonly string[] _separators = new string[1]
      {
        ","
      };
      public Dictionary<int, Dictionary<int, MinimapNavimesh.AreaGroupInfo>> AreaGroupTable = new Dictionary<int, Dictionary<int, MinimapNavimesh.AreaGroupInfo>>();

      public Dictionary<int, AssetBundleInfo> MapList { get; private set; } = new Dictionary<int, AssetBundleInfo>();

      public Dictionary<int, AssetBundleInfo> ChunkList { get; private set; } = new Dictionary<int, AssetBundleInfo>();

      public Dictionary<int, AssetBundleInfo> SEMeshList { get; private set; } = new Dictionary<int, AssetBundleInfo>();

      public Dictionary<int, AssetBundleInfo> CameraColliderList { get; private set; } = new Dictionary<int, AssetBundleInfo>();

      public Dictionary<int, Dictionary<int, string>> MapGroupNameList { get; set; } = new Dictionary<int, Dictionary<int, string>>();

      public Dictionary<int, Dictionary<int, List<int>>> MapGroupHiddenAreaList { get; set; } = new Dictionary<int, Dictionary<int, List<int>>>();

      public Dictionary<int, Dictionary<int, List<int>>> AgentHiddenAreaList { get; set; } = new Dictionary<int, Dictionary<int, List<int>>>();

      public Dictionary<int, AssetBundleInfo> PlantItemList { get; private set; } = new Dictionary<int, AssetBundleInfo>();

      public List<AIProject.ItemInfo> PlantIvyFilterList { get; private set; } = new List<AIProject.ItemInfo>();

      public Dictionary<int, ActionItemInfo> EventItemList { get; private set; } = new Dictionary<int, ActionItemInfo>();

      public Dictionary<int, Dictionary<int, int>> FoodEventItemList { get; private set; } = new Dictionary<int, Dictionary<int, int>>();

      public Dictionary<int, Dictionary<int, int>> FoodDateEventItemList { get; private set; } = new Dictionary<int, Dictionary<int, int>>();

      public Dictionary<int, AssetBundleInfo> NavMeshSourceList { get; private set; } = new Dictionary<int, AssetBundleInfo>();

      public Dictionary<int, AssetBundleInfo> BasePointGroupTable { get; private set; } = new Dictionary<int, AssetBundleInfo>();

      public Dictionary<int, AssetBundleInfo> DevicePointGroupTable { get; private set; } = new Dictionary<int, AssetBundleInfo>();

      public Dictionary<int, AssetBundleInfo> HarvestPointGroupTable { get; private set; } = new Dictionary<int, AssetBundleInfo>();

      public Dictionary<int, AssetBundleInfo> ShipPointGroupTable { get; private set; } = new Dictionary<int, AssetBundleInfo>();

      public Dictionary<int, AssetBundleInfo> ActionPointGroupTable { get; private set; } = new Dictionary<int, AssetBundleInfo>();

      public Dictionary<int, Dictionary<int, AssetBundleInfo>> MerchantPointGroupTable { get; private set; } = new Dictionary<int, Dictionary<int, AssetBundleInfo>>();

      public Dictionary<int, Dictionary<int, AssetBundleInfo>> EventPointGroupTable { get; private set; } = new Dictionary<int, Dictionary<int, AssetBundleInfo>>();

      public Dictionary<int, Dictionary<int, AssetBundleInfo>> StoryPointGroupTable { get; private set; } = new Dictionary<int, Dictionary<int, AssetBundleInfo>>();

      public Dictionary<int, Dictionary<int, AssetBundleInfo>> LightSwitchPointGroupTable { get; private set; } = new Dictionary<int, Dictionary<int, AssetBundleInfo>>();

      public Dictionary<int, Dictionary<int, List<ActionPointInfo>>> PlayerActionPointInfoTable { get; private set; } = new Dictionary<int, Dictionary<int, List<ActionPointInfo>>>();

      public Dictionary<int, Dictionary<int, List<ActionPointInfo>>> AgentActionPointInfoTable { get; private set; } = new Dictionary<int, Dictionary<int, List<ActionPointInfo>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<DateActionPointInfo>>>> PlayerDateActionPointInfoTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<DateActionPointInfo>>>>();

      public Dictionary<int, Dictionary<int, List<DateActionPointInfo>>> AgentDateActionPointInfoTable { get; private set; } = new Dictionary<int, Dictionary<int, List<DateActionPointInfo>>>();

      public Dictionary<int, Dictionary<int, List<ActionPointInfo>>> MerchantActionPointInfoTable { get; private set; } = new Dictionary<int, Dictionary<int, List<ActionPointInfo>>>();

      public Dictionary<int, Dictionary<int, List<MerchantPointInfo>>> MerchantPointInfoTable { get; private set; } = new Dictionary<int, Dictionary<int, List<MerchantPointInfo>>>();

      public Dictionary<int, AssetBundleInfo> EventParticleTable { get; private set; } = new Dictionary<int, AssetBundleInfo>();

      public Dictionary<int, Dictionary<bool, string[]>> AreaOpenStateObjectNameTable { get; private set; } = new Dictionary<int, Dictionary<bool, string[]>>();

      public Dictionary<int, List<string>> EventPointCommandLabelTextTable { get; private set; } = new Dictionary<int, List<string>>();

      public Dictionary<int, ValueTuple<int, List<string>>> EventDialogInfoTable { get; private set; } = new Dictionary<int, ValueTuple<int, List<string>>>();

      public Dictionary<int, string> AreaOpenIDTable { get; private set; } = new Dictionary<int, string>();

      public Dictionary<int, int[]> AreaOpenStateMapAreaLinkerTable { get; private set; } = new Dictionary<int, int[]>();

      public Dictionary<int, Dictionary<int, Dictionary<bool, Dictionary<int, List<ValueTuple<string, float>>>>>> TimeRelationObjectStateTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<bool, Dictionary<int, List<ValueTuple<string, float>>>>>>();

      public Dictionary<int, string> TimeRelationObjectIDTable { get; private set; } = new Dictionary<int, string>();

      public Dictionary<int, Dictionary<int, ActionCameraData>> ActionCameraDataTable { get; private set; } = new Dictionary<int, Dictionary<int, ActionCameraData>>();

      public Dictionary<int, List<List<Manager.Resources.MapTables.VisibleObjectInfo>>> VanishList { get; private set; } = new Dictionary<int, List<List<Manager.Resources.MapTables.VisibleObjectInfo>>>();

      public Dictionary<int, Dictionary<int, List<int>>> VanishHousingAreaGroup { get; private set; } = new Dictionary<int, Dictionary<int, List<int>>>();

      public Dictionary<int, Dictionary<int, Dictionary<int, List<ValueTuple<int, int>>>>> TempRangeTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, List<ValueTuple<int, int>>>>>();

      public Dictionary<int, MiniMapControler.MinimapInfo> MinimapInfoTable { get; private set; } = new Dictionary<int, MiniMapControler.MinimapInfo>();

      public void Load(DefinePack definePack)
      {
        this.LoadMapList(definePack);
        this.LoadChunkList(definePack);
        this.LoadSEMeshList(definePack);
        this.LoadCameraColliderList(definePack);
        this.LoadMapGroupList(definePack);
        this.LoadMapHiddenAreaList(definePack);
        this.LoadAgentVanishAreaList(definePack);
        this.LoadPlantItemList(definePack);
        this.LoadIvyFilterList(definePack);
        this.LoadNavMeshSourceTable(definePack);
        this.LoadEventItemList(definePack);
        this.LoadFoodEventItemList(definePack);
        this.LoadEventParticleList(definePack);
        this.LoadBasePointListTable(definePack);
        this.LoadDevicePointListTable(definePack);
        this.LoadFarmPointListTable(definePack);
        this.LoadShipPointListTable(definePack);
        this.LoadActionPointListTable(definePack);
        this.LoadLightSwitchPointListTable(definePack);
        this.LoadPlayerActionPointInfo(definePack);
        this.LoadAgentActionPointInfo(definePack);
        this.LoadPlayerDateActionPointInfo(definePack);
        this.LoadAgentDateActionPointInfo(definePack);
        this.LoadMerchantPoint(definePack);
        this.LoadEventPointListTable(definePack);
        this.LoadStoryPointListTable(definePack);
        this.LoadAreaOpenStateList(definePack);
        this.LoadTimeRelationInfoList(definePack);
        this.LoadAreaGroup(definePack);
        this.LoadActionCameraData(definePack);
        this.LoadVanish(definePack);
        this.LoadVanishHousingGroup(definePack);
        this.LoadEnviroInfoList(definePack);
        this.LoadMiniMapInfo();
      }

      public void Release()
      {
        this.MapList.Clear();
        this.ChunkList.Clear();
        this.SEMeshList.Clear();
        this.CameraColliderList.Clear();
        this.MapGroupNameList.Clear();
        this.MapGroupHiddenAreaList.Clear();
        this.AgentHiddenAreaList.Clear();
        this.PlantItemList.Clear();
        this.PlantIvyFilterList.Clear();
        this.EventItemList.Clear();
        this.FoodEventItemList.Clear();
        this.FoodDateEventItemList.Clear();
        this.NavMeshSourceList.Clear();
        this.BasePointGroupTable.Clear();
        this.DevicePointGroupTable.Clear();
        this.HarvestPointGroupTable.Clear();
        this.ShipPointGroupTable.Clear();
        this.ActionPointGroupTable.Clear();
        this.MerchantPointGroupTable.Clear();
        this.EventPointGroupTable.Clear();
        this.StoryPointGroupTable.Clear();
        this.LightSwitchPointGroupTable.Clear();
        this.PlayerActionPointInfoTable.Clear();
        this.AgentActionPointInfoTable.Clear();
        this.PlayerDateActionPointInfoTable.Clear();
        this.AgentDateActionPointInfoTable.Clear();
        this.MerchantActionPointInfoTable.Clear();
        this.MerchantPointInfoTable.Clear();
        this.EventParticleTable.Clear();
        this.AreaOpenStateObjectNameTable.Clear();
        this.EventPointCommandLabelTextTable.Clear();
        this.EventDialogInfoTable.Clear();
        this.AreaOpenIDTable.Clear();
        this.AreaOpenStateMapAreaLinkerTable.Clear();
        this.AreaGroupTable.Clear();
        this.TimeRelationObjectStateTable.Clear();
        this.TimeRelationObjectIDTable.Clear();
        this.ActionCameraDataTable.Clear();
        this.VanishList.Clear();
        this.TempRangeTable.Clear();
        this.MinimapInfoTable.Clear();
      }

      private void LoadMapList(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.MapList, false);
        nameListFromPath.Sort();
        if (nameListFromPath.Count == 0)
          return;
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            int num1 = 0;
            while (num1 < excelData.MaxCell)
            {
              ExcelData.Param obj = excelData.list[num1++];
              int num2 = 0;
              List<string> list1 = obj.list;
              int index2 = num2;
              int num3 = index2 + 1;
              int result;
              if (int.TryParse(list1.GetElement<string>(index2), out result))
              {
                List<string> list2 = obj.list;
                int index3 = num3;
                int num4 = index3 + 1;
                string element1 = list2.GetElement<string>(index3);
                List<string> list3 = obj.list;
                int index4 = num4;
                int num5 = index4 + 1;
                string element2 = list3.GetElement<string>(index4);
                List<string> list4 = obj.list;
                int index5 = num5;
                int num6 = index5 + 1;
                string element3 = list4.GetElement<string>(index5);
                List<string> list5 = obj.list;
                int index6 = num6;
                int num7 = index6 + 1;
                string element4 = list5.GetElement<string>(index6);
                AssetBundleInfo assetBundleInfo;
                ((AssetBundleInfo) ref assetBundleInfo).\u002Ector(element1, element2, element3, element4);
                this.MapList[result] = assetBundleInfo;
              }
            }
          }
        }
      }

      private void LoadChunkList(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.ChunkList, false);
        nameListFromPath.Sort();
        if (nameListFromPath.Count == 0)
          return;
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            int num1 = 0;
            while (num1 < excelData.MaxCell)
            {
              ExcelData.Param obj = excelData.list[num1++];
              int num2 = 0;
              List<string> list1 = obj.list;
              int index2 = num2;
              int num3 = index2 + 1;
              int result;
              if (int.TryParse(list1.GetElement<string>(index2), out result))
              {
                List<string> list2 = obj.list;
                int index3 = num3;
                int num4 = index3 + 1;
                string element1 = list2.GetElement<string>(index3);
                List<string> list3 = obj.list;
                int index4 = num4;
                int num5 = index4 + 1;
                string element2 = list3.GetElement<string>(index4);
                List<string> list4 = obj.list;
                int index5 = num5;
                int num6 = index5 + 1;
                string element3 = list4.GetElement<string>(index5);
                AssetBundleInfo assetBundleInfo;
                ((AssetBundleInfo) ref assetBundleInfo).\u002Ector(string.Empty, element1, element2, element3);
                this.ChunkList[result] = assetBundleInfo;
              }
            }
          }
        }
      }

      private void LoadSEMeshList(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.ChunkList, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str1 = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str1);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str1, string.Format("semesh_{0}", (object) withoutExtension), string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
            {
              List<string> list = excelData.list[index2].list;
              if (!list.IsNullOrEmpty<string>())
              {
                int num1 = 0;
                List<string> source1 = list;
                int index3 = num1;
                int num2 = index3 + 1;
                int result;
                if (int.TryParse(source1.GetElement<string>(index3) ?? string.Empty, out result))
                {
                  List<string> source2 = list;
                  int index4 = num2;
                  int num3 = index4 + 1;
                  string str2 = source2.GetElement<string>(index4) ?? string.Empty;
                  List<string> source3 = list;
                  int index5 = num3;
                  int num4 = index5 + 1;
                  string str3 = source3.GetElement<string>(index5) ?? string.Empty;
                  List<string> source4 = list;
                  int index6 = num4;
                  int num5 = index6 + 1;
                  string str4 = source4.GetElement<string>(index6) ?? string.Empty;
                  this.SEMeshList[result] = new AssetBundleInfo(string.Empty, str2, str3, str4);
                }
              }
            }
          }
        }
      }

      private void LoadCameraColliderList(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.ChunkList, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = CommonLib.LoadAsset<ExcelData>(str, string.Format("cam_col_{0}", (object) withoutExtension), false, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
            {
              List<string> list = excelData.list[index2].list;
              if (!list.IsNullOrEmpty<string>())
              {
                int num1 = 0;
                List<string> source1 = list;
                int index3 = num1;
                int num2 = index3 + 1;
                int result;
                if (int.TryParse(source1.GetElement<string>(index3), out result))
                {
                  int num3 = num2 + 1;
                  List<string> source2 = list;
                  int index4 = num3;
                  int num4 = index4 + 1;
                  string element1 = source2.GetElement<string>(index4);
                  List<string> source3 = list;
                  int index5 = num4;
                  int num5 = index5 + 1;
                  string element2 = source3.GetElement<string>(index5);
                  List<string> source4 = list;
                  int index6 = num5;
                  int num6 = index6 + 1;
                  string element3 = source4.GetElement<string>(index6);
                  this.CameraColliderList[result] = new AssetBundleInfo(string.Empty, element1, element2, element3);
                }
              }
            }
          }
        }
      }

      private void LoadMapGroupList(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.ChunkList, false);
        nameListFromPath.Sort();
        List<string> toRelease = ListPool<string>.Get();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (MapHiddenGroupData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (MapHiddenGroupData), (string) null).GetAllAssets<MapHiddenGroupData>())
          {
            foreach (MapHiddenGroupData.Param obj in allAsset.param)
            {
              Dictionary<int, string> dictionary1;
              if (!this.MapGroupNameList.TryGetValue(obj.MapID, out dictionary1))
              {
                Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
                this.MapGroupNameList[obj.MapID] = dictionary2;
                dictionary1 = dictionary2;
              }
              dictionary1[obj.ID] = obj.GroupName;
            }
          }
        }
        foreach (string assetBundleName in toRelease)
          AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
        ListPool<string>.Release(toRelease);
      }

      private void LoadMapHiddenAreaList(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.ChunkList, false);
        nameListFromPath.Sort();
        List<string> toRelease = ListPool<string>.Get();
        foreach (string str in nameListFromPath)
        {
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, string.Format("area_vanish_{0}", (object) withoutExtension), string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            foreach (ExcelData.Param obj in excelData.list)
            {
              int num1 = 1;
              List<string> list1 = obj.list;
              int index1 = num1;
              int num2 = index1 + 1;
              int result1;
              if (int.TryParse(list1.GetElement<string>(index1), out result1))
              {
                List<string> list2 = obj.list;
                int index2 = num2;
                int num3 = index2 + 1;
                int result2;
                if (int.TryParse(list2.GetElement<string>(index2), out result2))
                {
                  List<string> list3 = obj.list;
                  int index3 = num3;
                  int num4 = index3 + 1;
                  string[] strArray = list3.GetElement<string>(index3).Split(Manager.Resources.MapTables._separators, StringSplitOptions.RemoveEmptyEntries);
                  if (!((IList<string>) strArray).IsNullOrEmpty<string>())
                  {
                    Dictionary<int, List<int>> dictionary1;
                    if (!this.MapGroupHiddenAreaList.TryGetValue(result1, out dictionary1))
                    {
                      Dictionary<int, List<int>> dictionary2 = new Dictionary<int, List<int>>();
                      this.MapGroupHiddenAreaList[result1] = dictionary2;
                      dictionary1 = dictionary2;
                    }
                    List<int> intList1;
                    if (!dictionary1.TryGetValue(result2, out intList1))
                    {
                      List<int> intList2 = new List<int>();
                      dictionary1[result2] = intList2;
                      intList1 = intList2;
                    }
                    foreach (string s in strArray)
                    {
                      int result3;
                      if (int.TryParse(s, out result3) && !intList1.Contains(result3))
                        intList1.Add(result3);
                    }
                  }
                }
              }
            }
          }
        }
        foreach (string assetBundleName in toRelease)
          AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
        ListPool<string>.Release(toRelease);
      }

      public void LoadAgentVanishAreaList(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.ActorVanishList, false);
        nameListFromPath.Sort();
        List<string> toRelease = ListPool<string>.Get();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (AgentHiddenAreaData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (AgentHiddenAreaData), (string) null).GetAllAssets<AgentHiddenAreaData>())
          {
            foreach (AgentHiddenAreaData.Param obj in allAsset.param)
            {
              Dictionary<int, List<int>> dictionary1;
              if (!this.AgentHiddenAreaList.TryGetValue(obj.MapID, out dictionary1))
              {
                Dictionary<int, List<int>> dictionary2 = new Dictionary<int, List<int>>();
                this.AgentHiddenAreaList[obj.MapID] = dictionary2;
                dictionary1 = dictionary2;
              }
              List<int> intList1;
              if (!dictionary1.TryGetValue(obj.AreaID, out intList1))
              {
                List<int> intList2 = new List<int>();
                dictionary1[obj.AreaID] = intList2;
                intList1 = intList2;
              }
              string[] strArray = obj.HiddenAreaIDMulti.Split(Manager.Resources.MapTables._separators, StringSplitOptions.RemoveEmptyEntries);
              if (!((IList<string>) strArray).IsNullOrEmpty<string>())
              {
                foreach (string s in strArray)
                {
                  int result;
                  if (int.TryParse(s, out result) && !intList1.Contains(result))
                    intList1.Add(result);
                }
              }
            }
          }
        }
        foreach (string assetBundleName in toRelease)
          AssetBundleManager.UnloadAssetBundle(assetBundleName, false, (string) null, false);
        ListPool<string>.Release(toRelease);
      }

      private void LoadPlantItemList(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.PlantItemList, false);
        nameListFromPath.Sort();
        if (nameListFromPath.Count == 0)
          return;
        foreach (string str in nameListFromPath)
        {
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            int num1 = 0;
            while (num1 < excelData.MaxCell)
            {
              ExcelData.Param obj = excelData.list[num1++];
              int num2 = 0;
              List<string> list1 = obj.list;
              int index1 = num2;
              int num3 = index1 + 1;
              int result;
              if (int.TryParse(list1.GetElement<string>(index1), out result))
              {
                List<string> list2 = obj.list;
                int index2 = num3;
                int num4 = index2 + 1;
                string element1 = list2.GetElement<string>(index2);
                List<string> list3 = obj.list;
                int index3 = num4;
                int num5 = index3 + 1;
                string element2 = list3.GetElement<string>(index3);
                List<string> list4 = obj.list;
                int index4 = num5;
                int num6 = index4 + 1;
                string element3 = list4.GetElement<string>(index4);
                List<string> list5 = obj.list;
                int index5 = num6;
                int num7 = index5 + 1;
                string element4 = list5.GetElement<string>(index5);
                Dictionary<int, AssetBundleInfo> plantItemList = this.PlantItemList;
                int index6 = result;
                AssetBundleInfo assetBundleInfo1 = (AssetBundleInfo) null;
                assetBundleInfo1.name = (__Null) element1;
                assetBundleInfo1.assetbundle = (__Null) element2;
                assetBundleInfo1.asset = (__Null) element3;
                assetBundleInfo1.manifest = (__Null) element4;
                AssetBundleInfo assetBundleInfo2 = assetBundleInfo1;
                plantItemList[index6] = assetBundleInfo2;
              }
            }
          }
        }
      }

      private void LoadIvyFilterList(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.IvyFilterList, false);
        nameListFromPath.Sort();
        if (nameListFromPath.Count == 0)
          return;
        List<string> toRelease = ListPool<string>.Get();
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (PlantIvyFilterData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (PlantIvyFilterData), (string) null).GetAllAssets<PlantIvyFilterData>())
          {
            foreach (PlantIvyFilterData.Param obj in allAsset.param)
            {
              PlantIvyFilterData.Param param = obj;
              if (!this.PlantIvyFilterList.Exists((Predicate<AIProject.ItemInfo>) (x => x.CategoryID == param.CategoryID && x.ItemID == param.ItemID)))
                this.PlantIvyFilterList.Add(new AIProject.ItemInfo()
                {
                  CategoryID = param.CategoryID,
                  ItemID = param.ItemID,
                  ObjID = param.ObjID
                });
            }
          }
        }
        ListPool<string>.Release(toRelease);
      }

      private void LoadEventItemList(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.EventItemList, false);
        nameListFromPath.Sort();
        if (nameListFromPath.Count == 0)
          return;
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            int num1 = 0;
            while (num1 < excelData.MaxCell)
            {
              ExcelData.Param obj = excelData.list[num1++];
              int num2 = 0;
              List<string> list1 = obj.list;
              int index2 = num2;
              int num3 = index2 + 1;
              int result1;
              if (int.TryParse(list1.GetElement<string>(index2), out result1))
              {
                List<string> list2 = obj.list;
                int index3 = num3;
                int num4 = index3 + 1;
                string element1 = list2.GetElement<string>(index3);
                List<string> list3 = obj.list;
                int index4 = num4;
                int num5 = index4 + 1;
                string element2 = list3.GetElement<string>(index4);
                List<string> list4 = obj.list;
                int index5 = num5;
                int num6 = index5 + 1;
                string element3 = list4.GetElement<string>(index5);
                List<string> list5 = obj.list;
                int index6 = num6;
                int num7 = index6 + 1;
                string element4 = list5.GetElement<string>(index6);
                List<string> list6 = obj.list;
                int index7 = num7;
                int num8 = index7 + 1;
                bool result2;
                bool flag = bool.TryParse(list6.GetElement<string>(index7), out result2) && result2;
                List<string> list7 = obj.list;
                int index8 = num8;
                int num9 = index8 + 1;
                string element5 = list7.GetElement<string>(index8);
                List<string> list8 = obj.list;
                int index9 = num9;
                int num10 = index9 + 1;
                string element6 = list8.GetElement<string>(index9);
                Dictionary<int, ActionItemInfo> eventItemList = this.EventItemList;
                int index10 = result1;
                ActionItemInfo actionItemInfo1 = new ActionItemInfo();
                actionItemInfo1.assetbundleInfo = new AssetBundleInfo(element1, element2, element3, element4);
                actionItemInfo1.existsAnimation = flag;
                ActionItemInfo actionItemInfo2 = actionItemInfo1;
                AssetBundleInfo assetBundleInfo1 = (AssetBundleInfo) null;
                assetBundleInfo1.assetbundle = (__Null) element5;
                assetBundleInfo1.asset = (__Null) element6;
                AssetBundleInfo assetBundleInfo2 = assetBundleInfo1;
                actionItemInfo2.animeAssetBundle = assetBundleInfo2;
                ActionItemInfo actionItemInfo3 = actionItemInfo1;
                eventItemList[index10] = actionItemInfo3;
              }
            }
          }
        }
      }

      private void LoadFoodEventItemList(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.EventItemList, false);
        nameListFromPath.Sort();
        if (nameListFromPath.Count == 0)
          return;
        foreach (string assetBundleName in nameListFromPath)
        {
          foreach (FoodEventItemData allAsset in AssetBundleManager.LoadAllAsset(assetBundleName, typeof (FoodEventItemData), (string) null).GetAllAssets<FoodEventItemData>())
          {
            foreach (FoodEventItemData.Param obj in allAsset.param)
            {
              Dictionary<int, int> dictionary1;
              if (!this.FoodEventItemList.TryGetValue(obj.CategoryID, out dictionary1))
              {
                Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
                this.FoodEventItemList[obj.CategoryID] = dictionary2;
                dictionary1 = dictionary2;
              }
              dictionary1[obj.ItemID] = obj.EventItemID;
              Dictionary<int, int> dictionary3;
              if (!this.FoodDateEventItemList.TryGetValue(obj.CategoryID, out dictionary3))
              {
                Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
                this.FoodDateEventItemList[obj.CategoryID] = dictionary2;
                dictionary3 = dictionary2;
              }
              int result;
              if (int.TryParse(obj.DateEventItemID, out result))
                dictionary3[obj.ItemID] = result;
            }
          }
        }
      }

      private void LoadNavMeshSourceTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.MapList, false);
        nameListFromPath.Sort();
        foreach (string str in nameListFromPath)
        {
          string file = str;
          foreach (PointPrefabData allAsset in AssetBundleManager.LoadAllAsset(file, typeof (PointPrefabData), (string) null).GetAllAssets<PointPrefabData>())
          {
            foreach (PointPrefabData.Param obj in allAsset.param)
            {
              if (obj != null)
                this.NavMeshSourceList[obj.MapID] = new AssetBundleInfo(obj.Name, obj.AssetBundle, obj.Asset, obj.Manifest);
            }
          }
          if (!MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == file)))
            MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>(file, string.Empty));
        }
      }

      private void LoadActionPointListTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.ActionPointPrefabList, false);
        nameListFromPath.Sort();
        foreach (string str in nameListFromPath)
        {
          string file = str;
          foreach (PointPrefabData allAsset in AssetBundleManager.LoadAllAsset(file, typeof (PointPrefabData), (string) null).GetAllAssets<PointPrefabData>())
          {
            foreach (PointPrefabData.Param obj in allAsset.param)
            {
              if (obj != null)
                this.ActionPointGroupTable[obj.MapID] = new AssetBundleInfo(obj.Name, obj.AssetBundle, obj.Asset, obj.Manifest);
            }
          }
          if (!MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == file)))
            MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>(file, string.Empty));
        }
      }

      private void LoadBasePointListTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.BasePointPrefabList, false);
        nameListFromPath.Sort();
        foreach (string str in nameListFromPath)
        {
          string file = str;
          foreach (PointPrefabData allAsset in AssetBundleManager.LoadAllAsset(file, typeof (PointPrefabData), (string) null).GetAllAssets<PointPrefabData>())
          {
            foreach (PointPrefabData.Param obj in allAsset.param)
            {
              if (obj != null)
                this.BasePointGroupTable[obj.MapID] = new AssetBundleInfo(obj.Name, obj.AssetBundle, obj.Asset, obj.Manifest);
            }
          }
          if (!MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == file)))
            MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>(file, string.Empty));
        }
      }

      private void LoadDevicePointListTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.DevicePointPrefabList, false);
        nameListFromPath.Sort();
        foreach (string str in nameListFromPath)
        {
          string file = str;
          foreach (PointPrefabData allAsset in AssetBundleManager.LoadAllAsset(file, typeof (PointPrefabData), (string) null).GetAllAssets<PointPrefabData>())
          {
            foreach (PointPrefabData.Param obj in allAsset.param)
            {
              if (obj != null)
                this.DevicePointGroupTable[obj.MapID] = new AssetBundleInfo(obj.Name, obj.AssetBundle, obj.Asset, obj.Manifest);
            }
          }
          if (!MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == file)))
            MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>(file, string.Empty));
        }
      }

      private void LoadFarmPointListTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.FarmPointPrefabList, false);
        nameListFromPath.Sort();
        foreach (string str in nameListFromPath)
        {
          string file = str;
          foreach (PointPrefabData allAsset in AssetBundleManager.LoadAllAsset(file, typeof (PointPrefabData), (string) null).GetAllAssets<PointPrefabData>())
          {
            foreach (PointPrefabData.Param obj in allAsset.param)
            {
              if (obj != null)
                this.HarvestPointGroupTable[obj.MapID] = new AssetBundleInfo(obj.Name, obj.AssetBundle, obj.Asset, obj.Manifest);
            }
          }
          if (!MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == file)))
            MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>(file, string.Empty));
        }
      }

      private void LoadShipPointListTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.ShipPointPrefabList, false);
        nameListFromPath.Sort();
        foreach (string str in nameListFromPath)
        {
          string file = str;
          foreach (PointPrefabData allAsset in AssetBundleManager.LoadAllAsset(file, typeof (PointPrefabData), (string) null).GetAllAssets<PointPrefabData>())
          {
            foreach (PointPrefabData.Param obj in allAsset.param)
            {
              if (obj != null)
                this.ShipPointGroupTable[obj.MapID] = new AssetBundleInfo(obj.Name, obj.AssetBundle, obj.Asset, obj.Manifest);
            }
          }
          if (!MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == file)))
            MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>(file, string.Empty));
        }
      }

      private void LoadLightSwitchPointListTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.LightSwitchPointList, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        foreach (string str in nameListFromPath)
        {
          if (!str.IsNullOrEmpty())
          {
            string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
            ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
              {
                List<string> list = excelData.list[index1].list;
                if (!list.IsNullOrEmpty<string>())
                {
                  int num1 = 0;
                  List<string> source1 = list;
                  int index2 = num1;
                  int num2 = index2 + 1;
                  int result;
                  if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result))
                  {
                    AssetBundleInfo assetInfo;
                    ref AssetBundleInfo local = ref assetInfo;
                    List<string> source2 = list;
                    int index3 = num2;
                    int num3 = index3 + 1;
                    string element1 = source2.GetElement<string>(index3);
                    List<string> source3 = list;
                    int index4 = num3;
                    int num4 = index4 + 1;
                    string element2 = source3.GetElement<string>(index4);
                    List<string> source4 = list;
                    int index5 = num4;
                    int num5 = index5 + 1;
                    string element3 = source4.GetElement<string>(index5);
                    List<string> source5 = list;
                    int index6 = num5;
                    int num6 = index6 + 1;
                    string element4 = source5.GetElement<string>(index6);
                    ((AssetBundleInfo) ref local).\u002Ector(element1, element2, element3, element4);
                    if (result == 0)
                      this.LoadLightSwitchPointPrefabInfo(assetInfo);
                  }
                }
              }
            }
          }
        }
      }

      private void LoadLightSwitchPointPrefabInfo(AssetBundleInfo assetInfo)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(assetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                AssetBundleInfo assetBundleInfo;
                ref AssetBundleInfo local = ref assetBundleInfo;
                string empty = string.Empty;
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                string element1 = source3.GetElement<string>(index4);
                List<string> source4 = list;
                int index5 = num4;
                int num5 = index5 + 1;
                string element2 = source4.GetElement<string>(index5);
                List<string> source5 = list;
                int index6 = num5;
                int num6 = index6 + 1;
                string element3 = source5.GetElement<string>(index6);
                ((AssetBundleInfo) ref local).\u002Ector(empty, element1, element2, element3);
                if (!((string) assetBundleInfo.assetbundle).IsNullOrEmpty() && !((string) assetBundleInfo.asset).IsNullOrEmpty() && !((string) assetBundleInfo.manifest).IsNullOrEmpty())
                {
                  Dictionary<int, AssetBundleInfo> dictionary1;
                  if (!this.LightSwitchPointGroupTable.TryGetValue(result1, out dictionary1) || dictionary1 == null)
                  {
                    Dictionary<int, AssetBundleInfo> dictionary2 = new Dictionary<int, AssetBundleInfo>();
                    this.LightSwitchPointGroupTable[result1] = dictionary2;
                    dictionary1 = dictionary2;
                  }
                  dictionary1[result2] = assetBundleInfo;
                }
              }
            }
          }
        }
      }

      private void LoadPlayerActionPointInfo(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.PlayerActionPointList, false);
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData1 = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData1, (Object) null))
          {
            for (int index2 = 1; index2 < excelData1.MaxCell; ++index2)
            {
              ExcelData.Param obj = excelData1.list[index2];
              int num1 = 0;
              List<string> list1 = obj.list;
              int index3 = num1;
              int num2 = index3 + 1;
              int result;
              if (int.TryParse(list1.GetElement<string>(index3), out result))
              {
                List<string> list2 = obj.list;
                int index4 = num2;
                int num3 = index4 + 1;
                string element1 = list2.GetElement<string>(index4);
                List<string> list3 = obj.list;
                int index5 = num3;
                int num4 = index5 + 1;
                string element2 = list3.GetElement<string>(index5);
                List<string> list4 = obj.list;
                int index6 = num4;
                int num5 = index6 + 1;
                string element3 = list4.GetElement<string>(index6);
                Debug.Log((object) string.Format("{0} 読み込み", (object) element2));
                ExcelData excelData2 = AssetUtility.LoadAsset<ExcelData>(element1, element2, element3);
                Dictionary<int, List<ActionPointInfo>> table;
                if (!this.PlayerActionPointInfoTable.TryGetValue(result, out table))
                {
                  Dictionary<int, List<ActionPointInfo>> dictionary = new Dictionary<int, List<ActionPointInfo>>();
                  this.PlayerActionPointInfoTable[result] = dictionary;
                  table = dictionary;
                }
                this.LoadActionPointInfoListSheet(excelData2, table);
              }
            }
          }
        }
      }

      private void LoadAgentActionPointInfo(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.AgentActionPointList, false);
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData1 = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData1, (Object) null))
          {
            for (int index2 = 1; index2 < excelData1.MaxCell; ++index2)
            {
              ExcelData.Param obj = excelData1.list[index2];
              int num1 = 0;
              List<string> list1 = obj.list;
              int index3 = num1;
              int num2 = index3 + 1;
              int result;
              if (int.TryParse(list1.GetElement<string>(index3), out result))
              {
                List<string> list2 = obj.list;
                int index4 = num2;
                int num3 = index4 + 1;
                string element1 = list2.GetElement<string>(index4);
                List<string> list3 = obj.list;
                int index5 = num3;
                int num4 = index5 + 1;
                string element2 = list3.GetElement<string>(index5);
                List<string> list4 = obj.list;
                int index6 = num4;
                int num5 = index6 + 1;
                string element3 = list4.GetElement<string>(index6);
                Debug.Log((object) string.Format("{0} 読み込み", (object) element2));
                ExcelData excelData2 = AssetUtility.LoadAsset<ExcelData>(element1, element2, element3);
                Dictionary<int, List<ActionPointInfo>> table;
                if (!this.AgentActionPointInfoTable.TryGetValue(result, out table))
                {
                  Dictionary<int, List<ActionPointInfo>> dictionary = new Dictionary<int, List<ActionPointInfo>>();
                  this.AgentActionPointInfoTable[result] = dictionary;
                  table = dictionary;
                }
                this.LoadActionPointInfoListSheet(excelData2, table);
              }
            }
          }
        }
      }

      private void LoadActionPointInfoListSheet(
        ExcelData excelData,
        Dictionary<int, List<ActionPointInfo>> table)
      {
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          ExcelData.Param obj = excelData.list[index1];
          int num1 = 0;
          List<string> list1 = obj.list;
          int index2 = num1;
          int num2 = index2 + 1;
          int result1;
          if (int.TryParse(list1.GetElement<string>(index2), out result1))
          {
            List<string> list2 = obj.list;
            int index3 = num2;
            int num3 = index3 + 1;
            string element1 = list2.GetElement<string>(index3);
            List<string> list3 = obj.list;
            int index4 = num3;
            int num4 = index4 + 1;
            int result2;
            if (int.TryParse(list3.GetElement<string>(index4), out result2))
            {
              List<string> list4 = obj.list;
              int index5 = num4;
              int num5 = index5 + 1;
              int result3;
              if (int.TryParse(list4.GetElement<string>(index5), out result3))
              {
                List<string> list5 = obj.list;
                int index6 = num5;
                int num6 = index6 + 1;
                int result4;
                if (int.TryParse(list5.GetElement<string>(index6), out result4))
                  ;
                List<string> list6 = obj.list;
                int index7 = num6;
                int num7 = index7 + 1;
                bool result5;
                if (bool.TryParse(list6.GetElement<string>(index7), out result5))
                {
                  List<string> list7 = obj.list;
                  int index8 = num7;
                  int num8 = index8 + 1;
                  int result6;
                  if (!int.TryParse(list7.GetElement<string>(index8), out result6))
                    result6 = -1;
                  List<string> list8 = obj.list;
                  int index9 = num8;
                  int num9 = index9 + 1;
                  int result7;
                  if (int.TryParse(list8.GetElement<string>(index9), out result7))
                  {
                    List<string> list9 = obj.list;
                    int index10 = num9;
                    int num10 = index10 + 1;
                    string element2 = list9.GetElement<string>(index10);
                    List<string> list10 = obj.list;
                    int index11 = num10;
                    int num11 = index11 + 1;
                    string element3 = list10.GetElement<string>(index11);
                    List<string> list11 = obj.list;
                    int index12 = num11;
                    int num12 = index12 + 1;
                    string element4 = list11.GetElement<string>(index12);
                    List<string> list12 = obj.list;
                    int index13 = num12;
                    int num13 = index13 + 1;
                    int result8;
                    if (!int.TryParse(list12.GetElement<string>(index13), out result8))
                      result8 = -1;
                    List<string> list13 = obj.list;
                    int index14 = num13;
                    int num14 = index14 + 1;
                    int result9;
                    if (!int.TryParse(list13.GetElement<string>(index14), out result9))
                      result9 = -1;
                    List<string> list14 = obj.list;
                    int index15 = num14;
                    int num15 = index15 + 1;
                    int result10;
                    if (!int.TryParse(list14.GetElement<string>(index15), out result10))
                      result10 = -1;
                    AIProject.EventType eventType = AIProject.Definitions.Action.EventTypeTable[result2];
                    List<ActionPointInfo> actionPointInfoList1;
                    if (!table.TryGetValue(result1, out actionPointInfoList1))
                    {
                      List<ActionPointInfo> actionPointInfoList2 = new List<ActionPointInfo>();
                      table[result1] = actionPointInfoList2;
                      actionPointInfoList1 = actionPointInfoList2;
                    }
                    actionPointInfoList1.Add(new ActionPointInfo()
                    {
                      actionName = element1,
                      pointID = result1,
                      eventID = result2,
                      eventTypeMask = eventType,
                      poseID = result3,
                      datePoseID = result4,
                      isTalkable = result5,
                      iconID = result6,
                      cameraID = result7,
                      baseNullName = element2,
                      recoveryNullName = element3,
                      labelNullName = element4,
                      searchAreaID = result8,
                      gradeValue = result9,
                      doorOpenType = result10
                    });
                  }
                }
              }
            }
          }
        }
      }

      private void LoadPlayerDateActionPointInfo(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.PlayerActionPointList, false);
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData1 = AssetUtility.LoadAsset<ExcelData>(str, string.Format("date_{0}", (object) withoutExtension), string.Empty);
          if (!Object.op_Equality((Object) excelData1, (Object) null))
          {
            for (int index2 = 1; index2 < excelData1.MaxCell; ++index2)
            {
              ExcelData.Param obj1 = excelData1.list[index2];
              int num1 = 0;
              List<string> list1 = obj1.list;
              int index3 = num1;
              int num2 = index3 + 1;
              int result1;
              if (int.TryParse(list1.GetElement<string>(index3), out result1))
              {
                List<string> list2 = obj1.list;
                int index4 = num2;
                int num3 = index4 + 1;
                int result2;
                if (int.TryParse(list2.GetElement<string>(index4), out result2))
                {
                  List<string> list3 = obj1.list;
                  int index5 = num3;
                  int num4 = index5 + 1;
                  string element1 = list3.GetElement<string>(index5);
                  List<string> list4 = obj1.list;
                  int index6 = num4;
                  int num5 = index6 + 1;
                  string element2 = list4.GetElement<string>(index6);
                  List<string> list5 = obj1.list;
                  int index7 = num5;
                  int num6 = index7 + 1;
                  string element3 = list5.GetElement<string>(index7);
                  Debug.Log((object) string.Format("{0} 読み込み", (object) element2));
                  ExcelData excelData2 = AssetUtility.LoadAsset<ExcelData>(element1, element2, element3);
                  Dictionary<int, Dictionary<int, List<DateActionPointInfo>>> dictionary1;
                  if (!this.PlayerDateActionPointInfoTable.TryGetValue(result1, out dictionary1))
                  {
                    Dictionary<int, Dictionary<int, List<DateActionPointInfo>>> dictionary2 = new Dictionary<int, Dictionary<int, List<DateActionPointInfo>>>();
                    this.PlayerDateActionPointInfoTable[result1] = dictionary2;
                    dictionary1 = dictionary2;
                  }
                  Dictionary<int, List<DateActionPointInfo>> dictionary3;
                  if (!dictionary1.TryGetValue(result2, out dictionary3))
                  {
                    Dictionary<int, List<DateActionPointInfo>> dictionary2 = new Dictionary<int, List<DateActionPointInfo>>();
                    dictionary1[result2] = dictionary2;
                    dictionary3 = dictionary2;
                  }
                  for (int index8 = 1; index8 < excelData2.MaxCell; ++index8)
                  {
                    ExcelData.Param obj2 = excelData2.list[index8];
                    int num7 = 0;
                    List<string> list6 = obj2.list;
                    int index9 = num7;
                    int num8 = index9 + 1;
                    int result3;
                    if (int.TryParse(list6.GetElement<string>(index9), out result3))
                    {
                      List<string> list7 = obj2.list;
                      int index10 = num8;
                      int num9 = index10 + 1;
                      string element4 = list7.GetElement<string>(index10);
                      List<string> list8 = obj2.list;
                      int index11 = num9;
                      int num10 = index11 + 1;
                      int result4;
                      if (int.TryParse(list8.GetElement<string>(index11), out result4))
                      {
                        List<string> list9 = obj2.list;
                        int index12 = num10;
                        int num11 = index12 + 1;
                        int result5;
                        if (int.TryParse(list9.GetElement<string>(index12), out result5))
                        {
                          List<string> list10 = obj2.list;
                          int index13 = num11;
                          int num12 = index13 + 1;
                          int result6;
                          if (int.TryParse(list10.GetElement<string>(index13), out result6))
                          {
                            List<string> list11 = obj2.list;
                            int index14 = num12;
                            int num13 = index14 + 1;
                            bool result7;
                            if (bool.TryParse(list11.GetElement<string>(index14), out result7))
                              ;
                            List<string> list12 = obj2.list;
                            int index15 = num13;
                            int num14 = index15 + 1;
                            int result8;
                            if (!int.TryParse(list12.GetElement<string>(index15), out result8))
                              result8 = -1;
                            List<string> list13 = obj2.list;
                            int index16 = num14;
                            int num15 = index16 + 1;
                            int result9;
                            if (!int.TryParse(list13.GetElement<string>(index16), out result9))
                              result9 = -1;
                            List<string> list14 = obj2.list;
                            int index17 = num15;
                            int num16 = index17 + 1;
                            string element5 = list14.GetElement<string>(index17);
                            List<string> list15 = obj2.list;
                            int index18 = num16;
                            int num17 = index18 + 1;
                            string element6 = list15.GetElement<string>(index18);
                            List<string> list16 = obj2.list;
                            int index19 = num17;
                            int num18 = index19 + 1;
                            string element7 = list16.GetElement<string>(index19);
                            List<string> list17 = obj2.list;
                            int index20 = num18;
                            int num19 = index20 + 1;
                            string element8 = list17.GetElement<string>(index20);
                            List<string> list18 = obj2.list;
                            int index21 = num19;
                            int num20 = index21 + 1;
                            string element9 = list18.GetElement<string>(index21);
                            List<string> list19 = obj2.list;
                            int index22 = num20;
                            int num21 = index22 + 1;
                            int result10;
                            if (!int.TryParse(list19.GetElement<string>(index22), out result10))
                              result10 = -1;
                            List<string> list20 = obj2.list;
                            int index23 = num21;
                            int num22 = index23 + 1;
                            int result11;
                            if (!int.TryParse(list20.GetElement<string>(index23), out result11))
                              result11 = -1;
                            List<string> list21 = obj2.list;
                            int index24 = num22;
                            int num23 = index24 + 1;
                            int result12;
                            if (!int.TryParse(list21.GetElement<string>(index24), out result12))
                              result12 = -1;
                            AIProject.EventType eventType = AIProject.Definitions.Action.EventTypeTable[result4];
                            List<DateActionPointInfo> dateActionPointInfoList1;
                            if (!dictionary3.TryGetValue(result3, out dateActionPointInfoList1))
                            {
                              List<DateActionPointInfo> dateActionPointInfoList2 = new List<DateActionPointInfo>();
                              dictionary3[result3] = dateActionPointInfoList2;
                              dateActionPointInfoList1 = dateActionPointInfoList2;
                            }
                            dateActionPointInfoList1.Add(new DateActionPointInfo()
                            {
                              actionName = element4,
                              pointID = result3,
                              eventID = result4,
                              eventTypeMask = eventType,
                              poseIDA = result5,
                              poseIDB = result6,
                              isTalkable = result7,
                              iconID = result8,
                              cameraID = result9,
                              baseNullNameA = element5,
                              baseNullNameB = element6,
                              recoveryNullNameA = element7,
                              recoveryNullNameB = element8,
                              labelNullName = element9,
                              searchAreaID = result10,
                              gradeValue = result11,
                              doorOpenType = result12
                            });
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadAgentDateActionPointInfo(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.AgentActionPointList, false);
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          ExcelData excelData1 = AssetUtility.LoadAsset<ExcelData>(str, string.Format("date_{0}", (object) withoutExtension), string.Empty);
          if (!Object.op_Equality((Object) excelData1, (Object) null))
          {
            for (int index2 = 1; index2 < excelData1.MaxCell; ++index2)
            {
              ExcelData.Param obj1 = excelData1.list[index2];
              int num1 = 0;
              List<string> list1 = obj1.list;
              int index3 = num1;
              int num2 = index3 + 1;
              int result1;
              if (int.TryParse(list1.GetElement<string>(index3), out result1))
              {
                List<string> list2 = obj1.list;
                int index4 = num2;
                int num3 = index4 + 1;
                string element1 = list2.GetElement<string>(index4);
                List<string> list3 = obj1.list;
                int index5 = num3;
                int num4 = index5 + 1;
                string element2 = list3.GetElement<string>(index5);
                List<string> list4 = obj1.list;
                int index6 = num4;
                int num5 = index6 + 1;
                string element3 = list4.GetElement<string>(index6);
                Debug.Log((object) string.Format("{0} 読み込み", (object) element2));
                ExcelData excelData2 = AssetUtility.LoadAsset<ExcelData>(element1, element2, element3);
                Dictionary<int, List<DateActionPointInfo>> dictionary1;
                if (!this.AgentDateActionPointInfoTable.TryGetValue(result1, out dictionary1))
                {
                  Dictionary<int, List<DateActionPointInfo>> dictionary2 = new Dictionary<int, List<DateActionPointInfo>>();
                  this.AgentDateActionPointInfoTable[result1] = dictionary2;
                  dictionary1 = dictionary2;
                }
                for (int index7 = 1; index7 < excelData2.MaxCell; ++index7)
                {
                  ExcelData.Param obj2 = excelData2.list[index7];
                  int num6 = 0;
                  List<string> list5 = obj2.list;
                  int index8 = num6;
                  int num7 = index8 + 1;
                  int result2;
                  if (int.TryParse(list5.GetElement<string>(index8), out result2))
                  {
                    List<string> list6 = obj2.list;
                    int index9 = num7;
                    int num8 = index9 + 1;
                    string element4 = list6.GetElement<string>(index9);
                    List<string> list7 = obj2.list;
                    int index10 = num8;
                    int num9 = index10 + 1;
                    int result3;
                    if (int.TryParse(list7.GetElement<string>(index10), out result3))
                    {
                      List<string> list8 = obj2.list;
                      int index11 = num9;
                      int num10 = index11 + 1;
                      int result4;
                      if (int.TryParse(list8.GetElement<string>(index11), out result4))
                      {
                        List<string> list9 = obj2.list;
                        int index12 = num10;
                        int num11 = index12 + 1;
                        int result5;
                        if (int.TryParse(list9.GetElement<string>(index12), out result5))
                        {
                          List<string> list10 = obj2.list;
                          int index13 = num11;
                          int num12 = index13 + 1;
                          bool result6;
                          if (!bool.TryParse(list10.GetElement<string>(index13), out result6))
                            result6 = false;
                          List<string> list11 = obj2.list;
                          int index14 = num12;
                          int num13 = index14 + 1;
                          int result7;
                          if (!int.TryParse(list11.GetElement<string>(index14), out result7))
                            result7 = -1;
                          List<string> list12 = obj2.list;
                          int index15 = num13;
                          int num14 = index15 + 1;
                          int result8;
                          if (!int.TryParse(list12.GetElement<string>(index15), out result8))
                            result8 = 1;
                          List<string> list13 = obj2.list;
                          int index16 = num14;
                          int num15 = index16 + 1;
                          string element5 = list13.GetElement<string>(index16);
                          List<string> list14 = obj2.list;
                          int index17 = num15;
                          int num16 = index17 + 1;
                          string element6 = list14.GetElement<string>(index17);
                          List<string> list15 = obj2.list;
                          int index18 = num16;
                          int num17 = index18 + 1;
                          string element7 = list15.GetElement<string>(index18);
                          List<string> list16 = obj2.list;
                          int index19 = num17;
                          int num18 = index19 + 1;
                          string element8 = list16.GetElement<string>(index19);
                          List<string> list17 = obj2.list;
                          int index20 = num18;
                          int num19 = index20 + 1;
                          string element9 = list17.GetElement<string>(index20);
                          List<string> list18 = obj2.list;
                          int index21 = num19;
                          int num20 = index21 + 1;
                          int result9;
                          if (!int.TryParse(list18.GetElement<string>(index21), out result9))
                            result9 = -1;
                          List<string> list19 = obj2.list;
                          int index22 = num20;
                          int num21 = index22 + 1;
                          int result10;
                          if (!int.TryParse(list19.GetElement<string>(index22), out result10))
                            result10 = -1;
                          List<string> list20 = obj2.list;
                          int index23 = num21;
                          int num22 = index23 + 1;
                          int result11;
                          if (!int.TryParse(list20.GetElement<string>(index23), out result11))
                            result11 = -1;
                          AIProject.EventType eventType = AIProject.Definitions.Action.EventTypeTable[result3];
                          List<DateActionPointInfo> dateActionPointInfoList1;
                          if (!dictionary1.TryGetValue(result2, out dateActionPointInfoList1))
                          {
                            List<DateActionPointInfo> dateActionPointInfoList2 = new List<DateActionPointInfo>();
                            dictionary1[result2] = dateActionPointInfoList2;
                            dateActionPointInfoList1 = dateActionPointInfoList2;
                          }
                          dateActionPointInfoList1.Add(new DateActionPointInfo()
                          {
                            actionName = element4,
                            pointID = result2,
                            eventID = result3,
                            eventTypeMask = eventType,
                            poseIDA = result4,
                            poseIDB = result5,
                            isTalkable = result6,
                            iconID = result7,
                            cameraID = result8,
                            baseNullNameA = element5,
                            baseNullNameB = element6,
                            recoveryNullNameA = element7,
                            recoveryNullNameB = element8,
                            labelNullName = element9,
                            searchAreaID = result9,
                            gradeValue = result10,
                            doorOpenType = result11
                          });
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadMerchantPoint(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.MerchantActionPointList, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str1 = nameListFromPath[index1];
          string withoutExtension;
          string str2 = withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str1);
          if (AssetBundleCheck.IsFile(str1, str2))
          {
            ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str1, str2, string.Empty);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
              {
                List<string> list = excelData.list[index2].list;
                int result;
                if (!list.IsNullOrEmpty<string>() && int.TryParse(list.GetElement<string>(0) ?? string.Empty, out result))
                {
                  int num1 = 2;
                  List<string> source1 = list;
                  int index3 = num1;
                  int num2 = index3 + 1;
                  string element1 = source1.GetElement<string>(index3);
                  List<string> source2 = list;
                  int index4 = num2;
                  int num3 = index4 + 1;
                  string element2 = source2.GetElement<string>(index4);
                  if (!element1.IsNullOrEmpty() && !element2.IsNullOrEmpty())
                  {
                    switch (result)
                    {
                      case 0:
                        this.LoadMerchantPointPrefabInfo(list, withoutExtension);
                        continue;
                      case 1:
                        this.LoadMerchantPointInfo(list, withoutExtension);
                        continue;
                      case 2:
                        this.LoadMerchantActionPointInfo(list, withoutExtension);
                        continue;
                      default:
                        continue;
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadMerchantPointPrefabInfo(List<string> address, string id)
      {
        int num1 = 2;
        List<string> source1 = address;
        int index1 = num1;
        int num2 = index1 + 1;
        string assetbundleName = source1.GetElement<string>(index1) ?? string.Empty;
        List<string> source2 = address;
        int index2 = num2;
        int num3 = index2 + 1;
        string assetName = source2.GetElement<string>(index2) ?? string.Empty;
        List<string> source3 = address;
        int index3 = num3;
        int num4 = index3 + 1;
        string manifestName = source3.GetElement<string>(index3) ?? string.Empty;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(assetbundleName, assetName, manifestName);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index4 = 1; index4 < excelData.MaxCell; ++index4)
        {
          List<string> list = excelData.list[index4].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num5 = 0;
            List<string> source4 = list;
            int index5 = num5;
            int num6 = index5 + 1;
            string str = source4.GetElement<string>(index5) ?? string.Empty;
            List<string> source5 = list;
            int index6 = num6;
            int num7 = index6 + 1;
            int result1;
            if (int.TryParse(source5.GetElement<string>(index6) ?? string.Empty, out result1))
            {
              List<string> source6 = list;
              int index7 = num7;
              int num8 = index7 + 1;
              int result2;
              if (int.TryParse(source6.GetElement<string>(index7) ?? string.Empty, out result2))
              {
                List<string> source7 = list;
                int index8 = num8;
                int num9 = index8 + 1;
                string self1 = source7.GetElement<string>(index8) ?? string.Empty;
                List<string> source8 = list;
                int index9 = num9;
                int num10 = index9 + 1;
                string self2 = source8.GetElement<string>(index9) ?? string.Empty;
                List<string> source9 = list;
                int index10 = num10;
                int num11 = index10 + 1;
                string self3 = source9.GetElement<string>(index10) ?? string.Empty;
                if (!self1.IsNullOrEmpty() && !self2.IsNullOrEmpty() && !self3.IsNullOrEmpty())
                {
                  if (!this.MerchantPointGroupTable.ContainsKey(result1))
                    this.MerchantPointGroupTable[result1] = new Dictionary<int, AssetBundleInfo>();
                  this.MerchantPointGroupTable[result1][result2] = new AssetBundleInfo(str, self1, self2, self3);
                }
              }
            }
          }
        }
      }

      private void LoadMerchantPointInfo(List<string> address, string id)
      {
        int num1 = 2;
        List<string> source1 = address;
        int index1 = num1;
        int num2 = index1 + 1;
        string assetbundleName = source1.GetElement<string>(index1) ?? string.Empty;
        List<string> source2 = address;
        int index2 = num2;
        int num3 = index2 + 1;
        string assetName = source2.GetElement<string>(index2) ?? string.Empty;
        List<string> source3 = address;
        int index3 = num3;
        int num4 = index3 + 1;
        string manifestName = source3.GetElement<string>(index3) ?? string.Empty;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(assetbundleName, assetName, manifestName);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index4 = 1; index4 < excelData.MaxCell; ++index4)
        {
          List<string> list = excelData.list[index4].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num5 = 0;
            List<string> source4 = list;
            int index5 = num5;
            int num6 = index5 + 1;
            int result;
            if (int.TryParse(source4.GetElement<string>(index5) ?? string.Empty, out result))
            {
              List<string> source5 = list;
              int index6 = num6;
              int num7 = index6 + 1;
              string str1 = source5.GetElement<string>(index6) ?? string.Empty;
              List<string> source6 = list;
              int index7 = num7;
              int num8 = index7 + 1;
              string str2 = source6.GetElement<string>(index7) ?? string.Empty;
              if (!str1.IsNullOrEmpty() && !str2.IsNullOrEmpty() && !Object.op_Equality((Object) AssetUtility.LoadAsset<ExcelData>(str1, str2, string.Empty), (Object) null))
              {
                Dictionary<int, List<MerchantPointInfo>> dic;
                if (!this.MerchantPointInfoTable.TryGetValue(result, out dic))
                {
                  Dictionary<int, List<MerchantPointInfo>> dictionary = new Dictionary<int, List<MerchantPointInfo>>();
                  this.MerchantPointInfoTable[result] = dictionary;
                  dic = dictionary;
                }
                this.LoadMerchantPointInfoListSheet(list, id, dic);
              }
            }
          }
        }
      }

      private void LoadMerchantPointInfoListSheet(
        List<string> address,
        string id,
        Dictionary<int, List<MerchantPointInfo>> dic)
      {
        int num1 = 1;
        List<string> source1 = address;
        int index1 = num1;
        int num2 = index1 + 1;
        string assetbundleName = source1.GetElement<string>(index1) ?? string.Empty;
        List<string> source2 = address;
        int index2 = num2;
        int num3 = index2 + 1;
        string assetName = source2.GetElement<string>(index2) ?? string.Empty;
        List<string> source3 = address;
        int index3 = num3;
        int num4 = index3 + 1;
        string manifestName = source3.GetElement<string>(index3) ?? string.Empty;
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(assetbundleName, assetName, manifestName);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index4 = 1; index4 < excelData.MaxCell; ++index4)
        {
          List<string> list = excelData.list[index4].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num5 = 0;
            List<string> source4 = list;
            int index5 = num5;
            int num6 = index5 + 1;
            int result1;
            if (int.TryParse(source4.GetElement<string>(index5) ?? string.Empty, out result1))
            {
              List<string> source5 = list;
              int index6 = num6;
              int num7 = index6 + 1;
              string str1 = source5.GetElement<string>(index6) ?? string.Empty;
              List<string> source6 = list;
              int index7 = num7;
              int num8 = index7 + 1;
              int result2;
              if (int.TryParse(source6.GetElement<string>(index7) ?? string.Empty, out result2))
              {
                List<string> source7 = list;
                int index8 = num8;
                int num9 = index8 + 1;
                int result3;
                if (int.TryParse(source7.GetElement<string>(index8) ?? string.Empty, out result3))
                {
                  List<string> source8 = list;
                  int index9 = num9;
                  int num10 = index9 + 1;
                  bool result4;
                  if (bool.TryParse(source8.GetElement<string>(index9) ?? string.Empty, out result4))
                  {
                    List<string> source9 = list;
                    int index10 = num10;
                    int num11 = index10 + 1;
                    bool result5;
                    if (bool.TryParse(source9.GetElement<string>(index10) ?? string.Empty, out result5))
                    {
                      List<string> source10 = list;
                      int index11 = num11;
                      int num12 = index11 + 1;
                      string str2 = source10.GetElement<string>(index11) ?? string.Empty;
                      List<string> source11 = list;
                      int index12 = num12;
                      int num13 = index12 + 1;
                      string str3 = source11.GetElement<string>(index12) ?? string.Empty;
                      Merchant.EventType eventType = Merchant.EventTypeTable[result2];
                      List<MerchantPointInfo> merchantPointInfoList1;
                      if (!dic.TryGetValue(result1, out merchantPointInfoList1))
                      {
                        List<MerchantPointInfo> merchantPointInfoList2 = new List<MerchantPointInfo>();
                        dic[result1] = merchantPointInfoList2;
                        merchantPointInfoList1 = merchantPointInfoList2;
                      }
                      merchantPointInfoList1.Add(new MerchantPointInfo()
                      {
                        actionName = str1,
                        pointID = result1,
                        eventID = result2,
                        eventTypeMask = eventType,
                        poseID = result3,
                        isTalkable = result4,
                        isLooking = result5,
                        baseNullName = str2,
                        recoveryNullName = str3
                      });
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadMerchantActionPointInfo(List<string> address, string id)
      {
        int num1 = 2;
        List<string> source1 = address;
        int index1 = num1;
        int num2 = index1 + 1;
        string assetbundleName1 = source1.GetElement<string>(index1) ?? string.Empty;
        List<string> source2 = address;
        int index2 = num2;
        int num3 = index2 + 1;
        string assetName1 = source2.GetElement<string>(index2) ?? string.Empty;
        List<string> source3 = address;
        int index3 = num3;
        int num4 = index3 + 1;
        string manifestName1 = source3.GetElement<string>(index3) ?? string.Empty;
        ExcelData excelData1 = AssetUtility.LoadAsset<ExcelData>(assetbundleName1, assetName1, manifestName1);
        if (Object.op_Equality((Object) excelData1, (Object) null))
          return;
        for (int index4 = 1; index4 < excelData1.MaxCell; ++index4)
        {
          List<string> list = excelData1.list[index4].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num5 = 0;
            List<string> source4 = list;
            int index5 = num5;
            int num6 = index5 + 1;
            int result;
            if (int.TryParse(source4.GetElement<string>(index5) ?? string.Empty, out result))
            {
              List<string> source5 = list;
              int index6 = num6;
              int num7 = index6 + 1;
              string assetbundleName2 = source5.GetElement<string>(index6) ?? string.Empty;
              List<string> source6 = list;
              int index7 = num7;
              int num8 = index7 + 1;
              string assetName2 = source6.GetElement<string>(index7) ?? string.Empty;
              List<string> source7 = list;
              int index8 = num8;
              int num9 = index8 + 1;
              string manifestName2 = source7.GetElement<string>(index8) ?? string.Empty;
              ExcelData excelData2 = AssetUtility.LoadAsset<ExcelData>(assetbundleName2, assetName2, manifestName2);
              if (!Object.op_Equality((Object) excelData2, (Object) null))
              {
                Dictionary<int, List<ActionPointInfo>> table;
                if (!this.MerchantActionPointInfoTable.TryGetValue(result, out table))
                {
                  Dictionary<int, List<ActionPointInfo>> dictionary = new Dictionary<int, List<ActionPointInfo>>();
                  this.MerchantActionPointInfoTable[result] = dictionary;
                  table = dictionary;
                }
                this.LoadActionPointInfoListSheet(excelData2, table);
              }
            }
          }
        }
      }

      private void LoadEventPointListTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.EventPointList, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str1 = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str1);
          if (AssetBundleCheck.IsFile(str1, withoutExtension))
          {
            ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str1, withoutExtension, string.Empty);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
              {
                List<string> list = excelData.list[index2].list;
                if (!list.IsNullOrEmpty<string>())
                {
                  int num1 = 0;
                  List<string> source1 = list;
                  int index3 = num1;
                  int num2 = index3 + 1;
                  int result;
                  if (int.TryParse(source1.GetElement<string>(index3) ?? string.Empty, out result))
                  {
                    int num3 = num2 + 1;
                    List<string> source2 = list;
                    int index4 = num3;
                    int num4 = index4 + 1;
                    string str2 = source2.GetElement<string>(index4) ?? string.Empty;
                    List<string> source3 = list;
                    int index5 = num4;
                    int num5 = index5 + 1;
                    string str3 = source3.GetElement<string>(index5) ?? string.Empty;
                    if (!str2.IsNullOrEmpty() && !str3.IsNullOrEmpty())
                    {
                      switch (result)
                      {
                        case 0:
                          this.LoadEventPointPrefabList(str2, str3);
                          continue;
                        case 1:
                          this.LoadEventPointCommandText(str2, str3);
                          continue;
                        case 2:
                          this.LoadEventDialogInfo(str2, str3);
                          continue;
                        default:
                          continue;
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadEventPointPrefabList(string sheetBundle, string sheetAsset)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(sheetBundle, sheetAsset, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            string str1 = source1.GetElement<string>(index2) ?? string.Empty;
            List<string> source2 = list;
            int index3 = num2;
            int num3 = index3 + 1;
            int result1;
            if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result1))
            {
              List<string> source3 = list;
              int index4 = num3;
              int num4 = index4 + 1;
              int result2;
              if (int.TryParse(source3.GetElement<string>(index4) ?? string.Empty, out result2))
              {
                List<string> source4 = list;
                int index5 = num4;
                int num5 = index5 + 1;
                string self1 = source4.GetElement<string>(index5) ?? string.Empty;
                List<string> source5 = list;
                int index6 = num5;
                int num6 = index6 + 1;
                string self2 = source5.GetElement<string>(index6) ?? string.Empty;
                List<string> source6 = list;
                int index7 = num6;
                int num7 = index7 + 1;
                string str2 = source6.GetElement<string>(index7) ?? string.Empty;
                if (!self1.IsNullOrEmpty() && !self2.IsNullOrEmpty())
                {
                  Dictionary<int, AssetBundleInfo> dictionary;
                  if (!this.EventPointGroupTable.TryGetValue(result1, out dictionary))
                    this.EventPointGroupTable[result1] = dictionary = new Dictionary<int, AssetBundleInfo>();
                  dictionary[result2] = new AssetBundleInfo(str1, self1, self2, str2);
                }
              }
            }
          }
        }
      }

      private void LoadEventPointCommandText(string sheetBundle, string sheetAsset)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(sheetBundle, sheetAsset, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result;
            if (int.TryParse(source.GetElement<string>(index2) ?? string.Empty, out result))
            {
              List<string> stringList;
              if (!this.EventPointCommandLabelTextTable.TryGetValue(result, out stringList) || stringList != null)
                this.EventPointCommandLabelTextTable[result] = stringList = new List<string>();
              else
                stringList.Clear();
              while (num2 < list.Count)
                stringList.Add(list.GetElement<string>(num2++) ?? string.Empty);
            }
          }
        }
      }

      private void LoadEventDialogInfo(string sheetBundle, string sheetAsset)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(sheetBundle, sheetAsset, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              int num3 = num2 + 1;
              List<string> source2 = list;
              int index3 = num3;
              int num4 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                ValueTuple<int, List<string>> valueTuple1;
                if (this.EventDialogInfoTable.TryGetValue(result1, out valueTuple1))
                {
                  if (valueTuple1.Item2 == null)
                    valueTuple1.Item2 = (__Null) new List<string>();
                  else
                    ((List<string>) valueTuple1.Item2).Clear();
                  valueTuple1.Item1 = (__Null) result2;
                }
                else
                {
                  Dictionary<int, ValueTuple<int, List<string>>> eventDialogInfoTable = this.EventDialogInfoTable;
                  int index4 = result1;
                  ((ValueTuple<int, List<string>>) ref valueTuple1).\u002Ector(result2, new List<string>());
                  ValueTuple<int, List<string>> valueTuple2 = valueTuple1;
                  eventDialogInfoTable[index4] = valueTuple2;
                }
                while (num4 < list.Count)
                  ((List<string>) valueTuple1.Item2).Add(list.GetElement<string>(num4++));
                this.EventDialogInfoTable[result1] = valueTuple1;
              }
            }
          }
        }
      }

      private void LoadStoryPointListTable(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.StoryPointList, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          if (AssetBundleCheck.IsFile(str, withoutExtension))
          {
            ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
              {
                List<string> list = excelData.list[index2].list;
                if (!list.IsNullOrEmpty<string>())
                {
                  int num1 = 0;
                  List<string> source1 = list;
                  int index3 = num1;
                  int num2 = index3 + 1;
                  int result;
                  if (int.TryParse(source1.GetElement<string>(index3) ?? string.Empty, out result))
                  {
                    int num3 = num2 + 1;
                    List<string> source2 = list;
                    int index4 = num3;
                    int num4 = index4 + 1;
                    string element1 = source2.GetElement<string>(index4);
                    List<string> source3 = list;
                    int index5 = num4;
                    int num5 = index5 + 1;
                    string element2 = source3.GetElement<string>(index5);
                    if (!element1.IsNullOrEmpty() && !element2.IsNullOrEmpty() && result == 0)
                      this.LoadStoryPointPrefabList(element1, element2);
                  }
                }
              }
            }
          }
        }
      }

      private void LoadStoryPointPrefabList(string sheetBundle, string sheetAsset)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(sheetBundle, sheetAsset, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                string element1 = source3.GetElement<string>(index4);
                List<string> source4 = list;
                int index5 = num4;
                int num5 = index5 + 1;
                string element2 = source4.GetElement<string>(index5);
                List<string> source5 = list;
                int index6 = num5;
                int num6 = index6 + 1;
                string element3 = source5.GetElement<string>(index6);
                if (!element1.IsNullOrEmpty() && !element2.IsNullOrEmpty())
                {
                  Dictionary<int, AssetBundleInfo> dictionary;
                  if (!this.StoryPointGroupTable.TryGetValue(result1, out dictionary))
                    this.StoryPointGroupTable[result1] = dictionary = new Dictionary<int, AssetBundleInfo>();
                  dictionary[result2] = new AssetBundleInfo(string.Empty, element1, element2, element3);
                }
              }
            }
          }
        }
      }

      private void LoadEventParticleList(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.EventParticleList, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str1 = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str1);
          ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str1, withoutExtension, string.Empty);
          if (!Object.op_Equality((Object) excelData, (Object) null))
          {
            for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
            {
              List<string> list = excelData.list[index2].list;
              if (!list.IsNullOrEmpty<string>())
              {
                int num1 = 0;
                List<string> source1 = list;
                int index3 = num1;
                int num2 = index3 + 1;
                int result;
                if (int.TryParse(source1.GetElement<string>(index3) ?? string.Empty, out result))
                {
                  List<string> source2 = list;
                  int index4 = num2;
                  int num3 = index4 + 1;
                  string str2 = source2.GetElement<string>(index4) ?? string.Empty;
                  List<string> source3 = list;
                  int index5 = num3;
                  int num4 = index5 + 1;
                  string str3 = source3.GetElement<string>(index5) ?? string.Empty;
                  List<string> source4 = list;
                  int index6 = num4;
                  int num5 = index6 + 1;
                  string str4 = source4.GetElement<string>(index6) ?? string.Empty;
                  List<string> source5 = list;
                  int index7 = num5;
                  int num6 = index7 + 1;
                  string str5 = source5.GetElement<string>(index7) ?? string.Empty;
                  Dictionary<int, AssetBundleInfo> eventParticleTable = this.EventParticleTable;
                  int index8 = result;
                  AssetBundleInfo assetBundleInfo1 = (AssetBundleInfo) null;
                  assetBundleInfo1.name = (__Null) str2;
                  assetBundleInfo1.assetbundle = (__Null) str3;
                  assetBundleInfo1.asset = (__Null) str4;
                  assetBundleInfo1.manifest = (__Null) str5;
                  AssetBundleInfo assetBundleInfo2 = assetBundleInfo1;
                  eventParticleTable[index8] = assetBundleInfo2;
                }
              }
            }
          }
        }
      }

      private void LoadAreaOpenStateList(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.AreaOpenStateList, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str1 = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str1);
          if (AssetBundleCheck.IsFile(str1, withoutExtension))
          {
            AssetBundleInfo info;
            ((AssetBundleInfo) ref info).\u002Ector(string.Empty, str1, withoutExtension, string.Empty);
            ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(info);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
              {
                List<string> list = excelData.list[index2].list;
                if (!list.IsNullOrEmpty<string>())
                {
                  int num1 = 0;
                  List<string> source1 = list;
                  int index3 = num1;
                  int num2 = index3 + 1;
                  int result;
                  if (int.TryParse(source1.GetElement<string>(index3) ?? string.Empty, out result))
                  {
                    int num3 = num2 + 1;
                    List<string> source2 = list;
                    int index4 = num3;
                    int num4 = index4 + 1;
                    string str2 = source2.GetElement<string>(index4) ?? string.Empty;
                    List<string> source3 = list;
                    int index5 = num4;
                    int num5 = index5 + 1;
                    string str3 = source3.GetElement<string>(index5) ?? string.Empty;
                    List<string> source4 = list;
                    int index6 = num5;
                    int num6 = index6 + 1;
                    string sheetManifest = source4.GetElement<string>(index6) ?? string.Empty;
                    if (!str2.IsNullOrEmpty() && !str3.IsNullOrEmpty())
                    {
                      switch (result)
                      {
                        case 0:
                          this.LoadAreaOpenIDList(str2, str3, sheetManifest);
                          continue;
                        case 1:
                          this.LoadAreaOpenStateObjectName(str2, str3, sheetManifest);
                          continue;
                        case 2:
                          this.LoadAreaOpenStateMapAreaLinkerList(str2, str3, sheetManifest);
                          continue;
                        default:
                          continue;
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadAreaOpenIDList(string sheetBundle, string sheetAsset, string sheetManifest)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(sheetBundle, sheetAsset, sheetManifest);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              string str = source2.GetElement<string>(index3) ?? string.Empty;
              this.AreaOpenIDTable[result] = str;
            }
          }
        }
      }

      private void LoadAreaOpenStateObjectName(
        string sheetBundle,
        string sheetAsset,
        string sheetManifest)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(sheetBundle, sheetAsset, sheetManifest);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        int key = 0;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
              key = result1;
            List<string> source2 = list;
            int index3 = num2;
            int index4 = index3 + 1;
            bool result2;
            if (bool.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
            {
              List<string> toRelease = ListPool<string>.Get();
              for (; index4 < list.Count; ++index4)
              {
                string element = list.GetElement<string>(index4);
                if (!element.IsNullOrEmpty())
                  toRelease.Add(element);
              }
              string[] strArray = new string[toRelease.Count];
              for (int index5 = 0; index5 < strArray.Length; ++index5)
                strArray[index5] = toRelease[index5];
              ListPool<string>.Release(toRelease);
              Dictionary<bool, string[]> dictionary;
              if (!this.AreaOpenStateObjectNameTable.TryGetValue(key, out dictionary))
                this.AreaOpenStateObjectNameTable[key] = dictionary = new Dictionary<bool, string[]>();
              dictionary[result2] = strArray;
            }
          }
        }
      }

      private void LoadAreaOpenStateMapAreaLinkerList(
        string sheetBundle,
        string sheetAsset,
        string sheetManifest)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(sheetBundle, sheetAsset, sheetManifest);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              int num3 = num2 + 1;
              List<string> source2 = list;
              int index3 = num3;
              int num4 = index3 + 1;
              string[] strArray = (source2.GetElement<string>(index3) ?? string.Empty).Split(Manager.Resources._separationKeywords, StringSplitOptions.RemoveEmptyEntries);
              List<int> intList = ListPool<int>.Get();
              if (!((IList<string>) strArray).IsNullOrEmpty<string>())
              {
                foreach (string str in strArray)
                {
                  int result2;
                  if (!str.IsNullOrEmpty() && int.TryParse(str, out result2))
                    intList.Add(result2);
                }
              }
              int[] numArray = new int[intList.Count];
              if (!intList.IsNullOrEmpty<int>())
              {
                for (int index4 = 0; index4 < numArray.Length; ++index4)
                  numArray[index4] = intList.GetElement<int>(index4);
              }
              this.AreaOpenStateMapAreaLinkerTable[result1] = numArray;
              ListPool<int>.Release(intList);
            }
          }
        }
      }

      private void LoadTimeRelationInfoList(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.TimeRelationInfoList, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str1 = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str1);
          if (AssetBundleCheck.IsFile(str1, withoutExtension))
          {
            AssetBundleInfo info;
            ((AssetBundleInfo) ref info).\u002Ector(string.Empty, str1, withoutExtension, string.Empty);
            ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(info);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
              {
                List<string> list = excelData.list[index2].list;
                if (!list.IsNullOrEmpty<string>())
                {
                  int num1 = 0;
                  List<string> source1 = list;
                  int index3 = num1;
                  int num2 = index3 + 1;
                  int result;
                  if (int.TryParse(source1.GetElement<string>(index3) ?? string.Empty, out result))
                  {
                    int num3 = num2 + 1;
                    List<string> source2 = list;
                    int index4 = num3;
                    int num4 = index4 + 1;
                    string str2 = source2.GetElement<string>(index4) ?? string.Empty;
                    List<string> source3 = list;
                    int index5 = num4;
                    int num5 = index5 + 1;
                    string str3 = source3.GetElement<string>(index5) ?? string.Empty;
                    List<string> source4 = list;
                    int index6 = num5;
                    int num6 = index6 + 1;
                    string str4 = source4.GetElement<string>(index6) ?? string.Empty;
                    if (!str2.IsNullOrEmpty() && !str3.IsNullOrEmpty())
                    {
                      switch (result)
                      {
                        case 0:
                          this.LoadTimeRelationObjectIDList(str2, str3, str4);
                          continue;
                        case 1:
                          this.LoadTimeRelationObjectNameList(str2, str3, str4);
                          continue;
                        default:
                          continue;
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadTimeRelationObjectIDList(
        string sheetBundleName,
        string sheetAssetName,
        string sheetManifestName)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(sheetBundleName, sheetAssetName, sheetManifestName);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result))
            {
              Dictionary<int, string> relationObjectIdTable = this.TimeRelationObjectIDTable;
              int index3 = result;
              List<string> source2 = list;
              int index4 = num2;
              int num3 = index4 + 1;
              string str = source2.GetElement<string>(index4) ?? string.Empty;
              relationObjectIdTable[index3] = str;
            }
          }
        }
      }

      private void LoadTimeRelationObjectNameList(
        string sheetBundleName,
        string sheetAssetName,
        string manifestName)
      {
        AssetBundleInfo info;
        ((AssetBundleInfo) ref info).\u002Ector(string.Empty, sheetBundleName, sheetAssetName, manifestName);
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(info);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num3;
                int index5 = index4 + 1;
                int result3;
                bool key = int.TryParse(source3.GetElement<string>(index4) ?? string.Empty, out result3);
                if (!key)
                  result3 = 0;
                Dictionary<int, Dictionary<bool, Dictionary<int, List<ValueTuple<string, float>>>>> dictionary1;
                if (!this.TimeRelationObjectStateTable.TryGetValue(result1, out dictionary1))
                  this.TimeRelationObjectStateTable[result1] = dictionary1 = new Dictionary<int, Dictionary<bool, Dictionary<int, List<ValueTuple<string, float>>>>>();
                Dictionary<bool, Dictionary<int, List<ValueTuple<string, float>>>> dictionary2;
                if (!dictionary1.TryGetValue(result2, out dictionary2))
                  dictionary1[result2] = dictionary2 = new Dictionary<bool, Dictionary<int, List<ValueTuple<string, float>>>>();
                Dictionary<int, List<ValueTuple<string, float>>> dictionary3;
                if (!dictionary2.TryGetValue(key, out dictionary3))
                  dictionary2[key] = dictionary3 = new Dictionary<int, List<ValueTuple<string, float>>>();
                List<ValueTuple<string, float>> valueTupleList;
                if (!dictionary3.TryGetValue(result3, out valueTupleList) || valueTupleList == null)
                  dictionary3[result3] = valueTupleList = new List<ValueTuple<string, float>>();
                else
                  valueTupleList.Clear();
                switch (result2)
                {
                  case 0:
                    for (; index5 < list.Count; ++index5)
                    {
                      string element = list.GetElement<string>(index5);
                      if (!element.IsNullOrEmpty())
                        valueTupleList.Add(new ValueTuple<string, float>(element, 0.0f));
                    }
                    continue;
                  case 1:
                    for (; index5 < list.Count; ++index5)
                    {
                      string element1 = list.GetElement<string>(index5);
                      if (!element1.IsNullOrEmpty())
                      {
                        string[] source4 = element1.Split(Manager.Resources._separators, StringSplitOptions.RemoveEmptyEntries);
                        string element2 = source4.GetElement<string>(0);
                        string element3 = source4.GetElement<string>(1);
                        float result4;
                        if (!element2.IsNullOrEmpty() && !element3.IsNullOrEmpty() && float.TryParse(element3, out result4))
                          valueTupleList.Add(new ValueTuple<string, float>(element2, result4));
                      }
                    }
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
        }
      }

      private void LoadAreaGroup(DefinePack definePack)
      {
        ExcelData excelData1 = (ExcelData) null;
        List<string> stringList1 = new List<string>();
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        List<string> stringList2 = new List<string>();
        List<int>[] intListArray = new List<int>[2];
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.AreaGroupList, false);
        nameListFromPath.Sort();
        excelData1 = (ExcelData) null;
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          stringBuilder1.Clear();
          stringBuilder1.Append(nameListFromPath[index1]);
          stringBuilder2.Clear();
          stringBuilder2.AppendFormat("map{0}", (object) System.IO.Path.GetFileNameWithoutExtension(nameListFromPath[index1]));
          ExcelData excelData2 = CommonLib.LoadAsset<ExcelData>(stringBuilder1.ToString(), stringBuilder2.ToString(), false, string.Empty);
          if (!Object.op_Equality((Object) excelData2, (Object) null))
          {
            int num1 = 1;
            while (num1 < excelData2.MaxCell)
            {
              List<string> list = excelData2.list[num1++].list;
              int num2 = 0;
              int result1 = 0;
              List<string> stringList3 = list;
              int index2 = num2;
              int num3 = index2 + 1;
              if (int.TryParse(stringList3[index2], out result1))
              {
                int result2 = 0;
                List<string> stringList4 = list;
                int index3 = num3;
                int num4 = index3 + 1;
                if (int.TryParse(stringList4[index3], out result2))
                {
                  List<string> stringList5 = list;
                  int index4 = num4;
                  int index5 = index4 + 1;
                  string[] strArray1 = stringList5[index4].Split(',');
                  intListArray[0] = new List<int>();
                  foreach (string s in strArray1)
                  {
                    if (s != string.Empty)
                      intListArray[0].Add(int.Parse(s));
                  }
                  if (list.Count == index5)
                  {
                    intListArray[1] = new List<int>();
                    intListArray[1].Add(-1);
                  }
                  else
                  {
                    string str = list[index5];
                    if (str != string.Empty)
                    {
                      string[] strArray2 = str.Split(',');
                      intListArray[1] = new List<int>();
                      foreach (string s in strArray2)
                      {
                        if (s != string.Empty)
                          intListArray[1].Add(int.Parse(s));
                      }
                    }
                    else
                      intListArray[1].Add(-1);
                  }
                  if (!this.AreaGroupTable.ContainsKey(result1))
                    this.AreaGroupTable.Add(result1, new Dictionary<int, MinimapNavimesh.AreaGroupInfo>());
                  MinimapNavimesh.AreaGroupInfo areaGroupInfo = new MinimapNavimesh.AreaGroupInfo();
                  areaGroupInfo.areaID = intListArray[0];
                  areaGroupInfo.OpenStateID = intListArray[1];
                  if (!this.AreaGroupTable[result1].ContainsKey(result2))
                    this.AreaGroupTable[result1].Add(result2, areaGroupInfo);
                  else
                    this.AreaGroupTable[result1][result2] = areaGroupInfo;
                }
              }
            }
          }
        }
      }

      private void LoadActionCameraData(DefinePack definePack)
      {
        ExcelData excelData1 = (ExcelData) null;
        List<string> stringList1 = new List<string>();
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        StringBuilder stringBuilder3 = new StringBuilder();
        StringBuilder stringBuilder4 = new StringBuilder();
        List<string> stringList2 = new List<string>();
        int num1 = 1;
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.ActionCameraData, false);
        nameListFromPath.Sort();
        excelData1 = (ExcelData) null;
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          stringBuilder1.Clear();
          stringBuilder1.Append(nameListFromPath[index1]);
          stringBuilder2.Clear();
          stringBuilder2.Append(System.IO.Path.GetFileNameWithoutExtension(nameListFromPath[index1]));
          if (GlobalMethod.AssetFileExist(stringBuilder1.ToString(), stringBuilder2.ToString(), string.Empty))
          {
            ExcelData excelData2 = CommonLib.LoadAsset<ExcelData>(stringBuilder1.ToString(), stringBuilder2.ToString(), false, string.Empty);
            if (!Object.op_Equality((Object) excelData2, (Object) null))
            {
              int num2 = 1;
              while (num2 < excelData2.MaxCell)
              {
                List<string> list = excelData2.list[num2++].list;
                int index2 = 1;
                stringBuilder3.Clear();
                stringBuilder4.Clear();
                if (!(list[index2] == string.Empty))
                {
                  StringBuilder stringBuilder5 = stringBuilder3;
                  List<string> stringList3 = list;
                  int index3 = index2;
                  int index4 = index3 + 1;
                  string str1 = stringList3[index3];
                  stringBuilder5.Append(str1);
                  if (!(list[index4] == string.Empty))
                  {
                    StringBuilder stringBuilder6 = stringBuilder4;
                    List<string> stringList4 = list;
                    int index5 = index4;
                    num1 = index5 + 1;
                    string str2 = stringList4[index5];
                    stringBuilder6.Append(str2);
                    this.LoadActionCameraData(stringBuilder3.ToString(), stringBuilder4.ToString(), stringBuilder2.ToString());
                  }
                }
              }
            }
          }
        }
      }

      private void LoadActionCameraData(string abName, string asset, string SheetNumber)
      {
        List<string> stringList1 = new List<string>();
        int num1 = 2;
        int num2 = 2;
        int result1 = -1;
        int result2 = -1;
        Vector3 zero1 = Vector3.get_zero();
        Vector3 zero2 = Vector3.get_zero();
        if (!GlobalMethod.AssetFileExist(abName, asset, string.Empty))
          return;
        ExcelData excelData = CommonLib.LoadAsset<ExcelData>(abName, asset, false, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        while (num1 < excelData.MaxCell)
        {
          ActionCameraData actionCameraData = new ActionCameraData();
          List<string> list = excelData.list[num1++].list;
          int num3 = 2;
          if (list.Count >= 8)
            ;
          List<string> stringList2 = list;
          int index1 = num3;
          int num4 = index1 + 1;
          if (int.TryParse(stringList2[index1], out result1))
          {
            List<string> stringList3 = list;
            int index2 = num4;
            int num5 = index2 + 1;
            if (int.TryParse(stringList3[index2], out result2))
            {
              List<string> stringList4 = list;
              int index3 = num5;
              int num6 = index3 + 1;
              // ISSUE: cast to a reference type
              if (!float.TryParse(stringList4[index3], (float&) ref zero1.x))
                zero1.x = (__Null) 0.0;
              List<string> stringList5 = list;
              int index4 = num6;
              int num7 = index4 + 1;
              // ISSUE: cast to a reference type
              if (!float.TryParse(stringList5[index4], (float&) ref zero1.y))
                zero1.y = (__Null) 0.0;
              List<string> stringList6 = list;
              int index5 = num7;
              int num8 = index5 + 1;
              // ISSUE: cast to a reference type
              if (!float.TryParse(stringList6[index5], (float&) ref zero1.z))
                zero1.z = (__Null) 0.0;
              List<string> stringList7 = list;
              int index6 = num8;
              int num9 = index6 + 1;
              // ISSUE: cast to a reference type
              if (!float.TryParse(stringList7[index6], (float&) ref zero2.x))
                zero2.x = (__Null) 0.0;
              List<string> stringList8 = list;
              int index7 = num9;
              int num10 = index7 + 1;
              // ISSUE: cast to a reference type
              if (!float.TryParse(stringList8[index7], (float&) ref zero2.y))
                zero2.y = (__Null) 0.0;
              List<string> stringList9 = list;
              int index8 = num10;
              num2 = index8 + 1;
              // ISSUE: cast to a reference type
              if (!float.TryParse(stringList9[index8], (float&) ref zero2.z))
                zero2.z = (__Null) 0.0;
              actionCameraData.freePos = zero1;
              actionCameraData.nonMovePos = zero1;
              actionCameraData.nonMoveRot = zero2;
              if (!this.ActionCameraDataTable.ContainsKey(result1))
                this.ActionCameraDataTable.Add(result1, new Dictionary<int, ActionCameraData>());
              if (!this.ActionCameraDataTable[result1].ContainsKey(result2))
                this.ActionCameraDataTable[result1].Add(result2, new ActionCameraData());
              this.ActionCameraDataTable[result1][result2] = actionCameraData;
            }
          }
        }
      }

      private void LoadVanish(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.VanishCameraList, false);
        for (int index = 0; index < nameListFromPath.Count; ++index)
        {
          string str = nameListFromPath[index];
          if (GlobalMethod.AssetFileExist(str, "map_col_name", string.Empty))
          {
            ExcelData excelData = CommonLib.LoadAsset<ExcelData>(str, "map_col_name", false, string.Empty);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              int num = 0;
              int maxCell = excelData.MaxCell;
              while (num < maxCell)
              {
                ExcelData.Param obj = excelData.list[num++];
                string abName = obj.list[0];
                int result = 0;
                if (obj.list.Count < 2 || !int.TryParse(obj.list[1], out result))
                  result = 0;
                this.LoadVanish(str, abName, result);
              }
            }
          }
        }
      }

      private void LoadVanish(string assetbundle, string abName, int mapID)
      {
        if (!GlobalMethod.AssetFileExist(assetbundle, abName, string.Empty))
          return;
        ExcelData excelData = CommonLib.LoadAsset<ExcelData>(assetbundle, abName, false, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        List<Manager.Resources.MapTables.VisibleObjectInfo> visibleObjectInfoList = new List<Manager.Resources.MapTables.VisibleObjectInfo>();
        int maxCell = excelData.MaxCell;
        int num = 2;
        while (num < maxCell)
        {
          ExcelData.Param obj = excelData.list[num++];
          if (!obj.list[0].IsNullOrEmpty())
          {
            Manager.Resources.MapTables.VisibleObjectInfo visibleObjectInfo = new Manager.Resources.MapTables.VisibleObjectInfo();
            string str1 = obj.list[0];
            string str2 = obj.list[1];
            visibleObjectInfo.nameCollider = str1;
            visibleObjectInfo.VanishObjName = str2;
            visibleObjectInfoList.Add(visibleObjectInfo);
          }
        }
        if (!this.VanishList.ContainsKey(mapID))
          this.VanishList.Add(mapID, new List<List<Manager.Resources.MapTables.VisibleObjectInfo>>());
        this.VanishList[mapID].Add(new List<Manager.Resources.MapTables.VisibleObjectInfo>((IEnumerable<Manager.Resources.MapTables.VisibleObjectInfo>) visibleObjectInfoList));
      }

      private void LoadVanishHousingGroup(DefinePack definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.VanishCameraList, false);
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(nameListFromPath[index1]);
          if (GlobalMethod.AssetFileExist(str, withoutExtension, string.Empty))
          {
            ExcelData excelData = CommonLib.LoadAsset<ExcelData>(str, withoutExtension, false, string.Empty);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              int num1 = 1;
              int maxCell = excelData.MaxCell;
              while (num1 < maxCell)
              {
                ExcelData.Param obj = excelData.list[num1++];
                int num2 = 0;
                int result = 0;
                List<string> list1 = obj.list;
                int index2 = num2;
                int num3 = index2 + 1;
                if (int.TryParse(list1[index2], out result))
                {
                  List<string> list2 = obj.list;
                  int index3 = num3;
                  int num4 = index3 + 1;
                  string assetbundle = list2[index3];
                  List<string> list3 = obj.list;
                  int index4 = num4;
                  int num5 = index4 + 1;
                  string abName = list3[index4];
                  this.LoadVanishHousingGroup(assetbundle, abName, result);
                }
              }
            }
          }
        }
      }

      private void LoadVanishHousingGroup(string assetbundle, string abName, int mapID)
      {
        if (!GlobalMethod.AssetFileExist(assetbundle, abName, string.Empty))
          return;
        ExcelData excelData = CommonLib.LoadAsset<ExcelData>(assetbundle, abName, false, string.Empty);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        if (!this.VanishHousingAreaGroup.ContainsKey(mapID))
          this.VanishHousingAreaGroup.Add(mapID, new Dictionary<int, List<int>>());
        int maxCell = excelData.MaxCell;
        int num1 = 1;
        int result1 = 0;
        while (num1 < maxCell)
        {
          ExcelData.Param obj = excelData.list[num1++];
          int index1 = 0;
          List<int> intList = new List<int>();
          if (!obj.list[index1].IsNullOrEmpty())
          {
            List<string> list1 = obj.list;
            int index2 = index1;
            int num2 = index2 + 1;
            if (int.TryParse(list1[index2], out result1))
            {
              List<string> list2 = obj.list;
              int index3 = num2;
              int num3 = index3 + 1;
              string self = list2[index3];
              if (!self.IsNullOrEmpty())
              {
                string str = self;
                char[] chArray = new char[1]{ ',' };
                foreach (string s in str.Split(chArray))
                {
                  int result2 = 0;
                  if (int.TryParse(s, out result2))
                    intList.Add(result2);
                }
              }
              if (!this.VanishHousingAreaGroup[mapID].ContainsKey(result1))
                this.VanishHousingAreaGroup[mapID].Add(result1, new List<int>());
              if (intList != null && intList.Count > 0)
              {
                if (this.VanishHousingAreaGroup[mapID][result1].Count != 0)
                  this.VanishHousingAreaGroup[mapID][result1].Clear();
                this.VanishHousingAreaGroup[mapID][result1].AddRange((IEnumerable<int>) intList);
              }
            }
          }
        }
      }

      private void LoadEnviroInfoList(DefinePack definePack)
      {
        if (Object.op_Equality((Object) definePack, (Object) null))
          return;
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(definePack.ABDirectories.EnviroInfoList, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str1 = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str1);
          if (AssetBundleCheck.IsFile(str1, withoutExtension))
          {
            AssetBundleInfo info;
            ((AssetBundleInfo) ref info).\u002Ector(string.Empty, str1, withoutExtension, string.Empty);
            ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(info);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
              {
                List<string> list = excelData.list[index2].list;
                if (!list.IsNullOrEmpty<string>())
                {
                  int num1 = 0;
                  List<string> source1 = list;
                  int index3 = num1;
                  int num2 = index3 + 1;
                  int result;
                  if (int.TryParse(source1.GetElement<string>(index3) ?? string.Empty, out result))
                  {
                    List<string> source2 = list;
                    int index4 = num2;
                    int num3 = index4 + 1;
                    string str2 = source2.GetElement<string>(index4) ?? string.Empty;
                    List<string> source3 = list;
                    int index5 = num3;
                    int num4 = index5 + 1;
                    string self1 = source3.GetElement<string>(index5) ?? string.Empty;
                    List<string> source4 = list;
                    int index6 = num4;
                    int num5 = index6 + 1;
                    string self2 = source4.GetElement<string>(index6) ?? string.Empty;
                    List<string> source5 = list;
                    int index7 = num5;
                    int num6 = index7 + 1;
                    string str3 = source5.GetElement<string>(index7) ?? string.Empty;
                    if (!self1.IsNullOrEmpty() && !self2.IsNullOrEmpty())
                    {
                      AssetBundleInfo sheetInfo;
                      ((AssetBundleInfo) ref sheetInfo).\u002Ector(str2, self1, self2, str3);
                      if (result == 0)
                        this.LoadEnvTempRangeList(sheetInfo);
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadEnvTempRangeList(AssetBundleInfo sheetInfo)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              string self1 = source2.GetElement<string>(index3) ?? string.Empty;
              List<string> source3 = list;
              int index4 = num3;
              int num4 = index4 + 1;
              string self2 = source3.GetElement<string>(index4) ?? string.Empty;
              List<string> source4 = list;
              int index5 = num4;
              int num5 = index5 + 1;
              string str = source4.GetElement<string>(index5) ?? string.Empty;
              if (!self1.IsNullOrEmpty() && !self2.IsNullOrEmpty())
                this.LoadEnvTempRangeTable(new AssetBundleInfo(string.Empty, self1, self2, str), result);
            }
          }
        }
      }

      private void LoadEnvTempRangeTable(AssetBundleInfo sheetInfo, int mapID)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                string str = source3.GetElement<string>(index4) ?? string.Empty;
                Dictionary<int, Dictionary<int, List<ValueTuple<int, int>>>> dictionary1;
                if (!this.TempRangeTable.TryGetValue(mapID, out dictionary1) || dictionary1 == null)
                {
                  Dictionary<int, Dictionary<int, List<ValueTuple<int, int>>>> dictionary2 = new Dictionary<int, Dictionary<int, List<ValueTuple<int, int>>>>();
                  this.TempRangeTable[mapID] = dictionary2;
                  dictionary1 = dictionary2;
                }
                Dictionary<int, List<ValueTuple<int, int>>> dictionary3;
                if (!dictionary1.TryGetValue(result1, out dictionary3) || dictionary3 == null)
                {
                  Dictionary<int, List<ValueTuple<int, int>>> dictionary2 = new Dictionary<int, List<ValueTuple<int, int>>>();
                  dictionary1[result1] = dictionary2;
                  dictionary3 = dictionary2;
                }
                List<ValueTuple<int, int>> valueTupleList1;
                if (!dictionary3.TryGetValue(result2, out valueTupleList1) || valueTupleList1 == null)
                {
                  List<ValueTuple<int, int>> valueTupleList2 = new List<ValueTuple<int, int>>();
                  dictionary3[result2] = valueTupleList2;
                  valueTupleList1 = valueTupleList2;
                }
                else
                  valueTupleList1.Clear();
                while (num4 < list.Count)
                {
                  List<string> source4 = list;
                  int index5 = num4;
                  int num5 = index5 + 1;
                  string s1 = source4.GetElement<string>(index5) ?? string.Empty;
                  List<string> source5 = list;
                  int index6 = num5;
                  num4 = index6 + 1;
                  string s2 = source5.GetElement<string>(index6) ?? string.Empty;
                  int result3;
                  int result4;
                  if (int.TryParse(s1, out result3) && int.TryParse(s2, out result4))
                    valueTupleList1.Add(new ValueTuple<int, int>(result3, result4));
                }
              }
            }
          }
        }
      }

      private void LoadMiniMapInfo()
      {
        ExcelData excelData1 = (ExcelData) null;
        List<string> stringList1 = new List<string>();
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        List<string> stringList2 = new List<string>();
        int num1 = 0;
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(MiniMapControler.RoadPath, false);
        nameListFromPath.Sort();
        excelData1 = (ExcelData) null;
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          stringBuilder1.Clear();
          stringBuilder1.Append(nameListFromPath[index1]);
          stringBuilder2.Clear();
          stringBuilder2.Append(System.IO.Path.GetFileNameWithoutExtension(nameListFromPath[index1]));
          if (GlobalMethod.AssetFileExist(stringBuilder1.ToString(), stringBuilder2.ToString(), string.Empty))
          {
            ExcelData excelData2 = CommonLib.LoadAsset<ExcelData>(stringBuilder1.ToString(), stringBuilder2.ToString(), false, string.Empty);
            if (!Object.op_Equality((Object) excelData2, (Object) null))
            {
              int num2 = 1;
              while (num2 < excelData2.MaxCell)
              {
                List<string> list = excelData2.list[num2++].list;
                int index2 = 0;
                if (!(list[index2] == string.Empty))
                {
                  int result = -1;
                  List<string> stringList3 = list;
                  int index3 = index2;
                  int num3 = index3 + 1;
                  if (int.TryParse(stringList3[index3], out result))
                  {
                    MiniMapControler.MinimapInfo minimapInfo1 = new MiniMapControler.MinimapInfo();
                    MiniMapControler.MinimapInfo minimapInfo2 = minimapInfo1;
                    List<string> stringList4 = list;
                    int index4 = num3;
                    int num4 = index4 + 1;
                    string str1 = stringList4[index4];
                    minimapInfo2.assetPath = str1;
                    MiniMapControler.MinimapInfo minimapInfo3 = minimapInfo1;
                    List<string> stringList5 = list;
                    int index5 = num4;
                    int num5 = index5 + 1;
                    string str2 = stringList5[index5];
                    minimapInfo3.asset = str2;
                    MiniMapControler.MinimapInfo minimapInfo4 = minimapInfo1;
                    List<string> stringList6 = list;
                    int index6 = num5;
                    num1 = index6 + 1;
                    string str3 = stringList6[index6];
                    minimapInfo4.manifest = str3;
                    if (!this.MinimapInfoTable.ContainsKey(result))
                      this.MinimapInfoTable.Add(result, minimapInfo1);
                    else
                      this.MinimapInfoTable[result] = minimapInfo1;
                  }
                }
              }
            }
          }
        }
      }

      public class VisibleObjectInfo
      {
        public string nameCollider;
        public string VanishObjName;
      }
    }

    public class PopupInfoTable
    {
      private Dictionary<int, string[]> warningTable = new Dictionary<int, string[]>();
      private Dictionary<int, RequestInfo> requestTable = new Dictionary<int, RequestInfo>();
      private Dictionary<int, Tuple<string[], string[]>> requestTextTable = new Dictionary<int, Tuple<string[], string[]>>();
      private Dictionary<int, string[]> storySupportTable = new Dictionary<int, string[]>();
      private Dictionary<int, Sprite[]> tutorialImageTable = new Dictionary<int, Sprite[]>();
      private Dictionary<int, List<AssetBundleInfo>> tutorialImageABInfo = new Dictionary<int, List<AssetBundleInfo>>();
      private Dictionary<int, ValueTuple<string, GameObject[]>> tutorialPrefabTable = new Dictionary<int, ValueTuple<string, GameObject[]>>();
      private Dictionary<int, ValueTuple<string, List<AssetBundleInfo>>> tutorialPrefabABInfo = new Dictionary<int, ValueTuple<string, List<AssetBundleInfo>>>();
      private Dictionary<int, Manager.Resources.TutorialUIFilterInfo> tutorialFilterTable = new Dictionary<int, Manager.Resources.TutorialUIFilterInfo>();

      public PopupInfoTable()
      {
        this.WarningTable = new ReadOnlyDictionary<int, string[]>((IDictionary<int, string[]>) this.warningTable);
        this.RequestTable = new ReadOnlyDictionary<int, RequestInfo>((IDictionary<int, RequestInfo>) this.requestTable);
        this.StorySupportTable = new ReadOnlyDictionary<int, string[]>((IDictionary<int, string[]>) this.storySupportTable);
        this.TutorialImageTable = new ReadOnlyDictionary<int, Sprite[]>((IDictionary<int, Sprite[]>) this.tutorialImageTable);
        this.TutorialPrefabTable = new ReadOnlyDictionary<int, ValueTuple<string, GameObject[]>>((IDictionary<int, ValueTuple<string, GameObject[]>>) this.tutorialPrefabTable);
        this.TutorialFilterTable = new ReadOnlyDictionary<int, Manager.Resources.TutorialUIFilterInfo>((IDictionary<int, Manager.Resources.TutorialUIFilterInfo>) this.tutorialFilterTable);
      }

      public ReadOnlyDictionary<int, string[]> WarningTable { get; private set; }

      public ReadOnlyDictionary<int, RequestInfo> RequestTable { get; private set; }

      public ReadOnlyDictionary<int, string[]> StorySupportTable { get; private set; }

      public ReadOnlyDictionary<int, Sprite[]> TutorialImageTable { get; private set; }

      public ReadOnlyDictionary<int, ValueTuple<string, GameObject[]>> TutorialPrefabTable { get; private set; }

      public ReadOnlyDictionary<int, Manager.Resources.TutorialUIFilterInfo> TutorialFilterTable { get; private set; }

      public Dictionary<int, int> RequestFlavorAdditionBorderTable { get; private set; } = new Dictionary<int, int>();

      private string LogAssetBundleInfo(AssetBundleInfo _info)
      {
        return string.Format("AssetBundleName[{0}] AssetName[{1}] ManifestName[{2}]", (object) _info.assetbundle, (object) _info.asset, (object) _info.manifest);
      }

      private string LogAssetBundleInfo(AssetBundleInfo _info, int _row, int _clm)
      {
        return string.Format("AssetBundleName[{0}] AssetName[{1}] ManifestName[{2}] Row[{3}] Clm[{4}]", (object) _info.assetbundle, (object) _info.asset, (object) _info.manifest, (object) _row, (object) _clm);
      }

      private AssetBundleInfo GetAssetInfo(
        List<string> _address,
        ref int _idx,
        bool _addSummary)
      {
        string str1;
        if (_addSummary)
        {
          List<string> source = _address;
          int num;
          _idx = (num = _idx) + 1;
          int index = num;
          str1 = source.GetElement<string>(index) ?? string.Empty;
        }
        else
          str1 = string.Empty;
        string str2 = str1;
        List<string> source1 = _address;
        int num1;
        _idx = (num1 = _idx) + 1;
        int index1 = num1;
        string str3 = source1.GetElement<string>(index1) ?? string.Empty;
        List<string> source2 = _address;
        int num2;
        _idx = (num2 = _idx) + 1;
        int index2 = num2;
        string str4 = source2.GetElement<string>(index2) ?? string.Empty;
        List<string> source3 = _address;
        int num3;
        _idx = (num3 = _idx) + 1;
        int index3 = num3;
        string str5 = source3.GetElement<string>(index3) ?? string.Empty;
        return new AssetBundleInfo(str2, str3, str4, str5);
      }

      public void Load(DefinePack _definePack)
      {
        this.RequestFlavorAdditionBorderTable.Clear();
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(_definePack.ABDirectories.PopupInfoList, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        this.tutorialImageABInfo.Clear();
        this.tutorialPrefabABInfo.Clear();
        this.tutorialFilterTable.Clear();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          if (AssetBundleCheck.IsFile(str, withoutExtension))
          {
            ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(str, withoutExtension, string.Empty);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
              {
                List<string> list = excelData.list[index2].list;
                if (!list.IsNullOrEmpty<string>())
                {
                  int _idx = 0;
                  int result;
                  if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result))
                  {
                    AssetBundleInfo assetInfo = this.GetAssetInfo(list, ref _idx, true);
                    switch (result)
                    {
                      case 0:
                        this.LoadWarningInfo(assetInfo);
                        continue;
                      case 1:
                        this.LoadRequestInfo(assetInfo);
                        continue;
                      case 2:
                        this.LoadRequestText(assetInfo);
                        continue;
                      case 3:
                        this.LoadStorySupportInfo(assetInfo);
                        continue;
                      case 4:
                        this.LoadTutorialUIPrefab(assetInfo);
                        continue;
                      case 5:
                        this.LoadTutorialUIFilterInfo(assetInfo);
                        continue;
                      default:
                        continue;
                    }
                  }
                }
              }
            }
          }
        }
        List<ValueTuple<string, string>> toRelease1 = ListPool<ValueTuple<string, string>>.Get();
        using (Dictionary<int, List<AssetBundleInfo>>.Enumerator enumerator1 = this.tutorialImageABInfo.GetEnumerator())
        {
          while (enumerator1.MoveNext())
          {
            KeyValuePair<int, List<AssetBundleInfo>> current1 = enumerator1.Current;
            Manager.Resources.TutorialUIFilterInfo tutorialUiFilterInfo;
            if (!current1.Value.IsNullOrEmpty<AssetBundleInfo>() && (!this.tutorialFilterTable.TryGetValue(current1.Key, out tutorialUiFilterInfo) || Game.isAdd01 || !tutorialUiFilterInfo.Adult))
            {
              List<Sprite> spriteList = ListPool<Sprite>.Get();
              using (List<AssetBundleInfo>.Enumerator enumerator2 = current1.Value.GetEnumerator())
              {
                while (enumerator2.MoveNext())
                {
                  AssetBundleInfo current2 = enumerator2.Current;
                  Sprite sprite = Manager.Resources.ItemIconTables.LoadSpriteAsset((string) current2.assetbundle, (string) current2.asset, (string) current2.manifest);
                  if (!Object.op_Equality((Object) sprite, (Object) null))
                  {
                    Singleton<Manager.Resources>.Instance.AddLoadAssetBundle((string) current2.assetbundle, (string) current2.manifest);
                    spriteList.Add(sprite);
                  }
                }
              }
              if (!spriteList.IsNullOrEmpty<Sprite>())
              {
                Sprite[] spriteArray = new Sprite[spriteList.Count];
                for (int index = 0; index < spriteArray.Length; ++index)
                  spriteArray[index] = spriteList[index];
                this.tutorialImageTable[current1.Key] = spriteArray;
              }
              ListPool<Sprite>.Release(spriteList);
            }
          }
        }
        ListPool<ValueTuple<string, string>>.Release(toRelease1);
        using (Dictionary<int, ValueTuple<string, List<AssetBundleInfo>>>.Enumerator enumerator1 = this.tutorialPrefabABInfo.GetEnumerator())
        {
          while (enumerator1.MoveNext())
          {
            KeyValuePair<int, ValueTuple<string, List<AssetBundleInfo>>> current1 = enumerator1.Current;
            Manager.Resources.TutorialUIFilterInfo tutorialUiFilterInfo;
            if (!((List<AssetBundleInfo>) current1.Value.Item2).IsNullOrEmpty<AssetBundleInfo>() && (!this.tutorialFilterTable.TryGetValue(current1.Key, out tutorialUiFilterInfo) || Game.isAdd01 || !tutorialUiFilterInfo.Adult))
            {
              List<GameObject> gameObjectList = ListPool<GameObject>.Get();
              using (List<AssetBundleInfo>.Enumerator enumerator2 = ((List<AssetBundleInfo>) current1.Value.Item2).GetEnumerator())
              {
                while (enumerator2.MoveNext())
                {
                  AssetBundleInfo current2 = enumerator2.Current;
                  GameObject gameObject = CommonLib.LoadAsset<GameObject>((string) current2.assetbundle, (string) current2.asset, false, (string) current2.manifest);
                  if (!Object.op_Equality((Object) gameObject, (Object) null))
                  {
                    Singleton<Manager.Resources>.Instance.AddLoadAssetBundle((string) current2.assetbundle, (string) current2.manifest);
                    gameObjectList.Add(gameObject);
                  }
                }
              }
              if (!gameObjectList.IsNullOrEmpty<GameObject>())
              {
                GameObject[] gameObjectArray = new GameObject[gameObjectList.Count];
                for (int index = 0; index < gameObjectArray.Length; ++index)
                  gameObjectArray[index] = gameObjectList[index];
                this.tutorialPrefabTable[current1.Key] = new ValueTuple<string, GameObject[]>((string) current1.Value.Item1, gameObjectArray);
              }
              ListPool<GameObject>.Release(gameObjectList);
            }
          }
        }
        foreach (KeyValuePair<int, RequestInfo> keyValuePair in this.requestTable)
        {
          Tuple<string[], string[]> tuple;
          if (!this.requestTextTable.TryGetValue(keyValuePair.Key, out tuple))
            keyValuePair.Value.SetText(new string[2]
            {
              string.Empty,
              string.Empty
            }, new string[2]{ string.Empty, string.Empty });
          else
            keyValuePair.Value.SetText(tuple.Item1, tuple.Item2);
        }
        if (this.RequestFlavorAdditionBorderTable.IsNullOrEmpty<int, int>())
          return;
        List<ValueTuple<int, int>> toRelease2 = ListPool<ValueTuple<int, int>>.Get();
        foreach (KeyValuePair<int, int> keyValuePair in this.RequestFlavorAdditionBorderTable)
          toRelease2.Add(new ValueTuple<int, int>(keyValuePair.Key, keyValuePair.Value));
        this.RequestFlavorAdditionBorderTable.Clear();
        toRelease2.Sort((Comparison<ValueTuple<int, int>>) ((x1, x2) => (int) (x1.Item1 - x2.Item1)));
        using (List<ValueTuple<int, int>>.Enumerator enumerator = toRelease2.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ValueTuple<int, int> current = enumerator.Current;
            this.RequestFlavorAdditionBorderTable[(int) current.Item1] = (int) current.Item2;
          }
        }
        ListPool<ValueTuple<int, int>>.Release(toRelease2);
      }

      public void Release()
      {
        this.warningTable.Clear();
        this.requestTable.Clear();
        this.requestTextTable.Clear();
        this.storySupportTable.Clear();
        this.tutorialImageTable.Clear();
        this.tutorialImageABInfo.Clear();
        this.tutorialPrefabTable.Clear();
        this.tutorialPrefabABInfo.Clear();
        this.tutorialFilterTable.Clear();
        this.RequestFlavorAdditionBorderTable.Clear();
      }

      private void LoadWarningInfo(AssetBundleInfo _address)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_address);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result))
            {
              int num3 = num2 + 1;
              List<string> source2 = list;
              int index3 = num3;
              int num4 = index3 + 1;
              string str1 = source2.GetElement<string>(index3) ?? string.Empty;
              List<string> source3 = list;
              int index4 = num4;
              int num5 = index4 + 1;
              string str2 = source3.GetElement<string>(index4) ?? string.Empty;
              this.warningTable[result] = new string[2]
              {
                str1,
                str2
              };
            }
          }
        }
      }

      private void LoadRequestInfo(AssetBundleInfo _address)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_address);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                switch (result2)
                {
                  case 0:
                    List<Tuple<int, int, int>> tupleList = ListPool<Tuple<int, int, int>>.Get();
                    while (num3 < list.Count)
                    {
                      List<string> source3 = list;
                      int index4 = num3;
                      int num4 = index4 + 1;
                      int result3;
                      if (int.TryParse(source3.GetElement<string>(index4) ?? string.Empty, out result3))
                      {
                        List<string> source4 = list;
                        int index5 = num4;
                        int num5 = index5 + 1;
                        int result4;
                        if (int.TryParse(source4.GetElement<string>(index5) ?? string.Empty, out result4))
                        {
                          List<string> source5 = list;
                          int index6 = num5;
                          num3 = index6 + 1;
                          int result5;
                          if (int.TryParse(source5.GetElement<string>(index6) ?? string.Empty, out result5))
                            tupleList.Add(new Tuple<int, int, int>(result3, result4, result5));
                          else
                            break;
                        }
                        else
                          break;
                      }
                      else
                        break;
                    }
                    if (tupleList.IsNullOrEmpty<Tuple<int, int, int>>())
                    {
                      ListPool<Tuple<int, int, int>>.Release(tupleList);
                      continue;
                    }
                    Tuple<int, int, int>[] _items1 = new Tuple<int, int, int>[tupleList.Count];
                    for (int index4 = 0; index4 < tupleList.Count; ++index4)
                      _items1[index4] = tupleList[index4];
                    this.requestTable[result1] = new RequestInfo(result2, _items1);
                    ListPool<Tuple<int, int, int>>.Release(tupleList);
                    continue;
                  case 1:
                    this.requestTable[result1] = new RequestInfo(result2, (Tuple<int, int, int>[]) null);
                    continue;
                  case 2:
                    List<string> source6 = list;
                    int index7 = num3;
                    int num6 = index7 + 1;
                    int result6;
                    if (int.TryParse(source6.GetElement<string>(index7) ?? string.Empty, out result6))
                    {
                      Tuple<int, int, int>[] _items2 = new Tuple<int, int, int>[1]
                      {
                        new Tuple<int, int, int>(result6, 0, 0)
                      };
                      this.requestTable[result1] = new RequestInfo(result2, _items2);
                      this.RequestFlavorAdditionBorderTable[result1] = _items2[0].Item1;
                      continue;
                    }
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
        }
      }

      private void LoadRequestText(AssetBundleInfo _address)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_address);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              int result2;
              int num4;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                switch (result2)
                {
                  case 0:
                  case 2:
                    List<string> source3 = list;
                    int index4 = num3;
                    int num5 = index4 + 1;
                    string str1 = source3.GetElement<string>(index4) ?? string.Empty;
                    int num6 = num5 + 1;
                    List<string> source4 = list;
                    int index5 = num6;
                    num4 = index5 + 1;
                    string str2 = source4.GetElement<string>(index5) ?? string.Empty;
                    this.SetNewLine(ref str1);
                    this.SetNewLine(ref str2);
                    this.requestTextTable[result1] = new Tuple<string[], string[]>(new string[2]
                    {
                      str1,
                      str2
                    }, new string[2]);
                    continue;
                  case 1:
                    List<string> source5 = list;
                    int index6 = num3;
                    int num7 = index6 + 1;
                    string str3 = source5.GetElement<string>(index6) ?? string.Empty;
                    List<string> source6 = list;
                    int index7 = num7;
                    int num8 = index7 + 1;
                    string str4 = source6.GetElement<string>(index7) ?? string.Empty;
                    List<string> source7 = list;
                    int index8 = num8;
                    int num9 = index8 + 1;
                    string str5 = source7.GetElement<string>(index8) ?? string.Empty;
                    List<string> source8 = list;
                    int index9 = num9;
                    num4 = index9 + 1;
                    string str6 = source8.GetElement<string>(index9) ?? string.Empty;
                    this.SetNewLine(ref str3);
                    this.SetNewLine(ref str4);
                    this.SetNewLine(ref str5);
                    this.SetNewLine(ref str6);
                    this.requestTextTable[result1] = new Tuple<string[], string[]>(new string[2]
                    {
                      str3,
                      str5
                    }, new string[2]{ str4, str6 });
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
        }
      }

      private void LoadStorySupportInfo(AssetBundleInfo _address)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_address);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result))
            {
              int num3 = num2 + 2;
              List<string> source2 = list;
              int index3 = num3;
              int num4 = index3 + 1;
              string str1 = source2.GetElement<string>(index3) ?? string.Empty;
              List<string> source3 = list;
              int index4 = num4;
              int num5 = index4 + 1;
              string str2 = source3.GetElement<string>(index4) ?? string.Empty;
              this.storySupportTable[result] = new string[2]
              {
                str1,
                str2
              };
            }
          }
        }
      }

      private void LoadTutorialImage(AssetBundleInfo _address)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_address);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result))
            {
              int num3 = num2 + 1;
              List<ValueTuple<string, string, string>> toRelease = ListPool<ValueTuple<string, string, string>>.Get();
              while (num3 < list.Count)
              {
                List<string> source2 = list;
                int index3 = num3;
                int num4 = index3 + 1;
                string element1 = source2.GetElement<string>(index3);
                List<string> source3 = list;
                int index4 = num4;
                int num5 = index4 + 1;
                string element2 = source3.GetElement<string>(index4);
                List<string> source4 = list;
                int index5 = num5;
                num3 = index5 + 1;
                string element3 = source4.GetElement<string>(index5);
                if (!element1.IsNullOrEmpty() && !element2.IsNullOrEmpty())
                  toRelease.Add(new ValueTuple<string, string, string>(element1, element2, element3));
              }
              List<AssetBundleInfo> assetBundleInfoList;
              if (!this.tutorialImageABInfo.TryGetValue(result, out assetBundleInfoList) || assetBundleInfoList == null)
                this.tutorialImageABInfo[result] = assetBundleInfoList = new List<AssetBundleInfo>();
              else
                assetBundleInfoList.Clear();
              using (List<ValueTuple<string, string, string>>.Enumerator enumerator = toRelease.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  ValueTuple<string, string, string> current = enumerator.Current;
                  assetBundleInfoList.Add(new AssetBundleInfo(string.Empty, (string) current.Item1, (string) current.Item2, (string) current.Item3));
                }
              }
              ListPool<ValueTuple<string, string, string>>.Release(toRelease);
            }
          }
        }
      }

      private void LoadTutorialUIPrefab(AssetBundleInfo _address)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_address);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result))
            {
              List<string> source2 = list;
              int index3 = num2;
              int num3 = index3 + 1;
              string str = source2.GetElement<string>(index3) ?? string.Empty;
              List<ValueTuple<string, string, string>> toRelease = ListPool<ValueTuple<string, string, string>>.Get();
              while (num3 < list.Count)
              {
                List<string> source3 = list;
                int index4 = num3;
                int num4 = index4 + 1;
                string element1 = source3.GetElement<string>(index4);
                List<string> source4 = list;
                int index5 = num4;
                int num5 = index5 + 1;
                string element2 = source4.GetElement<string>(index5);
                List<string> source5 = list;
                int index6 = num5;
                num3 = index6 + 1;
                string element3 = source5.GetElement<string>(index6);
                if (!element1.IsNullOrEmpty() && !element2.IsNullOrEmpty())
                  toRelease.Add(new ValueTuple<string, string, string>(element1, element2, element3));
              }
              ValueTuple<string, List<AssetBundleInfo>> valueTuple1;
              if (!this.tutorialPrefabABInfo.TryGetValue(result, out valueTuple1))
              {
                ValueTuple<string, List<AssetBundleInfo>> valueTuple2 = new ValueTuple<string, List<AssetBundleInfo>>(str, new List<AssetBundleInfo>());
                this.tutorialPrefabABInfo[result] = valueTuple2;
                valueTuple1 = valueTuple2;
              }
              else if (valueTuple1.Item2 == null)
              {
                valueTuple1.Item1 = (__Null) str;
                valueTuple1.Item2 = (__Null) new List<AssetBundleInfo>();
              }
              else
              {
                valueTuple1.Item1 = (__Null) str;
                ((List<AssetBundleInfo>) valueTuple1.Item2).Clear();
              }
              using (List<ValueTuple<string, string, string>>.Enumerator enumerator = toRelease.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  ValueTuple<string, string, string> current = enumerator.Current;
                  ((List<AssetBundleInfo>) valueTuple1.Item2).Add(new AssetBundleInfo(string.Empty, (string) current.Item1, (string) current.Item2, (string) current.Item3));
                }
              }
              ListPool<ValueTuple<string, string, string>>.Release(toRelease);
              this.tutorialPrefabABInfo[result] = valueTuple1;
            }
          }
        }
      }

      private void LoadTutorialUIFilterInfo(AssetBundleInfo _address)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_address);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              int index3 = num2 + 1;
              bool result2;
              if (bool.TryParse(list.GetElement<string>(index3) ?? string.Empty, out result2))
                this.tutorialFilterTable[result1] = new Manager.Resources.TutorialUIFilterInfo()
                {
                  Success = true,
                  Adult = result2
                };
            }
          }
        }
      }

      private void SetNewLine(ref string str)
      {
        if (str.IsNullOrEmpty())
          return;
        str = str.Replace("\\n", "\n");
      }
    }

    public class TutorialUIInfo
    {
      public int groupID = -1;
      public string title = string.Empty;
      public List<Manager.Resources.TutorialUIInfo.ElemInfo> elemList = new List<Manager.Resources.TutorialUIInfo.ElemInfo>();
      public int layoutType;

      public class ElemInfo
      {
        public List<ValueTuple<AssetBundleInfo, Sprite>> spriteInfoList = new List<ValueTuple<AssetBundleInfo, Sprite>>();
        public string subTitle = string.Empty;
        public string flavorText = string.Empty;
      }
    }

    public struct TutorialUIFilterInfo
    {
      public bool Success;
      public bool Adult;
    }

    public class SoundTable
    {
      private string[] separators = new string[2]
      {
        "/",
        "／"
      };
      private string[] timeSeparators = new string[2]
      {
        ":",
        "："
      };
      private Regex regex = new Regex("<\\d{1,2}:[0-9]{1,2}\\/[0-9]{1,2}:[0-9]{1,2}>");

      public Dictionary<int, Dictionary<int, Dictionary<int, ValueTuple<int[], int[], int[]>>>> DefaultFootStepSETable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<int, ValueTuple<int[], int[], int[]>>>>();

      public Dictionary<int, AssetBundleInfo> MapBGMABTable { get; private set; } = new Dictionary<int, AssetBundleInfo>();

      public Dictionary<int, Dictionary<int, SoundPlayer.MapBGMInfo>> MapBGMInfoTable { get; private set; } = new Dictionary<int, Dictionary<int, SoundPlayer.MapBGMInfo>>();

      public Dictionary<int, Dictionary<int, AssetBundleInfo>> EnviroSEPrefabInfoTable { get; private set; } = new Dictionary<int, Dictionary<int, AssetBundleInfo>>();

      public Dictionary<int, Dictionary<int, int[]>> EnviroSEAdjacentInfoTable { get; private set; } = new Dictionary<int, Dictionary<int, int[]>>();

      public Dictionary<int, Dictionary<int, AssetBundleInfo>> ReverbPrefabInfoTable { get; private set; } = new Dictionary<int, Dictionary<int, AssetBundleInfo>>();

      public Dictionary<int, Dictionary<int, AssetBundleInfo>> ActorActionVoiceDataTable { get; private set; } = new Dictionary<int, Dictionary<int, AssetBundleInfo>>();

      public bool TryGetMapActionVoiceInfo(int personalID, int voiceID, out AssetBundleInfo info)
      {
        Dictionary<int, AssetBundleInfo> dictionary;
        if (this.ActorActionVoiceDataTable.TryGetValue(personalID, out dictionary))
          return dictionary.TryGetValue(voiceID, out info);
        info = (AssetBundleInfo) null;
        return false;
      }

      private string LogAssetBundleInfo(AssetBundleInfo _info)
      {
        return string.Format("AssetBundleName[{0}] AssetName[{1}] ManifestName[{2}]", (object) _info.assetbundle, (object) _info.asset, (object) _info.manifest);
      }

      private string LogAssetBundleInfo(AssetBundleInfo _info, string _ver)
      {
        return string.Format("AssetBundleName[{0}] AssetName[{1}] ManifestName[{2}] Ver[{3}]", (object) _info.assetbundle, (object) _info.asset, (object) _info.manifest, (object) _ver);
      }

      private string LogAssetBundleInfo(AssetBundleInfo _info, string _ver, int _row)
      {
        return string.Format("AssetBundleName[{0}] AssetName[{1}] ManifestName[{2}] Ver[{3}] 行[{4}]", (object) _info.assetbundle, (object) _info.asset, (object) _info.manifest, (object) _ver, (object) _row);
      }

      private string LogAssetBundleInfo(AssetBundleInfo _info, string _ver, int _row, int _clm)
      {
        return string.Format("AssetBundleName[{0}] AssetName[{1}] ManifestName[{2}] Ver[{3}] 行[{4}] 列[{5}]", (object) _info.assetbundle, (object) _info.asset, (object) _info.manifest, (object) _ver, (object) _row, (object) _clm);
      }

      private AssetBundleInfo GetAssetInfo(
        List<string> _address,
        ref int _idx,
        bool _addSummary)
      {
        string str1;
        if (_addSummary)
        {
          List<string> source = _address;
          int num;
          _idx = (num = _idx) + 1;
          int index = num;
          str1 = source.GetElement<string>(index) ?? string.Empty;
        }
        else
          str1 = string.Empty;
        string str2 = str1;
        List<string> source1 = _address;
        int num1;
        _idx = (num1 = _idx) + 1;
        int index1 = num1;
        string str3 = source1.GetElement<string>(index1) ?? string.Empty;
        List<string> source2 = _address;
        int num2;
        _idx = (num2 = _idx) + 1;
        int index2 = num2;
        string str4 = source2.GetElement<string>(index2) ?? string.Empty;
        List<string> source3 = _address;
        int num3;
        _idx = (num3 = _idx) + 1;
        int index3 = num3;
        string str5 = source3.GetElement<string>(index3) ?? string.Empty;
        return new AssetBundleInfo(str2, str3, str4, str5);
      }

      public void Load(DefinePack _definePack)
      {
        this.LoadMapBGMInfo(_definePack);
        this.LoadDefaultFootStep(_definePack.ABDirectories.DefaultMaleFootStepSEInfoList, 0);
        this.LoadDefaultFootStep(_definePack.ABDirectories.DefaultFemaleFootStepSEInfoList, 1);
        this.LoadEnviroInfo(_definePack);
        this.LoadPersonalVoiceInfo(_definePack.ABDirectories.MapActionVoiceInfoList, this.ActorActionVoiceDataTable);
      }

      public void Release()
      {
        this.DefaultFootStepSETable.Clear();
        this.MapBGMABTable.Clear();
        this.MapBGMInfoTable.Clear();
        this.EnviroSEPrefabInfoTable.Clear();
        this.EnviroSEAdjacentInfoTable.Clear();
        this.ReverbPrefabInfoTable.Clear();
        this.ActorActionVoiceDataTable.Clear();
      }

      private void LoadMapBGMInfo(DefinePack _definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(_definePack.ABDirectories.MapBGMInfoList, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          if (AssetBundleCheck.IsFile(str, withoutExtension))
          {
            AssetBundleInfo info;
            ((AssetBundleInfo) ref info).\u002Ector(string.Empty, str, withoutExtension, string.Empty);
            ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(info);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
              {
                List<string> list = excelData.list[index2].list;
                if (!list.IsNullOrEmpty<string>())
                {
                  int num = 0;
                  List<string> source = list;
                  int index3 = num;
                  int _idx = index3 + 1;
                  int result;
                  if (int.TryParse(source.GetElement<string>(index3) ?? string.Empty, out result))
                  {
                    AssetBundleInfo assetInfo = this.GetAssetInfo(list, ref _idx, true);
                    switch (result)
                    {
                      case 0:
                        this.LoadMapBGMAssetInfo(assetInfo, withoutExtension);
                        continue;
                      case 1:
                        this.LoadMapBGMChunkInfo(assetInfo, withoutExtension);
                        continue;
                      default:
                        continue;
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadMapBGMAssetInfo(AssetBundleInfo _address, string _ver)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_address);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num = 0;
            List<string> source = list;
            int index2 = num;
            int _idx = index2 + 1;
            int result;
            if (int.TryParse(source.GetElement<string>(index2) ?? string.Empty, out result))
              this.MapBGMABTable[result] = this.GetAssetInfo(list, ref _idx, true);
          }
        }
      }

      private void LoadMapBGMChunkInfo(AssetBundleInfo _address, string _ver)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_address);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int _idx = 0;
            int result;
            if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result))
              this.LoadMapBGMAreaInfoInChunk(this.GetAssetInfo(list, ref _idx, false), _ver, result);
          }
        }
      }

      private void LoadMapBGMAreaInfoInChunk(AssetBundleInfo _address, string _ver, int _chunkID)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_address);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              int num3 = num2 + 1;
              List<string> source2 = list;
              int index3 = num3;
              int num4 = index3 + 1;
              int result2;
              if (int.TryParse(source2.GetElement<string>(index3) ?? string.Empty, out result2))
              {
                List<string> source3 = list;
                int index4 = num4;
                int num5 = index4 + 1;
                string str1 = source3.GetElement<string>(index4) ?? string.Empty;
                if (!str1.IsNullOrEmpty())
                {
                  SoundPlayer.MapBGMInfo.Period[] periodArray1 = this.LoadPeriod(str1);
                  if (!periodArray1.IsNullOrEmpty<SoundPlayer.MapBGMInfo.Period>())
                  {
                    List<string> source4 = list;
                    int index5 = num5;
                    int num6 = index5 + 1;
                    int result3;
                    if (int.TryParse(source4.GetElement<string>(index5) ?? string.Empty, out result3))
                    {
                      List<string> source5 = list;
                      int index6 = num6;
                      int num7 = index6 + 1;
                      string str2 = source5.GetElement<string>(index6) ?? string.Empty;
                      if (!str2.IsNullOrEmpty())
                      {
                        SoundPlayer.MapBGMInfo.Period[] periodArray2 = this.LoadPeriod(str2);
                        if (!periodArray2.IsNullOrEmpty<SoundPlayer.MapBGMInfo.Period>())
                        {
                          Dictionary<int, SoundPlayer.MapBGMInfo> dictionary1;
                          if (!this.MapBGMInfoTable.TryGetValue(_chunkID, out dictionary1))
                          {
                            Dictionary<int, SoundPlayer.MapBGMInfo> dictionary2 = new Dictionary<int, SoundPlayer.MapBGMInfo>();
                            this.MapBGMInfoTable[_chunkID] = dictionary2;
                            dictionary1 = dictionary2;
                          }
                          dictionary1[result1] = new SoundPlayer.MapBGMInfo(result2, periodArray1, result3, periodArray2);
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      private SoundPlayer.MapBGMInfo.Period[] LoadPeriod(string _timePeriodStr)
      {
        MatchCollection matchCollection = this.regex.Matches(_timePeriodStr);
        if (0 >= matchCollection.Count)
          return (SoundPlayer.MapBGMInfo.Period[]) null;
        List<DateTime> dateTimeList = ListPool<DateTime>.Get();
        for (int index1 = 0; index1 < matchCollection.Count; ++index1)
        {
          List<DateTime> toRelease = ListPool<DateTime>.Get();
          string[] source1 = matchCollection[index1].Value.Replace(string.Empty, "<", ">").Split(this.separators, StringSplitOptions.RemoveEmptyEntries);
          if (!source1.IsNullOrEmpty<string>() && source1.Length == 2)
          {
            for (int index2 = 0; index2 < source1.Length; ++index2)
            {
              string[] source2 = (source1[index2] ?? string.Empty).Split(this.timeSeparators, StringSplitOptions.RemoveEmptyEntries);
              if (!source2.IsNullOrEmpty<string>() && source2.Length == 2)
              {
                int num1 = 0;
                string[] source3 = source2;
                int index3 = num1;
                int num2 = index3 + 1;
                int result1;
                if (int.TryParse(source3.GetElement<string>(index3) ?? string.Empty, out result1))
                {
                  string[] source4 = source2;
                  int index4 = num2;
                  int num3 = index4 + 1;
                  int result2;
                  if (int.TryParse(source4.GetElement<string>(index4) ?? string.Empty, out result2))
                    toRelease.Add(new DateTime(1, 1, 1, result1, result2, index2 != 0 ? 59 : 0));
                  else
                    break;
                }
                else
                  break;
              }
              else
                break;
            }
            if (toRelease.Count == 2)
              dateTimeList.AddRange((IEnumerable<DateTime>) toRelease);
            ListPool<DateTime>.Release(toRelease);
          }
        }
        SoundPlayer.MapBGMInfo.Period[] periodArray = (SoundPlayer.MapBGMInfo.Period[]) null;
        if (!dateTimeList.IsNullOrEmpty<DateTime>() && 0 < dateTimeList.Count && dateTimeList.Count % 2 == 0)
        {
          periodArray = new SoundPlayer.MapBGMInfo.Period[dateTimeList.Count / 2];
          for (int index = 0; index < dateTimeList.Count; index += 2)
          {
            periodArray[index / 2].Start = dateTimeList[index];
            periodArray[index / 2].End = dateTimeList[index + 1];
          }
        }
        ListPool<DateTime>.Release(dateTimeList);
        return periodArray;
      }

      private void LoadDefaultFootStep(string _directory, int _sexID)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(_directory, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          AssetBundleInfo info;
          ((AssetBundleInfo) ref info).\u002Ector(string.Empty, str, withoutExtension, string.Empty);
          if (AssetBundleCheck.IsFile(str, withoutExtension))
          {
            ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(info);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
              {
                List<string> list = excelData.list[index2].list;
                if (!list.IsNullOrEmpty<string>())
                {
                  int num = 0;
                  List<string> source = list;
                  int index3 = num;
                  int _idx = index3 + 1;
                  int result;
                  if (int.TryParse(source.GetElement<string>(index3) ?? string.Empty, out result))
                  {
                    AssetBundleInfo assetInfo = this.GetAssetInfo(list, ref _idx, true);
                    if (result == 0)
                    {
                      Dictionary<int, Dictionary<int, ValueTuple<int[], int[], int[]>>> _chunkTable;
                      if (!this.DefaultFootStepSETable.TryGetValue(_sexID, out _chunkTable))
                        this.DefaultFootStepSETable[_sexID] = _chunkTable = new Dictionary<int, Dictionary<int, ValueTuple<int[], int[], int[]>>>();
                      this.LoadDefaultFootStepList(assetInfo, withoutExtension, _chunkTable);
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadDefaultFootStepList(
        AssetBundleInfo _sheetInfo,
        string _ver,
        Dictionary<int, Dictionary<int, ValueTuple<int[], int[], int[]>>> _chunkTable)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int _idx = 0;
            int result;
            if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result))
            {
              AssetBundleInfo assetInfo = this.GetAssetInfo(list, ref _idx, false);
              Dictionary<int, ValueTuple<int[], int[], int[]>> _areaTable;
              if (!_chunkTable.TryGetValue(result, out _areaTable))
                _chunkTable[result] = _areaTable = new Dictionary<int, ValueTuple<int[], int[], int[]>>();
              this.LoadDefaultFootStepTable(assetInfo, _ver, _areaTable);
            }
          }
        }
      }

      private void LoadDefaultFootStepTable(
        AssetBundleInfo _sheetInfo,
        string _ver,
        Dictionary<int, ValueTuple<int[], int[], int[]>> _areaTable)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              int num3 = num2 + 1;
              List<string> source2 = list;
              int index3 = num3;
              int num4 = index3 + 1;
              string[] source3 = (source2.GetElement<string>(index3) ?? string.Empty).Split(this.separators, StringSplitOptions.RemoveEmptyEntries);
              List<string> source4 = list;
              int index4 = num4;
              int num5 = index4 + 1;
              string[] source5 = (source4.GetElement<string>(index4) ?? string.Empty).Split(this.separators, StringSplitOptions.RemoveEmptyEntries);
              List<string> source6 = list;
              int index5 = num5;
              int num6 = index5 + 1;
              string[] source7 = (source6.GetElement<string>(index5) ?? string.Empty).Split(this.separators, StringSplitOptions.RemoveEmptyEntries);
              if (!source3.IsNullOrEmpty<string>())
              {
                List<int> intList = ListPool<int>.Get();
                List<int> toRelease1 = ListPool<int>.Get();
                List<int> toRelease2 = ListPool<int>.Get();
                if (!source3.IsNullOrEmpty<string>())
                {
                  foreach (string s in source3)
                  {
                    int result2;
                    if (int.TryParse(s, out result2))
                      intList.Add(result2);
                  }
                }
                if (!source5.IsNullOrEmpty<string>())
                {
                  foreach (string s in source5)
                  {
                    int result2;
                    if (int.TryParse(s, out result2))
                      toRelease1.Add(result2);
                  }
                }
                if (!source7.IsNullOrEmpty<string>())
                {
                  foreach (string s in source7)
                  {
                    int result2;
                    if (int.TryParse(s, out result2))
                      toRelease2.Add(result2);
                  }
                }
                if (!intList.IsNullOrEmpty<int>())
                {
                  int[] numArray1 = new int[intList.Count];
                  int[] numArray2 = new int[toRelease1.Count];
                  int[] numArray3 = new int[toRelease2.Count];
                  for (int index6 = 0; index6 < numArray1.Length; ++index6)
                    numArray1[index6] = intList[index6];
                  for (int index6 = 0; index6 < numArray2.Length; ++index6)
                    numArray2[index6] = toRelease1[index6];
                  for (int index6 = 0; index6 < numArray3.Length; ++index6)
                    numArray3[index6] = toRelease2[index6];
                  _areaTable[result1] = new ValueTuple<int[], int[], int[]>(numArray1, numArray2, numArray3);
                }
                ListPool<int>.Release(intList);
                ListPool<int>.Release(toRelease1);
                ListPool<int>.Release(toRelease2);
              }
            }
          }
        }
      }

      private void LoadEnviroInfo(DefinePack _definePack)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(_definePack.ABDirectories.EnviroSEInfoList, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          if (AssetBundleCheck.IsFile(str, withoutExtension))
          {
            AssetBundleInfo info;
            ((AssetBundleInfo) ref info).\u002Ector(string.Empty, str, withoutExtension, string.Empty);
            ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(info);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
              {
                List<string> list = excelData.list[index2].list;
                if (!list.IsNullOrEmpty<string>())
                {
                  int num = 0;
                  List<string> source = list;
                  int index3 = num;
                  int _idx = index3 + 1;
                  int result;
                  if (int.TryParse(source.GetElement<string>(index3) ?? string.Empty, out result))
                  {
                    AssetBundleInfo assetInfo = this.GetAssetInfo(list, ref _idx, true);
                    switch (result)
                    {
                      case 0:
                        this.LoadEnviroPrefabList(assetInfo, withoutExtension);
                        continue;
                      case 1:
                        this.LoadEnviroAdjacentList(assetInfo, withoutExtension);
                        continue;
                      case 2:
                        this.LoadReverbInfo(assetInfo, withoutExtension);
                        continue;
                      default:
                        continue;
                    }
                  }
                }
              }
            }
          }
        }
      }

      private void LoadEnviroPrefabList(AssetBundleInfo _sheetInfo, string _ver)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int _idx = 0;
            int result;
            if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result))
              this.LoadEnviroPrefabInfoTable(this.GetAssetInfo(list, ref _idx, false), _ver, result);
          }
        }
      }

      private void LoadEnviroPrefabInfoTable(AssetBundleInfo _sheetInfo, string _ver, int _chunkID)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int _idx = 0;
            int result;
            if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result))
            {
              AssetBundleInfo assetInfo = this.GetAssetInfo(list, ref _idx, true);
              if (!((string) assetInfo.asset).IsNullOrEmpty() && !((string) assetInfo.assetbundle).IsNullOrEmpty() && !((string) assetInfo.manifest).IsNullOrEmpty())
              {
                Dictionary<int, AssetBundleInfo> dictionary1;
                if (!this.EnviroSEPrefabInfoTable.TryGetValue(_chunkID, out dictionary1))
                {
                  Dictionary<int, AssetBundleInfo> dictionary2 = new Dictionary<int, AssetBundleInfo>();
                  this.EnviroSEPrefabInfoTable[_chunkID] = dictionary2;
                  dictionary1 = dictionary2;
                }
                dictionary1[result] = assetInfo;
              }
            }
          }
        }
      }

      private void LoadEnviroAdjacentList(AssetBundleInfo _sheetInfo, string _ver)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int _idx = 0;
            int result;
            if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result))
              this.LoadEnviroAdjacentInfoTable(this.GetAssetInfo(list, ref _idx, false), _ver, result);
          }
        }
      }

      private void LoadEnviroAdjacentInfoTable(
        AssetBundleInfo _sheetInfo,
        string _ver,
        int _chunkID)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index1 = 1; index1 < excelData.MaxCell; ++index1)
        {
          List<string> list = excelData.list[index1].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int num1 = 0;
            List<string> source1 = list;
            int index2 = num1;
            int num2 = index2 + 1;
            int result1;
            if (int.TryParse(source1.GetElement<string>(index2) ?? string.Empty, out result1))
            {
              int num3 = num2 + 1;
              List<string> source2 = list;
              int index3 = num3;
              int num4 = index3 + 1;
              string[] source3 = (source2.GetElement<string>(index3) ?? string.Empty).Split(this.separators, StringSplitOptions.RemoveEmptyEntries);
              if (!source3.IsNullOrEmpty<string>())
                ;
              List<int> toRelease = ListPool<int>.Get();
              if (!source3.IsNullOrEmpty<string>())
              {
                foreach (string s in source3)
                {
                  int result2;
                  if (int.TryParse(s, out result2))
                    toRelease.Add(result2);
                }
              }
              int[] numArray = new int[toRelease.Count];
              for (int index4 = 0; index4 < numArray.Length; ++index4)
                numArray[index4] = toRelease[index4];
              Dictionary<int, int[]> dictionary;
              if (!this.EnviroSEAdjacentInfoTable.TryGetValue(_chunkID, out dictionary))
                this.EnviroSEAdjacentInfoTable[_chunkID] = dictionary = new Dictionary<int, int[]>();
              dictionary[result1] = numArray;
              ListPool<int>.Release(toRelease);
            }
          }
        }
      }

      private void LoadReverbInfo(AssetBundleInfo _sheetInfo, string _ver)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int _idx = 0;
            int result;
            if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result))
              this.LoadReverbPrefabTable(this.GetAssetInfo(list, ref _idx, false), _ver, result);
          }
        }
      }

      private void LoadReverbPrefabTable(AssetBundleInfo _sheetInfo, string _ver, int _chunkID)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int _idx = 0;
            int result;
            if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result))
            {
              AssetBundleInfo assetInfo = this.GetAssetInfo(list, ref _idx, true);
              if (!((string) assetInfo.assetbundle).IsNullOrEmpty() && !((string) assetInfo.asset).IsNullOrEmpty())
              {
                Dictionary<int, AssetBundleInfo> dictionary;
                if (!this.ReverbPrefabInfoTable.TryGetValue(_chunkID, out dictionary))
                  this.ReverbPrefabInfoTable[_chunkID] = dictionary = new Dictionary<int, AssetBundleInfo>();
                dictionary[result] = assetInfo;
              }
            }
          }
        }
      }

      private void LoadPersonalVoiceInfo(
        string _directory,
        Dictionary<int, Dictionary<int, AssetBundleInfo>> _setTable)
      {
        List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(_directory, false);
        if (nameListFromPath.IsNullOrEmpty<string>())
          return;
        nameListFromPath.Sort();
        for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
        {
          string str = nameListFromPath[index1];
          string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(str);
          if (AssetBundleCheck.IsFile(str, withoutExtension))
          {
            AssetBundleInfo info;
            ((AssetBundleInfo) ref info).\u002Ector(string.Empty, str, withoutExtension, string.Empty);
            ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(info);
            if (!Object.op_Equality((Object) excelData, (Object) null))
            {
              for (int index2 = 1; index2 < excelData.MaxCell; ++index2)
              {
                List<string> list = excelData.list[index2].list;
                if (!list.IsNullOrEmpty<string>())
                {
                  int num = 0;
                  List<string> source = list;
                  int index3 = num;
                  int _idx = index3 + 1;
                  int result;
                  if (int.TryParse(source.GetElement<string>(index3) ?? string.Empty, out result))
                  {
                    AssetBundleInfo assetInfo = this.GetAssetInfo(list, ref _idx, true);
                    Dictionary<int, AssetBundleInfo> _setTable1;
                    if (!_setTable.TryGetValue(result, out _setTable1))
                      _setTable[result] = _setTable1 = new Dictionary<int, AssetBundleInfo>();
                    this.LoadPersonalVoiceInfo(assetInfo, _setTable1);
                  }
                }
              }
            }
          }
        }
      }

      private void LoadPersonalVoiceInfo(
        AssetBundleInfo _sheetInfo,
        Dictionary<int, AssetBundleInfo> _setTable)
      {
        ExcelData excelData = AssetUtility.LoadAsset<ExcelData>(_sheetInfo);
        if (Object.op_Equality((Object) excelData, (Object) null))
          return;
        for (int index = 1; index < excelData.MaxCell; ++index)
        {
          List<string> list = excelData.list[index].list;
          if (!list.IsNullOrEmpty<string>())
          {
            int _idx = 0;
            int result;
            if (int.TryParse(list.GetElement<string>(_idx++) ?? string.Empty, out result))
            {
              AssetBundleInfo assetInfo = this.GetAssetInfo(list, ref _idx, true);
              if (!((string) assetInfo.assetbundle).IsNullOrEmpty() && !((string) assetInfo.asset).IsNullOrEmpty())
                _setTable[result] = assetInfo;
            }
          }
        }
      }
    }
  }
}
