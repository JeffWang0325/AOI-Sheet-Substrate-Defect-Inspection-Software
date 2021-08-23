using System.Runtime.InteropServices;

namespace gts
{
    public class mc
    {
        public const short DLL_VERSION_0 = 2;
        public const short DLL_VERSION_1 = 1;
        public const short DLL_VERSION_2 = 0;

        public const short DLL_VERSION_3 = 1;
        public const short DLL_VERSION_4 = 5;
        public const short DLL_VERSION_5 = 0;
        public const short DLL_VERSION_6 = 6;
        public const short DLL_VERSION_7 = 0;
        public const short DLL_VERSION_8 = 7;

        public const short MC_NONE = -1;

        public const short MC_LIMIT_POSITIVE = 0;
        public const short MC_LIMIT_NEGATIVE = 1;
        public const short MC_ALARM = 2;
        public const short MC_HOME = 3;
        public const short MC_GPI = 4;
        public const short MC_ARRIVE = 5;
        public const short MC_MPG = 6;

        public const short MC_ENABLE = 10;
        public const short MC_CLEAR = 11;
        public const short MC_GPO = 12;

        public const short MC_DAC = 20;
        public const short MC_STEP = 21;
        public const short MC_PULSE = 22;
        public const short MC_ENCODER = 23;
        public const short MC_ADC = 24;

        public const short MC_AXIS = 30;
        public const short MC_PROFILE = 31;
        public const short MC_CONTROL = 32;

        public const short CAPTURE_HOME = 1;
        public const short CAPTURE_INDEX = 2;
        public const short CAPTURE_PROBE = 3;
        public const short CAPTURE_HSIO0 = 6;
        public const short CAPTURE_HSIO1 = 7;
        public const short CAPTURE_HOME_GPI = 8;

        public const short PT_MODE_STATIC = 0;
        public const short PT_MODE_DYNAMIC = 1;

        public const short PT_SEGMENT_NORMAL = 0;
        public const short PT_SEGMENT_EVEN = 1;
        public const short PT_SEGMENT_STOP = 2;

        public const short GEAR_MASTER_ENCODER = 1;
        public const short GEAR_MASTER_PROFILE = 2;
        public const short GEAR_MASTER_AXIS = 3;

        public const short FOLLOW_MASTER_ENCODER = 1;
        public const short FOLLOW_MASTER_PROFILE = 2;
        public const short FOLLOW_MASTER_AXIS = 3;

        public const short FOLLOW_EVENT_START = 1;
        public const short FOLLOW_EVENT_PASS = 2;

        public const short GEAR_EVENT_START = 1;
        public const short GEAR_EVENT_PASS = 2;
        public const short GEAR_EVENT_AREA = 5;

        public const short FOLLOW_SEGMENT_NORMAL = 0;
        public const short FOLLOW_SEGMENT_EVEN = 1;
        public const short FOLLOW_SEGMENT_STOP = 2;
        public const short FOLLOW_SEGMENT_CONTINUE = 3;

        public const short INTERPOLATION_AXIS_MAX = 4;
        public const short CRD_FIFO_MAX = 4096;
        public const short FIFO_MAX = 2;
        public const short CRD_MAX = 2;
        public const short CRD_OPERATION_DATA_EXT_MAX = 2;

        public const short CRD_OPERATION_TYPE_NONE = 0;
        public const short CRD_OPERATION_TYPE_BUF_IO_DELAY = 1;
        public const short CRD_OPERATION_TYPE_LASER_ON = 2;
        public const short CRD_OPERATION_TYPE_LASER_OFF = 3;
        public const short CRD_OPERATION_TYPE_BUF_DA = 4;
        public const short CRD_OPERATION_TYPE_LASER_CMD = 5;
        public const short CRD_OPERATION_TYPE_LASER_FOLLOW = 6;
        public const short CRD_OPERATION_TYPE_LMTS_ON = 7;
        public const short CRD_OPERATION_TYPE_LMTS_OFF = 8;
        public const short CRD_OPERATION_TYPE_SET_STOP_IO = 9;
        public const short CRD_OPERATION_TYPE_BUF_MOVE = 10;
        public const short CRD_OPERATION_TYPE_BUF_GEAR = 11;
        public const short CRD_OPERATION_TYPE_SET_SEG_NUM = 12;
        public const short CRD_OPERATION_TYPE_STOP_MOTION = 13;
        public const short CRD_OPERATION_TYPE_SET_VAR_VALUE = 14;
        public const short CRD_OPERATION_TYPE_JUMP_NEXT_SEG = 15;
        public const short CRD_OPERATION_TYPE_SYNCH_PRF_POS = 16;
        public const short CRD_OPERATION_TYPE_VIRTUAL_TO_ACTUAL = 17;
        public const short CRD_OPERATION_TYPE_SET_USER_VAR = 18;
        public const short CRD_OPERATION_TYPE_SET_DO_BIT_PULSE = 19;
        public const short CRD_OPERATION_TYPE_BUF_COMPAREPULSE = 20;
        public const short CRD_OPERATION_TYPE_LASER_ON_EX = 21;
        public const short CRD_OPERATION_TYPE_LASER_OFF_EX = 22;
        public const short CRD_OPERATION_TYPE_LASER_CMD_EX = 23;
        public const short CRD_OPERATION_TYPE_LASER_FOLLOW_RATIO_EX = 24;
        public const short CRD_OPERATION_TYPE_LASER_FOLLOW_MODE = 25;
        public const short CRD_OPERATION_TYPE_LASER_FOLLOW_OFF = 26;
        public const short CRD_OPERATION_TYPE_LASER_FOLLOW_OFF_EX = 27;
        public const short CRD_OPERATION_TYPE_LASER_FOLLOW_SPLINE = 28;
        public const short CRD_OPERATION_TYPE_MOTION_DATA = 29;

        public const short INTERPOLATION_MOTION_TYPE_LINE = 0;
        public const short INTERPOLATION_MOTION_TYPE_CIRCLE = 1;
        public const short INTERPOLATION_MOTION_TYPE_HELIX = 2;
        public const short INTERPOLATION_MOTION_TYPE_CIRCLE_3D = 3;

        public const short INTERPOLATION_CIRCLE_PLAT_XY = 0;
        public const short INTERPOLATION_CIRCLE_PLAT_YZ = 1;
        public const short INTERPOLATION_CIRCLE_PLAT_ZX = 2;

        public const short INTERPOLATION_HELIX_CIRCLE_XY_LINE_Z = 0;
        public const short INTERPOLATION_HELIX_CIRCLE_YZ_LINE_X = 1;
        public const short INTERPOLATION_HELIX_CIRCLE_ZX_LINE_Y = 2;

