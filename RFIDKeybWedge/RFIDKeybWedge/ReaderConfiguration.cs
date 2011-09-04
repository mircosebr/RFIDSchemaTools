/*
 * Created by SharpDevelop.
 * User: Peter Brooks http://www.pbrooks.net
 * Date: 03/09/2011
 * Time: 11:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using Microsoft.Win32;
using System;


namespace RFIDKeybWedge
{
	/// <summary>
	/// Description of ReaderConfiguration.
	/// </summary>
	public class ReaderConfiguration
	{
		private RegistryKey configStore;
		
		public ReaderConfiguration()
		{
			configStore = Registry.CurrentUser;
			configStore = configStore.OpenSubKey("SOFTWARE", true);
			if(configStore.OpenSubKey("RFIDSchemaTools\\KeyWedge")==null)
			{
				configStore.CreateSubKey("RFIDSchemaTools\\KeyWedge");
			}
			configStore = configStore.OpenSubKey("RFIDSchemaTools\\KeyWedge");
		
		}
		
		public Boolean keyExists(string key){
			return configStore.GetValue(key) == null;
		}
		
		public string getString(string key){
			return configStore.GetValue(key).ToString();
		}
		
		public void setString(string key, string value){
			configStore.SetValue(key,value);
		}
	}
}
