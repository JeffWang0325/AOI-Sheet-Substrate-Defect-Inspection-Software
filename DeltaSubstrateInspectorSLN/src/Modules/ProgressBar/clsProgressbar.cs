using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

using System.Drawing; // (20200429) Jeff Revised!

namespace DeltaSubstrateInspector
{
    public class clsProgressbar
    {
        private ProgressDialog m_progress;
        /// <summary>
        /// 目前進度列的位置: 0~100 (20190604) Jeff Revised!
        /// </summary>
        private int m_nStep { get; set; }
        /// <summary>
        /// 是否正在執行進度條動作
        /// </summary>
        private bool isProcessRunning { get; set; } = false;
        private bool m_bRunning { get; set; } = false;

        /// <summary>
        /// 顯示文字
        /// </summary>
        private string m_strShow { get; set; } = "";

        /// <summary>
        /// 是否更新顯示文字
        /// </summary>
        private bool b_Update_strShow { get; set; } = false; // (20200429) Jeff Revised!

        /// <summary>
        /// 顯示文字位置
        /// </summary>
        private Point strShow_loc { get; set; } = new Point(); // (20200429) Jeff Revised!

        /// <summary>
        /// 顯示文字 Label尺寸
        /// </summary>
        private object strShow_size { get; set; } = null; // (20200429) Jeff Revised!

        /// <summary>
        /// 是否更新顯示文字位置
        /// </summary>
        private bool b_Update_strShow_loc { get; set; } = false; // (20200429) Jeff Revised!

        /// <summary>
        /// 標題文字
        /// </summary>
        private string m_strCaption { get; set; } = "";

        // (20190604) Jeff Revised!
        public delegate void FormClosedHandler2(); // 定義委派2
        public event FormClosedHandler2 FormClosedEvent2; // 定義事件2

        private bool b_trans2WaitProgress = false; // (20190611) Jeff Revised!

        public clsProgressbar()
        {
            this.m_progress = new ProgressDialog();
            this.m_nStep = 0;
            this.m_strShow = "";
        }

