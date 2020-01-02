// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug.LogFormat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug
{
  [TaskDescription("LogFormat is analgous to Debug.LogFormat().\nIt takes format string, substitutes arguments supplied a '{0-4}' and returns success.\nAny fields or arguments not supplied are ignored.It can be used for debugging.")]
  [TaskIcon("{SkinColor}LogIcon.png")]
  public class LogFormat : Action
  {
    [Tooltip("Text format with {0}, {1}, etc")]
    public SharedString textFormat;
    [Tooltip("Is this text an error?")]
    public SharedBool logError;
    public SharedVariable arg0;
    public SharedVariable arg1;
    public SharedVariable arg2;
    public SharedVariable arg3;

    public LogFormat()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      object[] objArray = this.buildParamsArray();
      if (this.logError.get_Value())
        Debug.LogErrorFormat(this.textFormat.get_Value(), objArray);
      else
        Debug.LogFormat(this.textFormat.get_Value(), objArray);
      return (TaskStatus) 2;
    }

    private object[] buildParamsArray()
    {
      object[] objArray;
      if (this.isValid(this.arg3))
      {
        objArray = new object[4]
        {
          null,
          null,
          null,
          this.arg3.GetValue()
        };
        objArray[2] = this.arg2.GetValue();
        objArray[1] = this.arg1.GetValue();
        objArray[0] = this.arg0.GetValue();
      }
      else if (this.isValid(this.arg2))
      {
        objArray = new object[3]
        {
          null,
          null,
          this.arg2.GetValue()
        };
        objArray[1] = this.arg1.GetValue();
        objArray[0] = this.arg0.GetValue();
      }
      else if (this.isValid(this.arg1))
      {
        objArray = new object[2]
        {
          null,
          this.arg1.GetValue()
        };
        objArray[0] = this.arg0.GetValue();
      }
      else
      {
        if (!this.isValid(this.arg0))
          return (object[]) null;
        objArray = new object[1]{ this.arg0.GetValue() };
      }
      return objArray;
    }

    private bool isValid(SharedVariable sv)
    {
      return sv != null && !sv.get_IsNone();
    }

    public virtual void OnReset()
    {
      this.textFormat = (SharedString) string.Empty;
      this.logError = (SharedBool) false;
      this.arg0 = (SharedVariable) null;
      this.arg1 = (SharedVariable) null;
      this.arg2 = (SharedVariable) null;
      this.arg3 = (SharedVariable) null;
    }
  }
}
