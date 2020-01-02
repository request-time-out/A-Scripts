// Decompiled with JetBrains decompiler
// Type: AIChara.ChaAccessoryDefine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace AIChara
{
  public static class ChaAccessoryDefine
  {
    public static readonly int[] AccessoryDefaultIndex = new int[14];
    public static readonly string[] AccessoryTypeName = new string[14]
    {
      "なし",
      "頭",
      "耳",
      "眼鏡",
      "顔",
      "首",
      "肩",
      "胸",
      "腰",
      "背中",
      "腕",
      "手",
      "脚",
      "下腹部"
    };
    public static readonly string[] AccessoryParentName = new string[55]
    {
      "未設定",
      "ポニー",
      "ツイン左",
      "ツイン右",
      "ヘアピン左",
      "ヘアピン右",
      "帽子",
      "額",
      "頭中心",
      "顔",
      "左耳",
      "右耳",
      "眼鏡",
      "鼻",
      "口",
      "首",
      "胸上",
      "胸上中央",
      "左胸",
      "右胸",
      "背中中央",
      "背中左",
      "背中右",
      "腰",
      "腰前",
      "腰後ろ",
      "腰左",
      "腰右",
      "左太もも",
      "左ひざ",
      "左足首",
      "かかと左",
      "右太もも",
      "右ひざ",
      "右足首",
      "かかと右",
      "左肩",
      "左肘",
      "左上腕",
      "左手首",
      "右肩",
      "右肘",
      "右上腕",
      "右手首",
      "左手",
      "左人差指",
      "左中指",
      "左薬指",
      "右手",
      "右人差指",
      "右中指",
      "右薬指",
      "下腹部\x2460",
      "下腹部\x2461",
      "下腹部\x2462"
    };

    static ChaAccessoryDefine()
    {
      ChaAccessoryDefine.dictAccessoryType = new Dictionary<int, string>();
      int length1 = Enum.GetValues(typeof (ChaAccessoryDefine.AccessoryType)).Length;
      int length2 = ChaAccessoryDefine.AccessoryTypeName.Length;
      if (length1 == length2)
      {
        for (int index = 0; index < length1; ++index)
          ChaAccessoryDefine.dictAccessoryType[index] = ChaAccessoryDefine.AccessoryTypeName[index];
      }
      ChaAccessoryDefine.dictAccessoryParent = new Dictionary<int, string>();
      int length3 = Enum.GetValues(typeof (ChaAccessoryDefine.AccessoryParentKey)).Length;
      int length4 = ChaAccessoryDefine.AccessoryParentName.Length;
      if (length3 != length4)
        return;
      for (int index = 0; index < length3; ++index)
        ChaAccessoryDefine.dictAccessoryParent[index] = ChaAccessoryDefine.AccessoryParentName[index];
    }

    public static string GetAccessoryTypeName(ChaListDefine.CategoryNo cate)
    {
      switch (cate)
      {
        case ChaListDefine.CategoryNo.ao_none:
          return ChaAccessoryDefine.AccessoryTypeName[0];
        case ChaListDefine.CategoryNo.ao_head:
          return ChaAccessoryDefine.AccessoryTypeName[1];
        case ChaListDefine.CategoryNo.ao_ear:
          return ChaAccessoryDefine.AccessoryTypeName[2];
        case ChaListDefine.CategoryNo.ao_glasses:
          return ChaAccessoryDefine.AccessoryTypeName[3];
        case ChaListDefine.CategoryNo.ao_face:
          return ChaAccessoryDefine.AccessoryTypeName[4];
        case ChaListDefine.CategoryNo.ao_neck:
          return ChaAccessoryDefine.AccessoryTypeName[5];
        case ChaListDefine.CategoryNo.ao_shoulder:
          return ChaAccessoryDefine.AccessoryTypeName[6];
        case ChaListDefine.CategoryNo.ao_chest:
          return ChaAccessoryDefine.AccessoryTypeName[7];
        case ChaListDefine.CategoryNo.ao_waist:
          return ChaAccessoryDefine.AccessoryTypeName[8];
        case ChaListDefine.CategoryNo.ao_back:
          return ChaAccessoryDefine.AccessoryTypeName[9];
        case ChaListDefine.CategoryNo.ao_arm:
          return ChaAccessoryDefine.AccessoryTypeName[10];
        case ChaListDefine.CategoryNo.ao_hand:
          return ChaAccessoryDefine.AccessoryTypeName[11];
        case ChaListDefine.CategoryNo.ao_leg:
          return ChaAccessoryDefine.AccessoryTypeName[12];
        case ChaListDefine.CategoryNo.ao_kokan:
          return ChaAccessoryDefine.AccessoryTypeName[13];
        default:
          return "不明";
      }
    }

    public static Dictionary<int, string> dictAccessoryType { get; private set; }

    public static Dictionary<int, string> dictAccessoryParent { get; private set; }

    public static ChaAccessoryDefine.AccessoryParentKey GetReverseParent(
      ChaAccessoryDefine.AccessoryParentKey key)
    {
      switch (key)
      {
        case ChaAccessoryDefine.AccessoryParentKey.N_Tikubi_L:
          return ChaAccessoryDefine.AccessoryParentKey.N_Tikubi_R;
        case ChaAccessoryDefine.AccessoryParentKey.N_Tikubi_R:
          return ChaAccessoryDefine.AccessoryParentKey.N_Tikubi_L;
        case ChaAccessoryDefine.AccessoryParentKey.N_Back_L:
          return ChaAccessoryDefine.AccessoryParentKey.N_Back_R;
        case ChaAccessoryDefine.AccessoryParentKey.N_Back_R:
          return ChaAccessoryDefine.AccessoryParentKey.N_Back_L;
        case ChaAccessoryDefine.AccessoryParentKey.N_Waist_L:
          return ChaAccessoryDefine.AccessoryParentKey.N_Waist_R;
        case ChaAccessoryDefine.AccessoryParentKey.N_Waist_R:
          return ChaAccessoryDefine.AccessoryParentKey.N_Waist_L;
        case ChaAccessoryDefine.AccessoryParentKey.N_Leg_L:
          return ChaAccessoryDefine.AccessoryParentKey.N_Leg_R;
        case ChaAccessoryDefine.AccessoryParentKey.N_Knee_L:
          return ChaAccessoryDefine.AccessoryParentKey.N_Knee_R;
        case ChaAccessoryDefine.AccessoryParentKey.N_Ankle_L:
          return ChaAccessoryDefine.AccessoryParentKey.N_Ankle_R;
        case ChaAccessoryDefine.AccessoryParentKey.N_Foot_L:
          return ChaAccessoryDefine.AccessoryParentKey.N_Foot_R;
        case ChaAccessoryDefine.AccessoryParentKey.N_Leg_R:
          return ChaAccessoryDefine.AccessoryParentKey.N_Leg_L;
        case ChaAccessoryDefine.AccessoryParentKey.N_Knee_R:
          return ChaAccessoryDefine.AccessoryParentKey.N_Knee_L;
        case ChaAccessoryDefine.AccessoryParentKey.N_Ankle_R:
          return ChaAccessoryDefine.AccessoryParentKey.N_Ankle_L;
        case ChaAccessoryDefine.AccessoryParentKey.N_Foot_R:
          return ChaAccessoryDefine.AccessoryParentKey.N_Foot_L;
        case ChaAccessoryDefine.AccessoryParentKey.N_Shoulder_L:
          return ChaAccessoryDefine.AccessoryParentKey.N_Shoulder_R;
        case ChaAccessoryDefine.AccessoryParentKey.N_Elbo_L:
          return ChaAccessoryDefine.AccessoryParentKey.N_Elbo_R;
        case ChaAccessoryDefine.AccessoryParentKey.N_Arm_L:
          return ChaAccessoryDefine.AccessoryParentKey.N_Arm_R;
        case ChaAccessoryDefine.AccessoryParentKey.N_Wrist_L:
          return ChaAccessoryDefine.AccessoryParentKey.N_Wrist_R;
        case ChaAccessoryDefine.AccessoryParentKey.N_Shoulder_R:
          return ChaAccessoryDefine.AccessoryParentKey.N_Shoulder_L;
        case ChaAccessoryDefine.AccessoryParentKey.N_Elbo_R:
          return ChaAccessoryDefine.AccessoryParentKey.N_Elbo_L;
        case ChaAccessoryDefine.AccessoryParentKey.N_Arm_R:
          return ChaAccessoryDefine.AccessoryParentKey.N_Arm_L;
        case ChaAccessoryDefine.AccessoryParentKey.N_Wrist_R:
          return ChaAccessoryDefine.AccessoryParentKey.N_Wrist_L;
        case ChaAccessoryDefine.AccessoryParentKey.N_Hand_L:
          return ChaAccessoryDefine.AccessoryParentKey.N_Hand_R;
        case ChaAccessoryDefine.AccessoryParentKey.N_Index_L:
          return ChaAccessoryDefine.AccessoryParentKey.N_Index_R;
        case ChaAccessoryDefine.AccessoryParentKey.N_Middle_L:
          return ChaAccessoryDefine.AccessoryParentKey.N_Middle_R;
        case ChaAccessoryDefine.AccessoryParentKey.N_Ring_L:
          return ChaAccessoryDefine.AccessoryParentKey.N_Ring_R;
        case ChaAccessoryDefine.AccessoryParentKey.N_Hand_R:
          return ChaAccessoryDefine.AccessoryParentKey.N_Hand_L;
        case ChaAccessoryDefine.AccessoryParentKey.N_Index_R:
          return ChaAccessoryDefine.AccessoryParentKey.N_Index_L;
        case ChaAccessoryDefine.AccessoryParentKey.N_Middle_R:
          return ChaAccessoryDefine.AccessoryParentKey.N_Middle_L;
        case ChaAccessoryDefine.AccessoryParentKey.N_Ring_R:
          return ChaAccessoryDefine.AccessoryParentKey.N_Ring_L;
        default:
          switch (key - 2)
          {
            case ChaAccessoryDefine.AccessoryParentKey.none:
              return ChaAccessoryDefine.AccessoryParentKey.N_Hair_twin_R;
            case ChaAccessoryDefine.AccessoryParentKey.N_Hair_pony:
              return ChaAccessoryDefine.AccessoryParentKey.N_Hair_twin_L;
            case ChaAccessoryDefine.AccessoryParentKey.N_Hair_twin_L:
              return ChaAccessoryDefine.AccessoryParentKey.N_Hair_pin_R;
            case ChaAccessoryDefine.AccessoryParentKey.N_Hair_twin_R:
              return ChaAccessoryDefine.AccessoryParentKey.N_Hair_pin_L;
            case ChaAccessoryDefine.AccessoryParentKey.N_Head:
              return ChaAccessoryDefine.AccessoryParentKey.N_Earring_R;
            case ChaAccessoryDefine.AccessoryParentKey.N_Face:
              return ChaAccessoryDefine.AccessoryParentKey.N_Earring_L;
            default:
              return ChaAccessoryDefine.AccessoryParentKey.none;
          }
      }
    }

    public static string GetReverseParent(string key)
    {
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (ChaAccessoryDefine.\u003C\u003Ef__switch\u0024mapA == null)
        {
          // ISSUE: reference to a compiler-generated field
          ChaAccessoryDefine.\u003C\u003Ef__switch\u0024mapA = new Dictionary<string, int>(36)
          {
            {
              "N_Hair_twin_L",
              0
            },
            {
              "N_Hair_pin_L",
              1
            },
            {
              "N_Earring_L",
              2
            },
            {
              "N_Tikubi_L",
              3
            },
            {
              "N_Back_L",
              4
            },
            {
              "N_Waist_L",
              5
            },
            {
              "N_Leg_L",
              6
            },
            {
              "N_Knee_L",
              7
            },
            {
              "N_Ankle_L",
              8
            },
            {
              "N_Foot_L",
              9
            },
            {
              "N_Shoulder_L",
              10
            },
            {
              "N_Elbo_L",
              11
            },
            {
              "N_Arm_L",
              12
            },
            {
              "N_Wrist_L",
              13
            },
            {
              "N_Hand_L",
              14
            },
            {
              "N_Index_L",
              15
            },
            {
              "N_Middle_L",
              16
            },
            {
              "N_Ring_L",
              17
            },
            {
              "N_Hair_twin_R",
              18
            },
            {
              "N_Hair_pin_R",
              19
            },
            {
              "N_Earring_R",
              20
            },
            {
              "N_Tikubi_R",
              21
            },
            {
              "N_Back_R",
              22
            },
            {
              "N_Waist_R",
              23
            },
            {
              "N_Leg_R",
              24
            },
            {
              "N_Knee_R",
              25
            },
            {
              "N_Ankle_R",
              26
            },
            {
              "N_Foot_R",
              27
            },
            {
              "N_Shoulder_R",
              28
            },
            {
              "N_Elbo_R",
              29
            },
            {
              "N_Arm_R",
              30
            },
            {
              "N_Wrist_R",
              31
            },
            {
              "N_Hand_R",
              32
            },
            {
              "N_Index_R",
              33
            },
            {
              "N_Middle_R",
              34
            },
            {
              "N_Ring_R",
              35
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (ChaAccessoryDefine.\u003C\u003Ef__switch\u0024mapA.TryGetValue(key, out num))
        {
          switch (num)
          {
            case 0:
              return "N_Hair_twin_R";
            case 1:
              return "N_Hair_pin_R";
            case 2:
              return "N_Earring_R";
            case 3:
              return "N_Tikubi_R";
            case 4:
              return "N_Back_R";
            case 5:
              return "N_Waist_R";
            case 6:
              return "N_Leg_R";
            case 7:
              return "N_Knee_R";
            case 8:
              return "N_Ankle_R";
            case 9:
              return "N_Foot_R";
            case 10:
              return "N_Shoulder_R";
            case 11:
              return "N_Elbo_R";
            case 12:
              return "N_Arm_R";
            case 13:
              return "N_Wrist_R";
            case 14:
              return "N_Hand_R";
            case 15:
              return "N_Index_R";
            case 16:
              return "N_Middle_R";
            case 17:
              return "N_Ring_R";
            case 18:
              return "N_Hair_twin_L";
            case 19:
              return "N_Hair_pin_L";
            case 20:
              return "N_Earring_L";
            case 21:
              return "N_Tikubi_L";
            case 22:
              return "N_Back_L";
            case 23:
              return "N_Waist_L";
            case 24:
              return "N_Leg_L";
            case 25:
              return "N_Knee_L";
            case 26:
              return "N_Ankle_L";
            case 27:
              return "N_Foot_L";
            case 28:
              return "N_Shoulder_L";
            case 29:
              return "N_Elbo_L";
            case 30:
              return "N_Arm_L";
            case 31:
              return "N_Wrist_L";
            case 32:
              return "N_Hand_L";
            case 33:
              return "N_Index_L";
            case 34:
              return "N_Middle_L";
            case 35:
              return "N_Ring_L";
          }
        }
      }
      return string.Empty;
    }

    public static bool CheckPartsOfHead(string keyName)
    {
      ChaAccessoryDefine.AccessoryParentKey result;
      return Enum.TryParse<ChaAccessoryDefine.AccessoryParentKey>(keyName, out result) && MathfEx.RangeEqualOn<int>(1, (int) result, 14);
    }

    public static int GetAccessoryParentInt(string keyName)
    {
      ChaAccessoryDefine.AccessoryParentKey result;
      return Enum.TryParse<ChaAccessoryDefine.AccessoryParentKey>(keyName, out result) ? (int) result : -1;
    }

    public enum AccessoryType
    {
      None,
      Head,
      Ear,
      Glasses,
      Face,
      Neck,
      Shoulder,
      Chest,
      Waist,
      Back,
      Arm,
      Hand,
      Leg,
      Kokan,
    }

    public enum AccessoryParentKey
    {
      none,
      N_Hair_pony,
      N_Hair_twin_L,
      N_Hair_twin_R,
      N_Hair_pin_L,
      N_Hair_pin_R,
      N_Head_top,
      N_Hitai,
      N_Head,
      N_Face,
      N_Earring_L,
      N_Earring_R,
      N_Megane,
      N_Nose,
      N_Mouth,
      N_Neck,
      N_Chest_f,
      N_Chest,
      N_Tikubi_L,
      N_Tikubi_R,
      N_Back,
      N_Back_L,
      N_Back_R,
      N_Waist,
      N_Waist_f,
      N_Waist_b,
      N_Waist_L,
      N_Waist_R,
      N_Leg_L,
      N_Knee_L,
      N_Ankle_L,
      N_Foot_L,
      N_Leg_R,
      N_Knee_R,
      N_Ankle_R,
      N_Foot_R,
      N_Shoulder_L,
      N_Elbo_L,
      N_Arm_L,
      N_Wrist_L,
      N_Shoulder_R,
      N_Elbo_R,
      N_Arm_R,
      N_Wrist_R,
      N_Hand_L,
      N_Index_L,
      N_Middle_L,
      N_Ring_L,
      N_Hand_R,
      N_Index_R,
      N_Middle_R,
      N_Ring_R,
      N_Dan,
      N_Kokan,
      N_Ana,
    }
  }
}
