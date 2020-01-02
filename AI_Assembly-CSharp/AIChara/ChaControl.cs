// Decompiled with JetBrains decompiler
// Type: AIChara.ChaControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using FBSAssist;
using Illusion.Extensions;
using IllusionUtility.SetUtility;
using Manager;
using OutputLogControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace AIChara
{
  public class ChaControl : ChaInfo
  {
    private ChaControl.MannequinBackInfo mannequinBackInfo = new ChaControl.MannequinBackInfo();
    private bool confSon = true;
    private bool confBody = true;
    private List<bool> lstActive = new List<bool>();
    private bool drawSimple;
    private AssignedAnotherWeights aaWeightsHead;
    private AssignedAnotherWeights aaWeightsBody;

    public void Initialize(
      byte _sex,
      GameObject _objRoot,
      int _id,
      int _no,
      ChaFileControl _chaFile = null)
    {
      if (_chaFile != null && (int) _sex != (int) _chaFile.parameter.sex)
        Debug.LogWarning((object) "作成する性別とファイルの性別が食い違っている");
      this.MemberInitializeAll();
      this.InitializeControlLoadAll();
      this.InitializeControlFaceAll();
      this.InitializeControlBodyAll();
      this.InitializeControlCoordinateAll();
      this.InitializeControlAccessoryAll();
      this.InitializeControlCustomBodyAll();
      this.InitializeControlCustomFaceAll();
      this.InitializeControlCustomHairAll();
      this.InitializeControlHandAll();
      this.objRoot = _objRoot;
      this.chaID = _id;
      this.loadNo = _no;
      this.hideMoz = false;
      this.lstCtrl = Singleton<Character>.Instance.chaListCtrl;
      if (_chaFile == null)
      {
        this.chaFile = new ChaFileControl();
        this.LoadPreset((int) _sex, string.Empty);
      }
      else
        this.chaFile = _chaFile;
      this.chaFile.parameter.sex = _sex;
      this.InitBaseCustomTextureBody();
      this.ChangeNowCoordinate(false, true);
      if (_sex == (byte) 0)
        this.chaFile.custom.body.shapeValueBody[0] = 0.75f;
      if (_sex == (byte) 1)
        this.chaFile.status.visibleSonAlways = false;
      else
        this.chaFile.status.visibleSon = false;
    }

    public void ReleaseAll()
    {
      this.ReleaseControlLoadAll();
      this.ReleaseControlFaceAll();
      this.ReleaseControlBodyAll();
      this.ReleaseControlCoordinateAll();
      this.ReleaseControlAccessoryAll();
      this.ReleaseControlCustomBodyAll();
      this.ReleaseControlCustomFaceAll();
      this.ReleaseControlCustomHairAll();
      this.ReleaseControlHandAll();
      this.ReleaseInfoAll();
    }

    public void ReleaseObject()
    {
      this.ReleaseControlLoadObject(true);
      this.ReleaseControlFaceObject(true);
      this.ReleaseControlBodyObject(true);
      this.ReleaseControlCoordinateObject(true);
      this.ReleaseControlAccessoryObject(true);
      this.ReleaseControlCustomBodyObject(true);
      this.ReleaseControlCustomFaceObject(true);
      this.ReleaseControlCustomHairObject(true);
      this.ReleaseControlHandObject(true);
      this.ReleaseInfoObject(true);
      if (!Singleton<Character>.Instance.enableCharaLoadGCClear)
        return;
      Resources.UnloadUnusedAssets();
      OutputLog.Log(nameof (ReleaseObject), false, "UnloadUnusedAssets");
      GC.Collect();
    }

    public void LoadPreset(int _sex, string presetName = "")
    {
      string empty = string.Empty;
      string assetName = !presetName.IsNullOrEmpty() ? presetName : (_sex != 0 ? "ill_Default_Female" : "ill_Default_Male");
      foreach (KeyValuePair<int, ListInfoBase> keyValuePair in _sex != 0 ? this.lstCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.cha_sample_f) : this.lstCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.cha_sample_m))
      {
        if (keyValuePair.Value.GetInfo(ChaListDefine.KeyType.MainData) == assetName)
        {
          this.chaFile.LoadFromAssetBundle(keyValuePair.Value.GetInfo(ChaListDefine.KeyType.MainAB), assetName, false, true);
          break;
        }
      }
    }

    public static ChaFileControl[] GetRandomFemaleCard(int num)
    {
      FolderAssist folderAssist = new FolderAssist();
      string[] searchPattern = new string[1]{ "*.png" };
      string folder = UserData.Path + "chara/female/";
      folderAssist.CreateFolderInfoEx(folder, searchPattern, true);
      List<string> list = folderAssist.lstFile.Shuffle<FolderAssist.FileInfo>().Select<FolderAssist.FileInfo, string>((Func<FolderAssist.FileInfo, string>) (n => n.FullPath)).ToList<string>();
      int num1 = Mathf.Min(list.Count, num);
      if (num1 == 0)
        return (ChaFileControl[]) null;
      List<ChaFileControl> chaFileControlList = new List<ChaFileControl>();
      for (int index = 0; index < num1; ++index)
      {
        ChaFileControl chaFileControl = new ChaFileControl();
        if (chaFileControl.LoadCharaFile(list[index], (byte) 1, true, true) && chaFileControl.parameter.sex != (byte) 0)
          chaFileControlList.Add(chaFileControl);
      }
      return chaFileControlList.ToArray();
    }

    public void SetActiveTop(bool active)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.objTop))
        return;
      this.objTop.SetActiveIfDifferent(active);
    }

    public bool GetActiveTop()
    {
      return Object.op_Inequality((Object) null, (Object) this.objTop) && this.objTop.get_activeSelf();
    }

    public void SetPosition(float x, float y, float z)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.objTop))
        return;
      this.objTop.get_transform().set_localPosition(new Vector3(x, y, z));
    }

    public void SetPosition(Vector3 pos)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.objTop))
        return;
      this.objTop.get_transform().set_localPosition(pos);
    }

    public Vector3 GetPosition()
    {
      return Object.op_Equality((Object) null, (Object) this.objTop) ? Vector3.get_zero() : this.objTop.get_transform().get_localPosition();
    }

    public void SetRotation(float x, float y, float z)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.objTop))
        return;
      this.objTop.get_transform().set_localRotation(Quaternion.Euler(x, y, z));
    }

    public void SetRotation(Vector3 rot)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.objTop))
        return;
      this.objTop.get_transform().set_localRotation(Quaternion.Euler(rot));
    }

    public void SetRotation(Quaternion rot)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.objTop))
        return;
      this.objTop.get_transform().set_localRotation(rot);
    }

    public Vector3 GetRotation()
    {
      if (Object.op_Equality((Object) null, (Object) this.objTop))
        return Vector3.get_zero();
      Quaternion localRotation = this.objTop.get_transform().get_localRotation();
      return ((Quaternion) ref localRotation).get_eulerAngles();
    }

    public void SetTransform(Transform trf)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.objTop))
        return;
      this.objTop.get_transform().set_localPosition(trf.get_localPosition());
      this.objTop.get_transform().set_localRotation(trf.get_localRotation());
      this.objTop.get_transform().set_localScale(trf.get_localScale());
    }

    public void ChangeSettingMannequin(bool mannequin)
    {
      if (mannequin)
      {
        if (this.mannequinBackInfo.mannequin)
          return;
        this.mannequinBackInfo.mannequin = true;
        this.mannequinBackInfo.Backup(this);
        this.chaFile.LoadMannequinFile(ChaABDefine.PresetAssetBundle((int) this.sex), ChaABDefine.PresetAsset((int) this.sex), true, true, true, false, false);
        this.Reload(true, false, false, false, true);
        this.ChangeEyesPtn(0, false);
        this.ChangeEyesOpenMax(0.0f);
        this.ChangeMouthPtn(0, false);
        this.fileStatus.mouthFixed = true;
        this.ChangeMouthOpenMax(0.0f);
        this.neckLookCtrl.neckLookScript.skipCalc = true;
        this.neckLookCtrl.ForceLateUpdate();
        this.eyeLookCtrl.ForceLateUpdate();
        this.resetDynamicBoneAll = true;
        this.LateUpdateForce();
      }
      else
      {
        if (!this.mannequinBackInfo.mannequin)
          return;
        this.mannequinBackInfo.Restore(this);
        this.mannequinBackInfo.mannequin = false;
      }
    }

    public void RestoreMannequinHair()
    {
      ChaFileControl chaFileControl = new ChaFileControl();
      chaFileControl.SetCustomBytes(this.mannequinBackInfo.custom, ChaFileDefine.ChaFileCustomVersion);
      this.fileCustom.hair = chaFileControl.custom.hair;
      this.Reload(true, true, false, true, true);
    }

    public bool IsVisibleInCamera
    {
      get
      {
        if (Object.op_Inequality((Object) null, (Object) this.cmpBody) && this.cmpBody.isVisible || Object.op_Inequality((Object) null, (Object) this.cmpFace) && this.cmpFace.isVisible)
          return true;
        if (this.cmpHair != null)
        {
          for (int index = 0; index < this.cmpHair.Length; ++index)
          {
            if (!Object.op_Equality((Object) null, (Object) this.cmpHair[index]) && this.cmpHair[index].isVisible)
              return true;
          }
        }
        if (this.cmpClothes != null)
        {
          for (int index = 0; index < this.cmpClothes.Length; ++index)
          {
            if (!Object.op_Equality((Object) null, (Object) this.cmpClothes[index]) && this.cmpClothes[index].isVisible)
              return true;
          }
        }
        if (this.cmpAccessory != null)
        {
          for (int index = 0; index < this.cmpAccessory.Length; ++index)
          {
            if (!Object.op_Equality((Object) null, (Object) this.cmpAccessory[index]) && this.cmpAccessory[index].isVisible)
              return true;
          }
        }
        if (this.cmpExtraAccessory != null)
        {
          for (int index = 0; index < this.cmpExtraAccessory.Length; ++index)
          {
            if (!Object.op_Equality((Object) null, (Object) this.cmpExtraAccessory[index]) && this.cmpExtraAccessory[index].isVisible)
              return true;
          }
        }
        return false;
      }
    }

    public void OnDestroy()
    {
      if (Singleton<Character>.IsInstance())
        Singleton<Character>.Instance.DeleteChara(this, true);
      this.ReleaseAll();
    }

    public void UpdateForce()
    {
      if (!this.loadEnd)
        return;
      this.UpdateBlendShapeVoice();
      this.UpdateSiru(false);
      if (!this.updateWet)
        return;
      this.UpdateWet();
      this.updateWet = false;
    }

    public void LateUpdateForce()
    {
      if (!this.loadEnd)
        return;
      this.UpdateVisible();
      if (this.resetDynamicBoneAll)
      {
        this.ResetDynamicBoneAll(false);
        this.resetDynamicBoneAll = false;
      }
      if (this.updateShapeBody)
      {
        this.UpdateShapeBody();
        this.updateShapeBody = false;
      }
      if (this.updateShapeFace)
      {
        this.UpdateShapeFace();
        this.updateShapeFace = false;
      }
      this.UpdateAlwaysShapeBody();
      this.UpdateAlwaysShapeHand();
      if (Object.op_Inequality((Object) null, (Object) this.cmpBoneBody) && Object.op_Inequality((Object) null, (Object) this.cmpBoneBody.targetEtc.trfAnaCorrect) && Object.op_Inequality((Object) null, (Object) this.cmpBoneBody.targetAccessory.acs_Ana))
        this.cmpBoneBody.targetAccessory.acs_Ana.set_localScale(new Vector3((float) (1.0 / this.cmpBoneBody.targetEtc.trfAnaCorrect.get_localScale().x), (float) (1.0 / this.cmpBoneBody.targetEtc.trfAnaCorrect.get_localScale().y), (float) (1.0 / this.cmpBoneBody.targetEtc.trfAnaCorrect.get_localScale().z)));
      if (this.reSetupDynamicBoneBust)
      {
        this.ReSetupDynamicBoneBust(0);
        this.UpdateBustSoftnessAndGravity();
        this.reSetupDynamicBoneBust = false;
      }
      if (this.updateBustSize && this.sex == (byte) 1)
      {
        int index = 1;
        float rate = 1f;
        if (0.5 > (double) this.chaFile.custom.body.shapeValueBody[index])
          rate = Mathf.InverseLerp(0.0f, 0.5f, this.chaFile.custom.body.shapeValueBody[index]);
        if (this.bustNormal != null)
          this.bustNormal.Blend(rate);
        this.updateBustSize = false;
      }
      if (!this.IsVisibleInCamera)
      {
        if (Object.op_Inequality((Object) null, (Object) this.cmpBoneBody))
        {
          for (int area = 0; area < this.enableDynamicBoneBustAndHip.Length; ++area)
            this.cmpBoneBody.EnableDynamicBonesBustAndHip(false, area);
        }
        if (this.cmpHair != null)
        {
          for (int index = 0; index < this.cmpHair.Length; ++index)
          {
            if (Object.op_Inequality((Object) null, (Object) this.cmpHair[index]))
              this.cmpHair[index].EnableDynamicBonesHair(false, (ChaFileHair.PartsInfo) null);
          }
        }
        if (this.cmpClothes != null)
        {
          for (int index = 0; index < this.cmpClothes.Length; ++index)
          {
            if (Object.op_Inequality((Object) null, (Object) this.cmpClothes[index]))
              this.cmpClothes[index].EnableDynamicBones(false);
          }
        }
        if (this.cmpAccessory != null)
        {
          for (int index = 0; index < this.cmpAccessory.Length; ++index)
          {
            if (Object.op_Inequality((Object) null, (Object) this.cmpAccessory[index]))
              this.cmpAccessory[index].EnableDynamicBones(false);
          }
        }
        if (this.cmpExtraAccessory == null)
          return;
        for (int index = 0; index < this.cmpExtraAccessory.Length; ++index)
        {
          if (Object.op_Inequality((Object) null, (Object) this.cmpExtraAccessory[index]))
            this.cmpExtraAccessory[index].EnableDynamicBones(false);
        }
      }
      else
      {
        if (Object.op_Inequality((Object) null, (Object) this.cmpBoneBody))
        {
          for (int area = 0; area < this.enableDynamicBoneBustAndHip.Length; ++area)
            this.cmpBoneBody.EnableDynamicBonesBustAndHip(this.cmpBody.isVisible && this.enableDynamicBoneBustAndHip[area], area);
        }
        if (this.cmpHair != null)
        {
          for (int index = 0; index < this.cmpHair.Length; ++index)
          {
            if (Object.op_Inequality((Object) null, (Object) this.cmpHair[index]))
              this.cmpHair[index].EnableDynamicBonesHair(this.cmpHair[index].isVisible, this.fileHair.parts[index]);
          }
        }
        if (this.cmpClothes != null)
        {
          for (int index = 0; index < this.cmpClothes.Length; ++index)
          {
            if (Object.op_Inequality((Object) null, (Object) this.cmpClothes[index]))
              this.cmpClothes[index].EnableDynamicBones(this.fileStatus.clothesState[index] == (byte) 0 && this.cmpClothes[index].isVisible);
          }
        }
        if (this.cmpAccessory != null)
        {
          for (int index = 0; index < this.cmpAccessory.Length; ++index)
          {
            if (Object.op_Inequality((Object) null, (Object) this.cmpAccessory[index]))
              this.cmpAccessory[index].EnableDynamicBones(!this.nowCoordinate.accessory.parts[index].noShake && this.cmpAccessory[index].isVisible);
          }
        }
        if (this.cmpExtraAccessory == null)
          return;
        for (int index = 0; index < this.cmpExtraAccessory.Length; ++index)
        {
          if (Object.op_Inequality((Object) null, (Object) this.cmpExtraAccessory[index]))
            this.cmpExtraAccessory[index].EnableDynamicBones(this.cmpExtraAccessory[index].isVisible);
        }
      }
    }

    public bool[] hideHairAcs { get; private set; }

    protected void InitializeControlAccessoryAll()
    {
      this.InitializeControlAccessoryObject();
    }

    protected void InitializeControlAccessoryObject()
    {
      this.hideHairAcs = new bool[20];
    }

    protected void ReleaseControlAccessoryAll()
    {
      this.ReleaseControlAccessoryObject(false);
    }

    protected void ReleaseControlAccessoryObject(bool init = true)
    {
      if (!init)
        return;
      this.InitializeControlAccessoryObject();
    }

    public bool IsAccessory(int slotNo)
    {
      return MathfEx.RangeEqualOn<int>(0, slotNo, 19) && !Object.op_Equality((Object) null, (Object) this.objAccessory[slotNo]);
    }

    public void SetAccessoryState(int slotNo, bool show)
    {
      if (this.fileStatus.showAccessory.Length <= slotNo)
        return;
      this.fileStatus.showAccessory[slotNo] = show;
    }

    public void SetAccessoryStateAll(bool show)
    {
      for (int index = 0; index < this.fileStatus.showAccessory.Length; ++index)
        this.fileStatus.showAccessory[index] = show;
    }

    public string GetAccessoryDefaultParentStr(int type, int id)
    {
      int length = Enum.GetNames(typeof (ChaListDefine.CategoryNo)).Length;
      if (!MathfEx.RangeEqualOn<int>(0, type, length - 1))
        return string.Empty;
      ChaListDefine.CategoryNo type1 = (ChaListDefine.CategoryNo) type;
      ListInfoBase listInfoBase = (ListInfoBase) null;
      return !this.lstCtrl.GetCategoryInfo(type1).TryGetValue(id, out listInfoBase) ? string.Empty : listInfoBase.GetInfo(ChaListDefine.KeyType.Parent);
    }

    public string GetAccessoryDefaultParentStr(int slotNo)
    {
      GameObject gameObject = this.objAccessory[slotNo];
      return Object.op_Equality((Object) null, (Object) gameObject) ? string.Empty : ((ListInfoComponent) gameObject.GetComponent<ListInfoComponent>()).data.GetInfo(ChaListDefine.KeyType.Parent);
    }

    public bool ChangeAccessoryParent(int slotNo, string parentStr)
    {
      if (!MathfEx.RangeEqualOn<int>(0, slotNo, 19))
        return false;
      GameObject gameObject = this.objAccessory[slotNo];
      if (Object.op_Equality((Object) null, (Object) gameObject))
        return false;
      if ("none" == parentStr)
      {
        gameObject.get_transform().SetParent((Transform) null, false);
        return true;
      }
      if ("0" == ((ListInfoComponent) gameObject.GetComponent<ListInfoComponent>()).data.GetInfo(ChaListDefine.KeyType.Parent))
        return false;
      try
      {
        Transform accessoryParentTransform = this.GetAccessoryParentTransform(parentStr);
        if (Object.op_Equality((Object) null, (Object) accessoryParentTransform))
        {
          Debug.LogWarning((object) "親が見付からない");
          return false;
        }
        gameObject.get_transform().SetParent(accessoryParentTransform, false);
        this.nowCoordinate.accessory.parts[slotNo].parentKey = parentStr;
        this.nowCoordinate.accessory.parts[slotNo].partsOfHead = ChaAccessoryDefine.CheckPartsOfHead(parentStr);
      }
      catch (ArgumentException ex)
      {
        return false;
      }
      return true;
    }

    public bool SetAccessoryPos(int slotNo, int correctNo, float value, bool add, int flags = 7)
    {
      if (!MathfEx.RangeEqualOn<int>(0, slotNo, 19))
        return false;
      Transform transform = this.trfAcsMove[slotNo, correctNo];
      if (Object.op_Equality((Object) null, (Object) transform))
        return false;
      ChaFileAccessory accessory = this.nowCoordinate.accessory;
      if ((flags & 1) != 0)
      {
        float num = float.Parse(((!add ? 0.0f : (float) (double) accessory.parts[slotNo].addMove[correctNo, 0].x) + value).ToString("f1"));
        accessory.parts[slotNo].addMove[correctNo, 0].x = (__Null) (double) Mathf.Clamp(num, -100f, 100f);
      }
      if ((flags & 2) != 0)
      {
        float num = float.Parse(((!add ? 0.0f : (float) (double) accessory.parts[slotNo].addMove[correctNo, 0].y) + value).ToString("f1"));
        accessory.parts[slotNo].addMove[correctNo, 0].y = (__Null) (double) Mathf.Clamp(num, -100f, 100f);
      }
      if ((flags & 4) != 0)
      {
        float num = float.Parse(((!add ? 0.0f : (float) (double) accessory.parts[slotNo].addMove[correctNo, 0].z) + value).ToString("f1"));
        accessory.parts[slotNo].addMove[correctNo, 0].z = (__Null) (double) Mathf.Clamp(num, -100f, 100f);
      }
      transform.set_localPosition(new Vector3((float) (accessory.parts[slotNo].addMove[correctNo, 0].x * 0.100000001490116), (float) (accessory.parts[slotNo].addMove[correctNo, 0].y * 0.100000001490116), (float) (accessory.parts[slotNo].addMove[correctNo, 0].z * 0.100000001490116)));
      return true;
    }

    public bool SetAccessoryRot(int slotNo, int correctNo, float value, bool add, int flags = 7)
    {
      if (!MathfEx.RangeEqualOn<int>(0, slotNo, 19))
        return false;
      Transform transform = this.trfAcsMove[slotNo, correctNo];
      if (Object.op_Equality((Object) null, (Object) transform))
        return false;
      ChaFileAccessory accessory = this.nowCoordinate.accessory;
      if ((flags & 1) != 0)
      {
        float num = (float) (int) ((!add ? 0.0 : (double) accessory.parts[slotNo].addMove[correctNo, 1].x) + (double) value);
        accessory.parts[slotNo].addMove[correctNo, 1].x = (__Null) (double) Mathf.Repeat(num, 360f);
      }
      if ((flags & 2) != 0)
      {
        float num = (float) (int) ((!add ? 0.0 : (double) accessory.parts[slotNo].addMove[correctNo, 1].y) + (double) value);
        accessory.parts[slotNo].addMove[correctNo, 1].y = (__Null) (double) Mathf.Repeat(num, 360f);
      }
      if ((flags & 4) != 0)
      {
        float num = (float) (int) ((!add ? 0.0 : (double) accessory.parts[slotNo].addMove[correctNo, 1].z) + (double) value);
        accessory.parts[slotNo].addMove[correctNo, 1].z = (__Null) (double) Mathf.Repeat(num, 360f);
      }
      transform.set_localEulerAngles(new Vector3((float) accessory.parts[slotNo].addMove[correctNo, 1].x, (float) accessory.parts[slotNo].addMove[correctNo, 1].y, (float) accessory.parts[slotNo].addMove[correctNo, 1].z));
      return true;
    }

    public bool SetAccessoryScl(int slotNo, int correctNo, float value, bool add, int flags = 7)
    {
      if (!MathfEx.RangeEqualOn<int>(0, slotNo, 19))
        return false;
      Transform transform = this.trfAcsMove[slotNo, correctNo];
      if (Object.op_Equality((Object) null, (Object) transform))
        return false;
      ChaFileAccessory accessory = this.nowCoordinate.accessory;
      if ((flags & 1) != 0)
      {
        float num = float.Parse(((!add ? 0.0f : (float) (double) accessory.parts[slotNo].addMove[correctNo, 2].x) + value).ToString("f2"));
        accessory.parts[slotNo].addMove[correctNo, 2].x = (__Null) (double) Mathf.Clamp(num, 0.01f, 100f);
      }
      if ((flags & 2) != 0)
      {
        float num = float.Parse(((!add ? 0.0f : (float) (double) accessory.parts[slotNo].addMove[correctNo, 2].y) + value).ToString("f2"));
        accessory.parts[slotNo].addMove[correctNo, 2].y = (__Null) (double) Mathf.Clamp(num, 0.01f, 100f);
      }
      if ((flags & 4) != 0)
      {
        float num = float.Parse(((!add ? 0.0f : (float) (double) accessory.parts[slotNo].addMove[correctNo, 2].z) + value).ToString("f2"));
        accessory.parts[slotNo].addMove[correctNo, 2].z = (__Null) (double) Mathf.Clamp(num, 0.01f, 100f);
      }
      transform.set_localScale(new Vector3((float) accessory.parts[slotNo].addMove[correctNo, 2].x, (float) accessory.parts[slotNo].addMove[correctNo, 2].y, (float) accessory.parts[slotNo].addMove[correctNo, 2].z));
      return true;
    }

    public bool ResetAccessoryMove(int slotNo, int correctNo, int type = 7)
    {
      bool flag = true;
      if ((type & 1) != 0)
        flag &= this.SetAccessoryPos(slotNo, correctNo, 0.0f, false, 7);
      if ((type & 2) != 0)
        flag &= this.SetAccessoryRot(slotNo, correctNo, 0.0f, false, 7);
      if ((type & 4) != 0)
        flag &= this.SetAccessoryScl(slotNo, correctNo, 1f, false, 7);
      return flag;
    }

    public bool UpdateAccessoryMoveFromInfo(int slotNo)
    {
      if (!MathfEx.RangeEqualOn<int>(0, slotNo, 19))
        return false;
      ChaFileAccessory accessory = this.nowCoordinate.accessory;
      for (int index = 0; index < 2; ++index)
      {
        Transform transform = this.trfAcsMove[slotNo, index];
        if (!Object.op_Equality((Object) null, (Object) transform))
        {
          transform.set_localPosition(new Vector3((float) (accessory.parts[slotNo].addMove[index, 0].x * 0.100000001490116), (float) (accessory.parts[slotNo].addMove[index, 0].y * 0.100000001490116), (float) (accessory.parts[slotNo].addMove[index, 0].z * 0.100000001490116)));
          transform.set_localEulerAngles(new Vector3((float) accessory.parts[slotNo].addMove[index, 1].x, (float) accessory.parts[slotNo].addMove[index, 1].y, (float) accessory.parts[slotNo].addMove[index, 1].z));
          transform.set_localScale(new Vector3((float) accessory.parts[slotNo].addMove[index, 2].x, (float) accessory.parts[slotNo].addMove[index, 2].y, (float) accessory.parts[slotNo].addMove[index, 2].z));
        }
      }
      return true;
    }

    public bool UpdateAccessoryMoveAllFromInfo()
    {
      for (int slotNo = 0; slotNo < 20; ++slotNo)
        this.UpdateAccessoryMoveFromInfo(slotNo);
      return true;
    }

    public bool ChangeAccessoryColor(int slotNo)
    {
      if (!MathfEx.RangeEqualOn<int>(0, slotNo, 19))
        return false;
      CmpAccessory cmpAccessory = this.cmpAccessory[slotNo];
      ChaFileAccessory.PartsInfo part = this.nowCoordinate.accessory.parts[slotNo];
      if (Object.op_Equality((Object) null, (Object) cmpAccessory))
        return false;
      if (cmpAccessory.rendNormal != null)
      {
        foreach (Renderer renderer in cmpAccessory.rendNormal)
        {
          if (cmpAccessory.useColor01)
          {
            renderer.get_material().SetColor(ChaShader.Color, part.colorInfo[0].color);
            renderer.get_material().SetFloat(ChaShader.ClothesGloss1, part.colorInfo[0].glossPower);
            renderer.get_material().SetFloat(ChaShader.Metallic, part.colorInfo[0].metallicPower);
          }
          if (cmpAccessory.useColor02)
          {
            renderer.get_material().SetColor(ChaShader.Color2, part.colorInfo[1].color);
            renderer.get_material().SetFloat(ChaShader.ClothesGloss2, part.colorInfo[1].glossPower);
            renderer.get_material().SetFloat(ChaShader.Metallic2, part.colorInfo[1].metallicPower);
          }
          if (cmpAccessory.useColor03)
          {
            renderer.get_material().SetColor(ChaShader.Color3, part.colorInfo[2].color);
            renderer.get_material().SetFloat(ChaShader.ClothesGloss3, part.colorInfo[2].glossPower);
            renderer.get_material().SetFloat(ChaShader.Metallic3, part.colorInfo[2].metallicPower);
          }
        }
      }
      if (cmpAccessory.rendAlpha != null)
      {
        foreach (Renderer renderer in cmpAccessory.rendAlpha)
        {
          renderer.get_material().SetColor(ChaShader.Color, part.colorInfo[3].color);
          renderer.get_material().SetFloat(ChaShader.ClothesGloss4, part.colorInfo[3].glossPower);
          renderer.get_material().SetFloat(ChaShader.Metallic4, part.colorInfo[3].metallicPower);
          ((Component) renderer).get_gameObject().SetActiveIfDifferent(part.colorInfo[3].color.a != 0.0);
        }
      }
      return true;
    }

    public bool GetAccessoryDefaultColor(
      ref Color color,
      ref float gloss,
      ref float metallic,
      int slotNo,
      int no)
    {
      if (!MathfEx.RangeEqualOn<int>(0, slotNo, 19))
        return false;
      CmpAccessory cmpAccessory = this.cmpAccessory[slotNo];
      if (Object.op_Equality((Object) null, (Object) cmpAccessory))
        return false;
      if (no == 0 && cmpAccessory.useColor01)
      {
        color = cmpAccessory.defColor01;
        gloss = cmpAccessory.defGlossPower01;
        metallic = cmpAccessory.defMetallicPower01;
        return true;
      }
      if (no == 1 && cmpAccessory.useColor02)
      {
        color = cmpAccessory.defColor02;
        gloss = cmpAccessory.defGlossPower02;
        metallic = cmpAccessory.defMetallicPower02;
        return true;
      }
      if (no == 2 && cmpAccessory.useColor03)
      {
        color = cmpAccessory.defColor03;
        gloss = cmpAccessory.defGlossPower03;
        metallic = cmpAccessory.defMetallicPower03;
        return true;
      }
      if (no != 3 || cmpAccessory.rendAlpha == null || cmpAccessory.rendAlpha.Length == 0)
        return false;
      color = cmpAccessory.defColor04;
      gloss = cmpAccessory.defGlossPower04;
      metallic = cmpAccessory.defMetallicPower04;
      return true;
    }

    public void SetAccessoryDefaultColor(int slotNo)
    {
      if (!MathfEx.RangeEqualOn<int>(0, slotNo, 19))
        return;
      CmpAccessory cmpAccessory = this.cmpAccessory[slotNo];
      if (Object.op_Equality((Object) null, (Object) cmpAccessory))
        return;
      if (cmpAccessory.useColor01)
      {
        this.nowCoordinate.accessory.parts[slotNo].colorInfo[0].color = cmpAccessory.defColor01;
        this.nowCoordinate.accessory.parts[slotNo].colorInfo[0].glossPower = cmpAccessory.defGlossPower01;
        this.nowCoordinate.accessory.parts[slotNo].colorInfo[0].metallicPower = cmpAccessory.defMetallicPower01;
      }
      if (cmpAccessory.useColor02)
      {
        this.nowCoordinate.accessory.parts[slotNo].colorInfo[1].color = cmpAccessory.defColor02;
        this.nowCoordinate.accessory.parts[slotNo].colorInfo[1].glossPower = cmpAccessory.defGlossPower02;
        this.nowCoordinate.accessory.parts[slotNo].colorInfo[1].metallicPower = cmpAccessory.defMetallicPower02;
      }
      if (cmpAccessory.useColor03)
      {
        this.nowCoordinate.accessory.parts[slotNo].colorInfo[2].color = cmpAccessory.defColor03;
        this.nowCoordinate.accessory.parts[slotNo].colorInfo[2].glossPower = cmpAccessory.defGlossPower03;
        this.nowCoordinate.accessory.parts[slotNo].colorInfo[2].metallicPower = cmpAccessory.defMetallicPower03;
      }
      if (cmpAccessory.rendAlpha == null || cmpAccessory.rendAlpha.Length == 0)
        return;
      this.nowCoordinate.accessory.parts[slotNo].colorInfo[3].color = cmpAccessory.defColor04;
      this.nowCoordinate.accessory.parts[slotNo].colorInfo[3].glossPower = cmpAccessory.defGlossPower04;
      this.nowCoordinate.accessory.parts[slotNo].colorInfo[3].metallicPower = cmpAccessory.defMetallicPower04;
    }

    public void ResetDynamicBoneAccessories(bool includeInactive = false)
    {
      if (this.cmpAccessory == null)
        return;
      for (int index = 0; index < this.cmpAccessory.Length; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) this.cmpAccessory[index]))
          this.cmpAccessory[index].ResetDynamicBones(includeInactive);
      }
    }

    public bool ChangeHairTypeAccessoryColor(int slotNo)
    {
      if (!MathfEx.RangeEqualOn<int>(0, slotNo, 19))
        return false;
      CmpAccessory cmpAccessory = this.cmpAccessory[slotNo];
      ChaFileAccessory.PartsInfo part = this.nowCoordinate.accessory.parts[slotNo];
      if (Object.op_Equality((Object) null, (Object) cmpAccessory))
        return false;
      if (cmpAccessory.rendNormal != null)
      {
        foreach (Renderer renderer in cmpAccessory.rendNormal)
        {
          if (cmpAccessory.useColor01)
            renderer.get_material().SetColor(ChaShader.HairMainColor, part.colorInfo[0].color);
          if (cmpAccessory.useColor02)
            renderer.get_material().SetColor(ChaShader.HairTopColor, part.colorInfo[1].color);
          if (cmpAccessory.useColor03)
            renderer.get_material().SetColor(ChaShader.HairUnderColor, part.colorInfo[2].color);
          renderer.get_material().SetColor(ChaShader.Specular, part.colorInfo[3].color);
          renderer.get_material().SetFloat(ChaShader.Smoothness, part.colorInfo[0].smoothnessPower);
          renderer.get_material().SetFloat(ChaShader.Metallic, part.colorInfo[0].metallicPower);
        }
      }
      return true;
    }

    public void ChangeSettingHairTypeAccessoryShaderAll()
    {
      for (int slotNo = 0; slotNo < 20; ++slotNo)
        this.ChangeSettingHairTypeAccessoryShader(slotNo);
    }

    public void ChangeSettingHairTypeAccessoryShader(int slotNo)
    {
      if (!MathfEx.RangeEqualOn<int>(0, slotNo, 19))
        return;
      CmpAccessory cmpAccessory = this.cmpAccessory[slotNo];
      if (Object.op_Equality((Object) null, (Object) cmpAccessory))
        return;
      ChaFileAccessory.PartsInfo part = this.nowCoordinate.accessory.parts[slotNo];
      if (!cmpAccessory.typeHair)
        return;
      ChaFileHair hair = this.chaFile.custom.hair;
      Shader shader = hair.shaderType != 0 ? Singleton<Character>.Instance.shaderCutout : Singleton<Character>.Instance.shaderDithering;
      if (this.infoAccessory[slotNo] == null)
        return;
      string info1 = this.infoAccessory[slotNo].GetInfo(ChaListDefine.KeyType.TexManifest);
      string info2 = this.infoAccessory[slotNo].GetInfo(ChaListDefine.KeyType.TexAB);
      string info3 = this.infoAccessory[slotNo].GetInfo(hair.shaderType != 0 ? ChaListDefine.KeyType.TexC : ChaListDefine.KeyType.TexD);
      Texture2D texture2D = CommonLib.LoadAsset<Texture2D>(info2, info3, false, info1);
      Singleton<Character>.Instance.AddLoadAssetBundle(info2, info1);
      for (int index1 = 0; index1 < cmpAccessory.rendNormal.Length; ++index1)
      {
        for (int index2 = 0; index2 < cmpAccessory.rendNormal[index1].get_materials().Length; ++index2)
        {
          int renderQueue = cmpAccessory.rendNormal[index1].get_materials()[index2].get_renderQueue();
          cmpAccessory.rendNormal[index1].get_materials()[index2].set_shader(shader);
          cmpAccessory.rendNormal[index1].get_materials()[index2].SetTexture(ChaShader.MainTex, (Texture) texture2D);
          cmpAccessory.rendNormal[index1].get_materials()[index2].set_renderQueue(renderQueue);
        }
      }
    }

    public void ChangeExtraAccessory(ChaControlDefine.ExtraAccessoryParts parts, int id)
    {
      this.StartCoroutine(this.ChangeExtraAccessory(parts, id, false));
    }

    [DebuggerHidden]
    public IEnumerator ChangeExtraAccessory(
      ChaControlDefine.ExtraAccessoryParts parts,
      int id,
      bool asyncFlags = true)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CChangeExtraAccessory\u003Ec__Iterator0()
      {
        parts = parts,
        id = id,
        asyncFlags = asyncFlags,
        \u0024this = this
      };
    }

    public void ResetDynamicBoneExtraAccessories(bool includeInactive = false)
    {
      if (this.cmpExtraAccessory == null)
        return;
      for (int index = 0; index < this.cmpExtraAccessory.Length; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) this.cmpExtraAccessory[index]))
          this.cmpExtraAccessory[index].ResetDynamicBones(includeInactive);
      }
    }

    public bool ChangeExtraAccessoryColor(
      ChaControlDefine.ExtraAccessoryParts parts,
      params Color[] color)
    {
      if (color == null)
        return false;
      int n = (int) parts;
      if (!MathfEx.RangeEqualOn<int>(0, n, Enum.GetNames(typeof (ChaControlDefine.ExtraAccessoryParts)).Length - 1))
        return false;
      CmpAccessory cmpAccessory = this.cmpExtraAccessory[n];
      if (Object.op_Equality((Object) null, (Object) cmpAccessory))
        return false;
      if (cmpAccessory.rendNormal != null)
      {
        foreach (Renderer renderer in cmpAccessory.rendNormal)
        {
          if (cmpAccessory.useColor01 && 1 <= color.Length)
            renderer.get_material().SetColor(ChaShader.Color, color[0]);
          if (cmpAccessory.useColor02 && 2 <= color.Length)
            renderer.get_material().SetColor(ChaShader.Color2, color[1]);
          if (cmpAccessory.useColor03 && 3 <= color.Length)
            renderer.get_material().SetColor(ChaShader.Color3, color[2]);
        }
      }
      if (cmpAccessory.rendAlpha != null && 4 <= color.Length)
      {
        foreach (Renderer renderer in cmpAccessory.rendAlpha)
          renderer.get_material().SetColor(ChaShader.Color, color[3]);
      }
      return true;
    }

    public bool GetExtraAccessoryDefaultColor(
      ref Color color,
      ChaControlDefine.ExtraAccessoryParts parts,
      int no)
    {
      int n = (int) parts;
      if (!MathfEx.RangeEqualOn<int>(0, n, Enum.GetNames(typeof (ChaControlDefine.ExtraAccessoryParts)).Length - 1))
        return false;
      CmpAccessory cmpAccessory = this.cmpExtraAccessory[n];
      if (Object.op_Equality((Object) null, (Object) cmpAccessory))
        return false;
      if (no == 0 && cmpAccessory.useColor01)
      {
        color = cmpAccessory.defColor01;
        return true;
      }
      if (no == 1 && cmpAccessory.useColor02)
      {
        color = cmpAccessory.defColor02;
        return true;
      }
      if (no == 2 && cmpAccessory.useColor03)
      {
        color = cmpAccessory.defColor03;
        return true;
      }
      if (no != 3 || cmpAccessory.rendAlpha == null || cmpAccessory.rendAlpha.Length == 0)
        return false;
      color = cmpAccessory.defColor04;
      return true;
    }

    public void ShowExtraAccessory(ChaControlDefine.ExtraAccessoryParts parts, bool show)
    {
      this.showExtraAccessory[(int) parts] = show;
    }

    public RuntimeAnimatorController LoadAnimation(
      string assetBundleName,
      string assetName,
      string manifestName = "")
    {
      if (Object.op_Equality((Object) null, (Object) this.animBody))
        return (RuntimeAnimatorController) null;
      RuntimeAnimatorController animatorController = CommonLib.LoadAsset<RuntimeAnimatorController>(assetBundleName, assetName, false, manifestName);
      if (Object.op_Equality((Object) null, (Object) animatorController))
        return (RuntimeAnimatorController) null;
      this.animBody.set_runtimeAnimatorController(animatorController);
      AssetBundleManager.UnloadAssetBundle(assetBundleName, true, (string) null, false);
      return animatorController;
    }

    public void AnimPlay(string stateName)
    {
      if (Object.op_Equality((Object) null, (Object) this.animBody))
        return;
      this.animBody.Play(stateName);
    }

    public AnimatorStateInfo getAnimatorStateInfo(int _nLayer)
    {
      return Object.op_Equality((Object) null, (Object) this.animBody) || Object.op_Equality((Object) null, (Object) this.animBody.get_runtimeAnimatorController()) ? (AnimatorStateInfo) null : this.animBody.GetCurrentAnimatorStateInfo(_nLayer);
    }

    public bool syncPlay(AnimatorStateInfo _syncState, int _nLayer)
    {
      if (Object.op_Equality((Object) null, (Object) this.animBody))
        return false;
      this.animBody.Play(((AnimatorStateInfo) ref _syncState).get_shortNameHash(), _nLayer, ((AnimatorStateInfo) ref _syncState).get_normalizedTime());
      return true;
    }

    public bool syncPlay(int _nameHash, int _nLayer, float _fnormalizedTime)
    {
      if (Object.op_Equality((Object) null, (Object) this.animBody))
        return false;
      this.animBody.Play(_nameHash, _nLayer, _fnormalizedTime);
      return true;
    }

    public bool syncPlay(string _strameHash, int _nLayer, float _fnormalizedTime)
    {
      if (Object.op_Equality((Object) null, (Object) this.animBody))
        return false;
      this.animBody.Play(_strameHash, _nLayer, _fnormalizedTime);
      return true;
    }

    public bool setLayerWeight(float _fWeight, int _nLayer)
    {
      if (Object.op_Equality((Object) null, (Object) this.animBody))
        return false;
      this.animBody.SetLayerWeight(_nLayer, _fWeight);
      return true;
    }

    public bool setAllLayerWeight(float _fWeight)
    {
      if (Object.op_Equality((Object) null, (Object) this.animBody))
        return false;
      for (int index = 1; index < this.animBody.get_layerCount(); ++index)
        this.animBody.SetLayerWeight(index, _fWeight);
      return true;
    }

    public float getLayerWeight(int _nLayer)
    {
      return Object.op_Equality((Object) null, (Object) this.animBody) ? 0.0f : this.animBody.GetLayerWeight(_nLayer);
    }

    public bool setPlay(string _strAnmName, int _nLayer)
    {
      if (Object.op_Equality((Object) null, (Object) this.animBody))
        return false;
      this.animBody.Play(_strAnmName, _nLayer);
      return true;
    }

    public void setAnimatorParamTrigger(string _strAnmName)
    {
      if (Object.op_Equality((Object) null, (Object) this.animBody))
        return;
      this.animBody.SetTrigger(_strAnmName);
    }

    public void setAnimatorParamResetTrigger(string _strAnmName)
    {
      if (Object.op_Equality((Object) null, (Object) this.animBody))
        return;
      this.animBody.ResetTrigger(_strAnmName);
    }

    public void setAnimatorParamBool(string _strAnmName, bool _bFlag)
    {
      if (Object.op_Equality((Object) null, (Object) this.animBody))
        return;
      this.animBody.SetBool(_strAnmName, _bFlag);
    }

    public bool getAnimatorParamBool(string _strAnmName)
    {
      return !Object.op_Equality((Object) null, (Object) this.animBody) && this.animBody.GetBool(_strAnmName);
    }

    public void setAnimatorParamFloat(string _strAnmName, float _fValue)
    {
      if (!Object.op_Inequality((Object) this.animBody, (Object) null))
        return;
      this.animBody.SetFloat(_strAnmName, _fValue);
    }

    public void setAnimPtnCrossFade(
      string _strAnmName,
      float _fBlendTime,
      int _nLayer,
      float _fCrossStateTime)
    {
      if (Object.op_Equality((Object) null, (Object) this.animBody))
        return;
      this.animBody.CrossFade(_strAnmName, _fBlendTime, _nLayer, _fCrossStateTime);
    }

    public bool isBlend(int _nLayer)
    {
      return !Object.op_Equality((Object) null, (Object) this.animBody) && this.animBody.IsInTransition(_nLayer);
    }

    public bool IsNextHash(int _nLayer, string _nameHash)
    {
      if (Object.op_Equality((Object) null, (Object) this.animBody))
        return false;
      AnimatorStateInfo animatorStateInfo = this.animBody.GetNextAnimatorStateInfo(_nLayer);
      return ((AnimatorStateInfo) ref animatorStateInfo).IsName(_nameHash);
    }

    public bool IsParameterInAnimator(string strParameter)
    {
      return !Object.op_Equality((Object) this.animBody, (Object) null) && !Object.op_Equality((Object) this.animBody.get_runtimeAnimatorController(), (Object) null) && Array.FindIndex<AnimatorControllerParameter>(this.animBody.get_parameters(), (Predicate<AnimatorControllerParameter>) (p => p.get_name() == strParameter)) != -1;
    }

    private Texture texBodyAlphaMask { get; set; }

    private Texture texBraAlphaMask { get; set; }

    private Texture texInnerTBAlphaMask { get; set; }

    private Texture texInnerBAlphaMask { get; set; }

    private Texture texPanstAlphaMask { get; set; }

    private Texture texBodyBAlphaMask { get; set; }

    private int underMaskReflectionType { get; set; } = -1;

    private bool underMaskBreakDisable { get; set; }

    public bool hideInnerBWithBot { get; set; }

    public BustNormal bustNormal { get; private set; }

    private byte[] siruNewLv { get; set; }

    protected void InitializeControlBodyAll()
    {
      this.siruNewLv = new byte[Enum.GetNames(typeof (ChaFileDefine.SiruParts)).Length];
      for (int index = 0; index < this.siruNewLv.Length; ++index)
        this.siruNewLv[index] = (byte) 0;
      this.InitializeControlBodyObject();
    }

    protected void InitializeControlBodyObject()
    {
      this.texBodyAlphaMask = (Texture) null;
      this.texBraAlphaMask = (Texture) null;
      this.texInnerTBAlphaMask = (Texture) null;
      this.texInnerBAlphaMask = (Texture) null;
      this.texPanstAlphaMask = (Texture) null;
      this.texBodyBAlphaMask = (Texture) null;
      this.hideInnerBWithBot = false;
      this.bustNormal = (BustNormal) null;
    }

    protected void ReleaseControlBodyAll()
    {
      this.ReleaseControlBodyObject(false);
    }

    protected void ReleaseControlBodyObject(bool init = true)
    {
      if (this.sex == (byte) 1)
      {
        if (this.bustNormal != null)
          this.bustNormal.Release();
        this.bustNormal = (BustNormal) null;
      }
      for (int index = 0; index < this.siruNewLv.Length; ++index)
        this.siruNewLv[index] = (byte) 0;
      OutputLog.Log(nameof (ReleaseControlBodyObject), false, "UnloadUnusedAssets");
      Resources.UnloadUnusedAssets();
      if (!init)
        return;
      this.InitializeControlBodyObject();
    }

    public void ResetDynamicBoneBustAndHip(bool includeInactive = false)
    {
      if (!Object.op_Inequality((Object) null, (Object) this.cmpBoneBody))
        return;
      this.cmpBoneBody.ResetDynamicBonesBustAndHip(includeInactive);
    }

    public void ResetDynamicBoneAll(bool includeInactive = false)
    {
      this.ResetDynamicBoneHair(includeInactive);
      this.ResetDynamicBoneBustAndHip(includeInactive);
      this.ResetDynamicBoneClothes(includeInactive);
      this.ResetDynamicBoneAccessories(includeInactive);
    }

    public DynamicBone_Ver02 GetDynamicBoneBustAndHip(
      ChaControlDefine.DynamicBoneKind area)
    {
      return Object.op_Equality((Object) null, (Object) this.cmpBoneBody) ? (DynamicBone_Ver02) null : this.cmpBoneBody.GetDynamicBoneBustAndHip(area);
    }

    public bool ReSetupDynamicBoneBust(int cate = 0)
    {
      if (cate == 0)
      {
        DynamicBone_Ver02 dynamicBoneBustAndHip1 = this.GetDynamicBoneBustAndHip(ChaControlDefine.DynamicBoneKind.BreastL);
        if (Object.op_Inequality((Object) null, (Object) dynamicBoneBustAndHip1))
          dynamicBoneBustAndHip1.ResetPosition();
        DynamicBone_Ver02 dynamicBoneBustAndHip2 = this.GetDynamicBoneBustAndHip(ChaControlDefine.DynamicBoneKind.BreastR);
        if (Object.op_Inequality((Object) null, (Object) dynamicBoneBustAndHip2))
          dynamicBoneBustAndHip2.ResetPosition();
      }
      else
      {
        DynamicBone_Ver02 dynamicBoneBustAndHip1 = this.GetDynamicBoneBustAndHip(ChaControlDefine.DynamicBoneKind.HipL);
        if (Object.op_Inequality((Object) null, (Object) dynamicBoneBustAndHip1))
          dynamicBoneBustAndHip1.ResetPosition();
        DynamicBone_Ver02 dynamicBoneBustAndHip2 = this.GetDynamicBoneBustAndHip(ChaControlDefine.DynamicBoneKind.HipR);
        if (Object.op_Inequality((Object) null, (Object) dynamicBoneBustAndHip2))
          dynamicBoneBustAndHip2.ResetPosition();
      }
      return true;
    }

    public void playDynamicBoneBust(int area, bool play)
    {
      if (area >= this.enableDynamicBoneBustAndHip.Length)
        return;
      this.enableDynamicBoneBustAndHip[area] = this.sex != (byte) 0 && play;
    }

    public void playDynamicBoneBust(ChaControlDefine.DynamicBoneKind area, bool play)
    {
      this.playDynamicBoneBust((int) area, play);
    }

    public bool ChangeNipRate(float rate)
    {
      this.chaFile.status.nipStandRate = rate;
      this.changeShapeBodyMask = true;
      this.updateShapeBody = true;
      return true;
    }

    public void ChangeBustInert(bool h)
    {
      if (this.sex != (byte) 1)
        return;
      float _inert = 0.8f;
      if (h)
        _inert = Mathf.Lerp(0.8f, 0.4f, Mathf.Clamp((float) ((double) this.fileBody.bustSoftness * (double) this.fileBody.shapeValueBody[1] + 0.00999999977648258), 0.0f, 1f));
      DynamicBone_Ver02 dynamicBoneBustAndHip1 = this.GetDynamicBoneBustAndHip(ChaControlDefine.DynamicBoneKind.BreastL);
      if (Object.op_Inequality((Object) null, (Object) dynamicBoneBustAndHip1))
      {
        dynamicBoneBustAndHip1.setSoftParamsEx(0, -1, _inert, true);
        dynamicBoneBustAndHip1.ResetPosition();
      }
      DynamicBone_Ver02 dynamicBoneBustAndHip2 = this.GetDynamicBoneBustAndHip(ChaControlDefine.DynamicBoneKind.BreastR);
      if (!Object.op_Inequality((Object) null, (Object) dynamicBoneBustAndHip2))
        return;
      dynamicBoneBustAndHip2.setSoftParamsEx(0, -1, _inert, true);
      dynamicBoneBustAndHip2.ResetPosition();
    }

    public void SetSiruFlag(ChaFileDefine.SiruParts parts, byte lv)
    {
      this.siruNewLv[(int) parts] = lv;
    }

    public byte GetSiruFlag(ChaFileDefine.SiruParts parts)
    {
      return this.chaFile.status.siruLv[(int) parts];
    }

    private bool UpdateSiru(bool forceChange = false)
    {
      if (this.sex == (byte) 0)
        return false;
      float[] numArray1 = new float[3]{ 0.0f, 0.5f, 1f };
      if (!Object.op_Equality((Object) null, (Object) this.customMatFace))
      {
        int index = 0;
        if (forceChange || (int) this.fileStatus.siruLv[index] != (int) this.siruNewLv[index])
        {
          this.fileStatus.siruLv[index] = this.siruNewLv[index];
          this.customMatFace.SetFloat(ChaShader.siruFace, numArray1[(int) this.fileStatus.siruLv[index]]);
        }
      }
      ChaFileDefine.SiruParts[] siruPartsArray = new ChaFileDefine.SiruParts[4]
      {
        ChaFileDefine.SiruParts.SiruFrontTop,
        ChaFileDefine.SiruParts.SiruFrontBot,
        ChaFileDefine.SiruParts.SiruBackTop,
        ChaFileDefine.SiruParts.SiruBackBot
      };
      List<string> stringList = new List<string>();
      bool flag = false;
      for (int index = 0; index < siruPartsArray.Length; ++index)
      {
        if (forceChange || (int) this.fileStatus.siruLv[(int) siruPartsArray[index]] != (int) this.siruNewLv[(int) siruPartsArray[index]])
          flag = true;
        if (this.siruNewLv[(int) siruPartsArray[index]] != (byte) 0)
        {
          string str = siruPartsArray[index].ToString() + this.siruNewLv[(int) siruPartsArray[index]].ToString("00");
          stringList.Add(str);
        }
        this.fileStatus.siruLv[(int) siruPartsArray[index]] = this.siruNewLv[(int) siruPartsArray[index]];
      }
      if (flag)
      {
        byte[] numArray2 = new byte[4]
        {
          this.fileStatus.siruLv[(int) siruPartsArray[0]],
          this.fileStatus.siruLv[(int) siruPartsArray[1]],
          this.fileStatus.siruLv[(int) siruPartsArray[2]],
          this.fileStatus.siruLv[(int) siruPartsArray[3]]
        };
        if (Object.op_Inequality((Object) null, (Object) this.cmpBody.targetCustom.rendBody) && 1 < this.cmpBody.targetCustom.rendBody.get_materials().Length)
        {
          this.cmpBody.targetCustom.rendBody.get_materials()[1].SetFloat(ChaShader.siruFrontTop, numArray1[(int) numArray2[0]]);
          this.cmpBody.targetCustom.rendBody.get_materials()[1].SetFloat(ChaShader.siruFrontBot, numArray1[(int) numArray2[1]]);
          this.cmpBody.targetCustom.rendBody.get_materials()[1].SetFloat(ChaShader.siruBackTop, numArray1[(int) numArray2[2]]);
          this.cmpBody.targetCustom.rendBody.get_materials()[1].SetFloat(ChaShader.siruBackBot, numArray1[(int) numArray2[3]]);
        }
        this.SetBodyBaseMaterial();
        int[] numArray3 = new int[5]{ 0, 1, 2, 3, 5 };
        for (int kind = 0; kind < numArray3.Length; ++kind)
          this.UpdateClothesSiru(kind, numArray1[(int) numArray2[0]], numArray1[(int) numArray2[1]], numArray1[(int) numArray2[2]], numArray1[(int) numArray2[3]]);
      }
      return true;
    }

    public float wetRate
    {
      get
      {
        return this.fileStatus.wetRate;
      }
      set
      {
        float num = Mathf.Clamp(value, 0.0f, 1f);
        if ((double) this.fileStatus.wetRate != (double) num)
          this.updateWet = true;
        this.fileStatus.wetRate = num;
      }
    }

    public float skinGlossRate
    {
      get
      {
        return this.fileStatus.skinTuyaRate;
      }
      set
      {
        float num = Mathf.Clamp(value, 0.0f, 1f);
        if ((double) this.fileStatus.skinTuyaRate == (double) num)
          return;
        this.fileStatus.skinTuyaRate = num;
        this.ChangeBodyGlossPower();
        this.ChangeFaceGlossPower();
      }
    }

    public void UpdateWet()
    {
      float wetRate = this.fileStatus.wetRate;
      for (int index = 0; index < this.cmpHair.Length; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) this.cmpHair[index]))
        {
          foreach (Renderer renderer in this.cmpHair[index].rendHair)
          {
            foreach (Material material in renderer.get_materials())
            {
              if (material.HasProperty(ChaShader.wetRate))
                material.SetFloat(ChaShader.wetRate, wetRate);
            }
          }
          foreach (Renderer renderer in this.cmpHair[index].rendAccessory)
          {
            foreach (Material material in renderer.get_materials())
            {
              if (material.HasProperty(ChaShader.wetRate))
                material.SetFloat(ChaShader.wetRate, wetRate);
            }
          }
        }
      }
      if (Object.op_Implicit((Object) this.customMatFace))
        this.customMatFace.SetFloat(ChaShader.wetRate, wetRate);
      if (Object.op_Implicit((Object) this.customMatBody))
        this.customMatBody.SetFloat(ChaShader.wetRate, wetRate);
      for (int index = 0; index < this.cmpClothes.Length; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) this.cmpClothes[index]))
        {
          foreach (Renderer renderer in this.cmpClothes[index].rendNormal01)
          {
            foreach (Material material in renderer.get_materials())
            {
              if (material.HasProperty(ChaShader.wetRate))
                material.SetFloat(ChaShader.wetRate, wetRate);
            }
          }
          foreach (Renderer renderer in this.cmpClothes[index].rendNormal02)
          {
            foreach (Material material in renderer.get_materials())
            {
              if (material.HasProperty(ChaShader.wetRate))
                material.SetFloat(ChaShader.wetRate, wetRate);
            }
          }
          foreach (Renderer renderer in this.cmpClothes[index].rendNormal03)
          {
            foreach (Material material in renderer.get_materials())
            {
              if (material.HasProperty(ChaShader.wetRate))
                material.SetFloat(ChaShader.wetRate, wetRate);
            }
          }
        }
      }
    }

    public void ChangeAlphaMask(params byte[] state)
    {
      if (Object.op_Implicit((Object) this.customMatBody))
      {
        if (this.customMatBody.HasProperty(ChaShader.alpha_a))
          this.customMatBody.SetFloat(ChaShader.alpha_a, (float) state[0]);
        if (this.customMatBody.HasProperty(ChaShader.alpha_b))
          this.customMatBody.SetFloat(ChaShader.alpha_b, (float) state[1]);
      }
      if (this.rendBra == null)
        return;
      for (int index = 0; index < 1; ++index)
      {
        if (Object.op_Implicit((Object) this.rendBra[index]))
        {
          Material material = this.rendBra[index].get_material();
          if (Object.op_Implicit((Object) material))
          {
            if (material.HasProperty(ChaShader.alpha_a))
              material.SetFloat(ChaShader.alpha_a, (float) state[0]);
            if (material.HasProperty(ChaShader.alpha_b))
              material.SetFloat(ChaShader.alpha_b, (float) state[1]);
          }
        }
      }
    }

    public void ChangeAlphaMaskEx()
    {
      float num = (double) this.nowCoordinate.clothes.parts[0].breakRate != 0.0 ? 1f : 0.0f;
      if (Object.op_Inequality((Object) null, (Object) this.customMatBody) && this.customMatBody.HasProperty(ChaShader.alpha_c))
        this.customMatBody.SetFloat(ChaShader.alpha_c, num);
      if (this.rendBra == null)
        return;
      for (int index = 0; index < 1; ++index)
      {
        if (Object.op_Implicit((Object) this.rendBra[index]))
        {
          Material material = this.rendBra[index].get_material();
          if (Object.op_Inequality((Object) null, (Object) material) && material.HasProperty(ChaShader.alpha_c))
            material.SetFloat(ChaShader.alpha_c, num);
        }
      }
    }

    public void ChangeAlphaMask2()
    {
      float num = 0.0f;
      if (this.underMaskReflectionType == 0)
      {
        if ((!this.underMaskBreakDisable || (double) this.nowCoordinate.clothes.parts[0].breakRate == 0.0) && this.fileStatus.clothesState[1] == (byte) 0)
          num = 1f;
      }
      else if (this.underMaskReflectionType == 1 && (!this.underMaskBreakDisable || (double) this.nowCoordinate.clothes.parts[1].breakRate == 0.0) && (this.fileStatus.clothesState[1] == (byte) 0 && !this.notBot))
        num = 1f;
      if (Object.op_Inequality((Object) null, (Object) this.customMatBody) && this.customMatBody.HasProperty(ChaShader.alpha_d))
        this.customMatBody.SetFloat(ChaShader.alpha_d, num);
      if (Object.op_Inequality((Object) null, (Object) this.rendInnerTB) && Object.op_Inequality((Object) null, (Object) this.rendInnerTB.get_material()) && this.rendInnerTB.get_material().HasProperty(ChaShader.alpha_d))
        this.rendInnerTB.get_material().SetFloat(ChaShader.alpha_d, num);
      if (Object.op_Inequality((Object) null, (Object) this.rendInnerB) && Object.op_Inequality((Object) null, (Object) this.rendInnerB.get_material()) && this.rendInnerB.get_material().HasProperty(ChaShader.alpha_d))
        this.rendInnerB.get_material().SetFloat(ChaShader.alpha_d, num);
      if (!Object.op_Inequality((Object) null, (Object) this.rendPanst) || !Object.op_Inequality((Object) null, (Object) this.rendPanst.get_material()) || !this.rendPanst.get_material().HasProperty(ChaShader.alpha_d))
        return;
      this.rendPanst.get_material().SetFloat(ChaShader.alpha_d, num);
    }

    public void ChangeSimpleBodyDraw(bool drawSimple)
    {
    }

    public void ChangeSimpleBodyColor(Color color)
    {
      if (Object.op_Equality((Object) null, (Object) this.cmpSimpleBody) || Color.op_Equality(this.fileStatus.simpleColor, color))
        return;
      this.fileStatus.simpleColor = color;
      if (Object.op_Implicit((Object) this.cmpSimpleBody.targetCustom.rendBody))
      {
        Material material = this.cmpSimpleBody.targetCustom.rendBody.get_material();
        if (Object.op_Implicit((Object) material))
          material.SetColor(ChaShader.Color, color);
      }
      if (!Object.op_Implicit((Object) this.cmpSimpleBody.targetEtc.rendTongue))
        return;
      Material material1 = this.cmpSimpleBody.targetEtc.rendTongue.get_material();
      if (!Object.op_Implicit((Object) material1))
        return;
      material1.SetColor(ChaShader.Color, color);
    }

    private void UpdateVisible()
    {
      this.confSon = true;
      this.confBody = true;
      this.drawSimple = false;
      if (Singleton<Manager.Config>.IsInstance() && Singleton<HSceneManager>.IsInstance() && HSceneManager.isHScene)
      {
        this.confSon = this.sex != (byte) 0 && (this.sex != (byte) 1 || !this.isPlayer) || Manager.Config.HData.Son;
        this.confBody = this.sex != (byte) 0 && (this.sex != (byte) 1 || !this.isPlayer) || Manager.Config.HData.Visible;
        this.drawSimple = (this.sex == (byte) 0 || this.sex == (byte) 1 && this.isPlayer) && Manager.Config.GraphicData.SimpleBody;
        this.fileStatus.visibleSimple = this.drawSimple;
        this.ChangeSimpleBodyColor(Manager.Config.GraphicData.SilhouetteColor);
      }
      if (Object.op_Implicit((Object) this.cmpBody))
      {
        if (Object.op_Implicit((Object) this.cmpBody.targetEtc.objTongue))
        {
          this.lstActive.Clear();
          this.lstActive.Add(this.visibleAll);
          this.lstActive.Add(this.fileStatus.tongueState == (byte) 1);
          this.lstActive.Add(this.confBody);
          this.lstActive.Add(!this.drawSimple);
          this.lstActive.Add(this.fileStatus.visibleHeadAlways);
          this.lstActive.Add(this.fileStatus.visibleBodyAlways);
          YS_Assist.SetActiveControl(this.cmpBody.targetEtc.objTongue, this.lstActive);
        }
        if (Object.op_Implicit((Object) this.cmpBody.targetEtc.objMNPB))
          YS_Assist.SetActiveControl(this.cmpBody.targetEtc.objMNPB, !this.hideMoz);
        if (Object.op_Implicit((Object) this.cmpBody.targetEtc.objBody))
        {
          this.lstActive.Clear();
          this.lstActive.Add(this.visibleAll);
          this.lstActive.Add(this.confBody);
          this.lstActive.Add(!this.drawSimple);
          this.lstActive.Add(this.fileStatus.visibleBodyAlways);
          YS_Assist.SetActiveControl(this.cmpBody.targetEtc.objBody, this.lstActive);
        }
        if (Object.op_Implicit((Object) this.cmpBody.targetEtc.objDanTop))
        {
          this.lstActive.Clear();
          this.lstActive.Add(this.visibleAll);
          this.lstActive.Add(this.drawSimple || !this.IsClothesStateKind(1) || this.fileStatus.clothesState[1] != (byte) 0 || this.fileStatus.visibleSon);
          this.lstActive.Add(this.confSon);
          this.lstActive.Add(this.fileStatus.visibleSonAlways);
          YS_Assist.SetActiveControl(this.cmpBody.targetEtc.objDanTop, this.lstActive);
        }
      }
      if (Object.op_Implicit((Object) this.cmpSimpleBody))
      {
        if (Object.op_Implicit((Object) this.cmpSimpleBody.targetEtc.objTongue))
        {
          this.lstActive.Clear();
          this.lstActive.Add(this.visibleAll);
          this.lstActive.Add(this.fileStatus.tongueState == (byte) 1);
          this.lstActive.Add(this.confBody);
          this.lstActive.Add(this.drawSimple);
          this.lstActive.Add(this.fileStatus.visibleHeadAlways);
          this.lstActive.Add(this.fileStatus.visibleBodyAlways);
          YS_Assist.SetActiveControl(this.cmpSimpleBody.targetEtc.objTongue, this.lstActive);
        }
        if (Object.op_Implicit((Object) this.cmpSimpleBody.targetEtc.objMNPB))
          YS_Assist.SetActiveControl(this.cmpSimpleBody.targetEtc.objMNPB, !this.hideMoz);
        if (Object.op_Implicit((Object) this.cmpSimpleBody.targetEtc.objBody))
        {
          this.lstActive.Clear();
          this.lstActive.Add(this.visibleAll);
          this.lstActive.Add(this.confBody);
          this.lstActive.Add(this.drawSimple);
          this.lstActive.Add(this.fileStatus.visibleBodyAlways);
          YS_Assist.SetActiveControl(this.cmpSimpleBody.targetEtc.objBody, this.lstActive);
        }
        if (Object.op_Implicit((Object) this.cmpSimpleBody.targetEtc.objDanTop))
        {
          this.lstActive.Clear();
          this.lstActive.Add(this.visibleAll);
          this.lstActive.Add(this.drawSimple && this.fileStatus.visibleSon);
          this.lstActive.Add(this.confSon);
          this.lstActive.Add(this.fileStatus.visibleSonAlways);
          YS_Assist.SetActiveControl(this.cmpSimpleBody.targetEtc.objDanTop, this.lstActive);
        }
      }
      if (Object.op_Implicit((Object) this.cmpFace))
      {
        this.lstActive.Clear();
        this.lstActive.Add(this.visibleAll);
        this.lstActive.Add(this.confBody);
        this.lstActive.Add(!this.drawSimple);
        this.lstActive.Add(this.fileStatus.visibleHeadAlways);
        this.lstActive.Add(this.fileStatus.visibleBodyAlways);
        YS_Assist.SetActiveControl(this.objHead, this.lstActive);
        if (Object.op_Implicit((Object) this.cmpFace.targetEtc.objTongue))
          YS_Assist.SetActiveControl(this.cmpFace.targetEtc.objTongue, this.fileStatus.tongueState == (byte) 0);
      }
      if (Object.op_Implicit((Object) this.cmpClothes[0]))
      {
        this.lstActive.Clear();
        this.lstActive.Add(this.visibleAll);
        this.lstActive.Add(this.confBody);
        this.lstActive.Add(!this.drawSimple);
        this.lstActive.Add((byte) 2 != this.fileStatus.clothesState[0]);
        this.lstActive.Add(this.fileStatus.visibleBodyAlways);
        YS_Assist.SetActiveControl(this.objClothes[0], this.lstActive);
      }
      bool flag1 = false;
      bool flag2 = false;
      if (Object.op_Implicit((Object) this.cmpClothes[0]))
      {
        if (YS_Assist.SetActiveControl(this.cmpClothes[0].objTopDef, this.fileStatus.clothesState[0] == (byte) 0))
          flag1 = true;
        if (YS_Assist.SetActiveControl(this.cmpClothes[0].objTopHalf, this.fileStatus.clothesState[0] == (byte) 1))
          flag1 = true;
        this.lstActive.Clear();
        this.lstActive.Add(this.fileStatus.clothesState[1] == (byte) 0);
        this.lstActive.Add(this.fileStatus.clothesState[0] != (byte) 2);
        if (YS_Assist.SetActiveControl(this.cmpClothes[0].objBotDef, this.lstActive))
          flag2 = true;
        this.lstActive.Clear();
        this.lstActive.Add(this.fileStatus.clothesState[1] != (byte) 0);
        this.lstActive.Add(this.fileStatus.clothesState[0] != (byte) 2);
        if (YS_Assist.SetActiveControl(this.cmpClothes[0].objBotHalf, this.lstActive))
          flag2 = true;
      }
      this.DrawOption(ChaFileDefine.ClothesKind.top);
      if (flag1 || this.updateAlphaMask)
      {
        byte num = this.fileStatus.clothesState[0];
        byte[,] numArray = new byte[3, 2]
        {
          {
            (byte) 1,
            (byte) 1
          },
          {
            (byte) 0,
            (byte) 1
          },
          {
            (byte) 0,
            (byte) 0
          }
        };
        this.ChangeAlphaMask(numArray[(int) num, 0], numArray[(int) num, 1]);
        this.updateAlphaMask = false;
      }
      bool flag3 = false;
      if (Object.op_Implicit((Object) this.cmpClothes[1]))
      {
        this.lstActive.Clear();
        this.lstActive.Add(this.visibleAll);
        this.lstActive.Add(!this.notBot);
        this.lstActive.Add(this.confBody);
        this.lstActive.Add(!this.drawSimple);
        this.lstActive.Add((byte) 2 != this.fileStatus.clothesState[1]);
        this.lstActive.Add(this.fileStatus.visibleBodyAlways);
        if (YS_Assist.SetActiveControl(this.objClothes[1], this.lstActive))
          flag3 = true;
      }
      if (Object.op_Implicit((Object) this.cmpClothes[1]))
      {
        if (YS_Assist.SetActiveControl(this.cmpClothes[1].objBotDef, this.fileStatus.clothesState[1] == (byte) 0))
          flag3 = true;
        if (YS_Assist.SetActiveControl(this.cmpClothes[1].objBotHalf, this.fileStatus.clothesState[1] == (byte) 1))
          flag3 = true;
        this.lstActive.Clear();
        this.lstActive.Add(this.fileStatus.clothesState[0] == (byte) 0);
        this.lstActive.Add(this.fileStatus.clothesState[1] != (byte) 2);
        YS_Assist.SetActiveControl(this.cmpClothes[1].objTopDef, this.lstActive);
        this.lstActive.Clear();
        this.lstActive.Add(this.fileStatus.clothesState[0] != (byte) 0);
        this.lstActive.Add(this.fileStatus.clothesState[1] != (byte) 2);
        YS_Assist.SetActiveControl(this.cmpClothes[1].objTopHalf, this.lstActive);
      }
      if (flag2 || flag3 || this.updateAlphaMask2)
      {
        this.ChangeAlphaMask2();
        this.updateAlphaMask2 = false;
      }
      this.DrawOption(ChaFileDefine.ClothesKind.bot);
      if (Object.op_Implicit((Object) this.cmpClothes[2]))
      {
        this.lstActive.Clear();
        this.lstActive.Add(this.visibleAll);
        this.lstActive.Add(this.confBody);
        this.lstActive.Add(!this.drawSimple);
        this.lstActive.Add(!this.notInnerT);
        this.lstActive.Add((byte) 2 != this.fileStatus.clothesState[2]);
        this.lstActive.Add(this.fileStatus.visibleBodyAlways);
        YS_Assist.SetActiveControl(this.objClothes[2], this.lstActive);
      }
      if (Object.op_Implicit((Object) this.cmpClothes[2]))
      {
        YS_Assist.SetActiveControl(this.cmpClothes[2].objTopDef, this.fileStatus.clothesState[2] == (byte) 0);
        YS_Assist.SetActiveControl(this.cmpClothes[2].objTopHalf, this.fileStatus.clothesState[2] == (byte) 1);
        this.lstActive.Clear();
        this.lstActive.Add(this.fileStatus.clothesState[3] == (byte) 0);
        this.lstActive.Add(this.fileStatus.clothesState[2] != (byte) 2);
        YS_Assist.SetActiveControl(this.cmpClothes[2].objBotDef, this.lstActive);
        this.lstActive.Clear();
        this.lstActive.Add(this.fileStatus.clothesState[3] != (byte) 0);
        this.lstActive.Add(this.fileStatus.clothesState[2] != (byte) 2);
        YS_Assist.SetActiveControl(this.cmpClothes[2].objBotHalf, this.lstActive);
      }
      this.DrawOption(ChaFileDefine.ClothesKind.inner_t);
      bool flag4 = true;
      if (Object.op_Implicit((Object) this.cmpClothes[3]))
      {
        this.lstActive.Clear();
        this.lstActive.Add(this.visibleAll);
        this.lstActive.Add(flag4);
        this.lstActive.Add(!this.notInnerB);
        this.lstActive.Add(this.confBody);
        this.lstActive.Add(!this.drawSimple);
        this.lstActive.Add((byte) 2 != this.fileStatus.clothesState[3]);
        this.lstActive.Add(this.fileStatus.visibleBodyAlways);
        YS_Assist.SetActiveControl(this.objClothes[3], this.lstActive);
      }
      if (Object.op_Implicit((Object) this.cmpClothes[3]))
      {
        YS_Assist.SetActiveControl(this.cmpClothes[3].objBotDef, this.fileStatus.clothesState[3] == (byte) 0);
        YS_Assist.SetActiveControl(this.cmpClothes[3].objBotHalf, this.fileStatus.clothesState[3] == (byte) 1);
      }
      this.DrawOption(ChaFileDefine.ClothesKind.inner_b);
      if (Object.op_Implicit((Object) this.cmpClothes[4]))
      {
        this.lstActive.Clear();
        this.lstActive.Add(this.visibleAll);
        this.lstActive.Add(this.fileStatus.clothesState[4] == (byte) 0);
        this.lstActive.Add(this.confBody);
        this.lstActive.Add(!this.drawSimple);
        this.lstActive.Add(this.fileStatus.visibleBodyAlways);
        YS_Assist.SetActiveControl(this.objClothes[4], this.lstActive);
      }
      this.DrawOption(ChaFileDefine.ClothesKind.gloves);
      if (Object.op_Implicit((Object) this.cmpClothes[5]))
      {
        this.lstActive.Clear();
        this.lstActive.Add(this.visibleAll);
        this.lstActive.Add(this.fileStatus.clothesState[5] != (byte) 2);
        this.lstActive.Add(this.confBody);
        this.lstActive.Add(!this.drawSimple);
        this.lstActive.Add(this.fileStatus.visibleBodyAlways);
        YS_Assist.SetActiveControl(this.objClothes[5], this.lstActive);
      }
      if (Object.op_Implicit((Object) this.cmpClothes[5]))
      {
        YS_Assist.SetActiveControl(this.cmpClothes[5].objBotDef, this.fileStatus.clothesState[5] == (byte) 0);
        YS_Assist.SetActiveControl(this.cmpClothes[5].objBotHalf, this.fileStatus.clothesState[5] == (byte) 1);
      }
      this.DrawOption(ChaFileDefine.ClothesKind.panst);
      if (Object.op_Implicit((Object) this.cmpClothes[6]))
      {
        this.lstActive.Clear();
        this.lstActive.Add(this.visibleAll);
        this.lstActive.Add(this.fileStatus.clothesState[6] == (byte) 0);
        this.lstActive.Add(this.confBody);
        this.lstActive.Add(!this.drawSimple);
        this.lstActive.Add(this.fileStatus.visibleBodyAlways);
        YS_Assist.SetActiveControl(this.objClothes[6], this.lstActive);
      }
      this.DrawOption(ChaFileDefine.ClothesKind.socks);
      if (Object.op_Implicit((Object) this.cmpClothes[7]))
      {
        this.lstActive.Clear();
        this.lstActive.Add(this.visibleAll);
        this.lstActive.Add(this.fileStatus.clothesState[7] == (byte) 0);
        this.lstActive.Add(this.confBody);
        this.lstActive.Add(!this.drawSimple);
        this.lstActive.Add(this.fileStatus.visibleBodyAlways);
        YS_Assist.SetActiveControl(this.objClothes[7], this.lstActive);
      }
      this.DrawOption(ChaFileDefine.ClothesKind.shoes);
      for (int index = 0; index < this.objAccessory.Length; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) this.objAccessory[index]))
        {
          bool flag5 = false;
          if (!this.fileStatus.visibleHeadAlways && this.nowCoordinate.accessory.parts[index].partsOfHead)
            flag5 = true;
          if (!this.fileStatus.visibleBodyAlways || !this.confBody)
            flag5 = true;
          this.lstActive.Clear();
          this.lstActive.Add(this.visibleAll);
          this.lstActive.Add(this.fileStatus.showAccessory[index]);
          this.lstActive.Add(!this.drawSimple);
          this.lstActive.Add(!flag5);
          YS_Assist.SetActiveControl(this.objAccessory[index], this.lstActive);
        }
      }
      for (int index = 0; index < this.objExtraAccessory.Length; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) this.objExtraAccessory[index]))
        {
          this.lstActive.Clear();
          this.lstActive.Add(this.visibleAll);
          this.lstActive.Add(this.showExtraAccessory[index]);
          this.lstActive.Add(!this.drawSimple);
          YS_Assist.SetActiveControl(this.objExtraAccessory[index], this.lstActive);
        }
      }
      foreach (GameObject gameObject in this.objHair)
      {
        this.lstActive.Clear();
        this.lstActive.Add(this.visibleAll);
        this.lstActive.Add(this.confBody);
        this.lstActive.Add(!this.drawSimple);
        this.lstActive.Add(this.fileStatus.visibleHeadAlways);
        this.lstActive.Add(this.fileStatus.visibleBodyAlways);
        YS_Assist.SetActiveControl(gameObject, this.lstActive);
      }
    }

    public void DrawOption(ChaFileDefine.ClothesKind kind)
    {
      CmpClothes cmpClothe = this.cmpClothes[(int) kind];
      if (Object.op_Equality((Object) null, (Object) cmpClothe))
        return;
      if (cmpClothe.objOpt01 != null)
      {
        foreach (GameObject gameObject in cmpClothe.objOpt01)
          YS_Assist.SetActiveControl(gameObject, !this.nowCoordinate.clothes.parts[(int) kind].hideOpt[0]);
      }
      if (cmpClothe.objOpt02 == null)
        return;
      foreach (GameObject gameObject in cmpClothe.objOpt02)
        YS_Assist.SetActiveControl(gameObject, !this.nowCoordinate.clothes.parts[(int) kind].hideOpt[1]);
    }

    public ChaFileCoordinate nowCoordinate { get; private set; }

    public bool notInnerT { get; private set; }

    public bool notBot { get; private set; }

    public bool notInnerB { get; private set; }

    public Dictionary<int, Dictionary<byte, string>> dictStateType { get; private set; }

    public bool AssignCoordinate(string path)
    {
      string path1 = ChaFileControl.ConvertCoordinateFilePath(path, this.sex);
      ChaFileCoordinate srcCoorde = new ChaFileCoordinate();
      return srcCoorde.LoadFile(path1) && this.AssignCoordinate(srcCoorde);
    }

    public bool AssignCoordinate(ChaFileCoordinate srcCoorde)
    {
      return this.chaFile.coordinate.LoadBytes(srcCoorde.SaveBytes(), srcCoorde.loadVersion);
    }

    public bool AssignCoordinate()
    {
      return this.chaFile.coordinate.LoadBytes(this.nowCoordinate.SaveBytes(), this.nowCoordinate.loadVersion);
    }

    public bool ChangeNowCoordinate(bool reload = false, bool forceChange = true)
    {
      return this.ChangeNowCoordinate(this.chaFile.coordinate, reload, forceChange);
    }

    public bool ChangeNowCoordinate(string path, bool reload = false, bool forceChange = true)
    {
      string path1 = ChaFileControl.ConvertCoordinateFilePath(path, this.sex);
      ChaFileCoordinate srcCoorde = new ChaFileCoordinate();
      return srcCoorde.LoadFile(path1) && this.ChangeNowCoordinate(srcCoorde, reload, forceChange);
    }

    public bool ChangeNowCoordinate(ChaFileCoordinate srcCoorde, bool reload = false, bool forceChange = true)
    {
      if (!this.nowCoordinate.LoadBytes(srcCoorde.SaveBytes(), srcCoorde.loadVersion))
        return false;
      return !reload || this.Reload(false, true, true, true, forceChange);
    }

    protected void InitializeControlCoordinateAll()
    {
      this.nowCoordinate = new ChaFileCoordinate();
      this.InitializeControlCoordinateObject();
    }

    protected void InitializeControlCoordinateObject()
    {
      this.notInnerT = false;
      this.notBot = false;
      this.notInnerB = false;
      this.dictStateType = new Dictionary<int, Dictionary<byte, string>>();
    }

    protected void ReleaseControlCoordinateAll()
    {
      this.ReleaseControlCoordinateObject(false);
    }

    protected void ReleaseControlCoordinateObject(bool init = true)
    {
      if (!init)
        return;
      this.InitializeControlCoordinateObject();
    }

    protected void ReleaseBaseCustomTextureClothes(int parts, bool createTex = true)
    {
      for (int index = 0; index < 3; ++index)
      {
        if (this.ctCreateClothes[parts, index] != null)
        {
          if (createTex)
            this.ctCreateClothes[parts, index].Release();
          else
            this.ctCreateClothes[parts, index].ReleaseCreateMaterial();
          this.ctCreateClothes[parts, index] = (CustomTextureCreate) null;
        }
        if (this.ctCreateClothesGloss[parts, index] != null)
        {
          if (createTex)
            this.ctCreateClothesGloss[parts, index].Release();
          else
            this.ctCreateClothesGloss[parts, index].ReleaseCreateMaterial();
          this.ctCreateClothesGloss[parts, index] = (CustomTextureCreate) null;
        }
      }
    }

    protected bool InitBaseCustomTextureClothes(int parts)
    {
      if (this.infoClothes == null)
        return false;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      string empty5 = string.Empty;
      string empty6 = string.Empty;
      ListInfoBase infoClothe = this.infoClothes[parts];
      string info1 = infoClothe.GetInfo(ChaListDefine.KeyType.MainManifest);
      string info2 = infoClothe.GetInfo(ChaListDefine.KeyType.MainAB);
      string info3 = infoClothe.GetInfo(ChaListDefine.KeyType.MainTex);
      string info4 = infoClothe.GetInfo(ChaListDefine.KeyType.MainTex02);
      string info5 = infoClothe.GetInfo(ChaListDefine.KeyType.MainTex03);
      string info6 = infoClothe.GetInfo(ChaListDefine.KeyType.ColorMaskTex);
      string info7 = infoClothe.GetInfo(ChaListDefine.KeyType.ColorMask02Tex);
      string info8 = infoClothe.GetInfo(ChaListDefine.KeyType.ColorMask03Tex);
      if ("0" == info3)
        return false;
      Texture2D texture2D1 = CommonLib.LoadAsset<Texture2D>(info2, info3, false, info1);
      if (Object.op_Equality((Object) null, (Object) texture2D1))
        return false;
      if ("0" == info6)
      {
        Resources.UnloadAsset((Object) texture2D1);
        return false;
      }
      Texture2D texture2D2 = CommonLib.LoadAsset<Texture2D>(info2, info6, false, info1);
      if (Object.op_Equality((Object) null, (Object) texture2D2))
      {
        Resources.UnloadAsset((Object) texture2D1);
        return false;
      }
      Texture2D texture2D3 = (Texture2D) null;
      if ("0" != info4)
        texture2D3 = CommonLib.LoadAsset<Texture2D>(info2, info4, false, info1);
      Texture2D texture2D4 = (Texture2D) null;
      if ("0" != info7)
        texture2D4 = CommonLib.LoadAsset<Texture2D>(info2, info7, false, info1);
      Texture2D texture2D5 = (Texture2D) null;
      if ("0" != info5)
        texture2D5 = CommonLib.LoadAsset<Texture2D>(info2, info5, false, info1);
      Texture2D texture2D6 = (Texture2D) null;
      if ("0" != info8)
        texture2D6 = CommonLib.LoadAsset<Texture2D>(info2, info8, false, info1);
      Texture2D[] texture2DArray1 = new Texture2D[3]
      {
        texture2D1,
        texture2D3,
        texture2D5
      };
      Texture2D[] texture2DArray2 = new Texture2D[3]
      {
        texture2D2,
        texture2D4,
        texture2D6
      };
      for (int index = 0; index < 3; ++index)
      {
        this.ctCreateClothes[parts, index] = (CustomTextureCreate) null;
        this.ctCreateClothesGloss[parts, index] = (CustomTextureCreate) null;
        CustomTextureCreate customTextureCreate1 = (CustomTextureCreate) null;
        int width = 0;
        int height = 0;
        if (Object.op_Inequality((Object) null, (Object) texture2DArray1[index]))
        {
          CustomTextureCreate customTextureCreate2 = new CustomTextureCreate(this.objRoot.get_transform());
          width = ((Texture) texture2DArray1[index]).get_width();
          height = ((Texture) texture2DArray1[index]).get_height();
          customTextureCreate2.Initialize("abdata", "chara/mm_base.unity3d", "create_clothes", width, height, (RenderTextureFormat) 0);
          customTextureCreate2.SetMainTexture((Texture) texture2DArray1[index]);
          customTextureCreate2.SetTexture(ChaShader.ColorMask, (Texture) texture2DArray2[index]);
          this.ctCreateClothes[parts, index] = customTextureCreate2;
        }
        customTextureCreate1 = (CustomTextureCreate) null;
        if (Object.op_Inequality((Object) null, (Object) texture2DArray1[index]))
        {
          CustomTextureCreate customTextureCreate2 = new CustomTextureCreate(this.objRoot.get_transform());
          customTextureCreate2.Initialize("abdata", "chara/mm_base.unity3d", "create_clothes detail", width, height, (RenderTextureFormat) 0);
          RenderTexture active = RenderTexture.get_active();
          RenderTexture renderTexture = new RenderTexture(width, height, 0, (RenderTextureFormat) 0);
          bool sRgbWrite = GL.get_sRGBWrite();
          GL.set_sRGBWrite(true);
          Graphics.SetRenderTarget(renderTexture);
          GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 1f));
          Graphics.SetRenderTarget((RenderTexture) null);
          GL.set_sRGBWrite(sRgbWrite);
          Texture2D texture2D7 = new Texture2D(width, height, (TextureFormat) 5, false);
          RenderTexture.set_active(renderTexture);
          texture2D7.ReadPixels(new Rect(0.0f, 0.0f, (float) width, (float) height), 0, 0);
          texture2D7.Apply();
          RenderTexture.set_active(active);
          Object.Destroy((Object) renderTexture);
          customTextureCreate2.SetMainTexture((Texture) texture2D7);
          customTextureCreate2.SetTexture(ChaShader.ColorMask, (Texture) texture2DArray2[index]);
          this.ctCreateClothesGloss[parts, index] = customTextureCreate2;
        }
      }
      return true;
    }

    public bool AddClothesStateKind(int clothesKind, string stateType)
    {
      ChaFileDefine.ClothesKind clothesKind1 = (ChaFileDefine.ClothesKind) Enum.ToObject(typeof (ChaFileDefine.ClothesKind), clothesKind);
      switch (clothesKind1)
      {
        case ChaFileDefine.ClothesKind.top:
        case ChaFileDefine.ClothesKind.bot:
          this.dictStateType.Remove(0);
          this.dictStateType.Remove(1);
          ListInfoBase infoClothe1 = this.infoClothes[0];
          byte type1 = 3;
          if (infoClothe1 != null)
            type1 = byte.Parse(infoClothe1.GetInfo(ChaListDefine.KeyType.StateType));
          byte type2 = 3;
          ListInfoBase infoClothe2 = this.infoClothes[1];
          if (infoClothe2 != null)
            type2 = byte.Parse(infoClothe2.GetInfo(ChaListDefine.KeyType.StateType));
          if (type2 != (byte) 0)
          {
            int index = 0;
            if (this.cmpClothes != null && Object.op_Inequality((Object) null, (Object) this.cmpClothes[index]) && Object.op_Inequality((Object) null, (Object) this.cmpClothes[index].objBotDef))
            {
              if (type1 == (byte) 0)
                type2 = (byte) 0;
              else if (type1 == (byte) 1 && type2 == (byte) 2)
                type2 = (byte) 1;
            }
          }
          if (type1 != (byte) 0)
          {
            int index = 1;
            if (this.cmpClothes != null && Object.op_Implicit((Object) this.cmpClothes[index]) && Object.op_Inequality((Object) null, (Object) this.cmpClothes[index].objTopDef))
            {
              switch (type2)
              {
                case 0:
                  type1 = (byte) 0;
                  break;
                case 1:
                  if (type1 == (byte) 2)
                  {
                    type1 = (byte) 1;
                    break;
                  }
                  break;
              }
            }
          }
          this.AddClothesStateKindSub(0, type1);
          this.AddClothesStateKindSub(1, type2);
          if (clothesKind1 == ChaFileDefine.ClothesKind.top)
          {
            this.AddClothesStateKind(2, string.Empty);
            break;
          }
          break;
        case ChaFileDefine.ClothesKind.inner_t:
        case ChaFileDefine.ClothesKind.inner_b:
          this.dictStateType.Remove(2);
          this.dictStateType.Remove(3);
          byte type3 = 3;
          if (!this.notInnerT)
          {
            ListInfoBase infoClothe3 = this.infoClothes[2];
            if (infoClothe3 != null)
              type3 = byte.Parse(infoClothe3.GetInfo(ChaListDefine.KeyType.StateType));
          }
          byte type4 = 3;
          if (!this.notInnerT || !this.notInnerB)
          {
            ListInfoBase infoClothe3 = this.infoClothes[3];
            if (infoClothe3 != null)
              type4 = byte.Parse(infoClothe3.GetInfo(ChaListDefine.KeyType.StateType));
          }
          if (type4 != (byte) 0)
          {
            int index = 2;
            if (this.cmpClothes != null && Object.op_Inequality((Object) null, (Object) this.cmpClothes[index]) && Object.op_Inequality((Object) null, (Object) this.cmpClothes[index].objBotDef))
            {
              if (type3 == (byte) 0)
                type4 = (byte) 0;
              else if (type3 == (byte) 1 && type4 == (byte) 2)
                type4 = (byte) 1;
            }
          }
          if (type3 != (byte) 0 && type3 != (byte) 2)
          {
            int index = 3;
            if (this.cmpClothes != null && Object.op_Inequality((Object) null, (Object) this.cmpClothes[index]) && Object.op_Inequality((Object) null, (Object) this.cmpClothes[index].objBotDef))
            {
              switch (type4)
              {
                case 0:
                  type3 = (byte) 0;
                  break;
                case 1:
                  if (type3 == (byte) 2)
                  {
                    type3 = (byte) 1;
                    break;
                  }
                  break;
              }
            }
          }
          this.AddClothesStateKindSub(2, type3);
          this.AddClothesStateKindSub(3, type4);
          break;
        case ChaFileDefine.ClothesKind.gloves:
          this.dictStateType.Remove(4);
          this.AddClothesStateKindSub(4, byte.Parse(stateType));
          break;
        case ChaFileDefine.ClothesKind.panst:
          this.dictStateType.Remove(5);
          this.AddClothesStateKindSub(5, byte.Parse(stateType));
          break;
        case ChaFileDefine.ClothesKind.socks:
          this.dictStateType.Remove(6);
          this.AddClothesStateKindSub(6, byte.Parse(stateType));
          break;
        case ChaFileDefine.ClothesKind.shoes:
          this.dictStateType.Remove(7);
          this.AddClothesStateKindSub(7, byte.Parse(stateType));
          break;
      }
      return true;
    }

    private bool AddClothesStateKindSub(int clothesKind, byte type)
    {
      if (!MathfEx.RangeEqualOn<int>(0, (int) type, 1))
        return false;
      Dictionary<byte, string> dictionary = new Dictionary<byte, string>();
      if (type == (byte) 0)
      {
        dictionary[(byte) 0] = "着衣";
        dictionary[(byte) 1] = "半脱";
        dictionary[(byte) 2] = "脱衣";
      }
      else
      {
        dictionary[(byte) 0] = "着衣";
        dictionary[(byte) 2] = "脱衣";
      }
      this.dictStateType[clothesKind] = dictionary;
      return true;
    }

    public void RemoveClothesStateKind(int clothesKind)
    {
      switch ((ChaFileDefine.ClothesKind) Enum.ToObject(typeof (ChaFileDefine.ClothesKind), clothesKind))
      {
        case ChaFileDefine.ClothesKind.top:
        case ChaFileDefine.ClothesKind.bot:
          this.AddClothesStateKind(0, string.Empty);
          break;
        case ChaFileDefine.ClothesKind.inner_t:
        case ChaFileDefine.ClothesKind.inner_b:
          this.AddClothesStateKind(2, string.Empty);
          break;
        case ChaFileDefine.ClothesKind.gloves:
          this.dictStateType.Remove(4);
          break;
        case ChaFileDefine.ClothesKind.panst:
          this.dictStateType.Remove(5);
          break;
        case ChaFileDefine.ClothesKind.socks:
          this.dictStateType.Remove(6);
          break;
        case ChaFileDefine.ClothesKind.shoes:
          this.dictStateType.Remove(7);
          break;
      }
    }

    public bool IsClothes(int clothesKind)
    {
      return this.IsClothesStateKind(clothesKind) && !Object.op_Equality((Object) null, (Object) this.objClothes[clothesKind]) && this.infoClothes[clothesKind] != null;
    }

    public bool IsClothesStateKind(int clothesKind)
    {
      return this.dictStateType.ContainsKey(clothesKind);
    }

    public Dictionary<byte, string> GetClothesStateKind(int clothesKind)
    {
      Dictionary<byte, string> dictionary = (Dictionary<byte, string>) null;
      this.dictStateType.TryGetValue(clothesKind, out dictionary);
      return dictionary;
    }

    public bool IsClothesStateType(int clothesKind, byte stateType)
    {
      Dictionary<byte, string> dictionary = (Dictionary<byte, string>) null;
      this.dictStateType.TryGetValue(clothesKind, out dictionary);
      return dictionary != null && dictionary.ContainsKey(stateType);
    }

    public bool IsBareFoot
    {
      get
      {
        return !this.IsClothes(7) || (byte) 0 != this.fileStatus.clothesState[7];
      }
    }

    public void SetClothesState(int clothesKind, byte state, bool next = true)
    {
      if (next)
      {
        byte num = this.fileStatus.clothesState[clothesKind];
        while (this.IsClothesStateKind(clothesKind))
        {
          if (this.IsClothesStateType(clothesKind, state))
          {
            this.fileStatus.clothesState[clothesKind] = state;
            goto label_16;
          }
          else
          {
            state = (byte) (((int) state + 1) % 3);
            if ((int) num == (int) state)
              goto label_16;
          }
        }
        this.fileStatus.clothesState[clothesKind] = state;
      }
      else
      {
        byte num = this.fileStatus.clothesState[clothesKind];
        while (this.IsClothesStateKind(clothesKind))
        {
          if (this.IsClothesStateType(clothesKind, state))
          {
            this.fileStatus.clothesState[clothesKind] = state;
            goto label_16;
          }
          else
          {
            state = (byte) (((int) state + 2) % 3);
            if ((int) num == (int) state)
              goto label_16;
          }
        }
        this.fileStatus.clothesState[clothesKind] = state;
      }
label_16:
      switch (clothesKind)
      {
        case 0:
          if (!this.notBot)
            break;
          if (this.fileStatus.clothesState[clothesKind] == (byte) 2)
          {
            this.fileStatus.clothesState[1] = (byte) 2;
            break;
          }
          if (this.fileStatus.clothesState[1] != (byte) 2)
            break;
          this.fileStatus.clothesState[1] = state;
          break;
        case 1:
          if (!this.notBot)
            break;
          if (this.fileStatus.clothesState[clothesKind] == (byte) 2)
          {
            this.fileStatus.clothesState[0] = (byte) 2;
            break;
          }
          if (this.fileStatus.clothesState[0] != (byte) 2)
            break;
          this.fileStatus.clothesState[0] = state;
          break;
        case 2:
          if (!this.notInnerB)
            break;
          if (this.fileStatus.clothesState[clothesKind] == (byte) 2)
          {
            this.fileStatus.clothesState[3] = (byte) 2;
            break;
          }
          if (this.fileStatus.clothesState[3] != (byte) 2)
            break;
          this.fileStatus.clothesState[3] = state;
          break;
        case 3:
          if (!this.notInnerB)
            break;
          if (this.fileStatus.clothesState[clothesKind] == (byte) 2)
          {
            this.fileStatus.clothesState[2] = (byte) 2;
            break;
          }
          if (this.fileStatus.clothesState[2] != (byte) 2)
            break;
          this.fileStatus.clothesState[2] = state;
          break;
      }
    }

    public void SetClothesStatePrev(int clothesKind)
    {
      byte state = (byte) (((int) this.fileStatus.clothesState[clothesKind] + 2) % 3);
      this.SetClothesState(clothesKind, state, false);
    }

    public void SetClothesStateNext(int clothesKind)
    {
      byte state = (byte) (((int) this.fileStatus.clothesState[clothesKind] + 1) % 3);
      this.SetClothesState(clothesKind, state, true);
    }

    public void SetClothesStateAll(byte state)
    {
      int length = Enum.GetNames(typeof (ChaFileDefine.ClothesKind)).Length;
      for (int clothesKind = 0; clothesKind < length; ++clothesKind)
        this.SetClothesState(clothesKind, state, true);
    }

    public void UpdateClothesStateAll()
    {
      int length = Enum.GetNames(typeof (ChaFileDefine.ClothesKind)).Length;
      for (int clothesKind = 0; clothesKind < length; ++clothesKind)
        this.SetClothesState(clothesKind, this.fileStatus.clothesState[clothesKind], true);
    }

    public int GetNowClothesType()
    {
      int clothesKind1 = 0;
      int clothesKind2 = 1;
      int clothesKind3 = 2;
      int clothesKind4 = 3;
      int num1 = !this.IsClothesStateKind(clothesKind1) ? 2 : (int) this.fileStatus.clothesState[clothesKind1];
      int num2 = !this.IsClothesStateKind(clothesKind2) ? 2 : (int) this.fileStatus.clothesState[clothesKind2];
      int num3 = !this.IsClothesStateKind(clothesKind3) ? 2 : (int) this.fileStatus.clothesState[clothesKind3];
      int num4 = !this.IsClothesStateKind(clothesKind4) ? 2 : (int) this.fileStatus.clothesState[clothesKind4];
      bool flag1 = true;
      bool flag2 = true;
      if (this.infoClothes[clothesKind3] != null)
        flag1 = this.infoClothes[clothesKind3].Kind == 1;
      if (this.infoClothes[clothesKind4] != null)
        flag2 = this.infoClothes[clothesKind4].Kind == 1;
      if (this.notInnerB)
      {
        num4 = num3;
        flag2 = flag1;
      }
      if (num1 == 0)
      {
        if (num2 == 0)
          return 0;
        if (num4 != 0)
          return 3;
        return flag2 ? 1 : 2;
      }
      if (num2 == 0)
      {
        if (num3 != 0)
          return 3;
        return flag1 ? 1 : 2;
      }
      if (num3 != 0 || num4 != 0)
        return 3;
      return flag1 || flag2 ? 1 : 2;
    }

    public bool IsKokanHide()
    {
      bool flag = false;
      int[] numArray1 = new int[4]{ 0, 1, 2, 3 };
      int[] numArray2 = new int[4]{ 1, 1, 3, 3 };
      for (int index = 0; index < numArray1.Length; ++index)
      {
        int clothesKind = numArray1[index];
        if (this.IsClothes(clothesKind) && (index != 0 && index != 2 || !("1" != this.infoClothes[clothesKind].GetInfo(ChaListDefine.KeyType.Coordinate))) && ("1" == this.infoClothes[clothesKind].GetInfo(ChaListDefine.KeyType.KokanHide) && this.fileStatus.clothesState[numArray2[index]] == (byte) 0))
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    public bool ChangeCustomClothes(
      int kind,
      bool updateColor,
      bool updateTex01,
      bool updateTex02,
      bool updateTex03)
    {
      CustomTextureCreate[] customTextureCreateArray1 = new CustomTextureCreate[3]
      {
        this.ctCreateClothes[kind, 0],
        this.ctCreateClothes[kind, 1],
        this.ctCreateClothes[kind, 2]
      };
      CustomTextureCreate[] customTextureCreateArray2 = new CustomTextureCreate[3]
      {
        this.ctCreateClothesGloss[kind, 0],
        this.ctCreateClothesGloss[kind, 1],
        this.ctCreateClothesGloss[kind, 2]
      };
      if (customTextureCreateArray1[0] == null)
        return false;
      CmpClothes clothesComponent = this.GetCustomClothesComponent(kind);
      if (Object.op_Equality((Object) null, (Object) clothesComponent))
        return false;
      ChaFileClothes.PartsInfo part = this.nowCoordinate.clothes.parts[kind];
      if (!updateColor && !updateTex01 && (!updateTex02 && !updateTex03))
        return false;
      bool flag = true;
      int[] numArray1 = new int[3]
      {
        ChaShader.PatternMask1,
        ChaShader.PatternMask2,
        ChaShader.PatternMask3
      };
      bool[] flagArray1 = new bool[3]
      {
        updateTex01,
        updateTex02,
        updateTex03
      };
      for (int index = 0; index < 3; ++index)
      {
        if (flagArray1[index])
        {
          Texture2D texture2D = (Texture2D) null;
          ListInfoBase listInfo = this.lstCtrl.GetListInfo(ChaListDefine.CategoryNo.st_pattern, part.colorInfo[index].pattern);
          if (listInfo != null)
          {
            string info1 = listInfo.GetInfo(ChaListDefine.KeyType.MainTexAB);
            string info2 = listInfo.GetInfo(ChaListDefine.KeyType.MainTex);
            if ("0" != info1 && "0" != info2)
            {
              texture2D = CommonLib.LoadAsset<Texture2D>(info1, info2, false, string.Empty);
              Singleton<Character>.Instance.AddLoadAssetBundle(info1, "abdata");
            }
          }
          if (Object.op_Inequality((Object) null, (Object) texture2D))
          {
            foreach (CustomTextureCreate customTextureCreate in customTextureCreateArray1)
              customTextureCreate?.SetTexture(numArray1[index], (Texture) texture2D);
          }
          else
          {
            foreach (CustomTextureCreate customTextureCreate in customTextureCreateArray1)
              customTextureCreate?.SetTexture(numArray1[index], (Texture) null);
          }
        }
      }
      if (updateColor)
      {
        int[] numArray2 = new int[3]
        {
          ChaShader.Color,
          ChaShader.Color2,
          ChaShader.Color3
        };
        int[] numArray3 = new int[3]
        {
          ChaShader.Color1_2,
          ChaShader.Color2_2,
          ChaShader.Color3_2
        };
        int[] numArray4 = new int[3]
        {
          ChaShader.patternuv1,
          ChaShader.patternuv2,
          ChaShader.patternuv3
        };
        int[] numArray5 = new int[3]
        {
          ChaShader.patternuv1Rotator,
          ChaShader.patternuv2Rotator,
          ChaShader.patternuv3Rotator
        };
        int[] numArray6 = new int[3]
        {
          ChaShader.ClothesGloss1,
          ChaShader.ClothesGloss2,
          ChaShader.ClothesGloss3
        };
        int[] numArray7 = new int[3]
        {
          ChaShader.Metallic,
          ChaShader.Metallic2,
          ChaShader.Metallic3
        };
        bool[] flagArray2 = new bool[3]
        {
          clothesComponent.useColorA01,
          clothesComponent.useColorA02,
          clothesComponent.useColorA03
        };
        for (int index1 = 0; index1 < customTextureCreateArray1.Length; ++index1)
        {
          if (customTextureCreateArray1[index1] != null)
          {
            for (int index2 = 0; index2 < 3; ++index2)
            {
              customTextureCreateArray1[index1].SetVector4(ChaShader.uvScalePattern, clothesComponent.uvScalePattern);
              if (!flagArray2[index2] && part.colorInfo[index2].baseColor.a != 1.0)
                part.colorInfo[index2].baseColor = new Color((float) part.colorInfo[index2].baseColor.r, (float) part.colorInfo[index2].baseColor.g, (float) part.colorInfo[index2].baseColor.b, 1f);
              customTextureCreateArray1[index1].SetColor(numArray2[index2], part.colorInfo[index2].baseColor);
              customTextureCreateArray1[index1].SetColor(numArray3[index2], part.colorInfo[index2].patternColor);
              Vector4 vector4;
              vector4.x = (__Null) (double) Mathf.Lerp(20f, 1f, (float) part.colorInfo[index2].layout.x);
              vector4.y = (__Null) (double) Mathf.Lerp(20f, 1f, (float) part.colorInfo[index2].layout.y);
              vector4.z = (__Null) (double) Mathf.Lerp(-1f, 1f, (float) part.colorInfo[index2].layout.z);
              vector4.w = (__Null) (double) Mathf.Lerp(-1f, 1f, (float) part.colorInfo[index2].layout.w);
              customTextureCreateArray1[index1].SetVector4(numArray4[index2], vector4);
              float num = Mathf.Lerp(-1f, 1f, part.colorInfo[index2].rotation);
              customTextureCreateArray1[index1].SetFloat(numArray5[index2], num);
            }
          }
        }
        for (int index1 = 0; index1 < customTextureCreateArray2.Length; ++index1)
        {
          if (customTextureCreateArray2[index1] != null)
          {
            for (int index2 = 0; index2 < 3; ++index2)
            {
              customTextureCreateArray2[index1].SetFloat(numArray6[index2], part.colorInfo[index2].glossPower);
              customTextureCreateArray2[index1].SetFloat(numArray7[index2], part.colorInfo[index2].metallicPower);
            }
          }
        }
      }
      for (int index = 0; index < customTextureCreateArray1.Length; ++index)
      {
        if (customTextureCreateArray1[index] != null)
          customTextureCreateArray1[index].SetColor(ChaShader.Color4, clothesComponent.defMainColor04);
      }
      for (int index = 0; index < customTextureCreateArray2.Length; ++index)
      {
        if (customTextureCreateArray2[index] != null)
        {
          customTextureCreateArray2[index].SetFloat(ChaShader.ClothesGloss4, clothesComponent.defGloss04);
          customTextureCreateArray2[index].SetFloat(ChaShader.Metallic4, clothesComponent.defMetallic04);
        }
      }
      bool[] flagArray3 = new bool[3]
      {
        clothesComponent.rendNormal01 != null && 0 != clothesComponent.rendNormal01.Length,
        clothesComponent.rendNormal02 != null && 0 != clothesComponent.rendNormal02.Length,
        clothesComponent.rendNormal03 != null && 0 != clothesComponent.rendNormal03.Length
      };
      Renderer[][] rendererArray = new Renderer[3][]
      {
        clothesComponent.rendNormal01,
        clothesComponent.rendNormal02,
        clothesComponent.rendNormal03
      };
      for (int index1 = 0; index1 < 3; ++index1)
      {
        if (flagArray3[index1] && customTextureCreateArray1[index1] != null)
        {
          Texture texture1 = customTextureCreateArray1[index1].RebuildTextureAndSetMaterial();
          Texture texture2 = (Texture) null;
          if (customTextureCreateArray2[index1] != null)
            texture2 = customTextureCreateArray2[index1].RebuildTextureAndSetMaterial();
          if (Object.op_Inequality((Object) null, (Object) texture1))
          {
            if (flagArray3[index1])
            {
              for (int index2 = 0; index2 < rendererArray[index1].Length; ++index2)
              {
                if (Object.op_Inequality((Object) null, (Object) rendererArray[index1][index2]))
                {
                  rendererArray[index1][index2].get_material().SetTexture(ChaShader.MainTex, texture1);
                  if (Object.op_Inequality((Object) null, (Object) texture2))
                    rendererArray[index1][index2].get_material().SetTexture(ChaShader.DetailMainTex, texture2);
                }
                else
                  flag = false;
              }
            }
          }
          else
            flag = false;
        }
        else
          flag = false;
      }
      return flag;
    }

    public void ChangeBreakClothes(int kind)
    {
      CmpClothes clothesComponent = this.GetCustomClothesComponent(kind);
      if (Object.op_Equality((Object) null, (Object) clothesComponent) || !clothesComponent.useBreak)
        return;
      ChaFileClothes.PartsInfo part = this.nowCoordinate.clothes.parts[kind];
      bool[] flagArray = new bool[3]
      {
        clothesComponent.rendNormal01 != null && 0 != clothesComponent.rendNormal01.Length,
        clothesComponent.rendNormal02 != null && 0 != clothesComponent.rendNormal02.Length,
        clothesComponent.rendNormal03 != null && 0 != clothesComponent.rendNormal03.Length
      };
      Renderer[][] rendererArray = new Renderer[3][]
      {
        clothesComponent.rendNormal01,
        clothesComponent.rendNormal02,
        clothesComponent.rendNormal03
      };
      for (int index1 = 0; index1 < 3; ++index1)
      {
        if (flagArray[index1] && flagArray[index1])
        {
          for (int index2 = 0; index2 < rendererArray[index1].Length; ++index2)
          {
            if (Object.op_Inequality((Object) null, (Object) rendererArray[index1][index2]))
              rendererArray[index1][index2].get_material().SetFloat(ChaShader.ClothesBreak, 1f - part.breakRate);
          }
        }
      }
      this.ChangeAlphaMaskEx();
      this.ChangeAlphaMask2();
    }

    public bool UpdateClothesSiru(
      int kind,
      float frontTop,
      float frontBot,
      float downTop,
      float downBot)
    {
      if (this.sex == (byte) 0 || Object.op_Equality((Object) null, (Object) this.cmpClothes[kind]))
        return false;
      if (!((IList<Renderer>) this.cmpClothes[kind].rendNormal01).IsNullOrEmpty<Renderer>())
      {
        foreach (Renderer renderer in this.cmpClothes[kind].rendNormal01)
        {
          if (!Object.op_Equality((Object) null, (Object) renderer))
          {
            renderer.get_material().SetFloat(ChaShader.siruFrontTop, frontTop);
            renderer.get_material().SetFloat(ChaShader.siruFrontBot, frontBot);
            renderer.get_material().SetFloat(ChaShader.siruBackTop, downTop);
            renderer.get_material().SetFloat(ChaShader.siruBackBot, downBot);
          }
        }
      }
      if (!((IList<Renderer>) this.cmpClothes[kind].rendNormal02).IsNullOrEmpty<Renderer>())
      {
        foreach (Renderer renderer in this.cmpClothes[kind].rendNormal02)
        {
          if (!Object.op_Equality((Object) null, (Object) renderer))
          {
            renderer.get_material().SetFloat(ChaShader.siruFrontTop, frontTop);
            renderer.get_material().SetFloat(ChaShader.siruFrontBot, frontBot);
            renderer.get_material().SetFloat(ChaShader.siruBackTop, downTop);
            renderer.get_material().SetFloat(ChaShader.siruBackBot, downBot);
          }
        }
      }
      if (!((IList<Renderer>) this.cmpClothes[kind].rendNormal03).IsNullOrEmpty<Renderer>())
      {
        foreach (Renderer renderer in this.cmpClothes[kind].rendNormal03)
        {
          if (!Object.op_Equality((Object) null, (Object) renderer))
          {
            renderer.get_material().SetFloat(ChaShader.siruFrontTop, frontTop);
            renderer.get_material().SetFloat(ChaShader.siruFrontBot, frontBot);
            renderer.get_material().SetFloat(ChaShader.siruBackTop, downTop);
            renderer.get_material().SetFloat(ChaShader.siruBackBot, downBot);
          }
        }
      }
      return true;
    }

    public ChaFileClothes.PartsInfo.ColorInfo GetClothesDefaultSetting(
      int kind,
      int no)
    {
      ChaFileClothes.PartsInfo.ColorInfo colorInfo = new ChaFileClothes.PartsInfo.ColorInfo();
      CmpClothes clothesComponent = this.GetCustomClothesComponent(kind);
      if (Object.op_Inequality((Object) null, (Object) clothesComponent))
      {
        switch (no)
        {
          case 0:
            colorInfo.baseColor = clothesComponent.defMainColor01;
            colorInfo.patternColor = clothesComponent.defPatternColor01;
            colorInfo.pattern = clothesComponent.defPtnIndex01;
            colorInfo.glossPower = clothesComponent.defGloss01;
            colorInfo.metallicPower = clothesComponent.defMetallic01;
            Vector4 vector4_1;
            vector4_1.x = (__Null) (double) Mathf.InverseLerp(20f, 1f, (float) clothesComponent.defLayout01.x);
            vector4_1.y = (__Null) (double) Mathf.InverseLerp(20f, 1f, (float) clothesComponent.defLayout01.y);
            vector4_1.z = (__Null) (double) Mathf.InverseLerp(-1f, 1f, (float) clothesComponent.defLayout01.z);
            vector4_1.w = (__Null) (double) Mathf.InverseLerp(-1f, 1f, (float) clothesComponent.defLayout01.w);
            colorInfo.layout = vector4_1;
            colorInfo.rotation = Mathf.InverseLerp(-1f, 1f, clothesComponent.defRotation01);
            break;
          case 1:
            colorInfo.baseColor = clothesComponent.defMainColor02;
            colorInfo.patternColor = clothesComponent.defPatternColor02;
            colorInfo.pattern = clothesComponent.defPtnIndex02;
            colorInfo.glossPower = clothesComponent.defGloss02;
            colorInfo.metallicPower = clothesComponent.defMetallic02;
            Vector4 vector4_2;
            vector4_2.x = (__Null) (double) Mathf.InverseLerp(20f, 1f, (float) clothesComponent.defLayout02.x);
            vector4_2.y = (__Null) (double) Mathf.InverseLerp(20f, 1f, (float) clothesComponent.defLayout02.y);
            vector4_2.z = (__Null) (double) Mathf.InverseLerp(-1f, 1f, (float) clothesComponent.defLayout02.z);
            vector4_2.w = (__Null) (double) Mathf.InverseLerp(-1f, 1f, (float) clothesComponent.defLayout02.w);
            colorInfo.layout = vector4_2;
            colorInfo.rotation = Mathf.InverseLerp(-1f, 1f, clothesComponent.defRotation02);
            break;
          case 2:
            colorInfo.baseColor = clothesComponent.defMainColor03;
            colorInfo.patternColor = clothesComponent.defPatternColor03;
            colorInfo.pattern = clothesComponent.defPtnIndex03;
            colorInfo.glossPower = clothesComponent.defGloss03;
            colorInfo.metallicPower = clothesComponent.defMetallic03;
            Vector4 vector4_3;
            vector4_3.x = (__Null) (double) Mathf.InverseLerp(20f, 1f, (float) clothesComponent.defLayout03.x);
            vector4_3.y = (__Null) (double) Mathf.InverseLerp(20f, 1f, (float) clothesComponent.defLayout03.y);
            vector4_3.z = (__Null) (double) Mathf.InverseLerp(-1f, 1f, (float) clothesComponent.defLayout03.z);
            vector4_3.w = (__Null) (double) Mathf.InverseLerp(-1f, 1f, (float) clothesComponent.defLayout03.w);
            colorInfo.layout = vector4_3;
            colorInfo.rotation = Mathf.InverseLerp(-1f, 1f, clothesComponent.defRotation03);
            break;
        }
      }
      return colorInfo;
    }

    public void SetClothesDefaultSetting(int kind)
    {
      CmpClothes clothesComponent = this.GetCustomClothesComponent(kind);
      if (!Object.op_Inequality((Object) null, (Object) clothesComponent))
        return;
      this.nowCoordinate.clothes.parts[kind].colorInfo[0].baseColor = clothesComponent.defMainColor01;
      this.nowCoordinate.clothes.parts[kind].colorInfo[1].baseColor = clothesComponent.defMainColor02;
      this.nowCoordinate.clothes.parts[kind].colorInfo[2].baseColor = clothesComponent.defMainColor03;
      this.nowCoordinate.clothes.parts[kind].colorInfo[0].patternColor = clothesComponent.defPatternColor01;
      this.nowCoordinate.clothes.parts[kind].colorInfo[1].patternColor = clothesComponent.defPatternColor02;
      this.nowCoordinate.clothes.parts[kind].colorInfo[2].patternColor = clothesComponent.defPatternColor03;
      this.nowCoordinate.clothes.parts[kind].colorInfo[0].pattern = clothesComponent.defPtnIndex01;
      this.nowCoordinate.clothes.parts[kind].colorInfo[1].pattern = clothesComponent.defPtnIndex02;
      this.nowCoordinate.clothes.parts[kind].colorInfo[2].pattern = clothesComponent.defPtnIndex03;
      this.nowCoordinate.clothes.parts[kind].colorInfo[0].glossPower = clothesComponent.defGloss01;
      this.nowCoordinate.clothes.parts[kind].colorInfo[1].glossPower = clothesComponent.defGloss02;
      this.nowCoordinate.clothes.parts[kind].colorInfo[2].glossPower = clothesComponent.defGloss03;
      this.nowCoordinate.clothes.parts[kind].colorInfo[0].metallicPower = clothesComponent.defMetallic01;
      this.nowCoordinate.clothes.parts[kind].colorInfo[1].metallicPower = clothesComponent.defMetallic02;
      this.nowCoordinate.clothes.parts[kind].colorInfo[2].metallicPower = clothesComponent.defMetallic03;
      Vector4 vector4;
      vector4.x = (__Null) (double) Mathf.InverseLerp(20f, 1f, (float) clothesComponent.defLayout01.x);
      vector4.y = (__Null) (double) Mathf.InverseLerp(20f, 1f, (float) clothesComponent.defLayout01.y);
      vector4.z = (__Null) (double) Mathf.InverseLerp(-1f, 1f, (float) clothesComponent.defLayout01.z);
      vector4.w = (__Null) (double) Mathf.InverseLerp(-1f, 1f, (float) clothesComponent.defLayout01.w);
      this.nowCoordinate.clothes.parts[kind].colorInfo[0].layout = vector4;
      vector4.x = (__Null) (double) Mathf.InverseLerp(20f, 1f, (float) clothesComponent.defLayout02.x);
      vector4.y = (__Null) (double) Mathf.InverseLerp(20f, 1f, (float) clothesComponent.defLayout02.y);
      vector4.z = (__Null) (double) Mathf.InverseLerp(-1f, 1f, (float) clothesComponent.defLayout02.z);
      vector4.w = (__Null) (double) Mathf.InverseLerp(-1f, 1f, (float) clothesComponent.defLayout02.w);
      this.nowCoordinate.clothes.parts[kind].colorInfo[1].layout = vector4;
      vector4.x = (__Null) (double) Mathf.InverseLerp(20f, 1f, (float) clothesComponent.defLayout03.x);
      vector4.y = (__Null) (double) Mathf.InverseLerp(20f, 1f, (float) clothesComponent.defLayout03.y);
      vector4.z = (__Null) (double) Mathf.InverseLerp(-1f, 1f, (float) clothesComponent.defLayout03.z);
      vector4.w = (__Null) (double) Mathf.InverseLerp(-1f, 1f, (float) clothesComponent.defLayout03.w);
      this.nowCoordinate.clothes.parts[kind].colorInfo[2].layout = vector4;
      this.nowCoordinate.clothes.parts[kind].colorInfo[0].rotation = Mathf.InverseLerp(-1f, 1f, clothesComponent.defRotation01);
      this.nowCoordinate.clothes.parts[kind].colorInfo[1].rotation = Mathf.InverseLerp(-1f, 1f, clothesComponent.defRotation02);
      this.nowCoordinate.clothes.parts[kind].colorInfo[2].rotation = Mathf.InverseLerp(-1f, 1f, clothesComponent.defRotation03);
    }

    public void ResetDynamicBoneClothes(bool includeInactive = false)
    {
      if (this.cmpClothes == null)
        return;
      for (int index = 0; index < this.cmpClothes.Length; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) this.cmpClothes[index]))
          this.cmpClothes[index].ResetDynamicBones(includeInactive);
      }
    }

    private int ShapeBodyNum { get; set; }

    public ShapeInfoBase sibBody { get; set; }

    private bool changeShapeBodyMask { get; set; }

    public BustSoft bustSoft { get; private set; }

    public BustGravity bustGravity { get; private set; }

    public bool[] updateCMBodyTex { get; private set; }

    public bool[] updateCMBodyColor { get; private set; }

    public bool[] updateCMBodyGloss { get; private set; }

    public bool[] updateCMBodyLayout { get; private set; }

    protected void InitializeControlCustomBodyAll()
    {
      this.ShapeBodyNum = ChaFileDefine.cf_bodyshapename.Length;
      this.InitializeControlCustomBodyObject();
    }

    protected void InitializeControlCustomBodyObject()
    {
      this.sibBody = (ShapeInfoBase) new ShapeBodyInfoFemale();
      this.changeShapeBodyMask = false;
      this.bustSoft = new BustSoft(this);
      this.bustGravity = new BustGravity(this);
      int length = Enum.GetNames(typeof (ChaControl.BodyTexKind)).Length;
      this.updateCMBodyTex = new bool[length];
      this.updateCMBodyColor = new bool[length];
      this.updateCMBodyGloss = new bool[length];
      this.updateCMBodyLayout = new bool[length];
    }

    protected void ReleaseControlCustomBodyAll()
    {
      this.ReleaseControlCustomBodyObject(true);
    }

    protected void ReleaseControlCustomBodyObject(bool init = true)
    {
      if (this.sibBody != null)
        this.sibBody.ReleaseShapeInfo();
      if (!init)
        return;
      this.InitializeControlCustomBodyObject();
    }

    public void AddUpdateCMBodyTexFlags(
      bool inpBase,
      bool inpPaint01,
      bool inpPaint02,
      bool inpSunburn)
    {
      if (inpBase)
        this.updateCMBodyTex[0] = inpBase;
      if (inpPaint01)
        this.updateCMBodyTex[1] = inpPaint01;
      if (inpPaint02)
        this.updateCMBodyTex[2] = inpPaint02;
      if (!inpSunburn)
        return;
      this.updateCMBodyTex[3] = inpSunburn;
    }

    public void AddUpdateCMBodyColorFlags(
      bool inpBase,
      bool inpPaint01,
      bool inpPaint02,
      bool inpSunburn)
    {
      if (inpBase)
        this.updateCMBodyColor[0] = inpBase;
      if (inpPaint01)
        this.updateCMBodyColor[1] = inpPaint01;
      if (inpPaint02)
        this.updateCMBodyColor[2] = inpPaint02;
      if (!inpSunburn)
        return;
      this.updateCMBodyColor[3] = inpSunburn;
    }

    public void AddUpdateCMBodyGlossFlags(bool inpPaint01, bool inpPaint02)
    {
      if (inpPaint01)
        this.updateCMBodyGloss[1] = inpPaint01;
      if (!inpPaint02)
        return;
      this.updateCMBodyGloss[2] = inpPaint02;
    }

    public void AddUpdateCMBodyLayoutFlags(bool inpPaint01, bool inpPaint02)
    {
      if (inpPaint01)
        this.updateCMBodyLayout[1] = inpPaint01;
      if (!inpPaint02)
        return;
      this.updateCMBodyLayout[2] = inpPaint02;
    }

    protected bool InitBaseCustomTextureBody()
    {
      if (this.customTexCtrlBody != null)
      {
        this.customTexCtrlBody.Release();
        this.customTexCtrlBody = (CustomTextureControl) null;
      }
      this.customTexCtrlBody = new CustomTextureControl(2, "abdata", "chara/mm_base.unity3d", ChaABDefine.BodyMaterialAsset((int) this.sex), this.objRoot.get_transform());
      this.customTexCtrlBody.Initialize(0, "abdata", "chara/mm_base.unity3d", "create_skin_body", 4096, 4096, (RenderTextureFormat) 0);
      this.customTexCtrlBody.Initialize(1, "abdata", "chara/mm_base.unity3d", "create_skin detail_body", 4096, 4096, (RenderTextureFormat) 0);
      return true;
    }

    public bool CreateBodyTexture()
    {
      bool flag1 = false;
      bool flag2 = false;
      CustomTextureCreate ctc1 = this.customTexCtrlBody.createCustomTex[0];
      CustomTextureCreate ctc2 = this.customTexCtrlBody.createCustomTex[1];
      if (this.updateCMBodyTex[0])
      {
        if (this.SetCreateTexture(ctc1, true, this.sex != (byte) 0 ? ChaListDefine.CategoryNo.ft_skin_b : ChaListDefine.CategoryNo.mt_skin_b, this.fileBody.skinId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.MainTex, -1))
          flag1 = true;
        Texture2D texture2D = CommonLib.LoadAsset<Texture2D>("chara/etc.unity3d", "black4096", false, "abdata");
        Singleton<Character>.Instance.AddLoadAssetBundle("chara/etc.unity3d", "abdata");
        if (Object.op_Inequality((Object) null, (Object) texture2D))
        {
          ctc2.SetMainTexture((Texture) texture2D);
          flag2 = true;
        }
        this.ChangeTexture(this.customTexCtrlBody.matDraw, this.sex != (byte) 0 ? ChaListDefine.CategoryNo.ft_detail_b : ChaListDefine.CategoryNo.mt_detail_b, this.fileBody.detailId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.OcclusionMapTex, ChaShader.SkinOcclusionMapTex, string.Empty);
        this.ChangeTexture(this.customTexCtrlBody.matDraw, this.sex != (byte) 0 ? ChaListDefine.CategoryNo.ft_detail_b : ChaListDefine.CategoryNo.mt_detail_b, this.fileBody.detailId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.NormalMapTex, ChaShader.SkinDetailTex, string.Empty);
        this.updateCMBodyTex[0] = false;
      }
      if (this.updateCMBodyColor[0])
      {
        ctc1.SetColor(ChaShader.SkinColor, this.fileBody.skinColor);
        flag1 = true;
        this.updateCMBodyColor[0] = false;
      }
      if (this.updateCMBodyTex[3])
      {
        if (this.SetCreateTexture(ctc1, false, this.sex != (byte) 0 ? ChaListDefine.CategoryNo.ft_sunburn : ChaListDefine.CategoryNo.mt_sunburn, this.fileBody.sunburnId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.AddTex, ChaShader.SunburnTex))
          flag1 = true;
        this.updateCMBodyTex[3] = false;
      }
      if (this.updateCMBodyColor[3])
      {
        ctc1.SetColor(ChaShader.SunburnColor, this.fileBody.sunburnColor);
        this.updateCMBodyColor[3] = false;
        flag1 = true;
      }
      int[] numArray1 = new int[2]{ 1, 2 };
      int[] numArray2 = new int[2]
      {
        ChaShader.Paint01Tex,
        ChaShader.Paint02Tex
      };
      int[] numArray3 = new int[2]
      {
        ChaShader.Paint01Color,
        ChaShader.Paint02Color
      };
      int[] numArray4 = new int[2]
      {
        ChaShader.Paint01Gloass,
        ChaShader.Paint02Gloass
      };
      int[] numArray5 = new int[2]
      {
        ChaShader.Paint01Metallic,
        ChaShader.Paint02Metallic
      };
      int[] numArray6 = new int[2]
      {
        ChaShader.Paint01Layout,
        ChaShader.Paint02Layout
      };
      int[] numArray7 = new int[2]
      {
        ChaShader.Paint01Rot,
        ChaShader.Paint02Rot
      };
      for (int index = 0; index < 2; ++index)
      {
        if (this.updateCMBodyTex[numArray1[index]])
        {
          if (this.SetCreateTexture(ctc1, false, ChaListDefine.CategoryNo.st_paint, this.fileBody.paintInfo[index].id, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.AddTex, numArray2[index]))
            flag1 = true;
          if (this.SetCreateTexture(ctc2, false, ChaListDefine.CategoryNo.st_paint, this.fileBody.paintInfo[index].id, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.GlossTex, numArray2[index]))
            flag2 = true;
          this.updateCMBodyTex[numArray1[index]] = false;
        }
        if (this.updateCMBodyColor[numArray1[index]])
        {
          ctc1.SetColor(numArray3[index], this.fileBody.paintInfo[index].color);
          this.updateCMBodyColor[numArray1[index]] = false;
          flag1 = true;
        }
        if (this.updateCMBodyGloss[numArray1[index]])
        {
          ctc2.SetFloat(numArray4[index], this.fileBody.paintInfo[index].glossPower);
          ctc2.SetFloat(numArray5[index], this.fileBody.paintInfo[index].metallicPower);
          this.updateCMBodyGloss[numArray1[index]] = false;
          flag2 = true;
        }
        ListInfoBase listInfo = this.lstCtrl.GetListInfo(ChaListDefine.CategoryNo.bodypaint_layout, this.fileBody.paintInfo[index].layoutId);
        if (listInfo != null)
        {
          float num1 = listInfo.GetInfoFloat(ChaListDefine.KeyType.CenterX) + listInfo.GetInfoFloat(ChaListDefine.KeyType.MoveX);
          float num2 = listInfo.GetInfoFloat(ChaListDefine.KeyType.CenterX) - listInfo.GetInfoFloat(ChaListDefine.KeyType.MoveX);
          float num3 = listInfo.GetInfoFloat(ChaListDefine.KeyType.CenterY) + listInfo.GetInfoFloat(ChaListDefine.KeyType.MoveY);
          float num4 = listInfo.GetInfoFloat(ChaListDefine.KeyType.CenterY) - listInfo.GetInfoFloat(ChaListDefine.KeyType.MoveY);
          float num5 = listInfo.GetInfoFloat(ChaListDefine.KeyType.CenterScale) + listInfo.GetInfoFloat(ChaListDefine.KeyType.AddScale);
          float num6 = listInfo.GetInfoFloat(ChaListDefine.KeyType.CenterScale) - listInfo.GetInfoFloat(ChaListDefine.KeyType.AddScale);
          Vector4 vector4;
          vector4.x = (__Null) (double) Mathf.Lerp(num5, num6, (float) this.fileBody.paintInfo[index].layout.x);
          vector4.y = (__Null) (double) Mathf.Lerp(num5, num6, (float) this.fileBody.paintInfo[index].layout.y);
          vector4.z = (__Null) (double) Mathf.Lerp(num1, num2, (float) this.fileBody.paintInfo[index].layout.z);
          vector4.w = (__Null) (double) Mathf.Lerp(num3, num4, (float) this.fileBody.paintInfo[index].layout.w);
          float num7 = Mathf.Lerp(1f, -1f, this.fileBody.paintInfo[index].rotation);
          ctc1.SetVector4(numArray6[index], vector4);
          ctc1.SetFloat(numArray7[index], num7);
          ctc2.SetVector4(numArray6[index], vector4);
          ctc2.SetFloat(numArray7[index], num7);
          this.updateCMBodyLayout[numArray1[index]] = false;
          flag1 = true;
          flag2 = true;
        }
      }
      if (flag1)
        this.customTexCtrlBody.SetNewCreateTexture(0, ChaShader.SkinTex);
      if (flag2)
        this.customTexCtrlBody.SetNewCreateTexture(1, ChaShader.SkinCreateDetailTex);
      if (this.releaseCustomInputTexture)
        this.ReleaseBodyCustomTexture();
      return true;
    }

    public bool ChangeBodyDetailPower()
    {
      this.customTexCtrlBody.matDraw.SetFloat(ChaShader.SkinDetailPower, this.fileBody.detailPower);
      return true;
    }

    public bool ChangeBodyGlossPower()
    {
      float num = Mathf.Lerp(0.0f, 0.8f, this.fileBody.skinGlossPower) + 0.2f * this.fileStatus.skinTuyaRate;
      this.customTexCtrlBody.matDraw.SetFloat(ChaShader.Gloss, num);
      return true;
    }

    public bool ChangeBodyMetallicPower()
    {
      this.customTexCtrlBody.matDraw.SetFloat(ChaShader.Metallic, this.fileBody.skinMetallicPower);
      return true;
    }

    public bool ChangeNipKind()
    {
      this.ChangeTexture(this.customTexCtrlBody.matDraw, ChaListDefine.CategoryNo.st_nip, this.fileBody.nipId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.AddTex, ChaShader.NipTex, string.Empty);
      return true;
    }

    public bool ChangeNipColor()
    {
      this.customTexCtrlBody.matDraw.SetColor(ChaShader.NipColor, this.fileBody.nipColor);
      return true;
    }

    public bool ChangeNipGloss()
    {
      this.customTexCtrlBody.matDraw.SetFloat(ChaShader.NipGloss, this.fileBody.nipGlossPower);
      return true;
    }

    public bool ChangeNipScale()
    {
      this.customTexCtrlBody.matDraw.SetFloat(ChaShader.NipScale, this.fileBody.areolaSize);
      return true;
    }

    public bool ChangeUnderHairKind()
    {
      this.ChangeTexture(this.customTexCtrlBody.matDraw, ChaListDefine.CategoryNo.st_underhair, this.fileBody.underhairId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.AddTex, ChaShader.UnderhairTex, string.Empty);
      return true;
    }

    public bool ChangeUnderHairColor()
    {
      this.customTexCtrlBody.matDraw.SetColor(ChaShader.UnderhairColor, this.fileBody.underhairColor);
      return true;
    }

    public bool ChangeNailColor()
    {
      this.customTexCtrlBody.matDraw.SetColor(ChaShader.NailColor, this.fileBody.nailColor);
      return true;
    }

    public bool ChangeNailGloss()
    {
      this.customTexCtrlBody.matDraw.SetFloat(ChaShader.NailGloss, this.fileBody.nailGlossPower);
      return true;
    }

    public bool SetBodyBaseMaterial()
    {
      if (Object.op_Equality((Object) null, (Object) this.customMatBody) || Object.op_Equality((Object) null, (Object) this.cmpBody))
        return false;
      Renderer rendBody = this.cmpBody.targetCustom.rendBody;
      return !Object.op_Equality((Object) null, (Object) rendBody) && this.SetBaseMaterial(rendBody, this.customMatBody);
    }

    public bool ReleaseBodyCustomTexture()
    {
      if (this.customTexCtrlBody == null)
        return false;
      CustomTextureCreate customTextureCreate1 = this.customTexCtrlBody.createCustomTex[0];
      CustomTextureCreate customTextureCreate2 = this.customTexCtrlBody.createCustomTex[1];
      customTextureCreate1.SetTexture(ChaShader.MainTex, (Texture) null);
      customTextureCreate1.SetTexture(ChaShader.SunburnTex, (Texture) null);
      customTextureCreate1.SetTexture(ChaShader.Paint01Tex, (Texture) null);
      customTextureCreate1.SetTexture(ChaShader.Paint02Tex, (Texture) null);
      customTextureCreate2.SetTexture(ChaShader.MainTex, (Texture) null);
      customTextureCreate2.SetTexture(ChaShader.SunburnTex, (Texture) null);
      customTextureCreate2.SetTexture(ChaShader.Paint01Tex, (Texture) null);
      customTextureCreate2.SetTexture(ChaShader.Paint02Tex, (Texture) null);
      OutputLog.Log(nameof (ReleaseBodyCustomTexture), false, "UnloadUnusedAssets");
      Resources.UnloadUnusedAssets();
      return true;
    }

    public void ChangeCustomBodyWithoutCustomTexture()
    {
      this.ChangeBodyGlossPower();
      this.ChangeBodyMetallicPower();
      this.ChangeBodyDetailPower();
      this.ChangeNipKind();
      this.ChangeNipColor();
      this.ChangeNipGloss();
      this.ChangeNipScale();
      this.ChangeUnderHairKind();
      this.ChangeUnderHairColor();
      this.ChangeNailColor();
      this.ChangeNailGloss();
    }

    public bool InitShapeBody(Transform trfBone)
    {
      if (this.sibBody == null || Object.op_Equality((Object) null, (Object) trfBone))
        return false;
      this.sibBody.InitShapeInfo("abdata", "list/customshape.unity3d", "list/customshape.unity3d", "cf_anmShapeBody", "cf_custombody", trfBone);
      float[] numArray = new float[this.fileBody.shapeValueBody.Length];
      this.chaFile.custom.body.shapeValueBody.CopyTo((Array) numArray, 0);
      numArray[32] = Mathf.Lerp(0.0f, 0.8f, numArray[32]) + 0.2f * this.fileStatus.nipStandRate;
      if (this.sex == (byte) 0 || this.isPlayer)
        numArray[0] = 0.75f;
      for (int category = 0; category < this.ShapeBodyNum; ++category)
        this.sibBody.ChangeValue(category, numArray[category]);
      this.updateShapeBody = true;
      this.updateBustSize = true;
      this.reSetupDynamicBoneBust = true;
      return true;
    }

    public void ReleaseShapeBody()
    {
      if (this.sibBody == null)
        return;
      this.sibBody.ReleaseShapeInfo();
    }

    public bool SetShapeBodyValue(int index, float value)
    {
      if (index >= this.ShapeBodyNum)
        return false;
      float num = this.fileBody.shapeValueBody[index] = value;
      if (index == 32)
        num = Mathf.Lerp(0.0f, 0.8f, value) + 0.2f * this.fileStatus.nipStandRate;
      if (index == 0 && (this.sex == (byte) 0 || this.isPlayer))
        num = 0.75f;
      if (this.sibBody != null && this.sibBody.InitEnd)
        this.sibBody.ChangeValue(index, num);
      this.updateShapeBody = true;
      this.updateBustSize = true;
      this.reSetupDynamicBoneBust = true;
      return true;
    }

    public bool UpdateShapeBodyValueFromCustomInfo()
    {
      if (this.sibBody == null || !this.sibBody.InitEnd)
        return false;
      float[] numArray = new float[this.fileBody.shapeValueBody.Length];
      this.fileBody.shapeValueBody.CopyTo((Array) numArray, 0);
      numArray[32] = Mathf.Lerp(0.0f, 0.8f, numArray[32]) + 0.2f * this.fileStatus.nipStandRate;
      if (this.sex == (byte) 0 || this.isPlayer)
        numArray[0] = 0.75f;
      for (int category = 0; category < this.fileBody.shapeValueBody.Length; ++category)
        this.sibBody.ChangeValue(category, numArray[category]);
      this.updateShapeBody = true;
      this.updateBustSize = true;
      this.reSetupDynamicBoneBust = true;
      return true;
    }

    public float GetShapeBodyValue(int index)
    {
      return index >= this.ShapeBodyNum ? 0.0f : this.fileBody.shapeValueBody[index];
    }

    public void UpdateShapeBody()
    {
      if (this.sibBody == null || !this.sibBody.InitEnd || !(this.sibBody is ShapeBodyInfoFemale sibBody))
        return;
      sibBody.updateMask = 31;
      this.sibBody.Update();
      if (!this.changeShapeBodyMask)
        return;
      float[] numArray1 = new float[ChaFileDefine.cf_BustShapeMaskID.Length];
      int[] numArray2 = new int[2]{ 1, 2 };
      float[] numArray3 = new float[8]
      {
        0.5f,
        0.5f,
        0.5f,
        0.5f,
        0.5f,
        0.5f,
        0.5f,
        0.5f
      };
      for (int index1 = 0; index1 < 2; ++index1)
      {
        for (int index2 = 0; index2 < ChaFileDefine.cf_BustShapeMaskID.Length; ++index2)
        {
          int index3 = ChaFileDefine.cf_BustShapeMaskID[index2];
          numArray1[index2] = !this.fileStatus.disableBustShapeMask[index1, index2] ? this.fileBody.shapeValueBody[index3] : numArray3[index2];
        }
        int index4 = 7;
        int index5 = ChaFileDefine.cf_BustShapeMaskID[index4];
        numArray1[index4] = !this.fileStatus.disableBustShapeMask[index1, index4] ? Mathf.Lerp(0.0f, 0.8f, this.fileBody.shapeValueBody[index5]) + 0.2f * this.fileStatus.nipStandRate : 0.5f;
        for (int index2 = 0; index2 < ChaFileDefine.cf_BustShapeMaskID.Length; ++index2)
          this.sibBody.ChangeValue(ChaFileDefine.cf_BustShapeMaskID[index2], numArray1[index2]);
        sibBody.updateMask = numArray2[index1];
        this.sibBody.Update();
      }
      this.changeShapeBodyMask = false;
    }

    public void UpdateAlwaysShapeBody()
    {
      if (this.sibBody == null || !this.sibBody.InitEnd)
        return;
      this.sibBody.UpdateAlways();
    }

    public void UpdateShapeBodyCalcForce()
    {
      if (this.sibBody == null || !this.sibBody.InitEnd)
        return;
      this.sibBody.ForceUpdate();
    }

    public void DisableShapeBodyID(int LR, int id, bool disable)
    {
      if (this.sibBody == null || !this.sibBody.InitEnd || id >= ChaFileDefine.cf_BustShapeMaskID.Length)
        return;
      this.changeShapeBodyMask = true;
      this.updateShapeBody = true;
      switch (LR)
      {
        case 0:
          this.fileStatus.disableBustShapeMask[0, id] = disable;
          break;
        case 1:
          this.fileStatus.disableBustShapeMask[1, id] = disable;
          break;
        default:
          this.fileStatus.disableBustShapeMask[0, id] = disable;
          this.fileStatus.disableBustShapeMask[1, id] = disable;
          break;
      }
      this.reSetupDynamicBoneBust = true;
    }

    public void DisableShapeBust(int LR, bool disable)
    {
      if (this.sibBody == null || !this.sibBody.InitEnd)
        return;
      this.changeShapeBodyMask = true;
      this.updateShapeBody = true;
      switch (LR)
      {
        case 0:
          for (int index = 0; index < ChaFileDefine.cf_ShapeMaskBust.Length; ++index)
            this.fileStatus.disableBustShapeMask[0, ChaFileDefine.cf_ShapeMaskBust[index]] = disable;
          break;
        case 1:
          for (int index = 0; index < ChaFileDefine.cf_ShapeMaskBust.Length; ++index)
            this.fileStatus.disableBustShapeMask[1, ChaFileDefine.cf_ShapeMaskBust[index]] = disable;
          break;
        default:
          for (int index1 = 0; index1 < 2; ++index1)
          {
            for (int index2 = 0; index2 < ChaFileDefine.cf_ShapeMaskBust.Length; ++index2)
              this.fileStatus.disableBustShapeMask[index1, ChaFileDefine.cf_ShapeMaskBust[index2]] = disable;
          }
          break;
      }
      this.reSetupDynamicBoneBust = true;
    }

    public void DisableShapeNip(int LR, bool disable)
    {
      if (this.sibBody == null || !this.sibBody.InitEnd)
        return;
      this.changeShapeBodyMask = true;
      this.updateShapeBody = true;
      switch (LR)
      {
        case 0:
          for (int index = 0; index < ChaFileDefine.cf_ShapeMaskNip.Length; ++index)
            this.fileStatus.disableBustShapeMask[0, ChaFileDefine.cf_ShapeMaskNip[index]] = disable;
          break;
        case 1:
          for (int index = 0; index < ChaFileDefine.cf_ShapeMaskNip.Length; ++index)
            this.fileStatus.disableBustShapeMask[1, ChaFileDefine.cf_ShapeMaskNip[index]] = disable;
          break;
        default:
          for (int index1 = 0; index1 < 2; ++index1)
          {
            for (int index2 = 0; index2 < ChaFileDefine.cf_ShapeMaskNip.Length; ++index2)
              this.fileStatus.disableBustShapeMask[index1, ChaFileDefine.cf_ShapeMaskNip[index2]] = disable;
          }
          break;
      }
      this.reSetupDynamicBoneBust = true;
    }

    public void UpdateBustSoftnessAndGravity()
    {
      this.UpdateBustSoftness();
      this.UpdateBustGravity();
    }

    public void ChangeBustSoftness(float soft)
    {
      if (this.bustSoft == null)
        return;
      this.bustSoft.Change(soft, new int[1]);
      this.reSetupDynamicBoneBust = true;
    }

    public bool UpdateBustSoftness()
    {
      if (this.bustSoft == null)
        return false;
      this.bustSoft.ReCalc(new int[1]);
      this.reSetupDynamicBoneBust = true;
      return true;
    }

    public void ChangeBustGravity(float gravity)
    {
      if (this.bustGravity == null)
        return;
      this.bustGravity.Change(gravity, new int[1]);
      this.reSetupDynamicBoneBust = true;
    }

    public bool UpdateBustGravity()
    {
      if (this.bustGravity == null)
        return false;
      this.bustGravity.ReCalc(new int[1]);
      this.reSetupDynamicBoneBust = true;
      return true;
    }

    private int ShapeFaceNum { get; set; }

    private ShapeInfoBase sibFace { get; set; }

    public bool[] updateCMFaceTex { get; private set; }

    public bool[] updateCMFaceColor { get; private set; }

    public bool[] updateCMFaceGloss { get; private set; }

    public bool[] updateCMFaceLayout { get; private set; }

    protected void InitializeControlCustomFaceAll()
    {
      this.ShapeFaceNum = ChaFileDefine.cf_headshapename.Length;
      this.InitializeControlCustomFaceObject();
    }

    protected void InitializeControlCustomFaceObject()
    {
      this.sibFace = (ShapeInfoBase) new ShapeHeadInfoFemale();
      int length = Enum.GetNames(typeof (ChaControl.FaceTexKind)).Length;
      this.updateCMFaceTex = new bool[length];
      this.updateCMFaceColor = new bool[length];
      this.updateCMFaceGloss = new bool[length];
      this.updateCMFaceLayout = new bool[length];
    }

    protected void ReleaseControlCustomFaceAll()
    {
      this.ReleaseControlCustomFaceObject(true);
    }

    protected void ReleaseControlCustomFaceObject(bool init = true)
    {
      if (this.sibFace != null)
        this.sibFace.ReleaseShapeInfo();
      if (!init)
        return;
      this.InitializeControlCustomFaceObject();
    }

    private bool SetBaseMaterial(Renderer rend, Material mat)
    {
      if (Object.op_Equality((Object) null, (Object) mat) || Object.op_Equality((Object) null, (Object) rend))
        return false;
      int length = rend.get_materials().Length;
      Material[] materialArray;
      if (length == 0)
      {
        materialArray = new Material[1]{ mat };
      }
      else
      {
        materialArray = new Material[length];
        for (int index = 0; index < length; ++index)
          materialArray[index] = rend.get_materials()[index];
        Material material = materialArray[0];
        materialArray[0] = mat;
        if (Object.op_Inequality((Object) material, (Object) mat))
          Object.Destroy((Object) material);
      }
      rend.set_materials(materialArray);
      return true;
    }

    private bool SetCreateTexture(
      CustomTextureCreate ctc,
      bool main,
      ChaListDefine.CategoryNo type,
      int id,
      ChaListDefine.KeyType manifestKey,
      ChaListDefine.KeyType assetBundleKey,
      ChaListDefine.KeyType assetKey,
      int propertyID)
    {
      ListInfoBase listInfo = this.lstCtrl.GetListInfo(type, id);
      if (listInfo == null)
        return false;
      string manifestName = listInfo.GetInfo(manifestKey);
      if ("0" == manifestName)
        manifestName = string.Empty;
      string info1 = listInfo.GetInfo(assetBundleKey);
      string info2 = listInfo.GetInfo(assetKey);
      Texture2D texture2D = (Texture2D) null;
      if ("0" != info1 && "0" != info2)
      {
        texture2D = CommonLib.LoadAsset<Texture2D>(info1, info2, false, manifestName);
        Singleton<Character>.Instance.AddLoadAssetBundle(info1, manifestName);
      }
      if (main)
        ctc.SetMainTexture((Texture) texture2D);
      else
        ctc.SetTexture(propertyID, (Texture) texture2D);
      return true;
    }

    private void ChangeTexture(
      Renderer rend,
      ChaListDefine.CategoryNo type,
      int id,
      ChaListDefine.KeyType manifestKey,
      ChaListDefine.KeyType assetBundleKey,
      ChaListDefine.KeyType assetKey,
      int propertyID,
      string addStr = "")
    {
      if (Object.op_Equality((Object) null, (Object) rend))
        OutputLog.Warning("レンダラーが見つからない。", true, "ChaCustom");
      else
        this.ChangeTexture(rend.get_material(), type, id, manifestKey, assetBundleKey, assetKey, propertyID, addStr);
    }

    private void ChangeTexture(
      Material mat,
      ChaListDefine.CategoryNo type,
      int id,
      ChaListDefine.KeyType manifestKey,
      ChaListDefine.KeyType assetBundleKey,
      ChaListDefine.KeyType assetKey,
      int propertyID,
      string addStr = "")
    {
      if (Object.op_Equality((Object) null, (Object) mat))
      {
        OutputLog.Warning("マテリアルが見つからない。", true, "ChaCustom");
      }
      else
      {
        Texture2D texture = this.GetTexture(type, id, manifestKey, assetBundleKey, assetKey, addStr);
        mat.SetTexture(propertyID, (Texture) texture);
      }
    }

    private Texture2D GetTexture(
      ChaListDefine.CategoryNo type,
      int id,
      ChaListDefine.KeyType manifestKey,
      ChaListDefine.KeyType assetBundleKey,
      ChaListDefine.KeyType assetKey,
      string addStr = "")
    {
      ListInfoBase listInfo = this.lstCtrl.GetListInfo(type, id);
      if (listInfo != null)
      {
        string manifestName = listInfo.GetInfo(manifestKey);
        if ("0" == manifestName)
          manifestName = string.Empty;
        string info1 = listInfo.GetInfo(assetBundleKey);
        string info2 = listInfo.GetInfo(assetKey);
        Texture2D texture2D = (Texture2D) null;
        if ("0" != info1 && "0" != info2)
        {
          if (!addStr.IsNullOrEmpty())
          {
            string assetName = info2 + addStr;
            texture2D = CommonLib.LoadAsset<Texture2D>(info1, assetName, false, manifestName);
          }
          if (Object.op_Equality((Object) null, (Object) texture2D))
            texture2D = CommonLib.LoadAsset<Texture2D>(info1, info2, false, manifestName);
          Singleton<Character>.Instance.AddLoadAssetBundle(info1, manifestName);
        }
        return texture2D;
      }
      OutputLog.Warning("テクスチャの情報が見つからない。", true, "ChaCustom");
      return (Texture2D) null;
    }

    public void AddUpdateCMFaceTexFlags(
      bool inpBase,
      bool inpEyeshadow,
      bool inpPaint01,
      bool inpPaint02,
      bool inpCheek,
      bool inpLip,
      bool inpMole)
    {
      if (inpBase)
        this.updateCMFaceTex[0] = inpBase;
      if (inpEyeshadow)
        this.updateCMFaceTex[1] = inpEyeshadow;
      if (inpPaint01)
        this.updateCMFaceTex[2] = inpPaint01;
      if (inpPaint02)
        this.updateCMFaceTex[3] = inpPaint02;
      if (inpCheek)
        this.updateCMFaceTex[4] = inpCheek;
      if (inpLip)
        this.updateCMFaceTex[5] = inpLip;
      if (!inpMole)
        return;
      this.updateCMFaceTex[6] = inpMole;
    }

    public void AddUpdateCMFaceColorFlags(
      bool inpBase,
      bool inpEyeshadow,
      bool inpPaint01,
      bool inpPaint02,
      bool inpCheek,
      bool inpLip,
      bool inpMole)
    {
      if (inpBase)
        this.updateCMFaceColor[0] = inpBase;
      if (inpEyeshadow)
        this.updateCMFaceColor[1] = inpEyeshadow;
      if (inpPaint01)
        this.updateCMFaceColor[2] = inpPaint01;
      if (inpPaint02)
        this.updateCMFaceColor[3] = inpPaint02;
      if (inpCheek)
        this.updateCMFaceColor[4] = inpCheek;
      if (inpLip)
        this.updateCMFaceColor[5] = inpLip;
      if (!inpMole)
        return;
      this.updateCMFaceColor[6] = inpMole;
    }

    public void AddUpdateCMFaceGlossFlags(
      bool inpEyeshadow,
      bool inpPaint01,
      bool inpPaint02,
      bool inpCheek,
      bool inpLip)
    {
      if (inpEyeshadow)
        this.updateCMFaceGloss[1] = inpEyeshadow;
      if (inpPaint01)
        this.updateCMFaceGloss[2] = inpPaint01;
      if (inpPaint02)
        this.updateCMFaceGloss[3] = inpPaint02;
      if (inpCheek)
        this.updateCMFaceGloss[4] = inpCheek;
      if (!inpLip)
        return;
      this.updateCMFaceGloss[5] = inpLip;
    }

    public void AddUpdateCMFaceLayoutFlags(bool inpPaint01, bool inpPaint02, bool inpMole)
    {
      if (inpPaint01)
        this.updateCMFaceLayout[2] = inpPaint01;
      if (inpPaint02)
        this.updateCMFaceLayout[3] = inpPaint02;
      if (!inpMole)
        return;
      this.updateCMFaceLayout[6] = inpMole;
    }

    private bool InitBaseCustomTextureFace(
      string drawManifest,
      string drawAssetBundleName,
      string drawAssetName)
    {
      if (this.customTexCtrlFace != null)
      {
        this.customTexCtrlFace.Release();
        this.customTexCtrlFace = (CustomTextureControl) null;
      }
      this.customTexCtrlFace = new CustomTextureControl(2, drawManifest, drawAssetBundleName, drawAssetName, this.objRoot.get_transform());
      this.customTexCtrlFace.Initialize(0, "abdata", "chara/mm_base.unity3d", "create_skin_face", 2048, 2048, (RenderTextureFormat) 0);
      this.customTexCtrlFace.Initialize(1, "abdata", "chara/mm_base.unity3d", "create_skin detail_face", 2048, 2048, (RenderTextureFormat) 0);
      return true;
    }

    public bool CreateFaceTexture()
    {
      ChaFileFace.MakeupInfo makeup = this.fileFace.makeup;
      bool flag1 = false;
      bool flag2 = false;
      CustomTextureCreate ctc1 = this.customTexCtrlFace.createCustomTex[0];
      CustomTextureCreate ctc2 = this.customTexCtrlFace.createCustomTex[1];
      if (this.updateCMFaceTex[0])
      {
        if (this.SetCreateTexture(ctc1, true, this.sex != (byte) 0 ? ChaListDefine.CategoryNo.ft_skin_f : ChaListDefine.CategoryNo.mt_skin_f, this.fileFace.skinId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.MainTex, -1))
          flag1 = true;
        Texture2D texture2D = CommonLib.LoadAsset<Texture2D>("chara/etc.unity3d", "black2048", false, "abdata");
        Singleton<Character>.Instance.AddLoadAssetBundle("chara/etc.unity3d", "abdata");
        if (Object.op_Inequality((Object) null, (Object) texture2D))
        {
          ctc2.SetMainTexture((Texture) texture2D);
          flag2 = true;
        }
        this.ChangeTexture(this.customTexCtrlFace.matDraw, this.sex != (byte) 0 ? ChaListDefine.CategoryNo.ft_skin_f : ChaListDefine.CategoryNo.mt_skin_f, this.fileFace.skinId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.OcclusionMapTex, ChaShader.SkinOcclusionMapTex, string.Empty);
        this.ChangeTexture(this.customTexCtrlFace.matDraw, this.sex != (byte) 0 ? ChaListDefine.CategoryNo.ft_skin_f : ChaListDefine.CategoryNo.mt_skin_f, this.fileFace.skinId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.NormalMapTex, ChaShader.SkinNormalMapTex, string.Empty);
        this.updateCMFaceTex[0] = false;
      }
      if (this.updateCMFaceColor[0])
      {
        ctc1.SetColor(ChaShader.SkinColor, this.fileBody.skinColor);
        flag1 = true;
        this.updateCMFaceColor[0] = false;
      }
      if (this.updateCMFaceTex[1])
      {
        if (this.SetCreateTexture(ctc1, false, ChaListDefine.CategoryNo.st_eyeshadow, makeup.eyeshadowId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.AddTex, ChaShader.EyeshadowTex))
          flag1 = true;
        if (this.SetCreateTexture(ctc2, false, ChaListDefine.CategoryNo.st_eyeshadow, makeup.eyeshadowId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.GlossTex, ChaShader.EyeshadowTex))
          flag2 = true;
        this.updateCMFaceTex[1] = false;
      }
      if (this.updateCMFaceColor[1])
      {
        ctc1.SetColor(ChaShader.EyeshadowColor, makeup.eyeshadowColor);
        flag1 = true;
        this.updateCMFaceColor[1] = false;
      }
      if (this.updateCMFaceGloss[1])
      {
        ctc2.SetFloat(ChaShader.EyeshadowGloss, makeup.eyeshadowGloss);
        flag2 = true;
        this.updateCMFaceGloss[1] = false;
      }
      int[] numArray1 = new int[2]{ 2, 3 };
      int[] numArray2 = new int[2]
      {
        ChaShader.Paint01Tex,
        ChaShader.Paint02Tex
      };
      int[] numArray3 = new int[2]
      {
        ChaShader.Paint01Color,
        ChaShader.Paint02Color
      };
      int[] numArray4 = new int[2]
      {
        ChaShader.Paint01Gloass,
        ChaShader.Paint02Gloass
      };
      int[] numArray5 = new int[2]
      {
        ChaShader.Paint01Metallic,
        ChaShader.Paint02Metallic
      };
      int[] numArray6 = new int[2]
      {
        ChaShader.Paint01Layout,
        ChaShader.Paint02Layout
      };
      int[] numArray7 = new int[2]
      {
        ChaShader.Paint01Rot,
        ChaShader.Paint02Rot
      };
      for (int index = 0; index < 2; ++index)
      {
        if (this.updateCMFaceTex[numArray1[index]])
        {
          if (this.SetCreateTexture(ctc1, false, ChaListDefine.CategoryNo.st_paint, makeup.paintInfo[index].id, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.AddTex, numArray2[index]))
            flag1 = true;
          if (this.SetCreateTexture(ctc2, false, ChaListDefine.CategoryNo.st_paint, makeup.paintInfo[index].id, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.GlossTex, numArray2[index]))
            flag2 = true;
          this.updateCMFaceTex[numArray1[index]] = false;
        }
        if (this.updateCMFaceColor[numArray1[index]])
        {
          ctc1.SetColor(numArray3[index], makeup.paintInfo[index].color);
          this.updateCMFaceColor[numArray1[index]] = false;
          flag1 = true;
        }
        if (this.updateCMFaceGloss[numArray1[index]])
        {
          ctc2.SetFloat(numArray4[index], makeup.paintInfo[index].glossPower);
          ctc2.SetFloat(numArray5[index], makeup.paintInfo[index].metallicPower);
          this.updateCMFaceGloss[numArray1[index]] = false;
          flag2 = true;
        }
        if (this.updateCMFaceLayout[numArray1[index]])
        {
          Vector4 zero = Vector4.get_zero();
          zero.x = (__Null) (double) Mathf.Lerp(10f, 1f, (float) makeup.paintInfo[index].layout.x);
          zero.y = (__Null) (double) Mathf.Lerp(10f, 1f, (float) makeup.paintInfo[index].layout.y);
          zero.z = (__Null) (double) Mathf.Lerp(0.28f, -0.3f, (float) makeup.paintInfo[index].layout.z);
          zero.w = (__Null) (double) Mathf.Lerp(0.28f, -0.3f, (float) makeup.paintInfo[index].layout.w);
          float num = Mathf.Lerp(1f, -1f, makeup.paintInfo[index].rotation);
          ctc1.SetVector4(numArray6[index], zero);
          ctc1.SetFloat(numArray7[index], num);
          ctc2.SetVector4(numArray6[index], zero);
          ctc2.SetFloat(numArray7[index], num);
          this.updateCMFaceLayout[numArray1[index]] = false;
          flag1 = true;
          flag2 = true;
        }
      }
      if (this.updateCMFaceTex[4])
      {
        if (this.SetCreateTexture(ctc1, false, ChaListDefine.CategoryNo.st_cheek, makeup.cheekId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.AddTex, ChaShader.CheekTex))
          flag1 = true;
        if (this.SetCreateTexture(ctc2, false, ChaListDefine.CategoryNo.st_cheek, makeup.cheekId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.GlossTex, ChaShader.CheekTex))
          flag2 = true;
        this.updateCMFaceTex[4] = false;
      }
      if (this.updateCMFaceColor[4])
      {
        ctc1.SetColor(ChaShader.CheekColor, makeup.cheekColor);
        this.updateCMFaceColor[4] = false;
        flag1 = true;
      }
      if (this.updateCMFaceGloss[4])
      {
        ctc2.SetFloat(ChaShader.CheekGloss, makeup.cheekGloss);
        this.updateCMFaceGloss[4] = false;
        flag2 = true;
      }
      if (this.updateCMFaceTex[5])
      {
        if (this.SetCreateTexture(ctc1, false, ChaListDefine.CategoryNo.st_lip, makeup.lipId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.AddTex, ChaShader.LipTex))
          flag1 = true;
        if (this.SetCreateTexture(ctc2, false, ChaListDefine.CategoryNo.st_lip, makeup.lipId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.GlossTex, ChaShader.LipTex))
          flag2 = true;
        this.updateCMFaceTex[5] = false;
      }
      if (this.updateCMFaceColor[5])
      {
        ctc1.SetColor(ChaShader.LipColor, makeup.lipColor);
        this.updateCMFaceColor[5] = false;
        flag1 = true;
      }
      if (this.updateCMFaceGloss[5])
      {
        ctc2.SetFloat(ChaShader.LipGloss, makeup.lipGloss);
        this.updateCMFaceGloss[5] = false;
        flag2 = true;
      }
      if (this.updateCMFaceTex[6])
      {
        if (this.SetCreateTexture(ctc1, false, ChaListDefine.CategoryNo.st_mole, this.fileFace.moleId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.AddTex, ChaShader.MoleTex))
          flag1 = true;
        this.updateCMFaceTex[6] = false;
      }
      if (this.updateCMFaceColor[6])
      {
        ctc1.SetColor(ChaShader.MoleColor, this.fileFace.moleColor);
        this.updateCMFaceColor[6] = false;
        flag1 = true;
      }
      if (this.updateCMFaceLayout[6])
      {
        Vector4 vector4 = ctc1.GetVector4(ChaShader.MoleLayout);
        vector4.x = (__Null) (double) Mathf.Lerp(5f, 1f, (float) this.fileFace.moleLayout.x);
        vector4.y = (__Null) (double) Mathf.Lerp(5f, 1f, (float) this.fileFace.moleLayout.y);
        vector4.z = (__Null) (double) Mathf.Lerp(0.3f, -0.3f, (float) this.fileFace.moleLayout.z);
        vector4.w = (__Null) (double) Mathf.Lerp(0.3f, -0.3f, (float) this.fileFace.moleLayout.w);
        ctc1.SetVector4(ChaShader.MoleLayout, vector4);
        this.updateCMFaceLayout[6] = false;
        flag1 = true;
      }
      if (flag1)
        this.customTexCtrlFace.SetNewCreateTexture(0, ChaShader.SkinTex);
      if (flag2)
        this.customTexCtrlFace.SetNewCreateTexture(1, ChaShader.SkinCreateDetailTex);
      if (this.releaseCustomInputTexture)
        this.ReleaseFaceCustomTexture();
      return true;
    }

    public bool ChangeFaceGlossPower()
    {
      float num = Mathf.Lerp(0.0f, 0.8f, this.fileBody.skinGlossPower) + 0.2f * this.fileStatus.skinTuyaRate;
      this.customTexCtrlFace.matDraw.SetFloat(ChaShader.Gloss, num);
      return true;
    }

    public bool ChangeFaceMetallicPower()
    {
      this.customTexCtrlFace.matDraw.SetFloat(ChaShader.Metallic, this.fileBody.skinMetallicPower);
      return true;
    }

    public bool ChangeFaceDetailKind()
    {
      this.ChangeTexture(this.customTexCtrlFace.matDraw, this.sex != (byte) 0 ? ChaListDefine.CategoryNo.ft_detail_f : ChaListDefine.CategoryNo.mt_detail_f, this.fileFace.detailId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.AddTex, ChaShader.SkinDetailTex, string.Empty);
      return true;
    }

    public bool ChangeFaceDetailPower()
    {
      this.customTexCtrlFace.matDraw.SetFloat(ChaShader.SkinDetailPower, this.fileFace.detailPower);
      return true;
    }

    public bool ChangeEyebrowKind()
    {
      this.ChangeTexture(this.customTexCtrlFace.matDraw, ChaListDefine.CategoryNo.st_eyebrow, this.fileFace.eyebrowId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.AddTex, ChaShader.EyebrowTex, string.Empty);
      return true;
    }

    public bool ChangeEyebrowColor()
    {
      this.customTexCtrlFace.matDraw.SetColor(ChaShader.EyebrowColor, this.fileFace.eyebrowColor);
      return true;
    }

    public bool ChangeEyebrowLayout()
    {
      Vector4 vector = this.customTexCtrlFace.matDraw.GetVector(ChaShader.EyebrowLayout);
      vector.x = (__Null) (double) Mathf.Lerp(-0.2f, 0.2f, (float) this.fileFace.eyebrowLayout.x);
      vector.y = (__Null) (double) Mathf.Lerp(0.16f, 0.0f, (float) this.fileFace.eyebrowLayout.y);
      vector.z = (__Null) (double) Mathf.Lerp(2f, 0.5f, (float) this.fileFace.eyebrowLayout.z);
      vector.w = (__Null) (double) Mathf.Lerp(2f, 0.5f, (float) this.fileFace.eyebrowLayout.w);
      this.customTexCtrlFace.matDraw.SetVector(ChaShader.EyebrowLayout, vector);
      return true;
    }

    public bool ChangeEyebrowTilt()
    {
      float num = Mathf.Lerp(-0.15f, 0.15f, this.fileFace.eyebrowTilt);
      this.customTexCtrlFace.matDraw.SetFloat(ChaShader.EyebrowTilt, num);
      return true;
    }

    public bool ChangeWhiteEyesColor(int lr)
    {
      if (Object.op_Equality((Object) null, (Object) this.cmpFace))
        return false;
      Renderer[] rendEyes = this.cmpFace.targetCustom.rendEyes;
      if (rendEyes == null)
        return false;
      for (int index = 0; index < 2; ++index)
      {
        if ((lr == 2 || lr == index) && (!Object.op_Equality((Object) null, (Object) rendEyes[index]) && !Object.op_Equality((Object) null, (Object) rendEyes[index].get_material())))
          rendEyes[index].get_material().SetColor(ChaShader.EyesWhiteColor, this.fileFace.pupil[index].whiteColor);
      }
      return true;
    }

    public bool ChangeEyesKind(int lr)
    {
      if (Object.op_Equality((Object) null, (Object) this.cmpFace))
        return false;
      Renderer[] rendEyes = this.cmpFace.targetCustom.rendEyes;
      if (rendEyes == null)
        return false;
      for (int index = 0; index < 2; ++index)
      {
        if ((lr == 2 || lr == index) && (!Object.op_Equality((Object) null, (Object) rendEyes[index]) && !Object.op_Equality((Object) null, (Object) rendEyes[index].get_material())))
          this.ChangeTexture(rendEyes[index], ChaListDefine.CategoryNo.st_eye, this.fileFace.pupil[index].pupilId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.AddTex, ChaShader.PupilTex, string.Empty);
      }
      return true;
    }

    public bool ChangeEyesWH(int lr)
    {
      if (Object.op_Equality((Object) null, (Object) this.cmpFace))
        return false;
      Renderer[] rendEyes = this.cmpFace.targetCustom.rendEyes;
      if (rendEyes == null)
        return false;
      for (int index = 0; index < 2; ++index)
      {
        if ((lr == 2 || lr == index) && (!Object.op_Equality((Object) null, (Object) rendEyes[index]) && !Object.op_Equality((Object) null, (Object) rendEyes[index].get_material())))
        {
          Vector4 vector = rendEyes[index].get_material().GetVector(ChaShader.PupilLayout);
          vector.x = (__Null) (double) Mathf.Lerp(2f, 0.5f, this.fileFace.pupil[index].pupilW);
          vector.y = (__Null) (double) Mathf.Lerp(2f, 0.5f, this.fileFace.pupil[index].pupilH);
          rendEyes[index].get_material().SetVector(ChaShader.PupilLayout, vector);
        }
      }
      return true;
    }

    public bool ChangeEyesColor(int lr)
    {
      if (Object.op_Equality((Object) null, (Object) this.cmpFace))
        return false;
      Renderer[] rendEyes = this.cmpFace.targetCustom.rendEyes;
      if (rendEyes == null)
        return false;
      for (int index = 0; index < 2; ++index)
      {
        if ((lr == 2 || lr == index) && (!Object.op_Equality((Object) null, (Object) rendEyes[index]) && !Object.op_Equality((Object) null, (Object) rendEyes[index].get_material())))
          rendEyes[index].get_material().SetColor(ChaShader.PupilColor, this.fileFace.pupil[index].pupilColor);
      }
      return true;
    }

    public bool ChangeEyesEmission(int lr)
    {
      if (Object.op_Equality((Object) null, (Object) this.cmpFace))
        return false;
      Renderer[] rendEyes = this.cmpFace.targetCustom.rendEyes;
      if (rendEyes == null)
        return false;
      for (int index = 0; index < 2; ++index)
      {
        if ((lr == 2 || lr == index) && (!Object.op_Equality((Object) null, (Object) rendEyes[index]) && !Object.op_Equality((Object) null, (Object) rendEyes[index].get_material())))
          rendEyes[index].get_material().SetFloat(ChaShader.PupilEmission, this.fileFace.pupil[index].pupilEmission);
      }
      return true;
    }

    public bool ChangeBlackEyesKind(int lr)
    {
      if (Object.op_Equality((Object) null, (Object) this.cmpFace))
        return false;
      Renderer[] rendEyes = this.cmpFace.targetCustom.rendEyes;
      if (rendEyes == null)
        return false;
      for (int index = 0; index < 2; ++index)
      {
        if ((lr == 2 || lr == index) && (!Object.op_Equality((Object) null, (Object) rendEyes[index]) && !Object.op_Equality((Object) null, (Object) rendEyes[index].get_material())))
          this.ChangeTexture(rendEyes[index], ChaListDefine.CategoryNo.st_eyeblack, this.fileFace.pupil[index].blackId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.AddTex, ChaShader.PupilBlackTex, string.Empty);
      }
      return true;
    }

    public bool ChangeBlackEyesColor(int lr)
    {
      if (Object.op_Equality((Object) null, (Object) this.cmpFace))
        return false;
      Renderer[] rendEyes = this.cmpFace.targetCustom.rendEyes;
      if (rendEyes == null)
        return false;
      for (int index = 0; index < 2; ++index)
      {
        if ((lr == 2 || lr == index) && (!Object.op_Equality((Object) null, (Object) rendEyes[index]) && !Object.op_Equality((Object) null, (Object) rendEyes[index].get_material())))
          rendEyes[index].get_material().SetColor(ChaShader.PupilBlackColor, this.fileFace.pupil[index].blackColor);
      }
      return true;
    }

    public bool ChangeBlackEyesWH(int lr)
    {
      if (Object.op_Equality((Object) null, (Object) this.cmpFace))
        return false;
      Renderer[] rendEyes = this.cmpFace.targetCustom.rendEyes;
      if (rendEyes == null)
        return false;
      for (int index = 0; index < 2; ++index)
      {
        if ((lr == 2 || lr == index) && (!Object.op_Equality((Object) null, (Object) rendEyes[index]) && !Object.op_Equality((Object) null, (Object) rendEyes[index].get_material())))
        {
          Vector4 vector = rendEyes[index].get_material().GetVector(ChaShader.PupilBlackLayout);
          vector.x = (__Null) (double) Mathf.Lerp(4f, 0.4f, this.fileFace.pupil[index].blackW);
          vector.y = (__Null) (double) Mathf.Lerp(4f, 0.4f, this.fileFace.pupil[index].blackH);
          rendEyes[index].get_material().SetVector(ChaShader.PupilBlackLayout, vector);
        }
      }
      return true;
    }

    public bool ChangeEyesBasePosY()
    {
      if (Object.op_Equality((Object) null, (Object) this.cmpFace))
        return false;
      Renderer[] rendEyes = this.cmpFace.targetCustom.rendEyes;
      if (rendEyes == null)
        return false;
      for (int index = 0; index < 2; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) rendEyes[index]) && !Object.op_Equality((Object) null, (Object) rendEyes[index].get_material()))
        {
          Vector4 vector = rendEyes[index].get_material().GetVector(ChaShader.PupilLayout);
          vector.w = (__Null) (double) Mathf.Lerp(0.5f, -0.5f, this.fileFace.pupilY);
          rendEyes[index].get_material().SetVector(ChaShader.PupilLayout, vector);
        }
      }
      return true;
    }

    public bool ChangeEyesHighlightKind()
    {
      if (Object.op_Equality((Object) null, (Object) this.cmpFace))
        return false;
      Renderer[] rendEyes = this.cmpFace.targetCustom.rendEyes;
      if (rendEyes == null)
        return false;
      for (int index = 0; index < 2; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) rendEyes[index]) && !Object.op_Equality((Object) null, (Object) rendEyes[index].get_material()))
          this.ChangeTexture(rendEyes[index], ChaListDefine.CategoryNo.st_eye_hl, this.fileFace.hlId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.AddTex, ChaShader.EyesHighlightTex, string.Empty);
      }
      return true;
    }

    public bool ChangeEyesHighlightColor()
    {
      if (Object.op_Equality((Object) null, (Object) this.cmpFace))
        return false;
      Renderer[] rendEyes = this.cmpFace.targetCustom.rendEyes;
      if (rendEyes == null)
        return false;
      for (int index = 0; index < 2; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) rendEyes[index]) && !Object.op_Equality((Object) null, (Object) rendEyes[index].get_material()))
          rendEyes[index].get_material().SetColor(ChaShader.EyesHighlightColor, this.fileFace.hlColor);
      }
      return true;
    }

    public bool ChangeEyesHighlighLayout()
    {
      if (Object.op_Equality((Object) null, (Object) this.cmpFace))
        return false;
      Renderer[] rendEyes = this.cmpFace.targetCustom.rendEyes;
      if (rendEyes == null)
        return false;
      Vector4 vector4;
      vector4.x = (__Null) (double) Mathf.Lerp(1.8f, 0.2f, (float) this.fileFace.hlLayout.x);
      vector4.y = (__Null) (double) Mathf.Lerp(1.8f, 0.2f, (float) this.fileFace.hlLayout.y);
      vector4.z = (__Null) (double) Mathf.Lerp(-0.3f, 0.3f, (float) this.fileFace.hlLayout.z);
      vector4.w = (__Null) (double) Mathf.Lerp(-0.3f, 0.3f, (float) this.fileFace.hlLayout.w);
      for (int index = 0; index < 2; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) rendEyes[index]) && !Object.op_Equality((Object) null, (Object) rendEyes[index].get_material()))
          rendEyes[index].get_material().SetVector(ChaShader.EyesHighlightLayout, vector4);
      }
      return true;
    }

    public bool ChangeEyesHighlighTilt()
    {
      if (Object.op_Equality((Object) null, (Object) this.cmpFace))
        return false;
      Renderer[] rendEyes = this.cmpFace.targetCustom.rendEyes;
      if (rendEyes == null)
        return false;
      float num = Mathf.Lerp(-1f, 1f, this.fileFace.hlTilt);
      for (int index = 0; index < 2; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) rendEyes[index]) && !Object.op_Equality((Object) null, (Object) rendEyes[index].get_material()))
          rendEyes[index].get_material().SetFloat(ChaShader.EyesHighlightTilt, num);
      }
      return true;
    }

    public bool ChangeEyesShadowRange()
    {
      if (Object.op_Equality((Object) null, (Object) this.cmpFace))
        return false;
      Renderer rendShadow = this.cmpFace.targetCustom.rendShadow;
      if (Object.op_Equality((Object) null, (Object) rendShadow) || Object.op_Equality((Object) null, (Object) rendShadow.get_material()))
        return false;
      float num = Mathf.Lerp(0.1f, 0.9f, this.fileFace.whiteShadowScale);
      rendShadow.get_material().SetFloat(ChaShader.EyesShadowRange, num);
      return true;
    }

    public bool ChangeEyelashesKind()
    {
      if (Object.op_Equality((Object) null, (Object) this.cmpFace))
        return false;
      Renderer rendEyelashes = this.cmpFace.targetCustom.rendEyelashes;
      if (Object.op_Equality((Object) null, (Object) rendEyelashes) || Object.op_Equality((Object) null, (Object) rendEyelashes.get_material()))
        return false;
      this.ChangeTexture(rendEyelashes, ChaListDefine.CategoryNo.st_eyelash, this.fileFace.eyelashesId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.AddTex, ChaShader.EyelashesTex, string.Empty);
      return true;
    }

    public bool ChangeEyelashesColor()
    {
      if (Object.op_Equality((Object) null, (Object) this.cmpFace))
        return false;
      Renderer rendEyelashes = this.cmpFace.targetCustom.rendEyelashes;
      if (Object.op_Equality((Object) null, (Object) rendEyelashes) || Object.op_Equality((Object) null, (Object) rendEyelashes.get_material()))
        return false;
      rendEyelashes.get_material().SetColor(ChaShader.EyelashesColor, this.fileFace.eyelashesColor);
      return true;
    }

    public bool ChangeBeardKind()
    {
      this.ChangeTexture(this.customTexCtrlFace.matDraw, ChaListDefine.CategoryNo.mt_beard, this.fileFace.beardId, ChaListDefine.KeyType.MainManifest, ChaListDefine.KeyType.MainAB, ChaListDefine.KeyType.AddTex, ChaShader.BeardTex, string.Empty);
      return true;
    }

    public bool ChangeBeardColor()
    {
      this.customTexCtrlFace.matDraw.SetColor(ChaShader.BeardColor, this.fileFace.beardColor);
      return true;
    }

    public bool SetFaceBaseMaterial()
    {
      if (Object.op_Equality((Object) null, (Object) this.customMatFace) || Object.op_Equality((Object) null, (Object) this.cmpFace))
        return false;
      Renderer rendHead = this.cmpFace.targetCustom.rendHead;
      return !Object.op_Equality((Object) null, (Object) rendHead) && this.SetBaseMaterial(rendHead, this.customMatFace);
    }

    public bool ReleaseFaceCustomTexture()
    {
      if (this.customTexCtrlFace == null)
        return false;
      CustomTextureCreate customTextureCreate1 = this.customTexCtrlFace.createCustomTex[0];
      CustomTextureCreate customTextureCreate2 = this.customTexCtrlFace.createCustomTex[1];
      customTextureCreate1.SetTexture(ChaShader.MainTex, (Texture) null);
      customTextureCreate1.SetTexture(ChaShader.EyeshadowTex, (Texture) null);
      customTextureCreate1.SetTexture(ChaShader.Paint01Tex, (Texture) null);
      customTextureCreate1.SetTexture(ChaShader.Paint02Tex, (Texture) null);
      customTextureCreate1.SetTexture(ChaShader.CheekTex, (Texture) null);
      customTextureCreate1.SetTexture(ChaShader.LipTex, (Texture) null);
      customTextureCreate1.SetTexture(ChaShader.MoleTex, (Texture) null);
      customTextureCreate2.SetTexture(ChaShader.MainTex, (Texture) null);
      customTextureCreate2.SetTexture(ChaShader.EyeshadowTex, (Texture) null);
      customTextureCreate2.SetTexture(ChaShader.Paint01Tex, (Texture) null);
      customTextureCreate2.SetTexture(ChaShader.Paint02Tex, (Texture) null);
      customTextureCreate2.SetTexture(ChaShader.CheekTex, (Texture) null);
      customTextureCreate2.SetTexture(ChaShader.LipTex, (Texture) null);
      OutputLog.Log("ReleaseBodyCustomTexture", false, "UnloadUnusedAssets");
      Resources.UnloadUnusedAssets();
      return true;
    }

    public void ChangeCustomFaceWithoutCustomTexture()
    {
      this.ChangeFaceGlossPower();
      this.ChangeFaceMetallicPower();
      this.ChangeFaceDetailKind();
      this.ChangeFaceDetailPower();
      this.ChangeEyebrowKind();
      this.ChangeEyebrowColor();
      this.ChangeEyebrowLayout();
      this.ChangeEyebrowTilt();
      this.ChangeWhiteEyesColor(2);
      this.ChangeEyesKind(2);
      this.ChangeEyesWH(2);
      this.ChangeEyesColor(2);
      this.ChangeEyesEmission(2);
      this.ChangeBlackEyesKind(2);
      this.ChangeBlackEyesColor(2);
      this.ChangeBlackEyesWH(2);
      this.ChangeEyesBasePosY();
      this.ChangeEyesHighlightKind();
      this.ChangeEyesHighlightColor();
      this.ChangeEyesHighlighLayout();
      this.ChangeEyesHighlighTilt();
      this.ChangeEyesShadowRange();
      this.ChangeEyelashesKind();
      this.ChangeEyelashesColor();
      if (this.sex != (byte) 0)
        return;
      this.ChangeBeardKind();
      this.ChangeBeardColor();
    }

    public bool InitShapeFace(
      Transform trfBone,
      string manifest,
      string assetBundleAnmShapeFace,
      string assetAnmShapeFace)
    {
      if (this.sibFace == null || Object.op_Equality((Object) null, (Object) trfBone))
        return false;
      string cateInfoName = ChaABDefine.ShapeHeadListAsset((int) this.sex);
      this.sibFace.InitShapeInfo(manifest, assetBundleAnmShapeFace, "list/customshape.unity3d", assetAnmShapeFace, cateInfoName, trfBone);
      for (int category = 0; category < this.ShapeFaceNum; ++category)
        this.sibFace.ChangeValue(category, this.fileFace.shapeValueFace[category]);
      this.updateShapeFace = true;
      return true;
    }

    public void ReleaseShapeFace()
    {
      if (this.sibFace == null)
        return;
      this.sibFace.ReleaseShapeInfo();
    }

    public bool SetShapeFaceValue(int index, float value)
    {
      if (index >= this.ShapeFaceNum)
        return false;
      this.fileFace.shapeValueFace[index] = value;
      if (this.sibFace != null && this.sibFace.InitEnd)
        this.sibFace.ChangeValue(index, value);
      this.updateShapeFace = true;
      return true;
    }

    public bool UpdateShapeFaceValueFromCustomInfo()
    {
      if (this.sibFace == null || !this.sibFace.InitEnd)
        return false;
      for (int category = 0; category < this.fileFace.shapeValueFace.Length; ++category)
        this.sibFace.ChangeValue(category, this.fileFace.shapeValueFace[category]);
      this.updateShapeFace = true;
      return true;
    }

    public float GetShapeFaceValue(int index)
    {
      return index >= this.ShapeFaceNum ? 0.0f : this.fileFace.shapeValueFace[index];
    }

    public void UpdateShapeFace()
    {
      if (this.sibFace == null || !this.sibFace.InitEnd)
        return;
      if (this.fileStatus.disableMouthShapeMask)
      {
        for (int index = 0; index < ChaFileDefine.cf_MouthShapeMaskID.Length; ++index)
          this.sibFace.ChangeValue(ChaFileDefine.cf_MouthShapeMaskID[index], ChaFileDefine.cf_MouthShapeDefault[index]);
      }
      else
      {
        foreach (int category in ChaFileDefine.cf_MouthShapeMaskID)
          this.sibFace.ChangeValue(category, this.fileFace.shapeValueFace[category]);
      }
      this.sibFace.Update();
    }

    public void DisableShapeMouth(bool disable)
    {
      this.updateShapeFace = true;
      this.fileStatus.disableMouthShapeMask = disable;
    }

    protected void InitializeControlCustomHairAll()
    {
      this.InitializeControlCustomHairObject();
    }

    protected void InitializeControlCustomHairObject()
    {
    }

    protected void ReleaseControlCustomHairAll()
    {
      this.ReleaseControlCustomHairObject(true);
    }

    protected void ReleaseControlCustomHairObject(bool init = true)
    {
      if (!init)
        return;
      this.InitializeControlCustomHairObject();
    }

    public void ChangeSettingHairShader()
    {
      ChaFileHair hair = this.chaFile.custom.hair;
      Shader shader = hair.shaderType != 0 ? Singleton<Character>.Instance.shaderCutout : Singleton<Character>.Instance.shaderDithering;
      for (int parts = 0; parts < this.cmpHair.Length; ++parts)
      {
        if (!Object.op_Equality((Object) null, (Object) this.cmpHair[parts]))
        {
          CmpHair customHairComponent = this.GetCustomHairComponent(parts);
          if (!Object.op_Equality((Object) null, (Object) customHairComponent) && customHairComponent.rendHair != null && (customHairComponent.rendHair.Length != 0 && this.infoHair != null) && this.infoHair[parts] != null)
          {
            string info1 = this.infoHair[parts].GetInfo(ChaListDefine.KeyType.TexManifest);
            string info2 = this.infoHair[parts].GetInfo(ChaListDefine.KeyType.TexAB);
            string info3 = this.infoHair[parts].GetInfo(hair.shaderType != 0 ? ChaListDefine.KeyType.TexC : ChaListDefine.KeyType.TexD);
            int num = this.infoHair[parts].GetInfoInt(ChaListDefine.KeyType.RingOff) != 1 ? 0 : 1;
            Texture2D texture2D = CommonLib.LoadAsset<Texture2D>(info2, info3, false, info1);
            Singleton<Character>.Instance.AddLoadAssetBundle(info2, info1);
            for (int index1 = 0; index1 < customHairComponent.rendHair.Length; ++index1)
            {
              for (int index2 = 0; index2 < customHairComponent.rendHair[index1].get_materials().Length; ++index2)
              {
                int renderQueue = customHairComponent.rendHair[index1].get_materials()[index2].get_renderQueue();
                customHairComponent.rendHair[index1].get_materials()[index2].set_shader(shader);
                customHairComponent.rendHair[index1].get_materials()[index2].SetTexture(ChaShader.MainTex, (Texture) texture2D);
                if (customHairComponent.rendHair[index1].get_materials()[index2].HasProperty(ChaShader.HairRingoff))
                  customHairComponent.rendHair[index1].get_materials()[index2].SetInt(ChaShader.HairRingoff, num);
                customHairComponent.rendHair[index1].get_materials()[index2].set_renderQueue(renderQueue);
              }
            }
          }
        }
      }
    }

    public void ChangeSettingHairColor(int parts, bool _main, bool _top, bool _under)
    {
      CmpHair customHairComponent = this.GetCustomHairComponent(parts);
      if (Object.op_Equality((Object) null, (Object) customHairComponent) || customHairComponent.rendHair == null || customHairComponent.rendHair.Length == 0)
        return;
      ChaFileHair hair = this.chaFile.custom.hair;
      for (int index1 = 0; index1 < customHairComponent.rendHair.Length; ++index1)
      {
        if (_main)
        {
          if (1.0 > hair.parts[parts].baseColor.a)
            hair.parts[parts].baseColor = new Color((float) hair.parts[parts].baseColor.r, (float) hair.parts[parts].baseColor.g, (float) hair.parts[parts].baseColor.b, 1f);
          for (int index2 = 0; index2 < customHairComponent.rendHair[index1].get_materials().Length; ++index2)
            customHairComponent.rendHair[index1].get_materials()[index2].SetColor(ChaShader.HairMainColor, hair.parts[parts].baseColor);
        }
        if (customHairComponent.useTopColor && _top)
        {
          if (1.0 > hair.parts[parts].topColor.a)
            hair.parts[parts].topColor = new Color((float) hair.parts[parts].topColor.r, (float) hair.parts[parts].topColor.g, (float) hair.parts[parts].topColor.b, 1f);
          for (int index2 = 0; index2 < customHairComponent.rendHair[index1].get_materials().Length; ++index2)
            customHairComponent.rendHair[index1].get_materials()[index2].SetColor(ChaShader.HairTopColor, hair.parts[parts].topColor);
        }
        if (customHairComponent.useUnderColor && _under)
        {
          if (1.0 > hair.parts[parts].underColor.a)
            hair.parts[parts].underColor = new Color((float) hair.parts[parts].underColor.r, (float) hair.parts[parts].underColor.g, (float) hair.parts[parts].underColor.b, 1f);
          for (int index2 = 0; index2 < customHairComponent.rendHair[index1].get_materials().Length; ++index2)
            customHairComponent.rendHair[index1].get_materials()[index2].SetColor(ChaShader.HairUnderColor, hair.parts[parts].underColor);
        }
      }
    }

    public void CreateHairColor(int parts, Color color)
    {
      ChaFileHair hair = this.chaFile.custom.hair;
      hair.parts[parts].baseColor = new Color((float) color.r, (float) color.g, (float) color.b, 1f);
      Color topColor;
      Color underColor;
      Color specular;
      this.CreateHairColor(color, out topColor, out underColor, out specular);
      hair.parts[parts].topColor = topColor;
      hair.parts[parts].underColor = underColor;
      hair.parts[parts].specular = specular;
    }

    public void CreateHairColor(
      Color baseColor,
      out Color topColor,
      out Color underColor,
      out Color specular)
    {
      float num1;
      float num2;
      float num3;
      Color.RGBToHSV(baseColor, ref num1, ref num2, ref num3);
      topColor = Color.HSVToRGB(num1, num2, Mathf.Max(num3 - 0.15f, 0.0f));
      underColor = Color.HSVToRGB(num1, Mathf.Max(num2 - 0.1f, 0.0f), Mathf.Min(num3 + 0.44f, 1f));
      specular = Color.HSVToRGB(num1, num2, Mathf.Min(num3 + 0.17f, 1f));
    }

    public void ChangeSettingHairSpecular(int parts)
    {
      CmpHair customHairComponent = this.GetCustomHairComponent(parts);
      if (Object.op_Equality((Object) null, (Object) customHairComponent) || customHairComponent.rendHair == null || customHairComponent.rendHair.Length == 0)
        return;
      ChaFileHair hair = this.chaFile.custom.hair;
      for (int index1 = 0; index1 < customHairComponent.rendHair.Length; ++index1)
      {
        if (1.0 > hair.parts[parts].specular.a)
          hair.parts[parts].specular = new Color((float) hair.parts[parts].specular.r, (float) hair.parts[parts].specular.g, (float) hair.parts[parts].specular.b, 1f);
        for (int index2 = 0; index2 < customHairComponent.rendHair[index1].get_materials().Length; ++index2)
          customHairComponent.rendHair[index1].get_materials()[index2].SetColor(ChaShader.Specular, hair.parts[parts].specular);
      }
    }

    public void ChangeSettingHairMetallic(int parts)
    {
      CmpHair customHairComponent = this.GetCustomHairComponent(parts);
      if (Object.op_Equality((Object) null, (Object) customHairComponent) || customHairComponent.rendHair == null || customHairComponent.rendHair.Length == 0)
        return;
      ChaFileHair hair = this.chaFile.custom.hair;
      for (int index1 = 0; index1 < customHairComponent.rendHair.Length; ++index1)
      {
        for (int index2 = 0; index2 < customHairComponent.rendHair[index1].get_materials().Length; ++index2)
          customHairComponent.rendHair[index1].get_materials()[index2].SetFloat(ChaShader.Metallic, hair.parts[parts].metallic);
      }
    }

    public void ChangeSettingHairSmoothness(int parts)
    {
      CmpHair customHairComponent = this.GetCustomHairComponent(parts);
      if (Object.op_Equality((Object) null, (Object) customHairComponent) || customHairComponent.rendHair == null || customHairComponent.rendHair.Length == 0)
        return;
      ChaFileHair hair = this.chaFile.custom.hair;
      for (int index1 = 0; index1 < customHairComponent.rendHair.Length; ++index1)
      {
        for (int index2 = 0; index2 < customHairComponent.rendHair[index1].get_materials().Length; ++index2)
          customHairComponent.rendHair[index1].get_materials()[index2].SetFloat(ChaShader.Smoothness, hair.parts[parts].smoothness);
      }
    }

    public int GetHairAcsColorNum(int parts)
    {
      CmpHair customHairComponent = this.GetCustomHairComponent(parts);
      return Object.op_Equality((Object) null, (Object) customHairComponent) || customHairComponent.acsDefColor == null || customHairComponent.acsDefColor.Length == 0 ? 0 : customHairComponent.acsDefColor.Length;
    }

    public void SetHairAcsDefaultColorParameterOnly(int parts)
    {
      ChaFileHair hair = this.chaFile.custom.hair;
      CmpHair customHairComponent = this.GetCustomHairComponent(parts);
      if (Object.op_Equality((Object) null, (Object) customHairComponent))
        return;
      int length = customHairComponent.acsDefColor.Length;
      for (int index = 0; index < length; ++index)
      {
        if (customHairComponent.acsDefColor != null)
          hair.parts[parts].acsColorInfo[index].color = customHairComponent.acsDefColor[index];
      }
    }

    public void ChangeSettingHairAcsColor(int parts)
    {
      if (this.cmpHair == null || Object.op_Equality((Object) null, (Object) this.cmpHair[parts]))
        return;
      int hairAcsColorNum = this.GetHairAcsColorNum(parts);
      if (hairAcsColorNum == 0)
        return;
      CmpHair customHairComponent = this.GetCustomHairComponent(parts);
      if (Object.op_Equality((Object) null, (Object) customHairComponent))
        return;
      int[] numArray = new int[3]
      {
        ChaShader.Color,
        ChaShader.Color2,
        ChaShader.Color3
      };
      bool[] flagArray = new bool[3]
      {
        this.cmpHair[parts].useAcsColor01,
        this.cmpHair[parts].useAcsColor02,
        this.cmpHair[parts].useAcsColor03
      };
      for (int index1 = 0; index1 < customHairComponent.rendAccessory.Length; ++index1)
      {
        for (int index2 = 0; index2 < hairAcsColorNum; ++index2)
        {
          if (flagArray[index2])
          {
            if (1.0 > this.fileHair.parts[parts].acsColorInfo[index2].color.a)
              this.fileHair.parts[parts].acsColorInfo[index2].color = new Color((float) this.fileHair.parts[parts].acsColorInfo[index2].color.r, (float) this.fileHair.parts[parts].acsColorInfo[index2].color.g, (float) this.fileHair.parts[parts].acsColorInfo[index2].color.b, 1f);
            foreach (Material material in customHairComponent.rendAccessory[index1].get_materials())
              material.SetColor(numArray[index2], this.fileHair.parts[parts].acsColorInfo[index2].color);
          }
        }
      }
    }

    public void ChangeSettingHairCorrectPos(int parts, int idx)
    {
      ChaFileHair.PartsInfo.BundleInfo bundleInfo;
      if (this.cmpHair == null || this.cmpHair.Length <= parts || (Object.op_Equality((Object) null, (Object) this.cmpHair[parts]) || this.cmpHair[parts].boneInfo == null) || (this.cmpHair[parts].boneInfo.Length <= idx || !this.chaFile.custom.hair.parts[parts].dictBundle.TryGetValue(idx, out bundleInfo)))
        return;
      this.cmpHair[parts].boneInfo[idx].moveRate = bundleInfo.moveRate;
    }

    public void ChangeSettingHairCorrectPosAll(int parts)
    {
      if (this.cmpHair == null || this.cmpHair.Length <= parts || Object.op_Equality((Object) null, (Object) this.cmpHair[parts]))
        return;
      ChaFileHair hair = this.chaFile.custom.hair;
      if (this.cmpHair[parts].boneInfo == null || this.cmpHair[parts].boneInfo.Length != hair.parts[parts].dictBundle.Count)
        return;
      for (int key = 0; key < this.cmpHair[parts].boneInfo.Length; ++key)
      {
        ChaFileHair.PartsInfo.BundleInfo bundleInfo;
        if (hair.parts[parts].dictBundle.TryGetValue(key, out bundleInfo))
          this.cmpHair[parts].boneInfo[key].moveRate = bundleInfo.moveRate;
      }
    }

    public bool SetHairCorrectPosValue(int parts, int idx, Vector3 val, int _flag)
    {
      ChaFileHair.PartsInfo.BundleInfo bundleInfo;
      if (this.cmpHair == null || this.cmpHair.Length <= parts || (Object.op_Equality((Object) null, (Object) this.cmpHair[parts]) || this.cmpHair[parts].boneInfo == null) || (this.cmpHair[parts].boneInfo.Length <= idx || !this.chaFile.custom.hair.parts[parts].dictBundle.TryGetValue(idx, out bundleInfo)))
        return false;
      val = this.cmpHair[parts].boneInfo[idx].trfCorrect.get_parent().InverseTransformPoint((float) val.x, (float) val.y, (float) val.z);
      Vector3 moveRate = bundleInfo.moveRate;
      if ((_flag & 1) != 0)
        moveRate.x = (__Null) (double) Mathf.InverseLerp((float) this.cmpHair[parts].boneInfo[idx].posMin.x, (float) this.cmpHair[parts].boneInfo[idx].posMax.x, (float) val.x);
      if ((_flag & 2) != 0)
        moveRate.y = (__Null) (double) Mathf.InverseLerp((float) this.cmpHair[parts].boneInfo[idx].posMin.y, (float) this.cmpHair[parts].boneInfo[idx].posMax.y, (float) val.y);
      if ((_flag & 4) != 0)
        moveRate.z = (__Null) (double) Mathf.InverseLerp((float) this.cmpHair[parts].boneInfo[idx].posMin.z, (float) this.cmpHair[parts].boneInfo[idx].posMax.z, (float) val.z);
      bundleInfo.moveRate = moveRate;
      return true;
    }

    public bool GetDefaultHairCorrectPosRate(int parts, int idx, out Vector3 v)
    {
      v = Vector3.get_zero();
      if (this.cmpHair == null || this.cmpHair.Length <= parts || (Object.op_Equality((Object) null, (Object) this.cmpHair[parts]) || this.cmpHair[parts].boneInfo == null) || (this.cmpHair[parts].boneInfo.Length <= idx || !this.chaFile.custom.hair.parts[parts].dictBundle.TryGetValue(idx, out ChaFileHair.PartsInfo.BundleInfo _)))
        return false;
      v.x = (__Null) (double) Mathf.InverseLerp((float) this.cmpHair[parts].boneInfo[idx].posMin.x, (float) this.cmpHair[parts].boneInfo[idx].posMax.x, (float) this.cmpHair[parts].boneInfo[idx].basePos.x);
      v.y = (__Null) (double) Mathf.InverseLerp((float) this.cmpHair[parts].boneInfo[idx].posMin.y, (float) this.cmpHair[parts].boneInfo[idx].posMax.y, (float) this.cmpHair[parts].boneInfo[idx].basePos.y);
      v.z = (__Null) (double) Mathf.InverseLerp((float) this.cmpHair[parts].boneInfo[idx].posMin.z, (float) this.cmpHair[parts].boneInfo[idx].posMax.z, (float) this.cmpHair[parts].boneInfo[idx].basePos.z);
      return true;
    }

    public void SetDefaultHairCorrectPosRate(int parts, int idx)
    {
      Vector3 v;
      ChaFileHair.PartsInfo.BundleInfo bundleInfo;
      if (!this.GetDefaultHairCorrectPosRate(parts, idx, out v) || !this.chaFile.custom.hair.parts[parts].dictBundle.TryGetValue(idx, out bundleInfo))
        return;
      bundleInfo.moveRate = v;
    }

    public void SetDefaultHairCorrectPosRateAll(int parts)
    {
      if (this.cmpHair == null || this.cmpHair.Length <= parts || Object.op_Equality((Object) null, (Object) this.cmpHair[parts]))
        return;
      ChaFileHair hair = this.chaFile.custom.hair;
      if (this.cmpHair[parts].boneInfo == null || this.cmpHair[parts].boneInfo.Length != hair.parts[parts].dictBundle.Count)
        return;
      for (int index = 0; index < this.cmpHair[parts].boneInfo.Length; ++index)
      {
        ChaFileHair.PartsInfo.BundleInfo bundleInfo;
        Vector3 v;
        if (hair.parts[parts].dictBundle.TryGetValue(index, out bundleInfo) && this.GetDefaultHairCorrectPosRate(parts, index, out v))
          bundleInfo.moveRate = v;
      }
    }

    public void ChangeSettingHairCorrectRot(int parts, int idx)
    {
      ChaFileHair.PartsInfo.BundleInfo bundleInfo;
      if (this.cmpHair == null || this.cmpHair.Length <= parts || (Object.op_Equality((Object) null, (Object) this.cmpHair[parts]) || this.cmpHair[parts].boneInfo == null) || (this.cmpHair[parts].boneInfo.Length <= idx || !this.chaFile.custom.hair.parts[parts].dictBundle.TryGetValue(idx, out bundleInfo)))
        return;
      this.cmpHair[parts].boneInfo[idx].rotRate = bundleInfo.rotRate;
    }

    public void ChangeSettingHairCorrectRotAll(int parts)
    {
      if (this.cmpHair == null || this.cmpHair.Length <= parts || Object.op_Equality((Object) null, (Object) this.cmpHair[parts]))
        return;
      ChaFileHair hair = this.chaFile.custom.hair;
      if (this.cmpHair[parts].boneInfo == null || this.cmpHair[parts].boneInfo.Length != hair.parts[parts].dictBundle.Count)
        return;
      for (int key = 0; key < this.cmpHair[parts].boneInfo.Length; ++key)
      {
        ChaFileHair.PartsInfo.BundleInfo bundleInfo;
        if (hair.parts[parts].dictBundle.TryGetValue(key, out bundleInfo))
          this.cmpHair[parts].boneInfo[key].rotRate = bundleInfo.rotRate;
      }
    }

    public bool SetHairCorrectRotValue(int parts, int idx, Vector3 val, int _flag)
    {
      ChaFileHair.PartsInfo.BundleInfo bundleInfo;
      if (this.cmpHair == null || this.cmpHair.Length <= parts || (Object.op_Equality((Object) null, (Object) this.cmpHair[parts]) || this.cmpHair[parts].boneInfo == null) || (this.cmpHair[parts].boneInfo.Length <= idx || !this.chaFile.custom.hair.parts[parts].dictBundle.TryGetValue(idx, out bundleInfo)))
        return false;
      val.x = val.x <= 180.0 ? (__Null) (double) val.x : (__Null) (val.x - 360.0);
      val.y = val.y <= 180.0 ? (__Null) (double) val.y : (__Null) (val.y - 360.0);
      val.z = val.z <= 180.0 ? (__Null) (double) val.z : (__Null) (val.z - 360.0);
      Vector3 rotRate = bundleInfo.rotRate;
      if ((_flag & 1) != 0)
        rotRate.x = (__Null) (double) Mathf.InverseLerp((float) this.cmpHair[parts].boneInfo[idx].rotMin.x, (float) this.cmpHair[parts].boneInfo[idx].rotMax.x, (float) val.x);
      if ((_flag & 2) != 0)
        rotRate.y = (__Null) (double) Mathf.InverseLerp((float) this.cmpHair[parts].boneInfo[idx].rotMin.y, (float) this.cmpHair[parts].boneInfo[idx].rotMax.y, (float) val.y);
      if ((_flag & 4) != 0)
        rotRate.z = (__Null) (double) Mathf.InverseLerp((float) this.cmpHair[parts].boneInfo[idx].rotMin.z, (float) this.cmpHair[parts].boneInfo[idx].rotMax.z, (float) val.z);
      bundleInfo.rotRate = rotRate;
      return true;
    }

    public bool GetDefaultHairCorrectRotRate(int parts, int idx, out Vector3 v)
    {
      v = Vector3.get_zero();
      if (this.cmpHair == null || this.cmpHair.Length <= parts || (Object.op_Equality((Object) null, (Object) this.cmpHair[parts]) || this.cmpHair[parts].boneInfo == null) || (this.cmpHair[parts].boneInfo.Length <= idx || !this.chaFile.custom.hair.parts[parts].dictBundle.TryGetValue(idx, out ChaFileHair.PartsInfo.BundleInfo _)))
        return false;
      v.x = (__Null) (double) Mathf.InverseLerp((float) this.cmpHair[parts].boneInfo[idx].rotMin.x, (float) this.cmpHair[parts].boneInfo[idx].rotMax.x, (float) this.cmpHair[parts].boneInfo[idx].baseRot.x);
      v.y = (__Null) (double) Mathf.InverseLerp((float) this.cmpHair[parts].boneInfo[idx].rotMin.y, (float) this.cmpHair[parts].boneInfo[idx].rotMax.y, (float) this.cmpHair[parts].boneInfo[idx].baseRot.y);
      v.z = (__Null) (double) Mathf.InverseLerp((float) this.cmpHair[parts].boneInfo[idx].rotMin.z, (float) this.cmpHair[parts].boneInfo[idx].rotMax.z, (float) this.cmpHair[parts].boneInfo[idx].baseRot.z);
      return true;
    }

    public void SetDefaultHairCorrectRotRate(int parts, int idx)
    {
      Vector3 v;
      ChaFileHair.PartsInfo.BundleInfo bundleInfo;
      if (!this.GetDefaultHairCorrectRotRate(parts, idx, out v) || !this.chaFile.custom.hair.parts[parts].dictBundle.TryGetValue(idx, out bundleInfo))
        return;
      bundleInfo.rotRate = v;
    }

    public void SetDefaultHairCorrectRotRateAll(int parts)
    {
      if (this.cmpHair == null || this.cmpHair.Length <= parts || Object.op_Equality((Object) null, (Object) this.cmpHair[parts]))
        return;
      ChaFileHair hair = this.chaFile.custom.hair;
      if (this.cmpHair[parts].boneInfo == null || this.cmpHair[parts].boneInfo.Length != hair.parts[parts].dictBundle.Count)
        return;
      for (int index = 0; index < this.cmpHair[parts].boneInfo.Length; ++index)
      {
        ChaFileHair.PartsInfo.BundleInfo bundleInfo;
        Vector3 v;
        if (hair.parts[parts].dictBundle.TryGetValue(index, out bundleInfo) && this.GetDefaultHairCorrectRotRate(parts, index, out v))
          bundleInfo.rotRate = v;
      }
    }

    public void ResetDynamicBoneHair(bool includeInactive = false)
    {
      if (this.cmpHair == null)
        return;
      for (int index = 0; index < this.cmpHair.Length; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) this.cmpHair[index]))
          this.cmpHair[index].ResetDynamicBonesHair(includeInactive);
      }
    }

    public AudioSource asVoice { get; private set; }

    private AudioAssist fbsaaVoice { get; set; }

    protected void InitializeControlFaceAll()
    {
      this.fbsaaVoice = new AudioAssist();
      this.InitializeControlFaceObject();
    }

    protected void InitializeControlFaceObject()
    {
      this.asVoice = (AudioSource) null;
    }

    protected void ReleaseControlFaceAll()
    {
      this.ReleaseControlFaceObject(false);
    }

    protected void ReleaseControlFaceObject(bool init = true)
    {
      if (!init)
        return;
      this.InitializeControlFaceObject();
    }

    public void ChangeLookEyesTarget(
      int targetType,
      Transform trfTarg = null,
      float rate = 0.5f,
      float rotDeg = 0.0f,
      float range = 1f,
      float dis = 2f)
    {
      if (Object.op_Equality((Object) null, (Object) this.eyeLookCtrl))
      {
        Debug.LogError((object) "視線の制御スクリプトが見つかりません");
      }
      else
      {
        if (targetType == -1)
          targetType = this.fileStatus.eyesTargetType;
        else
          this.fileStatus.eyesTargetType = targetType;
        this.eyeLookCtrl.target = (Transform) null;
        if (Object.op_Inequality((Object) null, (Object) trfTarg))
        {
          this.eyeLookCtrl.target = trfTarg;
        }
        else
        {
          if (targetType == 0)
          {
            if (Object.op_Inequality((Object) null, (Object) Camera.get_main()))
              this.eyeLookCtrl.target = ((Component) Camera.get_main()).get_transform();
          }
          else if (Object.op_Implicit((Object) this.objEyesLookTarget) && Object.op_Implicit((Object) this.objEyesLookTargetP))
          {
            switch (targetType)
            {
              case 1:
                rotDeg = 0.0f;
                range = 1f;
                break;
              case 2:
                rotDeg = 45f;
                range = 1f;
                break;
              case 3:
                rotDeg = 90f;
                range = 1f;
                break;
              case 4:
                rotDeg = 135f;
                range = 1f;
                break;
              case 5:
                rotDeg = 180f;
                range = 1f;
                break;
              case 6:
                rotDeg = 225f;
                range = 1f;
                break;
              case 7:
                rotDeg = 270f;
                range = 1f;
                break;
              case 8:
                rotDeg = 315f;
                range = 1f;
                break;
              case 9:
                rotDeg = 0.0f;
                range = 1f;
                break;
            }
            this.objEyesLookTargetP.get_transform().SetLocalPosition(0.0f, 0.7f, 0.0f);
            this.eyeLookCtrl.target = this.objEyesLookTarget.get_transform();
            this.eyeLookCtrl.target.SetLocalPosition(0.0f, Mathf.Lerp(0.0f, range, targetType != 9 ? rate : 0.0f), dis);
            this.objEyesLookTargetP.get_transform().set_localEulerAngles(new Vector3(0.0f, 0.0f, 360f - rotDeg));
          }
          this.fileStatus.eyesTargetAngle = rotDeg;
          this.fileStatus.eyesTargetRange = range;
          this.fileStatus.eyesTargetRate = rate;
        }
      }
    }

    public void ChangeLookEyesPtn(int ptn)
    {
      if (Object.op_Equality((Object) null, (Object) this.eyeLookCtrl))
      {
        Debug.LogError((object) "視線の制御スクリプトが見つかりません");
      }
      else
      {
        EyeLookController eyeLookCtrl = this.eyeLookCtrl;
        int num1 = ptn;
        this.fileStatus.eyesLookPtn = num1;
        int num2 = num1;
        eyeLookCtrl.ptnNo = num2;
      }
    }

    public int GetLookEyesPtn()
    {
      return this.fileStatus.eyesLookPtn;
    }

    public float GetLookEyesRate()
    {
      return this.fileStatus.eyesTargetRate;
    }

    public void ChangeLookNeckTarget(
      int targetType,
      Transform trfTarg = null,
      float rate = 0.5f,
      float rotDeg = 0.0f,
      float range = 1f,
      float dis = 0.8f)
    {
      if (Object.op_Equality((Object) null, (Object) this.neckLookCtrl))
      {
        Debug.LogError((object) "首の制御スクリプトが見つかりません");
      }
      else
      {
        if (targetType == -1)
          targetType = this.fileStatus.neckTargetType;
        else
          this.fileStatus.neckTargetType = targetType;
        this.neckLookCtrl.target = (Transform) null;
        if (Object.op_Inequality((Object) null, (Object) trfTarg))
        {
          this.neckLookCtrl.target = trfTarg;
        }
        else
        {
          if (targetType == 0)
          {
            if (Object.op_Inequality((Object) null, (Object) Camera.get_main()))
              this.neckLookCtrl.target = ((Component) Camera.get_main()).get_transform();
          }
          else if (Object.op_Inequality((Object) null, (Object) this.objNeckLookTarget) && Object.op_Inequality((Object) null, (Object) this.objNeckLookTargetP))
          {
            switch (targetType)
            {
              case 1:
                rotDeg = 0.0f;
                range = 1f;
                break;
              case 2:
                rotDeg = 45f;
                range = 1f;
                break;
              case 3:
                rotDeg = 90f;
                range = 1f;
                break;
              case 4:
                rotDeg = 135f;
                range = 1f;
                break;
              case 5:
                rotDeg = 180f;
                range = 1f;
                break;
              case 6:
                rotDeg = 225f;
                range = 1f;
                break;
              case 7:
                rotDeg = 270f;
                range = 1f;
                break;
              case 8:
                rotDeg = 315f;
                range = 1f;
                break;
            }
            this.objNeckLookTargetP.get_transform().SetLocalPosition(0.0f, 2.7f, 0.0f);
            this.neckLookCtrl.target = this.objNeckLookTarget.get_transform();
            this.neckLookCtrl.target.SetLocalPosition(0.0f, Mathf.Lerp(0.0f, range, rate), dis);
            this.objNeckLookTargetP.get_transform().set_localEulerAngles(new Vector3(0.0f, 0.0f, 360f - rotDeg));
          }
          this.fileStatus.neckTargetAngle = rotDeg;
          this.fileStatus.neckTargetRange = range;
          this.fileStatus.neckTargetRate = rate;
        }
      }
    }

    public void ChangeLookNeckPtn(int ptn, float rate = 1f)
    {
      if (Object.op_Equality((Object) null, (Object) this.neckLookCtrl))
      {
        Debug.LogError((object) "首の制御スクリプトが見つかりません");
      }
      else
      {
        NeckLookControllerVer2 neckLookCtrl = this.neckLookCtrl;
        int num1 = ptn;
        this.fileStatus.neckLookPtn = num1;
        int num2 = num1;
        neckLookCtrl.ptnNo = num2;
        this.neckLookCtrl.rate = rate;
      }
    }

    public int GetLookNeckPtn()
    {
      return this.fileStatus.neckLookPtn;
    }

    public float GetLookNeckRate()
    {
      return this.fileStatus.neckTargetRate;
    }

    public void HideEyeHighlight(bool hide)
    {
      if (Object.op_Equality((Object) null, (Object) this.cmpFace))
        return;
      Renderer[] rendEyes = this.cmpFace.targetCustom.rendEyes;
      this.fileStatus.hideEyesHighlight = hide;
      float num = !hide ? 1f : 0.0f;
      if (rendEyes == null)
        return;
      foreach (Renderer renderer in rendEyes)
      {
        if (Object.op_Equality((Object) null, (Object) renderer))
        {
          Debug.LogError((object) "瞳のハイライトのレンダラーが取得できていません");
        }
        else
        {
          Material material = renderer.get_material();
          if (Object.op_Inequality((Object) null, (Object) material))
            material.SetFloat(ChaShader.EyesHighlightOnOff, num);
          else
            Debug.LogError((object) "瞳のハイライトのマテリアルが取得できません");
        }
      }
    }

    public float tearsRate
    {
      get
      {
        return this.fileStatus.tearsRate;
      }
    }

    public void ChangeTearsRate(float value)
    {
      this.fileStatus.tearsRate = Mathf.Clamp(value, 0.0f, 1f);
      if (!Object.op_Inequality((Object) null, (Object) this.cmpFace) || !Object.op_Inequality((Object) null, (Object) this.cmpFace.targetEtc.rendTears))
        return;
      this.cmpFace.targetEtc.rendTears.get_material().SetFloat(ChaShader.tearsRate, this.fileStatus.tearsRate);
    }

    public int GetEyesPtnNum()
    {
      if (this.eyesCtrl != null)
        return this.eyesCtrl.GetMaxPtn();
      Debug.LogError((object) "目の制御スクリプトが見つかりません");
      return 0;
    }

    public void ChangeEyesPtn(int ptn, bool blend = true)
    {
      if (this.eyesCtrl == null)
      {
        Debug.LogError((object) "目の制御スクリプトが見つかりません");
      }
      else
      {
        this.fileStatus.eyesPtn = ptn;
        this.eyesCtrl.ChangePtn(ptn, blend);
      }
    }

    public int GetEyesPtn()
    {
      return this.fileStatus.eyesPtn;
    }

    public void ChangeEyesOpenMax(float maxValue)
    {
      if (this.eyesCtrl == null)
      {
        Debug.LogError((object) "目の制御スクリプトが見つかりません");
      }
      else
      {
        float rate = Mathf.Clamp(maxValue, 0.0f, 1f);
        this.fileStatus.eyesOpenMax = rate;
        this.eyesCtrl.OpenMax = rate;
        if (this.fileStatus.eyesBlink)
          return;
        this.eyesCtrl.SetOpenRateForce(rate);
      }
    }

    public float GetEyesOpenMax()
    {
      return this.fileStatus.eyesOpenMax;
    }

    public void ChangeEyebrowPtn(int ptn, bool blend = true)
    {
      if (this.eyebrowCtrl == null)
      {
        Debug.LogError((object) "眉の制御スクリプトが見つかりません");
      }
      else
      {
        this.fileStatus.eyebrowPtn = ptn;
        this.eyebrowCtrl.ChangePtn(ptn, blend);
      }
    }

    public int GetEyebrowPtn()
    {
      return this.fileStatus.eyebrowPtn;
    }

    public void ChangeEyebrowOpenMax(float maxValue)
    {
      if (this.eyebrowCtrl == null)
        return;
      float num = Mathf.Clamp(maxValue, 0.0f, 1f);
      this.fileStatus.eyebrowOpenMax = num;
      this.eyebrowCtrl.OpenMax = num;
      if (this.fileStatus.eyesBlink)
        return;
      this.eyebrowCtrl.SetOpenRateForce(1f);
    }

    public float GetEyebrowOpenMax()
    {
      return this.fileStatus.eyebrowOpenMax;
    }

    public void ChangeEyesBlinkFlag(bool blink)
    {
      if (Object.op_Equality((Object) null, (Object) this.fbsCtrl))
        Debug.LogError((object) "表情スクリプトが見つかりません");
      else if (this.fbsCtrl.BlinkCtrl == null)
      {
        Debug.LogError((object) "瞬きの制御スクリプトが見つかりません");
      }
      else
      {
        this.fileStatus.eyesBlink = blink;
        this.fbsCtrl.BlinkCtrl.SetFixedFlags(!blink ? (byte) 1 : (byte) 0);
        if (blink)
          return;
        this.eyesCtrl.SetOpenRateForce(1f);
        this.eyebrowCtrl.SetOpenRateForce(1f);
      }
    }

    public bool GetEyesBlinkFlag()
    {
      return this.fileStatus.eyesBlink;
    }

    public void ChangeMouthPtn(int ptn, bool blend = true)
    {
      if (this.mouthCtrl == null)
      {
        Debug.LogError((object) "口の制御スクリプトが見つかりません");
      }
      else
      {
        this.fileStatus.mouthPtn = ptn;
        this.mouthCtrl.ChangePtn(ptn, blend);
        this.ChangeTongueState(ptn == 10 || ptn == 13 ? (byte) 1 : (byte) 0);
        bool useFlags = true;
        if (9 <= ptn && ptn <= 16)
          useFlags = false;
        this.mouthCtrl.UseAdjustWidthScale(useFlags);
      }
    }

    public int GetMouthPtn()
    {
      return this.fileStatus.mouthPtn;
    }

    public void ChangeMouthOpenMax(float maxValue)
    {
      if (this.mouthCtrl == null)
        return;
      float num = Mathf.Clamp(maxValue, 0.0f, 1f);
      this.fileStatus.mouthOpenMax = num;
      this.mouthCtrl.OpenMax = num;
      if (!this.fileStatus.mouthFixed)
        return;
      this.mouthCtrl.FixedRate = num;
    }

    public float GetMouthOpenMax()
    {
      return this.fileStatus.mouthOpenMax;
    }

    public void ChangeMouthOpenMin(float minValue)
    {
      if (this.mouthCtrl == null)
        return;
      float num = Mathf.Clamp(minValue, 0.0f, 1f);
      this.fileStatus.mouthOpenMin = num;
      this.mouthCtrl.OpenMin = num;
      if (!this.fileStatus.mouthFixed)
        return;
      this.mouthCtrl.FixedRate = num;
    }

    public float GetMouthOpenMin()
    {
      return this.fileStatus.mouthOpenMin;
    }

    public void ChangeMouthFixed(bool fix)
    {
      if (this.mouthCtrl == null)
        return;
      this.fileStatus.mouthFixed = fix;
      if (fix)
        this.mouthCtrl.FixedRate = this.fileStatus.mouthOpenMax;
      else
        this.mouthCtrl.FixedRate = -1f;
    }

    public bool GetMouthFixed()
    {
      return this.fileStatus.mouthFixed;
    }

    public void ChangeTongueState(byte state)
    {
      this.fileStatus.tongueState = state;
    }

    public byte GetTongueState()
    {
      return this.fileStatus.tongueState;
    }

    public bool SetVoiceTransform(Transform trfVoice)
    {
      if (Object.op_Equality((Object) null, (Object) trfVoice))
      {
        this.asVoice = (AudioSource) null;
        return false;
      }
      this.asVoice = (AudioSource) ((Component) trfVoice).GetComponent<AudioSource>();
      return !Object.op_Equality((Object) null, (Object) this.asVoice);
    }

    private void UpdateBlendShapeVoice()
    {
      float num = 0.0f;
      float correct = 3f;
      if (Object.op_Inequality((Object) null, (Object) this.asVoice) && this.asVoice.get_isPlaying())
      {
        num = this.fbsaaVoice.GetAudioWaveValue(this.asVoice, correct);
        if (Object.op_Inequality((Object) null, (Object) this.cmpBoneBody) && Object.op_Inequality((Object) null, (Object) this.cmpBoneBody.targetEtc.trfHeadParent) && Object.op_Inequality((Object) null, (Object) ((Component) this.asVoice).get_transform()))
          ((Component) this.asVoice).get_transform().set_position(this.cmpBoneBody.targetEtc.trfHeadParent.get_position());
      }
      if (Object.op_Inequality((Object) null, (Object) this.fbsCtrl))
        this.fbsCtrl.SetVoiceVaule(num);
      if (!this.fileStatus.mouthAdjustWidth || !Object.op_Inequality((Object) null, (Object) this.objHeadBone))
        return;
      Transform mouthAdjustWidth = this.cmpBoneHead.targetEtc.trfMouthAdjustWidth;
      if (!Object.op_Inequality((Object) null, (Object) mouthAdjustWidth))
        return;
      float x = 1f;
      if (this.mouthCtrl != null)
        x = this.mouthCtrl.GetAdjustWidthScale();
      mouthAdjustWidth.SetLocalScaleX(x);
    }

    public float hohoAkaRate
    {
      get
      {
        return this.fileStatus.hohoAkaRate;
      }
    }

    public void ChangeHohoAkaRate(float value)
    {
      this.fileStatus.hohoAkaRate = Mathf.Clamp(value, 0.0f, 1f);
      if (!Object.op_Inequality((Object) null, (Object) this.customMatFace))
        return;
      this.customMatFace.SetFloat(ChaShader.HohoAka, this.fileStatus.hohoAkaRate);
    }

    public ShapeInfoBase sibHand { get; set; }

    protected void InitializeControlHandAll()
    {
      this.InitializeControlHandObject();
    }

    protected void InitializeControlHandObject()
    {
      this.sibHand = (ShapeInfoBase) new ShapeHandInfo();
    }

    protected void ReleaseControlHandAll()
    {
      this.ReleaseControlHandObject(true);
    }

    protected void ReleaseControlHandObject(bool init = true)
    {
      if (this.sibHand != null)
        this.sibHand.ReleaseShapeInfo();
      if (!init)
        return;
      this.InitializeControlHandObject();
    }

    public bool InitShapeHand(Transform trfBone)
    {
      if (this.sibHand == null || Object.op_Equality((Object) null, (Object) trfBone))
        return false;
      this.sibHand.InitShapeInfo("abdata", "list/customshape.unity3d", "list/customshape.unity3d", "cf_anmShapeHand", "cf_customhand", trfBone);
      for (int lr = 0; lr < 2; ++lr)
        this.SetShapeHandValue(lr, 0, 0, 0.0f);
      return true;
    }

    public void ReleaseShapeHand()
    {
      if (this.sibHand == null)
        return;
      this.sibHand.ReleaseShapeInfo();
    }

    public bool GetEnableShapeHand(int lr)
    {
      return this.fileStatus.enableShapeHand[lr];
    }

    public void SetEnableShapeHand(int lr, bool enable)
    {
      this.fileStatus.enableShapeHand[lr] = enable;
    }

    public int GetShapeIndexHandCount()
    {
      return this.sibHand.GetKeyCount();
    }

    public int GetShapeHandIndex(int lr, int no)
    {
      if (2 > no)
        return this.fileStatus.shapeHandPtn[lr, no];
      Debug.LogWarning((object) "手のパターンは２つの合成まで");
      return 0;
    }

    public float GetShapeHandBlendValue(int lr)
    {
      return this.fileStatus.shapeHandBlendValue[lr];
    }

    public bool SetShapeHandValue(int lr, int idx01, int idx02, float blend)
    {
      this.fileStatus.shapeHandPtn[lr, 0] = idx01;
      this.fileStatus.shapeHandPtn[lr, 1] = idx02;
      this.fileStatus.shapeHandBlendValue[lr] = blend;
      if (this.sibHand != null && this.sibHand.InitEnd)
        this.sibHand.ChangeValue(lr, idx01, idx02, blend);
      return true;
    }

    public bool SetShapeHandIndex(int lr, int idx01 = -1, int idx02 = -1)
    {
      if (idx01 != -1)
        this.fileStatus.shapeHandPtn[lr, 0] = idx01;
      if (idx02 != -1)
        this.fileStatus.shapeHandPtn[lr, 1] = idx02;
      if (this.sibHand != null && this.sibHand.InitEnd)
        this.sibHand.ChangeValue(lr, this.fileStatus.shapeHandPtn[lr, 0], this.fileStatus.shapeHandPtn[lr, 1], this.fileStatus.shapeHandBlendValue[lr]);
      return true;
    }

    public bool SetShapeHandBlend(int lr, float blend)
    {
      this.fileStatus.shapeHandBlendValue[lr] = blend;
      if (this.sibHand != null && this.sibHand.InitEnd)
        this.sibHand.ChangeValue(lr, this.fileStatus.shapeHandPtn[lr, 0], this.fileStatus.shapeHandPtn[lr, 1], this.fileStatus.shapeHandBlendValue[lr]);
      return true;
    }

    public void UpdateAlwaysShapeHand()
    {
      if (this.sibHand == null || !this.sibHand.InitEnd)
        return;
      ShapeHandInfo sibHand = this.sibHand as ShapeHandInfo;
      sibHand.updateMask = 0;
      if (this.fileStatus.enableShapeHand[0])
        sibHand.updateMask |= 1;
      if (this.fileStatus.enableShapeHand[1])
        sibHand.updateMask |= 2;
      this.sibHand.UpdateAlways();
    }

    private bool updateAlphaMask { get; set; }

    private bool updateAlphaMask2 { get; set; }

    protected void InitializeControlLoadAll()
    {
      this.InitializeControlLoadObject();
    }

    protected void InitializeControlLoadObject()
    {
      this.aaWeightsHead = new AssignedAnotherWeights();
      this.aaWeightsBody = new AssignedAnotherWeights();
      this.updateAlphaMask = true;
      this.updateAlphaMask2 = true;
    }

    protected void ReleaseControlLoadAll()
    {
      this.ReleaseControlLoadObject(false);
    }

    protected void ReleaseControlLoadObject(bool init = true)
    {
      if (this.aaWeightsHead != null)
        this.aaWeightsHead.Release();
      if (this.aaWeightsBody != null)
        this.aaWeightsBody.Release();
      if (!init)
        return;
      this.InitializeControlLoadObject();
    }

    public bool Load(bool reflectStatus = false)
    {
      this.StartCoroutine(this.LoadAsync(false, false));
      return true;
    }

    [DebuggerHidden]
    public IEnumerator LoadAsync(bool reflectStatus = false, bool asyncFlags = true)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CLoadAsync\u003Ec__Iterator1()
      {
        reflectStatus = reflectStatus,
        asyncFlags = asyncFlags,
        \u0024this = this
      };
    }

    public bool Reload(
      bool noChangeClothes = false,
      bool noChangeHead = false,
      bool noChangeHair = false,
      bool noChangeBody = false,
      bool forceChange = true)
    {
      this.StartCoroutine(this.ReloadAsync(noChangeClothes, noChangeHead, noChangeHair, noChangeBody, forceChange, false));
      return true;
    }

    [DebuggerHidden]
    public IEnumerator ReloadAsync(
      bool noChangeClothes = false,
      bool noChangeHead = false,
      bool noChangeHair = false,
      bool noChangeBody = false,
      bool forceChange = true,
      bool asyncFlags = true)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CReloadAsync\u003Ec__Iterator2()
      {
        asyncFlags = asyncFlags,
        noChangeBody = noChangeBody,
        noChangeHead = noChangeHead,
        forceChange = forceChange,
        noChangeHair = noChangeHair,
        noChangeClothes = noChangeClothes,
        \u0024this = this
      };
    }

    public void ChangeHead(bool forceChange = false)
    {
      this.StartCoroutine(this.ChangeHeadAsync(this.fileFace.headId, forceChange, false));
    }

    public void ChangeHead(int _headId, bool forceChange = false)
    {
      this.StartCoroutine(this.ChangeHeadAsync(_headId, forceChange, false));
    }

    [DebuggerHidden]
    public IEnumerator ChangeHeadAsync(bool forceChange = false)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CChangeHeadAsync\u003Ec__Iterator3()
      {
        forceChange = forceChange,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator ChangeHeadAsync(int _headId, bool forceChange = false, bool asyncFlags = true)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CChangeHeadAsync\u003Ec__Iterator4()
      {
        _headId = _headId,
        forceChange = forceChange,
        asyncFlags = asyncFlags,
        \u0024this = this
      };
    }

    public void ChangeHairAll(bool forceChange = false)
    {
      foreach (int kind in (int[]) Enum.GetValues(typeof (ChaFileDefine.HairKind)))
        this.StartCoroutine(this.ChangeHairAsync(kind, this.fileHair.parts[kind].id, forceChange, false));
    }

    [DebuggerHidden]
    public IEnumerator ChangeHairAllAsync(bool forceChange = false)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CChangeHairAllAsync\u003Ec__Iterator5()
      {
        forceChange = forceChange,
        \u0024this = this
      };
    }

    public bool ChangeHair(int kind, bool forceChange = false)
    {
      this.StartCoroutine(this.ChangeHairAsync(kind, this.fileHair.parts[kind].id, forceChange, false));
      return true;
    }

    public void ChangeHair(int kind, int id, bool forceChange = false)
    {
      this.StartCoroutine(this.ChangeHairAsync(kind, id, forceChange, false));
    }

    [DebuggerHidden]
    public IEnumerator ChangeHairAsync(
      int kind,
      int id,
      bool forceChange = false,
      bool asyncFlags = true)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CChangeHairAsync\u003Ec__Iterator6()
      {
        forceChange = forceChange,
        kind = kind,
        id = id,
        asyncFlags = asyncFlags,
        \u0024this = this
      };
    }

    public void ChangeClothes(bool forceChange = false)
    {
      foreach (int kind in (int[]) Enum.GetValues(typeof (ChaFileDefine.ClothesKind)))
        this.StartCoroutine(this.ChangeClothesAsync(kind, this.nowCoordinate.clothes.parts[kind].id, forceChange, false));
    }

    public void ChangeClothes(int kind, int id, bool forceChange = false)
    {
      this.StartCoroutine(this.ChangeClothesAsync(kind, id, forceChange, false));
    }

    [DebuggerHidden]
    public IEnumerator ChangeClothesAsync(bool forceChange = false)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CChangeClothesAsync\u003Ec__Iterator7()
      {
        forceChange = forceChange,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator ChangeClothesAsync(
      int kind,
      int id,
      bool forceChange = false,
      bool asyncFlags = true)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CChangeClothesAsync\u003Ec__Iterator8()
      {
        asyncFlags = asyncFlags,
        kind = kind,
        id = id,
        forceChange = forceChange,
        \u0024this = this
      };
    }

    public void ChangeClothesTop(int id, bool forceChange = false)
    {
      this.StartCoroutine(this.ChangeClothesTopAsync(id, forceChange, false));
    }

    [DebuggerHidden]
    public IEnumerator ChangeClothesTopAsync(int id, bool forceChange = false, bool asyncFlags = true)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CChangeClothesTopAsync\u003Ec__Iterator9()
      {
        forceChange = forceChange,
        id = id,
        asyncFlags = asyncFlags,
        \u0024this = this
      };
    }

    public void ChangeClothesBot(int id, bool forceChange = false)
    {
      this.StartCoroutine(this.ChangeClothesBotAsync(id, forceChange, false));
    }

    [DebuggerHidden]
    public IEnumerator ChangeClothesBotAsync(int id, bool forceChange = false, bool asyncFlags = true)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CChangeClothesBotAsync\u003Ec__IteratorA()
      {
        forceChange = forceChange,
        id = id,
        asyncFlags = asyncFlags,
        \u0024this = this
      };
    }

    public void ChangeClothesInnerT(int id, bool forceChange = false)
    {
      this.StartCoroutine(this.ChangeClothesInnerTAsync(id, forceChange, false));
    }

    [DebuggerHidden]
    public IEnumerator ChangeClothesInnerTAsync(
      int id,
      bool forceChange = false,
      bool asyncFlags = true)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CChangeClothesInnerTAsync\u003Ec__IteratorB()
      {
        forceChange = forceChange,
        id = id,
        asyncFlags = asyncFlags,
        \u0024this = this
      };
    }

    public void ChangeClothesInnerB(int id, bool forceChange = false)
    {
      this.StartCoroutine(this.ChangeClothesInnerBAsync(id, forceChange, false));
    }

    [DebuggerHidden]
    public IEnumerator ChangeClothesInnerBAsync(
      int id,
      bool forceChange = false,
      bool asyncFlags = true)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CChangeClothesInnerBAsync\u003Ec__IteratorC()
      {
        forceChange = forceChange,
        id = id,
        asyncFlags = asyncFlags,
        \u0024this = this
      };
    }

    public void ChangeClothesGloves(int id, bool forceChange = false)
    {
      this.StartCoroutine(this.ChangeClothesGlovesAsync(id, forceChange, false));
    }

    [DebuggerHidden]
    public IEnumerator ChangeClothesGlovesAsync(
      int id,
      bool forceChange = false,
      bool asyncFlags = true)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CChangeClothesGlovesAsync\u003Ec__IteratorD()
      {
        forceChange = forceChange,
        id = id,
        asyncFlags = asyncFlags,
        \u0024this = this
      };
    }

    public void ChangeClothesPanst(int id, bool forceChange = false)
    {
      this.StartCoroutine(this.ChangeClothesPanstAsync(id, forceChange, false));
    }

    [DebuggerHidden]
    public IEnumerator ChangeClothesPanstAsync(
      int id,
      bool forceChange = false,
      bool asyncFlags = true)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CChangeClothesPanstAsync\u003Ec__IteratorE()
      {
        forceChange = forceChange,
        id = id,
        asyncFlags = asyncFlags,
        \u0024this = this
      };
    }

    public void ChangeClothesSocks(int id, bool forceChange = false)
    {
      this.StartCoroutine(this.ChangeClothesSocksAsync(id, forceChange, false));
    }

    [DebuggerHidden]
    public IEnumerator ChangeClothesSocksAsync(
      int id,
      bool forceChange = false,
      bool asyncFlags = true)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CChangeClothesSocksAsync\u003Ec__IteratorF()
      {
        forceChange = forceChange,
        id = id,
        asyncFlags = asyncFlags,
        \u0024this = this
      };
    }

    public void ChangeClothesShoes(int id, bool forceChange = false)
    {
      this.StartCoroutine(this.ChangeClothesShoesAsync(id, forceChange, false));
    }

    [DebuggerHidden]
    public IEnumerator ChangeClothesShoesAsync(
      int id,
      bool forceChange = false,
      bool asyncFlags = true)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CChangeClothesShoesAsync\u003Ec__Iterator10()
      {
        forceChange = forceChange,
        id = id,
        asyncFlags = asyncFlags,
        \u0024this = this
      };
    }

    public void ChangeAccessory(bool forceChange = false)
    {
      for (int slotNo = 0; slotNo < 20; ++slotNo)
        this.StartCoroutine(this.ChangeAccessoryAsync(slotNo, this.nowCoordinate.accessory.parts[slotNo].type, this.nowCoordinate.accessory.parts[slotNo].id, this.nowCoordinate.accessory.parts[slotNo].parentKey, forceChange, false));
    }

    public void ChangeAccessory(int slotNo, int type, int id, string parentKey, bool forceChange = false)
    {
      this.StartCoroutine(this.ChangeAccessoryAsync(slotNo, type, id, parentKey, forceChange, false));
    }

    [DebuggerHidden]
    public IEnumerator ChangeAccessoryAsync(bool forceChange = false)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CChangeAccessoryAsync\u003Ec__Iterator11()
      {
        forceChange = forceChange,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator ChangeAccessoryAsync(
      int slotNo,
      int type,
      int id,
      string parentKey,
      bool forceChange = false,
      bool asyncFlags = true)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CChangeAccessoryAsync\u003Ec__Iterator12()
      {
        slotNo = slotNo,
        type = type,
        id = id,
        forceChange = forceChange,
        asyncFlags = asyncFlags,
        parentKey = parentKey,
        \u0024this = this
      };
    }

    private GameObject LoadCharaFbxData(
      int category,
      int id,
      string createName,
      bool copyDynamicBone,
      byte copyWeights,
      Transform trfParent,
      int defaultId,
      bool worldPositionStays = false)
    {
      GameObject actObj = (GameObject) null;
      this.StartCoroutine(this.LoadCharaFbxDataAsync((Action<GameObject>) (o => actObj = o), category, id, createName, copyDynamicBone, copyWeights, trfParent, defaultId, false, worldPositionStays));
      return actObj;
    }

    [DebuggerHidden]
    private IEnumerator LoadCharaFbxDataAsync(
      Action<GameObject> actObj,
      int category,
      int id,
      string createName,
      bool copyDynamicBone,
      byte copyWeights,
      Transform trfParent,
      int defaultId,
      bool AsyncFlags = true,
      bool worldPositionStays = false)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChaControl.\u003CLoadCharaFbxDataAsync\u003Ec__Iterator13()
      {
        category = category,
        createName = createName,
        actObj = actObj,
        id = id,
        defaultId = defaultId,
        AsyncFlags = AsyncFlags,
        trfParent = trfParent,
        worldPositionStays = worldPositionStays,
        copyDynamicBone = copyDynamicBone,
        copyWeights = copyWeights,
        \u0024this = this
      };
    }

    public void LoadHitObject()
    {
      this.ReleaseHitObject();
      string assetBundleName1 = "chara/oo_base.unity3d";
      string assetName1 = "p_cf_body_00_hit";
      this.objHitBody = CommonLib.LoadAsset<GameObject>(assetBundleName1, assetName1, true, "abdata");
      Singleton<Character>.Instance.AddLoadAssetBundle(assetBundleName1, "abdata");
      if (Object.op_Inequality((Object) null, (Object) this.objHitBody))
      {
        this.objHitBody.get_transform().SetParent(this.objTop.get_transform(), false);
        this.aaWeightsBody.AssignedWeights(this.objHitBody, "cf_J_Root", (Transform) null);
        foreach (SkinnedCollisionHelper componentsInChild in (SkinnedCollisionHelper[]) this.objHitBody.GetComponentsInChildren<SkinnedCollisionHelper>(true))
          componentsInChild.Init();
      }
      if (this.sex == (byte) 0 || !Object.op_Inequality((Object) null, (Object) this.objHead))
        return;
      ListInfoComponent component = (ListInfoComponent) this.objHead.GetComponent<ListInfoComponent>();
      string assetBundleName2 = component.data.dictInfo.get_Item(11);
      string assetName2 = component.data.dictInfo.get_Item(12) + "_hit";
      this.objHitHead = CommonLib.LoadAsset<GameObject>(assetBundleName2, assetName2, true, string.Empty);
      Singleton<Character>.Instance.AddLoadAssetBundle(assetBundleName2, string.Empty);
      if (!Object.op_Inequality((Object) null, (Object) this.objHitHead))
        return;
      this.objHitHead.get_transform().SetParent(this.objTop.get_transform(), false);
      this.aaWeightsHead.AssignedWeights(this.objHitHead, "cf_J_FaceRoot", (Transform) null);
      foreach (SkinnedCollisionHelper componentsInChild in (SkinnedCollisionHelper[]) this.objHitHead.GetComponentsInChildren<SkinnedCollisionHelper>(true))
        componentsInChild.Init();
    }

    public void ReleaseHitObject()
    {
      if (Object.op_Inequality((Object) null, (Object) this.objHitBody))
      {
        foreach (SkinnedCollisionHelper componentsInChild in (SkinnedCollisionHelper[]) this.objHitBody.GetComponentsInChildren<SkinnedCollisionHelper>(true))
          componentsInChild.Release();
        this.SafeDestroy(this.objHitBody);
        this.objHitBody = (GameObject) null;
      }
      if (!Object.op_Inequality((Object) null, (Object) this.objHitHead))
        return;
      foreach (SkinnedCollisionHelper componentsInChild in (SkinnedCollisionHelper[]) this.objHitHead.GetComponentsInChildren<SkinnedCollisionHelper>(true))
        componentsInChild.Release();
      this.SafeDestroy(this.objHitHead);
      this.objHitHead = (GameObject) null;
    }

    public bool LoadAlphaMaskTexture(string assetBundleName, string assetName, byte type)
    {
      if ("0" == assetBundleName || "0" == assetName)
        return false;
      Texture texture = CommonLib.LoadAsset<Texture>(assetBundleName, assetName, false, string.Empty);
      if (Object.op_Equality((Object) null, (Object) texture))
        return false;
      Singleton<Character>.Instance.AddLoadAssetBundle(assetBundleName, string.Empty);
      switch (type)
      {
        case 0:
          this.texBodyAlphaMask = texture;
          break;
        case 1:
          this.texBraAlphaMask = texture;
          break;
        case 2:
          this.texInnerTBAlphaMask = texture;
          break;
        case 3:
          this.texInnerBAlphaMask = texture;
          break;
        case 4:
          this.texPanstAlphaMask = texture;
          break;
        case 5:
          this.texBodyBAlphaMask = texture;
          break;
      }
      return true;
    }

    public bool ReleaseAlphaMaskTexture(byte type)
    {
      switch (type)
      {
        case 0:
          if (Object.op_Inequality((Object) null, (Object) this.customMatBody))
            this.customMatBody.SetTexture(ChaShader.AlphaMask, (Texture) null);
          this.texBodyAlphaMask = (Texture) null;
          break;
        case 1:
          if (this.rendBra != null && Object.op_Inequality((Object) null, (Object) this.rendBra[0]))
            this.rendBra[0].get_material().SetTexture(ChaShader.AlphaMask, (Texture) null);
          this.texBraAlphaMask = (Texture) null;
          break;
        case 2:
          if (Object.op_Inequality((Object) null, (Object) this.rendInnerTB))
            this.rendInnerTB.get_material().SetTexture(ChaShader.AlphaMask2, (Texture) null);
          this.texInnerTBAlphaMask = (Texture) null;
          break;
        case 3:
          if (Object.op_Inequality((Object) null, (Object) this.rendInnerB))
            this.rendInnerB.get_material().SetTexture(ChaShader.AlphaMask2, (Texture) null);
          this.texInnerBAlphaMask = (Texture) null;
          break;
        case 4:
          if (Object.op_Inequality((Object) null, (Object) this.rendPanst))
            this.rendPanst.get_material().SetTexture(ChaShader.AlphaMask2, (Texture) null);
          this.texPanstAlphaMask = (Texture) null;
          break;
        case 5:
          if (Object.op_Inequality((Object) null, (Object) this.customMatBody))
            this.customMatBody.SetTexture(ChaShader.AlphaMask2, (Texture) null);
          this.texBodyBAlphaMask = (Texture) null;
          break;
      }
      return true;
    }

    public bool InitializeExpression(int sex, bool _enable = true)
    {
      string assetBundleName = "list/expression.unity3d";
      string str = sex != 0 ? "cf_expression" : "cm_expression";
      if (!AssetBundleCheck.IsFile(assetBundleName, str))
      {
        Debug.LogWarning((object) ("読み込みエラー\r\nassetBundleName：" + assetBundleName + "\tassetName：" + str));
        return false;
      }
      this.expression = (CharaUtils.Expression) this.objRoot.AddComponent<CharaUtils.Expression>();
      this.expression.LoadSetting(assetBundleName, str);
      int[] numArray = new int[26]
      {
        0,
        0,
        4,
        0,
        0,
        0,
        0,
        1,
        1,
        5,
        1,
        1,
        1,
        1,
        6,
        6,
        6,
        2,
        2,
        6,
        7,
        7,
        7,
        3,
        3,
        7
      };
      for (int index = 0; index < this.expression.info.Length; ++index)
        this.expression.info[index].categoryNo = numArray[index];
      this.expression.SetCharaTransform(this.objRoot.get_transform());
      this.expression.Initialize();
      this.expression.enable = _enable;
      return true;
    }

    public class MannequinBackInfo
    {
      public byte[] custom;
      public int eyesPtn;
      public float eyesOpen;
      public int mouthPtn;
      public float mouthOpen;
      public bool mouthFixed;
      public int neckLook;
      public byte[] eyesInfo;
      public bool mannequin;

      public void Backup(ChaControl chaCtrl)
      {
        this.custom = chaCtrl.chaFile.GetCustomBytes();
        this.eyesPtn = chaCtrl.fileStatus.eyesPtn;
        this.eyesOpen = chaCtrl.fileStatus.eyesOpenMax;
        this.mouthPtn = chaCtrl.fileStatus.mouthPtn;
        this.mouthOpen = chaCtrl.fileStatus.mouthOpenMax;
        this.mouthFixed = chaCtrl.fileStatus.mouthFixed;
        this.neckLook = chaCtrl.fileStatus.neckLookPtn;
      }

      public void Restore(ChaControl chaCtrl)
      {
        chaCtrl.chaFile.SetCustomBytes(this.custom, ChaFileDefine.ChaFileCustomVersion);
        chaCtrl.Reload(true, false, false, false, true);
        chaCtrl.ChangeEyesPtn(this.eyesPtn, false);
        chaCtrl.ChangeEyesOpenMax(this.eyesOpen);
        chaCtrl.ChangeMouthPtn(this.mouthPtn, false);
        chaCtrl.fileStatus.mouthFixed = this.mouthFixed;
        chaCtrl.ChangeMouthOpenMax(this.mouthOpen);
        chaCtrl.ChangeLookNeckPtn(this.neckLook, 1f);
        chaCtrl.neckLookCtrl.ForceLateUpdate();
        chaCtrl.eyeLookCtrl.ForceLateUpdate();
        chaCtrl.neckLookCtrl.neckLookScript.skipCalc = false;
        chaCtrl.resetDynamicBoneAll = true;
        chaCtrl.LateUpdateForce();
      }
    }

    public enum BodyTexKind
    {
      inpBase,
      inpPaint01,
      inpPaint02,
      inpSunburn,
    }

    public enum FaceTexKind
    {
      inpBase,
      inpEyeshadow,
      inpPaint01,
      inpPaint02,
      inpCheek,
      inpLip,
      inpMole,
    }
  }
}
