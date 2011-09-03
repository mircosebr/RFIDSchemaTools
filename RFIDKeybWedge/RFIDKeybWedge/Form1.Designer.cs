/*
 * Created by SharpDevelop.
 * User: Mart
 * Date: 21/09/2010
 * Time: 15:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
 using System;
 using System.Windows.Forms;

namespace RFIDKeybWedge
{
	partial class frmSerial
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
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
			this.ConfigSerial = new System.Windows.Forms.ComboBox();
			this.lblSerialPort = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.ConfigType = new System.Windows.Forms.ComboBox();
			this.lblConfigType = new System.Windows.Forms.Label();
			this.ConfigPCSCDevice = new System.Windows.Forms.ComboBox();
			this.lblConfigPCSCDevice = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// ConfigSerial
			// 
			this.ConfigSerial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ConfigSerial.FormattingEnabled = true;
			this.ConfigSerial.Items.AddRange(new object[] {
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
			this.ConfigSerial.Location = new System.Drawing.Point(132, 37);
			this.ConfigSerial.Name = "ConfigSerial";
			this.ConfigSerial.Size = new System.Drawing.Size(121, 21);
			this.ConfigSerial.TabIndex = 0;
			this.ConfigSerial.Visible = false;
			this.ConfigSerial.SelectedIndexChanged += new System.EventHandler(this.ConfigSerialSelectedIndexChanged);
			// 
			// lblSerialPort
			// 
			this.lblSerialPort.Location = new System.Drawing.Point(13, 37);
			this.lblSerialPort.Name = "lblSerialPort";
			this.lblSerialPort.Size = new System.Drawing.Size(100, 23);
			this.lblSerialPort.TabIndex = 1;
			this.lblSerialPort.Text = "Serial Port";
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(38, 133);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.BtnOKClick);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(178, 133);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
			// 
			// ConfigType
			// 
			this.ConfigType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ConfigType.FormattingEnabled = true;
			this.ConfigType.Items.AddRange(new object[] {
									"ARC122U",
									"Riotec LS8000"});
			this.ConfigType.Location = new System.Drawing.Point(132, 10);
			this.ConfigType.Name = "ConfigType";
			this.ConfigType.Size = new System.Drawing.Size(121, 21);
			this.ConfigType.TabIndex = 4;
			this.ConfigType.SelectedIndexChanged += new System.EventHandler(this.Config_typeSelectedIndexChanged);
			// 
			// lblConfigType
			// 
			this.lblConfigType.Location = new System.Drawing.Point(13, 10);
			this.lblConfigType.Name = "lblConfigType";
			this.lblConfigType.Size = new System.Drawing.Size(100, 23);
			this.lblConfigType.TabIndex = 5;
			this.lblConfigType.Text = "Reader Type";
			this.lblConfigType.Click += new System.EventHandler(this.Label1Click);
			// 
			// ConfigPCSCDevice
			// 
			this.ConfigPCSCDevice.FormattingEnabled = true;
			this.ConfigPCSCDevice.Location = new System.Drawing.Point(132, 64);
			this.ConfigPCSCDevice.Name = "ConfigPCSCDevice";
			this.ConfigPCSCDevice.Size = new System.Drawing.Size(121, 21);
			this.ConfigPCSCDevice.TabIndex = 6;
			this.ConfigPCSCDevice.Visible = false;
			// 
			// lblConfigPCSCDevice
			// 
			this.lblConfigPCSCDevice.Location = new System.Drawing.Point(13, 64);
			this.lblConfigPCSCDevice.Name = "lblConfigPCSCDevice";
			this.lblConfigPCSCDevice.Size = new System.Drawing.Size(100, 23);
			this.lblConfigPCSCDevice.TabIndex = 7;
			this.lblConfigPCSCDevice.Text = "PCSC Device";
			this.lblConfigPCSCDevice.Click += new System.EventHandler(this.Label1Click);
			// 
			// frmSerial
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 168);
			this.Controls.Add(this.lblConfigPCSCDevice);
			this.Controls.Add(this.ConfigPCSCDevice);
			this.Controls.Add(this.lblConfigType);
			this.Controls.Add(this.ConfigType);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lblSerialPort);
			this.Controls.Add(this.ConfigSerial);
			this.Name = "frmSerial";
			this.Text = "Reader Configuration";
			this.Load += new System.EventHandler(this.FrmSerialLoad);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label lblConfigPCSCDevice;
		private System.Windows.Forms.ComboBox ConfigPCSCDevice;
		private System.Windows.Forms.Label lblConfigType;
		private System.Windows.Forms.ComboBox ConfigType;
		private System.Windows.Forms.ComboBox ConfigSerial;
		private System.Windows.Forms.Label lblSerialPort;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		
	
		
		void BtnOKClick(object sender, System.EventArgs e)
		{
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
			this.Close();
			this.Dispose();
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
			this.UpdateConfig();
		}
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
				
				/*
				foreach(string v in ACRRead.GetReaders())
				{
					this.ConfigPCSCDevice.Items.Add(v);
				}
				*/
				this.ConfigPCSCDevice.Show();
			}else{
				this.ConfigPCSCDevice.Hide();
			}		
			
		}
	}
}