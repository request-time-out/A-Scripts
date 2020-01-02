// Decompiled with JetBrains decompiler
// Type: Exploder.ExploderUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Diagnostics;
using UnityEngine;

namespace Exploder
{
  public static class ExploderUtils
  {
    [Conditional("UNITY_EDITOR_NO_DEBUG")]
    public static void Assert(bool condition, string message)
    {
      if (condition)
        return;
      Debug.LogError((object) ("Assert! " + message));
      Debug.Break();
    }

    [Conditional("UNITY_EDITOR_NO_DEBUG")]
    public static void Warning(bool condition, string message)
    {
      if (condition)
        return;
      Debug.LogWarning((object) ("Warning! " + message));
    }

    [Conditional("UNITY_EDITOR_NO_DEBUG")]
    public static void Log(string message)
    {
      Debug.Log((object) message);
    }

    public static Vector3 GetCentroid(GameObject obj)
    {
      MeshRenderer[] componentsInChildren = (MeshRenderer[]) obj.GetComponentsInChildren<MeshRenderer>();
      Vector3 vector3_1 = Vector3.get_zero();
      if (componentsInChildren == null || componentsInChildren.Length == 0)
      {
        SkinnedMeshRenderer componentInChildren = (SkinnedMeshRenderer) obj.GetComponentInChildren<SkinnedMeshRenderer>();
        if (!Object.op_Implicit((Object) componentInChildren))
          return obj.get_transform().get_position();
        Bounds bounds = ((Renderer) componentInChildren).get_bounds();
        return ((Bounds) ref bounds).get_center();
      }
      foreach (MeshRenderer meshRenderer in componentsInChildren)
      {
        Vector3 vector3_2 = vector3_1;
        Bounds bounds = ((Renderer) meshRenderer).get_bounds();
        Vector3 center = ((Bounds) ref bounds).get_center();
        vector3_1 = Vector3.op_Addition(vector3_2, center);
      }
      return Vector3.op_Division(vector3_1, (float) componentsInChildren.Length);
    }

    public static void SetVisible(GameObject obj, bool status)
    {
      if (!Object.op_Implicit((Object) obj))
        return;
      foreach (Renderer componentsInChild in (MeshRenderer[]) obj.GetComponentsInChildren<MeshRenderer>())
        componentsInChild.set_enabled(status);
    }

    public static void ClearLog()
    {
    }

    public static bool IsActive(GameObject obj)
    {
      return Object.op_Implicit((Object) obj) && obj.get_activeSelf();
    }

    public static void SetActive(GameObject obj, bool status)
    {
      if (!Object.op_Implicit((Object) obj))
        return;
      obj.SetActive(status);
    }

    public static void SetActiveRecursively(GameObject obj, bool status)
    {
      if (!Object.op_Implicit((Object) obj))
        return;
      int childCount = obj.get_transform().get_childCount();
      for (int index = 0; index < childCount; ++index)
        ExploderUtils.SetActiveRecursively(((Component) obj.get_transform().GetChild(index)).get_gameObject(), status);
      obj.SetActive(status);
    }

    public static void EnableCollider(GameObject obj, bool status)
    {
      if (!Object.op_Implicit((Object) obj))
        return;
      foreach (Collider componentsInChild in (Collider[]) obj.GetComponentsInChildren<Collider>())
        componentsInChild.set_enabled(status);
    }

    public static bool IsExplodable(GameObject obj)
    {
      bool flag = Object.op_Inequality((Object) obj.GetComponent<Explodable>(), (Object) null);
      if (!flag)
        flag = obj.CompareTag(ExploderObject.Tag);
      return flag;
    }

    public static void CopyAudioSource(AudioSource src, AudioSource dst)
    {
      dst.set_bypassEffects(src.get_bypassEffects());
      dst.set_bypassListenerEffects(src.get_bypassListenerEffects());
      dst.set_bypassReverbZones(src.get_bypassReverbZones());
      dst.set_clip(src.get_clip());
      dst.set_dopplerLevel(src.get_dopplerLevel());
      ((Behaviour) dst).set_enabled(((Behaviour) src).get_enabled());
      dst.set_ignoreListenerPause(src.get_ignoreListenerPause());
      dst.set_ignoreListenerVolume(src.get_ignoreListenerVolume());
      dst.set_loop(src.get_loop());
      dst.set_maxDistance(src.get_maxDistance());
      dst.set_minDistance(src.get_minDistance());
      dst.set_mute(src.get_mute());
      dst.set_outputAudioMixerGroup(src.get_outputAudioMixerGroup());
      dst.set_panStereo(src.get_panStereo());
      dst.set_pitch(src.get_pitch());
      dst.set_playOnAwake(src.get_playOnAwake());
      dst.set_priority(src.get_priority());
      dst.set_reverbZoneMix(src.get_reverbZoneMix());
      dst.set_rolloffMode(src.get_rolloffMode());
      dst.set_spatialBlend(src.get_spatialBlend());
      dst.set_spatialize(src.get_spatialize());
      dst.set_spread(src.get_spread());
      dst.set_time(src.get_time());
      dst.set_timeSamples(src.get_timeSamples());
      dst.set_velocityUpdateMode(src.get_velocityUpdateMode());
      dst.set_volume(src.get_volume());
    }
  }
}
