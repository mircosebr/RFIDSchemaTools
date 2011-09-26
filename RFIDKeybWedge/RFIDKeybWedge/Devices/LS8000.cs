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
using System.Runtime.InteropServices;
using LS8000CommsLib;


namespace RFIDKeybWedge.Devices
{
	/// <summary>
	/// Description of LS8000.
	/// </summary>
	public class LS8000 : PluginDevice
	{
	
		private static bool _connected, _antennaPowered = false;
		private static int _baud = 9600, _serialPort = -1;
		private static ushort _icDev;
		private static byte _authMode = (byte)0x60, _block = 0x08;
		private static string _key = "a0a1a2a3a4a5";
		private static IntPtr serial, data;
		private static String serialNumber = "";
		
		public LS8000()
		{
			_connected = false;
			Debug.WriteLine("Created LS8000 Device");
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
			if (_serialPort == -1){
				Debug.WriteLine("LS8000:: Invalid serial port!");
				return false;
			}
			
			// attempt to connect to the device
			int val = LS8000CommsLib.Comms.rf_init_com( _serialPort, _baud);
			if (val != 0){
				Debug.WriteLine("LS8000:: Connecting to the device failed!");
				return false;
			}
			
			// Get the Decvice ID
			if (LS8000CommsLib.Comms.rf_get_device_number(ref _icDev) != 0){
				Debug.WriteLine("LS8000:: Couldn't get device number!");
				return false;
			}
			 
			// power up the attenna
			if (!AntennaControl(true)) {
				Debug.WriteLine("LS8000:: Could not turn on antenna!");
				disconnect();
				return false;
			}
			
			_connected = true;
			Debug.WriteLine("LS8000:: Connected to the LS8000");
			return true;
			
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
		
		
		private bool AntennaControl(bool antennaPower) {
			int retval;
			if (antennaPower) {
				if (!_antennaPowered) {
				
					Debug.WriteLine("LS8000:: Powering up antenna");
					retval = LS8000CommsLib.Comms.rf_antenna_sta(_icDev,'1');
					if (retval != 0) {
						Debug.WriteLine("LS8000:: Antenna Powered up!");
						return true;
					}else{
						Debug.WriteLine("LS8000:: Antenna Power-up failed!");
						return false;
					}
				}else{
					return true;
				}
				
			}else {
				//_readerPower = true;
				if (_antennaPowered) {
					Debug.WriteLine("LS8000:: Powering Down Antenna");
					retval = LS8000CommsLib.Comms.rf_antenna_sta(_icDev,'0');
					//MessageBox.Show("Return value from antenna power down: " + retval.ToString(), "Antenna status");
					if (retval == 0) {
						_antennaPowered = false;
						Debug.WriteLine("LS8000:: Antenna Powered Down sucessfully");
						return true;
					}else{
						Debug.WriteLine("LS8000:: There was a problem powering down the antenna");
						return true;
					}
				}
				else
					return true;
			}
			
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
			
			public void select(){
				
				
				
				
				ushort cType = 0;
			
				byte[] serialarray = new byte[4];
				byte[] serBytes = new byte[serialarray.Length]; 
				byte[] carddata = new byte[16];
				int sersize = Marshal.SizeOf(byte.MaxValue) * serialarray.Length;
				int datasize = Marshal.SizeOf(byte.MaxValue) * carddata.Length;
				
				serial = Marshal.AllocHGlobal(sersize);
				data = Marshal.AllocHGlobal(datasize);
				
				StringBuilder cardcap = new StringBuilder();
				
				String serialNumber = "";
				int serialLength = 0;
				
				byte serlength;
				byte datalength;
				
				int i = 0;
				
				while (Comms.rf_request(_icDev, (char)0x26, ref cType) != 0) {
					System.Threading.Thread.Sleep(500);
				}
				
				
				if (Comms.rf_anticoll(_icDev, (char)0x04, serial, out serlength) != 0) {
					Debug.WriteLine("LS8000:: Could not perform anti collision on card!");
				}
				
				if (serlength != null){
					serBytes = new byte[serialarray.Length];
					Marshal.Copy(serial, serBytes,0,serialarray.Length);	
					serialNumber = BitConverter.ToString(serBytes);
				}
				
				if (serialNumber == null) {
					Debug.WriteLine("LS8000:: Serial number is null!!");
				}

				if (serlength != null){
					serialLength = serlength;
				}
				
				if (Comms.rf_select(_icDev, serBytes, serlength, ref cardcap) != 0 ) {
					Debug.WriteLine("LS8000:: Could not activate card!");
				}
			
				serialNumber = serial.ToString();
				Debug.WriteLine("LS8000:: Serial is " + serialNumber);
				Marshal.FreeHGlobal(serial);
					
			}		
			
			public bool authenticate(byte[] key, byte block)
			{
				if (Comms.rf_M1_authentication2(_icDev, _authMode, block, key) != 0) {
					Debug.WriteLine("LS8000:: Could not authenticate card, key error!");
					return false;
				}else{
					return true;
				}
			}
			
			public byte[] readBlock(byte block)
			{
				StringBuilder carddata = new StringBuilder();
				byte datalength;
				
				if (Comms.rf_M1_read(_icDev, block, data, out datalength) != 0) {
					Debug.WriteLine("LS8000:: Could not read card!");
				}
				
				Debug.WriteLine("LS8000:: Card-data: " + carddata);
				
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

