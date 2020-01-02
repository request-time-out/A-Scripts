// Decompiled with JetBrains decompiler
// Type: Helper.Sound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace Helper
{
  public static class Sound
  {
    public static readonly Dictionary<Sound.SystemSE, string> SystemSECast = new Dictionary<Sound.SystemSE, string>()
    {
      {
        Sound.SystemSE.sel,
        "sse_00_01"
      },
      {
        Sound.SystemSE.ok_s,
        "sse_00_02"
      },
      {
        Sound.SystemSE.ok_l,
        "sse_00_03"
      },
      {
        Sound.SystemSE.cancel,
        "sse_00_04"
      },
      {
        Sound.SystemSE.photo,
        "sse_00_05"
      },
      {
        Sound.SystemSE.title,
        "se_06_title"
      },
      {
        Sound.SystemSE.ok_s2,
        "se_07_button_A"
      },
      {
        Sound.SystemSE.window_o,
        "se_08_window_B"
      },
      {
        Sound.SystemSE.save,
        "se_09_save_A"
      }
    };
    public static readonly Dictionary<Manager.Sound.Type, string> SoundBasePath = new Dictionary<Manager.Sound.Type, string>()
    {
      {
        Manager.Sound.Type.BGM,
        "sound/data/bgm.unity3d"
      },
      {
        Manager.Sound.Type.ENV,
        "sound/data/env.unity3d"
      },
      {
        Manager.Sound.Type.GameSE2D,
        "sound/data/se.unity3d"
      },
      {
        Manager.Sound.Type.GameSE3D,
        "sound/data/se.unity3d"
      },
      {
        Manager.Sound.Type.SystemSE,
        "sound/data/systemse.unity3d"
      }
    };

    public static bool isPlay(Manager.Sound sound, Sound.SystemSE se)
    {
      return sound.IsPlay(Manager.Sound.Type.SystemSE, Sound.SystemSECast[se]);
    }

    public enum SystemSE
    {
      sel,
      ok_s,
      ok_l,
      cancel,
      photo,
      title,
      ok_s2,
      window_o,
      save,
    }

    public class Setting
    {
      public bool isAsync = true;
      public int settingNo = -1;
      public string manifestFileName = "sounddata";
      public Manager.Sound.Type type;
      public string assetBundleName;
      public string assetName;
      public float fadeOrdelayTime;
      public bool isBundleUnload;

      public Setting()
      {
      }

      public Setting(Sound.SystemSE se)
      {
        this.Cast(Manager.Sound.Type.SystemSE);
        this.assetName = Sound.SystemSECast[se];
      }

      public Setting(Manager.Sound.Type type)
      {
        this.Cast(type);
      }

      public void Cast(Manager.Sound.Type type)
      {
        this.type = type;
        this.assetBundleName = Sound.SoundBasePath[this.type];
      }
    }
  }
}
