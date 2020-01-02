// Decompiled with JetBrains decompiler
// Type: Studio.MapCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class MapCtrl : Singleton<MapCtrl>
  {
    private Vector3 oldValue = Vector3.get_zero();
    [SerializeField]
    private TMP_InputField[] inputPos;
    [SerializeField]
    private TMP_InputField[] inputRot;
    [SerializeField]
    private MapDragButton[] mapDragButton;
    [SerializeField]
    private Toggle toggleOption;
    private Transform transMap;
    private bool isUpdate;

    public bool active
    {
      set
      {
        ((Component) this).get_gameObject().SetActive(value);
        if (!value)
          return;
        this.UpdateUI();
      }
    }

    public void UpdateUI()
    {
      this.isUpdate = true;
      this.SetInputTextPos();
      this.SetInputTextRot();
      if (Singleton<Map>.Instance.IsOption)
      {
        ((Selectable) this.toggleOption).set_interactable(true);
        this.toggleOption.set_isOn(Singleton<Studio.Studio>.Instance.sceneInfo.mapOption);
      }
      else
      {
        ((Selectable) this.toggleOption).set_interactable(false);
        this.toggleOption.set_isOn(false);
      }
      this.isUpdate = false;
    }

    public void Reflect()
    {
      GameObject mapRoot = Singleton<Map>.Instance.MapRoot;
      if (Object.op_Inequality((Object) mapRoot, (Object) null))
      {
        Transform transform = mapRoot.get_transform();
        transform.set_localPosition(Singleton<Studio.Studio>.Instance.sceneInfo.caMap.pos);
        transform.set_localRotation(Quaternion.Euler(Singleton<Studio.Studio>.Instance.sceneInfo.caMap.rot));
      }
      Singleton<Map>.Instance.VisibleOption = Singleton<Studio.Studio>.Instance.sceneInfo.mapOption;
      this.UpdateUI();
    }

    public void OnEndEditPos(int _target)
    {
      float num = this.InputToFloat(this.inputPos[_target]);
      Vector3 pos = Singleton<Studio.Studio>.Instance.sceneInfo.caMap.pos;
      if ((double) ((Vector3) ref pos).get_Item(_target) != (double) num)
      {
        Vector3 vector3 = pos;
        ((Vector3) ref pos).set_Item(_target, num);
        Singleton<Studio.Studio>.Instance.sceneInfo.caMap.pos = pos;
        Singleton<UndoRedoManager>.Instance.Push((ICommand) new MapCommand.MoveEqualsCommand(new MapCommand.EqualsInfo()
        {
          newValue = pos,
          oldValue = vector3
        }));
      }
      this.SetInputTextPos();
    }

    public void OnEndEditRot(int _target)
    {
      float num = this.InputToFloat(this.inputRot[_target]) % 360f;
      Vector3 rot = Singleton<Studio.Studio>.Instance.sceneInfo.caMap.rot;
      if ((double) ((Vector3) ref rot).get_Item(_target) != (double) num)
      {
        Vector3 vector3 = rot;
        ((Vector3) ref rot).set_Item(_target, num);
        Singleton<Studio.Studio>.Instance.sceneInfo.caMap.rot = rot;
        Singleton<UndoRedoManager>.Instance.Push((ICommand) new MapCommand.RotationEqualsCommand(new MapCommand.EqualsInfo()
        {
          newValue = rot,
          oldValue = vector3
        }));
      }
      this.SetInputTextRot();
    }

    private float InputToFloat(TMP_InputField _input)
    {
      float result = 0.0f;
      return float.TryParse(_input.get_text(), out result) ? result : 0.0f;
    }

    private void SetInputTextPos()
    {
      Vector3 pos = Singleton<Studio.Studio>.Instance.sceneInfo.caMap.pos;
      for (int index = 0; index < 3; ++index)
        this.inputPos[index].set_text(((Vector3) ref pos).get_Item(index).ToString("0.000"));
    }

    private void SetInputTextRot()
    {
      Vector3 rot = Singleton<Studio.Studio>.Instance.sceneInfo.caMap.rot;
      for (int index = 0; index < 3; ++index)
        this.inputRot[index].set_text(((Vector3) ref rot).get_Item(index).ToString("0.000"));
    }

    private void OnBeginDragTrans()
    {
      this.oldValue = Singleton<Studio.Studio>.Instance.sceneInfo.caMap.pos;
      this.transMap = Singleton<Map>.Instance.MapRoot.get_transform();
    }

    private void OnEndDragTrans()
    {
      Singleton<UndoRedoManager>.Instance.Push((ICommand) new MapCommand.MoveEqualsCommand(new MapCommand.EqualsInfo()
      {
        newValue = Singleton<Studio.Studio>.Instance.sceneInfo.caMap.pos,
        oldValue = this.oldValue
      }));
      this.transMap = (Transform) null;
    }

    private void OnDragTransXZ()
    {
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector(Input.GetAxis("Mouse X"), 0.0f, Input.GetAxis("Mouse Y"));
      ChangeAmount caMap = Singleton<Studio.Studio>.Instance.sceneInfo.caMap;
      caMap.pos = Vector3.op_Addition(caMap.pos, this.transMap.TransformDirection(vector3));
    }

    private void OnDragTransY()
    {
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector(0.0f, Input.GetAxis("Mouse Y"), 0.0f);
      ChangeAmount caMap = Singleton<Studio.Studio>.Instance.sceneInfo.caMap;
      caMap.pos = Vector3.op_Addition(caMap.pos, this.transMap.TransformDirection(vector3));
    }

    private void OnBeginDragRot()
    {
      this.oldValue = Singleton<Studio.Studio>.Instance.sceneInfo.caMap.rot;
    }

    private void OnEndDragRot()
    {
      Singleton<UndoRedoManager>.Instance.Push((ICommand) new MapCommand.RotationEqualsCommand(new MapCommand.EqualsInfo()
      {
        newValue = Singleton<Studio.Studio>.Instance.sceneInfo.caMap.rot,
        oldValue = this.oldValue
      }));
    }

    private void OnDragRotX()
    {
      Vector3 rot = Singleton<Studio.Studio>.Instance.sceneInfo.caMap.rot;
      rot.x = (__Null) ((rot.x + (double) Input.GetAxis("Mouse Y")) % 360.0);
      Singleton<Studio.Studio>.Instance.sceneInfo.caMap.rot = rot;
    }

    private void OnDragRotY()
    {
      Vector3 rot = Singleton<Studio.Studio>.Instance.sceneInfo.caMap.rot;
      rot.y = (__Null) ((rot.y + (double) Input.GetAxis("Mouse X")) % 360.0);
      Singleton<Studio.Studio>.Instance.sceneInfo.caMap.rot = rot;
    }

    private void OnDragRotZ()
    {
      Vector3 rot = Singleton<Studio.Studio>.Instance.sceneInfo.caMap.rot;
      rot.z = (__Null) ((rot.z + (double) Input.GetAxis("Mouse X")) % 360.0);
      Singleton<Studio.Studio>.Instance.sceneInfo.caMap.rot = rot;
    }

    private void OnValueChangedOption(bool _value)
    {
      if (this.isUpdate)
        return;
      Singleton<Map>.Instance.VisibleOption = _value;
    }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      this.mapDragButton[0].onBeginDragFunc += new Action(this.OnBeginDragTrans);
      this.mapDragButton[0].onDragFunc += new Action(this.OnDragTransXZ);
      this.mapDragButton[0].onEndDragFunc += new Action(this.OnEndDragTrans);
      this.mapDragButton[1].onBeginDragFunc += new Action(this.OnBeginDragTrans);
      this.mapDragButton[1].onDragFunc += new Action(this.OnDragTransY);
      this.mapDragButton[1].onEndDragFunc += new Action(this.OnEndDragTrans);
      for (int index = 0; index < 3; ++index)
      {
        this.mapDragButton[2 + index].onBeginDragFunc += new Action(this.OnBeginDragRot);
        this.mapDragButton[2 + index].onEndDragFunc += new Action(this.OnEndDragRot);
      }
      this.mapDragButton[2].onDragFunc += new Action(this.OnDragRotX);
      this.mapDragButton[3].onDragFunc += new Action(this.OnDragRotY);
      this.mapDragButton[4].onDragFunc += new Action(this.OnDragRotZ);
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.toggleOption.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedOption)));
    }
  }
}
