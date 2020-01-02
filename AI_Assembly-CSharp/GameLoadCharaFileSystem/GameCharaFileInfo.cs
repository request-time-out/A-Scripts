// Decompiled with JetBrains decompiler
// Type: GameLoadCharaFileSystem.GameCharaFileInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace GameLoadCharaFileSystem
{
  public class GameCharaFileInfo
  {
    public string FullPath = string.Empty;
    public string FileName = string.Empty;
    public string name = string.Empty;
    public string personality = string.Empty;
    public int birthMonth = 1;
    public int birthDay = 1;
    public string strBirthDay = string.Empty;
    public Dictionary<int, int> flavorState = new Dictionary<int, int>();
    public Dictionary<int, int> normalSkill = new Dictionary<int, int>();
    public Dictionary<int, int> hSkill = new Dictionary<int, int>();
    public int lifeStyle = -1;
    public string data_uuid = string.Empty;
    public CategoryKind cateKind = CategoryKind.Female;
    public GameCharaFileInfoComponent fic;
    public int index;
    public DateTime time;
    public int voice;
    public int height;
    public int bustSize;
    public int hair;
    public int bloodType;
    public int sex;
    public int[] usePackage;
    public byte[] pngData;
    public bool gameRegistration;
    public int phase;
    public int favoritePlace;
    public bool futanari;
    public bool isInSaveData;
  }
}
