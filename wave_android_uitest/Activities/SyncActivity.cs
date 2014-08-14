
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace wave_android_uitest {
    [Activity(Label = "SyncActivity")]			
    public class SyncActivity : Activity 
    {
        private WaveAudioHandler audioHandler;

        private bool _start;
        private Button _btn;
        private string fileName;
        private TextView output;

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            audioHandler = new WaveAudioHandler(FileRead);

            // Create your application here
            SetContentView(Resource.Layout.sync);
            _start = true;

            _btn = FindViewById<Button>(Resource.Id.sync_button);
            _btn.Click += HandleClick;

            // back closes activity
            var txt = FindViewById<TextView>(Resource.Id.sync_back);
            txt.Click += (object sender, EventArgs e) => {
                Finish();
            };

            output = FindViewById<TextView>(Resource.Id.sync_output);

            output.Text = "Click START to begin transferring data";

            ActionBar.Hide();
        }

        protected override void OnDestroy() 
        {
            base.OnDestroy();
            audioHandler.CleanRecorder();
        }

        void FileRead()
        {
            Console.WriteLine("FileRead called");

            DecodeAudioStream();
        }

        void HandleClick (object sender, EventArgs e)
        {
            if (_start) {
                Start(sender, e);
                _btn.Text = "Stop";
            } else {
                Stop(sender, e);
                _btn.Text = "Start";
            }
            _start = !_start;
        }

        public void Start(object sender, EventArgs args) 
        {
            if(audioHandler.InitializeRecorder())
            {
                output.Text = "Reading data from device. This might take a moment, please wait...";
                fileName = audioHandler.StartRecording();
            }
            else
            {
                output.Text = "There was a problem reading from device. Please relaunch the product and try again.";
                Console.WriteLine("Could not set up recorder");
            }
        }

        public void Stop(object sender, EventArgs args) 
        {
            output.Text = "Finished reading data from device. Processing data. Thic might take a moment, please wait...";
            audioHandler.StopRecording();
        }

        private void AddOutput(string message)
        {
            output.Text = output.Text += message;
        }

        void DecodeAudioStream()
        {

            int [] outputBuffer = new int[9000];

            //Extract samples from captured file and return determine data logic values
            audioHandler.ConvertAudioStreamToIntByteArray (fileName, null, out outputBuffer);

            AddOutput("\\n\\n===============================\\n");
            AddOutput("Start decoding...\n");
            AddOutput("\\n\\n===============================\\n");
//            logTextView.Text = logTextView.Text + "\n\n===============================\n";
//            logTextView.Text = logTextView.Text + "Start decoding...\n";
//            logTextView.Text = logTextView.Text + "===============================\n\n";
            Console.WriteLine("\n\n===============================\n");
            Console.WriteLine("Start decoding...");
            Console.WriteLine("\n\n===============================\n");

            int numberofbits = 0;
            int numberofpackets = 0; 
            int number = 0;
            int ulcommand = 0;
            int packetcount = 0;

            // Need to count packets before we process packets - there cant be more then 17 packets 
            // but at the beginning there will be less until 8 challenges have started and stopped.
            int counts = 0;

            while (counts < 17 )
            {
                ulcommand = 0;
                number = 0;

                while (number < 8)
                {
                    //left shift
                    ulcommand = ulcommand << 1;

                    //Add in new bit
                    ulcommand = ulcommand + outputBuffer[number + numberofbits];

                    number++;
                }

                if ( ulcommand == 209 )
                {
                    packetcount++;
                    numberofbits = numberofbits + 112;
                }

                if ( ulcommand == 210 )
                {
                    packetcount++;
                    numberofbits = numberofbits + 120;
                }

                if (ulcommand == 0)
                {
                    numberofpackets = packetcount;
                }

                counts++;

                Console.WriteLine("ulcommand :: {0} == Number of bits {1}", ulcommand, numberofbits);
                //logTextView.Text = logTextView.Text + "ulcommand : " + ulcommand + " == Number of bits : " + numberofbits + "\n";
                AddOutput("ulcommand : " + ulcommand + " == Number of bits : " + numberofbits + "\n");
            }

            numberofpackets = packetcount;
            Console.WriteLine("Number of thedatabits : {0}", outputBuffer.Length);
            Console.WriteLine("Numberofpackets : {0}", numberofpackets);

//            logTextView.Text = logTextView.Text + "Number of Packets : " + numberofpackets + "   Number of Bits : " + numberofbits + "\n";
            AddOutput("Number of Packets : " + numberofpackets + "   Number of Bits : " + numberofbits + "\n");

            packetcount = 0;
            ulcommand = 0;
            numberofbits = 0;

            counts = 0;
            while (packetcount < numberofpackets)
                //while (counts < 17)
            {
                number = 0;
                int timestamp = 0;
                int numberofsteps = 0;
                int caloriesburned = 0;
                int adjustedcalories = 0;
                int checksum = 0;
                int weeks = 0;
                int days = 0;
                int hours = 0;
                int minutes = 0;
                int seconds = 0;
                int tempmod1 = 0;
                int tempmod2 = 0;
                int verifychecksum = 0;
                int packetindex = 0;
                //int count = 0;
                ulcommand = 0;

                // Start looking for packets
                while (number < 8)
                {
                    // First byte is command
                    //Shift ulcommand left
                    ulcommand = ulcommand + ulcommand;
                    //Add in new bit
                    ulcommand = ulcommand + outputBuffer[number + numberofbits];
                    number++;
                    //numberofbits++;
                }
                Console.WriteLine("ulcommand {0}", ulcommand);

                // There are two types of packets - the first byte differentiates
                // Look for either of two commands:
                // 0xd1 = 11010001 = 128 + 64 + 0 + 16 + 0 + 0 + 0 + 1 = 209
                // 0xd2 = 11010010 = 128 + 64 + 0 + 16 + 0 + 0 + 1 + 0 = 210
                number = 0;

                if (ulcommand == 209)
                    //if (counts == 0) 
                {
//                    logTextView.Text = logTextView.Text + "\nIndex Packet : " + ulcommand + "\n";
                    AddOutput("\nIndex Packet : " + ulcommand + "\n");

                    // total sports information -- 14 bytes or 112 bits
                    while (number < 32)
                    {
                        //
                        //Shift timestamp left
                        timestamp = timestamp + timestamp;
                        //Add in new bit
                        timestamp = timestamp + outputBuffer[number + numberofbits + 8]; 
                        //Shift numberofsteps left
                        numberofsteps = numberofsteps + numberofsteps;
                        //Add in new bit
                        numberofsteps = numberofsteps + outputBuffer[number + numberofbits + 40];
                        //Shift caloriesburned left
                        caloriesburned = caloriesburned + caloriesburned;
                        //Add in new bit
                        caloriesburned = caloriesburned + outputBuffer[number + numberofbits + 72];
                        // Last byte is checksum
                        //Shift checksum left
                        checksum = checksum + checksum;
                        //Add in new bit
                        checksum = checksum + outputBuffer[number + numberofbits + 80];
                        // Grab first bytes start calculating checksum
                        number++;
                    }

                    // 86400 = seconds / day
                    //  3600 = seconds / hour
                    //  1440 = minutes / day
                    days = timestamp / 86400;
                    weeks = days / 7;
                    tempmod1 = timestamp % 86400;
                    hours = tempmod1 / 3600;
                    tempmod2 = tempmod1 % 3600;
                    minutes = tempmod2 / 60;
                    seconds = tempmod2 % 60;

                    // Calories Firmwear is being corrected - this is a temporary fix 
                    adjustedcalories = caloriesburned;
                    Console.WriteLine ( "{0} Weeks, {1} Days, {2} Hours, {3} Mins, {4} Secs, {5} Steps, {6} Cals", weeks, days, hours, minutes, seconds, numberofsteps, adjustedcalories);

                    verifychecksum = (byte) ulcommand + (byte) timestamp + (byte) (timestamp >> 8) + (byte) (timestamp >> 16) + (byte) (timestamp >> 24) 
                        + (byte) numberofsteps + (byte) (numberofsteps >> 8) + (byte) (numberofsteps >> 16) + (byte) (numberofsteps >> 24) 
                        + (byte) caloriesburned + (byte) (caloriesburned >> 8) + (byte) (caloriesburned >> 16) + (byte) (caloriesburned >> 24); 

                    Console.WriteLine("{0:X8} {1:X8} {2:X8} {3:X8} CS: {4:X2} VC:{5:X2}", ulcommand, timestamp, numberofsteps, adjustedcalories, (byte) checksum, (byte) verifychecksum);

                    string timeStampString = string.Format ( "{0} Weeks, {1} Days, {2} Hours, {3} Mins, {4} Secs", weeks, days, hours, minutes, seconds);
                    Console.WriteLine ("=====================> Timestamp :: " + timeStampString);
//                    logTextView.Text = logTextView.Text + "==> TS : " + timeStampString + "\n";
                    AddOutput("==> TS : " + timeStampString + "\n");

                    string statsString = string.Format ( "Stepcount: {0}, Calories: {1} ", numberofsteps, adjustedcalories);
                    Console.WriteLine ("=====================> Stats :: " + statsString);
//                    logTextView.Text = logTextView.Text + "==> STATS : " + statsString + "\n";
                    AddOutput("==> STATS : " + statsString + "\n");

                    string checksumString = string.Format( "{0:X2} {1:X8} {2:X8} {3:X8} CS:{4:X2} Fail:{5:X2}", (byte) ulcommand, timestamp, numberofsteps, adjustedcalories, (byte) checksum, (byte) verifychecksum);

                    if ( (byte) checksum == (byte) verifychecksum)
                    { 
                        checksumString = string.Format( "{0:X2} {1:X8} {2:X8} {3:X8} CS:{4:X2} Pass:{5:X2}", (byte) ulcommand, timestamp, numberofsteps, adjustedcalories, (byte) checksum, (byte) verifychecksum);
                    }

//                    logTextView.Text = logTextView.Text + "==> " + checksumString + "\n\n";
                    AddOutput("==> " + checksumString + "\n\n");

                    numberofbits = numberofbits + 112;
                    packetcount++;
                }
                else
                {
                    timestamp = 0;
                    numberofsteps = 0;
                    packetindex = 0;
                    numberofsteps = 0;
                    caloriesburned = 0;
                    checksum = 0;
                    number = 0;
                    //
                    if (ulcommand == 210) 
                        //if (counts > 0) 
                    {
//                        logTextView.Text = logTextView.Text + "\nChallenge Packet : " + ulcommand + "\n";
                        AddOutput("\nChallenge Packet : " + ulcommand + "\n");

                        // total sports challenge information -- 15 bytes or 120 bits
                        while (number < 32)
                        {
                            // Get packet index
                            if (number < 8)
                            {
                                packetindex = packetindex + packetindex;
                                packetindex = packetindex + outputBuffer[number + numberofbits + 8];
                            }
                            //Shift timestamp left
                            timestamp = timestamp + timestamp;
                            //Add in new bit
                            timestamp = timestamp + outputBuffer[number + numberofbits + 16];
                            //Shift numberofsteps left
                            numberofsteps = numberofsteps + numberofsteps;
                            //Add in new bit
                            numberofsteps = numberofsteps + outputBuffer[number + numberofbits + 48];
                            //Shift caloriesburned left
                            caloriesburned = caloriesburned + caloriesburned;
                            //Add in new bit
                            caloriesburned = caloriesburned + outputBuffer[number + numberofbits + 80]; 
                            checksum = checksum + checksum;
                            //Add in new bit
                            checksum = checksum + outputBuffer[number + numberofbits + 88];
                            number++;
                        }

                        days = timestamp / 86400;
                        weeks = days / 7;
                        tempmod1 = timestamp % 86400;
                        hours = tempmod1 / 3600;
                        tempmod2 = tempmod1 % 3600;
                        minutes = tempmod2 / 60;
                        seconds = tempmod2 % 60;

                        // Calories Firmwear is being corrected - this is a temporary fix 
                        adjustedcalories = caloriesburned;

                        verifychecksum = (byte) ulcommand + (byte) timestamp + (byte) (timestamp >> 8) + (byte) (timestamp >> 16) + (byte) (timestamp >> 24) 
                            + (byte) numberofsteps + (byte) (numberofsteps >> 8) + (byte) (numberofsteps >> 16) + (byte) (numberofsteps >> 24) 
                            + (byte) caloriesburned + (byte) (caloriesburned >> 8) + (byte) (caloriesburned >> 16) + (byte) (caloriesburned >> 24)
                            + (byte) packetindex; 

                        Console.WriteLine ( "{0} Weeks {1} Days {2} Hours {3} Minutes {4} Seconds {5} Steps {6} Calories", weeks, days,hours, minutes, seconds, numberofsteps, adjustedcalories);
                        Console.WriteLine("Index: {0:X2} TS: {1:X8} Steps: {2:X8} Calories: {3:X8} Cksum {4:X2} VC: {5:X2}", packetindex, timestamp, numberofsteps, adjustedcalories, (byte) checksum, (byte) verifychecksum);

                        string statString2 = string.Format("{0:X2} {1:X8} {2:X8} {3:X8} CS:{4:X2} Fail:{5:X2}", packetindex, timestamp, numberofsteps, adjustedcalories, (byte) checksum, (byte) verifychecksum);
                        string statString1 = string.Format( "W:{0} D:{1} H:{2} M:{3} S:{4} Steps: {5} Cals: {6}", weeks, days,hours, minutes, seconds, numberofsteps, adjustedcalories);

                        if ( (byte) checksum == (byte) verifychecksum)
                        { 
                            statString1 = string.Format( "W:{0} D:{1} H:{2} M:{3} S:{4} Steps:{5} Cals:{6}", weeks, days,hours, minutes, seconds, numberofsteps, adjustedcalories);
                            statString2 = string.Format("{0:X2} {1:X8} {2:X8} {3:X8} CS:{4:X2} Pass:{5:X2}", packetindex, timestamp, numberofsteps, adjustedcalories, (byte) checksum, (byte) verifychecksum);
                        }

                        Console.WriteLine ("=====================> Stat String Format1 :: " + statString1);
                        Console.WriteLine ("=====================> Stat String Format2 :: " + statString2);

//                        logTextView.Text = logTextView.Text + "=> Stat : " + statString1 + "\n";
//                        logTextView.Text = logTextView.Text + "=> Stat : " + statString2 + "\n";
                        AddOutput("=> Stat : " + statString1 + "\n");
                        AddOutput("=> Stat : " + statString2 + "\n");

                        numberofbits = numberofbits + 120;
                        packetcount++;
                    }
                }
                counts++;
            }
//            File.Delete (fileName);
        }
    }

}

