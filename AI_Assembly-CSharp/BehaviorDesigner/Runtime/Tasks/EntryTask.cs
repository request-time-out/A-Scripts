// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.Tasks.EntryTask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace BehaviorDesigner.Runtime.Tasks
{
  [TaskIcon("{SkinColor}EntryIcon.png")]
  public class EntryTask : ParentTask
  {
    public EntryTask()
    {
      base.\u002Ector();
    }

    public virtual int MaxChildren()
    {
      return 1;
    }
  }
}
