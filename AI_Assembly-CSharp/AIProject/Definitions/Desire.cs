// Decompiled with JetBrains decompiler
// Type: AIProject.Definitions.Desire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEx;

namespace AIProject.Definitions
{
  public static class Desire
  {
    public static Dictionary<Desire.ActionType, Desire.ActionType> SickPairTable = new Dictionary<Desire.ActionType, Desire.ActionType>()
    {
      [Desire.ActionType.Cold2A] = Desire.ActionType.Cold2B,
      [Desire.ActionType.Cold3A] = Desire.ActionType.Cold3B,
      [Desire.ActionType.OverworkA] = Desire.ActionType.OverworkB,
      [Desire.ActionType.WeaknessA] = Desire.ActionType.WeaknessB
    };
    private static Desire.ActionType[] _sickFilterTable = new Desire.ActionType[9]
    {
      Desire.ActionType.Faint,
      Desire.ActionType.Cold2A,
      Desire.ActionType.Cold2B,
      Desire.ActionType.Cold2BMedicated,
      Desire.ActionType.Cold3A,
      Desire.ActionType.Cold3B,
      Desire.ActionType.Cold3BMedicated,
      Desire.ActionType.OverworkA,
      Desire.ActionType.OverworkB
    };
    public static readonly Desire.Type[] WithPlayerDesireTable = new Desire.Type[3]
    {
      Desire.Type.Gift,
      Desire.Type.Game,
      Desire.Type.H
    };
    private static Dictionary<Desire.Type, int> _desireKeyTable = new Dictionary<Desire.Type, int>()
    {
      [Desire.Type.Toilet] = 0,
      [Desire.Type.Bath] = 1,
      [Desire.Type.Sleep] = 2,
      [Desire.Type.Eat] = 3,
      [Desire.Type.Break] = 4,
      [Desire.Type.Gift] = 5,
      [Desire.Type.Want] = 6,
      [Desire.Type.Lonely] = 7,
      [Desire.Type.H] = 8,
      [Desire.Type.Dummy] = 9,
      [Desire.Type.Hunt] = 10,
      [Desire.Type.Game] = 11,
      [Desire.Type.Cook] = 12,
      [Desire.Type.Animal] = 13,
      [Desire.Type.Location] = 14,
      [Desire.Type.Drink] = 15
    };

    public static bool ContainsSickFilterTable(Desire.ActionType source)
    {
      foreach (Desire.ActionType actionType in Desire._sickFilterTable)
      {
        if (source == actionType)
          return true;
      }
      return false;
    }

    public static Dictionary<Desire.Type, string> NameTable { get; } = new Dictionary<Desire.Type, string>()
    {
      [Desire.Type.None] = "なし",
      [Desire.Type.Toilet] = "トイレ",
      [Desire.Type.Bath] = "風呂",
      [Desire.Type.Sleep] = "睡眠",
      [Desire.Type.Eat] = "飲食",
      [Desire.Type.Break] = "休憩",
      [Desire.Type.Gift] = "ギフト",
      [Desire.Type.Want] = "おねだり",
      [Desire.Type.Lonely] = "寂しい",
      [Desire.Type.H] = "H",
      [Desire.Type.Dummy] = "ダミー",
      [Desire.Type.Hunt] = "採取",
      [Desire.Type.Game] = "遊び",
      [Desire.Type.Cook] = "料理",
      [Desire.Type.Animal] = "動物",
      [Desire.Type.Location] = "ロケーション",
      [Desire.Type.Drink] = "飲む"
    };

