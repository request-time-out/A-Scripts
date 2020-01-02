// Decompiled with JetBrains decompiler
// Type: DebugCharaWindow.DebugChaControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DebugCharaWindow
{
  public class DebugChaControl : Singleton<DebugChaControl>
  {
    [HideInInspector]
    public int[] selectChara = new int[2];
    [HideInInspector]
    public int selectSex = 1;
    [HideInInspector]
    public Camera[] camView = new Camera[4];
    [HideInInspector]
    public int viewType = 5;
    public Dictionary<string, string> dictGuiStr = new Dictionary<string, string>();
    public Dictionary<ChaControl, DebugChaControl.DebugChaValue> dictChaValue = new Dictionary<ChaControl, DebugChaControl.DebugChaValue>();
    [HideInInspector]
    public bool drawOnGUI;
    [HideInInspector]
    public bool enableShape;
    private Camera camMain;
    [HideInInspector]
    public bool useAnotherDisplay;
    public GameObject objAudio;

    public GameScreenShot screenShot { get; private set; }

    public GameScreenShotSerial screenShotSerial { get; private set; }

    private void Start()
    {
    }

    private void Update()
    {
      if (Object.op_Inequality((Object) null, (Object) Camera.get_main()))
        this.camMain = Camera.get_main();
      if (this.enableShape)
      {
        foreach (KeyValuePair<ChaControl, DebugChaControl.DebugChaValue> keyValuePair in this.dictChaValue)
        {
          if (keyValuePair.Key.loadEnd)
          {
            for (int index = 0; index < keyValuePair.Value.shapeFace.Length; ++index)
              keyValuePair.Key.SetShapeFaceValue(index, keyValuePair.Value.shapeFace[index]);
            for (int index = 0; index < keyValuePair.Value.shapeBody.Length; ++index)
              keyValuePair.Key.SetShapeBodyValue(index, keyValuePair.Value.shapeBody[index]);
            keyValuePair.Key.ChangeBustSoftness(keyValuePair.Value.bustEtc[0]);
            keyValuePair.Key.ChangeBustGravity(keyValuePair.Value.bustEtc[1]);
            if ((double) keyValuePair.Value.bustEtc[2] != (double) keyValuePair.Key.chaFile.custom.body.areolaSize)
            {
              keyValuePair.Key.chaFile.custom.body.areolaSize = keyValuePair.Value.bustEtc[2];
              keyValuePair.Key.ChangeNipScale();
            }
            keyValuePair.Key.DisableShapeMouth(keyValuePair.Value.disableMaskFace);
            for (int LR = 0; LR < 2; ++LR)
            {
              for (int id = 0; id < ChaFileDefine.cf_BustShapeMaskID.Length; ++id)
                keyValuePair.Key.DisableShapeBodyID(LR, id, keyValuePair.Value.disableMaskBody[LR, id]);
            }
            keyValuePair.Key.updateBustSize = true;
            if (keyValuePair.Key.sex == (byte) 1 && Object.op_Inequality((Object) null, (Object) keyValuePair.Key.animBody))
            {
              foreach (AnimatorControllerParameter parameter in keyValuePair.Key.animBody.get_parameters())
              {
                switch (parameter.get_name().ToLower())
                {
                  case "height":
                    if (parameter.get_type() == 1)
                    {
                      keyValuePair.Key.animBody.SetFloat(parameter.get_name(), keyValuePair.Key.GetShapeBodyValue(0));
                      break;
                    }
                    break;
                  case "breast":
                    if (parameter.get_type() == 1)
                    {
                      keyValuePair.Key.animBody.SetFloat(parameter.get_name(), keyValuePair.Key.GetShapeBodyValue(1));
                      break;
                    }
                    break;
                }
              }
            }
          }
        }
      }
      if (!Object.op_Inequality((Object) null, (Object) this.camView[0]) || !((Behaviour) this.camView[0]).get_enabled() || !Object.op_Inequality((Object) null, (Object) this.camMain))
        return;
      ((Component) this.camView[0]).get_transform().set_localPosition(((Component) this.camMain).get_transform().get_position());
      ((Component) this.camView[0]).get_transform().set_localRotation(((Component) this.camMain).get_transform().get_rotation());
    }

    private void OnGUI()
    {
      if (!this.drawOnGUI)
        return;
      string[] array = this.dictGuiStr.Values.ToArray<string>();
      for (int index = 0; index < array.Length; ++index)
        GUI.TextField(new Rect(0.0f, (float) (20 * index), 1000f, 20f), array[index]);
    }

    protected new void Awake()
    {
      if (!this.CheckInstance())
        return;
      this.camMain = Camera.get_main();
      this.screenShot = (GameScreenShot) ((Component) this).get_gameObject().AddComponent<GameScreenShot>();
      this.screenShotSerial = (GameScreenShotSerial) ((Component) this).get_gameObject().AddComponent<GameScreenShotSerial>();
      this.CreateCamera();
      this.UpdateCameraSetting();
      this.objAudio = new GameObject("objAudio");
      if (!Object.op_Inequality((Object) null, (Object) this.objAudio))
        return;
      this.objAudio.AddComponent<AudioSource>();
    }

    public void UpdateCameraSetting()
    {
      foreach (Camera camera in this.camView)
        camera.set_targetDisplay(!this.useAnotherDisplay ? 0 : 1);
      if (Object.op_Inequality((Object) null, (Object) this.camMain))
        ((Behaviour) this.camMain).set_enabled(this.useAnotherDisplay);
      if (MathfEx.RangeEqualOn<int>(0, this.viewType, 3))
      {
        foreach (Behaviour behaviour in this.camView)
          behaviour.set_enabled(false);
        if (this.viewType == 0)
        {
          ((Behaviour) this.camView[0]).set_enabled(true);
          this.camView[0].set_rect(new Rect(0.0f, 0.0f, 1f, 1f));
        }
        else if (this.viewType == 1)
        {
          ((Behaviour) this.camView[1]).set_enabled(true);
          this.camView[1].set_rect(new Rect(0.0f, 0.0f, 1f, 1f));
        }
        else if (this.viewType == 2)
        {
          ((Behaviour) this.camView[2]).set_enabled(true);
          this.camView[2].set_rect(new Rect(0.0f, 0.0f, 1f, 1f));
        }
        else
        {
          if (this.viewType != 3)
            return;
          ((Behaviour) this.camView[3]).set_enabled(true);
          this.camView[3].set_rect(new Rect(0.0f, 0.0f, 1f, 1f));
        }
      }
      else if (this.viewType == 4)
      {
        foreach (Behaviour behaviour in this.camView)
          behaviour.set_enabled(true);
        this.camView[0].set_rect(new Rect(0.5f, 0.5f, 0.5f, 0.5f));
        this.camView[1].set_rect(new Rect(0.0f, 0.5f, 0.5f, 0.5f));
        this.camView[2].set_rect(new Rect(0.0f, 0.0f, 0.5f, 0.5f));
        this.camView[3].set_rect(new Rect(0.5f, 0.0f, 0.5f, 0.5f));
      }
      else
      {
        foreach (Behaviour behaviour in this.camView)
          behaviour.set_enabled(false);
        if (!Object.op_Inequality((Object) null, (Object) this.camMain))
          return;
        ((Behaviour) this.camMain).set_enabled(true);
      }
    }

    public Vector2 GetCameraPosition(int no)
    {
      Vector2 zero = Vector2.get_zero();
      switch (no)
      {
        case 1:
          zero.x = -((Component) this.camView[no]).get_transform().get_position().x;
          zero.y = -((Component) this.camView[no]).get_transform().get_position().z;
          break;
        case 2:
          zero.x = ((Component) this.camView[no]).get_transform().get_position().x;
          zero.y = -((Component) this.camView[no]).get_transform().get_position().y;
          break;
        case 3:
          zero.x = ((Component) this.camView[no]).get_transform().get_position().z;
          zero.y = -((Component) this.camView[no]).get_transform().get_position().y;
          break;
      }
      return zero;
    }

    public void SetCameraPosition(int no, Vector2 v)
    {
      switch (no)
      {
        case 1:
          ((Component) this.camView[no]).get_transform().set_position(new Vector3((float) -v.x, (float) ((Component) this.camView[no]).get_transform().get_position().y, (float) -v.y));
          break;
        case 2:
          ((Component) this.camView[no]).get_transform().set_position(new Vector3((float) v.x, (float) -v.y, (float) ((Component) this.camView[no]).get_transform().get_position().z));
          break;
        case 3:
          ((Component) this.camView[no]).get_transform().set_position(new Vector3((float) ((Component) this.camView[no]).get_transform().get_position().x, (float) -v.y, (float) v.x));
          break;
      }
    }

    public float GetCameraSize(int no)
    {
      return no == 4 ? 1f : 1f - this.camView[no].get_orthographicSize();
    }

    public void SetCameraSize(int no, float size)
    {
      if (no == 4)
        return;
      this.camView[no].set_orthographicSize(1f - size);
    }

    public bool GetWireframeRender(int no)
    {
      return no != 4 && ((WireFrameRender) ((Component) this.camView[no]).GetComponent<WireFrameRender>()).wireFrameDraw;
    }

    public void SetWireframeRender(int no, bool wire)
    {
      if (no == 4)
        return;
      ((WireFrameRender) ((Component) this.camView[no]).GetComponent<WireFrameRender>()).wireFrameDraw = wire;
    }

    private void CreateCamera()
    {
      string[] strArray = new string[4]
      {
        "Persp",
        "Top",
        "Front",
        "Side"
      };
      int num = LayerMask.NameToLayer("Chara") | LayerMask.NameToLayer("Map");
      if (Object.op_Inequality((Object) null, (Object) this.camMain))
        num = this.camMain.get_cullingMask();
      GameObject gameObject1 = new GameObject(strArray[0]);
      gameObject1.get_transform().SetParent(((Component) this).get_transform());
      this.camView[0] = (Camera) gameObject1.AddComponent<Camera>();
      if (Object.op_Inequality((Object) null, (Object) this.camMain))
        this.camView[0].CopyFrom(this.camMain);
      this.camView[0].set_clearFlags((CameraClearFlags) 2);
      this.camView[0].set_backgroundColor(new Color(0.6f, 0.6f, 0.6f, 1f));
      gameObject1.AddComponent<WireFrameRender>();
      Color[] colorArray = new Color[4]
      {
        new Color(0.6f, 0.6f, 0.6f, 1f),
        new Color(0.65f, 0.65f, 0.65f, 1f),
        new Color(0.7f, 0.7f, 0.7f, 1f),
        new Color(0.75f, 0.75f, 0.75f, 1f)
      };
      Vector3[] vector3Array1 = new Vector3[4]
      {
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(0.0f, 1f, 0.0f),
        new Vector3(0.0f, 1f, 0.0f)
      };
      Vector3[] vector3Array2 = new Vector3[4]
      {
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(90f, 0.0f, 0.0f),
        new Vector3(0.0f, 180f, 0.0f),
        new Vector3(0.0f, 90f, 0.0f)
      };
      for (int index = 1; index < 4; ++index)
      {
        GameObject gameObject2 = new GameObject(string.Format(strArray[index], (object) index));
        gameObject2.get_transform().SetParent(((Component) this).get_transform());
        gameObject2.get_transform().set_localPosition(vector3Array1[index]);
        gameObject2.get_transform().set_localEulerAngles(vector3Array2[index]);
        this.camView[index] = (Camera) gameObject2.AddComponent<Camera>();
        this.camView[index].set_cullingMask(num);
        this.camView[index].set_clearFlags((CameraClearFlags) 2);
        this.camView[index].set_backgroundColor(colorArray[index]);
        this.camView[index].set_orthographic(true);
        this.camView[index].set_orthographicSize(0.5f);
        this.camView[index].set_nearClipPlane(-10f);
        this.camView[index].set_farClipPlane(10f);
        this.camView[index].set_depth(1000f);
        this.camView[index].set_useOcclusionCulling(true);
        this.camView[index].set_allowHDR(false);
        this.camView[index].set_allowMSAA(true);
        gameObject2.AddComponent<WireFrameRender>();
      }
    }

    public class DebugChaValue
    {
      public float[] shapeFace = new float[ChaFileDefine.cf_headshapename.Length];
      public float[] shapeBody = new float[ChaFileDefine.cf_bodyshapename.Length];
      public float[] bustEtc = new float[3];
      public bool[,] disableMaskBody = new bool[2, ChaFileDefine.cf_BustShapeMaskID.Length];
      public bool disableMaskFace;

      public void Initialize(ChaControl _chaCtrl)
      {
        this.UpdateParam(_chaCtrl);
        this.disableMaskFace = _chaCtrl.chaFile.status.disableMouthShapeMask;
        for (int index1 = 0; index1 < 2; ++index1)
        {
          for (int index2 = 0; index2 < ChaFileDefine.cf_BustShapeMaskID.Length; ++index2)
            this.disableMaskBody[index1, index2] = _chaCtrl.chaFile.status.disableBustShapeMask[index1, index2];
        }
      }

      public void UpdateParam(ChaControl _chaCtrl)
      {
        for (int index = 0; index < this.shapeFace.Length; ++index)
          this.shapeFace[index] = _chaCtrl.chaFile.custom.face.shapeValueFace[index];
        for (int index = 0; index < this.shapeBody.Length; ++index)
          this.shapeBody[index] = _chaCtrl.chaFile.custom.body.shapeValueBody[index];
        this.bustEtc[0] = _chaCtrl.chaFile.custom.body.bustSoftness;
        this.bustEtc[1] = _chaCtrl.chaFile.custom.body.bustWeight;
        this.bustEtc[2] = _chaCtrl.chaFile.custom.body.areolaSize;
      }
    }
  }
}
