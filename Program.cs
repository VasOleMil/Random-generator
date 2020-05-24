using System;
using System.IO;

namespace RandomHSM
{
    
    class Program
    {
        //--------------Compression parameters--------------------------------
        static int          Dim = 4;                //Dim - dimension , 
        static int          Num = 100;              //Num number of elements
        //--------------------------------------------------------------------
        static CRandom      Rd = null;
        static FileStream   fs = null;
        static StreamWriter sw = null;
        static StreamWriter st = null;
        //--------------------------------------------------------------------
        static void SetInputParameters(string[] args)
        {
            try
            {
                int dim = Convert.ToInt32(args[0]); if (3 <= dim && dim <= 8)       Dim = dim;
                int num = Convert.ToInt32(args[1]); if (1 <= num && num <= 1000)    Num = num;
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
                string fn = "Rep-" + Dim.ToString("D2") + "-" + Num.ToString("D3") + ".txt";
                Console.WriteLine("Dim:\t{0:D2}\nNum:\t{1:D3}\nFile:\t{2}", Dim, Num, fn);
                fs = new FileStream(fn, FileMode.Create, FileAccess.Write, FileShare.Read);
                sw = new StreamWriter(fs);
            }
        }//Init streams  file an console
         //--------------------------------------------------------------------
        static void SetOutToFile()
        {
            st.Flush(); Console.SetOut(sw);
        }//Redirect out to log file
         //--------------------------------------------------------------------
        static void SetOutToCons()
        {
            sw.Flush(); Console.SetOut(st);
        }//Redirect out to log file
        //--------------------------------------------------------------------
        static void RemOutToFile()
        {
            if (sw != null)
            {
                sw.Flush(); sw.Close(); sw = null;
            }

            Console.SetOut(st);
        }//Redirect out to console
        //--------------------------------------------------------------------
        static void Main(string[] args)
        {
            //Get program parameters
            SetInputParameters(args);

            //Report and redirect out to log file
            IniOutToFile();
            SetOutToFile();

            //Create generator
            //Rd = new CRandom(3, 100, 15.8, 1.0, 10.0);
            //Rd = new CRandom(4, 100, 8.00, 1.0, 10.0);
            //Rd = new CRandom(5, 100, 5.70, 1.0, 10.0);
            //Rd = new CRandom(6, 100, 4.65, 1.0, 10.0);
            //Rd = new CRandom(7, 100, 4.07, 1.0, 10.0);
            //Rd = new CRandom(8, 100, 3.68, 1.0, 10.0);
            
            //Rd = new CRandom(3, 300, 28.5, 1.0, 10.0);

            switch (Dim)
            {
                case 3: Rd = new CRandom(3, 100, 15.8, 1.0, 10.0); break;
                case 4: Rd = new CRandom(4, 100, 8.00, 1.0, 10.0); break;
                case 5: Rd = new CRandom(5, 100, 5.70, 1.0, 10.0); break;
                case 6: Rd = new CRandom(6, 100, 4.65, 1.0, 10.0); break;
                case 7: Rd = new CRandom(7, 100, 4.07, 1.0, 10.0); break;
                case 8: Rd = new CRandom(8, 100, 3.68, 1.0, 10.0); break;
            }

            for (int i = 0; i < Num; i++)
            {
                Rd.Next();

                String rs = "{"+ Rd.Rs.V[0].ToString("G8").Replace(",","."); 
                for (int k = 1; k < Dim; k++) rs += ",\t" + Rd.Rs.V[k].ToString("G8").Replace(",", "."); rs += "},";
                Console.WriteLine("{0}", rs);
            }

            //Finally,
            RemOutToFile(); //Console.ReadKey(); 
        }
    }
}
