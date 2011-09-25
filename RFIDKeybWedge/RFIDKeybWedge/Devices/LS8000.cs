/*
 * Created by SharpDevelop.
 * Author: Peter Brooks http://www.pbrooks.net
 * Date: 04/09/2011
 * Time: 15:45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using GemCard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using LS8000CommsLib;

namespace RFIDKeybWedge.Devices
{
	/// <summary>
	/// Description of LS8000.
	/// </summary>
	public class LS8000 : PluginDevice
	{
	
		private bool _connected;
		private static int _baud = 96000, _serialPort = -1;
		private static ushort _icDev;
		private static char _authMode = (char)0x60, _block = '8';
		private static string _key = "a0a1a2a3a4a5";
		
		public LS8000()
		{
			_connected = false;
			
		}
		
		public string getName()
		{
			return "LS8000";
		}
		
		public string[] devices(){
			return new string[]{
				"Com1",
				"Com2",
				"Com3",
				"Com4",
				"Com5",
				"Com6",
				"Com7",
				"Com8",
				"Com9",
				"Com10",
				"Com11",
				"Com12"
			};
		}
		
		public DeviceQuery select()
		{
			if(!connected())
			{
				return null;
			}
			
			LS8000Query query = new LS8000Query();
			
			return query;
		}
			
		public bool connect(string device)
		{
		
			_serialPort = Convert.ToInt32(device.Replace("Com",""));
			// check port number is valid
			if (_serialPort == -1)
				return false;
			// attempt to connect to the device
			int val = LS8000CommsLib.Comms.rf_init_com( _serialPort, _baud);
			
			// Check the return code for the device
			if (val == 0){
				_connected = true;
				return true;
			}else{
				return false;
			}
			
		}
		
		public bool disconnect()
		{
			// attempt to close the device
			int val = LS8000CommsLib.Comms.rf_ClosePort();
			// check if disconnect
			if (val == 0) {
				_connected = false;
				return true;
			}
			else {
				return false;
			}
		}
		
		public bool connected(){
			return _connected;
		}
		
		class  LS8000Query : DeviceQuery {
			private CardNative iCard;
			private int _numCards;
			private byte[] _tagType;
			private byte _tag;
			private byte[] _uid;
			
			
			public LS8000Query()
			{	
				_numCards=-1;
				select();
			}
			
			public void select()
			{

				ushort cType = 0;
				StringBuilder serial = new StringBuilder(), cardcap = new StringBuilder();
				char serlength, datalength;
				
				while (Comms.rf_request(_icDev, (char)0x26, ref cType) != 0) {
					System.Threading.Thread.Sleep(500);
				}
				
				//MessageBox.Show("Got card","Got card");
				
				
				if (Comms.rf_anticoll(_icDev, (char)0x04, serial, out serlength) != 0) {
					//MessageBox.Show("Could not perform anti collision on card!", "Card Error!",//MessageBoxButtons.OK,//MessageBoxIcon.Error);
				}
			
				String serialNumber = serial.ToString();
				
				if (Comms.rf_select(_icDev, serial, serlength, ref cardcap) != 0 ) {
					//MessageBox.Show("Could not activate card!", "Card Error!",//MessageBoxButtons.OK,//MessageBoxIcon.Error);
				}
					
			}		
			public bool authenticate(byte[] key, byte block)
			{
				if (Comms.rf_M1_authentication2(_icDev, _authMode, _block, _key) != 0) {
					//MessageBox.Show("Could not authenticate card, key error!", "Card Error!",//MessageBoxButtons.OK,//MessageBoxIcon.Error);
					return false;
				}else{
					return true;
				}
			}
			
			public byte[] readBlock(byte block)
			{
				StringBuilder carddata = new StringBuilder();
				char datalength;
				
				if (Comms.rf_M1_read(_icDev, (char)block, ref carddata, out datalength) != 0) {
					//MessageBox.Show("Could not read card!", "Card Error!",//MessageBoxButtons.OK,//MessageBoxIcon.Error);
				}
				
				Debug.WriteLine(carddata);
				
				Comms.rf_halt(_icDev);
				Comms.rf_beep(_icDev, '9');
				Comms.rf_light(_icDev, '2');
				
				return new byte[]{};
			}
			
			public int numCards(){ return _numCards; }
			
			public string tagType(){ return "Then maybe you shouldn't be living here!";  }
			
			public byte[] uid(){ return _uid; }
		}
		
		
	}
}

