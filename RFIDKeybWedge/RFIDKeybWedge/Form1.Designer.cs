/*
 * Created by SharpDevelop.
 * User: Mart
 * Date: 21/09/2010
 * Time: 15:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
 using System;
 using System.Collections;
 using System.Windows.Forms;
 using RFIDKeybWedge.Devices;
 using RFIDKeybWedge.Schema;

namespace RFIDKeybWedge
{
	partial class frmSerial
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		private ReaderConfiguration readConfig;
		private ACR122 acr122;
		private ACR122_Sim acr122_sim;
		private KeeleCard keeleCard;
		
		private System.Windows.Forms.Label lblConfigType;
		private System.Windows.Forms.Label lblConfigDevice;
		private System.Windows.Forms.Label lblSchema;
		private System.Windows.Forms.ComboBox ConfigDevice;
		private System.Windows.Forms.ComboBox ConfigType;
		private System.Windows.Forms.ComboBox Schema;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		
		//private System.Windows.Forms.Label lblConfigPCSCDevice;
		//private System.Windows.Forms.ComboBox ConfigPCSCDevice;
		//private System.Windows.Forms.ComboBox ConfigSerial;
		//private System.Windows.Forms.Label lblSerialPort;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			readConfig = NotificationIcon.readConfiguration;
			acr122 = new ACR122();
			acr122_sim = new ACR122_Sim();
			keeleCard = new KeeleCard(acr122);
			ConfigDeviceLoad();
			ConfigTypeLoad();
			SchemaLoad();
			BtnOKLoad();
			BtnCancelLoad();
			
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 168);
			this.Name = "frmSerial";
			this.Text = "Reader Configuration";
			this.Load += new System.EventHandler(this.FrmSerialLoad);
			
			this.SuspendLayout();
			this.ResumeLayout(false);
		}
		
		private string[] ConfigTypes()
		{
			string[] types = new string[]{
				acr122.getName(),
				acr122_sim.getName()
			};
			
			return types;
		}
		
		private string[] SchemaTypes()
		{
			string[] types = new string[1];
			types[0] = keeleCard.getName();
			return types;
		}
		
		private void ConfigTypeLoad()
		{	
			lblConfigType = new System.Windows.Forms.Label();
			lblConfigType.Location = new System.Drawing.Point(13, 10);
			lblConfigType.Name = "lblConfigType";
			lblConfigType.Size = new System.Drawing.Size(100, 23);
			lblConfigType.TabIndex = 5;
			lblConfigType.Text = "Reader Type";
			lblConfigType.Click += new System.EventHandler(this.Label1Click);
			
			ConfigType = new System.Windows.Forms.ComboBox();
			ConfigType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			ConfigType.FormattingEnabled = true;
			ConfigType.Location = new System.Drawing.Point(132, 10);
			ConfigType.Name = "ConfigType";
			ConfigType.Size = new System.Drawing.Size(121, 21);
			ConfigType.TabIndex = 4;
			ConfigType.SelectedIndexChanged += new System.EventHandler(this.Config_typeSelectedIndexChanged);
			ConfigType.Items.AddRange(ConfigTypes());
			
			IEnumerator configEnumerator = ConfigType.Items.GetEnumerator();
			while(configEnumerator.MoveNext())
			{
				string value = readConfig.getString("type");
				if(value!=null && value.CompareTo(configEnumerator.Current)==0)
				{
					ConfigType.SelectedItem = configEnumerator.Current;
				}
			}

			Controls.Add(lblConfigType);
			Controls.Add(ConfigType);
		}
		
		private void ConfigDeviceLoad()
		{
			lblConfigDevice = new System.Windows.Forms.Label();
			lblConfigDevice.Location = new System.Drawing.Point(13, 37);
			lblConfigDevice.Name = "lblConfigDevice";
			lblConfigDevice.Size = new System.Drawing.Size(100, 23);
			lblConfigDevice.TabIndex = 1;
			lblConfigDevice.Text = "Device";
			
			ConfigDevice = new System.Windows.Forms.ComboBox();
			ConfigDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			ConfigDevice.FormattingEnabled = true;
			ConfigDevice.Location = new System.Drawing.Point(132, 37);
			ConfigDevice.Name = "ConfigDevice";
			ConfigDevice.Size = new System.Drawing.Size(121, 21);
			ConfigDevice.TabIndex = 0;
			ConfigDevice.Visible = true;
			ConfigDevice.Enabled = false;
			//ConfigDevice.SelectedIndexChanged += new System.EventHandler(this.ConfigSerialSelectedIndexChanged);
			
			Controls.Add(lblConfigDevice);
			Controls.Add(ConfigDevice);
		}
		
		private void SchemaLoad()
		{
			lblSchema = new System.Windows.Forms.Label();
			lblSchema.Location = new System.Drawing.Point(13, 64);
			lblSchema.Name = "lblSchema";
			lblSchema.Size = new System.Drawing.Size(100, 23);
			lblSchema.TabIndex = 1;
			lblSchema.Text = "Schema";
			
			Schema = new System.Windows.Forms.ComboBox();
			Schema.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			Schema.FormattingEnabled = true;
			Schema.Location = new System.Drawing.Point(132, 64);
			Schema.Name = "Schema";
			Schema.Size = new System.Drawing.Size(121, 21);
			Schema.TabIndex = 0;
			Schema.Visible = true;
			Schema.Items.AddRange(SchemaTypes());
			//ConfigDevice.SelectedIndexChanged += new System.EventHandler(this.ConfigSerialSelectedIndexChanged);
			
			IEnumerator schemaEnumerator = Schema.Items.GetEnumerator();
			while(schemaEnumerator.MoveNext())
			{
				string value = readConfig.getString("schema");
				if(value!=null && value.CompareTo(schemaEnumerator.Current)==0)
				{
					Schema.SelectedItem = schemaEnumerator.Current;
				}
			}
			
			Controls.Add(lblSchema);
			Controls.Add(Schema);
		}
		
		private void BtnOKLoad()
		{
			btnOK = new System.Windows.Forms.Button();
			btnOK.Location = new System.Drawing.Point(38, 133);
			btnOK.Name = "btnOK";
			btnOK.Size = new System.Drawing.Size(75, 23);
			btnOK.TabIndex = 2;
			btnOK.Text = "OK";
			btnOK.UseVisualStyleBackColor = true;
			btnOK.Click += new System.EventHandler(this.BtnOKClick);
			
			Controls.Add(btnOK);
		}
		
		private void BtnCancelLoad()
		{
			btnCancel = new System.Windows.Forms.Button();
			btnCancel.Location = new System.Drawing.Point(178, 133);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new System.Drawing.Size(75, 23);
			btnCancel.TabIndex = 3;
			btnCancel.Text = "Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
			
			Controls.Add(btnCancel);
		}
	
		
		void BtnOKClick(object sender, System.EventArgs e)
		{
			
			if(ConfigDevice.Enabled && Schema.SelectedItem!=null)
			{
				readConfig.setString("type",ConfigType.SelectedItem.ToString());
				readConfig.setString("device",ConfigDevice.SelectedItem.ToString());
				readConfig.setString("schema",Schema.SelectedItem.ToString());
				this.Close();
				this.Dispose();
			}else{
				
			}
			/*** REMOVED ALPHA
			if(ConfigType.Text == "ACR122")
			{
				//NotificationIcon.ConfigType = 
			}else{
				
			}
			if(ConfigType.Text == "Riotec LS8000")
			{
				NotificationIcon.serialPort = Convert.ToInt32(ConfigSerial.SelectedItem.ToString().Substring(3,ConfigSerial.SelectedItem.ToString().Length-3));
			}else
			{
				NotificationIcon.serialPort = 0;				
			}
			NotificationIcon.setRegistryConfig();
			END REMOVED ALPHA**/
			
			
			/*
			if (ConfigSerial.SelectedText != null) {
				//System.Windows.Forms.MessageBox.Show(ConfigSerial.SelectedItem.ToString(),"Blah");
				//System.Windows.Forms.MessageBox.Show(ConfigSerial.SelectedItem.ToString().Substring(3,ConfigSerial.SelectedItem.ToString().Length-3),"Blah2");
				//NotificationIcon.serialPort = Convert.ToInt32(ConfigSerial.SelectedItem.ToString().Substring(3,ConfigSerial.SelectedItem.ToString().Length-3));
				//NotificationIcon.setRegistryConfig();
				this.Close();
				this.Dispose();
			}*/			
		}
		
		void BtnCancelClick(object sender, EventArgs e)
		{
			this.Close();
			this.Dispose();
		}
		
		void Label1Click(object sender, EventArgs e)
		{
			
		}
		
		void FrmSerialLoad(object sender, EventArgs e)
		{
			
		}
		
		void ConfigSerialSelectedIndexChanged(object sender, EventArgs e){
			
		}
		
		void Config_typeSelectedIndexChanged(object sender, EventArgs e)
		{
			ConfigDevice.Items.Clear();
			
				
			if(ConfigType.SelectedItem.ToString().CompareTo(acr122.getName())==0)
			{
				string[] devices = acr122.devices();
				if(devices!=null){
					ConfigDevice.Items.AddRange(devices);
				}
			}
			if(ConfigType.SelectedItem.ToString().CompareTo(acr122_sim.getName())==0)
			{
				string[] devices = acr122_sim.devices();
				if(devices!=null){
					ConfigDevice.Items.AddRange(devices);
				}
			}
			
			
			IEnumerator configEnumerator = ConfigDevice.Items.GetEnumerator();
			while(configEnumerator.MoveNext())
			{
				string value = readConfig.getString("device");
				if(value!=null && value.CompareTo(configEnumerator.Current)==0)
				{
					ConfigDevice.SelectedItem = configEnumerator.Current;
				}
			}

			
			ConfigDevice.Enabled=ConfigDevice.Items.Count>0;
		}
		/*
		void UpdateConfig(){
			if(this.ConfigType.Text=="Riotec LS8000"){
				this.ConfigSerial.Show();
			}else{
				this.ConfigSerial.Hide();
			}
			if(this.ConfigType.Text=="ARC122U"){
				ACR122Reader ACRRead = new ACR122Reader();
				
				String[] readers = ACRRead.GetReaders();
				if(readers!=null){
					foreach(string v in readers){
						this.ConfigPCSCDevice.Items.Add(v);
						
					}
				}
				
				this.ConfigPCSCDevice.Show();
			}else{
				this.ConfigPCSCDevice.Hide();
			}		
			
		}
		*/
	}
}