        public const short INTERPOLATION_CIRCLE_DIR_CW = 0;
        public const short INTERPOLATION_CIRCLE_DIR_CCW = 1;

        public const short COMPARE_PORT_HSIO = 0;
        public const short COMPARE_PORT_GPO = 1;

        public const short COMPARE2D_MODE_2D = 1;
        public const short COMPARE2D_MODE_1D = 0;

        public const short INTERFACEBOARD20 = 2;
        public const short INTERFACEBOARD30 = 3;

        public const short AXIS_LASER = 7;
        public const short AXIS_LASER_EX = 8;

        public const short LASER_CTRL_MODE_PWM1 = 0;
        public const short LASER_CTRL_FREQUENCY = 1;
        public const short LASER_CTRL_VOLTAGE = 2;
        public const short LASER_CTRL_MODE_PWM2 = 3;

        public struct TTrapPrm
        {
            public double acc;
            public double dec;
            public double velStart;
            public short  smoothTime;
        }

        public struct TJogPrm
        {
            public double acc;
            public double dec;
            public double smooth;
        }

        public struct TPid
        {
            public double kp;
            public double ki;
            public double kd;
            public double kvff;
            public double kaff;

            public int integralLimit;
            public int derivativeLimit;
            public short  limit;
        }

        public struct TThreadSts
        {
            public short run;
            public short error;
            public double result;
            public short line;
        }

        public struct TVarInfo
        {
            public short id;
            public short dataType;
            public double dumb0;
            public double dumb1;
            public double dumb2;
            public double dumb3;
        }
        public struct TCompileInfo
        {
            public string pFileName;
            public short pLineNo1;
            public short pLineNo2;
            public string pMessage;
        }
        public struct TCrdPrm
        {
            public short dimension;
            public short profile1;
            public short profile2;
            public short profile3;
            public short profile4;
            public short profile5;
            public short profile6;
            public short profile7;
            public short profile8;

            public double synVelMax;
            public double synAccMax;
            public short evenTime;
            public short setOriginFlag;
            public int originPos1;
            public int originPos2;
            public int originPos3;
            public int originPos4;
            public int originPos5;
            public int originPos6;
            public int originPos7;
            public int originPos8;
        }

        public struct TCrdBufOperation
        {
            public short flag;
            public ushort delay;
            public short doType;
            public ushort doMask;
            public ushort doValue;
            public ushort dataExt1;
            public ushort dataExt2;
        }

        public struct TCrdData
        {
            public short motionType;
            public short circlePlat;
            public int posX;
            public int posY;
            public int posZ;
            public int posA;
            public double radius;
            public short circleDir;
            public double lCenterX;
            public double lCenterY;
            public double lCenterZ;
            public double vel;
            public double acc;
            public short velEndZero;
            public TCrdBufOperation operation;

            public double cosX;
            public double cosY;
            public double cosZ;
            public double cosA;
            public double velEnd;
            public double velEndAdjust;
            public double r;
        }

        public struct TTrigger
        {
            public short encoder;
            public short probeType;
            public short probeIndex;
            public short offset;
            public short windowOnly;
            public int firstPosition;
            public int lastPosition;
        }

        public struct TTriggerStatus
        {
            public short execute;
            public short done;
            public int position;
        }

        public struct T2DCompareData
        {
            public int px;
            public int py;
        }

        public struct T2DComparePrm
        {
            public short encx;
            public short ency;
            public short source;
            public short outputType;
            public short startLevel;
            public short time;
            public short maxerr;
            public short threshold; 
        }
        [DllImport("gts.dll")]
        public static extern short GT_GetDllVersion(out string pDllVersion);
        [DllImport("gts.dll")]
        public static extern short GT_SetCardNo(short index);
        [DllImport("gts.dll")]
        public static extern short GT_GetCardNo(out short index);

        [DllImport("gts.dll")]
        public static extern short GT_GetVersion(out string pVersion);
        [DllImport("gts.dll")]
        public static extern short GT_GetInterfaceBoardSts(out short pStatus);
        [DllImport("gts.dll")]
        public static extern short GT_SetInterfaceBoardSts(short type);

        [DllImport("gts.dll")]
        public static extern short GT_Open(short channel,short param);
        [DllImport("gts.dll")]
        public static extern short GT_Close();

        [DllImport("gts.dll")]
        public static extern short GT_LoadConfig(string pFile);

        [DllImport("gts.dll")]
        public static extern short GT_AlarmOff(short axis);
        [DllImport("gts.dll")]
        public static extern short GT_AlarmOn(short axis);
        [DllImport("gts.dll")]
        public static extern short GT_LmtsOn(short axis, short limitType);
        [DllImport("gts.dll")]
        public static extern short GT_LmtsOff(short axis, short limitType);
        [DllImport("gts.dll")]
        public static extern short GT_ProfileScale(short axis, short alpha, short beta);
        [DllImport("gts.dll")]
        public static extern short GT_EncScale(short axis, short alpha, short beta);
        [DllImport("gts.dll")]
        public static extern short GT_StepDir(short step);
        [DllImport("gts.dll")]
        public static extern short GT_StepPulse(short step);
        [DllImport("gts.dll")]
        public static extern short GT_SetMtrBias(short dac, short bias);
        [DllImport("gts.dll")]
        public static extern short GT_GetMtrBias(short dac, out short pBias);
        [DllImport("gts.dll")]
        public static extern short GT_SetMtrLmt(short dac, short limit);
        [DllImport("gts.dll")]
        public static extern short GT_GetMtrLmt(short dac, out short pLimit);
        [DllImport("gts.dll")]
        public static extern short GT_EncSns(ushort sense);
        [DllImport("gts.dll")]
        public static extern short GT_EncOn(short encoder);
        [DllImport("gts.dll")]
        public static extern short GT_EncOff(short encoder);
        [DllImport("gts.dll")]
        public static extern short GT_SetPosErr(short control, int error);
        [DllImport("gts.dll")]
        public static extern short GT_GetPosErr(short control, out int pError);
        [DllImport("gts.dll")]
        public static extern short GT_SetStopDec(short profile, double decSmoothStop, double decAbruptStop);
        [DllImport("gts.dll")]
        public static extern short GT_GetStopDec(short profile, out double pDecSmoothStop, out double pDecAbruptStop);
        [DllImport("gts.dll")]
        public static extern short GT_LmtSns(ushort sense);
        [DllImport("gts.dll")]
        public static extern short GT_CtrlMode(short axis, short mode);
        [DllImport("gts.dll")]
        public static extern short GT_SetStopIo(short axis, short stopType, short inputType, short inputIndex);
        [DllImport("gts.dll")]
        public static extern short GT_GpiSns(ushort sense);
        [DllImport("gts.dll")]
        public static extern short GT_SetAdcFilter(short adc,short filterTime);
        [DllImport("gts.dll")]
        public static extern short GT_SetAxisPrfVelFilter(short axis,short filterNumExp);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisPrfVelFilter(short axis,out short pFilterNumExp);
        [DllImport("gts.dll")]
        public static extern short GT_SetAxisEncVelFilter(short axis,short filterNumExp);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisEncVelFilter(short axis,out short pFilterNumExp);
        [DllImport("gts.dll")]
        public static extern short GT_SetAxisInputShaping(short axis, short enable, short count, double k);

