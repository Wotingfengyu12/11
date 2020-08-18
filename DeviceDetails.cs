using FieldIot.HARTDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQC.ConTest
{
    class DeviceDetails
    {
		public const int DEVICE_RESPONSECODE = 150;
		public const int DEVICE_COMM48_STATUS = 151; /* actually Device Status */
		public const int DEVICE_COMM_STATUS = 152;
		public const int DEVICE_MFG_ID = 153;
		public const int DEVICE_DEVICE_TYPE = 154;
		public const int DEVICE_XMTR_REVISION = 157;
		public const int DEVICE_HART_REV_LEVEL = 156;/* 5 till six arrives */
		public const int DEVICE_REQST_PREAMBLES = 155;
		public const int DEVICE_SFTWR_REVISION = 158;
		public const int DEVICE_HDWR_REVISION = 159;
		public const int DEVICE_POLL_ADDRESS = 162;
		public const int DEVICE_TAG = 163;
		public const int DEVICE_DISTRIBUTOR = 168;
		public const int DEVICE_ID = 210;
		public const int DEVICE_PROCESS_VARIABLES = 7003;
		public const int DEVICE_FACE_PLATE = 7024;
		/* VMKP added on 310304 */
		public const int DEVICE_BURST_MODE = 0xFFF;
		public const int DEVICE_BURST_OPTION = 0xFFD;
		/* VMKP added on 310304 */
		public const int MORE_STATUS_AVAIL = 0x10;/* bit 4 set */

		public string m_strDDRevision;
		public string m_strDistributor;
		public string m_strRevision;
		public string m_strType;
		public string strDDfile;

		public DeviceDetails()
		{
			m_strDDRevision = "";
			m_strDistributor = "";
			m_strRevision = "";
			m_strType = "";
			strDDfile = "";
		}

		public List<string> getDevDetails(HARTDevice dev)
		{
			List<string> devInfo = new List<string>();
			CDDLBase pItemBase = null;
			if (dev.getItembyID(DEVICE_DISTRIBUTOR, ref pItemBase))
			{
				if (pItemBase.eType == nitype.nVar)
				{
					CDDLVar devdis = pItemBase as CDDLVar;
					devdis.getDispValue().SetValue((int)dev.ddbDeviceID.wManufacturer, valueType_t.isIntConst);
					m_strDistributor = devdis.GetDispString();
				}
			}

			if (dev.getItembyID(DEVICE_DEVICE_TYPE, ref pItemBase))
			{
				if (pItemBase.eType == nitype.nVar)
				{
					CDDLVar devtype = pItemBase as CDDLVar;
					m_strType = devtype.GetDispString();
				}
			}

			m_strDDRevision = dev.parentform.strDDRevision;
			m_strRevision = dev.parentform.strDeviceRevision;
			devInfo.Add(m_strDistributor);
			devInfo.Add(m_strType);
			devInfo.Add(m_strDDRevision);
			devInfo.Add(m_strRevision);
			return devInfo;
			//strDDfile = dev.;
		}

	}

	class DeviceStatus
	{
		int nCommStatusValue;
		int nDevStatusValue;

		public string strDevStatus;
		public string strCommStatus;

		public DeviceStatus()
		{
			nCommStatusValue = 0;
			nDevStatusValue = 0;
			strDevStatus = "";
			strCommStatus = "";
		}

		public void getDevCommStatus(HARTDevice dev)
		{
			CDDLBase pItemBase = null;
			if (dev.getItembyID(DeviceDetails.DEVICE_COMM_STATUS, ref pItemBase))
			{
				if (pItemBase.eType == nitype.nVar)
				{
					CDDLVar comstatus = pItemBase as CDDLVar;
					nCommStatusValue = comstatus.getDispValue().GetInt();
				}
			}

			if (dev.getItembyID(DeviceDetails.DEVICE_COMM48_STATUS, ref pItemBase))
			{
				if (pItemBase.eType == nitype.nVar)
				{
					CDDLVar devstatus = pItemBase as CDDLVar;
					nDevStatusValue = devstatus.getDispValue().GetInt();
				}
			}
		}

		/*
		public string getDevStatusString(Context ct)
		{
			string str = "";
			if ((nDevStatusValue & 0x80) != 0)
			{
				str += ct.Resources.GetString(Resource.String.strStatusStrings0) + "\r\n";
			}
			if ((nDevStatusValue & 0x40) != 0)
			{
				str += ct.Resources.GetString(Resource.String.strStatusStrings1) + "\r\n";
			}
			if ((nDevStatusValue & 0x20) != 0)
			{
				str += ct.Resources.GetString(Resource.String.strStatusStrings2) + "\r\n";
			}
			if ((nDevStatusValue & 0x10) != 0)
			{
				str += ct.Resources.GetString(Resource.String.strStatusStrings3) + "\r\n";
			}
			if ((nDevStatusValue & 0x08) != 0)
			{
				str += ct.Resources.GetString(Resource.String.strStatusStrings4) + "\r\n";
			}
			if ((nDevStatusValue & 0x04) != 0)
			{
				str += ct.Resources.GetString(Resource.String.strStatusStrings5) + "\r\n";
			}
			if ((nDevStatusValue & 0x02) != 0)
			{
				str += ct.Resources.GetString(Resource.String.strStatusStrings6) + "\r\n";
			}
			if ((nDevStatusValue & 0x01) != 0)
			{
				str += ct.Resources.GetString(Resource.String.strStatusStrings7) + "\r\n";
			}

			if (str == "")
			{
				str = ct.Resources.GetString(Resource.String.nonestatus);
			}

			return str;
		}

		public string getCommStatusString(Context ct)
		{
			string str = "";

			if ((nCommStatusValue & 0x80) != 0)
			{
				str += ct.Resources.GetString(Resource.String.strCommStrings0) + "\r\n";
			}
			if ((nCommStatusValue & 0x40) != 0)
			{
				str += ct.Resources.GetString(Resource.String.strCommStrings1) + "\r\n";
			}
			if ((nCommStatusValue & 0x20) != 0)
			{
				str += ct.Resources.GetString(Resource.String.strCommStrings2) + "\r\n";
			}
			if ((nCommStatusValue & 0x10) != 0)
			{
				str += ct.Resources.GetString(Resource.String.strCommStrings3) + "\r\n";
			}
			if ((nCommStatusValue & 0x08) != 0)
			{
				str += ct.Resources.GetString(Resource.String.strCommStrings4) + "\r\n";
			}
			if ((nCommStatusValue & 0x04) != 0)
			{
				str += ct.Resources.GetString(Resource.String.strCommStrings5) + "\r\n";
			}
			if ((nCommStatusValue & 0x02) != 0)
			{
				str += ct.Resources.GetString(Resource.String.strCommStrings6) + "\r\n";
			}
			if ((nCommStatusValue & 0x01) != 0)
			{
				str += ct.Resources.GetString(Resource.String.strCommStrings7) + "\r\n";
			}

			if (str == "")
			{
				str = ct.Resources.GetString(Resource.String.nonestatus);
			}

			return str;
		}
		*/
	}
}
