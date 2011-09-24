/*
 * Created by SharpDevelop.
 * User: Mart
 * Date: 23/09/2010
 * Time: 22:20
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Windows.Forms;
using LS8000CommsLib;

namespace RFIDKeybWedge
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class KeeleCard_old
	{
		private String _serialNumber;
		private char _serialLength;
		private String _keeleCardNumber;
		private String _cardCapacity;
		private ushort _cardType;
		private String _kcLength;
		
		public KeeleCard_old()
		{
		}
		
		public String serialNumber {
			get { return _serialNumber; }
			set { _serialNumber = value; }
		}
		
		public char serialLength {
			get { return _serialLength; }
			set { _serialLength = value; }
		}
		
		public String keeleCardNumber {
			get { return _keeleCardNumber; }
			set { _keeleCardNumber = value; }
		}
		
		public String cardCapacity {
			get { return _cardCapacity; }
			set { _cardCapacity = value; }
		}
		
		public ushort cardType {
			get { return _cardType; }
			set { _cardType = value; }
		}
		
		public String kcLength {
			get { return _kcLength; }
			set { _kcLength = value; }
		}
		
		public bool ReadCard(){
			
			return false;
		}
		
		/*
		public bool ReadCard() {
			
			MessageBox.Show("In readcard");
			
			ushort cType = 0;
			StringBuilder serial = new StringBuilder(), cardcap = new StringBuilder(), carddata = new StringBuilder();
			char serlength, datalength;
			
			while (Comms.rf_request(NotificationIcon.icDev, (char)0x26, ref cType) != 0) {
				System.Threading.Thread.Sleep(500);
			}
			
			MessageBox.Show("Got card","Got card");
			cardType = cType;
			if (cType != 0) {
				if (Comms.rf_anticoll(NotificationIcon.icDev,(char)0x04, ref serial, out serlength) != 0) {
					MessageBox.Show("Could not perform anti collision on card!", "Card Error!",MessageBoxButtons.OK,MessageBoxIcon.Error);
					return false;
				}
				serialNumber = serial.ToString();
				if (serlength != null)
					serialLength = serlength;
				if (Comms.rf_select(NotificationIcon.icDev, serial.ToString(), serlength, ref cardcap) != 0 ) {
					MessageBox.Show("Could not activate card!", "Card Error!",MessageBoxButtons.OK,MessageBoxIcon.Error);
					return false;
				}
				if (Comms.rf_M1_authentication2(NotificationIcon.icDev, NotificationIcon.authMode, NotificationIcon.block, NotificationIcon.key) != 0) {
					MessageBox.Show("Could not authenticate card, key error!", "Card Error!",MessageBoxButtons.OK,MessageBoxIcon.Error);
					return false;
				}
				if (Comms.rf_M1_read(NotificationIcon.icDev, NotificationIcon.block, ref carddata, out datalength) != 0) {
					MessageBox.Show("Could not read card!", "Card Error!",MessageBoxButtons.OK,MessageBoxIcon.Error);
					return false;
				}
				
				cardCapacity = cardcap.ToString();
				keeleCardNumber = carddata.ToString();
				kcLength = datalength.ToString();
				Comms.rf_halt(NotificationIcon.icDev);
				Comms.rf_beep(NotificationIcon.icDev, '9');
				Comms.rf_light(NotificationIcon.icDev, '2');
				return true;
			}
			return false;
			
		}
		*/
		
		
	}
}
