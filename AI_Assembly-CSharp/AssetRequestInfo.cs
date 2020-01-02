// Decompiled with JetBrains decompiler
// Type: AssetRequestInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetRequestInfo
{
  public string resourcePath = string.Empty;
  public string srcFile = string.Empty;
  public double requestTime = (double) Time.get_realtimeSinceStartup();
  public int seqID;
  public int rootID;
  public ResourceRequestType requestType;
  public System.Type resourceType;
  public int srcLineNum;
  public int stacktraceHash;
  public double duration;
  public bool isAsync;

  public override string ToString()
  {
    Scene activeScene = SceneManager.GetActiveScene();
    return string.Format("{0:0.00} {1} {2} {3} {4:0.00}ms {5}", (object) this.requestTime, !(this.resourceType != (System.Type) null) ? (object) "<null_type>" : (object) this.resourceType.ToString(), (object) this.resourcePath, (object) ((Scene) ref activeScene).get_name(), (object) this.duration, this.requestType != ResourceRequestType.Async ? (object) "sync" : (object) "async");
  }

  public void RecordObject(Object obj)
  {
    if (obj.get_name() == "LuaAsset")
    {
      this.rootID = -1;
      this.resourceType = ((object) AssetRequestInfo.LuaAsset.Instance).GetType();
    }
    else
    {
      this.rootID = obj.GetInstanceID();
      this.resourceType = ((object) obj).GetType();
    }
  }

  private class LuaAsset : Object
  {
    public static AssetRequestInfo.LuaAsset Instance = new AssetRequestInfo.LuaAsset();

    public LuaAsset()
    {
      base.\u002Ector();
    }
  }
}
