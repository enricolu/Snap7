﻿using Sharp7;
using System.Threading;

namespace Synchronisiereinrichtung
{
    public partial class MainWindow
    {
        public bool Q1;
        public bool S1;
        public bool S2;

        public int Y;
        public int Ie;

        public short n;
        public short fGenerator;
        public short fNetz;
        public short UGenerator;
        public short UNetz;
        public short PNetz;
        public short UDiff;
        public short ph;

        public double Drehzahl;
        public double FrequenzGenerator;
        public double FrequenzNetz;
        public double SpannungGenerator;
        public double SpannungNetz;
        public double LeistungNetz;
        public double LeistungGenerator;
        public double SpannungDifferenz;
        public double Phasenlage;

        enum Datenbausteine
        {
            DigIn = 1,
            DigOut,
            AnIn,
            AnOut
        }
        enum BytePosition
        {
            Byte_0 = 0
        }
        enum AnzahlByte
        {
            Byte_1 = 1
        }
        enum BitPosAusgang
        {
            Q1 = 0
        }
        enum BitPosEingang
        {
            S1 = 0,
            S2
        }

        public void DatenRangieren_Task()
        {
            while (TaskAktiv && FensterAktiv)
            {
                if ((Client != null) && TaskAktiv && !DebugWindowAktiv)
                {
                    S7.SetIntAt(AnalogInput, 0, n);
                    S7.SetIntAt(AnalogInput, 2, UNetz);
                    S7.SetIntAt(AnalogInput, 4, fNetz);
                    S7.SetIntAt(AnalogInput, 6, UGenerator);
                    S7.SetIntAt(AnalogInput, 8, fGenerator);
                    S7.SetIntAt(AnalogInput, 10, PNetz);
                    S7.SetIntAt(AnalogInput, 12, UDiff);
                    S7.SetIntAt(AnalogInput, 14, ph);

                    Client.DBWrite((int)Datenbausteine.DigIn, (int)BytePosition.Byte_0, (int)AnzahlByte.Byte_1, DigInput);
                    Client.DBRead((int)Datenbausteine.DigOut, (int)BytePosition.Byte_0, (int)AnzahlByte.Byte_1, DigOutput);

                    Y = S7.GetIntAt(AnalogOutput, 0);
                    Ie = S7.GetIntAt(AnalogOutput, 2);

                    S7.SetBitAt(ref DigInput, (int)BytePosition.Byte_0, (int)BitPosEingang.S1, S1);
                    S7.SetBitAt(ref DigInput, (int)BytePosition.Byte_0, (int)BitPosEingang.S2, S2);
                    Q1 = S7.GetBitAt(DigOutput, (int)BytePosition.Byte_0, (int)BitPosAusgang.Q1);
                }

                Thread.Sleep(100);
            }
        }

        private int InByte(BitPosEingang Pos) { return ((int)Pos) / 8; }
        private int InBit(BitPosEingang Pos) { return ((int)Pos) % 8; }
        private int OutByte(BitPosAusgang Pos) { return ((int)Pos) / 8; }
        private int OutBit(BitPosAusgang Pos) { return ((int)Pos) % 8; }

        void EinAusgabeFelderInitialisieren()
        {
            foreach (byte b in DigInput) DigInput[b] = 0;
            foreach (byte b in DigOutput) DigOutput[b] = 0;
            foreach (byte b in AnalogOutput) AnalogOutput[b] = 0;
            foreach (byte b in AnalogInput) AnalogInput[b] = 0;
        }

    }
}
