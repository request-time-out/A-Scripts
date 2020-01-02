// Decompiled with JetBrains decompiler
// Type: UsvConsoleCmds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class UsvConsoleCmds
{
  public static UsvConsoleCmds Instance;

  [ConsoleHandler("showmesh")]
  public bool ShowMesh(string[] args)
  {
    return this.SetMeshVisible(args[1], true);
  }

  [ConsoleHandler("hidemesh")]
  public bool HideMesh(string[] args)
  {
    return this.SetMeshVisible(args[1], false);
  }

  private bool SetMeshVisible(string strInstID, bool visible)
  {
    int result = 0;
    if (!int.TryParse(strInstID, out result))
      return false;
    foreach (MeshRenderer meshRenderer in Object.FindObjectsOfType(typeof (MeshRenderer)) as MeshRenderer[])
    {
      if (((Object) ((Component) meshRenderer).get_gameObject()).GetInstanceID() == result)
      {
        ((Renderer) meshRenderer).set_enabled(visible);
        return true;
      }
    }
    return false;
  }

  [ConsoleHandler("testlogs")]
  public bool PrintTestLogs(string[] args)
  {
    Debug.Log((object) "A typical line of logging.");
    Debug.Log((object) "Another line.");
    Debug.LogWarning((object) "An ordinary warning.");
    Debug.LogError((object) "An ordinary error.");
    Debug.Log((object) "misc.中.文.测.试.test.中文测试.");
    try
    {
      throw new ApplicationException("A user-thrown exception.");
    }
    catch (ApplicationException ex)
    {
      Debug.LogException((Exception) ex);
    }
    return true;
  }

  [ConsoleHandler("toggle")]
  public bool ToggleSwitch(string[] args)
  {
    try
    {
      GameInterface.Instance.ToggleSwitch(args[1], int.Parse(args[2]) != 0);
    }
    catch (Exception ex)
    {
      Log.Exception(ex);
      throw;
    }
    return true;
  }

  [ConsoleHandler("slide")]
  public bool SlideChanged(string[] args)
  {
    try
    {
      GameInterface.Instance.ChangePercentage(args[1], double.Parse(args[2]));
    }
    catch (Exception ex)
    {
      Log.Exception(ex);
      throw;
    }
    return true;
  }

  [ConsoleHandler("flyto")]
  public bool FlyTo(string[] args)
  {
    try
    {
    }
    catch (Exception ex)
    {
      Log.Exception(ex);
      throw;
    }
    return true;
  }

  public event SysPost.StdMulticastDelegation QueryEffectList;

  public event SysPost.StdMulticastDelegation RunEffectStressTest;

  public event SysPost.StdMulticastDelegation StartAnalyzePixel;

  [ConsoleHandler("start_analyze_pixels")]
  public bool StartAnalysePixelsTriggered(string[] args)
  {
    try
    {
      bool b = false;
      if (args.Length == 2 && args[1] == "refresh")
        b = true;
      SysPost.InvokeMulticast((object) this, (MulticastDelegate) this.StartAnalyzePixel, (EventArgs) new UsvConsoleCmds.AnalysePixelsArgs(b));
    }
    catch (Exception ex)
    {
      Log.Exception(ex);
      throw;
    }
    return true;
  }

  [ConsoleHandler("get_effect_list")]
  public bool QueryEffectListTriggered(string[] args)
  {
    try
    {
      if (args.Length != 1)
      {
        Log.Error((object) "Command 'get_effect_list' parameter count mismatched. ({0} expected, {1} got)", (object) 1, (object) args.Length);
        return false;
      }
      SysPost.InvokeMulticast((object) this, (MulticastDelegate) this.QueryEffectList);
    }
    catch (Exception ex)
    {
      Log.Exception(ex);
      throw;
    }
    return true;
  }

  [ConsoleHandler("run_effect_stress")]
  public bool EffectStressTestTriggered(string[] args)
  {
    try
    {
      if (args.Length == 3)
      {
        SysPost.InvokeMulticast((object) this, (MulticastDelegate) this.RunEffectStressTest, (EventArgs) new UsvConsoleCmds.UsEffectStressTestEventArgs(args[1], int.Parse(args[2])));
      }
      else
      {
        int effectCount = int.Parse(args[args.Length - 1]);
        List<string> stringList = new List<string>((IEnumerable<string>) args);
        stringList.RemoveAt(0);
        stringList.RemoveAt(stringList.Count - 1);
        SysPost.InvokeMulticast((object) this, (MulticastDelegate) this.RunEffectStressTest, (EventArgs) new UsvConsoleCmds.UsEffectStressTestEventArgs(string.Join(" ", stringList.ToArray()), effectCount));
      }
    }
    catch (Exception ex)
    {
      Log.Exception(ex);
      throw;
    }
    return true;
  }

  public class AnalysePixelsArgs : EventArgs
  {
    public bool bRefresh;

    public AnalysePixelsArgs(bool b)
    {
      this.bRefresh = b;
    }
  }

  public class UsEffectStressTestEventArgs : EventArgs
  {
    public string _effectName;
    public int _effectCount;

    public UsEffectStressTestEventArgs(string effectName, int effectCount)
    {
      this._effectName = effectName;
      this._effectCount = effectCount;
    }
  }
}
