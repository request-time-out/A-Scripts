// Decompiled with JetBrains decompiler
// Type: Helper.Voice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
  public static class Voice
  {
    public static Dictionary<int, string> SoundBasePath = new Dictionary<int, string>()
    {
      {
        0,
        "sound/data/pcm/c00/"
      }
    };

    public static Transform Play(Manager.Voice voice, Voice.Setting s)
    {
      Manager.Voice voice1 = voice;
      int no1 = s.no;
      string assetBundleName1 = s.assetBundleName;
      string assetName1 = s.assetName;
      float pitch = s.pitch;
      float delayTime = s.delayTime;
      bool isAsync = s.isAsync;
      int no2 = no1;
      string assetBundleName2 = assetBundleName1;
      string assetName2 = assetName1;
      double num1 = (double) pitch;
      double num2 = (double) delayTime;
      int num3 = isAsync ? 1 : 0;
      Transform voiceTrans = s.voiceTrans;
      int type = (int) s.type;
      int settingNo = s.settingNo;
      int num4 = s.isPlayEndDelete ? 1 : 0;
      int num5 = s.isBundleUnload ? 1 : 0;
      return voice1.Play(no2, assetBundleName2, assetName2, (float) num1, (float) num2, 0.0f, num3 != 0, voiceTrans, (Manager.Voice.Type) type, settingNo, num4 != 0, num5 != 0, false);
    }

    public static Transform OnecePlay(Manager.Voice voice, Voice.Setting s)
    {
      Manager.Voice voice1 = voice;
      int no1 = s.no;
      string assetBundleName1 = s.assetBundleName;
      string assetName1 = s.assetName;
      float pitch = s.pitch;
      float delayTime = s.delayTime;
      bool isAsync = s.isAsync;
      int no2 = no1;
      string assetBundleName2 = assetBundleName1;
      string assetName2 = assetName1;
      double num1 = (double) pitch;
      double num2 = (double) delayTime;
      int num3 = isAsync ? 1 : 0;
      Transform voiceTrans = s.voiceTrans;
      int type = (int) s.type;
      int settingNo = s.settingNo;
      int num4 = s.isPlayEndDelete ? 1 : 0;
      int num5 = s.isBundleUnload ? 1 : 0;
      return voice1.OnecePlay(no2, assetBundleName2, assetName2, (float) num1, (float) num2, 0.0f, num3 != 0, voiceTrans, (Manager.Voice.Type) type, settingNo, num4 != 0, num5 != 0, false);
    }

    public static Transform OnecePlayChara(Manager.Voice voice, Voice.Setting s)
    {
      Manager.Voice voice1 = voice;
      int no1 = s.no;
      string assetBundleName1 = s.assetBundleName;
      string assetName1 = s.assetName;
      float pitch = s.pitch;
      float delayTime = s.delayTime;
      bool isAsync = s.isAsync;
      int no2 = no1;
      string assetBundleName2 = assetBundleName1;
      string assetName2 = assetName1;
      double num1 = (double) pitch;
      double num2 = (double) delayTime;
      int num3 = isAsync ? 1 : 0;
      Transform voiceTrans = s.voiceTrans;
      int type = (int) s.type;
      int settingNo = s.settingNo;
      int num4 = s.isPlayEndDelete ? 1 : 0;
      int num5 = s.isBundleUnload ? 1 : 0;
      return voice1.OnecePlayChara(no2, assetBundleName2, assetName2, (float) num1, (float) num2, 0.0f, num3 != 0, voiceTrans, (Manager.Voice.Type) type, settingNo, num4 != 0, num5 != 0, false);
    }

    public class Setting
    {
      public float pitch = 1f;
      public bool isAsync = true;
      public int settingNo = -1;
      public bool isPlayEndDelete = true;
      public string manifestFileName = "sounddata";
      public string assetBundleName;
      public string assetName;
      public Manager.Voice.Type type;
      public int no;
      public Transform voiceTrans;
      public float delayTime;
      public bool isBundleUnload;
    }
  }
}
