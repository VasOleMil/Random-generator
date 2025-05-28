using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace RandomHSM
{   
    class Program
    {
        //------------------Default parameters--------------------------------
        static long          Dim = 3L;               //Dim - dimension 
        static long          Num = 100;              //Num number of elements
        //--------------------------------------------------------------------
        static CRandom       Rd = null;
        static FileStream    fs = null;
        static StreamWriter  sw = null;
        static StreamWriter  st = null;
        static long          Hn = 200L;
        static long[]        Hm = null;
        static double        Hs;
        static Bitmap        Bm = null;
        static Graphics      Gr = null;
        static int           Bh = 600;
        static int           Bw = 600;
        static Color         Bc = Color.Blue;
        //--------------------------------------------------------------------
        static void SetInputParameters(string[] args)
        {
            try
            {
                long dim = Convert.ToInt64(args[0]); if (3 <= dim && dim <=    12  )   Dim = dim;
                long num = Convert.ToInt64(args[1]); if (3 <= num && num <= 1000000)   Num = num;
            }
            catch (Exception e)
            {
                e.Source = "SetInputParameters";
                Console.WriteLine("Default values");
            }
        }//Set input parameters if present
         //--------------------------------------------------------------------
        static void IniOutToFile()
        {
            if (sw == null)
            {
                st = new StreamWriter(Console.OpenStandardOutput()); st.AutoFlush = true;
                string fn = "Rep-" + Dim.ToString("D2") + "-" + Num.ToString("D3") + ".json";
                Console.WriteLine("Dim:\t{0:D2}\nNum:\t{1:D3}\nFile:\t{2}", Dim, Num, fn);
                fs = new FileStream(fn, FileMode.Create, FileAccess.Write, FileShare.Read);
                sw = new StreamWriter(fs);

                Bm = new Bitmap(Bw, Bh, PixelFormat.Format32bppArgb);
                Gr = Graphics.FromImage(Bm);
            }
        }//Init streams:  file and console
         //--------------------------------------------------------------------
        static void SetOutToFile()
        {
            st.Flush(); Console.SetOut(sw);
        }//Redirect out to log file
         //--------------------------------------------------------------------
        static void SetOutToCons()
        {
            sw.Flush(); Console.SetOut(st);
        }//Redirect out to console
        //--------------------------------------------------------------------
        static void RemOutToFile()
        {
            if (sw != null)
            {
                Console.WriteLine("}");//close json structure
                
                Bm.Save("Rep-" + Dim.ToString("D2") + "-" + Num.ToString("D3") + ".png", ImageFormat.Png);
                Bm.Dispose(); Bm = null;    
                Gr.Dispose(); Gr = null;    
                sw.Flush(); sw.Close(); sw = null;
            }

            Console.SetOut(st);
        }//Redirect out to console
         //--------------------------------------------------------------------
        static void Prepare_Step()
        {
            for (long i = 0L; i < Hn; i++) Hm[i] = 0L;   
            Console.Write("{\n\"values\":[\n");
        }//Set start of json array
        //--------------------------------------------------------------------
        static void ReportOnStep(long i)
        {
            String rs;
            rs = "[" + Rd.Rs.V[0].ToString("G15").Replace(",", ".");
            for (long k = 1; k < Dim; k++) rs += ",\t" + Rd.Rs.V[k].ToString("G15").Replace(",", ".");
            rs += "]"; if (i < Num) rs += ","; else rs += "]"; Console.WriteLine("{0}", rs);

        }//(i == Num) - end of json array
         //--------------------------------------------------------------------
        static void HystogramStp()
        {
            double rk, rr = 0D; long k;
            for (k = 0L; k < Dim - 2; k++) 
            {
                rk = Rd.Rs.X[k]; rk *= rk; rr += rk;
            }
            k = (long)(Hs * Math.Sqrt(rr));
            k -= (k == Hn) ? 1L : 0L; Hm[k]++;

            Bm.SetPixel(4 + (int)((Rd.Rs.V[0] + 1.0F) * ((Bw-10) /2)), 4 + (int)((Rd.Rs.V[1] + 1.0F) * ((Bh-10) /2)), Bc);
        }
        //--------------------------------------------------------------------
        static void HystogramRep()
        {
            String rs = ",\"density\":[\n";
            long k = Dim - 2L, e = Hn - 1L, n = k - 1L;
            double R = 1D, S = R / Hn, V = (Hn * R) / (Num * k), t;

            for (k = 0L; k < Hn; k++)
            {
                rs += "[" + (t = (S * (double)(k + 1L))).ToString("G8").Replace(",", ".") + ",\t";
                rs += (Math.Pow(R / t, n) * V * Hm[k]).ToString("G8").Replace(",", ".") + "]";
                if (k < e) rs += ",\n"; else rs += "]";
            }
            Console.WriteLine("{0}", rs);
        }
        //--------------------------------------------------------------------
        static int Main(string[] args)
        {
            //Get program parameters
            SetInputParameters(args);
            
            try
            {
                
                //Create random generator
                Rd = new CRandom(Dim, 100L);
                //prepare hystogram
                Hm = new long[Hn]; 
                Hs = (double)Hn / Rd.Rb;
                //Create and report log file
                IniOutToFile();
            }
            catch (Exception e)
            {
                e.Source = "Main"; return 1;
            }
            
            SetOutToFile(); //Redirect out to log file

            //Rd.TEnergy(); double kT = Rd.kT; Console.WriteLine("E0 = {0:G15}", (kT).ToString("G15").Replace(",", "."));

            for (long i = 1L; i <= 400000; i++) Rd.Next();//warm random generator                

            Prepare_Step();
            for (long i = 1L; i <= Num; i++)
            {
                Rd.Next();//Step random generator
                //Rd.TEnergy(); Console.WriteLine("Ee = {0:G15}", (Rd.kT).ToString("G15").Replace(",", "."));
                HystogramStp( );
                ReportOnStep(i);
            }
            HystogramRep();

            //Rd.TEnergy(); Console.WriteLine("Ed = {0:G15}", (1D-Rd.kT/kT).ToString("G15").Replace(",", "."));

            //Finally //Console.ReadKey(); //if necessary
            RemOutToFile(); return 0;  //fail safe report
        }
    }
}
