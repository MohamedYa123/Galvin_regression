using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
namespace regressor2
{
    [Serializable]
    public class regressor
    {
        public List<double[]> inputs;
        public List<double> outputs;
        public List<double[]> binputs;
        public List<double> boutputs;
        public List<double[]> equation;
        public List<double[,]> sides;
        public double expandlimit;
        public int shufflelen;
        double centerpoint;
        bool expandd;
        int Terms;
        double mainacc;
        public double accuracy;
        public bool neglect;
        public double neglectionlimit;
        public bool bracks;
        public double brackelimit;
        public double errpow;
        Random rd;
        public bool expon;
        public double powf;
        public double powneglect;
        public regressor copy()
        {
            regressor reg = new regressor(binputs, boutputs, Terms,expon);
            reg.inputs = inputs;
            reg.outputs = outputs;
            reg.equation = copier();
            reg.sides = sidecopy();
            reg.shufflelen = shufflelen;
            reg.expandlimit = expandlimit;
            reg.neglectionlimit = neglectionlimit;
            reg.expandd = expandd;
            reg.bracks = bracks;
            reg.brackelimit = brackelimit;
            reg.errpow = errpow;
            reg.centerpoint = centerpoint;
            reg.Terms = Terms;
            reg.termlimit = termlimit;
            reg.sadd = sadd;
            reg.ti = ti;
            reg.ti1 = ti1;
            reg.to = to;
            reg.to1 = to1;
            reg.too1 = too1;
            reg.minpow = minpow;
            reg.maxpow = maxpow;
            reg.arnge = arnge;
            reg.wperc = wperc;
            reg.powf = powf;
            reg.powneglect = powneglect;
            rd = new Random();
            //    bool Nano;
            //    List<double[]> ti;
            //    List<double> to;
            //    List<double[]> ti1;
            //    List<double> to1;
            //    double bestpoint1;
            //    double bestpoint2;
            //    double bestpoint;
            //public double minpow;
            //public double maxpow;
            //List<double[]> tii1;
            //List<double> too1;
            //public int arnge;
            //public double wperc;
            return reg;
        }
        public  string write( int numdg22)
        {
            string mywrite = "y=";
            var reg = this;

            for (int i = 0; i < reg.equation.Count; i++)
            {
                for (int i2 = 0; i2 < reg.equation[0].Length; i2++)
                {
                    if (Math.Abs(reg.equation[i][0]) > 0.000001)
                    {
                        var r = ((decimal)Math.Round(reg.equation[i][i2], numdg22)).ToString();
                        if (i2 > 0)
                        {
                            if (Math.Abs(Convert.ToDouble(r)) > 0.000001)
                            {
                                if (expon && i2%2==0)
                                {
                                    mywrite += " * " + Math.Round(Math.Pow(powf, reg.equation[i][i2]), numdg22) + "^X" + ((i2) / 2);
                                }
                                else if (expon)
                                {
                                    mywrite += " * x" + ((i2-1)/2+1) + "^" + r;
                                }
                                else
                                {
                                    mywrite += " * x" + (i2)  + "^" + r;
                                }
                            }

                        }
                        else
                        {
                            if (Math.Abs(Convert.ToDouble(r)) < 0.000001)
                            {
                                break;
                            }
                            if (Convert.ToDouble(r) >= 0)
                            {
                                mywrite += "\r\n+" + r;
                            }
                            else
                            {
                                mywrite += "\r\n" + r;
                            }

                        }
                    }
                }
            }
            return mywrite + "\r\n" + "Equation created by Galvin-regression app";
        }
        public string excell(int numdg2)
        {
            string mywrite = "";
            string[] ge = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P" };
            var reg = this;
            for (int i = 0; i < reg.equation.Count; i++)
            {
                for (int i2 = 0; i2 < reg.equation[0].Length; i2++)
                {
                    if (Math.Abs(reg.equation[i][i2]) > 0.0001 || true)
                    {
                        var r = ((decimal)Math.Round(reg.equation[i][i2], numdg2)).ToString();
                        if (i2 > 0)
                        {
                            // if (Math.Abs(r) > 0.001)
                            if (expon && i2%2==0)
                            {
                                mywrite += " * POWER("+ Math.Round(Math.Pow(powf, reg.equation[i][i2]), numdg2) + ","  + ge[i2/2 - 1] + "2"  + ")";
                            }
                            else if (expon)
                            {
                                mywrite += " * POWER(" + ge[(i2 - 1)/2] + "2" + "," + r + ")";
                            }
                            else
                            {
                                mywrite += " * POWER(" + ge[i2 - 1] + "2" + "," + r + ")";
                            }

                        }
                        else
                        {
                            if (Math.Abs(Convert.ToDouble(r)) < 0.001)
                            {
                                //      break;
                            }
                            if (Convert.ToDouble(r) >= 0)
                            {
                                mywrite += "+" + r;
                            }
                            else
                            {
                                mywrite += "" + r;
                            }

                        }
                    }
                }

            }
            return mywrite;
        }
        void mutat()
        {
            Random r = new Random();
            var i = r.Next(0, Terms);
            var i2 = r.Next(1, equation[0].Length);
            var q = Convert.ToDouble(r.Next(9000, 11000)) / 10000;
            r = new Random();
            var fq = 2;//r.Next(1, 3);
            if (fq == 0)
            {
                q = Convert.ToDouble(r.Next(7000, 13000)) / 10000;
            }
            else if (fq == 1)
            {
                q = Convert.ToDouble(r.Next(4000, 9000)) / 10000;
            }
            else if (fq == 2)
            {
                q = Convert.ToDouble(r.Next(9500, 10500)) / 10000;
            }
            var q2 = equation[i][i2];
            equation[i][i2] *= q;
            q2 = equation[i][i2];
        }
        public void mutatme(int mutats)
        {
            for (int u = 0; u < mutats; u++)
            {
                mutat();
                //Thread.Sleep(1);
            }

            modifier(equation);
        }
        public double termlimit;
        public int sadd;
        public regressor(List<double[]> Inputs, List<double> Outputs, int terms, bool Exp, double point = 0.01)
        {
            expandd = false;
            inputs = Inputs;
            termlimit = 100;
            ti = Inputs;
            to = Outputs;
            expandlimit = 0.01;
            shufflelen = 200;
            Terms = terms;
            powf = 1.001;
            outputs = Outputs;
            equation = new List<double[]>();
            sides = new List<double[,]>();
            binputs = new List<double[]>();
            boutputs = new List<double>();
            mainacc = -1000000;
            neglectionlimit = 0.001;
            neglect = false;
            minpow = -3;
            maxpow = 3;
            errpow = 1;
            wperc = 0.3;
            expon = Exp;
            for (int i = 0; i < inputs.Count; i++)
            {
                binputs.Add(inputs[i]);
                boutputs.Add(outputs[i]);
            }
            for (int i = 0; i < terms; i++)
            {
                int leng = inputs[0].Length + 1;
                if (expon)
                {
                    leng = inputs[0].Length*2 + 1;
                }
                double[] r = new double[leng];
                Random ro = new Random();
                for (int rr = 0; rr < r.Length; rr++)
                {
                    r[rr] = Convert.ToDouble(ro.Next(6000, 14000))*(point+0.05) / 10000 ;
                }
                double[,] sides2 = new double[leng, 2];
                for (int i3 = 0; i3 < leng; i3++)
                {
                    sides2[i3, 0] = r[i3] - 0.01;
                    sides2[i3, 1] = r[i3] + 0.01;
                }
                sides.Add(sides2);
                equation.Add(r);
                Random g = new Random();
                Thread.Sleep(g.Next(1, 4));
            }
         //   h = Matrices.MatrixCreate(binputs.Count, binputs[0].Length);
           // g = new double[boutputs.Count];

    //        for (int i = 0; i < h.Length; i++)
      //      {
             //   h[i] = binputs[i];
               // g[i] = boutputs[i];

//            }
  //          Array.Sort(g, h);
        }
        double powtr;
        int pos;
        int pos2;
        int idofinput;
        private void refreshfast()
        {
            if (pos < 0)
            {
                return;
            }

            //  double ans = 0;
            for (int i2 = 0; i2 < ti1.Count; i2++)
            {
                var input = ti1[i2];
                List<double> fr = new List<double>();
                //  int i = 0;
                //  foreach (double[] c in equation)

                // double ans2 = 1;
                // int i= pos2;
                //    double g = 1;
                // if (pos > -1 )
                // {
                double g = 1;// Math.Pow(input[pos], powtr);
                if (pos > -1 )
                {
                    //    g = Math.Pow(input[pos], powtr);
                    if (expon && (pos + 1) % 2 == 0)
                    {
                        g *= Math.Pow(Math.Pow(powf, input[(pos + 1) / 2 - 1]), powtr);
                    }
                    else if (expon)
                    {
                        g *= Math.Pow(input[(pos) / 2], powtr);
                    }
                    else
                    {
                        g *= Math.Pow(input[pos], powtr);
                    }
                }
                // }
                //double ans2 =  fasts[idofinput][i] * g;
                //ans += ans2;
                fasts[i2][pos2]*= g; 
                   // i++;
                
                
            }
        }
        public double calc(List<double[]> equationtemp, double[] input,bool mod=false)
        {
            double ans = 0;
            if (mod)
            {
                int i = 0;
                foreach (double[] c in equationtemp)
                {
                    var g = 1.0;
                    if (pos > -1 && pos2==i)
                    {
                    //    g = Math.Pow(input[pos], powtr);
                        if (expon && (pos+1) % 2 == 0)
                        {
                            g *= Math.Pow(Math.Pow(powf, input[(pos+1) / 2 - 1]), powtr);
                        }
                        else if (expon)
                        {
                            g *= Math.Pow(input[(pos ) / 2], powtr);
                        }
                        else
                        {
                            g *= Math.Pow(input[pos ], powtr);
                        }
                    }
                    double ans2 = c[0]*fasts[idofinput][i]*g;
                    ans += ans2;
                    i++;
                }
                return ans;
            }
            ans = 0;
            foreach (double[] c in equationtemp)
            {
                double ans2 = 1;
                for (int i = 0; i < c.Length; i++)
                {
                    if (i == 0)
                    {
                        ans2 *= c[i];
                    }
                    else
                    {
                        if (expon && i % 2 == 0)
                        {
                            ans2 *= Math.Pow(Math.Pow(powf,input[i/2 - 1]), c[i]);
                        }
                        else if (expon)
                        {
                            ans2 *= Math.Pow(input[(i - 1)/2], c[i]);
                        }
                        else
                        {
                            ans2 *= Math.Pow(input[i - 1], c[i]);
                        }
                    }
                }
                ans += ans2;
            }
            return ans;
        }
        public void shuffler()
        {
            inputs = new List<double[]>();
            outputs = new List<double>();
            List<int> arranges = new List<int>();
            for (int i = 0; i < binputs.Count; i++)
            {
                arranges.Add(i);
            }
            Random r = new Random();
            for (int y = 0; y < shufflelen; y++)
            {
                if (arranges.Count == 0)
                {
                    break;
                }
                var x = r.Next(0, arranges.Count);
                int q = arranges[x];
                inputs.Add(binputs[q]);
                outputs.Add(boutputs[q]);
                arranges.RemoveAt(x);
            }
        }
        public void shufflebyworest()
        {
         //   inputs = new List<double[]>();
           // outputs = new List<double>();
            List<double[]> newlists = new List<double[]>();
            int indx = 0;
            foreach(var u in binputs)
            {
                double[] ninput = new double[u.Length + 2];
                for (int i = 0; i < u.Length+2; i++)
                {
                    if (i < u.Length)
                    {
                        ninput[i] = u[i];
                    }
                    else if (i<u.Length+1)
                    {
                        ninput[i] = boutputs[indx];
                    }
                    else
                    {
                        var ans = calc(equation, u);
                        ninput[i]= Math.Abs((ans - boutputs[indx]) / boutputs[indx]);
                    }
                }
                newlists.Add(ninput);
                indx++;
            }
            int i2 = 0;
            foreach (var k in sortworest(newlists))
            {
                if (i2 < shufflelen*wperc)
                {
                    double[] nk = new double[k.Length - 2];
                    for (int io = 0; io < k.Length-2; io++)
                    {
                        nk[io] = k[io];
                    }
                    inputs.Add(nk);
                    outputs.Add(k[k.Length - 2]);
                }
                else
                {
                    break;
                }
                i2++;
            }

        }
        public double getacc()
        {
            accuracy = accCalc(equation, binputs, boutputs, par: 1);
            return accuracy;
        }
        List<List<double>> fasts;
        public double accCalc(List<double[]> equationtemp, List<double[]> Inputs, List<double> Outputs, double par = 0.5, bool mody = false, bool setfast = false)
        {
            double acc = 0;
            if (setfast)
            {
                fasts = new List<List<double>>();
            }
            for (int i2 = 0; i2 < Inputs.Count; i2++)
            {
                idofinput = i2;
                acc += 1 - Math.Pow(Math.Abs((Outputs[i2] - calc(equationtemp, Inputs[i2],mod:mody)) / Outputs[i2]), par);
                if (setfast)
                {

                  //  double ans = 0;
                    var input = Inputs[i2];
                    List<double> fr = new List<double>();
                    foreach (double[] c in equationtemp)
                    {
                        double ans2 = 1;
                        for (int i = 0; i < c.Length; i++)
                        {
                            if (i == 0)
                            {
                               // ans2 *= c[i];
                            }
                            else
                            {
                                if (expon && i % 2 == 0)
                                {
                                    ans2 *= Math.Pow(Math.Pow(powf, input[i / 2 - 1]), c[i]);
                                }
                                else if (expon)
                                {
                                    ans2 *= Math.Pow(input[(i - 1) / 2], c[i]);
                                }
                                else
                                {
                                    ans2 *= Math.Pow(input[i - 1], c[i]);
                                }
                            }
                        }
                        //ans += ans2;
                        fr.Add(ans2);
                    }
                    fasts.Add(fr);
                }
            }
            acc /= Inputs.Count;
            return acc;
        }
        public List<double[]> copier()
        {
            List<double[]> cn = new List<double[]>();
            for (int i = 0; i < equation.Count; i++)
            {
                double[] ro = new double[equation[i].Length];

                for (int ii = 0; ii < equation[i].Length; ii++)
                {
                    ro[ii] = equation[i][ii] + 0;
                }
                cn.Add(ro);
            }
            return cn;
        }
        public List<double[,]> sidecopy()
        {
            List<double[,]> sides3 = new List<double[,]>();
            double[,] sides2 = new double[equation[0].Length, 2];
            for (int i = 0; i < Terms; i++)
            {
                for (int i3 = 0; i3 < equation[0].Length; i3++)
                {
                    sides2[i3, 0] = sides[i][i3, 0] + 0;
                    sides2[i3, 1] = sides[i][i3, 1] + 0;
                }
                sides3.Add(sides2);
            }
            return sides3;
        }
        bool crtb;
        public double pointeval(double side, double avg, int i1, int i2, double accm,bool norm=false)
        {
            double crt = (side + avg) / 2;
            List<double[]> l1;
            List<double[]> l2;
            List<double[]> l3;
            pos = i2 - 1;
            pos2 = i1;
            crtb = false;
            var l = copier();
            //var accr = accCalc(l, ti, to, par: 1);
            l[i1][i2] = crt;
            powtr = crt-avg;
            double acc1 = 0;
            if (!norm)
            {
                modifier(l);
               acc1= accCalc(l, ti1, to1, par: errpow,mody:true);
          //     acc1 = accCalc(l, ti1, to1, par: errpow);
            }
            else
            {
                acc1 = accCalc(l, binputs, boutputs, par: errpow);
            }
            l1 = l;
            l = copier();
            l[i1][i2] = side;
            powtr = side - avg;
            var acc2 = 0.0;
            if (!norm)
            {
                modifier(l);
                acc2 = accCalc(l, ti1, to1, par: errpow,mody:true);
            //    acc2 = accCalc(l, ti1, to1, par: errpow);
            }
            else
            {
                acc2 = accCalc(l, binputs, boutputs, par: errpow);
            }
            // shuffler();
            l2 = l;
            l = equation;
            //l[i1][i2] = avg;

            //modifier(l);
            //shuffler();
            powtr = 0;
            l3 = l;
            var acc3 = accm;// accCalc(l, ti1, to1, par: 1);
            for (int i = 0; i < equation.Count; i++)
            {
                //   equation[i][0] = l[i][0]+0;
            }
            //var acc4 = accCalc(l, binputs, boutputs, par: 1);
            //l = copier();
            //modifier(l);
            //myreturn = -1* (Math.Abs(acc1 - acc2)+ Math.Abs(acc1 - acc3))  ;
            var myacc = acc1;
            expandd = true;
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        if (acc1 >= myacc)
                        {
                            centerpoint = crt;
                            myacc = acc1;
                            expandd = false;
                            crtb = true;
                            bestl = l1;
                        }
                        break;
                    case 1:
                        if (acc2 >= myacc)
                        {
                            myacc = acc2;
                            centerpoint = side;
                            expandd = true;
                            bestl = l2;
                            crtb = false;
                        }
                        break;
                    case 2:
                        if (acc3 >= myacc)
                        {
                            myacc = acc3;
                            centerpoint = avg;
                            expandd = false;
                            bestl = l3;
                            crtb = false;
                        }
                        break;
                }
            }
            bestpoint = myacc;
            return acc1;


        }
        List<double[]> bestl1;
        List<double[]> bestl2;
        List<double[]> bestl;
        public double[] expand(double s1, double s2)
        {
            double ss1 = 0;
            double ss2 = 0;
            if ((s1 > 0) == (s2 > 0))
            {
                if (s1 > s2)
                {
                    ss1 = s1 * 2;
                    ss2 = s2 / 2 - ss1 / 2;
                }
                else
                {
                    ss2 = s2 * 2;
                    ss1 = s1 / 2 - ss2 / 2;
                }
            }
            else
            {
                ss1 = s1 * 2;
                ss2 = s2 * 2;
            }
            double[] r = new double[2];
            r[0] = ss1;
            r[1] = ss2;
            return r;
        }
        public void modifier(List<double[]> l)
        {
            double[][] mymatrix = Matrices.MatrixCreate(Terms, Terms);
            double[][] mymatrix2 = Matrices.MatrixCreate(Terms, 1);
            int yy = shufflelen + 0;
            shufflelen = Terms;
            //shuffler();
            for (int z1 = 0; z1 < Terms; z1++)
            {
                double ans = 0;
                var z2 = 0;
                foreach (double[] c in l)
                {
                    double ans2 = 1;
                    for (int i = 0; i < c.Length; i++)
                    {
                        if (i == 0)
                        {
                            ans2 *= c[i];
                        }
                        else if(expon && i%2==0)
                        {
                            ans2 *= Math.Pow(Math.Pow(powf,inputs[z1][(i-2) / 2]), c[i]);
                        }
                        else if (expon)
                        {
                            ans2 *= Math.Pow(inputs[z1][(i - 1)/2], c[i]);
                        }
                        else
                        {
                            ans2 *= Math.Pow(inputs[z1][i - 1], c[i]);
                        }
                    }
                    ans += ans2;
                    mymatrix[z1][z2] = ans2;
                    z2++;
                }
                mymatrix2[z1][0] = outputs[z1];
            }
            try
            {
                shufflelen = yy;
                //   shuffler();
                double[][] newterms = Matrices.MatrixProduct(Matrices.MatrixInverse(mymatrix), mymatrix2);
                //  bool gr;
                bool exit = true;
                for (int i = 0; i < Terms; i++)
                {
                    if (!(l[i][0] * newterms[i][0] < termlimit && l[i][0] * newterms[i][0] > -1 * termlimit))
                    {
                        exit = false;
                        break;
                        //        gr = false;
                        //Exception ex = new Exception();
                        // throw ex; 
                        //  break;
                    }
                }
                for (int i = 0; i < Terms; i++)
                {
                    var f = newterms[i][0].ToString();
                    //if ((f == "NaN" && Nano) || exit)
                    //{

                    //}
                    if (!(f == "NaN" || f == "Infinity" || f == "-Infinity" || newterms[i][0] == 0) && exit)
                    {
                        var q = f == "NaN";
                        l[i][0] *= newterms[i][0];
                        if (bracks)
                        {
                            if (Math.Abs(l[i][0]) < brackelimit)
                            {
                                double chh = (l[i][0] > 0) ? 1 : -1;
                                l[i][0] += (Math.Abs(l[i][0]) + brackelimit);
                                l[i][0] *= chh;
                            }
                        }
                    }
                    else
                    {
                        Nano = false;
                        break;
                    }
                }

            }
            catch { Nano = false; }

        }
        double[][] h;
        double[] g;
        void shuffler3()
        {
            ti1 = new List<double[]>();
            to1 = new List<double>();
            inputs = new List<double[]>();
            outputs = new List<double>();
            Random rf = new Random();

            var fq = rf.Next(0, 2);

            if (fq == 0)
            {
                Array.Reverse(g);
                Array.Reverse(h);
            }
            int r = binputs.Count / (shufflelen + rf.Next(0, 5));

            for (int i = 0; i < h.Length; i += r)
            {
                ti1.Add(h[i]);
                to1.Add(g[i]);

            }

            r = binputs.Count / (equation.Count + rf.Next(0, 5));
            for (int i = 0; i < h.Length; i += r)
            {
                inputs.Add(h[i]);
                outputs.Add(g[i]);

            }
        }
        bool Nano;
        List<double[]> ti;
        List<double> to;
        List<double[]> ti1;
        List<double> to1;
        double bestpoint1;
        double bestpoint2;
        double bestpoint;
        public double minpow;
        public double maxpow;
        List<double[]> tii1;
        List<double> too1;
        public int arnge;
        public double wperc;
        public void Train()
        {
            Random rrq = new Random();

            // {
            // tii1 = inputs;
            //too1 = outputs;
            //shufflelen /= 2;
            shuffler();
            if (rrq.Next(0, 2)+1== 0)
            { shufflebyworest(); }
            // }
            ti1 = inputs;
            to1 = outputs;
            if (rrq.Next(0, 2) + arnge+1 == 0)
            {
                shuffler3();
            }
            if (sadd == 1)
            {
                ti1 = tii1;
                to1 = too1;
            }
            else
            {
                tii1 = ti1;
                too1 = to1;
            }
            //}
            var lq = copier();
            Nano = true;
            mainacc = accCalc(equation, binputs, boutputs, par: errpow);
            modifier(equation);

            var ecc = accCalc(equation, binputs, boutputs, par: errpow);
            var lenq = 7;// equation.Count;
            if (equation.Count < lenq)
            {
               // lenq = equation.Count;
            }
            int refg = rd.Next(0, lenq) + sadd;

            if ((ecc > mainacc || refg == 0) && Nano)
            {
                ti = inputs;
                to = outputs;
                mainacc = ecc;
            }
            else
            {
                inputs = ti;
                outputs = to;
                //   modifier(equation);
                equation = lq;
            }

            mainacc = accCalc(equation, ti1, to1, par: errpow,setfast:true);
            for (int i = 0; i < equation.Count; i++)
            {

                for (int i2 = 0; i2 < equation[0].Length; i2++)
                {
                    int qe = 5;
                    while (qe != 0)
                    {
                        //           shufflelen = rrq.Next(10, 30);


                        if (neglect &&( Math.Abs(equation[i][i2]) <= neglectionlimit || (i2 == 0 && equation.Count>2) || (i2!=0 && i2%2==0 && expon && Math.Abs( Math.Pow(powf, equation[i][i2])-1)<=neglectionlimit )))
                        {
                            if (i2 != 0)
                            {
                                double side11 = sides[i][i2, 0];
                                double side22 = sides[i][i2, 1];
                                var avg2 = (side11 + side22) / 2;
                                double expand11 = side11 - avg2;
                                equation[i][i2] = 0.00000001;
                                sides[i][i2, 0] = 0.00000001 + expand11;
                                sides[i][i2, 1] = 0.00000001 - expand11;
                            }
                            break;

                        }
                        Nano = false;
                        double side1 = sides[i][i2, 0];
                        double side2 = sides[i][i2, 1];
                        var avg = (side1 + side2) / 2;
                        double g = 0;
                        double g2 = 0;
                        g = pointeval(side1, avg, i, i2, mainacc);
                        bestl1 = bestl;
                        bestpoint1 = bestpoint;
                        var c1 = centerpoint;
                        bool ex1 = expandd;
                        bool crtb1 = crtb;
                        g2 = pointeval(side2, avg, i, i2, mainacc);
                        bestl2 = bestl;
                        bestpoint2 = bestpoint;
                        var c2 = centerpoint;
                        double expand1 = side1 - avg;

                        if (Math.Abs(expand1) > 2)
                        {
                            expand1 = 2;
                        }
                        bool qr = false;
                        double r0 = 0;
                        if (g > g2)
                        {
                            if (g < mainacc)
                            {
                                qr = true;
                                r0 = g;
                            }
                        }
                        else
                        {
                            if (g2 < mainacc)
                            {
                                qr = true;
                                r0 = g2;
                            }
                        }
                        if (qr)
                        {
                            if ((mainacc - r0) > 0.01 * Math.Abs(r0))
                            {
                                //accCalc(equation, binputs, boutputs, par: 1);
                                // g = pointeval(side1, avg, i, i2);
                            }
                        }
                        if (g > g2 && (c1 > minpow && c1 < maxpow || (i2==0 && Math.Abs(c1)<termlimit )))
                        {
                            var few = Math.Abs(c1);
                            if (!ex1)
                            {
                                if (!crtb1)
                                {
                                    expand1 /= 2;
                                }
                            }
                            else
                            {
                                expand1 *= 2;
                            }
                            if (Math.Abs(equation[i][0]) < 0.01)
                            {
                                expand1 *= 1.5;
                            }
                            if (Math.Abs(expand1) < expandlimit)
                            {
                                expand1 = Math.Abs(expand1) + expandlimit;//Math.Abs(expand1) + 
                            }
                            else if (Math.Abs(expand1) > 1)
                            {
                                expand1 /= 2;
                            }
                            sides[i][i2, 0] = c1 + expand1;
                            sides[i][i2, 1] = c1 - expand1;
                            powtr = c1 - equation[i][i2];
                            pos = i2 - 1;
                            pos2 = i;
                            equation = bestl1;//[i][i2] = c1+0.000000001;
                            refreshfast();
                            // mainacc = accCalc(equation, binputs, boutputs, par: 1);
                            mainacc = bestpoint1;
                        }
                        else if (c2 > minpow && c2 < maxpow || (i2==0 && Math.Abs(c2) < termlimit))
                        {
                            var few = Math.Abs(c2);
                            if (!expandd)
                            {
                                if (!crtb)
                                {
                                    expand1 /= 2;
                                }
                            }
                            else
                            {
                                expand1 *= 2;
                            }
                            if (Math.Abs(equation[i][0]) < 0.01)
                            {
                                expand1 *= 1.4;
                            }
                            if (Math.Abs(expand1) < expandlimit)
                            {
                                expand1 = Math.Abs(expand1) + expandlimit;
                            }
                            else if (Math.Abs(expand1) > 1)
                            {
                                expand1 /= 2;
                            }
                            sides[i][i2, 0] = c2 + expand1;
                            sides[i][i2, 1] = c2 - expand1;
                            // c2 + 0.000000001;
                            powtr =c2-equation[i][i2];
                            pos = i2 - 1;
                            pos2 = i;
                            equation = bestl2;
                            refreshfast();
                                              //mainacc = accCalc(equation, binputs, boutputs, par: 1);
                            mainacc = bestpoint2;
                        }
                        // modifier(equation);
                        break;


                    }
                }
            }
         //   trainnormal();
        }
        private void trainnormal()
        {
                mainacc = accCalc(equation, binputs, boutputs, par: errpow);
                for (int i = 0; i < equation.Count; i++)
                {

                    for (int i2 = 0; i2 < 1; i2++)
                    {
                        int qe = 5;
                        while (qe != 0)
                        {
                            //           shufflelen = rrq.Next(10, 30);
                            if (true)
                        {
                            double side11 = sides[i][i2, 0];
                            double side22 = sides[i][i2, 1];
                            var avg2 = (side11 + side22) / 2;
                            double expand11 = side11 - avg2;
                           // equation[i][i2] = 0.00000001;
                            sides[i][i2, 0] = equation[i][i2] + expand11;
                            sides[i][i2, 1] = equation[i][i2] - expand11;
                        }

                            Nano = false;
                            double side1 = sides[i][i2, 0];
                            double side2 = sides[i][i2, 1];
                            var avg = (side1 + side2) / 2;
                            double g = 0;
                            double g2 = 0;
                            g = pointeval(side1, avg, i, i2, mainacc,norm:true);
                            bestl1 = bestl;
                            bestpoint1 = bestpoint;
                            var c1 = centerpoint;
                            bool ex1 = expandd;
                            bool crtb1 = crtb;
                            g2 = pointeval(side2, avg, i, i2, mainacc,norm:true);
                            bestl2 = bestl;
                            bestpoint2 = bestpoint;
                            var c2 = centerpoint;
                            double expand1 = side1 - avg;

                            if (Math.Abs(expand1) > 2)
                            {
                                expand1 = 2;
                            }
                            bool qr = false;
                            double r0 = 0;
                            if (g > g2)
                            {
                                if (g < mainacc)
                                {
                                    qr = true;
                                    r0 = g;
                                }
                            }
                            else
                            {
                                if (g2 < mainacc)
                                {
                                    qr = true;
                                    r0 = g2;
                                }
                            }
                            if (qr)
                            {
                                if ((mainacc - r0) > 0.01 * Math.Abs(r0))
                                {
                                    //accCalc(equation, binputs, boutputs, par: 1);
                                    // g = pointeval(side1, avg, i, i2);
                                }
                            }
                            if (g > g2 && (i2 == 0 && Math.Abs(c1) < termlimit))
                            {
                                var few = Math.Abs(c1);
                                if (!ex1)
                                {
                                    if (!crtb1)
                                    {
                                        expand1 /= 2;
                                    }
                                }
                                else
                                {
                                    expand1 *= 2;
                                }
                                if (Math.Abs(equation[i][0]) < 0.01)
                                {
                                    expand1 *= 1.5;
                                }
                                if (Math.Abs(expand1) < expandlimit)
                                {
                                    expand1 = Math.Abs(expand1) + expandlimit;//Math.Abs(expand1) + 
                                }
                                else if (Math.Abs(expand1) > 1)
                                {
                                    expand1 /= 2;
                                }
                                sides[i][i2, 0] = c1 + expand1;
                                sides[i][i2, 1] = c1 - expand1;
                                equation = bestl1;//[i][i2] = c1+0.000000001;
                                                  // mainacc = accCalc(equation, binputs, boutputs, par: 1);
                                mainacc = bestpoint1;
                            }
                            else if (i2 == 0 && Math.Abs(c2) < termlimit)
                            {
                                var few = Math.Abs(c2);
                                if (!expandd)
                                {
                                    if (!crtb)
                                    {
                                        expand1 /= 2;
                                    }
                                }
                                else
                                {
                                    expand1 *= 2;
                                }
                                if (Math.Abs(equation[i][0]) < 0.01)
                                {
                                    expand1 *= 1.4;
                                }
                                if (Math.Abs(expand1) < expandlimit)
                                {
                                    expand1 = Math.Abs(expand1) + expandlimit;
                                }
                                else if (Math.Abs(expand1) > 1)
                                {
                                    expand1 /= 2;
                                }
                                sides[i][i2, 0] = c2 + expand1;
                                sides[i][i2, 1] = c2 - expand1;
                                equation = bestl2;// c2 + 0.000000001;
                                                  //mainacc = accCalc(equation, binputs, boutputs, par: 1);
                                mainacc = bestpoint2;
                            }
                            // modifier(equation);
                            break;


                        }
                    }
                }
            }
            public void saveme(string datapath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream writerFileStream =
                 new FileStream(datapath, FileMode.Create, FileAccess.Write);
            // Save our dictionary of friends to file
            formatter.Serialize(writerFileStream, this);
            // Close the writerFileStream when we are done.
            writerFileStream.Close();

        }

        public static regressor load(string datapath)
        {
            regressor tc;
            FileStream readerFileStream = new FileStream(datapath,
    FileMode.Open, FileAccess.Read);
            BinaryFormatter formatter = new BinaryFormatter();
            // Reconstruct information of our friends from file.
            tc = (regressor)formatter.Deserialize(readerFileStream);
            // Close the readerFileStream when we are done
            readerFileStream.Close();
            return tc;
        }
        static IEnumerable<double[]> sortworest(IEnumerable<double[]> e)
        {
            // Use LINQ to sort the array received and return a copy.
            var sorted = from s in e
                         orderby s[s.Length - 1] descending
                         select s;
            return sorted;
        }
    }
    [Serializable]
    class project
    {
        public string name;
        public double stop;
        public int numofthreads;
        public int maxsteps;
        public List<regressor> regressors;
        public List<regressor> liveregs;
        public List<regressor> bests;
       // public List<Thread> allthreads;
        public List<double[]> threadsdata;
        public List<double> sentdata;
        public List<double[]> inputs;
        public  List<double> outputs;
        public string[] columns;

        public project(string projectname,double stop2, int numofthreads2, int maxsteps2, List<regressor> regressors2, List<regressor> liveregs2, List<regressor> bests2, List<double[]> threadsdata2, List<double> sentdata2, List<double[]> inputs2,List<double> outputs2,string[] columns2)
        {
            name = projectname;
            stop = stop2;
            numofthreads = numofthreads2;
            maxsteps = maxsteps2;
            regressors = regressors2;
            liveregs = liveregs2;
            bests = bests2;
          //  allthreads = allthreads2;
            threadsdata = threadsdata2;
            sentdata = sentdata2;
            inputs = inputs2;
            outputs = outputs2;
            columns = columns2;
        }

        public void saveme(string datapath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream writerFileStream =
                 new FileStream(datapath, FileMode.Create, FileAccess.Write);
            // Save our dictionary of friends to file
            formatter.Serialize(writerFileStream, this);
            // Close the writerFileStream when we are done.
            writerFileStream.Close();

        }

        public static project load(string datapath)
        {
            project tc;
            FileStream readerFileStream = new FileStream(datapath,
    FileMode.Open, FileAccess.Read);
            BinaryFormatter formatter = new BinaryFormatter();
            // Reconstruct information of our friends from file.
            tc = (project)formatter.Deserialize(readerFileStream);
            // Close the readerFileStream when we are done
            readerFileStream.Close();
            return tc;
        }

    }
    class Matrices
    {

        public static double[][] MatrixCreate(int rows, int cols)
        {
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new double[cols];
            return result;
        }

        public static double[][] MatrixIdentity(int n)
        {
            // return an n x n Identity matrix
            double[][] result = MatrixCreate(n, n);
            for (int i = 0; i < n; ++i)
                result[i][i] = 1.0;

            return result;
        }

        public static double[][] MatrixProduct(double[][] matrixA, double[][] matrixB)
        {
            int aRows = matrixA.Length; int aCols = matrixA[0].Length;
            int bRows = matrixB.Length; int bCols = matrixB[0].Length;
            if (aCols != bRows)
                throw new Exception("Non-conformable matrices in MatrixProduct");

            double[][] result = MatrixCreate(aRows, bCols);

            for (int i = 0; i < aRows; ++i) // each row of A
                for (int j = 0; j < bCols; ++j) // each col of B
                    for (int k = 0; k < aCols; ++k) // could use k less-than bRows
                        result[i][j] += matrixA[i][k] * matrixB[k][j];

            return result;
        }

        public static double[][] MatrixInverse(double[][] matrix)
        {
            int n = matrix.Length;
            double[][] result = MatrixDuplicate(matrix);

            int[] perm;
            int toggle;
            double[][] lum = MatrixDecompose(matrix, out perm,
              out toggle);
            if (lum == null)
                throw new Exception("Unable to compute inverse");

            double[] b = new double[n];
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (i == perm[j])
                        b[j] = 1.0;
                    else
                        b[j] = 0.0;
                }

                double[] x = HelperSolve(lum, b);

                for (int j = 0; j < n; ++j)
                    result[j][i] = x[j];
            }
            return result;
        }

        static double[][] MatrixDuplicate(double[][] matrix)
        {
            // allocates/creates a duplicate of a matrix.
            double[][] result = MatrixCreate(matrix.Length, matrix[0].Length);
            for (int i = 0; i < matrix.Length; ++i) // copy the values
                for (int j = 0; j < matrix[i].Length; ++j)
                    result[i][j] = matrix[i][j];
            return result;
        }

        static double[] HelperSolve(double[][] luMatrix, double[] b)
        {
            // before calling this helper, permute b using the perm array
            // from MatrixDecompose that generated luMatrix
            int n = luMatrix.Length;
            double[] x = new double[n];
            b.CopyTo(x, 0);

            for (int i = 1; i < n; ++i)
            {
                double sum = x[i];
                for (int j = 0; j < i; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum;
            }

            x[n - 1] /= luMatrix[n - 1][n - 1];
            for (int i = n - 2; i >= 0; --i)
            {
                double sum = x[i];
                for (int j = i + 1; j < n; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum / luMatrix[i][i];
            }

            return x;
        }

        static double[][] MatrixDecompose(double[][] matrix, out int[] perm, out int toggle)
        {
            // Doolittle LUP decomposition with partial pivoting.
            // rerturns: result is L (with 1s on diagonal) and U;
            // perm holds row permutations; toggle is +1 or -1 (even or odd)
            int rows = matrix.Length;
            int cols = matrix[0].Length; // assume square
            if (rows != cols)
                throw new Exception("Attempt to decompose a non-square m");

            int n = rows; // convenience

            double[][] result = MatrixDuplicate(matrix);

            perm = new int[n]; // set up row permutation result
            for (int i = 0; i < n; ++i) { perm[i] = i; }

            toggle = 1; // toggle tracks row swaps.
                        // +1 -greater-than even, -1 -greater-than odd. used by MatrixDeterminant

            for (int j = 0; j < n - 1; ++j) // each column
            {
                double colMax = Math.Abs(result[j][j]); // find largest val in col
                int pRow = j;
                //for (int i = j + 1; i less-than n; ++i)
                //{
                //  if (result[i][j] greater-than colMax)
                //  {
                //    colMax = result[i][j];
                //    pRow = i;
                //  }
                //}

                // reader Matt V needed this:
                for (int i = j + 1; i < n; ++i)
                {
                    if (Math.Abs(result[i][j]) > colMax)
                    {
                        colMax = Math.Abs(result[i][j]);
                        pRow = i;
                    }
                }
                // Not sure if this approach is needed always, or not.

                if (pRow != j) // if largest value not on pivot, swap rows
                {
                    double[] rowPtr = result[pRow];
                    result[pRow] = result[j];
                    result[j] = rowPtr;

                    int tmp = perm[pRow]; // and swap perm info
                    perm[pRow] = perm[j];
                    perm[j] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle
                }

                // --------------------------------------------------
                // This part added later (not in original)
                // and replaces the 'return null' below.
                // if there is a 0 on the diagonal, find a good row
                // from i = j+1 down that doesn't have
                // a 0 in column j, and swap that good row with row j
                // --------------------------------------------------

                if (result[j][j] == 0.0)
                {
                    // find a good row to swap
                    int goodRow = -1;
                    for (int row = j + 1; row < n; ++row)
                    {
                        if (result[row][j] != 0.0)
                            goodRow = row;
                    }

                    if (goodRow == -1)
                        throw new Exception("Cannot use Doolittle's method");

                    // swap rows so 0.0 no longer on diagonal
                    double[] rowPtr = result[goodRow];
                    result[goodRow] = result[j];
                    result[j] = rowPtr;

                    int tmp = perm[goodRow]; // and swap perm info
                    perm[goodRow] = perm[j];
                    perm[j] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle
                }
                // --------------------------------------------------
                // if diagonal after swap is zero . .
                //if (Math.Abs(result[j][j]) less-than 1.0E-20) 
                //  return null; // consider a throw

                for (int i = j + 1; i < n; ++i)
                {
                    result[i][j] /= result[j][j];
                    for (int k = j + 1; k < n; ++k)
                    {
                        result[i][k] -= result[i][j] * result[j][k];
                    }
                }


            } // main j column loop

            return result;
        }




    }
    class evolutionpool
    {
        regressor reg; List<double[]> inputs; List<double> outputs;
        public int maxmutats;
        public int copies;
        public int best;
        public List<regressor> regls;
        public evolutionpool(regressor regg, List<double[]> inputsg, List<double> outputsg)
        {
            copies = 10;
            best = 500;
            reg = regg;
            inputs = inputsg;
            outputs = outputsg;
            maxmutats = 50;
            regls = new List<regressor>();

            for (int i = 0; i < (best + 2) * copies + best; i++)
            {
                var fg = regg.copy();
                Random r = new Random();
                if (i > 0)
                {
                    fg.mutatme(r.Next(1, maxmutats));
                }
                regls.Add(fg);
            }
        }
        public void run()
        {
            calc();
            mutat();
        }
        public void runsteps(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                run();
            }
        }
        public void calc()
        {
            foreach (var c in regls)
            {
                c.getacc();
            }
            int i = 0;
            foreach (var r in SortByLength(regls))
            {
                regls[i] = r;
                i++;
            }
        }
        public void mutat()
        {
            for (int i = 0; i < best; i++)
            {
                regls[i].shuffler();
                for (int i2 = best + copies * (i); i2 < best + copies * (i + 1); i2++)
                {
                    Random r = new Random();
                    var rr = regls[i].copy();

                    rr.mutatme(r.Next(1, maxmutats));
                    regls[i2] = rr;
                }
            }
        }
        static IEnumerable<regressor> SortByLength(IEnumerable<regressor> e)
        {
            // Use LINQ to sort the array received and return a copy.
            var sorted = from s in e
                         orderby s.accuracy descending
                         select s;
            return sorted;
        }
    }
}

