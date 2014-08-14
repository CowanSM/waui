using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Text;

using System.Runtime.InteropServices;

//Vedad - new imports
using Android.Media;
using Java.Lang;
using Android.OS;

namespace wave_android_uitest
{
	public class WaveAudioHandler
	{
        public static WaveAudioHandler onlyInstance;

        //Vedad - our recorder
        AudioRecord recorder;
        System.Action fileReadComplete;

        int sampleRate;
        ChannelIn channels;
        Android.Media.Encoding outputFormat;
        int bufferSize;

        string filePath;

        public WaveAudioHandler (System.Action fileReadCompleteCallback)
        {
            onlyInstance = this;

            fileReadComplete = fileReadCompleteCallback;
		}

        public class BufferWrite : Java.Util.TimerTask 
        {
            public BufferWrite()
            {
            }
            public override void Run ()
            {
                byte[] buffer = new byte[onlyInstance.bufferSize];

                List<byte> bufferList = new List<byte>();

                if(File.Exists(onlyInstance.filePath))
                    File.Delete(onlyInstance.filePath);

                Java.IO.FileOutputStream fos = new Java.IO.FileOutputStream(onlyInstance.filePath, true);
                             
                int bitSize = 16;                   

                //Vedad - byte rate is how much data is processed in a second. 
                int byteRate = (onlyInstance.sampleRate * bitSize * (int)onlyInstance.channels);

                //Vedad - write our wav header
                byte[] header = new byte[44];
                header[0] = (byte)'R';                                            //Vedad - 4 bytes are constant
                header[1] = (byte)'I';
                header[2] = (byte)'F';
                header[3] = (byte)'F';
                //Vedad - write this later
//                header[4] = (byte) (totalDataLen & 0xff);                 //Vedad 4 bytes are size of the file
//                header[5] = (byte) ((totalDataLen >> 8) & 0xff);
//                header[6] = (byte) ((totalDataLen >> 16) & 0xff);
//                header[7] = (byte) ((totalDataLen >> 24) & 0xff);
                header[8] = (byte)'W';                                            //Vedad - 4 bytes are constant
                header[9] = (byte)'A';
                header[10] = (byte)'V';
                header[11] = (byte)'E';
                header[12] = (byte)'f';                                           //Vedad - 4 bytes format section, trailing is null in this case
                header[13] = (byte)'m';
                header[14] = (byte)'t';
                header[15] = (byte)' ';   
                header[16] = 16;                                            //Vedad - Next 4 bytes represent length of the format data
                header[17] = 0;
                header[18] = 0;
                header[19] = 0;
                header[20] = 1;                                             //Vedad - 2 byte integer of the format type. We are using PCM(Pulse Code Modulation), represented by a 1
                header[21] = 0;
                header[22] = (byte)((int)onlyInstance.channels);                            //Vedad - 2 bytes for channles(mono, stereo)
                header[23] = 0;
                header[24] = (byte) (onlyInstance.sampleRate & 0xff);                    //Vedad - 24-27 4 bytes for sample rate. 24 gets the least significant 8 bits of the first byte of data
                header[25] = (byte) ((onlyInstance.sampleRate >> 8) & 0xff);             //Vedad - shift 8 bits to the right, grabs least significant 8 bits of this byte
                header[26] = (byte) ((onlyInstance.sampleRate >> 16) & 0xff);            //Vedad - shift 8 bits to the right, grabs least significant 8 bits of this byte
                header[27] = (byte) ((onlyInstance.sampleRate >> 24) & 0xff);            //Vedad - shift 8 bits to the right, grabs least significant 8 bits of this byte
                header[28] = (byte) (byteRate & 0xff);                      //Vedad - 28-31 4 bytes for byte rate
                header[29] = (byte) ((byteRate >> 8) & 0xff);
                header[30] = (byte) ((byteRate >> 16) & 0xff);
                header[31] = (byte) ((byteRate >> 24) & 0xff);
                header[32] = (byte) ((bitSize * ((int)onlyInstance.channels)) & 0xff);       //Vedad - block align, 32-33 byte size for one sample
                header[33] = (byte) (((bitSize * ((int)onlyInstance.channels)) >> 8) & 0xff);
                header[34] = (byte)bitSize;                                       //Vedad - 2 bytes, bits per sample
                header[35] = 0;
                header[36] = (byte)'d';                                           //Vedad - 4 bytes for 
                header[37] = (byte)'a';
                header[38] = (byte)'t';
                header[39] = (byte)'a';
                //Vedad - write this later
//                header[40] = (byte) (totalAudioLen & 0xff);
//                header[41] = (byte) ((totalAudioLen >> 8) & 0xff);
//                header[42] = (byte) ((totalAudioLen >> 16) & 0xff);
//                header[43] = (byte) ((totalAudioLen >> 24) & 0xff);
                //Vedad - write this later
//                fos.Write(header, 0, 44);

                int result;
                int totalBytes = 0;

                onlyInstance.recorder.StartRecording();

                while(onlyInstance.recorder.RecordingState == RecordState.Recording)
                {
                    result = onlyInstance.recorder.Read(buffer, 0, onlyInstance.bufferSize);

                    foreach(byte item in buffer)
                    {
                        bufferList.Add(item);
                    }

                    totalBytes += result;
                }

                header[4] = (byte) ((36 + totalBytes) & 0xff);                 //Vedad 4 bytes are size of the file
                header[5] = (byte) (((36 + totalBytes) >> 8) & 0xff);
                header[6] = (byte) (((36 + totalBytes) >> 16) & 0xff);
                header[7] = (byte) (((36 + totalBytes) >> 24) & 0xff);

                header[40] = (byte) (totalBytes & 0xff);
                header[41] = (byte) ((totalBytes >> 8) & 0xff);
                header[42] = (byte) ((totalBytes >> 16) & 0xff);
                header[43] = (byte) ((totalBytes >> 24) & 0xff);

                fos.Write(header, 0, 44);

                //Vedad - need a more efficient way of doing this...
                foreach(byte item in bufferList)
                {
                    fos.Write(item);
                }

                Console.WriteLine("Cleaning write stream and recorder");
                fos.Flush();
                fos.Close();
                fos.Dispose();


                onlyInstance.CleanRecorder();
//                Console.WriteLine("Sleeping");
//                SystemClock.Sleep(1000);
//                Console.WriteLine("Awake");

                Console.WriteLine("Cancelling write thread");

                onlyInstance.fileReadComplete();

                this.Cancel();
            }
        }

