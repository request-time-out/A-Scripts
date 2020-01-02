// Decompiled with JetBrains decompiler
// Type: AIProject.HousingDoorAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using UnityEngine;

namespace AIProject
{
  [RequireComponent(typeof (DoorPoint))]
  public class HousingDoorAnimation : DoorAnimation
  {
    [SerializeField]
    private DoorMatType _doorMatType;

    protected override void OnStart()
    {
      if (!Object.op_Inequality((Object) this._animator, (Object) null))
        return;
      this._animator.set_runtimeAnimatorController(Singleton<Resources>.Instance.Animation.GetItemAnimator(this._animatorID));
      this._animator.Play(Singleton<Resources>.Instance.CommonDefine.ItemAnims.DoorDefaultState, 0, 0.0f);
    }

    public override void PlayMoveSE(bool open)
    {
      if (!Singleton<Manager.Map>.IsInstance() || !Singleton<Resources>.IsInstance())
        return;
      Transform transform = ((Component) Manager.Map.GetCameraComponent())?.get_transform();
      if (Object.op_Equality((Object) transform, (Object) null))
        return;
      SoundPack soundPack = Singleton<Resources>.Instance.SoundPack;
      SoundPack.DoorSEIDInfo doorSeidInfo;
      if (!soundPack.DoorIDTable.TryGetValue(this._doorMatType, out doorSeidInfo))
        return;
      int clipID = !open ? doorSeidInfo.CloseID : doorSeidInfo.OpenID;
      SoundPack.Data3D data;
      if (!soundPack.TryGetActionSEData(clipID, out data))
        return;
      Vector3 position = ((Component) this).get_transform().get_position();
      float num = Mathf.Pow(data.MaxDistance + soundPack.Game3DInfo.MarginMaxDistance, 2f);
      Vector3 vector3 = Vector3.op_Subtraction(position, transform.get_position());
      float sqrMagnitude = ((Vector3) ref vector3).get_sqrMagnitude();
      if ((double) num < (double) sqrMagnitude)
        return;
      AudioSource audioSource = soundPack.Play((SoundPack.IData) data, Sound.Type.GameSE3D, 0.0f);
      if (Object.op_Equality((Object) audioSource, (Object) null))
        return;
      audioSource.Stop();
      ((Component) audioSource).get_transform().set_position(position);
      audioSource.Play();
    }
  }
}
