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

namespace RFIDKeybWedge.Devices
{
	/// <summary>
	/// Description of ACR122.
	/// </summary>
	public class ACR122 : PluginDevice
	{
		struct Key{
			public readonly byte dim1;
			public readonly byte dim2;
			public Key(byte d1, byte d2){
				dim1 = d1;
				dim2 = d2;
			}
		}
		
		private static readonly Dictionary<Key, string> tagTypes 
			= new Dictionary<Key, string>
		{
			{ new Key(0x00, 0x01), "mifare1k"},
			{ new Key(0x00, 0x02), "mifare4k" },
			{ new Key(0x00, 0x03), "mifareUltraLight" },
			{ new Key(0x00, 0x26), "mifareMini" },
			{ new Key(0xF0, 0x04), "topazJewl" },
			{ new Key(0xF0, 0x11), "felica212k" },
			{ new Key(0xF0, 0x12), "felica424k" }
		};
		
		private static readonly Dictionary<byte, string> errorMessages
			= new Dictionary<byte, string>
		{
			{0x00, "No error"},
			{0x01, "Time out, the target has not answered"},
			{0x02, "A CRC error has been detected by the contactless UART"},
			{0x03, "A parity error has been detected by the contactless UART"},
			{0x04, "During a MIFARE anti-collision/select operation, an errorneous bit count has been detected."},
			{0x05, "Framing error during MIFARE operation"},
			{0x06, "An abnormal bit-collision has been detected during bit wise anti-collision at 106 kbs"},
			{0x07, "Communication buffer size insufficient"},
			{0x08, "RF buffer overflow has been detected by the contactless UART (bin BufferOvfl of the register CL_ERROR"},
			{0x0A, "In active communication mode, the RF field ahs not been switched on in time by the counterpart (as defined in NFCIP-1 standard"},
			{0x0B, "RF protocol error"},
			{0x0D, "Temperature error: the internal temperature sensor has detected oveheating, and therefore has automatically switched off the antenna drivers"},
			{0x0E, "Internal buffer overflow"},
			{0x10, "Invalid parameter (range, format, ...)"}
			//{0x12, "DEP Protocol: The chip configured in target mode does not suppor 
			/// </summary>
		
		};
		


		
		private CardNative iCard;
		private APDUCommand apduCmd;
		private APDUResponse apduResp;
		private static bool _connected;
		
		
		public ACR122()
		{
			_connected = false;
		}
		
		public string getName()
		{
			return "ARC122U";
		}
		
		public string[] devices(){
			return this.iCard.ListReaders();
		}
		
		public DeviceQuery select()
		{
			if(!connected())
			{
				return null;
			}
			ACR122Query query = new ACR122Query(this.iCard);
			
			return query;
		}
			
		public bool connect(string device)
		{	
			
			iCard = new CardNative();
			
			String[] readers;
			
			try{
				readers = this.iCard.ListReaders();
			}catch(System.Exception){
				Debug.WriteLine("ACR122:: connect, ListReaders() failed");
				return false;
			}
			
			if(readers == null){
				Debug.WriteLine("ARC122:: No Devices connected!");
				return false;
			}
			
			bool ready = false;
				
			while(!ready){
				try{
					this.iCard.Connect(device, SHARE.Shared, PROTOCOL.T0orT1);
					_connected = true;
					ready = true;					
				}catch(System.Exception e){
					//Debug.WriteLine("ACR112:: ERROR, " + e.Message);
					Debug.WriteLine("Device not ready");
					System.Threading.Thread.Sleep(1500);
				}
			}

			Debug.WriteLine("ACR122:: Device Connected!");
			return true;
		}
	
		public bool disconnect()
		{
			APDUCommand c1 = new APDUCommand(0xFF, 0x00, 0x00, 0x00, new byte[]{0xD4, 0x44, 0x01}, 0x03);
			APDUResponse r1 = iCard.Transmit(c1);
	
			APDUCommand c2 = new APDUCommand(0xFF, 0xC0, 0x00, 0x00, null, 0x05);
			APDUResponse r2 = iCard.Transmit(c2);
			this.iCard.Disconnect(DISCONNECT.Reset);
			_connected = false;
			return true;
		}
		