        public void ShowWaitProgress()
        {
            // If a process is already running, warn the user and cancel the operation
            if (this.isProcessRunning)
            {
                MessageBox.Show("A process is already running.");
                return;
            }

            // Initialize the dialog that will contain the progress bar
            

            m_bRunning = true;

            try
            {
                // Initialize the thread that will handle the background process
                Thread backgroundThread = new Thread(
                    new ThreadStart(() =>
                    {
                        // Set the flag that indicates if a process is currently running
                        isProcessRunning = true;

                        ProgressDialog progressDialog = new ProgressDialog();

                        progressDialog.FormClosedEvent += new ProgressDialog.FormClosedHandler(SetFormClosed); // (20190604) Jeff Revised!

                        progressDialog.SetShowText(this.m_strShow);
                        this.b_Update_strShow = false; // (20200429) Jeff Revised!
                        if (this.b_Update_strShow_loc) // (20200429) Jeff Revised!
                        {
                            progressDialog.SetShowText_location(this.strShow_loc, this.strShow_size);
                            this.b_Update_strShow_loc = false;
                            this.strShow_size = null;
                        }

                        progressDialog.SetShowCaption(m_strCaption);

                        // Set the dialog to operate in indeterminate mode
                        progressDialog.SetIndeterminate(true);

                        // Open the dialog
                        progressDialog.Show();

                        // Pause the thread for five seconds
                        while (m_bRunning)
                        {
                            if (this.b_Update_strShow) // (20200429) Jeff Revised!
                            {
                                progressDialog.SetShowText(m_strShow);
                                progressDialog.UpdateShowText();
                                this.b_Update_strShow = false;
                            }

                            // UI才會更新 (ProgressDialog的Timer持續作用) !
                            Application.DoEvents(); // 處理當前在消息隊列中的所有 Windows 消息，可以防止界面停止響應 (20190604) Jeff Revised!
                            Thread.Sleep(1); // Sleep一下，避免一直跑DoEvents()導致計算效率低 (20190604) Jeff Revised!
                        }

                        progressDialog.B_FormClosed = true; // (20190613) Jeff Revised!

                        // Close the dialog if it hasn't been already
                        // Note: 縱使沒有做progressDialog.Close()，但因為progressDialog是宣告在Thread內，因此Thread結束時記憶體自動釋放，視窗會自動關閉
                        if (progressDialog.InvokeRequired)
                            progressDialog.BeginInvoke(new Action(() => progressDialog.Close()));
                        else // (20190613) Jeff Revised!
                            progressDialog.Close();

                        // Reset the flag that indicates if a process is currently running
                        isProcessRunning = false;
                    }
                ));

                // Start the background process thread
                backgroundThread.Start();
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 顯示完成進度百分比(%)
        /// </summary>
        public void ShowRunProgress()
        {
            // If a process is already running, warn the user and cancel the operation
            if (isProcessRunning)
            {
                MessageBox.Show("A process is already running.");
                return;
            }

            m_nStep = 0;
            m_bRunning = true;

            // Initialize the dialog that will contain the progress bar
            
            try
            {
                // Initialize the thread that will handle the background process
                Thread backgroundThread = new Thread(
                    new ThreadStart(() =>
                    {
                        // Set the flag that indicates if a process is currently running
                        isProcessRunning = true;

                        ProgressDialog progressDialog = new ProgressDialog();

                        progressDialog.FormClosedEvent += new ProgressDialog.FormClosedHandler(SetFormClosed); // (20190604) Jeff Revised!

                        progressDialog.SetShowText(m_strShow);
                        this.b_Update_strShow = false; // (20200429) Jeff Revised!
                        if (this.b_Update_strShow_loc) // (20200429) Jeff Revised!
                        {
                            progressDialog.SetShowText_location(this.strShow_loc, this.strShow_size);
                            this.b_Update_strShow_loc = false;
                            this.strShow_size = null;
                        }
                        progressDialog.SetShowCaption(m_strCaption); // (20190604) Jeff Revised!

                        // Set the dialog to operate in determinate mode (20190604) Jeff Revised!
                        progressDialog.SetIndeterminate(false);

                        // Open the dialog
                        progressDialog.Show();

                        while (m_bRunning)
                        {
                            if (this.b_Update_strShow) // (20200429) Jeff Revised!
                            {
                                progressDialog.SetShowText(m_strShow);
                                progressDialog.UpdateShowText();
                                this.b_Update_strShow = false;
                            }

                            progressDialog.UpdateProgress(this.m_nStep);
                            // UI才會更新 (ProgressDialog的Timer持續作用) !
                            Application.DoEvents(); // 處理當前在消息隊列中的所有 Windows 消息，可以防止界面停止響應 (20190604) Jeff Revised!
                            Thread.Sleep(1); // Sleep一下，避免一直跑DoEvents()導致計算效率低 (20190604) Jeff Revised!
                        }

                        progressDialog.B_FormClosed = true; // (20190613) Jeff Revised!

                        // Close the dialog if it hasn't been already
                        // Note: 縱使沒有做progressDialog.Close()，但因為progressDialog是宣告在Thread內，因此Thread結束時記憶體自動釋放，視窗會自動關閉
                        if (progressDialog.InvokeRequired)
                            progressDialog.BeginInvoke(new Action(() => progressDialog.Close()));
                        else // (20190613) Jeff Revised!
                            progressDialog.Close();

                        // Reset the flag that indicates if a process is currently running
                        isProcessRunning = false;

                    }
                ));

                // Start the background process thread
                backgroundThread.Start();           
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 先執行RunProgress，在執行WaitProgress
        /// </summary>
        public void ShowRunProgress_thenWait()
        {
            // If a process is already running, warn the user and cancel the operation
            if (isProcessRunning)
            {
                MessageBox.Show("A process is already running.");
                return;
            }

            m_nStep = 0;
            m_bRunning = true;

            // Initialize the dialog that will contain the progress bar

            try
            {
                // Initialize the thread that will handle the background process
                Thread backgroundThread = new Thread(
                    new ThreadStart(() =>
                    {
                        // Set the flag that indicates if a process is currently running
                        isProcessRunning = true;

                        ProgressDialog progressDialog = new ProgressDialog();

                        progressDialog.FormClosedEvent += new ProgressDialog.FormClosedHandler(SetFormClosed); // (20190604) Jeff Revised!

                        progressDialog.SetShowText(m_strShow);
                        this.b_Update_strShow = false; // (20200429) Jeff Revised!
                        if (this.b_Update_strShow_loc) // (20200429) Jeff Revised!
                        {
                            progressDialog.SetShowText_location(this.strShow_loc, this.strShow_size);
                            this.b_Update_strShow_loc = false;
                            this.strShow_size = null;
                        }
                        progressDialog.SetShowCaption(m_strCaption); // (20190604) Jeff Revised!

                        // Set the dialog to operate in determinate mode (20190604) Jeff Revised!
                        progressDialog.SetIndeterminate(false);

                        // Open the dialog
                        progressDialog.Show();

                        while (m_bRunning)
                        {
                            if (this.b_Update_strShow) // (20200429) Jeff Revised!
                            {
                                progressDialog.SetShowText(m_strShow);
                                progressDialog.UpdateShowText();
                                this.b_Update_strShow = false;
                            }

                            progressDialog.UpdateProgress(m_nStep);
                            // UI才會更新 (ProgressDialog的Timer持續作用) !
                            Application.DoEvents(); // 處理當前在消息隊列中的所有 Windows 消息，可以防止界面停止響應 (20190604) Jeff Revised!
                            Thread.Sleep(1); // Sleep一下，避免一直跑DoEvents()導致計算效率低 (20190604) Jeff Revised!

                            if (b_trans2WaitProgress) // (20190611) Jeff Revised!
                                break;
                        }

                        // Transform into WaitProgress (20190611) Jeff Revised!
                        progressDialog.SetShowText(m_strShow);
                        progressDialog.UpdateShowText();
                        this.b_Update_strShow = false; // (20200429) Jeff Revised!
                        progressDialog.SetShowCaption(m_strCaption);
                        progressDialog.UpdateShowCaption();
                        progressDialog.SetIndeterminate(true);
                        while (m_bRunning)
                        {
                            if (this.b_Update_strShow) // (20200429) Jeff Revised!
                            {
                                progressDialog.SetShowText(m_strShow);
                                progressDialog.UpdateShowText();
                                this.b_Update_strShow = false;
                            }

                            // UI才會更新 (ProgressDialog的Timer持續作用) !
                            Application.DoEvents(); // 處理當前在消息隊列中的所有 Windows 消息，可以防止界面停止響應 (20190604) Jeff Revised!
                            Thread.Sleep(1); // Sleep一下，避免一直跑DoEvents()導致計算效率低 (20190604) Jeff Revised!
                        }

                        progressDialog.B_FormClosed = true; // (20190613) Jeff Revised!

                        // Close the dialog if it hasn't been already
                        // Note: 縱使沒有做progressDialog.Close()，但因為progressDialog是宣告在Thread內，因此Thread結束時記憶體自動釋放，視窗會自動關閉
                        if (progressDialog.InvokeRequired)
                            progressDialog.BeginInvoke(new Action(() => progressDialog.Close()));
                        else // (20190613) Jeff Revised!
                            progressDialog.Close();

                        // Reset the flag that indicates if a process is currently running
                        isProcessRunning = false;

                    }
                ));

                // Start the background process thread
                backgroundThread.Start();
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 設定/更新 顯示文字
        /// </summary>
        /// <param name="strText"></param>
        public void SetShowText(string strText) // (20200429) Jeff Revised!
        {
            this.m_strShow = strText;
            this.b_Update_strShow = true; // (20200429) Jeff Revised!
        }

        /// <summary>
        /// 設定顯示文字位置
        /// </summary>
        public void SetShowText_location(Point loc, object size = null) // (20200429) Jeff Revised!
        {
            this.strShow_loc = loc;
            this.b_Update_strShow_loc = true;
            this.strShow_size = size;
        }

        /// <summary>
        /// 設定標題文字
        /// </summary>
        /// <param name="strText"></param>
        public void SetShowCaption(string strText)
        {
            this.m_strCaption = strText;
        }

        /// <summary>
        /// 更新目前進度條的位置 (20190604) Jeff Revised!
        /// </summary>
        /// <param name="nStep"></param>
        public void SetStep(int nStep)
        {
            this.m_nStep = nStep;
        }

        /// <summary>
        /// 結束進度條
        /// </summary>
        public void CloseProgress()
        {
            this.m_bRunning = false;
        }

        /// <summary>
        /// 執行ShowRunProgress_thenWait()中，轉換到WaitProgress
        /// </summary>
        public void trans2WaitProgress() // (20190611) Jeff Revised!
        {
            b_trans2WaitProgress = true;
        }

        public bool bFormClosed = true;
        public void SetFormClosed(bool pmbFormClosed) // (20190604) Jeff Revised!
        {
            this.bFormClosed = pmbFormClosed;

            // 觸發事件2
            FormClosedEvent2();
        }
    }
}
