// Decompiled with JetBrains decompiler
// Type: CTS.CTSLook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace CTS
{
  public class CTSLook : MonoBehaviour
  {
    [Header("Info")]
    private List<float> _rotArrayX;
    private List<float> _rotArrayY;
    private float rotAverageX;
    private float rotAverageY;
    private float mouseDeltaX;
    private float mouseDeltaY;
    [Header("Settings")]
    public bool _isLocked;
    public float _sensitivityX;
    public float _sensitivityY;
    [Tooltip("The more steps, the smoother it will be.")]
    public int _averageFromThisManySteps;
    [Header("References")]
    [Tooltip("Object to be rotated when mouse moves left/right.")]
    public Transform _playerRootT;
    [Tooltip("Object to be rotated when mouse moves up/down.")]
    public Transform _cameraT;

    public CTSLook()
    {
      base.\u002Ector();
    }

    private void Update()
    {
      this.MouseLookAveraged();
    }

    private void MouseLookAveraged()
    {
      this.rotAverageX = 0.0f;
      this.rotAverageY = 0.0f;
      this.mouseDeltaX = 0.0f;
      this.mouseDeltaY = 0.0f;
      this.mouseDeltaX += Input.GetAxis("Mouse X") * this._sensitivityX;
      this.mouseDeltaY += Input.GetAxis("Mouse Y") * this._sensitivityY;
      this._rotArrayX.Add(this.mouseDeltaX);
      this._rotArrayY.Add(this.mouseDeltaY);
      if (this._rotArrayX.Count >= this._averageFromThisManySteps)
        this._rotArrayX.RemoveAt(0);
      if (this._rotArrayY.Count >= this._averageFromThisManySteps)
        this._rotArrayY.RemoveAt(0);
      for (int index = 0; index < this._rotArrayX.Count; ++index)
        this.rotAverageX += this._rotArrayX[index];
      for (int index = 0; index < this._rotArrayY.Count; ++index)
        this.rotAverageY += this._rotArrayY[index];
      this.rotAverageX /= (float) this._rotArrayX.Count;
      this.rotAverageY /= (float) this._rotArrayY.Count;
      this._playerRootT.Rotate(0.0f, this.rotAverageX, 0.0f, (Space) 0);
      this._cameraT.Rotate(-this.rotAverageY, 0.0f, 0.0f, (Space) 1);
    }
  }
}
