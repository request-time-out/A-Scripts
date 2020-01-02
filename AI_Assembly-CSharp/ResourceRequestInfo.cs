// Decompiled with JetBrains decompiler
// Type: ResourceRequestInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class ResourceRequestInfo
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

  public override string ToString()
  {
    return string.Format("#{0} ({1:0.000}) {2} {3} {4} +{5} +{6} ({7})", (object) this.seqID, (object) this.requestTime, (object) this.rootID, !(this.resourceType != (System.Type) null) ? (object) (string) null : (object) this.resourceType.ToString(), this.requestType != ResourceRequestType.Async ? (object) string.Empty : (object) "(a)", (object) this.resourcePath, (object) this.srcFile, (object) this.srcLineNum);
  }

  public void RecordObject(Object obj)
  {
    this.rootID = obj.GetInstanceID();
    this.resourceType = ((object) obj).GetType();
  }
}
