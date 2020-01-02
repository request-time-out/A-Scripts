// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Wait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("Wait a specified amount of time. The task will return running until the task is done waiting. It will return success after the wait time has elapsed.")]
  [TaskIcon("{SkinColor}WaitIcon.png")]
  public class Wait : Action
  {
    [Tooltip("The amount of time to wait")]
    public SharedFloat waitTime;
    [Tooltip("Should the wait be randomized?")]
    public SharedBool randomWait;
    [Tooltip("The minimum wait time if random wait is enabled")]
    public SharedFloat randomWaitMin;
    [Tooltip("The maximum wait time if random wait is enabled")]
    public SharedFloat randomWaitMax;
    private float waitDuration;
    private float startTime;
    private float pauseTime;

    public Wait()
    {
      base.\u002Ector();
    }

    public virtual void OnStart()
    {
      this.startTime = Time.get_time();
      if (this.randomWait.get_Value())
        this.waitDuration = Random.Range(this.randomWaitMin.get_Value(), this.randomWaitMax.get_Value());
      else
        this.waitDuration = this.waitTime.get_Value();
    }

    public virtual TaskStatus OnUpdate()
    {
      return (double) this.startTime + (double) this.waitDuration < (double) Time.get_time() ? (TaskStatus) 2 : (TaskStatus) 3;
    }

    public virtual void OnPause(bool paused)
    {
      if (paused)
        this.pauseTime = Time.get_time();
      else
        this.startTime += Time.get_time() - this.pauseTime;
    }

    public virtual void OnReset()
    {
      this.waitTime = (SharedFloat) 1f;
      this.randomWait = (SharedBool) false;
      this.randomWaitMin = (SharedFloat) 1f;
      this.randomWaitMax = (SharedFloat) 1f;
    }
  }
}
