// Decompiled with JetBrains decompiler
// Type: Rewired.Utils.ExternalTools
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Rewired.Internal;
using Rewired.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Utils
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public class ExternalTools : IExternalTools
  {
    public object GetPlatformInitializer()
    {
      return (object) null;
    }

    public string GetFocusedEditorWindowTitle()
    {
      return string.Empty;
    }

    public bool IsEditorSceneViewFocused()
    {
      return false;
    }

    public bool LinuxInput_IsJoystickPreconfigured(string name)
    {
      return false;
    }

    public event Action<uint, bool> XboxOneInput_OnGamepadStateChange;

    public int XboxOneInput_GetUserIdForGamepad(uint id)
    {
      return 0;
    }

    public ulong XboxOneInput_GetControllerId(uint unityJoystickId)
    {
      return 0;
    }

    public bool XboxOneInput_IsGamepadActive(uint unityJoystickId)
    {
      return false;
    }

    public string XboxOneInput_GetControllerType(ulong xboxControllerId)
    {
      return string.Empty;
    }

    public uint XboxOneInput_GetJoystickId(ulong xboxControllerId)
    {
      return 0;
    }

    public void XboxOne_Gamepad_UpdatePlugin()
    {
    }

    public bool XboxOne_Gamepad_SetGamepadVibration(
      ulong xboxOneJoystickId,
      float leftMotor,
      float rightMotor,
      float leftTriggerLevel,
      float rightTriggerLevel)
    {
      return false;
    }

    public void XboxOne_Gamepad_PulseVibrateMotor(
      ulong xboxOneJoystickId,
      int motorInt,
      float startLevel,
      float endLevel,
      ulong durationMS)
    {
    }

    public Vector3 PS4Input_GetLastAcceleration(int id)
    {
      return Vector3.get_zero();
    }

    public Vector3 PS4Input_GetLastGyro(int id)
    {
      return Vector3.get_zero();
    }

    public Vector4 PS4Input_GetLastOrientation(int id)
    {
      return Vector4.get_zero();
    }

    public void PS4Input_GetLastTouchData(
      int id,
      out int touchNum,
      out int touch0x,
      out int touch0y,
      out int touch0id,
      out int touch1x,
      out int touch1y,
      out int touch1id)
    {
      touchNum = 0;
      touch0x = 0;
      touch0y = 0;
      touch0id = 0;
      touch1x = 0;
      touch1y = 0;
      touch1id = 0;
    }

    public void PS4Input_GetPadControllerInformation(
      int id,
      out float touchpixelDensity,
      out int touchResolutionX,
      out int touchResolutionY,
      out int analogDeadZoneLeft,
      out int analogDeadZoneright,
      out int connectionType)
    {
      touchpixelDensity = 0.0f;
      touchResolutionX = 0;
      touchResolutionY = 0;
      analogDeadZoneLeft = 0;
      analogDeadZoneright = 0;
      connectionType = 0;
    }

    public void PS4Input_PadSetMotionSensorState(int id, bool bEnable)
    {
    }

    public void PS4Input_PadSetTiltCorrectionState(int id, bool bEnable)
    {
    }

    public void PS4Input_PadSetAngularVelocityDeadbandState(int id, bool bEnable)
    {
    }

    public void PS4Input_PadSetLightBar(int id, int red, int green, int blue)
    {
    }

    public void PS4Input_PadResetLightBar(int id)
    {
    }

    public void PS4Input_PadSetVibration(int id, int largeMotor, int smallMotor)
    {
    }

    public void PS4Input_PadResetOrientation(int id)
    {
    }

    public bool PS4Input_PadIsConnected(int id)
    {
      return false;
    }

    public void PS4Input_GetUsersDetails(int slot, object loggedInUser)
    {
    }

    public int PS4Input_GetDeviceClassForHandle(int handle)
    {
      return -1;
    }

    public string PS4Input_GetDeviceClassString(int intValue)
    {
      return (string) null;
    }

    public int PS4Input_PadGetUsersHandles2(int maxControllers, int[] handles)
    {
      return 0;
    }

    public Vector3 PS4Input_GetLastMoveAcceleration(int id, int index)
    {
      return Vector3.get_zero();
    }

    public Vector3 PS4Input_GetLastMoveGyro(int id, int index)
    {
      return Vector3.get_zero();
    }

    public int PS4Input_MoveGetButtons(int id, int index)
    {
      return 0;
    }

    public int PS4Input_MoveGetAnalogButton(int id, int index)
    {
      return 0;
    }

    public bool PS4Input_MoveIsConnected(int id, int index)
    {
      return false;
    }

    public int PS4Input_MoveGetUsersMoveHandles(
      int maxNumberControllers,
      int[] primaryHandles,
      int[] secondaryHandles)
    {
      return 0;
    }

    public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers, int[] primaryHandles)
    {
      return 0;
    }

    public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers)
    {
      return 0;
    }

    public IntPtr PS4Input_MoveGetControllerInputForTracking()
    {
      return IntPtr.Zero;
    }

    public void GetSpecialControllerInformation(int id, int padIndex, object controllerInformation)
    {
    }

    public Vector3 PS4Input_SpecialGetLastAcceleration(int id)
    {
      return Vector3.get_zero();
    }

    public Vector3 PS4Input_SpecialGetLastGyro(int id)
    {
      return Vector3.get_zero();
    }

    public Vector4 PS4Input_SpecialGetLastOrientation(int id)
    {
      return Vector4.get_zero();
    }

    public int PS4Input_SpecialGetUsersHandles(int maxNumberControllers, int[] handles)
    {
      return 0;
    }

    public int PS4Input_SpecialGetUsersHandles2(int maxNumberControllers, int[] handles)
    {
      return 0;
    }

    public bool PS4Input_SpecialIsConnected(int id)
    {
      return false;
    }

    public void PS4Input_SpecialResetLightSphere(int id)
    {
    }

    public void PS4Input_SpecialResetOrientation(int id)
    {
    }

    public void PS4Input_SpecialSetAngularVelocityDeadbandState(int id, bool bEnable)
    {
    }

    public void PS4Input_SpecialSetLightSphere(int id, int red, int green, int blue)
    {
    }

    public void PS4Input_SpecialSetMotionSensorState(int id, bool bEnable)
    {
    }

    public void PS4Input_SpecialSetTiltCorrectionState(int id, bool bEnable)
    {
    }

    public void PS4Input_SpecialSetVibration(int id, int largeMotor, int smallMotor)
    {
    }

    public void GetDeviceVIDPIDs(out List<int> vids, out List<int> pids)
    {
      vids = new List<int>();
      pids = new List<int>();
    }

    public int GetAndroidAPILevel()
    {
      return -1;
    }

    public bool UnityUI_Graphic_GetRaycastTarget(object graphic)
    {
      return !Object.op_Equality((Object) (graphic as Graphic), (Object) null) && (graphic as Graphic).get_raycastTarget();
    }

    public void UnityUI_Graphic_SetRaycastTarget(object graphic, bool value)
    {
      if (Object.op_Equality((Object) (graphic as Graphic), (Object) null))
        return;
      (graphic as Graphic).set_raycastTarget(value);
    }

    public bool UnityInput_IsTouchPressureSupported
    {
      get
      {
        return Input.get_touchPressureSupported();
      }
    }

    public float UnityInput_GetTouchPressure(ref Touch touch)
    {
      return ((Touch) ref touch).get_pressure();
    }

    public float UnityInput_GetTouchMaximumPossiblePressure(ref Touch touch)
    {
      return ((Touch) ref touch).get_maximumPossiblePressure();
    }

    public IControllerTemplate CreateControllerTemplate(
      Guid typeGuid,
      object payload)
    {
      return ControllerTemplateFactory.Create(typeGuid, payload);
    }

    public Type[] GetControllerTemplateTypes()
    {
      return ControllerTemplateFactory.templateTypes;
    }

    public Type[] GetControllerTemplateInterfaceTypes()
    {
      return ControllerTemplateFactory.templateInterfaceTypes;
    }
  }
}
