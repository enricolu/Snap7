﻿using System.Threading;
using Utilities;

namespace LAP_2019_Foerderanlage
{
    public class Logikfunktionen
    {
        const double WagenGeschwindigkeit = 3;
        const double WagenPositionLinks = 0;
        const double WagenPositionRechts = 125;

        const double WagenFuellstandLeeren = 5;
        const double WagenFuellstandFuellen = 1;
        const double WagenFuellstandVoll = 88;

        const double MaterialSiloFuellen = 0.01;
        const double MaterialSiloLeeren = 0.002;

        private readonly MainWindow mainWindow;

        public Logikfunktionen(MainWindow window)
        {
            mainWindow = window;
        }

        public void Logikfunktionen_Task()
        {
            while (mainWindow.FensterAktiv)
            {
                switch (mainWindow.WagenRichtung)
                {
                    case MainWindow.Richtung.nachLinks:
                        if (mainWindow.WagenPosition_X > WagenPositionLinks) mainWindow.WagenPosition_X -= WagenGeschwindigkeit;
                        if (mainWindow.WagenPosition_X <= WagenPositionLinks)
                        {
                            mainWindow.WagenPosition_X = WagenPositionLinks;
                            mainWindow.WagenRichtung = MainWindow.Richtung.stehen;
                        }
                        break;

                    case MainWindow.Richtung.nachRechts:
                        if (mainWindow.WagenPosition_X < WagenPositionRechts) mainWindow.WagenPosition_X += WagenGeschwindigkeit;
                        if (mainWindow.WagenPosition_X >= WagenPositionRechts)
                        {
                            mainWindow.WagenPosition_X = WagenPositionRechts;
                            mainWindow.WagenRichtung = MainWindow.Richtung.stehen;
                        }
                        break;

                    case MainWindow.Richtung.stehen:
                    default:
                        break;
                }

                // Wagen bewegen
                if (mainWindow.WagenPosition_X == WagenPositionRechts) mainWindow.S7 = true; else mainWindow.S7 = false;
                if (mainWindow.WagenFuellstand == WagenFuellstandVoll) mainWindow.S8 = true; else mainWindow.S8 = false;

                if (mainWindow.WagenPosition_X == WagenPositionLinks)
                {
                    if (mainWindow.WagenFuellstand > 0) mainWindow.WagenFuellstand -= WagenFuellstandLeeren;
                }

                // Materialsilo fuellen/leeren
                if (mainWindow.XFU) mainWindow.MaterialSiloFuellstand += MaterialSiloFuellen;

                if (mainWindow.MaterialSiloFuellstand > 0 & mainWindow.Q4_LL & mainWindow.Y1)
                {
                    mainWindow.WagenFuellstand += WagenFuellstandFuellen;
                    mainWindow.MaterialSiloFuellstand -= MaterialSiloLeeren;
                }
                if (mainWindow.WagenFuellstand > WagenFuellstandVoll) mainWindow.WagenFuellstand = WagenFuellstandVoll;
                if (mainWindow.WagenFuellstand < 0) mainWindow.WagenFuellstand = 0;

                if (mainWindow.MaterialSiloFuellstand > 1) mainWindow.MaterialSiloFuellstand = 1;
                if (mainWindow.MaterialSiloFuellstand < 0) mainWindow.MaterialSiloFuellstand = 0;


                mainWindow.MaterialsiloPegel = S7Analog.S7_Analog_2_Short(mainWindow.MaterialSiloFuellstand, 1);

                Thread.Sleep(100);
            }
        }
    }
}