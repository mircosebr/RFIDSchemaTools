/*
 * Created by SharpDevelop.
 * User: Mart
 * Date: 21/09/2010
 * Time: 10:28
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using RFIDKeybWedge.Devices;
using RFIDKeybWedge.Schema;
using System.Reflection;
namespace RFIDKeybWedge
{
	public sealed class NotificationIcon
	{
		private NotifyIcon notifyIcon;
		private ContextMenu notificationMenu;
		private static int _serialPort;
		private static bool _incCRLF = true;
		public static frmSerial serialConfig;
		private static ushort _icDev;
		private MenuItem serialConfigItem, crlfConfig, connectItem, disconnectItem;
		private bool _connectedToReader = false;
		private static long _baud = 9600;
		private static string _key = "a0a1a2a3a4a5";
		private static char _authmode = (char)0x60, _block = '8';
		private static Thread mainProgram;
		private static bool _abort = false;
		
		public static ReaderConfiguration readConfiguration;
		private PluginDevice device;
		private PluginSchema schema;
		
		//NotificationIcon notificationIcon;
		
		
		#region Initialize icon and menu
		public NotificationIcon()
		{
			readConfiguration = new ReaderConfiguration();
			//ACR122 acr122 = new ACR122();
			//KeeleCard keeleCard = new KeeleCard(acr122);
			
			serialConfigItem = new MenuItem("Configure Reader",menuSerialConfigClick);
			crlfConfig = new MenuItem("Send CR/LF",menuCRLFClick);
			connectItem = new MenuItem("Connect to Reader",menuConnectClick);
			disconnectItem = new MenuItem("Disconnect from Reader",menuDisconnectClick);
			notifyIcon = new NotifyIcon();
			notificationMenu = new ContextMenu(InitializeMenu());
			
			notifyIcon.DoubleClick += IconDoubleClick;
			notifyIcon.Click += new EventHandler(IconSingleClick);
			
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationIcon));
			notifyIcon.Icon = (Icon)resources.GetObject("$this.Icon");
			notifyIcon.ContextMenu = notificationMenu;
		}
		
		private MenuItem[] InitializeMenu()
		{
			MenuItem[] menu = new MenuItem[] {
				new MenuItem("About", menuAboutClick),
				new MenuItem("Exit", menuExitClick),
				serialConfigItem,
				crlfConfig,
				connectItem,
				disconnectItem
			};
			return menu;
		}
		#endregion
		
		#region Main - Program entry point
		/// <summary>Program entry point.</summary>
		/// <param name="args">Command Line Arguments</param>
		[STAThread]
		public static void Main(string[] args)
		{
			
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			bool isFirstInstance;
			// Please use a unique name for the mutex to prevent conflicts with other programs
			using (Mutex mtx = new Mutex(true, "RFIDKeybWedge", out isFirstInstance)) {
				if (isFirstInstance) {
					NotificationIcon notificationIcon = new NotificationIcon();
					notificationIcon.notifyIcon.Visible = true;
					if(notificationIcon.connectReader())
					{
						if (notificationIcon.connectedToReader()) {
							notificationIcon.connectItem.Enabled = false;
							notificationIcon.disconnectItem.Enabled = true;
						}
						else {
							notificationIcon.connectItem.Enabled = true;
							notificationIcon.disconnectItem.Enabled = false;
						}
					}else{
						notificationIcon.connectItem.Enabled = false;
						notificationIcon.disconnectItem.Enabled = false;
						serialConfig = new frmSerial();
						serialConfig.Show();
					}
					
					if (NotificationIcon._incCRLF) 
						notificationIcon.crlfConfig.Checked = true;

					
					//NotificationIcon.mainProgram = new Thread(notificationIcon.startSchemaRead);
					mainProgram = new Thread(new ThreadStart(notificationIcon.startSchemaRead));
					mainProgram.Start();
						
						
						
					//NotificationIcon.mainProgram = new Thread(notificationIcon.startSchemaRead);

					Application.Run();
					notificationIcon.notifyIcon.Dispose();
					//notificationIcon.startSchemaRead();
				
				} else {
					// The application is already running
					// TODO: Display message box or change focus to existing application instance
				}
			} // releases the Mutex
			
			
			
			
		}
		#endregion
		
		#region Event Handlers
		private void menuAboutClick(object sender, EventArgs e)
		{
			MessageBox.Show("RFID Keyboard Wedge");
		}
		
		private void menuExitClick(object sender, EventArgs e)
		{
			//NotificationIcon.setRegistryConfig();
			NotificationIcon._abort = true;
			NotificationIcon.mainProgram.Abort();
			Application.Exit();
		}
		
		private void IconDoubleClick(object sender, EventArgs e)
		{
			MessageBox.Show("RFID Keyboard Wedge");
		}
			
		private void IconSingleClick(object sender, EventArgs e) {
    }

		
		private void menuSerialConfigClick(object sender, EventArgs e) {
			if (serialConfig == null)
				serialConfig = new frmSerial();
			if (serialConfig.IsDisposed)
				serialConfig = new frmSerial();
			if (!serialConfig.Visible)
				serialConfig.Show();
		}
		
		private void menuCRLFClick(object sender, EventArgs e) {
			if (sender == crlfConfig) {
				if (crlfConfig.Checked) {
					_incCRLF = false;
					crlfConfig.Checked = false;
				}
				else {
					_incCRLF = true;
					crlfConfig.Checked = true;
				}
			}
		}
		private void menuConnectClick(object sender, EventArgs e) {
			/*
			if ((!connectedToReader()) && (_serialPort != -1)) {
				if (sender == connectItem) {
					connectedToReader = connectReader();
					if (connectedToReader) {
						connectItem.Enabled = false;
						disconnectItem.Enabled = true;
					}
				}
			}
			*/
		}
		
		private void menuDisconnectClick(object sender, EventArgs e) {
			/*
			if (connectedToReader) {
				if (sender == disconnectItem) {
					NotificationIcon.disconnectReader();
					if (!connectedToReader) {
						disconnectItem.Enabled = false;
						connectItem.Enabled = true;
					}
				}
			}
			*/
		}
		
		#endregion

		public void startSchemaRead()
		{
			while(!_abort){
				if(connectedToReader()){
					string cardNo = schema.readCard();
					//SendKeys.SendWait(cardNo);
					Debug.WriteLine("NotificationIcon:: " + cardNo);
					System.Threading.Thread.Sleep(200);
				}
			}
		}
		
		public  bool connectReader() {
			string configType = readConfiguration.getString("type");
			string configDevice = readConfiguration.getString("device");
			string configSchema = readConfiguration.getString("schema");
			if(configType == null || configDevice == null || configSchema == null)
			{
				return false;
			}
			
			LS8000 ls8000 = new LS8000();
			if(ls8000.getName().CompareTo(configType)==0)
			{
				device = ls8000;
			}
			
			ACR122 acr122 = new ACR122();
			if(acr122.getName().CompareTo(configType)==0)
			{
				device = acr122;
			}
			
			ACR122_Sim acr122_sim = new ACR122_Sim();
			
			if(acr122_sim.getName().CompareTo(configType)==0)
			{
				device = acr122_sim;
			}
			
			ACR122_v2 acr122_v2 = new ACR122_v2();
			if(acr122_v2.getName().CompareTo(configType)==0)
			{
				device = acr122_v2;
			}
			
			if(device == null){
				return false;
			}
			
			KeeleCard keeleCard = new KeeleCard(device,configDevice);
			if(keeleCard.getName().CompareTo(configSchema)==0)
			{
				schema = keeleCard;
			}
			
			if(schema==null){
				return false;
				//return device.connect(configDevice);
			}
			return true;
			/*
			if (_serialPort == -1)
				return false;
			int val = LS8000CommsLib.Comms.rf_init_com(_serialPort,NotificationIcon.baud);
			if (val == 0) 
				return true;
			
			else
				MessageBox.Show("Failed to connect to reader on Com " + _serialPort, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			*/
		}
		
		public bool connectedToReader()
		{
			return device!=null && schema!=null;
			/*
			if(device==null || this.schema == null)
			{
				return false;
			}
			return this.device.connected();
			*/
		}
		
		public static bool disconnectReader() {
			return false;
			//return this.reader==null;
			/*
			int val = LS8000CommsLib.Comms.rf_ClosePort();
			if (val == 0) {
				connectedToReader = false;
				return true;
			}
			else {
				MessageBox.Show("Failed to disconnect from reader on Com " + _serialPort, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
				}
			*/
		}
		
		private static void ReadCard() {
		/*
			char[] tempstore;
			string temp2;
			while (!NotificationIcon._abort) {
				if (connectedToReader) {
					KeeleCard kc = new KeeleCard();
					if(kc.ReadCard()) {
						tempstore = kc.keeleCardNumber.Substring(15, 10).ToCharArray();
						Array.Reverse(tempstore);
						temp2 = tempstore.ToString();
						if (incCRLF) {
							temp2 = tempstore.ToString() + "\r\n";
						}
						SendKeys.SendWait(tempstore.ToString());
						System.Threading.Thread.Sleep(1000);
					}
					kc = null;
				}
			}
			*/
		}
		
		#region Variable Handlers
		public static int serialPort {
			get { return _serialPort; }
			set { _serialPort = value; }
		}
		
		public static ushort icDev {
			get { return _icDev; }
			set { _icDev = value; }
		}
		
		public static bool incCRLF {
			get { return _incCRLF; }
			set { _incCRLF = value; }
		}
		
		public static long baud {
			get { return _baud; }
			set { _baud = value; }
		}
		public static string key {
			get { return _key; }
			set { _key = value; }
		}
		public static char block {
			get { return _block; }
			set { _block = value; }
		}
		public static char authMode {
			get { return _authmode; }
			set { _authmode = value; }
		}
		#endregion
		
	}
	
}
		/*
		public static void getRegistryConfig() {
			
			int regSerialPort;
			bool regIncCRLF;
			
			RegistryKey configStore = Registry.CurrentUser;
			
			configStore = configStore.OpenSubKey("SOFTWARE", true);
			configStore.CreateSubKey("Keele");
			configStore = configStore.CreateSubKey("Keele\\RFIDKeyboardWedge");
			
			if (configStore.GetValue("SerialPort") != null) {
				
				switch(configStore.GetValue("SerialPort").ToString())
				{
					case "1":
						regSerialPort = 1;
						break;
					case "2":
						regSerialPort = 2;
						break;
					case "3":
						regSerialPort = 3;
						break;
					case "4":
						regSerialPort = 4;
						break;
					case "5":
						regSerialPort = 5;
						break;
					case "6":
						regSerialPort = 6;
						break;
					case "7":
						regSerialPort = 7;
						break;
					case "8":
						regSerialPort = 8;
						break;
					case "9":
						regSerialPort = 9;
						break;
					case "10":
						regSerialPort = 10;
						break;
					case "11":
						regSerialPort = 11;
						break;
					case "12":
						regSerialPort = 12;
						break;
					default:
						regSerialPort = -1;
						break;
				}
			}
			else
				regSerialPort = -1;
			
			if (configStore.GetValue("IncludeCRLF") != null) {
				if (configStore.GetValue("IncludeCRLF").ToString().CompareTo("true") == 0)
					regIncCRLF = true;
				else
					regIncCRLF = false;
			}
			else regIncCRLF = false;
			
			configStore.Close();
			configStore = null;
			NotificationIcon._serialPort = regSerialPort;
			NotificationIcon._incCRLF = regIncCRLF;
		}
		*/
	/*
		public static void setRegistryConfig() {
			
			RegistryKey configStore = Registry.CurrentUser;
			
			try {
				configStore = configStore.OpenSubKey("SOFTWARE", true);
				configStore.CreateSubKey("Keele");
				configStore = configStore.CreateSubKey("Keele\\RFIDKeyboardWedge");
			
				if (_serialPort != -1)
					configStore.SetValue("SerialPort",NotificationIcon._serialPort.ToString());
			
				configStore.SetValue("IncludeCRLF",NotificationIcon._incCRLF.ToString());
				configStore.Close();
			}
			catch (Exception ex) {
			}
			finally {
				configStore = null;
			}
		}
		*/