        [DllImport("gts.dll")]
        public static extern short GT_SetDo(short doType,int value);
        [DllImport("gts.dll")]
        public static extern short GT_SetDoBit(short doType,short doIndex,short value);
        [DllImport("gts.dll")]
        public static extern short GT_GetDo(short doType,out int pValue);
        [DllImport("gts.dll")]
        public static extern short GT_SetDoBitReverse(short doType,short doIndex,short value,short reverseTime);
        [DllImport("gts.dll")]
        public static extern short GT_SetDoMask(short doType,ushort doMask,int value);
        [DllImport("gts.dll")]
        public static extern short GT_EnableDoBitPulse(short doType,short doIndex,ushort highLevelTime,ushort lowLevelTime,int pulseNum,short firstLevel);
        [DllImport("gts.dll")]
        public static extern short GT_DisableDoBitPulse(short doType, short doIndex);

        [DllImport("gts.dll")]
        public static extern short GT_GetDi(short diType,out int pValue);
        [DllImport("gts.dll")]
        public static extern short GT_GetDiReverseCount(short diType,short diIndex,out uint reverseCount,short count);
        [DllImport("gts.dll")]
        public static extern short GT_SetDiReverseCount(short diType,short diIndex,ref uint reverseCount,short count);
        [DllImport("gts.dll")]
        public static extern short GT_GetDiRaw(short diType,out int pValue);

        [DllImport("gts.dll")]
        public static extern short GT_SetDac(short dac,ref short value,short count);
        [DllImport("gts.dll")]
        public static extern short GT_GetDac(short dac,out short value,short count,out uint pClock);

