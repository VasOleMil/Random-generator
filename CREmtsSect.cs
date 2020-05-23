using System;

namespace RandomHSM
{
    public partial class CRandom
    {
        //--------------------------------------------------------------------
        CElement[]  Ev;//Elements container
        int         Ei;//Impicit argument, Pointer for exchange
        int         Ej;//Impicit argument, Pointer for exchange
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
                //Set arguments
                Ei = i; 
                //Set Radius of element
                Ev[i].R = Ri + 0.01 * dR * 2D * (Rd.NextDouble() - 0.5D);
                //Set mass
                Ev[i].M = Vgamma(Rn);
                for (int k = 0; k < Rn; k++)
                {
                    Ev[i].M *= Ev[i].R;
                }
                //Set coordinates, random value
                do EmtsIniGtb(i); while (EmtsIniGte(i));
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
            int k; double Rr, r, xx, x; CElement ei = Ev[Ei];

            Rr = (r = Rb - ei.R) * r;

            do
            {
                xx = 0D; for (k = 0; k < Rn; k++)
                {
                    ei.X[k] = x = r * 2D * (Rd.NextDouble() - 0.5D);
                    xx += x * x;
                }
            }
            while (xx >= Rr);
        }//Set random coordinates of Ex element within bound
        //--------------------------------------------------------------------
        bool EmtsIniGte(int Ei)
        {
            int j, k; double Rr, rr, x; CElement ei = Ev[Ei], ej;

            for (j = 0; j < Ei; j++)
            {
                ej = Ev[j]; rr = 0D; for (k = 0; k < Rn; k++)
                {
                    rr += (x = ej.X[k] - ei.X[k]) * x;
                }

                Rr = (Rr = ej.R + ei.R) * Rr;

                if (rr <= Rr) return true; 
            }

            return false;
        }//Checks for absence of elements union
        //--------------------------------------------------------------------
        private void EmtsMovAll()
        {
            for (int i = 0; i < En; i++)
            {
                for (int k = 0; k < Rn; k++)
                {
                        Ev[i].X[k] += Ev[i].V[k] * dT;
                }
            }
        }//Moves all elements
        //--------------------------------------------------------------------
        private void EmtsColSel()
        {
            if (Ej < 0)
            {
                EmtsCollBE(); Cb++; EmtsRegBvt();
            }
            else
            {
                EmtsCollEE(); Ce++; EmtsRegEvt();
            }
        }//Interact elements
        //--------------------------------------------------------------------
        private void EmtsCollBE()
        {
            double RV, RR, Rr, Vv; int k; CElement ei = Ev[Ei];

            RR = 0D; RV = 0D; for (k = 0; k < Rn; k++)
            {
                Rr = ei.X[k];
                Vv = ei.V[k];

                RR += Rr * Rr;
                RV += Rr * Vv;
            }

            Vv = (RR <= 0D) ? (0D) : (2D * RV / RR);
            for (k = 0; k < Rn; k++) ei.V[k] -= Vv * ei.X[k];
        }//Bound element interaction
        //--------------------------------------------------------------------
        private void EmtsCollEE()
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
        }//Simplified e-e interaction
        //--------------------------------------------------------------------
        private void EmtsRegEvt()
        {
            int k; double x, xx;

            CElement ei = Ev[Ei]; Rs.Ei = Ei;
            CElement ej = Ev[Ej]; Rs.Ej = Ej;

            xx = 0D; for (k = 0; k < Rn; k++)
            {
                Rs.X[k] = x = (ej.X[k] * ei.R + ei.X[k] * ej.R) / (ei.R + ej.R);
                Rs.V[k] = x;
                xx += x * x;
            }

            x = 1D / Math.Sqrt(xx); for (k = 0; k < Rn; k++)
            {
                Rs.X[k] *= Rb * x;
                Rs.V[k] *= x;
            }
        }
        //--------------------------------------------------------------------
        private void EmtsRegBvt()
        {
            int k; double x, xx;

            CElement ei = Ev[Ei]; Rs.Ei = Ei; Rs.Ej = Ej;

            xx = 0D; for (k = 0; k < Rn; k++)
            {
                Rs.X[k] = x =  ei.X[k];
                Rs.V[k] = x;
                xx += x * x;
            }

            x = 1D / Math.Sqrt(xx); for (k = 0; k < Rn; k++)
            {
                Rs.X[k] *= Rb * x;
                Rs.V[k] *= x;
            }
        }
        //--------------------------------------------------------------------
    }
}
