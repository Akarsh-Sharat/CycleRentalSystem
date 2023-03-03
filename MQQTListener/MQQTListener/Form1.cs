using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Json;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MQQTListener
{
    public partial class Form1 : Form
    {
        MqttClient clientSendCommand = null;
        MqttClient clientLocationData = null;
        MqttClient clientDeviceData = null;
        List<String> processedrecord =new List<String>();
        string topicSendCommand = "COMMAND-RENT500001";
        string topicLocationData= "DEVICELOCATION-RENT500001";
        string topicDeviceData = "DEVICEDATA-RENT500001";
        public Form1()
        {
            
            InitializeComponent();
        }

        private void mqttClient()
        {
            try
            {
                //MessageBox.Show("Inside ");

                var broker = "a2voxmy6jorank-ats.iot.us-east-1.amazonaws.com"; //<AWS-IoT-Endpoint>           
                var port = 8883;
                var clientId = "RENT500001";
                var certPass = "Elutra123$";

                var certificatesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certs");

                var caCertPath = Path.Combine(certificatesPath, "RENT500001.cert.pem");
                var pfxPath = Path.Combine(certificatesPath, "rootCA.pfx");


                var caCert = X509Certificate.CreateFromCertFile(caCertPath);

                X509Certificate2 clientCert = new X509Certificate2(pfxPath, certPass);


                // Create a new MQTT client.
                clientSendCommand = new MqttClient(broker, port, true, caCert, clientCert, MqttSslProtocols.TLSv1_2);
                clientSendCommand.MqttMsgPublishReceived += Client_MqttSendCommandMsgPublishReceived;

                clientSendCommand.Connect(clientId);
                clientSendCommand.Subscribe(new string[] { topicSendCommand }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }

        }

        private void mqttClientLocationData()
        {
            try
            {
                //MessageBox.Show("Inside ");

                var broker = "a2voxmy6jorank-ats.iot.us-east-1.amazonaws.com"; //<AWS-IoT-Endpoint>           
                var port = 8883;
                var clientId = "RENT500001";
                var certPass = "Elutra123$";

                var certificatesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certs");

                var caCertPath = Path.Combine(certificatesPath, "RENT500001.cert.pem");
                var pfxPath = Path.Combine(certificatesPath, "rootCA.pfx");


                var caCert = X509Certificate.CreateFromCertFile(caCertPath);

                X509Certificate2 clientCert = new X509Certificate2(pfxPath, certPass);


                // Create a new MQTT client.
                clientLocationData = new MqttClient(broker, port, true, caCert, clientCert, MqttSslProtocols.TLSv1_2);
                clientLocationData.MqttMsgPublishReceived += Client_MqttLocationDataMsgPublishReceived;

                clientLocationData.Connect(clientId);
                clientLocationData.Subscribe(new string[] { topicLocationData }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }

        }

        private void mqttClientDeviceDataData()
        {
            try
            {
                //MessageBox.Show("Inside ");

                var broker = "a2voxmy6jorank-ats.iot.us-east-1.amazonaws.com"; //<AWS-IoT-Endpoint>           
                var port = 8883;
                var clientId = "RENT500001";
                var certPass = "Elutra123$";

                var certificatesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certs");

                var caCertPath = Path.Combine(certificatesPath, "RENT500001.cert.pem");
                var pfxPath = Path.Combine(certificatesPath, "rootCA.pfx");


                var caCert = X509Certificate.CreateFromCertFile(caCertPath);

                X509Certificate2 clientCert = new X509Certificate2(pfxPath, certPass);


                // Create a new MQTT client.
                clientDeviceData = new MqttClient(broker, port, true, caCert, clientCert, MqttSslProtocols.TLSv1_2);
                //clientDeviceData.MqttMsgPublishReceived += Client_MqttDeviceDataMsgPublishReceived;

                clientDeviceData.Connect(clientId);
                clientDeviceData.Subscribe(new string[] { topicDeviceData }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {



            
            mqttClient();
            mqttClientLocationData();
            //mqttClientDeviceDataData();
            /* System.Threading.Tasks.Task.Factory.StartNew(() =>
             {
                 Thread.Sleep(5000);
                 this.Invoke(new Action(() =>
                     sendData()));
             });*/



           /* Task task = new Task(() =>
            {
                while (true)
                {
                    sendCommandData();
                    Thread.Sleep(2000);
                }
            });
            task.Start();*/

        }


        private void sendCommandData()
        {
            String conStr = "Data Source=rentcycledb.chixyb1vnruq.ap-south-1.rds.amazonaws.com,1433;Initial Catalog=rentcyclewebappdb;User ID=admin;Password=Akarsh22;TrustServerCertificate=True";
            using (SqlConnection myConnection = new SqlConnection(conStr))
            {
                string oString = "select top 1 * from userrideinfo where locktime is null order by id desc";
                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        try
                        {
                            //MessageBox.Show("Inside ");

                            if (clientSendCommand == null || clientSendCommand.IsConnected==false)
                            {
                                var broker = "a2voxmy6jorank-ats.iot.us-east-1.amazonaws.com"; //<AWS-IoT-Endpoint>           
                                var port = 8883;
                                var clientId = "RENT500001";
                                var certPass = "Elutra123$";

                                var certificatesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certs");

                                var caCertPath = Path.Combine(certificatesPath, "RENT500001.cert.pem");
                                var pfxPath = Path.Combine(certificatesPath, "rootCA.pfx");


                                var caCert = X509Certificate.CreateFromCertFile(caCertPath);

                                X509Certificate2 clientCert = new X509Certificate2(pfxPath, certPass);


                                // Create a new MQTT client.
                                clientSendCommand = new MqttClient(broker, port, true, caCert, clientCert, MqttSslProtocols.TLSv1_2);
                                clientSendCommand.MqttMsgPublishReceived += Client_MqttSendCommandMsgPublishReceived;
                                clientSendCommand.Connect(clientId);
                                clientSendCommand.Subscribe(new string[] { topicSendCommand }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

                            }
                            if (processedrecord.Contains(oReader["Id"].ToString()))
                            {
                                return;
                            }

                            //MessageBox.Show("Client Connected");
                            
                            JsonObject jo = new JsonObject();
                            jo["command"] = "STARTRIDE";
                            jo["deviceID"] = oReader["DeviceID"].ToString();
                            jo["time"] = DateTime.Now.AddMinutes(330).ToString();
                            jo["accountid"] = oReader["UserAccountID"].ToString();

                            string startCommand = jo.ToString();//"STARTRIDE";//
                            clientSendCommand.Publish(topicSendCommand, Encoding.UTF8.GetBytes(startCommand));

                            processedrecord.Add(oReader["Id"].ToString());

                            
                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {

                        }
                    }

                    myConnection.Close();

                   
                }
            }

           

        }

        private static void Client_MqttSendCommandMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            try
            {
                // String nn = "b'{\"accountid\": \"22\", \"command\": \"STOPRIDE\", \"deviceID\": \"RENT500001\", \"time\": \"11-11-2022 19:10:57\"}'";
                //Console.WriteLine("Message received: " + Encoding.UTF8.GetString(e.Message));
                //string bb = Encoding.UTF8.GetString(e.Message);

                String bbbb = Encoding.UTF8.GetString(e.Message);
                //JsonObject json = (JsonObject)JsonObject.Parse(Encoding.UTF8.GetString(e.Message));

                //String jsonStr = json.ToString();

                //JsonObject DeviceId = (JsonObject)json["deviceID"];
                //JsonObject accountid = (JsonObject)json["accountid"];
                DateTime lockTime = DateTime.Now;
                DateTime ScanEndTime = DateTime.Now;
                String accountid = "22";
                String DeviceId = "RENT500001";

                if (bbbb.Contains("STOPRIDE"))
                {
                    String conStr = "Data Source=rentcycledb.chixyb1vnruq.ap-south-1.rds.amazonaws.com,1433;Initial Catalog=rentcyclewebappdb;User ID=admin;Password=Akarsh22;TrustServerCertificate=True";
                    using (SqlConnection myConnection = new SqlConnection(conStr))
                    {
                                 
                        string oString = "update userrideinfo set ScanendTime = " + lockTime + " and LockTime =" + ScanEndTime + " where useraccountid='"+ accountid + "' and DeviceId='"+ DeviceId + "' and locktime is null";
                        SqlCommand oCmd = new SqlCommand(oString, myConnection);
                        myConnection.Open();
                    }

                }

               
            }
            catch(Exception ex) { }
        }

        private static void Client_MqttLocationDataMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            try
            {
                if (e.Message == null)
                {
                    return;
                }
                //Console.WriteLine("Message received: " + Encoding.UTF8.GetString(e.Message));
                JsonObject json = (JsonObject)JsonObject.Parse(Encoding.UTF8.GetString(e.Message));

                String jsonStr = json.ToString();

                if (!jsonStr.Contains("STOPRIDE"))
                {
                    //JsonObject json = (JsonObject)JsonObject.Parse(bbbbbbbb);
                    string DeviceId = (string)json["cycleID"];
                    JsonObject jsonLocation = (JsonObject)json["location"];

                    string LocationInfo = jsonLocation.ToString();

                    LocationInfo = LocationInfo.Replace("{{", "{");
                    LocationInfo = LocationInfo.Replace("}}", "}");

                    String conStr = "Data Source=rentcycledb.chixyb1vnruq.ap-south-1.rds.amazonaws.com,1433;Initial Catalog=rentcyclewebappdb;User ID=admin;Password=Akarsh22;TrustServerCertificate=True";
                    using (SqlConnection myConnection = new SqlConnection(conStr))
                    {
                        DateTime lockTime = DateTime.Now;
                        DateTime ScanEndTime = DateTime.Now;

                        string oString = "update deviceshadow set location ='" + LocationInfo + "' where DeviceId = '" + DeviceId + "'";
                        SqlCommand oCmd = new SqlCommand(oString, myConnection);
                        myConnection.Open();
                        oCmd.ExecuteNonQuery();
                        myConnection.Close();

                    }
                }
                else
                {
                    //JsonObject json = (JsonObject)JsonObject.Parse(bbbbbbbb);
                    string DeviceId = (string)json["cycleID"];
                    JsonObject jsonLocation = (JsonObject)json["location"];

                    string LocationInfo = jsonLocation.ToString();

                    LocationInfo = LocationInfo.Replace("{{", "{");
                    LocationInfo = LocationInfo.Replace("}}", "}");

                    String conStr = "Data Source=rentcycledb.chixyb1vnruq.ap-south-1.rds.amazonaws.com,1433;Initial Catalog=rentcyclewebappdb;User ID=admin;Password=Akarsh22;TrustServerCertificate=True";
                    using (SqlConnection myConnection = new SqlConnection(conStr))
                    {
                        DateTime lockTime = DateTime.Now;
                        DateTime ScanEndTime = DateTime.Now;

                        string oString = "update deviceshadow set location ='" + LocationInfo + "' where DeviceId = '" + DeviceId + "'";
                        SqlCommand oCmd = new SqlCommand(oString, myConnection);
                        myConnection.Open();
                        oCmd.ExecuteNonQuery();
                        myConnection.Close();

                    }


                }
            }
            catch ( Exception ex)
            {

            }
        }
    }
}
