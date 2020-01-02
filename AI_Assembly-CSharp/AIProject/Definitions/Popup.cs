// Decompiled with JetBrains decompiler
// Type: AIProject.Definitions.Popup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace AIProject.Definitions
{
  public static class Popup
  {
    public static class Warning
    {
      public enum Type
      {
        PouchIsFull,
        ChestIsFull,
        NonFishFood,
        NotOpenThisSide,
        IsLocked,
        CantFix,
        CanOpenWithElec,
        EquipedItemRankIsLow,
        NotSetFishingRod,
        NotSetShavel,
        NotSetPickaxe,
        NotSetInsectNet,
        IsBroken,
        DontReactAlone,
        InsufficientBattery,
        PouchAndChestIsFull,
      }
    }

    public static class Request
    {
      public enum Type
      {
        GeneratorRepair,
        ShipRepair,
        Pod,
        Cuby,
        ForestBridge,
        RuinsDoor,
        StationValve,
      }
    }

    public static class StorySupport
    {
      public enum Type
      {
        Start,
        ExamineAround,
        TalkToGirlPart1,
        GetDriftwood,
        CraftFishingRod,
        EquippedFishingRod,
        FishingGetFish,
        TalkToGirlPart2,
        FindCookFishPlace,
        TalkToGirlPart3,
        ExamineNearbySignboard,
        ArrangeKitchen,
        CookFish,
        TalkToGirlPart4,
        FollowGirl,
        ExamineAbandonedHouse,
        GrowGirls1,
        ExamineStoryPoint1,
        ExamineNextStoryPoint1,
        GrowGirls2,
        ExamineStoryPoint2,
        ExamineNextStoryPoint2,
        GrowGirls3,
        ExamineStoryPoint3,
        RepairGenerator,
        ExamineStoryPoint4,
        ExamineNextStoryPoint3,
        RepairShip,
        Complete,
        GotoDifferentIslandByBoat,
      }
    }

    public static class Tutorial
    {
      public enum Type
      {
        Collection,
        Craft,
        Kitchen,
        Girl,
        Communication,
        H,
        DevicePoint,
        Sleep,
        Equipment,
        Fishing,
        Shop,
        Chest,
        BasePoint,
        Housing,
        Dressing,
        FarmPlant,
      }
    }
  }
}
