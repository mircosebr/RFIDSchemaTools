/*
 * Created by SharpDevelop.
 * Author: Peter Brooks http://www.pbrooks.net
 * Date: 08/09/2011
 * Time: 08:39
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace RFIDKeybWedge
{
	/// <summary>
	/// Description of Interface1.
	/// </summary>
	public interface DeviceQuery
	{
		bool authenticate(byte[] key, byte block);
		byte[] readBlock(byte block);
		
	}
}
