/*
 * Created by SharpDevelop.
 * Author: Peter Brooks http://www.pbrooks.net
 * Date: 04/09/2011
 * Time: 15:45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace RFIDKeybWedge.Devices
{
	/// <summary>
	/// Description of ACR122.
	/// </summary>
	public class ACR122 : PluginDevice
	{
		public ACR122()
		{
		}
		
		public string getName()
		{
			return "ARC122";
		}
		
		public string[] devices(){
			return null;
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