/*
		private void ConfigSerialLoad()
		{
			lblSerialPort = new System.Windows.Forms.Label();
			lblSerialPort.Location = new System.Drawing.Point(13, 37);
			lblSerialPort.Name = "lblSerialPort";
			lblSerialPort.Size = new System.Drawing.Size(100, 23);
			lblSerialPort.TabIndex = 1;
			lblSerialPort.Text = "Serial Port";
			
			ConfigSerial = new System.Windows.Forms.ComboBox();
			ConfigSerial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			ConfigSerial.FormattingEnabled = true;
			ConfigSerial.Items.AddRange(new object[] {
									"Com1",
									"Com2",
									"Com3",
									"Com4",
									"Com5",
									"Com6",
									"Com7",
									"Com8",
									"Com9",
									"Com10",
									"Com11",
									"Com12"});
			ConfigSerial.Location = new System.Drawing.Point(132, 37);
			ConfigSerial.Name = "ConfigSerial";
			ConfigSerial.Size = new System.Drawing.Size(121, 21);
			ConfigSerial.TabIndex = 0;
			ConfigSerial.Visible = false;
			ConfigSerial.SelectedIndexChanged += new System.EventHandler(this.ConfigSerialSelectedIndexChanged);

			Controls.Add(lblSerialPort);
			Controls.Add(ConfigSerial);
		}
		
		private void ConfigPCSCLoad()
		{
			lblConfigPCSCDevice = new System.Windows.Forms.Label();
			lblConfigPCSCDevice.Location = new System.Drawing.Point(13, 64);
			lblConfigPCSCDevice.Name = "lblConfigPCSCDevice";
			lblConfigPCSCDevice.Size = new System.Drawing.Size(100, 23);
			lblConfigPCSCDevice.TabIndex = 7;
			lblConfigPCSCDevice.Text = "PCSC Device";
			lblConfigPCSCDevice.Click += new System.EventHandler(this.Label1Click);
			
			ConfigPCSCDevice = new System.Windows.Forms.ComboBox();
			ConfigPCSCDevice.FormattingEnabled = true;
			ConfigPCSCDevice.Location = new System.Drawing.Point(132, 64);
			ConfigPCSCDevice.Name = "ConfigPCSCDevice";
			ConfigPCSCDevice.Size = new System.Drawing.Size(121, 21);
			ConfigPCSCDevice.TabIndex = 6;
			ConfigPCSCDevice.Visible = false;
			
			Controls.Add(lblConfigPCSCDevice);
			Controls.Add(ConfigPCSCDevice);
		}
		*/