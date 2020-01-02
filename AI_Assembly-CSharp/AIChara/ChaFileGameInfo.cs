// Decompiled with JetBrains decompiler
// Type: AIChara.ChaFileGameInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using MessagePack;
using System;
using System.Collections.Generic;

namespace AIChara
{
  [MessagePackObject(true)]
  public class ChaFileGameInfo
  {
    [IgnoreMember]
    public static readonly string BlockName = "GameInfo";

    public ChaFileGameInfo()
    {
      this.MemberInit();
    }

    public Version version { get; set; }

    public bool gameRegistration { get; set; }

    public ChaFileGameInfo.MinMaxInfo tempBound { get; set; }

    public ChaFileGameInfo.MinMaxInfo moodBound { get; set; }

    public Dictionary<int, int> flavorState { get; set; }

    public int totalFlavor { get; set; }

    public Dictionary<int, float> desireDefVal { get; set; }

    public Dictionary<int, float> desireBuffVal { get; set; }

    public int phase { get; set; }

    public Dictionary<int, int> normalSkill { get; set; }

    public Dictionary<int, int> hSkill { get; set; }

    public int favoritePlace { get; set; }

    public int lifestyle { get; set; }

    public int morality { get; set; }

    public int motivation { get; set; }

    public int immoral { get; set; }

    public bool isHAddTaii0 { get; set; }

    public bool isHAddTaii1 { get; set; }

    public void MemberInit()
    {
      this.version = ChaFileDefine.ChaFileGameInfoVersion;
      this.gameRegistration = false;
      this.tempBound = new ChaFileGameInfo.MinMaxInfo();
      this.moodBound = new ChaFileGameInfo.MinMaxInfo();
      this.flavorState = new Dictionary<int, int>();
      for (int index = 0; index < 8; ++index)
        this.flavorState[index] = 0;
      this.totalFlavor = 0;
      this.desireDefVal = new Dictionary<int, float>();
      this.desireBuffVal = new Dictionary<int, float>();
      for (int index = 0; index < 16; ++index)
      {
        this.desireDefVal[index] = 0.0f;
        this.desireBuffVal[index] = 0.0f;
      }
      this.phase = 0;
      this.normalSkill = new Dictionary<int, int>();
      this.hSkill = new Dictionary<int, int>();
      for (int index = 0; index < 5; ++index)
      {
        this.normalSkill[index] = -1;
        this.hSkill[index] = -1;
      }
      this.favoritePlace = -1;
      this.lifestyle = -1;
      this.morality = 50;
      this.motivation = 0;
      this.immoral = 0;
      this.isHAddTaii0 = false;
      this.isHAddTaii1 = false;
    }

    public void Copy(ChaFileGameInfo src)
    {
      this.version = src.version;
      this.gameRegistration = src.gameRegistration;
      this.tempBound.Copy(src.tempBound);
      this.moodBound.Copy(src.moodBound);
      this.flavorState = new Dictionary<int, int>((IDictionary<int, int>) src.flavorState);
      this.totalFlavor = src.totalFlavor;
      this.desireDefVal = new Dictionary<int, float>((IDictionary<int, float>) src.desireDefVal);
      this.desireBuffVal = new Dictionary<int, float>((IDictionary<int, float>) src.desireBuffVal);
      this.phase = src.phase;
      this.normalSkill = new Dictionary<int, int>((IDictionary<int, int>) src.normalSkill);
      this.hSkill = new Dictionary<int, int>((IDictionary<int, int>) src.hSkill);
      this.favoritePlace = src.favoritePlace;
      this.lifestyle = src.lifestyle;
      this.morality = src.morality;
      this.motivation = src.motivation;
      this.immoral = src.immoral;
      this.isHAddTaii0 = src.isHAddTaii0;
      this.isHAddTaii1 = src.isHAddTaii1;
    }

    public void ComplementWithVersion()
    {
      if (this.flavorState == null || this.flavorState.Count == 0)
      {
        this.flavorState = new Dictionary<int, int>();
        for (int index = 0; index < 8; ++index)
          this.flavorState[index] = 0;
      }
      if (this.desireDefVal == null || this.desireDefVal.Count == 0)
      {
        this.desireDefVal = new Dictionary<int, float>();
        for (int index = 0; index < 16; ++index)
          this.desireDefVal[index] = 0.0f;
      }
      if (this.desireBuffVal == null || this.desireBuffVal.Count == 0)
      {
        this.desireBuffVal = new Dictionary<int, float>();
        for (int index = 0; index < 16; ++index)
          this.desireBuffVal[index] = 0.0f;
      }
      if ((double) this.tempBound.lower == 0.0 && (double) this.tempBound.upper == 0.0)
      {
        this.tempBound.lower = 20f;
        this.tempBound.upper = 80f;
      }
      if ((double) this.moodBound.lower == 0.0 && (double) this.moodBound.upper == 0.0)
      {
        this.moodBound.lower = 20f;
        this.moodBound.upper = 80f;
      }
      if (this.phase < 3)
        this.lifestyle = -1;
      if (this.normalSkill == null || this.normalSkill.Count == 0)
      {
        this.normalSkill = new Dictionary<int, int>();
        for (int index = 0; index < 5; ++index)
          this.normalSkill[index] = -1;
      }
      if (this.hSkill == null || this.hSkill.Count == 0)
      {
        this.hSkill = new Dictionary<int, int>();
        for (int index = 0; index < 5; ++index)
          this.hSkill[index] = -1;
      }
      this.version = ChaFileDefine.ChaFileGameInfoVersion;
    }

    [MessagePackObject(true)]
    public class MinMaxInfo
    {
      public MinMaxInfo()
      {
        this.MemberInit();
      }

      public float lower { get; set; }

      public float upper { get; set; }

      public void MemberInit()
      {
        this.lower = 20f;
        this.upper = 80f;
      }

      public void Copy(ChaFileGameInfo.MinMaxInfo src)
      {
        this.lower = src.lower;
        this.upper = src.upper;
      }
    }
  }
}
