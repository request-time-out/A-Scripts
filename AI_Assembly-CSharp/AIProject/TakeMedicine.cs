// Decompiled with JetBrains decompiler
// Type: AIProject.TakeMedicine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;

namespace AIProject
{
  [TaskCategory("")]
  public class TakeMedicine : AgentAction
  {
    private int _layer = -1;
    private bool _inEnableFade;
    private float _inFadeSecond;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      PoseKeyPair medicId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.MedicID;
      PlayState info = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[medicId.postureID][medicId.poseID];
      this.Agent.Animation.LoadEventKeyTable(medicId.postureID, medicId.poseID);
      this._layer = info.Layer;
      this._inEnableFade = info.MainStateInfo.InStateInfo.EnableFade;
      this._inFadeSecond = info.MainStateInfo.InStateInfo.FadeSecond;
      this.Agent.Animation.InitializeStates(info);
      this.Agent.Animation.PlayInAnimation(this._inEnableFade, this._inFadeSecond, info.MainStateInfo.FadeOutTime, this._layer);
    }

    public virtual TaskStatus OnUpdate()
    {
      return this.Agent.Animation.PlayingInAnimation ? (TaskStatus) 3 : (TaskStatus) 2;
    }
  }
}
