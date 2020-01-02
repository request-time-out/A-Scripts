// Decompiled with JetBrains decompiler
// Type: AIChara.ChaFileDefine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace AIChara
{
  public static class ChaFileDefine
  {
    public static readonly Version ChaFileVersion = new Version("1.0.0");
    public static readonly Version ChaFileCustomVersion = new Version("0.0.0");
    public static readonly Version ChaFileFaceVersion = new Version("0.0.2");
    public static readonly Version ChaFileBodyVersion = new Version("0.0.1");
    public static readonly Version ChaFileHairVersion = new Version("0.0.3");
    public static readonly Version ChaFileCoordinateVersion = new Version("0.0.0");
    public static readonly Version ChaFileClothesVersion = new Version("0.0.0");
    public static readonly Version ChaFileAccessoryVersion = new Version("0.0.0");
    public static readonly Version ChaFileParameterVersion = new Version("0.0.1");
    public static readonly Version ChaFileStatusVersion = new Version("0.0.0");
    public static readonly Version ChaFileGameInfoVersion = new Version("0.0.0");
    public static readonly string[] cf_bodyshapename = new string[33]
    {
      "身長",
      "胸サイズ",
      "胸上下位置",
      "胸の左右開き",
      "胸の左右位置",
      "胸上下角度",
      "胸の尖り",
      "乳輪の膨らみ",
      "乳首太さ",
      "頭サイズ",
      "首周り幅",
      "首周り奥",
      "胴体肩周り幅",
      "胴体肩周り奥",
      "胴体上幅",
      "胴体上奥",
      "胴体下幅",
      "胴体下奥",
      "ウエスト位置",
      "腰上幅",
      "腰上奥",
      "腰下幅",
      "腰下奥",
      "尻",
      "尻角度",
      "太もも上",
      "太もも下",
      "ふくらはぎ",
      "足首",
      "肩",
      "上腕",
      "前腕",
      "乳首立ち"
    };
    public static readonly int[] cf_BustShapeMaskID = new int[8]
    {
      2,
      3,
      4,
      5,
      6,
      7,
      8,
      32
    };
    public static readonly int[] cf_ShapeMaskBust = new int[5]
    {
      0,
      1,
      2,
      3,
      4
    };
    public static readonly int[] cf_ShapeMaskNip = new int[2]
    {
      5,
      6
    };
    public static readonly float[] cf_bodyInitValue = new float[33]
    {
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.0f
    };
    public static readonly string[] cf_headshapename = new string[59]
    {
      "顔全体横幅",
      "顔上部前後",
      "顔上部上下",
      "顔下部前後",
      "顔下部横幅",
      "顎横幅",
      "顎上下",
      "顎前後",
      "顎角度",
      "顎下部上下",
      "顎先幅",
      "顎先上下",
      "顎先前後",
      "頬下部上下",
      "頬下部前後",
      "頬下部幅",
      "頬上部上下",
      "頬上部前後",
      "頬上部幅",
      "目上下",
      "目横位置",
      "目前後",
      "目の横幅",
      "目の縦幅",
      "目の角度Z軸",
      "目の角度Y軸",
      "目頭左右位置",
      "目尻左右位置",
      "目頭上下位置",
      "目尻上下位置",
      "まぶた形状１",
      "まぶた形状２",
      "鼻全体上下",
      "鼻全体前後",
      "鼻全体角度X軸",
      "鼻全体横幅",
      "鼻筋高さ",
      "鼻筋横幅",
      "鼻筋形状",
      "小鼻横幅",
      "小鼻上下",
      "小鼻前後",
      "小鼻角度X軸",
      "小鼻角度Z軸",
      "鼻先高さ",
      "鼻先角度X軸",
      "鼻先サイズ",
      "口上下",
      "口横幅",
      "口縦幅",
      "口前後位置",
      "口形状上",
      "口形状下",
      "口形状口角",
      "耳サイズ",
      "耳角度Y軸",
      "耳角度Z軸",
      "耳上部形状",
      "耳下部形状"
    };
    public static readonly int[] cf_MouthShapeMaskID = new int[10]
    {
      3,
      6,
      11,
      47,
      48,
      49,
      50,
      51,
      52,
      53
    };
    public static readonly float[] cf_MouthShapeDefault = new float[10]
    {
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f
    };
    public static readonly float[] cf_faceInitValue = new float[59]
    {
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f,
      0.5f
    };
    public const float MaleDefaultHeight = 0.75f;
    public const int AccessoryCategoryTypeNone = 120;
    public const int AccessoryColorNum = 4;
    public const int AccessoryCorrectNum = 2;
    public const int AccessorySlotNum = 20;
    public const int CustomPaintNum = 2;
    public const int ParamAttributeNum = 3;
    public const int ClothesColorNum = 3;
    public const int ClothesMaterialNum = 3;
    public const int FlavorKindNum = 8;
    public const int DesireKindNum = 16;
    public const int SkillSlotNum = 5;
    public const float NipStandLimit = 0.8f;
    public const float NipStandPlusH = 0.2f;
    public const float SkinGlossLimit = 0.8f;
    public const float SkinGlossPlusH = 0.2f;
    public const float VoicePitchMin = 0.94f;
    public const float VoicePitchMax = 1.06f;
    public const string CharaFileFemaleDir = "chara/female/";
    public const string CharaFileMaleDir = "chara/male/";
    public const string OldCharaFileFemaleDir = "chara/old/female/";
    public const string OldCharaFileEtcDir = "chara/old/etc/";
    public const string CoordinateFileFemaleDir = "coordinate/female/";
    public const string CoordinateFileMaleDir = "coordinate/male/";
    public const int LoadError_Tag = -1;
    public const int LoadError_Version = -2;
    public const int LoadError_ProductNo = -3;
    public const int LoadError_EndOfStream = -4;
    public const int LoadError_OnlyPNG = -5;
    public const int LoadError_FileNotExist = -6;
    public const int LoadError_ETC = -999;
    public const int M_DefHeadID = 0;
    public const int M_DefBodyID = 0;
    public const int M_DefHairBackID = 0;
    public const int M_DefHairFrontID = 2;
    public const int M_DefHairSideID = 0;
    public const int M_DefHairOptionID = 0;
    public const int M_DefClothesTopID = 0;
    public const int M_DefClothesBotID = 0;
    public const int M_DefClothesGlovesID = 0;
    public const int M_DefClothesShoesID = 0;
    public const int F_DefHeadID = 0;
    public const int F_DefBodyID = 0;
    public const int F_DefHairBackID = 0;
    public const int F_DefHairFrontID = 1;
    public const int F_DefHairSideID = 0;
    public const int F_DefHairOptionID = 0;
    public const int F_DefClothesTopID = 0;
    public const int F_DefClothesBotID = 0;
    public const int F_DefClothesInnerTID = 0;
    public const int F_DefClothesInnerBID = 0;
    public const int F_DefClothesGlovesID = 0;
    public const int F_DefClothesPanstID = 0;
    public const int F_DefClothesSocksID = 0;
    public const int F_DefClothesShoesID = 0;
    public const string Default_Chara_UserID = "illusion-2019-1025-xxxx-aisyoujyocha";
    public const int cf_ShapeMaskNipStand = 7;

    public static string GetMonthInEnglish(int month)
    {
      return new string[12]
      {
        "Jan.",
        "Feb.",
        "Mar.",
        "Apr.",
        "May",
        "June",
        "July",
        "Aug.",
        "Sept.",
        "Oct.",
        "Nov.",
        "Dec."
      }[month];
    }

    public static string GetBirthdayStr(int month, int day, string culture = "ja-JP")
    {
      string str = string.Empty;
      switch (culture)
      {
        case "ja-JP":
          str = month.ToString() + "月" + day.ToString() + "日";
          break;
        case "en-US":
          str = ChaFileDefine.GetMonthInEnglish(month) + " " + day.ToString();
          break;
      }
      return str;
    }

    public enum BodyShapeIdx
    {
      Height,
      BustSize,
      BustY,
      BustRotX,
      BustX,
      BustRotY,
      BustSharp,
      AreolaBulge,
      NipWeight,
      HeadSize,
      NeckW,
      NeckZ,
      BodyShoulderW,
      BodyShoulderZ,
      BodyUpW,
      BodyUpZ,
      BodyLowW,
      BodyLowZ,
      WaistY,
      WaistUpW,
      WaistUpZ,
      WaistLowW,
      WaistLowZ,
      Hip,
      HipRotX,
      ThighUp,
      ThighLow,
      Calf,
      Ankle,
      Shoulder,
      ArmUp,
      ArmLow,
      NipStand,
    }

    public enum FaceShapeIdx
    {
      FaceBaseW,
      FaceUpZ,
      FaceUpY,
      FaceLowZ,
      FaceLowW,
      ChinW,
      ChinY,
      ChinZ,
      ChinRot,
      ChinLowY,
      ChinTipW,
      ChinTipY,
      ChinTipZ,
      CheekLowY,
      CheekLowZ,
      CheekLowW,
      CheekUpY,
      CheekUpZ,
      CheekUpW,
      EyeY,
      EyeX,
      EyeZ,
      EyeW,
      EyeH,
      EyeRotZ,
      EyeRotY,
      EyeInX,
      EyeOutX,
      EyeInY,
      EyeOutY,
      EyelidForm01,
      EyelidForm02,
      NoseAllY,
      NoseAllZ,
      NoseAllRotX,
      NoseAllW,
      NoseBridgeH,
      NoseBridgeW,
      NoseBridgeForm,
      NoseWingW,
      NoseWingY,
      NoseWingZ,
      NoseWingRotX,
      NoseWingRotZ,
      NoseH,
      NoseRotX,
      NoseSize,
      MouthY,
      MouthW,
      MouthH,
      MouthZ,
      MouthUpForm,
      MouthLowForm,
      MouthCornerForm,
      EarSize,
      EarRotY,
      EarRotZ,
      EarUpForm,
      EarLowForm,
    }

    public enum HairKind
    {
      back,
      front,
      side,
      option,
    }

    public enum ClothesKind
    {
      top,
      bot,
      inner_t,
      inner_b,
      gloves,
      panst,
      socks,
      shoes,
    }

    public enum SiruParts
    {
      SiruKao,
      SiruFrontTop,
      SiruFrontBot,
      SiruBackTop,
      SiruBackBot,
    }
  }
}
