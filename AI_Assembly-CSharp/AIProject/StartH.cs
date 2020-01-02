// Decompiled with JetBrains decompiler
// Type: AIProject.StartH
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;

namespace AIProject
{
  [TaskCategory("")]
  public class StartH : AgentAction
  {
    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      if (agent.FromFemale)
      {
        if (player.ChaControl.sex == (byte) 1 && !player.ChaControl.fileParam.futanari)
        {
          Singleton<HSceneManager>.Instance.nInvitePtn = 0;
          agent.InitiateHScene(HSceneManager.HEvent.Normal);
        }
        else
          agent.InitiateHScene(HSceneManager.HEvent.FromFemale);
      }
      else
      {
        Singleton<HSceneManager>.Instance.nInvitePtn = 0;
        agent.InitiateHScene(HSceneManager.HEvent.Normal);
      }
      return (TaskStatus) 2;
    }
  }
}
