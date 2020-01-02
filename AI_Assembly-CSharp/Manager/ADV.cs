// Decompiled with JetBrains decompiler
// Type: Manager.ADV
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.CaptionScript;
using Cinemachine;
using IllusionUtility.GetUtility;
using System;
using UniRx;
using UnityEngine;
using UnityEx;

namespace Manager
{
  public class ADV : Singleton<ADV>
  {
    public AgentActor TargetCharacter { get; set; }

    public MerchantActor TargetMerchant { get; set; }

    public Captions Captions { get; set; }

    public AssetBundleInfo AssetBundleInfo { get; set; }

    public bool UsedBGM { get; set; }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
    }

    public static void ChangeADVCamera(Actor actor)
    {
      PlayerActor player = Singleton<Map>.Instance.Player;
      Vector3 position = player.Position;
      Vector3 pB = actor.Position;
      position.y = (__Null) (double) (pB.y = (__Null) 0.0f);
      Vector3 vector3 = Vector3.Normalize(Vector3.op_Subtraction(pB, position));
      if (Mathf.Approximately(((Vector3) ref vector3).get_magnitude(), 0.0f))
        vector3 = Vector3.get_back();
      Quaternion rotation = Quaternion.LookRotation(vector3);
      Transform target = ((Component) actor).get_transform().FindLoop("cf_J_Mune00").get_transform();
      Transform cameraTarget = player.CameraTarget.get_transform();
      Vector3 offset = Singleton<Resources>.Instance.LocomotionProfile.CommunicationOffset;
      ObservableExtensions.Subscribe<long>(Observable.Take<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), (Action<M0>) (_ =>
      {
        cameraTarget.set_position(new Vector3((float) pB.x, (float) target.get_position().y, (float) pB.z));
        if (Object.op_Inequality((Object) player.CameraControl.VanishControl, (Object) null))
          player.CameraControl.VanishControl.LookAtPosition = cameraTarget.get_position();
        player.CameraControl.ADVCamera.set_LookAt(cameraTarget);
        ((Component) player.CameraControl.ADVCamera).get_transform().set_position(Vector3.op_Addition(cameraTarget.get_position(), Quaternion.op_Multiply(rotation, offset)));
        player.CameraControl.Mode = CameraMode.ADV;
      }));
    }

    public static void ChangeADVCameraDiagonal(Actor actor)
    {
      ObservableExtensions.Subscribe<long>(Observable.Take<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), (Action<M0>) (_ =>
      {
        PlayerActor player = Singleton<Map>.Instance.Player;
        string diagonalTargetName = Singleton<Resources>.Instance.LocomotionProfile.CommunicationDiagonalTargetName;
        Transform transform1 = ((Component) player).get_transform().FindLoop(diagonalTargetName).get_transform();
        Transform transform2 = ((Component) actor).get_transform().FindLoop("cf_J_Mune00").get_transform();
        Vector3 vector3 = Vector3.Normalize(Vector3.op_Subtraction(transform1.get_position(), transform2.get_position()));
        if (Mathf.Approximately(((Vector3) ref vector3).get_magnitude(), 0.0f))
          vector3 = Vector3.get_back();
        Transform transform3 = player.CameraTarget.get_transform();
        transform3.set_position(transform2.get_position());
        if (Object.op_Inequality((Object) player.CameraControl.VanishControl, (Object) null))
          player.CameraControl.VanishControl.LookAtPosition = transform3.get_position();
        player.CameraControl.ADVCamera.set_LookAt(transform3);
        ((Component) player.CameraControl.ADVCamera).get_transform().set_position(Vector3.op_Addition(transform2.get_position(), Vector3.op_Multiply(vector3, 8f)));
        player.CameraControl.Mode = CameraMode.ADV;
      }));
    }

    public static void ChangeADVFixedAngleCamera(Actor actor, int attitudeID)
    {
      PlayerActor player = Singleton<Map>.Instance.Player;
      ObservableExtensions.Subscribe<long>(Observable.Take<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), (Action<M0>) (_ =>
      {
        GameObject loop = ((Component) actor.ChaControl.animBody).get_transform().FindLoop("cf_J_Mune00");
        if (Object.op_Inequality((Object) loop, (Object) null))
        {
          Transform advNotStandRoot = player.CameraControl.ADVNotStandRoot;
          advNotStandRoot.set_position(loop.get_transform().get_position());
          advNotStandRoot.set_rotation(actor.Rotation);
          if (Object.op_Inequality((Object) player.CameraControl.VanishControl, (Object) null))
            player.CameraControl.VanishControl.LookAtPosition = loop.get_transform().get_position();
        }
        ABInfoData.Param obj;
        if (!Singleton<Resources>.Instance.Action.ComCameraList.TryGetValue(attitudeID, out obj))
          return;
        string text = CommonLib.LoadAsset<TextAsset>(obj.AssetBundle, obj.AssetFile, false, string.Empty)?.get_text();
        if (text.IsNullOrEmpty())
        {
          GlobalMethod.DebugLog("cameraファイルが読み込めません", 1);
        }
        else
        {
          string[][] data;
          GlobalMethod.GetListString(text, out data);
          Vector3 vector3;
          float result1;
          vector3.x = !float.TryParse(data[0][0], out result1) ? (__Null) 0.0 : (__Null) (double) result1;
          vector3.y = !float.TryParse(data[1][0], out result1) ? (__Null) 0.0 : (__Null) (double) result1;
          vector3.z = !float.TryParse(data[2][0], out result1) ? (__Null) 0.0 : (__Null) (double) result1;
          Quaternion quaternion;
          quaternion.x = !float.TryParse(data[3][0], out result1) ? (__Null) 0.0 : (__Null) (double) result1;
          quaternion.y = !float.TryParse(data[4][0], out result1) ? (__Null) 0.0 : (__Null) (double) result1;
          quaternion.z = !float.TryParse(data[5][0], out result1) ? (__Null) 0.0 : (__Null) (double) result1;
          quaternion.w = !float.TryParse(data[6][0], out result1) ? (__Null) 0.0 : (__Null) (double) result1;
          CinemachineVirtualCameraBase advNotStandCamera = player.CameraControl.ADVNotStandCamera;
          ((Component) advNotStandCamera).get_transform().set_localPosition(vector3);
          ((Component) advNotStandCamera).get_transform().set_localRotation(quaternion);
          CinemachineVirtualCamera cinemachineVirtualCamera = advNotStandCamera as CinemachineVirtualCamera;
          float result2;
          if (Object.op_Inequality((Object) cinemachineVirtualCamera, (Object) null) && float.TryParse(data[7][0], out result2))
          {
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            (^(LensSettings&) ref cinemachineVirtualCamera.m_Lens).FieldOfView = (__Null) (double) result2;
          }
          player.CameraControl.Mode = CameraMode.ADVExceptStand;
        }
      }), (Action<Exception>) (ex => {}), (Action) (() => {}));
    }
  }
}
