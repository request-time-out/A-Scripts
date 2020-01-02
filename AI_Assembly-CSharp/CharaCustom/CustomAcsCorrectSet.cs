// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomAcsCorrectSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Illusion.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CustomAcsCorrectSet : MonoBehaviour
  {
    private readonly float[] movePosValue;
    private readonly float[] moveRotValue;
    private readonly float[] moveSclValue;
    [SerializeField]
    private Text title;
    [SerializeField]
    private Toggle[] tglPosRate;
    [SerializeField]
    private Button[] btnPos;
    [SerializeField]
    private InputField[] inpPos;
    [SerializeField]
    private Button[] btnPosReset;
    [SerializeField]
    private Toggle[] tglRotRate;
    [SerializeField]
    private Button[] btnRot;
    [SerializeField]
    private InputField[] inpRot;
    [SerializeField]
    private Button[] btnRotReset;
    [SerializeField]
    private Toggle[] tglSclRate;
    [SerializeField]
    private Button[] btnScl;
    [SerializeField]
    private InputField[] inpScl;
    [SerializeField]
    private Button[] btnSclReset;
    [SerializeField]
    private Button btnAllReset;
    [SerializeField]
    private Toggle tglDrawCtrl;
    [SerializeField]
    private Toggle[] tglCtrlType;
    [SerializeField]
    private Slider sldCtrlSpeed;
    [SerializeField]
    private Slider sldCtrlSize;
    private List<IDisposable> lstDisposable;
    [SerializeField]
    private GameObject tmpObjGuid;
    private CustomGuideObject cmpGuid;
    private bool isDrag;

    public CustomAcsCorrectSet()
    {
      base.\u002Ector();
    }

    private CustomBase customBase
    {
      get
      {
        return Singleton<CustomBase>.Instance;
      }
    }

    private ChaControl chaCtrl
    {
      get
      {
        return this.customBase.chaCtrl;
      }
    }

    private ChaFileAccessory orgAcs
    {
      get
      {
        return this.chaCtrl.chaFile.coordinate.accessory;
      }
    }

    private ChaFileAccessory nowAcs
    {
      get
      {
        return this.chaCtrl.nowCoordinate.accessory;
      }
    }

    private CustomBase.CustomSettingSave.AcsCtrlSetting acsCtrlSetting
    {
      get
      {
        return this.customBase.customSettingSave.acsCtrlSetting;
      }
    }

    public int slotNo { get; set; }

    public int correctNo { get; set; }

    public void UpdateCustomUI()
    {
      if (this.slotNo == -1 || this.correctNo == -1)
        return;
      int posRate = this.acsCtrlSetting.correctSetting[this.correctNo].posRate;
      int rotRate = this.acsCtrlSetting.correctSetting[this.correctNo].rotRate;
      int sclRate = this.acsCtrlSetting.correctSetting[this.correctNo].sclRate;
      this.tglPosRate[posRate].SetIsOnWithoutCallback(true);
      this.tglRotRate[rotRate].SetIsOnWithoutCallback(true);
      this.tglSclRate[sclRate].SetIsOnWithoutCallback(true);
      for (int index = 0; index < 3; ++index)
      {
        if (index != posRate)
          this.tglPosRate[index].SetIsOnWithoutCallback(false);
        if (index != rotRate)
          this.tglRotRate[index].SetIsOnWithoutCallback(false);
        if (index != sclRate)
          this.tglSclRate[index].SetIsOnWithoutCallback(false);
      }
      for (int index = 0; index < 3; ++index)
      {
        this.inpPos[index].set_text(((Vector3) ref this.nowAcs.parts[this.slotNo].addMove.Address(this.correctNo, 0)).get_Item(index).ToString());
        this.inpRot[index].set_text(((Vector3) ref this.nowAcs.parts[this.slotNo].addMove.Address(this.correctNo, 1)).get_Item(index).ToString());
        this.inpScl[index].set_text(((Vector3) ref this.nowAcs.parts[this.slotNo].addMove.Address(this.correctNo, 2)).get_Item(index).ToString());
      }
      this.tglDrawCtrl.SetIsOnWithoutCallback(this.acsCtrlSetting.correctSetting[this.correctNo].draw);
      this.tglCtrlType[this.acsCtrlSetting.correctSetting[this.correctNo].type].SetIsOnWithoutCallback(true);
      this.tglCtrlType[this.acsCtrlSetting.correctSetting[this.correctNo].type & 1].SetIsOnWithoutCallback(false);
      this.sldCtrlSpeed.set_value(this.acsCtrlSetting.correctSetting[this.correctNo].speed);
      this.sldCtrlSize.set_value(this.acsCtrlSetting.correctSetting[this.correctNo].scale);
    }

    public void UpdateDragValue(int type, int xyz, float move)
    {
      int[] numArray = new int[3]{ 1, 2, 4 };
      switch (type)
      {
        case 0:
          this.chaCtrl.SetAccessoryPos(this.slotNo, this.correctNo, move * this.movePosValue[this.acsCtrlSetting.correctSetting[this.correctNo].posRate], true, numArray[xyz]);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 0] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 0];
          this.inpPos[xyz].set_text(((Vector3) ref this.nowAcs.parts[this.slotNo].addMove.Address(this.correctNo, 0)).get_Item(xyz).ToString());
          break;
        case 1:
          this.chaCtrl.SetAccessoryRot(this.slotNo, this.correctNo, move * this.moveRotValue[this.acsCtrlSetting.correctSetting[this.correctNo].rotRate], true, numArray[xyz]);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 1] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 1];
          this.inpRot[xyz].set_text(((Vector3) ref this.nowAcs.parts[this.slotNo].addMove.Address(this.correctNo, 1)).get_Item(xyz).ToString());
          break;
        case 2:
          this.chaCtrl.SetAccessoryScl(this.slotNo, this.correctNo, move * this.moveSclValue[this.acsCtrlSetting.correctSetting[this.correctNo].sclRate], true, numArray[xyz]);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 2] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 2];
          this.inpScl[xyz].set_text(((Vector3) ref this.nowAcs.parts[this.slotNo].addMove.Address(this.correctNo, 2)).get_Item(xyz).ToString());
          break;
      }
      this.SetControllerTransform();
    }

    public void SetControllerTransform()
    {
      Transform transform = this.chaCtrl.trfAcsMove[this.slotNo, this.correctNo];
      if (Object.op_Equality((Object) null, (Object) transform) || Object.op_Equality((Object) null, (Object) this.cmpGuid))
        return;
      this.cmpGuid.amount.position = transform.get_position();
      this.cmpGuid.amount.rotation = transform.get_eulerAngles();
    }

    public void SetAccessoryTransform(bool updateInfo)
    {
      Transform transform = this.chaCtrl.trfAcsMove[this.slotNo, this.correctNo];
      if (Object.op_Equality((Object) null, (Object) transform) || Object.op_Equality((Object) null, (Object) this.cmpGuid))
        return;
      if (this.customBase.customSettingSave.acsCtrlSetting.correctSetting[this.correctNo].type == 0)
      {
        transform.set_position(this.cmpGuid.amount.position);
        if (updateInfo)
        {
          Vector3 localPosition = transform.get_localPosition();
          localPosition.x = (__Null) (double) Mathf.Clamp((float) (localPosition.x * 10.0), -100f, 100f);
          localPosition.y = (__Null) (double) Mathf.Clamp((float) (localPosition.y * 10.0), -100f, 100f);
          localPosition.z = (__Null) (double) Mathf.Clamp((float) (localPosition.z * 10.0), -100f, 100f);
          this.chaCtrl.SetAccessoryPos(this.slotNo, this.correctNo, (float) localPosition.x, false, 1);
          this.chaCtrl.SetAccessoryPos(this.slotNo, this.correctNo, (float) localPosition.y, false, 2);
          this.chaCtrl.SetAccessoryPos(this.slotNo, this.correctNo, (float) localPosition.z, false, 4);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 0] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 0];
          this.chaCtrl.UpdateAccessoryMoveFromInfo(this.slotNo);
          this.cmpGuid.amount.position = transform.get_position();
        }
      }
      else
      {
        transform.set_eulerAngles(this.cmpGuid.amount.rotation);
        if (updateInfo)
        {
          Vector3 localEulerAngles = transform.get_localEulerAngles();
          this.chaCtrl.SetAccessoryRot(this.slotNo, this.correctNo, (float) localEulerAngles.x, false, 1);
          this.chaCtrl.SetAccessoryRot(this.slotNo, this.correctNo, (float) localEulerAngles.y, false, 2);
          this.chaCtrl.SetAccessoryRot(this.slotNo, this.correctNo, (float) localEulerAngles.z, false, 4);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 1] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 1];
          this.chaCtrl.UpdateAccessoryMoveFromInfo(this.slotNo);
          this.cmpGuid.amount.rotation = transform.get_eulerAngles();
        }
      }
      this.UpdateCustomUI();
    }

    public void UpdateDrawControllerState()
    {
      int type = this.customBase.customSettingSave.acsCtrlSetting.correctSetting[this.correctNo].type;
      bool draw = this.customBase.customSettingSave.acsCtrlSetting.correctSetting[this.correctNo].draw;
      float speed = this.customBase.customSettingSave.acsCtrlSetting.correctSetting[this.correctNo].speed;
      float scale = this.customBase.customSettingSave.acsCtrlSetting.correctSetting[this.correctNo].scale;
      this.tglDrawCtrl.SetIsOnWithoutCallback(draw);
      this.tglCtrlType[type].SetIsOnWithoutCallback(true);
      this.sldCtrlSpeed.set_value(speed);
      this.sldCtrlSize.set_value(scale);
    }

    public bool IsDrag()
    {
      return Object.op_Inequality((Object) null, (Object) this.cmpGuid) && this.cmpGuid.isDrag;
    }

    public void ShortcutChangeGuidType(int type)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.cmpGuid) || this.cmpGuid.isDrag)
        return;
      this.tglCtrlType[type].set_isOn(true);
    }

    public void CreateGuid(GameObject objParent)
    {
      Transform transform = !Object.op_Equality((Object) null, (Object) objParent) ? objParent.get_transform() : (Transform) null;
      GameObject self = (GameObject) Object.Instantiate<GameObject>((M0) this.tmpObjGuid);
      self.get_transform().SetParent(transform);
      this.cmpGuid = (CustomGuideObject) self.GetComponent<CustomGuideObject>();
      self.SetActiveIfDifferent(true);
    }

    public void Initialize(int _slotNo, int _correctNo)
    {
      this.slotNo = _slotNo;
      this.correctNo = _correctNo;
      if (this.slotNo == -1 || this.correctNo == -1)
        return;
      if (Object.op_Implicit((Object) this.title))
        this.title.set_text(string.Format("{0}{1:00}", (object) "調整", (object) (this.correctNo + 1)));
      this.UpdateCustomUI();
      if (this.lstDisposable != null && this.lstDisposable.Count != 0)
      {
        int count = this.lstDisposable.Count;
        for (int index = 0; index < count; ++index)
          this.lstDisposable[index].Dispose();
      }
      IDisposable disposable = (IDisposable) null;
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<Toggle>) this.tglPosRate).Select<Toggle, \u003C\u003E__AnonType12<Toggle, byte>>((Func<Toggle, int, \u003C\u003E__AnonType12<Toggle, byte>>) ((p, i) => new \u003C\u003E__AnonType12<Toggle, byte>(p, (byte) i))).ToList<\u003C\u003E__AnonType12<Toggle, byte>>().ForEach((Action<\u003C\u003E__AnonType12<Toggle, byte>>) (p =>
      {
        disposable = ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(p.toggle), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (_ => this.acsCtrlSetting.correctSetting[this.correctNo].posRate = (int) p.index));
        this.lstDisposable.Add(disposable);
      }));
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<Toggle>) this.tglRotRate).Select<Toggle, \u003C\u003E__AnonType12<Toggle, byte>>((Func<Toggle, int, \u003C\u003E__AnonType12<Toggle, byte>>) ((p, i) => new \u003C\u003E__AnonType12<Toggle, byte>(p, (byte) i))).ToList<\u003C\u003E__AnonType12<Toggle, byte>>().ForEach((Action<\u003C\u003E__AnonType12<Toggle, byte>>) (p =>
      {
        disposable = ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(p.toggle), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (_ => this.acsCtrlSetting.correctSetting[this.correctNo].rotRate = (int) p.index));
        this.lstDisposable.Add(disposable);
      }));
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<Toggle>) this.tglSclRate).Select<Toggle, \u003C\u003E__AnonType12<Toggle, byte>>((Func<Toggle, int, \u003C\u003E__AnonType12<Toggle, byte>>) ((p, i) => new \u003C\u003E__AnonType12<Toggle, byte>(p, (byte) i))).ToList<\u003C\u003E__AnonType12<Toggle, byte>>().ForEach((Action<\u003C\u003E__AnonType12<Toggle, byte>>) (p =>
      {
        disposable = ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(p.toggle), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (_ => this.acsCtrlSetting.correctSetting[this.correctNo].sclRate = (int) p.index));
        this.lstDisposable.Add(disposable);
      }));
      float downTimeCnt = 0.0f;
      float loopTimeCnt = 0.0f;
      bool change = false;
      int[] flag = new int[3]{ 1, 2, 4 };
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<Button>) this.btnPos).Select<Button, \u003C\u003E__AnonType13<Button, int>>((Func<Button, int, \u003C\u003E__AnonType13<Button, int>>) ((p, i) => new \u003C\u003E__AnonType13<Button, int>(p, i))).ToList<\u003C\u003E__AnonType13<Button, int>>().ForEach((Action<\u003C\u003E__AnonType13<Button, int>>) (p =>
      {
        disposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(p.btn), (Action<M0>) (_ =>
        {
          if (change)
            return;
          int index = p.index / 2;
          int num = p.index % 2 != 0 ? 1 : -1;
          if (index == 0)
            num *= -1;
          this.chaCtrl.SetAccessoryPos(this.slotNo, this.correctNo, (float) num * this.movePosValue[this.acsCtrlSetting.correctSetting[this.correctNo].posRate], true, flag[index]);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 0] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 0];
          this.inpPos[index].set_text(((Vector3) ref this.nowAcs.parts[this.slotNo].addMove.Address(this.correctNo, 0)).get_Item(index).ToString());
          this.SetControllerTransform();
        }));
        this.lstDisposable.Add(disposable);
        disposable = (IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.RepeatUntilDestroy<Unit>(Observable.TakeUntil<Unit, PointerEventData>(Observable.SkipUntil<Unit, PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) p.btn), (IObservable<M1>) Observable.Do<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerDownAsObservable((UIBehaviour) p.btn), (Action<M0>) (_ =>
        {
          downTimeCnt = 0.0f;
          loopTimeCnt = 0.0f;
          change = false;
        }))), (IObservable<M1>) ObservableTriggerExtensions.OnPointerUpAsObservable((UIBehaviour) p.btn)), (Component) this), (Action<M0>) (_ =>
        {
          int index = p.index / 2;
          int num1 = p.index % 2 != 0 ? 1 : -1;
          if (index == 0)
            num1 *= -1;
          float num2 = (float) num1 * this.movePosValue[this.acsCtrlSetting.correctSetting[this.correctNo].posRate];
          float num3 = 0.0f;
          downTimeCnt += Time.get_deltaTime();
          if ((double) downTimeCnt <= 0.300000011920929)
            return;
          for (loopTimeCnt += Time.get_deltaTime(); (double) loopTimeCnt > 0.0500000007450581; loopTimeCnt -= 0.05f)
            num3 += num2;
          if ((double) num3 == 0.0)
            return;
          this.chaCtrl.SetAccessoryPos(this.slotNo, this.correctNo, num3, true, flag[index]);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 0] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 0];
          this.inpPos[index].set_text(((Vector3) ref this.nowAcs.parts[this.slotNo].addMove.Address(this.correctNo, 0)).get_Item(index).ToString());
          change = true;
          this.SetControllerTransform();
        })), (Component) this);
        this.lstDisposable.Add(disposable);
      }));
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<InputField>) this.inpPos).Select<InputField, \u003C\u003E__AnonType14<InputField, int>>((Func<InputField, int, \u003C\u003E__AnonType14<InputField, int>>) ((p, i) => new \u003C\u003E__AnonType14<InputField, int>(p, i))).ToList<\u003C\u003E__AnonType14<InputField, int>>().ForEach((Action<\u003C\u003E__AnonType14<InputField, int>>) (p =>
      {
        disposable = ObservableExtensions.Subscribe<string>(UnityEventExtensions.AsObservable<string>((UnityEvent<M0>) p.inp.get_onEndEdit()), (Action<M0>) (value =>
        {
          int index = p.index % 3;
          float num = CustomBase.ConvertValueFromTextLimit(-100f, 100f, 1, value);
          p.inp.set_text(num.ToString());
          this.chaCtrl.SetAccessoryPos(this.slotNo, this.correctNo, num, false, flag[index]);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 0] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 0];
          this.SetControllerTransform();
        }));
        this.lstDisposable.Add(disposable);
      }));
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<Button>) this.btnPosReset).Select<Button, \u003C\u003E__AnonType13<Button, int>>((Func<Button, int, \u003C\u003E__AnonType13<Button, int>>) ((p, i) => new \u003C\u003E__AnonType13<Button, int>(p, i))).ToList<\u003C\u003E__AnonType13<Button, int>>().ForEach((Action<\u003C\u003E__AnonType13<Button, int>>) (p =>
      {
        disposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(p.btn), (Action<M0>) (_ =>
        {
          this.inpPos[p.index].set_text("0");
          this.chaCtrl.SetAccessoryPos(this.slotNo, this.correctNo, 0.0f, false, flag[p.index]);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 0] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 0];
          this.SetControllerTransform();
        }));
        this.lstDisposable.Add(disposable);
      }));
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<Button>) this.btnRot).Select<Button, \u003C\u003E__AnonType13<Button, int>>((Func<Button, int, \u003C\u003E__AnonType13<Button, int>>) ((p, i) => new \u003C\u003E__AnonType13<Button, int>(p, i))).ToList<\u003C\u003E__AnonType13<Button, int>>().ForEach((Action<\u003C\u003E__AnonType13<Button, int>>) (p =>
      {
        disposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(p.btn), (Action<M0>) (_ =>
        {
          if (change)
            return;
          int index = p.index / 2;
          this.chaCtrl.SetAccessoryRot(this.slotNo, this.correctNo, (p.index % 2 != 0 ? 1f : -1f) * this.moveRotValue[this.acsCtrlSetting.correctSetting[this.correctNo].rotRate], true, flag[index]);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 1] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 1];
          this.inpRot[index].set_text(((Vector3) ref this.nowAcs.parts[this.slotNo].addMove.Address(this.correctNo, 1)).get_Item(index).ToString());
          this.SetControllerTransform();
        }));
        this.lstDisposable.Add(disposable);
        disposable = (IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.RepeatUntilDestroy<Unit>(Observable.TakeUntil<Unit, PointerEventData>(Observable.SkipUntil<Unit, PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) p.btn), (IObservable<M1>) Observable.Do<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerDownAsObservable((UIBehaviour) p.btn), (Action<M0>) (_ =>
        {
          downTimeCnt = 0.0f;
          loopTimeCnt = 0.0f;
          change = false;
        }))), (IObservable<M1>) ObservableTriggerExtensions.OnPointerUpAsObservable((UIBehaviour) p.btn)), (Component) this), (Action<M0>) (_ =>
        {
          int index = p.index / 2;
          float num1 = (p.index % 2 != 0 ? 1f : -1f) * this.moveRotValue[this.acsCtrlSetting.correctSetting[this.correctNo].rotRate];
          float num2 = 0.0f;
          downTimeCnt += Time.get_deltaTime();
          if ((double) downTimeCnt <= 0.300000011920929)
            return;
          for (loopTimeCnt += Time.get_deltaTime(); (double) loopTimeCnt > 0.0500000007450581; loopTimeCnt -= 0.05f)
            num2 += num1;
          if ((double) num2 == 0.0)
            return;
          this.chaCtrl.SetAccessoryRot(this.slotNo, this.correctNo, num2, true, flag[index]);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 1] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 1];
          this.inpRot[index].set_text(((Vector3) ref this.nowAcs.parts[this.slotNo].addMove.Address(this.correctNo, 1)).get_Item(index).ToString());
          change = true;
          this.SetControllerTransform();
        })), (Component) this);
        this.lstDisposable.Add(disposable);
      }));
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<InputField>) this.inpRot).Select<InputField, \u003C\u003E__AnonType14<InputField, int>>((Func<InputField, int, \u003C\u003E__AnonType14<InputField, int>>) ((p, i) => new \u003C\u003E__AnonType14<InputField, int>(p, i))).ToList<\u003C\u003E__AnonType14<InputField, int>>().ForEach((Action<\u003C\u003E__AnonType14<InputField, int>>) (p =>
      {
        disposable = ObservableExtensions.Subscribe<string>(UnityEventExtensions.AsObservable<string>((UnityEvent<M0>) p.inp.get_onEndEdit()), (Action<M0>) (value =>
        {
          int index = p.index % 3;
          float num = CustomBase.ConvertValueFromTextLimit(0.0f, 360f, 0, value);
          p.inp.set_text(num.ToString());
          this.chaCtrl.SetAccessoryRot(this.slotNo, this.correctNo, num, false, flag[index]);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 1] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 1];
          this.SetControllerTransform();
        }));
        this.lstDisposable.Add(disposable);
      }));
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<Button>) this.btnRotReset).Select<Button, \u003C\u003E__AnonType13<Button, int>>((Func<Button, int, \u003C\u003E__AnonType13<Button, int>>) ((p, i) => new \u003C\u003E__AnonType13<Button, int>(p, i))).ToList<\u003C\u003E__AnonType13<Button, int>>().ForEach((Action<\u003C\u003E__AnonType13<Button, int>>) (p =>
      {
        disposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(p.btn), (Action<M0>) (_ =>
        {
          this.inpRot[p.index].set_text("0");
          this.chaCtrl.SetAccessoryRot(this.slotNo, this.correctNo, 0.0f, false, flag[p.index]);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 1] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 1];
          this.SetControllerTransform();
        }));
        this.lstDisposable.Add(disposable);
      }));
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<Button>) this.btnScl).Select<Button, \u003C\u003E__AnonType13<Button, int>>((Func<Button, int, \u003C\u003E__AnonType13<Button, int>>) ((p, i) => new \u003C\u003E__AnonType13<Button, int>(p, i))).ToList<\u003C\u003E__AnonType13<Button, int>>().ForEach((Action<\u003C\u003E__AnonType13<Button, int>>) (p =>
      {
        disposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(p.btn), (Action<M0>) (_ =>
        {
          if (change)
            return;
          int index = p.index / 2;
          this.chaCtrl.SetAccessoryScl(this.slotNo, this.correctNo, (p.index % 2 != 0 ? 1f : -1f) * this.moveSclValue[this.acsCtrlSetting.correctSetting[this.correctNo].sclRate], true, flag[index]);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 2] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 2];
          this.inpScl[index].set_text(((Vector3) ref this.nowAcs.parts[this.slotNo].addMove.Address(this.correctNo, 2)).get_Item(index).ToString());
        }));
        this.lstDisposable.Add(disposable);
        disposable = (IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>(Observable.RepeatUntilDestroy<Unit>(Observable.TakeUntil<Unit, PointerEventData>(Observable.SkipUntil<Unit, PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) p.btn), (IObservable<M1>) Observable.Do<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerDownAsObservable((UIBehaviour) p.btn), (Action<M0>) (_ =>
        {
          downTimeCnt = 0.0f;
          loopTimeCnt = 0.0f;
          change = false;
        }))), (IObservable<M1>) ObservableTriggerExtensions.OnPointerUpAsObservable((UIBehaviour) p.btn)), (Component) this), (Action<M0>) (_ =>
        {
          int index = p.index / 2;
          float num1 = (p.index % 2 != 0 ? 1f : -1f) * this.moveSclValue[this.acsCtrlSetting.correctSetting[this.correctNo].sclRate];
          float num2 = 0.0f;
          downTimeCnt += Time.get_deltaTime();
          if ((double) downTimeCnt <= 0.300000011920929)
            return;
          for (loopTimeCnt += Time.get_deltaTime(); (double) loopTimeCnt > 0.0500000007450581; loopTimeCnt -= 0.05f)
            num2 += num1;
          if ((double) num2 == 0.0)
            return;
          this.chaCtrl.SetAccessoryScl(this.slotNo, this.correctNo, num2, true, flag[index]);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 2] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 2];
          this.inpScl[index].set_text(((Vector3) ref this.nowAcs.parts[this.slotNo].addMove.Address(this.correctNo, 2)).get_Item(index).ToString());
          change = true;
        })), (Component) this);
        this.lstDisposable.Add(disposable);
      }));
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<InputField>) this.inpScl).Select<InputField, \u003C\u003E__AnonType14<InputField, int>>((Func<InputField, int, \u003C\u003E__AnonType14<InputField, int>>) ((p, i) => new \u003C\u003E__AnonType14<InputField, int>(p, i))).ToList<\u003C\u003E__AnonType14<InputField, int>>().ForEach((Action<\u003C\u003E__AnonType14<InputField, int>>) (p =>
      {
        disposable = ObservableExtensions.Subscribe<string>(UnityEventExtensions.AsObservable<string>((UnityEvent<M0>) p.inp.get_onEndEdit()), (Action<M0>) (value =>
        {
          int index = p.index % 3;
          float num = CustomBase.ConvertValueFromTextLimit(0.01f, 100f, 2, value);
          p.inp.set_text(num.ToString());
          this.chaCtrl.SetAccessoryScl(this.slotNo, this.correctNo, num, false, flag[index]);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 2] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 2];
        }));
        this.lstDisposable.Add(disposable);
      }));
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<Button>) this.btnSclReset).Select<Button, \u003C\u003E__AnonType13<Button, int>>((Func<Button, int, \u003C\u003E__AnonType13<Button, int>>) ((p, i) => new \u003C\u003E__AnonType13<Button, int>(p, i))).ToList<\u003C\u003E__AnonType13<Button, int>>().ForEach((Action<\u003C\u003E__AnonType13<Button, int>>) (p =>
      {
        disposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(p.btn), (Action<M0>) (_ =>
        {
          this.inpScl[p.index].set_text("1");
          this.chaCtrl.SetAccessoryScl(this.slotNo, this.correctNo, 1f, false, flag[p.index]);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 2] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 2];
        }));
        this.lstDisposable.Add(disposable);
      }));
      disposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnAllReset), (Action<M0>) (_ =>
      {
        for (int index = 0; index < 3; ++index)
        {
          this.inpPos[index].set_text("0");
          this.chaCtrl.SetAccessoryPos(this.slotNo, this.correctNo, 0.0f, false, flag[index]);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 0] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 0];
          this.SetControllerTransform();
          this.inpRot[index].set_text("0");
          this.chaCtrl.SetAccessoryRot(this.slotNo, this.correctNo, 0.0f, false, flag[index]);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 1] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 1];
          this.SetControllerTransform();
          this.inpScl[index].set_text("1");
          this.chaCtrl.SetAccessoryScl(this.slotNo, this.correctNo, 1f, false, flag[index]);
          this.orgAcs.parts[this.slotNo].addMove[this.correctNo, 2] = this.nowAcs.parts[this.slotNo].addMove[this.correctNo, 2];
        }
      }));
      this.lstDisposable.Add(disposable);
      disposable = ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.tglDrawCtrl), (Action<M0>) (isOn => this.customBase.customSettingSave.acsCtrlSetting.correctSetting[this.correctNo].draw = isOn));
      this.lstDisposable.Add(disposable);
      if (((IEnumerable<Toggle>) this.tglCtrlType).Any<Toggle>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Toggle>) this.tglCtrlType).Select<Toggle, \u003C\u003E__AnonType15<Toggle, int>>((Func<Toggle, int, \u003C\u003E__AnonType15<Toggle, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Toggle, int>(val, idx))).Where<\u003C\u003E__AnonType15<Toggle, int>>((Func<\u003C\u003E__AnonType15<Toggle, int>, bool>) (item => Object.op_Inequality((Object) item.val, (Object) null))).ToList<\u003C\u003E__AnonType15<Toggle, int>>().ForEach((Action<\u003C\u003E__AnonType15<Toggle, int>>) (item =>
        {
          disposable = ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(item.val), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (isOn =>
          {
            this.customBase.customSettingSave.acsCtrlSetting.correctSetting[this.correctNo].type = item.idx;
            if (!Object.op_Implicit((Object) this.cmpGuid))
              return;
            this.cmpGuid.SetMode(item.idx);
          }));
          this.lstDisposable.Add(disposable);
        }));
      }
      disposable = ObservableExtensions.Subscribe<float>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.sldCtrlSpeed), (Action<M0>) (val =>
      {
        this.customBase.customSettingSave.acsCtrlSetting.correctSetting[this.correctNo].speed = val;
        if (!Object.op_Implicit((Object) this.cmpGuid))
          return;
        this.cmpGuid.speedMove = val;
      }));
      this.lstDisposable.Add(disposable);
      disposable = ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnScrollAsObservable((UIBehaviour) this.sldCtrlSpeed), (Action<M0>) (scl =>
      {
        if (!this.customBase.sliderControlWheel)
          return;
        this.sldCtrlSpeed.set_value(Mathf.Clamp(this.sldCtrlSpeed.get_value() + (float) (scl.get_scrollDelta().y * -0.00999999977648258), 0.1f, 1f));
      }));
      this.lstDisposable.Add(disposable);
      disposable = ObservableExtensions.Subscribe<float>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.sldCtrlSize), (Action<M0>) (val =>
      {
        this.customBase.customSettingSave.acsCtrlSetting.correctSetting[this.correctNo].scale = val;
        if (!Object.op_Implicit((Object) this.cmpGuid))
          return;
        this.cmpGuid.scaleAxis = val;
        this.cmpGuid.UpdateScale();
      }));
      this.lstDisposable.Add(disposable);
      disposable = ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnScrollAsObservable((UIBehaviour) this.sldCtrlSize), (Action<M0>) (scl =>
      {
        if (!this.customBase.sliderControlWheel)
          return;
        this.sldCtrlSize.set_value(Mathf.Clamp(this.sldCtrlSize.get_value() + (float) (scl.get_scrollDelta().y * -0.00999999977648258), 0.3f, 3f));
      }));
      this.lstDisposable.Add(disposable);
      this.UpdateDrawControllerState();
    }

    private void Start()
    {
      for (int index = 0; index < 3; ++index)
      {
        this.customBase.lstInputField.Add(this.inpPos[index]);
        this.customBase.lstInputField.Add(this.inpRot[index]);
        this.customBase.lstInputField.Add(this.inpScl[index]);
      }
    }

    private void LateUpdate()
    {
      if (!Object.op_Inequality((Object) null, (Object) this.cmpGuid) || !((Component) this.cmpGuid).get_gameObject().get_activeInHierarchy())
        return;
      if (this.cmpGuid.isDrag)
        this.SetAccessoryTransform(false);
      else if (this.isDrag)
        this.SetAccessoryTransform(true);
      else
        this.SetControllerTransform();
      this.isDrag = this.cmpGuid.isDrag;
      this.customBase.cursorDraw = !this.cmpGuid.isDrag;
    }
  }
}
