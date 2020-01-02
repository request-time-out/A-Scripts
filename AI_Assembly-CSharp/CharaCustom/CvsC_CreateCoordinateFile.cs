// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsC_CreateCoordinateFile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Illusion.Extensions;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CvsC_CreateCoordinateFile : MonoBehaviour
  {
    [SerializeField]
    private RawImage imgDummy;
    [SerializeField]
    private CustomRender custom2DRender;
    [SerializeField]
    private CustomRender custom3DRender;
    [SerializeField]
    private CustomDrawMenu customDrawMenu;
    [SerializeField]
    private GameObject objMap3D;
    [SerializeField]
    private GameObject objBackCamera;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Camera coordinateCamera;
    private string saveCoordinateName;
    private string saveCoordinateFileName;
    private bool saveOverwrite;
    private int backPoseNo;
    private float backPosePos;

    public CvsC_CreateCoordinateFile()
    {
      base.\u002Ector();
    }

    protected CustomBase customBase
    {
      get
      {
        return Singleton<CustomBase>.Instance;
      }
    }

    protected ChaControl chaCtrl
    {
      get
      {
        return this.customBase.chaCtrl;
      }
    }

    public void CreateCoordinateFile(string _savePath, string _coordinateName, bool _overwrite)
    {
      this.saveCoordinateName = _coordinateName;
      this.saveCoordinateFileName = _savePath;
      this.saveOverwrite = _overwrite;
      this.StartCoroutine(this.CreateCoordinateFileCoroutine());
    }

    public void CreateCoordinateFileBefore()
    {
      if (this.customBase.customCtrl.draw3D)
      {
        this.custom3DRender.update = false;
        this.imgDummy.set_texture((Texture) this.custom3DRender.GetRenderTexture());
      }
      else
      {
        this.custom2DRender.update = false;
        this.imgDummy.set_texture((Texture) this.custom2DRender.GetRenderTexture());
      }
      ((Component) ((Component) this.imgDummy).get_transform().get_parent()).get_gameObject().SetActiveIfDifferent(true);
      this.chaCtrl.ChangeSettingMannequin(true);
      this.customDrawMenu.ChangeClothesStateForCapture(true);
      this.customBase.saveFrameAssist.ChangeSaveFrameTexture(1, (Texture) PngAssist.LoadTexture2DFromAssetBundle("custom/custom_etc.unity3d", "coordinate_front"));
      this.backPoseNo = this.customBase.poseNo;
      if (Object.op_Inequality((Object) null, (Object) this.chaCtrl.animBody))
      {
        AnimatorStateInfo animatorStateInfo = this.chaCtrl.getAnimatorStateInfo(0);
        this.backPosePos = ((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime();
        this.customBase.animationPos = 0.0f;
        this.customBase.ChangeAnimationNo(0, true);
        this.chaCtrl.resetDynamicBoneAll = true;
      }
      this.customBase.updateCustomUI = true;
      for (int slotNo = 0; slotNo < 20; ++slotNo)
        this.customBase.ChangeAcsSlotName(slotNo);
      this.customBase.SetUpdateToggleSetting();
      this.customBase.forceUpdateAcsList = true;
    }

    [DebuggerHidden]
    public IEnumerator CreateCoordinateFileCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsC_CreateCoordinateFile.\u003CCreateCoordinateFileCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator CreateCoordinatePng(IObserver<byte[]> observer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsC_CreateCoordinateFile.\u003CCreateCoordinatePng\u003Ec__Iterator1()
      {
        observer = observer,
        \u0024this = this
      };
    }
  }
}
