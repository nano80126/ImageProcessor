using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessor.Algorithm
{

    /// <summary>
    /// 相機模組參數介面
    /// <para>Pixel Size & Magnification</para>
    /// </summary>
    public interface ICameraModuleConfigure
    {
        public double PixelSize { get; }

        public double LensMagnification { get; }

        public void SetPixelSize(double pxSize);
        public void SetMagnification(double mag);
    }

    public class CameraModuleConfig : ICameraModuleConfigure
    {
        public double PixelSize { get; private set; }

        public double LensMagnification { get; private set; }

        public void SetMagnification(double mag)
        {
            LensMagnification = mag;
        }

        public void SetPixelSize(double pxSize)
        {
            PixelSize = pxSize;
        }
    }

    /// <summary>
    /// 多相機模組介面
    /// </summary>
    public interface ICameraModules
    {
        public ICameraModuleConfigure GetModulesConfigure(int index);

        public void SetModulesCofigure(int index, double pixelSize, double magnification);
    }

    public  class CameraModules : ICameraModules
    {
        //public List<ICameraModuleConfigure> ModulesConfigureList { get; }
        private List<ICameraModuleConfigure> _modulesConfigureList;

        public CameraModules(int modulesCapacity)
        {
            _modulesConfigureList = new List<ICameraModuleConfigure>(modulesCapacity);

            for (int i = 0; i < modulesCapacity; i++)
            {
                _modulesConfigureList.Add(new CameraModuleConfig());
            }
        }

        public ICameraModuleConfigure GetModulesConfigure(int index)
        {
            return _modulesConfigureList[index];
        }

        public void SetModulesCofigure(int index, double pixelSize, double magnification)
        {
            ICameraModuleConfigure configure = _modulesConfigureList[index];

            configure.SetPixelSize(pixelSize);
            configure.SetMagnification(magnification);
        }
    }

    [Obsolete]
    public abstract class Algorithm : IDisposable
    {
        /// <summary>
        /// Source Image
        /// </summary>
        public readonly Mat srcImg;

        /// <summary>
        /// Image
        /// </summary>
        public Mat img;

        /// <summary>
        /// Queue Actions
        /// </summary>
        protected Queue<Action> DrawActions { get; set; } = new Queue<Action>();

        protected bool _disposed;

        protected Algorithm()
        {
            srcImg = new(@"./Image/sample.jpg");
            img = srcImg.Clone();
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="bmp"></param>
        protected Algorithm(Bitmap bmp)
        {
            srcImg = bmp.ToMat();
            img = srcImg.Clone();
            bmp.Dispose();
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="mat"></param>
        protected Algorithm(Mat mat)
        {
            srcImg = mat;
            img = srcImg.Clone();
            //mat.Dispose();
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="path"></param>
        protected Algorithm(string path)
        {
            srcImg = new Mat(path);
            img = srcImg.Clone();
        }

        public Mat GetMat()
        {
            return img;
        }

        public Bitmap GetBmp()
        {
            return img.ToBitmap();
        }

        public void Restore()
        {
            img.Dispose();
            img = srcImg.Clone();
        }

        public void ActionAdd(Action act)
        {
            DrawActions.Enqueue(act);
        }

        public void DoActions()
        {
            while (DrawActions.Count > 0)
            {
                Action act = DrawActions.Dequeue();
                act();
            }
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                throw new NotImplementedException();
            }
            _disposed = true;
        }
    }
}