    public static Dictionary<Desire.ActionType, Desire.Type> ModeTable { get; } = new Dictionary<Desire.ActionType, Desire.Type>()
    {
      [Desire.ActionType.Normal] = Desire.Type.None,
      [Desire.ActionType.Date] = Desire.Type.None,
      [Desire.ActionType.SearchBath] = Desire.Type.Bath,
      [Desire.ActionType.SearchToilet] = Desire.Type.Toilet,
      [Desire.ActionType.EndTaskEat] = Desire.Type.Eat,
      [Desire.ActionType.SearchActor] = Desire.Type.Lonely,
      [Desire.ActionType.WithPlayer] = Desire.Type.None,
      [Desire.ActionType.SearchGather] = Desire.Type.Hunt,
      [Desire.ActionType.SearchGift] = Desire.Type.Gift,
      [Desire.ActionType.EndTaskGift] = Desire.Type.Gift,
      [Desire.ActionType.SearchBreak] = Desire.Type.Break,
      [Desire.ActionType.SearchCook] = Desire.Type.Cook,
      [Desire.ActionType.SearchSleep] = Desire.Type.Sleep,
      [Desire.ActionType.EndTaskGimme] = Desire.Type.Want,
      [Desire.ActionType.EndTaskMasturbation] = Desire.Type.H,
      [Desire.ActionType.SearchH] = Desire.Type.H,
      [Desire.ActionType.EndTaskH] = Desire.Type.H,
      [Desire.ActionType.SearchRevRape] = Desire.Type.H,
      [Desire.ActionType.ReverseRape] = Desire.Type.H,
      [Desire.ActionType.SearchEat] = Desire.Type.Eat,
      [Desire.ActionType.SearchGame] = Desire.Type.Game,
      [Desire.ActionType.EndTaskTalkToPlayer] = Desire.Type.Lonely,
      [Desire.ActionType.EndTaskTalk] = Desire.Type.Lonely,
      [Desire.ActionType.ReceiveTalk] = Desire.Type.Lonely,
      [Desire.ActionType.Fight] = Desire.Type.Lonely,
      [Desire.ActionType.ReceiveFight] = Desire.Type.Lonely,
      [Desire.ActionType.SearchAnimal] = Desire.Type.Animal,
      [Desire.ActionType.EndTaskPetAnimal] = Desire.Type.Animal,
      [Desire.ActionType.EndTaskWildAnimal] = Desire.Type.Animal,
      [Desire.ActionType.SearchLocation] = Desire.Type.Location,
      [Desire.ActionType.EndTaskLocation] = Desire.Type.Location,
      [Desire.ActionType.WalkWithAgent] = Desire.Type.Game,
      [Desire.ActionType.ChaseToTalk] = Desire.Type.Lonely,
      [Desire.ActionType.ChaseToPairWalk] = Desire.Type.Lonely,
      [Desire.ActionType.SearchMasturbation] = Desire.Type.H,
      [Desire.ActionType.SearchItemForEat] = Desire.Type.Hunt,
      [Desire.ActionType.SearchEatSpot] = Desire.Type.Eat,
      [Desire.ActionType.SearchRevRape] = Desire.Type.H,
      [Desire.ActionType.SearchItemForDrink] = Desire.Type.Hunt,
      [Desire.ActionType.SearchDrinkSpot] = Desire.Type.Drink,
      [Desire.ActionType.WalkWithAgent] = Desire.Type.Game,
      [Desire.ActionType.EndTaskToilet] = Desire.Type.Toilet,
      [Desire.ActionType.EndTaskBath] = Desire.Type.Bath,
      [Desire.ActionType.GiftForceEncounter] = Desire.Type.Gift,
      [Desire.ActionType.SearchGimme] = Desire.Type.Want,
      [Desire.ActionType.EndTaskGame] = Desire.Type.Game,
      [Desire.ActionType.EndTaskGameThere] = Desire.Type.Game,
      [Desire.ActionType.EndTaskBreak] = Desire.Type.Break,
      [Desire.ActionType.EndTaskCook] = Desire.Type.Cook,
      [Desire.ActionType.EndTaskSleep] = Desire.Type.Sleep,
      [Desire.ActionType.EndTaskSecondSleep] = Desire.Type.Sleep,
      [Desire.ActionType.EndTaskGather] = Desire.Type.Hunt,
      [Desire.ActionType.EndTaskGatherForEat] = Desire.Type.Hunt,
      [Desire.ActionType.EndTaskGatherForDrink] = Desire.Type.Hunt,
      [Desire.ActionType.InviteSleep] = Desire.Type.Lonely,
      [Desire.ActionType.InviteSleepH] = Desire.Type.Lonely,
      [Desire.ActionType.InviteEat] = Desire.Type.Lonely,
      [Desire.ActionType.InviteBreak] = Desire.Type.Lonely,
      [Desire.ActionType.SearchWarpPoint] = Desire.Type.Game
    };

