/*
 * Created by SharpDevelop.
 * User: Mart
 * Date: 23/09/2010
 * Time: 17:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace LS8000CommsLib
{
	/// <summary>
	/// Description of Comms.
	/// </summary>
	public class Comms
	{
		[DllImport("MasterRD.dll")]
		public static extern int rf_init_com(int port, long baud);
		
		[DllImport("MasterRD.dll")]
		public static extern int rf_ClosePort();
		
		[DllImport("MasterRD.dll")]
		public static extern int rf_antenna_sta(ushort icdev, char model);
		
		[DllImport("MasterRD.dll")]
		public static extern int rf_get_model(ushort icdev, 
		                                      ref StringBuilder pVersion, 
		                                      ref StringBuilder fpLen);
		
		[DllImport("MasterRD.dll")]
		public static extern int rf_get_device_number(ref ushort pIcdev);
			
		[DllImport("MasterRD.dll")]
		public static extern int rf_init_device_number(ushort icdev);
			
		[DllImport("MasterRD.dll")]
		public static extern int rf_beep(ushort icdev, 
			                                 char msec);
			
		[DllImport("MasterRD.dll")]
		public static extern int rf_light(ushort icdev, 
			                                  char color);
			
		[DllImport("MasterRD.dll")]
		public static extern int rf_init_type(ushort icdev, 
			                                      char type);
			
		[DllImport("MasterRD.dll")]
		public static extern int rf_request(ushort icdev, 
			                                    char  model, 
			                                    ref ushort pTagType);
			
		[DllImport("MasterRD.dll")]
		public static extern int rf_anticoll(ushort icdev, 
			                                     char  bcnt, 
			                                     ref StringBuilder pSnr,
			                                     out char pLen);
			
		[DllImport("MasterRD.dll")]
		public static extern int rf_select(ushort icdev,
			                                   string pSnr,
			                                   char  snrLen,
			                                   ref StringBuilder pSize);
			
		[DllImport("MasterRD.dll")]
		public static extern int rf_M1_authentication2(ushort icdev, 
			                                               char  model, 
			                                               char  block, 
			                                               string pKey);

			
		[DllImport("MasterRD.dll")]
		public static extern int rf_M1_read (ushort icdev, 
			                                     char  block, 
			                                     ref StringBuilder pData, 
			                                     out char pLen);

			
		[DllImport("MasterRD.dll")]
		public static extern int rf_halt(ushort icdev);
			
	}
}