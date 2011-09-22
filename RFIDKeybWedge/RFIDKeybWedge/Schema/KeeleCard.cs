/*
 * Created by SharpDevelop.
 * Author: Peter Brooks http://www.pbrooks.net
 * Date: 05/09/2011
 * Time: 08:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace RFIDKeybWedge.Schema
{
	
	
	
	

	
	/// <summary>
	/// Description of KeeleCard.
	/// </summary>
	public class KeeleCard : PluginSchema
	{
		
		[System.Runtime.InteropServices.DllImport("user32.dll")]
	public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
		
		public const int MOUSEEVENTF_LEFTDOWN = 0x02;
	public const int MOUSEEVENTF_LEFTUP = 0x04;
	public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
	public const int MOUSEEVENTF_RIGHTUP = 0x10;
		
		
		private PluginDevice device;
		public static string name = "Keele Card";
		public string reader;
		
		public KeeleCard(PluginDevice device, string reader)
		{
			this.device = device;
			this.reader = reader;
		}
		
		public KeeleCard(PluginDevice device){
			
		}
		
		public string getName()
		{
			return "Keele Card";
		}
		
		
		
		public string readCard()
		{
			if(!device.connected()){
				device.connect(this.reader);
			}
			
			/*if(!device.connect(this.reader))
			{
				return null;
			}
			*/
			DeviceQuery query = device.select();
			if(!query.authenticate(new byte[6]{0xA0, 0xA1, 0xA2, 0xA3, 0xA4, 0xA5}, 0x08))
			{
				return null;
			}
			byte[] read = query.readBlock(0x08);
			
			string val = "";
			foreach(var b in read){
				byte bcd = b;
				ulong p = (ulong) ((bcd >> 0x04) * 10 + (bcd & 0x0F));
				
				if(p<10)
				{
					val+="0";
				}
				val+=p;
			}
			char[] arr = val.ToCharArray();
			Array.Reverse(arr);
			val = new string(arr);
			val = val.TrimStart('0');
			char[] cardNo = new char[8];
			Array.Copy(val.ToCharArray(),0,cardNo,0,8);

			device.disconnect();
			SendKeys.SendWait("72{ENTER}");
			System.Threading.Thread.Sleep(1000);
			SendKeys.SendWait(new string(cardNo));
			SendKeys.SendWait("{ENTER}");
			int x=10;
			int y=10;
			//device.disconnect();
			mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
			mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
			
			return new string(cardNo);
		}
	}
}