    public static int GetDesireKey(AIProject.EventType type)
    {
      foreach (ValueTuple<AIProject.EventType, Desire.Type> valuePair in Desire.ValuePairs)
      {
        if (type == (AIProject.EventType) valuePair.Item1)
          return Desire.GetDesireKey((Desire.Type) valuePair.Item2);
      }
      return -1;
    }

    public static int GetDesireKey(Desire.Type key)
    {
      int num;
      return !Desire._desireKeyTable.TryGetValue(key, out num) ? -1 : num;
    }

    public static Desire.Type DesireTypeFromValue(int value)
    {
      foreach (KeyValuePair<Desire.Type, int> keyValuePair in Desire._desireKeyTable)
      {
        if (keyValuePair.Value == value)
          return keyValuePair.Key;
      }
      return Desire.Type.None;
    }

    public static ValueTuple<AIProject.EventType, Desire.Type>[] FindAt(
      Desire.Type type)
    {
      List<ValueTuple<AIProject.EventType, Desire.Type>> toRelease = ListPool<ValueTuple<AIProject.EventType, Desire.Type>>.Get();
      foreach (ValueTuple<AIProject.EventType, Desire.Type> valuePair in Desire.ValuePairs)
      {
        if (type == (Desire.Type) valuePair.Item2)
          toRelease.Add(valuePair);
      }
      ValueTuple<AIProject.EventType, Desire.Type>[] array = toRelease.ToArray();
      ListPool<ValueTuple<AIProject.EventType, Desire.Type>>.Release(toRelease);
      return array;
    }

    public static ValueTuple<AIProject.EventType, Desire.Type>[] ValuePairs { get; } = new ValueTuple<AIProject.EventType, Desire.Type>[14]
    {
      new ValueTuple<AIProject.EventType, Desire.Type>(AIProject.EventType.Sleep, Desire.Type.Sleep),
      new ValueTuple<AIProject.EventType, Desire.Type>(AIProject.EventType.Break, Desire.Type.Break),
      new ValueTuple<AIProject.EventType, Desire.Type>(AIProject.EventType.Eat, Desire.Type.Eat),
      new ValueTuple<AIProject.EventType, Desire.Type>(AIProject.EventType.Toilet, Desire.Type.Toilet),
      new ValueTuple<AIProject.EventType, Desire.Type>(AIProject.EventType.Play, Desire.Type.Game),
      new ValueTuple<AIProject.EventType, Desire.Type>(AIProject.EventType.Search, Desire.Type.Hunt),
      new ValueTuple<AIProject.EventType, Desire.Type>(AIProject.EventType.Cook, Desire.Type.Cook),
      new ValueTuple<AIProject.EventType, Desire.Type>(AIProject.EventType.DressIn, Desire.Type.Bath),
      new ValueTuple<AIProject.EventType, Desire.Type>(AIProject.EventType.Bath, Desire.Type.Bath),
      new ValueTuple<AIProject.EventType, Desire.Type>(AIProject.EventType.DressOut, Desire.Type.Bath),
      new ValueTuple<AIProject.EventType, Desire.Type>(AIProject.EventType.Masturbation, Desire.Type.H),
      new ValueTuple<AIProject.EventType, Desire.Type>(AIProject.EventType.ShallowSleep, Desire.Type.Sleep),
      new ValueTuple<AIProject.EventType, Desire.Type>(AIProject.EventType.Location, Desire.Type.Location),
      new ValueTuple<AIProject.EventType, Desire.Type>(AIProject.EventType.Drink, Desire.Type.Drink)
    };

