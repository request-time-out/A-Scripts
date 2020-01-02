// Decompiled with JetBrains decompiler
// Type: Cinemachine.PostFX.CinemachinePostProcessing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

namespace Cinemachine.PostFX
{
  [DocumentationSorting]
  [ExecuteInEditMode]
  [AddComponentMenu("")]
  [SaveDuringPlay]
  public class CinemachinePostProcessing : CinemachineExtension
  {
    private static string sVolumeOwnerName = "__CMVolumes";
    private static List<PostProcessVolume> sVolumes = new List<PostProcessVolume>();
    private static Dictionary<CinemachineBrain, PostProcessLayer> mBrainToLayer = new Dictionary<CinemachineBrain, PostProcessLayer>();
    [Tooltip("If checked, then the Focus Distance will be set to the distance between the camera and the LookAt target.  Requires DepthOfField effect in the Profile")]
    public bool m_FocusTracksTarget;
    [Tooltip("Offset from target distance, to be used with Focus Tracks Target.  Offsets the sharpest point away from the LookAt target.")]
    public float m_FocusOffset;
    [Tooltip("This Post-Processing profile will be applied whenever this virtual camera is live")]
    public PostProcessProfile m_Profile;
    private bool mCachedProfileIsInvalid;
    private PostProcessProfile mProfileCopy;

    public CinemachinePostProcessing()
    {
      base.\u002Ector();
    }

    public PostProcessProfile Profile
    {
      get
      {
        return Object.op_Inequality((Object) this.mProfileCopy, (Object) null) ? this.mProfileCopy : this.m_Profile;
      }
    }

    public bool IsValid
    {
      get
      {
        return Object.op_Inequality((Object) this.m_Profile, (Object) null) && ((List<PostProcessEffectSettings>) this.m_Profile.settings).Count > 0;
      }
    }

    public void InvalidateCachedProfile()
    {
      this.mCachedProfileIsInvalid = true;
    }

    private void CreateProfileCopy()
    {
      this.DestroyProfileCopy();
      PostProcessProfile instance = (PostProcessProfile) ScriptableObject.CreateInstance<PostProcessProfile>();
      if (Object.op_Inequality((Object) this.m_Profile, (Object) null))
      {
        using (List<PostProcessEffectSettings>.Enumerator enumerator = ((List<PostProcessEffectSettings>) this.m_Profile.settings).GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            PostProcessEffectSettings processEffectSettings = (PostProcessEffectSettings) Object.Instantiate<PostProcessEffectSettings>((M0) enumerator.Current);
            ((List<PostProcessEffectSettings>) instance.settings).Add(processEffectSettings);
          }
        }
      }
      this.mProfileCopy = instance;
      this.mCachedProfileIsInvalid = false;
    }

    private void DestroyProfileCopy()
    {
      if (Object.op_Inequality((Object) this.mProfileCopy, (Object) null))
        RuntimeUtility.DestroyObject((Object) this.mProfileCopy);
      this.mProfileCopy = (PostProcessProfile) null;
    }

    protected virtual void OnDestroy()
    {
      base.OnDestroy();
      this.DestroyProfileCopy();
    }

    protected virtual void PostPipelineStageCallback(
      CinemachineVirtualCameraBase vcam,
      CinemachineCore.Stage stage,
      ref CameraState state,
      float deltaTime)
    {
      if (stage != 1)
        return;
      if (!this.IsValid)
      {
        this.DestroyProfileCopy();
      }
      else
      {
        if (!this.m_FocusTracksTarget)
        {
          this.DestroyProfileCopy();
        }
        else
        {
          if (Object.op_Equality((Object) this.mProfileCopy, (Object) null) || this.mCachedProfileIsInvalid)
            this.CreateProfileCopy();
          DepthOfField depthOfField;
          // ISSUE: cast to a reference type
          if (this.mProfileCopy.TryGetSettings<DepthOfField>((M0&) ref depthOfField))
          {
            float num1 = this.m_FocusOffset;
            if (((CameraState) ref state).get_HasLookAt())
            {
              double num2 = (double) num1;
              Vector3 vector3 = Vector3.op_Subtraction(((CameraState) ref state).get_FinalPosition(), ((CameraState) ref state).get_ReferenceLookAt());
              double magnitude = (double) ((Vector3) ref vector3).get_magnitude();
              num1 = (float) (num2 + magnitude);
            }
            ((ParameterOverride<float>) depthOfField.focusDistance).value = (__Null) (double) Mathf.Max(0.0f, num1);
          }
        }
        ((CameraState) ref state).AddCustomBlendable(new CameraState.CustomBlendable((Object) this, 1f));
      }
    }

