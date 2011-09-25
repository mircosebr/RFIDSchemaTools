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
	public class ACR122_v2 : PluginDevice
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
		private bool _connected;
		
		
		public ACR122_v2()
		{
			_connected = false;
			iCard = new CardNative();
		}
		
		public string getName()
		{
			return "ARC122U v2";
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
			ACR122Query_v2 query = new ACR122Query_v2(this.iCard);
			
			return query;
		}
			
		public bool connect(string device)
		{		
			
			String[] readers = this.iCard.ListReaders();
			if(readers == null){
				return false;
			}
	
			if(!_connected){
				try{
					this.iCard.Connect(device,SHARE.Direct,PROTOCOL.T0orT1);
				}catch(System.Exception){
					return false;;
				}
				_connected = true;
			}

			return true;
		}

		
		public bool disconnect()
		{
		
				
			this.iCard.Disconnect(DISCONNECT.Leave);
			_connected = false;
			return true;
		}
		
		public bool connected(){
			return _connected;
		}
		
		class  ACR122Query_v2 : DeviceQuery {
			private CardNative iCard;
			private int _numCards;
			private byte[] _tagType;
			private byte _tag;
			private byte[] _uid;
			
			
			public ACR122Query_v2(CardNative iCard)
			{
				this.iCard = iCard;
				_numCards=-1;
				select();
				
			}
			
			public void select()
			{
				//Firmware
				//APDUCommand c_f = new APDUCommand( 0xFF, 0x00, 0x48, 0x00, null, 0x00);
				//APDUResponse r_f = this.iCard.Transmit(c_f);
				
				//Select?
				APDUCommand cc0 = new APDUCommand( 0xFF, 0xCA, 0x00, 0x00 ,null, 0x04);
				try{
					APDUResponse rr0 = this.iCard.Transmit(cc0);
				}catch(System.Exception){
					;
				}
			}		
			
			public bool authenticate(byte[] key, byte block)
			{
				//Load authentication
				APDUCommand cc1 = new APDUCommand( 0xFF, 0x82, 0x00, 0x00, key, 0x06);
				try{
					APDUResponse rr1 = this.iCard.Transmit(cc1);
				}catch(System.Exception){
					return false;
				}
				//Authenticate
				APDUCommand cc2 = new APDUCommand(0xFF, 0x86, 0x00,  0x00, new byte[]{0x01,0x00,block,0x60,0x00} , 0x05);
				try{
					APDUResponse rr2 = this.iCard.Transmit(cc2);
				}catch(System.Exception){
					return false;
				}
				return true;
			}
			
			public byte[] readBlock(byte block)
			{
				//Read block
				APDUCommand cc3 = new APDUCommand(0xFF, 0xB0, 0x00, 0x08, null, 0x10);
				try{
					APDUResponse rr3 = this.iCard.Transmit(cc3);
					return rr3.Data;
				}catch(System.Exception){
					return null;
				}
				return null;
			}
			
			public int numCards(){ return _numCards; }
			
			public string tagType(){ return tagTypes[new Key(_tagType[0],_tagType[1])]; }
			
			public byte[] uid(){ return _uid; }
		}
		
		
	}
}

