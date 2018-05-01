using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private Dictionary<int, Boolean> timerIsOn = new Dictionary<int, Boolean>();
        private Dictionary<int, string> deviceName = new Dictionary<int, string>();
        private Dictionary<int, string> timingPoint = new Dictionary<int, string>();
        private Dictionary<int, GroupBox> groupBoxDict = new Dictionary<int, GroupBox>();
        //private Dictionary<int, Thread> threadDict = new Dictionary<int, Thread>();
        private Dictionary<int, Dictionary<string, Tag>> tagMasterList = new Dictionary<int, Dictionary<string, Tag>>();
        private int yLocation = 156;
        private int timerCount = 1;

        private string raceName;
        private int raceId;
        private List<Device> deviceList = new List<Device>();

        //public string server = "localhost";
        public string server = "localhost\\SQLEXPRESS";

        public Form1()
        {
            InitializeComponent();
            getRaceInfo();
            //getDeviceNames();
            getCurrentIPAddress();

            // init socketServer
            Thread t = new Thread(startSocketServer);
            t.Start();
        }

        private void startSocketServer()
        {
            Console.WriteLine("startSocketServer");
            var socketServer = new socketServer(this);
            socketServer.Start();
        }

        private void addTimerBoxGroup()
        {
            var grp = new GroupBox();
            var checkBox = new CheckBox();
            var textBox1 = new TextBox();
            //var comboBox1 = new ComboBox();
            var comboBox2 = new ComboBox();
            var table = new TableLayoutPanel();
            var removeButton = new Button();
            //var dateTimePicker = new DateTimePicker();
            //var label1 = new Label();
            var label2 = new Label();
            var label3 = new Label();
            var label4 = new Label();

            // grp
            grp.AutoSize = true;
            grp.Location = new Point(12, yLocation);
            grp.Name = "groupBox" + timerCount.ToString();
            grp.Padding = new System.Windows.Forms.Padding(0);
            grp.Size = new System.Drawing.Size(922, 65);
            grp.TabIndex = 1;
            grp.TabStop = false;
            grp.Text = "Timer " + timerCount.ToString();
            groupBoxDict[timerCount] = grp;

            // removeButton
            removeButton.Location = new System.Drawing.Point(805, 23);
            removeButton.Name = "removeButton" + timerCount.ToString();
            removeButton.Size = new System.Drawing.Size(101, 23);
            removeButton.TabIndex = 2;
            removeButton.Text = "Remove Timer";
            removeButton.UseVisualStyleBackColor = true;
            removeButton.Click += new System.EventHandler(this.removeTimerButton_Click);

            // checkBox
            checkBox.AutoSize = true;
            checkBox.Location = new System.Drawing.Point(13, 29);
            checkBox.Name = "checkBox" + timerCount.ToString();
            checkBox.Size = new System.Drawing.Size(57, 16);
            checkBox.TabIndex = 3;
            checkBox.Text = "On/Off";
            checkBox.UseVisualStyleBackColor = true;
            checkBox.Enabled = false;
            checkBox.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);

            // textBox1
            textBox1.Location = new System.Drawing.Point(102, 25);
            textBox1.Name = "comboBox" + timerCount.ToString();
            textBox1.Size = new System.Drawing.Size(108, 20);
            textBox1.TabIndex = 4;
            textBox1.Text = "Enter device";
            textBox1.ForeColor = Color.LightGray;
            textBox1.GotFocus += new System.EventHandler(RemoveText);
            textBox1.LostFocus += new System.EventHandler(AddText);
            textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);

            // comboBox1
            //comboBox1.FormattingEnabled = true;
            //comboBox1.Location = new System.Drawing.Point(102, 25);
            //comboBox1.Name = "comboBox" + timerCount.ToString();
            //comboBox1.Size = new System.Drawing.Size(108, 20);
            //comboBox1.TabIndex = 4;
            //comboBox1.Items.Add("Select device");
            //foreach (Device device in deviceList)
            //{
            //    comboBox1.Items.Add(device.name.ToString());
            //}
            //comboBox1.SelectedIndex = 0;
            //comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);

            // comboBox2
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new System.Drawing.Point(224, 25);
            comboBox2.Name = "comboBox" + timerCount.ToString();
            comboBox2.Size = new System.Drawing.Size(108, 20);
            comboBox2.TabIndex = 5;
            comboBox2.Items.Add("Select timing point");
            List<string> TimerNames = getTimePoints();
            foreach (String TimerName in TimerNames)
            {
                comboBox2.Items.Add(TimerName);
            }
            //comboBox2.Items.Add("Start");
            // comboBox2.Items.Add("TP0");
            //comboBox2.Items.Add("TP1");
            //comboBox2.Items.Add("TP2");
            //comboBox2.Items.Add("TP3");
            //comboBox2.Items.Add("Finish");
            //comboBox2.Items.Add("TP0 Bup");
            //comboBox2.Items.Add("TP1 Bup");
            //comboBox2.Items.Add("TP2 Bup");
            //comboBox2.Items.Add("TP3 Bup");
            //comboBox2.Items.Add("Finish Bup");
            comboBox2.SelectedIndex = 0;
            comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);

            // dateTimePicker
            //dateTimePicker.Anchor = System.Windows.Forms.AnchorStyles.None;
            //dateTimePicker.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            //dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            //dateTimePicker.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            //dateTimePicker.Location = new System.Drawing.Point(60, 5);
            //dateTimePicker.Name = "dateTimePicker" + timerCount.ToString();
            //dateTimePicker.RightToLeft = System.Windows.Forms.RightToLeft.No;
            //dateTimePicker.Size = new System.Drawing.Size(200, 22);
            //dateTimePicker.TabIndex = 6;
            //dateTimePicker.Value = DateTime.Now.Date;
            //datePollFrom[timerCount] = dateTimePicker.Value;
            //dateTimePicker.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);

            //// label1
            //label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            //label1.AutoSize = true;
            //label1.Location = new System.Drawing.Point(4, 10);
            //label1.Name = "label" + timerCount.ToString() + "_1";
            //label1.Size = new System.Drawing.Size(49, 12);
            //label1.TabIndex = 7;
            //label1.Text = "Poll from";
            //label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // label2
            label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(267, 10);
            label2.Name = "label" + timerCount.ToString() + "_2";
            label2.Size = new System.Drawing.Size(27, 12);
            label2.TabIndex = 8;
            label2.Text = "Tags";

            // label3
            label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(310, 10);
            label3.Name = "label" + timerCount.ToString() + "_3";
            label3.Size = new System.Drawing.Size(11, 12);
            label3.TabIndex = 9;
            label3.Text = "0";

            // label4
            label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(361, 10);
            label4.Name = "label" + timerCount.ToString() + "_4";
            label4.Size = new System.Drawing.Size(43, 12);
            label4.TabIndex = 10;
            label4.Text = "Stopped";

            // table
            table.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            table.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            table.ColumnCount = 3;
            table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            //table.Controls.Add(dateTimePicker, 1, 0);
            //table.Controls.Add(label1, 0, 0);
            table.Controls.Add(label2, 0, 0);
            table.Controls.Add(label3, 1, 0);
            table.Controls.Add(label4, 2, 0);
            table.Location = new System.Drawing.Point(352, 19);
            table.MaximumSize = new System.Drawing.Size(430, 32);
            table.Name = "tableLayoutPanel" + timerCount.ToString();
            table.RowCount = 1;
            table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            table.Size = new System.Drawing.Size(430, 32);
            table.TabIndex = 11;
            table.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);

            // Add to form
            this.Controls.Add(grp);
            grp.Controls.Add(removeButton);
            grp.Controls.Add(checkBox);
            grp.Controls.Add(textBox1);
            grp.Controls.Add(comboBox2);
            grp.Controls.Add(table);
            timerCount++;
            yLocation += 80;
        }


        private void addTimerButton_Click(object sender, EventArgs e)
        {
            addTimerBoxGroup();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("HH:mm:ss.f");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void removeTimerButton_Click(object sender, EventArgs e)
        {
            Button removeButton = sender as Button;
            GroupBox grp = removeButton.Parent as GroupBox;
            int key = groupBoxDict.FirstOrDefault(x => x.Value.Equals(grp)).Key;

            // Abort thread of that timer
            //if (threadDict.ContainsKey(key))
            //{
            //    threadDict[key].Abort();
            //}

            // Remove groupBox & release memory
            grp.Parent.Controls.Remove(grp);
            grp.Dispose();

            for (int i = key; i < groupBoxDict.Count; i++)
            {
                var item = groupBoxDict.ElementAt(i);
                var groupBox = item.Value;
                groupBox.Location = new Point(groupBox.Location.X, groupBox.Location.Y - 80);
            }
            yLocation -= 80;
        }

        //private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        //{
        //    DateTimePicker dateTimePicker = sender as DateTimePicker;
        //    int key = groupBoxDict.FirstOrDefault(x => x.Value.Equals(dateTimePicker.Parent.Parent)).Key;
        //    datePollFrom[key] = dateTimePicker.Value;
        //}

        private void RemoveText(object sender, EventArgs e)
        {
            TextBox textBox1 = sender as TextBox;
            if (textBox1.Text == "Enter device")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void AddText(object sender, EventArgs e)
        {
            TextBox textBox1 = sender as TextBox;
            if (String.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Enter device";
                textBox1.ForeColor = Color.LightGray;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox1 = sender as TextBox;
            int key = groupBoxDict.FirstOrDefault(x => x.Value.Equals(textBox1.Parent)).Key;
            deviceName[key] = textBox1.Text;
            changeCheckBoxStatus(key, textBox1);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int key = groupBoxDict.FirstOrDefault(x => x.Value.Equals(comboBox.Parent)).Key;
            deviceName[key] = comboBox.Text;
            changeCheckBoxStatus(key, comboBox);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int key = groupBoxDict.FirstOrDefault(x => x.Value.Equals(comboBox.Parent)).Key;
            timingPoint[key] = comboBox.Text;
            changeCheckBoxStatus(key, comboBox);
        }

        private void changeCheckBoxStatus(int key, Control Object)
        {


            if (deviceName.ContainsKey(key) && timingPoint.ContainsKey(key) && deviceName[key] != "Enter device" && timingPoint[key] != "Select timing point")
            {
                Object.Parent.Controls[1].Enabled = true;
            }
            else
            {
                Object.Parent.Controls[1].Enabled = false;
            }
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            int key = groupBoxDict.FirstOrDefault(x => x.Value.Equals(checkBox.Parent)).Key;

            if (checkBox.Checked == true)
            {
                timerIsOn[key] = true;
                // Change text to "Polling..."
                Label status = groupBoxDict[key].Controls[4].Controls[2] as Label;
                status.Text = "Started";

                // TODO: startreceiving
            }
            else
            {
                timerIsOn[key] = false;
                //threadDict[key].Abort();
                Label status = groupBoxDict[key].Controls[4].Controls[2] as Label;
                status.Text = "Stopped";
            }
        }

        public int ReceiveData(Tag tag, String ckpt_name)
        {
            int key = -1;
            //Console.WriteLine(tag);

            foreach (KeyValuePair<int, String> item in deviceName)
            {
                if (item.Value == ckpt_name)
                {
                    key = item.Key;
                    break;
                }
            }

            if (key >= 0)
            {
                CheckBox on_off = groupBoxDict[key].Controls[1] as CheckBox;
                if (on_off.Checked)
                {
                    //Label status = groupBoxDict[key].Controls[4].Controls[2] as Label;
                    //status.Invoke((MethodInvoker)(() => status.Text = "Receiving"));

                    return key;
                }
                else
                {
                    return -1;
                }
            }
            return -1;
        }

        //private void pullDataTimer(int key)
        //{
        //    while (timerIsOn[key] == true)
        //    {
        //        pullDataFromServer(key);
        //        Thread.Sleep(5000);
        //    }
        //    Label status = groupBoxDict[key].Controls[4].Controls[4] as Label;
        //    status.Invoke((MethodInvoker)(() => status.Text = "Stopped"));
        //}

        public void EpcMapping(int key, Tag tag)
        {
            // Add 1 to tag no.
            Label tagNo = groupBoxDict[key].Controls[4].Controls[1] as Label;
            int x = 0;
            Int32.TryParse(tagNo.Text, out x);
            string newString = (x + 1).ToString();
            tagNo.Invoke((MethodInvoker)(() => tagNo.Text = newString));

            if (tag.bib_no)
            {
                Tag tempTag = convertBibNoToChipNo(tag);
                tag.epc = tempTag.epc;
                tag.bib_no = tempTag.bib_no;
            }
            else
            {
                String epc_with_dashes = tag.epc;
                for (int i = 4; i < epc_with_dashes.Length; i += 4)
                {
                    epc_with_dashes = epc_with_dashes.Insert(i, "-");
                    i++;
                }
                tag.epc = epc_with_dashes;
            }

            if (tagMasterList.ContainsKey(key))
            {
                if (!tagMasterList[key].ContainsKey(tag.id))
                {
                    tagMasterList[key][tag.id] = tag;

                    // Insert into database
                    insertIntoSqlServer(key, tag);
                }
            }
            else
            {
                Dictionary<string, Tag> tagList = new Dictionary<string, Tag>();
                tagList[tag.id] = tag;
                tagMasterList[key] = tagList;

                // Insert into database
                insertIntoSqlServer(key, tag);
            }
        }

        private void insertIntoSqlServer(int key, Tag tag)
        {
            using (SqlConnection connection = new SqlConnection("Server=" + server + "; Database= RaceTec; Integrated Security = SSPI; "))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    string datetimeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT into ChipTime (RaceId, TimingPoint, ChipCode, ChipTime, Reader, CreateDate, ReplStatus, IsChipTime, CanUse) VALUES (@RaceId, @TimingPoint, @ChipCode, @ChipTime, @Reader, @CreateDate, @ReplStatus, @IsChipTime, @CanUse)";
                    command.Parameters.AddWithValue("@RaceId", raceId);
                    command.Parameters.AddWithValue("@TimingPoint", timingPoint[key]);
                    command.Parameters.AddWithValue("@ChipCode", tag.epc);
                    command.Parameters.AddWithValue("@ChipTime", tag.datetime);
                    command.Parameters.AddWithValue("@Reader", deviceName[key]);
                    command.Parameters.AddWithValue("@CreateDate", datetimeNow);
                    command.Parameters.AddWithValue("@ReplStatus", 0);
                    command.Parameters.AddWithValue("@IsChipTime", 1);
                    command.Parameters.AddWithValue("@CanUse", 1);

                    try
                    {
                        connection.Open();
                        int recordsAffected = command.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                        // error here
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private Tag convertBibNoToChipNo(Tag tag)
        {
            string chipCode = null;
            char last = tag.epc[tag.epc.Length - 1];
            switch (last)
            {
                case 'A':
                    tag.epc = "10" + tag.epc.Replace("A", "");
                    break;
                case 'B':
                    tag.epc = "20" + tag.epc.Replace("B", "");
                    break;
                case 'C':
                    tag.epc = "30" + tag.epc.Replace("C", "");
                    break;
                case 'D':
                    tag.epc = "40" + tag.epc.Replace("D", "");
                    break;
                default:
                    break;
            }
            string raceNo = tag.epc;
            using (SqlConnection connection = new SqlConnection("Server=" + server + "; Database= RaceTec; Integrated Security = SSPI; "))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT ChipCode FROM EventChip WHERE RaceId = @RaceId AND RaceNo = @RaceNo";
                    command.Parameters.AddWithValue("@RaceId", raceId);
                    command.Parameters.AddWithValue("@RaceNo", raceNo);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            chipCode = reader.GetString(0);
                        }
                    }
                    catch (SqlException)
                    {
                        Console.WriteLine("Error");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            if (chipCode != null && chipCode != String.Empty)
            {
                Console.WriteLine(chipCode);
                tag.epc = chipCode;
                tag.bib_no = false;
            }
            return tag;
        }

        private void getRaceInfo()
        {
            using (SqlConnection connection = new SqlConnection("Server=" + server + "; Database= RTSys; Integrated Security = SSPI; "))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT Race.RaceId, Race.RaceName FROM Race INNER JOIN Config ON Race.RaceId = Config.FieldValue WHERE SettingName = 'DefaultWebRace' ";

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            raceId = reader.GetInt16(0);
                            raceName = reader.GetString(1);
                        }
                    }
                    catch (SqlException)
                    {
                        Console.WriteLine("Error");
                    }
                    finally
                    {
                        label4.Text = raceName;
                        label5.Text = raceId.ToString();
                        connection.Close();
                    }
                }
            }
        }

        //private void getDeviceNames()
        //{
        //    // Send get request to server
        //    string responseText = string.Empty;
        //    string url = @"http://dev.racetimingsolutions.com/rfid-gun/get-device-names";

        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //    request.AutomaticDecompression = DecompressionMethods.GZip;

        //    // Get response
        //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //    using (Stream stream = response.GetResponseStream())
        //    using (StreamReader reader = new StreamReader(stream))
        //    {
        //        responseText = reader.ReadToEnd();
        //    }

        //    deviceList = JsonConvert.DeserializeObject<List<Device>>(responseText);
        //    Console.WriteLine(deviceList.Count);
        //}

        public void getCurrentIPAddress()
        {
            string HostName = Dns.GetHostName();
            Console.WriteLine("Host Name of machine =" + HostName);
            IPAddress[] ipaddress = Dns.GetHostAddresses(HostName);
            Console.Write("IPv4 of Machine is ");
            String ip_str = "";
            foreach (IPAddress ip4 in ipaddress.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
            {
                ip_str = ip4.ToString();
            }
            IPLabelText.Text = ("IP Address: " + ip_str);
            Console.WriteLine("Your current IP Address is : " + ip_str);
        }

        private List<string> getTimePoints()
        {
            using (SqlConnection connection = new SqlConnection("Server=" + server + "; Database= RaceTec; Integrated Security = SSPI; "))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT TimerName FROM EventSplit WHERE RaceId = @RaceId ";
                    command.Parameters.AddWithValue("@RaceId", raceId);
                    List<string> timerNames = new List<string>();

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string timerName = reader.GetString(0);
                            Console.WriteLine(timerName);
                            timerNames.Add(timerName);
                        }
                    }
                    catch (SqlException)
                    {
                        Console.WriteLine("Error");
                    }

                    connection.Close();
                    return timerNames;
                }
            }
        }
    }
}