    [Flags]
    public enum Type
    {
      None = 0,
      Toilet = 1,
      Bath = 2,
      Sleep = 4,
      Eat = 8,
      Break = 16, // 0x00000010
      Gift = 32, // 0x00000020
      Want = 64, // 0x00000040
      Lonely = 128, // 0x00000080
      H = 256, // 0x00000100
      Dummy = 512, // 0x00000200
      Hunt = 1024, // 0x00000400
      Game = 2048, // 0x00000800
      Cook = 4096, // 0x00001000
      Animal = 8192, // 0x00002000
      Location = 16384, // 0x00004000
      Drink = 32768, // 0x00008000
    }

    public enum ActionType
    {
      Called = -1, // 0xFFFFFFFF
      Normal = 0,
      Date = 1,
      Onbu = 2,
      Idle = 3,
      SearchBath = 4,
      SearchToilet = 5,
      EndTaskEat = 6,
      SearchActor = 7,
      WithPlayer = 8,
      WithAgent = 9,
      WithMerchant = 10, // 0x0000000A
      SearchGather = 11, // 0x0000000B
      SearchGift = 12, // 0x0000000C
      EndTaskGift = 13, // 0x0000000D
      SearchBreak = 14, // 0x0000000E
      SearchCook = 15, // 0x0000000F
      SearchSleep = 16, // 0x00000010
      EndTaskGimme = 17, // 0x00000011
      SearchH = 18, // 0x00000012
      EndTaskMasturbation = 19, // 0x00000013
      Escape = 20, // 0x00000014
      EndTaskH = 21, // 0x00000015
      ChaseLesbianH = 22, // 0x00000016
      EndTaskLesbianMerchantH = 23, // 0x00000017
      ReverseRape = 25, // 0x00000019
      EndTaskItemBox = 26, // 0x0000001A
      Steal = 27, // 0x0000001B
      SearchEat = 28, // 0x0000001C
      AfterCook = 29, // 0x0000001D
      ShallowSleep = 31, // 0x0000001F
      EndTaskSleep = 32, // 0x00000020
      SearchAnimal = 33, // 0x00000021
      EndTaskPetAnimal = 34, // 0x00000022
      SearchGame = 35, // 0x00000023
      EndTaskGame = 36, // 0x00000024
      SearchPlayerToTalk = 37, // 0x00000025
      EndTaskTalkToPlayer = 38, // 0x00000026
      EndTaskTalk = 39, // 0x00000027
      ReceiveTalk = 40, // 0x00000028
      Fight = 41, // 0x00000029
      ReceiveFight = 42, // 0x0000002A
      EndTaskWildAnimal = 43, // 0x0000002B
      Encounter = 44, // 0x0000002C
      Faint = 45, // 0x0000002D
      Cold2A = 46, // 0x0000002E
      Cold2B = 47, // 0x0000002F
      Cold2BMedicated = 48, // 0x00000030
      Cold3A = 49, // 0x00000031
      Cold3B = 50, // 0x00000032
      Cold3BMedicated = 51, // 0x00000033
      OverworkA = 53, // 0x00000035
      OverworkB = 54, // 0x00000036
      Cure = 55, // 0x00000037
      SearchLocation = 56, // 0x00000038
      EndTaskLocation = 57, // 0x00000039
      EndTaskSleepAfterDate = 58, // 0x0000003A
      WalkWithAgent = 59, // 0x0000003B
      EndTaskGameWithAgent = 60, // 0x0000003C
      EndTaskToilet = 61, // 0x0000003D
      GiftForceEncounter = 62, // 0x0000003E
      GiftStandby = 63, // 0x0000003F
      SearchGimme = 64, // 0x00000040
      WalkWithAgentFollow = 65, // 0x00000041
      ChaseToTalk = 66, // 0x00000042
      ChaseToPairWalk = 67, // 0x00000043
      EndTaskGather = 68, // 0x00000044
      SearchMasturbation = 69, // 0x00000045
      GoTowardItemBox = 70, // 0x00000046
      EndTaskGameThere = 71, // 0x00000047
      EndTaskEatThere = 72, // 0x00000048
      SearchItemForEat = 73, // 0x00000049
      EndTaskGatherForEat = 74, // 0x0000004A
      SearchEatSpot = 75, // 0x0000004B
      SearchRevRape = 76, // 0x0000004C
      EndTaskPeeing = 77, // 0x0000004D
      SearchDrink = 78, // 0x0000004E
      SearchItemForDrink = 79, // 0x0000004F
      EndTaskGatherForDrink = 80, // 0x00000050
      SearchDrinkSpot = 81, // 0x00000051
      EndTaskDrink = 82, // 0x00000052
      EndTaskDrinkThere = 83, // 0x00000053
      EndTaskDressIn = 84, // 0x00000054
      EndTaskBath = 85, // 0x00000055
      EndTaskDressOut = 86, // 0x00000056
      EndTaskCook = 87, // 0x00000057
      EndTaskBreak = 88, // 0x00000058
      EndTaskSleepTogether = 89, // 0x00000059
      DiscussLesbianH = 90, // 0x0000005A
      GotoLesbianSpot = 91, // 0x0000005B
      GotoLesbianSpotFollow = 92, // 0x0000005C
      EndTaskLesbianH = 93, // 0x0000005D
      EndTaskSecondSleep = 94, // 0x0000005E
      WokenUp = 95, // 0x0000005F
      PhotoEncounter = 96, // 0x00000060
      FoundPeeping = 97, // 0x00000061
      GotoBath = 98, // 0x00000062
      GotoDressOut = 99, // 0x00000063
      WaitForCalled = 100, // 0x00000064
      Ovation = 101, // 0x00000065
      GoTowardFacialWash = 102, // 0x00000066
      EndTaskFacialWash = 103, // 0x00000067
      EndTaskSteal = 104, // 0x00000068
      GoTowardChestInSearchLoop = 105, // 0x00000069
      EndTaskChestInSearchLoop = 106, // 0x0000006A
      GotoClothChange = 107, // 0x0000006B
      EndTaskClothChange = 108, // 0x0000006C
      GotoRestoreCloth = 109, // 0x0000006D
      EndTaskRestoreCloth = 110, // 0x0000006E
      GotoHandWash = 111, // 0x0000006F
      EndTaskHandWash = 112, // 0x00000070
      SearchBirthdayGift = 113, // 0x00000071
      BirthdayGift = 114, // 0x00000072
      WeaknessA = 115, // 0x00000073
      WeaknessB = 116, // 0x00000074
      TakeHPoint = 117, // 0x00000075
      CommonSearchBreak = 118, // 0x00000076
      CommonBreak = 119, // 0x00000077
      CommonGameThere = 120, // 0x00000078
      ComeSleepTogether = 121, // 0x00000079
      ChaseYobai = 122, // 0x0000007A
      InviteSleep = 123, // 0x0000007B
      InviteSleepH = 124, // 0x0000007C
      InviteEat = 125, // 0x0000007D
      InviteBreak = 126, // 0x0000007E
      TakeSleepPoint = 127, // 0x0000007F
      TakeSleepHPoint = 128, // 0x00000080
      TakeEatPoint = 129, // 0x00000081
      TakeBreakPoint = 130, // 0x00000082
      SearchWarpPoint = 131, // 0x00000083
      Warp = 132, // 0x00000084
    }
  }
}
