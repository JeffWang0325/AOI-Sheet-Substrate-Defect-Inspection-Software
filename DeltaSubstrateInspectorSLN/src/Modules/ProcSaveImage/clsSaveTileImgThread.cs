using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Threading;
using System.Collections.Concurrent;
using System.IO;

namespace DeltaSubstrateInspector
{
    public class clsSaveTileImgThread
    {
        public class clsPacket
        {
            public string ModuleName;
            public string SB_ID;
            public string MOVE_ID;
            public string Path;
            public string PartNumber;
            public HObject TileImg;
            public HObject AllDefectRegion;
            public HObject Defect;


            public clsPacket(HObject pmTileImg, HObject pmAllDefectRegion, string pmSaveImgPath, string pmModuleName, string pmSB_ID, string pmMOVE_ID, string pmPartNumber,HObject pmDefect)
            {
                this.Path = string.Copy(pmSaveImgPath);
                this.ModuleName = string.Copy(pmModuleName);
                this.MOVE_ID = string.Copy(pmMOVE_ID);
                this.SB_ID = string.Copy(pmSB_ID);
                this.PartNumber = string.Copy(pmPartNumber);
                this.TileImg = pmTileImg.Clone();
                this.AllDefectRegion = pmAllDefectRegion.Clone();
                this.Defect = pmDefect.Clone();
            }

            public void Dispose()
            {
                if (TileImg != null)
                {
                    TileImg.Dispose();
                    TileImg = null;
                }
                if (AllDefectRegion != null)
                {
                    AllDefectRegion.Dispose();
                    AllDefectRegion = null;
                }
                if (Defect != null)
                {
                    Defect.Dispose();
                    Defect = null;
                }
            }
        }

        private Thread Worker;
        private volatile bool Exit = false;
        private const int BufMaxSize = 5;
        private ConcurrentQueue<clsPacket> Buf = new ConcurrentQueue<clsPacket>();

        #region Function

        public clsSaveTileImgThread()
        {
            Worker = new Thread(new ThreadStart(WorkerFunc));
            Worker.IsBackground = true;
            Worker.Start();
        }

        public void Dispose()
        {
            Exit = true;
            Worker.Join();

            //clear buf
            while (true)
            {
                clsPacket Packet;
                if (Buf.TryDequeue(out Packet) == false)
                    break;

                Packet.Dispose();
            }
        }

        public bool PushImg(clsPacket Packet)
        {
            if (Buf.Count > BufMaxSize)
            {
                Packet.Dispose();
                return false;
            }

            Buf.Enqueue(Packet);
            return true;
        }

        public void SaveImg(clsPacket Packet)
        {
            try
            {
                string nowRecipeNameDir = "Default", nowSBIDDir = "00000000", nowMOVEID = "0", nowPartNumber = "Default1234";
                if (!string.IsNullOrEmpty(Packet.ModuleName)) nowRecipeNameDir = Packet.ModuleName;
                if (!string.IsNullOrEmpty(Packet.SB_ID)) nowSBIDDir = Packet.SB_ID;
                if (!string.IsNullOrEmpty(Packet.MOVE_ID)) nowMOVEID = Packet.MOVE_ID;
                if (!string.IsNullOrEmpty(Packet.PartNumber)) nowPartNumber = Packet.PartNumber;
                string SaveImgPath = Packet.Path + nowPartNumber + "\\" + nowRecipeNameDir + "\\" + nowSBIDDir + "\\";

                if (!Directory.Exists(SaveImgPath))
                {
                    Directory.CreateDirectory(SaveImgPath);
                }

                string PathFileName = SaveImgPath + "TileImage_" + nowSBIDDir;

                HObject DilRegion, URegion,DiffRegion;
              
                HOperatorSet.ConcatObj(Packet.Defect, Packet.AllDefectRegion, out URegion);

                HOperatorSet.DilationRectangle1(URegion, out DilRegion, 10, 10);

                HOperatorSet.Difference(DilRegion, URegion, out DiffRegion);

                HOperatorSet.OverpaintRegion(Packet.TileImg, DiffRegion, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "fill");

                HOperatorSet.OverpaintRegion(Packet.TileImg, URegion, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "margin");

                HOperatorSet.WriteImage(Packet.TileImg, "jpg", 0, PathFileName);

                DiffRegion.Dispose();
                DilRegion.Dispose();
                URegion.Dispose();
            }
            catch
            {
            }
        }
        public void Convert2Circle(HObject Regions,int radius, out HObject CirclrRegions)
        {
            HOperatorSet.GenEmptyObj(out CirclrRegions);
            try
            {
                HTuple area_, row_, col_, hv_Number;
                HTuple hv_Newtuple = null;

                HOperatorSet.AreaCenter(Regions, out area_, out row_, out col_);
                HOperatorSet.CountObj(Regions, out hv_Number);
                if (hv_Number > 0)
                {
                    HOperatorSet.TupleGenConst(hv_Number, radius, out hv_Newtuple);
                    HOperatorSet.GenCircle(out CirclrRegions, row_, col_, hv_Newtuple);
                }
            }
            catch { }
        }
        #endregion

        #region Worker

        private void WorkerFunc()
        {
            while (Exit == false)
            {
                Thread.Sleep(500);

                clsPacket Packet;
                if (Buf.TryDequeue(out Packet) == false)
                    continue;

                SaveImg(Packet);

                Packet.Dispose();
            }
        }

        #endregion

    }
}
