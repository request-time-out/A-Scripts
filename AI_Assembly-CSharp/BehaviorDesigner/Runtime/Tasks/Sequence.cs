// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.Sequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskDescription("The sequence task is similar to an \"and\" operation. It will return failure as soon as one of its child tasks return failure. If a child task returns success then it will sequentially run the next task. If all child tasks return success then it will return success.")]
  [TaskIcon("{SkinColor}SequenceIcon.png")]
  public class Sequence : Composite
  {
    private int currentChildIndex;
    private TaskStatus executionStatus;

    public Sequence()
    {
      base.\u002Ector();
    }

    public virtual int CurrentChildIndex()
    {
      return this.currentChildIndex;
    }

    public virtual bool CanExecute()
    {
      return this.currentChildIndex < ((List<Task>) ((ParentTask) this).children).Count && this.executionStatus != 1;
    }

    public virtual void OnChildExecuted(TaskStatus childStatus)
    {
      ++this.currentChildIndex;
      this.executionStatus = childStatus;
    }

    public virtual void OnConditionalAbort(int childIndex)
    {
      this.currentChildIndex = childIndex;
      this.executionStatus = (TaskStatus) 0;
    }

    public virtual void OnEnd()
    {
      this.executionStatus = (TaskStatus) 0;
      this.currentChildIndex = 0;
    }
  }
}