		public bool connected(){
			return _connected;
		}
		
		class  ACR122Query : DeviceQuery {
			private CardNative iCard;
			private int _numCards;
			private byte[] _tagType;
			private byte _tag;
			private byte[] _uid;
			
			
			public ACR122Query(CardNative iCard)
			{
				this.iCard = iCard;
				_numCards=-1;
				select();
				
			}
			
			public void select()
			{
				//Make the request
				byte[] d1 = new byte[9] {  0xD4, 0x60, 0x01, 0x01, 0x20, 0x23, 0x11, 0x04, 0x10};
				APDUCommand a1 = new APDUCommand( 0xFF, 0x00, 0x00, 0x00, d1 ,0x09);
				APDUResponse r1;
				try{
					r1 = this.iCard.Transmit(a1);
				}catch(System.Exception){
					return;
				}
				//Retrieve the result
				APDUCommand a2 = new APDUCommand( 0XFF, 0xC0, 0x00, 0x00, null, r1.SW2);
				APDUResponse r2;
				try{
					
					r2 = this.iCard.Transmit(a2);
				}catch(System.Exception){
					return;
				}
				int data_len = r2.Data.Length;
				
				
				//Exceptions: No tags found, tag type not mifare (Schema?) Wrong type of tag
				if(data_len==3){
					return;
				}
				//Retrieve the UID from the data returned
				_uid = new byte[4];
				Array.Copy( r2.Data,data_len-4, _uid, 0, 4);
				
				//Store card information
				_numCards = r2.Data[2];
				_tagType = new Byte[]{
					r2.Data[3],
					r2.Data[4]
				};
				_tag = r2.Data[7]; 
			}
			
			
			
			public bool authenticate(byte[] key, byte block)
			{
				byte[] card_uid=uid();
				if(card_uid==null)
				{
					return false;
				}
				//Send authentication command
				byte[] d1 = new byte[] { 0xD4, 0x40, 0x01, 0x60, block, key[0], key[1], key[2], key[3], key[4], key[5],
					card_uid[0], card_uid[1], card_uid[2], card_uid[3]
				};
				APDUCommand a1 = new APDUCommand( 0xFF, 0x00, 0x00, 0x00, d1, 0x0F);
				try{
					APDUResponse r1 = this.iCard.Transmit(a1);
				}catch(System.Exception){
					return false;
				}
				//Status code 61 05 is valid
				
				
				//Read response
				APDUCommand a2 = new APDUCommand( 0xFF, 0xC0 , 0x00, 0x00, null, 0x05);
				try{
					APDUResponse r2 = this.iCard.Transmit(a2);
				}catch(System.Exception){
					return false;
				}
				//Status code 90 00 is valid, else error
				return true;
			}
			
			public byte[] readBlock(byte block)
			{
				byte[] d1 = new Byte[] { 0xD4, 0x40, 0x01, 0x30, block };
				APDUCommand a1 = new APDUCommand( 0xFF, 0x00, 0x00, 0x00, d1, 0x05);
				try{
					APDUResponse r1 = this.iCard.Transmit(a1);
				}catch(System.Exception){
					return null;
				}
				//Status code 61 15
				
				APDUCommand a2 = new APDUCommand( 0xFF, 0xC0, 0x00, 0x00, null, 0x15);
				APDUResponse r2;
				try{
					
				
				r2 = this.iCard.Transmit(a2);
				}catch(System.Exception){
					return null;
				}
				
				//Status Code ??			
				_connected = false;
				
				return r2.Data;
			}
			
			public int numCards(){ return _numCards; }
			
			public string tagType(){ return tagTypes[new Key(_tagType[0],_tagType[1])]; }
			
			public byte[] uid(){ return _uid; }
		}
		
		
	}
}

