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
		
		public ACR122()
		{
			iCard = new CardNative();
		}
		
		public string getName()
		{
			return "ARC122U";
		}
		
		public string[] devices(){
			return this.iCard.ListReaders();
		}
		
		public bool connect()
		{
			return false;
		}
		public bool disconnect()
		{
			return false;
		}
	}
}
