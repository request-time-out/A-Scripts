// Decompiled with JetBrains decompiler
// Type: ADV.Backup.CameraData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace ADV.Backup
{
  internal class CameraData
  {
    private Rect rect;
    private Transform parent;
    private float fov;
    private float far;
    private CameraData.BlurBK blurBK;
    private CameraData.DOFBK dofBK;

    public CameraData(Camera cam)
    {
      this.blurBK = (CameraData.BlurBK) null;
      this.dofBK = (CameraData.DOFBK) null;
      if (Object.op_Equality((Object) cam, (Object) null))
        return;
      this.rect = cam.get_rect();
      this.parent = ((Component) cam).get_transform().get_parent();
      this.fov = cam.get_fieldOfView();
      this.far = cam.get_farClipPlane();
      Blur component1 = (Blur) ((Component) cam).GetComponent<Blur>();
      if (Object.op_Inequality((Object) component1, (Object) null))
        this.blurBK = new CameraData.BlurBK(component1);
      DepthOfField component2 = (DepthOfField) ((Component) cam).GetComponent<DepthOfField>();
      if (!Object.op_Inequality((Object) component2, (Object) null))
        return;
      this.dofBK = new CameraData.DOFBK(component2);
    }

    public void Load(Camera cam)
    {
      if (Object.op_Equality((Object) cam, (Object) null))
        return;
      cam.set_rect(this.rect);
      ((Component) cam).get_transform().set_parent(this.parent);
      cam.set_fieldOfView(this.fov);
      cam.set_farClipPlane(this.far);
      this.blurBK.SafeProc<CameraData.BlurBK>((Action<CameraData.BlurBK>) (p => p.Set((Blur) ((Component) cam).GetComponent<Blur>())));
      this.dofBK.SafeProc<CameraData.DOFBK>((Action<CameraData.DOFBK>) (p => p.Set((DepthOfField) ((Component) cam).GetComponent<DepthOfField>())));
    }

    private class BlurBK
    {
      private bool enabled;
      private int iterations;
      private float blurSpread;

      public BlurBK(Blur blur)
      {
        if (Object.op_Equality((Object) blur, (Object) null))
          return;
        this.enabled = ((Behaviour) blur).get_enabled();
        this.iterations = (int) blur.iterations;
        this.blurSpread = (float) blur.blurSpread;
      }

      public void Set(Blur blur)
      {
        if (Object.op_Equality((Object) blur, (Object) null))
          return;
        ((Behaviour) blur).set_enabled(this.enabled);
        blur.iterations = (__Null) this.iterations;
        blur.blurSpread = (__Null) (double) this.blurSpread;
      }
    }

    private class DOFBK
    {
      private bool enabled;
      private Transform focalTransform;
      private float focalLength;
      private float focalSize;
      private float aperture;

      public DOFBK(DepthOfField dof)
      {
        if (Object.op_Equality((Object) dof, (Object) null))
          return;
        this.enabled = ((Behaviour) dof).get_enabled();
        this.focalTransform = (Transform) dof.focalTransform;
        this.focalLength = (float) dof.focalLength;
        this.focalSize = (float) dof.focalSize;
        this.aperture = (float) dof.aperture;
      }

      public void Set(DepthOfField dof)
      {
        if (Object.op_Equality((Object) dof, (Object) null))
          return;
        ((Behaviour) dof).set_enabled(this.enabled);
        dof.focalTransform = (__Null) this.focalTransform;
        dof.focalLength = (__Null) (double) this.focalLength;
        dof.focalSize = (__Null) (double) this.focalSize;
        dof.aperture = (__Null) (double) this.aperture;
      }
    }
  }
}