    private static void OnCameraCut(CinemachineBrain brain)
    {
      PostProcessLayer ppLayer = CinemachinePostProcessing.GetPPLayer(brain);
      if (!Object.op_Inequality((Object) ppLayer, (Object) null))
        return;
      ppLayer.ResetHistory();
    }

    private static void ApplyPostFX(CinemachineBrain brain)
    {
      PostProcessLayer ppLayer = CinemachinePostProcessing.GetPPLayer(brain);
      if (Object.op_Equality((Object) ppLayer, (Object) null) || !((Behaviour) ppLayer).get_enabled() || LayerMask.op_Implicit((LayerMask) ppLayer.volumeLayer) == 0)
        return;
      CameraState currentCameraState = brain.get_CurrentCameraState();
      int customBlendables = ((CameraState) ref currentCameraState).get_NumCustomBlendables();
      List<PostProcessVolume> dynamicBrainVolumes = CinemachinePostProcessing.GetDynamicBrainVolumes(brain, ppLayer, customBlendables);
      for (int index = 0; index < dynamicBrainVolumes.Count; ++index)
      {
        dynamicBrainVolumes[index].weight = (__Null) 0.0;
        dynamicBrainVolumes[index].sharedProfile = null;
        dynamicBrainVolumes[index].set_profile((PostProcessProfile) null);
      }
      PostProcessVolume postProcessVolume1 = (PostProcessVolume) null;
      int num = 0;
      for (int index = 0; index < customBlendables; ++index)
      {
        CameraState.CustomBlendable customBlendable = ((CameraState) ref currentCameraState).GetCustomBlendable(index);
        CinemachinePostProcessing custom = customBlendable.m_Custom as CinemachinePostProcessing;
        if (!Object.op_Equality((Object) custom, (Object) null))
        {
          PostProcessVolume postProcessVolume2 = dynamicBrainVolumes[index];
          if (Object.op_Equality((Object) postProcessVolume1, (Object) null))
            postProcessVolume1 = postProcessVolume2;
          postProcessVolume2.sharedProfile = (__Null) custom.Profile;
          postProcessVolume2.isGlobal = (__Null) 1;
          postProcessVolume2.priority = (__Null) (3.40282346638529E+38 - (double) (customBlendables - index) - 1.0);
          postProcessVolume2.weight = customBlendable.m_Weight;
          ++num;
        }
        if (num > 1)
          postProcessVolume1.weight = (__Null) 1.0;
      }
    }

    private static List<PostProcessVolume> GetDynamicBrainVolumes(
      CinemachineBrain brain,
      PostProcessLayer ppLayer,
      int minVolumes)
    {
      GameObject gameObject1 = (GameObject) null;
      Transform transform = ((Component) brain).get_transform();
      int childCount = transform.get_childCount();
      CinemachinePostProcessing.sVolumes.Clear();
      for (int index = 0; Object.op_Equality((Object) gameObject1, (Object) null) && index < childCount; ++index)
      {
        GameObject gameObject2 = ((Component) transform.GetChild(index)).get_gameObject();
        if (((Object) gameObject2).get_hideFlags() == 61)
        {
          gameObject2.GetComponents<PostProcessVolume>((List<M0>) CinemachinePostProcessing.sVolumes);
          if (CinemachinePostProcessing.sVolumes.Count > 0)
            gameObject1 = gameObject2;
        }
      }
      if (minVolumes > 0)
      {
        if (Object.op_Equality((Object) gameObject1, (Object) null))
        {
          gameObject1 = new GameObject(CinemachinePostProcessing.sVolumeOwnerName);
          ((Object) gameObject1).set_hideFlags((HideFlags) 61);
          gameObject1.get_transform().set_parent(transform);
        }
        int num = ((LayerMask) ref ppLayer.volumeLayer).get_value();
        for (int index = 0; index < 32; ++index)
        {
          if ((num & 1 << index) != 0)
          {
            gameObject1.set_layer(index);
            break;
          }
        }
        while (CinemachinePostProcessing.sVolumes.Count < minVolumes)
          CinemachinePostProcessing.sVolumes.Add((PostProcessVolume) gameObject1.get_gameObject().AddComponent<PostProcessVolume>());
      }
      return CinemachinePostProcessing.sVolumes;
    }

