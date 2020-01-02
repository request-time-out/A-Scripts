// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.PressAnyButtonToJoinExample_Assigner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
  [AddComponentMenu("")]
  public class PressAnyButtonToJoinExample_Assigner : MonoBehaviour
  {
    public PressAnyButtonToJoinExample_Assigner()
    {
      base.\u002Ector();
    }

    private void Update()
    {
      if (!ReInput.get_isReady())
        return;
      this.AssignJoysticksToPlayers();
    }

    private void AssignJoysticksToPlayers()
    {
      IList<Joystick> joysticks = ReInput.get_controllers().get_Joysticks();
      for (int index = 0; index < ((ICollection<Joystick>) joysticks).Count; ++index)
      {
        Joystick joystick = joysticks[index];
        if (!ReInput.get_controllers().IsControllerAssigned(((Controller) joystick).get_type(), (int) ((Controller) joystick).id) && ((Controller) joystick).GetAnyButtonDown())
          ((Player.ControllerHelper) this.FindPlayerWithoutJoystick()?.controllers).AddController((Controller) joystick, false);
      }
      if (!this.DoAllPlayersHaveJoysticks())
        return;
      ReInput.get_configuration().set_autoAssignJoysticks(true);
      ((Behaviour) this).set_enabled(false);
    }

    private Player FindPlayerWithoutJoystick()
    {
      IList<Player> players = ReInput.get_players().get_Players();
      for (int index = 0; index < ((ICollection<Player>) players).Count; ++index)
      {
        if (((Player.ControllerHelper) players[index].controllers).get_joystickCount() <= 0)
          return players[index];
      }
      return (Player) null;
    }

    private bool DoAllPlayersHaveJoysticks()
    {
      return this.FindPlayerWithoutJoystick() == null;
    }
  }
}