        public class RecorderHandler : Java.Util.TimerTask {
            private bool isRecording;

            public RecorderHandler()
            {
                this.isRecording = false;
            }

            public override void Run ()
            {
                onlyInstance.recorder = new AudioRecord(AudioSource.Mic, onlyInstance.sampleRate, onlyInstance.channels, onlyInstance.outputFormat, onlyInstance.bufferSize);

                if (onlyInstance.recorder == null || onlyInstance.recorder.State != State.Initialized) 
                {
                    Console.WriteLine("Problem setting up recorder");
                } 
                else 
                {
                    Console.WriteLine("About to start recording");

                    Java.Util.Timer timer = new Java.Util.Timer ();
                    var task = new BufferWrite ();
                    timer.Schedule (task, 0);

                    this.isRecording = true;
                }

                this.Cancel();
            }
        }

        public bool InitializeRecorder()
        {
            bool retVal = false;

            sampleRate = 44100;
            channels = ChannelIn.Mono;
            outputFormat = Android.Media.Encoding.Pcm16bit;

            bufferSize = AudioRecord.GetMinBufferSize(sampleRate, channels, outputFormat);

            filePath = "/sdcard/test.wav";

            recorder = new AudioRecord(AudioSource.Mic, sampleRate, channels, outputFormat, bufferSize);

            if (recorder == null || recorder.State != State.Initialized) 
            {
                Console.WriteLine("Problem setting up recorder");
            } 
            else 
            {
                retVal = true;
                Console.WriteLine("Recorder successfully initialized");
            }

            return retVal;
        }

        public void CleanRecorder()
        {
            if(recorder != null)
            {
                onlyInstance.recorder.Release();
                onlyInstance.recorder = null;
            }
        }

		public string StartRecording()
		{
            //Vedad - start our main recorder thread
            Java.Util.Timer timer = new Java.Util.Timer ();
            var task = new BufferWrite ();
            timer.Schedule (task, 0);

//            Java.Util.Timer timer = new Java.Util.Timer ();
//            var task = new RecorderHandler ();
//            timer.Schedule(timer, 0);
//            timer.ScheduleAtFixedRate (task, 0, 5000);

            return onlyInstance.filePath;
		}

		public void StopRecording()
		{
            onlyInstance.recorder.Stop();
		}

