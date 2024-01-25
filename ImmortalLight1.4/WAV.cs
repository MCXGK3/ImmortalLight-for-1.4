using System;

namespace ImmortalLight
{
    // Token: 0x02000015 RID: 21
    public class WAV
    {
        // Token: 0x17000017 RID: 23
        // (get) Token: 0x06000076 RID: 118 RVA: 0x0000236B File Offset: 0x0000056B
        // (set) Token: 0x06000077 RID: 119 RVA: 0x00002373 File Offset: 0x00000573
        public float[] LeftChannel { get; internal set; }

        // Token: 0x17000018 RID: 24
        // (get) Token: 0x06000078 RID: 120 RVA: 0x0000237C File Offset: 0x0000057C
        // (set) Token: 0x06000079 RID: 121 RVA: 0x00002384 File Offset: 0x00000584
        public float[] RightChannel { get; internal set; }

        // Token: 0x17000019 RID: 25
        // (get) Token: 0x0600007A RID: 122 RVA: 0x0000238D File Offset: 0x0000058D
        // (set) Token: 0x0600007B RID: 123 RVA: 0x00002395 File Offset: 0x00000595
        public int ChannelCount { get; internal set; }

        // Token: 0x1700001A RID: 26
        // (get) Token: 0x0600007C RID: 124 RVA: 0x0000239E File Offset: 0x0000059E
        // (set) Token: 0x0600007D RID: 125 RVA: 0x000023A6 File Offset: 0x000005A6
        public int SampleCount { get; internal set; }

        // Token: 0x1700001B RID: 27
        // (get) Token: 0x0600007E RID: 126 RVA: 0x000023AF File Offset: 0x000005AF
        // (set) Token: 0x0600007F RID: 127 RVA: 0x000023B7 File Offset: 0x000005B7
        public int Frequency { get; internal set; }

        // Token: 0x06000080 RID: 128 RVA: 0x000054B8 File Offset: 0x000036B8
        public WAV(byte[] wav)
        {
            this.ChannelCount = (int)wav[22];
            this.Frequency = this.BytesToInt(wav, 24);
            int i = 12;
            while (wav[i] != 100 || wav[i + 1] != 97 || wav[i + 2] != 116 || wav[i + 3] != 97)
            {
                i += 4;
                int num = (int)wav[i] + (int)wav[i + 1] * 256 + (int)wav[i + 2] * 65536 + (int)wav[i + 3] * 16777216;
                i += 4 + num;
            }
            i += 8;
            this.SampleCount = (wav.Length - i) / 2;
            bool flag = this.ChannelCount == 2;
            bool flag2 = flag;
            if (flag2)
            {
                this.SampleCount /= 2;
            }
            this.LeftChannel = new float[this.SampleCount];
            bool flag3 = this.ChannelCount == 2;
            bool flag4 = flag3;
            if (flag4)
            {
                this.RightChannel = new float[this.SampleCount];
            }
            else
            {
                this.RightChannel = null;
            }
            int num2 = 0;
            while (i < wav.Length)
            {
                this.LeftChannel[num2] = this.BytesToFloat(wav[i], wav[i + 1]);
                i += 2;
                bool flag5 = this.ChannelCount == 2;
                bool flag6 = flag5;
                if (flag6)
                {
                    this.RightChannel[num2] = this.BytesToFloat(wav[i], wav[i + 1]);
                    i += 2;
                }
                num2++;
            }
        }

        // Token: 0x06000081 RID: 129 RVA: 0x00005628 File Offset: 0x00003828
        private float BytesToFloat(byte firstByte, byte secondByte)
        {
            short num = (short)((int)secondByte << 8 | (int)firstByte);
            return (float)num / 32768f;
        }

        // Token: 0x06000082 RID: 130 RVA: 0x0000564C File Offset: 0x0000384C
        private int BytesToInt(byte[] bytes, int offset = 0)
        {
            int num = 0;
            for (int i = 0; i < 4; i++)
            {
                num |= (int)bytes[offset + i] << i * 8;
            }
            return num;
        }
    }
}
