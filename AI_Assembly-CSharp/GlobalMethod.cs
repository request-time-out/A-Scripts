// Decompiled with JetBrains decompiler
// Type: GlobalMethod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using Cinemachine;
using FBSAssist;
using IllusionUtility.GetUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class GlobalMethod
{
  private static List<string> cell = new List<string>();
  private static List<string> row = new List<string>();
  private static List<ExcelData.Param> excelParams = new List<ExcelData.Param>();
  private static List<string> lstABName = new List<string>();
  private static StringBuilder strNo = new StringBuilder();
  private static ExcelData excelData;

  public static void setCameraMoveFlag(VirtualCameraController _ctrl, bool _bPlay)
  {
    if (Object.op_Equality((Object) _ctrl, (Object) null))
      return;
    _ctrl.NoCtrlCondition = (VirtualCameraController.NoCtrlFunc) (() => !_bPlay);
  }

  public static bool IsCameraMoveFlag(VirtualCameraController _ctrl)
  {
    if (Object.op_Equality((Object) _ctrl, (Object) null))
      return false;
    VirtualCameraController.NoCtrlFunc noCtrlCondition = _ctrl.NoCtrlCondition;
    bool flag = true;
    if (noCtrlCondition != null)
      flag = noCtrlCondition();
    return !flag;
  }

  public static bool IsCameraActionFlag(CameraControl_Ver2 _ctrl)
  {
    return !Object.op_Equality((Object) _ctrl, (Object) null) && _ctrl.isControlNow;
  }

  public static void setCameraBase(VirtualCameraController _ctrl, Transform _transTarget)
  {
    if (Object.op_Equality((Object) _ctrl, (Object) null))
      return;
    ((CinemachineVirtualCameraBase) _ctrl).get_Follow().set_position(_transTarget.get_position());
    ((CinemachineVirtualCameraBase) _ctrl).get_Follow().set_rotation(_transTarget.get_rotation());
  }

  public static void setCameraBase(VirtualCameraController _ctrl, Vector3 _pos, Vector3 _rot)
  {
    if (Object.op_Equality((Object) _ctrl, (Object) null))
      return;
    ((CinemachineVirtualCameraBase) _ctrl).get_Follow().set_position(_pos);
    ((CinemachineVirtualCameraBase) _ctrl).get_Follow().set_rotation(Quaternion.Euler(_rot));
  }

  public static void CameraKeyCtrl(VirtualCameraController _ctrl, ChaControl[] _Females)
  {
    if (_Females == null || Object.op_Equality((Object) _ctrl, (Object) null))
      return;
    if (!Input.GetKey((KeyCode) 304) && !Input.GetKey((KeyCode) 303))
    {
      if (Input.GetKeyDown((KeyCode) 113))
      {
        GameObject loop = _Females[0].objBodyBone.get_transform().FindLoop("cf_J_Head");
        if (Object.op_Equality((Object) loop, (Object) null))
          return;
        _ctrl.TargetPos = ((CinemachineVirtualCameraBase) _ctrl).get_Follow().InverseTransformPoint(loop.get_transform().get_position());
      }
      else if (Input.GetKeyDown((KeyCode) 119))
      {
        GameObject loop = _Females[0].objBodyBone.get_transform().FindLoop("cf_J_Mune00");
        if (Object.op_Equality((Object) loop, (Object) null))
          return;
        _ctrl.TargetPos = ((CinemachineVirtualCameraBase) _ctrl).get_Follow().InverseTransformPoint(loop.get_transform().get_position());
      }
      else
      {
        if (!Input.GetKeyDown((KeyCode) 101))
          return;
        GameObject loop = _Females[0].objBodyBone.get_transform().FindLoop("cf_J_Kokan");
        if (Object.op_Equality((Object) loop, (Object) null))
          return;
        _ctrl.TargetPos = ((CinemachineVirtualCameraBase) _ctrl).get_Follow().InverseTransformPoint(loop.get_transform().get_position());
      }
    }
    else
    {
      if (!Object.op_Inequality((Object) _Females[1], (Object) null) || !Object.op_Implicit((Object) _Females[1].objBodyBone))
        return;
      if (Input.GetKeyDown((KeyCode) 113))
      {
        GameObject loop = _Females[1].objBodyBone.get_transform().FindLoop("cf_J_Head");
        if (Object.op_Equality((Object) loop, (Object) null))
          return;
        _ctrl.TargetPos = ((CinemachineVirtualCameraBase) _ctrl).get_Follow().InverseTransformPoint(loop.get_transform().get_position());
      }
      else if (Input.GetKeyDown((KeyCode) 119))
      {
        GameObject loop = _Females[1].objBodyBone.get_transform().FindLoop("cf_J_Mune00");
        if (Object.op_Equality((Object) loop, (Object) null))
          return;
        _ctrl.TargetPos = ((CinemachineVirtualCameraBase) _ctrl).get_Follow().InverseTransformPoint(loop.get_transform().get_position());
      }
      else
      {
        if (!Input.GetKeyDown((KeyCode) 101))
          return;
        GameObject loop = _Females[1].objBodyBone.get_transform().FindLoop("cf_J_Kokan");
        if (Object.op_Equality((Object) loop, (Object) null))
          return;
        _ctrl.TargetPos = ((CinemachineVirtualCameraBase) _ctrl).get_Follow().InverseTransformPoint(loop.get_transform().get_position());
      }
    }
  }

  public static void saveCamera(
    VirtualCameraController _ctrl,
    string _strAssetPath,
    string _strfile)
  {
    if (Object.op_Equality((Object) _ctrl, (Object) null))
      return;
    _ctrl.CameraDataSave(_strAssetPath, _strfile);
  }

  public static void saveCamera(
    CinemachineVirtualCamera _ctrl,
    string _strAssetPath,
    string _strfile)
  {
    if (Object.op_Equality((Object) _ctrl, (Object) null))
      return;
    Vector3 localPosition = ((CinemachineVirtualCameraBase) _ctrl).get_LookAt().get_localPosition();
    Vector3 vector3 = Vector3.op_Subtraction(((Component) _ctrl).get_transform().get_localPosition(), ((CinemachineVirtualCameraBase) _ctrl).get_LookAt().get_localPosition());
    string path = new FileData(string.Empty).Create(_strAssetPath) + _strfile + ".txt";
    Debug.Log((object) path);
    using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.GetEncoding("UTF-8")))
    {
      streamWriter.Write((float) localPosition.x);
      streamWriter.Write('\n');
      streamWriter.Write((float) localPosition.y);
      streamWriter.Write('\n');
      streamWriter.Write((float) localPosition.z);
      streamWriter.Write('\n');
      streamWriter.Write((float) vector3.x);
      streamWriter.Write('\n');
      streamWriter.Write((float) vector3.y);
      streamWriter.Write('\n');
      streamWriter.Write((float) vector3.z);
      streamWriter.Write('\n');
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      streamWriter.Write((float) (^(LensSettings&) ref _ctrl.m_Lens).FieldOfView);
      streamWriter.Write('\n');
    }
  }

  public static void loadCamera(
    VirtualCameraController _ctrl,
    string _assetbundleFolder,
    string _strfile,
    bool _isDirect = false)
  {
    if (Object.op_Equality((Object) _ctrl, (Object) null))
      return;
    _ctrl.CameraDataLoad(_assetbundleFolder, _strfile, _isDirect);
  }

  public static void loadCamera(
    CinemachineVirtualCamera _ctrl,
    string _assetbundleFolder,
    string _strfile,
    bool _isDirect = false)
  {
    if (Object.op_Equality((Object) _ctrl, (Object) null))
      return;
    string text = string.Empty;
    if (!_isDirect)
    {
      text = GlobalMethod.LoadAllListText(_assetbundleFolder, _strfile, (List<string>) null);
    }
    else
    {
      TextAsset textAsset = CommonLib.LoadAsset<TextAsset>(_assetbundleFolder, _strfile, false, string.Empty);
      AssetBundleManager.UnloadAssetBundle(_assetbundleFolder, true, (string) null, false);
      if (Object.op_Implicit((Object) textAsset))
        text = textAsset.get_text();
    }
    if (text == string.Empty)
    {
      GlobalMethod.DebugLog("cameraファイル読み込めません", 1);
    }
    else
    {
      string[][] data;
      GlobalMethod.GetListString(text, out data);
      Vector3 vector3_1;
      vector3_1.x = (__Null) (double) float.Parse(data[0][0]);
      vector3_1.y = (__Null) (double) float.Parse(data[1][0]);
      vector3_1.z = (__Null) (double) float.Parse(data[2][0]);
      Vector3 vector3_2;
      vector3_2.x = (__Null) (double) float.Parse(data[3][0]);
      vector3_2.y = (__Null) (double) float.Parse(data[4][0]);
      vector3_2.z = (__Null) (double) float.Parse(data[5][0]);
      ((CinemachineVirtualCameraBase) _ctrl).get_LookAt().set_localPosition(vector3_1);
      ((Component) _ctrl).get_transform().set_localPosition(Vector3.op_Addition(vector3_2, vector3_1));
      float result = 0.0f;
      if (!float.TryParse(data[6][0], out result))
        return;
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      (^(LensSettings&) ref _ctrl.m_Lens).FieldOfView = (__Null) (double) result;
    }
  }

  public static void loadResetCamera(
    VirtualCameraController _ctrl,
    string _assetbundleFolder,
    string _strfile,
    bool _isDirect = false)
  {
    if (Object.op_Equality((Object) _ctrl, (Object) null))
      return;
    _ctrl.CameraResetDataLoad(_assetbundleFolder, _strfile, _isDirect);
  }

  public static void DebugLog(string _str, int _state = 0)
  {
  }

  public static void SetAllClothState(
    ChaControl _female,
    bool _isUpper,
    int _state,
    bool _isForce = false)
  {
    if (Object.op_Equality((Object) _female, (Object) null))
      return;
    if (_state < 0)
      _state = 0;
    List<int> intList = new List<int>();
    if (_isUpper)
    {
      intList.Add(0);
      intList.Add(2);
    }
    else
    {
      intList.Add(1);
      intList.Add(3);
      intList.Add(5);
    }
    foreach (int clothesKind in intList)
    {
      if (_female.IsClothesStateKind(clothesKind) && ((int) _female.fileStatus.clothesState[clothesKind] < _state || _isForce))
        _female.SetClothesState(clothesKind, (byte) _state, true);
    }
  }

  public static int ValLoop(int valNow, int valMax)
  {
    return valMax > 1 ? (valNow % valMax + valMax) % valMax : 0;
  }

  public static int ValLoopEX(int valNow, int valMin, int valMax)
  {
    return GlobalMethod.ValLoop(valNow - valMin, valMax - valMin) + valMin;
  }

  public static void GetListString(string text, out string[][] data)
  {
    string[] strArray1 = text.Split('\n');
    int length1 = strArray1.Length;
    if (length1 != 0 && strArray1[length1 - 1].Trim() == string.Empty)
      --length1;
    int length2 = 0;
    for (int index = 0; index < length1; ++index)
    {
      string[] strArray2 = strArray1[index].Split('\t');
      length2 = Mathf.Max(length2, strArray2.Length);
    }
    data = new string[length1][];
    for (int index1 = 0; index1 < length1; ++index1)
    {
      data[index1] = new string[length2];
      string[] strArray2 = strArray1[index1].Split('\t');
      for (int index2 = 0; index2 < strArray2.Length; ++index2)
      {
        strArray2[index2] = strArray2[index2].Replace("\r", string.Empty).Replace("\n", string.Empty);
        if (index2 < length2)
          data[index1][index2] = strArray2[index2];
        else
          break;
      }
    }
  }

  public static int GetIntTryParse(string _text, int _init = 0)
  {
    int result = 0;
    return int.TryParse(_text, out result) ? result : _init;
  }

  public static bool RangeOn<T>(T valNow, T valMin, T valMax) where T : IComparable
  {
    return valNow.CompareTo((object) valMax) <= 0 && valNow.CompareTo((object) valMin) >= 0;
  }

  public static bool RangeOff<T>(T valNow, T valMin, T valMax) where T : IComparable
  {
    return valNow.CompareTo((object) valMax) < 0 && valNow.CompareTo((object) valMin) > 0;
  }

  public static string LoadAllListText(
    string _assetbundleFolder,
    string _strLoadFile,
    List<string> _OmitFolderName = null)
  {
    StringBuilder stringBuilder = new StringBuilder();
    GlobalMethod.lstABName.Clear();
    GlobalMethod.lstABName = GlobalMethod.GetAssetBundleNameListFromPath(_assetbundleFolder, false);
    GlobalMethod.lstABName.Sort();
    for (int index1 = 0; index1 < GlobalMethod.lstABName.Count; ++index1)
    {
      string stringRight = YS_Assist.GetStringRight(Path.GetFileNameWithoutExtension(GlobalMethod.lstABName[index1]), 2);
      if (_OmitFolderName == null || !_OmitFolderName.Contains(stringRight))
      {
        string[] allAssetName = AssetBundleCheck.GetAllAssetName(GlobalMethod.lstABName[index1], false, (string) null, false);
        bool flag = false;
        for (int index2 = 0; index2 < allAssetName.Length; ++index2)
        {
          if (allAssetName[index2].Compare(_strLoadFile, true))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          GlobalMethod.DebugLog("[" + GlobalMethod.lstABName[index1] + "][" + _strLoadFile + "]は見つかりません", 1);
        }
        else
        {
          TextAsset textAsset = CommonLib.LoadAsset<TextAsset>(GlobalMethod.lstABName[index1], _strLoadFile, false, string.Empty);
          AssetBundleManager.UnloadAssetBundle(GlobalMethod.lstABName[index1], true, (string) null, false);
          if (!Object.op_Equality((Object) textAsset, (Object) null))
            stringBuilder.Append(textAsset.get_text());
        }
      }
    }
    return stringBuilder.ToString();
  }

  public static string LoadAllListText(List<string> _lstAssetBundleNames, string _strLoadFile)
  {
    StringBuilder stringBuilder = new StringBuilder();
    for (int index1 = 0; index1 < _lstAssetBundleNames.Count; ++index1)
    {
      string[] allAssetName = AssetBundleCheck.GetAllAssetName(_lstAssetBundleNames[index1], false, (string) null, false);
      bool flag = false;
      for (int index2 = 0; index2 < allAssetName.Length; ++index2)
      {
        if (allAssetName[index2].Compare(_strLoadFile, true))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        GlobalMethod.DebugLog("[" + _lstAssetBundleNames[index1] + "][" + _strLoadFile + "]は見つかりません", 1);
      }
      else
      {
        TextAsset textAsset = CommonLib.LoadAsset<TextAsset>(_lstAssetBundleNames[index1], _strLoadFile, false, string.Empty);
        AssetBundleManager.UnloadAssetBundle(_lstAssetBundleNames[index1], true, (string) null, false);
        if (!Object.op_Equality((Object) textAsset, (Object) null))
          stringBuilder.Append(textAsset.get_text());
      }
    }
    return stringBuilder.ToString();
  }

  public static List<string> LoadAllListTextFromList(
    string _assetbundleFolder,
    string _strLoadFile,
    ref List<string> lst,
    List<string> _OmitFolderName = null)
  {
    GlobalMethod.lstABName.Clear();
    GlobalMethod.lstABName = GlobalMethod.GetAssetBundleNameListFromPath(_assetbundleFolder, false);
    GlobalMethod.lstABName.Sort();
    for (int index = 0; index < GlobalMethod.lstABName.Count; ++index)
    {
      string stringRight = YS_Assist.GetStringRight(Path.GetFileNameWithoutExtension(GlobalMethod.lstABName[index]), 2);
      if (_OmitFolderName == null || !_OmitFolderName.Contains(stringRight))
      {
        if (GlobalMethod.AssetFileExist(GlobalMethod.lstABName[index], _strLoadFile, string.Empty))
        {
          TextAsset textAsset = CommonLib.LoadAsset<TextAsset>(GlobalMethod.lstABName[index], _strLoadFile, false, string.Empty);
          AssetBundleManager.UnloadAssetBundle(GlobalMethod.lstABName[index], true, (string) null, false);
          if (!Object.op_Equality((Object) textAsset, (Object) null))
            lst.Add(textAsset.get_text());
        }
      }
    }
    return lst;
  }

  public static List<ExcelData.Param> LoadExcelData(
    string _strAssetPath,
    string _strFileName,
    int sCell,
    int sRow,
    int eCell,
    int eRow,
    bool _isWarning = true)
  {
    if (!GlobalMethod.AssetFileExist(_strAssetPath, _strFileName, string.Empty))
    {
      if (_isWarning)
        GlobalMethod.DebugLog("excel : [" + _strAssetPath + "][" + _strFileName + "]は見つかりません", 1);
      return (List<ExcelData.Param>) null;
    }
    AssetBundleLoadAssetOperation loadAssetOperation = AssetBundleManager.LoadAsset(_strAssetPath, _strFileName, typeof (ExcelData), (string) null);
    AssetBundleManager.UnloadAssetBundle(_strAssetPath, true, (string) null, false);
    if (loadAssetOperation.IsEmpty())
    {
      if (_isWarning)
        GlobalMethod.DebugLog("excel : [" + _strFileName + "]は[" + _strAssetPath + "]の中に入っていません", 1);
      return (List<ExcelData.Param>) null;
    }
    GlobalMethod.excelData = loadAssetOperation.GetAsset<ExcelData>();
    GlobalMethod.cell.Clear();
    foreach (ExcelData.Param obj in GlobalMethod.excelData.list)
      GlobalMethod.cell.Add(obj.list[sCell]);
    GlobalMethod.row.Clear();
    foreach (string str in GlobalMethod.excelData.list[sRow].list)
      GlobalMethod.row.Add(str);
    List<string> cell1 = GlobalMethod.cell;
    List<string> row1 = GlobalMethod.row;
    ExcelData.Specify specify1 = new ExcelData.Specify(eCell, eRow);
    ExcelData.Specify specify2 = new ExcelData.Specify(cell1.Count, row1.Count);
    GlobalMethod.excelParams.Clear();
    if ((long) (uint) specify1.cell > (long) specify2.cell || (long) (uint) specify1.row > (long) specify2.row)
      return (List<ExcelData.Param>) null;
    if (specify1.cell < GlobalMethod.excelData.list.Count)
    {
      for (int cell2 = specify1.cell; cell2 < GlobalMethod.excelData.list.Count && cell2 <= specify2.cell; ++cell2)
      {
        ExcelData.Param obj = new ExcelData.Param();
        if (specify1.row < GlobalMethod.excelData.list[cell2].list.Count)
        {
          obj.list = new List<string>();
          for (int row2 = specify1.row; row2 < GlobalMethod.excelData.list[cell2].list.Count && row2 <= specify2.row; ++row2)
            obj.list.Add(GlobalMethod.excelData.list[cell2].list[row2]);
        }
        GlobalMethod.excelParams.Add(obj);
      }
    }
    return GlobalMethod.excelParams;
  }

  public static List<ExcelData.Param> LoadExcelDataAlFindlFile(
    string _strAssetPath,
    string _strFileName,
    int sCell,
    int sRow,
    int eCell,
    int eRow,
    List<string> _OmitFolderName = null,
    bool _isWarning = true)
  {
    GlobalMethod.lstABName.Clear();
    GlobalMethod.lstABName = GlobalMethod.GetAssetBundleNameListFromPath(_strAssetPath, false);
    GlobalMethod.lstABName.Sort();
    for (int index1 = 0; index1 < GlobalMethod.lstABName.Count; ++index1)
    {
      GlobalMethod.strNo.Clear();
      GlobalMethod.strNo.Append(Path.GetFileNameWithoutExtension(GlobalMethod.lstABName[index1]));
      GlobalMethod.strNo.Replace(GlobalMethod.strNo.ToString(), YS_Assist.GetStringRight(GlobalMethod.strNo.ToString(), 2));
      if (_OmitFolderName == null || !_OmitFolderName.Contains(GlobalMethod.strNo.ToString()))
      {
        string[] allAssetName = AssetBundleCheck.GetAllAssetName(GlobalMethod.lstABName[index1], false, (string) null, false);
        bool flag = false;
        for (int index2 = 0; index2 < allAssetName.Length; ++index2)
        {
          if (allAssetName[index2].Compare(_strFileName, true))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          GlobalMethod.DebugLog("[" + GlobalMethod.lstABName[index1] + "][" + _strFileName + "]は見つかりません", 1);
        }
        else
        {
          List<ExcelData.Param> objList = GlobalMethod.LoadExcelData(GlobalMethod.lstABName[index1], _strFileName, sCell, sRow, eCell, eRow, _isWarning);
          if (objList != null)
            return objList;
        }
      }
    }
    return (List<ExcelData.Param>) null;
  }

  public static T LoadAllFolderInOneFile<T>(
    string _findFolder,
    string _strLoadFile,
    List<string> _OmitFolderName = null)
    where T : Object
  {
    GlobalMethod.lstABName.Clear();
    GlobalMethod.lstABName = GlobalMethod.GetAssetBundleNameListFromPath(_findFolder, false);
    GlobalMethod.lstABName.Sort();
    for (int index = 0; index < GlobalMethod.lstABName.Count; ++index)
    {
      string stringRight = YS_Assist.GetStringRight(Path.GetFileNameWithoutExtension(GlobalMethod.lstABName[index]), 2);
      if (_OmitFolderName == null || !_OmitFolderName.Contains(stringRight))
      {
        if (GlobalMethod.AssetFileExist(GlobalMethod.lstABName[index].ToString(), _strLoadFile, string.Empty))
        {
          T obj = CommonLib.LoadAsset<T>(GlobalMethod.lstABName[index], _strLoadFile, false, string.Empty);
          AssetBundleManager.UnloadAssetBundle(GlobalMethod.lstABName[index], true, (string) null, false);
          return obj;
        }
      }
    }
    return (T) null;
  }

  public static List<T> LoadAllFolder<T>(
    string _findFolder,
    string _strLoadFile,
    List<string> _OmitFolderName = null)
    where T : Object
  {
    List<T> objList = new List<T>();
    GlobalMethod.lstABName.Clear();
    GlobalMethod.lstABName = GlobalMethod.GetAssetBundleNameListFromPath(_findFolder, false);
    GlobalMethod.lstABName.Sort();
    for (int index1 = 0; index1 < GlobalMethod.lstABName.Count; ++index1)
    {
      string stringRight = YS_Assist.GetStringRight(Path.GetFileNameWithoutExtension(GlobalMethod.lstABName[index1]), 2);
      if (_OmitFolderName == null || !_OmitFolderName.Contains(stringRight))
      {
        string[] allAssetName = AssetBundleCheck.GetAllAssetName(GlobalMethod.lstABName[index1], false, (string) null, false);
        bool flag = false;
        for (int index2 = 0; index2 < allAssetName.Length; ++index2)
        {
          if (allAssetName[index2].Compare(_strLoadFile, true))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          GlobalMethod.DebugLog("[" + GlobalMethod.lstABName[index1] + "][" + _strLoadFile + "]は見つかりません", 1);
        }
        else
        {
          T obj = CommonLib.LoadAsset<T>(GlobalMethod.lstABName[index1], _strLoadFile, false, string.Empty);
          AssetBundleManager.UnloadAssetBundle(GlobalMethod.lstABName[index1], true, (string) null, false);
          if (Object.op_Implicit((Object) (object) obj))
            objList.Add(obj);
        }
      }
    }
    return objList;
  }

  public static bool CheckFlagsArray(bool[] flags, int _check = 0)
  {
    if (flags.Length == 0)
      return false;
    bool flag1 = _check == 0;
    foreach (bool flag2 in flags)
    {
      if ((_check != 0 ? (flag2 ? 1 : 0) : (!flag2 ? 1 : 0)) != 0)
        return !flag1;
    }
    return flag1;
  }

  public static List<string> GetAssetBundleNameListFromPath(string path, bool subdirCheck = false)
  {
    List<string> stringList1 = new List<string>();
    if (!AssetBundleCheck.IsSimulation)
    {
      string path1 = AssetBundleManager.BaseDownloadingURL + path;
      if (subdirCheck)
      {
        List<string> stringList2 = new List<string>();
        CommonLib.GetAllFiles(path1, "*.unity3d", stringList2);
        stringList1 = stringList2.Select<string, string>((Func<string, string>) (s => s.Replace(AssetBundleManager.BaseDownloadingURL, string.Empty))).ToList<string>();
      }
      else
      {
        if (!Directory.Exists(path1))
          return stringList1;
        stringList1 = ((IEnumerable<string>) Directory.GetFiles(path1, "*.unity3d")).Select<string, string>((Func<string, string>) (s => s.Replace(AssetBundleManager.BaseDownloadingURL, string.Empty))).ToList<string>();
      }
    }
    return stringList1;
  }

  public static bool StartsWith(string a, string b)
  {
    int length1 = a.Length;
    int length2 = b.Length;
    int index1 = 0;
    int index2;
    for (index2 = 0; index1 < length1 && index2 < length2 && (int) a[index1] == (int) b[index2]; ++index2)
      ++index1;
    if (index2 == length2 && length1 >= length2)
      return true;
    return index1 == length1 && length2 >= length1;
  }

  public static bool AssetFileExist(string path, string targetName, string manifest = "")
  {
    bool flag = false;
    if (path.IsNullOrEmpty() || !AssetBundleCheck.IsFile(path, targetName))
      return flag;
    foreach (string self in AssetBundleCheck.GetAllAssetName(path, false, manifest, false))
    {
      if (self.Compare(targetName, true))
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  public class FloatBlend
  {
    private TimeProgressCtrl tpc = new TimeProgressCtrl(0.15f);
    private bool blend;
    private float min;
    private float max;

    public bool Start(float _min, float _max, float _timeBlend = 0.15f)
    {
      this.tpc.SetProgressTime(_timeBlend);
      this.tpc.Start();
      this.min = _min;
      this.max = _max;
      this.blend = true;
      return true;
    }

    public bool Proc(ref float _ans)
    {
      if (!this.blend)
        return false;
      float num = this.tpc.Calculate();
      _ans = Mathf.Lerp(this.min, this.max, num);
      if ((double) num >= 1.0)
        this.blend = false;
      return true;
    }
  }
}