        public void ConvertAudioStreamToIntByteArray(string filename, byte[] wav, out int [] newdatabits)
		{
			// Read header, data and channels as separated data
			// Normalized data stores. Store values in the format -1.0 to 1.0
			byte[] waveheader = null;
			byte[] wavedata = null;
			//int[] zerocrossinginterval = new int[500000];

			int sampleRate = 0;
			float maxSample = 0.9f;
			float minSample = -0.9f;

			float[] in_data_l = null;
			float[] in_data_r = null;

			// retrieve the data 
            GetWaveData(filename, wav, out waveheader, out wavedata, out sampleRate, out in_data_l, out in_data_r); 

			// Need to find maximum sample value and use that to detect intervals //
			int maxPeakNumber = 0;
			int minPeakNumber = 0;

			for (int i = 0; i < in_data_l.Length; i++)
			{
				if (in_data_l [i] > maxSample)
				{
					maxPeakNumber++;
				}
				else if (in_data_l [i] < minSample)
				{
					minPeakNumber++;
				}
			}

			// Build array of sampleinterval counts at maxsamplevalue * margin -- look for pulse tips
			int numberOfPeaks = maxPeakNumber + minPeakNumber;
			int[] peakIntervalArray = new int[numberOfPeaks];

			int peakIntervalIndex = 0;
			for (int i = 0; i < in_data_l.Length; i++)
			{
				if (in_data_l [i] < minSample)
				{
					peakIntervalArray [peakIntervalIndex] = i;

					Console.WriteLine ("Min Peak :: " + in_data_l [i] + "   index :: " + i);
					peakIntervalIndex++;
				}
			}

			// Calculate sample intervals and determine logic data
			newdatabits = new int[2400];
			int[] newintervaldiff = new int[10000]; 

			int newdatabitcount = 0;

			for (int i = 0; i < numberOfPeaks - 1; i++)
			{
				newintervaldiff [i] = peakIntervalArray [i + 1] - peakIntervalArray [i];

				if ( newintervaldiff [i] > 10 && newintervaldiff [i] < 19)
				{
					newdatabits[ newdatabitcount] = 0;
					newdatabitcount++;
				}
				if ( newintervaldiff [i] > 18 && newintervaldiff [i] < 27)
				{
					newdatabits[ newdatabitcount] = 1;
					newdatabitcount++;
				}

				Console.WriteLine ("New intervaldiff :: " + newintervaldiff [i]);
			}
		}


		// Returns left and right float arrays. 'right' will be null if sound is mono.
        public static void GetWaveData(string filename, byte[] wav, out byte[] header, out byte[] data, out int sampleRate, out float[] left, out float[] right)
		{
            if(wav == null)
			    wav = File.ReadAllBytes(filename);

			// Determine if mono or stereo
			int channels = wav[22];     // Forget byte 23 as 99.999% of WAVs are 1 or 2 channels

			// Get sample rate
			sampleRate = BitConverter.ToInt32(wav, 24);

			int pos = 12;

			// Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
			while(!(wav[pos]==100 && wav[pos+1]==97 && wav[pos+2]==116 && wav[pos+3]==97)) 
			{
				pos += 4;
				int chunkSize = wav[pos] + wav[pos + 1] * 256 + wav[pos + 2] * 65536 + wav[pos + 3] * 16777216;
				pos += 4 + chunkSize;
			}

			pos += 4;

			int subchunk2Size = BitConverter.ToInt32(wav, pos);
			pos += 4;

			// Pos is now positioned to start of actual sound data.
			int samples = subchunk2Size / 2;     // 2 bytes per sample (16 bit sound mono)
			if (channels == 2) 
				samples /= 2;        // 4 bytes per sample (16 bit stereo)

			// Allocate memory (right will be null if only mono sound)
			left = new float[samples];

			if (channels == 2) 
				right = new float[samples];
			else 
				right = null;

			header = new byte[pos];
			Array.Copy(wav, header, pos);

			data = new byte[subchunk2Size];
			Array.Copy(wav, pos, data, 0, subchunk2Size);

			// Write to float array/s:
			int i=0;            
			while (pos < subchunk2Size) 
			{
				left[i] = BytesToNormalized_16(wav[pos], wav[pos + 1]);
				pos += 2;
				if (channels == 2) 
				{
					right[i] = BytesToNormalized_16(wav[pos], wav[pos + 1]);
					pos += 2;
				}
				i++;
			}
		}
			
		// Convert two bytes to one double in the range -1 to 1
		static float BytesToNormalized_16(byte firstByte, byte secondByte) 
		{
			// convert two bytes to one short (little endian)
			short s = (short)((secondByte << 8) | firstByte);
			// convert to range from -1 to (just below) 1
			return s / 32678f;
		}