    private static PostProcessLayer GetPPLayer(CinemachineBrain brain)
    {
      PostProcessLayer postProcessLayer = (PostProcessLayer) null;
      if (CinemachinePostProcessing.mBrainToLayer.TryGetValue(brain, out postProcessLayer))
        return postProcessLayer;
      PostProcessLayer component = (PostProcessLayer) ((Component) brain).GetComponent<PostProcessLayer>();
      CinemachinePostProcessing.mBrainToLayer[brain] = component;
      if (Object.op_Inequality((Object) component, (Object) null))
      {
        // ISSUE: variable of the null type
        __Null cameraCutEvent = brain.m_CameraCutEvent;
        // ISSUE: reference to a compiler-generated field
        if (CinemachinePostProcessing.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          CinemachinePostProcessing.\u003C\u003Ef__mg\u0024cache0 = new UnityAction<CinemachineBrain>((object) null, __methodptr(OnCameraCut));
        }
        // ISSUE: reference to a compiler-generated field
        UnityAction<CinemachineBrain> fMgCache0 = CinemachinePostProcessing.\u003C\u003Ef__mg\u0024cache0;
        ((UnityEvent<CinemachineBrain>) cameraCutEvent).AddListener(fMgCache0);
      }
      else
      {
        // ISSUE: variable of the null type
        __Null cameraCutEvent = brain.m_CameraCutEvent;
        // ISSUE: reference to a compiler-generated field
        if (CinemachinePostProcessing.\u003C\u003Ef__mg\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          CinemachinePostProcessing.\u003C\u003Ef__mg\u0024cache1 = new UnityAction<CinemachineBrain>((object) null, __methodptr(OnCameraCut));
        }
        // ISSUE: reference to a compiler-generated field
        UnityAction<CinemachineBrain> fMgCache1 = CinemachinePostProcessing.\u003C\u003Ef__mg\u0024cache1;
        ((UnityEvent<CinemachineBrain>) cameraCutEvent).RemoveListener(fMgCache1);
      }
      return component;
    }

    [RuntimeInitializeOnLoadMethod]
    private static void InitializeModule()
    {
      // ISSUE: variable of the null type
      __Null cameraUpdatedEvent1 = CinemachineCore.CameraUpdatedEvent;
      // ISSUE: reference to a compiler-generated field
      if (CinemachinePostProcessing.\u003C\u003Ef__mg\u0024cache2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        CinemachinePostProcessing.\u003C\u003Ef__mg\u0024cache2 = new UnityAction<CinemachineBrain>((object) null, __methodptr(ApplyPostFX));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction<CinemachineBrain> fMgCache2 = CinemachinePostProcessing.\u003C\u003Ef__mg\u0024cache2;
      ((UnityEvent<CinemachineBrain>) cameraUpdatedEvent1).RemoveListener(fMgCache2);
      // ISSUE: variable of the null type
      __Null cameraUpdatedEvent2 = CinemachineCore.CameraUpdatedEvent;
      // ISSUE: reference to a compiler-generated field
      if (CinemachinePostProcessing.\u003C\u003Ef__mg\u0024cache3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        CinemachinePostProcessing.\u003C\u003Ef__mg\u0024cache3 = new UnityAction<CinemachineBrain>((object) null, __methodptr(ApplyPostFX));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction<CinemachineBrain> fMgCache3 = CinemachinePostProcessing.\u003C\u003Ef__mg\u0024cache3;
      ((UnityEvent<CinemachineBrain>) cameraUpdatedEvent2).AddListener(fMgCache3);
    }
  }
}
