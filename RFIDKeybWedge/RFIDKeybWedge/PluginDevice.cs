/*
 * Created by SharpDevelop.
 * Author: Peter Brooks http://www.pbrooks.net
 * Date: 04/09/2011
 * Time: 15:39
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace RFIDKeybWedge
{
	/// <summary>
	/// Description of PluginDevice.
	/// </summary>
	public interface PluginDevice
	{
		string getName();
		string[] devices();
		bool connect(string device);
		bool disconnect();
		bool connected();
		DeviceQuery select();
		
	}
}
