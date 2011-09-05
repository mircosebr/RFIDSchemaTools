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

namespace RFIDKeybWedge.Devices
{
	/// <summary>
	/// Description of ACR122.
	/// </summary>
	public class ACR122 : PluginDevice
	{
		private CardNative iCard;
		private APDUCommand apduCmd;
		private APDUResponse apduResp;
		private bool _connected;
		
		public ACR122()
		{
			_connected = false;
			iCard = new CardNative();
		}
		
		public string getName()
		{
			return "ARC122U";
		}
		
		public string[] devices(){
			return this.iCard.ListReaders();
		}
		
		public bool connect(string device)
		{
			this.iCard.Connect(device,SHARE.Direct,PROTOCOL.T0orT1);
			_connected = true;
			return true;
		}
		public bool disconnect()
		{
			this.iCard.Disconnect(DISCONNECT.Leave);
			_connected = false;
			return true;
		}
		
		public void foo(){
			APDUCommand apdu = new APDUCommand(255, 0, 0, 0, new byte [  212, 96, 1, 1, 32, 35, 17, 4, 16],9);
			this.iCard.Transmit(apdu);
		}
		
		public bool connected(){
			return _connected;
		}
	}
}
