using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;

using static DeltaSubstrateInspector.FileSystem.FileSystem;

namespace DeltaSubstrateInspector
{   
    public class StartProcReceive
    {
        private enum SorterTCPIPReceiveFmtIndex : int
        {
            ALL = 0,
            ID = 1,
            RECIPENAME = 2,
            WORKMODE = 3,
            MARKCOUNT = 4,
            TRIGGERCOUNT = 5,
            PARTNUMBER = 6
        }        

        private string mID { get; set; } = "0";
        private string mRecipeName { get; set; } = "null";
        private string mWorkMode { get; set; } = "workmode";
        private string mMark1Position { get; set; } = "null";
        private string mMark2Position { get; set; } = "null";
        private string mMarkCount { get; set; } = "null";
        private string mTriggerCount { get; set; } = "null";
        private string mPartNumber { get; set; } = "Default1234"; // (20200429) Jeff Revised!


        //private string mPatternStr = @"^\x02(\d{8});([\w\S]+);([\w\S]+);\x03$";
        private string mPatternStr { get; set; } = @"^\x02(\d{8});([\w\S]+);([\w\S]+);(\d{8});(\d{8});\x03$";
        private string mPatternStr2 { get; set; } = @"^\x02(\d{8});([\w\S]+);([\w\S]+);(\d{8});(\d{8});([\w\S]+);\x03$"; // (20200429) Jeff Revised!

        private string mErrorMsg = "";
        
        public string ID
        {
            get{return mID;}
            set { this.mID = value; }
        }

        public string RecipeName
        {
            get { return mRecipeName; }
            set { this.mRecipeName = value; }
        }

        public string WorkMode
        {
            get { return mWorkMode; }
            set { this.mWorkMode = value; }
        }

        public string MarkCount
        {
            get { return mMarkCount; }
        }

        public string TriggerCount
        {
            get { return mTriggerCount; }
        }

        public string ErrorMessage
        {
            get { return mErrorMsg; }
        }

        public string Part_Number // (20200429) Jeff Revised!
        {
            get { return mPartNumber; }
            set { this.mPartNumber = value; }
        }

        //-------------------------------------------------------------------------
        // 設定Pattern字串
        //-------------------------------------------------------------------------
        public void SetPatternStr(string _patternStr)
        {
            mPatternStr = _patternStr;
        }

        /// <summary>
        /// 解封包
        /// </summary>
        /// <param name="srcStr"></param>
        /// <returns></returns>
        public int PackDecode(string srcStr) // (20200429) Jeff Revised!
        {
            string tempID = "";
            string tempRecipeName = "";
            string tempWorkMode = "";
            string tempMarkCount = "";
            string tempTriggerCount = "";
            string tempPartNumber = ""; // (20200429) Jeff Revised!

            // Match
            //Match match = Regex.Match(srcStr, mPatternStr);
            Match match;
            if (Regex.IsMatch(srcStr, mPatternStr2)) // (20200429) Jeff Revised!
            {
                match = Regex.Match(srcStr, mPatternStr2);
                tempPartNumber = match.Groups[(int)SorterTCPIPReceiveFmtIndex.PARTNUMBER].Value;
                PartNumber = tempPartNumber;
            }
            else
            {
                match = Regex.Match(srcStr, mPatternStr);
                tempPartNumber = "Default1234";
            }

            // Check success           
            if (!match.Success)
            {
                mErrorMsg = "Sorter TCP/IP 接收字串錯誤!";
                //mainForm.syslog.WriteLine(enMessageType.Warnning, str);
                //MessageBox.Show(str);
                return 1;
            }

            tempID = match.Groups[(int)SorterTCPIPReceiveFmtIndex.ID].Value;
            tempRecipeName = match.Groups[(int)SorterTCPIPReceiveFmtIndex.RECIPENAME].Value;
            tempWorkMode = match.Groups[(int)SorterTCPIPReceiveFmtIndex.WORKMODE].Value;
            tempMarkCount = match.Groups[(int)SorterTCPIPReceiveFmtIndex.MARKCOUNT].Value;
            tempTriggerCount=match.Groups[(int)SorterTCPIPReceiveFmtIndex.TRIGGERCOUNT].Value;
            
            // Check ID format
            /*
            int checkID;
            if (!Int32.TryParse(tempID, out checkID))
            {
                mErrorMsg = "Sorter TCP/IP 接收字串ID格式錯誤";
                //mainForm.syslog.WriteLine(enMessageType.Warnning, str);
                //MessageBox.Show(str);
                return 2;
            }*/

            // Setting
            this.mID         = tempID;
            this.mRecipeName = tempRecipeName;
            this.mWorkMode   = tempWorkMode;
            this.mMarkCount = tempMarkCount;
            this.mTriggerCount = tempTriggerCount;
            this.mPartNumber = tempPartNumber; // (20200429) Jeff Revised!

            return 0;
        }


    }
}
