// Decompiled with JetBrains decompiler
// Type: ModChecker.ModChecker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UploaderSystem;

namespace ModChecker
{
  public class ModChecker : BaseLoader
  {
    private Dictionary<int, NetworkInfo.UserInfo> dictUserInfo = new Dictionary<int, NetworkInfo.UserInfo>();
    private FolderAssist faFile = new FolderAssist();
    private readonly string[] key = new string[1]{ "*.png" };
    private readonly string checkDirName = "check/chara";
    private readonly string checkModDirName = "check/mod";
    private List<ModChecker.ModChecker.CheckDataInfo> lstModInfo = new List<ModChecker.ModChecker.CheckDataInfo>();
    [SerializeField]
    private CheckLog log;
    [SerializeField]
    private Processing processing;
    [SerializeField]
    private ModChecker.ModChecker.TypeInfoText typeInfoText;
    [SerializeField]
    private Button btnCheck;
    [SerializeField]
    private Button btnOutput;
    [SerializeField]
    private Button btnCancel;
    [SerializeField]
    private CanvasGroup cvsgMenu;
    private int vSyncCountBack;
    private bool cancel;

    private string EncodeFromBase64(string buf)
    {
      return Encoding.UTF8.GetString(Convert.FromBase64String(buf));
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ModChecker.ModChecker.\u003CStart\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator GetFiles(IObserver<bool> observer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ModChecker.ModChecker.\u003CGetFiles\u003Ec__Iterator1()
      {
        observer = observer,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CheckMod(IObserver<bool> observer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ModChecker.ModChecker.\u003CCheckMod\u003Ec__Iterator2()
      {
        observer = observer,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CheckChara()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ModChecker.ModChecker.\u003CCheckChara\u003Ec__Iterator3()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator OutputModInfo(IObserver<bool> observer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ModChecker.ModChecker.\u003COutputModInfo\u003Ec__Iterator4()
      {
        observer = observer,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CreateModInfo()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ModChecker.ModChecker.\u003CCreateModInfo\u003Ec__Iterator5()
      {
        \u0024this = this
      };
    }

    private void OnDestroy()
    {
      this.log.EndLog();
      QualitySettings.set_vSyncCount(this.vSyncCountBack);
    }

    [Serializable]
    public class TypeInfoText
    {
      public UnityEngine.UI.Text[] textChara;
    }

    public class CheckDataInfo
    {
      public string dataID = string.Empty;
      public string userID = string.Empty;
      public string filename = string.Empty;
      public List<string> lstCheck = new List<string>();
    }
  }
}
