// Decompiled with JetBrains decompiler
// Type: AIProject.StartTutorialADV
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class StartTutorialADV : AgentAction
  {
    [SerializeField]
    private int _storyID;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this._storyID == 0)
        MapUIContainer.OpenStorySupportUI(Popup.StorySupport.Type.ExamineAround);
      return (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      ((Task) this).OnEnd();
    }
  }
}
