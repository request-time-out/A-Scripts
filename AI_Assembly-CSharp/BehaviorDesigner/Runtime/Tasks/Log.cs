// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Log
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Log is a simple task which will output the specified text and return success. It can be used for debugging.")]
  [TaskIcon("{SkinColor}LogIcon.png")]
  public class Log : Action
  {
    [Tooltip("Text to output to the log")]
    public SharedString text;
    [Tooltip("Is this text an error?")]
    public SharedBool logError;

    public Log()
    {
      base.\u002Ector();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.logError.get_Value())
        Debug.LogError((object) this.text);
      else
        Debug.Log((object) this.text);
      return (TaskStatus) 2;
    }

    public virtual void OnReset()
    {
      this.text = (SharedString) string.Empty;
      this.logError = (SharedBool) false;
    }
  }
}
