# Description
AOI Sheet Substrate Defect Inspection Software is installed in the AOI machine which can detect many kinds of defects by means of **【Image Processing Algorithm】** and/or **【AI (Deep Learning)】**.

AOI (Automated Optical Inspection) is a high-speed and high-precision optical image inspection system, which uses machine vision as the standard inspection technology to improve the traditional shortcomings of manual inspections using optical instruments.

Therefore, this software is equipped with **【IO】**,**【Camera Control】**, **【Light Control】**,**【TCP/IP】**,**【Visual Positioning】**,**【Laser Correction】**, etc.

It is written in C# and uses Windows Forms for its graphical user interface (GUI).

# Software Environment
| IDE                         | Visual Studio 2015/2019  |
| :-------------------------- | :----------------------- |
| .NET Framework              | .NET Framework 4.7.1     |
| Programming Language        | C#                       |
| HALCON                      | HALCON 13.0              |

# GUI Demo:

Please click the following figure or link to watch GUI demo videos:  
[AOI基板瑕疵檢測軟體 (AOI Substrate Defect Inspection Software)-Manual Review and Defect Integrated Output](https://youtu.be/tGu5Mw6vJDU)  
[![Everything Is AWESOME](http://img.youtube.com/vi/tGu5Mw6vJDU/sddefault.jpg)](https://youtu.be/tGu5Mw6vJDU)  

## ※Outline:   
***主題1: 單片檢測自動儲存瑕疵工單 (Single chip inspection automatically saves defective recipe)***
![alt text](https://github.com/JeffWang0325/AOI-Sheet-Substrate-Defect-Inspection-Software/blob/master/README%20Image/01.jpg "Logo Title Text 1")

●離線檢測>>單片檢測: 實際在機台執行檢測時，是使用 **【執行】** 模式。載入預先儲存之影像做模擬測試時，則使用 **【離線檢測】** 模式  
(OffLine Test>>SingleInsp: When actually performing the test on the machine, the **【Run】** mode is used. When loading pre-stored images for simulation test, use the **【OffLine Test】** mode)

●顯示檢測結果: 序號、運行結果、判定結果、良率 (%)、NG顆數、定位異常  
(Display Inspection Result: Serial Number, Running Result, Judgment Result, Yield (%), Number of NG, Abnormal Positioning)

***主題2: B面檢測完畢自動合併產生雙面工單 (Automatically merge and generate double-sided recipe after detecting side B)***
![alt text](https://github.com/JeffWang0325/AOI-Sheet-Substrate-Defect-Inspection-Software/blob/master/README%20Image/02.jpg "Logo Title Text 1")

●B面翻轉設定: 合併AB面方式，合併結果最終是以A面為依據  
(B Side Flip Setting: Combine AB side, the final result is based on A side)

●自動合併AB面，產生雙面工單  
(Automatically merge AB sides and generate double-sided recipe)

***主題3: 人工覆判 (Manual Review)***
![alt text](https://github.com/JeffWang0325/AOI-Sheet-Substrate-Defect-Inspection-Software/blob/master/README%20Image/03-1.jpg "Logo Title Text 1")

●人工方式針對檢測結果為NG之Cell，做人工覆判  
(Manually review the cells whose inspection results are NG)


![alt text](https://github.com/JeffWang0325/AOI-Sheet-Substrate-Defect-Inspection-Software/blob/master/README%20Image/03-2.jpg "Logo Title Text 1")

●人工覆判模式: 多顆標註模式、單顆標註模式  
(Manual Review Mode: Multiple Labelling Mode, Single Labelling Mode)

***主題4: 瑕疵整合輸出 (Defect Integrated Output)***
![alt text](https://github.com/JeffWang0325/AOI-Sheet-Substrate-Defect-Inspection-Software/blob/master/README%20Image/04-1.jpg "Logo Title Text 1")

![alt text](https://github.com/JeffWang0325/AOI-Sheet-Substrate-Defect-Inspection-Software/blob/master/README%20Image/04-2.jpg "Logo Title Text 1")

●整合多片之合併AB面瑕疵結果，輸出成PDF檔  
(Integrate the combined AB side defect results of multiple slices and output them as PDF files)

***主題5: 批量載入儲存所有序號 (Batch load and store all serial numbers)***
![alt text](https://github.com/JeffWang0325/AOI-Sheet-Substrate-Defect-Inspection-Software/blob/master/README%20Image/05-1.jpg "Logo Title Text 1")

●人員完全不需手動載入檔案，只需給定LotNum，即可一鍵搞定，一次滿足。  
(The users do not need to manually load files at all, just specify LotNum, and it can be done with one key and satisfied at one time.)


![alt text](https://github.com/JeffWang0325/AOI-Sheet-Substrate-Defect-Inspection-Software/blob/master/README%20Image/05-2.jpg "Logo Title Text 1")

●多片結果報表合併為一個PDF檔  
(Combine multiple result reports into one PDF file)

***主題6: 瑕疵分類統計 (Defect Classification & Statistics)***
![alt text](https://github.com/JeffWang0325/AOI-Sheet-Substrate-Defect-Inspection-Software/blob/master/README%20Image/06-1.jpg "Logo Title Text 1")

●整合AB面瑕疵類型: 顯示以A面為依據  
(Integrate AB Side Defect Category: Display based on side A)


![alt text](https://github.com/JeffWang0325/AOI-Sheet-Substrate-Defect-Inspection-Software/blob/master/README%20Image/06-2.jpg "Logo Title Text 1")

●多片瑕疵分類統計: 瑕疵名稱、NG顆數、良率 (%)  
(Multi-chip Defect Classification Statistics: Defect Name, Number of NG, Yield (%))

***主題7: 瑕疵標注卡參數設定 (Defect Marking Card Parameter Setting)***
![alt text](https://github.com/JeffWang0325/AOI-Sheet-Substrate-Defect-Inspection-Software/blob/master/README%20Image/07.jpg "Logo Title Text 1")

●調整參數，使每顆Cell落於正確位置上  
(Fine-tune the parameters to make each cell locate in the correct position)

●瑕疵標注卡輸出格式: 影像、Word、PDF  
(Defect Marking Card Output Format: Image, Word, PDF)

---
# Contact Information:
If you have any questions or suggestions about code, project or any other topics, please feel free to contact me and discuss with me. 😄😄😄

<a href="https://www.linkedin.com/in/tzu-wei-wang-a09707157" target="_blank"><img src="https://github.com/JeffWang0325/JeffWang0325/blob/master/Icon%20Image/linkedin_64.png" width="64"></a>
<a href="https://www.youtube.com/channel/UC9nOeQSWp0PQJPtUaZYwQBQ" target="_blank"><img src="https://github.com/JeffWang0325/JeffWang0325/blob/master/Icon%20Image/youtube_64.png" width="64"></a>
<a href="https://www.facebook.com/tzuwei.wang.33/" target="_blank"><img src="https://github.com/JeffWang0325/JeffWang0325/blob/master/Icon%20Image/facebook_64.png" width="64"></a>
<a href="https://www.instagram.com/tzuweiw/" target="_blank"><img src="https://github.com/JeffWang0325/JeffWang0325/blob/master/Icon%20Image/instagram_64.png" width="64"></a>
<a href="https://www.kaggle.com/tzuweiwang" target="_blank"><img src="https://github.com/JeffWang0325/JeffWang0325/blob/master/Icon%20Image/kaggle_64.png" width="64"></a>
<a href="https://github.com/JeffWang0325" target="_blank"><img src="https://github.com/JeffWang0325/JeffWang0325/blob/master/Icon%20Image/github_64.png" width="64"></a>
