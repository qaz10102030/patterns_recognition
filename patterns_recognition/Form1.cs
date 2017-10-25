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
                textBox4.Text != "" && IsNumeric(textBox4.Text))
            {
                n = short.Parse(textBox1.Text);
                d = short.Parse(textBox2.Text);
                prob[0] = double.Parse(textBox3.Text);
                prob[1] = double.Parse(textBox4.Text);
                int[] result = new int[d + 1];
                int[] result2 = new int[d + 1];
                double[] normal = new double[d + 1];

                test(d , n, prob,result,result2);
                draw(result, prob[0], d, n, _colors[0]);
                //draw(result2, prob[1], d, n, _colors[1]);
                double berno1 = bernoulli(d, result, prob[0]);
                double berno2 = bernoulli(d, result2, prob[1]);
                label5.Text = "berno1 = " + berno1;
                label6.Text = "berno2 = " + berno2;

                calcGuss(d,result);
                //drawGuss(normal, "高斯", d, n, Color.ForestGreen);
            }
        }

        public void calcGuss(int d,int[] array)
        {
            double mean = chart1.DataManipulator.Statistics.Mean("prob=0.25");
            double calcMean = 0.0;
            double calcVar = 0.0;
            for (int i = 1; i <= d; i++)
            {
                calcMean += (double)i * (double)((double)array[i] / (double)d);
            }
            for (int i = 1; i <= d; i++)
            {
                calcVar += Math.Pow(i - calcMean, 2);
            }
            double variance = chart1.DataManipulator.Statistics.Variance("prob=0.25", false);
            double[] result = new double[d + 1];
            for (int i = 1; i <= 100; i++)
            {
                result[i] = (1 / (Math.Sqrt(2 * Math.PI) * Math.Sqrt(variance))) * 
                    Math.Exp((Math.Pow(i - mean, 2) / (2 * variance)) * (-1));
            }
            int z = 0;
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

        public void draw(int[] _y, double prob, int _length, int time , Color _color)
        {
            chart1.Titles.Add("d=" + _length + ",n=" + time);
            Series _series = new Series();
            for (int index = 1; index <= _length; index++)
            {
                _series.Color = _color;
                _series.ChartType = SeriesChartType.Column;
                _series.IsValueShownAsLabel = true;
                 _series.Name ="prob=" + prob;
                if (_y[index] != 0)
                {
                    _series.Points.AddXY(index, _y[index]);
                }
            }
            chart1.Series.Add(_series);
        }

        public void drawGuss(double[] _y, String name, int _length, int time, Color _color)
        {
            Series _series = new Series();
            for (int index = 1; index <= _length; index++)
            {
                _series.Color = _color;
                _series.ChartType = SeriesChartType.Line;
                //_series.IsValueShownAsLabel = true;
                _series.Name = name;
                if (_y[index] != 0)
                {
                    _series.Points.AddXY(index, _y[index]*10);
                }
            }
            chart1.Series.Add(_series);
        }

        public double bernoulli(int d, int[] time, double prob)
        {
            double bernoRes = 0.0;
            for (int i = 1; i <= d; i++)
            {
                bernoRes += Math.Pow(prob, time[i]) * Math.Pow(1 - prob, d - time[i]);
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
            textBox1.Text = "100";
            textBox2.Text = "100";
            textBox3.Text = "0.25";
            textBox4.Text = "0.5";
        }

    }
}
