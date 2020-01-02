// Decompiled with JetBrains decompiler
// Type: AIProject.MapHAnimData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class MapHAnimData : ScriptableObject
  {
    public List<MapHAnimData.Param> param;

    public MapHAnimData()
    {
      base.\u002Ector();
    }

    [Serializable]
    public class Param
    {
      public string AnimName;
      public int ID;
      public string MaleAssetBundle;
      public string MaleAnimator;
      public string IsMaleHitObject;
      public string MaleFileMotionNeck;
      public string FemaleAssetBundle;
      public string FemaleAnimator;
      public string IsFemaleHitObject;
      public string FemaleFileMotionNeck;
      public int Action;
      public int Control;
      public string Position;
      public string Offset;
      public string NeedItem;
      public int DownPtn;
      public int FaintnessLimit;
      public string IyaAction;
      public string CanMerchantMotion;
      public int Hentai;
      public int Phase;
      public int InitiativeFemale;
      public int FemaleProcivity;
      public int BackInitiativeID;
      public string System;
      public int isMaleSon;
      public int FemaleUpperCloths0;
      public int FemaleLowerCloths0;
      public int FemaleUpperCloths1;
      public int FemaleLowerCloths1;
      public int IsFeelHit;
      public string NameCamera;
      public string FileSiruPaste;
      public string FileSE;
      public int PlayShortBreash;
      public string VoiceCategory;
      public int VoiceKindID;
      public int Iyagari;
      public int Promiscuity;
      public string AnimListiD;
    }
  }
}
