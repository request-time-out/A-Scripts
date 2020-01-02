// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomCharaFileInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace CharaCustom
{
  public class CustomCharaFileInfo
  {
    public string FullPath = string.Empty;
    public string FileName = string.Empty;
    public string name = string.Empty;
    public string personality = string.Empty;
    public int birthMonth = 1;
    public int birthDay = 1;
    public string strBirthDay = string.Empty;
    public int lifestyle = -1;
    public int skill_n01 = -1;
    public int skill_n02 = -1;
    public int skill_n03 = -1;
    public int skill_n04 = -1;
    public int skill_n05 = -1;
    public int skill_h01 = -1;
    public int skill_h02 = -1;
    public int skill_h03 = -1;
    public int skill_h04 = -1;
    public int skill_h05 = -1;
    public int wish_01 = -1;
    public int wish_02 = -1;
    public int wish_03 = -1;
    public Dictionary<int, int> flavorState = new Dictionary<int, int>();
    public Dictionary<int, int> normalSkill = new Dictionary<int, int>();
    public Dictionary<int, int> hSkill = new Dictionary<int, int>();
    public string data_uuid = string.Empty;
    public CharaCategoryKind cateKind = CharaCategoryKind.Female;
    public CustomCharaScrollViewInfo cssvi;
    public int index;
    public DateTime time;
    public int type;
    public int height;
    public int bustSize;
    public int hair;
    public int pheromone;
    public int reliability;
    public int reason;
    public int instinct;
    public int dirty;
    public int wariness;
    public int sociability;
    public int darkness;
    public int sex;
    public byte[] pngData;
    public bool gameRegistration;
    public int phase;
    public int favoritePlace;
    public bool futanari;
    public bool isInSaveData;
  }
}