        [DllImport("gts.dll")]
        public static extern short GT_GetAdc(short adc,out double pValue,short count,out uint pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAdcValue(short adc,out short pValue,short count,out uint pClock);

        [DllImport("gts.dll")]
        public static extern short GT_SetEncPos(short encoder,int encPos);
        [DllImport("gts.dll")]
        public static extern short GT_GetEncPos(short encoder,out double pValue,short count,out uint pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetEncPosPre(short encoder,out double pValue,short count,uint pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetEncVel(short encoder,out double pValue,short count,out uint pClock);

        [DllImport("gts.dll")]
        public static extern short GT_SetCaptureMode(short encoder,short mode);
        [DllImport("gts.dll")]
        public static extern short GT_GetCaptureMode(short encoder,out short pMode,short count);
        [DllImport("gts.dll")]
        public static extern short GT_GetCaptureStatus(short encoder,out short pStatus,out int pValue,short count,out uint pClock);
        [DllImport("gts.dll")]
        public static extern short GT_SetCaptureSense(short encoder, short mode,short sense);
        [DllImport("gts.dll")]
        public static extern short GT_ClearCaptureStatus(short encoder);
        [DllImport("gts.dll")]
        public static extern short GT_SetCaptureRepeat(short encoder,short count);
        [DllImport("gts.dll")]
        public static extern short GT_GetCaptureRepeatStatus(short encoder,out short pCount);
        [DllImport("gts.dll")]
        public static extern short GT_GetCaptureRepeatPos(short encoder, out int pValue, short startNum, short count);
        [DllImport("gts.dll")]
        public static extern short GT_SetCaptureEncoder(short trigger,short encoder);
        [DllImport("gts.dll")]
        public static extern short GT_GetCaptureWidth(short trigger,out short pWidth,short count);
        [DllImport("gts.dll")]
        public static extern short GT_GetCaptureHomeGpi(short trigger,out short pHomeSts,out short pHomePos,out short pGpiSts,out short pGpiPos,short count);

        [DllImport("gts.dll")]
        public static extern short GT_Reset();
        [DllImport("gts.dll")]
        public static extern short GT_GetClock(out uint pClock,out uint pLoop);
        [DllImport("gts.dll")]
        public static extern short GT_GetClockHighPrecision(out uint pClock);

        [DllImport("gts.dll")]
        public static extern short GT_GetSts(short axis,out int pSts,short count,out uint pClock);
        [DllImport("gts.dll")]
        public static extern short GT_ClrSts(short axis,short count);
        [DllImport("gts.dll")]
        public static extern short GT_AxisOn(short axis);
        [DllImport("gts.dll")]
        public static extern short GT_AxisOff(short axis);
        [DllImport("gts.dll")]
        public static extern short GT_Stop(int mask,int option);
        [DllImport("gts.dll")]
        public static extern short GT_SetPrfPos(short profile,int prfPos);
        [DllImport("gts.dll")]
        public static extern short GT_SynchAxisPos(int mask);
        [DllImport("gts.dll")]
        public static extern short GT_ZeroPos(short axis,short count);

        [DllImport("gts.dll")]
        public static extern short GT_SetSoftLimit(short axis,int positive,int negative);
        [DllImport("gts.dll")]
        public static extern short GT_GetSoftLimit(short axis,out int pPositive,out int pNegative);
        [DllImport("gts.dll")]
        public static extern short GT_SetAxisBand(short axis,int band,int time);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisBand(short axis,out int pBand,out int pTime);
        [DllImport("gts.dll")]
        public static extern short GT_SetBacklash(short axis,int compValue,double compChangeValue,int compDir);
        [DllImport("gts.dll")]
        public static extern short GT_GetBacklash(short axis,out int pCompValue,out double pCompChangeValue,out int pCompDir);
        [DllImport("gts.dll")]
        public static extern short GT_SetLeadScrewComp(short axis,short n,int startPos,int lenPos,out int pCompPos,out int pCompNeg);
        [DllImport("gts.dll")]
        public static extern short GT_EnableLeadScrewComp(short axis,short mode);
        [DllImport("gts.dll")]
        public static extern short GT_GetCompensate(short axis, out double pPitchError, out double pCrossError, out double pBacklashError, out double pEncPos, out double pPrfPos);
        
        [DllImport("gts.dll")]
        public static extern short GT_EnableGantry(short gantryMaster,short gantrySlave,double masterKp,double slaveKp);
        [DllImport("gts.dll")]
        public static extern short GT_DisableGantry();
        [DllImport("gts.dll")]
        public static extern short GT_SetGantryErrLmt(int gantryErrLmt);
        [DllImport("gts.dll")]
        public static extern short GT_GetGantryErrLmt(out int pGantryErrLmt);

        [DllImport("gts.dll")]
        public static extern short GT_GetPrfPos(short profile,out double pValue,short count,out uint pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetPrfVel(short profile,out double pValue,short count,out uint pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetPrfAcc(short profile,out double pValue,short count,out uint pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetPrfMode(short profile,out int pValue,short count,out uint pClock);

        [DllImport("gts.dll")]
        public static extern short GT_GetAxisPrfPos(short axis,out double pValue,short count,out uint pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisPrfVel(short axis,out double pValue,short count,out uint pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisPrfAcc(short axis,out double pValue,short count,out uint pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisEncPos(short axis,out double pValue,short count,out uint pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisEncVel(short axis,out double pValue,short count,out uint pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisEncAcc(short axis,out double pValue,short count,out uint pClock);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisError(short axis,out double pValue,short count,out uint pClock);

        [DllImport("gts.dll")]
        public static extern short GT_SetLongVar(short index,int value);
        [DllImport("gts.dll")]
        public static extern short GT_GetLongVar(short index,out int pValue);
        [DllImport("gts.dll")]
        public static extern short GT_SetDoubleVar(short index,double pValue);
        [DllImport("gts.dll")]
        public static extern short GT_GetDoubleVar(short index, out double pValue);

        [DllImport("gts.dll")]
        public static extern short GT_SetControlFilter(short control,short index);
        [DllImport("gts.dll")]
        public static extern short GT_GetControlFilter(short control,out short pIndex);

        [DllImport("gts.dll")]
        public static extern short GT_SetPid(short control,short index,ref TPid pPid);
        [DllImport("gts.dll")]
        public static extern short GT_GetPid(short control,short index,out TPid pPid);

        [DllImport("gts.dll")]
        public static extern short GT_SetKvffFilter(short control,short index,short kvffFilterExp,double accMax);
        [DllImport("gts.dll")]
        public static extern short GT_GetKvffFilter(short control, short index, out short pKvffFilterExp, out double pAccMax);

        [DllImport("gts.dll")]
        public static extern short GT_Update(int mask);
        [DllImport("gts.dll")]
        public static extern short GT_SetPos(short profile,int pos);
        [DllImport("gts.dll")]
        public static extern short GT_GetPos(short profile,out int pPos);
        [DllImport("gts.dll")]
        public static extern short GT_SetVel(short profile,double vel);
        [DllImport("gts.dll")]
        public static extern short GT_GetVel(short profile,out double pVel);

        [DllImport("gts.dll")]
        public static extern short GT_PrfTrap(short profile);
        [DllImport("gts.dll")]
        public static extern short GT_SetTrapPrm(short profile,ref TTrapPrm pPrm);
        [DllImport("gts.dll")]
        public static extern short GT_GetTrapPrm(short profile,out TTrapPrm pPrm);

        [DllImport("gts.dll")]
        public static extern short GT_PrfJog(short profile);
        [DllImport("gts.dll")]
        public static extern short GT_SetJogPrm(short profile,ref TJogPrm pPrm);
        [DllImport("gts.dll")]
        public static extern short GT_GetJogPrm(short profile,out TJogPrm pPrm);

        [DllImport("gts.dll")]
        public static extern short GT_PrfPt(short profile,short mode);
        [DllImport("gts.dll")]
        public static extern short GT_SetPtLoop(short profile,int loop);
        [DllImport("gts.dll")]
        public static extern short GT_GetPtLoop(short profile,out int pLoop);
        [DllImport("gts.dll")]
        public static extern short GT_PtSpace(short profile,out short pSpace,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_PtData(short profile,double pos,int time,short type,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_PtDataWN(short profile,double pos,int time,short type,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_PtClear(short profile,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_PtStart(int mask,int option);
        [DllImport("gts.dll")]
        public static extern short GT_SetPtMemory(short profile,short memory);
        [DllImport("gts.dll")]
        public static extern short GT_GetPtMemory(short profile,out short pMemory);
        [DllImport("gts.dll")]
        public static extern short GT_PtGetSegNum(short profile, out int pSegNum);

        [DllImport("gts.dll")]
        public static extern short GT_PrfGear(short profile,short dir);
        [DllImport("gts.dll")]
        public static extern short GT_SetGearMaster(short profile,short masterIndex,short masterType,short masterItem);
        [DllImport("gts.dll")]
        public static extern short GT_GetGearMaster(short profile,out short pMasterIndex,out short pMasterType,out short pMasterItem);
        [DllImport("gts.dll")]
        public static extern short GT_SetGearRatio(short profile,int masterEven,int slaveEven,int masterSlope);
        [DllImport("gts.dll")]
        public static extern short GT_GetGearRatio(short profile,out int pMasterEven,out int pSlaveEven,out int pMasterSlope);
        [DllImport("gts.dll")]
        public static extern short GT_GearStart(int mask);
        [DllImport("gts.dll")]
        public static extern short GT_SetGearEvent(short profile,short gearEvent,int startPara0,int startPara1);
        [DllImport("gts.dll")]
        public static extern short GT_GetGearEvent(short profile, out short pEvent,out int pStartPara0, out int pStartPara1);

        [DllImport("gts.dll")]
        public static extern short GT_PrfFollow(short profile,short dir);
        [DllImport("gts.dll")]
        public static extern short GT_SetFollowMaster(short profile,short masterIndex,short masterType,short masterItem);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowMaster(short profile,out short pMasterIndex,out short pMasterType,out short pMasterItem);
        [DllImport("gts.dll")]
        public static extern short GT_SetFollowLoop(short profile,int loop);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowLoop(short profile,out int pLoop);
        [DllImport("gts.dll")]
        public static extern short GT_SetFollowEvent(short profile,short followEvent,short masterDir,int pos);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowEvent(short profile,out short pFollowEvent,out short pMasterDir,out int pPos);
        [DllImport("gts.dll")]
        public static extern short GT_FollowSpace(short profile,out short pSpace,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_FollowData(short profile,int masterSegment,double slaveSegment,short type,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_FollowClear(short profile,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_FollowStart(int mask,int option);
        [DllImport("gts.dll")]
        public static extern short GT_FollowSwitch(int mask);
        [DllImport("gts.dll")]
        public static extern short GT_SetFollowMemory(short profile,short memory);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowMemory(short profile,out short memory);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowStatus(short profile, out short pFifoNum, out short pSwitchStatus);
        [DllImport("gts.dll")]
        public static extern short GT_SetFollowPhasing(short profile, short profilePhasing);
        [DllImport("gts.dll")]
        public static extern short GT_GetFollowPhasing(short profile, out short pProfilePhasing);

        [DllImport("gts.dll")]
        public static extern short GT_Compile(string pFileName, out TCompileInfo pWrongInfo);
        [DllImport("gts.dll")]
        public static extern short GT_Download(string pFileName);

        [DllImport("gts.dll")]
        public static extern short GT_GetFunId(string pFunName,out short pFunId);
        [DllImport("gts.dll")]
        public static extern short GT_Bind(short thread,short funId, short page);

        [DllImport("gts.dll")]
        public static extern short GT_RunThread(short thread);
        [DllImport("gts.dll")]
        public static extern short GT_StopThread(short thread);
        [DllImport("gts.dll")]
        public static extern short GT_PauseThread(short thread);

        [DllImport("gts.dll")]
        public static extern short GT_GetThreadSts(short thread,out TThreadSts pThreadSts);

        [DllImport("gts.dll")]
        public static extern short GT_GetVarId(string pFunName,string pVarName,out TVarInfo pVarInfo);
        [DllImport("gts.dll")]
        public static extern short GT_SetVarValue(short page,ref TVarInfo pVarInfo,ref double pValue,short count);
        [DllImport("gts.dll")]
        public static extern short GT_GetVarValue(short page,ref TVarInfo pVarInfo,out double pValue,short count);

        [DllImport("gts.dll")]
        public static extern short GT_SetCrdPrm(short crd, ref TCrdPrm pCrdPrm);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdPrm(short crd,out TCrdPrm pCrdPrm);
        [DllImport("gts.dll")]
        public static extern short GT_CrdSpace(short crd,out int pSpace,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_CrdData(short crd,System.IntPtr pCrdData,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_CrdDataCircle(short crd, ref TCrdData pCrdData, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXY(short crd, int x, int y, double synVel, double synAcc, double velEnd, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZ(short crd, int x, int y, int z, double synVel, double synAcc, double velEnd, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZA(short crd, int x, int y, int z, int a, double synVel, double synAcc, double velEnd, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYG0(short crd, int x, int y, double synVel, double synAcc, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZG0(short crd, int x, int y, int z, double synVel, double synAcc, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZAG0(short crd, int x, int y, int z, int a, double synVel, double synAcc, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcXYR(short crd, int x, int y, double radius, short circleDir, double synVel, double synAcc, double velEnd, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcXYC(short crd, int x, int y, double xCenter, double yCenter, short circleDir, double synVel, double synAcc, double velEnd, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcYZR(short crd, int y, int z, double radius, short circleDir, double synVel, double synAcc, double velEnd, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcYZC(short crd, int y, int z, double yCenter, double zCenter, short circleDir, double synVel, double synAcc, double velEnd, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcZXR(short crd, int z, int x, double radius, short circleDir, double synVel, double synAcc, double velEnd, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcZXC(short crd, int z, int x, double zCenter, double xCenter, short circleDir, double synVel, double synAcc, double velEnd, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcXYZ(short crd,int x,int y,int z,double interX,double interY,double interZ,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYOverride2(short crd,int x,int y,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZOverride2(short crd,int x,int y,int z,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZAOverride2(short crd,int x,int y,int z,int a,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYG0Override2(short crd,int x,int y,double synVel,double synAcc,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZG0Override2(short crd,int x,int y,int z,double synVel,double synAcc,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZAG0Override2(short crd,int x,int y,int z,int a,double synVel,double synAcc,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcXYROverride2(short crd,int x,int y,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcXYCOverride2(short crd,int x,int y,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcYZROverride2(short crd,int y,int z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcYZCOverride2(short crd,int y,int z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcZXROverride2(short crd,int z,int x,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcZXCOverride2(short crd,int z,int x,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixXYRZ(short crd,int x,int y,int z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixXYCZ(short crd,int x,int y,int z,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixYZRX(short crd,int x,int y,int z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixYZCX(short crd,int x,int y,int z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixZXRY(short crd,int x,int y,int z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixZXCY(short crd,int x,int y,int z,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixXYRZOverride2(short crd,int x,int y,int z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixXYCZOverride2(short crd,int x,int y,int z,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixYZRXOverride2(short crd,int x,int y,int z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixYZCXOverride2(short crd,int x,int y,int z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixZXRYOverride2(short crd,int x,int y,int z,double radius,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixZXCYOverride2(short crd,int x,int y,int z,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYWN(short crd,int x,int y,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZWN(short crd,int x,int y,int z,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZAWN(short crd,int x,int y,int z,int a,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYG0WN(short crd,int x,int y,double synVel,double synAcc,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZG0WN(short crd,int x,int y,int z,double synVel,double synAcc,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZAG0WN(short crd,int x,int y,int z,int a,double synVel,double synAcc,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcXYRWN(short crd,int x,int y,double radius,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcXYCWN(short crd,int x,int y,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcYZRWN(short crd,int y,int z,double radius,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcYZCWN(short crd,int y,int z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcZXRWN(short crd,int z,int x,double radius,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcZXCWN(short crd,int z,int x,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcXYZWN(short crd,int x,int y,int z,double interX,double interY,double interZ,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYOverride2WN(short crd,int x,int y,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZOverride2WN(short crd,int x,int y,int z,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZAOverride2WN(short crd,int x,int y,int z,int a,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYG0Override2WN(short crd,int x,int y,double synVel,double synAcc,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZG0Override2WN(short crd,int x,int y,int z,double synVel,double synAcc,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_LnXYZAG0Override2WN(short crd,int x,int y,int z,int a,double synVel,double synAcc,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcXYROverride2WN(short crd,int x,int y,double radius,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcXYCOverride2WN(short crd,int x,int y,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcYZROverride2WN(short crd,int y,int z,double radius,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcYZCOverride2WN(short crd,int y,int z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcZXROverride2WN(short crd,int z,int x,double radius,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_ArcZXCOverride2WN(short crd,int z,int x,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixXYRZWN(short crd,int x,int y,int z,double radius,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixXYCZWN(short crd,int x,int y,int z,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixYZRXWN(short crd,int x,int y,int z,double radius,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixYZCXWN(short crd,int x,int y,int z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixZXRYWN(short crd,int x,int y,int z,double radius,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixZXCYWN(short crd,int x,int y,int z,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixXYRZOverride2WN(short crd,int x,int y,int z,double radius,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixXYCZOverride2WN(short crd,int x,int y,int z,double xCenter,double yCenter,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixYZRXOverride2WN(short crd,int x,int y,int z,double radius,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixYZCXOverride2WN(short crd,int x,int y,int z,double yCenter,double zCenter,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixZXRYOverride2WN(short crd,int x,int y,int z,double radius,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_HelixZXCYOverride2WN(short crd,int x,int y,int z,double zCenter,double xCenter,short circleDir,double synVel,double synAcc,double velEnd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufIO(short crd, ushort doType, ushort doMask, ushort doValue, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufEnableDoBitPulse(short crd,short doType,short doIndex,ushort highLevelTime,ushort lowLevelTime,int pulseNum,short firstLevel,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufDisableDoBitPulse(short crd,short doType,short doIndex,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufDelay(short crd, ushort delayTime, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufComparePulse(short crd,short level,short outputType,short time,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufDA(short crd, short chn, short daValue, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufLmtsOn(short crd, short axis, short limitType, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufLmtsOff(short crd, short axis, short limitType, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufSetStopIo(short crd, short axis, short stopType, short inputType, short inputIndex, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufMove(short crd, short moveAxis, int pos, double vel, double acc, short modal, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufGear(short crd, short gearAxis, int pos, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufGearPercent(short crd,short gearAxis,int pos,short accPercent,short decPercent,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufStopMotion(short crd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufSetVarValue(short crd,short pageId,out TVarInfo pVarInfo,double value,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufJumpNextSeg(short crd,short axis,short limitType,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufSynchPrfPos(short crd,short encoder,short profile,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufVirtualToActual(short crd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufSetLongVar(short crd,short index,int value,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufSetDoubleVar(short crd,short index,double value,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_CrdStart(short mask,short option);
        [DllImport("gts.dll")]
        public static extern short GT_CrdStartStep(short mask, short option);
        [DllImport("gts.dll")]
        public static extern short GT_CrdStepMode(short mask, short option);
        [DllImport("gts.dll")]
        public static extern short GT_SetOverride(short crd,double synVelRatio);
        [DllImport("gts.dll")]
        public static extern short GT_SetOverride2(short crd, double synVelRatio);
        [DllImport("gts.dll")]
        public static extern short GT_InitLookAhead(short crd,short fifo,double T,double accMax,short n,ref TCrdData pLookAheadBuf);
        [DllImport("gts.dll")]
        public static extern short GT_GetLookAheadSpace(short crd,out int pSpace,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_GetLookAheadSegCount(short crd,out int pSegCount,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_CrdClear(short crd,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_CrdStatus(short crd,out short pRun,out int pSegment,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_SetUserSegNum(short crd,int segNum,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_GetUserSegNum(short crd,out int pSegment,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_GetUserSegNumWN(short crd,out int pSegment,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_GetRemainderSegNum(short crd,out int pSegment,short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_SetCrdStopDec(short crd,double decSmoothStop,double decAbruptStop);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdStopDec(short crd,out double pDecSmoothStop,out double pDecAbruptStop);
        [DllImport("gts.dll")]
        public static extern short GT_SetCrdLmtStopMode(short crd,short lmtStopMode);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdLmtStopMode(short crd,out short pLmtStopMode);
        [DllImport("gts.dll")]
        public static extern short GT_GetUserTargetVel(short crd,out double pTargetVel);
        [DllImport("gts.dll")]
        public static extern short GT_GetSegTargetPos(short crd,out int pTargetPos);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdPos(short crd,out double pPos);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdVel(short crd,out double pSynVel);
        [DllImport("gts.dll")]
        public static extern short GT_SetCrdSingleMaxVel(short crd,ref double pMaxVel);
        [DllImport("gts.dll")]
        public static extern short GT_GetCrdSingleMaxVel(short crd,out double pMaxVel);
        [DllImport("gts.dll")]
        public static extern short GT_GetCmdCount(short crd, out short pResult, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserOn(short crd,short fifo,short channel);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserOff(short crd,short fifo,short channel);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserPrfCmd(short crd,double laserPower,short fifo,short channel);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserFollowRatio(short crd,double ratio,double minPower,double maxPower,short fifo,short channel);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserFollowMode(short crd,short source ,short fifo,short channel,double startPower );
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserFollowOff(short crd,short fifo,short channel);
        [DllImport("gts.dll")]
        public static extern short GT_BufLaserFollowSpline(short crd,short tableId,double minPower,double maxPower,short fifo,short channel);

        [DllImport("gts.dll")]
        public static extern short GT_PrfPvt(short profile);
        [DllImport("gts.dll")]
        public static extern short GT_SetPvtLoop(short profile,int loop);
        [DllImport("gts.dll")]
        public static extern short GT_GetPvtLoop(short profile,out int pLoopCount,out int pLoop);
        [DllImport("gts.dll")]
        public static extern short GT_PvtStatus(short profile,out short pTableId,out double pTime,short count);
        [DllImport("gts.dll")]
        public static extern short GT_PvtStart(int mask);
        [DllImport("gts.dll")]
        public static extern short GT_PvtTableSelect(short profile,short tableId);

        [DllImport("gts.dll")]
        public static extern short GT_PvtTable(short tableId,int count,ref double pTime,ref double pPos,ref double pVel);
        [DllImport("gts.dll")]
        public static extern short GT_PvtTableEx(short tableId,int count,ref double pTime,ref double pPos,ref double pVelBegin,ref double pVelEnd);
        [DllImport("gts.dll")]
        public static extern short GT_PvtTableComplete(short tableId,int count,ref double pTime,ref double pPos,ref double pA,ref double pB,ref double pC,double velBegin,double velEnd);
        [DllImport("gts.dll")]
        public static extern short GT_PvtTablePercent(short tableId,int count,ref double pTime,ref double pPos,ref double pPercent,double velBegin);
        [DllImport("gts.dll")]
        public static extern short GT_PvtPercentCalculate(int n,ref double pTime,ref double pPos,ref double pPercent,double velBegin,ref double pVel);
        [DllImport("gts.dll")]
        public static extern short GT_PvtTableContinuous(short tableId,int count,ref double pPos,ref double pVel,ref double pPercent,ref double pVelMax,ref double pAcc,ref double pDec,double timeBegin);
        [DllImport("gts.dll")]
        public static extern short GT_PvtContinuousCalculate(int n,ref double pPos,ref double pVel,ref double pPercent,ref double pVelMax,ref double pAcc,ref double pDec,ref double pTime);
        [DllImport("gts.dll")]
        public static extern short GT_PvtTableMovePercent(short tableId, long distance, double vm, double acc, double pa1, double pa2, double dec, double pd1, double pd2, out double pVel, out double pAcc, out double pDec, out double pTime);

        [DllImport("gts.dll")]
        public static extern short GT_HomeInit();
        [DllImport("gts.dll")]
        public static extern short GT_Home(short axis,int pos,double vel,double acc,int offset);
        [DllImport("gts.dll")]
        public static extern short GT_Index(short axis,int pos,int offset);
        [DllImport("gts.dll")]
        public static extern short GT_HomeStop(short axis,int pos,double vel,double acc);
        [DllImport("gts.dll")]
        public static extern short GT_HomeSts(short axis,out ushort pStatus);

        [DllImport("gts.dll")]
        public static extern short GT_HandwheelInit();
        [DllImport("gts.dll")]
        public static extern short GT_SetHandwheelStopDec(short slave,double decSmoothStop,double decAbruptStop);
        [DllImport("gts.dll")]
        public static extern short GT_StartHandwheel(short slave,short master,short masterEven,short slaveEven,short intervalTime,double acc,double dec,double vel,short stopWaitTime);
        [DllImport("gts.dll")]
        public static extern short GT_EndHandwheel(short slave);

        [DllImport("gts.dll")]
        public static extern short GT_SetTrigger(short i,ref TTrigger pTrigger);
        [DllImport("gts.dll")]
        public static extern short GT_GetTrigger(short i,out TTrigger pTrigger);
        [DllImport("gts.dll")]
        public static extern short GT_GetTriggerStatus(short i,out TTriggerStatus pTriggerStatus,short count);
        [DllImport("gts.dll")]
        public static extern short GT_ClearTriggerStatus(short i);

        [DllImport("gts.dll")]
        public static extern short GT_SetComparePort(short channel,short hsio0,short hsio1);

        [DllImport("gts.dll")]
        public static extern short GT_ComparePulse(short level,short outputType,short time);
        [DllImport("gts.dll")]
        public static extern short GT_CompareStop();
        [DllImport("gts.dll")]
        public static extern short GT_CompareStatus(out short pStatus,out int pCount);
        [DllImport("gts.dll")]
        public static extern short GT_CompareData(short encoder,short source,short pulseType,short startLevel,short time,ref int pBuf1,short count1,ref int pBuf2,short count2);
        [DllImport("gts.dll")]
        public static extern short GT_CompareLinear(short encoder,short channel,int startPos,int repeatTimes,int interval,short time,short source);
        [DllImport("gts.dll")]
        public static extern short GT_CompareContinuePulseMode(short mode, short count, short standTime);

        [DllImport("gts.dll")]
        public static extern short GT_SetEncResponseCheck(short control, short dacThreshold, double minEncVel, int time);
        [DllImport("gts.dll")]
        public static extern short GT_GetEncResponseCheck(short control, out short pDacThreshold, out double pMinEncVel, out int pTime);
        [DllImport("gts.dll")]
        public static extern short GT_EnableEncResponseCheck(short control);
        [DllImport("gts.dll")]
        public static extern short GT_DisableEncResponseCheck(short control);

        [DllImport("gts.dll")]
        public static extern short GT_2DCompareMode(short chn, short mode);
        [DllImport("gts.dll")]
        public static extern short GT_2DComparePulse(short chn, short level, short outputType, short time);
        [DllImport("gts.dll")]
        public static extern short GT_2DCompareStop(short chn);
        [DllImport("gts.dll")]
        public static extern short GT_2DCompareClear(short chn);
        [DllImport("gts.dll")]
        public static extern short GT_2DCompareStatus(short chn, out short pStatus, out int pCount, out short pFifo, out short pFifoCount, out short pBufCount);
        [DllImport("gts.dll")]
        public static extern short GT_2DCompareSetPrm(short chn, ref T2DComparePrm pPrm);
        [DllImport("gts.dll")]
        public static extern short GT_2DCompareData(short chn, short count, ref T2DCompareData pBuf, short fifo);
        [DllImport("gts.dll")]
        public static extern short GT_2DCompareStart(short chn);

        [DllImport("gts.dll")]
        public static extern short GT_SetAxisMode(short axis, short mode);
        [DllImport("gts.dll")]
        public static extern short GT_GetAxisMode(short axis, out short pMode);
        [DllImport("gts.dll")]
        public static extern short GT_SetHSIOOpt(ushort value, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_GetHSIOOpt(out ushort pValue, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_LaserPowerMode(short laserPowerMode, double maxValue, double minValue, short channel, short delaymode);
        [DllImport("gts.dll")]
        public static extern short GT_LaserPrfCmd(double outputCmd, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_LaserOutFrq(double outFrq, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_SetPulseWidth(uint width, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_SetWaitPulse(ushort mode, double waitPulseFrq, double waitPulseDuty, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_SetPreVltg(ushort mode, double voltageValue, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_SetLevelDelay(ushort offDelay, ushort onDelay, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_EnaFPK(ushort time1, ushort time2, ushort laserOffDelay, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_DisFPK(short channel);
        [DllImport("gts.dll")]
        public static extern short GT_SetLaserDisMode(short mode, short source, ref int pPos, ref double pScale, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_SetLaserDisRatio(ref double pRatio, double minPower, double maxPower, short channel);
        [DllImport("gts.dll")]
        public static extern short GT_SetWaitPulseEx(ushort mode, double waitPulseFrq, double waitPulseDuty);
        [DllImport("gts.dll")]
        public static extern short GT_SetLaserMode(short mode);
        [DllImport("gts.dll")]
        public static extern short GT_SetLaserFollowSpline(short tableId,long n,ref double pX,ref double pY,double beginValue,double endValue,short channel);
        [DllImport("gts.dll")]
        public static extern short GT_GetLaserFollowSpline(short tableId,long n,out double pX,out double pY,out double pA,out double pB,out double pC,out long pCount,short channel);
        
        //ExtMdl
        [DllImport("gts.dll")]
        public static extern short GT_OpenExtMdl(string pDllName);
        [DllImport("gts.dll")]
        public static extern short GT_CloseExtMdl();
        [DllImport("gts.dll")]
        public static extern short GT_SwitchtoCardNoExtMdl(short card);
        [DllImport("gts.dll")]
        public static extern short GT_ResetExtMdl();
        [DllImport("gts.dll")]
        public static extern short GT_LoadExtConfig(string pFileName);
        [DllImport("gts.dll")]
        public static extern short GT_SetExtIoValue(short mdl,ushort value);
        [DllImport("gts.dll")]
        public static extern short GT_GetExtIoValue(short mdl,out ushort pValue);
        [DllImport("gts.dll")]
        public static extern short GT_SetExtIoBit(short mdl,short index,ushort value);
        [DllImport("gts.dll")]
        public static extern short GT_GetExtIoBit(short mdl,short index,out ushort pValue);
        [DllImport("gts.dll")]
        public static extern short GT_GetExtAdValue(short mdl,short chn,out ushort pValue);
        [DllImport("gts.dll")]
        public static extern short GT_GetExtAdVoltage(short mdl,short chn,out double pValue);
        [DllImport("gts.dll")]
        public static extern short GT_SetExtDaValue(short mdl,short chn,ushort value);
        [DllImport("gts.dll")]
        public static extern short GT_SetExtDaVoltage(short mdl,short chn,double value);
        [DllImport("gts.dll")]
        public static extern short GT_GetStsExtMdl(short mdl,short chn,out ushort pStatus);
        [DllImport("gts.dll")]
        public static extern short GT_GetExtDoValue(short mdl,out ushort pValue);
        [DllImport("gts.dll")]
        public static extern short GT_GetExtMdlMode(out short pMode);
        [DllImport("gts.dll")]
        public static extern short GT_SetExtMdlMode(short mode);
        [DllImport("gts.dll")]
        public static extern short GT_UploadConfig();
        [DllImport("gts.dll")]
        public static extern short GT_DownloadConfig();
        [DllImport("gts.dll")]
        public static extern short GT_GetUuid(out char pCode,short count);

        //2D Compensate
        public struct TCompensate2DTable
        {      
            public short count1;
            public short count2;
            public int posBegin1;
            public int posBegin2;   
            public int step1;
            public int step2;
        } 
        public struct TCompensate2D 
        {
	        public short enable;                                 
	        public short tableIndex;                              
	        public short axisType1;	
            public short axisType2;              
	        public short axisIndex1;
            public short axisIndex2;              
        } 
        [DllImport("gts.dll")]
        public static extern short GT_SetCompensate2DTable(short tableIndex,ref TCompensate2DTable pTable,ref int pData,short externComp);
        [DllImport("gts.dll")]
        public static extern short GT_GetCompensate2DTable(short tableIndex,out TCompensate2DTable pTable,out short pExternComp);
        [DllImport("gts.dll")]
        public static extern short GT_SetCompensate2D(short axis, ref TCompensate2D pComp2d);
        [DllImport("gts.dll")]
        public static extern short GT_GetCompensate2D(short axis, out TCompensate2D pComp2d);
        [DllImport("gts.dll")]
        public static extern short GT_GetCompensate2DValue(short axis, out double pValue);

        //Smart Home
        public const short HOME_STAGE_IDLE=0;
        public const short HOME_STAGE_START=1;
        public const short HOME_STAGE_SEARCH_LIMIT=10;
        public const short HOME_STAGE_SEARCH_LIMIT_STOP=11;
        public const short HOME_STAGE_SEARCH_LIMIT_ESCAPE = 13;
        public const short HOME_STAGE_SEARCH_LIMIT_RETURN=15;
        public const short HOME_STAGE_SEARCH_LIMIT_RETURN_STOP=16;
        public const short HOME_STAGE_SEARCH_HOME=20;
        public const short HOME_STAGE_SEARCH_HOME_RETURN=25;
        public const short HOME_STAGE_SEARCH_INDEX=30;
        public const short HOME_STAGE_SEARCH_GPI=40;
        public const short HOME_STAGE_SEARCH_GPI_RETURN=45;
        public const short HOME_STAGE_GO_HOME=80;
        public const short HOME_STAGE_END=100;
        public const short HOME_ERROR_NONE=0;
        public const short HOME_ERROR_NOT_TRAP_MODE=1;
        public const short HOME_ERROR_DISABLE=2;
        public const short HOME_ERROR_ALARM=3;
        public const short HOME_ERROR_STOP=4;
        public const short HOME_ERROR_STAGE=5;
        public const short HOME_ERROR_HOME_MODE=6;
        public const short HOME_ERROR_SET_CAPTURE_HOME=7;
        public const short HOME_ERROR_NO_HOME=8;
        public const short HOME_ERROR_SET_CAPTURE_INDEX=9;
        public const short HOME_ERROR_NO_INDEX=10;
        public const short HOME_MODE_LIMIT=10;
        public const short HOME_MODE_LIMIT_HOME=11;
        public const short HOME_MODE_LIMIT_INDEX=12;
        public const short HOME_MODE_LIMIT_HOME_INDEX=13;
        public const short HOME_MODE_HOME=20;
        public const short HOME_MODE_HOME_INDEX=22;
        public const short HOME_MODE_INDEX = 30;
        public struct THomePrm
        {
	        public short mode;						
	        public short moveDir;					
	        public short indexDir;					
	        public short edge;					
	        public short triggerIndex;			
			public short pad1_1;
	        public short pad1_2;
            public short pad1_3;
	        public double velHigh;				
	        public double velLow;				
	        public double acc;					
	        public double dec;
	        public short smoothTime;
			public short pad2_1;
		    public short pad2_2;
            public short pad2_3;
	        public int homeOffset;				
	        public int searchHomeDistance;	
	        public int searchIndexDistance;	
	        public int escapeStep;			
            public int pad3_1;
            public int pad3_2;
            public int pad3_3;
        } 
        public struct THomeStatus
        {
	        public short run;
	        public short stage;
            public short error;
            public short pad1;
	        public int capturePos;
	        public int targetPos;
        }
        [DllImport("gts.dll")]
        public static extern short GT_GoHome(short axis, ref THomePrm pHomePrm);
        [DllImport("gts.dll")]
        public static extern short GT_GetHomePrm(short axis, out THomePrm pHomePrm);
        [DllImport("gts.dll")]
        public static extern short GT_GetHomeStatus(short axis, out THomeStatus pHomeStatus);

        //Extern Control
        public struct TControlConfigEx
        {
	        public short refType;
            public short refIndex;
            public short feedbackType;
            public short feedbackIndex;
            public int errorLimit;
            public short feedbackSmooth;
            public short controlSmooth;	
        }
        [DllImport("gts.dll")]
        public static extern short GT_SetControlConfigEx(short control, ref TControlConfigEx pControl);
        [DllImport("gts.dll")]
        public static extern short GT_GetControlConfigEx(short control, out TControlConfigEx pControl);

        //Adc filter
        public struct TAdcConfig
        {
	        public short active;
            public short reverse;
            public double a;
            public double b;
            public short filterMode;	
        }
        [DllImport("gts.dll")]
        public static extern short GT_SetAdcConfig(short adc, ref TAdcConfig pAdc);
        [DllImport("gts.dll")]
        public static extern short GT_GetAdcConfig(short adc, out TAdcConfig pAdc);
        [DllImport("gts.dll")]
        public static extern short GT_SetAdcFilterPrm(short adc, double k);
        [DllImport("gts.dll")]
        public static extern short GT_GetAdcFilterPrm(short adc, out double pk);

        //Superimposed
        [DllImport("gts.dll")]
        public static extern short GT_SetControlSuperimposed(short control, short superimposedType, short superimposedIndex);
        [DllImport("gts.dll")]
        public static extern short GT_GetControlSuperimposed(short control, out short pSuperimposedType, out short pSuperimposedIndex);

        ////////////////////
        [DllImport("gts.dll")]
        public static extern short GT_ZeroLaserOnTime(short channel);
        [DllImport("gts.dll")]
        public static extern short GT_GetLaserOnTime(short channel,out uint pTime);
    }
}
