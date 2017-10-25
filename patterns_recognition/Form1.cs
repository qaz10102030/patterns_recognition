using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace patterns_recognition
{
    public partial class Form1 : Form
    {
        public Random rnd = new Random(Guid.NewGuid().GetHashCode());
        public int[] count = new int[] { 0, 0 };
        private readonly Color[] _colors = new Color[] { Color.DodgerBlue, Color.Red };

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();
            double[] prob = new double[2];
            int n = 0, d = 0;
            if (textBox1.Text != "" && IsNumeric(textBox1.Text) && 
                textBox2.Text != "" && IsNumeric(textBox2.Text) && 
                textBox3.Text != "" && IsNumeric(textBox3.Text) && 
                textBox4.Text != "" && IsNumeric(textBox4.Text) &&
                textBox5.Text != "" && IsNumeric(textBox5.Text) &&
                textBox6.Text != "" && IsNumeric(textBox6.Text))
            {
                n = short.Parse(textBox1.Text);
                d = short.Parse(textBox2.Text);
                prob[0] = double.Parse(textBox3.Text);
                prob[1] = double.Parse(textBox4.Text);
                int[] result = new int[d + 1];
                int[] result2 = new int[d + 1];
                double[] normal = new double[d + 1];
                double[] normal2 = new double[d + 1];

                test(d , n, prob,result,result2);
                draw(result, "prob1=" + prob[0], d, n, _colors[0]);
                draw(result2, "prob2=" + prob[1], d, n, _colors[1]);
                label5.Text = "bernoP1H = " + bernoulli(d,int.Parse(textBox5.Text) , prob[0]) + "";
                label6.Text = "bernoP2H = " + bernoulli(d, int.Parse(textBox6.Text), prob[1]) + "";

                normal = calcGauss(d, n, result);
                drawGauss(normal, "高斯1", d, n, Color.ForestGreen);
                normal2 = calcGauss(d, n, result2);
                drawGauss(normal2, "高斯2", d, n, Color.Yellow);
                label7.Text = "gaussianP1H = " + normal[int.Parse(textBox5.Text)];
                label8.Text = "gaussianP2H = " + normal2[int.Parse(textBox6.Text)];

                label9.Text = "bernoulli : " +
                    (bernoulli(d, int.Parse(textBox5.Text), prob[0]) > bernoulli(d, int.Parse(textBox6.Text), prob[1]) ? "P1H > P2H" : bernoulli(d, int.Parse(textBox5.Text), prob[0]) < bernoulli(d, int.Parse(textBox6.Text), prob[1]) ? "P1H < P2H" : "P1H = P2H") + " , " +
                    "gaussian : " + 
                    (normal[int.Parse(textBox5.Text)] > normal2[int.Parse(textBox6.Text)] ? "P1H > P2H" : normal[int.Parse(textBox5.Text)] < normal2[int.Parse(textBox6.Text)] ? "P1H < P2H" : "P1H = P2H");
            }
        }

        public double[] calcGauss(int d,int n,int[] array)
        {
            double calcMean = 0.0;
            double calcVar = 0.0;
            double squareMean = 0.0;
            for (int i = 0; i <= d; i++)
            {
                calcMean += i * (array[i] / (double)n);
            }
            for (int i = 0; i <= d; i++)
            {
                squareMean += Math.Pow(i,2) * (array[i] / (double)n);
            }
            calcVar = squareMean - Math.Pow(calcMean, 2);
            double[] result = new double[d + 1];
            for (int i = 0; i <= d; i++)
            {
                result[i] = (1 / (Math.Sqrt(2 * Math.PI) * Math.Sqrt(calcVar))) * 
                    Math.Exp((Math.Pow(i - calcMean, 2) / (2 * calcVar)) * (-1));
            }
            return result;
        }

        public void test(int d,int n,double[] prob,int[] result,int[] result2)
        {
            count[0] = 0;
            count[1] = 0;
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= d; j++)
                {
                    double num = rnd.NextDouble();
                    if (num <= prob[0]) count[0]++;
                }
                result[count[0]]++;
                count[0] = 0;
            }
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= d; j++)
                {
                    double num = rnd.NextDouble();
                    if (num <= prob[1]) count[1]++;
                }
                result2[count[1]]++;
                count[1] = 0;
            }
        }

        public void draw(int[] _y, string name, int _length, int time , Color _color)
        {
            if (chart1.Series.Count == 0)
                chart1.Titles.Add("d=" + _length + ",n=" + time);
            Series _series = new Series();
            for (int index = 0; index <= _length; index++)
            {
                _series.Color = _color;
                _series.ChartType = SeriesChartType.Column;
                _series.IsValueShownAsLabel = true;
                 _series.Name = name;
                if (_y[index] != 0)
                {
                    _series.Points.AddXY(index, _y[index]);
                }
            }
            chart1.Series.Add(_series);
        }

        public void drawGauss(double[] _y, string name, int _length, int time, Color _color)
        {
            Series _series = new Series();
            for (int index = 0; index <= _length; index++)
            {
                _series.Color = _color;
                _series.ChartType = SeriesChartType.Spline;
                //_series.IsValueShownAsLabel = true;
                _series.Name = name;
                _series.BorderWidth = 5;
                if (_y[index] * time > 0.5)
                {
                    _series.Points.AddXY(index, _y[index] * time);
                }
            }
            chart1.Series.Add(_series);
        }

        public double bernoulli(int d, int time, double prob)
        {
            double bernoRes = 0.0;
            for (int i = 0; i <= d; i++)
            {
                bernoRes += Math.Pow(prob, time) * Math.Pow(1 - prob, d - time);
            }
                return bernoRes;
        }

        public static bool IsNumeric(string TextBoxValue)
       {
            try{
                int i = Convert.ToInt32(TextBoxValue);
                return true;
            }
            catch{
                try {
                    double i = Convert.ToDouble(TextBoxValue);
                    return true;
                }
                catch {
                    return false;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "100";//n
            textBox2.Text = "10";//d
            textBox3.Text = "0.25";//p1
            textBox4.Text = "0.5";//p2
            textBox5.Text = "5";//berno1
            textBox6.Text = "5";//berno2
        }

    }
}
