// Decompiled with JetBrains decompiler
// Type: AIProject.StartYobaiADV
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using System;
using UniRx;

namespace AIProject
{
  [TaskCategory("")]
  public class StartYobaiADV : AgentAction
  {
    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, 1f, true), (System.Action<M0>) (_ => {}), (System.Action) (() =>
      {
        Singleton<Manager.Map>.Instance.Player.PlayerController.ChangeState("Sex");
        Singleton<Manager.Map>.Instance.Player.StartSneakH(agent);
      }));
      agent.ChangeBehavior(Desire.ActionType.Idle);
      Singleton<Manager.Map>.Instance.Player.PlayerController.ChangeState("Idle");
      MapUIContainer.SetActiveCommandList(false);
      Singleton<Manager.Map>.Instance.Player.SetScheduledInteractionState(false);
      Singleton<Manager.Map>.Instance.Player.ReleaseInteraction();
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      return (TaskStatus) 2;
    }
  }
}
