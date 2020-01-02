// Decompiled with JetBrains decompiler
// Type: Rewired.InputManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Rewired.Utils;
using Rewired.Utils.Interfaces;
using System.ComponentModel;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Rewired
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public sealed class InputManager : InputManager_Base
  {
    public InputManager()
    {
      base.\u002Ector();
    }

    protected virtual void DetectPlatform()
    {
      this.editorPlatform = (__Null) 0;
      this.platform = (__Null) 0;
      this.webplayerPlatform = (__Null) 0;
      this.isEditor = (__Null) 0;
      string str1 = SystemInfo.get_deviceName() ?? string.Empty;
      string str2 = SystemInfo.get_deviceModel() ?? string.Empty;
      this.platform = (__Null) 1;
    }

    protected virtual void CheckRecompile()
    {
    }

    protected virtual IExternalTools GetExternalTools()
    {
      return (IExternalTools) new ExternalTools();
    }

    private bool CheckDeviceName(string searchPattern, string deviceName, string deviceModel)
    {
      return Regex.IsMatch(deviceName, searchPattern, RegexOptions.IgnoreCase) || Regex.IsMatch(deviceModel, searchPattern, RegexOptions.IgnoreCase);
    }
  }
}
