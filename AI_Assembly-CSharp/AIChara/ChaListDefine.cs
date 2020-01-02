// Decompiled with JetBrains decompiler
// Type: AIChara.ChaListDefine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace AIChara
{
  public static class ChaListDefine
  {
    public static readonly Version CheckItemVersion = new Version("0.0.0");
    public static readonly string CheckItemFile = "save/checkitem.dat";

    public static string GetCategoryName(int no)
    {
      switch (no)
      {
        case 348:
          return "パターンテクスチャ";
        case 350:
          return "アクセサリ(なし)";
        case 351:
          return "アクセサリ頭";
        case 352:
          return "アクセサリ耳";
        case 353:
          return "アクセサリ眼鏡";
        case 354:
          return "アクセサリ顔";
        case 355:
          return "アクセサリ首";
        case 356:
          return "アクセサリ肩";
        case 357:
          return "アクセサリ胸";
        case 358:
          return "アクセサリ腰";
        case 359:
          return "アクセサリ背中";
        case 360:
          return "アクセサリ腕";
        case 361:
          return "アクセサリ手";
        case 362:
          return "アクセサリ脚";
        case 363:
          return "アクセサリ股間";
        default:
          switch (no - 313)
          {
            case 0:
              return "ペイント顔・体";
            case 1:
              return "眉毛";
            case 2:
              return "睫毛";
            case 3:
              return "アイシャドウ";
            case 4:
              return "瞳";
            case 5:
              return "瞳孔";
            case 6:
              return "ハイライト";
            case 7:
              return "チーク";
            case 9:
              return "リップ";
            case 10:
              return "ホクロ";
            case 21:
              return "乳首";
            case 22:
              return "アンダーヘア";
            case 23:
              return "肌のカラープリセット";
            default:
              switch (no)
              {
                case 0:
                  return "キャラサンプル男";
                case 1:
                  return "キャラサンプル女";
                case 3:
                  return "タイプ別初期値";
                case 4:
                  return "願望設定初期値";
                case 5:
                  return "サンプル音声";
                case 6:
                  return "ホクロの配置設定";
                case 7:
                  return "フェイスペイントの配置設定";
                case 8:
                  return "ボディーペイントの配置設定";
                default:
                  switch (no - 240)
                  {
                    case 0:
                      return "女服上";
                    case 1:
                      return "女服下";
                    case 2:
                      return "女インナー上";
                    case 3:
                      return "女インナー下";
                    case 4:
                      return "女手袋";
                    case 5:
                      return "女パンスト";
                    case 6:
                      return "女靴下";
                    case 7:
                      return "女靴";
                    default:
                      switch (no - 500)
                      {
                        case 0:
                          return "カスタム男ポーズ";
                        case 1:
                          return "カスタム女ポーズ";
                        case 2:
                          return "カスタム男眉パターン";
                        case 3:
                          return "カスタム女眉パターン";
                        case 4:
                          return "カスタム男目パターン";
                        case 5:
                          return "カスタム女目パターン";
                        case 6:
                          return "カスタム男口パターン";
                        case 7:
                          return "カスタム女口パターン";
                        default:
                          switch (no - 300)
                          {
                            case 0:
                              return "後ろ髪";
                            case 1:
                              return "前髪";
                            case 2:
                              return "横髪";
                            case 3:
                              return "エクステ";
                            case 5:
                              return "髪の毛のカラープリセット";
                            default:
                              switch (no - 140)
                              {
                                case 0:
                                  return "男服上";
                                case 1:
                                  return "男服下";
                                case 4:
                                  return "男手袋";
                                case 7:
                                  return "男靴";
                                default:
                                  switch (no - 110)
                                  {
                                    case 0:
                                      return "男頭";
                                    case 1:
                                      return "男肌";
                                    case 2:
                                      return "男シワ";
                                    default:
                                      switch (no - 131)
                                      {
                                        case 0:
                                          return "男肌";
                                        case 1:
                                          return "男肉感";
                                        case 2:
                                          return "男日焼け";
                                        default:
                                          switch (no - 210)
                                          {
                                            case 0:
                                              return "女頭";
                                            case 1:
                                              return "女肌";
                                            case 2:
                                              return "女シワ";
                                            default:
                                              switch (no - 231)
                                              {
                                                case 0:
                                                  return "女肌";
                                                case 1:
                                                  return "女肉感";
                                                case 2:
                                                  return "女日焼け";
                                                default:
                                                  return no == 121 ? "男ヒゲ" : string.Empty;
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

    public enum CategoryNo
    {
      unknown = -1, // 0xFFFFFFFF
      cha_sample_m = 0,
      cha_sample_f = 1,
      init_type_param = 3,
      init_wish_param = 4,
      cha_sample_voice = 5,
      mole_layout = 6,
      facepaint_layout = 7,
      bodypaint_layout = 8,
      mo_head = 110, // 0x0000006E
      mt_skin_f = 111, // 0x0000006F
      mt_detail_f = 112, // 0x00000070
      mt_beard = 121, // 0x00000079
      mt_skin_b = 131, // 0x00000083
      mt_detail_b = 132, // 0x00000084
      mt_sunburn = 133, // 0x00000085
      mo_top = 140, // 0x0000008C
      mo_bot = 141, // 0x0000008D
      mo_gloves = 144, // 0x00000090
      mo_shoes = 147, // 0x00000093
      fo_head = 210, // 0x000000D2
      ft_skin_f = 211, // 0x000000D3
      ft_detail_f = 212, // 0x000000D4
      ft_skin_b = 231, // 0x000000E7
      ft_detail_b = 232, // 0x000000E8
      ft_sunburn = 233, // 0x000000E9
      fo_top = 240, // 0x000000F0
      fo_bot = 241, // 0x000000F1
      fo_inner_t = 242, // 0x000000F2
      fo_inner_b = 243, // 0x000000F3
      fo_gloves = 244, // 0x000000F4
      fo_panst = 245, // 0x000000F5
      fo_socks = 246, // 0x000000F6
      fo_shoes = 247, // 0x000000F7
      so_hair_b = 300, // 0x0000012C
      so_hair_f = 301, // 0x0000012D
      so_hair_s = 302, // 0x0000012E
      so_hair_o = 303, // 0x0000012F
      preset_hair_color = 305, // 0x00000131
      st_paint = 313, // 0x00000139
      st_eyebrow = 314, // 0x0000013A
      st_eyelash = 315, // 0x0000013B
      st_eyeshadow = 316, // 0x0000013C
      st_eye = 317, // 0x0000013D
      st_eyeblack = 318, // 0x0000013E
      st_eye_hl = 319, // 0x0000013F
      st_cheek = 320, // 0x00000140
      st_lip = 322, // 0x00000142
      st_mole = 323, // 0x00000143
      st_nip = 334, // 0x0000014E
      st_underhair = 335, // 0x0000014F
      preset_skin_color = 336, // 0x00000150
      st_pattern = 348, // 0x0000015C
      ao_none = 350, // 0x0000015E
      ao_head = 351, // 0x0000015F
      ao_ear = 352, // 0x00000160
      ao_glasses = 353, // 0x00000161
      ao_face = 354, // 0x00000162
      ao_neck = 355, // 0x00000163
      ao_shoulder = 356, // 0x00000164
      ao_chest = 357, // 0x00000165
      ao_waist = 358, // 0x00000166
      ao_back = 359, // 0x00000167
      ao_arm = 360, // 0x00000168
      ao_hand = 361, // 0x00000169
      ao_leg = 362, // 0x0000016A
      ao_kokan = 363, // 0x0000016B
      custom_pose_m = 500, // 0x000001F4
      custom_pose_f = 501, // 0x000001F5
      custom_eyebrow_m = 502, // 0x000001F6
      custom_eyebrow_f = 503, // 0x000001F7
      custom_eye_m = 504, // 0x000001F8
      custom_eye_f = 505, // 0x000001F9
      custom_mouth_m = 506, // 0x000001FA
      custom_mouth_f = 507, // 0x000001FB
    }

    public enum KeyType
    {
      Unknown = -1, // 0xFFFFFFFF
      ListIndex = 0,
      Category = 1,
      DistributionNo = 2,
      ID = 3,
      Kind = 4,
      Possess = 5,
      Name = 6,
      EN_US = 7,
      ZH_CN = 8,
      ZH_TW = 9,
      MainManifest = 10, // 0x0000000A
      MainAB = 11, // 0x0000000B
      MainData = 12, // 0x0000000C
      MainData02 = 13, // 0x0000000D
      Weights = 14, // 0x0000000E
      RingOff = 15, // 0x0000000F
      AddScale = 16, // 0x00000010
      AddTex = 17, // 0x00000011
      CenterScale = 18, // 0x00000012
      CenterX = 19, // 0x00000013
      CenterY = 20, // 0x00000014
      ColorMask02Tex = 21, // 0x00000015
      ColorMask03Tex = 22, // 0x00000016
      ColorMaskTex = 23, // 0x00000017
      Coordinate = 24, // 0x00000018
      Data01 = 25, // 0x00000019
      Data02 = 26, // 0x0000001A
      Data03 = 27, // 0x0000001B
      Eye01 = 28, // 0x0000001C
      Eye02 = 29, // 0x0000001D
      Eye03 = 30, // 0x0000001E
      EyeMax01 = 31, // 0x0000001F
      EyeMax02 = 32, // 0x00000020
      EyeMax03 = 33, // 0x00000021
      Eyebrow01 = 34, // 0x00000022
      Eyebrow02 = 35, // 0x00000023
      Eyebrow03 = 36, // 0x00000024
      EyeHiLight01 = 37, // 0x00000025
      EyeHiLight02 = 38, // 0x00000026
      EyeHiLight03 = 39, // 0x00000027
      GlossTex = 40, // 0x00000028
      HeadID = 41, // 0x00000029
      KokanHide = 42, // 0x0000002A
      MainTex = 43, // 0x0000002B
      MainTex02 = 44, // 0x0000002C
      MainTex03 = 45, // 0x0000002D
      MainTexAB = 46, // 0x0000002E
      MatData = 47, // 0x0000002F
      Mouth01 = 48, // 0x00000030
      Mouth02 = 49, // 0x00000031
      Mouth03 = 50, // 0x00000032
      MouthMax01 = 51, // 0x00000033
      MouthMax02 = 52, // 0x00000034
      MouthMax03 = 53, // 0x00000035
      MoveX = 54, // 0x00000036
      MoveY = 55, // 0x00000037
      NormalMapTex = 56, // 0x00000038
      NotBra = 57, // 0x00000039
      OcclusionMapTex = 58, // 0x0000003A
      OverBodyMask = 59, // 0x0000003B
      OverBodyMaskAB = 60, // 0x0000003C
      OverBraMask = 61, // 0x0000003D
      OverBraMaskAB = 62, // 0x0000003E
      BreakDisableMask = 63, // 0x0000003F
      OverInnerTBMask = 64, // 0x00000040
      OverInnerTBMaskAB = 65, // 0x00000041
      OverInnerBMask = 66, // 0x00000042
      OverInnerBMaskAB = 67, // 0x00000043
      OverPanstMask = 68, // 0x00000044
      OverPanstMaskAB = 69, // 0x00000045
      OverBodyBMask = 70, // 0x00000046
      OverBodyBMaskAB = 71, // 0x00000047
      Parent = 72, // 0x00000048
      PosX = 73, // 0x00000049
      PosY = 74, // 0x0000004A
      Preset = 75, // 0x0000004B
      Rot = 76, // 0x0000004C
      Scale = 77, // 0x0000004D
      SetHair = 78, // 0x0000004E
      ShapeAnime = 79, // 0x0000004F
      StateType = 80, // 0x00000050
      ThumbAB = 81, // 0x00000051
      ThumbTex = 82, // 0x00000052
      Clip = 83, // 0x00000053
      OverBraType = 84, // 0x00000054
      Pattern = 85, // 0x00000055
      Target = 86, // 0x00000056
      Correct = 87, // 0x00000057
      TempLow = 88, // 0x00000058
      TempUp = 89, // 0x00000059
      MoodLow = 90, // 0x0000005A
      MoodUp = 91, // 0x0000005B
      TexManifest = 92, // 0x0000005C
      TexAB = 93, // 0x0000005D
      TexD = 94, // 0x0000005E
      TexC = 95, // 0x0000005F
      FS_00 = 96, // 0x00000060
      FS_01 = 97, // 0x00000061
      FS_02 = 98, // 0x00000062
      FS_03 = 99, // 0x00000063
      FS_04 = 100, // 0x00000064
      FS_05 = 101, // 0x00000065
      FS_06 = 102, // 0x00000066
      FS_07 = 103, // 0x00000067
      DD_00 = 104, // 0x00000068
      DD_01 = 105, // 0x00000069
      DD_02 = 106, // 0x0000006A
      DD_03 = 107, // 0x0000006B
      DD_04 = 108, // 0x0000006C
      DD_05 = 109, // 0x0000006D
      DD_06 = 110, // 0x0000006E
      DD_07 = 111, // 0x0000006F
      DD_08 = 112, // 0x00000070
      DD_09 = 113, // 0x00000071
      DD_10 = 114, // 0x00000072
      DD_11 = 115, // 0x00000073
      DD_12 = 116, // 0x00000074
      DD_13 = 117, // 0x00000075
      DD_14 = 118, // 0x00000076
      DD_15 = 119, // 0x00000077
      DB_00 = 120, // 0x00000078
      DB_01 = 121, // 0x00000079
      DB_02 = 122, // 0x0000007A
      DB_03 = 123, // 0x0000007B
      DB_04 = 124, // 0x0000007C
      DB_05 = 125, // 0x0000007D
      DB_06 = 126, // 0x0000007E
      DB_07 = 127, // 0x0000007F
      DB_08 = 128, // 0x00000080
      DB_09 = 129, // 0x00000081
      DB_10 = 130, // 0x00000082
      DB_11 = 131, // 0x00000083
      DB_12 = 132, // 0x00000084
      DB_13 = 133, // 0x00000085
      DB_14 = 134, // 0x00000086
      DB_15 = 135, // 0x00000087
      Motivation = 136, // 0x00000088
      Immoral = 137, // 0x00000089
      SampleH = 138, // 0x0000008A
      SampleS = 139, // 0x0000008B
      SampleV = 140, // 0x0000008C
      TopH = 141, // 0x0000008D
      TopS = 142, // 0x0000008E
      TopV = 143, // 0x0000008F
      BaseH = 144, // 0x00000090
      BaseS = 145, // 0x00000091
      BaseV = 146, // 0x00000092
      UnderH = 147, // 0x00000093
      UnderS = 148, // 0x00000094
      UnderV = 149, // 0x00000095
      SpecularH = 150, // 0x00000096
      SpecularS = 151, // 0x00000097
      SpecularV = 152, // 0x00000098
      Metallic = 153, // 0x00000099
      Smoothness = 154, // 0x0000009A
      IKAB = 155, // 0x0000009B
      IKData = 156, // 0x0000009C
    }
  }
}
