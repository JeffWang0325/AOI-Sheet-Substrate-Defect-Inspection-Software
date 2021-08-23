using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace DeltaSubstrateInspector
{
    public partial class ProgressDialog : Form
    {
        string m_strShowText = "";
        string m_strCaption = "";

        // (20190604) Jeff Revised!
        public delegate void FormClosedHandler(bool FormClosed); // 定義委派
        public event FormClosedHandler FormClosedEvent; // 定義事件

        public ProgressDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 更新目前進度條的位置 & 顯示處理進度百分比 (20190604) Jeff Revised!
        /// </summary>
        /// <param name="progress"></param>
        public void UpdateProgress(int progress)
        {
            if (progress < 0)
            {
                progress = 0;
            }
            if (progress > 100)
            {
                progress = 100;
            }

            try
            {
                if (progressBar1.InvokeRequired)
                {
                    progressBar1.BeginInvoke(new Action(() => progressBar1.Value = progress));
                    this.labelPercent.BeginInvoke(new Action(() => this.labelPercent.Text = progress.ToString() + "%")); // (20190604) Jeff Revised!
                }
                else
                {
                    progressBar1.Value = progress;
                    this.labelPercent.Text = progress.ToString() + "%"; // (20190604) Jeff Revised!
                }
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }
        
        public void UpdateShowText() // (20190611) Jeff Revised!
        {
            try
            {
                if (this.labelShow.InvokeRequired)
                {
                    this.labelShow.BeginInvoke(new Action(() => this.labelShow.Text = this.m_strShowText));
                }
                else
                {
                    this.labelShow.Text = this.m_strShowText;
                }
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        public void UpdateShowCaption() // (20190611) Jeff Revised!
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => this.Text = m_strCaption));
                }
                else
                {
                    this.Text = m_strCaption;
                }
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        public void SetIndeterminate(bool isIndeterminate)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.BeginInvoke(new Action(() =>
                    {
                        if (isIndeterminate)
                        {
                            progressBar1.Style = ProgressBarStyle.Marquee;
                            this.labelPercent.Visible = false; // (20190604) Jeff Revised!
                        }
                        else
                        {
                            progressBar1.Style = ProgressBarStyle.Blocks;
                            this.labelPercent.Visible = true; // (20190604) Jeff Revised!
                        }
                    }
                ));
            }
            else
            {
                if (isIndeterminate)
                {
                    progressBar1.Style = ProgressBarStyle.Marquee;
                    this.labelPercent.Visible = false; // (20190604) Jeff Revised!
                }
                else
                {
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    this.labelPercent.Visible = true; // (20190604) Jeff Revised!
                }
            }
        }

        public void SetShowText(string strText)
        {
            this.m_strShowText = strText;
        }

        /// <summary>
        /// 設定顯示文字位置
        /// </summary>
        public void SetShowText_location(Point loc, object size = null) // (20200429) Jeff Revised!
        {
            this.labelShow.Location = loc;
            if (size != null)
                this.labelShow.Size = (Size)size;
        }

        public void SetShowCaption(string strText)
        {
            m_strCaption = strText;

        }

        private void ProgressDialog_Shown(object sender, EventArgs e)
        {
            this.labelShow.Text = m_strShowText;
            this.Text = m_strCaption;
            progressBar1.Step = 0;
            timer1.Enabled = true;
            sw.Reset();
            sw.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //float fSec = (float)timecnt.Elapsed(clsCmData.enuSecUnit.MilliSec) / 1000.0f;
            //labelSecCount.Text = fSec.ToString("0.##") + " sec";
        }
        private static System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        public static double calc_sw;
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            try
            {
                float fSec = (float)sw.ElapsedMilliseconds / 1000.0f;
                labelSecCount.Text = fSec.ToString("0.##") + " sec";
                Application.DoEvents();
                Thread.Sleep(10);
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            
        }

        private void ProgressDialog_Load(object sender, EventArgs e)
        {
            int a = Screen.PrimaryScreen.Bounds.Height;
            int b = Screen.PrimaryScreen.Bounds.Width;
        }

        private bool b_FormClosed = false; // (20190613) Jeff Revised!
        public bool B_FormClosed
        {
            get { return b_FormClosed; }
            set { b_FormClosed = value; }
        }
        private Thread backgroundThread; // (20190613) Jeff Revised!
        private void ProgressDialog_FormClosing(object sender, FormClosingEventArgs e) // (20190604) Jeff Revised!
        {
            /* 法1: 等待操作者回應，導致UI無法更新 */
            //if (e.CloseReason != CloseReason.WindowsShutDown)
            //{
            //    if (MessageBox.Show("是否確定要關閉程式", "關閉程式", MessageBoxButtons.YesNo) == DialogResult.No)
            //    {
            //        e.Cancel = true;
            //    }
            //}

            /* 法2:  */
            if (b_FormClosed)
                Abort_thread(); // 確保Thread已關閉
            else if (backgroundThread != null && backgroundThread.IsAlive) // 避免操作人員重覆按下停止執行按鈕，導致出現多個提示視窗
            {
                e.Cancel = true; // 取消關閉視窗
                return;
            }
            else if (e.CloseReason != CloseReason.WindowsShutDown) // (20190613) Jeff Revised!
            {
                e.Cancel = true; // 取消關閉視窗
                // Initialize the thread that will handle the background process
                backgroundThread = new Thread(
                    new ThreadStart(() =>
                    {
                        btnStop.BeginInvoke(new Action(() => btnStop.Enabled = false));
                        if (MessageBox.Show("是否確定要關閉程式", "關閉程式", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //e.Cancel = true;
                            b_FormClosed = true;

                            if (this.InvokeRequired)
                                this.BeginInvoke(new Action(() => this.Close())); // 多執行緒(線程)中安全的更新介面顯示
                            else
                                this.Close();
                        }
                        btnStop.BeginInvoke(new Action(() => btnStop.Enabled = true));
                    }
                ));
                
                // Start the background process thread
                backgroundThread.Start();
            }
        }

        public void Abort_thread() // (20190613) Jeff Revised!
        {
            if (backgroundThread != null && backgroundThread.IsAlive)
            {
                backgroundThread.Abort();
                backgroundThread = null;
            }
        }

        private void ProgressDialog_FormClosed(object sender, FormClosedEventArgs e) // (20190604) Jeff Revised!
        {
            try
            {
                Abort_thread(); // 確保Thread已關閉

                // 觸發事件
                FormClosedEvent(true);
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        private void btnStop_Click(object sender, EventArgs e) // (20190604) Jeff Revised!
        {
            this.Close();
        }
    }
}
