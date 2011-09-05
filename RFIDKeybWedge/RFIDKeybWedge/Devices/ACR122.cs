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
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

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
			//foo();
			byte[] uid = getUID();
			authenticate(uid, 0x08, new byte[6] {0xA0, 0xA1, 0xA2, 0xA3, 0xA4, 0xA5});
			byte[] data = readBlock(0x08);
			IEnumerator en1 = data.GetEnumerator();
			while(en1.MoveNext())
			{
			
				Debug.WriteLine(en1.Current.ToString());
			}
			//string data = readBlock(0x08).ToString();
			byte[] read = readBlock(0x08);

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
			string cardNoString = new string(cardNo);
			
			disconnect();
			return true;
		}
		public bool disconnect()
		{
			this.iCard.Disconnect(DISCONNECT.Eject);
			_connected = false;
			return true;
		}
		
		private  byte[] getUID()
		{
			if(!connected())
			{
				return null;
			}
			//Make the request
			byte[] d1 = new byte[9] {  0xD4, 0x60, 0x01, 0x01, 0x20, 0x23, 0x11, 0x04, 0x10};
			APDUCommand a1 = new APDUCommand( 0xFF, 0x00, 0x00, 0x00, d1 ,0x09);
			APDUResponse r1 = this.iCard.Transmit(a1);
			
			//Retrieve the result
			APDUCommand a2 = new APDUCommand( 0XFF, 0xC0, 0x00, 0x00, null, r1.SW2);
			APDUResponse r2 = this.iCard.Transmit(a2);
			int data_len = r2.Data.Length;
			
			//Exceptions: No tags found, tag type not mifare (Schema?) Wrong type of tag
			
			//Retrieve the UID from the data returned
			byte[] UID = new byte[4];
			Array.Copy( r2.Data,data_len-4, UID, 0, 4);
			
			return UID;
		}
		
		public void authenticate(byte[] uid, byte block, byte[] key)
		{
			//Send authentication command
			byte[] d1 = new byte[] { 0xD4, 0x40, 0x01, 0x60, block, key[0], key[1], key[2], key[3], key[4], key[5],
				uid[0], uid[1], uid[2], uid[3]
			};
			APDUCommand a1 = new APDUCommand( 0xFF, 0x00, 0x00, 0x00, d1, 0x0F);
			APDUResponse r1 = this.iCard.Transmit(a1);
			
			//Status code 61 05 is valid
			
			
			//Read response
			APDUCommand a2 = new APDUCommand( 0xFF, 0xC0, 0x00, 0x00, null, 0x05);
			APDUResponse r2 = this.iCard.Transmit(a2);
			
			//Status code 90 00 is valid, else error
		}
		
		public byte[] readBlock(byte block)
		{
			byte[] d1 = new Byte[] { 0xD4, 0x40, 0x01, 0x30, block };
			APDUCommand a1 = new APDUCommand( 0xFF, 0x00, 0x00, 0x00, d1, 0x05);
			APDUResponse r1 = this.iCard.Transmit(a1);
			
			//Status code 61 15
			
			APDUCommand a2 = new APDUCommand( 0xFF, 0xC0, 0x00, 0x00, null, 0x15);
			APDUResponse r2 = this.iCard.Transmit(a2);
			
			//Status Code ??
			
			return r2.Data;
		}
		
		public void foo(){
			
			byte[] data = new byte[9] {  0xD4, 0x60, 0x01, 0x01, 0x20, 0x23, 0x11, 0x04, 0x10};
			APDUCommand apdu = new APDUCommand( 0xFF, 0x00, 0x00, 0x00, data ,0x09);
			APDUResponse r1 = this.iCard.Transmit(apdu);
	
			
			APDUCommand a2 = new APDUCommand( 0XFF, 0xC0, 0x00, 0x00, null, r1.SW2);
			APDUResponse r2 = this.iCard.Transmit(a2);
			int len = r2.Data.Length;
			
			byte[] d3 = new byte[] { 0xD4, 0x40, 0x01, 0x60, 0x08, 0xA0, 0xA1, 0xA2, 0xA3, 0xA4, 0xA5, 
				r2.Data[len-4],r2.Data[len-3],r2.Data[len-2],r2.Data[len-1]
			};
			APDUCommand a3 = new APDUCommand( 0xFF, 0x00, 0x00, 0x00, d3, 0x0F);
			APDUResponse r3 = this.iCard.Transmit(a3);
			
			APDUCommand a4 = new APDUCommand( 0xFF, 0xC0, 0x00, 0x00, null, 0x05);
			APDUResponse r4 = this.iCard.Transmit(a4);
			
			byte[] d5 = new Byte[] { 0xD4, 0x40, 0x01, 0x30, 0x08 };
			APDUCommand a5 = new APDUCommand( 0xFF, 0x00, 0x00, 0x00, d5, 0x05);
			APDUResponse r5 = this.iCard.Transmit(a5);
			
			APDUCommand a6 = new APDUCommand( 0xFF, 0xC0, 0x00, 0x00, null, 0x15);
			APDUResponse r6 = this.iCard.Transmit(a6);
			
			
			int i = 0;
		}
		
		public bool connected(){
			return _connected;
		}
	}
}