		// Convert a float value into two bytes (use k as conversion value and not Int16.MaxValue to avoid peaks)
		static void NormalizedToBytes_16(float value, float k, out byte firstByte, out byte secondByte)
		{
			short s = (short)(value * k);
			firstByte  = (byte)(s & 0x00FF);
			secondByte = (byte)(s >> 8);
		}

		public void PrintByteArray(byte[] bytes)
		{
            var sb = new System.Text.StringBuilder("new byte[] { ");
			foreach (var b in bytes)
			{
				sb.Append(b + ", ");
			}
			sb.Append("}");
			Console.WriteLine(sb.ToString());
		}

		public bool IsSilence(float[] samples)
		{
			// power_RMS = sqrt(sum(x^2) / N)

			double sum = 0;

			for (int i = 0; i < samples.Length; i++)
				sum += samples[i] * samples[i];

			double power_RMS = System.Math.Sqrt(sum / samples.Length);

			return power_RMS < 0.01;
		}


		private double GoertzelMagnitude(float[] samples, double targetFrequency)
		{
			double n = samples.Length;
			int k = (int)(0.5D + ((double)n * targetFrequency) / (double)44100);
			double w = 2.0D * System.Math.PI  * k / n;
			double cosine = System.Math.Cos(w);
            double sine = System.Math.Sin(w);
			double coeff = 2.0D * cosine;

			double q0 = 0, q1 = 0, q2 = 0;

			for (int i = 0; i < samples.Length; i++)
			{
				double sample = samples[i];

				q0 = sample + coeff * q1 - q2 ;
				q2 = q1;
				q1 = q0;
			}

			double real = (q1 * q2 * cosine);
			double imag = (q2 * sine);

			//double magnitude = Math.Sqrt(q1 * q1 + q2 * q2 - q1 * q2 * coeff);
            double magnitude = System.Math.Sqrt (real * real + imag * imag);
			return magnitude;
		}

		public void FFT(double[] data, bool forward)
		{
			int n, i, i1, j, k, i2, l, l1, l2;
			double c1, c2, tx, ty, t1, t2, u1, u2, z;

			// Calculate the number of points
            int m = (int)(System.Math.Log(data.Length,2.0)-0.05);
			n = data.Length;
			// check all are valid
			if (((n & (n - 1)) != 0) || (n != (2<<m))) // checks nn is a power of 2 in 2's complement format
                throw new System.Exception("data length " + n + " in FFT is not a power of 2");
			n /= 2;

			//Console.WriteLine("Length {0} {1}",n,m);

			// Do the bit reversal
			i2 = n >> 1;
			j = 0;
			for (i = 0; i < n - 1; i++)
			{
				if (i < j)
				{
					tx = data[i * 2];
					ty = data[i * 2 + 1];
					data[i * 2] = data[j * 2];
					data[i * 2 + 1] = data[j * 2 + 1];
					data[j * 2] = tx;
					data[j * 2 + 1] = ty;
				}
				k = i2;

				while (k <= j)
				{
					j -= k;
					k >>= 1;
				}
				j += k;
			}

			// Compute the FFT 
			c1 = -1.0f;
			c2 = 0.0f;
			l2 = 1;
			for (l = 0; l < m; l++)
			{
				l1 = l2;
				l2 <<= 1;
				u1 = 1.0f;
				u2 = 0.0f;
				for (j = 0; j < l1; j++)
				{
					for (i = j; i < n; i += l2)
					{
						i1 = i + l1;
						t1 = u1 * data[i1 * 2] - u2 * data[i1 * 2 + 1];
						t2 = u1 * data[i1 * 2 + 1] + u2 * data[i1 * 2];
						data[i1 * 2] = data[i * 2] - t1;
						data[i1 * 2 + 1] = data[i * 2 + 1] - t2;
						data[i * 2] += t1;
						data[i * 2 + 1] += t2;
					}
					z = u1 * c1 - u2 * c2;
					u2 = u1 * c2 + u2 * c1;
					u1 = z;
				}
                c2 = (float)System.Math.Sqrt((1.0f - c1) / 2.0f);
				if (!forward)
					c2 = -c2;
                c1 = (float)System.Math.Sqrt((1.0f + c1) / 2.0f);
			}

			// Scaling for inverse transform 
			if (!forward)
			{
				for (i = 0; i < n; i++)
				{
					data[i * 2] /= n;
					data[i * 2 + 1] /= n;
				}
			}
		}
	}
}
