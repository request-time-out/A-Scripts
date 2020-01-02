// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Normal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Manager;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace AIProject.Player
{
  public class Normal : PlayerStateBase
  {
    private float _elapsedTimeOnLeft;

    protected override void OnAwake(PlayerActor player)
    {
      player.Animation.RefsActAnimInfo = true;
      player.ActivateTransfer();
      if (player.HandsHolder.EnabledHolding)
      {
        player.HandsHolder.EnabledHolding = false;
        player.HandsHolder.EnabledHolding = true;
      }
      player.SetActiveOnEquipedItem(true);
      player.ResetCoolTime();
      if (player.CameraControl.Mode == CameraMode.ActionNotMove || player.CameraControl.Mode == CameraMode.ActionFreeLook)
      {
        player.CameraControl.Mode = CameraMode.Normal;
        player.CameraControl.RecoverShotType();
      }
      if (!Singleton<Manager.Map>.IsInstance())
        return;
      Singleton<Manager.Map>.Instance.CheckTutorialState(player);
    }

    public override void Release(Actor actor, EventType type)
    {
      if (!Singleton<GameCursor>.IsInstance())
        return;
      Singleton<GameCursor>.Instance.SetCursorLock(false);
    }

    protected override void OnAfterUpdate(PlayerActor actor, Actor.InputInfo info)
    {
      actor.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    protected override void OnUpdate(PlayerActor actor, ref Actor.InputInfo info)
    {
      Singleton<Manager.Map>.Instance.CheckStoryProgress();
      if (actor == null)
        return;
      Input instance = Singleton<Input>.Instance;
      PlayerActor playerActor = actor;
      if (instance.State == Input.ValidType.Action)
      {
        Transform transform = ((Component) playerActor.CameraControl).get_transform();
        Vector2 moveAxis = instance.MoveAxis;
        Quaternion rotation = transform.get_rotation();
        Vector3 vector3 = Quaternion.op_Multiply(Quaternion.Euler(0.0f, (float) ((Quaternion) ref rotation).get_eulerAngles().y, 0.0f), Vector3.ClampMagnitude(new Vector3((float) moveAxis.x, 0.0f, (float) moveAxis.y), 1f));
        info.move = vector3;
        info.lookPos = Vector3.op_Addition(((Component) actor).get_transform().get_position(), Vector3.op_Multiply(transform.get_forward(), 100f));
        StuffItem equipedLampItem = playerActor.PlayerData.EquipedLampItem;
        if (Mathf.Approximately(((Vector2) ref moveAxis).get_sqrMagnitude(), 0.0f) && (equipedLampItem == null || !Singleton<Resources>.Instance.CommonDefine.ItemIDDefine.ContainsLightItem(equipedLampItem)))
        {
          this._elapsedTimeOnLeft += Time.get_deltaTime();
          if ((double) this._elapsedTimeOnLeft <= (double) Singleton<Resources>.Instance.LocomotionProfile.TimeToLeftState)
            return;
          playerActor.Controller.ChangeState("Houchi");
        }
        else
          this._elapsedTimeOnLeft = 0.0f;
      }
      else
      {
        info.move = Vector3.get_zero();
        Transform transform = ((Component) playerActor.CameraControl).get_transform();
        info.lookPos = Vector3.op_Addition(((Component) actor).get_transform().get_position(), Vector3.op_Multiply(transform.get_forward(), 100f));
      }
    }

    [DebuggerHidden]
    public override IEnumerator End(Actor actor)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Normal.\u003CEnd\u003Ec__Iterator0 endCIterator0 = new Normal.\u003CEnd\u003Ec__Iterator0();
      return (IEnumerator) endCIterator0;
    }
  }
}
