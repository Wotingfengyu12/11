using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldIot.HARTDD
{
    //struct fileInfo_s
    //{
    //    int LoFileTime;
    //    int HiFileTime;
    //    // end of FILETIME look-alike

    //    string file_name;
    //}
    
    //class DeviceMgr
    //{
    //    //Identity_s



    //}

    public class Identity_s
    {
        public ushort wManufacturer; //this		upgraded for hart 7 07feb07
        public ushort wDeviceType;   //and this	upgraded for hart 7 07feb07
        public byte cUniversalRev;
        public byte cDeviceRev;
        public byte cSoftwareRev;
        public byte cHardwareRev;// and Signalling Code!
        public byte cZeroFlags;
        /* VMKP added on 311203*/
        public byte PollAddr;
        /* VMKP added on 311203*/
        public byte cReqPreambles;
        public int dwDeviceId;   //and this; gives a unique instance ID
                                    //Added for HART 7 capability
        public byte cRespPreambles;
        public byte cMaxDevVars;
        public ushort wCfgChngCnt;
        //byte cInternalFlags;
        public byte cExtDevStatus;
        // end 07feb07 additions
        // used during identify to differentiate phases stevev 29jun07
                             /* VMKP added on 030404 */
        public Identity_s()
        {
            clear();
        }

        //Identity_s() { clear(); };

        /* VMKP added on 030404 */
        public void clear()
        {
            wManufacturer = wDeviceType = wCfgChngCnt = 0; 
            cUniversalRev = PollAddr = 0;
            cDeviceRev = cSoftwareRev = cHardwareRev = cZeroFlags = cReqPreambles = 0;
            dwDeviceId = 0; 
            cRespPreambles = cMaxDevVars = cExtDevStatus = 0;
            //cInternalFlags = 0;
        }
        public bool isEmpty()
        {
            return ((wManufacturer | wDeviceType | cUniversalRev | cDeviceRev |
                cSoftwareRev | cHardwareRev | cZeroFlags | cReqPreambles | PollAddr | dwDeviceId |
                cRespPreambles | cMaxDevVars | wCfgChngCnt | cExtDevStatus/*|cInternalFlags*/) == 0L);
        }//stevev - 24oct07 - fix the selection window value not found issues
        
        /*
        public struct Identity_s& operator = (const struct Identity_s& s)
	{ wManufacturer=s.wManufacturer; wDeviceType=s.wDeviceType;  cUniversalRev=s.cUniversalRev;
      cDeviceRev=s.cDeviceRev;   cSoftwareRev=s.cSoftwareRev;   cHardwareRev=s.cHardwareRev;
      cZeroFlags=s.cHardwareRev; PollAddr=s.PollAddr;   cReqPreambles=s.cReqPreambles;
      dwDeviceId=s.dwDeviceId;   cRespPreambles=s.cRespPreambles;   cMaxDevVars=s.cMaxDevVars;
      wCfgChngCnt=s.wCfgChngCnt; cExtDevStatus=s.cExtDevStatus; cInternalFlags=s.cInternalFlags;
	  return (*this); };*/
    }
}
