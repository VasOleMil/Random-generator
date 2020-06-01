using System;

namespace RandomHSM
{
    public partial class CRandom
    {
        //--------------------------------------------------------------------
        CElement[]  Ev;//Elements container
        //--------------------------------------------------------------------
        private void EmtsIni()
        {
            //Allocate resources
            try
            {
                Ev = new CElement[En]; for (int i = 0; i < En; i++) Ev[i] = new CElement();
            }
            catch (Exception e)
            {
                e.Source = "EmtsIni";
                Ev = null;
                return;
            }
            //Allocate resources
            for (int i = 0; i < En; i++) if(Ev[i].Dim(Rn))
            {
                //throw exeption
                Ev = null;
                return;
            }
            //init elements
            for (int i = 0; i < En; i++) 
            {
                //Set Radius of element
                Ev[i].R = Re * (1D + 0.01 * dR * 2D * (Rd.NextDouble() - 0.5D));
                //Set mass
                Ev[i].M = Vgamma(Rn);
                for (int k = 0; k < Rn; k++)
                {
                    Ev[i].M *= Ev[i].R;
                }
                //Set coordinates, random value
                EmtsIniGtb(i);
                //set speed, random value
                double V = 1D / Math.Sqrt(Rn * Ev[i].M);
                for (int k = 0; k < Rn; k++)
                {
                    Ev[i].V[k] = V * 2D * (Rd.NextDouble() - 0.5D);
                }
            }
        }
        //--------------------------------------------------------------------
        private void EmtsIniGtb(int Ei)
        {
            int k; double Rr, xx, x; CElement ei = Ev[Ei];

            Rr =  Rb * Rb; do
            {
                xx = 0D; for (k = 0; k < Rn; k++)
                {
                    ei.X[k] = x = Rb * 2D * (Rd.NextDouble() - 0.5D);
                    xx += x * x;
                }
            }
            while (xx >= Rr);
        }//Set random coordinates of Ex element within bound
        //--------------------------------------------------------------------
        private void EmtsMovAll()
        {
            for (int i = 0; i < En; i++)
            {
                CElement ei= Ev[i]; for (int k = 0; k < Rn; k++)
                {
                        ei.X[k] += ei.V[k] * dT;
                }
            }
        }//Moves all elements
        //--------------------------------------------------------------------
        private void EmtsColSel(int Ei, int Ej)
        {
            CElement ei = Ev[Ei], ej = Ev[Ej];

            double Mi = ei.M;
            double Mj = ej.M;

            double Mimj = (Mi - Mj);
            double Mipj = 1D / (Mi + Mj);

            for (int k = 0; k < Rn; k++)
            {
                double EiVk = ei.V[k];
                double EjVk = ej.V[k];

                ei.V[k] = Mipj * (2D * Mj * EjVk + Mimj * EiVk);
                ej.V[k] = Mipj * (2D * Mi * EiVk - Mimj * EjVk);
            }
        }//Interact elements
        //--------------------------------------------------------------------
        private void EmtsCollBE()
        {
            double RV, RR, Rr, Vv; int k; CElement ex = Ev[Rs.E];

            RR = 0D; RV = 0D; for (k = 0; k < Rn; k++)
            {
                Rr = ex.X[k];
                Vv = ex.V[k];

                RR += Rr * Rr;
                RV += Rr * Vv;
            }

            Vv = (RR <= 0D) ? (0D) : (2D * RV / RR);
            for (k = 0; k < Rn; k++) ex.V[k] -= Vv * ex.X[k];
        }//Bound element interaction
        //--------------------------------------------------------------------
        private void EmtsRegEvt()
        {
            CElement ex = Ev[Rs.E]; for (int k = 0; k < Rn; k++)
            {
                double x = ex.X[k];
                Rs.X[k] = x; 
                Rs.V[k] = x / Rb;
            }        
        }
        //--------------------------------------------------------------------
    }
}
