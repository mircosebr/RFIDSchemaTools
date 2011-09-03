/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 01/02/2011
 * Time: 07:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using GemCard;

namespace RFIDKeybWedge
{
	/// <summary>
	/// Description of ARC122Reader.
	/// </summary>
	public class ACR122Reader
	{
		private CardNative iCard;
		private APDUCommand apduCmd;
		private APDUResponse apduResp;
		
		public ACR122Reader()
		{
			this.iCard = new CardNative();
						
		}
		
		public string[] GetReaders(){
			return this.iCard.ListReaders();
		}
	}
}
