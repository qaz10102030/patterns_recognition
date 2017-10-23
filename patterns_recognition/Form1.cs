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
        public int[] count = new int[] { 0, 0, 0, 0 };
        private readonly Color[] _colors = new Color[] { Color.Peru, Color.PowderBlue };
        public int[] result = new int[1001];
        public int[] result2 = new int[1001];
        public double[] bernoArray = new double[1001];
        public double[] bernoArray2 = new double[1001];


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = 0;
                result2[i] = 0;
                bernoArray[i] = 0;
                bernoArray2[i] = 0;
            }
            double[] prob = new double[] { 0.25, 0.5 };
            int n = Int16.Parse(textBox1.Text), d = Int16.Parse(textBox2.Text) ;
            test(d, n, prob);
            draw(result, result2, prob, d, n,bernoArray,bernoArray2);
        }

        public void test(int d,int n,double[] prob)
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
            for (int j = 1; j <= d; j++)
            {
                bernoArray[j] = bernoulli(d, result[j], prob[0]);
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
            for (int j = 1; j <= d; j++)
            {
                bernoArray2[j] = bernoulli(d, result2[j], prob[1]);
            }
        }

        public void draw(int[] _y, int[] _y2, double[] prob, int _length, int time, double[] _bernoArray, double[] _bernoArray2)
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();
            chart1.Titles.Add("d=" + _length + ",n=" + time);
            Series _series = new Series();
            for (int index = 1; index <= _length; index++)
            {
                _series.Color = Color.DodgerBlue;
                _series.ChartType = SeriesChartType.Column;
                _series.IsValueShownAsLabel = true;
                 _series.Name ="prob=" + prob[0];
                if (_y[index] != 0)
                {
                    _series.Points.AddXY(index, _y[index]);
                }
            }
            chart1.Series.Add(_series);

            _series = new Series();
            for (int index = 1; index <= _length; index++)
            {
                _series.ChartType = SeriesChartType.Column;
                _series.Color = Color.Red;
                _series.IsValueShownAsLabel = true;
                _series.Name ="prob=" + prob[1];
                if (_y2[index] != 0)
                {
                    _series.Points.AddXY(index, _y2[index]);
                }
            }
            chart1.Series.Add(_series);
        }

        public double bernoulli(int d, int time, double prob)
        {
            double bernoRes = 0.0;
            bernoRes = Math.Pow(prob, time) * Math.Pow(1 - prob, d - time);
                return bernoRes;
        }

    }
}
