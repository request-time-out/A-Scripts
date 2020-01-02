// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomCapture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace CharaCustom
{
  public class CustomCapture : MonoBehaviour
  {
    [SerializeField]
    private Camera camBG;
    public Camera camMain;

    public CustomCapture()
    {
      base.\u002Ector();
    }

    public byte[] CapCharaCard(
      bool enableBG,
      SaveFrameAssist saveFrameAssist,
      bool forceHideBackFrame = false)
    {
      byte[] pngData = (byte[]) null;
      bool flag1 = !Object.op_Equality((Object) null, (Object) saveFrameAssist) && saveFrameAssist.backFrameDraw;
      if (forceHideBackFrame)
        flag1 = false;
      bool flag2 = !Object.op_Equality((Object) null, (Object) saveFrameAssist) && saveFrameAssist.frontFrameDraw;
      Camera _camBackFrame = !Object.op_Equality((Object) null, (Object) saveFrameAssist) ? (!flag1 ? (Camera) null : saveFrameAssist.backFrameCam) : (Camera) null;
      Camera _camFrontFrame = !Object.op_Equality((Object) null, (Object) saveFrameAssist) ? (!flag2 ? (Camera) null : saveFrameAssist.frontFrameCam) : (Camera) null;
      CustomCapture.CreatePng(ref pngData, !enableBG ? (Camera) null : this.camBG, _camBackFrame, this.camMain, _camFrontFrame);
      Camera camera = !Object.op_Inequality((Object) null, (Object) saveFrameAssist) ? (Camera) null : saveFrameAssist.backFrameCam;
      if (Object.op_Inequality((Object) null, (Object) camera))
        camera.set_targetTexture((RenderTexture) null);
      return pngData;
    }

    public byte[] CapCoordinateCard(bool enableBG, SaveFrameAssist saveFrameAssist, Camera main)
    {
      byte[] pngData = (byte[]) null;
      bool flag1 = !Object.op_Equality((Object) null, (Object) saveFrameAssist) && saveFrameAssist.backFrameDraw;
      bool flag2 = !Object.op_Equality((Object) null, (Object) saveFrameAssist) && saveFrameAssist.frontFrameDraw;
      Camera _camBackFrame = !Object.op_Equality((Object) null, (Object) saveFrameAssist) ? (!flag1 ? (Camera) null : saveFrameAssist.backFrameCam) : (Camera) null;
      Camera _camFrontFrame = !Object.op_Equality((Object) null, (Object) saveFrameAssist) ? (!flag2 ? (Camera) null : saveFrameAssist.frontFrameCam) : (Camera) null;
      CustomCapture.CreatePng(ref pngData, !enableBG ? (Camera) null : this.camBG, _camBackFrame, main, _camFrontFrame);
      Camera camera = !Object.op_Inequality((Object) null, (Object) saveFrameAssist) ? (Camera) null : saveFrameAssist.backFrameCam;
      if (Object.op_Inequality((Object) null, (Object) camera))
        camera.set_targetTexture((RenderTexture) null);
      if (Object.op_Inequality((Object) null, (Object) this.camMain))
        this.camMain.set_targetTexture((RenderTexture) null);
      if (Object.op_Inequality((Object) null, (Object) this.camBG))
        this.camBG.set_targetTexture((RenderTexture) null);
      return pngData;
    }

    public static void CreatePng(
      ref byte[] pngData,
      Camera _camBG = null,
      Camera _camBackFrame = null,
      Camera _camMain = null,
      Camera _camFrontFrame = null)
    {
      int num1 = 1280;
      int num2 = 720;
      int num3 = 504;
      int num4 = 704;
      RenderTexture renderTexture = QualitySettings.get_antiAliasing() != 0 ? RenderTexture.GetTemporary(num1, num2, 24, (RenderTextureFormat) 7, (RenderTextureReadWrite) 0, QualitySettings.get_antiAliasing()) : RenderTexture.GetTemporary(num1, num2, 24);
      bool sRgbWrite = GL.get_sRGBWrite();
      GL.set_sRGBWrite(true);
      if (Object.op_Inequality((Object) null, (Object) _camMain))
      {
        RenderTexture targetTexture = _camMain.get_targetTexture();
        bool allowHdr = _camMain.get_allowHDR();
        _camMain.set_allowHDR(false);
        _camMain.set_targetTexture(renderTexture);
        _camMain.Render();
        _camMain.set_targetTexture(targetTexture);
        _camMain.set_allowHDR(allowHdr);
      }
      if (Object.op_Inequality((Object) null, (Object) _camBG))
      {
        bool allowHdr = _camBG.get_allowHDR();
        _camBG.set_allowHDR(false);
        _camBG.set_targetTexture(renderTexture);
        _camBG.Render();
        _camBG.set_targetTexture((RenderTexture) null);
        _camBG.set_allowHDR(allowHdr);
      }
      if (Object.op_Inequality((Object) null, (Object) _camBackFrame))
      {
        _camBackFrame.set_targetTexture(renderTexture);
        _camBackFrame.Render();
        _camBackFrame.set_targetTexture((RenderTexture) null);
      }
      if (Object.op_Inequality((Object) null, (Object) _camFrontFrame))
      {
        _camFrontFrame.set_targetTexture(renderTexture);
        _camFrontFrame.Render();
        _camFrontFrame.set_targetTexture((RenderTexture) null);
      }
      GL.set_sRGBWrite(sRgbWrite);
      Texture2D tex = new Texture2D(num3, num4, (TextureFormat) 3, false, true);
      RenderTexture.set_active(renderTexture);
      tex.ReadPixels(new Rect((float) (num1 - num3) / 2f, (float) (num2 - num4) / 2f, (float) num3, (float) num4), 0, 0);
      tex.Apply();
      RenderTexture.set_active((RenderTexture) null);
      RenderTexture.ReleaseTemporary(renderTexture);
      TextureScale.Bilinear(tex, num3 / 2, num4 / 2);
      pngData = ImageConversion.EncodeToPNG(tex);
      Object.Destroy((Object) tex);
    }
  }
}
