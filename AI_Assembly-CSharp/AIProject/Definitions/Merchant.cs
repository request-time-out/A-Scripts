// Decompiled with JetBrains decompiler
// Type: AIProject.Definitions.Merchant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace AIProject.Definitions
{
  public static class Merchant
  {
    public static Dictionary<Merchant.ActionType, Merchant.StateType> StateTypeTable { get; } = new Dictionary<Merchant.ActionType, Merchant.StateType>()
    {
      [Merchant.ActionType.Absent] = Merchant.StateType.Absent,
      [Merchant.ActionType.ToAbsent] = Merchant.StateType.Absent,
      [Merchant.ActionType.Wait] = Merchant.StateType.Wait,
      [Merchant.ActionType.ToWait] = Merchant.StateType.Wait,
      [Merchant.ActionType.Search] = Merchant.StateType.Search,
      [Merchant.ActionType.ToSearch] = Merchant.StateType.Search,
      [Merchant.ActionType.TalkWithPlayer] = Merchant.StateType.TalkWithPlayer,
      [Merchant.ActionType.TalkWithAgent] = Merchant.StateType.TalkWithAgent,
      [Merchant.ActionType.HWithPlayer] = Merchant.StateType.HWithPlayer,
      [Merchant.ActionType.HWithAgent] = Merchant.StateType.HWithAgent,
      [Merchant.ActionType.Encounter] = Merchant.StateType.Encounter,
      [Merchant.ActionType.Idle] = Merchant.StateType.Idle,
      [Merchant.ActionType.GotoLesbianSpotFollow] = Merchant.StateType.GotoLesbianSpotFollow
    };

    public static Dictionary<int, Merchant.EventType> EventTypeTable { get; } = new Dictionary<int, Merchant.EventType>()
    {
      [0] = Merchant.EventType.Wait,
      [1] = Merchant.EventType.Search
    };

    public static Dictionary<Merchant.StateType, MerchantActor.MerchantSchedule.MerchantEvent> ScheduleTaskTable { get; } = new Dictionary<Merchant.StateType, MerchantActor.MerchantSchedule.MerchantEvent>()
    {
      [Merchant.StateType.Absent] = new MerchantActor.MerchantSchedule.MerchantEvent(Merchant.ActionType.ToAbsent, Merchant.ActionType.Absent),
      [Merchant.StateType.Wait] = new MerchantActor.MerchantSchedule.MerchantEvent(Merchant.ActionType.ToWait, Merchant.ActionType.Wait),
      [Merchant.StateType.Search] = new MerchantActor.MerchantSchedule.MerchantEvent(Merchant.ActionType.ToSearch, Merchant.ActionType.Search)
    };

    public static List<Merchant.ActionType> ChangeableModeList { get; } = new List<Merchant.ActionType>()
    {
      Merchant.ActionType.Absent,
      Merchant.ActionType.ToSearch,
      Merchant.ActionType.Wait,
      Merchant.ActionType.ToWait
    };

    public static List<Merchant.ActionType> NormalModeList { get; } = new List<Merchant.ActionType>()
    {
      Merchant.ActionType.Absent,
      Merchant.ActionType.ToAbsent,
      Merchant.ActionType.Search,
      Merchant.ActionType.ToSearch,
      Merchant.ActionType.Wait,
      Merchant.ActionType.ToWait
    };

    public static List<Merchant.ActionType> EncountList { get; } = new List<Merchant.ActionType>()
    {
      Merchant.ActionType.ToAbsent,
      Merchant.ActionType.ToWait,
      Merchant.ActionType.Wait,
      Merchant.ActionType.ToSearch,
      Merchant.ActionType.Search
    };

    public enum StateType
    {
      Absent,
      Wait,
      Search,
      TalkWithPlayer,
      TalkWithAgent,
      HWithPlayer,
      HWithAgent,
      Encounter,
      Idle,
      GotoLesbianSpotFollow,
    }

    public enum ActionType
    {
      ToAbsent,
      Absent,
      ToWait,
      Wait,
      ToSearch,
      Search,
      TalkWithPlayer,
      TalkWithAgent,
      HWithPlayer,
      HWithAgent,
      Encounter,
      Idle,
      GotoLesbianSpotFollow,
    }

    [Flags]
    public enum EventType
    {
      Wait = 1,
      Search = 2,
    }
  }
}
